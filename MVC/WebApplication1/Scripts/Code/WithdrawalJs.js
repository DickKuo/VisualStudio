

$(function () {

    $('.selectbank').on("click", function () {
        $.ajax({
            type: "Post",
            url: "AjaxFuctions/Index",
            cache: false,
            async: false,
            data: {             
            },
            dataType: "json",
            success: function (data, status, xhr) {
                $.colorbox({
                    html: CreateTable(data, true),
                    width: 600,
                    height: 500
                });               
            },
            error: function (xhr, status, errorThrown) {
                alert("Error");
            }
        });
    });

    $(".btn-success").click(function () {

        if (parseFloat($("#Draw").val()) <= 0 ) {
            alert('請輸入金額');
            return false;
        }

        if ($("#BankAccount").val() == "" || $("#BankAccount").val() == null) {
            alert('銀行帳號未填寫');
            return false;
        }

        if ($("#BankName").val() == "" || $("#BankName").val() == null) {
            alert('銀行名稱未填寫');
            return false;
        }

        if ($("#BankCode").val() == "" || $("#BankCode").val() == null) {
            alert('銀行分行未填寫');
            return false;
        }

        if (!$("#AgreeCheck").prop("checked")) {
            alert(IsRead);
            return false;
        }

        $("#WithdrawalForm").submit();       

    });

});

///------------------------------------------------------------------------------
///燈箱顯示圖片
///------------------------------------------------------------------------------
function ShowImage(ItemData) {
    $.colorbox({
        html: ' <img class="preview" src="' + ItemData + '" />'
    });
}//end ShowImage


///------------------------------------------------------------------------------
///建立所有帳號的清單Table
///------------------------------------------------------------------------------
function CreateTable(data, IsInput) {
    var table = '<table class="table table-striped table-bordered table-hover" id="DataTable1">' +
                          '<thead>' +
                              '<tr>' +
                                  '<th style="text-align:center">銀行帳號</th>' +
                                  '<th style="text-align:center">銀行名稱</th>' +
                                  '<th style="text-align:center">銀行分行</th>' +
                                   '<th style="text-align:center">確認</th>' +
                              '</tr>' +
                          '</thead>' +
                          '<tbody>';
    for (var i = 0; i < data.length; i++) {
        table += '<tr id=' + data[i].SN + '>' +
                     '<td style="text-align:center">' + data[i].BankAccount + '</td> ' +
                     '<td style="text-align:center">' + data[i].BankName + '</td>' +
                     '<td style="text-align:center">' + data[i].BankCode + '</td>' +
                     "<td style='text-align:center' > <i class='glyphicon glyphicon-ok-sign' onclick='Select(" + JSON.stringify(data[i]) + ");' ></i></td>" +
                '</tr>';
    }
    table += '</tbody></table>'; 
    return table;
}//end CreateTable


function Select(data) {
    $("#BankAccount").val(data.BankAccount);
    $("#BankName").val(data.BankName);
    $("#BankCode").val(data.BankCode);   
    $.colorbox.close();
}