var day;
var bag;
var debug = false;


function ResetView() {

    setInterval(function () {

        var currentView = $(".calendar-view").val();

        if ($(".sidr").is(":hidden")) {

            if (currentView == "List View") {
                SetStandard();
            }
            else {
                SetFullWidth();
            }
        }
        else {

            if (currentView == "List View") {
                SetStandard();
            }
            else {
                SetStandard();
            }
        }
    }, 500);
}

function SetFullWidth() {

    $(".innerContentForm").css("position", "absolute");
    $(".innerContentForm").css("top", "300px");
    $(".innerContentForm").css("width", "100%");
}

function SetStandard() {


    $(".innerContentForm").css("position", "static");
    $(".innerContentForm").css("top", "0px");
}


$(document).ready(function () {

    ResetView();

    $("#calendarTypes input[type='radio']").each(function () {
        attachReloadEvent(this);
    });

    $("#sidr input[type='checkbox']").each(function () {
        attachReloadEvent(this);
    });


    LoadStateObject();
    moveCalendarTo(day);
});

function attachReloadEvent(sender) {

    $(sender).on("click", function () {
        moveCalendarTo(day);
    });
}

function loadMiniCalendar() { 
    $(".calendar").load("/controls/admin/calendar.aspx?venue=" + $(".ddlVenueFilter").val());

}

function SaveStateObject(venue, hideexternal, hideinternal, hideuntagged, type, date, view, privacy) {
    bag = [venue, hideexternal, hideinternal, hideuntagged, type, date, view, privacy];
    json = JSON.stringify(bag, null, 2);
    $.cookie("CRM_CalendarStateBag", json, { expires: 365 });
    console.log(privacy);
}

function LoadStateObject() {

    var unparsed = $.cookie("CRM_CalendarStateBag");

    if (debug)
        alert("loadstateobject : " + JSON.parse(unparsed));


    if (unparsed != undefined) {
        bag = JSON.parse(unparsed);

        $(".ddlVenueFilter").val(bag[0]);

        $(".ddlPrivacyFilter").val(bag[7]);

        $(".calendar-view").val(bag[6]);

        $("#chkHideExternalVenues").attr("checked", bag[1]);

        $("#chkHideInternalVenues").attr("checked", bag[2]);

        $("#chkHideNonTagged").attr("checked", bag[3]);

        $("#calendarTypes input[type='radio']").each(function () {

            var rb = $(this);

            if (bag[4] == $(rb).data("id")) {
                $(rb).attr("checked", true);
            }

        });


        if (bag[5] != '')
            $(".hdnCalendarCurrentDate").val(bag[5]);

        if (debug)
            alert(bag[5]);
    }

    day = $(".hdnCalendarCurrentDate").val();


    if (debug)
        alert(day);

}


function CollectAndRetrieve() {
    $("#calendar-frame").html("<img src='/_assets/images/admin/icons/loading.gif' alt='loading'/>");

    if (debug)
        alert("collect" + day);
    
    SaveStateObject($(".ddlVenueFilter").val(),
     $("#chkHideExternalVenues").is(":checked"),
     $("#chkHideInternalVenues").is(":checked"),
     $("#chkHideNonTagged").is(":checked"),
     $($("#calendarTypes input[type='radio']:checked")).data("id"), day, $(".calendar-view").val(), $(".ddlPrivacyFilter").val());

    if (debug)
        alert("movecalendarto : " + bag);

    var venue = "&venue=" + bag[0];
    var hideExternal = "&hideexternal=" + bag[1];
    var hideInternal = "&hideinternal=" + bag[2];
    var hideUntagged = "&hideNonTagged=" + bag[3];
    var type = "&type=" + bag[4];
    var date = "&date=" + bag[5];
    var privacy = "&privacy=" + bag[7];

    if (bag[6] == "Rota") {
        $("#advanced-filters").slideUp();
    } else {
        $("#advanced-filters").slideDown();   
    }
    
    $("#calendar-frame").load(DetailsURL(venue + hideExternal + hideInternal + hideUntagged + type + date + privacy), function () {
        loadMiniCalendar();
        moveToEntry();
       
    });
    ResetView();

}

function moveCalendarTo(updateDate) {
    $(".hdnCalendarCurrentDate").val(updateDate);
    day = updateDate;

    if (debug)
        alert("movetocalendar " + day);
        
    CollectAndRetrieve();
}

function viewChange() {
    CollectAndRetrieve();
    loadMiniCalendar();
    ResetView();
}


var GridCalendarView = "/admin/calendar/frame/calendar.aspx?";
var ListCalendarView = "/admin/calendar/frame/calendarlist.aspx?";
var RotaCalendarView = "/admin/calendar/frame/rota.aspx?";
var WeekViewCalendarView = "/admin/calendar/frame/weekview.aspx?";

function DetailsURL(querystring) {
    
    if (debug)
        alert(querystring);

        var key = $(".calendar-view").val();

    if (key == "Grid View")
        return GridCalendarView + querystring;
    else if (key == "Rota")
        return RotaCalendarView + querystring;
    else if (key == "Week View")
        return WeekViewCalendarView + querystring;
    else
        return ListCalendarView + querystring;
}


function moveToEntry() {

    if ($('.slot-default').length == 0) {
        $("#calendar-view").animate({ scrollTop: $('#calendar-view .hour:nth-child(9)').offset().top - 400 }, 100);
    }
    else {

        $("#calendar-view").animate({ scrollTop: $('.slot-default:first').offset().top - 400 }, 100);
    }
    
     countEvents();
}

function countEvents() {
    $("#no-events-count").text($(".slot-default").length);
}
