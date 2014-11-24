<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Person.Passes.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


  <div class="topContentBox">

        <div class="contentBoxTop"><h3>Person Pass Records</h3></div>
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navPasses" />

        <div class="innerContentForm">

        <p class="top"><a href="/admin/annualpasscard/list.aspx">Use the annual pass tab to add/create a new pass</a></p>               

        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>



</asp:Content>
