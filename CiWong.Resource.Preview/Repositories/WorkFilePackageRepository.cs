using CiWong.Infrastructure.Data;
using CiWong.Resource.Preview.DataContracts;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CiWong.Resource.Preview.Repositories
{
	internal class WorkFilePackageRepository : RepositoryBase
	{
		/// <summary>
		/// 获取布置信息对像
		/// </summary>
		/// <param name="recordId"></param>
		/// <returns></returns>
		public WorkFilePackageContract GetWorkFilePackage(long recordId)
		{
			//var builder = new SelectBuilder("SELECT * from workfilepackage")
			//		.RegisterEqualClause("RecordId", recordId);

			//return AdoProvide.ExecuteReader(ReadConnectionString, builder, reader => EntityHelper.GetEntity<WorkFilePackageContract>(reader)).FirstOrDefault();

			string sql = "SELECT * from workfilepackage WHERE RecordId = @RecordId";

			WorkFilePackageContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@RecordId", recordId)))
			{
				if (dr.Read())
				{
					model = new WorkFilePackageContract()
					{
						RecordId = dr.GetInt64("RecordId"),
						FilePackageName = dr.GetString("FilePackageName"),
						FilePackageType = dr.GetInt32("FilePackageType"),
						UserId = dr.GetInt32("UserId"),
						UserName = dr.GetString("UserName"),
						CreateDate = dr.GetDateTime("CreateDate"),
						Status = dr.GetInt32("Status")
					};
				}
			}

			return model;
		}

		/// <summary>
		/// 根据Id查询打包资源
		/// </summary>
		/// <param name="contentId"></param>
		/// <returns></returns>
		public WorkFileResourceContract GetWorkFileResource(long contentId)
		{
			//var bulider = new SelectBuilder("SELECT * FROM workfileresource")
			//	   .RegisterEqualClause("ContentId", contentId);
			//return AdoProvide.ExecuteReader(ReadConnectionString, bulider, reader => EntityHelper.GetEntity<WorkFileResourceContract>(reader)).FirstOrDefault();

			string sql = "SELECT * FROM workfileresource WHERE ContentId = @contentId";

			WorkFileResourceContract model = null;
			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@contentId", contentId)))
			{
				if (dr.Read())
				{
					model = new WorkFileResourceContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						FileName = dr.GetString("FileName"),
						FileUrl = dr.GetString("FileUrl"),
						FileExt = dr.GetString("FileExt"),
						FileType = dr.GetInt32("FileType"),
						FileDesc = dr.GetString("FileDesc")
					};
				}
			}

			return model;
		}

		/// <summary>
		/// 根据打包记录id查询布置资源
		/// </summary>
		/// <param name="recordId"></param>
		/// <returns></returns>
		public List<WorkFileResourceContract> GetWorkFileResources(long recordId)
		{
			//var bulider = new SelectBuilder("SELECT * FROM workfileresource")
			//	 .RegisterEqualClause("RecordId", recordId);

			//return AdoProvide.ExecuteReader(ReadConnectionString, bulider, reader => EntityHelper.GetEntity<WorkFileResourceContract>(reader)).ToList();

			string sql = "SELECT * FROM workfileresource WHERE RecordId = @RecordId";

			var _list = new List<WorkFileResourceContract>();

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@RecordId", recordId)))
			{
				while (dr.Read())
				{
					_list.Add(new WorkFileResourceContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						FileName = dr.GetString("FileName"),
						FileUrl = dr.GetString("FileUrl"),
						FileExt = dr.GetString("FileExt"),
						FileType = dr.GetInt32("FileType"),
						FileDesc = dr.GetString("FileDesc")
					});
				}
			}
			return _list;
		}

		/// <summary>
		/// 根据打包记录id查询布置资源
		/// </summary>
		/// <param name="recordIds"></param>
		/// <returns></returns>
		public List<WorkFileResourceContract> GetWorkFileResources(IEnumerable<long> recordIds)
		{
			var _list = new List<WorkFileResourceContract>();

			if (null == recordIds || !recordIds.Any())
			{
				return _list;
			}

			string sql = string.Format("SELECT * FROM workfileresource WHERE RecordId IN ({0})", string.Join(",", recordIds));

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql))
			{
				while (dr.Read())
				{
					_list.Add(new WorkFileResourceContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						FileName = dr.GetString("FileName"),
						FileUrl = dr.GetString("FileUrl"),
						FileExt = dr.GetString("FileExt"),
						FileType = dr.GetInt32("FileType"),
						FileDesc = dr.GetString("FileDesc")
					});
				}
			}

			return _list;
		}


		/// <summary>
		/// 添加资源打包布置总表
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		public long InsertWorkFilePackage(WorkFilePackageContract model)
		{
			var bulider = new InsertBuilder("workfilepackage")
				.RegisterField("RecordId", model.RecordId)
				.RegisterField("FilePackageName", model.FilePackageName)
				.RegisterField("FilePackageType", model.FilePackageType)
				.RegisterField("UserId", model.UserId)
				.RegisterField("UserName", model.UserName ?? string.Empty)
				.RegisterField("CreateDate", model.CreateDate)
				.RegisterField("Status", model.Status);

			return AdoProvide.ExecuteScalar<long>(WriteConnectionString, bulider);
		}

		/// <summary>
		/// 批量添加作业附件
		/// </summary>
		/// <param name="list"></param>
		/// <returns></returns>
		public int InsertWorkFileResources(IEnumerable<WorkFileResourceContract> list, long recordId)
		{
			if (null == list || !list.Any())
			{
				return 0;
			}

			var _params = new List<IDataParameter>();
			_params.Add(AdoProvide.BuildParameter("@RecordId", recordId));
			var builder = new StringBuilder("INSERT INTO workfileresource(RecordId,FileName,FileUrl,FileExt,FileType,FileDesc) VALUES");
			var rowIndex = 0;
			list.ToList().ForEach(item =>
			{
				builder.AppendFormat("(@RecordId,@FileName{0},@FileUrl{0},@FileExt{0},@FileType{0},@FileDesc{0}),", rowIndex);
				_params.Add(AdoProvide.BuildParameter("@FileName" + rowIndex, item.FileName));
				_params.Add(AdoProvide.BuildParameter("@FileUrl" + rowIndex, item.FileUrl));
				_params.Add(AdoProvide.BuildParameter("@FileExt" + rowIndex, item.FileExt));
				_params.Add(AdoProvide.BuildParameter("@FileType" + rowIndex, item.FileType));
				_params.Add(AdoProvide.BuildParameter("@FileDesc" + rowIndex, item.FileDesc));
				rowIndex++;
			});

			builder.Remove(builder.Length - 1, 1);

			//批量写入数据库	
			return AdoProvide.ExecuteNonQuery(WriteConnectionString, builder.ToString(), _params.ToArray());
		}
	}
}
