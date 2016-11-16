using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiWong.Resource.Preview.Common
{
    public static class WebAPI
    {
        /// <summary>
        /// 获取包目录结构
        /// </summary>
        public const string GetPackCatalogs = "/tools/package/catalogues";

        /// <summary>
        /// 根据packageId获取模块列表
        /// </summary>
        public const string GetModules = "/tools/package/modules";

        /// <summary>
        /// 获取资源包信息
        /// </summary>
        public const string GetPackageCategory = "/tools/package/packagecategory";

        /// <summary>
        /// 获取当前资源包信息(当前资源详细信息(商品购买期限))
        /// </summary>
        public const string TaskResultContent = "/tools/package/taskresult";

        /// <summary>
        /// 获取资源包信息
        /// </summary>
        public const string GetPackage = "/tools/package/index";

        /// <summary>
        ///获取单个资源信息
        /// </summary>
        public const string ResourceGet = "/tools/resource/get";

        /// <summary>
        /// 获取资源列表
        /// </summary>
        public const string ResourceGetList = "/tools/resource/getlist";

        /// <summary>
        /// 试卷API
        /// </summary>
        public const string GetExamineeModule = "/paper/PaperModel";
    }
}
