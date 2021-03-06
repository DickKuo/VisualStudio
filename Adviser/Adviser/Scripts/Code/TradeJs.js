﻿
$(function () {
    var Url = "../Trade/";

    var BtnName = "";
    var Contract = "";

    function fancyconfirm(msg, options, callback) {
        $.fancybox("#confirm", {
            modal: options.modal,
            beforeShow: function () {
                this.content.prepend("<p class=\"title\"></p>");
                $(".title").html(msg);
                if (options.buttons == null) {
                    options.buttons = [{
                        clsName: "yes",
                        title: "Yes",
                        value: true
                    }, {
                        clsName: "no",
                        title: "No",
                        value: false
                    }];
                }
                for (i = 0; i < options.buttons.length; i++) {
                    this.content.append($("<input>", {
                        type: "button",
                        class: "confirm " + options.buttons[i].class,
                        value: options.buttons[i].title
                    }).data("index", i).css("margin-left", ((i > 0) ? "10px" : "")));
                }
                $(this.content).css("text-align", "center");
            },
            afterShow: function () {
                $(".confirm").on("click", function (event) {
                    ret = options.buttons[$(event.target).data("index")].value; 
                    if (ret == "yes") {
                        Commd(BtnName);
                    }
                    else {
                        $.fancybox.close();
                    }
                });
            },
            afterClose: function () {
                this.content.html("");
                callback.call(this, ret);
            }
        });
    }//end fancyconfirm

    $("#QuotesTable").on("click", ".btn-primary", function (e) {
        e.preventDefault();
        BtnName = $(this).attr("name");
        Contract = $(this).attr("data-Contract");
         

        var ChildIndex = 0;
        if (BtnName == "Put") {
            ChildIndex = 9;
        }
        else {
            ChildIndex = 3;
        }

        if ($(this).parent().parent().children().eq(ChildIndex).html() == null)
        {
            ChildIndex = 3;           
        }

        var values = $(this).parent().parent().children().eq(ChildIndex).html();

        fancyconfirm("Do you wish to continue?<br>" +
                "<label class='col-xs-12 control-label'> " + $("#DueMonth").text() + " </label>" +
                "<div class='form-group'  style='padding-top:25px;'>" +
                        "<label class='col-xs-4 control-label'>Contract</label>" +
                    "<div class='col-xs-8'>" +
                        "<input class='form-control'  id='Contract' readonly='true'  type='text' value='" + Contract + "'>" +
                    "</div>" +
                "</div>" +
                "<div class='form-group' style='padding-top:25px;'>" +
                    "<label class='col-xs-4 control-label'> Price </label>" +
                    "<div class='col-xs-8'>" +
                        "<input class='form-control' id='Price'  type='text' value='" + values + "'>" +
                    "</div>" +
                "</div>" +

                 "<div class='form-group'  style='padding-top:25px;'>" +
                        "<label class='col-xs-4 control-label'>Lot</label>" +
                    "<div class='col-xs-8'>" +
                        "<input class='form-control'  id='Lot'    type='text' value='1'>" +
                    "</div>" +
                "</div>" +

                "<div class='form-group' style='padding-top:25px;'>" +
                   	"<label class='radio-inline'><input type='radio' value='Sell' name='optradio'  data-name='Sell'>Sell</label>" +
                    "<label class='radio-inline'><input type='radio' value='Buy'  name='optradio'  data-name='Buy'>Buy</label>" +
			
                "</div>    <br/>"
              , {
                  buttons: [{
                      class: "yes",
                      type: "button",
                      title: "Yes",
                      value: "yes"
                  }, {
                      class: "no",
                      type: "button",
                      title: "No",
                      value: "no"
                  }],
                  modal: true
              },
         function (resp) {
             if (resp == "yes") {
             }
         });
    });
    
    function Commd(BtnName) {       
        var BuyType = $('input[name="optradio"]:checked').val();
        var DueMonth;        
        if ($("#DueMonth").html() != '') {
            DueMonth = $("#DueMonth").html();
        }
        else {
            DueMonth = $("#DueMonth").val();
        }

        $.ajax({
            type: "POST",
            url: Url + "AddTrade",
            data: {
                "Op": BtnName,
                "Contract": Contract,
                "Type": BuyType,
                "Clinch": $("#Price").val(),
                "DueMonth": DueMonth,
                "Lot": $("#Lot").val()
            },
            success: function (Model) {
                $.fancybox.close();
                alert(Model);
            }
        });

    }//end Commd
    
    $("#Btn_Search").on("click", function () {
        //$.fancybox.showLoading();       
        $.ajax({
            type: "POST",
            url: Url + "Quotes",
            data: {
                "DueMonth": $("#Drop_DueMonth").val(),
                IsPartial :true
            },

            success: function (Model) {
                $("#QuotesTable").html(Model);
               //$.fancybox.hideLoading();
            }
        });

    });

});