using CiWong.Resource.Preview.DataContracts.Business;
using System;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    [DataContract(Name = "Support")]
    public class SupportContract
    {
        [DataMember(Name = "Id")]
        public long Id { get; set; }

        [DataMember(Name = "ResourseId")]
        public long ResourceId { get; set; }

        [DataMember(Name = "SupportNum")]
        public int SupportNum { get; set; }

        [DataMember(Name = "OpposeNum")]
        public int OpposeNum { get; set; }

        [DataMember(Name = "ReadNum")]
        public int ReadNum { get; set; }

    }

    [DataContract(Name = "SupportRecord")]
    public class SupportRecordContract
    {
        [DataMember(Name = "Id")]
        public long Id { get; set; }

        [DataMember(Name = "UserId")]
        public long UserId { get; set; }

        [DataMember(Name = "UserName")]
        public string UserName { get; set; }

        [DataMember(Name = "CreatDate")]
        public DateTime CreatDate { get; set; }

        [DataMember(Name = "Status")]
        public int Status { get; set; }

        [DataMember(Name = "ResourceId")]
        public long ResourceId { get; set; }
    }

    /// <summary>
    /// 跟读作业内容要求序列化实体
    /// 字段请设置为允许null值,方便序列化时忽略此字段.
    /// 控制序列化之后的字符串长度在300以内
    /// </summary>
    [DataContract(Name = "requirement_content")]
	public class RequirementContentContract<T> : IRequirement
    {
        /// <summary>
        /// 跟读次数
        /// </summary>
        [DataMember(Name = "readtimes")]
        public int? ReadTimes { get; set; }

        /// <summary>
        /// 达标分数
        /// </summary>
        [DataMember(Name = "passscorce")]
        public int? PassScorce { get; set; }

        /// <summary>
		/// 跟读类型 1逐句跟读 2独立通读 3选段背诵
        /// </summary>
        [DataMember(Name = "speekingtype")]
        public int? SpeekingType { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        [DataMember(Name = "attchment")]
        public T Attchment { get; set; }
    }
}
