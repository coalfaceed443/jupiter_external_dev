<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="CRM.admin.Merge.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">

        <div class="contentBoxTop"><h3>External records for merging</h3></div>

        <div class="innerContentForm">


        <br class="clearFix" />
        <ucUtil:ListView ID="ucList" runat="server" />

        </div>

    </div>

</asp:Content>
