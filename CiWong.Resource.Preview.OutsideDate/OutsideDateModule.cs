using Autofac;
using CiWong.Examination.API;
using CiWong.Resource.Preview.OutsideDate.Service;

namespace CiWong.Resource.Preview.OutsideDate
{
    /// <summary>
    /// AutoFac模块与实例注入
    /// </summary>
    /// 
    public class OutsideDateModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ExaminationAPIModule());
            builder.RegisterType<ResourceService>();
            builder.RegisterType<PackageService>();
            builder.RegisterType<CorrectService>();
			builder.RegisterType<SpeakAPIService>().As<ISpeakAPIService>();
            base.Load(builder);
        }
    }
}
