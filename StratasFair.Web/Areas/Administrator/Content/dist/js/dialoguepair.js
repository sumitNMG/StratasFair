var isResrict = 0;
var firstPair = 0;
var secondPair = 0;
var thirdPair = 0;
var fourthPair = 0;
var fifthPair = 0;
var sixthPair = 0;

function SaveDialogPair() {
    // for the divDialoguePair1

    isResrict = 0;
    firstPair = CheckAllInput('LearnerDialogue1');
    if (firstPair == 0) {
        SetError('LearnerDialogue1_FullText', 'Full text required');
        SetError('LearnerDialogue1_KeyPhrase', 'Key phrase required');
        SetError('LearnerDialogue1_AudioPathBase', 'Audio path required');
        SetError('LearnerDialogue1_GraphicPathBase', 'Graphic path required');
        SetError('LearnerDialogue1_CustomerAudioPathBase', 'Audio path required');
        SetError('LearnerDialogue1_CustomerGraphicPathBase', 'Graphic path required');
    }
    else {
        RemoveError('LearnerDialogue1_FullText', 'class');
        RemoveError('LearnerDialogue1_KeyPhrase', 'class');
        RemoveError('LearnerDialogue1_AudioPathBase', '');
        RemoveError('LearnerDialogue1_GraphicPathBase', '');
        RemoveError('LearnerDialogue1_CustomerAudioPathBase', '');
        RemoveError('LearnerDialogue1_CustomerGraphicPathBase', '');
    }

    secondPair = CheckAllInput('LearnerDialogue2');
    if (secondPair == 0) {
        SetError('LearnerDialogue2_FullText', 'Full text required');
        SetError('LearnerDialogue2_KeyPhrase', 'Key phrase required');
        SetError('LearnerDialogue2_AudioPathBase', 'Audio path required');
        SetError('LearnerDialogue2_GraphicPathBase', 'Graphic path required');
        SetError('LearnerDialogue2_CustomerAudioPathBase', 'Audio path required');
        SetError('LearnerDialogue2_CustomerGraphicPathBase', 'Graphic path required');
    }
    else {
        RemoveError('LearnerDialogue2_FullText', 'class');
        RemoveError('LearnerDialogue2_KeyPhrase', 'class');
        RemoveError('LearnerDialogue2_AudioPathBase', '');
        RemoveError('LearnerDialogue2_GraphicPathBase', '');
        RemoveError('LearnerDialogue2_CustomerAudioPathBase', '');
        RemoveError('LearnerDialogue2_CustomerGraphicPathBase', '');
    }


    thirdPair = CheckAllInput('LearnerDialogue3');
    if (thirdPair == 0) {
        SetError('LearnerDialogue3_FullText', 'Full text required');
        SetError('LearnerDialogue3_KeyPhrase', 'Key phrase required');
        SetError('LearnerDialogue3_AudioPathBase', 'Audio path required');
        SetError('LearnerDialogue3_GraphicPathBase', 'Graphic path required');
        SetError('LearnerDialogue3_CustomerAudioPathBase', 'Audio path required');
        SetError('LearnerDialogue3_CustomerGraphicPathBase', 'Graphic path required');
    }
    else {
        RemoveError('LearnerDialogue3_FullText', 'class');
        RemoveError('LearnerDialogue3_KeyPhrase', 'class');
        RemoveError('LearnerDialogue3_AudioPathBase', '');
        RemoveError('LearnerDialogue3_GraphicPathBase', '');
        RemoveError('LearnerDialogue3_CustomerAudioPathBase', '');
        RemoveError('LearnerDialogue3_CustomerGraphicPathBase', '');
    }


    fourthPair = CheckAllInput('LearnerDialogue4');
    if (fourthPair == 0) {
        SetError('LearnerDialogue4_FullText', 'Full text required');
        SetError('LearnerDialogue4_KeyPhrase', 'Key phrase required');
        SetError('LearnerDialogue4_AudioPathBase', 'Audio path required');
        SetError('LearnerDialogue4_GraphicPathBase', 'Graphic path required');
        SetError('LearnerDialogue4_CustomerAudioPathBase', 'Audio path required');
        SetError('LearnerDialogue4_CustomerGraphicPathBase', 'Graphic path required');
    }
    else {
        RemoveError('LearnerDialogue4_FullText', 'class');
        RemoveError('LearnerDialogue4_KeyPhrase', 'class');
        RemoveError('LearnerDialogue4_AudioPathBase', '');
        RemoveError('LearnerDialogue4_GraphicPathBase', '');
        RemoveError('LearnerDialogue4_CustomerAudioPathBase', '');
        RemoveError('LearnerDialogue4_CustomerGraphicPathBase', '');
    }

    fifthPair = CheckAllInput('LearnerDialogue5');
    if (fifthPair == 0) {
        SetError('LearnerDialogue5_FullText', 'Full text required');
        SetError('LearnerDialogue5_KeyPhrase', 'Key phrase required');
        SetError('LearnerDialogue5_AudioPathBase', 'Audio path required');
        SetError('LearnerDialogue5_GraphicPathBase', 'Graphic path required');
        SetError('LearnerDialogue5_CustomerAudioPathBase', 'Audio path required');
        SetError('LearnerDialogue5_CustomerGraphicPathBase', 'Graphic path required');
    }
    else {
        RemoveError('LearnerDialogue5_FullText', 'class');
        RemoveError('LearnerDialogue5_KeyPhrase', 'class');
        RemoveError('LearnerDialogue5_AudioPathBase', '');
        RemoveError('LearnerDialogue5_GraphicPathBase', '');
        RemoveError('LearnerDialogue5_CustomerAudioPathBase', '');
        RemoveError('LearnerDialogue5_CustomerGraphicPathBase', '');
    }


    if (((firstPair == 1 && secondPair == 1 && thirdPair == 1 && fourthPair == 1 && fifthPair == 1)) || isResrict == 1) {
        if (isResrict == 0) {
            alert("Oops! No data found.");
        }
        return false;
    } else {
        return true;
    }
}


