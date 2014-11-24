<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Fundraising.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Fundraising :
                <%if (Entity != null)
                  {%>
                Edit Donation
                <%}
                  else
                  {%>
                Add a new Donation
                <%} %></h3>
        </div>
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false" runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>Primary Contact :</label>
                    </td>
                    <td>
                        <ucUtil:AutoComplete ID="ucACPrimaryContact" runat="server" Required="true" Name="Primary Contact" />

                        <asp:Panel ID="pnlImportPrompt" runat="server" Visible="false">
                            <script type="text/javascript">
                                

                                $(document).ready(function () {
                                    if (confirm("This user HAS NOT opted into gift aid, however - do you wish to import gift aid details for this contact?")) {
                                        eval($("#<%=lnkPopulateContact.ClientID %>").attr("href"));
                                    }
                                });
                            </script>
                        </asp:Panel>
                        <asp:Panel ID="pnlImportIsGift" runat="server" Visible="false">
                            <script type="text/javascript">


                                $(document).ready(function () {
                                    eval($("#<%=lnkPopulateContact.ClientID %>").attr("href"));                                    
                                });
                            </script>
                        </asp:Panel>
                        <asp:LinkButton ID="lnkPopulateContact" runat="server" OnClick="lnkPopulateContact_Click" CausesValidation="false"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           In Kind : *</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsInKind" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Pledged Amount : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAmount" runat="server" DataType="decimal" Name="Pledged Amount" Text="0.00" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Gift Aid
                        </label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsGiftAid" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Gift Aid Firstname
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtGiftaidFirstname" runat="server" Name="Gift Aid Firstname" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Gift Aid Lastname
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtGiftaidLastname" runat="server" Name="Gift Aid Lastname" />
                    </td>
                </tr>
                <ucUtil:Address ID="ucGiftAidAddress" runat="server" />


            
            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>Is Recurring</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsRecurring" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Recurring every x Weeks
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtRecurringWeeks" runat="server" DataType="int" Text="0" Required="true" Name="Recurring every x weeks - zero for not recurring" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Duration in weeks
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtDuration" runat="server" DataType="int" Text="0" Required="true" Name="Duration" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Payment Type
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPaymentType" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Fundraising Reason
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFundReason" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>

            </table>
            
            <div style="float:right;margin-top:30px;">
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                            Add Fund split
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlFund" runat="server" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true" AutoPostBack="true" OnSelectedIndexChanged="ddlFund_SelectedIndexChanged"> 
                            <asp:ListItem Text="[Please select at least one fund to donate to]" Value="" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <asp:MultiView ID="mvSplit" runat="server" ActiveViewIndex="0">
                <asp:View ID="viewNew" runat="server">
                    
                    <table class="sTable" style="width:470px;background-color:#f6f7f5;">
                        <tr>
                            <td style="text-align:center;height:80px;vertical-align:middle;font-size:16px;"><label><strong>You must add at least one split to proceed</strong></label></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="viewSplit" runat="server">
                
            <table class="sTable" style="width:470px;">

                <asp:Repeater ID="rptSplits" runat="server" OnItemDataBound="rptSplits_ItemDataBound">
                    <ItemTemplate>

                        <tr  class="header">
                            <th colspan="4">Split <%# Container.ItemIndex + 1 %></th>
                        </tr>

                        <tr>
                            <td>
                                <label>Fund</label>
                            </td>
                            <td colspan="3">                                
                                <asp:DropDownList ID="ddlFund" runat="server" DataTextField="Name" DataValueField="ID" /> 
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Amount</label>
                            </td>
                            <td>  
                                <asp:TextBox ID="txtPrice" runat="server" Width="50" />
                            </td>
                            <td>
                                <label>Date Given</label>
                            </td>
                            <td>  
                                <ucUtil:DateCalendar ID="txtDateGiven" runat="server" Text="0" Width="50" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>Gift aid pence per pound</label>
                            </td>
                            <td class="entry-box" colspan="2">
                                <asp:TextBox ID="txtGiftAidRate" runat="server" Width="50" />
                            </td>
                            <td class="entry-box">
                                <asp:LinkButton ID="lnkRemove" runat="server" OnClick="lnkRemove_Click" CausesValidation="false">
                                    <img src="/_assets/images/admin/icons/remove.png" alt="Remove" width="20px" />
                                </asp:LinkButton>
                            
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
                </asp:View>
            </asp:MultiView>
            </div>

            <br class="clearFix" />
            <br class="clearFix" />

            

            <ucUtil:CustomFields ID="ucCustomFields" runat="server" />
            
            <br class="clearFix" />
            <br class="clearFix" />


            <div class="buttons">
                <%if (Entity == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Item" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else
                  { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Item" ImagePath="tick.png"
                    Class="positive" />

   
                <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Item" ImagePath="cross.png"
                    Class="negative" />
     
                <%} %>
            </div>
            <div class="back">
                <a href='list.aspx'>
                    Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" /> 

</asp:Content>
