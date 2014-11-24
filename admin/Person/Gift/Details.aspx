<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Person.Gift.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Person :
                <%if (CRM_FundraisingGiftProfile != null)
                  {%>
                Edit this gift aid profile for
                <%= Entity.Fullname %>
                <%}
                  else
                  {%>
                Add a new gift profile for
                <%= Entity.Fullname %>
                <%} %></h3>
        </div>
        <ucUtil:NavigationPerson ID="ucNavPerson" runat="server" Section="navGiftaid" />
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />
            <asp:CustomValidator ID="cusNextDate" runat="server" EnableClientScript="false" Text="The next payment date must be between the start and end date, or the active checkbox should be marked as not active"
                OnServerValidate="cusNextDate_ServerValidate" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Recognisable profile name
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtName" runat="server" Name="Name" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Payment Reference
                        </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPaymentReference" runat="server" Name="Payment Reference"
                            Required="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Monetary Value</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtAmountToCharge" runat="server" DataType="money" Required="true"
                            Name="Monetary Value" Text="0.00" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Is Active
                        </label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsActive" runat="server" Checked="true" />
                    </td>
                </tr>
            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                            Start Date
                        </label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtStartDate" runat="server" Name="Start Date" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            End Date
                        </label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtEndDate" runat="server" Required="true" Name="End Date" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Day of Month of payment
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlDayOfMonth" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            In the following monthly intervals, every x months
                        </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlMonthlyIntervals" runat="server" />
                    </td>
                </tr>
                <asp:Panel ID="pnlNextDate" runat="server">
                    <tr>
                        <td>
                            <label>
                                Next renewal date
                            </label>
                        </td>
                        <td>
                            <ucUtil:DateCalendar ID="txtNextDate" runat="server" />
                        </td>
                    </tr>
                </asp:Panel>
            </table>
            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (CRM_FundraisingGiftProfile == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Item" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else if (CRM_FundraisingGiftProfile.IsArchived)
                  { %>
                <ucUtil:Button ID="btnReinstate" runat="server" ButtonText="Reinstate Archived Item"
                    ImagePath="refresh_small.png" Class="negative" />
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
                <a href='<%= Entity.GiftRecordListURL %>'>Back to profile list</a>
            </div>
            <br class="clearFix" />
            
            <br class="clearFix" />
            <asp:Panel ID="pnlGiftAidLogs" runat="server">
            
            
            <h2>Raised gift aid logs</h2>

            <p><a href="/admin/fundraising/gifts/list.aspx?id=<%= Entity.ID %>">Click here to edit gift aid logs</a></p>

            <br class="clearFix" />

            <table class="sTable">
                <thead>
                    <tr class="header">
                        <td>
                            Date Raised
                        </td>
                        <td>
                            Date Confirmed
                        </td>
                        <td>
                            Payment Reference
                        </td>
                        <td>
                            Amount
                        </td>
                    </tr>
                </thead>
                <asp:Repeater ID="rptGiftLog" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%# Eval("DateCreatedOutput")%>
                            </td>
                            <td>
                                <%# Eval("DateConfirmedOutput")%>                                
                            </td>
                            <td>
                                <%# Eval("PaymentReference")%>                                     
                            </td>
                            <td>
                                &pound;<%# ((decimal)Eval("AmountCharged")).ToString("N2") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            
                <asp:Panel ID="pnlNoEntries" runat="server">
                
                    <tr>
                        <td colspan="4">
                            <p>There are no gift aid logs to display</p>
                        </td>
                    </tr>
                    
                </asp:Panel>
            
            </table>

                
            </asp:Panel>


        </div>
    </div>
    <ucUtil:LogHistory ID="ucLogHistory" runat="server" />
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" />
</asp:Content>
