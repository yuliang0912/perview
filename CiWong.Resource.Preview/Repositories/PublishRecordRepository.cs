using CiWong.Infrastructure.Data;
using CiWong.Resource.Preview.DataContracts;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CiWong.Resource.Preview.Repositories
{
    internal class PublishRecordRepository : RepositoryBase
    {

        /// <summary>
        /// 获取布置信息对像
        /// </summary>
        /// <param name="recordId"></param>
        /// <returns></returns>
		public PublishRecordContract GetWorkPackage(long recordId)
		{
			PublishRecordContract model = null;

			string sql = "SELECT * from publishRecord WHERE RecordId = @RecordId";

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@RecordId", recordId)))
			{
				if (dr.Read())
				{
					model = new PublishRecordContract()
					{
						RecordId = dr.GetInt64("RecordId"),
						ProductId = dr.GetString("ProductId"),
						AppId = dr.GetInt32("AppId"),
						PackageId = dr.GetInt64("PackageId"),
						PackageName = dr.GetString("PackageName"),
						PackageType = dr.GetInt32("PackageType"),
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
		/// 获取资源包创建记录
		/// </summary>
		/// <param name="userIds"></param>
		/// <param name="packageId"></param>
		/// <param name="cid"></param>
		/// <param name="moduleIds"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public List<PublishRecordContract> GetPublishRecord(IEnumerable<int> userIds, IEnumerable<int> moduleIds, long packageId, string cid, int pageSize)
		{
			var _list = new List<PublishRecordContract>();

			if (null == userIds || !userIds.Any())
			{
				return _list;
			}

			string sql = @"SELECT DISTINCT publishrecord.* FROM publishrecord
						   INNER JOIN workresource ON publishrecord.RecordId = workresource.RecordId
						   WHERE publishrecord.PackageId = @PackageId AND publishrecord.UserId IN ({0}) AND workresource.TaskId = @TaskId {1} 
						   ORDER BY RecordId DESC LIMIT @Limit";

			sql = string.Format(sql, string.Join(",", userIds), null != moduleIds && moduleIds.Any() ? string.Format(" AND workresource.ModuleId IN ({0}) ", string.Join(",", moduleIds)) : string.Empty);

			var _parms = new MySqlParameter[] { 
				new MySqlParameter("@PackageId", packageId),
				new MySqlParameter("@TaskId", cid),
				new MySqlParameter("@Limit", pageSize),
			};

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, _parms))
			{
				while (dr.Read())
				{
					_list.Add(new PublishRecordContract()
					{
						RecordId = dr.GetInt64("RecordId"),
						ProductId = dr.GetString("ProductId"),
						AppId = dr.GetInt32("AppId"),
						PackageId = dr.GetInt64("PackageId"),
						PackageName = dr.GetString("PackageName"),
						PackageType = dr.GetInt32("PackageType"),
						UserId = dr.GetInt32("UserId"),
						UserName = dr.GetString("UserName"),
						CreateDate = dr.GetDateTime("CreateDate"),
						Status = dr.GetInt32("Status")
					});
				}
			}

			return _list;
		}

        /// <summary>
        /// 根据Id查询打包资源
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
		public WorkResourceContract GetWorkResource(long contentId)
		{
			string sql = "SELECT * FROM workResource WHERE ContentId = @ContentId";

			WorkResourceContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@ContentId", contentId)))
			{
				if (dr.Read())
				{
					model = new WorkResourceContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						PackageId = dr.GetInt64("PackageId"),
						TaskId = dr.GetString("TaskId"),
						ModuleId = dr.GetInt32("ModuleId"),
						VersionId = dr.GetInt64("VersionId"),
						RelationPath = dr.GetString("RelationPath"),
						SonModuleId = dr.GetString("SonModuleId"),
						ResourceName = dr.GetString("ResourceName"),
						ResourceType = dr.GetString("ResourceType"),
						RequirementContent = dr.GetString("RequirementContent"),
						IsFull = dr.GetBoolean("IsFull")
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
		public List<WorkResourceContract> GetWorkResources(long recordId)
		{
			var _list = new List<WorkResourceContract>();

			string sql = "SELECT * FROM workResource WHERE RecordId=@RecordId";

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@RecordId", recordId)))
			{
				while (dr.Read())
				{
					_list.Add(new WorkResourceContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						PackageId = dr.GetInt64("PackageId"),
						TaskId = dr.GetString("TaskId"),
						ModuleId = dr.GetInt32("ModuleId"),
						VersionId = dr.GetInt64("VersionId"),
						RelationPath = dr.GetString("RelationPath"),
						SonModuleId = dr.GetString("SonModuleId"),
						ResourceName = dr.GetString("ResourceName"),
						ResourceType = dr.GetString("ResourceType"),
						RequirementContent = dr.GetString("RequirementContent"),
						IsFull = dr.GetBoolean("IsFull")
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
		public List<WorkResourceContract> GetWorkResources(IEnumerable<long> recordIds)
		{
			var _list = new List<WorkResourceContract>();

			if (null == recordIds || !recordIds.Any())
			{
				return _list;
			}

			string sql = string.Format("SELECT * FROM workResource WHERE RecordId IN ({0})", string.Join(",", recordIds));

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql))
			{
				while (dr.Read())
				{
					_list.Add(new WorkResourceContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						RecordId = dr.GetInt64("RecordId"),
						PackageId = dr.GetInt64("PackageId"),
						TaskId = dr.GetString("TaskId"),
						ModuleId = dr.GetInt32("ModuleId"),
						VersionId = dr.GetInt64("VersionId"),
						RelationPath = dr.GetString("RelationPath"),
						SonModuleId = dr.GetString("SonModuleId"),
						ResourceName = dr.GetString("ResourceName"),
						ResourceType = dr.GetString("ResourceType"),
						RequirementContent = dr.GetString("RequirementContent"),
						IsFull = dr.GetBoolean("IsFull")
					});
				}
			}

			return _list;
		}

        /// <summary>
        /// 根据资源内容id查询子资源
        /// </summary>
        /// <param name="contentId"></param>
        /// <returns></returns>
		public List<ResourcePartsContract> GetResourceParts(long contentId)
		{
			string sql = "SELECT * FROM resourceParts WHERE ContentId = @ContentId";

			var _list = new List<ResourcePartsContract>();

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@ContentId", contentId)))
			{
				while (dr.Read())
				{
					_list.Add(new ResourcePartsContract()
					{
						ContentId = dr.GetInt64("ContentId"),
						VersionId = dr.GetInt64("VersionId"),
						ResourceType = dr.GetString("ResourceType")
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
        public long InsertPublishRecord(PublishRecordContract model)
        {
            var bulider = new InsertBuilder("publishRecord")
                .RegisterField("ProductId", model.ProductId)
                .RegisterField("AppId", model.AppId)
                .RegisterField("PackageId", model.PackageId)
                .RegisterField("PackageName", model.PackageName ?? string.Empty)
                .RegisterField("PackageType", model.PackageType)
                .RegisterField("UserId", model.UserId)
                .RegisterField("UserName", model.UserName ?? string.Empty)
                .RegisterField("CreateDate", model.CreateDate)
                .RegisterField("Status", model.Status);

            return AdoProvide.ExecuteScalar<long>(WriteConnectionString, bulider);
        }

        /// <summary>
        /// 添加打包资源
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
		public long InsertWorkResource(WorkResourceContract model)
		{
			var bulider = new InsertBuilder("workResource")
					.RegisterField("RecordId", model.RecordId)
					.RegisterField("PackageId", model.PackageId)
					.RegisterField("TaskId", model.TaskId)
					.RegisterField("ModuleId", model.ModuleId)
					.RegisterField("VersionId", model.VersionId)
					.RegisterField("RelationPath", model.RelationPath ?? model.VersionId.ToString())
					.RegisterField("SonModuleId", model.SonModuleId ?? string.Empty)
					.RegisterField("ResourceName", model.ResourceName ?? string.Empty)
					.RegisterField("ResourceType", model.ResourceType ?? string.Empty)
					.RegisterField("RequirementContent", model.RequirementContent ?? string.Empty)
					.RegisterField("IsFull", model.IsFull);

			model.ContentId = AdoProvide.ExecuteScalar<long>(WriteConnectionString, bulider);
			return model.ContentId;
		}

        /// <summary>
        /// 批量添加试题子资源
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public int InsertResourceParts(IEnumerable<ResourcePartsContract> list)
        {
            if (null == list || !list.Any())
            {
                return 0;
            }

            var _params = new List<IDataParameter>();
            var builder = new StringBuilder("INSERT INTO resourceParts VALUES");
            var rowIndex = 0;
            list.ToList().ForEach(item =>
            {
                builder.AppendFormat("(@C{0},@V{0},@R{0}),", rowIndex);
                _params.Add(AdoProvide.BuildParameter("@C" + rowIndex, item.ContentId));
                _params.Add(AdoProvide.BuildParameter("@V" + rowIndex, item.VersionId));
                _params.Add(AdoProvide.BuildParameter("@R" + rowIndex, item.ResourceType));
                rowIndex++;
            });

            builder.Remove(builder.Length - 1, 1);

            //批量写入数据库	
            return AdoProvide.ExecuteNonQuery(WriteConnectionString, builder.ToString(), _params.ToArray());
        }
    }
}
