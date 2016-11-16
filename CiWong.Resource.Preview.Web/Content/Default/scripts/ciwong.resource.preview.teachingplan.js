define(function (r, exports) {

	var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko");

	var methods = {
		initHtmlEventHandler: function () {
			$("#_step_change li").live("click", function () {
				$("#step_" + $(this).index()).show().siblings().hide();
				$(this).addClass("curr").siblings().removeClass("curr");
			});
			$("div.read-step ul.tepList li:first").addClass("curr");
		}
	};

	var viewModel = {
		preView: function (settingOptions) {
			var model = this,
               settings = settingOptions || {};

			model.viewType = ko.observable(1);

			model.teachingplan = ko.observable();

			model.afterRender = function () {
				methods.initHtmlEventHandler();
			}

			ko.computed(function () {
				model.teachingplan() && $("#_ebook_top hgroup h1").text(model.teachingplan().name);
			});

			settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "a5df23d4-c0fe-4629-b8d3-35288695136b" }, model.teachingplan).done(function () {
				ciwong.colseLoading();
			});
		}
	}

	exports.viewModel = viewModel;
	exports.moduleId = "a5df23d4-c0fe-4629-b8d3-35288695136b";
});