using CiWong.Security;
using CiWong.Tools.Package.DataContracts;
using CiWong.Resource.Preview.DataContracts;
using System.Collections.Generic;
using System.Linq;
using System;

namespace CiWong.Resource.Preview.Web
{
	public abstract class BaseParam
	{
		/// <summary>
		/// 1:自主练习 2:作业
		/// </summary>
		public int ViewType { get; set; }

		/// <summary>
		/// 资源包类型(1电子书,2课程,3电子报)
		/// </summary>
		public int PackageType { get; set; }

		/// <summary>
		/// 资源包信息
		/// </summary>
		public PackageContract Package { get; internal set; }

		/// <summary>
		/// 资源包使用权限信息
		/// </summary>
		public PackagePermissionContract PackagePermission { get; internal set; }

		/// <summary>
		/// 是否有使用权限
		/// </summary>
		public bool IsCan
		{
			get
			{
				return null != this.PackagePermission && (this.PackagePermission.IsFree || this.PackagePermission.ExpirationDate > DateTime.Now);
			}
		}

		/// <summary>
		/// 引用模板
		/// </summary>
		public string Layout
		{
			get
			{
				if (PackageType == 1) //电子书
				{
					switch (ViewType)
					{
						case 1:
							return "~/Views/Shared/Practice/_EbookLayout.cshtml";
						case 2:
							return "~/Views/Shared/Work/_EbookLayout.cshtml";
						default:
							return "~/Views/Shared/Practice/_EbookNoNavigation.cshtml";
					}
				}
				else if (PackageType == 3) //电子报
				{
					switch (ViewType)
					{
						case 1:
							return "~/Views/Shared/Practice/_NewsPaperLayout.cshtml";
						case 2:
							return "~/Views/Shared/Work/_NewsPaperLayout.cshtml";
						default:
							return "~/Views/Shared/Practice/_NewsPaperNoNavigation.cshtml";
					}
				}

				return "~/Views/Shared/Practice/_NewsPaperNoNavigation.cshtml";
			}
		}

		public virtual CiWong.Resource.Preview.DataContracts.ReturnResult ReturnResult { get; set; }
	}

	/// <summary>
	/// 资源预览基础参数
	/// </summary>
	public class ResourceParam : BaseParam
	{
		/// <summary>
		/// 资源版本ID
		/// </summary>
		public long VersionId { get; set; }

		/// <summary>
		/// 资源包ID
		/// </summary>
		public long PackageId { get; set; }

		/// <summary>
		/// 目录ID
		/// </summary>
		public string CatalogueId { get; set; }

		/// <summary>
		/// 用户信息
		/// </summary>
		public User User { get; internal set; }

		/// <summary>
		/// 当前目录模块信息集合
		/// </summary>
		public PackageCategoryContentContract CategoryContent { get; internal set; }

		/// <summary>
		/// 当前资源模块
		/// </summary>
		public TaskResultContentContract TaskResultContent
		{
			get
			{
				if (null == CategoryContent)
				{
					return null;
				}
				return CategoryContent.ResultContents.FirstOrDefault(t => t.ResourceVersionId == VersionId);
			}
		}
	}

	/// <summary>
	/// 作业基础参数
	/// </summary>
	public class WorkParam : BaseParam
	{
		/// <summary>
		/// 作业状态(0:未提交 1:暂存 2:已提交 3:已批改 4:退回 5:延期提交)
		/// </summary>
		public int workStatus = 0;

		/// <summary>
		/// 用户信息
		/// </summary>
		public User User { get; internal set; }

		/// <summary>
		/// 做作业对象
		/// </summary>
		public DoWorkBaseContract DoWorkBase { get; set; }

		/// <summary>
		/// 资源对象
		/// </summary>
		public WorkResourceContract WorkResource { get; set; }

		/// <summary>
		/// 单元作业
		/// </summary>
		public UnitWorksContract UnitWork { get; set; }

	}
}