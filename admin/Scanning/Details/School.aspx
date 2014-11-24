<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="School.aspx.cs" Inherits="CRM.admin.Scanning.Details.School" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ucUtil:Scripts ID="ucScripts" runat="server" />


</head>
<body id="ajaxtab">
    <form id="form1" runat="server">
    <br class="clearFix" /> 
    <div id="LHSDetails">

    <ucUtil:ListView ID="lvSchool" runat="server" />

    </div>

    <ucUtil:NavigationScan ID="ucNavScan" runat="server" section="navSchool" />
    
    </form>
</body>
</html>
