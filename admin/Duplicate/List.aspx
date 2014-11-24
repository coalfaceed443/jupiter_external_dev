<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Duplicate.List" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <ucUtil:Scripts ID="ucScripts" runat="server" />

        <script type="text/javascript">

            $(document).ready(function () {
                $("a").each(function () {
                    var href = $(this).attr("href");
                    $(this).on("click", function () {
                        window.parent.location = href;
                    })
                });
            });
         
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <p>Potential duplicates:</p>
     <ucUtil:ListView ID="ucList" runat="server" />
    </div>
    </form>
</body>
</html>
