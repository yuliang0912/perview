using CiWong.Infrastructure.Data;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Infrastructure.Data;
using System;
using System.Linq;
using CiWong.Extensions;
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace CiWong.Resource.Preview.Repositories
{
	/// <summary>
	/// 作业基础表底层数据提供
	/// 此类只供内部调用.对外提供,请通过service文件下的具体接口提供
	/// 示例代码 余亮 2014-7-7
	/// </summary>
	internal class WorkBaseRepository : RepositoryBase
	{
		public WorkBaseRepository() : base("RoomWorkRead", "RoomWorkWrite") { }

		/// <summary>
		/// 获取基础作业信息
		/// </summary>
		/// <param name="workId"></param>
		/// <returns></returns>
		public WorkBaseContract GetWorkBase(long workId)
		{
			var sql = @"SELECT WorkName,WorkType,SonWorkType,PublishUserID,PublishUserName,PublishDate,SendDate,
                        EffectiveDate,WorkStatus,TotalNum,CompletedNum,ReferLong,PublishType,WorkScore,ReviceUserID,
                        ReviceUserName,MarkNum,MarkType,ExaminationID,WorkDesc,RedirectParm,IsWiki,RecordId 
                        FROM WorkBase WHERE WorkID=@WorkID";

			WorkBaseContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@WorkID", workId)))
			{
				model = EntityHelper.GetEntity<WorkBaseContract>(dr);
			}

			return model;
		}

		/// <summary>
		/// 用户作业接收人信息
		/// </summary>
		/// <param name="doWorkId"></param>
		/// <returns></returns>
		public DoWorkBaseContract GetDoWorkBase(long doWorkId)
		{
			var sql = @"SELECT d.DoWorkID,d.WorkID,d.WorkName,d.SubmitUserID,d.SubmitUserName,d.SubmitDate,d.WorkStatus,d.WorkPractice,
                        d.WorkLong,d.ActualScore,d.WorkScore,d.EffectiveDate,d.WorkDesc,d.WorkType,d.SonWorkType,d.RedirectParm,workbase.TotalNum,
                        workbase.publishUserId,workbase.publishUserName,workbase.sendDate,workbase.WorkStatus as workBaseStatus
                        FROM DoWorkBase AS d INNER JOIN workbase ON workbase.WorkID = d.WorkID
                        WHERE DoWorkID=@DoWorkID AND DelStatus=0";

			DoWorkBaseContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@DoWorkID", doWorkId)))
			{
				if (dr.Read())
				{
					model = new DoWorkBaseContract()
					{
						DoWorkID = dr.GetInt64("DoWorkID"),
						WorkID = dr.GetInt64("WorkID"),
						WorkName = dr.GetString("WorkName"),
						SubmitUserID = dr.GetInt32("SubmitUserID"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						WorkStatus = dr.GetInt32("WorkStatus"),
						WorkPractice = dr.GetInt32("WorkPractice"),
						WorkLong = dr.GetInt32("WorkLong"),
						ActualScore = dr.GetDecimal("ActualScore"),
						WorkScore = dr.GetDecimal("WorkScore"),
						EffectiveDate = dr.GetDateTime("EffectiveDate"),
						WorkDesc = dr.GetString("WorkDesc"),
						WorkType = dr.GetInt32("WorkType"),
						SonWorkType = dr.GetInt32("SonWorkType"),
						RedirectParm = dr.GetString("RedirectParm"),
						TotalNum = dr.GetInt32("TotalNum"),
						PublishUserId = dr.GetInt32("publishUserId"),
						PublishUserName = dr.GetString("publishUserName"),
						SendDate = dr.GetDateTime("sendDate"),
						WorkBaseStatus = dr.GetInt32("workBaseStatus")
					};
				}
			}

			return model;
		}

		/// <summary>
		/// 更新做作业信息以及统计数据
		/// </summary>
		/// <param name="doworkId"></param>
		/// <param name="submitDate"></param>
		/// <returns></returns>
		public bool UpdateDoWorkStatus(long doworkId, DateTime submitDate, int workStatus, int workLong, decimal actualScore)
		{
			var builder = "CALL Proc_SetWorkOnlineInfo(@DoWorkID,@SubmitDate,@WorkStatus,@WorkLong,@ActualScore,0,0)";

			var _params = new IDataParameter[] { 
                AdoProvide.BuildParameter("@DoWorkID",doworkId),
                AdoProvide.BuildParameter("@SubmitDate",submitDate),
				AdoProvide.BuildParameter("@WorkStatus",workStatus),
				AdoProvide.BuildParameter("@WorkLong",workLong),
				AdoProvide.BuildParameter("@ActualScore",actualScore)
            };

			return AdoProvide.ExecuteScalar<int>(WriteConnectionString, builder, _params) == 1;
		}

		/// <summary>
		/// 批量批改作业
		/// </summary>
		/// <param name="workId"></param>
		/// <param name="userIdList"></param>
		/// <returns></returns>
		public bool UpdateDoWorkStatus(IEnumerable<int> userIdList, long workId, int workStatus, decimal actualScore)
		{
			if (null == userIdList || !userIdList.Any())
			{
				return false;
			}

			string sql = string.Empty;
			if (actualScore < 0)
			{
				sql = string.Format("UPDATE doworkbase SET WorkStatus = @WorkStatus WHERE WorkId = @WorkId AND SubmitUserId IN ({0});", string.Join(",", userIdList));
			}
			else
			{
				sql = string.Format("UPDATE doworkbase SET WorkStatus = @WorkStatus, ActualScore = @ActualScore WHERE WorkId = @WorkId AND SubmitUserId IN ({0});", string.Join(",", userIdList));
			}

			sql += @"UPDATE workbase SET CompletedNum = (SELECT COUNT(*) FROM doworkbase WHERE doworkbase.WorkStatus IN(2,3,5) AND doworkbase.WorkID = workbase.WorkID),
				     MarkNum = (SELECT COUNT(*) FROM doworkbase WHERE doworkbase.WorkStatus = 3 AND doworkbase.WorkID = workbase.WorkID) WHERE WorkID = @workId;";

			return AdoProvide.ExecuteNonQuery(WriteConnectionString, sql, AdoProvide.BuildParameter("@WorkId", workId), AdoProvide.BuildParameter("@ActualScore", actualScore), AdoProvide.BuildParameter("@WorkStatus", workStatus)) > 0;
		}
	}
}
