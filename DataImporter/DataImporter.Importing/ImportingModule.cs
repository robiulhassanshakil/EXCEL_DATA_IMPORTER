using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using DataImporter.Importing.Contexts;
using DataImporter.Importing.Repositories;
using DataImporter.Importing.Services;
using DataImporter.Importing.UniteOfWorks;

namespace DataImporter.Importing
{
    public class ImportingModule : Module
    {
        private readonly string _connectionString;
        private readonly string _migrationAssemblyName;
        public ImportingModule(string connectionString, string migrationAssemblyName)
        {
            _connectionString = connectionString;
            _migrationAssemblyName = migrationAssemblyName;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ImportingDbContext>().AsSelf()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<ImportingDbContext>().As<IImportingDbContext>()
                .WithParameter("connectionString", _connectionString)
                .WithParameter("migrationAssemblyName", _migrationAssemblyName)
                .InstancePerLifetimeScope();

            builder.RegisterType<GroupRepository>().As<IGroupRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<GroupService>().As<IGroupService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ImportingUnitOfWork>().As<IImportingUnitOfWork>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ExcelFileRepository>().As<IExcelFileRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ExcelDataRepository>().As<IExcelDataRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ExcelFieldDataRepository>().As<IExcelFieldDataRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ExcelFileService>().As<IExcelFileService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ContactService>().As<IContactService>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ExportFileHistoryRepository>().As<IExportFileHistoryRepository>()
                .InstancePerLifetimeScope();
            builder.RegisterType<ImportingDataModelService>().As<IImportingDataModelService>()
                .InstancePerLifetimeScope();
            base.Load(builder);
        }
    }
}
