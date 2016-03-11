//--------------------------------------------------------------
//加金額的按鈕控制
//-------------------------------------------------------------
function OnAddMoney(ItemData) {    
    $.ajax({
        type: "POST",
        url: "BankAccountPartial",
        data: {
            "BankNo": $('input[name=group2]:checked').attr('id'),
            "EmpNo": $('#SelectEmpNo').text(),
            "Money": $('#Form_Money').val(),
            'Operation': 0
        },
        success: function (TradeInfo)
        {
            var code = new ResultCode();
            if (code.Sucess == TradeInfo.ApiResultCode)
            {
                alert('加金額成功');
            }
            else
            {
                alert('加金額失敗');
            }
        }
    });    
}//  end  OnAddMoney


//--------------------------------------------------------------
//減金額的按鈕控制
//-------------------------------------------------------------
function OnReduceMoney() {  
    $.ajax({
        type: "POST",
        url: "BankAccountPartial",
        data: {
            "BankNo": $('input[name=group2]:checked').attr('id'),
            "EmpNo": $('#SelectEmpNo').text(),
            "Money": $('#Form_Money').val()*-1,
            'Operation': 1
        },
        success: function (TradeInfo) {
            var code = new ResultCode();
            if (code.Sucess == TradeInfo.ApiResultCode) {
                alert('減金額成功');
            }
            else {
                alert('減金額失敗');
            }
        }
    });
}//  end  OnReduceMoney




//--------------------------------------------------------------
//載入金流商輸入
//-------------------------------------------------------------
function OnCashClick(ItemData) {        
    $('#SelectEmpNo').text(ItemData.EmpNo);
    $.ajax({
        type: "POST",
        url: "CashFlowPartialViewAction",
        data: {
            "EmployeeJson": JSON.stringify(ItemData)
        },
        success: function (Model) {
            $("#PartialView").html(Model);
            PowerSwitch("PartialView", "BrowseView")
        }
    });
}// end OnCashClick