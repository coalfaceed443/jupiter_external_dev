<%@ Control Language="C#" AutoEventWireup="true" Inherits="CRM.Controls.Forms.UserControlDateCalendar" Codebehind="DateCalendar.ascx.cs" %>

<asp:TextBox ID="txtDate" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>

<asp:ImageButton runat="Server" ID="imbDate" ImageUrl="~/_assets/images/admin/calendar.png" CssClass="cal-button" AlternateText="Click to show calendar" />
<ajaxToolkit:CalendarExtender ID="calendarDate" runat="server" TargetControlID="txtDate" PopupButtonID="imbDate" Format="dd/MM/yyyy" />

<asp:TextBox ID="txtTime" runat="server" CssClass="textbox" Visible="false" Width="35px">00:00</asp:TextBox>

<asp:RequiredFieldValidator ID="reqDate" ControlToValidate="txtDate" ErrorMessage="" runat="server" Display="None" EnableClientScript="false"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="regDate" ControlToValidate="txtDate" ErrorMessage="" ValidationExpression="^(((((0[1-9])|(1\d)|(2[0-8]))/((0[1-9])|(1[0-2])))|((31/((0[13578])|(1[02])))|((29|30)/((0[1,3-9])|(1[0-2])))))/((20[0-9][0-9]))|((((0[1-9])|(1\d)|(2[0-8]))/((0[1-9])|(1[0-2])))|((31/((0[13578])|(1[02])))|((29|30)/((0[1,3-9])|(1[0-2])))))/((19[0-9][0-9]))|(29/02/20(([02468][048])|([13579][26])))|(29/02/19(([02468][048])|([13579][26]))))$" runat="server" Display="None" EnableClientScript="false"></asp:RegularExpressionValidator>

<asp:RequiredFieldValidator ID="reqTime" ControlToValidate="txtTime" Enabled="false" ErrorMessage="" runat="server" Display="None" EnableClientScript="false"></asp:RequiredFieldValidator>
<asp:RegularExpressionValidator ID="regTime" ControlToValidate="txtTime" Enabled="false" ErrorMessage="" ValidationExpression="^((0?[1-9]|1[012])(:[0-5]\d){0,2}(\ [AP]M))$|^([01]\d|2[0-3])(:[0-5]\d){0,2}$" runat="server" Display="None" EnableClientScript="false"></asp:RegularExpressionValidator>