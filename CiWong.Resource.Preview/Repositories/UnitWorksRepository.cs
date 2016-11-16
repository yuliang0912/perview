using CiWong.Infrastructure.Data;
using CiWong.Resource.Preview.DataContracts;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CiWong.Resource.Preview.Repositories
{
	internal class UnitWorksRepository : RepositoryBase
	{
		#region unitWorks
		/// <summary>
		/// 添加单元作业完成记录,返回自增id
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public long Insert(UnitWorksContract model)
		{
			var bulider = new InsertBuilder("unitworks")
				   .RegisterField("ContentId", model.ContentId)
				   .RegisterField("RecordId", model.RecordId)
				   .RegisterField("WorkId", model.WorkId)
				   .RegisterField("DoWorkId", model.DoWorkId)
				   .RegisterField("SubmitUserId", model.SubmitUserId)
				   .RegisterField("SubmitUserName", model.SubmitUserName ?? string.Empty)
				   .RegisterField("WorkScore", model.WorkScore)
				   .RegisterField("ActualScore", model.ActualScore)
				   .RegisterField("CorrectRate", model.CorrectRate)
				   .RegisterField("WorkLong", model.WorkLong)
				   .RegisterField("SubmitDate", model.SubmitDate)
				   .RegisterField("IsTimeOut", model.IsTimeOut)
				   .RegisterField("SubmitCount", model.SubmitCount)
				   .RegisterField("Comment", model.Comment ?? string.Empty)
				   .RegisterField("CommentType", model.CommentType)
				   .RegisterField("Status", model.Status);

			model.DoId = AdoProvide.ExecuteScalar<long>(WriteConnectionString, bulider);

			return model.DoId;
		}

		/// <summary>
		/// 获取完成详情集合
		/// </summary>
		/// <param name="contentId"></param>
		/// <param name="workId"></param>
		/// <returns></returns>
		public List<UnitWorksContract> GetUnitWorks(long contentId, long workId)
		{
			var _list = new List<UnitWorksContract>();

			string sql = "SELECT * FROM unitworks WHERE ContentId = @ContentId AND WorkId = @WorkId";

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@ContentId", contentId), new MySqlParameter("@WorkId", workId)))
			{
				while (dr.Read())
				{
					_list.Add(new UnitWorksContract()
					{
						DoId = dr.GetInt64("DoId"),
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						WorkId = dr.GetInt64("WorkId"),
						DoWorkId = dr.GetInt64("DoWorkId"),
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						WorkScore = dr.GetDecimal("WorkScore"),
						ActualScore = dr.GetDecimal("ActualScore"),
						CorrectRate = dr.GetDecimal("CorrectRate"),
						WorkLong = dr.GetInt32("WorkLong"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						IsTimeOut = dr.GetBoolean("IsTimeOut"),
						SubmitCount = dr.GetInt32("SubmitCount"),
						Comment = dr.GetString("Comment"),
						CommentType = dr.GetInt32("CommentType"),
						Status = dr.GetInt32("Status")
					});
				}
				//_list = EntityHelper.GetList<UnitWorksContract>(dr); 对外提供API的接口需要使用ADO.NET 响应时间有10ms左右的区别
			}

			return _list;
		}

		/// <summary>
		/// 获取用户完成详情集合
		/// </summary>
		/// <param name="doWorkId"></param>
		/// <returns></returns>
		public List<UnitWorksContract> GetUserUnitWorks(long doWorkId)
		{
			string sql = "SELECT * FROM unitworks WHERE DoWorkId = @DoWorkId";

			var _list = new List<UnitWorksContract>();

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@DoWorkId", doWorkId)))
			{
				while (dr.Read())
				{
					_list.Add(new UnitWorksContract()
					{
						DoId = dr.GetInt64("DoId"),
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						WorkId = dr.GetInt64("WorkId"),
						DoWorkId = dr.GetInt64("DoWorkId"),
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						WorkScore = dr.GetDecimal("WorkScore"),
						ActualScore = dr.GetDecimal("ActualScore"),
						CorrectRate = dr.GetDecimal("CorrectRate"),
						WorkLong = dr.GetInt32("WorkLong"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						IsTimeOut = dr.GetBoolean("IsTimeOut"),
						SubmitCount = dr.GetInt32("SubmitCount"),
						Comment = dr.GetString("Comment"),
						CommentType = dr.GetInt32("CommentType"),
						Status = dr.GetInt32("Status")
					});
				}
			}

			return _list;
		}

		/// <summary>
		/// 获取用户单个单元完成详情
		/// </summary>
		/// <param name="contentId"></param>
		/// <param name="doworkId"></param>
		/// <returns></returns>
		public UnitWorksContract GetUserUnitWork(long contentId, long doworkId)
		{
			string sql = "SELECT * FROM unitworks WHERE ContentId = @ContentId AND DoWorkId = @DoWorkId";

			UnitWorksContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@ContentId", contentId), new MySqlParameter("@DoWorkId", doworkId)))
			{
				if (dr.Read())
				{
					model = new UnitWorksContract()
					{
						DoId = dr.GetInt64("DoId"),
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						WorkId = dr.GetInt64("WorkId"),
						DoWorkId = dr.GetInt64("DoWorkId"),
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						WorkScore = dr.GetDecimal("WorkScore"),
						ActualScore = dr.GetDecimal("ActualScore"),
						CorrectRate = dr.GetDecimal("CorrectRate"),
						WorkLong = dr.GetInt32("WorkLong"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						IsTimeOut = dr.GetBoolean("IsTimeOut"),
						SubmitCount = dr.GetInt32("SubmitCount"),
						Comment = dr.GetString("Comment"),
						CommentType = dr.GetInt32("CommentType"),
						Status = dr.GetInt32("Status")
					};
				}
			}
			return model;
		}


		/// <summary>
		/// 获取用户单个单元完成详情
		/// </summary>
		/// <param name="doId"></param>
		/// <returns></returns>
		public UnitWorksContract GetUserUnitWork(long doId)
		{
			string sql = "SELECT * FROM unitworks WHERE DoId = @DoId";

			UnitWorksContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@DoId", doId)))
			{
				if (dr.Read())
				{
					model = new UnitWorksContract()
					{
						DoId = dr.GetInt64("DoId"),
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						WorkId = dr.GetInt64("WorkId"),
						DoWorkId = dr.GetInt64("DoWorkId"),
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						WorkScore = dr.GetDecimal("WorkScore"),
						ActualScore = dr.GetDecimal("ActualScore"),
						CorrectRate = dr.GetDecimal("CorrectRate"),
						WorkLong = dr.GetInt32("WorkLong"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						IsTimeOut = dr.GetBoolean("IsTimeOut"),
						SubmitCount = dr.GetInt32("SubmitCount"),
						Comment = dr.GetString("Comment"),
						CommentType = dr.GetInt32("CommentType"),
						Status = dr.GetInt32("Status")
					};
				}
			}
			return model;
		}

		/// <summary>
		/// 获取单元模块完成详情
		/// </summary>
		/// <param name="recordIds"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public List<UnitWorksContract> GetUserUnitWorks(int userId, IEnumerable<long> recordIds)
		{
			var _list = new List<UnitWorksContract>();

			if (null == recordIds || !recordIds.Any())
			{
				return _list;
			}

			string sql = string.Format("SELECT * FROM unitworks WHERE RecordId IN ({0}) AND SubmitUserId = @SubmitUserId", string.Join(",", recordIds));

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@SubmitUserId", userId)))
			{
				while (dr.Read())
				{
					_list.Add(new UnitWorksContract()
					{
						DoId = dr.GetInt64("DoId"),
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						WorkId = dr.GetInt64("WorkId"),
						DoWorkId = dr.GetInt64("DoWorkId"),
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						WorkScore = dr.GetDecimal("WorkScore"),
						ActualScore = dr.GetDecimal("ActualScore"),
						CorrectRate = dr.GetDecimal("CorrectRate"),
						WorkLong = dr.GetInt32("WorkLong"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						IsTimeOut = dr.GetBoolean("IsTimeOut"),
						SubmitCount = dr.GetInt32("SubmitCount"),
						Comment = dr.GetString("Comment"),
						CommentType = dr.GetInt32("CommentType"),
						Status = dr.GetInt32("Status")
					});
				}
			}
			return _list;
		}

		/// <summary>
		/// 获取用户所有单元模块完成详情
		/// </summary>
		/// <param name="recordId"></param>
		/// <param name="workId"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public List<UnitWorksContract> GetUserUnitWorks(long recordId, long workId, int userId)
		{
			var _list = new List<UnitWorksContract>();

			string sql = "SELECT * FROM unitworks WHERE RecordId=@RecordId AND WorkId=@WorkId AND SubmitUserId=@SubmitUserId";

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@RecordId", recordId), new MySqlParameter("@WorkId", workId), new MySqlParameter("@SubmitUserId", userId)))
			{
				while (dr.Read())
				{
					_list.Add(new UnitWorksContract()
					{
						DoId = dr.GetInt64("DoId"),
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						WorkId = dr.GetInt64("WorkId"),
						DoWorkId = dr.GetInt64("DoWorkId"),
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						WorkScore = dr.GetDecimal("WorkScore"),
						ActualScore = dr.GetDecimal("ActualScore"),
						CorrectRate = dr.GetDecimal("CorrectRate"),
						WorkLong = dr.GetInt32("WorkLong"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						IsTimeOut = dr.GetBoolean("IsTimeOut"),
						SubmitCount = dr.GetInt32("SubmitCount"),
						Comment = dr.GetString("Comment"),
						CommentType = dr.GetInt32("CommentType"),
						Status = dr.GetInt32("Status")
					});
				}
			}
			return _list;
		}

		/// <summary>
		/// 更新记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public bool Update(UnitWorksContract model)
		{
			var bulider = new UpdateBuilder("unitworks")
			   .RegisterField("ContentId", model.ContentId)
			   .RegisterField("RecordId", model.RecordId)
			   .RegisterField("WorkId", model.WorkId)
			   .RegisterField("DoWorkId", model.DoWorkId)
			   .RegisterField("SubmitUserId", model.SubmitUserId)
			   .RegisterField("SubmitUserName", model.SubmitUserName)
			   .RegisterField("WorkScore", model.WorkScore)
			   .RegisterField("ActualScore", model.ActualScore)
			   .RegisterField("CorrectRate", model.CorrectRate)
			   .RegisterField("WorkLong", model.WorkLong)
			   .RegisterField("SubmitDate", model.SubmitDate)
			   .RegisterField("IsTimeOut", model.IsTimeOut)
			   .RegisterField("SubmitCount", model.SubmitCount)
			   .RegisterField("Comment", model.Comment)
			   .RegisterField("CommentType", model.CommentType)
			   .RegisterField("Status", model.Status)
			   .RegisterClause(new EqualBuilder<long>("DoId", model.DoId));

			return AdoProvide.ExecuteNonQuery(WriteConnectionString, bulider) > 0;
		}

		/// <summary>
		/// 作业点评
		/// </summary>
		/// <param name="userIds"></param>
		/// <param name="workId"></param>
		/// <param name="contentId"></param>
		/// <param name="content"></param>
		/// <param name="commentType"></param>
		/// <returns></returns>
		public bool CommentUnitWorks(IEnumerable<int> userIds, long workId, long contentId, string content, int commentType)
		{
			if (null == userIds || !userIds.Any() || string.IsNullOrWhiteSpace(content))
			{
				return false;
			}

			var builder = @"UPDATE unitworks SET `Comment` = @comment,CommentType = @commentType
                            WHERE WorkId = @workId AND ContentId = @contentId AND SubmitUserId IN({0})";

			builder = string.Format(builder, string.Join(",", userIds));

			var _params = new IDataParameter[] { 
                AdoProvide.BuildParameter("@workId",workId),
                AdoProvide.BuildParameter("@contentId",contentId),
                AdoProvide.BuildParameter("@commentType",commentType),
                AdoProvide.BuildParameter("@comment",content)
            };

			return AdoProvide.ExecuteNonQuery(WriteConnectionString, builder, _params) > 0;
		}

		/// <summary>
		/// 是否完成本次所有的单元作业
		/// </summary>
		/// <param name="recordId"></param>
		/// <param name="doWorkId"></param>
		/// <returns></returns>
		public bool IsCompletedAllUnits(long recordId, long doWorkId)
		{
			var builder = "SELECT COUNT(*) = (SELECT COUNT(*) FROM workResource WHERE RecordId = @recordId) FROM unitworks WHERE DoWorkId = @doWorkId AND RecordId = @recordId AND `Status` = 3";

			var _params = new IDataParameter[] { 
                AdoProvide.BuildParameter("@recordId",recordId),
                AdoProvide.BuildParameter("@doWorkId",doWorkId)
            };

			return AdoProvide.ExecuteScalar<int>(WriteConnectionString, builder, _params) == 1;
		}

		#endregion

		#region unitSummary
		/// <summary>
		/// 获取单元完成情况汇总
		/// </summary>
		public UnitSummaryContract GetUnitSummary(long contentId, long workId)
		{
			string sql = "SELECT * FROM unitsummary WHERE ContentId = @ContentId AND WorkId = @WorkId";

			UnitSummaryContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@workId", workId), new MySqlParameter("@ContentId", contentId)))
			{
				if (dr.Read())
				{
					model = new UnitSummaryContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						WorkId = dr.GetInt64("WorkId"),
						RecordId = dr.GetInt64("RecordId"),
						TotalNum = dr.GetInt32("TotalNum"),
						CompletedNum = dr.GetInt32("CompletedNum"),
						MarkNum = dr.GetInt32("MarkNum"),
						CommentNum = dr.GetInt32("CommentNum")
					};
				}
			}

			return model;
		}

		/// <summary>
		/// 获取单元完成情况汇总
		/// </summary>
		public List<UnitSummaryContract> GetUnitSummarys(long recordId, long workId)
		{
			var _list = new List<UnitSummaryContract>();

			string sql = "SELECT * FROM unitsummary WHERE workId = @workId AND recordId = @recordId";

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@workId", workId), new MySqlParameter("@recordId", recordId)))
			{
				while (dr.Read())
				{
					_list.Add(new UnitSummaryContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						WorkId = dr.GetInt64("WorkId"),
						RecordId = dr.GetInt64("RecordId"),
						TotalNum = dr.GetInt32("TotalNum"),
						CompletedNum = dr.GetInt32("CompletedNum"),
						MarkNum = dr.GetInt32("MarkNum"),
						CommentNum = dr.GetInt32("CommentNum")
					});
				}
			}

			return _list;
		}

		/// <summary>
		/// 更新单元汇总
		/// </summary>
		/// <param name="contentId"></param>
		/// <param name="workId"></param>
		/// <param name="recordId"></param>
		/// <param name="totalNum"></param>
		public void SetUnitSummary(long contentId, long workId, long recordId, int totalNum)
		{
			var builder = "CALL Proc_CensusUnitSummary(@contentId,@workId,@recordId,@totalNum)";

			var _params = new IDataParameter[] { 
                  AdoProvide.BuildParameter("@contentId", contentId),
                  AdoProvide.BuildParameter("@workId", workId),
                  AdoProvide.BuildParameter("@recordId", recordId),
                  AdoProvide.BuildParameter("@totalNum", totalNum)
             };

			AdoProvide.ExecuteNonQuery(WriteConnectionString, builder, _params);
		}
		#endregion

		#region 作业统计
		/// <summary>
		/// 获取作业统计
		/// </summary>
		/// <param name="userIdList"></param>
		/// <returns></returns>
		public List<WorkCensusContract> GetWorkCensus(IEnumerable<int> userIdList, DateTime beginDate, DateTime endDate, int moduleId = 0)
		{
			var _list = new List<WorkCensusContract>();

			if (null == userIdList || !userIdList.Any())
			{
				return _list;
			}

			var sql = string.Empty;
			if (moduleId == 0)
			{
				sql = string.Format(@"SELECT submitUserId AS UserId,submitUserName AS UserName,COUNT(*) AS TotalWorkNum,SUM(submitCount) AS TotalSubmitNum,SUM(workLong) AS TotalWorkLong FROM unitworks
									  WHERE SubmitUserId IN ({0}) AND SubmitDate BETWEEN @beginDate AND @endDate GROUP BY SubmitUserId", string.Join(",", userIdList));
			}
			else
			{
				sql = string.Format(@"SELECT submitUserId AS UserId,submitUserName AS UserName,COUNT(*) AS TotalWorkNum,SUM(submitCount) AS TotalSubmitNum,SUM(workLong) AS TotalWorkLong FROM unitworks
								      INNER JOIN workresource ON unitworks.ContentId = workresource.ContentId
								      WHERE SubmitUserId IN ({0}) AND SubmitUserId IN ({0}) AND SubmitDate BETWEEN @beginDate AND @endDate AND workresource.ModuleId = @moduleId
								      GROUP BY SubmitUserId", string.Join(",", userIdList));
			}

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@beginDate", beginDate), new MySqlParameter("@endDate", endDate), new MySqlParameter("@moduleId", moduleId)))
			{
				while (dr.Read())
				{
					_list.Add(new WorkCensusContract()
					{
						UserId = dr.GetInt32("UserId"),
						UserName = dr.GetString("UserName"),
						TotalWorkNum = dr.GetInt32("TotalWorkNum"),
						TotalSubmitNum = dr.GetInt32("TotalSubmitNum"),
						TotalWorkLong = dr.GetInt64("TotalWorkLong")
					});
				}
			}

			return _list;
		}
		#endregion
	}
}
