<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.AnnualPassCard.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>Annual Pass Cards</h3>
        </div>

        <div class="innerContentForm">

            <div class="buttons">

                <p>
                    <a href="/scheduled/rebuildtempmembers.ashx" target="_blank">Resync Cardpresso</a>
                </p>
                <p><a href="details.aspx">Create a new Card</a></p>
            </div>


            <ucUtil:ListView ID="lvAnnualPass" runat="server" />

        </div>

    </div>

</asp:Content>
