using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CiWong.Resource.Preview.DataContracts
{
    [Serializable, DataContract(Name = "work_base")]
    public class WorkBaseContract
    {
        /// <summary>
        /// 作业ID
        /// </summary>
        public long WorkId { get; set; }

        /// <summary>
        /// 作业名称
        /// </summary>
        public string WorkName { get; set; }

        /// <summary>
        /// 作业类型
        /// </summary>
        public int WorkType { get; set; }

        /// <summary>
        /// 作业子应用
        /// </summary>
        public int SonWorkType { get; set; }

        /// <summary>
        /// 布置人ID
        /// </summary>
        public int PublishUserId { get; set; }

        /// <summary>
        /// 布置人姓名
        /// </summary>
        public string PublishUserName { get; set; }

        /// <summary>
        /// 布置时间
        /// </summary>
        public DateTime PublishDate { get; set; }

        /// <summary>
        /// 发送时间(实指学生收到作业时间)
        /// </summary>
        public DateTime SendDate { get; set; }

        /// <summary>
        /// 有效时间(实指学生作业过期时间) 注:当学生作业过期之后,该份作业只能当自主测试,不会再被提交到老师.
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public int WorkStatus { get; set; }

        /// <summary>
        /// 本次作业人数
        /// </summary>
        public int TotalNum { get; set; }

        /// <summary>
        /// 已完成作业人数
        /// </summary>
        public int CompletedNum { get; set; }

        /// <summary>
        /// 参考时长(分钟)/朗读次数/朗读分数 根据CompletedType决定
        /// </summary>
        public int ReferLong { get; set; }

        /// <summary>
        /// 分数(默认为100分)
        /// </summary>
        public decimal WorkScore { get; set; }

        /// <summary>
        /// 布置对象ID
        /// </summary>
        public long ReviceUserId { get; set; }

        /// <summary>
        /// 布置对象名称
        /// </summary>
        public string ReviceUserName { get; set; }

        /// <summary>
        /// 已批改人数
        /// </summary>
        public int MarkNum { get; set; }

        /// <summary>
        /// 批改方式（1：手动批改 2：自动批改 ）
        /// </summary>
        public int MarkType { get; set; }

        /// <summary>
        /// 试卷ID
        /// </summary>
        public long ExaminationId { get; set; }

        /// <summary>
        /// 作业描述
        /// </summary>
        public string WorkDesc { get; set; }

        /// <summary>
        /// 记录ID
        /// </summary>
        public long RecordId { get; set; }

        /// <summary>
        /// 跳转参数
        /// </summary>
        public string RedirectParm { get; set; }
    }
}
