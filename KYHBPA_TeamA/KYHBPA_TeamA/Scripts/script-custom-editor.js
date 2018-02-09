$(document).ready(function () {
    // Initialize Editor   
    $('.textarea-editor').summernote(
        {
            height: 300,         // set editor height
            width: 1140,
            minHeight: null,       // set minimum height of editor  
            maxHeight: null,       // set maximum height of editor  
            focus: true,       // set focus to editable area after initializing summernote  
            maximumImageFileSize: 500000 // set max image file size
        });
});  