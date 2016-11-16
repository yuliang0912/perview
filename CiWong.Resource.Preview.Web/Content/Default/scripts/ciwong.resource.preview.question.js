define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko");

    var defaultSettings = function () {
        return {
            questionShowTemplate: {
                0: "danxuanTemplate", 1: "danxuanTemplate", 2: "duoxuanTemplate", 3: "tiankongTemplate", 4: "wanxingTemplate",
                5: "yuedulijieTemplate", 6: "jiandaTemplate", 7: "jiandaTemplate", 8: "panduanTemplate"
            }
        }
    };

    var methods = {
        initHtmlEventHandler: function () { //初始化题目的事件与效果
            $("div.gestalt div.q-txt[qid]").die().live("click", function () {
                var x = $(this).offset().left,
                    y = $(this).offset().top,
                    self = $(this);

                $("#wanxing_" + $(this).attr("qid")).css({ top: y, left: x, "display": "block" }).siblings().hide();
                /**答案显示区域**/
                self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".rl").eq($(this).next().attr("index") - 1).show().siblings().hide();
                /**解题思路显示区域**/
                //if (self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".tl").eq(self.index()).length > 0 && self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".tl").eq(self.index()).html().trim() != "") {
                	//self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".refThinking").show();//上一级为空的时候隐藏需要触发显示
                	self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".refThinking").show().eq($(this).next().attr("index") - 1).show().siblings().hide();
                //} else {
                //    self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".tl").parent().hide()
                //}
                /*文本(答案:    解题思路:)*/
                self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".tit").show();
            })

            $("article.work-quesCon div.read-step ul.tepList li").die().live("click", function () {
                var self = $(this);
                //切换过程中隐藏评分层
                //var index = parseInt($(this).attr("index"));
                //$("#manual-mark").hide();
                var question = ko.contextFor(this).$data;

                if (question.qtype == 3 && $(this).parent().parent().parent().find("li").length > 0) {
                    if (question.options.length > 1) {
                        $("#manual-mark").hide();
                    }
                }

                self.addClass("curr").siblings().removeClass("curr");
                self.parentsUntil("article").next(".work-con").find("article").eq(self.index()).show().siblings().hide();

                /**答案显示区域**/
                self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".rl").eq(self.index()).show().siblings().hide();
                /**解题思路显示区域**/
                var _self = self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".tl").eq(self.index());
                if (_self.length > 0 && _self.html().trim() != "") {
                	self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".refThinking").show().eq(self.index()).show().siblings().hide();
                    _self.show().siblings().hide();
                } else {
                    self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".tl").parent().hide()
                }
                /*文本(答案:    解题思路:)*/
                self.parentsUntil("article").nextAll(".answerBox").find(".answer").find(".tit").show();
            })

            $("div.q-txt[contenteditable=true][index],div.shortAn[contenteditable=true]").die().live("blur", function () {
                var index = $(this).attr("index"),
                    userAnswer = $(this).text(),
                    question = ko.contextFor(this).$data;

                if (userAnswer != "") {
                    index && (index = parseInt(index, 10) - 1);
                    question.setAnswerHanlder(question, null, userAnswer, index);
                }
            })

            $("div.q-txt[contenteditable=true][index]").die().live("keypress", function (evnet) {
                if (event.keyCode == 13) {
                    return false;
                }
            })
        },
        yuedulijieEventHandler: function () {
            $("article.work-quesCon div.read-step ul.tepList li").each(function () {
                $(this).index() == 0 && $(this).addClass("curr");
            });
        },
        attachmentLoadingHandler: function () {
            setTimeout(function () { $("article.work-quesCon div[data-attachment]").show() }, 50);
        },
        wanxingEventHandler: function (qid, text) {
            if ($("#wanxing_" + qid).length > 0) {
                $("#wanxing_" + qid).hide();

                text && $("div.gestalt div.q-txt[qid=" + qid + "]").html(text);
            }
        },
        attachmentHandler: function (attachments, stem) {
            var temp = '{AttachmentPosition.1}{AttachmentPosition.3}{AttachmentPosition.4}<div>{Question.Stem}</div>{AttachmentPosition.2}';

            if (/{#video#}(\S*){#\/video#}/.test(stem)) {
                var swfPlayer = "<embed src='{0}' videosrc='{0}' autostart='false' type='application/x-shockwave-flash' width='768' height='480' allowfullscreen='true' allownetworking='all' allowscriptaccess='always' wmode='transparent' />";
                stem = stem.replace(/{#video#}(\S*){#\/video#}/g, swfPlayer.format("$1"));
            }

            function buildHtml(attachment) {
                var id = Math.random().toString().replace("0.", ""),
                    html = '<div class="{0}">'.format(attachment.position == 1 ? "mb10" : attachment.position == 2 ? "mt10" : attachment.position == 3 ? "fl mr10" : "fr ml10");

                switch (attachment.file_type) {
                    case 1:
                        html += '<img src="{0}" onload="javascript:DrawImage(this,350,350)" />'.format(attachment.file_url);
                        break;
                    case 2:
                        html += '<div data-attachment="1" style="display:none">';
                        html += '<object type="application/x-shockwave-flash" data="/Content/Flash/player.swf" id="{0}" height="24" width="185">'.format(id);
                        html += '<param name="movie" value="/Content/Flash/player.swf">';
                        html += '<param name="FlashVars" value="titles=loading&amp;playerID={0}&amp;bg=0xf3f6f8&amp;leftbg=0xcef4da&amp;lefticon=0x647062&amp;voltrack:0xacb8aa&amp;volslider:0xc4d0c3&amp;rightbg=0x61d26a&amp;rightbghover=0x84e45e&amp;righticon=0xF2F2F2&amp;righticonhover=0xFFFFFF&amp;text=0x408d05&amp;track=0xFFFFFF&amp;border=0xffffff&amp;loader=0xc7e795&amp;tracker=0x6dae05&amp;soundFile={1}{2}">'.format(id, attachment.file_url, attachment.file_url.indexOf("http://img1.") > -1 ? "?fileType=mp3" : "");
                        html += '<param name="quality" value="high">';
                        html += '<param name="menu" value="false">';
                        html += '<param name="wmode" value="transparent">';
                        html += '</object>';
                        html += '</div>';
                        require(['./flash/audio-player-noswfobject']);
                        break;
                    case 3:
                        html += '<a href="javascript:closeFlash(\'{0}\', \'{0}_\')"><img style="cursor:pointer; display: none;" src="http://img.ciwong.com/edu/common/basic/guanbi.gif" alt="关闭" id="{0}_"></a>'.format(id);
                        html += '<a href="javascript:loadFlash(\'{0}\', 850, 520, \'{1}\', \'{1}_\')"><img style="cursor:pointer; display:block;" src="http://img.ciwong.com/edu/common/basic/jiazai.gif" alt="加载" id="{1}"></a>'.format(attachment.file_url, id);
                        html += '<div id="swfDIV_{0}"></div>'.format(id);
                        require(['./flash/swfobject']);
                        break;
                    default:
                        html += '<a href="{0}">下载附件</a>'.format(attachment.file_url);
                        break;
                }
                html += '</div>';

                return html;
            }
            attachments && $.each(attachments, function (i, item) {
                temp = temp.replace("{AttachmentPosition.{0}}".format(item.position), buildHtml(item));
            });
            return temp.replace(/{AttachmentPosition.(\d*)}/g, "").replace("{Question.Stem}", stem ? stem.replace(/&nbsp;/g, "	") : "");
        },
        stemBuildV1: function (question) {
            if (question.qtype == 3) {
                question.stem = question.stem.replace(/{#blank#}(\d*){#\/blank#}/g, '<div class="q-txt" contenteditable="true" index="$1"></div>&nbsp;');
            }
            else if (question.qtype == 4) {
                $.each(question.children, function (i) {
                    this.qtype = 1;
                    this.attachmentIgnore = true;
                    question.stem = question.stem.replace('{#blank#}{0}{#\/blank#}'.format(i + 1), '<div class="q-txt tc" qid="{0}">{1}</div>'.format(this.version_id, i + 1));
                });
            }
            else if (question.qtype == 6 || question.qtype == 7) {
                question.answerBox = '<div class="shortAn" contenteditable="true"></div>';
            }

            $.each(question.options, function () {
                if (!question.attachmentIgnore) {
                    this.stem = methods.attachmentHandler(this.attachments, this.stem);
                } else {
                    this.stem = this.stem.removeHtmlTag();
                }
            });
            question.stem = methods.attachmentHandler(question.attachments, question.stem);
            question.assessResult = function () { return "" };
        },
        stemBuildV2: function (question) {
            if (question.qtype == 3) {
                $.each(question.userAnswer.answers, function (i, item) {
                	question.stem = question.stem.replace('{#blank#}{0}{#\/blank#}'.format(item.sid + 1), '<div class="q-txt tc">{0}</div>{1}&nbsp;'.format(ciwong.encoder.htmlEncode(item.content), methods.getCorrectStyle((item.assess == undefined ? 2 : item.assess), item.sid + 1, question.question_ref_sorce == 0 ? undefined : item.item_score)));
                });
                question.stem = question.stem.replace(/{#blank#}(\d*){#\/blank#}/g, '<div class="q-txt"></div>&nbsp;' + methods.getCorrectStyle(2, "$1", question.question_ref_sorce == 0 ? undefined : 0, 1));
            }
            else if (question.qtype == 4) {
                $.each(question.children, function (i, item, checkOption) {
                    item.qtype = 1;
                    item.attachmentIgnore = true;
                    item.userAnswer = item.userAnswer || { assess: 2, score: 0, answers: [] };
                    item.userAnswer.answers.length > 0 && (checkOption = methods.getOptionStem(item.options, item.userAnswer.answers[0].content));
                    question.stem = question.stem.replace('{#blank#}{0}{#\/blank#}'.format(i + 1), '<div class="q-txt tc" qid="{0}">{1}</div>{2}'.format(item.version_id, checkOption ? checkOption : i + 1, methods.getCorrectStyle(item.userAnswer.assess, i + 1, item.question_ref_sorce == 0 ? undefined : item.userAnswer.score)));
                });
            }
            else if (question.qtype == 6 || question.qtype == 7) {
                question.answerBox = '<div class="shortAn">{0}</div>'.format(question.userAnswer.answers.length > 0 ? ciwong.encoder.htmlEncode(question.userAnswer.answers[0].content) : "");
            }

            $.each(question.options, function () {
                if (!question.attachmentIgnore) {
                    this.stem = methods.attachmentHandler(this.attachments, this.stem);
                } else {
                    this.stem = this.stem.removeHtmlTag();
                }
            });
            //题干
            question.stem = methods.attachmentHandler(question.attachments, question.stem);

            question.ref_info.solving_idea = methods.attachmentHandler(question.ref_info.attachments, question.ref_info.solving_idea);
            if (question.ref_info.solving_idea == "<div></div>") {
                question.ref_info.solving_idea = undefined;
            }

        },
        getCorrectStyle: function (assess, index, score) {
            var assessIco = assess == 1 ? '<i class="res-right"></i>' :
							assess == 2 ? '<i class="res-miss"></i>' :
							assess == 4 ? '<i class="res-ask"></i>' :
							assess == 3 ? '<i class="res-hRight"></i>' : "";
            if (score != undefined) {
            	return '<span class="manual" index="{0}">{1}<em class="red">({2}){3}</em></span>'.format(index, assessIco, score, assess == 4 ? "分数由老师批改" : "");
            } else {
                return '<span class="manual" index="{0}">{1}</span>'.format(index, assessIco, score);
            }
        },
        getOptionStem: function (options, optionId) {
            var option = $.grep(options, function (n) { return n.id == optionId; });
            return option.length > 0 ? option[0].stem.removeHtmlTag() : undefined;
        },
        getRefAnswer: function (question) {
            if (question.parent_version != "0") {
                return undefined;
            }
            function getSingleAnswer(question) {
                if (question.ref_info.answers.length == 0) {
                    return "";
                }
                var _answer = "";
                switch (question.qtype) {
                    case 1:
                        $.each(question.options, function (i) {
                            if (this.id == question.ref_info.answers[0]) {
                                _answer = ciwong.englishLetter[i];
                                return false;
                            }
                        });
                        break;
                    case 2:
                        $.each(question.options, function (i) {
                            if ($.inArray(this.id, question.ref_info.answers) > -1) {
                                _answer += ciwong.englishLetter[i];
                            }
                        });
                        break;
                    case 8:
                        _answer = question.ref_info.answers[0] == "0" ? "错误" : question.ref_info.answers[0] == "1" ? "正确" : "";
                        break;
                    default:
                        _answer = question.ref_info.answers.join("<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;");
                        break;
                }
                return _answer.replace(/&nbsp;/g, "	");
            }

            var _anserArray = [];

            if (question.qtype == 3) {
                _anserArray = question.ref_info.answers;
            }
            else if (question.children.length == 0) {
                _anserArray.push(getSingleAnswer(question));
            } else {
                $.each(question.children, function (i, item) {
                    _anserArray.push(getSingleAnswer(item));
                });
            }
            return _anserArray;
        },
        answerHelper: function () {
            var answerList = {}, questionList = {};
            return {
                setAnswer: function (question, answer, index) {
                    index = parseInt(index, 10) || 0;
                    if (!answerList[question.version_id]) {
                        answerList[question.version_id] = [];
                        questionList[question.version_id] = question.question_ref_sorce;
                    }
                    var isRemove = false;
                    $.each(answerList[question.version_id], function (i, item) {
                        if (item.sid == index) {
                            isRemove = true;
                            answerList[question.version_id].splice(i, 1);
                            return false;
                        }
                    });
                    if (question.qtype != 2 || !isRemove) {
                        answerList[question.version_id].push({ sid: index, content: answer });
                    }
                    return answerList[question.version_id];
                },
                getAnswer: function () {
                    var versions = [], results = [];
                    for (var item in answerList) {
                        versions.push(item.toString());
                    }
                    $.each(versions, function (i, item) {
                        results.push({ version_id: item, score: questionList[item], answers: answerList[item] });
                    });
                    return results;
                }
            }
        },
        /**每个小题的解题思路**/
        getRefThinking: function (question) {
            if (question.parent_version != "0") {
                return undefined;
            }
            function getSingleThinking(question) {
            	if (question.ref_info.solving_idea == "" || question.ref_info.solving_idea == undefined) {
            		return "";
            	}
            	return methods.attachmentHandler(question.ref_info.attachments, question.ref_info.solving_idea);
            }

            var _thinkingArray = [];
            /**如果解题思路为空就不向集合中添加**/
            if (question.children.length == 0) {
                getSingleThinking(question) != "" && _thinkingArray.push(getSingleThinking(question));
            } else {
                $.each(question.children, function (i, item) {
                    getSingleThinking(item) != "" && _thinkingArray.push(getSingleThinking(item));
                });
            }
            return _thinkingArray;
        }
    };

    var viewModel = {
        preView: function (settingOptions) {
            var model = this,
                settings = $.extend(true, defaultSettings(), settingOptions || {}),
                answerHelper = methods.answerHelper();

            model.dataSource = null;

            model.list = ko.observableArray();

            model.list.subscribe(function (newQuesions) {
                model.dataSource = ko.toJS(newQuesions);

                function questionHandler(list) {
                    list && $.each(list, function (i, question) {
                        question.templateName = settings.questionShowTemplate[question.qtype];

                        methods.stemBuildV1(question);
                        //根据权限判断是否显示答案  现在首先默认为flase
                        question.refAnswers = false;

                        //question.refAnswers = settings.isShowAnswer ? methods.getRefAnswer(question) : undefined;

                        question.userAnswer = ko.observableArray();

                        question.setAnswerHanlder = function (data, event, other, index) {
                            question.qtype == 2 && (index = $.inArray(data, question.options));
                            question.userAnswer(answerHelper.setAnswer(question, other || data.id, index));
                            question.qtype == 1 && methods.wanxingEventHandler(question.version_id, data.stem);
                        };

                        question.optionClass = function (data) {
                            return $.grep(question.userAnswer(), function (n) { return n.content == data; }).length > 0 ? "currBg" : "";
                        };

                        question.isComplete = function () {
                            return question.userAnswer().length > 0;
                        };

                        question.children && question.children.length > 0 && questionHandler(question.children);
                    });
                }
                questionHandler(newQuesions);
            });

            model.sortNoTemplate = settings.sortNoTemplate || "sortNoTemplate";

            /*
               注:以下两种默认的数据加载请根据情况选择其中的一种
               1.已经获取到题目数据的,为了防止重复获取,请通过settingOptions.dataSource传入
               2.已经知道题目ID的,即工具中心中有引用题目的资源部分,请把对应的题目资源的part部分传入
               3.已知题目版本ID,通过settings.versions传入
               4.自定义模式,自己通过question.viewModel.preview.list()传入
            */
            model.setQuestion = function (versions) {
                ciwong.ajax.getJSON("/resource/getlist", { versionIds: versions, moduleId: "008a020d-72c6-4df5-ba6c-73086b8db022" }, model.list);
            }

            model.getAnswers = function () {
                return answerHelper.getAnswer();
            }

            model.afterRender = function () {
                methods.attachmentLoadingHandler();
            }

            model.wanxingAfterRender = function () {
                methods.yuedulijieEventHandler();
            }

            settings.dataSource && model.list(settings.dataSource);

            settings.versions && model.setQuestion(settings.versions);

            methods.initHtmlEventHandler();
        },
        resultView: function (settingOptions) {
            var model = this,
                settings = $.extend(true, defaultSettings(), settingOptions || {});

            model.dataSource = null;

            model.list = ko.observableArray();

            model.videolist = ko.observableArray();


            settings.isShowQuestionVideo ? ko.computed(function () {
                var versionId = '';
                settings.dataSource && $.each(settings.dataSource, function (i, item) {
                    versionId += item.id + ',';
                });
                if (versionId != '' && versionId.length > 0) { versionId = versionId.substr(0, versionId.length - 1); }

                versionId && versionId != '' && ciwong.ajax.getJSON("/paper/getquestionvideomodel", { qid: versionId }, function (listVideo) {
                    $.each(listVideo, function (i, item) {
                        model.videolist(item);
                    });
                });
            }) : '';


            model.videolist.subscribe(function (item) {
                var entity = $("#video_" + item.id).html();
                if (entity == "" || entity == undefined) {
                    $("#video_" + item.id).html('<div class="answer mt10"><p class="tit">解题视频：</p><div class="cf rl"><a versionId=' + item.video_version_id + '>' + item.name + '</a></div></div>');//
                    $("#video_" + item.id).find('a').live("click", function () {
                        $.dialog({
                            title: '视频', width: 800, height: 500, padding: 0, lock: true, cancel: true, cancelValue: '关闭'
                        }).extend({
                            ie6: true, overflow: 'inherit',
                            iframe: String.format("/paper/videopreview?versionId={0}&math={1}", $(this).attr("versionId"), Math.random())
                        });
                        $(".d-footer").hide();
                    })
                }
            });

            model.list.subscribe(function (newQuesions) {
                model.dataSource = ko.toJS(newQuesions);
                function questionHandler(list) {
                    list && $.each(list, function (i, question) {
                        question.templateName = settings.questionShowTemplate[question.qtype];

                        if (!question.userAnswer && question.qtype != 4 && question.qtype != 5) {
                            question.userAnswer = { assess: 2, score: 0, answers: [] };
                        }

                        methods.stemBuildV2(question);
                        //答案
                        question.refAnswers = settings.isShowAnswer ? methods.getRefAnswer(question) : undefined;
                        //解题思路
                        question.refThinking = settings.isShowThinking ? methods.getRefThinking(question) : undefined;
                        //解题视频

                        //settings.isShowQuestionVideo ? ciwong.ajax.getJSON("/paper/getquestionvideomodel", { qid: question.id }, function (data) {
                        //    question.questionVideo = data;
                        //}) : ''; 

                        settings.isShowQuestionVideo ? model.videolist().length > 0 && $.each(model.videolist(), function (i, item) {
                            if (item.id == question.id) {
                                question.questionVideo = item;
                            }
                        }) : question.questionVideo = '';


                        question.setAnswerHanlder = function (data, event, other, index) {
                            question.qtype == 1 && methods.wanxingEventHandler(question.version_id);
                        }

                        question.optionClass = function (data) {
                            var isChecked = $.grep(question.userAnswer.answers, function (n) { return n.content == data; }).length > 0;
                            return question.userAnswer.assess == 1 && isChecked ? "chkRight" : isChecked ? "chkWrong" : "";
                        }

                        question.isComplete = function () {
                            return question.userAnswer && question.userAnswer.answers.length > 0;
                        }

                        question.assessResult = function (index) {
                            if (index != undefined) {
                                return question.qtype == 4 || question.qtype == 5 || !question.userAnswer ? '<span class="num">{0}.</span>'.format(index + 1) :
                                       question.userAnswer.assess == 1 ? '<span class="right"></span>' :
                                       question.userAnswer.assess == 2 ? '<span class="wrong"></span>' :
                                       question.userAnswer.assess == 4 ? '<span class="ask"></span>' :
                                       question.userAnswer.assess == 3 ? '<span class="hRight"></span>' : "";
                            } else {
                                return (question.qtype == 3 && question.parent_version == "0") || question.qtype == 4 || question.qtype == 5 ? "" : methods.getCorrectStyle(question.userAnswer.assess, 1, question.question_ref_sorce == 0 ? undefined : question.userAnswer.score);
                            }
                        }
                        question.children && question.children.length > 0 && questionHandler(question.children);
                    });
                }
                questionHandler(newQuesions);
            });

            model.sortNoTemplate = settings.sortNoTemplate || "scoreTemplate";

            model.afterRender = function () {
                methods.attachmentLoadingHandler();
            }

            model.wanxingAfterRender = function () {
                methods.yuedulijieEventHandler();
            }

            settings.dataSource && model.list(settings.dataSource);

            methods.initHtmlEventHandler();
        },
        correctView: function (settingOptions) {
            var model = new viewModel.resultView(settingOptions);

            model.afterRender = function () {
                methods.attachmentLoadingHandler();
            }
            return model;
        }
    };

    exports.viewModel = viewModel;
    exports.moduleId = "008a020d-72c6-4df5-ba6c-73086b8db022";

    (function (ciwong, window) {

        var styleTag = document.createElement("link");
        styleTag.setAttribute('type', 'text/css');
        styleTag.setAttribute('rel', 'stylesheet');
        styleTag.setAttribute('href', "/Content/Question/css/question.css");
        $("head")[0].appendChild(styleTag);




        /*  以下为当前viewModel所用的模板以及部分函数 */
        ciwong.koTemplateEngine.add("questionListTemplate", '\
		<!--ko if: $data -->\
			<!--ko template { name: "questionTemplate", foreach: list, afterRender: afterRender } --><!--/ko-->\
        <!--/ko-->');

        /**非会员隐藏参考答案  只是显示当前题目的正确和错误**/
        ciwong.koTemplateEngine.add("questionTemplate", '\
        <article class="work-quesCon">\
            <!--ko template: $parent.sortNoTemplate -->\
            <!--/ko-->\
            <!--ko template: templateName -->\
            <!--/ko-->\
			<!-- ko if:refAnswers -->\
			<div class="answerBox mt10">\
				<div class="answer">\
					<p class="tit">参考答案：</p>\
					<!--ko if:qtype!=3 -->\
					<!--ko foreach:refAnswers -->\
						<div class="cf rl" data-bind="visible: $index() == 0,html:\'(\'+ ($index() + 1) +\')、&nbsp;\' + $data"></div>\
                    <!--/ko-->\
					<!--/ko-->\
					<!--ko if:qtype == 3 -->\
					<!--ko foreach:refAnswers -->\
						<div class="cf rl" data-bind="html:\'(\'+ ($index() + 1) +\')、&nbsp;\' + $data"></div>\
                    <!--/ko-->\
					<!--/ko-->\
				</div>\
            <div data-bind="attr:{\'id\':\'video_\'+$data.id}"></div>\
				<!-- ko if: refThinking && refThinking.length > 0 -->\
				<div class="answer mt10" visible: $.trim(refThinking[0])!=\'\'">\
                    <!--ko foreach:refThinking -->\
                        <!--ko if:$data.toString().trim()!=""-->\
                            <div class="refThinking" data-bind="visible: $index() == 0"><p class="tit">解题思路：</p>\
						    <div class="cf tl" data-bind="html:$data"></div>\</div>\
					    <!--/ko-->\
                        <!--ko if:$data.toString().trim()=="" -->\
						    <div class="cf tl" data-bind="visible: $index() == 0"></div>\
					    <!--/ko-->\
                    <!--/ko-->\
				</div>\
				<!-- /ko -->\
			</div>\
			<!-- /ko -->\
        </article>');

        ciwong.koTemplateEngine.add("sortNoTemplate", '\
        <div class="work-tBar">\
            <span class="num" data-bind="text: $index() + 1 + \'.\'">&nbsp;</span>\
            <p class="f14" data-bind="visible:question_ref_sorce > 0, text:question_ref_sorce + \'分\'"></p>\
        </div>');

        ciwong.koTemplateEngine.add("scoreTemplate", '\
        <div class="work-tBar">\
            <span class="num" data-bind="text: $index() + 1 + \'.\'">&nbsp;</span>\
            <p class="f14" data-bind="visible:question_ref_sorce > 0, text: question_ref_sorce + \'分\'"></p>\
        </div>\
        <div class="ml15" data-bind="html:assessResult()">\
        </div>');

        ciwong.koTemplateEngine.add("noScoreTemplate", '\
        <div class="work-tBar" data-bind="html:assessResult($index())">\
        </div>');

        ciwong.koTemplateEngine.add("sonquestionTemplate", '\
        <article data-bind="visible: $index() == 0">\
            <div><i class="qesNum" data-bind="text: \'Q\'+ ($index() + 1)"></i></div>\
            <div data-bind="template: { name: templateName }"></div>\
        </article>');

        ciwong.koTemplateEngine.add("danxuanTemplate", '\
        <div class="work-t cf" data-bind="html: stem"></div>\
        <div class="work-con">\
            <ul class="list" data-bind="foreach: options">\
                <li class="cf" data-bind="click: $parent.setAnswerHanlder, css: $parent.optionClass(id)">\
                    <span class="icon"><i class="radio" data-bind="text: ciwong.englishLetter[$index()]"></i></span>\
                    <p data-bind="html: stem"></p>\
                </li>\
            </ul>\
        </div>');

        ciwong.koTemplateEngine.add("duoxuanTemplate", '\
        <div class="work-t cf" data-bind="html: stem"></div>\
        <div class="work-con">\
            <ul class="list" data-bind="foreach: options">\
                <li class="cf" data-bind="click: $parent.setAnswerHanlder, css: $parent.optionClass(id)">\
                    <span class="icon"><i class="checkbox" data-bind="text: ciwong.englishLetter[$index()]"></i></span>\
                    <p data-bind="html: stem"></p>\
                </li>\
            </ul>\
        </div>');

        ciwong.koTemplateEngine.add("tiankongTemplate", '\
        <div class="work-t cf" data-bind="html: stem">\
        </div>');

        ciwong.koTemplateEngine.add("panduanTemplate", '\
        <div class="work-t cf" data-bind="html: stem"></div>\
        <div class="work-con">\
            <ul class="list">\
                <li data-bind="click: function(data,event){ setAnswerHanlder(data,event,\'0\'); }, css:optionClass(\'0\')">\
                    <span class="icon"><i class="radio"></i><i class="miss"></i></span>\
                </li>\
                <li data-bind="click: function(data,event){ setAnswerHanlder(data,event,\'1\'); }, css:optionClass(\'1\') ">\
                    <span class="icon"><i class="radio"></i><i class="right"></i></span>\
                </li>\
            </ul>\
        </div>');

        ciwong.koTemplateEngine.add("jiandaTemplate", '\
        <div class="work-t cf" data-bind="html: stem"></div>\
        <div class="work-con" data-bind="html:answerBox">\
        </div>');

        //<a data-bind="text:JSON.stringify($data)"></a>\
        ciwong.koTemplateEngine.add("yuedulijieTemplate", '\
        <div class="work-t read-com ovh cf" data-bind="html: stem"></div>\
        <div class="read-step">\
            <ul class="tepList cf" data-bind="foreach: children">\
                <li data-bind="css: { \'ok\': isComplete() }">\
                    <i class="ico" data-bind="html: assessResult()"></i>\
                    <p class="qt" data-bind="text: \'Question\' + ($index() + 1) "><i class="arrow a-t"></i><i class="arrow a-in-t"></i></p>\
                </li>\
            </ul>\
        </div>\
        <div class="work-con">\
            <div class="read-con" data-bind="template: { name: \'sonquestionTemplate\', foreach: children, afterRender:$parent.wanxingAfterRender }"></div>\
        </div>');

        ciwong.koTemplateEngine.add("wanxingTemplate", '\
        <div class="work-t gestalt cf" data-bind="html: stem"></div>\
        <div data-bind="foreach: children">\
            <div class="ges-opt" data-bind="attr: { \'id\': \'wanxing_\' + version_id }">\
                <i class="arrow a-t"></i>\
                <i class="arrow a-in-t"></i>\
                <!--ko foreach: options -->\
                <a href="javascript:;" data-bind="click: $parent.setAnswerHanlder, css: $parent.optionClass(id)">\
                    <em data-bind="text:ciwong.englishLetter[$index()] + \'.\'"></em>\
                    <span data-bind="html:stem">&nbsp;</span>\
                </a>\
                <!--/ko-->\
            </div>\
        </div>');

        window.DrawImage = function (ImgD, iwidth, iheight) {
            var iwidth = 450, iheight = 2000;
            //参数(图片,允许的宽度,允许的高度)
            var image = new Image();
            image.src = ImgD.src;
            if (image.width > 0 && image.height > 0) {
                if (image.width / image.height >= iwidth / iheight) {
                    if (image.width > iwidth) {
                        ImgD.width = iwidth;
                        ImgD.height = (image.height * iwidth) / image.width;
                    } else {
                        ImgD.width = image.width;
                        ImgD.height = image.height;
                    }
                } else {
                    if (image.height > iheight) {
                        ImgD.height = iheight;
                        ImgD.width = (image.width * iheight) / image.height;
                    } else {
                        ImgD.width = image.width;
                        ImgD.height = image.height;
                    }
                }
            }
        }

        window.convertVideo = function (userAnswer) {
            var html = "";
            if (userAnswer && userAnswer.answers.length > 0 && userAnswer.answers[0].content != "") {
                var id = Math.random().toString().replace("0.", "");
                html += '<div>我的录音答案：';
                html += '<object type="application/x-shockwave-flash" data="/Content/Flash/player.swf" id="{0}" height="24" width="185">'.format(id);
                html += '<param name="movie" value="/Content/Flash/player.swf">';
                html += '<param name="FlashVars" value="titles=loading&amp;playerID={0}&amp;bg=0xf3f6f8&amp;leftbg=0xcef4da&amp;lefticon=0x647062&amp;voltrack:0xacb8aa&amp;volslider:0xc4d0c3&amp;rightbg=0x61d26a&amp;rightbghover=0x84e45e&amp;righticon=0xF2F2F2&amp;righticonhover=0xFFFFFF&amp;text=0x408d05&amp;track=0xFFFFFF&amp;border=0xffffff&amp;loader=0xc7e795&amp;tracker=0x6dae05&amp;soundFile={1}">'.format(id, userAnswer.answers[0].content);
                html += '<param name="quality" value="high">';
                html += '<param name="menu" value="false">';
                html += '<param name="wmode" value="transparent">';
                html += '</object>';
                html += '</div>';
            }
            return html;
        }
    })(ciwong, window);
});