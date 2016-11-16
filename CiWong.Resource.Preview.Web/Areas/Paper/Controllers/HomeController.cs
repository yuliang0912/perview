using System.Web.Mvc;
using System.Linq;
using System;
using CiWong.Resource.Preview.Common;
using CiWong.Tools.Workshop.DataContracts;
using System.Collections.Generic;
using RestSharp;
using AQiYi;
using System.Net;

namespace CiWong.Resource.Preview.Web.Areas.Paper.Controllers
{
    /// <summary>
    /// 试卷模块
    /// </summary>
    public class HomeController : ResourceController
    {
        public override ActionResult Index(ResourceParam baseParam, long versionId)
        {
            return View(baseParam.PackageType == 3 ? "Index" : "EbookIndex", baseParam);
        }

        /// <summary>
        /// 做作业viwe显示(重载)
        /// </summary>
        public override ActionResult DoWork(WorkParam baseParam, long doWorkId, long contentId)
        {
            //此处通过状态判断,如果已经提交作业,则直接用结果视图(WorkResult.cshtml)对数据进行处理
            if (baseParam.workStatus == 2 || baseParam.workStatus == 3)
            {
                return View(baseParam.PackageType == 3 ? "WorkResult" : "EbookWorkResult", baseParam);
            }
            return View(baseParam.PackageType == 3 ? "DoWork" : "EbookDoWork", baseParam);
        }

        [WorkAuthorize]
        public ActionResult Correct(WorkParam baseParam, long doWorkId, long contentId)
        {
            if (baseParam.DoWorkBase.SubmitUserID == baseParam.User.UserID)
            {
                return Redirect(string.Format("/jump/work?doworkId={0}&contentId={1}", doWorkId, contentId));
            }
            else if (baseParam.DoWorkBase.PublishUserId != baseParam.User.UserID)
            {
                return Redirect("/home/Error?message=当前登录用户无操作权限");
            }
            return View(baseParam);
        }


        /// <summary>
        /// 查询试题对应的视频对象
        /// </summary>
        /// <param name="qid">试题ID</param>
        /// <returns></returns>
        public ContentResult GetQuestionVideoModel(string qid)
        {
            var bookResult = new RestClient(100001).ExecuteContent(WebApi.GetQuestionVideo, new { versionIds = qid });

            return Content(bookResult, "application/json");
        }


        /// <summary>
        /// 根据视频版本ID获取视频信息
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public ActionResult videopreview(string versionId)
        {
            var key = string.Concat(string.Concat("_preview_resource_getquestionvideoModel_", string.Join("_", versionId)));

            //获取缓存的数据
            var cacheContent = RedisHelper.GetItem<CiWong.Resource.Preview.DataContracts.ReturnResult<VideoContract>>(key);

            if (cacheContent != null)
            {
                return View(cacheContent);
            }

            var videoEntity = new RestClient(100001).ExecuteGet<CiWong.Resource.Preview.DataContracts.ReturnResult<VideoContract>>(WebApi.GetVideoEntity, new { versionid = versionId });
            //如果有数据写入缓存
            if (videoEntity != null && videoEntity.Data != null)
            {
                RedisHelper.SetItem<CiWong.Resource.Preview.DataContracts.ReturnResult<VideoContract>>(key, videoEntity);
                return View(videoEntity);
            }
            else
            {
                return View(new CiWong.Resource.Preview.DataContracts.ReturnResult<VideoContract>
                {
                    Ret = -1,
                    Data = null,
                    Message = "无效视频"

                });
            }

        }





