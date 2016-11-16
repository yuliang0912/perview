/**
*  框架基础类，实现一些最基础的方法
*
* @static class
* @priority 100
**/


if (!window.ciWong) {
    window.ciWong = (function () {
        return {};
    })();
}

/*----------------------------- html5.js --------------------------*/
/*
 HTML5 Shiv v3.7.0 copy from http://html5shiv.googlecode.com/svn/trunk/html5.js
*/
(function (l, f) { function m() { var a = e.elements; return "string" == typeof a ? a.split(" ") : a } function i(a) { var b = n[a[o]]; b || (b = {}, h++, a[o] = h, n[h] = b); return b } function p(a, b, c) { b || (b = f); if (g) return b.createElement(a); c || (c = i(b)); b = c.cache[a] ? c.cache[a].cloneNode() : r.test(a) ? (c.cache[a] = c.createElem(a)).cloneNode() : c.createElem(a); return b.canHaveChildren && !s.test(a) ? c.frag.appendChild(b) : b } function t(a, b) { if (!b.cache) b.cache = {}, b.createElem = a.createElement, b.createFrag = a.createDocumentFragment, b.frag = b.createFrag(); a.createElement = function (c) { return !e.shivMethods ? b.createElem(c) : p(c, a, b) }; a.createDocumentFragment = Function("h,f", "return function(){var n=f.cloneNode(),c=n.createElement;h.shivMethods&&(" + m().join().replace(/[\w\-]+/g, function (a) { b.createElem(a); b.frag.createElement(a); return 'c("' + a + '")' }) + ");return n}")(e, b.frag) } function q(a) { a || (a = f); var b = i(a); if (e.shivCSS && !j && !b.hasCSS) { var c, d = a; c = d.createElement("p"); d = d.getElementsByTagName("head")[0] || d.documentElement; c.innerHTML = "x<style>article,aside,dialog,figcaption,figure,footer,header,hgroup,main,nav,section{display:block}mark{background:#FF0;color:#000}template{display:none}</style>"; c = d.insertBefore(c.lastChild, d.firstChild); b.hasCSS = !!c } g || t(a, b); return a } var k = l.html5 || {}, s = /^<|^(?:button|map|select|textarea|object|iframe|option|optgroup)$/i, r = /^(?:a|b|code|div|fieldset|h1|h2|h3|h4|h5|h6|i|label|li|ol|p|q|span|strong|style|table|tbody|td|th|tr|ul)$/i, j, o = "_html5shiv", h = 0, n = {}, g; (function () { try { var a = f.createElement("a"); a.innerHTML = "<xyz></xyz>"; j = "hidden" in a; var b; if (!(b = 1 == a.childNodes.length)) { f.createElement("a"); var c = f.createDocumentFragment(); b = "undefined" == typeof c.cloneNode || "undefined" == typeof c.createDocumentFragment || "undefined" == typeof c.createElement } g = b } catch (d) { g = j = !0 } })(); var e = { elements: k.elements || "abbr article aside audio bdi canvas data datalist details dialog figcaption figure footer header hgroup main mark meter nav output progress section summary template time video", version: "3.7.0", shivCSS: !1 !== k.shivCSS, supportsUnknownElements: g, shivMethods: !1 !== k.shivMethods, type: "default", shivDocument: q, createElement: p, createDocumentFragment: function (a, b) { a || (a = f); if (g) return a.createDocumentFragment(); for (var b = b || i(a), c = b.frag.cloneNode(), d = 0, e = m(), h = e.length; d < h; d++) c.createElement(e[d]); return c } }; l.html5 = e; q(f) })(this, document);


/*----------------------------- json.js --------------------------*/
/**
 *  json转换辅助工作  
    json2.js
    2013-05-26

        @param {Object} value javascript对象
        @param [Array/Function] 序列化方式
        @param [] space
        JSON.stringify(value, replacer, space)
                   

            replacer    an optional parameter that determines how object
                        values are stringified for objects. It can be a
                        function or an array of strings.

            space       an optional parameter that specifies the indentation
                        of nested structures. If it is omitted, the text will
                        be packed without extra whitespace. If it is a number,
                        it will specify the number of spaces to indent at each
                        level. If it is a string (such as '\t' or '&nbsp;'),
                        it contains the characters used to indent at each level.

            This method produces a JSON text from a JavaScript value.

            When an object value is found, if the object contains a toJSON
            method, its toJSON method will be called and the result will be
            stringified. A toJSON method does not serialize: it returns the
            value represented by the name/value pair that should be serialized,
            or undefined if nothing should be serialized. The toJSON method
            will be passed the key associated with the value, and this will be
            bound to the value

            For example, this would serialize Dates as ISO strings.

                Date.prototype.toJSON = function (key) {
                    function f(n) {
                        // Format integers to have at least two digits.
                        return n < 10 ? '0' + n : n;
                    }

                    return this.getUTCFullYear()   + '-' +
                         f(this.getUTCMonth() + 1) + '-' +
                         f(this.getUTCDate())      + 'T' +
                         f(this.getUTCHours())     + ':' +
                         f(this.getUTCMinutes())   + ':' +
                         f(this.getUTCSeconds())   + 'Z';
                };

            You can provide an optional replacer method. It will be passed the
            key and value of each member, with this bound to the containing
            object. The value that is returned from your method will be
            serialized. If your method returns undefined, then the member will
            be excluded from the serialization.

            If the replacer parameter is an array of strings, then it will be
            used to select the members to be serialized. It filters the results
            such that only members with keys listed in the replacer array are
            stringified.

            Values that do not have JSON representations, such as undefined or
            functions, will not be serialized. Such values in objects will be
            dropped; in arrays they will be replaced with null. You can use
            a replacer function to replace those with JSON values.
            JSON.stringify(undefined) returns undefined.

            The optional space parameter produces a stringification of the
            value that is filled with line breaks and indentation to make it
            easier to read.

            If the space parameter is a non-empty string, then that string will
            be used for indentation. If the space parameter is a number, then
            the indentation will be that many spaces.

            Example:

            text = JSON.stringify(['e', {pluribus: 'unum'}]);
            // text is '["e",{"pluribus":"unum"}]'


            text = JSON.stringify(['e', {pluribus: 'unum'}], null, '\t');
            // text is '[\n\t"e",\n\t{\n\t\t"pluribus": "unum"\n\t}\n]'

            text = JSON.stringify([new Date()], function (key, value) {
                return this[key] instanceof Date ?
                    'Date(' + this[key] + ')' : value;
            });
            // text is '["Date(---current time---)"]'


        JSON.parse(text, reviver)
            This method parses a JSON text to produce an object or array.
            It can throw a SyntaxError exception.

            The optional reviver parameter is a function that can filter and
            transform the results. It receives each of the keys and values,
            and its return value is used instead of the original value.
            If it returns what it received, then the structure is not modified.
            If it returns undefined then the member is deleted.

            Example:

            // Parse the text. Values that look like ISO date strings will
            // be converted to Date objects.

            myData = JSON.parse(text, function (key, value) {
                var a;
                if (typeof value === 'string') {
                    a =
/^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)Z$/.exec(value);
                    if (a) {
                        return new Date(Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4],
                            +a[5], +a[6]));
                    }
                }
                return value;
            });

            myData = JSON.parse('["Date(09/09/2001)"]', function (key, value) {
                var d;
                if (typeof value === 'string' &&
                        value.slice(0, 5) === 'Date(' &&
                        value.slice(-1) === ')') {
                    d = new Date(value.slice(5, -1));
                    if (d) {
                        return d;
                    }
                }
                return value;
            });


    This is a reference implementation. You are free to copy, modify, or
    redistribute.
*/

/*jslint evil: true, regexp: true */

/*members "", "\b", "\t", "\n", "\f", "\r", "\"", JSON, "\\", apply,
    call, charCodeAt, getUTCDate, getUTCFullYear, getUTCHours,
    getUTCMinutes, getUTCMonth, getUTCSeconds, hasOwnProperty, join,
    lastIndex, length, parse, prototype, push, replace, slice, stringify,
    test, toJSON, toString, valueOf
*/


// Create a JSON object only if one does not already exist. We create the
// methods in a closure to avoid creating global variables.

if (typeof JSON !== 'object') {
    JSON = {};
}

