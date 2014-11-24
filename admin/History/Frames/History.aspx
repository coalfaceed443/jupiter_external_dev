<%@ Page Language="C#" AutoEventWireup="True" CodeBehind="History.aspx.cs" Inherits="CRM.admin.History.Frames.History" %>



<script type="text/javascript">
    $(document).ready(function () {
        var jsoncells = $(".jsonout");

        $.each(jsoncells, function () {


            var newOutput = "<table>";

            var innerJson = $(this).text();

            if (innerJson.length > 1) {

                var json = $.parseJSON(innerJson);

                $.each(json, function (key, value) {

                    $.each(value, function (key, value) {
                        newOutput = FormatJson(newOutput, key, value);
                    });

                });
            }

            newOutput = newOutput + "</table>";

            $(this).html(newOutput);

        });



    });

    function FormatJson(stringbuilder, key, value) {
        return stringbuilder + "<tr><td><strong>" + key + "</strong></td><td>" + value + "</td></tr>";
    }

</script>

<div class="innerContentTable" style="width:95%;">
            <table class="sTable" style="width:99%;">

            <asp:ListView ID="lvItems" runat="server">
                    
                <LayoutTemplate>      
                    
                    <thead>
                        <tr class="header">
                            <th style="width: 80px;">Key</th>    
                            <th style="width: 80px;">Time</th>                            
                            <th style="text-align:left;width:100px;"><strong>Admin</strong></th>
                            <th style="text-align:left;width:80px;"><strong>Action</strong></th>
                            <th style="text-align:left;width:80px;"><strong>Field</strong></th>
                            <th style="text-align:center;width:500px;"><strong>Old Value</strong></th>
                            <th style="text-align:center;"><strong>New Value</strong></th>
                        </tr>
                    </thead>
                        
                    <div id="itemPlaceholder" runat="server" />                        
                    
                </LayoutTemplate>
                
                <ItemTemplate>
                        
                    <tr>
                 <td style="text-align:center;">
                                  <%# Eval("Key1") %>             
                             </td>  
                        <td style="text-align:center;">
                                  <%# Eval("ActionDateTime") %>             
                             </td>  
                             <td><%# Eval("ActionUserName") %> </td>
                             
                              <td><%# Eval("ActionType") %> </td>
                              <td><%# Eval("Property") %> </td>
                               <td class="jsonout"><%# Eval("OldValue") %> </td>
                               <td class="jsonout"><%# Eval("NewValue") %> </td>
                    </tr>
                        
                </ItemTemplate>
                    
                <EmptyDataTemplate>
                    
                    <tr><td>No history exists for this record</td></tr>
                    
                </EmptyDataTemplate>
                    
            </asp:ListView>

            </table>
            </div>

            
    <ucUtil:LogHistory ID="ucLogHistory" runat="server" />