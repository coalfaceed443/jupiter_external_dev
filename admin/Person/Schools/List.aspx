<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Person.Schools.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Organisation Records for <%= Entity.Fullname %></h3></div>
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navSchools"  />
        <div class="innerContentForm">

        <p class="top"><a href="details.aspx?id=<%= Entity.ID %>">Add <%= Entity.Firstname %> to a new school</a></p>       
                           
        <br class="clearFix" />
        
        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>
    </asp:Content>