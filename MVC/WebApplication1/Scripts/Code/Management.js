//-----------------------------------------------------------------------------
//員工顯示瀏覽畫面
//----------------------------------------------------------------------------
function EmpListShow() {
    $("#Partial_Emp").html(null);
    $("#div_Emp_Partial").show();
}


//-----------------------------------------------------------------------------
//飛出特效Alert
//----------------------------------------------------------------------------
function FadAlert(Message)
{
    $("#Alert_Body").html(Message);
    $("#bt_diloag").click();
}


function FadComfirm(Message,Data)
{
    $("#Comfirm_Body").html(Message);
    $("#Delete_Data").html(Data);
    $("#Delete_Data").hide();
    $("#bt_comfirm").click();
}




//-----------------------------------------------------------------------------
//員工顯示編輯畫面
//----------------------------------------------------------------------------
function EmpPartialShow(Model) {
    $("#Partial_Emp").html(Model);
    $("#div_Emp_Partial").hide();
}


//----------------------------------------------------------------------------
//載入員工管理畫面
//----------------------------------------------------------------------------
function OnEmployeeClick() {
    $.ajax({
        type: "POST",
        url: "Management/LoadingEmployee",
        success: function (Model) {
            $("#Div_Department").hide();
            $("#Div_Postition").hide();
            $("#Div_Employee").show();
            $("#Div_Employee").html(Model);
        }
    });
}


//----------------------------------------------------------------------------
//載入部門管理畫面
//----------------------------------------------------------------------------
function OnDepartmentClick() {
    $.ajax({
        type: "POST",
        url: "Management/LoadingDepartment",
        success: function (Model) {
            $("#Div_Employee").hide();
            $("#Div_Postition").hide();
            $("#Div_Department").show();
            $("#Div_Department").html(Model);
        }
    });
}





//-----------------------------------------------------------------------------
//載入員工新增畫面
//----------------------------------------------------------------------------
function OnEmpAddClick() {
    
    $.ajax({
        type: "POST",
        url: "Management/AddEmpView",
        success: function (Model) {
            EmpPartialShow(Model);
        }
    });
}


//-----------------------------------------------------------------------------
//載入部門新增畫面
//----------------------------------------------------------------------------
function OnDepAddClick() {  
    $.ajax({
        type: "POST",
        url: "Management/AddDepartmentView",
        success: function (Model) {
            $("#Partial_Dep").html(Model);
            $("#Partail_DepsEmp").html(null);
        }
    });
}//end OnDepAddClick


//-----------------------------------------------------------------------------
//取消部門新增畫面
//----------------------------------------------------------------------------
function OnAddDepCancel() {
    $("#Partial_Dep").html(null);
    $("#Div_DepPartial").show();

}//end OnAddDepCancel


//------------------------------------------------------------------------------
//新增部門結束後的處理
//-----------------------------------------------------------------------------
function OnAddResult(ItemData) {
    if (ItemData != null) {
        FadAlert('新增'+ItemData.Name+'成功。');
        $("#Div_DepPartial").show();
        $("#DepTable tr:last").after("<tr Id=Dep_" + ItemData.DepNo + "><td>" + ItemData.DepNo + "</td>" +
            "<td>" + ItemData.Name + "</td>" + 
            //" <td> <input type='submit' value='編輯' onclick='OnDepEditClick(" + JSON.stringify(ItemData) + ")'  /> " +
            " </td> <td>  <input type='submit' name='delete' class='btn btn-danger' value='刪除' onclick='OnDepDeleteClick(" + JSON.stringify(ItemData) + ")' /> </td> </tr>");
        $("#Partial_Dep").html(null);
    }
    else {
        FadAlert("新增失敗。");
    }
}// end OnAddResult


//------------------------------------------------------------------------------
//返回部門
//-----------------------------------------------------------------------------
function OnAddEmpCancel()
{
    var ItemData = {
        DepNo: $("#EditForm_DepNo"),
        DepName: $("#EditForm_DepName")
    };


    $("#Partail_DepsEmp").html(null);

 

}


//------------------------------------------------------------------------------
//編輯畫面的處理
//-----------------------------------------------------------------------------
function OnDepEditClick(ItemData) {
    $.ajax({
        type: "POST",
        url: "Management/EditDepartmentView",
        success: function (Model) {
            $("#Partial_Dep").html(Model);
            $("#EditForm_DepNo").val(ItemData.DepNo);
            $("#EditForm_DepName").val(ItemData.Name);  
            $("#Query_Panel").show();
            $.ajax({
                type: "POST",
                url: "Management/GetDepartmentEmnpsAction",
                data: { 
                   "Dep": ItemData},
                success: function (Model) {
                    $("#Partail_DepsEmp").html(Model);
                    $("#Partail_DepsEmp").addClass("col-md-8");
                }
            });
        }
    });
}//end OnDepEditClick


