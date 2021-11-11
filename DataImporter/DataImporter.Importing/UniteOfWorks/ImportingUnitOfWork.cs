using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Data;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Repositories;
using Microsoft.EntityFrameworkCore;

namespace DataImporter.Importing.UniteOfWorks
{
    public class ImportingUnitOfWork : UnitOfWork, IImportingUnitOfWork
    {
        public IGroupRepository Groups { get; private set; }
        public IExcelFileRepository ExcelFiles { get; private set; }
        public IExcelFieldDataRepository ExcelFieldDatas { get; private set; }
        public IExcelDataRepository ExcelDatas { get; private set; }
        public IExportFileHistoryRepository ExportFileHistories { get; private set; }
        public ImportingUnitOfWork(IImportingDbContext context,
           IGroupRepository groups,
           IExcelFileRepository excelFiles,
           IExcelFieldDataRepository excelFieldDatas,
           IExcelDataRepository excelDatas,
           IExportFileHistoryRepository exportFileHistories

       ) : base((DbContext)context)
        {
            Groups = groups;
            ExcelFiles = excelFiles;
            ExcelDatas = excelDatas;
            ExcelFieldDatas = excelFieldDatas;
            ExportFileHistories = exportFileHistories;
        }
    }
}
