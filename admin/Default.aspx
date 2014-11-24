<%@ Page Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True"
    Inherits="CRM.Admin.Default" CodeBehind="Default.aspx.cs" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="mainContent" runat="Server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Dashboard</h3>
        </div>
        <div class="innerContentForm">
            <h2>
                Welcome to your admin panel</h2>
            <p>
                Please select on option from the menu above to continue.</p>
            <div class="editor-nolabel">
                <h3>
                    Overdue tasks</h3>
                <ucUtil:ListView ID="lvOverdue" runat="server" />
            </div>
            <div class="editor-nolabel">
                <h3>
                    Tasks due in the next 7 days</h3>
                <ucUtil:ListView ID="lvTasks" runat="server" />
            </div>
        </div>
    </div>
</asp:Content>