//------------------------------------------------------------------------------
//編輯部門結束後的處理
//-----------------------------------------------------------------------------
function OnEditResult(ItemData) {
    if (ItemData != null) {
        $("#Div_DepPartial").show();
        $("tr[id=Dep_" + ItemData.DepNo + "] ").find("td:eq(1)").text(ItemData.Name);
        $("tr[id=Dep_" + ItemData.DepNo + "] ").find("td:eq(2)").text(ItemData.EmpCount);
        FadAlert('編輯完成。');
    }
    else {
        FadAlert("編輯失敗。");
    }
}//end OnEditResult


//-----------------------------------------------------------------------------------
//刪除事件--------------加入防止避免部門有員工的狀態下刪除部門
//-------------------------------------------------------------------------------------
function OnDepDeleteClick(ItemData) {    
    FadComfirm("確定要刪除 '" + ItemData.Name + "' 嗎??", JSON.stringify(ItemData));    
}//end OnDeleteClick



//-----------------------------------------------------------------------------------
//部門刪除-----確認刪除
//-------------------------------------------------------------------------------------
function ToDelete()
{        
    if ($('#EmpTable tr').length == 1) {
        $.ajax({
            type: "POST",
            url: "Management/DeleteAction",
            data: {
                "QueryString": $("#Delete_Data").html()
            },
            success:
                function (Model) {
                    DeleteDepResult(Model);
                    FadAlert('部門刪除完成。');
                }
        });
    }
    else {
        FadAlert('此部門已有員工，無法刪除。');
    }
}


//-----------------------------------------------------------------------------------
//刪除完成挑整畫面
//-------------------------------------------------------------------------------------
function DeleteDepResult(ItemData) {
    if (ItemData != null) {
        $("tr[id=Dep_" + ItemData.DepNo + "]").remove();
    }
    else {
        FadAlert("刪除失敗。");
    }
}//end DeleteDepResult


//-----------------------------------------------------------------------------------
//新增員工調整畫面
//-------------------------------------------------------------------------------------
function OnAddEmployeeResult(ItemData) {
    if (ItemData != null) {
        FadAlert("新增成功");
        $("#EmpTable tr:last").after("<tr Id=Emp_" + ItemData.EmpNo + "><td>" + ItemData.EmpNo + "</td>" +
            "<td>" + ItemData.EmpFirstName + "</td>" + "<td>" + ItemData.EmpLastName + "</td>" + " <td>" + ItemData.Tel + "</td> " +
             " <td>" + ItemData.Addr + "</td> " + " <td>" + ItemData.EmpType + "</td> " +
            " <td> <input type='submit' value='編輯' onclick='OnDepEditClick(" + JSON.stringify(ItemData) + ")'  /> " +
            " </td> <td>  <input type='submit' name='delete' value='刪除' onclick='OnDeleteClick(" + JSON.stringify(ItemData) + ")' /> </td> </tr>");
        EmpListShow();
    }
    else {
        FadAlert("新增失敗。");
    }
}//end OnAddEmployeeResult


//-----------------------------------------------------------------------------------
//Emp刪除事件
//-------------------------------------------------------------------------------------
function OnEmpDeleteClick(ItemData) {
    if (confirm("確定要刪除嗎?")) {
        $.ajax({
            type: "POST",
            url: "Management/DeleteEmpAction",
            data: {
                "EmployeeJson": ItemData.EmpNo,
                "EmployeeType": jQuery.trim($("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(4)").text())
            },
            success:
                function (Model) {
                    if (ItemData != null) {
                        $("tr[id=Emp_" + ItemData.EmpNo + "]").remove();
                    }
                    else {
                        FadAlert("刪除失敗。");
                    }
                }
        });
    }
}//end OnDeleteClick



//------------------------------------------------------------------------------
//員工編輯畫面的處理
//-----------------------------------------------------------------------------
function OnEmpEditClick(ItemData) {
    $.ajax({
        type: "POST",
        url: "Management/EditEmployeeView",
        success: function (Model) {
            EmpPartialShow(Model);
            $("#EditForm_EmpNo").val(ItemData.EmpNo);
            $("#EditForm_FirstName").val(ItemData.EmpFirstName);
            $("#EditForm_EmpLastName").val(ItemData.EmpLastName);
            $("#EditForm_Addr").val(ItemData.Addr);
            $("#EditForm_Tel").val(ItemData.Tel);
            $("input[type='radio']").prop({
                disabled: true
            });
            var Type = jQuery.trim($("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(4)").text());
            if (Type == "全職") {
                $("#0").prop({
                    checked: true
                });
                OnRadio_FullTime();
            }
            if (Type == "臨時工") {
                $("#1").prop({
                    checked: true
                });
                OnRadio_PTime();
            }
            if (Type == "駐點") {
                $("#2").prop({
                    checked: true
                });
                OnRadio_Stagnation();
            }
        }
    });
}//end OnEmpEditClick


