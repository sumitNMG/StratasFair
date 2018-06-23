$(document).ready(function () {
    var initialLocaleCode = 'en';
    $('#eventcalendar').fullCalendar({
      header: {
        left: 'prev, today',
        center: 'title',
        right: 'month,agendaDay, next'
      },
      defaultDate: '2018-05-10',
      locale: initialLocaleCode,
      buttonIcons: false, // show the prev/next text
      weekNumbers: false,
      navLinks: true, // can click day/week names to navigate views
      editable: false,
      eventLimit: false, // allow "more" link when too many events
      events: [
        {
          title: 'All Day Event',
          start: '2018-05-01'
        },
        {
          title: 'first Event',
          start: '2018-05-07',
          end: '2018-05-10',
		   color: '#f4516c'
        },
		 {
          title: 'second Event',
          start: '2018-05-07',
          end: '2018-05-10',
		  color: '#d73550'
        },
        {
          id: 999,
          title: 'Repeating Event',
          start: '2018-05-09T16:00:00'
        },
        {
          id: 999,
          title: 'Repeating Event',
          start: '2018-05-16T16:00:00'
        },
        {
          //title: 'Conference',
         // start: '2018-12-11',
          //end: '2018-12-13'
        },
        {
          title: 'Meeting',
          start: '2018-05-12T10:30:00',
          end: '2018-05-12T12:30:00',
		  color: '#ff8f1f'
        },
		 {
          title: 'Meeting2',
          start: '2018-05-12T10:30:00',
          end: '2018-05-12T12:30:00'
        },
		 {
          title: 'Meeting3',
          start: '2018-05-12T10:30:00',
          end: '2018-05-12T12:30:00'
        },
        {
          title: 'Lunch',
          start: '2018-05-12T12:00:00',
		   end: '2018-05-12T13:30:00',
		    color: '#257e4a'
        },

        {
          title: 'Meeting',
          start: '2018-05-12T14:30:00',
		   color: '#ff0000'
        },
        {
          title: 'Happy Hour',
          start: '2018-05-12T17:30:00'
        },
        {
          title: 'Dinner',
          start: '2018-05-12T20:00:00'
        },
        {
          title: 'Birthday Party',
          start: '2018-05-13T07:00:00'
        },
        {
          title: 'Click for Google',
          url: 'http://google.com/',
          start: '2018-05-28'
        }
      ]
    });

    // build the locale selector's options
    $.each($.fullCalendar.locales, function(localeCode) {
      $('#locale-selector').append(
        $('<option/>')
          .attr('value', localeCode)
          .prop('selected', localeCode == initialLocaleCode)
          .text(localeCode)
      );
    });

  });
  //calendar
$(document).ready(function() {
    var initialLocaleCode = 'en';

    $('#eventcalendar2').fullCalendar({
      header: {
        left: 'prev, today',
        center: 'title',
        right: 'month,agendaDay, next'
      },
      defaultDate: '2018-05-12',
      locale: initialLocaleCode,
      buttonIcons: false, // show the prev/next text
      weekNumbers: false,
      navLinks: true, // can click day/week names to navigate views
      editable: false,
      eventLimit: false, // allow "more" link when too many events
      events: [
        {
          title: 'All Day Event',
          start: '2018-05-01'
        },
        {
          title: 'first Event',
          start: '2018-05-07',
          end: '2018-05-10',
		   color: '#f4516c'
        },
		 {
          title: 'second Event',
          start: '2018-05-07',
          end: '2018-05-10',
		  color: '#d73550'
        },
        {
          id: 999,
          title: 'Repeating Event',
          start: '2018-05-09T16:00:00'
        },
        {
          id: 999,
          title: 'Repeating Event',
          start: '2018-05-16T16:00:00'
        },
        {
          //title: 'Conference',
         // start: '2018-12-11',
          //end: '2018-12-13'
        },
        {
          title: 'Meeting',
          start: '2018-05-12T10:30:00',
          end: '2018-05-12T12:30:00',
		  color: '#ff8f1f'
        },
		 {
          title: 'Meeting2',
          start: '2018-05-12T10:30:00',
          end: '2018-05-12T12:30:00'
        },
		 {
          title: 'Meeting3',
          start: '2018-05-12T10:30:00',
          end: '2018-05-12T12:30:00'
        },
        {
          title: 'Lunch',
          start: '2018-05-12T12:00:00',
		   end: '2018-05-12T13:30:00',
		    color: '#257e4a'
        },

        {
          title: 'Meeting',
          start: '2018-05-12T14:30:00',
		   color: '#ff0000'
        },
        {
          title: 'Happy Hour',
          start: '2018-05-12T17:30:00'
        },
        {
          title: 'Dinner',
          start: '2018-05-12T20:00:00'
        },
        {
          title: 'Birthday Party',
          start: '2018-05-13T07:00:00'
        },
        {
          title: 'Click for Google',
          url: 'http://google.com/',
          start: '2018-05-28'
        }
      ]
    });

    // build the locale selector's options
    $.each($.fullCalendar.locales, function(localeCode) {
      $('#locale-selector').append(
        $('<option/>')
          .attr('value', localeCode)
          .prop('selected', localeCode == initialLocaleCode)
          .text(localeCode)
      );
    });

  });
