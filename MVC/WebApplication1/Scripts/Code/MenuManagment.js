

//--------------------------------------------------------------
//Tr --Click
//-------------------------------------------------------------
function EachTrClick() {
    $('#BrowseTable tr').click(function (event) {
        var Data = {
            MenuNo: jQuery.trim($(this).find("td:eq(0)").text()),
            Name: jQuery.trim($(this).find("td:eq(1)").text()),
            Url: jQuery.trim($(this).find("td:eq(2)").text()),
            ParentNo: jQuery.trim($(this).find("td:eq(3)").text()),
            IsEnable: jQuery.trim($(this).find("td:eq(4)").attr("id")),
        };
        if ($(this).attr('id') != null) {
            $("#ParentNo").val(Data.MenuNo);
            OnEditMenuClick(Data);
        }
    });
}







//--------------------------------------------------------------
//新增功能選單---載入新增畫面
//-------------------------------------------------------------
function OnAddMenuclick()
{
    $.ajax({
        type: "POST",
        url: "EditPartialView",
        success: function (Model) {
            $("#MeunPartial").html(Model);
            $("#Hr_Title").text("新增");
            $("#Btn_Edit").hide();
            $("#Btn_Add").show();
            $("#Lab_No").hide();
            $("#Menu_No").hide();
            $("#Btn_Delete").hide();
           

            $("#ChildPartial").html(null);
        }
    });

}//end OnAddMenuclick


//--------------------------------------------------------------
//新增功能選單---載入Child新增畫面
//-------------------------------------------------------------
function OnChildAddMenuclick() {
    $.ajax({
        type: "POST",
        url: "ChildEditPartialView",
        success: function (Model) {
            $("#Div_ChildPartial").html(Model);
            $("#Child_Hr_Title").text("新增");
            $("#Btn_Child_Edit").hide();
            $("#Btn_Child_Add").show();
            $("#Child_Lab_No").hide();
            $("#Child_Menu_No").hide();
            $("#Btn_Child_Delete").hide();
        }
    });
}//end OnChildAddMenuclick


//--------------------------------------------------------------
//新增功能選單---載入Child編輯畫面
//-------------------------------------------------------------
function OnChildEditMenuclick(ItemData) {
    $.ajax({
        type: "POST",
        url: "ChildEditPartialView",
        success: function (Model) {
            $("#Div_ChildPartial").html(Model);
            $("#Child_Hr_Title").text("編輯");
            $("#Btn_Child_Edit").show();
            $("#Btn_Child_Add").hide();
            $("#Child_Lab_No").hide();
            $("#Child_Menu_No").hide();
            $("#Btn_Child_Delete").hide();

            $("#Child_Menu_No").val(ItemData.MenuNo);
            $("#Child_Menu_Name").val(ItemData.Name);
            $("#Child_Menu_Url").val(ItemData.Url);
        }
    });
}//end OnChildEditMenuclick


//--------------------------------------------------------------
//編輯---傳資料給後端
//-------------------------------------------------------------
function OnChildMenuEditClick() {
    var Menu = {
        "MenuNo": $("#Child_Menu_No").val(),
        "Url": $("#Child_Menu_Url").val(),
        "Name": $("#Child_Menu_Name").val(),
        "ParentNo": $("#ParentNo").val(),
        "IsEnable": true,
        "MenuOrder": 1,
        "MenuList": null
    };
    $.ajax({
        type: "POST",
        url: "EditMothd",
        data: {
            "JsonString": JSON.stringify(Menu)
        },
        success: function (Model) {
            //$("#MeunPartial").html(Model);
            $("tr[id=ChildMenu_" + Menu.MenuNo + "] ").find("td:eq(1)").text(Menu.Name);
            $("tr[id=ChildMenu_" + Menu.MenuNo + "] ").find("td:eq(2)").text(Menu.Url);
            //$("tr[id=ChildMenu_" + Menu.MenuNo + "] ").find("td:eq(3)").text(Menu.ParentNo);
            //$("tr[id=ChildMenu_" + Menu.MenuNo + "] ").find("td:eq(4) input").attr("checked", Menu.IsEnable);
            //$("#Menu_No").val(Menu.MenuNo);
            //$("#Menu_Name").val(Menu.Name);
            //$("#Menu_Url").val(Menu.Url);
            //$("#Menu_ParentNo").val(Menu.ParentNo);
            //$("#Menu_IsEnable").attr("checked", Menu.IsEnable);
            $("#Div_ChildPartial").html(null);
            $("#Btn_Add").hide();
        }
    });
}//end OnMenuEditClick





