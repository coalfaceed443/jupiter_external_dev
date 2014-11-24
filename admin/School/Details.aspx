<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="True" CodeBehind="Details.aspx.cs" Inherits="CRM.admin.School.Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">



</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                School :
                <%if (Entity != null)
                  {%>
                Edit
                <%}
                  else
                  {%>
                Add
                <%} %></h3>
        </div>


        <ucUtil:NavigationSchool ID="ucNavSchool" runat="server" Section="navDetails"  />

        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"  runat="server" />
            <table class="details searchTableLeft">
                <tr>
                    <td>
                        <label>
                            Name : *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtOrganisationName" runat="server" Required="true" Name="Name" />
                    </td>
                </tr>            
                <tr>
                    <td>
                        <label>
                            School Type : *</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlSchoolType" runat="server" DataTextField="Text" DataValueField="Value" />
                    </td>
                </tr>        
                <tr>
                    <td>
                        <label>
                            LA :</label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlLEA" runat="server" DataTextField="Name" DataValueField="ID" AppendDataBoundItems="true">
                            <asp:ListItem Text="N/A" Value="" />
                        </asp:DropDownList>
                    </td>
                </tr>    
                <tr>
                    <td>
                        <label>
                            Is SEN :</label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIsSEN" runat="server" />
                    </td>
                </tr>   
                <tr>
                    <td>
                        <label>
                            SEN Info : </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtSENFrequency" runat="server" Required="false" />
                    </td>
                </tr>          
                <tr>
                    <td>
                        <label>Key Stages</label>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="chkKeyStages" runat="server" DataTextField="Text" 
                            DataValueField="Value" RepeatLayout="Table" CssClass="checkboxlist" 
                            ondatabound="chkKeyStages_DataBound"  />
                    </td>
                </tr>
                                                                 
            </table>

            <table class="details searchTableRight">
                <tr>
                    <td>
                        <label>
                            Email: </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtEmail" runat="server" IsEmail="true" />
                    </td>
                </tr>     
                <tr>
                    <td>
                        <label>
                            Phone: </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPhone" runat="server" Required="false" />
                    </td>
                </tr>     
                <tr>
                    <td>
                        <label>
                            Approx. Pupils: *</label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtPupils" runat="server" Required="true" Name="Approximate pupils" DataType="int" Text="0" />
                    </td>
                </tr>              
                <tr>
                    <td>
                        <label>
                            Sage Account No.: </label>
                    </td>
                    <td>
                        <ucUtil:TextBox ID="txtSage" runat="server" />
                    </td>
                </tr>                

                <ucUtil:Address ID="ucAddress" runat="server" />
            </table>

                        <ucUtil:CustomFields ID="ucCustomFields" runat="server" />



            <br class="clearFix" />
            <br class="clearFix" />
            <div class="buttons">
                <%if (Entity == null)
                  { %>
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Item" ImagePath="tick.png"
                    Class="positive" />
                <%}
                  else if (Entity.IsArchived)
                  { %>

                <ucUtil:Button ID="btnReinstate" runat="server" ButtonText="Reinstate Archived Item" ImagePath="refresh_small.png"
                    Class="negative" />
                  <%}
                    else
                    { %>
                <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Item" ImagePath="tick.png"
                    Class="positive" />

   
                <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Item" ImagePath="cross.png"
                    Class="negative" />
     
                <%} %>
            </div>
            <div class="back">
                <a href='list.aspx'>
                    Back to list</a>
            </div>
            <br class="clearFix" />
        </div>
    </div>
    
        <ucUtil:LogHistory ID="ucLogHistory" runat="server" />  
    <ucUtil:LogNotes ID="ucLogNotes" runat="server" Visible="false" /> 

    <div id="duplicates">
        
    </div>

</asp:Content>
