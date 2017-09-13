
$(function () {
    $("#InputAccount").on("change", function () {
        validateEmail($(this).val());
    });

    $('#Register_Form').submit(function () {
        validateEmail($("#InputAccount").val());

        ChkMobile();
    });

    $("#InputBirthDay").datetimepicker();

 

});

///驗證信箱
function validateEmail(email) {
    reg = /^[^\s]+@[^\s]+\.[^\s]{2,3}$/;
    if (!reg.test(email)) { 
        alert("E-Mail 格式錯誤!");
        $("#InputAccount").focus();
        return false;
    }
}

///驗證手機
function ChkMobile() {

    if ($("#InputPhone").val().length != 0) {

        var handy = $("#InputPhone").val().length;

        if (IsNumeric($("#InputPhone").val()) == false) {
            alert('手機號碼只能為數字！');
            $("#InputPhone").focus();
            return false;
        }
        if (IsNumeric($("#InputPhone").val()) == false) {
            alert('手機號碼只能為數字！');
            $("#InputPhone").focus();
            return false;
        }
        if (handy != 10) {
            alert('手機號碼只能為10碼！');
            $("#InputPhone").focus();
            return false;
        }
        if (Left($("#InputPhone").val(), 2) != '09') {

            alert('手機號碼只能為09開頭的數字！');
            $("#InputPhone").focus();
            return false;
        }
    }
}
