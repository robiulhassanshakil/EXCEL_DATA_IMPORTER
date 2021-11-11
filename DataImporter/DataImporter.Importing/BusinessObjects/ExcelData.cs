using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Data;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DataImporter.Importing.BusinessObjects
{
    public class ExcelData
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public int GroupId { get; set; }

    }
}
