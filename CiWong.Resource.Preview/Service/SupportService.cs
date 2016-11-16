using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Repositories;

namespace CiWong.Resource.Preview.Service
{
    public class SupportService
    {
        private readonly SupportRepository _support = new SupportRepository();

        public bool Insert(SupportContract model)
        {
            return _support.Insert(model);
        }

        public bool Update(SupportContract model)
        {
            return _support.Update(model);
        }

        public ReturnResult<SupportContract> GetModelByResourceId(long resourceId)
        {
            var model=_support.GetModelByResourceId(resourceId);
            return new ReturnResult<SupportContract>(model);
        }

        public bool InsertRecord(SupportRecordContract model)
        {
            return _support.InsertRecord(model);
        }

        public bool IsSupport(int UserId, long ResourceId)
        {
            return _support.IsSupport(UserId, ResourceId);
        }
    }
}
