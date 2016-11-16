using CiWong.Framework.Cache;
using CiWong.Resource.Preview.Common;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.OutsideDate.Service;
using CiWong.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Controllers
{
    /// <summary>
    /// 资源控制器
    /// </summary>
    public class ResourceController : CustomerController
    {
        private PackageService packageService;
        private ResourceService resourceService;
        public ResourceController(ResourceService _resourceService, PackageService _packageService)
        {
            this.resourceService = _resourceService;
            this.packageService = _packageService;
        }

        /// <summary>
        /// 根据资源类型ID和版本ID获取
        /// </summary>
        public ContentResult Get(Guid moduleId, long versionId)
        {
            DateTime d1 = DateTime.Now;
            //RedisHelper.RemoveItemByKey("_preview");
            var key = string.Concat("_preview_resource_get_", moduleId, "_", versionId);
            //var cacheContent = RedisHelper.GetItemFromSet<string>(key).FirstOrDefault();
            var cacheContent = RedisHelper.GetItem<string>(key);

            if (!string.IsNullOrEmpty(cacheContent))
            {
                //记录redis服务器用时.
                Response.Headers.Add("Redis-Total-Milliseconds", (DateTime.Now - d1).TotalMilliseconds.ToString());
                return Content(cacheContent, "application/json");
            }

            if (moduleId == ResourceModuleOptions.ExaminationPaper) //试卷
            {
                var ExaminationPaper = resourceService.GetExamination(versionId);
                if (ExaminationPaper.Ret == 0)
                {
                    cacheContent = JSONHelper.Encode(ExaminationPaper);
                }
            }
            else if (moduleId == ResourceModuleOptions.Question)//试题
            {
                //cacheContent = JSONHelper.Encode(resourceService.GetQuestions(versionId));
                var Question = resourceService.GetQuestions(versionId);
                if (Question.Ret == 0)
                {
                    cacheContent = JSONHelper.Encode(Question);
                }
            }
            else if (moduleId == ResourceModuleOptions.ListeningAndSpeakingExam)
            {
                var ListeningAndSpeakingExam = resourceService.GetListenExamination(versionId, ResourceModuleOptions.ListeningAndSpeaking);
                if (ListeningAndSpeakingExam.Ret == 0) { cacheContent = JSONHelper.Encode(ListeningAndSpeakingExam); }
                // cacheContent = JSONHelper.Encode(resourceService.GetListenExamination(versionId, ResourceModuleOptions.ListeningAndSpeaking));
            }
            else
            {
                cacheContent = new RestClient(100001).ExecuteContent(WebAPI.ResourceGet, new { versionid = versionId, moduleid = moduleId });
                //result.Data = resultArticle.Data;
                //result.Ret = resultArticle.Ret;
                //result.Message = resultArticle.Message;
                //cacheContent = JSONHelper.Encode(resourceService.GetByVersionId(moduleId, versionId));
            }

            if (!string.IsNullOrEmpty(cacheContent))
            {
                //RedisHelper.AddItemToSet<string>(key, cacheContent);
                RedisHelper.SetItem<string>(key, cacheContent, 432000);//设置缓存时间5天
            }

            return Content(cacheContent, "application/json");
        }

        /// <summary>
        /// 根据资源类型ID和版本ID获取
        /// </summary>
        public ActionResult GetList(Guid moduleId, string versionIds)
        {
            var versions = versionIds.Split(',').Select(t => Convert.ToInt64(t)).ToArray();

            if (!versions.Any())
            {
                return Json(new ReturnResult(1, "无效的资源"), JsonRequestBehavior.AllowGet);
            }

            DateTime d1 = DateTime.Now;
            var key = string.Concat(string.Concat("_preview_resource_getlist_", moduleId, "_", string.Join("_", versionIds)));

            //获取缓存的数据
            //var cacheContent = RedisHelper.GetItemFromSet<string>(key).FirstOrDefault();
            var cacheContent = RedisHelper.GetItem<string>(key);

            if (!string.IsNullOrEmpty(cacheContent))
            {
                //记录redis服务器用时.
                Response.Headers.Add("Redis-Total-Milliseconds", (DateTime.Now - d1).TotalMilliseconds.ToString());
                return Content(cacheContent, "application/json");
            }

            if (moduleId == ResourceModuleOptions.ExaminationPaper) //试卷
            {
                cacheContent = JSONHelper.Encode(resourceService.GetExamination(versions[0]));
            }
            else if (moduleId == ResourceModuleOptions.Question)//试题
            {
                cacheContent = JSONHelper.Encode(resourceService.GetQuestions(versions));
            }
            else
            {
                cacheContent = resourceService.GetByVersionIds(moduleId, versionIds);
            }

            if (!string.IsNullOrEmpty(cacheContent) && cacheContent.Contains("\"ret\":0"))
            {
                RedisHelper.SetItem<string>(key, cacheContent, 432000);//设置缓存时间(s)
            }

            return Content(cacheContent, "application/json");
        }

        /// <summary>
        /// 电子报目录及版面信息
        /// </summary>
        public ContentResult NewsPaperInfo(long packageId, long bookVersion)
        {

            var key = string.Concat("_preview_newspaper_info_", packageId);

            //var cacheContent = RedisHelper.GetItemFromSet<string>(key).FirstOrDefault();
            var cacheContent = RedisHelper.GetItem<string>(key);

            if (!string.IsNullOrEmpty(cacheContent))
            {
                return Content(cacheContent, "application/json");
            }

            //获取资源包目录*****/
            var catalogues = packageService.GetCatalogues(packageId, false, true).ToList();

            var bookResult = new RestClient(100001).ExecuteGet<ReturnResult<BookVersionContract>>(WebApi.GetBookVersion, new { bookId = bookVersion });

            var newsPaper = new
            {
                bookVersionName = bookResult != null && bookResult.Data != null ? bookResult.Data.Name : string.Empty,
                catalogues = catalogues.Where(m => !catalogues.Any(t => t.ParentId == m.ID)).Select(t => new
                {
                    id = t.ID,
                    name = t.Name
                })
            };

            cacheContent = JSONHelper.Encode(new ReturnResult<object>(newsPaper));

            if (!string.IsNullOrEmpty(cacheContent) && cacheContent.Contains("\"ret\":0"))
            {
                // RedisHelper.AddItemToSet<string>(key, cacheContent);
                RedisHelper.SetItem<string>(key, cacheContent);
            }
            return Content(cacheContent, "application/json");
        }

        /// <summary>
        /// 电子书目录信息
        /// </summary>
        public ContentResult EbookCatalogues(long packageId)
        {
            var key = string.Concat("_preview_ebook_catalogues_", packageId);

            //var cacheContent = RedisHelper.GetItemFromSet<string>(key).FirstOrDefault();
            var cacheContent = RedisHelper.GetItem<string>(key);
            if (!string.IsNullOrEmpty(cacheContent))
            {
                return Content(cacheContent, "application/json");
            }

            var catalogues = packageService.GetCatalogues(packageId).OrderByDescending(t => t.DisplayOrder);

            var modules = packageService.GetTaskModules(packageId);
            var level = catalogues.OrderBy(t => t.DisplayOrder).Where(t => t.Level == 1).ToList();

            foreach (var catalogue in level)
            {
                catalogue.Recursion(item =>
                {
                    item.Children = catalogues.Where(c => c.ParentId != null && c.ParentId.Equals(item.ID));
                }, item => item.Children);
            }

            cacheContent = JSONHelper.Encode(new ReturnResult<object>(new { catalogues = level, modules = modules }));

            if (!string.IsNullOrEmpty(cacheContent))
            {
                //RedisHelper.AddItemToSet(key, cacheContent);
                RedisHelper.SetItem(key, cacheContent);//设置缓存时间为12小时
            }
            return Content(cacheContent, "application/json");
        }

        /// <summary>
        /// 获取模块资源
        /// </summary>
        public ContentResult GetTaskResultContents(long packageId, string cid, int moduleId)
        {
            var key = string.Concat("_preview_taskresult_contents_", packageId, "_", cid, "_", moduleId);
            //var cacheContent = RedisHelper.GetItemFromSet<string>(key).FirstOrDefault();
            var cacheContent = RedisHelper.GetItem<string>(key);

            if (!string.IsNullOrEmpty(cacheContent))
            {
                return Content(cacheContent, "application/json");
            }

            var currContents = packageService.GetTaskResultContents(packageId, cid).ResultContents.Where(t => t.ModuleId.Equals(moduleId)).ToList();

            if (!currContents.Any())
            {
                return Content(JSONHelper.Encode(new ReturnResult("未找到资源")), "application/json");
            }

            cacheContent = JSONHelper.Encode(new ReturnResult<List<Tools.Package.DataContracts.TaskResultContentContract>>(currContents));

            if (!string.IsNullOrEmpty(cacheContent))
            {
                //RedisHelper.AddItemToSet(key, cacheContent);
                RedisHelper.SetItem(key, cacheContent);//缓存时间设置为半天 12小时
            }

            return Content(cacheContent, "application/json");

        }
    }
}
