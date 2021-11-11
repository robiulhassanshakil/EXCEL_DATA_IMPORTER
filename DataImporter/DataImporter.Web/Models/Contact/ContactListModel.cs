using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using DataImporter.Common.Utilities;
using DataImporter.Importing.Services;
using DataImporter.Web.Models.Commons;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace DataImporter.Web.Models.Contact
{
    public class ContactListModel
    {
        private  IContactService _contactService;
        private  IMapper _mapper;
        private  IDateTimeUtility _dateTime;
        private  ILifetimeScope _scope;

        public ContactListModel()
        {

        }
        public ContactListModel(IContactService contactService, IMapper mapper, IDateTimeUtility dateTime, ILifetimeScope scope)
        {
            _contactService = contactService;
            _mapper = mapper;
            _dateTime = dateTime;
            _scope = scope;
        }
        internal object LoadData(DataTablesAjaxRequestModel dataTableModel, Guid applicationUserId)
        {
            var data = _contactService.GetExcelfile(
                dataTableModel.PageIndex,
                dataTableModel.PageSize,
                dataTableModel.SearchText,
                dataTableModel.GetSortText(new string[] { "Name", "ExcelFileName", "DateTime", "Status" }), applicationUserId);
            return new
            {
                recordsTotal = data.total,
                recordsFiltered = data.totalDisplay,
                data = (from record in data.records
                        select new string[]
                        {
                            record.GroupName,
                            record.ExcelFileName,
                            record.ImportDate.ToString(),
                            record.Status
                        }
                    ).ToArray()
            };
        }

        public void Resolve(ILifetimeScope scope)
        {
            _scope = scope;
            _contactService = _scope.Resolve<IContactService>();
            _mapper = _scope.Resolve<IMapper>();
            _dateTime = _scope.Resolve<IDateTimeUtility>();
        }

    }
}
