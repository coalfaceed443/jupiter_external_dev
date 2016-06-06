<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="CRM.admin.Attendance.Report" %>
<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

     <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script type="text/javascript">

        google.load('visualization', '1.0', { 'packages': ['corechart', 'bar'] });


        google.setOnLoadCallback(drawGroupOverTime);
        google.setOnLoadCallback(drawAttendanceOverTime);

        function drawAttendanceOverTime()
        {

            <% var groupOverTimeEnd = dcDateTo.Value;%>
            <% var groupOverTimeLoop = dcDateFrom.Value;%>

            
            var groupData = new google.visualization.arrayToDataTable([
                ['Date', 'Quantity'],


            <% while(groupOverTimeLoop < groupOverTimeEnd)
               {%>
                ['<%= groupOverTimeLoop.ToString("dd/MM/yyyy")%>',
                     <%= db.CRM_AttendanceLogs.Where(r => r.CRM_AttendanceLogGroup.AddedTimeStamp.Date == groupOverTimeLoop.Date).Sum(s => (int?)s.Quantity) ?? 0%>
                  
                ],
                
                <% groupOverTimeLoop = groupOverTimeLoop.AddDays(1);%>
                <%}%>
            ]);


            var options = {
                'title': 'Totals over Time',
                'width': 1500,
                'height': 800,
                bars: 'vertical',
                vAxis: { format: '###,###' },
                legend: { position: 'right' },
                explorer: {
                    axis: 'vertical'
                },
                range: { start: new Date(2016, 01, 01), end: new Date(2017, 01, 01) }
            };

            var dateChart = new google.visualization.LineChart(document.getElementById('totalsOverTime'));
            dateChart.draw(groupData, options);


        }

        function drawGroupOverTime()
        {

            <% groupOverTimeEnd = dcDateTo.Value;%>
            <% groupOverTimeLoop = dcDateFrom.Value;%>


            var groupData = new google.visualization.arrayToDataTable([
                ['Date',
                    <% foreach (CRM.Code.Models.CRM_AttendancePersonType type in db.CRM_AttendancePersonTypes)
                       {%>
                   '<%= type.Name%>',
                    <%}%>],

                    

            <% while(groupOverTimeLoop < groupOverTimeEnd)
               {%>
                
                <% if (db.CRM_AttendanceLogs.Any(c => c.CRM_AttendanceLogGroup.AddedTimeStamp.Date == groupOverTimeLoop.Date))
                   {%> 
                
                ['<%= groupOverTimeLoop.ToString("dd/MM/yyyy")%>',

                    
                    <% foreach (CRM.Code.Models.CRM_AttendancePersonType type in db.CRM_AttendancePersonTypes)
                       {%>

                    <%= db.CRM_AttendanceLogs.Where(r => r.CRM_AttendanceLogGroup.AddedTimeStamp.Date == groupOverTimeLoop.Date && r.CRM_AttendancePersonTypeID == type.ID).Sum(s => (int?)s.Quantity) ?? 0%>
                   ,
                    <%}%>

            ],
           <% }%>

                <% groupOverTimeLoop = groupOverTimeLoop.AddDays(1);%>
             <% }%>

            ]);


            var options = {
                'title': 'Types over Time',
                'width': 1500,
                'height': 800,
                bars: 'vertical',
                vAxis: { format: '###,###' },
                legend: { position: 'right' },
                explorer: {
                    axis: 'vertical'
                }
            };

            var dateChart = new google.visualization.ColumnChart(document.getElementById('typesOverTime'));
            dateChart.draw(groupData, options);

        }



        $(function () {

        })



    </script>


    <style type="text/css">
        .graph {
            float: left;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <div class="topContentBox">



        <div class="innerContentForm">



            <table class="details" style="width:950px;">

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

    
    
        <div style="width:100%;">
            <div id="typesOverTime" style="width:100%;height:800px;clear:both;">

            </div>

            <div id="totalsOverTime" style="width:100%;height:600px;clear:both;">

            </div>
        </div>


</asp:Content>
