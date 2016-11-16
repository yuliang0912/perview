using CiWong.Resource.Preview.DataContracts;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using CiWong.Tools.Package.DataContracts;

namespace CiWong.Resource.Preview.Web
{
	public static class Extensions
	{
		public static CiWong.Resource.Preview.DataContracts.UserContract ToDataContract(this CiWong.Security.User user)
		{
			return user == null ? null : new CiWong.Resource.Preview.DataContracts.UserContract(user.UserID, user.UserName);
		}

		/// <summary>
		/// 四舍五入,并且去掉0结尾的小数
		/// </summary>
		public static string ToRound(this decimal num)
		{
			return num.ToString(string.Format("f{0}", num % 1 == 0 ? "0" : num * 10 % 1 == 0 ? "1" : "2"));
		}

		/// <summary>
		/// 转换为电子报自定义模块
		/// </summary>
		/// <param name="taskResultContents"></param>
		/// <returns></returns>
		public static List<NewsPaperMenu> ToNewsPaperResults(this PackageCategoryContentContract categoryContent)
		{
			var list = new List<NewsPaperMenu>();

			foreach (var item in categoryContent.ResultContents.OrderBy(t => ResourceModuleOptions.newsPaperModuleSortArray.IndexOf(t.ModuleId)).ThenBy(t => t.DisplayOrder).GroupBy(t => t.ModuleId))
			{
				var model = new NewsPaperMenu() { ModuleId = item.Key, ResourceVersionId = item.First().ResourceVersionId };
				switch (item.Key)
				{
					case 7:
						model.ModuleName = "时文";
						break;
					case 10:
						model.ModuleName = "同步跟读";
						break;
					case 15:
						model.ModuleName = "听说模考";
						break;
					case 18:
						model.ModuleName = "技能训练";
						break;
					case 9:
						model.ModuleName = "同步讲练";
						break;
					case 5:
						model.ModuleName = "单元测试";
						break;
					default:
						break;
				}

				if ((model.ModuleId == 18 || model.ModuleId == 5) && item.Count() > 1)
				{
					model.ResourceList = item.Select(t => new KeyValuePair<long, string>(t.ResourceVersionId, t.ResourceName)).ToList();
				}
				list.Add(model);
			}

			return list;
		}
	}

	/// <summary>
	/// 电子报菜单导航
	/// </summary>
	public class NewsPaperMenu
	{
		public NewsPaperMenu()
		{
			this.ResourceList = Enumerable.Empty<KeyValuePair<long, string>>();
		}

		/// <summary>
		/// 模块ID
		/// </summary>
		public int ModuleId { get; set; }

		/// <summary>
		/// 模块名称
		/// </summary>
		public string ModuleName { get; set; }

		/// <summary>
		/// 资源版本ID
		/// </summary>
		public long ResourceVersionId { get; set; }

		/// <summary>
		/// 模块资源列表(Key:resourceVersionId Value:resourceName)
		/// </summary>
		public IEnumerable<KeyValuePair<long, string>> ResourceList { get; set; }
	}

	/// <summary>
	/// 资源中心附件实体
	/// </summary>
	public class ResourceAttachment
	{
		/// <summary>
		///  自增ID
		/// </summary>
		public long id { get; set; }

		/// <summary>
		/// 文件名
		/// </summary>
		public string file_name { get; set; }

		/// <summary>
		/// 源文件地址（下载地址）
		/// </summary>
		public string source_url { get; set; }

		/// <summary>
		/// 发布地址（Swf预览地址）
		/// </summary>
		public string publish_url { get; set; }

		/// <summary>
		/// 封面图片地址
		/// </summary>
		public string cover_url { get; set; }

		/// <summary>
		/// 文件扩展名
		/// </summary>
		public string file_extention { get; set; }
	}

	/// <summary>
	/// 资源包权限
	/// </summary>
	[DataContract(Name = "packagePermission")]
	public class PackagePermissionContract
	{
		/// <summary>
		/// 验证码
		/// </summary>
        [DataMember(Name = "vipCode")]
		public int VipCode { get; set; }

		/// <summary>
		/// 过期时间
		/// </summary>
		[DataMember(Name = "expirationDate")]
		public DateTime ExpirationDate { get; set; }

		/// <summary>
		/// 是否免费资源
		/// </summary>
		internal bool IsFree { get; set; }
	}

	/// <summary>
	/// 教材资源版本
	/// </summary>
	public class BookVersionContract
	{
		/// <summary>
		/// 教材版本ID
		/// </summary>
		[DataMember(Name = "id")]
		public long Id { get; set; }

		/// <summary>
		/// 教材版本名称
		/// </summary>
		[DataMember(Name = "name")]
		public string Name { get; set; }

		/// <summary>
		/// 学段ID
		/// </summary>
		[DataMember(Name = "period_id")]
		public int PeriodId { get; set; }

		/// <summary>
		/// 年级ID
		/// </summary>
		[DataMember(Name = "grade_id")]
		public int GradeId { get; set; }

		/// <summary>
		/// 学期ID
		/// </summary>
		[DataMember(Name = "semester_id")]
		public int SemesterId { get; set; }

		/// <summary>
		/// 科目ID
		/// </summary>
		[DataMember(Name = "subject_id")]
		public int SubjectId { get; set; }
	}


	[DataContract(Name = "returnauthor")]
	public class ReturnAuthor<T>
	{
		[DataMember(Name = "ret")]
		public int Ret { get; set; }

		[DataMember(Name = "errcode")]
		public int ErrCode { get; set; }

		[DataMember(Name = "msg")]
		public string Msg { get; set; }

		[DataMember(Name = "data")]
		public IEnumerable<T> Data { get; set; }
	}

	/// <summary>
	/// 用户信息
	/// </summary>
	public class UserInfoDTO
	{
		/// <summary>
		/// 用户id
		/// </summary>
		public int user_id { get; set; }
		/// <summary>
		/// 用户名称
		/// </summary>
		public string user_name { get; set; }
	}
}