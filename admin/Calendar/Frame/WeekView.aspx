<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WeekView.aspx.cs" Inherits="CRM.admin.Calendar.Frame.WeekView" %>


<script type="text/javascript">

    $(document).ready(function () {


        $("#current-date").text("<%= CRM.Code.Utils.Text.Text.PrettyDate(CurrentDate) %>");
        $("#no-events-count").text($(".calendar-week-day-event").length);

        $(".calendar-week-day-event").hover(function () {

            $(this).children(".venues").toggle();


        });
    });
</script>

<div id="calendar-view">

    <asp:Repeater ID="rptDays" runat="server" 
        onitemdatabound="rptDays_ItemDataBound">
        <ItemTemplate>
            <div class="calendar-week-dayrow"> <!-- Fixed height -->
                <div class="calendar-week-day"> <!-- Fixed Width -->
                    <strong><%# CRM.Code.Utils.Text.Text.ToPrettyDay((DateTime)Container.DataItem)%></strong><br />
                    <%# ((DateTime)Container.DataItem).ToString("ddd") %>
                    <br />
                                    <a href="/admin/calendar/details.aspx?slot=<%# ((DateTime)Container.DataItem).ToString("dd/MM/yyyy HH:mm") %>"><img src="/_assets/images/admin/icons/new.png" alt="add new" title="add a new calendar item" /></a>
                

                </div>
                <asp:Repeater ID="rptEvents" runat="server" onitemdatabound="rptEvents_ItemDataBound">
                    <ItemTemplate>
                          <div class="calendar-week-day-event <%# Eval("CRM_CalendarType.CSSClass") %>  <%# (bool)Eval("IsCancelled") ? "cancelled" : "" %>">       <!-- Square tbh could be whatever width since for monitor, maybe %? -->                     
                          <a href="<%# Eval("DetailsURL") %>"></a><img src="/_assets/images/admin/icons/view.png" alt="view" />
                                                    <img src="/_assets/images/admin/icons/time.png" id="imgVenues" runat="server" class="venue-icon" />
                            <%# ((DateTime)Eval("StartDateTime")).ToString("HH:mm")  + " - " + ((DateTime)Eval("EndDateTime")).ToString("HH:mm")%><br />
                            <%# Eval("DisplayName")%>

                            <div class="venues">
                            <asp:Repeater ID="rptVenues" runat="server">
                                                        <ItemTemplate>
                                                            <div class="venue-item">
                                                                <strong><%# Eval("CRM_Venue.Name") %></strong><br />
                                                                <%# Eval("OutputTimeRange") %>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater> 
                                                    </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>

        </ItemTemplate>
    </asp:Repeater>

</div>