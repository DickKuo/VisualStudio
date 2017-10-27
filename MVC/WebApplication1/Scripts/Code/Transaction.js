$(function () {

    var Url = "Transaction/";

    $(".span_c1ick").on("click", function () {
        if (!$(this).hasClass("active")) {
            CahgePage($(this).parent().attr('id'));
        }
    });

    function CahgePage(Page) {
        $.ajax({
            type: "post",
            url: Url + "ChagePage",
            //不用傳參數的話，放個大括弧就好
            data: {
                BeginTime: $('#BeginTimePicker').val(),
                EndTime: $('#EndTimePicker').val(),
                Page: Page,
                Range:10,
                TradeType: $('#TradeType').val(),
                Money: $('#Money').val(),
                AuditState: $('#AuditState').val()
            },
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: false,//由於最後需要使用ajax取得的result的數值，必須設定為false(才會變成sync同步執行）
            cache: false, //防止ie8一直取到舊資料的話，請設定為false
            success: function (result) {
                $("#_TransactionTable").html(result);
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    }


});
