<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScanResultFrame.aspx.cs" Inherits="CRM.admin.Scanning.ScanResultFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title></title>
    
    <style type="text/css">
         html
         {
             overflow:hidden;
         }
    </style>
       
    <ucUtil:Scripts ID="ucScripts" runat="server" />


</head>
<body>
    <form id="form1" runat="server">
    <div>

            <table class="sTable" style="border-collapse:collapse">
                <thead>
                    <tr class="header">
                        <th><strong>Details</strong></th>
                        <th><strong>Pass Type</strong></th>
                        <th style="width:100px;"><strong>Expiry</strong></th>
                        <th style="text-align: left;"><strong>Name</strong></th>
                        <th><strong>Reason For Visit</strong></th>
                        <th style="text-align: center;"><strong>Check In</strong></th>
                    </tr>
                </thead>
           
              <asp:ListView ID="lvItems" runat="server" OnItemDataBound="lvItems_ItemBound">
                    <LayoutTemplate>
                   
                        <div id="itemPlaceholder" runat="server" />
                
                    </LayoutTemplate>
                    <ItemTemplate>
                     <tr>
                        <td style="text-align: left;">
                            <a href="#" onclick="parent.ShowBasicDetails(<%# Eval("Parent_CRM_Person.ID") %>)">View</a>
                        </td>                        

                        <td style="text-align: center;">
                            <%# Eval("CRM_AnnualPass.TypeOfPass")%>
                        </td>     


                        <td style="text-align: center;">
                            <%# ((DateTime)Eval("CRM_AnnualPass.ExpiryDate")).ToString("dd/MM/yyyy HH:mm")%>
                        </td>     


                        <td style="text-align: left;">
                            <%# Eval("Parent_CRM_Person.Fullname")%>
                        </td>                                             
                        <td style="text-align: left;">
                            <asp:DropDownList ID="ddlReasonForVisit" runat="server" AppendDataBoundItems="true" DataTextField="Name" 
                            DataValueField="ID" AutoPostBack="true" OnSelectedIndexChanged="ddlReasonForVisit_SelectedIndexChanged">
                                <asp:ListItem Text="[Please Select]" Value="" />
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlAttending" runat="server" DataTextField="DisplayName" DataValueField="ID" AppendDataBoundItems="true">
                                <asp:ListItem Text="Please Select" Value="" />
                            </asp:DropDownList>
                        </td>                        
                        <td style="text-align: center;">
                            <asp:CheckBox ID="chkCheckIn" runat="server" AutoPostBack="true" OnCheckedChanged="chkCheckIn_CheckChanged" />
                        </td>                     
                            </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <tr>
                            <td colspan="20">
                                There are no active passes or people on this card.
                            </td>
                        </tr>
                    </EmptyDataTemplate>
                </asp:ListView>
                
                 </table>


    </div>
    </form>
</body>
</html>
