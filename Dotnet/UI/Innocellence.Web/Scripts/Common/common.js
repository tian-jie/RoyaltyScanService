//$.metadata.setType("attr", "validate");

var Common = {};
Common.local = {
    ajaxErrMsg: 'Server Error!',
    ajaxJsonTypeErrMsg: 'Operate Fail,Wrong return data type!',
    ajaxErrTitle: '',//Operate Fail ! 
    tableDateErrMsg: 'Server Error!',
    tableLanguage: {},
    UpdateFormTitle: 'Add/Edit',
    FormGetErrMsg: 'Please select to edit the data read failure or has been deleted!',
    FormSelectErrMsg: 'Please select to edit the line or lines of more than one choice!',
    FromAddSuccess: 'Add Success!',
    FromUpdateSuccess: 'Update Success!',
    FormDeleteMsg: 'Sure you want to delete the selected data?',
    FormDeleteSelectMsg: 'Please choose to operate rows of data!',
    FormConfirmBtn: 'Confirm',
    FormCancelBtn: 'Cancel',
};


$.ajaxSetup({
    dataFilter: function (data, type) {

        window.clearTimeout(_tHandleAjaxLoading);
        ajaxLoadEnd();

        data = data.replace(/(\w+"\:")\\\/Date\((\d+)\)\\\//g, function ($0, $1, $2) {
            var strName = $1.replace(/"/g, '').replace(":", "");
            var dDate = new Date(parseInt($2));
            return $1 + DateParse(strName, dDate);
        });
        if (data.indexOf('"_JsonFlag_":"__"') >= 0 && data.indexOf('"Data":null') >= 0) {
            ReturnValueFilter($.parseJSON(data), 1);
        }
        // alert(data);
        return data;
    },
    success: function (data, textStatus, jqXHR) {
        //alert(data);
    },
    error: function (XMLHttpRequest, textStatus, errorThrown) {
        artDialog.alert(Common.local.ajaxErrMsg + "Code:" + XMLHttpRequest.status + " Message:" + errorThrown);
    },
    complete: function (XMLHttpRequest, textStatus) {

    }
}); //全局变量，标记加载条是否已经显示
var _tHandleAjaxLoading;

//全局ajax处理，加载进度条。
$(document).ajaxSend(function (evt, request, settings) {
    if (_tHandleAjaxLoading != null) { window.clearTimeout(_tHandleAjaxLoading); }
    _tHandleAjaxLoading = window.setTimeout(function () { ajaxLoading(); }, 500);
});

//全局ajax处理，隐藏进度条。
$(document).ajaxStop(function () {
    window.clearTimeout(_tHandleAjaxLoading);
    ajaxLoadEnd();
});

