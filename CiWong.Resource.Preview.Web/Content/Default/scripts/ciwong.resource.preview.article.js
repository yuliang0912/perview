define(function (r, exports) {

	var ciwong = require("ciwong"), ko = require("ko");

	var methods = {
		//去掉文章字符
		trimContent: function (str, bool_a, bool_b, bool_c) {
			if (!str)
				return "";
			if (bool_a) //去掉html标签
				str = str.replace(/<[^>]*>|/g, "");
			//if (bool_b) //去掉标点符号
			//str = str.replace(/[\~|\`|\!|\！|\@|\#|\$|\%|\^|\&|\*|\(|\)|\、|\-|\_|\+|\=|\||\\|\[|\]|\{|\}|\;|\；|\:|\：|\"|\“|\'|\‘|\,|\，|\<|\.|\。|\>|\/|\?|\？]/g, "");
			if (bool_c)//去掉多余空格，只保留一个
				str = str.replace(/\s+/g, ' ');
			var reg_en = new RegExp(/([\']?\w+)/g); // new RegExp(/[a-zA-Z]+([\']?[a-zA-Z]+)?/g);
			var count_en = str.match(reg_en) == null ? 0 : str.match(reg_en).length; //英文单词、数字个数
			var count_ch = str.replace(/[^\u4e00-\u9fa5]/gi, "").length; //中文个数
			return count_en + count_ch; //返回总字符数
		}
	};

	var viewModel = {
		preView: function (settingOptions) {
			var model = this,
                settings = $.extend(true, settingOptions || {});

			model.viewType = ko.observable(settings.viewType || 1);//1.阅读 2.完成阅读 3.预览

			model.article = ko.observable();

			model.afterRender = function () {
				settings.afterRender && settings.afterRender.call(model);
			}

			model.submitWork = function () {
				model.viewType() == 1 && settings.submitWork && settings.submitWork.call(model);
			}

		    /**开通服务**/
			model.OpenServise = function () {
			    var agentInfo = window.navigator.userAgent;
			    if (agentInfo.indexOf("Android") > -1) {
			        window.WebLoad.openService(); //获取试卷分数或者听说模考的批改分数float score;
			    } else if (agentInfo.indexOf("iPhone") > -1 || agentInfo.indexOf("iPad") > -1 || agentInfo.indexOf("iPod") > -1) {
			        //var url = "ciwong_jingsai://" + data; //iphone调用     
			        //document.location.href = url; //iPhone调用 
			    }
			}

			settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: "05a3bf23-b65b-4d7f-956c-5db2b76b9c11" }, model.article).done(function () {
				ciwong.colseLoading();
			});
		}
	};

	exports.viewModel = viewModel;
	exports.moduleId = "05a3bf23-b65b-4d7f-956c-5db2b76b9c11";
});