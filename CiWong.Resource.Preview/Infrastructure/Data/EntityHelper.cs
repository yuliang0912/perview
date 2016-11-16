using System.Collections.Generic;
using System.Data;

namespace CiWong.Resource.Preview.Infrastructure.Data
{
    /// <summary>
    /// IDataReader转化实体（Entity）操作方法  
    /// caoxilong
    /// </summary>
    internal static class EntityHelper
    {
        /// <summary>
        /// IDataReader实体转换为List实体对象 
        /// 使用说明：EntityHelper.GetEntityByReader(实体对象）(IDataReader对象)
        /// </summary>
        /// <param name="dr">IDataReader 对象</param>
        /// <returns></returns>
        public static List<T> GetList<T>(this IDataReader dr)
        {
			var list = new List<T>();

			if (null == dr)
			{
				return list;
			}

			var eblist = IDataReaderEntityBuilder<T>.CreateBuilder(dr);

			while (dr.Read())
			{
				list.Add(eblist.Build(dr));
			}
			return list;
        }

        /// <summary>
        /// IDataReader实体转换为实体对象
        /// 使用说明：EntityHelper.GetEntity(实体对象）(IDataReader对象)
        /// </summary>
        /// <param name="dr">IDataReader 对象</param>
        /// <returns></returns>
        public static T GetEntity<T>(this IDataReader dr)
        {
			if (null == dr)
			{
				return default(T);
			}
			if (dr.Read())
			{
				return IDataReaderEntityBuilder<T>.CreateBuilder(dr).Build(dr);
			}
			return default(T);
        }

        /// <summary>
        /// IDataReader实体转换为List实体对象(未使用缓存，测试使用，不推荐使用) 
        /// 使用说明：EntityHelper.GetEntityByReader(实体对象）(IDataReader对象)
        /// </summary>
        /// <param name="dr">IDataReader 对象</param>
        /// <returns></returns>
        public static T GetEntityByReaderNoCache<T>(this IDataReader dr)
        {
            if (null == dr)
            {
                return default(T);
            }
            return IDataReaderEntityBuilder<T>.CreateBuilderNoCache(dr).Build(dr);
        }
    }
}
