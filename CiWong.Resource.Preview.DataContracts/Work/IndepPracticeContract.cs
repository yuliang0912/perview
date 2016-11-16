using System;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 自主练习记录
    /// </summary>
    [Serializable, DataContract(Name = "indep_practice")]
    public class IndepPracticeContract
    {
        /// <summary>
        /// 主键记录ID
        /// </summary>
        [DataMember(Name = "id")]
        public long Id { get; set; }

        /// <summary>
        /// 产品ID
        /// </summary>
        [DataMember(Name = "product_id")]
        public string ProductId { get; set; }

        /// <summary>
        /// 平台ID
        /// </summary>
        [DataMember(Name = "app_id")]
        public int AppId { get; set; }

        /// <summary>
        /// 资源包ID
        /// </summary>
        [DataMember(Name = "package_id")]
        public long PackageId { get; set; }

        /// <summary>
        /// 资源包名称
        /// </summary>
        [DataMember(Name = "package_name")]
        public string PackageName { get; set; }

        /// <summary>
        /// 资源包类型
        /// </summary>
        [DataMember(Name = "package_type")]
        public int PackageType { get; set; }

        /// <summary>
        /// 目录ID
        /// </summary>
        [DataMember(Name = "task_id")]
        public string TaskId { get; set; }

        /// <summary>
        /// 资源模块类型
        /// </summary>
        [DataMember(Name = "resource_type")]
        public int ResourceType { get; set; }

        /// <summary>
        /// 资源版本ID
        /// </summary>
        [DataMember(Name = "version_id")]
        public long VersionId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        [DataMember(Name = "resource_name")]
        public string ResourceName { get; set; }

        /// <summary>
        /// 资源类型ID
        /// </summary>
        [DataMember(Name = "module_id")]
        public string ModuleId { get; set; }

        /// <summary>
        /// 提交人ID
        /// </summary>
        [DataMember(Name = "submit_user_id")]
        public int SubmitUserId { get; set; }

        /// <summary>
        /// 提交人姓名
        /// </summary>
        [DataMember(Name = "submit_user_name")]
        public string SubmitUserName { get; set; }

        /// <summary>
        /// 作业分值
        /// </summary>
        [DataMember(Name = "work_score")]
        public decimal WorkScore { get; set; }

        /// <summary>
        /// 实际得分
        /// </summary>
        [DataMember(Name = "actual_score")]
        public decimal ActualScore { get; set; }

        /// <summary>
        /// 正确率
        /// </summary>
        [DataMember(Name = "correct_rate")]
        public decimal CorrectRate { get; set; }

        /// <summary>
        /// 作业时长
        /// </summary>
        [DataMember(Name = "work_long")]
        public int WorkLong { get; set; }

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
    }
}
