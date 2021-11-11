using DataImporter.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;

namespace DataImporter.Importing.Entities
{
    public class ExcelFieldData : IEntity<Guid>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int ExcelDataId { get; set; }
        public ExcelData ExcelData { get; set; }

    }
}
