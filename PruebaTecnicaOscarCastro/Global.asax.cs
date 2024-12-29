using PruebaTecnicaOscarCastro.Repositorios.Interfaces;
using PruebaTecnicaOscarCastro.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Unity;
using Unity.Mvc5;

namespace PruebaTecnicaOscarCastro {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Configurar Unity Container
            var container = new UnityContainer();

            // Registrar las dependencias
            container.RegisterType<ISalaRepository, SalaRepository>();
            container.RegisterType<IReservaRepository, ReservaRepository>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}
