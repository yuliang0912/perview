/*
 * Name: SewiseSubtitles framework 1.0.0
 * Author: Jack Zhang
 * Website: http://www.sewise.com/
 * Date: July 25, 2014
 * Copyright: 2013-2014, Sewise
 * Mail: jackzhang1204@gmail.com
 * 
 */

(function (root) {
    var $ = root.jquery || root.jQuery || root.Zepto || null;
    function isType(o) {
        if (Object.prototype.toString.apply(o) === '[object Array]') {
            return 'Array'
        } else {
            return typeof (o)
        }
    }
    //格式化url
    function parseURL(url) {
        var inc = 0;
        var a = document.createElement('a');
        a.href = url;
        var params = (function () {
            var ret = {}, seg = a.search.replace(/^\?/, '').split('&'),
				len = seg.length,
				i = 0,
				s;
            for (; i < len; i++) {
                if (!seg[i]) {
                    continue;
                }
                s = seg[i].split('=');
                ret[s[0]] = s[1];
                inc++;
            }
            return ret;
        })()
        params.path = a.pathname;
        params.search = a.search;

        //if (a.port && a.port !== "") params.porthost = a.hostname + ":" + a.port;
        if (a.port && a.port !== "0" && a.port !== "") params.porthost = a.hostname + ":" + a.port;
        else params.porthost = a.hostname;

        var srcPath = url.replace(/\?.*/, '');
        var lastIndex = srcPath.lastIndexOf("/");
        params.localPath = srcPath.slice(0, lastIndex + 1);

        params.file = a.pathname.match(/.[^\/]*\.[\w\d\_\.]+/) || ''

        if (inc) {
            return params;
        } else {
            return null
        }
    }
    //定位当前脚本所在位置
    var _rndID = '_rnd' + (new Date()).getTime(); //产生一个即时id
    var tmpHtml = '<p id="' + _rndID + '"></p>'
    //document.write(tmpHtml);
    $("#divVideo").append(tmpHtml);
    var rndElement = document.getElementById(_rndID)
    var Target = rndElement.parentNode;
    //var script = rndElement.previousSibling;
    var script = $(rndElement).siblings().first();
    Target.removeChild(rndElement); //从document树中移除，使之形成孤岛，不在document树中，但还在document内存中
    rndElement = null; //从环境断开栈与堆之间的联系，从而释放内存
    //var queryData = parseURL(script.src);
    var queryData = parseURL(script[0].src);
    window.SewiseSubtitles = {};
    window.SewiseSubtitles.queryData = queryData;

    if (!queryData) {
        return false;
    };
    var playerElement = document.createElement('div');
    playerElement.className = 'player_layout'
    var subtitlesElement = document.createElement('div');
    subtitlesElement.className = 'subtitles_layout'
    Target.appendChild(playerElement);
    Target.appendChild(subtitlesElement);
    //语言设置
    queryData.lang = queryData.sublang ? queryData.sublang : 'zh_cn';
    var plsearch = queryData.search;
    // en;zh_cn 后续可能有其他的

    //字幕引擎
    var TSubtitlesShowed = function () {
        this.handle = null;
        this.debug = 0; //是否开启调试
        this.data = []; //保存数据
        this.TmpDat = []; //保存临时数据
        this.prevTime = -1; //上一个时间
        this.memTime = 0; //记忆时间
        this.lightDoms = []; //当前高亮元素集合
        this.options = {}; //配置项
        this.inited = false;
        this.plight = null;
    }
    TSubtitlesShowed.prototype = {
        init: function (data) {
            var _ost = this;
            _ost.options = {
                stopEvent: null, //字幕滚动结束后要执行的动作 
                htmlEndEvent: null, //加载字幕完成后要执行的动作,
                jumpEvent: null, //每次跳时间时的事件
                lineEvent: null,
                data: [], //必须
                target: null, //必须
                highlight: 'highlight', //高亮的类名
                subtitles: 'subtitles', //字幕预存区域的类名
                wrap: 'div',
                line: 'p',
                word: 'span'
            };
            //this.options = $.extend({}, _ost.options, data);
            for (var i in _ost.options) {
                if (_ost.options.hasOwnProperty(i)) {
                    if (data[i]) {
                        _ost.options[i] = data[i];
                    };
                }
            }
            _ost.data = _ost.clone(_ost.options.data)
            _ost.TmpDat = _ost.clone(_ost.options.data);
            _ost.html();
            _ost.inited = true;
        },
        clone: function (o) {
            var _o = o.constructor === Array ? [] : {};
            for (var i in o) {
                if (o.hasOwnProperty(i)) {
                    _o[i] = typeof o[i] === "object" ? this.clone(o[i]) : o[i];
                }
            }
            return _o;
        },
        jump: function (currentTime) {
            //跳到指定时间	
            var _ost = this;
            if (!_ost.inited) {
                return
            };
            var time = currentTime && parseInt(currentTime) ? parseInt(currentTime) : 0
            if (_ost.prevTime > time) {
                var tArr = _ost.clone(_ost.data)
                _ost.TmpDat = _ost.clone(tArr)
                _ost.memTime = 0;
            };
            _ost.prevTime = time + 0;
            num = 0;
            baseline = -1;
            var fsTime = 0;
            nextTime = 0, itemTime = 0, maxTime = 0;
            while (_ost.TmpDat[1].length > 0) {
                var len = _ost.TmpDat[1].length
                fsTime = _ost.TmpDat[1][0][0];
                var TmpDatItem = _ost.clone(_ost.TmpDat[1][0][1]);
                //大到小
                TmpDatItem.sort(function (n, p) {
                    return n[0] < p[0]
                });
                maxTime = fsTime + TmpDatItem[0][0];
                if (len > 1) { //数量大于1
                    nextTime = _ost.TmpDat[1][1][0];

                    if (time >= nextTime) { //时间大于下一个

                        _ost.shift(_ost.TmpDat[1]) //排出第一个元素
                    } else {
                        if (time > maxTime) {
                            _ost.shift(_ost.TmpDat[1]) //排出第一个元素	
                            break;
                        };
                        if (_ost.memTime == fsTime) {
                            break;
                        }
                        //循环处理
                        var ishell = 0;
                        for (var i = TmpDatItem.length - 1; i >= 0; i--) {
                            //非统一音长
                            //itemTime = fsTime + TmpDatItem[i][0];
                            // time >= fsTime&&itemTime>time;
                            //统一音长
                            if (time >= fsTime) {
                                //添加高亮								
                                _ost.showlight(TmpDatItem[i][1], TmpDatItem[i][2]);
                                ishell++;
                            };

                        };
                        if (ishell) {
                            _ost.memTime = fsTime;

                            TmpDatItem.sort(function (n, p) {
                                return p[1] >= n[1] && p[2] > n[2]
                            });
                            var Dom = _ost.dquery(TmpDatItem[0][1], TmpDatItem[0][2]);
                            _ost.event('jumpEvent', [Dom, TmpDatItem[0][1], TmpDatItem[0][2]]);
                        };
                        break;
                    }
                } else { //等于1					
                    //循环处理
                    for (var i = TmpDatItem.length - 1; i >= 0; i--) {
                        itemTime = fsTime + TmpDatItem[i][0];
                        if (time >= fsTime && itemTime > time) {
                            //添加高亮
                            _ost.showlight(TmpDatItem[i][1], TmpDatItem[i][2])
                        };
                    };
                    TmpDatItem.sort(function (n, p) {
                        return p[1] >= n[1] && p[2] > n[2]
                    });

                    var Dom = _ost.dquery(TmpDatItem[0][1], TmpDatItem[0][2]);
                    _ost.event('jumpEvent', [Dom, TmpDatItem[0][1], TmpDatItem[0][2]])
                    _ost.event('stopEvent', [Dom, TmpDatItem[0][1], TmpDatItem[0][2]])
                    _ost.TmpDat[1].splice(0, 1);
                    break;
                }
            };
            if (_ost.TmpDat[1].length == 0) {
                _ost.closelight()
            };
        },
        shift: function (data) {

            var _ost = this;
            _ost.memTime = 0;
            //移除高亮
            _ost.closelight()
            data.splice(0, 1);
            return data;
        },
        html: function () {
            var _ost = this,
				target = _ost.options.target,
				data = _ost.options.data,
				wrap = _ost.options.wrap,
				line = _ost.options.line;
            //word=  _ost.options.word;
            var rnd = 'rnd' + (new Date()).getTime();
            var el = '';
            for (var i = 0; i < data[0].length; i++) {
                el += '<' + line + '>' + data[0][i] + '</' + line + '>'
            };
            target.innerHTML = '<' + wrap + ' id="' + rnd + '" class="' + _ost.options.subtitles + '">' + el + '</' + wrap + '>';
            _ost.wrap = document.getElementById(rnd);
            _ost.event('htmlEndEvent')
        },
        dquery: function (first, second) {
            if (isNaN(first) || isNaN(second)) {
                return null;
            };
            var _ost = this;
            var Target = _ost.wrap;
            var chid = first < Target.childNodes.length ? Target.childNodes[first] : null;
            if (!chid) {
                return null;
            };
            var grandson = second < chid.childNodes.length ? chid.childNodes[second] : null;
            if (!grandson) {
                return null;
            } else {
                return grandson;
            }
        },
        showlight: function (first, second) {
            var _ost = this;
            var dom = _ost.dquery(first, second);
            var pom = dom.parentNode;
            var pc = pom.className;
            if (!dom) {
                return
            };
            var cn = dom.className;
            var light = _ost.options.highlight;
            var reg = new RegExp(light);
            if (!reg.test(cn)) {
                var ca = cn.split(' ');
                ca.push(light);
                dom.className = ca.join(' ');
            };
            if (!reg.test(pc)) {
                if (_ost.plight) {
                    _ost.plight.className = '';
                    _ost.event('lineEvent', [pom])
                };
                var ca = pc.split(' ');
                ca.push(light);
                pom.className = ca.join(' ');
                _ost.plight = pom;
            };
            _ost.lightDoms.push(dom);
        },
        closelight: function () {
            var _ost = this;
            var light = _ost.options.highlight;
            var reg = new RegExp(light, 'gm');
            for (var i = 0; i < _ost.lightDoms.length; i++) {
                _ost.lightDoms[i].className = _ost.lightDoms[i].className.replace(reg, '');
            };
            _ost.lightDoms.length = 0;
        },
        event: function (evt, args) {
            try {
                var fn = this.options[evt] || null;
                if (fn && typeof (fn) == 'function') {
                    if (args) {
                        fn.apply(fn, args);
                    } else {
                        fn.apply(fn, []);
                    }
                };
            } catch (e) { };
        }
    }
    //数据请求
    var getSubtitleData = function (sourceid, lang, callback) {
        var serverPath;
        //if(queryData.debug === "true"){
        //	queryData.porthost = "192.168.1.24";
        //	sourceid = queryData.recordid;
        //}
        //serverPath = 'http://' + queryData.porthost + '/service/playerapi/?do=getsubtitle';
        serverPath = 'http://video1.ciwong.net/service/playerapi/?do=getsubtitle';
        $.ajax({
            url: serverPath,
            type: 'GET',
            dataType: 'jsonp',
            jsonp: 'jsproxy',
            contentType: "application/json; charset=utf8",
            data: {
                "do": "getsubtitle",
                'sourceid': sourceid
            },
            success: function (data) {
                if (data.errors) {
                    return;
                };
                if (isType(data.words) != 'Array') {
                    return;
                };
                if (callback && typeof (callback) == 'function') {
                    callback(data.words, data);
                };
            },
            error: function (xhr, ajaxOptions, thrownError) {
                window.location.reload();
            }
        });
    }
    // 重建字幕数据 
    var rebuildSubData = function (data) {
        var time = 0;
        var Arr = [],
			Ahtml = [],
			rtArr = [];
        var html = '';
        var njson = {}, key = '';
        var index = 0,
			num = 0;
        for (var i = 0; i < data.length; i++) {
            html = ''
            for (var j = 0; j < data[i].length; j++) {
                time = parseInt((parseFloat(data[i][j].st) * 1000).toFixed(0));
                etime = parseInt((parseFloat(data[i][j].et) * 1000).toFixed(0));
                num = etime - time;
                key = time.toString();
                if (num > 0) {
                    rtArr.push([time, i, j, num])
                    html += '<span time="' + time + '">' + data[i][j].token + ' </span>';
                    if (!njson[key]) {
                        njson[key] = [];
                    };
                    njson[key].push([num, i, j]);
                } else {
                    html += '<span>' + data[i][j].token + ' </span>';
                }
                index++;
            };
            Ahtml.push(html);
        };
        //Array化
        var rtArr = [];
        for (var i in njson) {
            if (njson.hasOwnProperty(i)) {
                rtArr.push([parseInt(i), njson[i]])
            }
        }
        //排序小到大
        /*rtArr.sort(function(next, prev) {
			return next[0] > prev[0]
		});*/
        Arr[0] = Ahtml;
        Arr[1] = rtArr;
        return Arr;
    }
    //挂载到window下
    var oSubtitlesShow = root.oSubtitlesShow = (function () {
        var ots = new TSubtitlesShowed();
        var inited = false
        root._engine_of_TSubtitlesShowed = ots;
        return {
            init: function (options) {
                if (inited) {
                    return
                };
                ots.init(options)
                inited = true;
            },
            run: function (time) {
                ots.jump(time)
            }
        }
    })()
    //run\执行程序初始化
    var run = function () {
        var Target = subtitlesElement;
        getSubtitleData(queryData.sourceid, queryData.lang, function (wordsData) {
            var fd = rebuildSubData(wordsData);
            oSubtitlesShow.init({
                'target': Target,
                'data': fd,
                'htmlEndEvent': function () {
                    //事件--html加载完成时
                    if (queryData.click && queryData.click == 'false') {
                        return false;
                    };
                    $(Target).find('span').click(function () {
                        var time = $(this).attr('time');
                        if (SewisePlayer && SewisePlayer.doSeek) {
                            SewisePlayer.doSeek(time / 1000);
                        };
                    })
                },
                'jumpEvent': function () { }, //每次跳转一个位置时的时间
                'lineEvent': function (pdom) {  //每次跳转一行时的时间
                    if (queryData.runline && queryData.runline == 'false') {
                        return false;
                    };
                    if (Target.scrollHeight > Target.clientHeight) {
                        var ch = Target.clientHeight / 2
                        var h = pdom.offsetTop - ch - Target.offsetTop;
                        if (h > 0) {
                            if (Target.scrollTopMax < h) {
                                Target.scrollTop = Target.scrollTopMax
                            } else {
                                Target.scrollTop = h
                            }
                        } else {
                            Target.scrollTop = 1
                        }
                    };
                }
            })
        });
    }
    root.onPlayTime = function (time) {
        var t = time ? parseFloat(time) : 0;
        oSubtitlesShow.run(t * 1000)
    };

    sws_loadPlayer(playerElement, plsearch, function () {
        if ($) {
            run();
        } else {
            var $sc = document.createElement('script');
            //var src = script.src.replace(/\?.*/,'');
            //$sc.src = src.replace(queryData.file, '/js/jquery.min.js');

            $sc.src = queryData.localPath + 'js/jquery.min.js';
            //console.log("$sc.src: " + $sc.src);

            document.getElementsByTagName('head')[0].appendChild($sc);
            var load = function () {
                $ = root.jQuery;
                run();
            }
            //$sc.onload = $sc.onreadystatechange = load;
            $sc.onload = $sc.onreadystatechange = function () {
                if (!$sc.readyState || $sc.readyState == "loaded" || $sc.readyState == 'complete') {
                    load();
                }
            }

        }
    })

})(window || this);
//style 样式

