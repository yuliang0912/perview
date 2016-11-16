/// <reference path="../../Default/scripts/ciwong.resource.preview.extends.js" />

define(function (r, exports) {

	var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko");
	var isPc = true;
	var defaultSettings = function () {
		return {
			questionShowTemplate: {
				0: "danxuanTemplate", 1: "danxuanTemplate", 2: "duoxuanTemplate", 3: "tiankongTemplate", 4: "wanxingTemplate",
				5: "yuedulijieTemplate", 6: "jiandaTemplate", 7: "jiandaTemplate", 8: "panduanTemplate"
			}
		};
	};

	var methods = {
		initHtmlEventHandler: function () { //初始化题目的事件与效果
			$("div.q-txt[contenteditable=true][index],div.shortAn[contenteditable=true]").die().live("blur", function () {
				var index = $(this).attr("index"),
		            userAnswer = $(this).text(),
		            question = ko.contextFor(this).$data;

				if (userAnswer != "") {
					index && (index = parseInt(index, 10) - 1);
					question.setAnswerHanlder(question, null, userAnswer, index);
				}
			});

			$("div.q-txt[contenteditable=true][index]").die().live("keypress", function (event) {
				if (event.keyCode == 13) {
					return false;
				}
			});
		},
		yuedulijieEventHandler: function () {
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

			if (/{#video#}(\S*){#\/video#}/.test(stem) && stem.lastIndexOf("video.qiyi.com") > 0) {
				var regexp = /video_id=([0-9a-z]*)/;/**获取字符串中的video_id**/
				var videoContent = /{#video#}(http(s)?:\/\/([\w-]+\.)+[\w-]+(\/[\w- .\/?%&=]*)){#\/video#}/g || /{#video#}(\S*){#\/video#}/g;

				$.each(stem.match(videoContent), function (i, item) {
					//console.log(item);
					//id = item.match(regexp)[1];
					//ciwong.ajax.getJSON("/paper/GetAiQiYiURL", { voteid: id }, function (xx) {
					//    stem = stem.replace(item, '<video controls="" name="media" style="width: 100%;"><source src="' + xx.mp42 + '" type="video/mp4"></video>');
					//}, null, false);
					stem = stem.replace(item, '<video controls="" name="media" style="width: 100%;"><source id="' + item.match(regexp)[1] + '" type="video/mp4"></video>');
				});
			} else if (/{#video#}(\S*){#\/video#}/.test(stem)) {
				var swfPlayer = "<embed src='{0}' videosrc='{0}' autostart='false' type='application/x-shockwave-flash' width='768' height='480' allowfullscreen='true' allownetworking='all' allowscriptaccess='always' wmode='transparent' />";
				stem = stem.replace(/{#video#}(\S*){#\/video#}/g, swfPlayer.format("$1"));
			}


			//if (/{#video#}(\S*){#\/video#}/.test(stem)) {
			//    var swfPlayer = "<embed src='{0}' videosrc='{0}' autostart='false' type='application/x-shockwave-flash' width='768' height='480' allowfullscreen='true' allownetworking='all' allowscriptaccess='always' wmode='transparent' />";
			//    stem = stem.replace(/{#video#}(\S*){#\/video#}/g, swfPlayer.format("$1"));
			//}

			function buildHtml(attachment) {
				var id = Math.random().toString().replace("0.", ""),
                    html = '<div class="{0}">'.format(attachment.position == 1 ? "mb10" : attachment.position == 2 ? "" : attachment.position == 3 ? "fl mr10" : "fr ml10");//mt10

				switch (attachment.file_type) {
					case 1:
						html += '<img src="{0}" onload="javascript:DrawImage(this,350,350)" />'.format(attachment.file_url);
						break;
					case 2:
						html += '<div data-attachment="1" style="display:none">';
						if (isPc) {
							html += '<object type="application/x-shockwave-flash" data="/Content/Flash/player.swf" id="{0}" height="24" width="185">'.format(id);
							html += '<param name="movie" value="/Content/Flash/player.swf">';
							html += '<param name="FlashVars" value="titles=loading&amp;playerID={0}&amp;bg=0xf3f6f8&amp;leftbg=0xcef4da&amp;lefticon=0x647062&amp;voltrack:0xacb8aa&amp;volslider:0xc4d0c3&amp;rightbg=0x61d26a&amp;rightbghover=0x84e45e&amp;righticon=0xF2F2F2&amp;righticonhover=0xFFFFFF&amp;text=0x408d05&amp;track=0xFFFFFF&amp;border=0xffffff&amp;loader=0xc7e795&amp;tracker=0x6dae05&amp;soundFile={1}{2}">'.format(id, attachment.file_url, attachment.file_url.indexOf("http://img1.") > -1 ? "?fileType=mp3" : "");
							html += '<param name="quality" value="high">';
							html += '<param name="menu" value="false">';
							html += '<param name="wmode" value="transparent">';
							html += '</object>';
						} else {
							html += '<audio src="{0}{1}" preload=""></audio>'.format(attachment.file_url, attachment.file_url.indexOf("http://img1.") > -1 ? "?fileType=mp3" : "");
							//html += '<audio src="{0}" preload="none"></audio>'.format(attachment.file_url); //{1}, attachment.file_url.indexOf("http://img1.") > -1 ? "?fileType=mp3" : "");
							//html5音频控件样式太丑
							//html += '<audio controls="controls">';
							//html += '<source src="{0}" type="audio/mpeg">'.format(attachment.file_url);
							//html += '<source src="{0}" type="audio/wav">'.format(attachment.file_url);
							//html += '</audio>';
						}
						html += '</div>';
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
				question.stem = question.stem.replace(/{#blank#}(\d*){#\/blank#}/g, '<div class="q-txt" style="word-break:break-all;" contenteditable="true" index="$1"></div>&nbsp;');
			}
			else if (question.qtype == 4) {
				$.each(question.children, function (i) {
					this.qtype = 1;
					this.attachmentIgnore = true;
					question.stem = question.stem.replace('{#blank#}{0}{#\/blank#}'.format(i + 1), '<div class="q-txt tc" style="word-break:break-all;" qid="{0}">{1}</div>'.format(this.version_id, i + 1));
				});
			}
			else if (question.qtype == 6 || question.qtype == 7) {
				question.answerBox = '<div class="shortAn" style="word-break:break-all" contenteditable="true"></div>';
			}

			$.each(question.options, function () {
				if (!question.attachmentIgnore) {
					this.stem = methods.attachmentHandler(this.attachments, this.stem);
				} else {
					this.stem = this.stem.removeHtmlTag();
				}
			});
			question.stem = question.stem.replace(/width="(\d*)(\w*)"/g, '');//首先清除题干中的宽度样式，比如用户复制的表格样式将会导致在手机展示时宽度超出
			question.stem = methods.attachmentHandler(question.attachments, question.stem);
			question.assessResult = function () { return ""; };
		},
		stemBuildV2: function (question) {
			if (question.qtype == 3) {
				$.each(question.userAnswer.answers, function (i, item) {
					question.stem = question.stem.replace('{#blank#}{0}{#\/blank#}'.format(item.sid + 1), '<div class="q-txt tc" style="word-break:break-all;">{0}</div>{1}&nbsp;'.format(ciwong.encoder.htmlEncode(item.content), methods.getCorrectStyle(question.version_id + (parseInt(item.sid) + 1), item.assess, item.sid + 1, question.question_ref_sorce == 0 ? undefined : item.item_score)));
				});
				question.stem = question.stem.replace(/{#blank#}(\d*){#\/blank#}/g, '<div class="q-txt" style="word-break:break-all;"></div>&nbsp;' + methods.getCorrectStyle(question.version_id + "$1", 2, "$1", question.question_ref_sorce == 0 ? undefined : 0));
			}
			else if (question.qtype == 4) {
				$.each(question.children, function (i, item, checkOption) {
					item.qtype = 1;
					item.attachmentIgnore = true;
					item.userAnswer = item.userAnswer || { assess: 2, score: 0, answers: [] };
					item.userAnswer.answers.length > 0 && (checkOption = methods.getOptionStem(item.options, item.userAnswer.answers[0].content));
					question.stem = question.stem.replace('{#blank#}{0}{#\/blank#}'.format(i + 1), '<div class="q-txt tc" style="word-break:break-all;" qid="{0}">{1}</div>{2}'.format(item.version_id, checkOption ? checkOption : i + 1, methods.getCorrectStyle(item.version_id, item.userAnswer.assess, i + 1, item.question_ref_sorce == 0 ? undefined : item.userAnswer.score)));
				});
			}
			else if (question.qtype == 6 || question.qtype == 7) {
				question.answerBox = '<div class="shortAn" style="word-break:break-all">{0}</div>'.format(question.userAnswer.answers.length > 0 ? ciwong.encoder.htmlEncode(question.userAnswer.answers[0].content) : "");
			}

			$.each(question.options, function () {
				if (!question.attachmentIgnore) {
					this.stem = methods.attachmentHandler(this.attachments, this.stem);
				} else {
					this.stem = this.stem.removeHtmlTag();
				}
			});
			question.stem = question.stem.replace(/width="(\d*)(\w*)"/g, '');//首先清除题干中的宽度样式，比如用户复制的表格样式将会导致在手机展示时宽度超出
			question.stem = methods.attachmentHandler(question.attachments, question.stem);
		},
		getCorrectStyle: function (id, assess, index, score) {
			var assessIco = assess == 1 ? '<i class="res-right"></i>' :
							assess == 2 ? '<i class="res-miss"></i>' :
							assess == 4 ? '<i class="res-ask"></i>' :
							assess == 3 ? '<i class="res-hRight"></i>' : "";
			if (score != undefined) {
				return '<span class="manual" index="{0}" id="manual{1}">{2}<em class="red">({3}){4}</em></span>'.format(index, id, assessIco, score, assess == 4 ? "分数由老师批改" : "");
			} else {
				return '<span class="manual" index="{0}" id="manual{1}">{2}</span>'.format(index, id, assessIco);
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
			};
		},
		/*批改试卷方法列表开始*/
		correctScore: function (question, index) {
			var retScore = 0;
			if (question.qtype != 3 || !question.ref_info || question.ref_info.answers.length <= 1) {
				retScore = question.question_ref_sorce;
			}
			else if (!isNaN(index) && index <= question.ref_info.answers.length) {
				var avgScore = parseFloat((question.question_ref_sorce / question.ref_info.answers.length).toFixed(2));
				retScore = index < question.ref_info.answers.length ? avgScore : question.question_ref_sorce - avgScore * index + avgScore;
			}
			return parseFloat(parseFloat(retScore).toFixed(2));
		},
		getItemAnswer: function (question, index) {
			if (question.userAnswer && question.userAnswer.answers) {
				var answer = $.grep(question.userAnswer.answers, function (item) { return item.sid == index - 1 })[0];
				if (answer && !answer.item_score) {
					answer.item_score = answer.assess == 1 ? methods.correctScore(question, index) : 0;
				}
				return answer;
			}
			return null;
		},
		setItemAnswer: function (question, index, assess, score) {
			var answer = methods.getItemAnswer(question, index);
			if (answer) {
				answer.item_score = score;
				answer.assess = assess;
			} else {
				question.userAnswer.answers.push({ sid: index - 1, assess: assess, score: score });
			}
		},
		setCorrectHtmlHandler: function (mark, assess, score, totalScore) {
			var assessIco = assess == 1 ? '<i class="res-right"></i>' :
                            assess == 2 ? '<i class="res-miss"></i>' :
                            assess == 4 ? '<i class="res-ask"></i>' :
                            assess == 3 ? '<i class="res-hRight"></i>' : "";
			$("#_totalScore").html('得分<i>' + totalScore + '</i>分');
			//$("#manual-mark").hide();
			$(mark).html('{0}<em class="red">({1})</em></span>'.format(assessIco, score));
		},
		/*批改试卷方法列表结束*/
		getRefThinking: function (question) {
			if (question.children.length > 0) {
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
			isPc = settings.isPc;
			model.dataSource = null;

			model.list = ko.observableArray();

			model.list.subscribe(function (newQuesions) {
				model.dataSource = ko.toJS(newQuesions);

				function questionHandler(list) {
					list && $.each(list, function (i, question) {
						question.templateName = settings.questionShowTemplate[question.qtype];
						question.isCorrectView = false;
						methods.stemBuildV1(question);

						question.refAnswers = undefined;// settings.isShowAnswer ? methods.getRefAnswer(question) : undefined;
						//解题思路
						question.refThinking = undefined;
						//用户答案
						question.userAnswer = ko.observableArray();

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
			};

			model.getAnswers = function () {
				return answerHelper.getAnswer();
			};

			model.afterRender = function () {
				methods.attachmentLoadingHandler();

				/**读取题目中的视频地址**/
				var vlist = $("video source");
				$.each(vlist, function (i, item) {
					var playVideo = $(item).attr("t");/**获取当前地址是否存在URl**/
					var videoId = $(item).attr("id");/****/
					if (!playVideo || playVideo == '') {
						$(item).attr("t", "1");
						ciwong.ajax.getJSON("/paper/GetAiQiYiURL", { voteid: videoId }, function (data) {
							$("#" + videoId).attr("src", data.mp42);
						});
					}
				});
			};

			model.wanxingAfterRender = function () {
				methods.yuedulijieEventHandler();
			};

			settings.dataSource && model.list(settings.dataSource);

			settings.versions && model.setQuestion(settings.versions);

			methods.initHtmlEventHandler();
		},
		resultView: function (settingOptions) {
			var model = this,
                settings = $.extend(true, defaultSettings(), settingOptions || {});
			isPc = settings.isPc;
			model.dataSource = null;

			model.list = ko.observableArray();
			model.list.subscribe(function (newQuesions) {
				model.dataSource = ko.toJS(newQuesions);
				function questionHandler(list) {
					list && $.each(list, function (i, question) {
						question.templateName = settings.questionShowTemplate[question.qtype];

						if (!question.userAnswer && question.qtype != 4 && question.qtype != 5) {
							question.userAnswer = { assess: 2, score: 0, answers: [] };
						}

						methods.stemBuildV2(question);

						question.refAnswers = settings.isShowAnswer ? methods.getRefAnswer(question) : undefined;

						//解题思路
						question.refThinking = settings.isShowThinking ? methods.getRefThinking(question) : undefined;
						question.setAnswerHanlder = function (data, event, other, index) {
							question.qtype == 1 && methods.wanxingEventHandler(question.version_id);
						};

						question.optionClass = function (data) {
							var isChecked = $.grep(question.userAnswer.answers, function (n) { return n.content == data; }).length > 0;
							return question.userAnswer.assess == 1 && isChecked ? "chkRight" : isChecked ? "chkWrong" : "";
						};

						question.isComplete = function () {
							return question.userAnswer && question.userAnswer.answers.length > 0;
						};

						question.assessResult = function (index) {
							if (index != undefined) {
								return question.qtype == 4 || question.qtype == 5 || !question.userAnswer ? '<span class="num">{0}.</span>'.format(index + 1) :
                                       question.userAnswer.assess == 1 ? '<span class="right"></span>' :
                                       question.userAnswer.assess == 2 ? '<span class="wrong"></span>' :
                                       question.userAnswer.assess == 4 ? '<span class="ask"></span>' :
                                       question.userAnswer.assess == 3 ? '<span class="hRight"></span>' : "";
							} else {
								return question.qtype == 3 || question.qtype == 4 || question.qtype == 5 ? "" : methods.getCorrectStyle(question.version_id, question.userAnswer.assess, 1, question.question_ref_sorce == 0 ? undefined : question.userAnswer.score);
							}
						};

						/*作业批改相关代码开始*/
						question.isCorrectView = settings.isCorrectView;
						question.correctParams = { question: null, index: 1, assess: ko.observable(0), itemScore: ko.observable(0), isShow: ko.observable(0), maxScore: 0, lastMark: null };
						if (question.qtype == 3) {//填空题作业批改特殊处理
							question.tiankongCorrectParamsList = ko.observableArray();
							$(question.options).each(function (index, item) {
								var tiankongCorrectParams = { sid: index, question: null, index: 1, assess: ko.observable(0), itemScore: ko.observable(0), isShow: ko.observable(0), maxScore: 0, lastMark: null };
								tiankongCorrectParams.isShow.subscribe(function (currentCorrectParams) {
									var answer = undefined;
									answer = methods.getItemAnswer(question, currentCorrectParams.sid + 1);
									currentCorrectParams.assess(answer ? answer.assess : 2);
									currentCorrectParams.itemScore(answer ? answer.item_score : 0);
									currentCorrectParams.maxScore = methods.correctScore(question, currentCorrectParams.index);
								});
								question.tiankongCorrectParamsList.push(tiankongCorrectParams);
							});
						}
						question.correctParams.isShow.subscribe(function (currentCorrectParams) {
							var answer = undefined;
							if (question.qtype == 3) {//填空题作业批改特殊处理
								answer = methods.getItemAnswer(question, currentCorrectParams.sid);
							} else {
								answer = methods.getItemAnswer(question, 1);
							}
							//var answer = methods.getItemAnswer(question, question.qtype == 3 ? correctParams.index : 1);//question.correctParams.question
							currentCorrectParams.assess(answer ? answer.assess : 2);
							currentCorrectParams.itemScore(answer ? answer.item_score : 0);
							currentCorrectParams.maxScore = methods.correctScore(question, currentCorrectParams.index);
						});

						question.setCorrectScore = function (type, id, data, event) {
							var currentCorrectParams = null;
							if (question.qtype == 3) {//填空题作业批改特殊处理
								currentCorrectParams = data;
							} else {
								currentCorrectParams = question.correctParams;
							}
							currentCorrectParams.assess(type);
							if (type == 1) {
								currentCorrectParams.itemScore(currentCorrectParams.maxScore);
							} else if (type == 2) {
								currentCorrectParams.itemScore(0);
							} else {
								currentCorrectParams.itemScore(parseFloat((currentCorrectParams.maxScore / 2).toFixed(2)));
							}
						};

						question.saveCorrect = function (data, event) {
							var currentCorrectParams = null;
							if (question.qtype == 3) {//填空题作业批改特殊处理
								currentCorrectParams = data;
							} else {
								currentCorrectParams = question.correctParams;
							}
							if (isNaN(currentCorrectParams.itemScore()) || currentCorrectParams.itemScore() < 0 || currentCorrectParams.itemScore().toString().trim().length == 0) {
								return $.error("请输入正确的分值");
							}
							if (currentCorrectParams.itemScore() > currentCorrectParams.maxScore) {
								return $.error("分值不能大于当前选项分值({0}分)".format(currentCorrectParams.maxScore));
							}

							var mark = currentCorrectParams.lastMark;
							var urlParam = {
								doId: settings.doId,
								versionId: question.version_id,
								sid: currentCorrectParams.index - 1,
								assess: currentCorrectParams.assess(),
								itemScore: parseFloat(currentCorrectParams.itemScore()),
								moduleId: settings.moduleId || "1f693f76-02f5-4a40-861d-a8503df5183f"
							};

							ciwong.ajax.postJSON("/question/SaveCorrect?" + $.param(urlParam), undefined, function (data) {
								methods.setItemAnswer(question, currentCorrectParams.index, urlParam.assess, urlParam.itemScore);
								methods.setCorrectHtmlHandler(mark, urlParam.assess, urlParam.itemScore, data);
								//每次提交批改后回调android或ios客户端的方法并传递其所需的分数等参数
								/* **作业批改现只支持在PC端进行  所以不需要实时提交Android**
                                var agentInfo = window.navigator.userAgent;
                                if (agentInfo.indexOf("Android") > -1) {
                                    //window.WebLoad.getResultScore(parseFloat(data)); //获取试卷分数或者听说模考的批改分数float score;
                                } else if (agentInfo.indexOf("iPhone") > -1 || agentInfo.indexOf("iPad") > -1 || agentInfo.indexOf("iPod") > -1) {
                                    //var url = "ciwong_jingsai://" + data; //iphone调用     
                                    //document.location.href = url; //iPhone调用 
                                }*/
							});
						};
						/*作业批改相关代码结束*/
						question.children && question.children.length > 0 && questionHandler(question.children);

					});
				}
				questionHandler(newQuesions);
			});

			model.sortNoTemplate = settings.sortNoTemplate || "scoreTemplate";

			model.afterRender = function () {
				methods.attachmentLoadingHandler();
			};

			model.wanxingAfterRender = function () {
				methods.yuedulijieEventHandler();
			};

			settings.dataSource && model.list(settings.dataSource);

			methods.initHtmlEventHandler();
		},
		correctView: function (settingOptions) {
			settingOptions.isCorrectView = true;

			var model = new viewModel.resultView(settingOptions);

			model.afterRender = function () {
				methods.attachmentLoadingHandler();
			};
			return model;
		}
	};

	exports.viewModel = viewModel;
	exports.moduleId = "008a020d-72c6-4df5-ba6c-73086b8db022";

	(function (ciwong, window) {

		var styleTag = document.createElement("link");
		styleTag.setAttribute('type', 'text/css');
		styleTag.setAttribute('rel', 'stylesheet');
		styleTag.setAttribute('href', "/Content/Mobile/css/question.css");
		$("head")[0].appendChild(styleTag);

		/*
			以下为当前viewModel所用的模板以及部分函数
		*/
		ciwong.koTemplateEngine.add("questionListTemplate", '\
		<!--ko if: $data -->\
			<!--ko template { name: "questionTemplate", foreach: list, afterRender: afterRender } --><!--/ko-->\
        <!--/ko-->');
		ciwong.koTemplateEngine.add("questionTemplate", '\
        <article class="work-quesCon">\
            <!--ko template: $parent.sortNoTemplate -->\
            <!--/ko-->\
            <!--ko template: templateName -->\
            <!--/ko-->\
            <!-- ko if:isCorrectView -->\
                <!-- ko if:qtype != 3 && qtype != 4 && qtype != 5-->\
                <div id="manual-mark" class="manual-mark cf" data-bind="template:{name:\'corretHtmlTemplate\'}"></div>\
                <!-- /ko -->\
                <!-- ko if:qtype == 3-->\
                    <!-- ko foreach:tiankongCorrectParamsList -->\
                        <br/><div id="manual-mark" class="manual-mark cf" data-bind="template:{name:\'tiankongCorretHtmlTemplate\'}"></div>\
                    <!-- /ko -->\
                <!-- /ko -->\
            <!-- /ko -->\
            <!-- ko if:qtype != 5 && qtype != 4 -->\
                <div data-bind="template:{name:\'questionAnswer\'}"></div>\
            <!-- /ko -->\
        </article>');

		/**显示答案和解题思路**/
		ciwong.koTemplateEngine.add("questionAnswer", '\
               	<!-- ko if: refAnswers -->\
                <div  class="answerBox mt10">\
        	        <div class="answer mt10">\
        		        <p class="tit">参考答案：</p>\
        		        <!--ko foreach: refAnswers -->\
        			        <div class="cf" data-bind="html:\'(\'+ ($index() + 1) +\')、&nbsp;\' + $data"></div>\
        		        <!--/ko-->\
        	        </div>\
                    <!-- ko if: refThinking && refThinking.length > 0 && refThinking!= null -->\
        		    <div class="answer mt10">\
        			    <p class="tit" data-bind="html:\'解题思路:\'+ $data.refThinking"></p>\
        		    </div>\
                <!--/ko-->\
                </div>\
                <!-- /ko -->');

		ciwong.koTemplateEngine.add("sortNoTemplate", '\
        <div class="work-tBar">\
            <span class="num" data-bind="text: $index() + 1 + \'.\'">&nbsp;</span>\
            <p data-bind="visible:question_ref_sorce > 0, text:question_ref_sorce + \'分\'"></p>\
        </div>');
		ciwong.koTemplateEngine.add("scoreTemplate", '\
        <div class="work-tBar">\
            <span class="num" data-bind="text: $index() + 1 + \'.\'">&nbsp;</span>\
            <p data-bind="visible:question_ref_sorce > 0, text: question_ref_sorce + \'分\'"></p>\
        </div>\
        <div class="ml15" data-bind="html:assessResult()"></div>');
		ciwong.koTemplateEngine.add("noScoreTemplate", '\
        <div class="work-tBar" data-bind="html:assessResult($index())">\
        </div>');
		ciwong.koTemplateEngine.add("sonquestionTemplate", '\
        <article>\
            <i class="qesNum" data-bind="text: \'Q\'+ ($index() + 1)"></i>\
            <!-- ko if:qtype == 6 -->\
                <div class="ml15" data-bind="html:assessResult()"></div>\
            <!-- /ko -->\
            <div data-bind="template: { name: templateName }"></div>\
<div data-bind="template:{name:\'questionAnswer\'}"></div>\
        </article>\
        <!-- ko if:isCorrectView -->\
            <!-- ko if:qtype != 3-->\
            <div id="manual-mark" class="manual-mark cf" data-bind="template:{name:\'corretHtmlTemplate\'}"></div>\
            <!-- /ko -->\
            <!-- ko if:qtype == 3-->\
                <!-- ko foreach:tiankongCorrectParamsList -->\
                    <br/><div id="manual-mark" class="manual-mark cf" data-bind="template:{name:\'tiankongCorretHtmlTemplate\'}"></div>\
                <!-- /ko -->\
            <!-- /ko -->\
        <!-- /ko -->');
		ciwong.koTemplateEngine.add("corretHtmlTemplate", '\
          <p class="fl">该题回答：</p>\
          <span class="mark fl">\
                <em index=1 data-bind="click: function(data,event){ setCorrectScore(1,version_id,data,event) }, css:{ \'curr\': correctParams.assess() == 1 }"><a href="javascript:;" class="mark-right"></a></em><i class="mark-line"></i>\
                <em index=1 data-bind="click: function(data,event){ setCorrectScore(3,version_id,data,event) }, css:{ \'curr\': correctParams.assess() == 3 }"><a href="javascript:;" class="mark-hRight"></a></em><i class="mark-line"></i>\
                <em index=1 data-bind="click: function(data,event){ setCorrectScore(2,version_id,data,event) }, css:{ \'curr\': correctParams.assess() == 2 }"><a href="javascript:;" class="mark-miss"></a></em>\
          </span>\
          <p class="fl">\
                得分：<span><input type="text" maxlength="5" class="vm inp-score" data-bind="value:correctParams.itemScore"><input type="button" class="vm btn-confirm" data-bind="click: saveCorrect" value="确定"></span>\
          </p>');
		ciwong.koTemplateEngine.add("danxuanTemplate", '\
        <div class="work-t cf" data-bind="html: stem"></div>\
        <div class="work-con">\
            <ul class="list" data-bind="foreach: options">\
                <li data-bind="click: $parent.setAnswerHanlder, css: $parent.optionClass(id)">\
                    <span class="icon"><i class="radio" data-bind="text: ciwong.englishLetter[$index()]"></i></span>\
                    <p data-bind="html: stem"></p>\
                </li>\
            </ul>\
        </div>');
		ciwong.koTemplateEngine.add("duoxuanTemplate", '\
        <div class="work-t cf" data-bind="html: stem"></div>\
        <div class="work-con">\
            <ul class="list" data-bind="foreach: options">\
                <li data-bind="click: $parent.setAnswerHanlder, css: $parent.optionClass(id)">\
                    <span class="icon"><i class="checkbox" data-bind="text: ciwong.englishLetter[$index()]"></i></span>\
                    <p data-bind="html: stem"></p>\
                </li>\
            </ul>\
        </div>');
		ciwong.koTemplateEngine.add("tiankongTemplate", '\
        <div class="work-t cf" data-bind="html: stem">\
        </div>');
		ciwong.koTemplateEngine.add("tiankongCorretHtmlTemplate", '\
          <p class="fl" data-bind="html:\'(\'+ parseInt(sid+1) +\')、该题回答：\'"></p>\
          <span class="mark fl">\
                <em index=1 data-bind="click: function(data,event){ $parent.setCorrectScore(1,$parent.version_id + \'\'+ sid,data,event) }, css:{ \'curr\': assess() == 1 }"><a href="javascript:;" class="mark-right"></a></em><i class="mark-line"></i>\
                <em index=1 data-bind="click: function(data,event){ $parent.setCorrectScore(3,$parent.version_id + \'\'+ sid,data,event) }, css:{ \'curr\': assess() == 3 }"><a href="javascript:;" class="mark-hRight"></a></em><i class="mark-line"></i>\
                <em index=1 data-bind="click: function(data,event){ $parent.setCorrectScore(2,$parent.version_id + \'\'+ sid,data,event) }, css:{ \'curr\': assess() == 2 }"><a href="javascript:;" class="mark-miss"></a></em>\
          </span>\
          <p class="fl">\
                得分：<span><input type="text" maxlength="5" class="vm inp-score" data-bind="value: itemScore"><input type="button" class="vm btn-confirm" data-bind="click: $parent.saveCorrect" value="确定"></span>\
          </p>');
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
		ciwong.koTemplateEngine.add("yuedulijieTemplate", '\
        <div class="work-t read-com ovh cf" data-bind="html: stem"></div>\
        <div class="read-con" data-bind="template: { name: \'sonquestionTemplate\', foreach: children, afterRender:$parent.wanxingAfterRender }"></div>\
        </div>');
		ciwong.koTemplateEngine.add("wanxingTemplate", '\
        <div class="work-t gestalt cf" data-bind="html: stem"></div>\
        <div class="read-con write-con" data-bind="template: { name: \'sonquestionTemplate\', foreach: children, afterRender:$parent.wanxingAfterRender }"></div>\
        </div>');

		window.DrawImage = function (ImgD, iwidth, iheight) {
			var iwidth = 600, iheight = 2000;
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
		};

		window.convertVideo = function (userAnswer) {
			var html = '';
			if (userAnswer && userAnswer.answers.length > 0 && userAnswer.answers[0].content != "") {
				var id = Math.random().toString().replace("0.", "");
				html += '<div>我的录音答案：';
				if (isPc) {
					html += '<object type="application/x-shockwave-flash" data="/Content/Flash/player.swf" id="{0}" height="24" width="185">'.format(id);
					html += '<param name="movie" value="/Content/Flash/player.swf">';
					html += '<param name="FlashVars" value="titles=loading&amp;playerID={0}&amp;bg=0xf3f6f8&amp;leftbg=0xcef4da&amp;lefticon=0x647062&amp;voltrack:0xacb8aa&amp;volslider:0xc4d0c3&amp;rightbg=0x61d26a&amp;rightbghover=0x84e45e&amp;righticon=0xF2F2F2&amp;righticonhover=0xFFFFFF&amp;text=0x408d05&amp;track=0xFFFFFF&amp;border=0xffffff&amp;loader=0xc7e795&amp;tracker=0x6dae05&amp;soundFile={1}">'.format(id, userAnswer.answers[0].content);
					html += '<param name="quality" value="high">';
					html += '<param name="menu" value="false">';
					html += '<param name="wmode" value="transparent">';
					html += '</object>';
				} else {
					html += '<audio src="{0}{1}" preload=""></audio>'.format(userAnswer.answers[0].content, userAnswer.answers[0].content.indexOf("http://img1.") > -1 ? "?fileType=mp3" : "");
					//html += '<audio src="{0}" preload="none"></audio>'.format(userAnswer.answers[0].content);
					//html5音频控件样式太丑
					//html += '<audio controls="controls">';
					//html += '<source src="{0}" type="audio/mpeg">'.format(attachment.file_url);
					//html += '<source src="{0}" type="audio/wav">'.format(attachment.file_url);
					//html += '</audio>';
				}
				html += '</div>';
			}
			return html;
		};
	})(ciwong, window);
});
