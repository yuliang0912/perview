/*
 comment:习网分享及评论插件
 developtime:2014-07-11
 developer:chenqinghua
*/

define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko"), pagination = require("./jquery.pagination"), dialog = require("dialog");

    var methods = {
        ChangeDateFormat: function (cellval, f) {
            cellval = cellval + "";
            try {
                var date = new Date(parseInt(cellval.replace("/Date(", "").replace(")/", ""), 10) * 1000);
                var dd = date.getFullYear() + "-" + (date.getMonth() + 1) + "-" + date.getDate();
                var currDate = new Date();
                var compareDate = currDate.getFullYear() + "-" + (currDate.getMonth() + 1) + "-" + currDate.getDate();
                var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
                var currentDate = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
                var hours = "" + date.getHours();
                if (hours.length != 2) {
                    hours = "0" + hours;
                }
                var minutes = "" + date.getMinutes();
                if (minutes.length != 2) {
                    minutes = "0" + minutes;
                }
                var seconds = "" + date.getSeconds();
                if (seconds.length != 2) {
                    seconds = "0" + seconds;
                }
                var d = methods.getDays(compareDate, dd);
                if (d == 0) {
                    return "今天 " + hours + ":" + minutes;
                }
                if (d == 1) {
                    return "昨天 " + hours + ":" + minutes;;
                }
                if (d == 2) {
                    return "前天 " + hours + ":" + minutes;;
                }
                if (f) {
                    return date.getFullYear() + "-" + month + "-" + currentDate + " " + hours + ":" + minutes + ":" + seconds;
                }
                else {
                    return date.getFullYear() + "-" + month + "-" + currentDate
                }
            } catch (e) {
                return "";
            }
        },
        IsIE: function () {
            if (!!window.ActiveXObject || "ActiveXObject" in window)
                return true;
            else
                return false;
        },
        getDays: function (strDateStart, strDateEnd) {
            var strSeparator = "-"; //日期分隔符
            var oDate1;
            var oDate2;
            var iDays;
            oDate1 = strDateStart.split(strSeparator);
            oDate2 = strDateEnd.split(strSeparator);
            var strDateS = new Date(oDate1[0], oDate1[1] - 1, oDate1[2]);
            var strDateE = new Date(oDate2[0], oDate2[1] - 1, oDate2[2]);
            iDays = parseInt(Math.abs(strDateS - strDateE) / 1000 / 60 / 60 / 24)//把相差的毫秒数转换为天数
            return iDays;
        }
    };

    var comentModel = {
        preView: function (settingOptions) {//20537       
            var _self = this,
            settings = $.extend(true, settingOptions || {});

            _self.comentCount = ko.observable(0);//评论总数
            _self.ComentList = ko.observableArray();//评论列表    
            _self.curRepcont = ko.observable("");
            _self.pageIndex = ko.observable(0);
            _self.pageSize = 10;

            _self.getCommentList = function () {
                ko.computed(function () {
                    _self.AjaxDataList();
                });
            };

            _self.AjaxDataList = function () {
                if (!settings.id) {
                    return;
                }
                var params = {
                    subID: settings.id,
                    page: (_self.pageIndex() + 1),
                    pageSize: _self.pageSize
                };
                ciwong.ajax.getJSON("/Sns/Partial/GetComentList", params, function (result) {
                    if (result.msg == "success") {
                        _self.comentCount(result.record_count);
                        _self.ComentList(result.record_list);
                        $("#emCommentNum").text(result.record_count);
                        _self.pageHtml(result.record_count, result.page_index);
                    }
                    else {
                        $("#emCommentNum").text(0);
                    }
                });
            };

            _self.ComentList.subscribe(function (data) {
                data && $.each(data, function (i, e) {
                    e.commentdate = methods.ChangeDateFormat(e.comment_date, true);
                    e.subreplys = ko.observableArray();
                    e.subreplys(e.replys);
                    e.oheight = ko.observable(0);
                    var count = 0;
                    var n = 0;
                    var h = 0;
                    $.each(e.replys, function (i, e) {
                        e.replydate = methods.ChangeDateFormat(e.reply_date, true);
                        if (i < 8) {
                            n = 54;
                            if (e.reply_content.length > 80) {
                                n = 76;
                            }
                            if (e.reply_content.length > 122) {
                                n = 98;
                            }
                            h += n;
                        }
                    });
                    e.oheight(h);
                });
            });

            _self.pageHtml = function (totalRecord, pageIndex) {
                $('#pagination').pagination(totalRecord, {
                    items_per_page: _self.pageSize,
                    num_edge_entries: 1,
                    current_page: (pageIndex - 1),
                    prev_text: '上一页',
                    next_text: '下一页',
                    callback: _self.pageIndex
                });
            };

            //回复
            _self.SaveReplay = function (data, event) {
                var txt = $.trim($("#reply_" + data.comment_id).val());
                if (txt != "") {
                    if (txt.length > 140) {
                        $.alert("不能超过140个字符！");
                    }
                    else {
                        var params = { cont: escape(txt), comid: data.comment_id };
                        ciwong.ajax.postJSON("/Sns/Partial/PublishReComent", params, function (result) {
                            if (parseInt(result)== 0) {
                                _self.AjaxDataList();
                            }
                            else {
                                $.error("抱歉，提交回复失败!");
                            }
                        });
                    }
                }
                else {
                    $.alert("回复内容不能为空！");
                }
            };

            _self.getMore = function (data, event) {
                var comId = data.comment_id;
                var liCount = "#ul" + comId + " li";
                var ul = "#ul" + comId;
                var p = $(ul).attr("p");
                p = parseInt(p) + 1;
                $(ul).attr("p", p);
                var totalHeight = 0;
                var count = $(liCount).length;
                var end = p * 8;
                var start = end - 9;
                var hg = 0;
                var lishow = "#ul" + comId + " li:lt(" + end + "):gt(" + start + ")";
                var showCount = $(lishow).length;
                $.each($(liCount), function () {
                    var h = $(this).innerHeight();
                    totalHeight += h;
                });
                $.each($(lishow), function () {
                    var h = $(this).innerHeight();
                    hg += h;
                });
                var currentHeight = $("#ul" + comId).height();
                var showHeight = totalHeight - currentHeight;
                if (showHeight > hg) {
                    $("#ul" + comId).height(currentHeight + hg);
                }
                else {
                    var ht = currentHeight + showHeight + 25;
                    $("#ul" + comId).height(ht);
                }
                if (showCount >= 8) {
                    $("#aRep_" + comId).show();
                }
                else {
                    $("#aRep_" + comId).hide();
                }
            };

            //评论
            _self.SaveComent = function () {
                var cont = $.trim($("#txtcont").val());
                var subID = settings.id;
                if (cont.length > 0 && cont != "说点什么吧......") {
                    if (cont.length > 140) {
                        $.alert("不能超过140个字符！");
                    }
                    else {
                        var params = {
                            subid: subID,
                            type: 1,
                            soncategory: 0,
                            cont: escape(cont)
                        };
                        ciwong.ajax.getJSON("/Sns/Partial/PublishComent", params, function (result) {
                            if (parseInt(result) > 0) {
                                $("#txtcont").val("说点什么吧......");
                                CompareWordCount('#txtcont', '#spantip');
                                _self.pageIndex(0);
                                _self.AjaxDataList();
                            }
                            else {
                                $.alert("抱歉，提交评论失败!");
                            }
                        });
                    }
                }
                else {
                    $.alert("请输入评论内容！");
                }
            };
        },
        shareModel: function (settingOptions) {
            var self = this,
                settings = settingOptions || {};

            var model = this;

            settings.url = (settings.url || document.location.href);
            settings.title = (settings.title || "");
            var isie = methods.IsIE();

            self.ShareList = ko.observableArray([
                { title: "新浪微博", tip: "分享到新浪微博", cls: "ico-weibo", url: "http://v.t.sina.com.cn/share/share.php?url={0}&title={1}&source=bookmark" },
                { title: "QQ好友", tip: "分享到QQ好友", cls: "ico-qq", url: "http://connect.qq.com/widget/shareqq/index.html?url={0}&title={1}" },
                { title: "QQ空间", tip: "分享到QQ空间", cls: "ico-qzone", url: "http://sns.qzone.qq.com/cgi-bin/qzshare/cgi_qzshare_onekey?url={0}&title={1}" },
                { title: "腾讯微博", tip: "分享到腾讯微博", cls: "ico-qqweibo", url: "http://share.v.t.qq.com/index.php?c=share&a=index&pic=&url={0}&title={1}" }
            ]);

            model.share = function (data) {
                if (data.title == "QQ空间") {
                    window.open(String.format(data.url, encodeURIComponent(document.location.href), isie ? settings.title : encodeURIComponent(settings.title)), 'newwindow', 'width=450,height=400');
                } else {
                    window.open(String.format(data.url, encodeURIComponent(encodeURIComponent(document.location.href)), isie ? settings.title : encodeURIComponent(settings.title)), 'newwindow', 'width=450,height=400');
                }
            }
        }
    };

    exports.viewModel = comentModel;
});

