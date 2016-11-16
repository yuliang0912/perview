define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko");

    var methods = {
        getSupport: function (articleId) {
            var params = { articleId: articleId };
            ciwong.ajax.getJSON("/Sns/Partial/getSupport", params, function (result) {
                $("#emNum").text(result);
            });
        },
        newsCategory: { 1: "images", 2: "video", 3: "words", 4: "report" },
        setArticle: function (newsArray, articles) {
            var news = {};
            $.each(newsArray, function () {
                $.each(this.list, function (i, item) {
                    item.Article = $.grep(articles, function (n) { return n.version_id == item.version_id; })[0];
                });
            })
            $.each(newsArray, function () {
                news[methods.newsCategory[this.id]] = $.map(this.list, function (n) { return n.Article });
            });
            return news;
        },
        trimContent: function (str, bool_a, bool_b, bool_c) {
            if (!str)
                return "";
            if (bool_a) //去掉html标签
                str = str.replace(/<[^>]*>|/g, "");
            if (bool_b) //去掉标点符号
                str = str.replace(/[\~|\`|\!|\！|\@|\#|\$|\%|\^|\&|\*|\(|\)|\、|\-|\_|\+|\=|\||\\|\[|\]|\{|\}|\;|\；|\:|\：|\"|\“|\'|\‘|\,|\，|\<|\.|\。|\>|\/|\?|\？]/g, "");
            if (bool_c)//去掉多余空格，只保留一个
                str = str.replace(/\s+/g, ' ');
            var reg_en = new RegExp(/([\']?\w+)/g); // new RegExp(/[a-zA-Z]+([\']?[a-zA-Z]+)?/g);
            var count_en = str.match(reg_en) == null ? 0 : str.match(reg_en).length; //英文单词、数字个数
            var count_ch = str.replace(/[^\u4e00-\u9fa5]/gi, "").length; //中文个数
            return count_en + count_ch; //返回总字符数
        }
    };

    var viewModel = {
        preView: function (settingOptions) {
            var model = this,
               settings = settingOptions || {},
			   articleModel, ep;

            model.news = ko.observable();

            model.showArticle = function (data) {
                require(["sns"], function (comment) {
                    if (!articleModel) {
                        articleModel = new viewModel.dialogView({ comment: comment });
                        ep = ciwong.epDialog.init("newsArticleTemplate", "currArticle", function () {
                            window.location.href = "#c";
                        });
                        ko.applyBindings(articleModel, document.getElementById("_dialog"));
                        $("#completedRead").live("click", function () { ep.close(); window.location.href = "#c"; });
                    }
                    articleModel.currArticle(data);
                    ep.open();
                    window.location.hash = "#" + data.version_id;
                    $("body").scrollTop(0);
                    $("#timer").timer().start();
                });
            }

            model.afterRender = function () {
                var openNewsVersionId = window.location.hash.replace("#", "");
                if (openNewsVersionId.length > 0) {
                    if (!isNaN(openNewsVersionId)) {
                        $("#" + openNewsVersionId).click();
                    }
                }
            }

            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "1daf88c8-cde8-4d81-94be-d42bc30f52ed" }, function (data) {
                var versions = [];
                $.each(data.parts, function (i, item) {
                    $.merge(versions, $.map(item.list, function (n) { return n.version_id }));
                });
                ciwong.ajax.getJSON("/resource/getlist", { versionIds: versions.join(","), moduleId: "05a3bf23-b65b-4d7f-956c-5db2b76b9c11" }, function (articles) {
                    model.news(methods.setArticle(data.parts, articles));
                }).done(function () {
                    ciwong.colseLoading();
                });
            });
        },
        dialogView: function (settingOptions) {
            var model = this,
				settings = settingOptions || {};

            var comment = settings.comment;

            model.currArticle = ko.observable();

            model.currArticle.subscribe(function (article) {
                methods.getSupport(article.version_id); //赞
                if (comment) {
                    article.shareModel = new comment.viewModel.shareModel({ title: article.name });
                    article.commentModel = new comment.viewModel.preView({ id: article.version_id });
                    article.commentModel.getCommentList();
                }
            });
        }
    };

    exports.viewModel = viewModel;
    exports.moduleId = "1daf88c8-cde8-4d81-94be-d42bc30f52ed";

    /*
	以下为当前viewModel所用的模板以及部分函数
	*/
    ciwong.koTemplateEngine.add("newsArticleTemplate", '\
        <div class="sidetools tc">\
			<div id="like" class="likeBtn" data-bind="template: { name: \'SupportTemplate\',data: version_id }">\
			</div>\
			<a class="mt20" href="#dvcoment"><i class="ico-msg"></i>\
				<p><em id="emCommentNum">-</em></p>\
			</a>\
			<div id="share" class="share mt20" data-bind="template: { name: \'ShareTemplate\',data:shareModel }">\
			</div>\
		</div>\
		<div class="p20">\
			<h4 class="newsDlgT tc" data-bind="text:name"></h4>\
			<div class="newsCont mt20">\
            <!--ko if: parts.length > 0 && parts[0].list.length > 0  -->\
            <div class="tc">\
                <img data-bind="attr:{ \'src\':  parts[0].list[0].url }" /></div>\
            <!--/ko-->\
            <p data-bind="html:content"></p>\
        </div>\
		<div class="timerBox cf yh"><span class="fl time"><h4>阅读计时</h4><p><em id=\"timer\">00:00:00</em></p></span>\
			<span class="fr btn"><a id="completedRead">我读完了</a></span>\
		</div>\
		<div id="dvcoment" data-bind="template: { name: \'newsComment\', data:commentModel }"></div>');
});