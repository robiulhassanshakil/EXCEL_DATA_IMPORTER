using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataImporter.Importing.BusinessObjects
{
    public class ExcelFile
    {
        public DateTime ImportDate { get; set; }
        public string ExcelFileName { get; set; }
        public string ExcelFilePath { get; set; }
        public string Status { get; set; }
        public int GroupId { get; set; }
        public string GroupName { get; set; }
    }
}
