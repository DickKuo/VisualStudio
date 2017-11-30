
$(function () {    
    //$("#InputAccount").on("change", function () {   
    //    validateEmail($(this).val());
    //});
           
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
                        $("#Register_Form").submit();
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
    }//end fancyconfirm
         
    $('.btn-primary').on('click',function(){
       
        var Isemail = false;

        Isemail = validateEmail($("#InputAccount").val());
              
        if (!Isemail) {
            alert('Email 格式錯誤');
            return false;
        }

        if ($("#InputNickName").val() == "")
        {
            alert('請填寫暱稱');
            return false;
        }
            
        if ($("#InputPassword").val() == "") {
            alert('請填寫密碼');
            return false;
        }
        else {
            if ($("#InputPassword").val().length < 8)
            {
                alert('密碼長度請大於8碼');
                return false;
            }
        }
       
          //ChkMobile();                
        fancyconfirm("請詳細閱讀以下說明<br>" +
                        "<ul  > " + Role + "</ul>"
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

       return false;

    });
    
    $("#InputBirthDay").datepicker({
        format: "yyyy/mm/dd"
    });
 
});





///驗證信箱
function validateEmail(email) {
    var reg = /[\w-]+@([\w-]+\.)+[\w-]+/;
     //reg = /^[^\s]+@[^\s]+\.[^\s]{2,3}$/;   
    if (!reg.test(email)) {
        return false;
    }
    else {
        return true;
    }
}

///驗證手機
function ChkMobile() {

    if ($("#InputPhone").val().length != 0) {

        var handy = $("#InputPhone").val().length;

        if ($.isNumeric($("#InputPhone").val()) == false) {
            alert('手機號碼只能為數字！');
            $("#InputPhone").focus();
            return false;
        }
        if ($.isNumeric($("#InputPhone").val()) == false) {
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
