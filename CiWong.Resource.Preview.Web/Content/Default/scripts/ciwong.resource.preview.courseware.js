define(function (r, exports) {

	var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko");

	var viewModel = {
		preView: function (settingOptions) {
			var model = this,
               settings = settingOptions || {};

			model.coursewares = ko.observableArray();

			if (settings.packageId && settings.catalogueId) {
				ciwong.ajax.getJSON("/resource/getTaskResultContents", { packageId: settings.packageId, cid: settings.catalogueId, moduleId: 4 }
				).then(function (data) {
					return $.map(data.data, function (n) { return n.resource_version_id }).toString();
				}).done(function (result) {
					result && ciwong.ajax.getJSON("/resource/getlist", { versionIds: result, moduleId: "40cafe50-68a6-11e4-a4b4-782bcb066f05" }, model.coursewares).done(function () {
						ciwong.colseLoading();
					})
				});
			}
		}
	}

	window.getFileUrl = function (model) {
		if (model.url) {
			if (model.url.indexOf("yishang") > -1) {
				return "http://resource.view.ciwong.com/" + model.url.substr(model.url.indexOf("yishang"));
			} else {
				return model.url;
			}
		} else {
			return "http://resource.view.ciwong.com/yishang/" + model.file_name;
		}
	}

	exports.viewModel = viewModel;
	exports.moduleId = "40cafe50-68a6-11e4-a4b4-782bcb066f05";
});