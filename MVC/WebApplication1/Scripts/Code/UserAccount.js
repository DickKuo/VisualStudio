


function AddBtnHide()
{
    $("#Btn_EditView").show();
    $("#Btn_AddView").hide();
}


function AddBtnShow() {
    $("#Btn_EditView").hide();
    $("#Btn_AddView").show();
}


//---------------------------------------------------
//載入畫面---(新增)
//---------------------------------------------------
function OnAddClick()
{
    $("#Div_Panel").addClass("col-md-6");
    $.ajax({
        type: "POST",
        url: "UserAccount/LoadingEditPartialView",
        success: function (Model) {
            $("#PartialView").html(Model);
            AddBtnShow();
            
        }
    });
}


//---------------------------------------------------
//新增動作
//---------------------------------------------------
function OnAddAction()
{
    var Data = {
        UserName: $("#User_Name").val(),
        PassWord: $("#User_PassWord").val(),
        Email: $("#User_Email").val(),
        RoleID: $("#User_Role").val()
    };

    $.ajax({
        type: "POST",
        url: "UserAccount/AddAction",
        data:{
        "User":Data
        },
        success: function (Model) {
            $("#PartialView").html(null);
            var d = new Date();
            var month = d.getMonth() + 1;
            var Day = d.getDate();
            if (month < 10) {
                month = '0' + month
            }
            if (Day < 10) {
                Day = '0' + Day
            }

            $("#BrowseTable tr:last").after("<tr id='" + Model .UserID+ "' title='點擊欄位進行編輯'><td>" + Data.UserName + "</td>" +
            "<td>" + Data.Email + "</td>" + " <td>" + d.getFullYear().toString() +"/"+ month.toString() +"/"+ Day.toString() + "</td> " +
            "<td><button type='button' name='edit' class='btn btn-primary' onclick='EditLoad(@JsonConvert.SerializeObject(" + JSON.stringify(Data) + "))' >編輯</button></td>" +
            "<td><button type='button' name='delete' class='btn btn-danger' onclick='OnDeleteClick(@JsonConvert.SerializeObject(" + JSON.stringify(Data) + "))' >刪除</button> </td>" +
            " </tr>");

            alert('新增完成。');
        }
    });
}



//---------------------------------------------------
//取消動作
//---------------------------------------------------
function OnCancelAction()
{
    $("#PartialView").html(null);
    $("#Div_Panel").removeClass("col-md-6");
}


//---------------------------------------------------
//載入畫面---(編輯)
//---------------------------------------------------
function EditLoad(ItemData)
{
    $("#Div_Panel").addClass("col-md-6");

    $.ajax({
        type: "POST",
        url: "UserAccount/LoadingEditPartialView",
        data: {
            "User":ItemData,
            "JsonString":JSON.stringify(ItemData)
        },
        success: function (Model) {
            $("#PartialView").html(Model);
            AddBtnHide();
            $("#User_Name").val(ItemData.UserName);
            $("#User_PassWord").val(ItemData.PassWord);
            $("#User_Email").val(ItemData.Email);
            $("#User_Role").val(ItemData.RoleID);
            $("#Hr_Title").text('編輯');
            $("#User_Name").prop('readonly', true);

            $("#User_UserID").val(ItemData.UserID);
        }
    });
}



//---------------------------------------------------
//編輯動作
//---------------------------------------------------
function OnUserEditClick() {
    var Data = {
        UserName: $("#User_Name").val(),
        PassWord: $("#User_PassWord").val(),
        Email: $("#User_Email").val(),
        RoleID: $("#User_Role").val(),
        UserID: $("#User_UserID").val()
    };
    $.ajax({
        type: "POST",
        url: "UserAccount/EditAction",
        data: {
            "User": Data
        },
        success: function (Model) {
            alert('修改完成。');
            $("#PartialView").html(null);
            $("tr[id=" + Model.UserID + "] ").find("td:eq(1)").text(Model.Email);            
            $("#Div_Panel").removeClass("col-md-6");
        }
    });
}


//--------------------------------------------------------------
//刪除功能選單
//-------------------------------------------------------------
function OnDeleteClick(ItemData) {
    if (confirm("確定要刪除嗎?")) {
         $.ajax({
            type: "POST",
            url: "UserAccount/DeleteMothd",
            data: {
                "JsonString": JSON.stringify(ItemData)
            },
            success: function (Model) {
                $("#PartialView").html(null);
                if (Model != null) {
                    $("tr[id=" + ItemData.UserID + "] ").remove();
                }
                else {
                    alert("刪除失敗。");
                }
            }
        });
    }
}//end OnDeleteMenuClick