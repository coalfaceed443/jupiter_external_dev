<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Organisation.Persons.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Employee Records for <%= Entity.Name%></h3></div>
        
        <ucUtil:NavigationOrganisation ID="ucNavOrg" runat="server" Section="navPersons"  />
        <div class="innerContentForm">

        <p class="top"><a href="/admin/person/list.aspx">Search for or Add a person to create a new employee</a></p>       
                           
        <br class="clearFix" />
        
        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>

</asp:Content>