//------------------------------------------------------------------------------
//編輯部門結束後的處理
//-----------------------------------------------------------------------------
function OnEditEmpResult(ItemData) {
    if (ItemData != null) {
        if (!IsCancel) {
            $("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(1)").text(ItemData.EmpFirstName);
            $("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(2)").text(ItemData.EmpLastName);
            $("tr[id=Emp_" + ItemData.EmpNo + "] ").find("td:eq(3)").text(ItemData.Tel);
            EmpListShow();
        }
    }
    else {
        FadAlert("編輯失敗。");
    }
}//end OnEditEmpResult


//------------------------------------------------------------------------------
//取消員工新增
//-----------------------------------------------------------------------------
function OnAddCancel() {
    EmpListShow();
}//end OnAddCancel


//------------------------------------------------------------------------------
//部門查詢員工
//-----------------------------------------------------------------------------
function OnDepQueryClick() {
    $.ajax({
        type: "POST",
        url: "Management/GetDepartmentEmnpsAction",
        success: function (Model) {
            $("#Partial_Dep").show();
            $("#Partial_Dep").html(Model);
        }
    });
}//end OnDepQueryClick


//------------------------------------------------------------------------------
//查詢面板 --查詢按鈕
//-----------------------------------------------------------------------------
function Search_Emp() {
    var Data = { DepNo: $("#EditForm_DepNo").val() };
    $.ajax({
        type: "POST",
        url: "Management/GetDepartmentEmnpsAction",
        data: {
            "Dep": Data,
            "QueryString": $("#Input_QueryString").val()
        },
        success: function (Model) {
            $("#Partail_DepsEmp").html(Model);
        }
    });
}//end Search_Emp


//------------------------------------------------------------------------------
//新增員工---
//-----------------------------------------------------------------------------
function OnEmpAdd(Type) {
    var Message = "";
    $(".noNull").each(function () {
        var name = $(this).attr("name");
        if ($(this).val() == "") {
            Message = Message + $(this).attr('notNull') + "不能為空  ";
        }
    })
    var Url = "";
    if (Type == "Edit") {
        Url = "Management/EditEmpAction";
    }
    if (Type == "Add") {
        Url = "Management/AddEmpAction";
    }
    if (Message == "") {
        var ItemData = {
            "DepNo": $("#EditForm_DepNo").val(),
            "EmpNo": $("#EditForm_EmpNo").val(),
            "EmpFirstName": $("#EditForm_FirstName").val(),
            "EmpLastName": $("#EditForm_EmpLastName").val(),
            "Tel": $("#EditForm_Tel").val(),
            "Addr": $("#EditForm_Addr").val()
        };
        $.ajax({
            type: "POST",
            url: Url,
            data: {
                "EmployeeJson": JSON.stringify(ItemData),
                "EmployeeType": $("input[name='group2']:checked").attr('id'),
                "Extend": $("#Input_Extend").val()
            },
            success: function (Model) {
                Search_Emp();
                FadAlert("操作完成。");
            }
        });
    }
    else {
        FadAlert(Message);
    }
}//end OnEmpAdd



//------------------------------------------------------------------------------
//部門新增所選擇員工 
//-----------------------------------------------------------------------------
function OnEmpSelect_OK()
{
    var ids = "";
    $('#EmpTable tr td input[type="checkbox"]').each(function () {
        if ($(this).is(":checked")) {
            ids = ids + $(this).attr('id') + ",";
        }
    });

    var Data = {
        DepNo: $("#EditForm_DepNo").val()
    };

    $.ajax({
        type: "POST",
        url: "Management/AddEmployeeByDepNo",
        data: {
            "Dep": Data,
            "QueryString": ids
        },
        success: function (Model) {
            $("#Partail_DepsEmp").html(null);
            FadAlert("操作完成。");
        }
    });
}


//------------------------------------------------------------------------------
//移除部門員工
//-----------------------------------------------------------------------------
function OnDepDeleteEmpClick()
{   
    var ids = "";
    $('#EmpTable tr td input[type="checkbox"]').each(function () {
        if ($(this).is(":checked")) {
            ids = ids + $(this).attr('id') + ",";
        }
    });
    var Data = {
        DepNo: $("#EditForm_DepNo").val()
    };
    
    if (ids != "") {
        $.ajax({
            type: "POST",
            url: "Management/DeleteDepartmentEmp",
            data: {
                "Dep": Data,
                "QueryString": ids
            }
        });
        $("#Partail_DepsEmp").html(null);
        FadAlert("操作完成。");
    }
    else {
        FadAlert('請選擇員工。');
    }
}


