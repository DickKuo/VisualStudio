$(function () {
    var Url = "Setting/";    

    if ($("#hideMinimunLotLimit").val() == 999999) {       
        $("#SettingLot").hide();
    }
    else {
        $("#SettingLot").show();
    }

    $("input[name='group2']").on("change", function () {
        if ($(this).attr('id') == 1) {
            $("#SettingLot").show();
        }
        else {
            $("#SettingLot").hide();
            $.ajax({
                type: "POST",
                url: Url + "ChangeMiniLot",
                data: {
                    "MinimunLotLimit": 999999
                },
                success: function (Model) {                   
                    if (Model == "OK") {
                        alert("變更完成");
                    }
                    else {
                        alert("變更失敗");
                    }
                }
            });
        }
    });

    $("#ChangeLot").on("click", function () {
        $.ajax({
            type: "POST",
            url: Url + "ChangeMiniLot",
            data: {
                "MinimunLotLimit": $("#InputLot").val()
            },
            success: function (Model) {               
                if (Model == "OK") {
                    alert("變更完成");
                }
                else {
                    alert("變更失敗");
                }
            }
        });        
    });

});