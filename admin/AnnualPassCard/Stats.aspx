<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="CRM.admin.AnnualPassCard.Stats" %>

<%@ Import Namespace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script type="text/javascript">

        google.load('visualization', '1.0', { 'packages': ['corechart', 'bar'] });


        google.setOnLoadCallback(drawMemberChart);
        google.setOnLoadCallback(drawTimeline);
        google.setOnLoadCallback(drawTotalMembers);
        google.setOnLoadCallback(amountPaid);
        google.setOnLoadCallback(actualIncome);
        google.setOnLoadCallback(paymentMethods);

        function drawMemberChart() {


            <% var dataset = db.CRM_AnnualPassTypes.Where(r => !r.IsArchived).ToList();%>
            <% DateTime activeStartDate = CRM.Code.Utils.Time.UKTime.Now.AddMonths(-36); %>


            var incomedata = new google.visualization.arrayToDataTable([

                ['Year',
                    <% foreach (CRM.Code.Models.CRM_AnnualPassType type in db.CRM_AnnualPassTypes)
                       {%>
                   '<%= type.Name%>',
                    <%}%>

                ],



            <% while (activeStartDate <= CRM.Code.Utils.Time.UKTime.Now)
               {%>

            ['<%= activeStartDate.ToString("MMMM yy")%>', 
                <% for (int i = 0; i < dataset.Count(); i++)
                   { %>
                       <%= dataset[i].CRM_AnnualPasses.Where(r => r.CRM_AnnualPassTypeID == dataset[i].ID && r.StartDate <= activeStartDate && r.ExpiryDate >= activeStartDate).Count()%><% if (i + 1 != dataset.Count())
                                                                                                                                                                                            {%>,<%}%>
                <% }%> 
                
                    <% activeStartDate = activeStartDate.AddMonths(1); %>
            ],
            <%}%>


            ]);




            var memberoptions = {
                'title': 'Active Memberships by Type',
                'width': 1500,
                'height': 800,
                bars: 'vertical',
                legend: { position: 'right' }

            };

            

            <% var financeDataset = db.CRM_AnnualPassTypes.Where(r => !r.IsArchived).ToList();%>
            <% DateTime financeActiveStartDate = CRM.Code.Utils.Time.UKTime.Now.AddMonths(-36); %>


            var financeData = new google.visualization.arrayToDataTable([

                ['Year',
                    <% foreach (CRM.Code.Models.CRM_AnnualPassType type in db.CRM_AnnualPassTypes)
                       {%>
                   '<%= type.Name%>',
                    <%}%>

                ],



            <% while (financeActiveStartDate <= CRM.Code.Utils.Time.UKTime.Now)
               {%>

            ['<%= financeActiveStartDate.ToString("MMMM yy")%>',
                <% for (int i = 0; i < financeDataset.Count(); i++)
                   { %>
                
                       <%= financeDataset[i].
                            CRM_AnnualPasses.Where(r => r.CRM_AnnualPassTypeID == financeDataset[i].ID &&
                                r.StartDate.Month == financeActiveStartDate.Month &&
                                r.StartDate.Year == financeActiveStartDate.Year)
                                .Sum(r => r.AmountPaid).ToString("N2").Replace(",", "") %>,
                  <%}%>

                    <% financeActiveStartDate = financeActiveStartDate.AddMonths(1); %>
            ],
            <%}%>


            ]);




            var financeoptions = {
                'title': 'Income by Membership',
                'width': 1500,
                'height': 800,
                bars: 'vertical',
                vAxis: { format: '###,###' },
                legend: { position: 'right' }

            };

            var memberchart = new google.visualization.ColumnChart(document.getElementById('membershipByType'));
            memberchart.draw(incomedata, memberoptions);

            var chart = new google.visualization.ColumnChart(document.getElementById('financeByType'));
            chart.draw(financeData, financeoptions);

        }

        

        function drawTimeline() {
            <% DateTime startDate = CRM.Code.Utils.Time.UKTime.Now.AddMonths(-36); %>


            var data = google.visualization.arrayToDataTable([
              ['Month', 'Signups', 'Renewals', 'Total'],

            <% while (startDate <= CRM.Code.Utils.Time.UKTime.Now)
               {%>

                ['<%= startDate.ToString("MMM yy")%>',
                    <%= db.CRM_AnnualPasses.Where(r => r.StartDate.Month == startDate.Month && r.StartDate.Year == startDate.Year)
    .Where(r => !r.CRM_AnnualPassCard.CRM_AnnualPasses.Any(c => c.StartDate.Year < startDate.Year)).Count()%>,
                    <%= db.CRM_AnnualPasses.Where(r => r.StartDate.Month == startDate.Month && r.StartDate.Year == startDate.Year)
     .Where(r => r.CRM_AnnualPassCard.CRM_AnnualPasses.Any(c => c.StartDate.Year < startDate.Year)).Count()%>,
                    <%= db.CRM_AnnualPasses.Where(r => r.StartDate.Month == startDate.Month && r.StartDate.Year == startDate.Year).Count()%>
                ],


                    <% startDate = startDate.AddMonths(1); %>
               <%}%>
            ]);


            var chart = new google.visualization.LineChart(document.getElementById('memberTimeline'));

            var options = {
                'title': 'Memberships singups month on month, in the past 36 months',
                'width': 600,
                'height': 300,
                is3D: true
            };
            chart.draw(data, options);

        }

        function drawTotalMembers() {

            <% startDate = CRM.Code.Utils.Time.UKTime.Now.AddMonths(-36); %>


            var data = google.visualization.arrayToDataTable([
              ['Month', 'Total Susbcribers'],

            <% while (startDate <= CRM.Code.Utils.Time.UKTime.Now)
               {%>
                ['<%= startDate.ToString("MMM yy")%>', <%= db.CRM_AnnualPasses.Where(r => r.StartDate <= startDate && r.ExpiryDate >= startDate).Count()%>],
                
                    <% startDate = startDate.AddMonths(1); %>
            <%}%>
            ]);


            var chart = new google.visualization.LineChart(document.getElementById('memberTimelineTotal'));

            var options = {
                'title': ' Memberships total active, during the past 36 months',
                'width': 550,
                'height': 300,
                is3D: true,
                legend: { position: 'bottom' }
            };
            chart.draw(data, options);

        }

        function amountPaid() {
            <% startDate = CRM.Code.Utils.Time.UKTime.Now.AddMonths(-36); %>

            var data = google.visualization.arrayToDataTable([
              ['Month', 'Total (£)'],


            <% while (startDate <= CRM.Code.Utils.Time.UKTime.Now)
               {%>
                ['<%= startDate.ToString("MMM yy")%>', <%= db.CRM_AnnualPasses.Where(r => r.StartDate <= startDate && r.ExpiryDate >= startDate).Sum(r => r.AmountPaid)%>],

                    <% startDate = startDate.AddMonths(1); %>
            <%}%>
            ]);

            var chart = new google.visualization.LineChart(document.getElementById('memberRevenueActive'));

            var options = {
                'title': 'Total value of active members over the past 36 months',
                'width': 500,
                'height': 300,
                is3D: true,
                legend: { position: 'bottom' }
            };
            chart.draw(data, options);

            
            <% startDate = CRM.Code.Utils.Time.UKTime.Now.AddYears(-6); %>
            
            var paiddata = google.visualization.arrayToDataTable([
              ['Year', 'Total (£)'],


            <% while (startDate <= CRM.Code.Utils.Time.UKTime.Now)
               {%>
                ['<%= startDate.ToString("yyyy")%>', <%= db.CRM_AnnualPasses.Where(r => r.StartDate <= startDate && r.ExpiryDate >= startDate).Sum(r => r.AmountPaid)%>],

                    <% startDate = startDate.AddYears(1); %>
            <%}%>
            ]);
            var yearchart = new google.visualization.ColumnChart(document.getElementById('memberRevenueActiveYear'));

            var paidoptions = {
                'title': 'Total value of active members over the past 6 years',
                'width': 500,
                'height': 300,
                is3D: true,
                legend: { position: 'bottom' }
            };
            yearchart.draw(paiddata, paidoptions);

        }

        function actualIncome() {
               <% startDate = CRM.Code.Utils.Time.UKTime.Now.AddMonths(-36); %>

            var data = google.visualization.arrayToDataTable([
              ['Month', 'Total (£)'],


            <% while (startDate <= CRM.Code.Utils.Time.UKTime.Now)
               {%>
                ['<%= startDate.ToString("MMM yy")%>', <%= db.CRM_AnnualPasses.Where(r => r.StartDate.Month == startDate.Month && r.StartDate.Year == startDate.Year).Sum(r => (decimal?)r.AmountPaid) ?? 0%>],

                    <% startDate = startDate.AddMonths(1); %>
            <%}%>
            ]);

            var chart = new google.visualization.LineChart(document.getElementById('memberActualRevenue'));

            var options = {
                'title': 'Actual month by month income 36 months',
                'width': 600,
                'height': 300,
                is3D: true,
                legend: { position: 'bottom' }
            };
            chart.draw(data, options);

            
               <% startDate = CRM.Code.Utils.Time.UKTime.Now.AddYears(-6); %>

            var yeardata = google.visualization.arrayToDataTable([
             ['Year', 'Total (£)'],
                         <% while (startDate <= CRM.Code.Utils.Time.UKTime.Now)
               {%>
                ['<%= startDate.ToString("yyyy")%>', <%= db.CRM_AnnualPasses.Where(r => r.StartDate.Year == startDate.Year).Sum(r => (decimal?)r.AmountPaid) ?? 0%>],

                    <% startDate = startDate.AddYears(1); %>
            <%}%>
            ]);

            var yearchart = new google.visualization.ColumnChart(document.getElementById('memberActualRevenueYear'));

            var yearoptions = {
                'title': 'Actual income over the past 6 years',
                'width': 600,
                'height': 300,
                is3D: true,
                legend: { position: 'bottom' }
            };
            yearchart.draw(yeardata, yearoptions);
        }

        function paymentMethods() {

            var data = new google.visualization.DataTable();
            data.addColumn('string', 'Payment Type');
            data.addColumn('number', 'Subscribers');

            <% var subscribers = db.CRM_AnnualPasses.Where(r => r.ExpiryDate >= DateTime.Now).ToList().Where(r => r.IsCurrent).ToList();%>

            <% var methods = CRM.Code.Utils.Enumeration.Enumeration.GetAll<CRM.Code.Helpers.PaymentType.Types>().ToList(); %>

            data.addRows([
                    <% for (int i = 0; i < methods.Count(); i++)
                       { %>
                    ['<%= methods[i].Value%>', <%= subscribers.Where(r => r.PaymentMethod == methods[i].Key).Count()%>],
                <% }%>
            ]);

            var options = {
                'title': 'Payment Methods by Active Memberships',
                'width': 500,
                'height': 300,
                is3D: true
            };

            var chart = new google.visualization.PieChart(document.getElementById('paymentMethods'));
            chart.draw(data, options);

        }


        function drawMap() {
            var data = google.visualization.arrayToDataTable([
              ['Town City', 'Subscribers'],
               <% foreach (IGrouping<string, CRM.Code.Models.CRM_Person> item in GeoMember)
                  {
         %>
               ['<%= item.Key.Replace("'", "").Replace("\"", "") %>',
                    <%= item.Count()%>],

               <%}%>
            ]);


            var options = {
                chart: { title: 'Member locations' },
                width: 900,
                legend: { position: 'none' },
                bars: 'horizontal', // Required for Material Bar Charts.
                axes: {
                    x: {
                        0: { side: 'top', label: 'Members' } // Top x-axis.
                    }
                }
            };

            var chart = new google.charts.Bar(document.getElementById('geoMap'));
            chart.draw(data, options);
        };

        $(function () {

            $("#ctl00_fullWidthContent_txtDate_txtDate").on("change", DateFilter);

            DateFilter();

        })

        function DateFilter() {


            var userDate = $("#ctl00_fullWidthContent_txtDate_txtDate").val();


            var rbMembersActive = $("#rbMembersActive");

            $.ajax({
                type: "POST",
                url: "StatsResponse.ashx",
                data: { type: "bydate", date: userDate },
                dataType: "json",
                success: function (response) {
                    console.log("success");
                    console.log(response);

                    $("#spnMembersOnDate").text(response.ActiveOnDate);
                    $("#spnJoinedOnDate").text(response.JoinedOnDate);;

                }
            });
        }


    </script>


    <style type="text/css">
        .graph {
            float: left;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>Members Report</h3>
        </div>


    </div>


</asp:Content>

<asp:Content ID="cntFullWidth" ContentPlaceHolderID="fullWidthContent" runat="server">


    <div class="innerContentForm" style="width: 100%;">


        <div id="financeByType" class="graph">
            Loading...

        </div>


        <div id="membershipByType" class="graph">
            Loading...

        </div>

        <div id="paymentMethods" class="graph">
            Loading...
        </div>

        <div id="memberTimeline" class="graph">
            Loading...
        </div>

        <div id="memberTimelineTotal" class="graph">
            Loading...
        </div>

        <div id="memberRevenueActive" class="graph">
            Loading...
        </div>

        
        <div id="memberRevenueActiveYear" class="graph">
            Loading...
        </div>

        <div id="memberActualRevenue" class="graph">
            Loading...
        </div>
        
        <div id="memberActualRevenueYear" class="graph">
            Loading...
        </div>

        <div id="memberByDate" class="graph" style="width:500px;">
            <h5 style="text-align: center; margin-top: 20px;">By Date</h5>
            <ucUtil:DateCalendar ID="txtDate" runat="server" />
            <label><span id="spnMembersOnDate"></span> Members active on date</label>
            <label><span id="spnJoinedOnDate"></span> Members joined on date</label>

        </div>


    </div>


</asp:Content>
