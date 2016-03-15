
//----------------------------------------------------------------------------
//設定時間控件
//----------------------------------------------------------------------------
function SetDatePicker()
{

    $("#BeginDatePicker").datepicker();
    $("#EndDatePicker").datepicker();
}

//----------------------------------------------------------------------------
//
//----------------------------------------------------------------------------
function OnSubmit() {
    $("#Load_Form").submit();
}


//----------------------------------------------------------------------------
//選擇上個月
//----------------------------------------------------------------------------
function LastMonth()
{    
    var d = new Date();
    var month = d.getMonth();
    var year = d.getFullYear();

    var new_year = year;  //取當前的年份   
    var new_month = month;
    if (month > 12)      //如果當前大於12月，則年份轉到下一年   
    {
        new_month -= 12;    //月份減   
        new_year++;      //年份增   
    }
    var new_date = new Date(new_year, new_month, 1);        //取當年當月中的第一天  
    var LastDay=new Date(new_date.getTime()-1000*60*60*24).getDate();

    $("#BeginDatePicker").val(month + "/" + "1/" + year);

    $("#EndDatePicker").val(month+"/"+LastDay+"/"+year);

}//end  LastMonth


//----------------------------------------------------------------------------
//選擇上週
//----------------------------------------------------------------------------
function LastWeek()
{
    var myDate = new Date();
    var myDay = myDate.getDay();
    if (myDay == 0) {
        myDay = 7;
    }
    var myStartDate = new Date();
    // 將日期變成目前禮拜的星期一
    myStartDate.setDate(myStartDate.getDate() + (0 - (myDay - 1)));

    var myEndDate = new Date();
    myEndDate.setDate(myEndDate.getDate() + (7 - myDay));    
    var EndLastWeek = myEndDate.getd - 7;
    document.getElementById('BeginDatePicker').value = GetFormatDate((myStartDate.getMonth() + 1)) + '/' + GetFormatDate(myStartDate.getDate() - 7) + '/' + myStartDate.getFullYear();
    document.getElementById('EndDatePicker').value = GetFormatDate((myEndDate.getMonth() + 1)) + '/' + GetFormatDate(myEndDate.getDate() - 7) + '/' + myEndDate.getFullYear();
}


function GetFormatDate(InputValue) {
    if (InputValue < 10) {
        InputValue = '0' + InputValue;
    }

    return InputValue;
}


//----------------------------------------------------------------------------
//今日
//----------------------------------------------------------------------------
function GetToDay() {
    var d = new Date();
    var day = d.getDate();
    var month = d.getMonth() + 1;
    var year = d.getFullYear();

    $("#BeginDatePicker").val(month + "/" + day + "/" + year);
    $("#EndDatePicker").val(month + "/" + day + "/" + year);

}


//----------------------------------------------------------------------------
//查詢報表
//----------------------------------------------------------------------------
function Search() {
    var arryName = ['美元', '日元', '人民幣', '新台幣', '港元', '歐元', '泰銖', '韓元', '英鎊', '新加坡元'];
    var arrary = [];
    $('.btn-group3 .btn').each(function () {
        if ($(this).hasClass('btn btn-default active'))
        {
            arrary.push($(this).attr('id'));
        }

    });
    
    if (!$("#BeginDatePicker").val()) {
        FadAlert("開始日期不可為空");
    }
    else {
        if (!$("#EndDatePicker").val()) {
            FadAlert("結束日期不可為空");
        }
        else {

            if ($("#BeginDatePicker").val() > $("#EndDatePicker").val()) {

                FadAlert("開始日期不可大於結束日期");
            }
            else {
                var Data = {
                    "BeginDate": $("#BeginDatePicker").val(),
                    "EndDate": $("#EndDatePicker").val(),
                    "MainCurrency": $("#Radio_Select").val(),
                    "SubCurrency": arrary
                }

                $.ajax({
                    type: "POST",
                    url: "Chart/GenerateData",
                    data: Data,
                    success: function (Model) {
                        Chart.xAxis[0].setCategories(Model.Categories);

                        for (var i = Chart.series.length-1; i >=0 ; i--)
                            Chart.series[i].remove();

                        for(var i=0;i<Model.Exchange.length;i++)
                        {
                            Chart.addSeries({
                                name:arryName[ Model.Naemes[i]],
                                data: Model.Exchange[i]
                            });
                        }
                        
                    }
                });
            }
        }
    }
}