<%@ Page Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" Inherits="Website.Admin.Setting.Redirect.List" Codebehind="List.aspx.cs" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="mainContent" runat="Server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                404 Redirects</h3>
        </div>
        <div class="innerContentTable">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                HeaderText="The form could not be submitted for the following reasons:" runat="server" />
            <table class="sTable" style="width: 740px;">
                <thead>
                    <tr class="header">
                        <th style="text-align: left;">
                            <strong>Current Url</strong>
                        </th>
                        <th style="text-align: left;">
                            <strong>Redirect Url</strong>
                        </th>
                        <th style="text-align: center; width: 180px;">
                            <strong>Actions</strong>
                        </th>
                    </tr>
                </thead>
                <tr>
                    <td style="text-align: left;">
                        <ucUtil:TextBox ID="txtCurrent_New" runat="server" Width="286" />
                    </td>
                    <td style="text-align: left;">
                        <ucUtil:TextBox ID="txtRedirect_New" runat="server" Width="286" />
                    </td>
                    <td style="text-align: center;">
                        <asp:LinkButton ID="lnkNew" runat="server" OnClick="lnkNew_Click">Add</asp:LinkButton>
                    </td>
                </tr>
                <asp:Repeater ID="repItems" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td style="text-align: left;">
                                <ucUtil:TextBox ID="txtCurrent" runat="server" Width="286" Text='<%# Eval("CurrentUrl") %>' />
                            </td>
                            <td style="text-align: left;">
                                <ucUtil:TextBox ID="txtRedirect" runat="server" Width="286" Text='<%# Eval("RedirectUrl") %>' />
                            </td>
                            <td style="text-align: center;">
                                <asp:LinkButton ID="lnkUpdate" runat="server" OnClick="lnkUpdate_Click" CommandArgument='<%# Eval("ID") %>'>Update</asp:LinkButton>
                                |
                                <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click" CommandArgument='<%# Eval("ID") %>'>Remove</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Literal ID="litNoRedirects" runat="server" Visible="false">
                <tr><td colspan="3">No 404 redirects have been setup</td></tr>
                </asp:Literal>
            </table>
            <br class="clearFix" />
            <a href="../list.aspx">Back to settings</a>
        </div>
    </div>
</asp:Content>