        private RestSharp.RestClient _iqiyiclient;
        private static String clientid = "ce81cb32b7dc4e19b1cedffdfa150d27";
        private static String clientsecret = "37760a5a878f41dcf1516f147ffb959b";
        private static DateTime overDate = DateTime.Now;
        private static TokenInfo tokeninfo = null;
        public JsonResult GetAiQiYiURL(string voteid, bool repeatget = true)
        {
            VideoInfo videoInfo = null;
            _iqiyiclient = new RestSharp.RestClient("http://openapi.iqiyi.com");
            _iqiyiclient.AddHandler("application/json", new RestSharp.Deserializers.JsonDeserializer());
            if (DateTime.Now > overDate || tokeninfo == null)
            {
                var req = new RestSharp.RestRequest("/api/iqiyi/authorize", RestSharp.Method.GET);
                //接口参数
                req.AddParameter("client_id", clientid);
                req.AddParameter("client_secret", clientsecret);

                try
                {
                    var response = _iqiyiclient.Execute(req);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = response.Content.Replace("\\", "");
                        json = json.Substring(1, json.Length - 2);

                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseInfo<TokenInfo>>(json);
                        if (data != null && data.code == "A00000")
                        {
                            overDate = DateTime.Now.AddSeconds(data.data.expires_in);
                            tokeninfo = data.data;
                        }
                    }
                    else
                    {
                        return Json(videoInfo, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(videoInfo, JsonRequestBehavior.AllowGet);
                }
            }
            if (tokeninfo != null)
            {
                var req = new RestSharp.RestRequest("/api/file/fullStatus", RestSharp.Method.GET);
                //接口参数
                req.AddParameter("access_token", tokeninfo.access_token);
                req.AddParameter("file_id", voteid);
                try
                {
                    var response = _iqiyiclient.Execute(req);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var json = response.Content.Replace("\\", "");
                        json = json.Substring(1, json.Length - 2);
                        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseInfo<VideoInfo>>(json);
                        if (data != null && data.code == "A00000")
                        {
                            data.data.m3u896 = data.data.urllist.m3u8[96].ToString();

                            var mp4Client = new RestSharp.RestClient();
                            mp4Client.AddHandler("application/json", new RestSharp.Deserializers.JsonDeserializer());
                            var reqmp4 = new RestSharp.RestRequest(data.data.urllist.mp4[2], RestSharp.Method.GET);
                            var repsonsemp4url = mp4Client.Execute(reqmp4);
                            try
                            {
                                if (repsonsemp4url.StatusCode == HttpStatusCode.OK)
                                {
                                    json = repsonsemp4url.Content;
                                    var i = json.IndexOf('{');
                                    var f = json.LastIndexOf(";");
                                    json = json.Substring(i, f - i);
                                    var mp4urldata = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseInfo<MP4Info>>(json);
                                    if (mp4urldata.code == "A00000")
                                    {
                                        data.data.mp42 = mp4urldata.data.l;
                                    }
                                    else
                                    {
                                        data.data.mp42 = "";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //_logger.Error("获取MP4地址错误1：" + ex.ToString());
                                throw ex;
                            }

                            videoInfo = data.data;
                        }
                        else if (data != null && data.code == "A00007")
                        {
                            if (repeatget)
                            {
                                var repdata = Newtonsoft.Json.JsonConvert.DeserializeObject<BaseInfo<RepeatVideo>>(json);
                                if (repdata != null && repdata.data != null && repdata.data.fileIdBefore.Length == 32)
                                    return GetAiQiYiURL(repdata.data.fileIdBefore, false);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return Json(videoInfo, JsonRequestBehavior.AllowGet);
        }


    }



}

namespace AQiYi
{
    public class BaseInfo<T>
    {
        public string code { get; set; }
        public T data { get; set; }
        public string msg { get; set; }
    }

    public class TokenInfo
    {
        public long expires_in { get; set; }
        public string access_token { get; set; }
        public string refresh_token { get; set; }
    }
    public class RepeatVideo
    {
        public string is_repeat { get; set; }
        public string fileIdBefore { get; set; }
    }
    public class VideoInfo
    {
        public string thumbnail { get; set; }
        public string swfurl { get; set; }
        public string img { get; set; }
        public string mp42 { get; set; }
        public string m3u896 { get; set; }
        public UrlList urllist { get; set; }
    }

    public class UrlList
    {
        public Dictionary<int, string> mp4 { get; set; }
        public Dictionary<int, string> m3u8 { get; set; }
    }

    public class MP4Info
    {
        public string l { get; set; } //url
        public string t { get; set; } //时间 "CT|GuangDong_ShenZhen-183.37.241.206",
        public string s { get; set; } //mp4 中的下标 "1"
        public string z { get; set; } //地区 "dongguan_ct",
        public string h { get; set; } //"19"
        public string e { get; set; } //"0"    
    }
    public class KeyValue
    {
        public int index { get; set; }
        public string value { get; set; }
    }
}
