define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko"), question = require("question"), dialog = require("dialog");

    var methods = {
        initHtmlEventHandler: function () {
            $("#_step_change li").live("click", function () {
                $("#step_" + $(this).index()).show().siblings().hide();
                $(this).addClass("curr").siblings().removeClass("curr");
            });
        },
        findUserAnswer: function (userAnswers, versionId) {
            var result = $.grep(userAnswers, function (m) { return m.version_id == versionId; })[0];
            if (result && !$.isArray(result.answers)) {
                result.answers = JSON.parse(result.answers);
            }
            return result;
        },
        setUserAnswer: function (data, questions) {
            questions && $.each(questions, function () {
                this.userAnswer = methods.findUserAnswer(data, this.version_id);
                this.children.length > 0 && methods.setUserAnswer(data, this.children);
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
        }
    };

    var viewModel = {
        preView: function (settingOptions) {
            var model = this,
               settings = settingOptions || {};

            model.viewType = ko.observable(1);

            model.guidance = ko.observable();
            model.allSingleQuestions = ko.observable(0);
            var versionIds = [];
            model.guidance.subscribe(function (newGuidance) {
                $.each(newGuidance.stepList, function () {
                    if (model.viewType() == 1) {
                        versionIds.push(this.questions);
                        this.questions = ko.observable(new question.viewModel.preView({ versions: this.questions.join(",") }));
                    } else {
                        this.questions = ko.observable(new question.viewModel.resultView({ dataSource: this.questions, isShowAnswer: true, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));//解题思路 //解题视频
                    }
                });
                if (model.allSingleQuestions() == 0 && versionIds.length > 0) {
                    var allQuestions = [];
                    var ids = versionIds.join(",");
                    var count = 0;
                    ciwong.ajax.getJSON("/resource/getlist", { versionIds: ids.substring(0, ids.length - 1), moduleId: "008a020d-72c6-4df5-ba6c-73086b8db022" }, function (data) {
                        $.each(data, function (i, item) {
                            if (item.children.length > 0) {
                                count += item.children.length;
                            }
                            else {
                                count += 1;
                            }
                        })
                        model.allSingleQuestions(count);
                    }, null, false);

                }
            });

            model.afterRender = function () {
                $("div.read-step ul.tepList li:first").addClass("curr");
                settings.afterRender && settings.afterRender.call(model);
            }

            model.getUserAnswer = function () {
                var userAnswer = [];
                model.viewType() == 1 && $.each(model.guidance().stepList, function () {
                    $.merge(userAnswer, this.questions().getAnswers());
                });
                return userAnswer;
            };

            model.correctQuestion = function () {
                model.viewType() == 1 && ciwong.ajax.postJSON("/question/Correct", { content: JSON.stringify(model.getUserAnswer()) }, function (data) {
                    $.each(model.guidance().stepList, function (i, item) {
                        item.questions = item.questions().dataSource;
                        methods.setUserAnswer(data, item.questions);
                    });
                    model.viewType(2);
                    model.guidance(model.guidance());
                });
            }

            ko.computed(function () {
                model.guidance() && $("#_ebook_top hgroup h1").text(model.guidance().name);
            });

            model.submitWork = function () {
                if (model.viewType() == 1 && settings.submitWork) {
                    settings.submitWork.call(model);
                }
            }

            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "09aca375-7634-42a8-a39e-098fee65c341" }, function (data) {
                var stepList = $.map(data.items, function (n) {
                    var step = { name: n.name, content: n.content, questions: [] };
                    if (n.parts.length > 0 && n.parts[0].module_id == "008a020d-72c6-4df5-ba6c-73086b8db022" && n.parts[0].list.length > 0) {
                        step.questions = $.map(n.parts[0].list, function (m) { return m.version_id });
                    }
                    return step;
                });
                model.guidance({ name: data.name, stepList: stepList });
            }).done(function () {
                ciwong.colseLoading();
            });
            methods.initHtmlEventHandler();
        },

        resultView: function (settingOptions) {
            var model = this,
                settings = settingOptions || {};
            model.viewType = ko.observable(2);//1:预览模式 2:结果模式
            model.guidance = ko.observable();
            model.guidance.subscribe(function (newGuidance) {
                $.each(newGuidance.stepList, function () {
                    this.questions = ko.observable(new question.viewModel.resultView({ dataSource: this.questions, isShowAnswer: true, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));//解题思路
                });
            });

            model.afterRender = function () {
                $("div.read-step ul.tepList li:first").addClass("curr");
                settings.afterRender && settings.afterRender.call(model);
            }

            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "09aca375-7634-42a8-a39e-098fee65c341" }, function (data) {
                var userAnswer = methods.getUserAnswer(settings.doId, settings.answerType || 1);
                var stepList = $.map(data.items, function (n) {
                    var step = { name: n.name, content: n.content, questions: [] };
                    if (n.parts.length > 0 && n.parts[0].module_id == "008a020d-72c6-4df5-ba6c-73086b8db022" && n.parts[0].list.length > 0) {
                        var ids = $.map(n.parts[0].list, function (m) { return m.version_id }).join(',');
                        ciwong.ajax.getJSON("/resource/getlist", { versionIds: ids, moduleId: "008a020d-72c6-4df5-ba6c-73086b8db022" }, function (data) {
                            step.questions = data;
                            methods.setUserAnswer(userAnswer, data);
                        }, true, false);
                    }
                    return step;
                });
                model.guidance({ name: data.name, stepList: stepList });

            }, true, true);
            methods.initHtmlEventHandler();
        }
    }
    exports.viewModel = viewModel;
    exports.moduleId = "09aca375-7634-42a8-a39e-098fee65c341";
});