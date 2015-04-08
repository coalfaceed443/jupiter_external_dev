<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Admin.Navigation.Persons" Codebehind="Persons.ascx.cs" %>

<div class="sectionMenuContainer">

    <div class="sectionMenu" id="divHolder" runat="server">

        <div class="sectionMenuItem" runat="server" id="navDetails">
            <a href='/admin/person/details.aspx?id=<%= Entity.ID %>'>Details</a>
        </div>
        
        <div class="sectionMenuItem" runat="server" id="navPersonal">
            <a href='/admin/person/personal/list.aspx?id=<%= Entity.ID %>' title="This persons personal contact details">Personal Details</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navOrganisations">
            <a href='/admin/person/organisations/list.aspx?id=<%= Entity.ID %>' title="Organisations this person belongs to and their roles">Organisations</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navSchools">
            <a href='/admin/person/schools/list.aspx?id=<%= Entity.ID %>' title="Schools this person belongs to and their roles">Schools</a>
        </div>
       
        <div class="sectionMenuItem" runat="server" id="navRelations">
            <a href='/admin/person/relation/list.aspx?id=<%= Entity.ID %>' title="Relationships for this person">Relations</a>
        </div>

<%--        <div class="sectionMenuItem" runat="server" id="navFamilies">
            <a href='/admin/person/families/list.aspx?id=<%= Entity.ID %>' title="Families this person belongs to">Families</a>
        </div>
        --%>
        <div class="sectionMenuItem" runat="server" id="navDonations">
            <a href='/admin/person/donations/list.aspx?id=<%= Entity.ID %>' title="Donations this person has made">Donations</a>
        </div>
                
        <div class="sectionMenuItem" runat="server" id="navGiftAid">
            <a href='/admin/person/gift/list.aspx?id=<%= Entity.ID %>' title="Gift aid profiles for this person">Gift Aid</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navAttendance">
            <a href='/admin/person/attendance/list.aspx?id=<%= Entity.ID %>' title="Any records of attendance for this person">Attendance</a>
        </div>

        <div class="sectionMenuItem" runat="server" id="navPasses">
            <a href='/admin/person/passes/list.aspx?id=<%= Entity.ID %>' title="Passes this person is on">Passes</a>
        </div>



    </div>

</div>