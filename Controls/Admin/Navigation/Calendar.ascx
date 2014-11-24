<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Admin.Navigation.Calendar" Codebehind="Calendar.ascx.cs" %>

<div class="sectionMenuContainer">

    <div class="sectionMenu" id="divHolder" runat="server">

        <div class="sectionMenuItem" runat="server" id="navDetails">
            <a href='/admin/calendar/details.aspx?id=<%= Entity.ID %>'>Details</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navGroupBooking" visible="false">
            <a href="/admin" id="lnkGroupSchool" runat="server">Group/School Booking Info</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navOutreach" visible="false">
            <a href="/admin" id="lnkOutreach" runat="server">Outreach Info</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navParties" visible="false">
            <a href='/admin/calendar/parties/details.aspx?id=<%= Entity.ID %>'>Party Info</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navCDP" visible="false">
            <a href='/admin/calendar/cpd/details.aspx?id=<%= Entity.ID %>'>CPD Info</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navVenues">
            <a href='/admin/calendar/venues/default.aspx?id=<%= Entity.ID %>'>Venues</a>
        </div>
        
        <div class="sectionMenuItem" runat="server" id="navTask" visible="false">
            <a href='/admin/calendar/task/details.aspx?id=<%= Entity.ID %>'>Task Details / Participants</a>
        </div>
        
        <div class="sectionMenuItem" runat="server" id="navDuplicate">
            <a href='/admin/calendar/duplicate/details.aspx?id=<%= Entity.ID %>'>Duplicate</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navInvoice">
            <a href='/admin/calendar/finance/details.aspx?id=<%= Entity.ID %>'>Finance</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navInvites">
            <a href='/admin/calendar/invite/list.aspx?id=<%= Entity.ID %>'>Invites & Attendance</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navUserTags">
            <a href='/admin/calendar/UserTags/list.aspx?id=<%= Entity.ID %>'>User Tags</a>
        </div>

    </div>

</div>