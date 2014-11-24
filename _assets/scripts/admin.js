
function closeError(sender) {
    $(sender).parent().slideUp();
}

function fadeLI(li) {
    var li = $(li);

    setTimeout(function () {
        $(li).fadeOut();

        var nextLi = $(li).next();
        if (nextLi != undefined) {
            fadeLI(nextLi);
        }

    }, 8000);


}


function ToggleQueryManager(sender) {

    if ($(sender).text() == 'Show Query Manager') {
        $(sender).text('Hide Query Manager');
    } else {
    $(sender).text('Show Query Manager');
    }

    $(".query-section").slideToggle(function () {

        $.cookie("7stories_query_status", $(".query-section").is(":hidden"), { path: '/' });
     

    });

}

function movehistoryback() {
    $("#nav-history").animate({ right: -320 }, 500);
}

$(document).ready(function () {

    $("#nav-history").on('click', function (event) {
        if ($(this).data("state") == "closed") {
            $(this).data("state", "open");
            $(this).animate({ right: 250 }, 500);
        }
        else {
            movehistoryback();
            $(this).data("state", "closed");
        }
    });

    var cookie = $.cookie("7stories_query_status");

    if (cookie != undefined) {
        if (cookie == "false") {
            $(".query-toggle").text('Hide Query Manager');
            $(".query-section").show();
        } else {
            $(".query-toggle").text('Show Query Manager');
            $(".query-section").hide();
        }
    }

    $(".innerContentForm").fadeIn("slow");

    $("#menu a").on("click", function () {
        $(".innerContentForm").fadeOut("slow");
    });

});


$(function () {



    $(".header th").hover(function () {
        $(this).find(".list-controls").toggleClass("show-inline");
    });

    $(".validation li").each(function () {
        $(this).prepend("<img src='/_assets/images/admin/icons/delete.png' class='validation-error'><span class='validation-prompt'>ERROR</span>");
        $(this).append("<a href='#' class='validation-close' onclick='closeError(this);'><img src='/_assets/images/admin/icons/cross.png' alt='close' /></a>");
    })

    $(".validation li:first").each(function () {
        fadeLI(this);
    });

    var tabContainers = $('div#forms > div.innerContent'); // change div#forms to your new div id (example:div#pages) if you want to use tabs on another page or div.
    tabContainers.hide().filter(':first').show();

    $('ul.switcherTabs a').click(function () {
        tabContainers.hide();
        tabContainers.filter(this.hash).show();
        $('ul.switcherTabs li').removeClass('selected');
        $(this).parent().addClass('selected');
        return false;
    }).filter(':last').click();

    // dropdown menus        
    $("#menu ul").find("li").filter(function () {
        return ($(this).children("ul").length > 0);
    }).each(function () {
        //Iterate through the sublist and set the width on each item so the main nav stays the same width but the sub nav expands
        var parent = this;
        $(this).children("ul").each(function () {
            $(this).show();
            var width = parseInt($(parent).width()) - 39;
            $(this).find('.text-link').each(function () {
                if ($(this).width() > width)
                    width = parseInt($(this).width());
            });
            $(this).find("a").width(width - 35);
            $(this).hide();
        });
        $(this).width($(this).width());
    }).hover(function () {
        $(this).children("ul").stop(true, true).slideDown('fast');
        $(this).children("a").addClass("selected");
    }, function () {
        $(this).children("ul").stop(true, true).slideUp('fast');
        $(this).children("a").removeClass("selected");
    });

});

function serverCall(url, params, callback, errorFunction) {
    jQuery.post(url, params, callback).error(errorFunction);
}

function IsAlphaNumericKeyPress(keyCode) {
    switch (keyCode) {
        case 9: //Enter
        case 16: //Shift
        case 17: //L Ctrl
        case 18: //L Alt
        case 93: //Context menu key
        case 91: //Windows key
        case 27: //Escape
        case 37: //Left arrow
        case 38: //Up arrow
        case 39: //Right arrow
        case 40: //Down arrow
        case 144: //Numlock
            return false;
        default:
            return true;
    }
}


function AttachResizeEvent(id) {
    var iframe = document.getElementById(id);
    
    if (iframe.addEventListener) {
        iframe.addEventListener('load', function () {
            ResizeIFrame(id);
        }, false);
    }
    else {
        iframe.attachEvent('onload', function () {
            ResizeIFrame(id);
        });
    }
}

var ResizeIFrame = function (id) {
    var iframe = document.getElementById(id);
    console.log(id);
    var body = iframe.contentDocument.getElementsByTagName('body')[0];
    $(iframe).css("height", $(body).css("height"));
    console.log($(body).css("height"));
};

function PollIFrame(idcollection) {
    $(idcollection).each(function (i) {
        AttachResizeEvent((idcollection)[i]);
    });


};