//--------------------------------------------------------------
//新增功能---執行新增功能
//-------------------------------------------------------------
function OnAddAction()
{       
    var Menu = {
        "MenuNo": GetNewNo(),
        "Url": $("#Menu_Url").val(),
        "Name": $("#Menu_Name").val(),
        "ParentNo": "",
        "IsEnable": true,
        "MenuOrder": 1,
        "MenuList": null
    };
    $.ajax({
        type: "POST",
        url: "AddMethod",
        data: {
            "JsonString": JSON.stringify(Menu)
        },
        success: function (Model) {
            $("#MeunPartial").html(Model);
            $("#Btn_Add").hide();   
            $("#BrowseTable tr:last").after("<tr Id=Menu_" + Menu.MenuNo + " title='點擊欄位進行編輯' ><td>" + Menu.MenuNo + "</td>" +
           "<td>" + Menu.Name + "</td>" + "<td>" + Menu.Url + "</td>" + 
            "</tr>");
            BrowseTableInit();
            alert('新增完成。');
        }
    });
}//end OnAddAction


function OnChildAddAction()
{
    var Menu = {
        "MenuNo": GetNewNo(),
        "Url": $("#Child_Menu_Url").val(),
        "Name": $("#Child_Menu_Name").val(),
        "ParentNo": $("#ParentNo").val(),
        "IsEnable": true,
        "MenuOrder": 1,
        "MenuList": null
    };
    $.ajax({
        type: "POST",
        url: "AddMethod",
        data: {
            "JsonString": JSON.stringify(Menu)
        },
        success: function (Model) {
            $("#MeunPartial").html(Model);
            $("#Btn_Add").hide();
            $("#ChildTable tr:last").after("<tr Id=ChildMenu_" + Menu.MenuNo + " title='點擊欄位進行編輯'><td>" + Menu.MenuNo + "</td>" +
           "<td>" + Menu.Name + "</td>" + "<td>" + Menu.Url + "</td>" +
           "    <td> <button type='button' name='ChildEdit' class='btn btn-primary' onclick='OnChildEditMenuclick(" + JSON.stringify(Menu) + ")'>編輯</button>" +
           "</td>"+
           "<td>"+
           "    <button type='button' name='ChildDelete' class='btn btn-danger' onclick='OnChildDeleteMenuClick(" + JSON.stringify(Menu) + ")'>刪除</button>" +
           "</td>"+

            "</tr>");
            $("#Div_ChildPartial").html(null);
            BrowseTableInit();
        }
    });
}


//--------------------------------------------------------------
//刪除功能選單---Child
//-------------------------------------------------------------
function OnChildDeleteMenuClick(ItemData) {
    if (confirm("確定要刪除嗎?")) {
              $.ajax({
            type: "POST",
            url: "DeleteMothd",
            data: {
                "JsonString": JSON.stringify(ItemData)
            },
            success: function (Model) {
                //$("#MeunPartial").html(null);
                if (Model != null) {
                    $("tr[id=ChildMenu_" + ItemData.MenuNo + "] ").remove();                    
                }
                else {
                    alert("刪除失敗。");
                }
            }
        });
    }
}//end OnChildDeleteMenuClick





//--------------------------------------------------------------
//建立編號
//-------------------------------------------------------------
function GetNewNo()
{
    var d = new Date();
    var month = d.getMonth() + 1;
    var Day = d.getDate();
    if (month < 10) {
        month = '0' + month
    }
    if (Day < 10) {
        Day = '0' + Day
    }
    var ResulNo = d.getFullYear().toString() + month.toString() + Day.toString() + d.getHours().toString() + d.getMinutes().toString() + d.getSeconds().toString();
    return ResulNo;
}//end GetNewNo


