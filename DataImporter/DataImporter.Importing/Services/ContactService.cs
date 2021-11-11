using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataImporter.Common.Utilities;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.UniteOfWorks;

namespace DataImporter.Importing.Services
{
    public class ContactService : IContactService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly IDateTimeUtility _dateTimeUtility;
        private readonly IMapper _mapper;

        public ContactService(IImportingUnitOfWork importingUnitOfWork, IDateTimeUtility dateTimeUtility, IMapper mapper)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _dateTimeUtility = dateTimeUtility;
            _mapper = mapper;
        }

        public (IList<ExcelFile> records, int total, int totalDisplay) GetExcelfile(int pageIndex, int pageSize, string searchText, string sortText, Guid applicationUser)
        {
            var groupData = _importingUnitOfWork.Groups.GetDynamic(
                string.IsNullOrWhiteSpace(searchText) ? x => x.ApplicationUserId == applicationUser : x => x.Name.Contains(searchText) && x.ApplicationUserId == applicationUser,
                null, "ExcelFile", pageIndex, pageSize, true);

            var excelfiles = new List<ExcelFile>();

            foreach (var gp in groupData.data)
            {
                foreach (var excelFile in gp.ExcelFile)
                {
                    var excelfile = new ExcelFile()
                    {
                        GroupName = gp.Name,
                        Status = excelFile.Status,
                        ImportDate = excelFile.ImportDate,
                        ExcelFileName = excelFile.ExcelFileName,

                    };
                    excelfiles.Add(excelfile);
                }
            }

            return (excelfiles, groupData.total, groupData.totalDisplay);
        }

        public void LoadAllData(Guid applicationUserId)
        {
            var groupWithExcelfiles = _importingUnitOfWork.Groups.Get(x => x.ApplicationUserId == applicationUserId, null, "ExcelFile", true);

            var excelfile = (from g in groupWithExcelfiles
                             where g.ExcelFile.Count != 0
                             select g.ExcelFile).ToList();

        }
    }
}