(function (root) {
    if (root.document) {
        var styleText;
        if (root.document.URL.lastIndexOf("videopreview") > 0) {
            styleText = 'span.highlight{ background-color: #000; color: #4f1; } p.highlight{ background-color: #E8E8E8; color: inherit; } .player_layout{ height: 100%; width: 92%; float: left;margin-left: 8px; } .subtitles_layout{ float: right; height: 100%; width: 36%; overflow: auto; } ';
        } else {
            styleText = 'span.highlight{ background-color: #000; color: #4f1; } p.highlight{ background-color: #E8E8E8; color: inherit; } .player_layout{ height: 100%; width: 60%; float: left; } .subtitles_layout{ float: right; height: 100%; width: 36%; overflow: auto; } ';
        }
        var style = document.createElement('style');
        style.className = 'Subtitles_Style_U_Can_Custom';
        style.setAttribute("type", "text/css");

        if (style.textContent) {
            style.textContent = styleText;
            //console.log("What");
        } else if (style.styleSheet) {
            //IE
            style.styleSheet.cssText = styleText;
            //console.log("IE");
        } else {
            //Chrome, Firefox
            style.innerHTML = styleText;
            //console.log("Chrome, Firefox");
        }

        /*if(style.styleSheet){
			//IE
			style.styleSheet.cssText = styleText;
			//console.log("IE");
		}else{
			//Chrome, Firefox
			style.innerHTML = styleText;
			//console.log("Chrome, Firefox");
		}*/

        document.getElementsByTagName('head')[0].appendChild(style);
    };
})(window || this);


//加载播放器
function sws_loadPlayer(dom, search, fn) {
    //console.log(window.SewiseSubtitles.queryData.localPath);
    //var url = window.SewiseSubtitles.queryData.localPath + 'player/sewise.player.min.js' + search;   
    var url;
    if (search.lastIndexOf('onplay') > 1) {
        url = "http://video1.ciwong.net/libs/swfplayer/player/" + '/sewise.player.min.js' + search;
    } else {
        url = "http://video1.ciwong.net/libs/swfplayer/subtitlePlayer/player" + '/sewise.player.min.js' + search;
    }
    var script = document.createElement('script');
    script.src = url;

    //script.onload =function () {
    script.onload = script.onreadystatechange = function () {
        if (!script.readyState || script.readyState == "loaded" || script.readyState == 'complete') {
            if (fn && typeof (fn) == 'function') {
                fn();
            };
        }
    }
    dom.appendChild(script);
}