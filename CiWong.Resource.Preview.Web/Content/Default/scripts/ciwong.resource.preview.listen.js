define(function (r, exports) {

	var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko"), cwBrowerAtiveX = require("cwBrowerAtiveX"), dialog = require("dialog");

    var defaultSettings = function () {
        return {
            data: null,
            clientUrl: {
                resouseUrl: "http://121.14.117.24:8087/expandwork/speak/resource_v2",
                sumbitWorkUrl: "http://121.14.117.24:8087/expandwork/speak/submit_v2"
            }
        };
    };

    //听力视图模型
    var viewModel = {
        //自主练习视图
        preView: function (settingOptions) {
        	var model = this,
			   settings = $.extend(true, defaultSettings(), settingOptions || {});

        	model.start = function (type) {
        		!cwBrowerAtiveX.isInit && cwBrowerAtiveX.init();
        		ciwong.ajax.getJSON("/Listen/CreatePraiseWork", { versionId: model.resource().version_id, moudleId: model.resource().module_id, appId: model.resource().app_id == null ? 0 : model.resource().app_id, packageId: 0, productId: 0, taskId: 0 }, function (d) {
        		    d && cwBrowerAtiveX.workV2(settings.clientUrl.resouseUrl + "?type=1&versionid=" + model.resource().version_id, settings.clientUrl.sumbitWorkUrl + "?id=" + d + "&is_work=0&type=1&userId=" + settings.user_id, type);
        		});
        	}
        	model.resource = ko.observable();

        	settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "fcfd6131-cdb6-4eb8-9cb9-031f710a8f15" }, function (data) {
        		model.resource(data);
        		$("#resourceName,#_ebook_top hgroup h1").html(data.name);
        	}).done(function () {
        		$("#_listen_panel,#listen_content").show();
        		$("#listen_loading").hide();
        		ciwong.colseLoading();
        	});
        },
        //做作业视图
        doView: function (settingOptions) {
            var model = this,
				settings = $.extend(true, defaultSettings(), settingOptions || {});

            model.start = function () {
                !cwBrowerAtiveX.isInit && cwBrowerAtiveX.init();
                ciwong.ajax.getJSON("/Listen/CreateWork", { contentId: settings.WorkInfo.ContentId, doWorkId: settings.WorkInfo.DoWorkId }, function (data) {
                    data && cwBrowerAtiveX.workV2(settings.clientUrl.resouseUrl + "?type=1&versionid=" + settings.WorkInfo.VersionId, settings.clientUrl.sumbitWorkUrl + "?id=" + data + "&is_work=1&type=1&userId=" + settings.UserInfo.user_id, 1);
                }).done(function (data) {
                    if (data && data.code == 1) {
                        location.reload();
                    }
                })
            };
            ciwong.colseLoading();
        }
    };
    //对外返回
    exports.viewModel = viewModel;
    exports.moduleId = "fcfd6131-cdb6-4eb8-9cb9-031f710a8f15";

    ciwong.koTemplateEngine.add("listenTemplate", '\
		<p class="tc"> <a href="javascript:void(0);" data-bind="click: function(){ start(0); }" class="btn-2 yh">自由练习</a><a href="javascript:void(0);" data-bind="click: function(){ start(1); }" class="btn-2 yh">模拟考试</a></p>');

});