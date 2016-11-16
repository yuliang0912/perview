using CiWong.Infrastructure.Data;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Infrastructure.Data;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace CiWong.Resource.Preview.Repositories
{
    internal class WorkAnswerRepository : RepositoryBase
    {
		/// <summary>
		/// 获取单个作业答案
		/// </summary>
		/// <param name="doId"></param>
		/// <param name="answerType"></param>
		/// <returns></returns>
		public WorkAnswerContract GetAnswer(long doId, int answerType, long versionId)
		{
			var sql = "SELECT * FROM workanswer WHERE DoId = @DoId AND AnswerType = @AnswerType AND VersionId = @VersionId";

			WorkAnswerContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@DoId", doId), new MySqlParameter("@AnswerType", answerType), new MySqlParameter("@VersionId", versionId)))
			{
				if (dr.Read())
				{
					model = new WorkAnswerContract()
					{
						Id = dr.GetInt64("Id"),
						DoId = dr.GetInt64("DoId"),
						VersionId = dr.GetInt64("VersionId"),
						AnswerType = dr.GetInt32("AnswerType"),
						AnswerContent = dr.GetString("AnswerContent"),
						ResourceType = dr.GetString("ResourceType"),
						Score = dr.GetDecimal("Score"),
						Assess = dr.GetInt32("Assess")
					};
				}
			}
			return model;
		}

        /// <summary>
        /// 查询作业答案
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="answerType"></param>
        /// <returns></returns>
		public List<WorkAnswerContract> GetAnswers(long doId, int answerType)
		{
			string sql = "SELECT * FROM workanswer WHERE DoId = @DoId AND AnswerType = @AnswerType";

			var _list = new List<WorkAnswerContract>();

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@DoId", doId), new MySqlParameter("@AnswerType", answerType)))
			{
				while (dr.Read())
				{
					_list.Add(new WorkAnswerContract()
					{
						Id = dr.GetInt64("Id"),
						DoId = dr.GetInt64("DoId"),
						VersionId = dr.GetInt64("VersionId"),
						AnswerType = dr.GetInt32("AnswerType"),
						AnswerContent = dr.GetString("AnswerContent"),
						ResourceType = dr.GetString("ResourceType"),
						Score = dr.GetDecimal("Score"),
						Assess = dr.GetInt32("Assess")
					});
				}
			}
			return _list;
		}


        /// <summary>
        /// 批量获取作业答案
        /// </summary>
		public List<WorkAnswerContract> GetUnitWorkAnswers(long workId, long contentId, bool hasAnswerContent = false)
		{
			var sql = @"SELECT unitworks.SubmitUserId,workanswer.DoId,workanswer.VersionId,workanswer.Score,workanswer.Assess {0} FROM unitworks 
                        INNER JOIN workanswer ON unitworks.DoId =  workanswer.DoId
                        WHERE unitworks.WorkId = @workId AND unitworks.ContentId = @contentId AND workanswer.AnswerType = 1";

			sql = string.Format(sql, hasAnswerContent ? ",workanswer.answerContent" : string.Empty);

			var _list = new List<WorkAnswerContract>();

			WorkAnswerContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@workId", workId), new MySqlParameter("@contentId", contentId)))
			{
				while (dr.Read())
				{
					model = new WorkAnswerContract()
					{
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						DoId = dr.GetInt64("DoId"),
						VersionId = dr.GetInt64("VersionId"),
						Score = dr.GetDecimal("Score"),
						Assess = dr.GetInt32("Assess")
					};
					if (hasAnswerContent)
					{
						model.AnswerContent = dr.GetString("AnswerContent");
					}
					_list.Add(model);
				}
			}

			return _list;
		}

		/// <summary>
		/// 批量获取作业答案
		/// </summary>
		public List<WorkAnswerContract> GetFileWorkAnswers(long workId, long recordId, bool hasAnswerContent = false)
		{
			var sql = @"SELECT fileworks.SubmitUserId,workanswer.DoId,workanswer.VersionId,workanswer.Score,workanswer.Assess {0} FROM fileworks 
                        INNER JOIN workanswer ON fileworks.DoId =  workanswer.DoId
                        WHERE fileworks.WorkId = @workId AND fileworks.RecordId = @recordId AND workanswer.AnswerType = 3";

			sql = string.Format(sql, hasAnswerContent ? ",workanswer.answerContent" : string.Empty);

			var _list = new List<WorkAnswerContract>();

			WorkAnswerContract model = null;

			using (var dr = MySqlHelper.ExecuteReader(ReadConnectionString, sql, new MySqlParameter("@workId", workId), new MySqlParameter("@recordId", recordId)))
			{
				while (dr.Read())
				{
					model = new WorkAnswerContract()
					{
						SubmitUserId = dr.GetInt32("SubmitUserId"),
						DoId = dr.GetInt64("DoId"),
						VersionId = dr.GetInt64("VersionId"),
						Score = dr.GetDecimal("Score"),
						Assess = dr.GetInt32("Assess")
					};
					if (hasAnswerContent)
					{
						model.AnswerContent = dr.GetString("AnswerContent");
					}
					_list.Add(model);
				}
			}

			return _list;
		}

        /// <summary>
        /// 批量添加答案记录
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="answerType"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Insert(long doId, int answerType, IEnumerable<WorkAnswerContract> list)
        {
            if (doId == 0 || null == list || !list.Any())
            {
                return 0;
            }

            var rowIndex = 0;
            var _params = new List<IDataParameter>() { 
                AdoProvide.BuildParameter("@DoId", doId), 
                AdoProvide.BuildParameter("@AnswerType", answerType),
            };

            var builder = new StringBuilder(@"DELETE FROM workanswer WHERE DoId=@DoId AND AnswerType=@AnswerType;");
            builder.Append("INSERT INTO workanswer(DoId,VersionId,AnswerType,AnswerContent,ResourceType,Score,Assess) VALUES");
            list.ToList().ForEach(item =>
            {
                builder.AppendFormat("(@DoId,@VersionId{0},@AnswerType,@AnswerContent{0},@ResourceType{0},@Score{0},@Assess{0}),", rowIndex);
                _params.Add(AdoProvide.BuildParameter("@VersionId" + rowIndex, item.VersionId));
                _params.Add(AdoProvide.BuildParameter("@AnswerContent" + rowIndex, item.AnswerContent ?? string.Empty));
                _params.Add(AdoProvide.BuildParameter("@ResourceType" + rowIndex, item.ResourceType ?? string.Empty));
                _params.Add(AdoProvide.BuildParameter("@Score" + rowIndex, item.Score));
                _params.Add(AdoProvide.BuildParameter("@Assess" + rowIndex, item.Assess));
                rowIndex++;
            });
            builder.Remove(builder.Length - 1, 1);
            //写入数据库
            return AdoProvide.ExecuteNonQuery(WriteConnectionString, builder.ToString(), _params.ToArray());
        }

		/// <summary>
		/// 保存批改答案
		/// </summary>
		/// <param name="workAnswer"></param>
		/// <returns></returns>
		public bool CorrectAnswer(WorkAnswerContract workAnswer)
		{
			var bulider = string.Empty;
			if (workAnswer.Id == 0)
			{
				bulider = @"INSERT INTO workanswer(DoId,VersionId,AnswerType,AnswerContent,ResourceType,Score,Assess)
							VALUES (@DoId,@VersionId,@AnswerType,@AnswerContent,@ResourceType,@Score,@Assess)";
			}
			else
			{
				bulider = @"REPLACE INTO workanswer(Id,DoId,VersionId,AnswerType,AnswerContent,ResourceType,Score,Assess)
							VALUES (@Id,@DoId,@VersionId,@AnswerType,@AnswerContent,@ResourceType,@Score,@Assess)";
			}
			var _params = new IDataParameter[] { 
                AdoProvide.BuildParameter("@Id", workAnswer.Id), 
                AdoProvide.BuildParameter("@DoId", workAnswer.DoId),
				AdoProvide.BuildParameter("@VersionId", workAnswer.VersionId),
				AdoProvide.BuildParameter("@AnswerType", workAnswer.AnswerType),
				AdoProvide.BuildParameter("@AnswerContent", workAnswer.AnswerContent??string.Empty),
				AdoProvide.BuildParameter("@ResourceType", workAnswer.ResourceType),
				AdoProvide.BuildParameter("@Score", workAnswer.Score),
				AdoProvide.BuildParameter("@Assess", workAnswer.Assess)
            };

			return AdoProvide.ExecuteNonQuery(WriteConnectionString, bulider, _params.ToArray()) > 0;
		}


        /// <summary>
        /// 查询作业答案(无效方法)
        /// </summary>
        /// <param name="doId"></param>
        /// <param name="answerType"></param>
        /// <returns></returns>
		public List<WorkAnswerContract<FlowReadAnswerEntity>> GetReadAnswers(long doId, int answerType)
		{
			var builder = new SelectBuilder("SELECT * FROM workanswer where DoId =@DoId and AnswerType=@AnswerType GROUP BY VersionId ORDER BY Score desc");
			
			var _params = new IDataParameter[] { 
                AdoProvide.BuildParameter("@DoId", doId), 
                AdoProvide.BuildParameter("@AnswerType", answerType)
            };

			return AdoProvide.ExecuteReader(ReadConnectionString, builder, reader => EntityHelper.GetEntity<WorkAnswerContract<FlowReadAnswerEntity>>(reader)).ToList();
		}
    }
}