//--------------------------------------------------------------
//編輯功能選單--預設帶值
//-------------------------------------------------------------
function OnEditMenuClick(ItemData)
{
    $.ajax({
        type: "POST",
        url: "EditPartialView",
        data:ItemData,
        success: function (Model) {
            $("#MeunPartial").html(Model);
            $("#Menu_Name").val(ItemData.Name);
            $("#Menu_Url").val(ItemData.Url);
            $("#Menu_ParentNo").val(ItemData.ParentNo);
            $("#Menu_No").val(ItemData.MenuNo);            
            $("#Menu_IsEnable").attr("checked", $("tr[id=Menu_" + ItemData.MenuNo + "] ").find("td:eq(4) input").is(":checked"));

            $("#Btn_Add").hide();
        }
    });    
    $.ajax({
        type: "POST",
        url: "ChildMenuList",
        data: { "MainMenu": ItemData },
        success: function (Model) {
            $("#ChildPartial").html(Model);
        }
    });

}//end OnEditMenuClick


//--------------------------------------------------------------
//編輯---傳資料給後端
//-------------------------------------------------------------
function OnMenuEditClick()
{
   var Menu = {
        "MenuNo": $("#Menu_No").val(),
        "Url": $("#Menu_Url").val(),
        "Name": $("#Menu_Name").val(),
        "ParentNo": $("#Menu_ParentNo").val(),
        "IsEnable": $("#Menu_IsEnable").is(":checked"),
        "MenuOrder":1,
        "MenuList":null
    };
    $.ajax({
        type: "POST",
        url: "EditMothd",
        data:{
            "JsonString": JSON.stringify(Menu)
        },
        success: function (Model) {
            $("#MeunPartial").html(Model);
            $("tr[id=Menu_" + Menu.MenuNo + "] ").find("td:eq(1)").text(Menu.Name);
            $("tr[id=Menu_" + Menu.MenuNo + "] ").find("td:eq(2)").text(Menu.Url);
            $("tr[id=Menu_" + Menu.MenuNo + "] ").find("td:eq(3)").text(Menu.ParentNo);
            $("tr[id=Menu_" + Menu.MenuNo + "] ").find("td:eq(4) input").attr("checked", Menu.IsEnable);
            $("#Menu_No").val(Menu.MenuNo);
            $("#Menu_Name").val(Menu.Name);
            $("#Menu_Url").val(Menu.Url);
            $("#Menu_ParentNo").val(Menu.ParentNo);
            $("#Menu_IsEnable").attr("checked", Menu.IsEnable);
            $("#Btn_Add").hide();
            alert('修改完成。');
        }
    });


}//end OnMenuEditClick


//--------------------------------------------------------------
//刪除功能選單
//-------------------------------------------------------------
function OnDeleteMenuClick()
{   
    if (confirm("確定要刪除嗎?")) {
        if ($('#ChildTable tr').length > 1)
        {
            alert('有子功能，無法刪除');
        }
        else
        {
            var Menu = {
                "MenuNo": $("#Menu_No").val(),
                "Url": $("#Menu_Url").val(),
                "Name": $("#Menu_Name").val(),
                "ParentNo": $("#Menu_ParentNo").val(),
                "IsEnable": $("#Menu_IsEnable").is(":checked"),
                "MenuOrder": 1,
                "MenuList": null
            };
            $.ajax({
                type: "POST",
                url: "DeleteMothd",
                data: {
                    "JsonString": JSON.stringify(Menu)
                },
                success: function (Model) {
                    $("#MeunPartial").html(null);
                    if (Model != null) {
                        $("tr[id=Menu_" + Menu.MenuNo + "] ").remove();
                    }
                    else {
                        alert("刪除失敗。");
                    }
                }
            });
        }
    }
}//end OnDeleteMenuClick


