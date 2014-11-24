<%@ Page Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true" Inherits="CRM.Admin.Setting.List" Codebehind="List.aspx.cs" %>

<asp:Content ID="ContentMain" ContentPlaceHolderID="mainContent" runat="Server">

    <div class="topContentBox">

        <div class="contentBoxTop"><h3>Settings</h3></div>

        <div class="innerContentTable">
            <p class="top"><a href="adminuser/list.aspx">Edit Admin Permissions</a></p>
            <p class="top"><a href="venues/list.aspx">Edit Venues</a></p>
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"  runat="server" />
            <!--
            <table class="sTable" style="width: 619px;">

            <asp:Repeater ID="repItems" runat="server">
                   
                <HeaderTemplate>      
                    
                    <thead>
                        <tr class="header">
                            <th style="text-align:left;width:200px;"><strong>Name</strong></th>
                            <th style="text-align:left;"><strong>Value</strong></th>
                        </tr>
                    </thead>         
                    
                </HeaderTemplate>
                <ItemTemplate>
                        
                    <tr>
                        <td style="text-align:left;text-transform:capitalize;"><%# Eval("Name").ToString().Replace("-", " ") %></td>
                        <td style="text-align:left;">
                            <ucUtil:TextBox ID="txtValue" runat="server" Width="386" Required="true" Text='<%# Eval("Value") %>' SettingName='<%# Eval("Name") %>' />
                        </td>
                    </tr>
                        
                </ItemTemplate>    
            </asp:Repeater>

            <asp:Literal ID="litNoSettings" runat="server" Visible="false">
                <tr><td>No settings are available</td></tr>
            </asp:Literal>

            </table>

            <br class="clearFix" />

            <div class="buttons">
                <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Save Settings" ImagePath="tick.png" Class="positive" />
            </div>-->

        </div>

    </div>

</asp:Content>