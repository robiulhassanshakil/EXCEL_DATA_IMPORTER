using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Autofac;
using DataImporter.Web.Models;
using DataImporter.Web.Models.Account;
using DataImporter.Web.Models.Commons;
using DataImporter.Web.Models.Contact;
using DataImporter.Web.Models.ExcelData;
using DataImporter.Web.Models.Files;
using DataImporter.Web.Models.GroupModel;


namespace DataImporter.Web
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ConfirmEmailModel>().AsSelf();
            builder.RegisterType<LoginModel>().AsSelf();
            builder.RegisterType<RegisterConfirmationModel>().AsSelf();
            builder.RegisterType<RegisterModel>().AsSelf();
            builder.RegisterType<DataTablesAjaxRequestModel>().AsSelf();
            builder.RegisterType<ContactListModel>().AsSelf();
            builder.RegisterType<ExcelFromDatabase>().AsSelf();
            builder.RegisterType<SendMailContactsModel>().AsSelf();
            builder.RegisterType<ExportHistoryListModel>().AsSelf();
            builder.RegisterType<FileUploadModel>().AsSelf();
            builder.RegisterType<AllGroupForContacts>().AsSelf();
            builder.RegisterType<CreateGroupModel>().AsSelf();
            builder.RegisterType<GroupEditModel>().AsSelf();
            builder.RegisterType<GroupListModel>().AsSelf();
            builder.RegisterType<DashBoardModel>().AsSelf();
            
            base.Load(builder);
        }
    }
}
