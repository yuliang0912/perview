using CiWong.Resource.Preview.Common;
using System.Collections.Generic;
using System.Linq;


namespace CiWong.Resource.Preview.Web
{
    /// <summary>
    /// 学习页面跳转帮助类
    /// </summary>
    public static class RedirectHelper
    {
        private static object Locks = new object();
        private static Dictionary<string, string> _redirectDictionary = null;

        /// <summary>
        /// 根据资源模块ID获取跳转地址
        /// </summary>
        /// <param name="moduleId"></param>
        /// <returns></returns>
        public static string GetRedirectUrl(string moduleId)
        {
            if (null == _redirectDictionary)
            {
                lock (Locks)
                {
                    if (null == _redirectDictionary)
                    {
                        _redirectDictionary = new Dictionary<string, string>();
                        var redirect = SiteSettings.Instance.Redirect;
                        foreach (var key in redirect.AllKeys)
                        {
                            key.Split(',').ToList().ForEach(t =>
                            {
                                if (!_redirectDictionary.ContainsKey(t))
                                {
                                    _redirectDictionary.Add(t, redirect[key].Value);
                                }
                            });
                        }
                    }
                }
            }

            if (_redirectDictionary.ContainsKey(moduleId))
            {
                return _redirectDictionary[moduleId];
            }

            return string.Empty;
        }

		/// <summary>
		/// 检查是否支持当前module
		/// </summary>
		/// <param name="moduleId"></param>
		/// <returns></returns>
		public static bool IsExists(int moduleId)
		{
			return !string.IsNullOrEmpty(GetRedirectUrl(moduleId.ToString()));
		}
    }
}