//ajax弹出加载条
function ajaxLoading(strMsg) {
    var dPanel = window.top.$(".window-mask");
    var dBody = "body";
    if (strMsg == undefined) { strMsg = '&nbsp;&nbsp;&nbsp;&nbsp;Loading......&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;'; }

    if (dPanel.length > 0 && dPanel.css("display") != "none") {
        dBody = window.top.$("#dd").length > 0 ? window.top.$("#dd").parent() : window.top.$("body");
        $("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: dBody.width() + 10, height: dBody.height() + 10 }).appendTo(dBody);
        // $("<div class=\"datagrid-mask-msg\"></div>").html(strMsg).appendTo(dBody).css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2 });
        $("<div class=\"datagrid-mask-msg\"></div>").html(strMsg).appendTo(dBody).css({ display: "block", left: (dBody.width() - $("div.datagrid-mask-msg", dBody).outerWidth()) / 2, top: (dBody.height() - $("div.datagrid-mask-msg", dBody).outerHeight()) / 2 });
    } else {
        $("<div class=\"datagrid-mask\"></div>").css({ display: "block", width: "100%", height: $(window).height() }).appendTo("body");
        $("<div class=\"datagrid-mask-msg\"></div>").html(strMsg).appendTo("body").css({ display: "block", left: ($(document.body).outerWidth(true) - 190) / 2, top: ($(window).height() - 45) / 2 });
    }
    //alert($(".datagrid-mask").length);
}
//隐藏
function ajaxLoadEnd() {
    $(".datagrid-mask").remove();
    $(".datagrid-mask-msg").remove();
}



$(document).ajaxComplete(function (evt, request, settings) {
    if (request.responseText.indexOf('_loginID_') > 0) {
        window.top.location = '/login/index';
    }


    // alert(request.responseText);
    // request.responseText = request.responseText.replace(/\\\/Date\((\d+)\)\\\//g, function ($0,$1) {
    //   return   DateParse($1);
    // });
    // alert(request.responseText);
});


function DateParse(strName, dDate) {
    //strDate
    return dDate.pattern("yyyy-MM-dd HH:mm");
}

function InitCKEditor(objID) {
    if (typeof CKEDITOR == undefined) { return; }

    eval('var _editor=CKEDITOR.instances.' + objID);

    if (typeof (_editor) != 'undefined' && _editor) { _editor.destroy(); _editor = null; }

    _editor = CKEDITOR.replace(objID);
    _editor.setData($('#' + objID).val());

    _editor.on('change', function () {
        $('#' + objID).val(_editor.getData());
    });

    _editor.on('mode', function () {
        if (this.mode == 'source') {
            var editable = _editor.editable();
            editable.attachListener(editable, 'input', function () {
                $('#' + objID).val(_editor.getData());
            });
        }
    });
}


//更新等ajax操作返回值处理
function ReturnValueFilter(objValue, oType) {
    // alert(strValue);
    if (objValue == null || objValue.Message == undefined
        || objValue.Message.Status == undefined) { artDialog.alert(Common.local.ajaxJsonTypeErrMsg); return false; }
    var strKey = objValue.Message.Status;
    if (oType == undefined || oType == null) { if (strKey == 200) { return true; } else { return false; } }

    switch (strKey) {
        case 200:
            if (objValue.Message.Text == "/lccpadmin/Task/")
            {
                window.location.href = objValue.Message.Text;
            }
            return true;
            break;
        case 500:
        case 501:
        case 400:
        case 404:
            artDialog.alert(Common.local.ajaxErrTitle + objValue.Message.Text);
            return false;
            break;
        case 401:
        case 403:
            //redirect to login form
            window.location.href = '/403.html';
            return false;
            break;
        case 103:
            artDialog.alert(objValue.Message.Text);

            return false;
            break;
        default:
            break;

    }
    return true;
}

// Ajax 文件下载
jQuery.download = function (url, data, method) {    // 获得url和data
    if (url && data) {
        // data 是 string 或者 array/object
        data = typeof data == 'string' ? data : jQuery.param(data);        // 把参数组装成 form的  input
        var inputs = '';
        jQuery.each(data.split('&'), function () {
            var pair = this.split('=');
            inputs += '<input type="hidden" name="' + pair[0] + '" value="' + pair[1] + '" />';
        });        // request发送请求
        jQuery('<form action="' + url + '" method="' + (method || 'post') + '">' + inputs + '</form>')
        .appendTo('body').submit().remove();
    }
};

/**     
 * 对Date的扩展，将 Date 转化为指定格式的String     
 * 月(M)、日(d)、12小时(h)、24小时(H)、分(m)、秒(s)、周(E)、季度(q) 可以用 1-2 个占位符     
 * 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)     
 * eg:     
 * (new Date()).pattern("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423     
 * (new Date()).pattern("yyyy-MM-dd E HH:mm:ss") ==> 2009-03-10 二 20:09:04     
 * (new Date()).pattern("yyyy-MM-dd EE hh:mm:ss") ==> 2009-03-10 周二 08:09:04     
 * (new Date()).pattern("yyyy-MM-dd EEE hh:mm:ss") ==> 2009-03-10 星期二 08:09:04     
 * (new Date()).pattern("yyyy-M-d h:m:s.S") ==> 2006-7-2 8:9:4.18     
使用：(eval(value.replace(/\/Date\((\d+)\)\//gi, "new Date($1)"))).pattern("yyyy-M-d h:m:s.S");
 */
Date.prototype.pattern = function (fmt) {
    var o = {
        "M+": this.getMonth() + 1, //月份        
        "d+": this.getDate(), //日        
        "h+": this.getHours() % 12 == 0 ? 12 : this.getHours() % 12, //小时        
        "H+": this.getHours(), //小时        
        "m+": this.getMinutes(), //分        
        "s+": this.getSeconds(), //秒        
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度        
        "S": this.getMilliseconds() //毫秒        
    };
    var week = {
        "0": "/u65e5",
        "1": "/u4e00",
        "2": "/u4e8c",
        "3": "/u4e09",
        "4": "/u56db",
        "5": "/u4e94",
        "6": "/u516d"
    };
    if (/(y+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    }
    if (/(E+)/.test(fmt)) {
        fmt = fmt.replace(RegExp.$1, ((RegExp.$1.length > 1) ? (RegExp.$1.length > 2 ? "/u661f/u671f" : "/u5468") : "") + week[this.getDay() + ""]);
    }
    for (var k in o) {
        if (new RegExp("(" + k + ")").test(fmt)) {
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        }
    }
    return fmt;
}; //字符串截取,中文算两个字符
function subStrCN(str, len, strPad) {
    if (!str || !len) { return ''; }
    var a = 0;
    var i = 0;
    var temp = '';
    for (i = 0; i < str.length; i++) {
        if (str.charCodeAt(i) > 255) { a += 2; }
        else { a++; }
        if (a > len) { return temp + (strPad == null ? "..." : strPad); }
        temp += str.charAt(i);
    }
    return str;
}

//string类的扩展函数，获取中文字符串长度
String.prototype.lengthCN = function () {
    return this.replace(/[^\u0000-\u007f]/g, "*").length;
};
String.prototype.replaceAll = function (s1, s2) {
    return this.replace(new RegExp(s1, "gm"), s2);
};
String.prototype.Trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
};

//获取当前窗口链接的参数
function getUrlParam(parameters) {
    var reg = new RegExp("(^|&)" + parameters + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]);
    return null;
}


