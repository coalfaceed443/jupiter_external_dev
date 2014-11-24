<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Calendar.Clashes.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Calendar : Clashes</h3>
        </div>

         <div class="innerContentForm">

            <ucUtil:ListView ID="ucClash" runat="server" />

         </div>

    </div>

</asp:Content>
