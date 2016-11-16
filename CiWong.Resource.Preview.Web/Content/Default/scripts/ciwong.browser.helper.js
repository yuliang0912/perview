
define(["jquery", "dialog"], function ($, dialog) {
    var vip = false;
    var cwBrowerAtiveX = {
        currVersion: window.currVersion || "1.0.0.17",
        isChecked: false,
        isInit: false,
        //初始化
        init: function () {
            var _self = this;
            var div = document.createElement("div");
            if ("ActiveXObject" in window) {
                div.innerHTML = '<object id="xiEnglish" style="position: absolute;" width="0" height="0" CLASSID="CLSID:2600692F-6718-418e-A327-583EEAE7FDC8"></object>';
            } else { //non-ie kernel
                div.innerHTML = '<object id="xiEnglish" style="position: absolute;" width="0" height="0" type="application/x-ciwong-xienglish"></object>';
            }
            document.body.appendChild(div);
            cwBrowerAtiveX.iscwbrowser();
            if (cwBrowerAtiveX.isChecked) {
                _czc.push(["_setCustomVar", "当前版本", cwBrowerAtiveX.version(), 1]);
            }
            $("a.downloadPlug").die().live("click", function () {
                _czc.push(["_setCustomVar", "下载插件", cwBrowerAtiveX.currVersion, 1]);
            });
            $("a.updatePlug").die().live("click", function () {
                _czc.push(["_setCustomVar", "更新插件", cwBrowerAtiveX.version(), 1]);
            });
            cwBrowerAtiveX.isInit = true;
        },
        //判断是否安装插件
        iscwbrowser: function () {
            var _self = this;
            try {
                cwBrowerAtiveX.version();//先调用一次,第二次才能取到版本(部分浏览器检查不到插件的BUG)
                if (xiEnglish.getVersion().length > 0) {
                    cwBrowerAtiveX.isChecked = true;
                }
                else {
                    cwBrowerAtiveX.isChecked = false;
                }
            }
            catch (e) {
                cwBrowerAtiveX.isChecked = false;
            }
        },
        //版本
        version: function () {
            try {
                return xiEnglish.getVersion();
            } catch (e) {
                return undefined;
            }
        },
        //版本
        open: function (url) {
            window.external.invoke("cwBrowser_OpenWeb", url);
        },
        //作业
        work: function (version_id, type, is_work, content_id, publish_id, work_id, dowork_id, user_id, user_name, repeat_type_or_scores, vip, packageId) {
            var _self = this;
            //验证版本
            /**验证是否是会员  然后弹出对应的对话框   是会员直接判断是否需要更新插件   如果不是会员先弹出对话框**/
            //content: window.isMember,//  window.plugUpdateContent || '插件要更新啦,<a class="updatePlug" href="http://file.ciwong.com/xixi/uploadfile/soft/cwEvaluationSetup_v' + _self.currVersion + '.exe" target="_blank">点击更新</a>',
            if (vip == "false" && parseInt(packageId) > 0) {
            	$.dialog({
            		id: "artDialogMember",
            		title: '友情提示',
            		content: '<div style="background:#fff; width:700px;">\
                                <div class="opeVipDlg p20">\
                                    <h4>想知道自己读的标不标准？哪里读错了？怎么样可以读的更好？...</h4>\
                                    <h2 class="yh  mt10">购买后，即可拥有专属于你的智能口语教练！</h2>\
                                    <img src="/content/default/images/xixinTip.jpg" alt="" class="mt20" width="100%">\
                                    <p class="mt30 tc" style="padding-top:20px"><a href="/jump/buy?packageId='+ packageId + '" target="_blank" class="btn">立即购买</a><a href="javascript:void(0);" onClick="javascript:_self.memberwork(_self.version_id,_self.type, _self.is_work, _self.content_id, _self.publish_id, _self.work_id, _self.dowork_id, _self.user_id, _self.user_name, _self.repeat_type_or_scores);return false;"  class="ml20 f14 blue">暂不购买&gt;&gt;</a></p>\
                                </div>\
                                <!-- 开通会员弹窗，弹窗宽度设置为700px(代码不包含外层div结构) -->\
                            </div>',
            		width: 500,
            		lock: true,
            		padding: 10
            	});
            }
            else {
            	_self.memberwork(version_id, type, is_work, content_id, publish_id, work_id, dowork_id, user_id, user_name, repeat_type_or_scores);
            }

            _self.version_id = version_id;
            _self.type = type;
            _self.is_work = is_work;
            _self.content_id = content_id;
            _self.publish_id = publish_id;
            _self.work_id = work_id;
            _self.dowork_id = dowork_id;
            _self.user_id = user_id;
            _self.user_name = user_name;
            _self.repeat_type_or_scores = repeat_type_or_scores;

            window._self = _self;
        },
        memberwork: function (version_id, type, is_work, content_id, publish_id, work_id, dowork_id, user_id, user_name, repeat_type_or_scores) {
            var list = window.parent.$.dialog.list;
            for (var i in list) { list[i].close(); };
            var _self = this;
            if (!_self.checkheightversion()) {
                $.dialog({
                    id: "artDialog",
                    title: '友情提示',
                    content: window.plugUpdateContent || '插件要更新啦,<a class="updatePlug" href="http://file.ciwong.com/xixi/uploadfile/soft/cwEvaluationSetup_v' + _self.currVersion + '.exe" target="_blank">点击更新</a>',
                    icon: 'succeed',
                    width: 500,
                    lock: true,
                    padding: 10
                });
            }
            else {
                var param = '{"name":"cwBrowser_BeginSpeeking", "1":"' + version_id.toString() + '", "2":"' + type.toString() + '", "3":"' + is_work.toString() + '", "4":"' + content_id.toString() + '", "5":"' + publish_id.toString() + '", "6":"' + work_id.toString() + '", "7": "' + dowork_id.toString() + '", "8":"' + user_id.toString() + '", "9":"' + user_name.toString() + '","10":"' + repeat_type_or_scores.toString() + '"}';

                if (xiEnglish.begin(param, function (result) { cwBrowerAtiveX.callback(result); })) {
                    $.dialog({
                        id: "artDialog",
                        title: '跟读作业提示',
                        content: '<div class="yh" style="width:600px;"><h4 class="previeT">您目前正在进行同步跟读作业,请不要关闭当前窗口,作业提交成功后会自动关闭！如果长时间窗口未自动关闭,您还可以选择手动关闭！</h4>\
									<p class="tc mt20"><a href="javascript:;" onclick="javascript:cwBrowerAtiveX.callback(-2);" class="btn-big">关闭</a></p>\
								</div>',
                        icon: 'succeed',
                        width: 500,
                        lock: true,
                        padding: 30
                    });
                }
            }
        },
        //作业 新版模听力考插件接口调用
        workV2: function (get_url, post_url, type) {
            var _self = this;
            //验证版本
            if (!_self.checkheightversion()) {
                $.dialog({
                    id: "artDialog",
                    title: '插件更新提示',
                    content: window.plugUpdateContent || '插件要更新啦,<a class="updatePlug" href="http://file.ciwong.com/xixi/uploadfile/soft/cwEvaluationSetup_v' + _self.currVersion + '.exe" target="_blank">点击更新</a>',
                    icon: 'succeed',
                    width: 500,
                    lock: true,
                    padding: 30
                });
            }
            else {
                var param = '{"name":"cwBrowser_BeginCommonlyExam", "1":"' + get_url + '", "2":"' + post_url + '","3":' + type + '}';
                if (xiEnglish.begin(param, function (result) { cwBrowerAtiveX.callback(result); })) {
                    $.dialog({
                        id: "artDialog",
                        title: '听力模考作业提示',
                        content: '<div class="yh" style="width:600px;"><h4 class="previeT">您目前正在进行听力模考作业,请不要关闭当前窗口,作业提交成功后会自动关闭！如果长时间窗口未自动关闭,您还可以选择手动关闭！</h4>\
									<p class="tc mt20"><a href="javascript:;" onclick="javascript:cwBrowerAtiveX.callback(-2);" class="btn-big">关闭</a></p>\
								</div>',
                        icon: 'succeed',
                        width: 500,
                        lock: true,
                        padding: 30
                    });
                }
            }
        },
        //登出
        loginout: function (url) {
            window.external.invoke("cwBrowser_Logout", url);
        },
        //登录
        login: function (id, pwd) {
            window.external.invoke("cwBrowser_Login", id, pwd);
        },
        //作业回调
        callback: function evalation_callback(result) {
            switch (result) {
                case 0:
                    $.dialog.list["artDialog"].content('<div class="yh" style="width:600px;"><h4 class="previeT tc">本次作业提交成功！</h4></div>').time(2000);
                    window.location.reload();
                    break;
                case 1:
                    $.dialog.list["artDialog"].content('<div class="yh" style="width:600px;"><h4 class="previeT tc">本次作业提交失败，请稍后再试！</h4></div>').time(2000);
                    break;
                case -1:
                    $.dialog.list["artDialog"].content('<div class="yh" style="width:600px;"><h4 class="previeT tc">本次已取消！</h4></div>').time(2000);
                    break;
                default:
                    $.dialog.list["artDialog"].close();
                    break;
            }
        },
        //录音
        record: function (callback) {
            xiEnglish.setRecordCallback(function (key, duration) {
                callback(key, duration);
            });
            xiEnglish.record();
        },
        //停止
        endrecord: function () {
            xiEnglish.endRecord();
        },

        //播放
        play: function (file, callback) {
            xiEnglish.setPlayCallback(function () {
                callback();
            });
            return xiEnglish.play(file ? file : "http://121.14.117.254:8081/playenglish/845670E78D5CF3D02350A8BE8A61E19C827117232.mp3")
        },
        //暂停
        pause: function () { xiEnglish.pause(); },
        //恢复
        resume: function () { xiEnglish.resume(); },
        //停止
        stop: function () { xiEnglish.stop(); },
        //当前播放时间
        getposition: function () { xiEnglish.getPosition(); },
        //总时长
        getduration: function () { xiEnglish.getDuration(); },
        //验证版本
        checkheightversion: function () {
            var version = cwBrowerAtiveX.version();
            if (!version) {
                return false;
            }
            return version >= cwBrowerAtiveX.currVersion;
        }
    };

    window.cwBrowerAtiveX = cwBrowerAtiveX;

    return cwBrowerAtiveX;
});