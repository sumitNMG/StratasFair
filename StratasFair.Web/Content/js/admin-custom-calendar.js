// This JS is written to fullfill the requirement for Owner Calendar (General Calendar and Community Calendar).
// Don't use it on another page. 


var calUrl = '';
var currDate = new Date();


// For fetching the record
function GetOwnerData(type) {
    var tmp = null;
    calUrl = generalCalUrl;  // This variable(generalCalUrl) is declared on the parent page and assign the destination URL...
    $.ajax({
        'async': false,
        'type': "GET",
        'global': false,
        'dataType': 'json',
        'url': calUrl,
        'data': { 'type': type },
        'success': function (data) {
            tmp = data.resultSet;
        }
    });
    return tmp;
}




//Owner Common Area Calendar
$(document).ready(function () {
    var initialLocaleCode = 'en';
    var eventListingCommonArea = GetOwnerData('admin-commonarea');
    $('#eventcalendar2').fullCalendar({
        header: {
            left: 'prev, today',
            center: 'title',
            right: 'month,agendaDay, next'
        },
        //defaultDate: "'" + dateString + "'",
        locale: initialLocaleCode,
        buttonIcons: false, // show the prev/next text
        weekNumbers: false,
        navLinks: true, // can click day/week names to navigate views
        editable: false,
        eventLimit: false, // allow "more" link when too many events
        events: function (start, end, timezone, callback) {
            var obj = jQuery.parseJSON(eventListingCommonArea);
            var eventsArr = [];
            for (var i in obj) {
                eventsArr.push({
                    title: obj[i].title,
                    start: obj[i].start,
                    end: obj[i].end,
                    description: obj[i].description,
                    color: obj[i].color
                });
            }
            callback(eventsArr); // Callback is used for rendering the events dynamically while calendar is rendering
        },
        eventRender: function (event, element) {
            // This tool is used to show the tooltip on the events in calendar
            // Related JS (jquery.qtip.js) and CSS (jquery.qtip.css) file is added on the parent page
            element.qtip({
                content: {
                    text: '<b>' + event.title + '</b></br>Description: ' + event.description
                },
                position: {
                    at: 'bottom left'
                }
            });
        }

    });

    // build the locale selector's options
    $.each($.fullCalendar.locales, function (localeCode) {
        $('#locale-selector').append(
          $('<option/>')
            .attr('value', localeCode)
            .prop('selected', localeCode == initialLocaleCode)
            .text(localeCode)
        );
    });

});
