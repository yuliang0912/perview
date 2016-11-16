define(function (r, exports) {

    var ciwong = require("ciwong"), $ = require("jquery"), ko = require("ko"), question = require("mobilequestion"), dialog = require("dialog");

    var methods = {
        isPc: function () {
            var userAgentInfo = navigator.userAgent;
            var agents = ["Android", "iPhone", "SymbianOS", "Windows Phone", "iPad", "iPod"];
            var flag = true;
            $(agents).each(function (index, item) {
                if (userAgentInfo.indexOf(item) > -1) {
                    flag = false;
                }
            });
            return flag;
        },
        convertToChineseNum: function (num) {
            num = num.toString();
            var ary0 = ["零", "一", "二", "三", "四", "五", "六", "七", "八", "九"],
                ary1 = ["", "十", "百", "千"],
                ary2 = ["", "万", "亿", "兆"];

            function strrev() {
                var ary = [];
                for (var i = num.length; i >= 0; i--) {
                    var n = num.charAt(i);
                    if (n != "") {
                        ary.push(n);
                    }
                }
                return ary.join("");
            }
            var ary = strrev(), zero = "", newary = "", i4 = -1;
            for (var i = 0; i < ary.length; i++) {
                if (i % 4 == 0) {
                    i4++;
                    newary = ary2[i4] + newary;
                    zero = "";
                }
                if (ary.charAt(i) == '0') {
                    switch (i % 4) {
                        case 0:
                            break;
                        default:
                            if (ary.charAt(i - 1) != '0') {
                                zero = "零";
                            }
                            break;
                    }
                    newary = zero + newary;
                    zero = '';
                }
                else {
                    newary = ary0[parseInt(ary.charAt(i))] + ary1[i % 4] + newary;
                }
            }
            if (newary.indexOf("一十") == 0 || newary.indexOf("零") == 0) {
                newary = newary.substr(1);
            }
            return newary;
        },
        findUserAnswer: function (userAnswers, versionId, moluleId) {
            var result = $.grep(userAnswers, function (m) { return m.version_id == versionId; })[0];
            if (result && !$.isArray(result.answers)) {
                if (moluleId == "e9430760-9f2e-4256-af76-3bd8980a9de4") {
                    result.answers = $.map(JSON.parse(result.answers), function (n, i) {
                        var content = n.audio_url != "" ? n.audio_url : n.option_id != "" ? n.option_id : n.blank_content;
                        return { assess: result.assess, sid: i, content: content, item_score: result.score };
                    });
                } else {
                    result.answers = JSON.parse(result.answers);
                }
            }
            return result;
        },
        setUserAnswer: function (data, questions, moduleId) {
            questions && $.each(questions.children, function () {
                this.userAnswer = methods.findUserAnswer(data, this.version_id, moduleId);
                this.children && this.children.length > 0 && methods.setUserAnswer(data, this, moduleId);
            });
        },
        getUserAnswer: function (doId, answerType) {
            var userAnswer;
            ciwong.ajax.getJSON("/work/GetAnswer", { doId: doId, answerType: answerType }, function (data) {
                userAnswer = data;
            }, null, false).done(function () {
                ciwong.colseLoading();
            });
            return userAnswer;
        },
        initCorrectHtmlEventHandler: function (id, ques, currentCorrectParams) {
            var index = 0;
            var currentMark = $('#manual' + id);
            index = parseInt(currentMark.attr("index"));
            if (ques.qtype == 4) {
                $.each(ques.children, function (i) {
                    if (i = index - 1) {
                        index = 1;
                        ques = ques.children[i];
                        return false;
                    }
                });
            }
            currentCorrectParams.index = index;
            currentCorrectParams.isShow(currentCorrectParams);
            currentCorrectParams.lastMark = currentMark;
        },
        DataBindPerformanceOptimise: function (model, paperData) {
            //为了提高第一屏数据加载的性能，首先只加载第一道大题
            var firstPart = [];
            firstPart.push(paperData.parts.shift());
            var otherParts = [].concat(paperData.parts);
            paperData.parts = firstPart;
            model.paperInitStart(paperData);
            model.paperInit(true, paperData.parts);
            paperData.parts = firstPart;
            model.paper(paperData);
            ////model.paperInitEnd(paperData);
            ////延迟加载非第一屏的其余大题
            window.setTimeout(function () {
                paperData.parts = model.lastDataSource.parts.concat(otherParts);
                model.paperInitStart(paperData);
                paperData.parts = firstPart.concat(model.paperInit(false, otherParts));
                model.paper(paperData);
                model.paperInitEnd(paperData);
            }, 100);
            //model.paper(paperData);
        }
    };
    var viewModel = {
        preView: function (settingOptions) {
            var model = this,
                settings = settingOptions || {};
            settings.isPc = methods.isPc();
            model.lastDataSource = undefined;
            model.allSingleQuestions = [];

            model.viewType = ko.observable(1);//1:预览模式 2:结果模式

            model.paper = ko.observable();
            //model.paper.subscribe(function (newPaper) {
            //	model.lastDataSource = ko.toJS(newPaper);

            //	newPaper.parts && $.each(newPaper.parts, function (i) {
            //	    this.module_type_name = "第{0}题 {1}".format(methods.convertToChineseNum(i + 1), this.module_type_name);
            //	    settings.dataSource = this.children;
            //		if (model.viewType() == 1) {
            //		    this.children = ko.observable(new question.viewModel.preView(settings));//{ dataSource: this.children }
            //		} else {
            //		    this.children = ko.observable(new question.viewModel.resultView(settings));//{ dataSource: this.children, isShowAnswer: settings.isShowAnswer }
            //		}
            //	});

            //	if (model.allSingleQuestions.length == 0) {
            //		var allQuestions = [];
            //		$.each(model.lastDataSource.parts, function () { allQuestions = allQuestions.concat(this.children) });
            //		$.each(allQuestions, function (i, item) {
            //			if (item.children.length > 0) {
            //			    model.allSingleQuestions = model.allSingleQuestions.concat(item.children);
            //			} else {
            //				model.allSingleQuestions.push(item);
            //			}
            //		});
            //	}
            //});

            model.paperInitStart = function (newPaper) {
                model.lastDataSource = ko.toJS(newPaper);
            };
            model.paperInit = function (isFirst, parts) {
                parts && $.each(parts, function (i) {
                    this.module_type_name = "第{0}题 {1}".format(isFirst ? '一' : methods.convertToChineseNum(i + 2), this.module_type_name);
                    settings.dataSource = this.children;
                    if (model.viewType() == 1) {
                        this.children = ko.observable(new question.viewModel.preView(settings));
                    } else {
                        settings.isShowAnswer = true, settings.isShowThinking = true;
                        this.children = ko.observable(new question.viewModel.resultView(settings));
                    }
                });
                return parts;
            };

            model.paperInitEnd = function () {
                var allQuestions = [];
                $.each(model.lastDataSource.parts, function () { allQuestions = allQuestions.concat(this.children) });
                $.each(allQuestions, function (i, item) {
                    if (item.children.length > 0) {
                        model.allSingleQuestions = model.allSingleQuestions.concat(item.children);
                    } else {
                        model.allSingleQuestions.push(item);
                    }
                });
            };

            model.afterRender = function () {
                $("table").css({ width: 'auto' });
                !settings.isPc && audiojs.createAll();//如果是移动端，ko渲染完成之后通过audiojs实例化所有音频对象
                settings.afterRender && settings.afterRender.call(model);
            };

            model.getUserAnswer = function () {
                var userAnswer = [];
                if (model.viewType() == 1) {
                    $.each(model.paper().parts, function () {
                        $.merge(userAnswer, this.children().getAnswers());
                    });
                }
                return userAnswer;
            };

            model.correctQuestion = function () {
                model.viewType() == 1 && ciwong.ajax.postJSON("/question/Correct", { content: JSON.stringify(model.getUserAnswer()) }, function (data) {
                    $.each(model.lastDataSource.parts, function (i, item) {
                        methods.setUserAnswer(data, item);
                    });
                    model.viewType(2);
                    methods.DataBindPerformanceOptimise(model, model.lastDataSource); //model.paper(model.lastDataSource);
                    settings.correctComplete && settings.correctComplete(data);
                }, '提交成功', null, true, function () { ciwong.ajax.isPreventRequest = true; });
            };

            model.submitWork = function () {
                if (model.viewType() == 1 && settings.submitWork) {
                    settings.submitWork.call(model);
                }
            };

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
            ///e20501eccc0d4c67b64d7c3f6e8b336a
            //b7303ec7d9d8503e9e4dbb710395c89a
            //c8f77017f9ca53001b094394097a1274
            //var xx = { "data": { "title": "七年级 暑假合刊 第43期 拓展?听力篇", "ref_score": 100.0, "curriculum_id": -1, "parts": [{ "sid": 0, "module_type_name": "Listening 1", "module_type_url": "", "children": [{ "sid": 0, "stem": "<strong>【步骤练习】<br />听英语歌曲，熟记歌词。</strong><br /><strong>步骤一：听歌曲（完整听三遍，不要中断，不要看文字），直到大体听懂大意为止。</strong><br /><br /><strong>步骤二：逐句听歌曲录音，将歌词补充完整，每空填一个单词。</strong><br /><strong>She&rsquo;ll be coming round the mountain</strong><br />She&rsquo;ll be coming round the {#blank#}1{#/blank#}&nbsp;when she comes<br />She&rsquo;ll be driving six white {#blank#}2{#/blank#}&nbsp;when she comes<br />\nWe&rsquo;ll all go down to {#blank#}3{#/blank#}&nbsp;her when she comes<br />\n<br />\n<strong>步骤三：看视频内容，根据视频的歌词文字记录自己听写的错误。</strong><br />\n{#video#}http://dispatcher.video.qiyi.com/disp/shareplayer.swf?vid=bc2a34e718d7ea45b2d76b7f452791b6&tvId=3199383909&cnId=23&coop=ugc_openapi_ciwongxx&cid=qc_100001_300089&bd=1&autoChainPlay=0&autoplay=1&showRecommend=0&video_id=14d0a77789e8461dada4015dba9e17d4{#/video#}<br />\n{#blank#}4{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤四：把步骤三中记录的错误进行归类。</strong><br />\nA. 听懂但拼写错误：{#blank#}5{#/blank#}&nbsp;<br />\nB. 误听：{#blank#}6{#/blank#}&nbsp;<br />\nC. 生词：{#blank#}7{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤五：查词典，搞清楚下面这些词汇和短语在本歌曲中的准确含义。</strong><br />\nA. come round&nbsp;{#blank#}8{#/blank#}&nbsp;<br />\nB. drive&nbsp;{#blank#}9{#/blank#}&nbsp;<br />\nC. go down&nbsp;{#blank#}10{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤六：对照抄写的歌词，完整地听两遍歌曲，不要中断。</strong>", "qtype": 3, "is_objective": false, "parent_version": "0", "question_ref_sorce": 20.0, "ref_info": { "answers": ["mountain", "horses", "meet", "略", "略", "略", "略", "略", "略", "略"], "solving_idea": "", "attachments": [] }, "options": [{ "id": "950460955557056683", "stem": "mountain", "attachments": [] }, { "id": "950460955557056684", "stem": "horses", "attachments": [] }, { "id": "950460955557056685", "stem": "meet", "attachments": [] }, { "id": "950460955557056686", "stem": "略", "attachments": [] }, { "id": "950460955557056687", "stem": "略", "attachments": [] }, { "id": "950460955557056688", "stem": "略", "attachments": [] }, { "id": "950460955557056689", "stem": "略", "attachments": [] }, { "id": "950460955557056690", "stem": "略", "attachments": [] }, { "id": "950460955557056691", "stem": "略", "attachments": [] }, { "id": "950460955557056692", "stem": "略", "attachments": [] }], "attachments": [{ "file_type": 2, "position": 4, "file_url": "http://rimg2.ciwong.net/cwf/6v68/tools/editor/15623/000/10000/d9a5c0c470a6617a46f4b267cb0fccbc.mp3" }], "children": [], "id": "855789107374586459", "version_id": "950460940484938225" }, { "sid": 1, "stem": "<strong>【步骤练习】<br />\n看电影视频，回答问题。</strong><br />\n<strong>步骤一：播放没有字幕的视频（完整看两遍，不要停顿），直到看懂大概意思。</strong><br />\n{#video#}http://dispatcher.video.qiyi.com/disp/shareplayer.swf?vid=054e9955bdb4732ef02c3d7044a916f5&tvId=3199395509&cnId=23&coop=ugc_openapi_ciwongxx&cid=qc_100001_300089&bd=1&autoChainPlay=0&autoplay=1&showRecommend=0&video_id=5892de70fdfe492ba5cd6f4d07453d1a{#/video#}<br />\n<br />\n<strong>步骤二：看电影视频，回答问题。</strong><br />\nWhere do they find the news about the Hyenas?<br />\n{#blank#}1{#/blank#}&nbsp;<br />\n&nbsp;<br />\n<strong>步骤三：播放有字幕的视频，修改自己的答案。</strong><br />\n?{#video#}http://dispatcher.video.qiyi.com/disp/shareplayer.swf?vid=94c1b56faf09bae02b7d164fa1ca0294&tvId=3199402209&cnId=23&coop=ugc_openapi_ciwongxx&cid=qc_100001_300089&bd=1&autoChainPlay=0&autoplay=1&showRecommend=0&video_id=7c3a065c9d7d4a31b76797adc021224c{#/video#}", "qtype": 3, "is_objective": false, "parent_version": "0", "question_ref_sorce": 10.0, "ref_info": { "answers": ["From the underground."], "solving_idea": "", "attachments": [] }, "options": [{ "id": "950460955557056213", "stem": "From the underground.", "attachments": [] }], "attachments": [], "children": [], "id": "855789107374586513", "version_id": "950460940484938088" }] }, { "sid": 1, "module_type_name": "Listening 2", "module_type_url": "", "children": [{ "sid": 0, "stem": "<strong>【步骤练习】<br />\n听英语歌曲，熟记歌词。</strong><br />\n<strong>步骤一：听歌曲（完整听三遍，不要中断，不要看文字），直到大体听懂大意为止。</strong><br />\n<br />\n<strong>步骤二：逐句听歌曲录音，将歌词补充完整，每空填一个单词。</strong><br />\n<strong>Big Big World</strong>&mdash;<strong>Emilia</strong><br />\nI can see the first {#blank#}1{#/blank#}&nbsp;falling<br />\nIt&rsquo;s all {#blank#}2{#/blank#}&nbsp;and nice<br />\nIt&rsquo;s so very cold&nbsp;{#blank#}3{#/blank#}&nbsp;<br />\nOutside it&rsquo;s now&nbsp;{#blank#}4{#/blank#}&nbsp;<br />\nI have your {#blank#}5{#/blank#}&nbsp;around me ooooh like fire<br />\n<br />\n<strong>步骤三：看视频内容，根据视频的歌词文字记录自己听写的错误。</strong><br />\n{#video#}http://player.56.com/3000002473/open_MTM3NzIxNTU5.swf{#/video#}?<br />\n{#blank#}6{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤四：把步骤三中记录的错误进行归类。</strong><br />\nA. 听懂但拼写错误：{#blank#}7{#/blank#}&nbsp;<br />\nB. 误听：{#blank#}8{#/blank#}&nbsp;<br />\nC. 生词：{#blank#}9{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤五：查词典，搞清楚下面这些词汇和短语在本歌曲中的准确含义。</strong><br />\nA. big&nbsp;{#blank#}10{#/blank#}&nbsp;<br />\nB. miss&nbsp;{#blank#}11{#/blank#}&nbsp;<br />\nC. end&nbsp;{#blank#}12{#/blank#}&nbsp;<br />\nD. feel inside&nbsp;{#blank#}13{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤六：对照抄写的歌词，完整地听两遍歌曲，不要中断。</strong>", "qtype": 3, "is_objective": false, "parent_version": "0", "question_ref_sorce": 26.0, "ref_info": { "answers": ["leaf", "yellow", "outside", "raining", "arms", "略", "略", "略", "略", "略", "略", "略", "略"], "solving_idea": "", "attachments": [] }, "options": [{ "id": "950460955557056693", "stem": "leaf", "attachments": [] }, { "id": "950460955557056694", "stem": "yellow", "attachments": [] }, { "id": "950460955557056695", "stem": "outside", "attachments": [] }, { "id": "950460955557056696", "stem": "raining", "attachments": [] }, { "id": "950460955557056697", "stem": "arms", "attachments": [] }, { "id": "950460955557056698", "stem": "略", "attachments": [] }, { "id": "950460955557056699", "stem": "略", "attachments": [] }, { "id": "950460955557056700", "stem": "略", "attachments": [] }, { "id": "950460955557056701", "stem": "略", "attachments": [] }, { "id": "950460955557056702", "stem": "略", "attachments": [] }, { "id": "950460955557056703", "stem": "略", "attachments": [] }, { "id": "950460955557056704", "stem": "略", "attachments": [] }, { "id": "950460955557056705", "stem": "略", "attachments": [] }], "attachments": [{ "file_type": 2, "position": 4, "file_url": "http://rimg2.ciwong.net/cwf/6v68/tools/editor/15623/000/10000/bb04228aca2825c7c4880bfa683260ea.mp3" }], "children": [], "id": "855789107374586527", "version_id": "950460940484938226" }, { "sid": 1, "stem": "<strong>【步骤练习】<br />\n看电影视频，回答问题。</strong><br />\n<strong>步骤一：播放没有字幕的视频（完整看两遍，不要停顿），直到看懂大概意思。</strong><br />\n?{#video#}http://player.56.com/3000002473/open_MTM3NzIxNTE1.swf{#/video#}<br />\n<br />\n<strong>步骤二：看电影视频，回答问题。</strong><br />\nWhy couldn&rsquo;t Simba go there?<br />\n{#blank#}1{#/blank#}&nbsp;<br />\n&nbsp;<br />\n<strong>步骤三：播放有字幕的视频，修改自己的答案。</strong><br />\n?{#video#}http://player.56.com/3000002473/open_MTM3NzIxNTM2.swf{#/video#}", "qtype": 3, "is_objective": false, "parent_version": "0", "question_ref_sorce": 10.0, "ref_info": { "answers": ["Because it&rsquo;s far too dangerous."], "solving_idea": "", "attachments": [] }, "options": [{ "id": "950460955557056592", "stem": "Because it&rsquo;s far too dangerous.", "attachments": [] }], "attachments": [], "children": [], "id": "855789107374586530", "version_id": "950460940484938200" }] }, { "sid": 2, "module_type_name": "Listening 3", "module_type_url": "", "children": [{ "sid": 0, "stem": "<strong>【步骤练习】<br />\n听英语歌曲，熟记歌词。</strong><br />\n<strong>步骤一：听歌曲（完整听三遍，不要中断，不要看文字），直到大体听懂大意为止。</strong><br />\n<br />\n<strong>步骤二：逐句听歌曲录音，将歌词补充完整，每空填一个单词。</strong><br />\n<strong>Listening for the weather</strong><br />\n<em>By Bic Runga</em><br />\nI&rsquo;ll call you on the&nbsp;{#blank#}1{#/blank#}&nbsp;<br />\nAnd this busy inner&nbsp;{#blank#}2{#/blank#}&nbsp;<br />\nAnd I&rsquo;m sure that as I&rsquo;m&nbsp;{#blank#}3{#/blank#}&nbsp;<br />\nIn a supermarket checkout or the&nbsp;{#blank#}4{#/blank#}&nbsp;<br />\nAnd the days are getting {#blank#}5{#/blank#}&nbsp;but that&rsquo;s alright with me<br />\n<br />\n<strong>步骤三：看视频内容，根据视频的歌词文字记录自己听写的错误。</strong><br />\n{#video#}http://player.56.com/3000002473/open_MTM3NzIxNTY3.swf{#/video#}?<br />\n{#blank#}6{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤四：把步骤三中记录的错误进行归类。</strong><br />\nA. 听懂但拼写错误：{#blank#}7{#/blank#}&nbsp;<br />\nB. 误听：{#blank#}8{#/blank#}&nbsp;<br />\nC. 生词：{#blank#}9{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤五：查词典，搞清楚下面这些词汇和短语在本歌曲中的准确含义。</strong><br />\nA. listen for&nbsp;{#blank#}10{#/blank#}&nbsp;<br />\nB. blow in&nbsp;{#blank#}11{#/blank#}&nbsp;<br />\nC. cling to&nbsp;{#blank#}12{#/blank#}&nbsp;<br />\nD. restless&nbsp;{#blank#}13{#/blank#}&nbsp;<br />\nE. nothing much&nbsp;{#blank#}14{#/blank#}&nbsp;<br />\nF. grow old&nbsp;{#blank#}15{#/blank#}&nbsp;<br />\n<br />\n<strong>步骤六：对照抄写的歌词，完整地听两遍歌曲，不要中断。</strong>", "qtype": 3, "is_objective": false, "parent_version": "0", "question_ref_sorce": 30.0, "ref_info": { "answers": ["phone", "city", "writing", "restaurant", "cold", "略", "略", "略", "略", "略", "略", "略", "略", "略", "略"], "solving_idea": "", "attachments": [] }, "options": [{ "id": "950460955557056668", "stem": "phone", "attachments": [] }, { "id": "950460955557056669", "stem": "city", "attachments": [] }, { "id": "950460955557056670", "stem": "writing", "attachments": [] }, { "id": "950460955557056671", "stem": "restaurant", "attachments": [] }, { "id": "950460955557056672", "stem": "cold", "attachments": [] }, { "id": "950460955557056673", "stem": "略", "attachments": [] }, { "id": "950460955557056674", "stem": "略", "attachments": [] }, { "id": "950460955557056675", "stem": "略", "attachments": [] }, { "id": "950460955557056676", "stem": "略", "attachments": [] }, { "id": "950460955557056677", "stem": "略", "attachments": [] }, { "id": "950460955557056678", "stem": "略", "attachments": [] }, { "id": "950460955557056679", "stem": "略", "attachments": [] }, { "id": "950460955557056680", "stem": "略", "attachments": [] }, { "id": "950460955557056681", "stem": "略", "attachments": [] }, { "id": "950460955557056682", "stem": "略", "attachments": [] }], "attachments": [{ "file_type": 2, "position": 4, "file_url": "http://rimg2.ciwong.net/cwf/6v68/tools/editor/15623/000/10000/26c2d7a961d5bc8bc1afe9b638496948.mp3" }], "children": [], "id": "855789107374586649", "version_id": "950460940484938224" }, { "sid": 1, "stem": "<strong>【步骤练习】<br />\n看电影视频，回答问题。</strong><br />\n<strong>步骤一：播放没有字幕的视频（完整看两遍，不要停顿），直到看懂大概意思。</strong><br />\n{#video#}http://player.56.com/3000002473/open_MTM3NzIxNjg5.swf{#/video#}?<br />\n<br />\n<strong>步骤二：看电影视频，回答问题。</strong>\n<div>An elephant what?<br />\n{#blank#}1{#/blank#}&nbsp;</div>\n<br />\n<strong>步骤三：播放有字幕的视频，修改自己的答案。</strong><br />\n?{#video#}http://player.56.com/3000002473/open_MTM3NzIxODE4.swf{#/video#}", "qtype": 3, "is_objective": false, "parent_version": "0", "question_ref_sorce": 4.0, "ref_info": { "answers": ["An elephant graveyard."], "solving_idea": "", "attachments": [] }, "options": [{ "id": "950460955557057120", "stem": "An elephant graveyard.", "attachments": [] }], "attachments": [], "children": [], "id": "855789107374586663", "version_id": "950460940484938355" }] }], "id": "634828917904305097", "version_id": null }, "code": 0, "ret": 0, "errcode": 0, "msg": "success", "is_succeed": true };
            //methods.DataBindPerformanceOptimise(model, xx.data);
            settings.id && ciwong.ajax.getJSON("/resource/get", { versionId: settings.id, moduleId: settings.moduleId || "1f693f76-02f5-4a40-861d-a8503df5183f" }, function (paperData) {
                methods.DataBindPerformanceOptimise(model, paperData);
                //model.paper(paperData);
            }, null, false).done(function () {
                ciwong.colseLoading();
            });
        },
        resultView: function (settingOptions) {
            var model = this,
                settings = settingOptions || {};
            settings.isPc = methods.isPc();
            model.viewType = ko.observable(2);//1:预览模式 2:结果模式

            model.paper = ko.observable();
            model.paper.subscribe(function (newPaper) {
                newPaper.parts && $.each(newPaper.parts, function (i) {
                    this.module_type_name = "第{0}题 {1}".format(methods.convertToChineseNum(i + 1), this.module_type_name);
                    settings.dataSource = this.children;
                    this.children = ko.observable(new question.viewModel.resultView(settings));//{ dataSource: this.children, isShowAnswer: settings.isShowAnswer }
                });
            });

            model.afterRender = function () {
                $("table").css({ width: 'auto' });
                !settings.isPc && audiojs.createAll();//如果是移动端，ko渲染完成之后通过audiojs实例化所有音频对象
                settings.afterRender && settings.afterRender.call(model);
            };

            settings.id && ciwong.ajax.getJSON("/resource/get", { moduleId: settings.moduleId || "1f693f76-02f5-4a40-861d-a8503df5183f", versionId: settings.id }, function (paperData) {
                var userAnswer = methods.getUserAnswer(settings.doId, settings.answerType || 1);
                $.each(paperData.parts, function (i, item) {
                    methods.setUserAnswer(userAnswer, item, settings.moduleId);
                });
                model.paper(paperData);
            }, true, false);
        },
        correctView: function (settingOptions) {
            var model = this,
                settings = settingOptions || {};
            settings.isPc = methods.isPc();
            model.isCorrectView = true;
            model.paper = ko.observable();
            model.paper.subscribe(function (newPaper) {
                newPaper.parts && $.each(newPaper.parts, function (i) {
                    this.module_type_name = "第{0}题 {1}".format(methods.convertToChineseNum(i + 1), this.module_type_name);
                    settings.dataSource = this.children;
                    this.children = ko.observable(new question.viewModel.correctView(settings));//{ dataSource: this.children, isShowAnswer: settings.isShowAnswer }
                });
            });

            model.afterRender = function () {
                $("table").css({ width: 'auto' });
                !settings.isPc && audiojs.createAll();//如果是移动端，ko渲染完成之后通过audiojs实例化所有音频对象
                model.paper().parts && $(model.paper().parts).each(function (index, item) {
                    model.RecursiveToInit(item.children().list());
                });

                settings.afterRender && settings.afterRender.call(model);
            };
            model.RecursiveToInit = function (list) {
                list && $(list).each(function (index, item) {
                    if (item.children && item.children.length > 0) {
                        model.RecursiveToInit(item.children);
                    } else {
                        if (item.qtype == 3) {//填空题作业批改特殊处理开始
                            item.tiankongCorrectParamsList() && $(item.tiankongCorrectParamsList()).each(function (index2, item2) {
                                methods.initCorrectHtmlEventHandler(item.version_id + parseInt(item2.sid + 1), item, item2);
                            });
                        }
                        else {
                            methods.initCorrectHtmlEventHandler(item.version_id, item, item.correctParams);
                        }
                    }
                });
            };
            settings.id && ciwong.ajax.getJSON("/resource/get", { moduleId: settings.moduleId || "1f693f76-02f5-4a40-861d-a8503df5183f", versionId: settings.id }, function (paperData) {
                var userAnswer = methods.getUserAnswer(settings.doId, settings.answerType || 1);
                $.each(paperData.parts, function (i, item) {
                    methods.setUserAnswer(userAnswer, item, settings.moduleId);
                });
                model.paper(paperData);
            }, true, false);
        }
    };

    exports.viewModel = viewModel;
    exports.moduleId = "1f693f76-02f5-4a40-861d-a8503df5183f";

    /*
	以下为当前viewModel所用的模板以及部分函数
	*/
    ciwong.koTemplateEngine.add("paperTemplate", '\
        <!--ko foreach: parts -->\
        <section class="work-quesBox">\
            <nav class="work-quesT">\
                <h3 class="t"><i></i><span data-bind="text:module_type_name.removeHtmlTag()"></span></h3>\
            </nav>\
            <!--ko template: { name: "questionListTemplate", data: children } -->\
            <!--/ko-->\
        </section>\
        <!-- /ko -->');
});