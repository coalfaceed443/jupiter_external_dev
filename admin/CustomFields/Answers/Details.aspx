<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.CustomFields.Answers.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Custom Fields : <%= Entity.Name %> : Answers : 
                <%if (CRM_FormFieldItem != null)
                  {%>
                Edit
                <%}
                  else
                  {%>
                Add
                <%} %>
            </h3>
        </div>
        
        <ucUtil:NavigationCustomField ID="navForm" runat="server" Section="navAnswers" />
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                HeaderText="The form could not be submitted for the following reasons:" runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            <span class="help" title="This is the option that will be shown in the control">
                                Name:</span> *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtName" Name="Name" runat="server" Width="286" Required="true" />
                    </td>
                </tr>

            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                            <span class="help" title="Active answers will display to the user filling out the form">
                                Is Active:</span> *</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" />
                    </td>
                </tr>
            </table>
            <br class="clearFix" />
            <div class="buttons">
                <%if (CRM_FormFieldItem == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Answer" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else
                  { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Changes" ImagePath="tick.png"
                    Class="positive" />
                <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Answer" ImagePath="cross.png"
                    Class="negative" />
                <%} %>
            </div>
            <div class="back">
                <a href="list.aspx?id=<%= Entity.ID %>">Back to answer list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>


</asp:Content>
