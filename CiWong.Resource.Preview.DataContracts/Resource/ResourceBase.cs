using System;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 资源实体基类
    /// </summary>
    [Serializable, DataContract(Name = "resource")]
    public abstract class ResourceBase
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        [DataMember(Name = "id")]
        public long? Id { get; set; }

        /// <summary>
        /// 资源版本Id
        /// </summary>
        [DataMember(Name = "version_id")]
        public long? VersionId { get; set; }
    }
}
