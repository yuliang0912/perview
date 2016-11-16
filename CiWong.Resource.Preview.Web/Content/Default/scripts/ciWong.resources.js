/**
*  习网通用资源 json数据
*  此文件影响重大，请慎重修改，或请联系 作业中心“万总”
**/


var ciwong;
if (!ciwong) {
    ciwong = {};
}

if (!ciwong.resources) {
    /*
    * 基础资源 数据集
    */
    ciwong.resources = {
        //学段
        period: [
            { id: 1001, name: '小学' },
            { id: 1003, name: '初中' },
            { id: 1005, name: '高中' }
            //{ id: 1007, name: '大学'},
            //{ id: 1009, name: '幼儿'}
        ],
        //年级
        grade: [
            //小学
            { id: 1101, name: '一年级' },
            { id: 1102, name: '二年级' },
            { id: 1103, name: '三年级' },
            { id: 1104, name: '四年级' },
            { id: 1105, name: '五年级' },
            { id: 1106, name: '六年级' },
            //初中
            { id: 1107, name: '七年级' },
            { id: 1108, name: '八年级' },
            { id: 1109, name: '九年级' },

            //高中
            { id: 1111, name: '高一（旧版）' },
            { id: 1112, name: '高二（旧版）' },
            { id: 1113, name: '高三（旧版）' },

            { id: 1201, name: '必修1' },
            { id: 1202, name: '必修2' },
            { id: 1203, name: '必修3' },
            { id: 1204, name: '必修4' },
            { id: 1205, name: '必修5' },

            { id: 1301, name: '选修1' },
            { id: 1302, name: '选修2' },
            { id: 1303, name: '选修3' },
            { id: 1304, name: '选修4' },
            { id: 1305, name: '选修5' },
            { id: 1306, name: '选修6' },
            { id: 1307, name: '选修7' },
            { id: 1308, name: '选修8' },
            { id: 1309, name: '选修9' },
            { id: 1310, name: '选修10' },
            { id: 1311, name: '选修11' },
            { id: 1312, name: '选修12' },
            { id: 1313, name: '选修13' },
            { id: 1314, name: '选修14' },
            { id: 1315, name: '选修15' },
            { id: 1316, name: '选修16' },

            { id: 1401, name: '选修1-1' },
            { id: 1402, name: '选修1-2' },
            { id: 1403, name: '选修2-1' },
            { id: 1404, name: '选修2-2' },
            { id: 1405, name: '选修2-3' },
            { id: 1406, name: '选修3-1' },
            { id: 1407, name: '选修3-2' },
            { id: 1408, name: '选修3-3' },
            { id: 1409, name: '选修3-4' },
            { id: 1410, name: '选修3-5' },
            { id: 1411, name: '选修3-6' },
            { id: 1412, name: '选修4-1' },
            { id: 1413, name: '选修4-2' },
            { id: 1414, name: '选修4-3' },
            { id: 1415, name: '选修4-4' },
            { id: 1416, name: '选修4-5' },
            { id: 1417, name: '选修4-6' },
            { id: 1418, name: '选修4-7' },
            { id: 1419, name: '选修4-8' },
            { id: 1420, name: '选修4-9' },
            { id: 1421, name: '选修4-10' }

            //大普
            //{ id: 1118, name: '一年级' },
            //{ id: 1119, name: '二年级' },
            //{ id: 1120, name: '三年级' },
            //{ id: 1121, name: '四年级' },
            ////
            //{ id: 1114, name: '小班' },
            //{ id: 1115, name: '中班' },
            //{ id: 1116, name: '大班' },
            //{ id: 1117, name: '大大班' }
        ],
        //科目
        subject: [
           	{ id: 1, name: '语文' },
            { id: 2, name: '数学' },
            { id: 3, name: '英语' },
            //{ id: 4, name: '英语II' },
            //{ id: 5, name: '思想品德' },
            //{ id: 6, name: '思想政治' },
            { id: 7, name: '历史' },
            { id: 8, name: '地理' },
            { id: 9, name: '物理' },
            { id: 10, name: '化学' },
            { id: 11, name: '生物' },
            //{ id: 12, name: '历史与社会' },
            //{ id: 13, name: '计算机' },
            //{ id: 14, name: '文科数学' },
            //{ id: 15, name: '理科数学' },
            //{ id: 16, name: '文综合' },
            //{ id: 17, name: '理综合' },
            //{ id: 18, name: '大综合' },
            //{ id: 19, name: '品德与社会' },
            //{ id: 20, name: '体音美劳' },
            { id: 21, name: '信息科学与技术' },
            { id: 22, name: '政治' },
            //{ id: 23, name: '代数' },
            //{ id: 24, name: '几何' },
            //{ id: 25, name: '普及类竞赛' },
            { id: 26, name: '科学' }
            //{ id: 27, name: '德育' },
            //{ id: 28, name: '百科苑' }
        ],
        //学期
        semester: [
            { id: 1201, name: '上学期' },
            { id: 1202, name: '下学期' },
            { id: 1203, name: '全册' }
        ],
        category: [
            //小学
            { periodId: 1001, subjectId: 1, grades: [1101, 1102, 1103, 1104, 1105, 1106] }, //语文
            { periodId: 1001, subjectId: 2, grades: [1101, 1102, 1103, 1104, 1105, 1106] }, //数学
            { periodId: 1001, subjectId: 3, grades: [1101, 1102, 1103, 1104, 1105, 1106] }, //英语
            { periodId: 1001, subjectId: 26, grades: [1104, 1105, 1106] }, //科学
            { periodId: 1001, subjectId: 21, grades: [1104, 1105, 1106] }, //信息技术
            //初中
            { periodId: 1003, subjectId: 1, grades: [1107, 1108, 1109] },//语文
            { periodId: 1003, subjectId: 2, grades: [1107, 1108, 1109] },//数学
            { periodId: 1003, subjectId: 3, grades: [1107, 1108, 1109] },//英语
            { periodId: 1003, subjectId: 9, grades: [1108, 1109] }, //物理
            { periodId: 1003, subjectId: 10, grades: [1109] }, //化学
            { periodId: 1003, subjectId: 11, grades: [1107, 1108, 1109] },//生物
            { periodId: 1003, subjectId: 22, grades: [1107, 1108, 1109] },//政治
            { periodId: 1003, subjectId: 7, grades: [1107, 1108, 1109] },//历史
            { periodId: 1003, subjectId: 8, grades: [1107, 1108] },//地理
            { periodId: 1003, subjectId: 26, grades: [1107, 1108, 1109] },//科学
            { periodId: 1003, subjectId: 21, grades: [1107, 1108, 1109] },//信息技术
            //高中
            { periodId: 1005, subjectId: 1, grades: [1111,1112,1113,1201, 1202, 1203, 1204, 1205, 1301, 1302, 1303, 1304, 1305, 1306, 1307, 1308, 1309, 1310, 1311, 1312, 1313, 1314, 1315, 1316] },//语文
            { periodId: 1005, subjectId: 2, grades: [1111,1112,1113,1201, 1202, 1203, 1204, 1205, 1401, 1402, 1403, 1404, 1405, 1406, 1407, 1408, 1409, 1410, 1411, 1412, 1413, 1414, 1415, 1416, 1417, 1418, 1419, 1420, 1421] },//数学
            { periodId: 1005, subjectId: 3, grades: [1111,1112,1113,1201, 1202, 1203, 1204, 1205, 1306, 1307, 1308, 1309, 1310, 1311, 1312] },//英语
            { periodId: 1005, subjectId: 9, grades: [1111,1112,1113,1201, 1202, 1401, 1402, 1403, 1404, 1405, 1406, 1407, 1408, 1409, 1410] },//物理
            { periodId: 1005, subjectId: 10, grades: [1111,1112,1113,1201, 1202, 1301, 1302, 1303, 1304, 1305, 1306] },//化学
            { periodId: 1005, subjectId: 11, grades: [1111,1112,1113,1201, 1202, 1203, 1301, 1302, 1303] },//生物
            { periodId: 1005, subjectId: 22, grades: [1111,1112,1113,1201, 1202, 1203, 1204, 1301, 1302, 1303, 1304, 1305, 1306] },//政治
            { periodId: 1005, subjectId: 7, grades: [1111,1112,1113,1201, 1202, 1203, 1301, 1302, 1303, 1304, 1305, 1306] },//历史
            { periodId: 1005, subjectId: 8, grades: [1111,1112,1113,1201, 1202, 1203, 1301, 1302, 1303, 1304, 1305, 1306, 1307] },//地理
        ],
        //地区
        area: [
            '京', '沪', '津', '渝', '鲁', '苏', '湘', '闽', '川', '赣', '皖', '浙', '陕', '宁', '辽', '鄂', '粤', '琼', '冀', '豫', '晋', '桂', '滇', '黔', '黑', '吉', '蒙', '青', '藏'
        ],
        //适用范围
        scope: [
            { id: '1', name: '课文同步' },
            { id: '2', name: '单元测试' },
            { id: '3', name: '期中考试' },
            { id: '4', name: '期末考试' },
            { id: '5', name: '小升初' },
            { id: '6', name: '中考' },
            { id: '7', name: '高考' },
            { id: '15', name: '暑假作业' },
            { id: '16', name: '寒假作业' }
        ],
        //难易度
        level: [
            { id: '1', name: '容易' },
            { id: '2', name: '中等' },
            { id: '3', name: '困难' }
        ]
    };

    /*
    * 数据集辅助查询方法
    */
    ciwong.resources.finder = {
        findBy: function (array, value, filter, index) {
            if (!filter) {
                filter = function(data) {
                    return data.id;
                };
            }

            var resultData = $(array).map(function () {
                if (parseInt(filter(this)) == parseInt(value)) {
                    return this;
                }
            });

            if (index != null && index != undefined) {
                if (resultData.length > index) {
                    return resultData.get(index);
                }
                else {
                    return null;
                }
            }
            else {
                return resultData.get();
            }
        },
        findPeriodById: function (periodId) {
            return ciwong.resources.finder.findBy(ciwong.resources.period, periodId, undefined, 0);
        },
        findGradeById: function (gradeId) {
            return ciwong.resources.finder.findBy(ciwong.resources.grade, gradeId, undefined, 0);
        },
        findSubjectById: function (subjectId) {
            return ciwong.resources.finder.findBy(ciwong.resources.subject, subjectId, undefined, 0);
        },
        findSemesterById: function (semesterId) {
            return ciwong.resources.finder.findBy(ciwong.resources.semester, semesterId, undefined, 0);
        },
        findSemesterByPeriod: function (periodId) {
            if (periodId == 1005)
                return [];
            return ciwong.resources.semester;
        },
        findGradeByPeriodAndSubject: function (periodId, subjectId) {
            var gradeList = [];
            if (!periodId || !subjectId) {
                return gradeList;
            }

            $.each(ciwong.resources.category, function () {
                if (this.periodId == periodId && this.subjectId == subjectId) {
                    gradeList = this.grades;
                    return false;
                }
            });

            return $(ciwong.resources.grade).map(function () {
                if ($.inArray(this.id, gradeList) >= 0) {
                    return this;
                }
            }).get();
        },
        findSubjectByPeriod: function (periodId) {

            var period = ciwong.resources.finder.findPeriodById(periodId);
            if (!period) {
                return [];
            }

            var subjectList = ciwong.resources.finder.findBy(ciwong.resources.category, period.id, function (data) { return data.periodId; });

            return $(subjectList).map(function () {
                return ciwong.resources.finder.findSubjectById(this.subjectId);
            }).get();
        }
    };

    /*
    * 数据集UI绑定辅助方法
    */
    ciwong.resources.binder = {
        //绑定生成下拉框
        bindDropDownList: function (settingOptions) {
            var settings = {
                defaultItem: {
                    enabled: true,              //是否启用默认项
                    value: 0,                   //默认项的value
                    text: '请选择'               //默认项的text
                },
                options: {
                    data: [],                   //选项数据集
                    value: 'id',                //option.value所绑定的对象
                    text: 'name'                  //option.text所绑定的对象
                },
                target: {
                    expr: '',                   //要绑定的dropdownlist对象的JQuery表达式
                    change: function () { }     //change事件
                }
            };

            $.extend(true, settings, settingOptions || {});

            var targetElement = $(settings.target.expr);
            if (targetElement.length == 0) {
                return targetElement;
            }

            //绑定change事件
            targetElement.change(settings.target.change);

            //处理Options Data
            var data = [];
            if(settings.options.data)
            {
                data = $.isArray(settings.options.data)
                    ? settings.options.data
                    : [data];
            }

            //处理default Item
            if (settings.defaultItem.enabled) {
                var item={};
                item[settings.options.value]=settings.defaultItem.value;
                item[settings.options.text]=settings.defaultItem.text;

                data.unshift(item);
            }

            //绑定Options
            targetElement.empty();
            for (var i = 0, j = data.length; i < j; i++) {
                var value = data[i][settings.options.value],
                    text = data[i][settings.options.text];

                if (value != undefined && text) {
                    targetElement.append('<option  value="' + value + '">' + text + '</option>');
                }
            }

            return targetElement;
        },
        //绑定学段
        bindPeriod: function (selectId, isOne) {
            ciwong.resources.binder.bindDropDownList({
                defaultItem: {
                    enabled: !isOne,
                    text: '选择学段'
                },
                options: {
                    data: ciwong.resources.period
                },
                target: {
                    expr: '#' + selectId
                }
            });
        },

        //绑定科目
        bindSubject: function (selectId, periodId, isOne) {
            ciwong.resources.binder.bindDropDownList({
                defaultItem: {
                    enabled: !isOne,
                    text: '选择科目'
                },
                options: {
                    data: ciwong.resources.finder.findSubjectByPeriod(periodId)
                },
                target: {
                    expr: '#' + selectId
                }
            });
        },

        //绑定年级
        bindGrade: function (selectId, periodId, subjectId, isOne) {
            ciwong.resources.binder.bindDropDownList({
                defaultItem: {
                    enabled: !isOne,
                    text: (periodId && periodId == '1005') ? '选择版本' : '选择年级'
                },
                options: {
                    data: ciwong.resources.finder.findGradeByPeriodAndSubject(periodId, subjectId)
                },
                target: {
                    expr: '#' + selectId
                }
            });
        },

        //绑定学期
        bindSemester: function (selectId, periodId, isOne) {
            var dropDownList = ciwong.resources.binder.bindDropDownList({
                defaultItem: {
                    enabled: !isOne,
                    text: '选择学期'
                },
                options: {
                    data: ciwong.resources.semester
                },
                target: {
                    expr: '#' + selectId
                }
            });

            if (periodId && parseInt(periodId) == 1005) {
                dropDownList.hide();
            }
            else {
                dropDownList.show();
            }
        }
    };

    /*
    * knockout视图模型
    */
    ciwong.resources.viewModels = {
        /*
        * 联动模型 （学段、科目、年级、科目）
        *
        */
        cascadePeriod: function (settingOptions) {

            function buildSettings(value) {
                !value && (value = {});

                return {
                    period: ciwong.resources.finder.findPeriodById(value.periodId),
                    subject: ciwong.resources.finder.findSubjectById(value.subjectId),
                    grade: ciwong.resources.finder.findGradeById(value.gradeId),
                    semester: ciwong.resources.finder.findSemesterById(value.semesterId),
                    autoDefaultValue: value.autoDefaultValue != undefined ? value.autoDefaultValue : false
                };
            }

            var model = this,
                settings = buildSettings(settingOptions);

            this.getValueId = function (value) {
                return value != undefined
                    ? value.id || null
                    : null;
            };
            
            //学段
            this.periods = {
                visible: ko.observable(true),                      //是否显示
                value: ko.observable(settings.period),             //当前选择对象 
                list: ko.observableArray(ciwong.resources.period)  //数据集
            };
            //科目
            this.subjects = {
                visible: ko.computed(function() {
                    return !!model.periods.value();
                }),
                value: ko.observable(settings.subject),
                list: ko.computed(function () {
                    return ciwong.resources.finder.findSubjectByPeriod(model.getValueId(model.periods.value()));
                })
            };
            //年级
            this.grades = {
                visible: ko.computed(function() {
                    return !!model.subjects.value();
                }),
                value: ko.observable(settings.grade),
                list: ko.computed(function () {
                    var periodId = model.getValueId(model.periods.value()),
                        subjectId = model.getValueId(model.subjects.value()),
                        grades = ciwong.resources.finder.findGradeByPeriodAndSubject(periodId, subjectId);

                    return periodId == 1005
                        ? grades.slice(3)
                        : grades;
                }).extend({ rateLimit: 5 })
            };
            //学段
            this.semesters = {
                visible: ko.computed(function () {
                    return model.grades.visible() && model.getValueId(model.periods.value()) != 1005;
                }),
                value: ko.observable(settings.semester),
                list: ko.computed(function () {
                    return ciwong.resources.finder.findSemesterByPeriod(model.getValueId(model.periods.value()));
                })
            };

            //学段，年级，学期，科目 选择事件
            //注：用DropDownlist等有自动处理选择事件的对象展示时不需要使用此对象
            this.selectedHandler = function (targetName) {
                model[targetName].value(this);
            };

            //学段，年级，学期，科目，判断选择的是否为当前对象
            //注：用DropDownlist等有自动处理选择事件的对象展示时不需要使用此对象
            this.isCurrent = function (data,targetName) {
                return model[targetName].value() == data;
            };


            this.value = function (value) {

                if (value = buildSettings(value)) {
                    value.period && model.periods.value(value.period);
                    value.subject && model.subjects.value(value.subject);

                    //grades设置了rateLimit: 5所以这里要用setTimeout让grades赋值完成之后在执行
                    setTimeout(function() {
                        value.grade && model.grades.value(value.grade);
                        value.semester && model.semesters.value(value.semester);
                    }, 50);
                }

                return {
                    periodId: model.getValueId(model.periods.value()),
                    gradeId: model.getValueId(model.grades.value()),
                    subjectId: model.getValueId(model.subjects.value()),
                    semesterId: model.semesters.visible() ? model.getValueId(model.semesters.value()) : null
                };
            };

            function subscribeDefaultValue(targetName,defaultValue) {
                var target = model[targetName];

                function handler(newValue) {
                    var item = $.inArray(defaultValue, newValue) >= 0
                        ? defaultValue
                        : newValue[0];

                    target.value(item);
                }

                target.list.subscribe(handler);
                handler(target.list());
            }

            if (settings.autoDefaultValue) {
                subscribeDefaultValue('periods', settings.period);
                subscribeDefaultValue('subjects', settings.subject);
                subscribeDefaultValue('grades', settings.grade);
                subscribeDefaultValue('semesters', settings.semester);
            } 
        }
    };
}
