using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CiWong.Resource.Preview.DataContracts
{
    #region 提交答案实体
    /// <summary>
    /// PC 端答案提交实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable, DataContract(Name = "speeking_answers_entity")]
    public class SpeekingAnswersEntity<T>
    {
        /// <summary>
        /// 作业类型
        /// </summary>
        [DataMember(Name = "work_type")]
        public int Work_Type { get; set; }

        /// <summary>
        /// 练习还是作业 0 练习 1 作业
        /// </summary>
        [DataMember(Name = "is_work")]
        public int Is_Work { get; set; }
        /// <summary>
        /// 练习对像列表（作业模式不需要）
        /// </summary>
        [DataMember(Name = "practice_info")]
        public IndepPracticeContract practiceInfo { get; set; }
        /// <summary>
        /// 作业提交记录（练习模式不需要）
        /// </summary>
        [DataMember(Name = "works_info")]
        public UnitWorksContract worksInfo { get; set; }
        /// <summary>
        /// 提交答案内容实体 包括单词、句子、听力、考试
        /// </summary>
        [DataMember(Name = "answer_data")]
        public List<T> AnswerData { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable, DataContract(Name = "speeking_submit_ids")]
    public class SubmitWorkIds
    {
        /// <summary>
        /// 作业ID
        /// </summary>
        [DataMember(Name = "dowork_id")]
        public long DoWorkId { get; set; }
        /// <summary>
        /// 作业资源包ID 
        /// </summary>
        [DataMember(Name = "content_id")]
        public long ContentId { get; set; }
        /// <summary>
        /// 布置记录ID
        /// </summary>
        [DataMember(Name = "publish_id")]
        public long PublishId { get; set; }

    }
    /// <summary>
    /// 作业类型
    /// </summary>
    public enum SpeekingWorkType
    {
        FolwRead = 3,//同步跟读
        SimulationExam = 1,//模拟考试
        ListenExam = 2 //听力考试
    }
    /// <summary>
    /// 作业内容类型
    /// </summary>
    public enum SpeekingWorkRescoreType
    {

        SimulationExam = 1,//模拟考试
        ListenExam = 2, //听力考试
        FolwReadWord = 3,//同步跟读单词
        FolwReadSentence = 4//同步跟读句子
    }

    /// <summary>
    /// 接口构建实体
    /// </summary>
    [Serializable, DataContract(Name = "speeking_answers")]
    public class SpeekingAnswers
    {
        /// <summary>
        /// 提交作业答案明细
        /// </summary>
        [DataMember(Name = "list_answers")]
        public List<WorkAnswerContract> ListAnswers { get; set; }
    }
    #endregion

    #region PC端资源获取对像
    /// <summary>
    /// 跟读作业内容信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReadWorkContent<T>
    {
        /// <summary>
        /// 规则
        /// </summary>
        public Rule Rules { get; set; }
        /// <summary>
        /// 类型（单词列表，句子列表）
        /// </summary>
        public T Content { get; set; }
    }



    /// <summary>
    /// 跟读作业规则，后续可按业务增加
    /// </summary>
    public class Rule
    {
        /// <summary>
        /// 根读次数
        /// </summary>
        public int ReadTimes { get; set; }
        /// <summary>
        /// 达标分数
        /// </summary>
        public decimal PassScores { get; set; }
    }

    [Serializable, DataContract(Name = "work_contents")]
    public class WorkConents<T>
    {
        /// <summary>
        /// 具体作业实现
        /// </summary>
        [DataMember(Name = "work_info")]
        public WorkResourceContract WorkInfo { get; set; }
        /// <summary>
        /// 发布记录
        /// </summary>
        [DataMember(Name = "work_publish_info")]
        public PublishRecordContract WorkPublishInfo { get; set; }
        /// <summary>
        /// 类型（单词ID）
        /// </summary>
        [DataMember(Name = "work_content")]
        public T Content { get; set; }
    }


    /// <summary>
    /// 练习资源包
    /// </summary>
    /// <typeparam name="T1"></typeparam>
    /// <typeparam name="T2"></typeparam>
    [Serializable, DataContract(Name = "res_contents")]
    public class ResConents<T1, T2>
    {
        [DataMember(Name = "res_list")]
        public T1 ResList { get; set; }
        [DataMember(Name = "res_entity")]
        public T2 ResEntity { get; set; }
    }
    #endregion
}
