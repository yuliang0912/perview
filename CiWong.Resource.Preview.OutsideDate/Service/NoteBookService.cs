//using CiWong.Framework.Helper;
//using CiWong.NoteBook.Mapping;
//using CiWong.Resource.Preview.DataContracts;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace CiWong.Resource.Preview.OutsideDate.Service
//{
//	public class NoteBookService
//	{
//		public static void AddQuesToNoteBook(int userId, List<Ques.Core.Models.Question> wikiQues, List<WorkAnswerContract> userAnswers)
//		{
//			var mistakeQuestList = new List<MistakeQuest>();
//			foreach (var item in userAnswers)
//			{
//				var question = wikiQues.FirstOrDefault(t => t.Version == item.VersionId);
//				if (item.Assess == 1 || null == question || !question.IsObjective || question.ParentOfVersion > 0)
//				{
//					continue;
//				}
//				var mistakeQuest = new MistakeQuest()
//				{
//					Userid = userId,
//					CreateDate = DateTime.Now,
//					ModifyDate = DateTime.Now,
//					QuestId = question.Version,
//					PointIDs = string.Join("^", question.Point.Select(t => t.Code)),
//					PointNames = string.Join("^", question.Point.Select(t => t.Name)),
//					SubjectId = (int)question.CurriculumID,
//					QuestType = (int)question.qType
//				};
//				mistakeQuest.AnswerHistory = new MistakeHistory()
//				{
//					DoWorkID = 0,
//					CreateDate = mistakeQuest.CreateDate,
//					IsRight = item.Assess == 1,
//					QuestID = mistakeQuest.QuestId,
//					UserID = mistakeQuest.Userid,
//					//WorkTitle = work.WorkName,
//					QuestType = (short)mistakeQuest.QuestType,
//					//QuestAnswer = ques.WorkQuestionAnswer
//				};
//			}
//		}
//	}
//}
