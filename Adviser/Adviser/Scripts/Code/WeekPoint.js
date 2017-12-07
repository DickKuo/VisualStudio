
$(function () {    
    $('.btn-info').on('click', function () {
        $.ajax({
            type: "post",
            url: "../Report/SearchWeekPoint",
            //不用傳參數的話，放個大括弧就好
            data: {
                Year: $('#ListYear').val(),               
            },
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            async: false,//由於最後需要使用ajax取得的result的數值，必須設定為false(才會變成sync同步執行）
            cache: false, //防止ie8一直取到舊資料的話，請設定為false
            success: function (result) {
                $("#WeekPointPartial").html(result);
            },
            failure: function (response) {
                alert(response.d);
            }
        });
    });    
}).ajaxStart(function () {
    $.fancybox.showLoading();
}).ajaxComplete(function () {
    $.fancybox.hideLoading();
});