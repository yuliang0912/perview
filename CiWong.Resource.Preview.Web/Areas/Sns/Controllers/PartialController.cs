using CiWong.Resource.Preview.Common;
using CiWong.Resource.Preview.DataContracts;
using CiWong.Resource.Preview.DataContracts.Resource;
using CiWong.Resource.Preview.Service;
using CiWong.Security;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CiWong.Resource.Preview.Web.Areas.Sns.Controllers
{
    public class PartialController : CustomerController
    {
        private SupportService supportService;
        public PartialController(SupportService _supportService)
        {
            this.supportService = _supportService;
        }

        public ActionResult Index()
        {
            return View();
        }


        public PartialViewResult Comment(int templateType = 1)
        {
            ViewBag.templateType = 1;
            return PartialView();
        }

		[LoginAuthorize(false)]
		public JsonResult GetComentList(User user, long subID = 2050, int page = 1, int pageSize = 10)
		{
			if (user == null || user.UserID == 0)
			{
				user = new User() { UserID = 10000, UserName = string.Empty };
			}
			int pageindex = page;
			var result = new RestClient(user.UserID).ExecuteGet<ReturnResult<CiWong.Resource.Preview.DataContracts.Resource.CwComent>>(WebApi.GetCommentList, new
			{
				subject_id = subID,
				type = 1,
				son_category = 0,
				is_select_reply = 1,
				page_size = pageSize,
				page = page
			});
			if (result.Data.record_count > 0)
			{
				var data = new CwComent()
				{
					page_index = result.Data.page_index,
					page_size = result.Data.page_size,
					record_count = result.Data.record_count,
					msg = "success",
					record_list = result.Data.record_list.Select(t => new CwCommentDTO()
					{
						comment_id = t.comment_id,
						com_subject_id = t.com_subject_id,
						com_content = t.com_content,
						com_user_id = t.com_user_id,
						com_user_name = t.com_user_name,
						com_userphoto = PhotoHelper.GetUserPhoto(t.com_user_id, 50),
						comment_type = t.comment_type,
						comment_type_name = t.comment_type_name,
						son_category = t.son_category,
						comment_date = t.comment_date,
						com_status = (int)t.com_status,
						replys = t.replys.OrderByDescending(p => p.reply_date).Select(m => new CwReplyDTO()
						{
							comment_id = m.comment_id,
							reply_id = m.reply_id,
							reply_user_id = m.reply_user_id,
							reply_user_name = m.reply_user_name,
							reply_content = m.reply_content,
							parend_id = m.parend_id,
							reply_date = m.reply_date,
							reply_userphoto = PhotoHelper.GetUserPhoto(m.reply_user_id, 50)
						})

					}).ToList()

				};
				return Json(new ReturnResult<CwComent>(data));
			}
			return Json(result);
		}

		[HttpPost, LoginAuthorize]
		public JsonResult PublishReComent(User user, string cont, long comid)
		{
			var result = new RestClient(user.UserID).ExecutePost<ReturnResult<long>>(WebApi.PublishReComent, new
			{
				comment_id = comid,
				content = cont
			});
			return Json(new ReturnResult<int>(result.Code));
		}

		[LoginAuthorize, ValidateInput(false)]
		public JsonResult PublishComent(User user, long subid, int soncategory, int type, string cont)
		{
			var result = new RestClient(user.UserID).ExecutePost<ReturnResult<long>>(WebApi.PublishComent, new
			{
				type = type,
				son_category = soncategory,
				subject_uid = user.UserID,
				subject_id = subid,
				content = cont
			});
			return Json(new ReturnResult<long>(result.Data));
		}

        public PartialViewResult Share()
        {
            return PartialView();
        }

        public PartialViewResult Support(long articleId)
        {
            int tip = 0;
            int low = 0;
            var returnResult = supportService.GetModelByResourceId(articleId);
            if (returnResult.Code == 0 && returnResult.Data != null)
            {
                tip = returnResult.Data.SupportNum;
                low = returnResult.Data.OpposeNum;
            }
            ViewBag.tip = tip;
            ViewBag.low = low;
            return PartialView();
        }

        public JsonResult getSupport(long articleId)
        {
            int tip = 0;
            var returnResult = supportService.GetModelByResourceId(articleId);
            if (returnResult.Code == 0 && returnResult.Data != null)
            {
                tip = returnResult.Data.SupportNum;
            }
            return Json(tip);
        }

		[LoginAuthorize,ValidateInput(false)]
        public JsonResult setSupport(User user, int status, long articleId)
        {
            int result = 0;
            var isSupport = supportService.IsSupport(user.UserID, articleId);
            if (isSupport)
            {
                result = 1;
            }
            else
            {
                var returnResult = supportService.GetModelByResourceId(articleId);
                if (returnResult.Code == 0 && returnResult.Data != null)
                {
                    if (status == 0)
                    {
                        returnResult.Data.SupportNum = returnResult.Data.SupportNum + 1;
                    }
                    else
                    {
                        returnResult.Data.OpposeNum = returnResult.Data.OpposeNum + 1;
                    }
                    supportService.Update(returnResult.Data);
                }
                else
                {
                    SupportContract support = new SupportContract();
                    support.OpposeNum = 0;
                    support.ReadNum = 0;
                    support.SupportNum = 0;
                    if (status == 0)
                    {
                        support.SupportNum = 1;
                    }
                    else
                    {
                        support.OpposeNum = 1;
                    }
                    support.ResourceId = articleId;
                    supportService.Insert(support);

                }
                SupportRecordContract record = new SupportRecordContract();
                record.Status = status;
                record.ResourceId = articleId;
                record.UserId = user.UserID;
                record.UserName = user.UserName;
                record.CreatDate = DateTime.Now;
                supportService.InsertRecord(record);//添加记录
            }
            return Json(new ReturnResult<int>(result));
        }

    }
}
