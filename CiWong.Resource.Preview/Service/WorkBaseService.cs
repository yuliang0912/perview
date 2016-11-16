using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.Repositories;

namespace CiWong.Resource.Preview.Service
{
    /// <summary>
    /// 此处为对外业务接口
    /// </summary>
    public class WorkBaseService
    {
        /// <summary>
        /// 可以考虑使用IOC进行实例化管理.待讨论
        /// </summary>
        private readonly WorkBaseRepository _workBase = new WorkBaseRepository();
        /// <summary>
        /// 获取基础作业信息
        /// </summary>
        public WorkBaseContract GetWorkBase(long workId)
        {
            return _workBase.GetWorkBase(workId);
        }

        /// <summary>
        /// 获取用户作业信息
        /// </summary>
        public DoWorkBaseContract GetDoWorkBase(long doWorkId)
        {
            return _workBase.GetDoWorkBase(doWorkId);
        }
    }
}
