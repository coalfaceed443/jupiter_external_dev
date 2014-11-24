<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.AnnualPassCard.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Annual Pass Cards</h3>
        </div>

         <div class="innerContentForm">

            <p><a href="details.aspx">Create a new Card</a></p>

            <ucUtil:ListView ID="lvAnnualPass" runat="server" />

         </div>

    </div>

</asp:Content>
