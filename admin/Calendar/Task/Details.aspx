<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Calendar.Task.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Task :
                <%if (Task != null)
                  {%>
                Edit this task record
                <%}
                  else
                  {%>
                Add details for this task
                <%} %></h3>
        </div>        
        
                <ucUtil:NavigationCalendar ID="ucNavCal" runat="server" Section="navTask"  />

        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                 runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                           Name : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtName" runat="server" Name="Name" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Due date : *</label>
                    </td>
                    <td>
                        <ucUtil:DateCalendar ID="txtDueDate" runat="server" Name="Due Date" Required="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                           Owner :</label>
                    </td>
                    <td>
                        <asp:Label ID="lblOwner" runat="server" />
                    </td>
                </tr>
               </table>
               <table class="details searchTableRight">
                    <tr>
                        <td>
                            <label>
                               Is Cancelled : *</label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsCancelled" runat="server" />
                        </td>
                    </tr>          
                    <tr>
                        <td>
                            <label>
                               Is Completed : *</label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsCompleted" runat="server" />
                        </td>
                    </tr>                               
                    <tr>
                        <td>
                            <label>
                               Is Public : *</label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsPublic" runat="server" />
                        </td>
                    </tr>   
               </table>
               <div class="editor-nolabel">
                    <ucUtil:TextEditor ID="txtDescription" runat="server" Name="Description" Required="true" />
               </div>
            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (Task == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save and start adding participants" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else
                  { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Task" ImagePath="tick.png"
                    Class="positive" />
                <%} %>
            </div>

            <br class="clearFix" />
            <br class="clearFix" />

            <asp:Panel ID="pnlAddParticipant" runat="server" Visible="false">
                <table class="details">
                    <tr>
                        <td>
                            <label>Add Participant</label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAddParticipant" runat="server" 
                                AppendDataBoundItems="true" DataTextField="DisplayName" DataValueField="ID" 
                                AutoPostBack="true" 
                                onselectedindexchanged="ddlAddParticipant_SelectedIndexChanged">
                                <asp:ListItem Text="[Please Select]" Value="" />
                            </asp:DropDownList>
                        </td>
                    </tr>

                </table>
            
                   <br class="clearFix" />
            <ucUtil:ListView ID="ucListView" runat="server" />
            </asp:Panel>
            <br class="clearFix" />
        </div>
    </div>
        <ucUtil:LogHistory ID="ucLogHistory" runat="server" />   
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" />   
</asp:Content>
