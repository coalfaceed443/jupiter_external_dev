<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="CRM.admin.Person.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>Persons : Reports</h3>
        </div>

        <div class="innerContentForm">

            <div class="buttons">
                <ucUtil:Button ID="btnExportExhibitionPostal" runat="server" ButtonText="Export Exhibitions Postal Mail Out" />
            </div>
        </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
