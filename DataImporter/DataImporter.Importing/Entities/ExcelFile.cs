using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataImporter.Importing.Entities
{
    public class ExcelFile : IEntity<int>
    {
        public int Id { get; set; }
        public string ExcelFileName { get; set; }
        public string ExcelFilePath { get; set; }
        public DateTime ImportDate { get; set; }
        public string Status { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