(function () {
    'use strict';

    function f(n) {
        // Format integers to have at least two digits.
        return n < 10 ? '0' + n : n;
    }

    if (typeof Date.prototype.toJSON !== 'function') {

        Date.prototype.toJSON = function () {

            return isFinite(this.valueOf())
                ? this.getUTCFullYear() + '-' +
                    f(this.getUTCMonth() + 1) + '-' +
                    f(this.getUTCDate()) + 'T' +
                    f(this.getUTCHours()) + ':' +
                    f(this.getUTCMinutes()) + ':' +
                    f(this.getUTCSeconds()) + 'Z'
                : null;
        };

        String.prototype.toJSON =
            Number.prototype.toJSON =
            Boolean.prototype.toJSON = function () {
                return this.valueOf();
            };
    }

    var cx = /[\u0000\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        escapable = /[\\\"\x00-\x1f\x7f-\x9f\u00ad\u0600-\u0604\u070f\u17b4\u17b5\u200c-\u200f\u2028-\u202f\u2060-\u206f\ufeff\ufff0-\uffff]/g,
        gap,
        indent,
        meta = {    // table of character substitutions
            '\b': '\\b',
            '\t': '\\t',
            '\n': '\\n',
            '\f': '\\f',
            '\r': '\\r',
            '"': '\\"',
            '\\': '\\\\'
        },
        rep;


    function quote(string) {

        // If the string contains no control characters, no quote characters, and no
        // backslash characters, then we can safely slap some quotes around it.
        // Otherwise we must also replace the offending characters with safe escape
        // sequences.

        escapable.lastIndex = 0;
        return escapable.test(string) ? '"' + string.replace(escapable, function (a) {
            var c = meta[a];
            return typeof c === 'string'
                ? c
                : '\\u' + ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
        }) + '"' : '"' + string + '"';
    }


    function str(key, holder) {

        // Produce a string from holder[key].

        var i,          // The loop counter.
            k,          // The member key.
            v,          // The member value.
            length,
            mind = gap,
            partial,
            value = holder[key];

        // If the value has a toJSON method, call it to obtain a replacement value.

        if (value && typeof value === 'object' &&
                typeof value.toJSON === 'function') {
            value = value.toJSON(key);
        }

        // If we were called with a replacer function, then call the replacer to
        // obtain a replacement value.

        if (typeof rep === 'function') {
            value = rep.call(holder, key, value);
        }

        // What happens next depends on the value's type.

        switch (typeof value) {
            case 'string':
                return quote(value);

            case 'number':

                // JSON numbers must be finite. Encode non-finite numbers as null.

                return isFinite(value) ? String(value) : 'null';

            case 'boolean':
            case 'null':

                // If the value is a boolean or null, convert it to a string. Note:
                // typeof null does not produce 'null'. The case is included here in
                // the remote chance that this gets fixed someday.

                return String(value);

                // If the type is 'object', we might be dealing with an object or an array or
                // null.

            case 'object':

                // Due to a specification blunder in ECMAScript, typeof null is 'object',
                // so watch out for that case.

                if (!value) {
                    return 'null';
                }

                // Make an array to hold the partial results of stringifying this object value.

                gap += indent;
                partial = [];

                // Is the value an array?

                if (Object.prototype.toString.apply(value) === '[object Array]') {

                    // The value is an array. Stringify every element. Use null as a placeholder
                    // for non-JSON values.

                    length = value.length;
                    for (i = 0; i < length; i += 1) {
                        partial[i] = str(i, value) || 'null';
                    }

                    // Join all of the elements together, separated with commas, and wrap them in
                    // brackets.

                    v = partial.length === 0
                        ? '[]'
                        : gap
                        ? '[\n' + gap + partial.join(',\n' + gap) + '\n' + mind + ']'
                        : '[' + partial.join(',') + ']';
                    gap = mind;
                    return v;
                }

                // If the replacer is an array, use it to select the members to be stringified.

                if (rep && typeof rep === 'object') {
                    length = rep.length;
                    for (i = 0; i < length; i += 1) {
                        if (typeof rep[i] === 'string') {
                            k = rep[i];
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                } else {

                    // Otherwise, iterate through all of the keys in the object.

                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = str(k, value);
                            if (v) {
                                partial.push(quote(k) + (gap ? ': ' : ':') + v);
                            }
                        }
                    }
                }

                // Join all of the member texts together, separated with commas,
                // and wrap them in braces.

                v = partial.length === 0
                    ? '{}'
                    : gap
                    ? '{\n' + gap + partial.join(',\n' + gap) + '\n' + mind + '}'
                    : '{' + partial.join(',') + '}';
                gap = mind;
                return v;
        }
    }

    // If the JSON object does not yet have a stringify method, give it one.

    if (typeof JSON.stringify !== 'function') {
        JSON.stringify = function (value, replacer, space) {

            // The stringify method takes a value and an optional replacer, and an optional
            // space parameter, and returns a JSON text. The replacer can be a function
            // that can replace values, or an array of strings that will select the keys.
            // A default replacer method can be provided. Use of the space parameter can
            // produce text that is more easily readable.

            var i;
            gap = '';
            indent = '';

            // If the space parameter is a number, make an indent string containing that
            // many spaces.

            if (typeof space === 'number') {
                for (i = 0; i < space; i += 1) {
                    indent += ' ';
                }

                // If the space parameter is a string, it will be used as the indent string.

            } else if (typeof space === 'string') {
                indent = space;
            }

            // If there is a replacer, it must be a function or an array.
            // Otherwise, throw an error.

            rep = replacer;
            if (replacer && typeof replacer !== 'function' &&
                    (typeof replacer !== 'object' ||
                    typeof replacer.length !== 'number')) {
                throw new Error('JSON.stringify');
            }

            // Make a fake root object containing our value under the key of ''.
            // Return the result of stringifying the value.

            return str('', { '': value });
        };
    }


    // If the JSON object does not yet have a parse method, give it one.
    if (typeof JSON.parse !== 'function') {
        JSON.parse = function (text, reviver) {

            // The parse method takes a text and an optional reviver function, and returns
            // a JavaScript value if the text is a valid JSON text.

            var j;

            function walk(holder, key) {

                // The walk method is used to recursively walk the resulting structure so
                // that modifications can be made.

                var k, v, value = holder[key];
                if (value && typeof value === 'object') {
                    for (k in value) {
                        if (Object.prototype.hasOwnProperty.call(value, k)) {
                            v = walk(value, k);
                            if (v !== undefined) {
                                value[k] = v;
                            } else {
                                delete value[k];
                            }
                        }
                    }
                }
                return reviver.call(holder, key, value);
            }


            // Parsing happens in four stages. In the first stage, we replace certain
            // Unicode characters with escape sequences. JavaScript handles many characters
            // incorrectly, either silently deleting them, or treating them as line endings.

            text = String(text);
            cx.lastIndex = 0;
            if (cx.test(text)) {
                text = text.replace(cx, function (a) {
                    return '\\u' +
                        ('0000' + a.charCodeAt(0).toString(16)).slice(-4);
                });
            }

            // In the second stage, we run the text against regular expressions that look
            // for non-JSON patterns. We are especially concerned with '()' and 'new'
            // because they can cause invocation, and '=' because it can cause mutation.
            // But just to be safe, we want to reject all unexpected forms.

            // We split the second stage into 4 regexp operations in order to work around
            // crippling inefficiencies in IE's and Safari's regexp engines. First we
            // replace the JSON backslash pairs with '@' (a non-JSON character). Second, we
            // replace all simple value tokens with ']' characters. Third, we delete all
            // open brackets that follow a colon or comma or that begin the text. Finally,
            // we look to see that the remaining characters are only whitespace or ']' or
            // ',' or ':' or '{' or '}'. If that is so, then the text is safe for eval.

            if (/^[\],:{}\s]*$/
                    .test(text.replace(/\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g, '@')
                        .replace(/"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g, ']')
                        .replace(/(?:^|:|,)(?:\s*\[)+/g, ''))) {

                // In the third stage we use the eval function to compile the text into a
                // JavaScript structure. The '{' operator is subject to a syntactic ambiguity
                // in JavaScript: it can begin a block or an object literal. We wrap the text
                // in parens to eliminate the ambiguity.

                j = eval('(' + text + ')');

                // In the optional fourth stage, we recursively walk the new structure, passing
                // each name/value pair to a reviver function for possible transformation.

                return typeof reviver === 'function'
                    ? walk({ '': j }, '')
                    : j;
            }

            // If the text is not JSON parseable, then a SyntaxError is thrown.

            throw new SyntaxError('JSON.parse');
        };
    }

    if (window.ciWong) {
        window.ciWong.json = JSON;
    }
}());

/*----------------------------- type.js.js --------------------------*/
/**
* 对象类型信息
*
* @static class
**/


ciWong.type = (function () {

    var classType = {};
    var types = ['Boolean', 'Number', 'String', 'Function', 'Array', 'Date', 'RegExp', 'Object', 'Error'];
    for (var i = 0; i < types.length; i++) {
        classType['[object ' + types[i] + ']'] = types[i].toLowerCase();
    }

    var type = {
        /**
        * 获取目标对象类型
        *
        * @param {Object/Function} target 
        * @static method  {String}
        **/
        getType: function (target) {
            if (target == null) {
                return String(target);
            }

            // Support: Safari <= 5.1 (functionish RegExp)
            return typeof target === "object" || typeof target === "function"
                ? classType[classType['toString'].call(target)] || "object"
                : typeof target;
        },
        is: function (target, typeString) {
            return (typeString === "Null" && target === null) ||
                    (typeString === "Undefined" && target === undefined) ||
                    (type.getType(target) === typeString);
        },
        /**
        * 目标对象是否为function
        *
        * @param {Object/Function} target 
        * @static method  {Boolean}
        **/
        isFunction: function (target) {
            return type.is(target, 'function');
        },
        /**
        * 目标对象是否为Array
        *
        * @param {Object/Function} target 
        * @static method  {Boolean}
        **/
        isArray: function (target) {
            return type.is(target, 'array');
        },
        /**
        * 目标对象是否为Number对象
        *
        * @param {Object/Function} target 
        * @static method  {Boolean}
        **/
        isNumeric: function (target) {
            return !isNaN(parseFloat(target)) && isFinite(target);
        },
        /**
        * 目标对象是否为window对象
        *
        * @param {Object/Function} target 
        * @static method  {Boolean}
        **/
        isWindow: function (target) {
            return target != null && target == target.window;
        },
        /**
        * 是否为纯object对象，即使用new Object创建的对象，或是使用{}创建的对象。如：var a={},b=new Object();
        * window、DOM nodes、Boolean, Number, String, Function, Array, Date, RegExp, Error等都不是纯object对象
        *
        * @param {Object/Function} target 
        * @static method  {Boolean}
        **/
        isPlainObject: function (target) {
            // window、DOM nodes、Boolean, Number, String, Function, Array, Date, RegExp, Error都不是纯object对象
            if (type.getType(target) !== "object" || target.nodeType || type.isWindow(target)) {
                return false;
            }

            try {
                if (target.constructor &&
                        !classType['hasOwnProperty'].call(target.constructor.prototype, "isPrototypeOf")) {
                    return false;
                }
            } catch (e) {
                return false;
            }

            // |obj| is a plain object, created by {} or constructed with new Object
            return true;
        }
    };

    return type;
})();

/*----------------------------- utility.js.js --------------------------*/
/**
* 辅助工具类
*
* @expand ciWong
* @requires type
**/

(function () {
    var utility = {
        /**
        * 合并对象
        *
        * @param [Boolean] deep 是否递归合并对象的子级，默认为true。
        * @param {Object} target 合并目标对象，将src对象向target对象合并 
        * @param {Object} src 向目标对象合并的对象1
        * @param {Object} src2 向目标对象合并的对象2
        * @param {Object} src3 向目标对象合并的对象3
        * @static method {target} 返回目标对象
        **/
        extend: function () {

            var options, name, src, copy, copyIsArray, clone,
                target = arguments.length > 0 ? arguments[0] : {}, //注意这里不能用 arguments[0] || {}的方式，因为arguments[0]是一个boolean值，当为false时永远取后边的那个值
                i = 1,  //src在arguments中的开始索引
                length = arguments.length,
                deep = true;

            // 如第一个对象是Boolean类型，则是deep参数，如果不是Boolean类型则表示deep参数忽略采用默认值
            if (typeof target === 'boolean') {
                deep = target;
                target = arguments[1] || {};
                i = 2;
            }

            // 当目标对象是字符串或其它对象的时候，需要进行深拷贝
            if (typeof target !== 'object' && !typeof target !== 'function') {
                target = {};
            }

            // 如果只传递一个参数，那么则是扩展ciWong本身
            if (length === i) {
                target = this;
                --i;
            }

            for (; i < length; i++) {
                // 只处理非null或非undefined的对象
                if ((options = arguments[i]) != null) {

                    for (name in options) {
                        src = target[name];
                        copy = options[name];

                        //防止死循环
                        if (target === copy) {
                            continue;
                        }

                        if (deep && copy && (ciWong.type.isPlainObject(copy) || (copyIsArray = ciWong.type.isArray(copy)))) {
                            if (copyIsArray) {
                                copyIsArray = false;
                                clone = src && ciWong.type.isArray(src) ? src : [];

                            } else {
                                clone = src && ciWong.type.isPlainObject(src) ? src : {};
                            }

                            target[name] = utility.extend(deep, clone, copy);

                        } else if (copy !== undefined) {
                            target[name] = copy;
                        }
                    }
                }
            }

            return target;
        },
        /**
        * 对象深copy
        * clone对象不能扩展Object对象，因为Object对象太过于普遍，容易引起死循环造成内存溢出
        *
        * @method {Object}
        **/
        clone: function (obj) {
            var clone;

            // Array.
            if (obj && (obj instanceof Array)) {
                clone = [];

                for (var i = 0; i < obj.length; i++)
                    clone[i] = ciWong.clone(obj[i]);

                return clone;
            }

            // "Static" types.
            if (obj === null || (typeof (obj) != 'object') || (obj instanceof String) || (obj instanceof Number) || (obj instanceof Boolean) || (obj instanceof Date) || (obj instanceof RegExp)) {
                return obj;
            }

            // Objects.
            clone = new obj.constructor();

            for (var propertyName in obj) {
                var property = obj[propertyName];
                clone[propertyName] = ciWong.clone(property);
            }

            return clone;
        }
        //bind: function (func, obj) {
        //    return function () {
        //        return func.apply(obj, arguments);
        //    };
        //}
    };

    if (ciWong) {
        utility.extend(ciWong, utility);
    }
})();


/*----------------------------- convert.js --------------------------*/
/**
* 对象类型转换
*
* @static class
**/

ciWong.convert = (function () {

    var convert = {
        toInt: function (strNum) {
            var label = parseInt(strNum);

            return isNaN(label)
                ? 0
                : label;
        },
        toBoolean: function (strBool) {
            if (!strBool) {
                return false;
            }

            return strBool.toString().toLowerCase() === 'true' || parseInt(strBool) > 0;
        },
        toDate: function (dateTime) {
            if (dateTime instanceof Date) {
                return dateTime;
            }

            var date = new Date((parseInt(dateTime) || 0) * 1000);
            if (typeof (dateTime) === 'string') {
                //Date(13000085500)
                //var date= longDateTime.match(/Date\((\d+)\)/
                if (dateTime.indexOf('Date') >= 0) {
                    date = new Date(parseInt(dateTime.replace('/Date(', '').replace(')/', ''), 10) - 8 * 3600 * 1000);
                } else if (dateTime.indexOf('T') > 0) {
                    date = new Date(dateTime.replace('T', ' ').replace('Z', '').replace(/-/g, '/'));
                }
                else if (dateTime.match(/^\d+$/)) {
                    date = new Date(parseInt(dateTime, 10) * 1000);
                }
                else {
                    date = new Date(dateTime.replace(/-/g, '/'));
                }
            }

            return date;
        }
    };

    return convert;
})();

/*----------------------------- path.js.js --------------------------*/
/**
* 相对虚拟路径信息
*
*       var virtualPath=new ciWong.path('framework.js');
*       alert(virtualPath.basePath)
* 
* @static class ciwong.path
**/

ciWong.path = (function () {
    var path = {
        /**
        * 获取已加载js文件目录的路径
        *
        *   alert(ciWong.path.getFileDirectoryPath('framework.js'))
        * 
        * @param [String] fileName 要获取路径的文件名，如果为空则为根目录。
        * @method {String}
        **/
        getFileDirectoryPath: function (fileName) {
            var path = (!fileName || fileName == '/' || fileName == '') ? '/' : '';

            if (!path) {

                var scripts = document.getElementsByTagName('script');

                for (var i = 0; i < scripts.length; i++) {

                    var match = scripts[i].src.match(/(^|.*[\\\/])([\w-]+.js)(?:\?.*)?$/i);
                    if (match && match[2] == fileName) {
                        path = match[1];
                        break;
                    }
                }
            }

            if (!path) {
                path = '/';
            }

            // ie浏览器script.src返回的是Absolute path，而其它浏览器返回的则是Relative path，所以这里需要进行处理
            if (path.indexOf(':/') == -1) {
                // Absolute path.
                if (path.indexOf('/') === 0)
                    path = location.href.match(/^.*?:\/\/[^\/]*/)[0] + path;
                    // Relative path.
                else
                    path = location.href.match(/^[^\?]*\/(?:)/)[0] + path;
            }

            return path.toLowerCase();
        },
        /**
        * 将多个path地址合并为一个有效path地址
        *
        *   var virtualPath=ciWong.path.combine('/abc','../bcd','ab');
        *
        * @method {String}
        **/
        combine: function () {

            var basePath = 'http://' + document.location.host;
            if (!arguments || arguments.length == 0) {
                return basePath;
            }

            function combinePath(leftPath, rightPath) {
                var leftPaths = leftPath ? leftPath.split('/') : new Array(),
                    rightPaths = rightPath ? rightPath.split('/') : new Array();

                //处理"../"符号返上级目录
                while (rightPaths[0] == '..' && leftPaths.length > 0 && leftPaths[leftPaths.length - 1] != '..') {
                    if (leftPath.indexOf(':/') == -1 || leftPaths.length > 3) {
                        leftPaths.pop();
                    }
                    rightPaths.shift();
                }

                return leftPaths.concat(rightPaths).join('/');
            }

            var path = '';
            for (var i = arguments.length; i > 0; i--) {
                path = combinePath(arguments[i - 1], path);
                //如果item是绝对路径或完整url，则退出循环，因为绝对路径之前的那些路径已经没有意义
                if (path.indexOf('/') == 0 || path.indexOf(':/') >= 0) {
                    path = path.substring(1, path.length);
                    break;
                }
            }

            return combinePath(basePath, path);
        }
    };

    return path;
})();

/*----------------------------- scriptloader.js.js --------------------------*/
/**
 * 异步js文件加载器.
 * RequireJS是一个js加载组件，以后的升级可以参考一下这个组件
 *
 * @static class ciWong.scriptLoader/scriptLoader
 */

window.scriptLoader = (function () {
    var uniqueScripts = {},
		waitingList = {};

    return {
        /**
		 * 检测一个或多个js文件，如果没有加载过就加载js文件
		 * 示例：
		 *     ciWong.scriptLoader.load( '/myscript.js' ); 
         *
		 *     ciWong.scriptLoader.load( '/myscript.js', function( success ) {
		 *			// 弹出窗口提示js文件是否加载成功
		 *			// 如果 HTTP 404 错误，则success为false
		 *			alert( success );
		 *		} );
		 *
		 *		ciWong.scriptLoader.load( [ '/myscript1.js', '/myscript2.js' ], function( completed, failed ) {
		 *			alert( '成功加载js文件: ' + completed.length );
		 *			alert( '失败加载js文件: ' + failed.length );
		 *		} );
		 *
		 * @param {String/Array} scriptUrl 
                要加载js文件的url或url数组
		 * @param {Function} [callback] 
                加载结束后的回调函数．
         *      如果scriptUrl是一个url路径，那么callback函数只有一个参数，用来表示js文件是否加载成功
         *      如果scriptUrl是一个url数组，那么callback函数有两个参数，分别表示成功加载的url数组，和失败加载的url数组
		 * @param {Object} [thisArg] 
                在callback回调方法中this指针的对象 默认为scriptLoader
		 * @param {Boolean} [showBusy] 
                是否改变光标的状态，以标识文件正加载中．(如：光标状态改为漏斗状，以示加载中)
		 */
        load: function (scriptUrl, callback, thisArg, showBusy) {
            var isString = (typeof scriptUrl == 'string');

            if (isString)
                scriptUrl = [scriptUrl];

            if (!thisArg)
                thisArg = window.scriptLoader;

            var scriptCount = scriptUrl.length,
				completed = [],
				failed = [];

            var doCallback = function (success) {
                if (callback) {
                    if (isString)
                        callback.call(thisArg, success);
                    else
                        callback.call(thisArg, completed, failed);
                }
            };

            if (scriptCount === 0) {
                doCallback(true);
                return;
            }

            var checkLoaded = function (url, success) {
                (success ? completed : failed).push(url);

                if (--scriptCount <= 0) {
                    showBusy && (document.body.style.cursor = 'none');
                    doCallback(success);
                }
            };

            var onLoad = function (url, success) {

                // 标识这个js文件已经加载
                uniqueScripts[url] = 1;

                // Get the list of callback checks waiting for this file.
                var waitingInfo = waitingList[url];
                delete waitingList[url];

                // Check all callbacks waiting for this file.
                for (var i = 0; i < waitingInfo.length; i++)
                    waitingInfo[i](url, success);
            };

            var loadScript = function (url) {
                if (uniqueScripts[url]) {
                    checkLoaded(url, true);
                    return;
                }

                var waitingInfo = waitingList[url] || (waitingList[url] = []);
                waitingInfo.push(checkLoaded);

                // Load it only for the first request.
                if (waitingInfo.length > 1)
                    return;

                // Create the <script> element.
                var script = document.createElement("script");
                script.setAttribute('type', 'text/javascript');
                script.setAttribute('src', url);

                if (callback) {
                    if (eval('/*@cc_on!@*/false')) { //如果是ie浏览器
                        script.onreadystatechange = function () {
                            if (script.readyState == 'loaded' || script.readyState == 'complete') {
                                script.onreadystatechange = null;
                                onLoad(url, true);
                            }
                        };
                    }
                    else {
                        script.onload = function () {
                            setTimeout(function () {
                                onLoad(url, true);
                            }, 0);
                        };

                        script.onerror = function () {
                            onLoad(url, false);
                        };
                    }
                }

                // Append it to <head>.
                var head = document.getElementsByTagName('head')[0] || document.documentElement.appendChild(document.createElement('head'));
                head.appendChild(script);
            };

            showBusy && (document.body.style.cursor = 'wait');
            for (var i = 0; i < scriptCount; i++) {
                loadScript(scriptUrl[i]);
            }
        },

        /**
		 * 从js文件队列中加载js文件
         * 加载一个文件完成后，在加载完成的回调方法中加载下一个文件，所以同一时间只有一个js文件处于加载中
		 *
		 * @param {String} scriptUrl 要放入队列中进行加载的js Url.
		 * @param {Function(success)} [callback] 加载完成后的回调方法，它有一个success参数用来表示js文件是否加载成功
		 * @static method
		 */
        queue: (function () {
            var pending = [];

            // 加载队列中的下一个scriptUrl
            function loadNext() {
                var script;

                if ((script = pending[0]))
                    this.load(script.scriptUrl, script.callback, ciWong, 0);
            }

            return function (scriptUrl, callback) {
                var that = this;

                // scriptUrl加载完毕后的回调方法
                function callbackWrapper() {
                    //回调callback方法
                    callback && callback.apply(this, arguments);
                    // 将当前已经加载的scriptUrl从队列中删除
                    pending.shift();
                    //加载队列中下一个scriptUrl
                    loadNext.call(that);
                }

                // 将scriptUrl加入到队列中
                pending.push({ scriptUrl: scriptUrl, callback: callbackWrapper });

                //当队列不为空，并且是第一个加入队列的scriptUrl时，启动队列加载
                if (pending.length == 1)
                    loadNext.call(this);
            };
        })()
    };
})();

if (window.ciWong) {
    window.ciWong.scriptLoader = window.scriptLoader;
}


/*----------------------------- serialNumber.js.js --------------------------*/
/*
* 生成各种流水号或随机数等
*
* @static class
*/


window.serialNumber = (function () {
    var sequentialNumber = 1;

    var serialNumber = {
        /**
        * 生成随机数　
        *
        * @param {Number} length 随时数的长度，默认为3，最大为10
        * @static methods {Number}
        **/
        random: function (length) {
            (!length || length < 1) && (length = 3);    //length默认值为3
            length > 10 && (length = 10);               //length最大为10

            var number = parseInt('10000000000'.substring(0, length + 1));

            return Math.ceil(Math.random() * number);
        },
        /**
        * 根据当前日期生成序号
        *
        * @param {Number} randomLength 日期后边附加随时数的位数，默认为0
        **/
        date: function (randomLength) {
            var date = new Date();

            return date.getFullYear().toString()
                + (date.getMonth() + 1).toString()
                + date.getDate().toString()
                + date.getHours().toString()
                + date.getMinutes().toString()
                + date.getSeconds().toString()
                + date.getMilliseconds().toString()
                + serialNumber.random(randomLength || 1);
        },
        /**
        * 自增有序数，从1开始，每读取一次加1
        **/
        sequential: function () {
            return sequentialNumber++;
        }
    };

    return serialNumber;
})();

if (window.ciWong) {
    window.ciWong.serialNumber = window.serialNumber;
}

/*----------------------------- timeSpan.js.js --------------------------*/
/**
* timeSpan
*
* @class
* @requires convert
**/

(function () {

    function timeSpan(beginTime, endTime) {
        var _ = this._ = {
            totalMilliseconds: 0
        };

        var beginDate = ciWong.convert.toDate(beginTime),
            endDate = ciWong.convert.toDate(endTime);

        if (!beginDate || !endDate) {
            alert('beginTime或endTime无效，必须是时间类型，或时间格式的字符串。')
        }

        _.totalMilliseconds = Math.abs(endDate.getTime() - beginDate.getTime());

    }

    function calculateByDivision(left, right) {
        if (right == undefined || right == null) {
            right = this._.totalMilliseconds;
        }

        return Math.floor(right / left);
    };


    ciWong.timeSpan = timeSpan;

    ciWong.extend(timeSpan.prototype, {
        /*
        * 时间间隔的天数部分
        *@method {Number}
        */
        days: function () {
            return this.totalDays();
        },
        /*
        * 时间间隔的小时部分
        * @method {Number}
        */
        hours: function () {
            return this.totalHours(this._.totalMilliseconds % (1000 * 60 * 60 * 24));
        },
        /*
        * 时间间隔的分钟部分
        * @method {Number}
        */
        minutes: function () {
            return this.totalMinutes(this._.totalMilliseconds % (1000 * 60 * 60));
        },
        /*
        * 时间间隔的秒部分
        * @method {Number}
        */
        seconds: function () {
            return this.totalSeconds(this._.totalMilliseconds % (1000 * 60));
        },
        /*
        *　时间间隔的总小时数（天数 * 24 + 小时数）
        */
        totalDays: function (totalMilliseconds) {
            return calculateByDivision.call(this, 1000 * 60 * 60 * 24, totalMilliseconds);
        },
        /*
        *　时间间隔的总分钟数（(天数 * 24 + 小时数) * 60  + 分钟数）
        */
        totalHours: function (totalMilliseconds) {
            return calculateByDivision.call(this, 1000 * 60 * 60, totalMilliseconds);
        },
        /*
        * 时间间隔的总分钟数
        */
        totalMinutes: function (totalMilliseconds) {
            return calculateByDivision.call(this, 1000 * 60, totalMilliseconds);
        },
        /*
        * 时间间隔的总秒数
        */
        totalSeconds: function (totalMilliseconds) {
            return calculateByDivision.call(this, 1000, totalMilliseconds);
        }
    });

})();

/*----------------------------- encoder.js.js --------------------------*/
/**
* encoder　编码信息
*
* @static class encoder
* @static class ciWong.encoder
**/

window.encoder = (function () {
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
                        .replace(/&apos;/g, "'");

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
})();

if (window.ciWong) {
    window.ciWong.encoder = window.encoder;
}

/*----------------------------- utf8.js.js --------------------------*/
/**
* utf-8格式编码、解码
*
* @static class
**/


window.utf8Encoder = (function () {
    var encoder = {
        getBytes: function (str) {
            var bytes = new Array();
            var c;
            for (var i = 0; i < str.length; i++) {
                c = str.charCodeAt(i);
                // Convert char code to bytes.
                if (c < 0x80) {
                    bytes.push(c);
                } else if (c < 0x800) {
                    bytes.push(0xC0 | c >> 6);
                    bytes.push(0x80 | c & 0x3F);
                } else if (c < 0x10000) {
                    bytes.push(0xE0 | c >> 12);
                    bytes.push(0x80 | c >> 6 & 0x3F);
                    bytes.push(0x80 | c & 0x3F);
                } else if (c < 0x200000) {
                    bytes.push(0xF0 | c >> 18);
                    bytes.push(0x80 | c >> 12 & 0x3F);
                    bytes.push(0x80 | c >> 6 & 0x3F);
                    bytes.push(0x80 | c & 0x3F);
                } else {
                    // If char is unknown then push "?".
                    bytes.push(0x3F);
                }
            }
            return bytes;
        },
        getString: function (bytes) {
            var s = new String;
            var b;
            var b1;
            var b2;
            var b3;
            var b4;
            var bE;
            var ln = bytes.length;
            for (var i = 0; i < ln; i++) {
                b = bytes[i];
                if (!b) {
                    continue;
                }
                if (b < 0x80) {
                    // Char represended by 1 byte.
                    s += (b > 0) ? String.fromCharCode(b) : "";
                } else if (b < 0xC0) {
                    // Byte 2,3,4 of unicode char.
                } else if (b < 0xE0) {
                    // Char represended by 2 bytes.
                    if (ln > i + 1) {
                        b1 = (b & 0x1F); i++;
                        b2 = (bytes[i] & 0x3F);
                        bE = (b1 << 6) | b2;
                        s += String.fromCharCode(bE);
                    }
                } else if (b < 0xF0) {
                    // Char represended by 3 bytes.
                    if (ln > i + 2) {
                        b1 = (b & 0xF); i++;
                        b2 = (bytes[i] & 0x3F); i++;
                        b3 = (bytes[i] & 0x3F);
                        bE = (b1 << 12) | (b2 << 6) | b3;
                        s += String.fromCharCode(bE);
                    }
                } else if (b < 0xF8) {
                    // Char represended by 4 bytes.
                    if (ln > i + 3) {
                        b1 = (b & 0x7); i++;
                        b2 = (bytes[i] & 0x3F); i++;
                        b3 = (bytes[i] & 0x3F); i++;
                        b4 = (bytes[i] & 0x3F);
                        bE = (b1 << 18) | (b2 << 12)(b3 << 6) | b4;
                        s += String.fromCharCode(bE);
                    }
                } else {
                    s += "?";
                }
            }
            return s;
        }
    };

    return encoder;
})();

if (window.encoder) {
    window.encoder.utf8 = window.utf8Encoder;
}

/*----------------------------- arrayExpand.js.js --------------------------*/
/**
* 数据组对象扩展
*
* @expand Array
* @priority 99
* @requires utility
**/

(function () {

    function getStartIndex(length, startIndex) {
        return startIndex != undefined && startIndex != null
            ? startIndex < 0 ? Math.max(0, length + startIndex) : startIndex
            : 0;
    }

    var arrayExpand = {
        /**
        * 遍历数组
        *
        * @param {[Boolean] function(item,index,array)} callback 遍历回调方法，
        * @param {Object} [thisArg] 遍历回调方法中的this指针，默认为数组对象
        * @method {self} 这是一个方法，返回自己
        **/
        forEach: function (callback, thisArg) {
            if (!callback) {
                return this;
            }

            for (var i = 0; i < this.length; i++) {
                var value = callback.call(thisArg || this, this[i], i, this);
                if (value === false) {
                    break;
                }
            }

            return this;
        },
        /**
        * 遍历数组，并根据callback函数中返回的对象生成新的数组
        *
        * @param {function(item,index,array)} callback 遍历回调方法，此方法返回一个新的对象
        * @param {Object} [thisArg] 遍历回调方法中的this指针
        * @method {Array} 这是一个方法，返回新数组
        **/
        map: function (callback, thisArg) {
            var ret = new Array();
            for (var i = 0, j = this.length; i < j; i++) {
                var value = callback.call(thisArg || this, this[i], i, this);
                if (value != null) {
                    ret.push(value);
                }
            }

            return ret;
        },
        /**
        * 根据callback回调方法筛选数组
        * 
        * @param {{Boolean} function(item,index,array)} callback
        * @param {Object} [thisArg] 遍历回调方法中的this指针
        * @method {Array} 这是一个方法，返回新数组
        **/
        filter: function (callback, thisArg) {
            var ret = new Array();
            for (var i = 0, j = this.length; i < j; i++) {
                var item = this[i],
                    value = callback.call(thisArg || this, item, this);

                if (value === true) {
                    ret.push(item);
                }
            }

            return ret;
        },
        /*
        * 查询对象在数组中的索引位
        * 
        * @param {Object/Function} searchElement 要检查的对象
        * @param [Int] startIndex 开始索引位置，如果startIndex为负数，则为(Array.Length + startIndex)
        * @method {Int} 这是一个方法，返回一个int型索引值，如果对象没有在数组中，那么返回-1
        */
        indexOf: function (searchElement, startIndex, isFilter) {

            var length = this.length,
                i = getStartIndex(length, startIndex);

            var verify = function (item) {
                return item === searchElement;
            };

            //有可能数组中存储的就是function对象，所以这里如果是一个筛选functaion的话会产生冲突。
            //function增加一个isFilter属性来表示它是一个筛选function
            if (typeof searchElement === 'function' && isFilter) {
                verify = function (item) {
                    return searchElement.call(item, item);
                };
            }

            for (i; i < length; i++) {
                if (verify(this[i]))
                    return i;
            }

            return -1;
        },
        /*
        * 查询对象在数组中最后一个的索引位
        * 
        * @param {Object/Function} searchElement 要检查的对象
        * @param [Int] startIndex 开始索引位置，如果startIndex为负数，则为(Array.Length + startIndex)
        * @method {Int} 这是一个方法，返回一个int型索引值，如果对象没有在数组中，那么返回-1
        */
        lastIndexOf: function (searchElement, startIndex, isFilter) {

            var length = this.length,
                i = getStartIndex(length, startIndex);

            var verify = function (item) {
                return item === searchElement;
            };

            if (typeof searchElement === 'function' && isFilter) {
                verify = function (item) {
                    return searchElement.call(item, item);
                };
            }

            for (length; length >= i; length--) {
                if (verify.call(this[length]))
                    return length;
            }

            return -1;
        },
        /*
        * 查询对象是否存在于数组中
        * 
        * @param {Object/Function} searchElement 要检查的对象
        * @method {Boolean} 这是一个方法，返回一个Boolean型值
        */
        exists: function (searchElement, isFilter) {
            return this.indexOf(searchElement, 0, isFilter) >= 0;
        },
        /*
        * 根据索引移除数组中的对象
        * 
        * @param {Int} index 要移除对象的索引，如果索引为负数，那么值为则为(Array.Length + index)
        * @method {self} 这是一个方法，返回一个Boolean型值
        */
        removeAt: function (index) {

            if (this.length > index) {
                this.splice(getStartIndex(this.length, index), 1);
            }

            return this;
        },
        /*
        * 从数组中移除指定的对象
        * 
        * @param {Object/function} element 要移除的对象或移动的筛选方法
        * @method {self} 这是一个方法，返回一个Boolean型值
        */
        remove: function (element) {
            if (typeof element === 'function') {

                var i = 0;
                while (i < this.length) {
                    var item = this[i];
                    if (element(item) === true) {
                        this.splice(i, 1);
                    }
                    else {
                        i++;
                    }
                }
            }
            else {
                this.removeAt(this.indexOf(element));
            }

            return this;
        }
    };

    ciWong.extend(Array.prototype, arrayExpand);

})();

/*----------------------------- dateExpand.js.js --------------------------*/
/**
*
* @expand Date
* @requires utility
**/

(function () {

    var dateExpand = {
        /**
        * 时间字符串格式化
        *  格式：yyyy-MM-dd hh:ss SSS
        * 
        * @param {String} format 格式化形式字符串
        * @method {String}
        **/
        format: function (format) {
            var o = {
                "M+": this.getMonth() + 1,  //month
                "d+": this.getDate(),       //day
                "h+": this.getHours(),      //hour
                "m+": this.getMinutes(),    //minute
                "s+": this.getSeconds(),    //second
                "q+": Math.floor((this.getMonth() + 3) / 3), //quarter
                "S": this.getMilliseconds() //millisecond
            }
            if (/(y+)/.test(format)) {
                format = format.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
            }

            for (var k in o) {
                if (new RegExp("(" + k + ")").test(format)) {
                    format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
                }
            }
            return format;
        }
    };

    ciWong.extend(Date.prototype, dateExpand);

})();


/*----------------------------- regexpExpand.js.js --------------------------*/
/**
*
* @expand RegExp
* @requires utility
**/

(function () {

    var regexEnum =
    {
        intege: "^-?[1-9]\\d*$",					                //整数
        intege1: "^[1-9]\\d*$",					                    //正整数
        intege2: "^-[1-9]\\d*$",					                //负整数
        num: "^([+-]?)\\d*\\.?\\d+$",			                    //数字
        num1: "^[1-9]\\d*|0$",					                    //正数（正整数 + 0）
        num2: "^-[1-9]\\d*|0$",					                    //负数（负整数 + 0）
        decmal: "^([+-]?)\\d*\\.\\d+$",			                    //浮点数
        decmal1: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*$",　　	            //正浮点数
        decmal2: "^-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*)$",　            //负浮点数
        decmal3: "^-?([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0)$",　 //浮点数
        decmal4: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0$",　　      //非负浮点数（正浮点数 + 0）
        decmal5: "^(-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*))|0?.0+|0$",　　//非正浮点数（负浮点数 + 0）
        email: "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$", //邮件
        color: "^[a-fA-F0-9]{6}$",				//颜色
        url: "^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$",	//url
        chinese: "^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$",					//仅中文
        ascii: "^[\\x00-\\xFF]+$",				    //仅ACSII字符
        zipcode: "^\\d{6}$",						//邮编
        mobile: "^13[0-9]{9}|15[012356789][0-9]{8}|18[0256789][0-9]{8}|147[0-9]{8}$",				//手机
        ip4: "^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)$",	//ip地址
        notempty: "^\\S+$",						//非空
        picture: "(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$",	//图片
        rar: "(.*)\\.(rar|zip|7zip|tgz)$",								//压缩文件
        date: "^\\d{4}(\\-|\\/|\.)\\d{1,2}\\1\\d{1,2}$",					//日期
        qq: "^[1-9]*[1-9][0-9]*$",				//QQ号码
        tel: "^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$",	//电话号码的函数(包括验证国内区号,国际区号,分机号)
        username: "^\\w+$",						//用来用户注册。匹配由数字、26个英文字母或者下划线组成的字符串
        //title: "^[\\-\\w\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$",						//用来用户注册。匹配由数字、26个英文字母或者下划线组成的字符串
        title: "^[^<>]+$",						//标题，不允许输入<>来过滤html
        //title: "^[^`~!@#$%^&*()+=|\\\][\]\{\}:;'\,.<>/?]{1}[^`~!@$%^&()+=|\\\][\]\{\}:;'\,.<>?]{3}$"

        letter: "^[A-Za-z]+$"					//字母
    };

    //短时间，形如 (13:04:06)
    function isTime(str) {
        var a = str.match(/^(\d{1,2})(:)?(\d{1,2})\2(\d{1,2})$/);
        if (a == null) { return false }
        if (a[1] > 24 || a[3] > 60 || a[4] > 60) {
            return false;
        }
        return true;
    }

    //短日期，形如 (2003-12-05)
    function isDate(str) {
        var r = str.match(/^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2})$/);
        if (r == null) return false;
        var d = new Date(r[1], r[3] - 1, r[4]);
        return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4]);
    }

    //长时间，形如 (2003-12-05 13:04:06)
    function isDateTime(str) {
        var reg = /^(\d{1,4})(-|\/)(\d{1,2})\2(\d{1,2}) (\d{1,2}):(\d{1,2}):(\d{1,2})$/;
        var r = str.match(reg);
        if (r == null) return false;
        var d = new Date(r[1], r[3] - 1, r[4], r[5], r[6], r[7]);
        return (d.getFullYear() == r[1] && (d.getMonth() + 1) == r[3] && d.getDate() == r[4] && d.getHours() == r[5] && d.getMinutes() == r[6] && d.getSeconds() == r[7]);
    }

    var aCity = { 11: "北京", 12: "天津", 13: "河北", 14: "山西", 15: "内蒙古", 21: "辽宁", 22: "吉林", 23: "黑龙江", 31: "上海", 32: "江苏", 33: "浙江", 34: "安徽", 35: "福建", 36: "江西", 37: "山东", 41: "河南", 42: "湖北", 43: "湖南", 44: "广东", 45: "广西", 46: "海南", 50: "重庆", 51: "四川", 52: "贵州", 53: "云南", 54: "西藏", 61: "陕西", 62: "甘肃", 63: "青海", 64: "宁夏", 65: "新疆", 71: "台湾", 81: "香港", 82: "澳门", 91: "国外" }

    //身份证号验证
    function isCardID(sId) {
        var iSum = 0;
        var info = "";
        if (!/^\d{17}(\d|x)$/i.test(sId)) return "你输入的身份证长度或格式错误";
        sId = sId.replace(/x$/i, "a");
        if (aCity[parseInt(sId.substr(0, 2))] == null) return "你的身份证地区非法";
        sBirthday = sId.substr(6, 4) + "-" + Number(sId.substr(10, 2)) + "-" + Number(sId.substr(12, 2));
        var d = new Date(sBirthday.replace(/-/g, "/"));
        if (sBirthday != (d.getFullYear() + "-" + (d.getMonth() + 1) + "-" + d.getDate())) return "身份证上的出生日期非法";
        for (var i = 17; i >= 0; i--) iSum += (Math.pow(2, i) % 11) * parseInt(sId.charAt(17 - i), 11);
        if (iSum % 11 != 1) return "你输入的身份证号非法";
        return true;//aCity[parseInt(sId.substr(0,2))]+","+sBirthday+","+(sId.substr(16,1)%2?"男":"女") 
    }

    function verify(pattern, value) {
        if (!value) {
            return false;
        }

        if (typeof value != 'string') {
            value = '' + value; //将对象转为字符串
        }

        return new RegExp(pattern).test(value);
    }

    function verifyNumber(defaultPattern, positivePattern, negativePattern, value, numberOption) {
        switch (numberOption) {
            case undefined:
            case null:
                return verify(defaultPattern, value);
            case this.numberOptions.positive:
                return verify(positivePattern, value);
            case this.numberOptions.negative:
                return verify(negativePattern, value);
        }

        return verify(defaultPattern, value);
    }

    var regExpExpand = {
        /*
        * 数字型正则参数值
        *
        */
        numberOptions: {
            none: 0,             //不限制
            positive: 1,        //正数
            negative: 2          //负数
        },
        dateTimeOptions: {
            none: 0,             //默认dateTime格式 2003-12-05 13:04:06
            date: 1,             //短日期格式 2003-12-05
            time: 2              //时间格式 13:04:06
        },
        /**
        * 根据提供的正则表达式判断是否匹配指定字符串
        * @param {String/Pattern} pattern 要进行判断的字符串对象
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isMatch: function (pattern, value) {
            return new RegExp(pattern).test(value);
        },
        /**
        * 判断字符串是否为整数
        * @param {String} value 要进行判断的字符串对象
        * @param {RegExp.numberOptions} 判断选项
        * @method {Boolean}
        **/
        isIntege: function (value, numberOption) {
            return verifyNumber.call(this, regexEnum.intege, regexEnum.intege1, regexEnum.intege2, value, numberOption);
        },
        /**
        * 判断字符串是否为数字
        * @param {String} value 要进行判断的字符串对象
        * @param {RegExp.numberOptions} 判断选项
        * @method {Boolean}
        **/
        isNumber: function (value, numberOption) {
            return verifyNumber.call(this, regexEnum.num, regexEnum.num1, regexEnum.num2, value, numberOption)
        },
        /**
        * 判断字符串是否为浮点数
        * @param {String} value 要进行判断的字符串对象
        * @param {RegExp.numberOptions} 判断选项
        * @method {Boolean}
        **/
        isDecmal: function (value, numberOption) {
            return verifyNumber.call(this, regexEnum.decmal, regexEnum.decmal1, regexEnum.decmal2, value, numberOption)
        },
        /**
        * 判断字符串是否为Email
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isEmail: function (value) {
            return verify(regexEnum.email, value);
        },
        /**
        * 判断字符串是否为16进制颜色代码
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isColor: function (value) {
            return verify(regexEnum.color, value);
        },
        /**
        * 判断字符串是否为url
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isUrl: function (value) {
            return verify(regexEnum.url, value);
        },
        /**
        * 判断字符串是否为中文字符
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isChinese: function (value) {
            return verify(regexEnum.chinese, value);
        },
        /**
        * 判断字符串是否为邮政编码
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isZipCode: function (value) {
            return verify(regexEnum.zipcode, value);
        },
        /**
        * 判断字符串是否为ip地址
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isIP4: function (value) {
            return verify(regexEnum.ip4, value);
        },
        /**
        * 判断字符串是否为ip地址
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isEmpty: function (value) {
            return verify(regexEnum.notempty, value);
        },
        /**
        * 判断字符串是图片格式
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isPicture: function (value) {
            return verify(regexEnum.picture, value)
        },
        /**
        * 判断字符串是否为压缩文件 zip,rar等
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isRAR: function (value) {
            return verify(regexEnum.rar, value);
        },
        /**
        * 判断字符串是否为QQ号码
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isQQ: function (value) {
            return verify(regexEnum.qq, value);
        },
        /**
        * 判断字符串是否为电话号码
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isTel: function (value) {
            return verify(regexEnum.tel, value);
        },
        /**
        * 判断字符串是否为手机号码
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isMobile: function (value) {
            return verify(regexEnum.mobile, value);
        },
        /**
        * 用来用户注册。匹配由数字、26个英文字母或者下划线组成的字符串
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isUserName: function (value) {
            return verify(regexEnum.username, value);
        },
        /**
        * 匹配由数字、26个英文字母、中文或者下划线组成的字符串
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isTitle: function (value) {
            return verify(regexEnum.title, value);
        },
        /**
        * 是否为英文字母
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isLetter: function (value) {
            return verify(regexEnum.letter, value);
        },
        /**
        * 是否为身份证号
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isIdentityCard: function (value) {
            return isCardID(value);
        },
        /**
        * 是否为时间格式
        * @param {String} value 要进行判断的字符串对象
        * @method {Boolean}
        **/
        isDateTime: function (value, dateTimeOption) {
            switch (dateTimeOption) {
                case undefined:
                case null:
                    return isDateTime(value);
                case regExpExpand.dateTimeOptions.date:
                    return isDate(value);
                case regExpExpand.dateTimeOptions.time:
                    return isTime(value);
            }

            return isDateTime(value);
        }
    };

    //ciWong.extend不支持，所以需要自己regExpExpand copy到widow.RegExp
    for (var item in regExpExpand) {
        window.RegExp[item] = regExpExpand[item];
    }
})();


