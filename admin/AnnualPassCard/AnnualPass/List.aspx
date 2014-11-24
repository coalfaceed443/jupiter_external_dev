<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.AnnualPassCard.AnnualPass.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Annual Pass Card : Annual Passes (Membership Number <%= Entity.MembershipNumber %>)</h3>
        </div>

         <div class="innerContentForm">

            <p><a href="details.aspx?id=<%= Entity.ID %>">Create a new Annual Pass</a></p>

            <ucUtil:ListView ID="lvAnnualPass" runat="server" />

         </div>

    </div>

</asp:Content>
