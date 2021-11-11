using DataImporter.Importing.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataImporter.Common.Utilities;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.UniteOfWorks;

namespace DataImporter.Importing.Services
{
    public class GroupService : IGroupService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly IDateTimeUtility _dateTimeUtility;
        private readonly IMapper _mapper;


        public GroupService(IImportingUnitOfWork importingUnitOfWork, IDateTimeUtility dateTimeUtility, IMapper mapper)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _dateTimeUtility = dateTimeUtility;
            _mapper = mapper;
        }
        public void CreateGroup(Group group)
        {
            if (group == null)
                throw new InvalidParameterException("Group was not provided");

            if (IsNameAlreadyUsed(group.Name, group.ApplicationUserId))
                throw new DuplicateTitleException("Group title already exists");


            _importingUnitOfWork.Groups.Add(
                _mapper.Map<Entities.Group>(group)
            );

            _importingUnitOfWork.Save();
        }

        public void DeleteGroup(int id)
        {
            if (id == 0)
            {
                throw new InvalidParameterException("Group Id  is not provided");
            }

            _importingUnitOfWork.Groups.Remove(id);
            _importingUnitOfWork.Save();
        }

        public int GetAllExportData(Guid applicationUserId)
        {
            var AllExportData = _importingUnitOfWork.ExportFileHistories.Get(x=>x.Group.ApplicationUserId==applicationUserId, "").Count;


            return AllExportData;
        }

        public IList<Group> GetAllGroup(Guid applicationuser)
        {
            var groupEntities = _importingUnitOfWork.Groups.Get(x => x.ApplicationUserId == applicationuser, "");
            var groups = new List<Group>();

            foreach (var gp in groupEntities)
            {
                groups.Add(_mapper.Map<Group>(gp));
            }

            return groups;
        }

        public int GetAllImportData(Guid applicationUserId)
        {
            var allGroupData = _importingUnitOfWork.ExcelFiles.Get(x => x.Group.ApplicationUserId == applicationUserId, "").Count;

            return allGroupData;

        }

        public Group GetGroup(int id)
        {
            var group = _importingUnitOfWork.Groups.GetById(id);
            if (group == null) return null;

            return _mapper.Map<Group>(group);

        }

        public (IList<Group> records, int total, int totalDisplay) GetGroups(int pageIndex, int pageSize, string searchText, string sortText, Guid applicationUser)
        {
            var groupData = _importingUnitOfWork.Groups.GetDynamic(
                string.IsNullOrWhiteSpace(searchText) ? x => x.ApplicationUserId == applicationUser : x => x.Name.Contains(searchText) && x.ApplicationUserId == applicationUser,
                sortText, string.Empty, pageIndex, pageSize);
            var resultData = (from gp in groupData.data
                              select (_mapper.Map<Group>(gp))).ToList();

            return (resultData, groupData.total, groupData.totalDisplay);
        }

        public List<Group> LoadAllGroupFroViewData(Guid applicationuserId)
        {
            var groupListForViewData =_importingUnitOfWork.Groups.Get(x => x.ApplicationUserId == applicationuserId && x.ExcelData.Count>0, "");

            var groups = new List<Group>();

            foreach (var gp in groupListForViewData)
            {
                groups.Add(_mapper.Map<Group>(gp));
            }

            return groups;
        }

        public void UpdateGroup(Group group)
        {
            if (group == null)
            {
                throw new InvalidParameterException("Group is messing");
            }

            var groupEntity = _importingUnitOfWork.Groups.GetById(group.Id);
            if (groupEntity != null)
            {
                groupEntity.Name = group.Name;
                _importingUnitOfWork.Save();
            }
            else
            {
                throw new InvalidParameterException("Couldn't find Group");
            }
        }

        private bool IsNameAlreadyUsed(string name, Guid userId) =>
            _importingUnitOfWork.Groups.GetCount(x => x.Name == name && x.ApplicationUserId == userId) > 0;
    }
}
