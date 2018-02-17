$(document).ready(function () {
    var eventButtonClicked = false;

    $('.collapsible').click(function () {
        var itemID = $(this).attr('data-target');
        var targetElement = $(itemID);
        var eventHasBeenClicked = targetElement.attr('class');

        if (eventHasBeenClicked != "in") {
            $(this).empty().append("<i style='color:#072419' data-toggle='collapse' data-target='" + itemID + "' class='fa fa-angle-up fa-2x'></i>");
            eventButtonClicked = true;
        } else {
            $(this).empty().append("<i style='color:#072419' data-toggle='collapse' data-target='" + itemID + "' class='fa fa-angle-down fa-2x'></i>");
            eventButtonClicked = false;
        }
    });
});