
//---------------------------------------------------------------------------
//Table 列的HighLight---單選取背景色
//--------------------------------------------------------------------------
function TableHighLightAndRowSelected(TableName) {
    $("#" + TableName + " tr").not(':first').hover(
                  function () {
                      if ($(this).attr('id') != null) {
                          $(this).css("background", "#add8e6");
                      }
                  },
                  function () {
                      $(this).css("background", "");
                  }
             );

    $("#" + TableName + " tr").not(':first').click(function () {          
        $("#" + TableName + " tr").not(this).removeClass('row-selected');
            $(this).css("background", "");
            $(this).toggleClass("row-selected");
        }
     );
}//end TableHighLightAndRowSelected


//---------------------------------------------------------------------------
//Table 列的HighLight---單選取背景色
//--------------------------------------------------------------------------
function TableHighLight(TableName)
{
    $("#" + TableName + " tr").not(':first').hover(
               function () {
                   if ($(this).attr('id') != null) {
                       $(this).css("background", "#add8e6");
                   }
               },
               function () {
                   $(this).css("background", "");
               }
          );
}//end TableHighLight


//---------------------------------------------------------------------------
//影藏顯示切換
//--------------------------------------------------------------------------
function PowerSwitch(Show,Hid)
{
    $("#" + Show).show();
    $("#" + Hid).hide();
}/// end PowerSwitch



//--------------------------------------------------------------
//驗證只能輸入數字
//-------------------------------------------------------------
function ValidateNumber(e, pnumber) {
    if (!/^\d+$/.test(pnumber)) {
        $(e).val(/^\d+/.exec($(e).val()));
    }
    return false;
}// end ValidateNumber