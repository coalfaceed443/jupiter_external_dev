<%@ Control Language="C#" AutoEventWireup="true" Inherits="Website.Controls.Admin.SharedObjects.Media.Details" Codebehind="Details.ascx.cs" %>

 <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />



            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false" HeaderText="The form could not be submitted for the following reasons:" runat="server" />
                    
            <table class="details searchTableLeft">
                <tr>
                    <td><label>Name: *</label></td>
                    <td><ucUtil:TextBox ID="txtName" Name="Name" Required="True" runat="server" /></td>
                </tr>
                <tr>
                    <td><label>Is Active: </label></td>
                    <td><asp:CheckBox ID="chkIsActive" runat="server" Checked="true" /></td>
                </tr>

            </table>

            <table class="details searchTableRight">                
                <tr>
                    <td>
                        <label>
                            <span title="Choose a media image from the image library." class="help">Media Image:</span> *</label>
                    </td>
                     <td>

                        <ucUtil:ImageUpload ID="imgUpload" runat="server" Name="Image" />
                    </td>
                </tr>
                <!--
                <tr>
                    <td><label>Video</label></td>
                    <td>
                        <asp:DropDownList ID="ddlVideo" runat="server" AppendDataBoundItems="true" DataTextField="Name" DataValueField="ID">
                            <asp:ListItem Text="None" Value="" />
                        </asp:DropDownList>
                    </td>
                </tr>
                -->
            </table>
            <br class="clearFix" />
            <br class="clearFix" />


            <ucUtil:TextEditor ID="txtShortDescription" Required="false" Name="Short Description" runat="server" Height="140" ToolbarSet="Basic"  /> 
                      
   
            
            <br style="clear: both;" />
                
            <div class="buttons">

                <asp:Panel ID="pnlAdd" runat="server">

                    <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add Media" ImagePath="tick.png" Class="positive" />
                
                </asp:Panel>
                
                <asp:Panel ID="pnlEdit" runat="server">

                    <ucUtil:Button ID="btnSubmitChanges" runat="server" ButtonText="Save Changes" ImagePath="tick.png" Class="positive" />

                    <ucUtil:Button ID="btnDelete" runat="server" ButtonText="Delete Media" ImagePath="cross.png" Class="negative"  />

                </asp:Panel>

            </div>
                
