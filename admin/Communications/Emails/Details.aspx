<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Emails.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Automated Emails :
                <%if (Entity != null)
                  {%>
                Edit
                <%}
                  else
                  {%>
                Add
                <%} %></h3>
        </div>
        <div class="innerContentForm">

            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                HeaderText="The form could not be submitted for the following reasons:" runat="server" />
            
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Name: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtTitle" Name="Title" runat="server" ShowText="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Subject: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtSubject" Name="Title" runat="server"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Enabled: *</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsEnabled" runat="server" />
                    </td>
                </tr>
                
            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                            From Emails: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtFrom" Name="Title" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            To Emails: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtToEmail" Name="Title" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            CC Emails:</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtCCEmail" Name="Title" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            BCC Emails:</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtBCCEmail" Name="Title" runat="server" />
                    </td>
                </tr>
            
                <tr>
                    <td>
                        <label>
                            Send test to this email on save:</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtTestEmail" Name="Title" runat="server" />
                    </td>
                </tr>
            </table>

            <br class="clearFix" />

            <h2>Placeholders</h2>
            <table class="details searchTableLeft">
                <asp:Repeater ID="rptLeft" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <label>
                                    <%# Eval("Description") %>:</label>
                            </td>
                            <td>
                                    <label><%# Eval("Placeholder") %></label>
                            </td>
                        </tr>
                    
                    </ItemTemplate>
                </asp:Repeater>
             </table>    
            <table class="details searchTableRight">
                <asp:Repeater ID="rptRight" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <label>
                                    <%# Eval("Description") %>:</label>
                            </td>
                            <td>
                                    <label><%# Eval("Placeholder") %></label>
                            </td>
                        </tr>
                    
                    </ItemTemplate>
                </asp:Repeater>
             </table>    
    
    

            <br class="clearFix" />
                    <h2>Email contents</h2>
                    <ucUtil:TextEditor ID="txtBody" runat="server" Required="true" Name="Email Body" />
            <br class="clearFix" />
            <div class="buttons">
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Email Template / Send Test" ImagePath="tick.png"
                    Class="positive" />
            </div>
            <div class="back">
                <a href='list.aspx'>
                    Back to email list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>

</asp:Content>
