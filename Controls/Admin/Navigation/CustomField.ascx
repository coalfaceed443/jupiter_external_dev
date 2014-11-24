<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Admin.Navigation.CustomField" Codebehind="CustomField.ascx.cs" %>

<div class="sectionMenuContainer">

    <div class="sectionMenu" id="divHolder" runat="server">

        <div class="sectionMenuItem" runat="server" id="navDetails">
            <a href='/admin/customfields/details.aspx?id=<%= Entity.ID %>'>Field Details</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navAnswers">
            <a href='/admin/customfields/answers/list.aspx?id=<%= Entity.ID %>'>Possible Answers</a>
        </div>

    </div>

</div>