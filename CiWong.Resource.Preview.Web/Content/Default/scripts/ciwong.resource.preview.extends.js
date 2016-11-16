define(["jquery", window.JSON ? undefined : "json2"], function ($) {

	var ciwong = window.ciwong || {};

	//ciwong.ajax = new function () {
	//	var isPreventRequest = false;
	//	var defaultSettings = function () {
	//		return {
	//			contentType: "application/json; charset=utf8",
	//			dataType: 'json',
	//			cache: false,
	//			async: true
	//		};
	//	};
	//};

	ciwong.ajax = {
		isPreventRequest: false, //是否阻止其他post请求
		getJSON: function (url, postData, successFunc, cache, async) {
			return $.ajax({
				type: 'get',
				url: url,
				contentType: "application/json; charset=utf8",
				dataType: 'json',
				data: postData,
				cache: cache == undefined ? false : cache,
				async: async == undefined ? true : async,
				success: function (result) {
					if (result && result.hasOwnProperty('is_succeed') && !result.is_succeed) {
						return require(["dialog"], function () { $.error(result.msg) });
					}

					successFunc && successFunc.call(result, result.data == undefined || result.data == null ? result : result.data);
				}
			});
		},
		postJSON: function (url, postData, successFunc, succeedMessage, formatError, async, beforeSend) {
			if (ciwong.ajax.isPreventRequest) {
				return null;
			}
			return $.ajax({
				type: 'post',
				url: url,
				data: postData,
				async: async == undefined ? true : async,
				beforeSend: beforeSend,
				success: function (result) {
					ciwong.ajax.isPreventRequest = false;
					if (result && result.hasOwnProperty('is_succeed') && !result.is_succeed) {
					    return $.isFunction(formatError) ? formatError.call(result, result.msg || '')
							   : require(["dialog"], function () { $.error(result.msg); }) && false;
					}

					successFunc && successFunc.call(result, result.data == undefined || result.data == null ? result : result.data);

					succeedMessage && require(["dialog"], function () { $.succeed(succeedMessage); });
				},
				error: function () {
					ciwong.ajax.isPreventRequest = false;
					require(["dialog"], function () { $.error('数据请求失败，请重试。'); });
				}
			});
		}
	};

	ciwong.englishLetter = ["A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"];

	ciwong.colseLoading = function (reverse) {
		if (!reverse) {
			$("#nw_loading").hide();
			$("#newspaper_foot").show();
		} else {
			$("#nw_loading").show();
			$("#newspaper_foot").hide();
		}
	};

	ciwong.colsePage = function () {
		window.opener = null;
		window.open("", "_self");
		window.close();
	};
    
	ciwong.encoder = new function () {
		var encoder = {
			/**
			* html解码
			*
			* @param {String} html　经过htmlEncoder编码的html字符串
			* @static method {String}
			**/
			htmlDecode: function (html) {
				if (!html) {
					return '';
				}

				html = html.replace(/&amp;/g, '&')
							.replace(/&lt;/g, '<')
							.replace(/&gt;/g, '>')
							.replace(/&quot;/g, "\"")
							.replace(/&apos;/g, "'")
			                .replace(/&nbsp;/g," ");

				return html;
			},
			/**
			* html加码
			*
			* @param {String} html
			* @static method {String}
			**/
			htmlEncode: function (html) {
				if (!html) {
					return '';
				}

				html = html.replace(/&/g, '&amp;')
							.replace(/</g, '&lt;')
							.replace(/>/g, '&gt;')
							.replace(/"/g, "&quot;")
							.replace(/'/g, "&apos;");

				return html;
			},
			urlDecode: function (url) {

				if (!url) {
					return '';
				}

				return decodeURIComponent(url)

			},
			urlEncode: function (url) {
				if (!url) {
					return '';
				}

				return encodeURIComponent(url)
					.replace(/!/g, '%21')
					.replace(/'/g, '%27')
					.replace(/\(/g, '%28')
					.replace(/\)/g, '%29')
					.replace(/\*/g, '%2A')
					.replace(/%20/g, '+')
					.replace(/%C2%A0/gi, '+');
			}
		};

		return encoder;
	};

	ciwong.koTemplateEngine = new function () {
		var allowTemplateRewriting = false;
		return {
			add: function (templateName, templateMarkup) {
				if (!allowTemplateRewriting && $("#" + templateName).length > 0) {
					return;
				}
				$("#" + templateName).remove();
				$("body").append("<script type='text/html' id='" + templateName + "'>" + templateMarkup + "<\/script>");
			},
			allowTemplateRewriting: function (value) {
				allowTemplateRewriting = value;
			}
		}
	};

	ciwong.epDialog = {
		init: function (templateName, dataName, closeFunc) {
			var _isInit = null;

			var epDialog = function (templateName, dataName, closeFunc) {
				var that = this;

				that.epmain = $("div.mainwrap");
				that.wrap = $(".commend-listwrap");

				if ($(".commend-epwrap").length == 0) {
					var epwrap = $('<div class="commend-epwrap" id="_dialog"></div>');
					var html = '<a class="ep-close" id="ep-close" href="javascript:void(0)" title="返回"></a>';
					html += ' <div class="mainarea clearfix">';
					html += '<div class="commend-epbody" {0}>'.format(dataName ? 'data-bind="if:' + dataName + '"' : '');
					html += '<div class="commend-epbody-inner" id="epContent" data-bind="template: { name: \'{0}\' {1} }\"></div>'.format(templateName, dataName ? ',data: ' + dataName + '' : '');
					html += '</div>';
					html += '<div class="epmask" id="epMask"></div>';
					html += '</div>';
					epwrap.html(html);
					that.epmain.append(epwrap);
				}

				that.doms = {
					$content: $("#epContent"),
					$epbody: $(".commend-epbody"),
					$epclose: $("#ep-close"),
					$epmask: $("#epMask")
				};

				that.$body = $("body");
				that.active = false;

				function close(e) {
					e.preventDefault && e.preventDefault();
					if (that.active) {
						that.close();
						closeFunc && closeFunc();
					}
				}

				that.doms.$epbody.bind("click", function (e) { if (this == e.target) close(e) });
				that.doms.$epclose.bind('click', close);
				that.doms.$epmask.bind('click', close);

				that.open = function () {
					if (!that.active) {
						that.active = true;
						that.sTop = document.body.scrollTop || document.documentElement.scrollTop;
						that.wrap.css({ "top": -that.sTop });
						that.$body.addClass("commend-showep");
					}
					that.$body.scrollTop(0);
					that.doms.$epclose.css({ visibility: "visible" });
					that.epmain.addClass("commend-eploaded");
					$("#epContent").css({ visibility: "visible" }).show();
				};
				that.close = function () {
					that.active = false;
					$("#epContent").hide();
					that.$body.removeClass("commend-showep");
					document.body.scrollTop = this.sTop;
					document.documentElement.scrollTop = this.sTop;
				};
				return that;
			};

			return new function () {
				if (_isInit == null) {
					_isInit = new epDialog(templateName, dataName, closeFunc);
					ciwong.epDialog.isInit = true;
				}
				return _isInit;
			}
		},
		html: function (html) {
			var ep = this.init();
			$("#epContent").html(html);
			return ep;
		},
		isInit: false
	};

	/*
		#region 拓展函数区域
	*/
	window.String.format = new function () {
		return function () {
			var args = Array.prototype.slice.call(arguments),
				str = args.shift();

			return str.replace(/\{(\d+)\}/g,
				function (m, i) { return args[i]; }
			);
		};
	};

	window.String.prototype.format = function () {
		var args = Array.prototype.slice.call(arguments)

		return this.replace(/\{(\d+)\}/g,
			function (m, i) { return args[i]; }
		);
	}

	window.String.prototype.removeHtmlTag = function () {
		return this.replace(/<[a-zA-Z0-9]+? [^<>]*?>|<\/[a-zA-Z0-9]+?>|<[a-zA-Z0-9]+?>|<[a-zA-Z0-9]+?\/>|\r|\n/ig, "");
	}

	window.String.prototype.trim = function () {
		return this.replace(/(^\s*)|(\s*$)/g, "");
	}

	/*
		#endregion 拓展函数区域
	*/

	$.fn.timer = function (settingOption) {
		var self = this, interval = null,
			settings = {
				startTimeLine: 0, //开始计时时刻
				timeSpan: 1000,   //步伐长度
				formatString: "{2}:{1}:{0}",// 秒/分/小时
				isfillZero: true //是否0填充
			};

		$.extend(settings, settingOption || {});

		var Timer = new Date(settings.startTimeLine * 1000);

		var runHandler = function () {
			Timer.setTime(Timer.getTime() + settings.timeSpan);
			setHtml();
		}

		var setHtml = function () {
			var minute = Timer.getMinutes();
			var second = Timer.getSeconds();
			var hour = Timer.getHours() - 8;

			var argLength = settings.formatString.match(/\{(\d+)\}/g).length;
			if (argLength == 2) {
				minute += hour * 60;
			}

			if (settings.isfillZero) {
				if (hour < 10) {
					hour = "0{0}".format(hour);
				}
				if (minute < 10) {
					minute = "0{0}".format(minute);
				}
				if (second < 10) {
					second = "0{0}".format(second);
				}
			}
			self.html(settings.formatString.format(second, minute, hour));
		}

		self.State = "";

		self.getTime = function () {
			return Timer.getTime() / 1000;
		}

		self.start = function (index) {
			if (!interval) {
				interval = setInterval(function () {
					runHandler();
				}, settings.timeSpan);
			}
			self.State = "start";
			return self;
		};

		self.stop = function () {
			clearInterval(interval);
			interval = null;
			self.State = "stop";
			return self;
		};

		self.clear = function () {
			self.State = "clear";
			self.stop();
			Timer.setTime(settings.startTimeLine - 1000);
			runHandler();
			return self;
		};
		setHtml();
		return self;
	};

	window.ciwong = ciwong;

	return ciwong; //对外返回
});


