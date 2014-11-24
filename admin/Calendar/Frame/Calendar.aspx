<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Calendar.aspx.cs" Inherits="CRM.admin.Calendar.Frame.Calendar" %>

<script type="text/javascript">
    $(document).ready(function () {


    $(".slot-default").hover(function () {
        $(this).toggleClass("slot-hover");
    })
        $("#current-date").text("<%= CRM.Code.Utils.Text.Text.PrettyDate(CurrentDate) %>");
    });
</script>

<div id="calendar-view">

                <asp:Repeater ID="rptCalendar" runat="server" 
                    onitemdatabound="rptCalendar_ItemDataBound">
                    <ItemTemplate>

                        <div class="hour">
                            <div class="hour-label">
                                <label><%# ((DateTime)Eval("Hour")).ToString("HH:mm")%></label>
                            </div>
                            <div class="hour-slots">

                                <asp:Repeater ID="rptSlots" runat="server" onitemdatabound="rptSlots_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="slot">
                                            <asp:Panel ID="pnlExisting" runat="server">
                                                <div style="height:<%# Eval("SlotHeight")%>px;margin-top:<%# Eval("CalendarItem.SlotAdjustmentMargin") %>px" class="slot-default <%# Eval("CalendarItem.CRM_CalendarType.CSSClass") %> <%# !(bool)Eval("InsideFilter") ? "filtered" : "" %>">
                                                    <a href="<%# Eval("CalendarItem.DetailsURL") %>"></a>
                                                    <img src="/_assets/images/admin/icons/view.png" alt="view" />
                                                    <p class="<%# (Eval("CalendarItem") != null ? ((bool)Eval("CalendarItem.IsCancelled") ? "cancelled" : "") : "") %>"><%# Eval("CalendarItem.DisplayName")%><br />
                                                    <%# Eval("CalendarItem") != null ? ((DateTime)Eval("CalendarItem.StartDateTime")).ToString("HH:mm")  + " - " + ((DateTime)Eval("CalendarItem.EndDateTime")).ToString("HH:mm") : ""%></p>
                                                
                                                    <asp:Repeater ID="rptVenues" runat="server">
                                                        <ItemTemplate>
                                                            <div class="venue-item">
                                                                <strong><%# Eval("CRM_Venue.Name") %></strong><br />
                                                                <%# Eval("OutputTimeRange") %>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater> 
                                                
                                                </div>

                       

                                            </asp:Panel>
                                            
                                            <asp:Panel ID="pnlAdd" runat="server">
                                            <a href="<%# Eval("NewDetailsURL") %>" class="slot-new">
                                                
                                            </a>
                                            </asp:Panel>
                                        </div>                                        
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                   
                        </div>

                    </ItemTemplate>
                </asp:Repeater>
            </div>