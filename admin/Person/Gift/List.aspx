<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Person.Gift.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Giftaid Records : <%= Entity.Fullname %></h3></div>
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navGiftaid" />
        <div class="innerContentForm">

        <p class="top"><a href="<%= Entity.GiftRecordNewURL %>">Add new profile</a></p>       
        

        <br class="clearFix" />
        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>

</asp:Content>
