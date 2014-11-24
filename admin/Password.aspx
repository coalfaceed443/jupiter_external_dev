<%@ Page Language="C#" AutoEventWireup="true" Inherits="CRM.Admin.Password" Codebehind="Password.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="txtBox" runat="server" />
        <asp:Button ID="btnCalculate" runat="server" OnClick="btnCalculate_Click" Text="Calculate" />
        <asp:Label ID="lblPassword" runat="server" />
    </div>
    </form>
</body>
</html>
