namespace CiWong.Resource.Preview.Web
{
    public static class WebApi
    {
        /// <summary>
        /// 获取评论列表
        /// </summary>
        public const string GetCommentList = "/Common/Comment/list_comment";

        /// <summary>
        /// 回复评论
        /// </summary>
        public const string PublishReComent = "/Common/Comment/publish_reply";

        /// <summary>
        /// 发表评论
        /// </summary>
        public const string PublishComent = "/Common/Comment/publish_comment";

        /// <summary>
        /// 获取资源库资源
        /// </summary>
        public const string GetResourceByIds = "/resource/getByIds";

        /// <summary>
        /// 获取版本信息
        /// </summary>
        public const string GetBookVersion = "/wiki/books/GetById";

        /// <summary>
        /// 获取书柜电子书使用权限
        /// </summary>
        public const string PackagePermission = "/bookCase/home/getUserPackagePermission";

        /// <summary>
        /// 学生是否开通校本
        /// </summary>
        public const string IsProxyProduct = "/expandwork/Agent/Is_Proxy";

        /// <summary>
        /// 发送书房消息
        /// </summary>
        public const string SendMessage = "/common/Message/send_message";

        /// <summary>
        /// 获取用户家长信息
        /// </summary>
        public const string GetParents = "/family/List_MyParent";

        /// <summary>
        ///验证用户对于当前书的权限
        /// </summary>
        public const string UserIsCan = "/bookcase/home/IsCan";


        /// <summary>
        /// 获取试题中的视频ID
        /// </summary>
        public const string GetQuestionVideo = "/tools/resource/questionvideo";

        /// <summary>
        /// 获取视频详细
        /// </summary>
        public const string GetVideoEntity = "/tools/resource/getvideo";


    }
}