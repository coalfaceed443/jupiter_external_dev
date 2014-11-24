<%@ Page Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Admin.Calendar" Codebehind="calendar.aspx.cs" %>

<script type="text/javascript">


    function LoadWithoutCache(url, source) {
        $.ajax({
            url: url,
            cache: false,
            dataType: "html",
            success: function (data) {
                $("." + source).html(data);
                return false;
            }
        });
    }

    $(document).ready(function () {
        $(".calendar").css("opacity", "1.0");
    });



</script>

    <h3>     <a href="#" class="prev" onclick='$(".calendar").css("opacity", "0.3");$("#cal-month").css("opacity", "0.3");LoadWithoutCache("/controls/admin/calendar.aspx?action=prev&ran=" + Math.floor(Math.random()*1000), "calendar");return false;' id="cal-left">
                        <img src="/_assets/images/admin/icons/left.png" alt="left" />
                    </a><%= dataContext.month.ToString("MMMM yyyy") %>  <a href="#" class="next" onclick='$(".calendar").css("opacity", "0.3");$("#cal-month").css("opacity", "0.3");LoadWithoutCache("/controls/admin/calendar.aspx?action=next&ran=" + Math.floor(Math.random()*1000), "calendar");return false;' id="cal-right">
                        <img src="/_assets/images/admin/icons/right.png" alt="right" />
                    </a></h3>

                  
                
<table style="margin-top:20px;">
    <tr class="week">
        <td>M</td>
                      <td>T</td>
                      <td>W</td>
                      <td>T</td>
                      <td>F</td>
                      <td>S</td>
                      <td>S</td>
    </tr>
    
<% int count = 0; int instance = 0; bool skip = false; %>


 <% foreach (DateTime day in dataContext.days)
    { %>
    
                   <% if (day.Month != dataContext.month.Month)
                      {

                          if (day.DayOfWeek.ToString() == "Monday")
                          { %> <tr> <% } %>

                                 <td> </td>
                          <%} %>

       
                     <% if (day.Month == dataContext.month.Month)
                        {
                            count = 0;
                            skip = false;
            %>
                     
                       
                                <td class="<%= dataContext.hasEvents[day.Day] ? "hasevents" : "" %>">
                                <a href="#" onclick="moveCalendarTo('<%= day.ToString("dd-MM-yyyy") %>')"><%= day.Day.ToString()%></a>
                                </td>
                                                    
                        
                     
           <% } %>
                    
           
           <%  if (day.DayOfWeek.ToString() == "Sunday")
                          {
                              %>
                              
                              </tr>
                              <%
                          } %>
  <% } %>
</table>
