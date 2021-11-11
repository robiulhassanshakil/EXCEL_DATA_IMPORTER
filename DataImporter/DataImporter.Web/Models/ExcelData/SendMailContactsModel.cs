using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataImporter.Common.Utilities;

namespace DataImporter.Web.Models.ExcelData
{
    public class SendMailContactsModel
    {
        public string Email { get; set; }
        public int GroupId { get; set; }
    }
}
