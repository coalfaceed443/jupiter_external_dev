<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="Comms.aspx.cs" Inherits="CRM.admin.History.Frames.Comms" %>

<div class="innerContentTable" style="width:95%;">
            <table class="sTable" style="width:99%;">

            <asp:ListView ID="lvItems" runat="server">
                    
                <LayoutTemplate>      
                    
                    <thead>
                        <tr class="header">
                            <th style="text-align:center;width:80px;">Type</th>    
                            <th style="text-align:center;width: 120px;">Date Logged</th>    
                            <th style="width: 120px;text-align:center;">Logged By</th>                            
                            <th style="text-align:left;width:400px;"><strong>Name</strong></th>
                            <th style="text-align:center;width:100px;"><strong>View Mail list</strong></th>
                        </tr>
                    </thead>
                        
                    <div id="itemPlaceholder" runat="server" />                        
                    
                </LayoutTemplate>
                
                <ItemTemplate>
                        
                    <tr>
                 <td style="text-align:center;">
                                  <%# Eval("CRM_Communication.Type") %>             
                             </td>  
                        <td style="text-align:center;">
                                  <%# Eval("CRM_Communication.Uploaded")%>             
                             </td>  
                             <td style="text-align:center;"><%# Eval("CRM_Communication.Admin.DisplayName")%> </td>
                             
                              <td><%# Eval("CRM_Communication.Name")%> </td>
                               <td style="text-align:center;"><%# Eval("CRM_Communication.ExportListLink")%> </td>
                    </tr>
                        
                </ItemTemplate>
                    
                <EmptyDataTemplate>
                    
                    <tr><td>No communications exist for this record</td></tr>
                    
                </EmptyDataTemplate>
                    
            </asp:ListView>

            </table>
            </div>

            