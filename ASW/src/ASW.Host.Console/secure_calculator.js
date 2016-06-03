var page = require('webpage').create();
page.navigationLocked;

page.open("secure_temp.html", function (status) {
    wait_form();
});

function wait_form() {
    setTimeout(function () {
        var hasFrom = page.evaluate(function () { return document.forms[0] ? true : false; });
        if (!hasFrom) {
            wait_form();
        } else {
            output();
        }
    }, 100);
}

function output() {
    var _frmData = page.evaluate(function () {
        var frm = document.forms[0];
        //not found
        if (!frm) return null;
        //got the form
        var len = frm.elements.length;
        var data = "", prefix = '[';
        for (var i = 0; i < len; i++) {
            data += prefix + '{Name:\'' + frm.elements[i].name + '\',\'Value\':\'' + frm.elements[i].value + '\'}';
            prefix = ",";
        }
        data += ']';

        //var data = "", prefix = '';
        //for (var i = 0; i < len; i++) {
        //    data += prefix + frm.elements[i].name + '=' + frm.elements[i].value;
        //    prefix = "&";
        //}

        return data;
    });

    console.log(_frmData);
    phantom.exit();
}