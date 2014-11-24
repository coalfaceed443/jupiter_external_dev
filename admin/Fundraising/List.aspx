<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Fundraising.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Fundraising</h3></div>

        <div class="innerContentForm">

        <p class="top"><a href="details.aspx">Add a new Donation</a></p>       

        <br class="clearFix" />
        <ucUtil:ListView ID="ucList" runat="server" />
        </div>

    </div>


</asp:Content>
