using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.OutsideDate.Service;
using CiWong.Resource.Preview.Service;
using CiWong.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace CiWong.Resource.Preview.Web
{
	public static class AuthorizeHelper
	{
		//基础参数定义 app_id:平台ID,id:产品ID,package_id:资源包ID,expires有效期
		private static readonly string[] resourceParamsArray = new string[] { "packageid", "cid", "versionid" };
		private static readonly string[] worksParamsArray = new string[] { "doworkid", "contentid" };

		/// <summary>
		/// 资源参数验证与基础信息赋值
		/// </summary>
		public static ReturnResult<ResourceParam> CheckResourceParams(this HttpRequestBase request, PackageService packageService, User user)
		{
			var query = request.Url.ParseQueryString();

			var client = new RestClient(100001);

			var defectParams = resourceParamsArray.Except(query.AllKeys.Select(t => t.ToLower()));

			if (defectParams.Any())
			{
				return new ReturnResult<ResourceParam>(1, null, string.Concat("基础参数验证失败,参数:", string.Join(",", defectParams), "未找到"));
			}

			var baseParam = new ResourceParam()
			{
				User = user,
				ViewType = 1,
				PackageId = Convert.ToInt64(query.Get("packageId")),
				CatalogueId = query.Get("cid"),
				VersionId = Convert.ToInt64(query.Get("versionId")),
				PackagePermission = new PackagePermissionContract()
			};

			baseParam.Package = packageService.GetPackage(baseParam.PackageId);
			if (null == baseParam.Package)
			{
				return new ReturnResult<ResourceParam>(1, baseParam, "错误的URL,参数packageId错误");
			}

			baseParam.PackageType = baseParam.Package.GroupType;

			baseParam.CategoryContent = packageService.GetTaskResultContents(baseParam.PackageId, baseParam.CatalogueId, null);

			if (null == baseParam.CategoryContent || !baseParam.CategoryContent.ResultContents.Any())
			{
				return new ReturnResult<ResourceParam>(2, baseParam, "错误的URL,资源包中未包含所需资源");
			}
			if (null == baseParam.TaskResultContent)
			{
				return new ReturnResult<ResourceParam>(3, baseParam, "错误的URL,资源包中未找到所需资源");
			}

			#region 资源权限认证    DateTime.Now < DateTime.Parse("2015-09-01")  时间小于九月一号的所有资源免费开放
			if (baseParam.TaskResultContent.IsFree || user.IsTeacher || DateTime.Now < DateTime.Parse("2015-09-01"))
			{
				baseParam.PackagePermission = new PackagePermissionContract { ExpirationDate = DateTime.Now.AddYears(1) };
			}
			else if (user.UserID > 0)
			{
				//判断用户使用权限 VipCode(1:已购买书籍 2:无使用权限 3:已购买但已过期  4:免费资源  5:已开通会员服务 6:已经开通服务但已经过期)
				//var result = new RestClient(user.UserID, "http://192.168.1.61:8123").ExecuteGet<ReturnResult<PackagePermissionContract>>("/bookcase/home/IsCan", new { packageId = baseParam.PackageId, userid = user.UserID });
				var result = new RestClient(user.UserID).ExecuteGet<ReturnResult<PackagePermissionContract>>(WebApi.UserIsCan, new { packageId = baseParam.Package.PackageId, userid = user.UserID });

				if (result.Ret == 0 && null != result.Data)
				{
					baseParam.PackagePermission = result.Data;
				}
			}
			else
			{
				return new ReturnResult<ResourceParam>(-1, baseParam, "非免费资源,需要登录!");
			}

			if (null == baseParam.PackagePermission)
			{
				return new ReturnResult<ResourceParam>(102, baseParam, "您还未购买,请购买后再使用!");
			}
			if (baseParam.PackagePermission.ExpirationDate < DateTime.Now)
			{
				return new ReturnResult<ResourceParam>(103, baseParam, "产品已过期,请重新购买!");
			}
			#endregion

			return new ReturnResult<ResourceParam>(baseParam);
		}

		/// <summary>
		/// 作业参数验证与基础信息赋值
		/// </summary>
		public static ReturnResult<WorkParam> CheckWorkParams(this HttpRequestBase request, User user, WorkBaseService workBaseService, WorkService workService, PackageService packageService, bool isGetParts = true, bool isRedirectBuy = true)
		{
			var query = request.Url.ParseQueryString();
			var client = new RestClient(100001);

			var defectParams = worksParamsArray.Except(query.AllKeys.Select(t => t.ToLower()));

			if (defectParams.Any())
			{
				return new ReturnResult<WorkParam>(1, null, string.Concat("基础参数验证失败,参数:", string.Join(",", defectParams), "未找到"));
			}

			var baseParam = new WorkParam()
			{
				User = user,
				ViewType = 2
			};

			long doWorkId = Convert.ToInt64(query.Get("doWorkId")), contentId = Convert.ToInt64(query.Get("contentId"));

			///请求用户个人列表
			baseParam.DoWorkBase = workBaseService.GetDoWorkBase(doWorkId);

			if (null == baseParam.DoWorkBase || baseParam.DoWorkBase.WorkType < 101)
			{
				return new ReturnResult<WorkParam>(2, baseParam, "未找到符合条件的作业");
			}
			if (baseParam.DoWorkBase.SubmitUserID != user.UserID && baseParam.DoWorkBase.PublishUserId != user.UserID)
			{
				return new ReturnResult<WorkParam>(3, baseParam, "暂无作业权限");
			}
			if (baseParam.DoWorkBase.WorkBaseStatus != 0)
			{
				return new ReturnResult<WorkParam>(7, baseParam, "这份作业已经被老师取消布置啦,再去作业系统看看有没有重新布置哦!");
			}

			var recordId = Convert.ToInt64(baseParam.DoWorkBase.RedirectParm.Split('.')[0].Replace("bid_", string.Empty));

			baseParam.WorkResource = workService.GetWorkResource(contentId);

			if (null == baseParam.WorkResource || baseParam.WorkResource.RecordId != recordId)
			{
				return new ReturnResult<WorkParam>(4, baseParam, "未找到符合条件的作业");
			}

			baseParam.Package = packageService.GetPackage(baseParam.WorkResource.PackageId);

			baseParam.PackageType = baseParam.Package.GroupType;

			baseParam.UnitWork = workService.GetUserUnitWork(contentId, doWorkId);

			baseParam.workStatus = null == baseParam.UnitWork ? 0 : baseParam.UnitWork.Status;

			#region 使用权限判断

			if (!string.IsNullOrEmpty(baseParam.WorkResource.RelationPath))
			{
				long rootVersion = Convert.ToInt64(baseParam.WorkResource.RelationPath.Split('/').First());

				var packageResults = packageService.GetTaskResultContents(baseParam.Package.PackageId, rootVersion);

				if (null == packageResults || !packageResults.Any())
				{
					return new ReturnResult<WorkParam>(5, baseParam, "未找到指定资源");
				}

				////  DateTime.Now < DateTime.Parse("2015-09-01")  时间小于九月一号的所有资源免费开放 
				if (baseParam.workStatus > 0 || packageResults.Any(t => t.IsFree) || DateTime.Now < DateTime.Parse("2015-09-01")) //如果免费资源或者已经完成的作业,则不进行验证
				{
					baseParam.PackagePermission = new PackagePermissionContract() { IsFree = true };
				}
				else
				{
					//判断用户使用权限 VipCode(1:已购买书籍 2:无使用权限 3:已购买但已过期  4:免费资源  5:已开通会员服务 6:已经开通服务但已经过期)
					//var result = new RestClient(user.UserID, "http://192.168.1.61:8123").ExecuteGet<ReturnResult<PackagePermissionContract>>("/bookcase/home/IsCan", new { packageId = baseParam.Package.PackageId, userid = user.UserID });
					var result = new RestClient(user.UserID).ExecuteGet<ReturnResult<PackagePermissionContract>>(WebApi.UserIsCan, new { packageId = baseParam.Package.PackageId, userid = user.UserID });

					if (result.Ret == 0 && null != result.Data)
					{
						baseParam.PackagePermission = result.Data;
					}

					if (null == baseParam.PackagePermission && (baseParam.WorkResource.ModuleId != 5 || baseParam.WorkResource.ModuleId != 9))
					{
						return new ReturnResult<WorkParam>(102, baseParam, "您还未购买,请购买后再使用!");
					}
					if (baseParam.WorkResource.ModuleId != 5 && baseParam.WorkResource.ModuleId != 9)
					{
						if (null != baseParam.PackagePermission && baseParam.PackagePermission.ExpirationDate < DateTime.Now)
						{
							return new ReturnResult<WorkParam>(103, baseParam, "产品已过期,请重新购买!");
						}
					}
				}
			}
			#endregion

			if (baseParam.WorkResource.IsFull && isGetParts)
			{
				baseParam.WorkResource.resourceParts = workService.GetResourceParts(contentId);
			}

			if (baseParam.DoWorkBase.PublishUserId == user.UserID && baseParam.DoWorkBase.SubmitUserID != user.UserID && baseParam.workStatus != 3)
			{
				return new ReturnResult<WorkParam>(6, baseParam, "学生未完成作业,无法查看");
			}

			return new ReturnResult<WorkParam>(baseParam);
		}
	}
}