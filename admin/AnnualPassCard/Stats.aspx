<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="CRM.admin.AnnualPassCard.Stats" %>
<%@ Import NameSpace="System.Linq" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
    <script type="text/javascript" src="https://www.google.com/jsapi"></script>

    <script type="text/javascript">

        google.load('visualization', '1.0', { 'packages': ['corechart', 'bar'] });
        google.setOnLoadCallback(drawMap);
        google.setOnLoadCallback(drawChart);
        google.setOnLoadCallback(drawTimeline);
        google.setOnLoadCallback(drawTotalMembers);
        google.setOnLoadCallback(amountPaid);
        google.setOnLoadCallback(actualIncome);
        google.setOnLoadCallback(paymentMethods);

        function drawChart() {


            <% var dataset = db.CRM_AnnualPassTypes.Where(r => !r.IsArchived).ToList();%>
            <% DateTime activeStartDate = CRM.Code.Utils.Time.UKTime.Now.AddMonths(-36); %>


            var data = new google.visualization.arrayToDataTable([

                ['Year', 
                    <% foreach (CRM.Code.Models.CRM_AnnualPassType type in db.CRM_AnnualPassTypes)
                       {%>
                   '<%= type.Name%>',
                    <%}%>

                ],



            <% while (activeStartDate <= CRM.Code.Utils.Time.UKTime.Now)
            {%>

            [ '<%= activeStartDate.ToString("MMMM yy")%>', 
                <% for (int i = 0; i < dataset.Count(); i++)
                  { %>
                       <%= dataset[i].CRM_AnnualPasses.Where(r => r.CRM_AnnualPassTypeID == dataset[i].ID && r.StartDate <= activeStartDate && r.ExpiryDate >= activeStartDate).Count()%><% if (i +1 != dataset.Count()){%>,<%}%>
                <% }%> 
                
                    <% activeStartDate = activeStartDate.AddMonths(1); %>
            ],
            <%}%>


            ]);


            

            var options = {
                'title': 'Active Memberships by Type',
                'width': 1500,
                'height': 400,
                bars: 'vertical',
                legend: { position: 'top' }

            };

            var chart = new google.charts.Bar(document.getElementById('membershipByType'));
            chart.draw(data, google.charts.Bar.convertOptions(options));
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
                'title':' Memberships total active, during the past 36 months',
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

        }

        function actualIncome() {
               <% startDate = CRM.Code.Utils.Time.UKTime.Now.AddMonths(-36); %>

            var data = google.visualization.arrayToDataTable([
              ['Month', 'Total (£)'],


            <% while (startDate <= CRM.Code.Utils.Time.UKTime.Now)
            {%>
                ['<%= startDate.ToString("MMM yy")%>', <%= db.CRM_AnnualPasses.Where(r => r.StartDate.Month == startDate.Month && r.StartDate.Year == startDate.Year).Sum(r => r.AmountPaid)%>],

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
                chart: {title: 'Member locations'},
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

    </script>


    <style type="text/css">

        .graph {
            float:left;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Members Report</h3>
        </div>


    </div>


</asp:Content>

<asp:Content ID="cntFullWidth" ContentPlaceHolderID="fullWidthContent" runat="server">

        
         <div class="innerContentForm" style="width:100%;">
             
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

             <div id="memberActualRevenue" class="graph">

                 Loading...
             </div>

             
             <div id="geoMap" class="graph" style="width:90%;height:800px;margin:0 auto;text-align:center;">

                 Loading...
             </div>
             


         </div>


</asp:Content>