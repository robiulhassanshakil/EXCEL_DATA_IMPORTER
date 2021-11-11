using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataImporter.Importing.Entities
{
    public class ExcelData : IEntity<int>
    {
        public int Id { get; set; }
        public DateTime ImportDate { get; set; }
        public List<ExcelFieldData> ExcelFieldData { get; set; }
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
