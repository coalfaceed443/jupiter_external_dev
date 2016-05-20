<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="AddAttendance.aspx.cs" Inherits="CRM.admin.Attendance.AddAttendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">

        <p class="top">Please add the quantities for each type of person</p>
        <p class="top"><span id="spnPeopleToday" runat="server">0</span> People Added Today</p>

        <div class="innerContentForm">
            <table class="details searchTableLeft">
                <asp:Repeater ID="repTypes" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <label>
                                    <%#Eval("Name") %> :
                                </label>
                            </td>
                            <td>
                                <input type="text" name='<%#Eval("Name") %>' id="txtQuantity" runat="server" value="0" />
                            </td>
                        </tr>
                        <input type="hidden" id="hdn_typeID" runat="server" value='<%#Eval("ID") %>' />
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>Date & Time Override (leave blank to use the current time)</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="dcDateOverride" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>Link to event</label>
                    </td>
                    <td>
                        <asp:DropDownList id="ddlEvents" runat="server" DataTextField="Value" DataValueField="Key" />
                    </td>
                </tr>
            </table>

            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <ucUtil:Button ID="btnSubmit" OnClick="btnSubmit_OnClick" runat="server" ButtonText="Save & Add Next" ImagePath="tick.png"
                    Class="positive" />
            </div>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
