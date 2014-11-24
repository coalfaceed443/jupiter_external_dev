<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Calendar.Finance.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

    <style type="text/css">
        .entry-box
        {
            width:50px;
            text-align:center;
        }
        
        .entry-box input
        {
            text-align:center;
        }
        
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Calendar : Finance
            </h3>
        </div>
        <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navInvoice" />
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Price Agreed:</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPriceAgreed" runat="server" DataType="decimal" Required="true"
                            Width="80" Name="Price agreed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Price Type: *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPriceType" runat="server">
                            <asp:ListItem Value="1">Per Head</asp:ListItem>
                            <asp:ListItem Value="0">Flat Fee</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Mark as Invoiced</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsInvoiced" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            PO Number</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPONumber" runat="server" Name="PO Number" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Date Paid:</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtDatePaid" runat="server" Required="false" Name="Date paid" />
                    </td>
                </tr>
            </table>

            <table class="details searchTableRight">

                <tr>
                    <td>
                        <label>
                            Title: </label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlTitles" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Firstname: </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtFirstname" runat="server" Required="false"
                            Width="80" Name="Firstname" />
                    </td>
                </tr>
                <tr>
                    <td>  
                        <label>
                            Lastname: </label>                      
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtLastname" runat="server" Required="false"
                            Width="80" Name="Lastname" />
                    </td>
                </tr>                                                

                <ucUtil:Address ID="ucAddress" runat="server" />
            </table>

            <asp:Panel ID="pnlPerHead" runat="server" style="float:left;">
                <table class="details searchTableLeft" style="float:left;margin-top:20px;">
                    <tr>
                        <td><label>Add Customer</label></td>
                        <td>
                        
                            <ucUtil:AutoComplete ID="ucAddCustomer" runat="server" />
                        
                        </td>
                    </tr>
                    </table>
            <table class="sTable" style="float:left;width:470px;">
                <tr  class="header">
                    <th>Name</th>
                    <th>Price</th>
                    <th>+ Children</th>
                    <th>Children Price</th>
                    <th>
                        &nbsp;
                    </th>
                </tr>
                <asp:Repeater ID="rptCustomersPerHead" runat="server" OnItemDataBound="rptCustomersPerHead_ItemDataBound">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:Label ID="lblName" runat="server" />
                            </td>
                            <td class="entry-box">
                                <asp:TextBox ID="txtPrice" runat="server" Width="50" />
                            </td>
                            <td class="entry-box">
                                <asp:TextBox ID="txtPlusChildren" runat="server" Text="0" Width="50" />
                            </td>
                            <td class="entry-box">
                                <asp:TextBox ID="txtChildrenPrice" runat="server" Width="50" />
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
            </asp:Panel>



            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Finance" ImagePath="tick.png"
                    Class="positive" />
            </div>
            
        </div>
    </div>
    <ucUtil:LogHistory ID="ucLogHistory" runat="server" />
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" />
</asp:Content>
