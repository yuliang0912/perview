using CiWong.Infrastructure.Data;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Infrastructure.Data;
using System.Linq;

namespace CiWong.Resource.Preview.Repositories
{
    internal class SupportRepository : RepositoryBase
    {
        public bool Insert(SupportContract model)
        {
            var bulider = new InsertBuilder("support")
            .RegisterField("ResourceId", model.ResourceId)
            .RegisterField("SupportNum", model.SupportNum)
            .RegisterField("OpposeNum", model.OpposeNum)
            .RegisterField("ReadNum", model.ReadNum);
            return AdoProvide.ExecuteNonQuery(WriteConnectionString, bulider) > 0;
        }

        public bool Update(SupportContract model)
        {
            var bulider = new UpdateBuilder("support")
            .RegisterField("SupportNum", model.SupportNum)
            .RegisterField("OpposeNum", model.OpposeNum)
            .RegisterClause(new EqualBuilder<long>("ResourceId", model.ResourceId));
            return AdoProvide.ExecuteNonQuery(WriteConnectionString, bulider) > 0;
        }

        public SupportContract GetModelByResourceId(long resourceId)
        {
            var bulider = new SelectBuilder("select * from support") //查询的SQL语句.
                .RegisterEqualClause("ResourceId", resourceId);
            //自动根据dataReader转成实体
            return AdoProvide.ExecuteReader(ReadConnectionString, bulider, reader => EntityHelper.GetEntity<SupportContract>(reader)).FirstOrDefault();

        }

        public bool InsertRecord(SupportRecordContract model)
        {
            var bulider = new InsertBuilder("supportrecord")
            .RegisterField("UserId", model.UserId)
            .RegisterField("UserName", model.UserName)
            .RegisterField("ResourceId", model.ResourceId)
            .RegisterField("CreateDate", model.CreatDate)
            .RegisterField("Status", model.Status);
            return AdoProvide.ExecuteNonQuery(WriteConnectionString, bulider) > 0;
        }

        public bool IsSupport(int UserId, long ResourceId)
        {
            var bulider = new SelectBuilder("select id from supportrecord")
            .RegisterEqualClause("UserId", UserId)
            .RegisterEqualClause("ResourceId", ResourceId);
            return AdoProvide.ExecuteScalar<int>(ReadConnectionString, bulider) > 0;
        }
    }
}
