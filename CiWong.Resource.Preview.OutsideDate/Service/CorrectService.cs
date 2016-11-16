using CiWong.Examination.API;
using CiWong.Resource.Preview.Common;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Work.Correct;
using System.Collections.Generic;
using System.Linq;

namespace CiWong.Resource.Preview.OutsideDate.Service
{
    public class CorrectService
    {
        #region IOC模块
        private IQuesService quesService;
        public CorrectService(IQuesService _quesService)
        {
            this.quesService = _quesService;
        }
        #endregion

        /// <summary>
        /// 批改试题
        /// </summary>
		public List<WorkAnswerContract> WikiQuesCorrect(List<WorkAnswerContract<QuestionAnswer>> userAnswer, ref decimal totalScore, ref List<Ques.Core.Models.Question> wikiQues)
		{
			var _list = new List<WorkAnswerContract>();

			if (null == userAnswer || !userAnswer.Any())
			{
				return _list;
			}

			wikiQues = quesService.GetQuestionByVersions(userAnswer.Select(t => t.VersionId).ToList());

			var corrResult = CorrectContext.Instance.Correct(userAnswer, wikiQues, ref totalScore);

			corrResult.ForEach(t =>
			{
				t.AnswerContent = JSONHelper.Encode(t.Answers);
				_list.Add(t);
			});

			return _list;
		}
    }
}
