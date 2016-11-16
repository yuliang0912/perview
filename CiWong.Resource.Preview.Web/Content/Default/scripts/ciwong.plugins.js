

ciWong.ajax = {
    /*
    *
    */
    getJSON: function (url, postData, success, succeedMessage, formatError, cache, async) {
        $.ajax({
            type: 'get',
            url: url,
            contentType: "application/json; charset=utf8",
            dataType: 'json',
            data: postData,
            cache: cache == undefined ? false : cache,
            async: async == undefined ? true : async,
            success: function (result) {

                if (result && result.hasOwnProperty('is_succeed') && !result.is_succeed) {
                    return ciWong.type.isFunction(formatError)
                        ? formatError.call(result, result.message || '')
                        : $.error(formatError ? String.format(formatError, result.message) : result.message) && false;
                }

                success && success.call(result, result.data || result);

                succeedMessage && $.succeed(succeedMessage);
            },
            error: function () {
                $.error('数据请求失败，请重试。');
            }
        });
    },
    postJSON: function (url, postData, success, succeedMessage, formatError, async) {
        $.ajax({
            type: 'post',
            url: url,
            contentType: "application/json; charset=utf8",
            dataType: 'json',
            data: ciWong.json.stringify(postData),
            async: async == undefined ? true : async,
            success: function (result) {
                if (result && result.hasOwnProperty('is_succeed') && !result.is_succeed) {
                    return ciWong.type.isFunction(formatError)
                        ? formatError.call(result, result.message || '')
                        : $.error(formatError ? String.format(formatError, result.message) : result.message) && false;
                }

                success && success.call(result, result.data || result);

                succeedMessage && $.succeed(succeedMessage);
            },
            error: function () {
                $.error('数据请求失败，请重试。');
            }
        });
    }
};

(function () {

    var functions = [];

    ciWong.func = {
        add: function (fn, scope) {
            return functions.push(function () {
                return fn.apply(scope || this, arguments);
            }) - 1;
        },

        /**
         * Removes the function reference created with {@link #addFunction}.
         *
         * @param {Number} ref The function reference created with
         * {@link #addFunction}.
         */
        remove: function (ref) {
            functions[ref] = null;
        },

        /**
         * Executes a function based on the reference created with {@link #addFunction}.
         *
         *		var ref = CKEDITOR.tools.addFunction( function() {
         *			alert( 'Hello!');
         *		} );
         *		CKEDITOR.tools.callFunction( ref ); // 'Hello!'
         *
         * @param {Number} ref The function reference created with {@link #addFunction}.
         * @param {Mixed} params Any number of parameters to be passed to the executed function.
         * @returns {Mixed} The return value of the function.
         */
        call: function (ref) {
            var fn = functions[ref];
            return fn && fn.apply(window, Array.prototype.slice.call(arguments, 1));
        },
        apply: function (ref, args) {
            var fn = functions[ref];
            return fn && fn.apply(window, args);
        }
    };

})();


