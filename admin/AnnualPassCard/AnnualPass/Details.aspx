<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True"
    CodeBehind="Details.aspx.cs" Inherits="CRM.admin.AnnualPassCard.AnnualPass.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">


<script type="text/javascript" src="/_assets/scripts/moment.js"></script>

<% if (CRM_AnnualPass == null)
   { %>
<script type="text/javascript">


    $(function () {
        $("#<%= txtStartDate.ClientID %>").on("change", function () {
            var date = $(this).val();

            var addYear = moment(date, "DD-MM-YYYY").add(1, 'years').calendar();

            $("#<%= txtExpiryDate.ClientID %>").val(moment(addYear).format("DD/MM/YYYY"));
        });
    });
    
</script>
<%} %>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Annual Pass :
                <%if (Entity != null)
                  {%>
                View this Annual Pass - Membership no. <%= Entity.MembershipNumber %>
                <%}
                  else
                  {%>
                Add a new Annual Pass
                <%} %></h3>
        </div>

        <ucUtil:NavigationAnnualPass ID="ucNav" runat="server" Section="navPasses" />

        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />

            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Pass Type : *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPassType" runat="server" DataTextField="NameandPrice" DataValueField="ID" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Primary Contact : *</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucACPrimaryContact" runat="server" Required="true" Name="Primary Contact" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Discount Applied :</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtDiscountApplied" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Amount Paid : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAmountPaid" runat="server" Name="Amount Paid" Required="true" Width="80" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Payment Type: 
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPaymentType" runat="server" DataTextField="Value" DataValueField="Key" AppendDataBoundItems="true">
                            
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Offer: 
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlOffer" runat="server" DataTextField="Text" DataValueField="Value" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select Offer" Value="" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>

            <table class="details searchTableRight">

                <tr>
                    <td>
                        <label>
                            Start Date : *</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtStartDate" runat="server" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Expiry Date : *</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtExpiryDate" runat="server" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            <span class="help" title="Annual passes can be set as pending until verified">Is Pending</span> : *</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsPending" runat="server" />
                    </td>
                </tr>

            </table>
            
            <br class="clearFix" />
            <div class="buttons">
                <%if (CRM_AnnualPass == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save and start adding members" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else
                  { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Pass" ImagePath="tick.png"
                    Class="positive" />
   
                <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Item" ImagePath="cross.png"
                    Class="negative" />
                <%} %>
            </div>

            <br class="clearFix" />  
            <br class="clearFix" />

            <% if (CRM_AnnualPass != null)
               { %>
            <table class="details">

                <tr>
                    <td>
                        <label>
                            Add a new person to pass :
                        </label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucACNewPerson" runat="server" />

                    </td>

                </tr>

            </table> 
            <% } %>
            <ucUtil:ListView ID="lvPersons" runat="server" />

        </div>
    </div>

    <ucUtil:LogNotes ID="ucNotes" runat="server" />

</asp:Content>