/*----------------------------- stringExpand.js.js --------------------------*/
/**
*
* @expand String
* @requires utility
**/

//#region 静态扩展

/**
* 字符串格式化
* @param {param String} str
* @method {String}
**/
window.String.format = (function () {
    return function () {
        var args = Array.prototype.slice.call(arguments),
            str = args.shift();

        return str.replace(/\{(\d+)\}/g,
            function (m, i) { return args[i]; }
        );
    };
})();

//#endregion 静态扩展

(function () {

    function trimChars(chars, option) {
        var removeValue = chars.join('');

        //all
        //因为\s\uFEFF\xA0赋值为字符串时会自动发生转变，使变在无效正则，所以这里使用？：分别处理两种情况
        var pattern = removeValue
            ? eval(String.format('/^[{0}]+|[{0}]+$/g', removeValue))
            : /^[\s\uFEFF\xA0]+|[\s\uFEFF\xA0]+$/;

        switch (option) {
            //left
            case 1:
                pattern = removeValue
                    ? eval(String.format('/^[{0}]+/g', removeValue))
                    : /^[\s\uFEFF\xA0]+/;
                break;
                //right
            case 2:
                pattern = removeValue
                    ? eval(String.format('/[{0}]+$/g', removeValue))
                    : /[\s\uFEFF\xA0]+$/;
                break;
        }

        return (this + '').replace(pattern, '');
    }

    var stringExpand = {

        /**
        * 判断字符串是否是以指定的字符为结尾
        * @method {Boolean}
        **/
        endsWith: function (suffix) {
            return this.indexOf(suffix, this.length - suffix.length) !== -1;
        },
        /**
        * 判断字符串是否是以指定的字符为开始
        * @method {Boolean}
        **/
        startsWith: function (suffix) {
            return this.indexOf(suffix) == 0;
        },
        /**
        * 删除字符串中的指定字符，并返回删除之后的新字符串
        * @param {String/Regex} removeValue
        * @method {String}
        **/
        remove: function (removeValue) {
            return (this + '').replace(removeValue, '');
        },
        /**
        * 删除头尾指定字符
        * @param [param Char] char 
        * @method {String}
        **/
        trim: function () {
            return trimChars.call(this, Array.prototype.splice.call(arguments));
        },
        /**
        * 删除尾部指定字符
        * @param [param Char] char 
        * @method {String}
        **/
        trimEnd: function () {
            return trimChars.call(this, Array.prototype.splice.call(arguments), 2);
        },
        /**
        * 删除头部指定字符
        * @param [param Char] char 
        * @method {String}
        **/
        trimStart: function () {
            return trimChars.call(this, Array.prototype.splice.call(arguments), 1);
        },
        /**
        * 将字符串第一个字母改为大写
        *
        **/
        toUpperCaseFirst: function () {
            return this ? this.charAt(0).toUpperCase() + this.substr(1) : '';
        }
    };

    ciWong.extend(String.prototype, stringExpand);

})();


