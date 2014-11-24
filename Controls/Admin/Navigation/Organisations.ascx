<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Organisations.ascx.cs" Inherits="CRM.Controls.Admin.Navigation.Organisations" %>

<div class="sectionMenuContainer">

    <div class="sectionMenu" id="divHolder" runat="server">

        <div class="sectionMenuItem" runat="server" id="navDetails">
            <a href='/admin/organisation/details.aspx?id=<%= Entity.ID %>'>Details</a>
        </div>
        
        <div class="sectionMenuItem" runat="server" id="navPersons">
            <a href='/admin/organisation/persons/list.aspx?id=<%= Entity.ID %>' title="People in this organisation">Employee List</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navSchools">
            <a href='/admin/organisation/schools/list.aspx?id=<%= Entity.ID %>' title="Schools linked to this organisation">Schools</a>
        </div>
    </div>

</div>