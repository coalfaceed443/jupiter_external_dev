<%@ Page Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" Inherits="CRM.Admin.NoPermission" Codebehind="NoPermission.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" Runat="Server">

     <div class="topContentBox" id="forms">

        <div class="contentBoxTop">
            <h3>Insufficient Privileges</h3>
        </div>

        <div class="innerContent" id="box-1">
            <h2>You cannot access this page</h2>
            <p>You do not have the correct level of security to access this page.</p>
            <p>If you believe that you should have access please consult a higher admin.</p>
            <p>Restricted Page: <asp:Literal ID="litFromURL" runat="server" /></p>
        </div>

    </div>

</asp:Content>
