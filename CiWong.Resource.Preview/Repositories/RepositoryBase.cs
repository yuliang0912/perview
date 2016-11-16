using System.Configuration;
using CiWong.Infrastructure.Data;

namespace CiWong.Resource.Preview.Repositories
{
    internal abstract class RepositoryBase
    {
		protected RepositoryBase(string readConfigurationKey = "learning_platform_read", string writeConfigurationKey = "learning_platform_write")
        {
            this.AdoProvide = new MySqlAdoProvide();
            this.WriteConnectionString = ConfigurationManager.ConnectionStrings[writeConfigurationKey].ConnectionString;
            this.ReadConnectionString = ConfigurationManager.ConnectionStrings[readConfigurationKey].ConnectionString;
        }

        /// <summary>
        /// 数据驱动对象
        /// </summary>
        protected IAdoProvide AdoProvide { get; private set; }

        /// <summary>
        /// 读数据库连接字符串
        /// </summary>
        protected string ReadConnectionString { get; private set; }

        /// <summary>
        /// 写数据库连接字符串
        /// </summary>
        protected string WriteConnectionString { get; private set; }
    }
}
