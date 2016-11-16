define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), questionCorrect = require("correct"), ko = require("ko"), question = require("question"), dialog = require("dialog");

    var methods = {
        convertToChineseNum: function (num) {
            num = num.toString();
            var ary0 = ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九"],
                ary1 = ["", "十", "百", "千"],
                ary2 = ["", "万", "亿", "兆"];

            function strrev() {
                var ary = []
                for (var i = num.length; i >= 0; i--) {
                    var n = num.charAt(i);
                    if (n != "") {
                        ary.push(n);
                    }
                }
                return ary.join("");
            }
            var ary = strrev(), zero = "", newary = "", i4 = -1;
            for (var i = 0; i < ary.length; i++) {
                if (i % 4 == 0) {
                    i4++;
                    newary = ary2[i4] + newary;
                    zero = "";
                }
                if (ary.charAt(i) == '0') {
                    switch (i % 4) {
                        case 0:
                            break;
                        default:
                            if (ary.charAt(i - 1) != '0') {
                                zero = "零"
                            }
                            break;
                    }
                    newary = zero + newary;
                    zero = '';
                }
                else {
                    newary = ary0[parseInt(ary.charAt(i))] + ary1[i % 4] + newary;
                }
            }
            if (newary.indexOf("一十") == 0 || newary.indexOf("零") == 0) {
                newary = newary.substr(1);
            }
            return newary;
        },
        findUserAnswer: function (userAnswers, versionId, moluleId) {
            var result = $.grep(userAnswers, function (m) { return m.version_id == versionId; })[0];
            if (result && !$.isArray(result.answers)) {
                if (moluleId == "e9430760-9f2e-4256-af76-3bd8980a9de4") {
                    result.answers = $.map(JSON.parse(result.answers), function (n, i) {
                        var content = n.audio_url != "" ? n.audio_url : n.option_id != "" ? n.option_id : n.blank_content;
                        return { assess: result.assess, sid: i, content: content, item_score: n.item_score };
                    });
                } else {
                    result.answers = JSON.parse(result.answers);
                }
            }
            return result;
        },
        setUserAnswer: function (data, questions, moduleId) {
            questions && $.each(questions.children, function () {
                this.userAnswer = methods.findUserAnswer(data, this.version_id, moduleId);
                this.children && this.children.length > 0 && methods.setUserAnswer(data, this, moduleId);
            });
        },
        getUserAnswer: function (doId, answerType) {
            var userAnswer;
            ciwong.ajax.getJSON("/work/GetAnswer", { doId: doId, answerType: answerType }, function (data) {
                userAnswer = data;
            }, null, false).done(function () {
                ciwong.colseLoading();
            });
            return userAnswer;
        },
        initCorrectHtmlEventHandler: function () {
            var manual_mark = $("#manual-mark"), index = 0;
            $('article.work-quesCon span.manual').click(function () {
                /*初始化隐藏评分div  避免上一个没有关闭*/
                manual_mark.hide();

                var index = parseInt($(this).attr("index"));
                var question = ko.contextFor(this).$data;

                if (question.qtype == 3 && $(this).parent().parent().parent().find("li").length > 0) {
                    if (question.options.length > 1) {
                        manual_mark.hide(); //manual_mark.toggle();
                        return;
                    }
                }
                manual_mark.stop().show();
                var x = $(this).offset().left;
                var y = $(this).offset().top + 25;
                var winW = $(window).width()

                if (winW - x > 450) {
                    manual_mark.css({ left: x + "px", top: y + "px" });
                } else {
                    manual_mark.css({ left: x - 410 + "px", top: y + "px" });
                }
                if (question.qtype == 4) {
                    $.each(question.children, function (i) {
                        if (i == index - 1) {
                            index = 1;
                            question = question.children[i];
                            return false;
                        }
                    });
                }
                correctParams.index = index;
                correctParams.question = question;
                correctParams.isShow(Math.random());
                correctParams.lastMark = this;
            });
        },
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
            var answer = $.grep(question.userAnswer.answers, function (item) { return item.sid == index - 1 })[0];
            if (answer && !answer.item_score) {
                answer.item_score = answer.assess == 1 ? methods.correctScore(question, index) : 0;
            }
            return answer;
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
            $("#_totalScore").html(totalScore);
            $("#manual-mark").hide();

            $(mark).html('{0}<em class="red">({1})</em></span>'.format(assessIco, score));
        },
        isSubjective: function (data) {
            var count = 0;
            $.each(data, function (i, item) {
                var answersList;
                //初始化的时候获取到的答案列表是对象数组，但是提交的时候获取到的是字符串类型的数组  so try catch
                try { answersList = JSON.parse(item.answers); } catch (ex) { answersList = item.answers; }
                $.each(answersList, function (j, answersModel) {
                    answersModel.assess == 4 && count++;
                });
            });
            return count;
        }
    };

    var correctParams = { question: null, index: 1, assess: ko.observable(0), itemScore: ko.observable(0), isShow: ko.observable(0), maxScore: 0, lastMark: null };

    var viewModel = {
        preView: function (settingOptions) {
            var model = this,
                settings = settingOptions || {};

            model.lastDataSource = undefined;
            model.allSingleQuestions = [];

            model.viewType = ko.observable(1);//1:预览模式 2:结果模式

            model.paper = ko.observable();
            model.paper.subscribe(function (newPaper) {
                model.lastDataSource = ko.toJS(newPaper);

                newPaper.parts && $.each(newPaper.parts, function (i) {
                    this.module_type_name = "第{0}题 {1}".format(methods.convertToChineseNum(i + 1), this.module_type_name);
                    if (model.viewType() == 1) {
                        this.children = ko.observable(new question.viewModel.preView({ dataSource: this.children }));
                    } else {
                        this.children = ko.observable(new question.viewModel.resultView({ dataSource: this.children, isShowAnswer: settings.isShowAnswer, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));
                    }
                });

                if (model.allSingleQuestions.length == 0) {
                    var allQuestions = [];
                    $.each(model.lastDataSource.parts, function () { allQuestions = allQuestions.concat(this.children) });
                    $.each(allQuestions, function (i, item) {
                        if (item.children.length > 0) {
                            model.allSingleQuestions = model.allSingleQuestions.concat(item.children)
                        } else {
                            model.allSingleQuestions.push(item);
                        }
                    });
                }
            });

            model.afterRender = function () {
                settings.afterRender && settings.afterRender.call(model);
            }

            model.getUserAnswer = function () {
                var userAnswer = [];
                if (model.viewType() == 1) {
                    $.each(model.paper().parts, function () {
                        $.merge(userAnswer, this.children().getAnswers());
                    });
                }
                return userAnswer;
            };

            model.correctQuestion = function () {
                //model.viewType() == 1 && ciwong.ajax.postJSON("/question/Correct", { content: JSON.stringify(model.getUserAnswer()) }, function (data) {
                //    $.each(model.lastDataSource.parts, function (i, item) {
                //        methods.setUserAnswer(data, item);
                //    });
                //    model.viewType(2);
                //    model.paper(model.lastDataSource);
                //    settings.correctComplete && settings.correctComplete(data);
                //}, null, null, true, function () { ciwong.ajax.isPreventRequest = true; });

                //试题批改方法(用户个人答案,试题列表)
                var data = questionCorrect.question.correct(model.getUserAnswer(), model.lastDataSource.parts);

                $.each(model.lastDataSource.parts, function (i, item) {
                    methods.setUserAnswer(data, item);
                });
                model.viewType(2);
                model.paper(model.lastDataSource);
                settings.correctComplete && settings.correctComplete(data);
            }

            model.submitWork = function () {
                if (model.viewType() == 1 && settings.submitWork) {
                    settings.submitWork.call(model);
                }
            }

            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: settings.moduleId || "1f693f76-02f5-4a40-861d-a8503df5183f" }, model.paper).done(function () {
                ciwong.colseLoading();
            });
        },
        resultView: function (settingOptions) {
            var model = this,
                settings = settingOptions || {};

            model.viewType = ko.observable(2);//1:预览模式 2:结果模式

            model.paper = ko.observable();
            model.paper.subscribe(function (newPaper) {
                newPaper.parts && $.each(newPaper.parts, function (i) {
                    this.module_type_name = "第{0}题 {1}".format(methods.convertToChineseNum(i + 1), this.module_type_name);
                    this.children = ko.observable(new question.viewModel.resultView({ dataSource: this.children, isShowAnswer: settings.isShowAnswer, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));
                });
            });

            model.afterRender = function () {
                $("div.read-step li:first").addClass("curr");
                settings.afterRender && settings.afterRender.call(model);
            }

            settings.id && ciwong.ajax.getJSON("/resource/get", { moduleId: settings.moduleId || "1f693f76-02f5-4a40-861d-a8503df5183f", versionId: settings.id }, function (paperData) {
                var userAnswer = methods.getUserAnswer(settings.doId, settings.answerType || 1);
                $.each(paperData.parts, function (i, item) {
                    methods.setUserAnswer(userAnswer, item, settings.moduleId);
                });
                model.paper(paperData);
            }, true, true);
        },
        correctView: function (settingOptions) {
            var model = this,
                settings = settingOptions || {};

            model.paper = ko.observable();
            model.paper.subscribe(function (newPaper) {
                newPaper.parts && $.each(newPaper.parts, function (i) {
                    this.module_type_name = "第{0}题 {1}".format(methods.convertToChineseNum(i + 1), this.module_type_name);
                    this.children = ko.observable(new question.viewModel.correctView({ dataSource: this.children, isShowAnswer: settings.isShowAnswer, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));
                });
            });

            model.subjective = ko.observableArray(0);

            model.lastCorrectQuestion = ko.observable(correctParams);

            correctParams.isShow.subscribe(function () {
                var answer = methods.getItemAnswer(correctParams.question, correctParams.index);

                correctParams.assess(answer ? answer.assess : 2);
                correctParams.itemScore(answer ? answer.item_score : 0);
                correctParams.maxScore = methods.correctScore(correctParams.question, correctParams.index);
            });

            model.setCorrectScore = function (type) {
                correctParams.assess(type);
                if (type == 1) {
                    correctParams.itemScore(correctParams.maxScore);
                } else if (type == 2) {
                    correctParams.itemScore(0);
                } else {
                    correctParams.itemScore(parseFloat((correctParams.maxScore / 2).toFixed(2)));
                }
            }

            model.saveCorrect = function () {

                if (isNaN(correctParams.itemScore()) || correctParams.itemScore() < 0 || correctParams.itemScore().toString().trim().length == 0) {
                    return $.error("请输入正确的分值");
                }
                if (correctParams.itemScore() > correctParams.maxScore) {
                    return $.error("分值不能大于当前选项分值({0}分)".format(correctParams.maxScore));
                }

                var mark = correctParams.lastMark;
                var urlParam = {
                    doId: settings.doId,
                    versionId: correctParams.question.version_id,
                    sid: correctParams.index - 1,
                    assess: correctParams.assess(),
                    itemScore: parseFloat(correctParams.itemScore()),
                    moduleId: settings.moduleId || "1f693f76-02f5-4a40-861d-a8503df5183f"
                };

                ciwong.ajax.postJSON("/question/SaveCorrect?" + $.param(urlParam), undefined, function (data) {
                    methods.setItemAnswer(correctParams.question, correctParams.index, urlParam.assess, urlParam.itemScore);
                    methods.setCorrectHtmlHandler(mark, urlParam.assess, urlParam.itemScore, data);

                    //post 异步请求会有数据延迟  所以需要将数据修改放到post请求里面
                    var userAnswer = methods.getUserAnswer(settings.doId, settings.answerType || 1);
                    model.subjective(methods.isSubjective(userAnswer));

                });
            }

            model.afterRender = function () {
                $("div.read-step li:first").addClass("curr");
                methods.initCorrectHtmlEventHandler();
                settings.afterRender && settings.afterRender.call(model);
            }

            settings.id && ciwong.ajax.getJSON("/resource/get", { moduleId: settings.moduleId || "1f693f76-02f5-4a40-861d-a8503df5183f", versionId: settings.id }, function (paperData) {
                var userAnswer = methods.getUserAnswer(settings.doId, settings.answerType || 1);
                $.each(paperData.parts, function (i, item) {
                    methods.setUserAnswer(userAnswer, item, settings.moduleId);
                });

                model.subjective(methods.isSubjective(userAnswer));

                model.paper(paperData);
            }, true, true);
        }
    };

    exports.viewModel = viewModel;
    exports.moduleId = "1f693f76-02f5-4a40-861d-a8503df5183f";

    /*
    以下为当前viewModel所用的模板以及部分函数
    */
    ciwong.koTemplateEngine.add("paperTemplate", '\
        <!--ko foreach: parts -->\
        <section class="work-quesBox">\
            <nav class="work-quesT">\
                <h3 class="t"><i></i><em data-bind="text:module_type_name.removeHtmlTag()"></em></h3>\
            </nav>\
            <!--ko template: { name: "questionListTemplate", data: children } -->\
            <!--/ko-->\
        </section>\
        <!-- /ko -->');
});