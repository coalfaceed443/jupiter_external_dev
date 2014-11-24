<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CalendarList.aspx.cs" Inherits="CRM.admin.Calendar.Frame.Calendar" %>

<script type="text/javascript">
    $(document).ready(function () {

        $(".slot-default").hover(function () {
            $(this).toggleClass("slot-hover");
        })
        $("#current-date").text("<%= CRM.Code.Utils.Text.Text.PrettyDate(CurrentDate) %>");

        $(".innerContentForm").css("position", "relative");
        $(".innerContentForm").css("top", "0");
        $(".innerContentForm").css("width", "960");
    });
</script>


<div id="calendar-view-list">

                <asp:Repeater ID="rptCalendar" runat="server" 
                    onitemdatabound="rptCalendar_ItemDataBound">
                    <ItemTemplate>

    
                                <asp:Repeater ID="rptSlots" runat="server" onitemdatabound="rptSlots_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="list-slot">
                                            <asp:Panel ID="pnlExisting" runat="server">
                                                <div class="slot-default <%# Eval("CalendarItem.CRM_CalendarType.CSSClass") %> <%# !(bool)Eval("InsideFilter") ? "filtered" : "" %>">
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

                    </ItemTemplate>
                </asp:Repeater>
            </div>