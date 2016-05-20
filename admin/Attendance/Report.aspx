<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="CRM.admin.Attendance.Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">

        <div class="innerContentForm">



            <table class="details searchTableLeft">

                <tr>
                    <td><label>Search Between Dates:</label></td>
                    <td><div class="buttons"><label><asp:LinkButton ID="btnReSearch" runat="server" OnClick="btnReSearch_Click" Text="Reload" /></label></div></td>
                </tr>

                <tr>
                    <td><label>Date From:</label></td>
                    <td><ucUtil:DateCalendar ID="dcDateFrom" runat="server" ShowTime="true" /></td>
                </tr>

                <tr>
                    <td><label>Date To:</label></td>
                    <td><ucUtil:DateCalendar ID="dcDateTo" runat="server" ShowTime="true" /></td>
                </tr>

                <tr>
                    <td>
                        <label>
                            People 
                        </label>
                    </td>
                    <td>
                        <span><%=PeopleSearch %></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Groups
                        </label>
                    </td>
                    <td>
                        <span><%=GroupsSearch %></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Average Group Size
                        </label>
                    </td>
                    <td>
                        <span><%=averageGroupSizeSearch.ToString("N1") %></span>
                    </td>
                </tr>

                <asp:Repeater ID="repSearchTypeTotals" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <label>
                                    Total <%#Eval("Key")%> :
                                </label>
                            </td>
                            <td>
                                <span><%#Eval("Value") %></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>

                <asp:Repeater ID="repEventsTotals" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td>
                                <label>
                                    Total Attendees For <%#Eval("Key")%> :
                                </label>
                            </td>
                            <td>
                                <span><%#Eval("Value") %></span>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>


            </table>

            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            People Today
                        </label>
                    </td>
                    <td>
                        <span><%=PeopleToday %></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Groups Today
                        </label>
                    </td>
                    <td>
                        <span><%=GroupsToday %></span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            Average Group Size Today
                        </label>
                    </td>
                    <td>
                        <span><%=averageGroupSizeToday.ToString("N1") %></span>
                    </td>
                </tr>
            </table>

        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
