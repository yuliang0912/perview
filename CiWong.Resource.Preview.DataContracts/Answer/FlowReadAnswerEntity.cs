using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 模拟及听力考试答案单体 暂是不用 看需求定义一下
    /// </summary>
    [DataContract(Name = "hearing_answer")]
    public class FlowReadAnswerEntity : IAnswer
    {
        /// <summary>
        /// 答案位置下标
        /// </summary>
        public int Sid { get; set; }

        /// <summary>
        /// 原文
        /// </summary>
        [DataMember(Name = "word")]
        public string Word { get; set; }

        /// <summary>
        /// 选择题或者填空题答案（A，或者TXT）
        /// </summary>
        [DataMember(Name = "answer")]
        public string Answer { get; set; }

        /// <summary>
        /// 录音文件URL
        /// </summary>
        [DataMember(Name = "audio_uul")]
        public string AudioUrl { get; set; }
        /// <summary>
        /// 跟读次数
        /// </summary>
        [DataMember(Name = "read_times")]
        public int ReadTimes { get; set; }
    }
}
