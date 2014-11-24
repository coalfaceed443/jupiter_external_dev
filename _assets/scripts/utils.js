/*overstate*/
$(document).ready(function () {
    $(".menuoverstate").each(function (i) {
        $(this).createOverstate();
    });

    // creates the functionality to open a link in a new window using target="_new?{params}" //

    $('a[target^=_new]').bind("click", function () {
        var href = $(this).attr("href") || "";
        var name = $(this).attr("name") || "";
        var specs = $(this).attr("target").split("?")[1] || "";

        window.open(href, name, specs);

        return false;
    });

    // creates the functionality to create simple share links - sharing an url and title/via //
    // example anchor - <a href="facebook:http://www.google.com" rel="This is google" target="_new?width=655,height=380">Facebook</a> //

    $('a[href^=facebook\\:]').each(function () {
        var url = encodeURIComponent($(this).attr("href").splitByFirst(":")[1]),
            title = formatDataToUrl($(this).attr("rel"), "t"),
            fbUrl = "http://www.facebook.com/sharer.php";
        $(this).attr("href", fbUrl + "?u=" + url + title);
    });

    $('a[href^=twitter\\:]').each(function () {
        var url = encodeURIComponent($(this).attr("href").splitByFirst(":")[1]),
            via = formatDataToUrl($(this).attr("rel"), "via"),
            twUrl = "https://twitter.com/share";
        $(this).attr("href", twUrl + "?url=" + url + via);
    });

    $('a[href^=gplus\\:]').each(function () {
        var url = encodeURIComponent($(this).attr("href").splitByFirst(":")[1]),
            gpUrl = "http://plus.google.com/share";
        $(this).attr("href", gpUrl + "?u=" + url);
    });

    $('a[href^=pinit\\:]').each(function () {
        var url = encodeURIComponent($(this).attr("href").splitByFirst(":")[1]),
            desc = formatDataToUrl($(this).attr("rel"), "description"),
            media = formatDataToUrl($(this).attr("media"), "mdeia"),
            pnUrl = "http://pinterest.com/pin/create/button/";
        $(this).attr("href", pnUrl + "?url=" + url + desc + media);
    });

    function formatDataToUrl(data, variableName) {
        if (data !== undefined && data !== null)
            return "&" + variableName + "=" + encodeURIComponent(data);
        else
            return "";
    }

    $('img[data-crop]').each(function () {
        var size = $(this).data("crop").split(",");
        $(this).cropImage(size[0].size[1]);
    });

    $('a').autoScrollTo();
});

function post_to_url(path, params, method) {
    method = method || "post"; // Set method to post by default, if not specified.

    var form = document.createElement("form");
    form.setAttribute("method", method);
    form.setAttribute("action", path);

    for (var key in params) {
        var hiddenField = document.createElement("input");
        hiddenField.setAttribute("type", "hidden");
        hiddenField.setAttribute("name", key);
        hiddenField.setAttribute("value", params[key]);

        form.appendChild(hiddenField);
    }

    document.body.appendChild(form);    // Needed for cross browser
    form.submit();
}

String.prototype.replaceAll = function (replace, with_this) {
    return this.replace(new RegExp(replace, 'g'), with_this);
}

function characterLimit(sender, maxChars, onlyNumbers, e) {
    var keynum
    var keychar
    var numcheck

    if (window.event) // IE
    {
        keynum = e.keyCode
    }
    else if (e.which) // Netscape/Firefox/Opera
    {
        keynum = e.which
    }

    keychar = String.fromCharCode(keynum);
    if (sender.value.length == maxChars) {
        return !keynum || keynum == 8;
    }

    if (onlyNumbers) {
        numcheck = /\d/
        return numcheck.test(keychar) || !keynum || keynum == 8;
    }

    return true;
}

