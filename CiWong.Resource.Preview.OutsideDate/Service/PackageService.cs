using CiWong.Resource.Preview.Common;
using CiWong.Tools.Package.DataContracts;
using System.Collections.Generic;
using System.Linq;

namespace CiWong.Resource.Preview.OutsideDate.Service
{
    /// <summary>
    /// 打包中心数据接口
    /// </summary>
    public class PackageService
    {
        /// <summary>
        /// 获取模块
        /// </summary>
        /// <param name="packageId"></param>
        /// <returns></returns>

        public List<TaskModuleContract> GetTaskModules(long packageId)
        {
            var key = string.Concat("_preview_list_module_content_", packageId);

            var cacheContent = RedisHelper.GetItemFromSet<List<TaskModuleContract>>(key).FirstOrDefault();

            if (cacheContent != null) { return cacheContent; }

            var result = new RestClient(100001).ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<List<TaskModuleContract>>>(WebAPI.GetModules, new { packageId = packageId });
            ///判断数据是否获取成功
            if (result.Ret == 0 && null != result.Data && result.Data.Any())
            {
                RedisHelper.AddItemToSet(key, result.Data.ToList());
                return result.Data.ToList();
            }
            return new List<TaskModuleContract>();
        }

        /// <summary>
        /// 获取资源包
        /// </summary>
        public PackageContract GetPackage(long packageId)
        {
            var key = string.Concat("_preview_package_content_", packageId);

            var cacheContent = RedisHelper.GetItemFromSet<PackageContract>(key).FirstOrDefault();

            if (cacheContent != null) { return cacheContent; }

            var result = new RestClient(100001).ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<PackageContract>>(WebAPI.GetPackage, new { packageId = packageId });
            if (result.Ret == 0 && null != result.Data)
            {
                RedisHelper.AddItemToSet(key, result.Data);
                return result.Data;
            }
            return null;
        }

        /// <summary>
        /// 获取资源包目录
        /// </summary>
        /// <param name="hasResult">是否获取全部目录(包含未连载资源的)</param>
        /// <returns></returns>
        public List<PackageCatalogueContract> GetCatalogues(long packageId, bool istree = false, bool hasResult = false)
        {
            var result = new RestClient(100001).ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<List<PackageCatalogueContract>>>(WebAPI.GetPackCatalogs, new { packageid = packageId, istree = false, hasResults = hasResult });

            if (result.Ret == 0 && null != result.Data && result.Data.Any())
            {
                return result.Data.ToList();
            }
            return new List<PackageCatalogueContract>();
        }

        /// <summary>
        /// 获取单个目录的模块,内容集合
        /// </summary>
        /// <param name="isDeleted">null:查询全部 false:查询未删除的 true:查询已删除的</param>
        public PackageCategoryContentContract GetTaskResultContents(long packageId, string catalogueId, bool? isDeleted = false)
        {
            var result = new RestClient(100001).ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<PackageCategoryContentContract>>(WebAPI.GetPackageCategory, new { packageId = packageId, catalogueId = catalogueId });

            if (result.Ret == 0 && result.Data != null)
            {
                return result.Data;
            }

            return new PackageCategoryContentContract()
            {
                ResultContents = new List<TaskResultContentContract>(),
                TaskModules = new List<TaskModuleContract>(),
                ResultCategories = new List<TaskResultCategoryContract>()
            };
        }

        /// <summary>
        /// 获取商品使用权限
        /// </summary>
        /// <param name="packageId">包ID</param>
        /// <param name="versionId">版本ID</param>
        /// <returns></returns>
        public List<TaskResultContentContract> GetTaskResultContents(long packageId, long versionId)
        {
            var result = new RestClient(100001).ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<List<TaskResultContentContract>>>(WebAPI.TaskResultContent, new { packageid = packageId, versionid = versionId });

            if (result.Ret == 0 && null != result.Data && result.Data.Any())
            {
                return result.Data.ToList();
            }

            return new List<TaskResultContentContract>();
        }
    }
}
