using ServiceStack.Redis;
using ServiceStack.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml;

namespace CiWong.Resource.Preview.Common
{
    /// <summary>
    /// Redis配置信息类
    /// </summary>
    internal class RedisConfigInfo : IConfigurationSectionHandler
    {
        private UrnId urnId = null; //无用申明,解决DLL无法自动引用问题
        public List<string> ReadServerList { get; set; }
        public List<string> WriteServerList { get; set; }
        public bool AutoStart { get; set; }
        public int MaxReadPoolSize { get; set; }
        public int MaxWritePoolSize { get; set; }
        public bool IsStart { get; set; }

        public const int CACHE_SECONDS = 43200;//默认缓存12小时

        public object Create(object parent, object configContext, XmlNode section)
        {
            return new RedisConfigInfo()
            {
                AutoStart = bool.Parse(section.Attributes["AutoStart"].Value),
                MaxReadPoolSize = int.Parse(section.Attributes["MaxReadPoolSize"].Value),
                MaxWritePoolSize = int.Parse(section.Attributes["MaxWritePoolSize"].Value),
                ReadServerList = section.Attributes["ReadServerList"].Value.Split(',').ToList(),
                WriteServerList = section.Attributes["WriteServerList"].Value.Split(',').ToList(),
                IsStart = int.Parse(section.Attributes["IsStart"].Value) == 1
            };
        }
    }

    /// <summary>
    /// Redis操作辅助类
    /// </summary>
    public class RedisHelper
    {
        //Redis服务器配置信息
        private static RedisConfigInfo redisConfigInfo = ConfigurationManager.GetSection("RedisConfig") as RedisConfigInfo;

        private static PooledRedisClientManager prcm = CreateManager();

        public static PooledRedisClientManager CreateManager()
        {
            //ServiceStack.Redis.Commands
            if (prcm == null)
            {
                return new PooledRedisClientManager(redisConfigInfo.ReadServerList, redisConfigInfo.WriteServerList, new RedisClientManagerConfig()
                {
                    AutoStart = redisConfigInfo.AutoStart,
                    MaxReadPoolSize = redisConfigInfo.MaxReadPoolSize,
                    MaxWritePoolSize = redisConfigInfo.MaxWritePoolSize
                });
            }
            return prcm;
        }