//--------------------------------------------------------------
//功能列表初始化
//-------------------------------------------------------------
function BrowseTableInit() {
    $('#BrowseTable tr').click(function (event) {
        var Data = {
            //RoleNo: jQuery.trim($(this).find("td:eq(0)").attr('id').val())
            Name: jQuery.trim($(this).find("td:eq(0)").text()),
            RoleNo: $(this).attr('id')
            //IsEnable: jQuery.trim($(this).find("td:eq(2) input").is(":checked")),
        };
        if ($(this).attr('id') != null) {
            LoadingEditView(Data);
        }
    });
    HighLight()
}//end RoleInitSetting


//--------------------------------------------------------------
//角色列表初始化
//-------------------------------------------------------------
function RoleInitSetting()
{
    $('#RoleTable tr').click(function (event) {
        var Data = {
            //RoleNo: jQuery.trim($(this).find("td:eq(0)").attr('id').val())
            Name: jQuery.trim($(this).find("td:eq(0)").text()),
            RoleNo: $(this).attr('id')
            //IsEnable: jQuery.trim($(this).find("td:eq(2) input").is(":checked")),
        };
        if ($(this).attr('id') != null) {
            LoadingEditView(Data);
        }
    });
    HighLight()
}//end RoleInitSetting


//--------------------------------------------------------------
//列表HighLight
//-------------------------------------------------------------
function HighLight()
{   
    $("tr").not(':first').hover(
          function () {
              $(this).css("background", "#add8e6");
              $(this).tooltip();
          },
          function () {
              $(this).css("background", "");
          }
     );
}//end HighLight


//--------------------------------------------------------------
//切換頁籤---角色頁籤
//-------------------------------------------------------------
function OnRolePageClick()
{
    $.ajax({
        type: "POST",
        url: "LoadingRoles",
        success: function (Model) {
            $("#Div_Menus").html(null);
            $("#Div_Roles").html(Model);
        }
    })
}//end OnRolePageClick


//--------------------------------------------------------------
//切換頁籤---功能清單頁籤
//-------------------------------------------------------------
function OnMenuPageClick()
{
    $.ajax({
        type: "POST",
        url: "MenuList",
        success: function (Model) {
            $("#Div_Roles").html(null);
            $("#Div_Menus").html(Model);
        }
    });
}//end OnMenuPageClick


//--------------------------------------------------------------
//切換頁籤---使用者頁籤
//-------------------------------------------------------------
function OnUserPageClick() {
    //$.ajax({
    //    type: "POST",
    //    url: "MenuList",
    //    success: function (Model) {
    //        $("#Div_Roles").html(null);
    //        $("#Div_Menus").html(Model);
    //    }
    //});
    alert('施工中');
}//end OnUserPageClick


//--------------------------------------------------------------
//角色頁籤----載入編輯畫面(編輯)
//-------------------------------------------------------------
function LoadingEditView(ItemData)
{
    $.ajax({
        type: "POST",
        url: "EditRolePartialView",
        success: function (Model) {
            $("#RolePartial").html(Model);
            $("#Role_No").val(ItemData.RoleNo);
            $("#Role_Name").val(ItemData.Name);
            $("#Role_IsEnable").attr("checked", ItemData.IsEnable);
            $("#Btn_Add").hide();
        }
    });
    $("#RoleNO").val(ItemData.RoleNo);
    var Data = {
        RoleNo: ItemData.RoleNo,
        Name: ItemData.Name,
        IsEnable: ItemData.IsEnable,
        RoleID: null
    };
    $.ajax({
        type: "POST",
        url: "MenuListMatchRolesMenu",
        data:{
            "Role":Data
        },
        success: function (Model) {
            $("#RolesMenuPartial").html(Model);
        }
    });

}//end LoadingEditView


//--------------------------------------------------------------
//角色頁籤----載入編輯畫面(新增)
//-------------------------------------------------------------
function OnRoleAddClick()
{
    $.ajax({
        type: "POST",
        url: "EditRolePartialView",
        success: function (Model) {
            $("#RolePartial").html(Model);
            $("#Btn_Edit").hide();
            $("#Hr_Title").text("新增");
            $("#Btn_Delete").hide();
            $("#Lab_No").hide();
            $("#Role_No").hide();
        }
    });
    $.ajax({
        type: "POST",
        url: "MenuListMatchRolesMenu",
        success: function (Model) {
            $("#RolesMenuPartial").html(Model);
        }
    });  
}//end OnRoleAddClick


