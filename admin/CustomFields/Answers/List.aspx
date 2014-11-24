<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.CustomFields.Answers.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">


        <div class="contentBoxTop"><h3>Custom Fields : <%= Entity.Name %> : Answers</h3></div>
        
        <ucUtil:NavigationCustomField ID="navForm" runat="server" Section="navAnswers" />
        <div class="innerContentForm">

        <p class="top"><a href="details.aspx" id="lnkAdd" runat="server">Add a new Answer</a></p>       

        <br class="clearFix" />
        <ucUtil:ListView ID="ucList" runat="server" />
        
        <p>
        <a href="../list.aspx?id=<%= Entity._DataTableID %>">Back to custom fields list</a>
        </p>
        </div>

    </div>


</asp:Content>