function LimitToLines($element, limit, variableText, fixedEnd) {
    if ($element.length > 0) {
        var lineHeight = parseFloat($element.css("line-height").replace("px", ""));
        var fontSize = $element.css("font-size").replace("px", "");

        if (isNaN(lineHeight)) {
            var x = ($.browser.msie) ? 1.2 : 1.25;
            lineHeight = parseFloat(fontSize * x);
        }
        else if ($.browser.msie && $.browser.version == "7.0" && fontSize > lineHeight) {
            lineHeight = fontSize * 1.2;
        }

        var startText = variableText

        $element.html(variableText + fixedEnd);

        while ($element.innerHeight() > limit * lineHeight + 1) {
            variableText = variableText.substring(0, variableText.length - 2);

            if (variableText == "") {
                $element.html(startText + fixedEnd);
                break;
            }

            $element.html(variableText + "... " + fixedEnd);
        }
    }
}

jQuery.fn.cropImage = function(width, height) {
    var div = "<div style='overflow:hidden;width:" + width + "px;height:" + height + "px;display:inline-block;*display:inline;' />";
    this.resizeToOverflow(width, height);
    this.wrap(div);
}

// cookies //

function getCookie(name) {
    var cookies = document.cookie.split(";");

    for (var i = 0; i < cookies.length; i++) {
        if (cookies[i].toLowerCase().indexOf(name.toLowerCase()) != -1) {
            return cookies[i].split("=")[1];
        }
    }

    return "";
}

function addCookie(name, value) {
    document.cookie = name + "=" + value + ";path=/";
}

// query string //

window.location.queryString = function (name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);

    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

jQuery.fn.resizeToOverflow = function (width, height, size, fn) {
    fn = fn || null;
    size = size || "100%";
    this.load(function () {
        var percentage = parseInt(width) / $(this).width(),
        newHeight = $(this).height() * percentage;
        if (newHeight >= parseInt(height))
            $(this).css({ "width": size, "height": "auto" });
        else
            $(this).css({ "width": "auto", "height": size });
        fn.call();
    });
}

// jQuery Extensions //

/**************************************************
* a simplified way to populate elements using
* ajax requests
* 
* it loads content from the href attribute 
* into an element declared in the rel attribute
* -- optional tag :- saTarget - selects only the
*    content from a specified element
*
* Init example :- $('a[href][rel]').click(function(){ $(this).simpleAjax(); return false; });
*
**************************************************/

jQuery.fn.simpleAjax = function (fn) {
    fn = fn || null;
    var $this = this;
    var $elm = this.attr("saTarget") || null;
    jQuery.ajax({
        url: $this.attr("href"),
        success: function (data, textStatus, jqXHR) {
            if ($elm !== null)
                data = jQuery($elm, data).html();
            jQuery($this.attr("rel")).html(data);
            fn.call();
        }
    });
}

/*****************************************
*
*   Preloads an image and creates an
*   overstate.
*
*   Iterates down the dom until it comes
*   to an image.
*
*   overstate is image with _o appended
*
*****************************************/

jQuery.fn.createOverstate = function () {
    var $img = ($(this).is('img')) ? $(this) : $(this).children("img").first();

    if ($img.attr("src") != undefined) {
        preload_image = new Image();
        var normalExt = ".jpg";
        var overExt = "_o.jpg";

        if ($img.attr("src").indexOf(".png") != -1) {
            normalExt = ".png";
            overExt = "_o.png";
        } else if ($img.attr("src").indexOf(".gif") != -1) {
            normalExt = ".gif";
            overExt = "_o.gif";
        } else if ($img.attr("src").indexOf(".jpeg") != -1) {
            normalExt = ".jpeg";
            overExt = "_o.jpeg";
        } else if ($img.attr("src").indexOf(".bmp") != -1) {
            normalExt = ".bmp";
            overExt = "_o.bmp";
        }

        preload_image.src = $img.attr("src").replace(normalExt, overExt);

        $(this).hover(
                    function () {
                        if ($img.attr("src").indexOf(overExt) == -1) {
                            $img.attr("src", $img.attr("src").replace(normalExt, overExt));
                        }
                    },
                    function () {
                        $img.attr("src", $img.attr("src").replace(overExt, normalExt));
                    }
                );
    }
}