jQuery.validator.addMethod("WChar", function (value, element) {
    var tel = /^\w+$/;
    return this.optional(element) || (tel.test(value));
}, "Please enter a valid char or number. ");

$(function () {
    Layout.Load();
});

var Layout = {
    Load: function() {                
        Layout.Event.TopMenu();
    },
    Event:
        {
            TopMenu: function () {
                var pathname = window.location.pathname;
                $("#navbar .navbar-buttons ul li").removeClass("layout-top-active");

                $("#navbar .navbar-buttons ul li a").each(function () {
                    if (this.pathname == pathname && this.className !== "dropdown-toggle")
                    {
                        $(this.parentNode).addClass("layout-top-active")
                    }
                })
            }
        }
}


//Judge Data Function
$.extend({
    JudgeNull: function (Value) {
        if (Value == null || Value == undefined) {
            return true;
        };
        if ($.JudgeString(Value)) {
            if (Value.trim().length == 0) {
                return true;
            };
        } else if ($.JudgeNumber(Value)) {
            // if (Value == _Option.Default.Key) {
            //     return true;
            // };
        } else if ($.JudgeArray(Value)) {
            if (Value.length == 0) {
                return true;
            };
        } else if ($.JudgeObject(Value)) {
            var Array = [];
            for (name in Value) {
                Array.push(Value[name])
            };
            if (Array.length == 0) {
                return true;
            };
        };
        return false;
    },
    JudgeString: function (Value) {
        return Value != null && Value != undefined && Value.constructor == String;
    },
    JudgeNumber: function (Value) {
        return Value != null && Value != undefined && Value.constructor == Number;
    },
    JudgeArray: function (Value) {
        return Value != null && Value != undefined && Value.constructor == Array;
    },
    JudgeObject: function (Value) {
        return Value != null && Value != undefined && Value.constructor == Object;
    },
    JudgeFunction: function (Value) {
        return Value != null && Value != undefined && Value.constructor == Function;
    },
    JudgeInteger: function (Value, Range) {
        Range = Range || '+';
        var Judge = false;
        if (Range.indexOf('-') != -1) {
            if (/^-[1-9]\d*$/.test(Value)) {
                Judge = true;
            };
        } else if (Range.indexOf('0') != -1) {
            if (Value == 0) {
                Judge = true;
            };
        } else if (Range.indexOf('+') != -1) {
            if (/^[1-9]\d*$/.test(Value)) {
                Judge = true;
            };
        };
        return Judge;
    },
    JudgeDecimal: function (Value, Digit, Range) {
        Digit = Digit || 2,
        Range = Range || '+';
        var Judge = false;
        if (Range.indexOf('-') != -1) {
            if (eval('/^-[0-9]+([.]{1}[0-9]{1,' + Digit + '})?$/.test(Value)')) {
                Judge = true;
            };
        } else if (Range.indexOf('0') != -1) {
            if (Value == 0) {
                Judge = true;
            };
        } else if (Range.indexOf('+') != -1) {
            if (eval('/^[0-9]+([.]{1}[0-9]{1,' + Digit + '})?$/.test(Value)')) {
                Judge = true;
            };
        };
        return Judge;
    },
    JudgeUser: function (FormSettings, SuccessCallBack, FailCallBack) {
        if ($.JudgeNull(_Data.CurrentUser)) {
            $.Alert(_PromptMessage.AccountNull, function () {
                if (_Environment == _EnvironmentSeetings.IPAD) {
                    salesPortalLogin($.LogOff);
                } else {
                    $.JumpTo($.GetRootPath() + 'login.aspx?invalid=1&ReturnUrl=' + window.location.pathname + window.location.search);
                };
            });
            return false;
        };
        if ($.ParseNumber(_Data.CurrentUser.Flag) == 1) {
            $.Alert(_PromptMessage.AccountInactive, function () {
                if (_Environment == _EnvironmentSeetings.IPAD) {
                    salesPortalLogin($.BackToHome);
                } else {
                    $.JumpTo($.GetRootPath() + 'login.aspx?invalid=2&ReturnUrl=' + window.location.pathname + window.location.search);
                };
            });
            return false;
        };
        var Authority = $.GetRoleNav(_Data.CurrentUser.Role, FormSettings.RoleList, FormSettings.NavList);
        var Current = $.GetCurrentNav(FormSettings.NavList);
        if ($.JudgeNull(Authority)) {
            $.Alert(_PromptMessage.AuthorityNull, $.BackToHome);
        } else {
            if ($.JudgeAuthority(Authority, Current)) {
                SuccessCallBack();
            } else {
                $.Alert(_PromptMessage.AuthorityNull, FailCallBack);
            };
        };
    },
    JudgeAuthority: function (Authority, Current) {
        if ($.JudgeNull(Authority)) {
            return false;
        };
        if ($.JudgeNull(Current)) {
            return false;
        };
        for (var i = 0; i < Authority.length; i++) {
            if (Authority[i].Id == Current.Id) {
                return true;
            };
        };
        return false;
    },
    JudgeEmail: function (Value) {
        var RegExp = /\w@\w*\.\w/;
        return RegExp.test(Value);
    },
});