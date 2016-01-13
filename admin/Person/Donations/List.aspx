<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Person.Donations.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


  <div class="topContentBox">

        <div class="contentBoxTop"><h3>Person Donations</h3></div>
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navDonations" />

        <div class="innerContentForm">

        <p class="top"><a href="/admin/fundraising/list.aspx">Use the fundraising tab to create a new donation</a></p>               

        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>



</asp:Content>
