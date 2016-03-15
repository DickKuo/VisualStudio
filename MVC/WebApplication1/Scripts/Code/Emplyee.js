
var BrowseView = "#BrowseView";         //員工列表Div
var PartialView = "#PartialView";       //動態展開Div
var IsCancel = false;                   //按下取消扭



//---------------------------------------------------------------------------
//展開員工瀏覽列表
//--------------------------------------------------------------------------
function LoadView()
{
    $(BrowseView).show();
    $(PartialView).hide();
}//end LoadView


//----------------------------------------------------------------------------
//隱藏員工瀏覽列表
//----------------------------------------------------------------------------
function UnLoadView()
{
    $(BrowseView).hide();
    $(PartialView).show();
}//end UnLoadView


//-----------------------------------------------------------------------------
//新增事件
//-----------------------------------------------------------------------------
function OnAddClick()
{
    UnLoadView();

    //$("#Load_Add_Form").submit();    

    $.ajax({
        type: "POST",
        url: "ADDPartialViewAction",        
        success: function (Model) {
            $("#PartialView").html(Model);
        }
    });

} // end OnAddClick


//------------------------------------------------------------------------------
//取消新增
//-----------------------------------------------------------------------------
function OnAddCancel()
{
    LoadView();
}// end OnAddCancel


//------------------------------------------------------------------------------
//新增結束後的處理
//-----------------------------------------------------------------------------
function OnAddResult(ItemData)
{
    if (ItemData != null) {
        FadAlert("新增成功");
        $("#BrowseTable tr:last").after("<tr Id=Emp_" + ItemData.EmpNo + "><td>" + ItemData.EmpNo + "</td><td>" + ItemData.EmpFirstName + "</td>" +
            "<td>" + ItemData.EmpLastName + "</td>" + " <td>" + ItemData.Tel + "</td> <td>" + ItemData.Addr + "</td> " +
            " <td> <input type='submit' value='編輯' onclick='OnEditClick(" + JSON.stringify(ItemData) + ")'  /> " +
            " </td> <td>  <input type='submit' name='delete' value='刪除' onclick='OnDeleteClick(" + JSON.stringify(ItemData) + ")' /> </td> </tr>");
        LoadView();
    }
    else {
        FadAlert("新增失敗。");
    }
}// end OnAddResult


//------------------------------------------------------------------------------
//編輯按鈕按下的處理
//------------------------------------------------------------------------------
//function OnEditClick(ItemData)
//{
//    $("#Hidden_EmployeeEidtJson").val(JSON.stringify(ItemData));  
//    $("#Load_Edit_Form").submit();
//    UnLoadView();
//    IsCancel = false;
//}// end OnEditClick


//------------------------------------------------------------------------------
//編輯按鈕按下的處理
//------------------------------------------------------------------------------
function EditLoad(ItemData) {
    UnLoadView();
    $.ajax({
        type: "POST",
        url: "EditPartialViewAction",
        data:{
            "EmployeeJson": JSON.stringify(ItemData)
        },
        success: function (Model) {         
            $("#PartialView").html(Model);
        }
    });
}// end OnEditClick



   
//-------------------------------------------------------------------------------
//編輯按鈕取消
//-------------------------------------------------------------------------------
function OnEditCancel() {
    LoadView();
    IsCancel = true;
}// end OnEditCancel


//------------------------------------------------------------------------------
//編輯完成後的處理
//--------------------------------------------------------------------------------
function OnAjaxSuccesForEdit(ItemData) {
    if (ItemData != null) {
        if (!IsCancel) {
            alert("編輯成功。");
            $("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(1)").text(ItemData.EmpFirstName);
            $("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(2)").text(ItemData.EmpLastName);
            $("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(3)").text(ItemData.Tel);
            $("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(4)").text(ItemData.Addr);
            LoadView();
        }
    }
    else {
        FadAlert("編輯失敗。");
    }
}//end OnAjaxSuccesForEdit


//-----------------------------------------------------------------------------------
//刪除事件
//-------------------------------------------------------------------------------------
function OnDeleteClick(ItemData)
{
    if (confirm("確定要刪除嗎?"))
    {
        //$("#Hidden_EmployeeDeleteJson").val(JSON.stringify(ItemData));
        //$("#Load_Delete_Form").submit();

        $.ajax({
            type: "POST",
            url: "DeleteAction",
            data: {
                "EmployeeJson": JSON.stringify(ItemData)
            },
            success:
                DeleteResult(ItemData)
                //function (Model) {
                //$("#PartialView").html(Model);
            //}
        });
        
    }
}//end OnDeleteClick


//---------------------------------------------------------------------------------------
///刪除後的處理
//-------------------------------------------------------------------------------------
function DeleteResult(ItemData) {
    if (ItemData != '0') {
        FadAlert("刪除成功。");
        $("tr[id=" + ItemData.EmpNoIdGet + "]").remove();
    }
    else {
        FadAlert("刪除失敗。");
    }
}// end DeleteResult


//----------------------------------------------------------------------------------------
//tr的點選項目
//----------------------------------------------------------------------------------------
function OntrClick(ItemData) {
    //$(this).find("td:eq(0)").text()  取得Id

    UnLoadView(); 
    $('#SelectEmpNo').text(ItemData.EmpNo);
    $('#SelectEmpName').text(ItemData.EmpFirstName);    
    $('#Load_Account_Form').submit();
}//end OntrClick


//----------------------------------------------------------------------------------------
//非ajax 的編輯按鈕事件
//----------------------------------------------------------------------------------------
function EditButtonClick(ItemData) {
    $("#Hidden_EmployeeJson").val(JSON.stringify(ItemData));
    $("#Load_Edit_Form").submit();
}//end EditButtonClick


//----------------------------------------------------------------------------------------
//非ajax 的刪除按鈕事件
//----------------------------------------------------------------------------------------
function DeleteButtonClick(ItemData) {
    $("#Hidden_EmployeeDeleteJson").val(JSON.stringify(ItemData));
    $("#Load_Delete_Form").submit();
}//end EditButtonClick



function OnCashClick(ItemData) {
    //UnLoadView();
    $('#SelectEmpNo').text(ItemData.EmpNo);
    $.ajax({
        type: "POST",
        url: "CashFlowPartialViewAction",
        data: {
            "EmployeeJson": JSON.stringify(ItemData)
        },
        success: function (Model) {
            $("#PartialView").html(Model);
        }
    });
}


