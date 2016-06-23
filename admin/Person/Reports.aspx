<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="CRM.admin.Person.Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">


    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>Persons : Reports</h3>
        </div>

        <div class="innerContentForm">

            <div class="buttons">

                <h2>Exhibition Postal</h2>
              
                    <ul>
                        <li>All contacts that are no annual passholders</li>
                        <li>Removing relationship duplicates</li>
                        <li>Who are OK to be mailed</li>
                        <li>Within the constituents "PersonalFriend, Press, Museums, Council,
                            Galleries, Artist, Festivals, Government, Volunteer" </li>
                        <li>Who aren't archived</li>
                    </ul>
                    
                <p>
                    <ucUtil:Button ID="btnExportExhibitionPostal" runat="server" ButtonText="Export Exhibitions Postal Mail Out" />
                </p>
                <h2>Learning Contacts</h2>
               
                    <ul>
                        <li>All contacts who aren't archived</li>
                        <li>Within the constituents "CourseAttendee, Schools, Freelance Teacher, Educational"</li>
                    </ul>

                <p>
                    <ucUtil:Button ID="btnExportLearningContacts" runat="server" ButtonText="Export Learning Contacts" />
                </p>
            </div>
        </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="fullWidthContent" runat="server">
</asp:Content>
