using CiWong.Infrastructure.Data;
using CiWong.Resource.Preview.DataContracts;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CiWong.Resource.Preview.Repositories
{
	internal class FileWorksRepository : RepositoryBase
	{
		/// <summary>
		/// 添加单元作业完成记录,返回自增id
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public long Insert(FileWorksContracts model)
		{
			var bulider = new InsertBuilder("fileWorks")
				   .RegisterField("RecordId", model.RecordId)
				   .RegisterField("WorkId", model.WorkId)
				   .RegisterField("DoWorkId", model.DoWorkId)
				   .RegisterField("SubmitUserId", model.SubmitUserId)
				   .RegisterField("SubmitUserName", model.SubmitUserName ?? string.Empty)
				   .RegisterField("WorkLevel", model.WorkLevel)
				   .RegisterField("WorkLong", model.WorkLong)
				   .RegisterField("SubmitDate", model.SubmitDate)
				   .RegisterField("IsTimeOut", model.IsTimeOut)
				   .RegisterField("SubmitCount", model.SubmitCount)
				   .RegisterField("Message", model.Message ?? string.Empty)
				   .RegisterField("Comment", model.Comment ?? string.Empty)
				   .RegisterField("CommentType", model.CommentType)
				   .RegisterField("FileCount", model.FileCount)
				   .RegisterField("Status", model.Status);

			model.DoId = AdoProvide.ExecuteScalar<long>(WriteConnectionString, bulider);

			return model.DoId;
		}


		/// <summary>
		/// 更新记录
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public bool Update(FileWorksContracts model)
		{
			var bulider = new UpdateBuilder("fileWorks")
			   .RegisterField("WorkLevel", model.WorkLevel)
			   .RegisterField("WorkLong", model.WorkLong)
			   .RegisterField("SubmitDate", model.SubmitDate)
			   .RegisterField("IsTimeOut", model.IsTimeOut)
			   .RegisterField("SubmitCount", model.SubmitCount)
			   .RegisterField("Message", model.Message ?? string.Empty)
			   .RegisterField("Comment", model.Comment)
			   .RegisterField("CommentType", model.CommentType)
			   .RegisterField("FileCount", model.FileCount)
			   .RegisterField("Status", model.Status)
			   .RegisterClause(new EqualBuilder<long>("DoId", model.DoId));

			return AdoProvide.ExecuteNonQuery(WriteConnectionString, bulider) > 0;
		}

		/// <summary>
		/// 更新附件作业状态
		/// </summary>
		/// <param name="doWorkId"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public bool UpdateStatus(long doWorkId, int status)
		{
			var bulider = new UpdateBuilder("fileWorks")
				.RegisterField("Status", status)
				.RegisterClause(new EqualBuilder<long>("DoWorkId", doWorkId));

			return AdoProvide.ExecuteNonQuery(WriteConnectionString, bulider) > 0;
		}

		/// <summary>
		/// 获取附件作业完成详情集合
		/// </summary>
		public List<FileWorksContracts> GetFileWorks(long recordId, long workId)
		{
			string sql = "SELECT * FROM fileWorks WHERE RecordId = @RecordId AND WorkId = @WorkId ORDER BY WorkLevel DESC,SubmitDate ASC";

			var _list = new List<FileWorksContracts>();

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@RecordId", recordId), new MySqlParameter("@WorkId", workId)))
			{
				while (dr.Read())
				{
					_list.Add(new FileWorksContracts()
					{
						DoId = dr.GetInt64("DoId"),
						RecordId = dr.GetInt64("RecordId"),
						WorkId = dr.GetInt64("WorkId"),
						DoWorkId = dr.GetInt64("DoWorkId"),
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						WorkLevel = dr.GetDecimal("WorkLevel"),
						WorkLong = dr.GetInt32("WorkLong"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						IsTimeOut = dr.GetBoolean("IsTimeOut"),
						SubmitCount = dr.GetInt32("SubmitCount"),
						Message = dr.GetString("Message"),
						Comment = dr.GetString("Comment"),
						CommentType = dr.GetInt32("CommentType"),
						Status = dr.GetInt32("Status"),
						FileCount = dr.GetInt32("FileCount")
					});
				}
			}

			return _list;
		}

		/// <summary>
		/// 获取用户附件作业完成详情
		/// </summary>
		/// <returns></returns>
		public FileWorksContracts GetUserFileWork(long doWorkId)
		{
			string sql = "SELECT * FROM fileWorks WHERE DoWorkId = @DoWorkId";

			FileWorksContracts model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@DoWorkId", doWorkId)))
			{
				if (dr.Read())
				{
					model = new FileWorksContracts()
					{
						DoId = dr.GetInt64("DoId"),
						RecordId = dr.GetInt64("RecordId"),
						WorkId = dr.GetInt64("WorkId"),
						DoWorkId = dr.GetInt64("DoWorkId"),
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						SubmitUserName = dr.GetString("SubmitUserName"),
						WorkLevel = dr.GetDecimal("WorkLevel"),
						WorkLong = dr.GetInt32("WorkLong"),
						SubmitDate = dr.GetDateTime("SubmitDate"),
						IsTimeOut = dr.GetBoolean("IsTimeOut"),
						SubmitCount = dr.GetInt32("SubmitCount"),
						Message = dr.GetString("Message"),
						Comment = dr.GetString("Comment"),
						CommentType = dr.GetInt32("CommentType"),
						Status = dr.GetInt32("Status"),
						FileCount = dr.GetInt32("FileCount")
					};
				}
			}
			return model;
		}

		/// <summary>
		/// 批量作业点评
		/// </summary>
		/// <param name="userIds">用户更ID</param>
		/// <param name="workId">作业ID</param>
		/// <param name="recordId">附件资源包ID</param>
		/// <param name="workLevel">作业评分</param>
		/// <param name="content">点评内容</param>
		/// <param name="commentType">点评类型</param>
		/// <returns></returns>
		public bool CommentFileWorks(IEnumerable<int> userIds, long workId, long recordId, decimal workLevel, string content, int commentType)
		{
			if (null == userIds || !userIds.Any() || string.IsNullOrWhiteSpace(content))
			{
				return false;
			}

			var builder = @"UPDATE fileworks SET WorkLevel = @workLevel, `Comment` = @comment,CommentType = @commentType
                            WHERE WorkId = @workId AND RecordId = @recordId AND SubmitUserId IN({0})";

			builder = string.Format(builder, string.Join(",", userIds));

			var _params = new IDataParameter[] { 
                AdoProvide.BuildParameter("@workId",workId),
                AdoProvide.BuildParameter("@recordId",recordId),
				AdoProvide.BuildParameter("@workLevel",workLevel),
                AdoProvide.BuildParameter("@commentType",commentType),
                AdoProvide.BuildParameter("@comment",content??string.Empty)
            };

			return AdoProvide.ExecuteNonQuery(WriteConnectionString, builder, _params) > 0;
		}
	}
}
