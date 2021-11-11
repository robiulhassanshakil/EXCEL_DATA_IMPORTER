using Autofac;
using DataImporter.Common.Utilities;

namespace DataImporter.Common
{
    public class CommonModule : Module
    {

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DateTimeUtility>().As<IDateTimeUtility>()
                .InstancePerLifetimeScope();
            builder.RegisterType<EmailService>().As<IEmailService>()
                .InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
