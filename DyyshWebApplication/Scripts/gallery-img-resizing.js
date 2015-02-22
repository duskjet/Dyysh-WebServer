$(window).load(function () {
    $('#image').each(function () {
        var maxWidth = $(window).width() * 0.9; // Max width for the image
        var maxHeight = $(window).height() * 0.8;    // Max height for the image
        var ratio = 0;  // Used for aspect ratio
        var width = $(this).width();    // Current image width
        var height = $(this).height();  // Current image height

        // Check if the current width is larger than the max
        if (width > maxWidth) {
            ratio = maxWidth / width;   // get ratio for scaling image
            $(this).css("width", maxWidth); // Set new width
            $(this).css("height", height * ratio);  // Scale height based on ratio
            height = height * ratio;    // Reset height to match scaled image
        }

        var width = $(this).width();    // Current image width
        var height = $(this).height();  // Current image height
        $('#img-container').css("width", width); // Change width of the parent container

        // Check if current height is larger than max
        if (height > maxHeight) {
            ratio = maxHeight / height; // get ratio for scaling image
            $(this).css("height", maxHeight);   // Set new height
            $(this).css("width", width * ratio);    // Scale width based on ratio
            $('#img-container').css("width", width * ratio); // Change width of the parent container
            width = width * ratio;    // Reset width to match scaled image
        }
    });
});