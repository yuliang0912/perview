/**同步讲练**/

define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), questionCorrect = require("correct"), ko = require("ko"), question = require("question"), dialog = require("dialog");

    /*公共方法提炼*/
    var methods = {
        clearCurr: function (id) {
            $("div.trainBoxBar a,.boxMain ul li").removeClass("curr");
            $("#" + id).addClass("curr");
        },
        getQuestionItemList: function (moduleId, versionIds) {//获取试题及下面的选项
            var qdata;
            ciwong.ajax.getJSON("/resource/getlist", { versionIds: versionIds, moduleId: moduleId }, function (data) {
                qdata = data;
            }, null, false);
            return qdata;
        },
        findUserAnswerById: function (userAnswers, versionId) {
            var result = $.grep(userAnswers, function (m) { return m.version_id == versionId; })[0];
            if (result && !$.isArray(result.answers)) {
                result.answers = JSON.parse(result.answers);
            }
            return result;
        },
        setUserAnswer: function (answer, questions, type) {
            if (type == 1) {//练习模式              
                questions.userAnswer = methods.findUserAnswerById(answer, questions.version_id);
                if (questions.children != "") {
                    $.each(questions.children, function () {
                        this.userAnswer = methods.findUserAnswerById(answer, this.version_id);
                        this.children && this.children.length > 0 && methods.setUserAnswer(answer, this, type);
                    });
                }
            }
            else if (type > 1)//做作业
            {
                var question_versions = $.map(questions.list, function (n) { return n.version_id; }).join(",");
                var data = methods.getQuestionItemList(questions.module_id, question_versions);
                $.each(data, function () {
                    this.userAnswer = methods.findUserAnswerById(answer, this.version_id);//获取该试题的答案               
                    this.children && this.children.length > 0 && methods.setUserAnswer(answer, this, 1);
                });
                questions.list = data;
            }
        },
        //getUserAnswer: function (doId, answerType) {
        //    var userAnswer;
        //    ciwong.ajax.getJSON("/work/GetAnswer", { doId: doId, answerType: answerType }, function (data) {
        //        userAnswer = data;
        //    }, null, false);
        //    return userAnswer;
        //}
    };

    var ksDataCache = {};//课时，专项 每个缓存
    var viewModel = {
        preView: function (settingOptions) {
            var model = this, settings = settingOptions || {};
            model.trainmodel = ko.observable(settings.trainmodel);//练习模式 先讲后练 先练后讲 边讲边练
            model.workmodel = ko.observable(settings.workmodel);//工作模式 ：自主练习 做作业
            model.viewtype = ko.observable(settings.viewtype);//视图形式 试题还是结果视图
            //model.btnTip = ko.observable("做练习");
            //model.TrainModelList = ko.observableArray(methods.defaultTrainModes()); //无须切换 

            model.trainmodelChange = function (data, event) {
            	var currid = (event.currentTarget || event.srcElement).version_id;//当前点击的li    
                $("div.trainNavBar li").removeClass("curr");
                $("#" + currid).addClass("curr");
                model.trainmodel(data.version_id);
                model.viewtype(data.vt);
                $("[id^=wanxing_]").hide();
            };

            model.CurrCont = ko.observable("");
            model.questionModules = ko.observable();
            model.leftcss = ko.observable("");
            //做练习试卷对象
            model.doPract = ko.observable();
            model.papers = ko.observable();
            model.papers.subscribe(function (data) {
                if (model.workmodel() == 1)//自主练习模式
                {
                    data && data.parts && $.each(data.parts, function (index) {
                        $.each(this.list, function (i, item) {
                            var self = this;
                            this.getQuestion = function (value) {
                                model.userAnswers([]);
                                ciwong.ajax.getJSON("/resource/get", { versionId: item.version_id, moduleId: item.module_id }, function (data) {
                                    if (data.version_id == item.version_id) {
                                        var versions = $.map(data.parts[0].list, function (n) { return n.version_id; }).join(",");
                                        model.questionModules(new question.viewModel.preView({ versions: versions }));//试题视图                                                                                          
                                        model.CurrCont(data.content);
                                        methods.clearCurr(value.version_id);
                                        //model.trainmodel() == 1 && model.viewtype(0);
                                        //model.trainmodel() == 2 && model.viewtype(1);
                                        //model.btnTip("做练习");
                                    }
                                })
                            };
                        	//i == 0 && index == 0 && this.getQuestion(self);
                        });
                    });
                }
                else if (model.workmodel() == 2)//做作业模式
                {
                    model.CurrCont(data.content);
                    var question_versions = $.map(data.parts[0].list, function (n) { return n.version_id; }).join(",");
                    model.questionModules(new question.viewModel.preView({ versions: question_versions }));
                    model.doPract(data.parts[0]);
                }
                else if (model.workmodel() == 3)//结果
                {
                    model.CurrCont(data.content);
                    model.questionModules(new question.viewModel.resultView({ dataSource: data.parts[0].list, sortNoTemplate: "noScoreTemplate", isShowAnswer: true, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));
                }
            });
            model.btnDoWork = function (trainmodel, vtype) {
                model.trainmodel(trainmodel);
                model.viewtype(vtype);
            };

            model.afterRender = function () {
            	settings.afterRender && settings.afterRender();
            }

            /**再练一次**/
            //model.btnDoWorkAgain = function (trainmodel, vtype) {
            //    model.trainmodel(trainmodel);
            //    model.viewtype(vtype);
            //    model.userAnswers([]);
            //    model.btnTip("做练习");
            //    model.questionModules(new question.viewModel.preView({ dataSource: model.questionModules().dataSource }));//试题视图     
            //};

            model.submitWork = function () {
                var userAnswer = model.questionModules().getAnswers();//获取到用户的答案
                if (userAnswer.length > 0) {
                    if (model.workmodel() == 1)//做练习
                    {
                        model.viewtype(2);
                        model.btnTip("查看练习");
                        //试题批改方法(用户个人答案,试题列表)
                        var content = questionCorrect.question.correct(model.questionModules().getAnswers(), model.questionModules().list());
                        model.userAnswers(content);

                        //userAnswer && ciwong.ajax.postJSON("/SyncTrain/Correct", { content: JSON.stringify(userAnswer), type: 1 }, function (data) { model.userAnswers(data); });
                    }
                    else if (model.workmodel() == 2)//做作业
                    {
                        model.btnTip("查看练习");
                        settings.submitWork && settings.submitWork.call(model);
                    }
                }
                else $.alert("请您先填写答案再提交");
            };
            model.userAnswers = ko.observableArray();
            model.userAnswers.subscribe(function (answers) {
                if (model.userAnswers().length == 0) {
                    return;
                }
                if (model.workmodel() == 1 && answers)//自主练习
                {
                    var data = model.questionModules().dataSource;
                    data && $.each(data, function () {
                        methods.setUserAnswer(answers, this, model.workmodel());
                    });
                    data && model.questionModules(new question.viewModel.resultView({ dataSource: data, sortNoTemplate: "noScoreTemplate", isShowAnswer: true, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));
                }
                else if (model.workmodel() == 2)//作业模式
                {
                    var userAnswer = methods.getUserAnswer(answers, 1);
                    $.each(model.papers().parts, function (i, item) {
                        methods.setUserAnswer(userAnswer, item, model.workmodel());
                    });
                    model.questionModules(new question.viewModel.resultView({ dataSource: model.papers().parts[0].list, sortNoTemplate: "noScoreTemplate", isShowAnswer: true, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));
                }
            });

            /**获取同步讲练数据信息**/
            settings.workmodel == 1 && settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "2bea5300-972e-494f-b730-6cbc05f0a717" }, function (data) {
                if (data.ret > 0) {
                    $.error(data.msg);
                }
                else {
                    model.papers(data);
                }

            }).done(function () { ciwong.colseLoading(); });//异步获取自主练习的数据

            settings.workmodel > 1 && settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: settings.modeid }, function (paperData) {
                if (settings.doId != undefined) {
                    var userAnswer = methods.getUserAnswer(settings.doId, settings.answerType);
                    $.each(paperData.parts, function () {
                        methods.setUserAnswer(userAnswer, this, model.workmodel());
                    });
                }
                model.papers(paperData);
            }).done(function () {
                ciwong.colseLoading();
            });//获取做作业的数据及答案
        }
    };

    exports.viewModel = viewModel;
    exports.moduleId = "2bea5300-972e-494f-b730-6cbc05f0a717";
});