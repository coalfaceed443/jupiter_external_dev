<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="CRM.admin.Calendar.Calendar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

<style type="text/css">


body
{
    overflow-x:visible;
}

#nav-history
{
    display:none;
}


</style>

    <script type="text/javascript" src="/_assets/scripts/7S.CMS.Calendar.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3 style="width:960px;">
                Calendar 
                <span id="no-events" style="float:right;" >&nbsp;Events: <span id="no-events-count">0</span></span>
                <span style="float:right;margin-right:20px;" id="current-date"><%= CRM.Code.Utils.Text.Text.PrettyDate(CurrentDate) %></span></h3>
                
        </div>
        <div class="innerContentForm" style="width:auto;position:absolute;left:0;top:300px;">
           
            <input type="hidden" id="hdnCalendarCurrentDate" runat="server" class="hdnCalendarCurrentDate" />
            <div id="calendar-frame">
                <img src="/_assets/images/admin/icons/loading.gif" alt="loading, please wait" class="calendar-loading" />
            </div>
           
     
        </div>


    </div>
    <footer>

           <div id="calendar-key">
              
              <p><strong>Key</strong>           
                  <asp:Repeater ID="rptKey" runat="server">
                    <ItemTemplate>
                      <span class="<%# Eval("CSSClass") %>"><%# Eval("Name") %></span> 
                    </ItemTemplate>
                  </asp:Repeater>
              </p>
          
          </div>

    </footer>

        <div id="sidr">

  <h3>View</h3>

  <p>
      <select class="sidr-select calendar-view" onchange="viewChange();return false;">
        <option>Week View</option>
        <option>Grid View</option>
        <option>List View</option>
        <option>Rota</option>
      </select>
  </p>
  <div id="advanced-filters">
  <h3>Filters</h3>

  <p><input type="checkbox" id="chkHideExternalVenues" /><label for="chkHideExternalVenues">Hide external venues</label></p>
  <p><input type="checkbox" id="chkHideInternalVenues" /><label for="chkHideInternalVenues">Hide internal venues</label></p>
  <p><input type="checkbox" id="chkHideNonTagged" /><label for="chkHideNonTagged">Hide events i'm not tagged against</label></p>
  <p>
                <asp:DropDownList ID="ddlVenueFilter" runat="server" CssClass="ddlVenueFilter sidr-select" AppendDataBoundItems="true" DataTextField="Text" DataValueField="Value" onchange="viewChange();return false;">
                    <asp:ListItem Text="Filter by Venue" Value="" />
                </asp:DropDownList>
</p>

<asp:DropDownList ID="ddlPersonCalendar" runat="server" CssClass="ddlPrivacyFilter sidr-select" AppendDataBoundItems="true" DataTextField="DisplayName" DataValueField="ID" onchange="viewChange();return false;">
    <asp:ListItem Text="Change Calendar" Value="" />
</asp:DropDownList>

  <h3>Display Calendar Types</h3>
  <div id="calendarTypes">
     <p><input type="radio" id="rbShowAllTypes" name="ctypes" checked="checked" data-id="-1" /><label for="rbShowAllTypes">Show All Types</label></p>
  <p><input type="radio" id="rbShowSchoolVisits" name="ctypes" data-id="1" /><label for="rbShowSchoolVisits">Show School Visits</label></p>
    <p><input type="radio" id="rbShowGroupBookings" name="ctypes"  data-id="0" /><label for="rbShowGroupBookings">Show Group Bookings</label></p>
      <p><input type="radio" id="rbParty" name="ctypes" data-id="2" /><label for="rbParty">Show Parties</label></p>
            <p><input type="radio" id="rbExternal" name="ctypes" data-id="3" /><label for="rbExternal">Show External</label></p>
  <p><input type="radio" id="rbShowGeneric" name="ctypes"  data-id="4" /><label for="rbShowGeneric">Show Day Visit Calendar Entries</label></p>
  <p><input type="radio" id="rbShowEvents" name="ctypes" data-id="7" /><label for="rbShowEvents">Show Events</label></p>
</div>
</div>
  <div class="calendar">
  
  </div>

  <a href="#" onclick="closeSidr();">
  <img src="/_assets/images/admin/icons/tick_b.png" class="ok-icon"  />
  </a>
</div>

<script type="text/javascript">
    $(document).ready(function () {
        $('.simple-menu').sidr();

    });

    
</script>
<a class="simple-menu" href="#sidr"><img src="/_assets/images/admin/icons/time.png" /></a>
</asp:Content>
    