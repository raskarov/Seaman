using System;
using System.Linq;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using Common.Logging;
using Seaman.Core;
using Seaman.EntityFramework;

namespace Seaman.Web
{
    public static class InjectionConfig
    {
        internal static IContainer Container { get; set; }

        public static void Register()
        {
            ServiceMappers.Check();

            var builder = new ContainerBuilder();
            var assembly = typeof (InjectionConfig).Assembly;
            builder.RegisterModule(new LoggingModule());
            SeamanDbContext.SetMigrateToLatestVersionDatabaseInitializer();


            builder.RegisterType<SeamanDbContext>()
                .AsSelf()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterType<PlainHasher>().SingleInstance();
            builder.RegisterType<CryptoHasher>().SingleInstance();

            builder.Register<IPasswordHasher>((c, p) =>
            {
                var parameters = p as Parameter[] ?? p.ToArray();
                var name = parameters.Named<string>("name");
                switch (name)
                {
                    case PasswordHasherNames.Plain:
                        return c.Resolve<PlainHasher>(parameters);
                    case PasswordHasherNames.Crypto:
                        return c.Resolve<CryptoHasher>(parameters);
                }
                return null;
            }).As<IPasswordHasher>().SingleInstance();

            builder.RegisterType<UserManager>().As<IUserManager>().InstancePerLifetimeScope();
            builder.RegisterType<SampleManager>().As<ISampleManager>().InstancePerLifetimeScope();

            builder.RegisterApiControllers(assembly);

            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);
            var container = builder.Build();
            Container = container;
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        public class LoggingModule : Module
        {
            protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
            {
                registration.Preparing += (s, e) =>
                {
                    e.Parameters = new[] { _loggerParameter }.Concat(e.Parameters);
                };
            }

            private readonly Parameter _loggerParameter = new ResolvedParameter((p, c) => p.ParameterType == typeof(ILog),
                (p, c) => LogManager.GetLogger(p.Member.DeclaringType));
        }
    }

    public static class PasswordHasherNames
    {
        public const String Plain = "plain";
        public const String Crypto = "crypto";
    }
}