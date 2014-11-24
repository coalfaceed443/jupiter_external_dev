<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true"
    CodeBehind="List.aspx.cs" Inherits="CRM.admin.Setting.AdminUser.BespokePermissions.List" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

<script type="text/javascript">
    $(document).ready(function () {
        $("#master-checkall").on("click", function () {
            CheckAll($(this));
        });

        $(".chkall").on("click", function () {
            CheckRAED($(this));
        });
    });

    function CheckRAED(sender) {
        $(sender).parent().parent().filter(":has(:checkbox)").find("input[type='checkbox']").each(function () {
            $(this).prop('checked', $(sender).is(":checked"));
        });
    }

    function CheckAll(sender) {

        $(".innerContentForm input[type='checkbox']").each(function () {
            console.log($(this).is(":checked"));

            if (!$(this).hasClass("chkall")) {
                $(this).prop('checked', $(sender).is(":checked"));            
            }

        });
    }

</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>
                Bespoke Permissions :
                <%if (Entity != null)
                  {%>
                Edit
                <%}
                  else
                  {%>
                Add
                <%} %></h3>
              
        </div>
        <div class="innerContentForm">

              <h2 style="float:right;"><%= Entity.DisplayName %></h2>
            <p>
                <input type="checkbox" id="master-checkall" /><label for="master-checkall">Toggle All Permissions</label></p>


           <div class="buttons" style="margin-bottom:20px;">

                            <ucUtil:Button ID="btnSubmitChangesTop" runat="server" ButtonText="Save Changes" ImagePath="tick.png"
                    Class="positive" />

            </div>
            

            <br class="clearFix" />

            <table class="details">
                <tr>
                    <td>
                        <label>
                            Name
                        </label>    
                    </td>
                    <td>
                        <asp:TextBox ID="txtName" runat="server" CssClass="textbox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <label>
                            URL
                        </label>    
                    </td>
                    <td>
                        <asp:TextBox ID="txtURL" runat="server" CssClass="textbox"  />                        
                    </td>
                </tr>
            </table>


           <div class="buttons" style="margin-bottom:20px;">

                            <ucUtil:Button ID="btnSubmit" runat="server" ButtonText="Add URL" ImagePath="add.png"
                    Class="positive" />

            </div>
            
            <br class="clearFix" />



            <table class="sTable">
                <asp:Repeater ID="rptItems" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr class="header">
                                <th style="text-align: left;">
                                    &nbsp;
                                </th>
                                <th style="text-align: left;">
                                    <strong>Name</strong>
                                </th>
                                <th style="text-align: left;">
                                    <strong>URL</strong>
                                </th>
                                <th style="text-align: center;">
                                    <strong>Read</strong>
                                </th>
                                <th style="text-align: center;">
                                    <strong>Add</strong>
                                </th>
                                <th style="text-align: center;">
                                    <strong>Edit</strong>
                                </th>
                                <th style="text-align: center;">
                                    <strong>Delete</strong>
                                </th>
                            </tr>
                        </thead>
                        <div id="itemPlaceholder" runat="server" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="text-align: center;">
                                <input type="hidden" id="hdnPage" runat="server" value='<%# Eval("ID") %>' />
                                <input class="chkall" type="checkbox" />
                            </td>
                            <td style="text-align: left;">
                                <%#Eval("BespokeName")%>
                            </td>
                            <td style="text-align: left;">
                                <%#Eval("BespokeURL")%>
                            </td>
                            <td style="text-align: center;">
                                <asp:CheckBox ID="chkRead" runat="server" Checked='<%# Eval("IsRead") %>' />
                            </td>
                            <td style="text-align: center;">
                                <asp:CheckBox ID="chkAdd" runat="server" Checked='<%# Eval("IsAdd") %>' Enabled='<%# (byte)Eval("BespokeType") == (byte)CRM.Code.Models.CRM_SystemAccess.PageTypes.list ? false : true %>' />
                            </td>
                            <td style="text-align: center;">
                                <asp:CheckBox ID="chkEdit" runat="server" Checked='<%# Eval("IsWrite") %>' Enabled='<%# (byte)Eval("BespokeType") == (byte)CRM.Code.Models.CRM_SystemAccess.PageTypes.list ? false : true %>' />
                            </td>
                            <td style="text-align: center;">
                                <asp:CheckBox ID="chkDelete" runat="server" Checked='<%# Eval("IsDelete") %>' Enabled='<%# (byte)Eval("BespokeType") == (byte)CRM.Code.Models.CRM_SystemAccess.PageTypes.list ? false : true %>' />
                            </td>
                        </tr>
                    </ItemTemplate>

                </asp:Repeater>
            </table>

            

           <div class="buttons" style="margin-top:20px;">

                            <ucUtil:Button ID="btnSubmitChangesBottom" runat="server" ButtonText="Save Changes" ImagePath="tick.png"
                    Class="positive" />

            </div>

            <div class="back">
                <a href="../list.aspx">Back to admin user list</a>
            </div>
            

        </div>
    </div>
</asp:Content>
