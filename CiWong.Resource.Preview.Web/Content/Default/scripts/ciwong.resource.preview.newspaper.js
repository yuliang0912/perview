define(function (r, exports) {
	var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko");

	var defaultSettings = {
		grade: {
			"1101": "1", "1102": "2", "1103": "3", "1104": "4", "1105": "5", "1106": "6", "1107": "7", "1108": "8", "1109": "9"
		}
	};

	var methods = {
		getAreaName: function (areaName) {
			if (!areaName || areaName == "全国通用") {
				return "通用版";
			}
			if (areaName.length > 2) {
				return areaName.replace(/省|市|区|县/, "专版");
			}
			return areaName + "专版";
		}
	};

	var viewModel = {
		preView: function (settingOptions) {
			var model = this,
                settings = $.extend(true, settingOptions || {});

			model.catalogues = ko.observableArray();

			model.works = ko.observableArray();

			model.currCatalogueName = ko.observable();
			model.bookVersion = ko.observable();

			model.grade = defaultSettings.grade[settings.gradeId]
			model.area = methods.getAreaName(settings.areaName);

			settings.packageId && settings.bookVersion && ciwong.ajax.getJSON("/resource/newspaperinfo", { packageId: settings.packageId, bookVersion: settings.bookVersion }, function (data) {
				model.catalogues(data.catalogues);

				model.bookVersion(data.bookVersionName);

				$.each(data.catalogues, function (i) {
					if (this.id == settings.catalogueId) {
						model.currCatalogueName(this.name);
						return false;
					}
				});
			});

			settings.doWorkId && settings.recordId && ciwong.ajax.getJSON("/work/GetNotCompletedWorks", { doWorkId: settings.doWorkId, recordId: settings.recordId }, model.works);
		}
	};

	exports.viewModel = viewModel;
	exports.moduleId = "6360b174-8450-48cd-80f5-cee9067e9a59";
});