/*----------------------------- httpBrowser.js.js --------------------------*/
/**
* 浏览器信息
*
* @static class httpBrowser
* @static class httpRequest.browser
* @static class httpContent.request.browser
* @static calss ciWong.httpContent.request.browser
**/

window.httpBrowser = (function () {
    var agent = navigator.userAgent.toLowerCase();
    var opera = window.opera;

    var env = {
        /**
         * 是否是ie浏览器
         * 
         *		if ( httpBrowser.ie )
         *			alert( 'I\'m running in IE!' );
         *
         * @property {Boolean}
         */
        ie: eval('/*@cc_on!@*/false'),
        /**
         * 是否是opera浏览器
         *
         *		if ( httpBrowser.opera )
         *			alert( 'I\'m running in Opera!' );
         *
         * @property {Boolean}
         */
        opera: (!!opera && opera.version),

        /**
         * 是否是一个基于WebKit的浏览器，如：Safari
         *
         *		if ( httpBrowser.webkit )
         *			alert( 'I\'m running in a WebKit browser!' );
         *
         * @property {Boolean}
         */
        webkit: (agent.indexOf(' applewebkit/') > -1),

        /**
         * 是否运行于 Adobe AIR.
         *
         *		if ( httpBrowser.air )
         *			alert( 'I\'m on AIR!' );
         *
         * @property {Boolean}
         */
        air: (agent.indexOf(' adobeair/') > -1),

        /**
         * 是否是运行于Macintosh(mac，苹果).
         *
         *		if ( httpBrowser.mac )
         *			alert( 'I love apples!'' );
         *
         * @property {Boolean}
         */
        mac: (agent.indexOf('macintosh') > -1),

        /**
         * 是否是运行于 Quirks Mode(如：ie的怪异模式).
         *
         *		if ( httpBrowser.quirks )
         *			alert( 'Nooooo!' );
         *
         * @property {Boolean}
         */
        quirks: (document.compatMode == 'BackCompat'),

        /**
         * 是否运行于 mobile 系统.
         *
         *		if ( httpBrowser.mobile )
         *			alert( 'I\'m running with CKEditor today!' );
         *
         * @property {Boolean}
         */
        mobile: (agent.indexOf('mobile') > -1),

        /**
         * 是否运行于 Apple iPhone/iPad/iPod devices.
         *
         *		if ( httpBrowser.iOS )
         *			alert( 'I like little apples!' );
         *
         * @property {Boolean}
         */
        iOS: /(ipad|iphone|ipod)/.test(agent),

        /**
         * 页面是否运行于安全加密形式，如https
         *
         *		if ( httpBrowser.secure )
         *			alert( 'I\'m on SSL!' );
         *
         * @property {Boolean}
         */
        secure: location.protocol == 'https:'
    };

    /**
     * 是否为基于 Gecko-based 的浏览顺, 如：Firefox.
     *
     *		if ( ciWong.browser.gecko )
     *			alert( 'I\'m riding a gecko!' );
     *
     * @property {Boolean}
     */
    env.gecko = (navigator.product == 'Gecko' && !env.webkit && !env.opera);

    /**
     * 是否是Chrome浏览器.
     *
     *		if ( httpBrowser.chrome )
     *			alert( 'I\'m running in Chrome!' );
     *
     * @property {Boolean} chrome
     */

    /**
    * 是否是Safari浏览器
    *
    *		if ( httpBrowser.safari )
    *			alert( 'I\'m on Safari!' );
    *
    * @property {Boolean} safari
    */
    if (env.webkit) {
        if (agent.indexOf('chrome') > -1)
            env.chrome = true;
        else
            env.safari = true;
    }

    var version = 0;

    // Internet Explorer 6.0+
    if (env.ie) {

        version = (env.quirks || !document.documentMode)
            ? parseFloat(agent.match(/msie (\d+)/)[1])
            : document.documentMode;

        env.ie11Compat = version == 11;
        env.ie10Compat = version == 10;
        env.ie9Compat = version == 9;
        env.ie8Compat = version == 8;
        env.ie7Compat = version == 7;
        env.ie6Compat = version < 7 || env.quirks;
    }

    // Gecko.
    if (env.gecko) {
        var geckoRelease = agent.match(/rv:([\d\.]+)/);
        if (geckoRelease) {
            geckoRelease = geckoRelease[1].split('.');
            version = geckoRelease[0] * 10000 + (geckoRelease[1] || 0) * 100 + (geckoRelease[2] || 0) * 1;
        }
    }

    // Opera 9.50+
    if (env.opera) {
        version = parseFloat(opera.version());
    }

    // Adobe AIR 1.0+
    if (env.air) {
        version = parseFloat(agent.match(/ adobeair\/(\d+)/)[1]);
    }

    // WebKit 522+ (Safari 3+)
    if (env.webkit) {
        version = parseFloat(agent.match(/ applewebkit\/(\d+)/)[1]);
    }

    env.version = version;

    /**
     * 判断是否兼容当前浏览器
     *
     *		if ( !httpBrowser.isCompatible )
     *			alert( '很遗憾，我们不支持您的浏览器!' );
     *
     * @property {Boolean}
     */
    env.isCompatible =
        env.iOS && version >= 534 ||
        !env.mobile && (
            (env.ie && version > 6) ||
            (env.gecko && version >= 10801) ||
            (env.opera && version >= 9.5) ||
            (env.air && version >= 1) ||
            (env.webkit && version >= 522) ||
            false
        );

    /**
     * 是否运行在HiDPI环境.(高像素密度屏)
     *
     *		if ( httpBrowser.hidpi )
     *			alert( '您正在使用高像素密度屏幕.' );
     *
     * @property {Boolean}
     */
    env.hidpi = window.devicePixelRatio >= 2;

    return env;
})();

