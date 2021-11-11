using DataImporter.Importing.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Importing.Services
{
    public interface IGroupService
    {
        void CreateGroup(Group group);
        (IList<Group> records, int total, int totalDisplay) GetGroups(int pageIndex, int pageSize, string searchText, string sortText, Guid applicationUser);
        void DeleteGroup(int id);
        Group GetGroup(int id);
        void UpdateGroup(Group group);
        IList<Group> GetAllGroup(Guid applicationuser);
        int GetAllImportData(Guid applicationUserId);
        int GetAllExportData(Guid applicationUserId);
        List<Group> LoadAllGroupFroViewData(Guid applicationuserId);
    }
}
