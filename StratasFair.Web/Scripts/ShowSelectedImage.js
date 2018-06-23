function Upload(fileUploaderId, ImageId) {
    //Get reference of FileUpload.
    var fileUpload = document.getElemntById(fileUploaderId);

    //Check whether the file is valid Image.
    var regex = new RegExp("([a-zA-Z0-9\s_\\.\-:])+(.jpg|.png|.gif)$");
    if (regex.test(fileUpload.value.toLowerCase())) {

        //Check whether HTML5 is supported.
        if (typeof (fileUpload.files) != "undefined") {
            //Initiate the FileReader object.
            var reader = new FileReader();
            //Read the contents of Image File.
            reader.readAsDataURL(fileUpload.files[0]);
            reader.onload = function (e) {
                //Initiate the JavaScript Image object.
                var image = new Image();

                //Set the Base64 string return from FileReader as source.
                image.src = e.target.result;
                var img = document.getElementById(ImageId);

                img.src = e.target.result;

                //Validate the File Height and Width.
                image.onload = function () {
                    var height = this.height;
                    var width = this.width;
                    if (height > 100 || width > 100) {
                        alert("Height and Width must not exceed 100px.");
                        return false;
                    }
                    return true;
                };

            }
        } else {
            alert("This browser does not support HTML5.");
            return false;
        }
    } else {
        alert("Please select a valid Image file.");
        return false;
    }
}