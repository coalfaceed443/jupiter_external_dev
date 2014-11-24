<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="ScanLog.aspx.cs" Inherits="CRM.admin.Scanning.Frame.ScanLog" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ucUtil:Scripts ID="ucScripts" runat="server" />
    <script type="text/javascript">
        $(document).ready(function () {

            $("#updatefreq").change(function () {
                var freq = $("#updatefreq").val();
                $.cookie('7StoriesFreqTimer', freq);
            })

            var freqTime = $.cookie('7StoriesFreqTimer');
            var time = 10000;

            $("#updatefreq").val(freqTime);

            if (freqTime == undefined) {
                freqTime = time;
            }

            setTimeout(function () {
                window.location = window.location;
            }, freqTime);


        });



    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div style="padding:10px;" class="innerContentForm">
        <h2>Seven Stories Event Log</h2>

        <select id="updatefreq">
        <option label="Update every minute">60000</option>
            <option label="Update every 10 seconds">10000</option>
                        <option label="Update every 5 seconds">1000</option>
            <option label="Update every 3 seconds">1000</option>


        </select>
        <p>Last updated <%= CRM.Code.Utils.Time.UKTime.Now %></p>
        <ucUtil:ListView ID="lvLog" runat="server" Width="780" />
    </div>
    </form>
</body>
</html>