function ValidateFileInput(obj, type) {
    var val = $(obj).val().toLowerCase();
    var regexG = new RegExp("(.*?)\.(png|jpg|jpeg|gif)$");
    var regexA = new RegExp("(.*?)\.(mp3)$");
    if (type == "G" && !(regexG.test(val))) {
        $(obj).val('');
        $("#" + obj.id + "-error").remove();
        $("#" + obj.id).addClass("input-validation-error");
        $('span[data-valmsg-for="' + obj.id.replace('_', '.') + '"]').append("<span id='" + obj.id + "-error' style='color:#ff0000'>Invalid file.</span>");
        return false;
    }
    else if (type == "A" && !(regexA.test(val))) {
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

function SetError(id, msg) {
    if ($("#" + id).val().replace(/(\r\n|\n|\r)/gm, "") == "") {
        $("#" + id + "-error").remove();
        $("#" + id).addClass("input-validation-error");
        $('span[data-valmsg-for="' + id.replace('_', '.') + '"]').append("<span id='" + id + "-error' style='color:#ff0000'>" + msg + ".</span>");
        isResrict = 1;
    }
    else {
        $('span[data-valmsg-for="' + id.replace('_', '.') + '"]').empty();
    }
}

function RemoveError(id, rem) {
    if (rem == 'class') {
        $("#" + id).removeClass("input-validation-error");
    }
    $('span[data-valmsg-for="' + id.replace('_', '.') + '"]').empty();
}

function CheckAllInput(obj1) {
    if ($("#" + obj1 + "_FullText").val() == ""
      && $("#" + obj1 + "_KeyPhrase").val() == ""
      && $("#" + obj1 + "_Time").val() == ""
      && $("#" + obj1 + "_AudioPathBase").val() == ""
      && $("#" + obj1 + "_GraphicPathBase").val() == ""
      && $("#" + obj1 + "_Notes").val() == ""
      && $("#" + obj1 + "_CustomerAudioPathBase").val() == ""
      && $("#" + obj1 + "_CustomerTime").val() == ""
      && $("#" + obj1 + "_CustomerGraphicPathBase").val() == ""
      && $("#" + obj1 + "_CustomerNotes").val() == "") {
        return 1;
    }
    else {
        return 0;
    }
}


function SaveEditDialogPair() {
    isResrict = 0;
    firstPair = CheckAllInputEdit();
    if (firstPair == 0) {
        SetError('FullText', 'Full text required');
        SetError('KeyPhrase', 'Key phrase required');
     
    }
    else {
        RemoveError('LearnerDialogue1_FullText', 'class');
        RemoveError('LearnerDialogue1_KeyPhrase', 'class');
        RemoveError('LearnerDialogue1_AudioPathBase', '');
        RemoveError('LearnerDialogue1_GraphicPathBase', '');
        RemoveError('LearnerDialogue1_CustomerAudioPathBase', '');
        RemoveError('LearnerDialogue1_CustomerGraphicPathBase', '');
    }


    if ((firstPair == 1) || isResrict == 1) {
        return false;
    } else {
        return true;
    }
}

function CheckAllInputEdit() {
    if ($("#FullText").val().replace(/(\r\n|\n|\r)/gm, "") == "" && $("#KeyPhrase").val().replace(/(\r\n|\n|\r)/gm, "") == "" && $("#Time").val() == "" && $("#AudioPathBase").val() == "" && $("#GraphicPathBase").val() == ""
      && $("#Notes").val() == "" && $("#CustomerAudioPathBase").val() == "" && $("#CustomerTime").val() == "" && $("#CustomerGraphicPathBase").val() == ""
      && $("#CustomerNotes").val() == "") {
        return 1;
    }
    else {
        return 0;
    }
}




function SaveQuizQuestion() {
    isResrict = 0;
    firstPair = CheckAllQuizInput('GetQuizQuestion1');
    if (firstPair == 0) {
        SetQuizError('GetQuizQuestion1_Question', 'Question required');
        if ($('#GetQuizQuestion1_QuestionType').val() != "truefalse") {
            SetQuizError('GetQuizQuestion1_AnswerA', 'Answer required');
            SetQuizError('GetQuizQuestion1_AnswerB', 'Answer required');
            SetQuizError('GetQuizQuestion1_AnswerC', 'Answer required');
            SetQuizError('GetQuizQuestion1_AnswerD', 'Answer required');
        }
    }
    else {
        RemoveQuizError('GetQuizQuestion1_Question');
        RemoveQuizError('GetQuizQuestion1_AnswerA');
        RemoveQuizError('GetQuizQuestion1_AnswerB');
        RemoveQuizError('GetQuizQuestion1_AnswerC');
        RemoveQuizError('GetQuizQuestion1_AnswerD');
    }

    secondPair = CheckAllQuizInput('GetQuizQuestion2');
    if (secondPair == 0) {
        SetQuizError('GetQuizQuestion2_Question', 'Question required');
        if ($('#GetQuizQuestion2_QuestionType').val() != "truefalse") {
            SetQuizError('GetQuizQuestion2_AnswerA', 'Answer required');
            SetQuizError('GetQuizQuestion2_AnswerB', 'Answer required');
            SetQuizError('GetQuizQuestion2_AnswerC', 'Answer required');
            SetQuizError('GetQuizQuestion2_AnswerD', 'Answer required');
        }
    }
    else {
        RemoveQuizError('GetQuizQuestion2_Question');
        RemoveQuizError('GetQuizQuestion2_AnswerA');
        RemoveQuizError('GetQuizQuestion2_AnswerB');
        RemoveQuizError('GetQuizQuestion2_AnswerC');
        RemoveQuizError('GetQuizQuestion2_AnswerD');
    }


    thirdPair = CheckAllQuizInput('GetQuizQuestion3');
    if (thirdPair == 0) {
        SetQuizError('GetQuizQuestion3_Question', 'Question required');
        if ($('#GetQuizQuestion3_QuestionType').val() != "truefalse") {
            SetQuizError('GetQuizQuestion3_AnswerA', 'Answer required');
            SetQuizError('GetQuizQuestion3_AnswerB', 'Answer required');
            SetQuizError('GetQuizQuestion3_AnswerC', 'Answer required');
            SetQuizError('GetQuizQuestion3_AnswerD', 'Answer required');
        }
    }
    else {
        RemoveQuizError('GetQuizQuestion3_Question');
        RemoveQuizError('GetQuizQuestion3_AnswerA');
        RemoveQuizError('GetQuizQuestion3_AnswerB');
        RemoveQuizError('GetQuizQuestion3_AnswerC');
        RemoveQuizError('GetQuizQuestion3_AnswerD');
    }


    fourthPair = CheckAllQuizInput('GetQuizQuestion4');
    if (fourthPair == 0) {
        SetQuizError('GetQuizQuestion4_Question', 'Question required');
        if ($('#GetQuizQuestion4_QuestionType').val() != "truefalse") {
            SetQuizError('GetQuizQuestion4_AnswerA', 'Answer required');
            SetQuizError('GetQuizQuestion4_AnswerB', 'Answer required');
            SetQuizError('GetQuizQuestion4_AnswerC', 'Answer required');
            SetQuizError('GetQuizQuestion4_AnswerD', 'Answer required');
        }
    }
    else {
        RemoveQuizError('GetQuizQuestion4_Question');
        RemoveQuizError('GetQuizQuestion4_AnswerA');
        RemoveQuizError('GetQuizQuestion4_AnswerB');
        RemoveQuizError('GetQuizQuestion4_AnswerC');
        RemoveQuizError('GetQuizQuestion4_AnswerD');
    }

    fifthPair = CheckAllQuizInput('GetQuizQuestion5');
    if (fifthPair == 0) {
        SetQuizError('GetQuizQuestion5_Question', 'Question required');
        if ($('#GetQuizQuestion5_QuestionType').val() != "truefalse") {
            SetQuizError('GetQuizQuestion5_AnswerA', 'Answer required');
            SetQuizError('GetQuizQuestion5_AnswerB', 'Answer required');
            SetQuizError('GetQuizQuestion5_AnswerC', 'Answer required');
            SetQuizError('GetQuizQuestion5_AnswerD', 'Answer required');
        }
    }
    else {
        RemoveQuizError('GetQuizQuestion5_Question');
        RemoveQuizError('GetQuizQuestion5_AnswerA');
        RemoveQuizError('GetQuizQuestion5_AnswerB');
        RemoveQuizError('GetQuizQuestion5_AnswerC');
        RemoveQuizError('GetQuizQuestion5_AnswerD');
    }

    sixthPair = CheckAllQuizInput('GetQuizQuestion6');
    if (sixthPair == 0) {
        SetQuizError('GetQuizQuestion6_Question', 'Question required');


        if ($('#GetQuizQuestion6_QuestionType').val() != "truefalse") {
            SetQuizError('GetQuizQuestion6_AnswerA', 'Answer required');
            SetQuizError('GetQuizQuestion6_AnswerB', 'Answer required');
            SetQuizError('GetQuizQuestion6_AnswerC', 'Answer required');
            SetQuizError('GetQuizQuestion6_AnswerD', 'Answer required');
        }
    }
    else {
        RemoveQuizError('LearnerDialogue6_Question');
        RemoveQuizError('LearnerDialogue6_AnswerA');
        RemoveQuizError('LearnerDialogue6_AnswerB');
        RemoveQuizError('LearnerDialogue6_AnswerC');
        RemoveQuizError('LearnerDialogue6_AnswerD');
    }


    if (((firstPair == 1 && secondPair == 1 && thirdPair == 1 && fourthPair == 1 && fifthPair == 1 && sixthPair == 1)) || isResrict == 1) {
        if (isResrict == 0) {
            alert("Oops! No data found.");
        }
        return false;
    } else {
        return true;
    }
}


function CheckAllQuizInput(obj) {
    if ($("#" + obj + "_Question").val().replace(/(\r\n|\n|\r)/gm, "").trim() == "" && $("#" + obj + "_AnswerA").val().replace(/(\r\n|\n|\r)/gm, "").trim() == "" && $("#" + obj + "_AnswerB").val().replace(/(\r\n|\n|\r)/gm, "").trim() == "" && $("#" + obj + "_AnswerC").val().replace(/(\r\n|\n|\r)/gm, "").trim() == "" && $("#" + obj + "_AnswerD").val().replace(/(\r\n|\n|\r)/gm, "").trim() == "") {
        return 1;
    }
    else {
        return 0;
    }
}

function CheckAllQuizInputEdit() {
    if ($("#Question").val().replace(/(\r\n|\n|\r)/gm, "").trim() == ""
            && $("#AnswerA").val().replace(/(\r\n|\n|\r)/gm, "").trim() == ""
            && $("#AnswerB").val().replace(/(\r\n|\n|\r)/gm, "").trim() == ""
            && $("#AnswerC").val().replace(/(\r\n|\n|\r)/gm, "").trim() == ""
            && $("#AnswerD").val().replace(/(\r\n|\n|\r)/gm, "").trim() == "") {
        return 1;
    }
    else {
        return 0;
    }
}

function SetQuizError(id, msg) {

    if ($("#" + id).val().replace(/(\r\n|\n|\r)/gm, "").trim() == "") {
        $("#" + id + "-error").remove();
        $("#" + id).addClass("input-validation-error");
        $('span[data-valmsg-for="' + id.replace('_', '.') + '"]').append("<span id='" + id + "-error' style='color:#ff0000'>" + msg + ".</span>");
        isResrict = 1;
    }
    else {
        $('span[data-valmsg-for="' + id.replace('_', '.') + '"]').empty();
    }
}

function RemoveQuizError(id) {
    $("#" + id).removeClass("input-validation-error");
    $('span[data-valmsg-for="' + id.replace('_', '.') + '"]').empty();
}



function EnableDisableQuizAnswer(obj, cobj) {
    var posteElement = document.getElementById(cobj + '_Answer');
    if ($(obj).val() == "truefalse") {

        posteElement.removeChild(posteElement[3]);
        posteElement.removeChild(posteElement[2]);
        posteElement.removeChild(posteElement[1]);
        posteElement.removeChild(posteElement[0]);

        $("#" + cobj + "_Answer").addDropDownValues({ true: "True", false: "False" });

        $('#' + cobj + '_AnswerA').attr('disabled', 'disabled'); // disable
        $('#' + cobj + '_AnswerA').val('');

        $('#' + cobj + '_AnswerB').attr('disabled', 'disabled'); // disable
        $('#' + cobj + '_AnswerB').val('');

        $('#' + cobj + '_AnswerC').attr('disabled', 'disabled'); // disable
        $('#' + cobj + '_AnswerC').val('');

        $('#' + cobj + '_AnswerD').attr('disabled', 'disabled'); // disable
        $('#' + cobj + '_AnswerD').val('');
    } else {
        $('#' + cobj + '_AnswerA').removeAttr('disabled'); // enable
        $('#' + cobj + '_AnswerB').removeAttr('disabled'); // enable
        $('#' + cobj + '_AnswerC').removeAttr('disabled'); // enable
        $('#' + cobj + '_AnswerD').removeAttr('disabled'); // enable

        posteElement.removeChild(posteElement[1]);
        posteElement.removeChild(posteElement[0]);
        $("#" + cobj + "_Answer").addDropDownValues({ A: "A", B: "B", C: "C", D: "D" });
    }
}

function EditEnableDisableQuizAnswer(obj) {
    var posteElement = document.getElementById('Answer');
    if ($(obj).val() == "truefalse") {

        posteElement.removeChild(posteElement[3]);
        posteElement.removeChild(posteElement[2]);
        posteElement.removeChild(posteElement[1]);
        posteElement.removeChild(posteElement[0]);

        $("#Answer").addDropDownValues({ true: "True", false: "False" });

        $('#AnswerA').attr('disabled', 'disabled'); // disable
        $('#AnswerA').val('');

        $('#AnswerB').attr('disabled', 'disabled'); // disable
        $('#AnswerB').val('');

        $('#AnswerC').attr('disabled', 'disabled'); // disable
        $('#AnswerC').val('');

        $('#AnswerD').attr('disabled', 'disabled'); // disable
        $('#AnswerD').val('');
    } else {
        $('#AnswerA').removeAttr('disabled'); // enable
        $('#AnswerB').removeAttr('disabled'); // enable
        $('#AnswerC').removeAttr('disabled'); // enable
        $('#AnswerD').removeAttr('disabled'); // enable

        posteElement.removeChild(posteElement[1]);
        posteElement.removeChild(posteElement[0]);
        $("#Answer").addDropDownValues({ A: "A", B: "B", C: "C", D: "D" });
    }
}

jQuery.fn.addDropDownValues = function (options) {

    // Grab the class selector given to the function
    var itemClass = $(this);

    // Iterate through each element and value
    $.each(options, function (value, text) {
        itemClass
            .append($('<option>', { value: value })
            .text(text));
    });
};


function SaveEditQuizQuestion() {
    isResrict = 0;
    firstPair = CheckAllQuizInputEdit();
    if (firstPair == 0) {
        SetQuizError('Question', 'Question required');
        if ($('#QuestionType').val() != "truefalse") {
            SetQuizError('AnswerA', 'Answer required');
            SetQuizError('AnswerB', 'Answer required');
            SetQuizError('AnswerC', 'Answer required');
            SetQuizError('AnswerD', 'Answer required');
        }
    }
    else {
        RemoveQuizError('Question');
        RemoveQuizError('AnswerA');
        RemoveQuizError('AnswerB');
        RemoveQuizError('AnswerC');
        RemoveQuizError('AnswerD');
    }

  

    if ((firstPair == 1) || isResrict == 1) {
        return false;
    } else {
        return true;
    }
}