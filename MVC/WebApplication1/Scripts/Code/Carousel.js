



//-----------------------------------------------------------------------------------
//刪除事件
//-------------------------------------------------------------------------------------
function OnPhotoDeleteClick(ItemData) {
    if (confirm("確定要刪除嗎?")) {
        $.ajax({
            type: "POST",
            url: "Carousel/DeletePhotoAction",
            data: {
                "PhotoJson": JSON.stringify(ItemData)
            },
            success:
                function (Model) {
                    DeleteDepResult(Model);
                }
        });

    }
}//end OnDeleteClick


//-----------------------------------------------------------------------------------
//刪除完成挑整畫面
//-------------------------------------------------------------------------------------
function DeleteDepResult(ItemData) {
    if (ItemData != null) {
        $("tr[id=Photo_" + ItemData.ImageNo + "]").remove();
    }
    else {
        alert("刪除失敗。");
    }
}


//-----------------------------------------------------------------------------------
//載入上傳畫面
//-------------------------------------------------------------------------------------
function LoadingUpLoadPage()
{
    $.ajax({
        type: "POST",
        //url: "Carousel/LoadingUploadPage",
        url: "FileLoad/Index",
        success:
            function (Model) {
                $("#Image_List").html(Model);
            }
    });
}

//var IsCheckImageeType = true;  //是否檢查圖片副檔名 
//var IsCheckImageWidth = true;   //是否檢查圖片寬度 
//var IsCheckImageHeight = true; //是否檢查圖片高度
//var IsCheckImageSize = true;   //是否檢查圖片檔案大小 

//var ImageSizeLimit = 10000;   //上傳上限，單位:byte 
//var ImageWidthLimit = 1200;    //圖片寬度上限
//var ImageHeightLimit = 1000;   //圖片高度上限


//function checkImage() {
//    if (IsCheckImageWidth && this.width > ImageWidthLimit) {
//        showMessage('寬度', 'px', this.width, ImageWidthLimit);
//    } else if (IsCheckImageHeight && this.height > ImageHeightLimit) {
//        showMessage('高度', 'px', this.height, ImageHeightLimit);
//    } else if (IsCheckImageSize && this.fileSize > ImageSizeLimit) {
//        showMessage('檔案大小', 'kb', this.fileSize / 1000, ImageSizeLimit / 1000);
//    } else {
//        $("form[name='myForm']").submit();
//    }
//}


//function showMessage(kind, unit, real, limit) {
//    var msg = "您所選擇的圖片kind為 real unit\n超過了上傳上限 limit unit\n不允許上傳！"
//    alert(msg.replace(/kind/, kind).replace(/unit/g, unit).replace(/real/, real).replace(/limit/, limit));
//}


//function UpLoadPhoto() {
//    $("input[name='act']").attr("value", "upload");
//    checkImage();
//    //$("form[name='myForm']").submit();
//}


//function postForm() {
//    $("input[name='act']").attr("value", "post");
//}

