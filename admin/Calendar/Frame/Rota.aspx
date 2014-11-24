<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Rota.aspx.cs" Inherits="CRM.admin.Calendar.Frame.Rota" %>
<script type="text/javascript">
    $(document).ready(function () {


        $(".slot-default").hover(function () {
            $(this).toggleClass("slot-hover");
        })
    });
</script>


                                    <div class="hour">
                                            <div class="hour-label">
                                                <label>&nbsp;</label>
                                            </div>
                                    <asp:Repeater ID="rptUsersHeading" runat="server">
                                    
                                        <ItemTemplate>
                                            <div class="rota-admin">
                                            <span><%# Eval("DisplayName") %></span>
                                            </div>
                                        </ItemTemplate>
                                        

                                    </asp:Repeater>

                                    </div>
                                    <div id="calendar-view" style="float:left;">

                <asp:Repeater ID="rptCalendar" runat="server" 
                    onitemdatabound="rptCalendar_ItemDataBound">
                    <ItemTemplate>

                        <div class="hour">
                            <div class="hour-label">
                                <label><%# ((DateTime)Eval("Time")).ToString("HH:mm")%></label>
                            </div>
                            <div class="hour-slots">

                                                
                                    <asp:Repeater ID="rptUsers" runat="server" onitemdatabound="rptUsers_ItemDataBound">
                                    
                                        <ItemTemplate>
                                        <div class="slots rota-slots">
                                            <asp:Repeater ID="rptEntry" runat="server" onitemdatabound="rptEntry_ItemDataBound">
                                                <ItemTemplate>
                                               
                                                <div style="height:<%# Eval("CRM_Calendar.SlotHeight")%>px;"  class="slot-default rota-item <%# Eval("CRM_Calendar.CRM_CalendarType.CSSClass") %>">
                                                 <a href="<%# Eval("CRM_Calendar.DetailsURL") %>"><img src="/_assets/images/admin/icons/view.png" alt="view" /></a>
                                                    <p class="<%# (bool)Eval("IsCancelled") ? "cancelled" : "" %>"><%# Eval("CRM_Calendar.DisplayName") %><br />
                                                      
                                                    <%# ((DateTime)Eval("CRM_Calendar.StartDateTime")).ToString("HH:mm")%></p>
                                                
                                                                                      
                                                    <asp:Repeater ID="rptVenues" runat="server">
                                                        <ItemTemplate>
                                                            <div class="venue-item">
                                                                <strong><%# Eval("CRM_Venue.Name") %></strong><br />
                                                                <%# Eval("OutputTimeRange") %>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater> 


                                                </div>
                                          
                                                </ItemTemplate>
                                            </asp:Repeater>
                                            </div>
                                        </ItemTemplate>
                                               

                                    </asp:Repeater>

                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                </div>