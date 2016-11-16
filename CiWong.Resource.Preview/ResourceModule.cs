using Autofac;
using CiWong.Resource.Preview.Service;

namespace CiWong.Resource.Preview
{
	/// <summary>
	/// 资源作业接口注入
	/// </summary>
    public class ResourceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SupportService>();
            //builder.RegisterType<SpeekingService>();
            builder.RegisterType<WorkService>();
            builder.RegisterType<WorkBaseService>();
            base.Load(builder);
        }
    }
}
