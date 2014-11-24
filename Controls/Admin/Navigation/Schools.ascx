<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Schools.ascx.cs" Inherits="CRM.Controls.Admin.Navigation.Schools" %>

<div class="sectionMenuContainer">

    <div class="sectionMenu" id="divHolder" runat="server">

        <div class="sectionMenuItem" runat="server" id="navDetails">
            <a href='/admin/school/details.aspx?id=<%= Entity.ID %>'>Details</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navSchools">
            <a href='/admin/school/organisations/list.aspx?id=<%= Entity.ID %>' title="Organisations linked to this school">Organisations</a>
        </div>
    </div>

</div>