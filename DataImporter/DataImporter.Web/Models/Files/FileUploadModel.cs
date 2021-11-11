using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using DataImporter.Common.Utilities;
using DataImporter.Importing.BusinessObjects;
using DataImporter.Importing.Exceptions;
using DataImporter.Importing.Services;
using DataImporter.Web.Models.GroupModel;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace DataImporter.Web.Models.Files
{
    public class FileUploadModel
    {
        private  IExcelFileService _fileService;
        private  IMapper _mapper;
        private  IDateTimeUtility _dateTime;
        private  ILogger<FileUploadModel> _logger;
        private ILifetimeScope _scope;
        public DataTable DataTable { get; set; }
        public string ExcelFileName { get; set; }
        public string ExcelFilePath { get; set; }
        public int GroupId { get; set; }

        public FileUploadModel()
        {

        }
        public FileUploadModel(IExcelFileService fileService, IMapper mapper, IDateTimeUtility dateTime, ILogger<FileUploadModel> logger)
        {
            _fileService = fileService;
            _mapper = mapper;
            _dateTime = dateTime;
            _logger = logger;
        }

        public void PreviewExcelLoad(IFormFile file, AllGroupForContacts allGroupForContacts)
        {

            if (allGroupForContacts.GroupId == 0)
            {
                throw new InvalidParameterException("Select Group");
            }
            else
            {
                GroupId = allGroupForContacts.GroupId;
                string fileext = Path.GetExtension(file.FileName);
                if (fileext == ".xlsx" || fileext == ".xlsm" || fileext == ".xls" || fileext == ".xlsb")
                {
                    ExcelFileName = file.FileName;
                    var filePath = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "uploadfiles"));
                    ExcelFilePath = Path.Combine(filePath, ExcelFileName);
                    using (var stream = new FileStream(ExcelFilePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    using (var stream = new FileStream(ExcelFilePath, FileMode.Open, FileAccess.Read))
                    {
                        IExcelDataReader reader;

                        reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                        //// reader.IsFirstRowAsColumnNames
                        var conf = new ExcelDataSetConfiguration
                        {
                            ConfigureDataTable = _ => new ExcelDataTableConfiguration
                            {
                                UseHeaderRow = true
                            }
                        };
                        var dataset = reader.AsDataSet(conf);

                        DataTable = dataset.Tables[0];
                    }
                }
            }
        }

        public bool ExcelFileUpload()
        {
            var isAFirstGroup = _fileService.CheckFirstGroup(GroupId);
            if (isAFirstGroup)
            {
                var excelFile1 = new ExcelFile()
                {
                    ExcelFileName = ExcelFileName,
                    ExcelFilePath = ExcelFilePath,
                    Status = "Incomplete",
                    ImportDate = _dateTime.Now,
                    GroupId = GroupId
                };
                _fileService.FileUploadToDb(excelFile1);
            }
            else
            {
                var ImportColumnCheck = _fileService.GetExcelDatabase(GroupId);
                DataTable = ImportColumnCheck.dataTable;
                DataTable dataTable = new DataTable();
                using (var stream = new FileStream(ExcelFilePath, FileMode.Open, FileAccess.Read))
                {
                    IExcelDataReader reader;
                    reader = ExcelDataReader.ExcelReaderFactory.CreateReader(stream);

                    //// reader.IsFirstRowAsColumnNames
                    var conf = new ExcelDataSetConfiguration
                    {
                        ConfigureDataTable = _ => new ExcelDataTableConfiguration
                        {
                            UseHeaderRow = true
                        }
                    };
                    var dataset = reader.AsDataSet(conf);

                    dataTable = dataset.Tables[0];
                }

                if (dataTable.Columns.Count != DataTable.Columns.Count)
                {
                    return false;
                }

                for (int j = 0; j < DataTable.Columns.Count; j++)
                {
                    if (!dataTable.Columns.Contains(DataTable.Columns[j].ColumnName))
                    {
                        try
                        {
                            File.Delete(ExcelFilePath);
                        }
                        catch (Exception e)
                        {
                            _logger.LogInformation($"{e.Message}");
                        }
                        return false;
                    }
                }

                var excelFile = new ExcelFile()
                {
                    ExcelFileName = ExcelFileName,
                    ExcelFilePath = ExcelFilePath,
                    Status = "Incomplete",
                    ImportDate = _dateTime.Now,
                    GroupId = GroupId
                };
                _fileService.FileUploadToDb(excelFile);
            }
            return true;
        }
        public void ExcelFileCancel()
        {
            File.Delete(ExcelFilePath);
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _fileService = _scope.Resolve<IExcelFileService>();
            _mapper = _scope.Resolve<IMapper>();
            _dateTime = _scope.Resolve<IDateTimeUtility>();
            _logger = _scope.Resolve<ILogger<FileUploadModel>>();
        }
    }
}
