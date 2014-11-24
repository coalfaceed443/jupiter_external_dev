<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Notes.Frames.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <ucUtil:Scripts ID="ucScripts" runat="server" />

    <script type="text/javascript">

        $(window).load(function () {

            var unreadPosts = 0;
            $(".ReadStatus").each(function () {
                var id = $(this).val();
                var hdnField = $(this);


                $.ajax({
                    url: "GetStatus.ashx?id=" + id,
                    type: "POST",
                    dataType: "json",
                    success: function (data) {
                        console.log(data);
                        var anchor = $(hdnField).prev().html(data.Message);

                        if (data.IsRead == false) {
                            unreadPosts++;
                            $(hdnField).parent().parent().css("background-color", "#F3FCFF");
                            parent.updateCounter(unreadPosts);
                        }
                    }
                });

            });

        });

    </script>

</head>
<body id="body">
    <form id="form1" runat="server">
        <div id="submenu-wrapper" class="slide-frame">

        <div class="container">
 <div class="topContentBox">

        <div class="contentBoxTop"><h3>Notes</h3></div>

        <div class="innerContentForm">

        <p class="top"><a href="details.aspx?references=<%= Request.QueryString["references"] %>">Add new note</a></p>       
        
        <br class="clearFix" />
            <ucUtil:ListView ID="ucList" runat="server" />
        </div>

    </div>
    </div>
    </div>
    </form>
</body>
</html>
