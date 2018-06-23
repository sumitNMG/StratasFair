function validateFileType(element, extArray) {
    var MaxImageSize = (5*1024*1024); // in MB
    var MaxFileSize = (50*1024*1024); // in MB



    var file = element.val();
    // first check if file field has any value
    if (file) {
        var ext = "";
        $.each(extArray, function (index, value) {
            ext = ext + value + ", ";
        });
        // split file name at dot
        var get_ext = file.split('.');
        // reverse name to check extension
        get_ext = get_ext.reverse();
        // check file type is valid as given in 'exts' array
        if ($.inArray(get_ext[0].toLowerCase(), extArray) <= -1) {
            swal('Error','Please select valid file. Only ' + ext + ' files are allowed','error');
            element.val('');
        }
        else {
            var iSize = (element[0].files[0].size);
            var fileExt = get_ext[0].toLowerCase();
            if (fileExt == "jpg" || fileExt == "png" || fileExt == "jpeg" || fileExt == "gif") {
                if (iSize > MaxImageSize) {
                    swal('Error','Image must be less than ' + MaxImageSize + ' MB','error');
                    element.val('');
                }                
            }
            else
            {
                if (iSize > MaxFileSize) {
                    swal('Error', 'File must be less than ' + MaxFileSize + ' MB', 'error');
                    element.val('');
                }
            }
        }
    }
}