<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Person.Attendance.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


  <div class="topContentBox">

        <div class="contentBoxTop"><h3>Person Attendance</h3></div>
        
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navAttendance" />

        <div class="innerContentForm">

        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>



</asp:Content>