//--------------------------------------------------------------
//角色頁籤----新增角色動作
//-------------------------------------------------------------
function OnAddRoleAction()
{
    var ids = "";
    $('#BrowseTable tr td input[type="checkbox"]').each(function () {
        if ($(this).is(":checked")) {
            ids = ids + $(this).attr('id') + ",";          
        }
    });    
    var Data = {
        RoleNo: GetNewNo(),
        Name: $("#Role_Name").val(),
        //IsEnable: $("#Role_IsEnable").is(":checked"),
        RoleID:null
    };
    $.ajax({
        type: "POST",
        url: "AddRoleAction",
        data: {
            "Role": Data,
            "JsonString": ids
        },
        success: function (Model) {
            $("#RolePartial").html(Model);
            $("#Btn_Edit").hide();
            $("#Hr_Title").text("新增");
            $("#Btn_Delete").hide();           
            //var check = "";
            //if (Data.IsEnable) {
            //    check = "checked";
            //}
            $("#RoleTable tr:last").after("<tr Id=" + Data.RoleNo + " title='點擊欄位進行編輯'>" +
              "<td>" + Data.Name + "</td>" +              
              "</tr>");
            RoleInitSetting();
            alert('新增完成。');
        }
    });
} //end OnAddRoleAction


//--------------------------------------------------------------
//角色頁籤----全選
//-------------------------------------------------------------
function SelectAllCheckbox()
{
    if ($("#Cb_all").is(":checked")) {
        $('#BrowseTable tr td input[type="checkbox"]').each(function () {
            $(this).attr("checked",true)
        });
    }
    else {
        $('#BrowseTable tr td input[type="checkbox"]').each(function () {
            $(this).attr("checked", false)
        });
    }
}//end SelectAllCheckbox


//--------------------------------------------------------------
//角色頁籤----角色刪除
//-------------------------------------------------------------
function OnDeleteRoleClick()
{
    if (confirm("確定要刪除嗎?")) {
        var Data = {
            RoleNo: $("#RoleNO").val(),
            Name: $("#Role_Name").val()
            //IsEnable: $("#Role_IsEnable").is(":checked"),
            //RoleID: null
        };
        $.ajax({
            type: "POST",
            url: "DeleteAction",
            data: {
                "Role": Data
            },
            success: function (Model) {
                $("tr[id=" + Data.RoleNo + "]").remove();
                $("#RolePartial").html(null);
            }
        });
    }
}


//--------------------------------------------------------------
//角色頁籤----角色編輯
//-------------------------------------------------------------
function OnEditRoleAction()
{
    var ids = "";
    $('#BrowseTable tr td input[type="checkbox"]').each(function () {
        if ($(this).is(":checked")) {
            ids = ids + $(this).attr('id') + ",";
        }
    });
    var Data = {
        RoleNo: $("#RoleNO").val(),
        Name: $("#Role_Name").val(),
        IsEnable: $("#Role_IsEnable").is(":checked"),
        RoleID: null
    };
    $.ajax({
        type: "POST",
        url: "EditRoleAction",
        data: {
            "Role": Data,
            "JsonString": ids
        },
        success: function (Model) {
            //$("#RolePartial").html(Model);
            //$("#Btn_Edit").hide();
            //$("#Hr_Title").text("新增");
            //$("#Btn_Delete").hide();
            //var check = "";
            //if (Data.IsEnable) {
            //    check = "checked";
            //}
            //$("#RoleTable tr:last").after("<tr Id=" + Data.RoleNo + " title='點擊欄位進行編輯'>" +
            //  "<td>" + Data.Name + "</td>" +
            //  "</tr>");
            //RoleInitSetting()
            alert('修改完成。');
        }
    });

}