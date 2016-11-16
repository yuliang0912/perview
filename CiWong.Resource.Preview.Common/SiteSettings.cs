using System.Configuration;

namespace CiWong.Resource.Preview.Common
{
    public partial class SiteSettings : ConfigurationSection
    {
        #region static single

        private static class SiteSettingsHolder
        {
            // ReSharper disable once MemberHidesStaticFromOuterClass
            public readonly static SiteSettings Instance;

            static SiteSettingsHolder()
            {
                Instance = ConfigurationManager.GetSection("siteSettings") as SiteSettings;
            }
        }

		public static SiteSettings Instance
		{
			get { return SiteSettingsHolder.Instance; }
		}

        #endregion

        /// <summary>
        /// 站点域名
        /// </summary>
        [ConfigurationProperty("domain")]
        public string Domain
        {
            get { return base["domain"] as string; }
        }

     
        /// <summary>
        /// 样式文件、css文件域名Url
        /// </summary>
        [ConfigurationProperty("style")]
        public StyleElement Style
        {
            get { return base["style"] as StyleElement; }
        }

        /// <summary>
        /// web api配置
        /// </summary>
        [ConfigurationProperty("webApi")]
        public WebApiElement WebApi
        {
            get { return base["webApi"] as WebApiElement; }
        }

        [ConfigurationProperty("other")]
        public KeyValueConfigurationCollection Other
        {
            get { return base["other"] as KeyValueConfigurationCollection; }
        }

        /// <summary>
        /// 跳转地址配置集合
        /// </summary>
        [ConfigurationProperty("redirect")]
        public KeyValueConfigurationCollection Redirect
        {
            get { return base["redirect"] as KeyValueConfigurationCollection; }
        }
    }

    /// <summary>
    /// 样式配置项
    /// </summary>
    public class StyleElement : ConfigurationElement
    {
        [ConfigurationProperty("domain")]
        public string Domain
        {
            get { return base["domain"] as string; }
        }
    }

    public class WebApiElement : ConfigurationElement
    {
        [ConfigurationProperty("domain")]
        public string Domain
        {
            get { return base["domain"] as string; }
        }

        [ConfigurationProperty("appId")]
        public string AppId
        {
            get { return base["appId"] as string; }
        }
    }
}