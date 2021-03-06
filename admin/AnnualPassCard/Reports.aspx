﻿<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="CRM.admin.AnnualPassCard.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


        <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Annual Pass Cards : Reports</h3>
        </div>

         <div class="innerContentForm">

             <div class="buttons">

                 <p>Active friends between <%= startDate.ToString("dd/MM/yyyy") %> and <%= endDate.ToString("dd/MM/yyyy") %></p>

                <ucUtil:Button ID="btnExportAudit" runat="server" ButtonText="Export Audit" />
             </div>

             
             <div class="buttons">

             <p><strong>Active Friends</strong></p>
             
             <p>All friends that are<br />
                not deceased<br />
                OK to send postals<br />
                 not archived <br />
                 removing relationship duplicates<br />
                 removing duplicates by full name + postcode<br />
                 not expired, or have expired within <asp:TextBox ID="txtDays" runat="server" Text="30" Width="80" /> days<br />
                 <asp:CheckBox ID="chkExhibition" runat="server" /> also include "Press, Museums, Council, Galleries, Artist, Festivals, Government"
             </p>

                 </div>

             <div class="buttons">
                <ucUtil:Button ID="btnActiveFriends" runat="server" ButtonText="Export Active Friends and Personal Friends" />
             </div>

         </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
