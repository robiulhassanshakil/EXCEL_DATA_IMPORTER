using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.Repositories
{
    public class ExcelDataRepository : Repository<ExcelData, int>, IExcelDataRepository
    {
        public ExcelDataRepository(IImportingDbContext context)
            : base((DbContext)context)
        {

        }
    }
}
