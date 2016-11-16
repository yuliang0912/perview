using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 作业打包记录
    /// </summary>
    [Serializable, DataContract(Name = "publish_record")]
    public class PublishRecordContract
    {
        public PublishRecordContract()
        {
            this.workResources = Enumerable.Empty<WorkResourceContract>();
        }

        /// <summary>
        /// 打包ID
        /// </summary>
        [DataMember(Name = "record_id")]
        public long RecordId { get; set; }

        /// <summary>
        /// 所属产品ID
        /// </summary>
        [DataMember(Name = "product_id")]
        public string ProductId { get; set; }

        /// <summary>
        /// 产品所属平台ID
        /// </summary>
        [DataMember(Name = "app_id")]
        public int AppId { get; set; }

        /// <summary>
        /// 跟读资源包ID
        /// </summary>
        [DataMember(Name = "package_id")]
        public long PackageId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [DataMember(Name = "package_name")]
        public string PackageName { get; set; }

        /// <summary>
        /// 产品类型(1电子书,2课程,3电子报)
        /// </summary>
        [DataMember(Name = "package_type")]
        public int PackageType { get; set; }

        /// <summary>
        /// 创建人用户ID
        /// </summary>
        [DataMember(Name = "user_id")]
        public int UserId { get; set; }

        /// <summary>
        /// 创建人用户名称
        /// </summary>
        [DataMember(Name = "user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember(Name = "create_date")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [DataMember(Name = "status")]
        public int Status { get; set; }

        /// <summary>
        /// 打包资源集合
        /// </summary>
        [DataMember(Name = "workresources")]
        public IEnumerable<WorkResourceContract> workResources { get; set; }

    }
}
