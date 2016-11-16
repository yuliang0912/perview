define(function (r, exports) {
    var ciwong = require("ciwong"), ko = require("ko"), dialog = require("dialog");

    var viewModel = {
        previewModel: function (settingOptions) {
            var model = this,
                   settings = settingOptions || {};
            model.entity = ko.observable();
            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "f0833ebe-6a8b-4cc1-a6b5-f4d47d93df35" }, function (data) {
                if (data.ret > 0) {
                    $.error("数据读取失败!");
                } else {
                    model.entity(data);
                }
            });
        },
        /**课文**/
        preViewTextModel: function (settingOptions) {
            var model = this,
                    settings = settingOptions || {};

            model.entity = ko.observable();
            /**时文资源列表*/

            settings.version_id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.version_id, moduleId: settings.module_Id }, function (data) {
                if (data.ret > 0) {
                    $.error("数据读取失败!");
                } else {
                    model.entity(data);
                    audiojs.createAll();
                }
            });

        }
    };
    exports.viewModel = viewModel;
    exports.moduleId = "f0833ebe-6a8b-4cc1-a6b5-f4d47d93df35";
});