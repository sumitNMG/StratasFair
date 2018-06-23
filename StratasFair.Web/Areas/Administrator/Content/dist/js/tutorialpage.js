var firstPair = 0;
var secondPair = 0;
var thirdPair = 0;
var fourthPair = 0;
var fifthPair = 0;
var sixthPair = 0;

function SaveTutorialPage() {
    firstPair = ValidateTutorialFileInputSave('GetTutorialPage1_FilePathBase');
    secondPair = ValidateTutorialFileInputSave('GetTutorialPage2_FilePathBase');
    thirdPair = ValidateTutorialFileInputSave('GetTutorialPage3_FilePathBase');
    fourthPair = ValidateTutorialFileInputSave('GetTutorialPage4_FilePathBase');
    fifthPair = ValidateTutorialFileInputSave('GetTutorialPage5_FilePathBase');
    sixthPair = ValidateTutorialFileInputSave('GetTutorialPage6_FilePathBase');
  

    if (firstPair == 1 || secondPair == 1 || thirdPair == 1 || fourthPair == 1 || fifthPair == 1 || sixthPair == 1) {
        return false;
    } else {
        return true;
    }
}




function ValidateTutorialFileInputSave(obj) {
    var val = $('#' + obj).val().toLowerCase();
    if (val != '') {
        var regexA = new RegExp("(.*?)\.(png|jpg|jpeg|gif|mp3|mp4)$");
        var regexG = new RegExp("(.*?)\.(png|jpg|jpeg|gif)$");
        if (!(regexA.test(val))) {
            $('#' + obj).val('');
            $('#' + obj + "-error").remove();
            $('#' + obj).addClass("input-validation-error");
            $('span[data-valmsg-for="' + obj.replace('_', '.') + '"]').append("<span id='" + obj + "-error' style='color:#ff0000'>Invalid file.</span>");
            return 1;
        }
        else {
            $('span[data-valmsg-for="' + obj.replace('_', '.') + '"]').empty();
        }

        var extension = getFileExtension(val);
        var obj1 = obj.replace('FilePathBase', 'GraphicPathBase');
        var val2 = $('#' + obj1).val().toLowerCase();
        if (extension == 'mp3') {
            if (val2 == '') {
                $('#' + obj1 + "-error").remove();
                $('span[data-valmsg-for="' + obj1.replace('_', '.') + '"]').append("<span id='" + obj1 + "-error' style='color:#ff0000'>File required.</span>");
                return 1;
            }
            else {
                if (!(regexG.test(val2))) {
                    $('#' + obj1).val('');
                    $('#' + obj1 + "-error").remove();
                    $('#' + obj1).addClass("input-validation-error");
                    $('span[data-valmsg-for="' + obj1.replace('_', '.') + '"]').append("<span id='" + obj1 + "-error' style='color:#ff0000'>Invalid file.</span>");
                    return 1;
                }
                else {
                    $('span[data-valmsg-for="' + obj1.replace('_', '.') + '"]').empty();
                }
            }
        }
        else {
            $('span[data-valmsg-for="' + obj1.replace('_', '.') + '"]').empty();
        }
    }
    return 0;
}


function ValidateTutorialFileInput(obj) {
    var val = $(obj).val().toLowerCase();
    var regexG = new RegExp("(.*?)\.(png|jpg|jpeg|gif|mp3|mp4)$");
    if (!(regexG.test(val))) {
        $(obj).val('');
        $("#" + obj.id + "-error").remove();
        $("#" + obj.id).addClass("input-validation-error");
        $('span[data-valmsg-for="' + obj.id.replace('_', '.') + '"]').append("<span id='" + obj.id + "-error' style='color:#ff0000'>Invalid file.</span>");
        return false;
    }
    else {
        $('span[data-valmsg-for="' + obj.id.replace('_', '.') + '"]').empty();
    }
    return true;
}


function getFileExtension(name) {
    var found = name.lastIndexOf('.') + 1;
    return (found > 0 ? name.substr(found) : "");
}

function ValidateTutorialFileInput(obj, type) {
    var val = $(obj).val().toLowerCase();

    var regexA = new RegExp("(.*?)\.(png|jpg|jpeg|gif|mp3|mp4)$");
    var regexG = new RegExp("(.*?)\.(png|jpg|jpeg|gif)$");

    if (type == "A" && !(regexA.test(val))) {
        $(obj).val('');
        $("#" + obj.id + "-error").remove();
        $("#" + obj.id).addClass("input-validation-error");
        $('span[data-valmsg-for="' + obj.id.replace('_', '.') + '"]').append("<span id='" + obj.id + "-error' style='color:#ff0000'>Invalid file.</span>");
        return false;
    }
    else if (type == "G" && !(regexG.test(val))) {
        $(obj).val('');
        $("#" + obj.id + "-error").remove();
        $("#" + obj.id).addClass("input-validation-error");
        $('span[data-valmsg-for="' + obj.id.replace('_', '.') + '"]').append("<span id='" + obj.id + "-error' style='color:#ff0000'>Invalid file.</span>");
        return false;
    }
    else {
       
        if (type == 'A') {
            var extension = getFileExtension(val);
            if (extension != 'mp3') {
                $('#' + obj.id.replace('FilePathBase', 'GraphicPathBase')).attr('disabled', 'disabled');
                $('#' + obj.id.replace('FilePathBase', 'GraphicPathBase')).val('');
                $('span[data-valmsg-for="' + obj.id.replace('_', '.').replace('FilePathBase', 'GraphicPathBase') + '"]').empty();
            }
            else {
                $('#' + obj.id.replace('FilePathBase', 'GraphicPathBase')).removeAttr('disabled');
            }
        }
        $('span[data-valmsg-for="' + obj.id.replace('_', '.') + '"]').empty();
    }
    return true;
}


