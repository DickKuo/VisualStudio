
$(function () {
    var Url = "http://192.168.1.111:8022/Report/";
    $(".span_c1ick").on("click", function () {
        if (!$(this).hasClass("active"))
        {
            CahgePage($(this).parent().attr('id'));
        }      
    });

    $("#Previous").on("click", function () {
        var Page = $(".active").attr('id');
        Page = Page - 1;
        if (Page <= 0) {
            Page = 1;
        }
        CahgePage(Page);
    });

    $("#Next").on("click", function () {
        var NextPage = $(".active").attr('id');
        NextPage = parseInt(NextPage) + 1;
        CahgePage(NextPage);
    });

    function CahgePage(Page) {
        $.ajax({
            type: "post",
            url: Url + "ChagePage",
            //不用傳參數的話，放個大括弧就好
            data: {
                BeginTime: $('#datetimepicker4').val(),
                EndTime: $('#datetimepicker5').val(),
                Page: Page
            },
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: false,//由於最後需要使用ajax取得的result的數值，必須設定為false(才會變成sync同步執行）
            cache: false, //防止ie8一直取到舊資料的話，請設定為false
            success: function (result) {
                $("#_ReportTable").html(result);
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    }

    function fancyconfirm(msg, options, callback) {
        $.fancybox("#confirm", {
            modal: options.modal,
            beforeShow: function () {
                this.content.prepend("<p class=\"title\"></p>");
                $(".title").html(msg);
                if (options.buttons == null) {
                    options.buttons = [{
                        clsName: "yes",
                        title: "Yes",
                        value: true
                    }, {
                        clsName: "no",
                        title: "No",
                        value: false
                    }];
                }
                for (i = 0; i < options.buttons.length; i++) {
                    this.content.append($("<input>", {
                        type: "button",
                        class: "confirm " + options.buttons[i].class,
                        value: options.buttons[i].title
                    }).data("index", i).css("margin-left", ((i > 0) ? "10px" : "")));
                }
                $(this.content).css("text-align", "center");
            },
            afterShow: function () {
                $(".confirm").on("click", function (event) {
                    ret = options.buttons[$(event.target).data("index")].value;
                    if (ret == "yes") {
                        Commd();
                    }
                    else {
                        $.fancybox.close();
                    }
                });
            },
            afterClose: function () {
                this.content.html("");
                callback.call(this, ret);
            }
        });
    }
    
    $(".btn-primary").on("click", function () {
        fancyconfirm("Do you wish to continue?<br>" +
               "<label class='col-xs-12 control-label'> 201707 </label>" +
               "<div class='form-group'  style='padding-top:25px;'>" +
                       "<label class='col-xs-4 control-label'>Contract</label>" +
                   "<div class='col-xs-8'>" +
                       "<input class='form-control'  id='Contract' readonly='true'  type='text' value='" + $(this).parent().parent().children().eq(2).html() + "'>" +
                   "</div>" +
               "</div>" +
               "<input   id='BtnSN' hidden='hidden' type='text' value='" + $(this).attr('id')+ "' >" +
               "<div class='form-group' style='padding-top:25px;'>" +
                   "<label class='col-xs-4 control-label'> StopPrice </label>" +
                   "<div class='col-xs-8'>" +
                       "<input class='form-control' id='StopPrice'  type='text'  >" +
                   "</div>" +
               "</div>" +
                "<div class='form-group'  style='padding-top:25px;'>" +
                       "<label class='col-xs-4 control-label'>Lot</label>" +
                   "<div class='col-xs-8'>" +
                       "<input class='form-control'  id='Lot'    type='text' value='1'>" +
                   "</div>" +
               "</div>" +

               "</div> <br/>"
             , {
                 buttons: [{
                     class: "yes",
                     type: "button",
                     title: "Yes",
                     value: "yes"
                 }, {
                     class: "no",
                     type: "button",
                     title: "No",
                     value: "no"
                 }],
                 modal: true
             },
        function (resp) {
            if (resp == "yes") {
            }
        });     
    });

    function Commd()
    {
        $.ajax({
            type: "post",
            url: Url + "Pyeongchang",
            //不用傳參數的話，放個大括弧就好
            data: {
                SN: $("#BtnSN").val(),
                StopPrice: $("#StopPrice").val(),
                Lot: $("#Lot").val()
            },
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: false,//由於最後需要使用ajax取得的result的數值，必須設定為false(才會變成sync同步執行）
            cache: false, //防止ie8一直取到舊資料的話，請設定為false
            success: function (result) {
                alert(result);
                $.fancybox.close();
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    }

});