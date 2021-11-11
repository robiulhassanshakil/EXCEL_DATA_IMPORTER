using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DataImporter.ExcelToDatabaseService.Model;

namespace DataImporter.ExcelToDatabaseService
{
    public class WorkerModule : Module
    {

        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;

        public WorkerModule(string connectionStringName, string migrationAssemblyName)
        {
            _connectionString = connectionStringName;
            _migrationAssemblyName = migrationAssemblyName;

        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImportingDataModel>().As<IImportingDataModel>()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
