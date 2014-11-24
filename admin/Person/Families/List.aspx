<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Person.Families.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Family Records for <%= Entity.Fullname %></h3></div>
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navFamilies"  />
        <div class="innerContentForm">

        <p class="top"><a href="details.aspx?id=<%= Entity.ID %>">Add <%= Entity.Firstname %> to a new family</a></p>       
                           
        <br class="clearFix" />
        
        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>
    </asp:Content>