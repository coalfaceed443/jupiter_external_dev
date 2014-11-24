<%@ Page Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" Inherits="CRM.Admin.AdminUser.Details" Codebehind="Details.aspx.cs" %>

<asp:Content ID="cntHead" runat="server" ContentPlaceHolderID="cphHeader">
</asp:Content>
<asp:Content ID="ContentMain" ContentPlaceHolderID="mainContent" runat="Server">
    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Admins :
                <%if (Entity != null) {%>
                Edit
                <%} else {%>
                Add
                <%} %></h3>
        </div>
        
        <div class="innerContentForm">

            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />
            <table class="details">
                <tr>
                    <td>
                        <label>
                            First Name: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtFirstName" Name="First Name" runat="server" Width="286" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Surname: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtSurname" Name="Surname" runat="server" Width="286" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Email address: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtEmail" Name="Email address" runat="server" Width="286" Required="true" IsEmail="true" />
                </tr>
                <tr>
                    <td>
                        <label><span class="help" title="This is the username that the admin will login using">
                            Username:</span> *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtUsername" Name="username" runat="server" Width="286" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <span title="This will generate a new password for the artist (it is only required for new artists)" class="help">
                                Password:</span><%= Entity == null ? " *" : "" %></label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPassword" Name="Password" Width="286" runat="server" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                                Last Logged In:</label>
                    </td>
                    <td>
                        <span><%= Entity != null ? (Entity.LastLogin.HasValue ? Entity.LastLogin.Value.ToString("dd/MM/yyyy HH:mm") : "Not yet logged in") : "&nbsp;"%></span>
                    </td>
                </tr>
            </table>
            <asp:CustomValidator ID="cusValUsername" runat="server" OnServerValidate="cusValUsername_Validate" ErrorMessage="The username entered is already in use" Display="None" EnableClientScript="false" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (Entity == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Admin User" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else
                  { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Changes" ImagePath="tick.png"
                    Class="positive" />
                <%} %>
            </div>
   
            <br class="clearFix" />
        </div>
    </div>
</asp:Content>
