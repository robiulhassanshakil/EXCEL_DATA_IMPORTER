using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataImporter.Common.Utilities;
using DataImporter.Importing.Entities;
using DataImporter.Importing.Services;
using DataImporter.Importing.UniteOfWorks;
using ExcelDataReader;

namespace DataImporter.ExcelToDatabaseService.Model
{
    public class ImportingDataModel : IImportingDataModel
    {
        private readonly IImportingDataModelService _importingDataModelService;
        private readonly IDateTimeUtility _dateTimeUtility;

        public ImportingDataModel(IImportingDataModelService importingDataModelService, IDateTimeUtility dateTimeUtility)
        {
            _importingDataModelService = importingDataModelService;
            _dateTimeUtility = dateTimeUtility;
        }

        public void ImportDatabase()
        {
            _importingDataModelService.ImportDatabase();
        }
    }

}
