using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using GoodsShopper.Ap.Hubs;
using Live.Libs.KeepAliveConn;
using Live.Libs.Persistent;

namespace GoodsShopper.Ap.Applibs
{
    internal static class AutofacConfig
    {
        private static IContainer container;

        public static IContainer Container
        {
            get
            {
                if (container == null) Register();

                return container;
            }
        }

        public static void Register()
        {
            var builder = new ContainerBuilder();
            var asm = Assembly.GetExecutingAssembly();
            builder.RegisterApiControllers(asm);

            //// Action Handler
            builder.RegisterAssemblyTypes(asm)
                .Named<IMicroServiceActionHandler>(t => t.Name.Replace("ActionHandler", string.Empty).ToLower())
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            // repository ioc
            builder.RegisterAssemblyTypes(Assembly.Load("GoodsShopper.Domain"), Assembly.Load("GoodsShopper.Persistent"))
                .Where(t => t.Namespace == "GoodsShopper.Persistent" ||
                            t.Namespace == "GoodsShopper.Domain.Repository")
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == $"I{t.Name}"))
                .SingleInstance();

            // svc ioc
            builder.RegisterAssemblyTypes(Assembly.Load("GoodsShopper.Ap"))
                .Where(t => t.Namespace == "GoodsShopper.Ap.Model.Service")
                .As(t => t.GetInterfaces().FirstOrDefault(i => i.Name == $"I{t.Name}"))
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .SingleInstance();

            // BackEnd Procedure
            {
                // KeepAliveProcedure
                builder.RegisterType<KeepAliveProcedure>()
                    .Keyed<IProcedure>(ProcedureType.KeepAlive)
                    .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                    .SingleInstance();
            }

            //// signalr client
            builder.RegisterType<HubClient>()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .As<IHubClient>()
                .SingleInstance();

            //// 流水號
            builder.RegisterType<SerialNumberRepository>()
                .WithParameter("conn", NoSqlService.RedisConnections)
                .As<ISerialNumberRepository>()
                .SingleInstance();

            container = builder.Build();
        }
    }
}
