<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.CustomFields.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Custom Fields :
                <%if (FormField != null)
                  {%>
                Edit
                <%}
                  else
                  {%>
                Add
                <%} %>
            </h3>
        </div>
        
            <ucUtil:NavigationCustomField ID="navCustomField" runat="server" Section="navDetails" />


        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                HeaderText="The form could not be submitted for the following reasons:" runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>                            
                             Data Set:</span> *</label>
                    </td>
                    <td>

                        <asp:DropDownList ID="ddlTable" runat="server" DataTextField="FriendlyName" DataValueField="ID" AutoPostBack="true" />

                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Question Type: *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlType" runat="server" CssClass="select" DataTextField="Value" DataValueField="Key" />
                    </td>
                </tr>

            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                            <span class="help" title="This is the label that will be shown in the left column">
                                Name:</span> *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtName" Name="Name" runat="server" Width="286" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <span class="help" title="If ticked the field will appear on the form for users">
                                Is Active:</span> *</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <span class="help" title="If ticked the user must fill in this question to submit the form">
                                Is Required:</span> *</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsRequired" runat="server" />
                    </td>
                </tr>
            </table>
            <br class="clearFix" />
            <div class="buttons">
                <%if (FormField == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Field" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else
                  { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Changes" ImagePath="tick.png"
                    Class="positive" />
                <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Field" ImagePath="cross.png"
                    Class="negative" />
                <%} %>
            </div>
            <div class="back">
                <a href="list.aspx" id="lnkBack" runat="server">Back to field list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>

</asp:Content>
