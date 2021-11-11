using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using DataImporter.Common.Utilities;
using DataImporter.Importing.Services;
using DataImporter.Web.Models.Commons;

namespace DataImporter.Web.Models.Files
{
    public class ExportHistoryListModel
    {
        private ILifetimeScope _scope;
        private  IExcelFileService _excelFileService;
        private  IMapper _mapper;
        private  IDateTimeUtility _dateTime;
        public ExportHistoryListModel()
        {
          
        }
        public ExportHistoryListModel(IExcelFileService excelFileService, IMapper mapper, IDateTimeUtility dateTime)
        {
            _excelFileService = excelFileService;
            _mapper = mapper;
            _dateTime = dateTime;
        }

        internal object LoadDataExcelHistory(DataTablesAjaxRequestModel dataTableModel, Guid applicationuserId)
        {
            var data = _excelFileService.GetExcelFileHistory(
                dataTableModel.PageIndex,
                dataTableModel.PageSize,
                dataTableModel.SearchText,
                dataTableModel.GetSortText(new string[] { "GroupName", "Email", "ExportDate", }), applicationuserId);

            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.GroupName,
                            record.Email,
                            record.ExportDate.ToString(),
                            record.ExportLastExcelFieldId.ToString()
                        }
                    ).ToArray()
            };
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _mapper = _scope.Resolve<IMapper>();
            _dateTime = _scope.Resolve<IDateTimeUtility>();
            _excelFileService = _scope.Resolve<IExcelFileService>();
        }
    }
}
