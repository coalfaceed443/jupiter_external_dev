<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Scripts.ascx.cs" Inherits="CRM.Controls.Admin.SharedObjects.Scripts" %>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE9" />

<link href="/_assets/images/admin/favicon.ico" rel="icon" type="image/x-icon" />

<link href="/_assets/css/admin/default.css?v=4" rel="stylesheet" type="text/css" />
<link href="/_assets/css/admin/buttons.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="/_assets/scripts/typeahead.css" />
<script src="//ajax.googleapis.com/ajax/libs/jquery/1.9.0/jquery.min.js" ></script>
<script src="/_assets/scripts/modernizr.custom.00047.js" type="text/javascript"></script>
<script src="/_assets/scripts/hogan-2.0.0.js" type="text/javascript"></script>
<script src="/_assets/scripts/typeahead.min.js?v=2" ></script>
<script type="text/javascript" src="/_assets/scripts/fancybox/jquery.mousewheel-3.0.2.pack.js"></script>
<script type="text/javascript" src="/_assets/scripts/fancybox/jquery.fancybox-1.3.1.js"></script>
<script src="/_assets/scripts/autocomplete/jquery.autocomplete.pack.js" type="text/javascript"></script>
<script type="text/javascript" src="/_assets/scripts/admin.js"></script>
<script type="text/javascript" src="/_assets/scripts/qTip.js"></script>
<script type="text/javascript" src="/_assets/scripts/jquery.cookie.js"></script>
<link rel="stylesheet" href="/_assets/scripts/stylesheets/jquery.sidr.light.css">
<link href="/_assets/scripts/autocomplete/jquery.autocomplete.css" rel="stylesheet" type="text/css" />
<link rel="stylesheet" type="text/css" href="/_assets/scripts/fancybox/jquery.fancybox-1.3.1.css" media="screen" />
<link href='https://fonts.googleapis.com/css?family=Ubuntu:700' rel='stylesheet' type='text/css'>

<link href="/_assets/scripts/jquery.mCustomScrollbar.css" rel="stylesheet" type="text/css" />
<script src="/_assets/scripts/jquery.mousewheel.min.js"></script>

<script src="/_assets/scripts/jquery.mCustomScrollbar.min.js"></script>


<% if (this.Page.Request.RawUrl.StartsWith("/admin/scanning/details"))
   { %>
        <script type="text/javascript">
            $(document).ready(function () {
                $(".sTable").find("a").each(function () {
                    $(this).attr("target", "_blank");

                });
            });
    </script>
    <%} %>