jQuery.fn.exists = function () {
    return this.length > 0;
}

jQuery.fn.contentupdate = function (fn) {
    this.bind("contentupdate", fn);
}

jQuery.fn.allImagesLoaded = function (fn) {
    var allImages = $("img", this),
                    imagesLoaded = 0,
                    imageCount = allImages.length;

    if (imageCount == 0) fn.call();

    allImages.each(function () {
        var i = new Image();
        i.onload = function () { imagesLoaded++; if (imagesLoaded == imageCount) fn.call(); }
        i.src = $(this).attr("src");
    });
}

var contentchangeInterval;
jQuery.event.special.contentupdate = {
    setup: function () {
        var self = this,
                $this = $(this),
                $oContent = $this.text();
        contentchangeInterval = setInterval(function () {
            if ($oContent != $this.text()) {
                $oContent = $this.text();
                jQuery.event.handle.call(self, { type: 'contentupdate' });
            }
        }, 100);
    },
    teardown: function () {
        clearInterval(contentchangeInterval);
    }
}


/*************************************
*
*    Automaticially scrolls to
*    the top of an element when  
*    a the elements ID is provided
*    in the href preceeded by a #
*
*************************************/

jQuery.fn.autoScrollTo = function (speed) {
    speed = speed || 500;
    this.bind("click.scrollTo", function () {
        var href = $(this).attr("href");
        if (href !== undefined && href.startsWith("#") && href.length > 1 && $(href).exists()) {
            $('html,body').animate({
                scrollTop: $(href).offset().top
            }, speed);
            return false;
        }
    });
}

// Useful functions //

String.prototype.startsWith = function (str, caseSensitive) {
    caseSensitive = caseSensitive || false;
    var sensitive = caseSensitive ? "g" : "gi";
    return this.match(new RegExp("^" + str, sensitive)) != null;
}

String.prototype.endsWith = function (str, caseSensitive) {
    caseSensitive = caseSensitive || false;
    var sensitive = caseSensitive ? "g" : "gi";
    return this.match(str + "$") != null;
}

String.prototype.splitByFirst = function (char) {
    var entities = this.split(char);
    return [entities.shift(), entities.join(char)];
}

String.prototype.contains = function (s) {
    return this.indexOf(s) !== -1;
}

// follwing code would be more effiecent and cleaner if Object is used however prototyping Objects breaks jQuery!

Function.prototype.waitUntilTrue = function (timeout, fn) {
    var self = this,
        waitUntilTrueInterval,
        count = 0;

    waitUntilTrueInterval = setInterval(function () {
        count++;
        if (self.value || count * 10 >= timeout) {
            fn.call();
            clearInterval(waitUntilTrueInterval);
        }
    }, 10);
}

var isMobile = {
    Android: function () {
        return navigator.userAgent.match(/Android/i) ? true : false;
    },
    BlackBerry: function () {
        return navigator.userAgent.match(/BlackBerry/i) ? true : false;
    },
    iOS: function () {
        return navigator.userAgent.match(/iPhone|iPad|iPod/i) ? true : false;
    },
    iPad: function () {
        return navigator.userAgent.match(/iPad/i) ? true : false;
    },
    Windows: function () {
        return navigator.userAgent.match(/IEMobile/i) ? true : false;
    },
    any: function () {
        return (isMobile.Android() || isMobile.BlackBerry() || isMobile.iOS() || isMobile.Windows());
    }
};

// Detect file input support
var isFileInputSupported = (function () {
    // Handle devices which falsely report support
    if (navigator.userAgent.match(/(Android (1.0|1.1|1.5|1.6|2.0|2.1))|(Windows Phone (OS 7|8.0)|(XBLWP)|(ZuneWP)|(w(eb)?OSBrowser)|(webOS)|Pre\/1.2|Kindle\/(1.0|2.0|2.5|3.0))/)) {
        return false;
    }
    // Create test element
    var el = document.createElement("input");
    el.type = "file";
    return !el.disabled;
})();
