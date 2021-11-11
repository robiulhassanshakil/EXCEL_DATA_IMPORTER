using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.UniteOfWorks;
using Microsoft.AspNetCore.Http;

namespace DataImporter.Importing.Services
{
    public class ExcelFileService : IExcelFileService
    {
        private readonly IImportingUnitOfWork _importingUnitOfWork;
        private readonly IMapper _mapper;

        public ExcelFileService(IImportingUnitOfWork importingUnitOfWork, IMapper mapper)
        {
            _importingUnitOfWork = importingUnitOfWork;
            _mapper = mapper;
        }

        public void FileUploadToDb(ExcelFile file)
        {
            if (file == null)
                throw new InvalidParameterException("File was not provided");
            _importingUnitOfWork.ExcelFiles.Add(new Entities.ExcelFile()
            {
                ExcelFileName = file.ExcelFileName,
                ExcelFilePath = file.ExcelFilePath,
                GroupId = file.GroupId,
                ImportDate = file.ImportDate,
                Status = file.Status
            });
            _importingUnitOfWork.Save();
        }

        public (DataTable dataTable, int excelDataLastId) GetExcelDatabase(int groupId)
        {
            var allDatas = _importingUnitOfWork.ExcelDatas.Get(x => x.GroupId == groupId, null, "ExcelFieldData", true);

            int LastExcelDataId = allDatas.LastOrDefault().Id;


            var allExcelFieldData = new List<ExcelFieldData>();
            var coloumCounter = 0;
            foreach (var allData in allDatas)
            {
                coloumCounter = 0;
                foreach (var excelFieldData in allData.ExcelFieldData)
                {
                    coloumCounter++;
                    var oneExcelFielData = new ExcelFieldData()
                    {
                        ExcelDataId = excelFieldData.ExcelDataId,
                        Name = excelFieldData.Name,
                        Value = excelFieldData.Value
                    };
                    allExcelFieldData.Add(oneExcelFielData);
                }
            }

            var dataTable = ConvertExcelDataFieldToDataTable(allExcelFieldData, coloumCounter);

            return (dataTable, LastExcelDataId);
        }
        public DataTable ConvertExcelDataFieldToDataTable(List<ExcelFieldData> allExcelFieldData, int coloumCounter)
        {
            var rows = allExcelFieldData.Count / coloumCounter;
            DataTable dataTable = new DataTable();
            bool columnsAdded = false;
            var coloum = 0;
            foreach (var excelFieldData in allExcelFieldData)
            {
                coloum++;
                DataColumn dataColumn = new DataColumn();
                dataColumn.ColumnName = excelFieldData.Name;
                dataTable.Columns.Add(dataColumn);
                if (coloum == coloumCounter)
                {
                    break;
                }
            }
            var allvalueList = new List<object>();
            foreach (var excelFieldData in allExcelFieldData)
            {
                allvalueList.Add(excelFieldData.Value);
            }
            var z = 0;
            for (int i = 0; i < rows; i++)
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (DataColumn column in dataTable.Columns)
                {
                    dataRow[column] = allvalueList[z];
                    z++;
                }

                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

        public string GetExcelFileName(int groupId)
        {
            var excelFileName = _importingUnitOfWork.Groups.GetById(groupId).Name;

            return excelFileName;
        }

        public void ExportFileHistoryCreate(ExportFileHistory exportFileHistory)
        {
            _importingUnitOfWork.ExportFileHistories.Add(new Entities.ExportFileHistory()
            {
                GroupId = exportFileHistory.GroupId,
                ExportDate = exportFileHistory.ExportDate,
                Email = exportFileHistory.Email,
                ExportLastExcelFieldId = exportFileHistory.ExportLastExcelFieldId
            });
            _importingUnitOfWork.Save();
        }

        public (IList<ExportFileHistory> records, int total, int totalDisplay) GetExcelFileHistory(int pageIndex, int pageSize, string searchText, string sortText, Guid applicationUser)
        {
            var groupData = _importingUnitOfWork.Groups.GetDynamic(
                string.IsNullOrWhiteSpace(searchText) ? x => x.ApplicationUserId == applicationUser : x => x.Name.Contains(searchText) && x.ApplicationUserId == applicationUser,
                null, "ExportFileHistory", pageIndex, pageSize, true);

            var exportFilesHistory = new List<ExportFileHistory>();

            foreach (var gp in groupData.data)
            {
                foreach (var exportFileHistory in gp.ExportFileHistory)
                {
                    var exportHistory = new ExportFileHistory()
                    {
                        GroupName = gp.Name,
                        Email = exportFileHistory.Email,
                        ExportDate = exportFileHistory.ExportDate,
                        ExportLastExcelFieldId = exportFileHistory.ExportLastExcelFieldId
                    };
                    exportFilesHistory.Add(exportHistory);
                }
            }

            return (exportFilesHistory, groupData.total, groupData.totalDisplay);
        }

        public int GetGroupId(int excelLastId)
        {
            var groupid = _importingUnitOfWork.ExcelDatas.GetById(excelLastId).GroupId;
            return groupid;
        }

        public DataTable GetExcelDataForHistoryDownload(int groupId, int excelLastDataId)
        {
            var allDatas = _importingUnitOfWork.ExcelDatas.Get(x => x.GroupId == groupId, null, "ExcelFieldData", true);

            var datafilter = (from excelData in allDatas
                              where excelData.Id <= excelLastDataId
                              select excelData).ToList();


            var allExcelFieldData = new List<ExcelFieldData>();
            var coloumCounter = 0;
            foreach (var allData in datafilter)
            {
                coloumCounter = 0;
                foreach (var excelFieldData in allData.ExcelFieldData)
                {
                    coloumCounter++;
                    var oneExcelFielData = new ExcelFieldData()
                    {
                        ExcelDataId = excelFieldData.ExcelDataId,
                        Name = excelFieldData.Name,
                        Value = excelFieldData.Value
                    };
                    allExcelFieldData.Add(oneExcelFielData);
                }
            }

            var dataTable = ConvertExcelDataFieldToDataTable(allExcelFieldData, coloumCounter);
            return dataTable;
        }

       

        public bool CheckFirstGroup(int groupId)
        {
            var counter = _importingUnitOfWork.ExcelDatas.Get(x => x.GroupId == groupId, "").Count;

            if (counter > 0)
            {
                return false;
            }
            return true;
        }
    }
}