        #region  操作单个对象
        /// <summary>
        /// 设置单个对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool SetItem<T>(string key, T t)
        {
            //0 就不使用redis缓存
            if (!redisConfigInfo.IsStart)
            {
                return false;
            }

            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t, new TimeSpan(0, 0, RedisConfigInfo.CACHE_SECONDS));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 设置单个对象
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="expiresTime"></param>
        /// <returns></returns>
        public static bool SetItem<T>(string key, T t, int seconds)
        {
            if (!redisConfigInfo.IsStart)
            {
                return false;
            }
            try
            {
                using (IRedisClient redis = prcm.GetClient())
                {
                    return redis.Set<T>(key, t, new TimeSpan(0, 0, seconds));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取单个对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetItem<T>(string key) where T : class
        {
            if (!redisConfigInfo.IsStart)
            {
                return default(T);
            }
            using (IRedisClient redis = prcm.GetClient())
            {
                return redis.Get<T>(key);
            }
        }

        /// <summary>
        /// 移除单个对象
        /// </summary>
        /// <param name="key"></param>
        ///  0 引用
        //public static bool RemoveItem(string key)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return false;
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        return redis.Remove(key);
        //    }
        //}
        #endregion


        #region 操作集合

        /// <summary>
        /// 添加一个项到内部的List的底部(右侧) 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="t"></param>
        ///  0 引用
        //public static void AddItemToList<T>(string key, T t)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return;
        //    }
        //    if (t == null)
        //    {
        //        return;
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var redisTypedClient = redis.As<T>();
        //        redisTypedClient.AddItemToList(redisTypedClient.Lists[key], t);
        //    }
        //}


        /// <summary>
        /// 移除指定Key的内部List中与T值相同的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        ///  0 引用
        //public static bool RemoveList<T>(string key, T t)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return false;
        //    }
        //    if (t == null)
        //    {
        //        return false;
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var redisTypedClient = redis.As<T>();
        //        return redisTypedClient.RemoveItemFromList(redisTypedClient.Lists[key], t) > 0;
        //    }
        //}

        /// <summary>
        /// 移除指定Key的内部List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        ///  0 引用
        //public static void RemoveAllList<T>(string key)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return;
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var redisTypedClient = redis.As<T>();
        //        redisTypedClient.Lists[key].RemoveAll();
        //    }
        //}

        /// <summary>
        /// 移除以keyPrefix为前缀的所有key对应的数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="keyPrefix"></param>
        ///  0 引用
        //public static void RemoveItemByKey(string keyPrefix)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return;
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var keyList = redis.SearchKeys(string.Format("{0}*", keyPrefix));
        //        if (keyList != null && keyList.Count > 0)
        //        {
        //            keyList.Where(p => p.Contains(keyPrefix)).ToList().ForEach(k =>
        //            {
        //                //RedisHelper.RemoveAllList<T>(k);
        //                redis.Remove(k);
        //            });
        //        }
        //    }
        //}

        /// <summary>
        /// 根据key获取对应List的项数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ///  0 引用
        //public static long ListCount(string key)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(long);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        return redis.GetListCount(key);
        //    }
        //}

        /// <summary>
        /// 根据Key获取集合，并取得该集合中指定下标的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        ///  0 引用
        //public static List<T> GetListByRange<T>(string key, int start, int count)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(List<T>);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var redisTypedClient = redis.As<T>();
        //        return redisTypedClient.Lists[key].GetRange(start, start + count - 1);
        //    }
        //}

        /// <summary>
        /// 根据Key获取集合，并以分页方式获取该集合中指定下标的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ///  0 引用
        //public static List<T> GetList<T>(string key, int pageIndex = 1, int pageSize = 9999)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(List<T>);
        //    }
        //    int start = pageSize * (pageIndex - 1);
        //    return GetListByRange<T>(key, start, pageSize);
        //}


        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        ///  0 引用
        //public static void SetListExpire(string key, DateTime datetime)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return;
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        redis.ExpireEntryAt(key, datetime);
        //    }
        //}
        #endregion


        #region 操作Set
        /// <summary>
        /// 根据Key查找对应的Set，并往里面插入对象T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        public static void AddItemToSet<T>(string key, T t)
        {
            if (!redisConfigInfo.IsStart)
            {
                return;
            }
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                redisTypedClient.Sets[key].Add(t);
            }
        }

        /// <summary>
        /// 判断与Key对应的Set里是否存在对象T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        ///  0个引用
        //public static bool SetContains<T>(string key, T t)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(bool);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var redisTypedClient = redis.As<T>();
        //        return redisTypedClient.Sets[key].Contains(t);
        //    }
        //}

        /// <summary>
        /// 根据key从set中获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static List<T> GetItemFromSet<T>(string key)
        {
            if (!redisConfigInfo.IsStart)
            {
                return new List<T>();
            }
            using (IRedisClient redis = prcm.GetClient())
            {
                var redisTypedClient = redis.As<T>();
                return redisTypedClient.Sets[key].GetAll().ToList();
            }
        }

        /// <summary>
        /// 根据Key查找相应的Set，并移除Set中与T相同的对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        ///  0个引用
        //public static bool SetRemove<T>(string key, T t)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(bool);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var redisTypedClient = redis.As<T>();
        //        return redisTypedClient.Sets[key].Remove(t);
        //    }
        //}
        #endregion


        #region 操作Hash （0引用）
        ///// <summary>
        ///// 判断指定HashId的Hash中是否包含指定的Key
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="dataKey"></param>
        ///// <returns></returns>
        //public static bool HashContainsEntry<T>(string key, string dataKey)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(bool);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        return redis.HashContainsEntry(key, dataKey);
        //    }
        //}

        ///// <summary>
        ///// 存储数据到hash表
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="dataKey"></param>
        ///// <returns></returns>
        //public static bool SetEntryInHash<T>(string key, string dataKey, T t)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(bool);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
        //        return redis.SetEntryInHash(key, dataKey, value);
        //    }
        //}
        ///// <summary>
        ///// 移除hash中的某值
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="dataKey"></param>
        ///// <returns></returns>
        //public static bool RemoveEntryFromHash(string key, string dataKey)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(bool);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        return redis.RemoveEntryFromHash(key, dataKey);
        //    }
        //}

        ///// <summary>
        ///// 移除整个hash
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="dataKey"></param>
        ///// <returns></returns>
        //public static bool RemoveHash(string key)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(bool);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        return redis.Remove(key);
        //    }
        //}

        ///// <summary>
        ///// 从hash表获取数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <param name="dataKey"></param>
        ///// <returns></returns>
        //public static T GetValueFromHash<T>(string key, string dataKey)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(T);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        string value = redis.GetValueFromHash(key, dataKey);
        //        return ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(value);
        //    }
        //}

        ///// <summary>
        ///// 获取整个hash的数据
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="key"></param>
        ///// <returns></returns>
        //public static List<T> GetHashValues<T>(string key)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(List<T>);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var list = redis.GetHashValues(key);
        //        if (list != null && list.Count > 0)
        //        {
        //            List<T> result = new List<T>();
        //            foreach (var item in list)
        //            {
        //                var value = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
        //                result.Add(value);
        //            }
        //            return result;
        //        }
        //        return null;
        //    }
        //}

        ///// <summary>
        ///// 设置缓存过期
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="datetime"></param>
        //public static void SetHashExpire(string key, DateTime datetime)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return;
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        redis.ExpireEntryAt(key, datetime);
        //    }
        //}
        #endregion


        #region 操作有序Set (0引用)
        /// <summary>
        ///  添加项到排序List，并按Score升序排列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <param name="score"></param>
        ///  0个引用
        //public static bool AddItemToSortedSet<T>(string key, T t, double score)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(bool);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
        //        return redis.AddItemToSortedSet(key, value, score);
        //    }
        //}

        /// <summary>
        /// 从SortedSet移除与T相等的项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        ///  0个引用
        //public static bool RemoveItemFromSortedSet<T>(string key, T t)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(bool);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        string value = ServiceStack.Text.JsonSerializer.SerializeToString<T>(t);
        //        return redis.RemoveItemFromSortedSet(key, value);
        //    }
        //}

        /// <summary>
        /// 从SortedSet中移除指定下标范围的项
        /// </summary>
        /// <param name="key"></param>
        /// <param name="size">保留的条数</param>
        /// <returns></returns>
        ///  0个引用
        //public static long RemoveRangeFromSortedSet(string key, int size, int index)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(long);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        return redis.RemoveRangeFromSortedSet(key, size, index);
        //    }
        //}

        /// <summary>
        /// 获取SortedSet的长度
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ///  0个引用
        //public static long GetSortedSetCount(string key)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(long);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        return redis.GetSortedSetCount(key);
        //    }
        //}

        /// <summary>
        /// 获取SortedSet的分页数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ///  0个引用
        //public static List<T> GetRangeFromSortedSet<T>(string key, int pageIndex, int pageSize)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(List<T>);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var list = redis.GetRangeFromSortedSet(key, (pageIndex - 1) * pageSize, pageIndex * pageSize - 1);
        //        if (list != null && list.Count > 0)
        //        {
        //            List<T> result = new List<T>();
        //            foreach (var item in list)
        //            {
        //                var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
        //                result.Add(data);
        //            }
        //            return result;
        //        }
        //    }
        //    return null;
        //}


        /// <summary>
        /// 获取SortedSet的全部数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        ///  0个引用
        //public static List<T> GetRangeFromSortedSet<T>(string key)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return default(List<T>);
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        var list = redis.GetRangeFromSortedSet(key, 0, 9999999);
        //        if (list != null && list.Count > 0)
        //        {
        //            List<T> result = new List<T>();
        //            foreach (var item in list)
        //            {
        //                var data = ServiceStack.Text.JsonSerializer.DeserializeFromString<T>(item);
        //                result.Add(data);
        //            }
        //            return result;
        //        }
        //    }
        //    return null;
        //}

        /// <summary>
        /// 设置缓存过期
        /// </summary>
        /// <param name="key"></param>
        /// <param name="datetime"></param>
        ///  0个引用
        //public static void SetSortedSetExpire(string key, DateTime datetime)
        //{
        //    if (!redisConfigInfo.IsStart)
        //    {
        //        return;
        //    }
        //    using (IRedisClient redis = prcm.GetClient())
        //    {
        //        redis.ExpireEntryAt(key, datetime);
        //    }
        //}
        #endregion

    }

}


