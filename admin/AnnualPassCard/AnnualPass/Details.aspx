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
            
            <h2 style="border-bottom: solid 1px #ccc; margin: 0 0 10px 0; padding: 0 0 10px 0;">Annual Pass Details</h2>

            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false" runat="server" />

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
                
                <%if (CRM_AnnualPass == null) { %>
                    <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save and start adding members" ImagePath="tick.png" Class="positive" />
                <% } else { %>
                    <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Pass" ImagePath="tick.png" Class="positive" />
                    <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Item" ImagePath="cross.png" Class="negative" />
                <% } %>

            </div>
            
            <% if (CRM_AnnualPass != null && CRM_AnnualPass.CRM_AnnualPassType.Type == 2) { %>
            
                <br class="clearFix" /><br class="clearFix" />

                <h2 style="border-bottom: solid 1px #ccc; margin: 0 0 10px 0; padding: 20px 0 10px 0;">Group Members</h2>
            
                <p><strong><%= CRM_AnnualPass.CRM_AnnualPassCorporates.Count %> of <%= CRM_AnnualPass.CRM_AnnualPassType.GroupSize %> group members added</strong></p>
            
                <% if (CRM_AnnualPass.CRM_AnnualPassCorporates.Count < CRM_AnnualPass.CRM_AnnualPassType.GroupSize) { %>

                    <table class="details">

                        <tr>
                            <td style="width: 200px;"><label>Add a new person to group :</label></td>
                            <td><ucUtil:TextBox ID="txtGroupName" runat="server" Name="Name" /></td>
                            <td><ucUtil:Button ID="btnAddGroup" runat="server" ButtonText="Add Member" ImagePath="tick.png" Class="positive" /></td>
                        </tr>

                    </table> 
            
                <% } %>
            
                <table class="sTable" style="width: 950px">
                    <thead>
                        <tr class="header">
                            <th style="text-align: left;"><strong>Name</strong></th>
                            <th style="text-align: left;"></th>
                        </tr>
                    </thead>
                    <tbody>
                    
                        <% foreach (var crmAnnualPassCorporate in CRM_AnnualPass.CRM_AnnualPassCorporates) { %>
                   
                            <tr>
                                <td style="text-align: left;"><%= crmAnnualPassCorporate.Name %></td>
                                <td style="text-align: center; width: 120px;"><a href="/admin/annualpasscard/annualpass/details.aspx?id=<%= CRM_AnnualPass.CRM_AnnualPassCardID %>&amp;pid=<%= CRM_AnnualPass.ID %>&action=delete&cid=<%= crmAnnualPassCorporate.ID %>">Delete</a></td>
                            </tr>
                
                        <% } %>
                        
                        <% if (CRM_AnnualPass.CRM_AnnualPassCorporates.Count == 0) { %>
                        
                            <tr>
                                <td style="text-align: left;" colspan="2">There are currently no group members</td>
                            </tr>

                        <% } %>

                    </tbody>
                </table>

            <% } %>

            <br class="clearFix" /><br class="clearFix" />
            
            <h2 style="border-bottom: solid 1px #ccc; margin: 0 0 10px 0; padding: 20px 0 10px 0;">People</h2>

            <% if (CRM_AnnualPass != null)
               { %>
            <table class="details">

                <tr>
                    <td style="width: 200px;"><label>Add a new person to pass :</label></td>
                    <td><ucUtil:AutoComplete ID="ucACNewPerson" runat="server" /></td>
                </tr>

            </table> 
            <% } %>

            <ucUtil:ListView ID="lvPersons" runat="server" />

        </div>
    </div>
    
    <ucUtil:LogNotes ID="ucNotes" runat="server" />

</asp:Content>
