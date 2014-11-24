<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Scan.ascx.cs" Inherits="CRM.Controls.Admin.Navigation.Scan" %>
    <div id="divHolder" runat="server">

   <div id="RHSMenu">
        <ul>
            <li id="navDetails" runat="server">
                <a href="/admin/scanning/details/basic.aspx?id=<%= Entity.ID %>">
                    Basic Details
                </a>
            </li>
            <li id="navPersonal" runat="server"> <a href="/admin/scanning/details/personal.aspx?id=<%= Entity.ID %>">Personal</a></li>
            <li id="navOrganisation" runat="server"> <a href="/admin/scanning/details/organisation.aspx?id=<%= Entity.ID %>">Organisations</a></li>
            <li id="navSchool" runat="server"> <a href="/admin/scanning/details/school.aspx?id=<%= Entity.ID %>">Schools</a></li>
            <li id="navFamily" runat="server"> <a href="/admin/scanning/details/families.aspx?id=<%= Entity.ID %>">Families</a></li>
        </ul>
    </div>
   <br class="clearFix" /> 
   </div>