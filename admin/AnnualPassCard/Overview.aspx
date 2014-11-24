<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Overview.aspx.cs" Inherits="CRM.admin.AnnualPassCard.Overview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Annual Pass Overview</h3>
        </div>

         <div class="innerContentForm">


            <ucUtil:ListView ID="lvAnnualPass" runat="server" />

         </div>

    </div>


</asp:Content>
