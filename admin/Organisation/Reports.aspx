<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="CRM.admin.Organisation.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>Persons : Reports</h3>
        </div>

        <div class="innerContentForm">

            <div class="buttons">

                <h2>Organisation Contacts</h2>
               
                    <ul>
                        <li>All contacts who aren't archived, along with their organisations</li>                        
                    </ul>

                <p>
                    <ucUtil:Button ID="btnExportOrgContacts" runat="server" ButtonText="Export Contacts" />
                </p>   
                
            </div>
        </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
