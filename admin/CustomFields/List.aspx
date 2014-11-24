<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="List.aspx.cs" Inherits="CRM.admin.CustomFields.List" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Custom Fields</h3></div>

        <div class="innerContentForm">

        <p class="top"><a href="details.aspx" id="lnkAdd" runat="server">Add a new Custom Field to the <%= ddlTable.SelectedItem.Text %> Table</a></p>       

                <table class="details">
            <tr>
                <td>
                    <label>
                        Data Set
                    </label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTable" runat="server" DataTextField="FriendlyName" 
                        DataValueField="ID" AutoPostBack="true" AppendDataBoundItems="true" 
                        onselectedindexchanged="ddlTable_SelectedIndexChanged">
                        <asp:ListItem Text="Select a Table" Value="" />
                    </asp:DropDownList>
                </td>
            </tr>
        </table>

        <br class="clearFix" />
        <ucUtil:ListView ID="ucList" runat="server" />
        </div>

    </div>


</asp:Content>
