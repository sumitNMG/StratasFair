//Header Fixed
$(document).scroll(function () {

    if ($(window).scrollTop() > 20) {
        $('header').addClass('fixed');
    }
    else {
        $('header').removeClass('fixed');
    }
});
//Tab Menu
jQuery(function () {
    jQuery('.targetDiv').hide();
    jQuery('.showSingle').click(function () {
        jQuery('.showSingle').removeClass('active');
        jQuery(this).addClass('active');
        jQuery('.targetDiv').hide();
        jQuery('#div' + $(this).attr('target')).show();

    });
    jQuery('#first-anchor').trigger('click');
});
//general information more view
$(document).ready(function () {
    // Configure/customize these variables.
    var showChar = 200;  // How many characters are shown by default
    var ellipsestext = "";
    var moretext = "More...";
    var lesstext = "Less";

    $('.more').each(function () {
        var content = $(this).html();

        if (content.length > showChar) {

            var c = content.substr(0, showChar);
            var h = content.substr(showChar, content.length - showChar);

            var html = c + '<span class="moreellipses">' + ellipsestext + '&nbsp;</span><span class="morecontent"><span>' + h + '</span><a href="" class="morelink positive">' + moretext + '</a></span>';

            $(this).html(html);
        }

    });
    var h = $(window).height();
    $('.left_col').height(h);

    $(".morelink").click(function () {
        if ($(this).hasClass("less")) {
            $(this).removeClass("less");
            $(this).addClass("positive");
            $(this).removeClass("negative");
            $(this).html(moretext);
        } else {
            $(this).addClass("less");
            $(this).addClass("negative");
            $(this).removeClass("positive");
            $(this).html(lesstext);
        }
        $(this).parent().prev().toggle();
        $(this).prev().toggle();
        return false;
    });
    //my request Page
    $('.my-request .view-cont').click(function () {
        $('.my-request .more-cont').slideToggle('slow', function () {
            if ($('.my-request .view-cont').hasClass("text-yellow")) {

                $('.my-request .view-cont').removeClass('text-yellow');
                $('.my-request .view-cont').addClass('text-black');

            }
            else {
                $('.my-request .view-cont').removeClass('text-black');
                $('.my-request .view-cont').addClass('text-yellow');
            }
        });
    });
    //data tooltip
    $('[data-toggle="tooltip"]').tooltip();
    //data tooltip
    $('.booking-expand').hide().before('');
    $('a#toggle-booking').click(function () {
        $('.booking-expand').slideToggle(1000);
        return false;
    });
    $('.search-expand').hide().before('');
    $('a#search-owner').click(function () {
       $('.search-expand').slideToggle(500);
        return false;
    });
   // $(".strata-email ul.inbox-mail li").click(function () {
   //     $(".strata-email .viewmore").show();
   // });
    //$(".strata-email .reply").click(function () {
    //    $(".strata-email .form-section").show();
    //});
   // $(".strata-email a.close-btn").click(function () {
   //     $(".strata-email .viewmore").slideToggle('1000');
   // });
});

//date picker
function uploadFile(target) {
    var fileType = target.files[0].name.split('.');;
    var fileName = target.files[0].name;
    var finalName = "";
    if (fileName.length > 14) {
        finalName = fileName.substr(0, 3)+"...";
        finalName = finalName + fileName.substr(fileName.length - 4, 3);
        finalName=finalName + "." + fileName[1]
    }
    else {
        finalName = fileName + "." + fileName[1];
    }
    $("#file-name").text(finalName);
    $("#file-name").attr("title", target.files[0].name);

}
$(function () {
    $(".datepicker").datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: new Date()
    });

    $("#datepicker").datepicker({
        changeMonth: true,
        changeYear: true
    });

    $("#datepicker2").datepicker({
        changeMonth: true,
        changeYear: true
    });


    $("#datepicker1").datepicker({
        changeMonth: true,
        changeYear: true,
        minDate: new Date()
    });

    // Since confModal is essentially a nested modal it's enforceFocus method
    // must be no-op'd or the following error results 
    // "Uncaught RangeError: Maximum call stack size exceeded"
    // But then when the nested modal is hidden we reset modal.enforceFocus
    var enforceModalFocusFn = $.fn.modal.Constructor.prototype.enforceFocus;

    $.fn.modal.Constructor.prototype.enforceFocus = function () { };

   
});
