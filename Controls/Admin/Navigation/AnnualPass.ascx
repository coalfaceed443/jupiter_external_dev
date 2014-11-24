<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Admin.Navigation.AnnualPass" Codebehind="AnnualPass.ascx.cs" %>

<div class="sectionMenuContainer">

    <div class="sectionMenu" id="divHolder" runat="server">

        <div class="sectionMenuItem" runat="server" id="navDetails">
            <a href='/admin/annualpasscard/details.aspx?id=<%= Entity.ID %>'>Card Details</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navPasses">
            <a href='/admin/annualpasscard/annualpass/list.aspx?id=<%= Entity.ID %>'>Annual Passes</a>
        </div>

    </div>

</div>