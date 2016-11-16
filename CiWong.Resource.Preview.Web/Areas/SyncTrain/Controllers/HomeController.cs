using CiWong.Resource.Preview.Common;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.OutsideDate.Service;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.SyncTrain.Controllers
{
	public class HomeController : ResourceController
	{
		private CorrectService correctService;
		public HomeController(CorrectService _correctService)
		{
			this.correctService = _correctService;
		}

		[HttpPost, ValidateInput(false)]
		public JsonResult Correct()
		{
			var answerContent = Request.Form["content"];

			List<WorkAnswerContract<QuestionAnswer>> userAnswer = null;
			try
			{
				userAnswer = JSONHelper.Decode<List<WorkAnswerContract<QuestionAnswer>>>(answerContent);
			}
			catch (Exception e)
			{
				return Json(new CiWong.Resource.Preview.DataContracts.ReturnResult(1, "数据格式错误,exception:" + e.ToString()));
			}

			decimal totalScore = 0;
			var wikiQues = new List<Ques.Core.Models.Question>();

			var result = correctService.WikiQuesCorrect(userAnswer, ref totalScore, ref wikiQues);

			return Json(new CiWong.Resource.Preview.DataContracts.ReturnResult<List<WorkAnswerContract>>(result));
		}
	}
}
