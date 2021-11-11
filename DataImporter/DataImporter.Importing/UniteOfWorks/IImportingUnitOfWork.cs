using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Data;
using DataImporter.Importing.Repositories;

namespace DataImporter.Importing.UniteOfWorks
{
    public interface IImportingUnitOfWork : IUnitOfWork
    {
        IGroupRepository Groups { get; }
        IExcelFileRepository ExcelFiles { get; }
        IExcelFieldDataRepository ExcelFieldDatas { get; }
        IExcelDataRepository ExcelDatas { get; }
        IExportFileHistoryRepository ExportFileHistories { get; }
    }
}
