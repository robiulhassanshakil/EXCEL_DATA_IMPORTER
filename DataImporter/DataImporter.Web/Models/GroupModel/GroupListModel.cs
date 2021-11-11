using System;
using System.Linq;
using Autofac;
using AutoMapper;
using DataImporter.Importing.Services;
using DataImporter.Web.Models.Commons;


namespace DataImporter.Web.Models.GroupModel
{
    public class GroupListModel
    {
        private  IGroupService _groupService;
        private  IMapper _mapper;
        private ILifetimeScope _scope;

        public GroupListModel()
        {
           
        }
        public GroupListModel(IGroupService groupService, IMapper mapper)
        {
            _groupService = groupService;
            _mapper = mapper;
        }
        internal object GetGroupData(DataTablesAjaxRequestModel dataTableModel, Guid applicationUser)
        {
            var data = _groupService.GetGroups(
                dataTableModel.PageIndex,
                dataTableModel.PageSize,
                dataTableModel.SearchText,
                dataTableModel.GetSortText(new string[] { "Name" }), applicationUser);

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.Name,
                            record.Id.ToString()
                        }
                    ).ToArray()
            };
        }
        internal void Delete(int id)
        {
            _groupService.DeleteGroup(id);
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _groupService=_scope.Resolve<IGroupService>();
            _mapper=_scope.Resolve<IMapper>();
        }
    }
}