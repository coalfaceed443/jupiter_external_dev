<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavigationHistory.ascx.cs" Inherits="CRM.Controls.Admin.Navigation.NavigationHistory" %>

<div id="nav-history">

    <h2>Page History <a href="#" onclick='movehistoryback()'>Hide ></a></h2>

    <asp:Repeater ID="rptHistory" runat="server">
        <ItemTemplate>
            <ul>
        
                <li>    
                    <label><%# ((DateTime)Eval("LastAccessed")).ToString("dd-MM-yyyy HH:mm") %></label>
                    <a href="<%# Eval("URL") %>"><%# Eval("OutputName")%></a>
                    <a href="<%# Eval("URL") %>"><%# Eval("ContextName")%></a>
                </li>
        
            </ul>
        </ItemTemplate>
    </asp:Repeater>
</div>