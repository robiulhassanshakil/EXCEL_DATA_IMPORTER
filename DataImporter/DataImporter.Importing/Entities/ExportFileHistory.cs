using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Data;

namespace DataImporter.Importing.Entities
{
    public class ExportFileHistory : IEntity<int>
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public string Email { get; set; }
        public DateTime ExportDate { get; set; }
        public int ExportLastExcelFieldId { get; set; }
    }
}
