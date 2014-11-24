<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Notes.Frames.Details" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <ucUtil:Scripts ID="ucScripts" runat="server" />
</head>
<body id="body" runat="server">

    <ucUtil:DropDownNotice ID="ddnAlert" runat="server" />
    
    <form id="form1" runat="server">
        <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div id="submenu-wrapper" class="slide-frame">
        <div class="container">
            <div class="topContentBox">
                <div class="contentBoxTop">
                    <h3>
                        Notes</h3>
                </div>
                <div class="innerContentForm">
                    <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                         runat="server" />
                    <table class="details">
                        <tr>
                            <td>
                                <label>
                                    Title : *</label>
                            </td>
                            <td>
                               <ucUtil:TextBox ID="txtTitle" runat="server" Required="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Date Created : </label>
                            </td>
                            <td>
                                <asp:Label ID="lblCreated" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    Owner : </label>
                            </td>
                            <td>
                                <asp:Label ID="lblOwner" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <br class="clearFix" />

                    <ucUtil:TextEditor ID="txtBody" runat="server" Required="true" />
                    <asp:ScriptManager ID="scrpt" runat="server" />
                    <asp:UpdatePanel ID="updFrame" runat="server">
                        <ContentTemplate>
                      
                    <br class="clearFix" />
                    <div class="buttons">
                        <%if (Entity == null)
                          { %>
                        <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Note" ImagePath="tick.png"
                            Class="positive" />
                        <%}
                          else
                          { %>
                        <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Note" ImagePath="tick.png"
                            Class="positive" />
                        <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Archive Note" ImagePath="cross.png"
                            Class="negative" />

                        <%} %>
                    </div>

                      
                        </ContentTemplate>
                    </asp:UpdatePanel>
                    <div class="back">
                        <a href='list.aspx?references=<%= Request.QueryString["references"] %>'>Back to list</a>
                    </div>
                    <br class="clearFix" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>
