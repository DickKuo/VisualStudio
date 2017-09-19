

//-----------------------------------------------------------------------------------
//檔案上傳預覽-檢查檔案大小
//-------------------------------------------------------------------------------------
function FileUpLoad()
{
    $(':file').change(function () {  //選取類型為file且值發生改變的

        var CheckOK = true;
        var file = this.files[0]; //定義file=發生改的file
        name = file.name; //name=檔案名稱
        size = file.size; //size=檔案大小
        type = file.type; //type=檔案型態
        
        if (file.size > 100000) { //假如檔案大小超過300KB (300000/1024)
            CheckOK = false;
            alert("圖片上限100KB!!"); //顯示警告!!
            $(this).val('');  //將檔案欄設為空白
        }
        else if (file.type != 'image/png' && file.type != 'image/jpg'

        && !file.type != 'image/gif' && file.type != 'image/jpeg') { //假如檔案格式不等於 png 、jpg、gif、jpeg
            CheckOK = false;
            alert("檔案格式不符合: png, jpg or gif"); //顯示警告
            $(this).val(''); //將檔案欄設為空
        }
        if (CheckOK) {
            $("input[name='act']").attr("value", "upload");
            $("form[name='myForm']").submit();
        }
    });
}//end FileUpLoad


//-----------------------------------------------------------------------------------
//儲存檔案
//-------------------------------------------------------------------------------------
function postForm() {
    $("input[name='act']").attr("value", "post");
}//end postForm
