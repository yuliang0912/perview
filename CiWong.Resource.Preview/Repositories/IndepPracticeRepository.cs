using CiWong.Infrastructure.Data;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Infrastructure.Data;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CiWong.Resource.Preview.Repositories
{
	internal class IndepPracticeRepository : RepositoryBase
	{
		/// <summary>
		/// 添加自主练习记录，返回自增id
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public long Insert(IndepPracticeContract model)
		{
			var bulider = new InsertBuilder("indeppractice")
				.RegisterField("ProductId", model.ProductId)
				.RegisterField("AppId", model.AppId)
				.RegisterField("PackageId", model.PackageId)
				.RegisterField("PackageName", model.PackageName)
				.RegisterField("PackageType", model.PackageType)
				.RegisterField("TaskId", model.TaskId)
				.RegisterField("ResourceType", model.ResourceType)
				.RegisterField("VersionId", model.VersionId)
				.RegisterField("ResourceName", model.ResourceName)
				.RegisterField("ModuleId", model.ModuleId)
				.RegisterField("SubmitUserId", model.SubmitUserId)
				.RegisterField("SubmitUserName", model.SubmitUserName)
				.RegisterField("WorkScore", model.WorkScore)
				.RegisterField("ActualScore", model.ActualScore)
				.RegisterField("CorrectRate", model.CorrectRate)
				.RegisterField("WorkLong", model.WorkLong)
				.RegisterField("CreateDate", model.CreateDate)
				.RegisterField("Status", model.Status);

			return AdoProvide.ExecuteScalar<long>(WriteConnectionString, bulider);
		}



		/// <summary>
		/// 更新对象
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public bool UpDate(IndepPracticeContract model)
		{
			var bulider = new UpdateBuilder("indeppractice")
				.RegisterField("ProductId", model.ProductId)
				.RegisterField("AppId", model.AppId)
				.RegisterField("PackageId", model.PackageId)
				.RegisterField("PackageName", model.PackageName)
				.RegisterField("PackageType", model.PackageType)
				.RegisterField("TaskId", model.TaskId)
				.RegisterField("ResourceType", model.ResourceType)
				.RegisterField("VersionId", model.VersionId)
				.RegisterField("ResourceName", model.ResourceName)
				.RegisterField("ModuleId", model.ModuleId)
				.RegisterField("SubmitUserId", model.SubmitUserId)
				.RegisterField("SubmitUserName", model.SubmitUserName)
				.RegisterField("WorkScore", model.WorkScore)
				.RegisterField("ActualScore", model.ActualScore)
				.RegisterField("CorrectRate", model.CorrectRate)
				.RegisterField("WorkLong", model.WorkLong)
				.RegisterField("CreateDate", model.CreateDate)
				.RegisterField("Status", model.Status)
				.RegisterClause(new EqualBuilder<long>("Id", model.Id));

			return AdoProvide.ExecuteNonQuery(WriteConnectionString, bulider) > 0;
		}


		public IndepPracticeContract GetPracticeByIds(long verionId, string moduleId, int userId)
		{
			var builder = new SelectBuilder("SELECT * FROM indeppractice")
					.RegisterEqualClause("VersionId", verionId)
					.RegisterEqualClause("ModuleId", moduleId)
					.RegisterEqualClause("SubmitUserId", userId)
					.RegisterEqualClause("Status", "0");

			//自动根据dataReader转成实体
			return AdoProvide.ExecuteReader(ReadConnectionString, builder, reader => EntityHelper.GetEntity<IndepPracticeContract>(reader)).FirstOrDefault();
		}

		public IndepPracticeContract GetPracticeById(long Id)
		{
			var builder = new SelectBuilder("SELECT * FROM indeppractice")
					.RegisterEqualClause("id", Id);

			//自动根据dataReader转成实体
			return AdoProvide.ExecuteReader(ReadConnectionString, builder, reader => EntityHelper.GetEntity<IndepPracticeContract>(reader)).FirstOrDefault();
		}

		/// <summary>
		/// 获取自主练习统计
		/// </summary>
		/// <param name="userIdList"></param>
		/// <returns></returns>
		public List<WorkCensusContract> GetIndeppracticeCensus(IEnumerable<int> userIdList, DateTime beginDate, DateTime endDate, params string[] moduleId)
		{
			var _list = new List<WorkCensusContract>();

			if (userIdList == null || moduleId == null || !userIdList.Any() || !moduleId.Any())
			{
				return _list;
			}

			var sql = string.Format(@"SELECT submitUserId AS UserId,submitUserName AS UserName,COUNT(*) AS TotalWorkNum,SUM(workLong) AS TotalWorkLong FROM indeppractice
									  WHERE SubmitUserId IN ({0}) AND CreateDate BETWEEN @beginDate AND @endDate AND ModuleId IN ({1}) AND `Status` = 3
									  GROUP BY SubmitUserId", string.Join(",", userIdList), string.Join(",", moduleId.Select(t => "'" + t + "'")));

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@beginDate", beginDate), new MySqlParameter("@endDate", endDate)))
			{
				_list = EntityHelper.GetList<WorkCensusContract>(dr);
			}

			return _list;
		}
	}
}
