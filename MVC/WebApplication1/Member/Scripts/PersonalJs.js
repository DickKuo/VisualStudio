
$(function () {
   
    $("#btn_Change").on("click", function () {
        if ($("#InputNewPassWord").val() != $("#InputPassWordComfirm").val()) {
            alert('新密碼必須與確認密碼相同');
            $("#InputPassWordComfirm").focus();
            return false;
        }

        if ($("#InputNewPassWord").val().length < 8)
        {
            $("#InputNewPassWord").focus();
            alert('新密碼長度不足');
            return false;
        }
                
    });   
        
});
