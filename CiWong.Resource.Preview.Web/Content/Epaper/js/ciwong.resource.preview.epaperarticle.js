define(function (r, exports) {

    var ciwong = require("ciwong"), ko = require("ko"), dialog = require("dialog");

    var viewModel = {
        preView: function (settingOptions) {
            var model = this,
			settings = $.extend(true, settingOptions || {});

            model.article = ko.observable();



            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "05a3bf23-b65b-4d7f-956c-5db2b76b9c11" }, function (data) {
                if (data.ret > 0) {
                    $.error("资源读取失败");
                } else {
                    model.article(data); audiojs.createAll();
                }
            })
        }
    };

    exports.viewModel = viewModel;
    exports.moduleId = "1daf88c8-cde8-4d81-94be-d42bc30f52ed";
});