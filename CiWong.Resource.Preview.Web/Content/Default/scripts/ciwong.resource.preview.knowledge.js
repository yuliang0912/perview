define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko"), question = require("question");

    var methods = {
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
        }
    };

    var viewModel = {
        preView: function (settingOptions) {
            var model = this,
               settings = settingOptions || {};

            model.viewType = ko.observable(1);

            model.knowledge = ko.observable();

            model.knowledge.subscribe(function (newknowledge) {
                if (model.viewType() == 1) {
                    newknowledge.questions = ko.observable(new question.viewModel.preView({ versions: newknowledge.questions.join(",") }));
                } else {
                    newknowledge.questions = ko.observable(new question.viewModel.resultView({ dataSource: newknowledge.questions, isShowAnswer: true, isShowThinking: settings.isShowThinking, isShowQuestionVideo: settings.isShowQuestionVideo }));
                }
            });

            model.correctQuestion = function () {
                model.viewType() == 1 && ciwong.ajax.postJSON("/question/Correct", { content: JSON.stringify(model.knowledge().questions().getAnswers()) }, function (data) {
                    model.knowledge().questions = model.knowledge().questions().dataSource;
                    methods.setUserAnswer(data, model.knowledge().questions);
                    model.viewType(2);
                    model.knowledge(model.knowledge());
                });
            };

            ko.computed(function () {
                model.knowledge() && $("#_ebook_top hgroup h1").text(model.knowledge().name);
            });

            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "c4fab15f-a0a6-45b5-b116-b0f087cb7119" }, function (data) {
                var knowledge = { name: data.name, content: data.content };
                if (data.parts.length > 0 && data.parts[0].module_id == "008a020d-72c6-4df5-ba6c-73086b8db022") {
                    knowledge.questions = $.map(data.parts[0].list, function (m) { return m.version_id });
                }
                model.knowledge(knowledge);
            }).done(function () {
                ciwong.colseLoading();
            });
        }
    }

    exports.viewModel = viewModel;
    exports.moduleId = "c4fab15f-a0a6-45b5-b116-b0f087cb7119";
});