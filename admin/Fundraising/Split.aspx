<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Split.aspx.cs" Inherits="CRM.admin.Fundraising.Split" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

       <div class="topContentBox">

        <div class="contentBoxTop"><h3>Fundraising Splits</h3></div>

        <div class="innerContentForm">
        <br class="clearFix" />
        <ucUtil:ListView ID="ucList" runat="server" />

            
            <div class="buttons">

                <a href="ImportGiftaid.aspx" class="neutral fancybox">Batch log gift aid claims</a>

            </div>

        </div>

    </div>


</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
