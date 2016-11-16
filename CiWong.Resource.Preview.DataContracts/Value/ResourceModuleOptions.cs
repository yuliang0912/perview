using System;
using System.Collections.Generic;

namespace CiWong.Resource.Preview.DataContracts
{
    /// <summary>
    /// 资源类型定义
    /// </summary>
    public static class ResourceModuleOptions
    {
        /// <summary>
        /// 电子报模板指定排序集合
        /// </summary>
		public static readonly List<int> newsPaperModuleSortArray = new List<int> { 7, 10, 15, 18, 9, 5, 8 };

		/// <summary>
		/// 试题
		/// </summary>
		public static readonly Guid Question = new Guid("008a020d-72c6-4df5-ba6c-73086b8db022");

		/// <summary> 
		/// 教学设计
		/// </summary>
		public readonly static Guid TeachingPlan = new Guid("a5df23d4-c0fe-4629-b8d3-35288695136b");

		/// <summary>
		/// 导学案
		/// </summary>
		public readonly static Guid Guidance = new Guid("09aca375-7634-42a8-a39e-098fee65c341");

		/// <summary>
		/// 知识点精讲
		/// </summary>
		public readonly static Guid Knowledge = new Guid("c4fab15f-a0a6-45b5-b116-b0f087cb7119");

		/// <summary>
		/// 课件素材
		/// </summary>
		public static readonly Guid Courseware = new Guid("40cafe50-68a6-11e4-a4b4-782bcb066f05");
		
		/// <summary>
		/// 试卷
		/// </summary>
		public static readonly Guid ExaminationPaper = new Guid("1f693f76-02f5-4a40-861d-a8503df5183f");

		/// <summary>
		/// 考点
		/// </summary>
		public static readonly Guid TestPoint = new Guid("14f56239-17ca-453d-b7d3-35718f4b0d35");

		/// <summary>
		/// 导图课程
		/// </summary>
		public static readonly Guid CourseMap = new Guid("a17f1a5a-72e8-4f28-90ce-4811277d50e0");

		/// <summary>
		/// 考点导图
		/// </summary>
		public static readonly Guid TreeView = new Guid("7fad0f70-f74e-42e4-a114-5a05261c624b");

		/// <summary>
		/// 考点课程
		/// </summary>
		public static readonly Guid TestCourse = new Guid("a9cad590-ac3e-4a27-92ee-a2520c0f031b");

		/// <summary>
		/// 文章
		/// </summary>
		public static readonly Guid Article = new Guid("05a3bf23-b65b-4d7f-956c-5db2b76b9c11");

		/// <summary>
		/// 新闻
		/// </summary>
		public static readonly Guid News = new Guid("1daf88c8-cde8-4d81-94be-d42bc30f52ed");

		/// <summary>
		/// 同步阅读
		/// </summary>
		public static readonly Guid SynchronousRead = new Guid("c4a8676b-070d-4578-a5cf-8baf546a0ebd");

		/// <summary>
		/// 同步训练
		/// </summary>
		public static readonly Guid SyncTrain = new Guid("2bea5300-972e-494f-b730-6cbc05f0a717");

		/// <summary>
		/// 同步训练专项
		/// </summary>
		public static readonly Guid SyncTrainSpecial = new Guid("b7ff32e4-cc3f-4a5c-be27-50231095dc50");

		/// <summary>
		/// 同步训练课时
		/// </summary>
		public static readonly Guid SyncTrainClassHour = new Guid("4f00fec0-b5c7-496f-86f1-4f5187667130");

		/// <summary>
		/// 同步跟读
		/// </summary>
		public static readonly Guid SyncFollowRead = new Guid("f0833ebe-6a8b-4cc1-a6b5-f4d47d93df35");

		/// <summary>
		/// 同步跟读课文
		/// </summary>
		public static readonly Guid SyncFollowReadText = new Guid("992a5055-e9d0-453f-ab40-666b4d7030bb");

		/// <summary>
		/// 单词
		/// </summary>
		public static readonly Guid Word = new Guid("a7527f97-14e6-44ef-bf73-3039033f128e");

		/// <summary>
		/// 短语
		/// </summary>
		public static readonly Guid Phrase = new Guid("6ed8a021-8cd1-45b1-8ec1-a6369dfb19ae");

		/// <summary>
		/// 书签
		/// </summary>
		public static readonly Guid BookMarker = new Guid("6c48221d-feaa-11e3-a3e9-782bcb066f05");


		/// <summary>
		/// 视频
		/// </summary>
		public static readonly Guid Video = new Guid("2a7e4182-337c-f83f-de99-53911f02650f");

		/// <summary>
		/// 问题里程碑
		/// </summary>
		public static readonly Guid ProblemMileage = new Guid("bac9bd9a-1265-11e4-a277-782bcb066f05");

		/// <summary>
		///听说训练模板
		/// </summary>
		public static readonly Guid ListeningAndSpeakingTemplate = new Guid("3f514793-4007-4e3e-82df-85f98438daf7");

		/// <summary>
		/// 模考
		/// </summary>
		public static readonly Guid ListeningAndSpeaking = new Guid("fcfd6131-cdb6-4eb8-9cb9-031f710a8f15");

		/// <summary>
		/// 听说模考试卷(转换为标准试卷结构)
		/// </summary>
		public static readonly Guid ListeningAndSpeakingExam = new Guid("e9430760-9f2e-4256-af76-3bd8980a9de4");

		/// <summary>
		/// 汉字
		/// </summary>
		public static readonly Guid ChineseWord = new Guid("10e41c8d-228a-11e4-973b-782bcb066f05");

		/// <summary>
		/// 词
		/// </summary>
		public static readonly Guid chineseGroupWord = new Guid("afd5823e-ded0-455b-aaf5-bd06cff78225");

		/// <summary>
		/// 字体演变
		/// </summary>
		public static readonly Guid ChineseFont = new Guid("d6cd0306-2298-11e4-973b-782bcb066f05");

		/// <summary>
		/// 作业快传附件
		/// </summary>
		public static readonly Guid WorkFile = new Guid("9207e432-20d4-4315-a6d5-367cc402ef2e");
    }
}
