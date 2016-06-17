<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="CRM.admin.AnnualPassCard.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


        <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Annual Pass Cards : Reports</h3>
        </div>

         <div class="innerContentForm">

             <div class="buttons">
             <ucUtil:Button ID="btnExportAudit" runat="server" ButtonText="Export Audit" />
                 </div>
         </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
