using Autofac;
using Autofac.Integration.Mvc;
using CiWong.Framework;
using CiWong.Resource.Preview.OutsideDate;
using CiWong.Users;
using System.Reflection;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web
{
    public static class AutofacConfig
    {
        /// <summary>
        /// 注册依赖注入
        /// </summary>
        public static void RegisterIOC()
        {
            var builder = new ContainerBuilder();

			builder.RegisterModule(new AutofacWebTypesModule());

            //框架
            builder.RegisterModule(new FrameworkModule());
            //用户
            builder.RegisterModule(new SecurityModule());
            //试卷
            builder.RegisterModule(new OutsideDateModule());

            builder.RegisterModule(new ResourceModule());


            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly());
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterFilterProvider();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));
        }
    }
}