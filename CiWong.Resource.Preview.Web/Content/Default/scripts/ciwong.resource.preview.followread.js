define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko"), cwBrowerAtiveX = require("cwBrowerAtiveX");

    var defaultSettings = function () {
        return {
            UserInfo: { user_id: 0, user_name: "" },
            vip: 'false'
        };
    };

    //跟读视图模型
    var viewModel = {
        //自主练习视图 
        preView: function (settingOptions) {
            var model = this,
                settings = $.extend(true, defaultSettings(), settingOptions || {}),
                epDialog,
                epDialogViewModel;

            model.list = ko.observableArray();

            model.start = function (data, type) {
                !cwBrowerAtiveX.isInit && cwBrowerAtiveX.init();
                cwBrowerAtiveX.work(data.version_id, data.module_id == "a7527f97-14e6-44ef-bf73-3039033f128e" ? 3 : 4, 0, 0, 0, 0, 0, settings.UserInfo.user_id, settings.UserInfo.user_name, type, settings.vip.toLocaleLowerCase(), settings.packageId);
            }

            model.contentView = function (data) {
                if (!epDialogViewModel) {
                    epDialog = ciwong.epDialog.init("contentViewTemplate");
                    epDialogViewModel = new viewModel.epDialogView();
                    ko.applyBindings(epDialogViewModel, document.getElementById("_dialog"));
                    $("#completedRead").click(function () { epDialog.close() });
                }
                epDialogViewModel.list([]);
                epDialogViewModel.name(data.name);
                if (data.module_id == "a7527f97-14e6-44ef-bf73-3039033f128e" && $.isArray(data.list)) {
                    epDialogViewModel.viewType(1);
                    epDialogViewModel.list($.map(data.list, function (n) { return n.name }));
                } else {
                    ciwong.ajax.getJSON("/resource/get", { versionId: data.version_id, moduleId: "992a5055-e9d0-453f-ab40-666b4d7030bb" }, function (data) {
                        epDialogViewModel.viewType(2);
                        epDialogViewModel.list($.map(data.sections, function (n) {
                            return $.map(n.sentences, function (m) { return m.name }).join(" ");
                        }));
                    });
                }
                epDialog.open();
            }

            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "f0833ebe-6a8b-4cc1-a6b5-f4d47d93df35" }, function (data) {
                var resourceList = [];
                $.each(data.parts, function (i, item) {
                    if (item.id == "word") {
                        resourceList.push({ version_id: settings.id, module_id: "a7527f97-14e6-44ef-bf73-3039033f128e", name: "[单词表]", hidden: true, list: this.list });
                    }
                    else if (item.id == "text") {
                        $.each(item.list, function () {
                            resourceList.push({ version_id: this.version_id, module_id: this.module_id, name: "[课文]" + this.name, hidden: false, list: this.list });
                        });
                    }
                });
                $("#resourceName,#_ebook_top hgroup h1").html(data.name);
                model.list(resourceList);
            }).done(function () {
                $("#_followread_panel,#followread_content").show();
                $("#followread_loading").hide();
                ciwong.colseLoading();
            });
        },
        //做作业视图
        doView: function (settingOptions) {
            var model = this,
                settings = $.extend(true, defaultSettings(), settingOptions || {})

            model.start = function () {
                !cwBrowerAtiveX.isInit && cwBrowerAtiveX.init();
                if (settings.WorkInfo.ModuleId == "a7527f97-14e6-44ef-bf73-3039033f128e") {
                    cwBrowerAtiveX.work(0, 3, 1, settings.WorkInfo.ContentId, settings.WorkInfo.PublishId, settings.WorkInfo.WorkId, settings.WorkInfo.DoWorkId, settings.UserInfo.user_id, settings.UserInfo.user_name, 0, settings.vip.toLocaleLowerCase(), settingOptions.packageId);
                }
                else {
                    cwBrowerAtiveX.work(0, 4, 1, settings.WorkInfo.ContentId, settings.WorkInfo.PublishId, settings.WorkInfo.WorkId, settings.WorkInfo.DoWorkId, settings.UserInfo.user_id, settings.UserInfo.user_name, settings.WorkInfo.HistoryScore, settings.vip.toLocaleLowerCase(), settingOptions.packageId);
                }
            }
            ciwong.colseLoading();
        },
        //跟读预览弹框视图
        epDialogView: function () {
            var model = this;
            model.name = ko.observable();
            model.viewType = ko.observable(); //1:单词 2:课文
            model.list = ko.observableArray();
        }
    };

    //对外返回
    exports.viewModel = viewModel;
    exports.moduleId = "f0833ebe-6a8b-4cc1-a6b5-f4d47d93df35";

    ciwong.koTemplateEngine.add("followReadTemplate", '\
		<!--ko foreach: list -->\
			<li class="cf" data-bind="visible: !(module_id == \'a7527f97-14e6-44ef-bf73-3039033f128e\' && list.length == 0)">\
				<div class="txt">\
					<p class="tit"><a data-bind="text: name, attr:{ \'title\': name }, click: function(data){ $parent.contentView(data) }"></a></p>\
					<a data-bind="click: function(data){ $parent.contentView(data) }" class="lnk-preview">预览</a>\
				</div>\
				<p class="tr">\
					<a data-bind="click: function(data){ $parent.start(data,1) }, text: module_id == \'a7527f97-14e6-44ef-bf73-3039033f128e\' ? \'单词跟读\' : \'逐句跟读\'" class="trainBtn"></a>\
					<a data-bind="click: function(data){ $parent.start(data,2) }, visible: !hidden" class="trainBtn">独立通读</a>\
					<a data-bind="click: function(data){ $parent.start(data,3) }, visible: !hidden" class="trainBtn">选段背诵</a>\
				</p>\
			</li>\
		<!--/ko-->');

    ciwong.koTemplateEngine.add("contentViewTemplate", '\
		<p class="tc yh previeT" data-bind="text: name"></p>\
		<div data-bind="if: viewType() == 1">\
			<p class="previewords cf" data-bind="foreach:list">\
				<span data-bind="text:$data"></span>\
			</p>\
		</div>\
		<div data-bind="if: viewType() == 2">\
			<ul class="previewList mt20 f14 yh" data-bind="foreach:list">\
				<li data-bind="text:$data"></li>\
			</ul>\
		</div>\
		<div class="tc mt20" data-bind="visible:list().length == 0"><img src="/Content/Default/images/loading.gif" /></div>\
		<div class="tc"><a id="completedRead" class="btn-big">关闭</a></div>');
});
