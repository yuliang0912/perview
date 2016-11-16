define(function (r, exports) {

	var ciwong = require("ciwong"), ko = require("ko");

	var viewModel = {
		preView: function (settingOptions) {
			var model = this,
                settings = $.extend(true, settingOptions || {});

			model.catalogues = ko.observableArray();
			model.currModules = ko.observableArray();

			model.showModules = function (data) {
				if (data.modules && data.modules.length == 1 && settings.packageId) {
					location.href = "/jump/preview?packageId={0}&cid={1}&moduleId={2}".format(settings.packageId, data.modules[0].package_catalogue_id, data.modules[0].module_id);
				}
				else if (data.modules && data.modules.length > 0) {
					model.currModules(data.modules);
				} else {
					model.currModules([]);
				}
			};

			settings.packageId && ciwong.ajax.getJSON("/resource/ebookcatalogues", { packageId: settings.packageId }, function (data) {

				function setModules(catalogues, modules) {
					$.each(catalogues, function (i, item) {
						item.modules = $.grep(modules, function (n) { return n.package_catalogue_id == item.id });
						item.children.length > 0 && setModules(item.children, modules);
					})
				}
				setModules(data.catalogues, data.modules);

				model.catalogues(data.catalogues);
			}, true);
		}
	};
	exports.viewModel = viewModel;

	ciwong.koTemplateEngine.add("catalogueTemplate", '\
		<div class="pl25" data-bind="foreach: children,visible: children.length > 0">\
			<p><a data-bind="text: name, attr: { \'title\': name }, click: $root.showModules,css: ($data.children.length == 0 && $data.modules.length == 0) ? \'no\':\'\'"></a></p>\
			<!--ko template: { name: \"catalogueTemplate\" } -->\
			<!--/ko-->\
		</div>');
});