if (window.httpRequest) {
    window.httpRequest.browser = window.httpBrowser;
}



/*----------------------------- httpContent.js.js --------------------------*/
/**
* 当前http请求相关的信息
* 
* @static class httpContent
* @static class ciWong.httpContent
**/

window.httpContent = (function () {

    var httpContent = {
    };

    return httpContent;

})();

if (window.ciWong) {
    window.ciWong.httpContent = window.httpContent;
}

/*----------------------------- httpRequest.js.js --------------------------*/
/**
* 当前httpRequest信息
*
*
**/

window.httpRequest = (function () {

    function setQueryValue(url, queryName, value) {
        var reg = new RegExp("(&|\\?)" + queryName.toLowerCase() + "=([^&]*)(&|$)");
        url = url.replace(reg, "");

        var split = "?";
        if (url.indexOf(split) >= 0) {
            split = "&";
        }
        return url + split + queryName + "=" + encodeURIComponent(value);
    }

    function getUrl(url) {
        //注：这里不能使用decodeURIComponent解码url因为有些参数可能传递的就是网址，一担解码后正则匹配就会出错
        //如:1.apsx?returnUrl=/home?id=1&name=2
        return (url || window.location.search.substr(1)).toLowerCase();
    }

    var request = {
        parseQueryString: function (url) {

            var str = getUrl(url).split('?')[1], items = str.split('&');
            var result = {};
            var arr;
            for (var i = 0; i < items.length; i++) {
                arr = items[i].split("=");
                result[arr[0]] = arr[1];
            }
            return result;
        },
        //取获参数值
        getQueryValue: function (queryName, url) {

            var match = getUrl(url).match(new RegExp("(^|[&\?])" + queryName.toLowerCase() + "=([^&]*)(&|$)"));

            return match != null ? decodeURIComponent(match[2]) : null;
        },
        setQueryValue: function () {

            var query = arguments[0];

            if (typeof query === 'string') {
                return setQueryValue(getUrl(arguments[2]), query, arguments[1]);
            }
            else {
                var url = getUrl(arguments[1]);
                for (var name in query) {
                    url = setQueryValue(url, name, query[name]);
                }

                return url;
            }
        }
    };

    return request;
})();

if (window.httpContent) {
    window.httpContent.request = window.httpRequest;
}

/*----------------------------- httpResponse.js.js --------------------------*/
/**
* 当前httpResponse信息
*
*
**/

window.httpResponse = (function () {
})();

if (window.httpContent) {
    window.httpContent.response = window.httpResponse;
}

if (!window.Namespace) {
    Namespace = new Object();
    Namespace.register = function (path) {
        var arr = path.split(".");
        var ns = "";
        for (var i = 0; i < arr.length; i++) {
            if (i > 0)
                ns += ".";
            ns += arr[i];
            eval("if(typeof(" + ns + ") == 'undefined') " + ns + " = new Object();");
        }
    }
}
