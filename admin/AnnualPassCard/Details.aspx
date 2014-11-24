<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Details.aspx.cs" Inherits="CRM.admin.AnnualPassCard.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Annual Pass :
                <%if (Entity != null)
                  {%>
                View this Annual Pass Card
                <%}
                  else
                  {%>
                Add a new Annual Pass Card
                <%} %></h3>
        </div>

        <ucUtil:NavigationAnnualPass ID="ucNav" runat="server" Section="navDetails" />

        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />

               <asp:CustomValidator ID="cusMemberNo" runat="server" 
                onservervalidate="cusMemberNo_ServerValidate" ErrorMessage="This membership number is already in use" />

            <table class="details">
                <tr>
                    <td>
                        <label>
                            Membership Number : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtMembershipNumber" runat="server" Name="Membership Number" DataType="int" Required="true" />
                    </td>
                </tr>
            </table>

            <div class="buttons">
                <%if (Entity == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save and start adding memberships" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else
                  { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Pass" ImagePath="tick.png"
                    Class="positive" />
                <%} %>
            </div>


        </div>
    </div>
</asp:Content>