jQuery.fn.extend({
    //根据浏览器窗口大小变化自动调整对象height与width
    autoResize: function (settingOptions) {
        var self = this;
        this.settings = {
            reduceWidth: 0,
            reduceHeight: 0,
            minWidth: 0,
            maxWidth: 0,
            minHeight: 0,
            maxHeight: 0,
            orientationMode: "all", //all、height、width 
            referToExpr: window //参照对象jquery expr
        }

        $.extend(true, this.settings, settingOptions || {});

        this.referToControl = $(this.settings.referToExpr);
        if (this.referToControl.length == 0 || this.length == 0) {
            return;
        }

        $(window).resize(function () {

            self.each(function () {
                var width = self.referToControl.width() - self.settings.reduceWidth - $(this).offsetAllWidth() + $(this).width();
                var height = self.referToControl.height() - self.settings.reduceHeight - $(this).offsetAllHeight() + $(this).height();

                if (this.length == 0)
                    return;

                if (self.settings.orientationMode == 'all' || self.settings.orientationMode == 'width') {
                    if ($(this).css("overflow-y") == "scroll") {
                        width = width - 20;
                    }

                    if (self.settings.minWidth != 0 && self.settings.minWidth > width) {
                        width = self.settings.minWidth;
                    }

                    if (self.settings.maxWidth != 0 && self.settings.maxWidth < width) {
                        width = self.settings.maxWidth;
                    }

                    $(this).width(width);
                }

                if (self.settings.orientationMode == 'all' || self.settings.orientationMode == 'height') {
                    if ($(this).css("overflow-x") == "scroll") {
                        height = height - 20;
                    }

                    if (self.settings.minHeight != 0 && self.settings.minHeight > height) {
                        height = self.settings.minHeight;
                    }

                    if (self.settings.maxHeight != 0 && self.settings.maxHeight < height) {
                        height = self.settings.maxHeight;
                    }

                    $(this).height(height);
                }
            });
        });

        $(window).resize();
    },

    //对象高度 + 边框 + 内补白 + 外补白
    offsetAllHeight: function () {
        var offsetHeight = this[0].offsetHeight;
        if (!offsetHeight) {
            offsetHeight = this.height();
        }

        return offsetHeight + ue.framework.convert.toInt32(this.css("margin-top")) + ue.framework.convert.toInt32(this.css("margin-bottom"));
    },

    //对象宽度 宽度 + 边框 + 内补白 + 外补白 
    offsetAllWidth: function () {
        var offsetWidth = this[0].offsetWidth;
        if (!offsetWidth) {
            offsetWidth = this.width();
        }
        return this[0].offsetWidth + ue.framework.convert.toInt32(this.css("margin-left")) + ue.framework.convert.toInt32(this.css("margin-right"));
    },

    //输入表格
    inputTable: function (className) {
        this.each(function () {
            var cells = parseInt($(this).attr("cells"))
            if (!cells) cells = 2;
            $(this).find(" > tbody > tr > td:not([isTitle=false],[class=btnConfirm])").each(function (i) {
                if (i % cells == 0) $(this).addClass("title");
            });

            $(this).find("td.btnConfirm").attr("colspan", $(this).find("tr:first > td").length);
        });

        if (!className) {
            className = "inputTable";
        }

        this.addClass(className);
    },

    dataGrid: function (settingOptions) {
        var self = this;
        this.settings = {
            Class: "dataTable",
            SeparatorTemplateClass: "separator",
            HoverClass: "hover",
            gridExpr: ">tbody>tr"
        }

        $.extend(true, this.settings, settingOptions || {});

        self.addClass(self.settings.Class);

        var grid = self.find(self.settings.gridExpr);
        if (grid.length == 0) {
            grid = self.find(">tr");
        }

        if (grid && grid.length > 0) {
            grid.filter(":odd").addClass(self.settings.SeparatorTemplateClass);
            grid.hover(function () {
                $(this).addClass(self.settings.HoverClass);
            }, function () {
                $(this).removeClass(self.settings.HoverClass);
            });

            grid.each(function () {
                var href = $(this).attr("href");

                if (href && href.length > 0) {
                    $(this).css("cursor", "pointer").click(function () {
                        ue.framework.response.redirect(href);
                    }).find("td > a,td>input").click(function (e) {
                        //阻止事件冒泡
                        if (e && e.stopPropagation) {
                            //W3C取消冒泡事件
                            e.stopPropagation();
                        } else {
                            //IE取消冒泡事件
                            window.event.cancelBubble = true;
                        }
                    });
                }
            });
        }
    },

    //加入收藏夹
    addFavorite: function (url, title) {
        this.click(function () {
            if (document.all) {
                window.external.AddFavorite(url, title);
            }
            else if (window.sidebar) {
                window.sidebar.addPanel(title, url, "");
            }
        });
    },

    //js时钟
    clock: function () {
        var self = this;
        var stepFun = function (format) {
            var Timer = new Date();
            var minutes = Timer.getMinutes();
            var seconds = Timer.getSeconds();

            if (minutes < 10)
                minutes = "0{0}".Format(minutes);

            if (seconds < 10)
                seconds = "0{0}".Format(seconds);

            if (!format) {
                format = "{0}年{1}月{2}日 {3}:{4}:{5} 星期{6}";
            }

            self.text(format.Format(Timer.getFullYear(), Timer.getMonth() + 1, Timer.getDate(), Timer.getHours(), minutes, seconds, ["日", "一", "二", "三", "四", "五", "六"][Timer.getDay()]));

            setTimeout(stepFun, 1000);
        }
        stepFun();
    },

    //全选，反选
    allChecked: function (otherCheckExpr, otherIsEnabled) {
        this.each(function () {
            $(otherCheckExpr).attr("checked", $(this).attr("checked") == "checked");

            $(this).change(function () {
                var checked = $(this).attr("checked") == "checked";
                $(otherCheckExpr).attr("checked", checked);
                if (otherIsEnabled) {
                    $(otherCheckExpr).attr("disabled", checked);
                }
            }).change();
        });
    },

    //智能浮动框
    smartFloat: function () {
        var position = function (element) {
            //占位符，当element设置position时，需要让占位符替代element显示在element的原位置上，要不然页面element原位置因缺少element而乱掉
            var placeholder = null,
                position = element.css("position"),
                width = element.css('width');

            $(window).scroll(function () {
                var top = (placeholder || element).position().top,
                    scrollTop = $(this).scrollTop();

                if (!placeholder && scrollTop > top) {
                    placeholder = element.clone().insertAfter(element).show();

                    element.css({
                        position: "fixed",
                        top: 0,
                        width: width,
                        'z-index': 99
                    });
                }
                else if (placeholder && scrollTop < top) {
                    element.css('position', position);
                    placeholder.hide();
                    placeholder = null;
                }
            });
        };
        return this.each(function () {
            position($(this));
        });
    },

    //设置输入框默认显示文字，当鼠标焦点时文字删除，离开焦点时如果为空，重新显示提示信息
    textBoxTip: function (settingOptions) {
        var settings = {
            Title: "请输入关键字",
            TipClass: "tipClass",
            TipStyle: { color: "#999" },
            SubmitExp: "#btnConfirm,#btnSearch,#btnAdd,#btnEdit"
        };

        $.extend(true, settings, settingOptions || {});

        var getDefaultValue = function (element) {
            var label = element.attr("title");
            return label ? label : settings.Title;
        }

        var setClass = function (element) {
            element.addClass(settings.TipClass);
            element.css(settings.TipStyle);
        };

        var clearClass = function (element) {
            element.removeClass(settings.TipClass);
            element.removeAttr("style");
        };

        var valIsChanged = function (element) {
            return element.val() != getDefaultValue(element) && element.val().length > 0;
        };

        var tip = function (element) {

            element.focus(function () {
                if (!valIsChanged(element)) {
                    element.val("");
                    clearClass(element);
                }
            });

            element.focusout(function () {
                if (!valIsChanged(element)) {
                    element.val(getDefaultValue(element));
                    setClass(element);
                }
                else {
                    clearClass(element);
                }

            });

            element.focusout();

            $(settings.SubmitExp).click(function () {
                if (!valIsChanged(element)) {
                    element.val("");
                }

                setTimeout(function () {
                    element.focusout();
                }, 300)
            });
        };

        return this.each(function () {
            tip($(this));
        });
    },

    //为按钮添加一个事件，当目标对象显示的时候，点击按钮隐藏目标对象。当目标对象显示的时候，点击按钮隐藏目标对象。
    toggleButton: function (targetExp, settingOptions) {
        var self = this;
        self.settings = {
            targetShowBtnClass: "btn_arrow_down",
            targetHideBtnClass: "btn_arrow_up",
            targetDefaultStatus: false
        };
        self.target = $(targetExp);

        $.extend(true, self.settings, settingOptions || {});

        self.toggleTarget = function (isShow) {
            if (isShow) {
                self.addClass(self.settings.targetShowBtnClass);
                self.removeClass(self.settings.targetHideBtnClass);
                self.target.show();
            }
            else {
                self.addClass(self.settings.targetHideBtnClass);
                self.removeClass(self.settings.targetShowBtnClass);
                self.target.hide();
            }
        };

        self.toggle(function () {
            self.toggleTarget(true);
        }, function () {
            self.toggleTarget(false);
        });

        if (this.settings.targetDefaultStatus) {
            self.click();
        }
        else {
            self.toggleTarget(false);
        }
    },

    //条件筛选器，多个a链接选择项，点击某个链接后，其它的链接消失，并显示选中的项
    conditionFilter: function (settingOptions) {
        var self = this;
        self.settings = {
            template: "<div><em>您已选择：<em><span>{0}<a class='close'></a></span></div>",
            closeOnClick: function (element) { },
            onClick: function (element) { }
        }
        self.itemArray = self.find("a");
        self.showFlat = false;

        $.extend(true, self.settings, settingOptions || {});

        self.itemArray.click(function () {
            self.showFlat = $(string.Format(self.settings.template, $(this).text()));
            self.showFlat.find("a.close").click(function () {
                if (self.settings.closeOnClick) {
                    self.settings.closeOnClick(this);
                }

                self.showFlat.remove();
                self.show();
            });

            if (self.settings.onClick) {
                self.settings.onClick(this);
            }

            self.after(self.showFlat);
            self.showFlat.show();
            self.hide();
        });
    },

    //倒计时
    countdown: function (settingOptions) {
        var self = this;
        self.settings = {
            showExpr: "span.countdown",
            timeSpan: "00:00:00",
            //倒计时结束回调
            endCallback: function (content) { }
        };
        $.extend(true, self.settings, settingOptions || {});

        self.Interval = null;
        self.countArray = $(self.settings.timeSpan.split(':')).map(function () { return parseInt(this); });
        self.maxCountArray = [999, 59, 59];

        if (self.countArray.length == 4) {
            self.countArray[1] = self.countArray[1] + self.countArray[0] * 24;
            self.countArray.splice(0, 1);
        }

        //显示当前计时
        self.showCount = function () {
            self.find(self.settings.showExpr).each(function (index) {
                if (self.countArray.length > index && index < 3) {
                    var count = Math.round(self.countArray[index]);
                    $(this).text(count > 9 ? count : "0" + count);
                }
            });
        };

        //跳秒
        self.removeStep = function (index) {
            if (self.countArray[index] > 0) {
                self.countArray[index] = self.countArray[index] - 1;
            }
            else {
                self.countArray[index] = self.maxCountArray[index];

                if (index > 0) {
                    self.removeStep(index - 1);
                }
            }
        };

        //判断是否结束，如果结束那么清空Interval并调用endCallback回调方法
        self.judgeEndHandler = function () {
            var greaterThanZeroArray = $.grep(self.countArray, function (val, itemIndex) { if (val > 0) return val; });
            if (greaterThanZeroArray == 0) {
                self.settings.endCallback(self);
                clearInterval(self.Interval);
                self.Interval = null;
            }
        }

        self.showCount();
        self.Interval = setInterval(function () {
            self.removeStep(2);
            self.showCount();
            self.judgeEndHandler();
        }, 1000);
    },

    //上传组件
    fileUpload: function (settingOptions) {

        var settings = $.extend(true, {
            uploader: 'http://resource.upload.ciwong.com/xiwang/upload',
            swf: 'http://style.ciwong.net/tools/scripts/plugins/uploadify/uploadify.swf',
            'fileObjName': 'file',
            formData: {
                appid: 1,
                mode: 2,
                uid: null,
                tid: null,
                sid: null,
                ver: Math.random(),
                type: 51 //在高校课堂数据库中的文件类型
            },
            buttonClass: 'min-btn',
            buttonText: '+上传文件',
            height: 32,
            width: 77,
            fileTypeExts: "*.doc;*.docx;*.ppt;*.pptx;*.jpeg;*.pdf;*.txt;", //*.flv;*.swf;*.mpeg;*.wmv;*.rmvb;*.mov;*.avi,*.mp4;
            onUploadSuccess: function (item) { }
        }, settingOptions || {});

        settings.onUploadSuccess = function (fileObject, dataString) {

            var data = ciWong.json.parse(dataString),
                file;

            if (!data || data.errcode != 0 || !(file = data.data[0])) {
                return $.error('文件上传失败，请重试。') && false;
            }

            var item = {
                name: file.filename.replace('.' + file.suffix, ''),
                extention_name: file.suffix,
                url: String.format('{0}/{1}', file.appid, file.md5filename),
                size: file.filesize,
                type: settings.formData.type
            };

            settingOptions.onUploadSuccess && settingOptions.onUploadSuccess(item);
        };

        this.each(function () {
            $(this).uploadify(settings);
        });
    }
});
