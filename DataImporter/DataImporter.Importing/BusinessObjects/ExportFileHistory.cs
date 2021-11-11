using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Importing.BusinessObjects
{
    public class ExportFileHistory
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public string Email { get; set; }
        public DateTime ExportDate { get; set; }
        public int ExportLastExcelFieldId { get; set; }
    }
}
