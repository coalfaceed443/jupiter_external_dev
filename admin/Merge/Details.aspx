<%@ Page Title="" Language="C#" MasterPageFile="~/admin/MasterPage.master" AutoEventWireup="true"
    CodeBehind="Details.aspx.cs" Inherits="CRM.admin.Merge.Details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphHeader" runat="server">

<style type="text/css">

    .merge
    {
        padding: 5px;
        border-radius:3px;
        width:450px;
    }
    
    .fl
    {
        float:left;
    }
    
    .fr
    {
        margin-left:10px;
        float:right;
    }

    .dupes td,
    .dupes th
    {
        font-size:12px;
    }
    
    .merge h2
    {
        font-size:12px !important;
        text-align:right;
        padding: 5px 0;
        text-transform:uppercase;
    }

    .merge span.box{
        padding:20px;float:left;
        width:410px;
background: rgb(255,255,255);
border-radius: 5px;
border: 1px solid #d9d9d9;
background: -moz-linear-gradient(top, rgba(255,255,255,1) 0%, rgba(246,246,246,1) 47%, rgba(237,237,237,1) 100%);
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(255,255,255,1)), color-stop(47%,rgba(246,246,246,1)), color-stop(100%,rgba(237,237,237,1)));
background: -webkit-linear-gradient(top, rgba(255,255,255,1) 0%,rgba(246,246,246,1) 47%,rgba(237,237,237,1) 100%);
background: -o-linear-gradient(top, rgba(255,255,255,1) 0%,rgba(246,246,246,1) 47%,rgba(237,237,237,1) 100%);
background: -ms-linear-gradient(top, rgba(255,255,255,1) 0%,rgba(246,246,246,1) 47%,rgba(237,237,237,1) 100%);
background: linear-gradient(to bottom, rgba(255,255,255,1) 0%,rgba(246,246,246,1) 47%,rgba(237,237,237,1) 100%);
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#ededed',GradientType=0 );
}
         

    
    .merge [type="checkbox"]
    {
        width:22px;
        height:22px;
    }
    
    .interest-list [type="checkbox"]
    {
        float:left;
        width:22px;
        height:22px;
    }
    .interest-list td:first-child {
        width:182px !important;
    }

</style>

    <script type="text/javascript">

        $(function () {

            var recordid = $("#<%= hdnRecord.ClientID%>").val();

            $("#record-" + recordid + "").children("td").css({"border-top" : "1px solid black", "border-bottom": "1px solid black"});

            var spans = $(".searchTableLeft").find("*[data-merge]");
            
            for (var i =0; i< spans.length; i++)
            {
                var input = $(spans[i]).children();
                if (input.is(":checked")) {
                    $(".merge").find("[data-merge='" + $(spans[i]).data('merge') + "']").css("font-weight", "bold");
                }
            }
        })
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainContent" runat="server">

    <input type="hidden" value="" id="hdnRecord" runat="server" />

    <ucUtil:ConfirmationPage ID="confirmationDelete" runat="server" />
    <div class="topContentBox">
        <div class="contentBoxTop">
            <h3>Merge tool</h3>
        </div>
        <div class="innerContentForm">
            <asp:ValidationSummary ID="ValidationSummary1" CssClass="validation" EnableClientScript="false"
                runat="server" />

            
            
                <div class="buttons fr" style="width:100%;">
                <p class="fr" style="width:100%;">
          
                        <asp:LinkButton ID="lnkCreateNew" runat="server" CssClass="fl" Text="Create a new CMS record based on this website record" OnClick="lnkCreateNew_Click" />
                               <asp:LinkButton ID="lnkNextRecord" runat="server" CssClass="fr" Text="Finish with this merge and move to next record" OnClick="lnkNextRecord_Click" />
             </p>
                </div>


            <div class="fl merge">
            
            <p><strong>Received: <%= CRM.Code.Utils.Text.Text.PrettyDate(Entity.DateReceived) %> <%= Entity.DateReceived.ToString("HH:mm") %></strong></p>
            <p><strong><em>Origin: <%= Entity.OriginDescription %> (Record <%= Entity.OriginAccountID %>)</em></strong></p>


                <table class="details searchTableLeft">
                    <tr>
                        <td>
                            <label>
                                Title
                            </label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlTitle" runat="server" DataTextField="Name" DataValueField="ID" Width="190" style="min-width:205px;" /> <asp:CheckBox ID="chkTitle" runat="server" data-merge="title" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Firstname</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtFirstname" runat="server" Width="200" /> <asp:CheckBox ID="chkFirstname" runat="server" data-merge="firstname" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Lastname</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtLastname" runat="server" Width="200" /> <asp:CheckBox ID="chkLastname" runat="server" data-merge="lastname"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Email</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtEmail" runat="server" Width="200" /> <asp:CheckBox ID="chkEmail" runat="server"  data-merge="primaryemail"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Address 1</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtAddress1" runat="server" Width="200" /> <asp:CheckBox ID="chkAddress1" runat="server" data-merge="address1"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Address 2</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtAddress2" runat="server" Width="200" /> <asp:CheckBox ID="chkAddress2" runat="server"  data-merge="address2"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Address 3</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtAddress3" runat="server" Width="200" /> <asp:CheckBox ID="chkAddress3" runat="server"  data-merge="address3"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                City</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtCity" runat="server" Width="200" /> <asp:CheckBox ID="chkCity" runat="server" data-merge="town"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                County</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtCounty" runat="server" Width="200" /> <asp:CheckBox ID="chkCounty" runat="server"  data-merge="county"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Postcode</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtPostcode" runat="server" Width="200" /> <asp:CheckBox ID="chkPostcode" runat="server"  data-merge="postcode"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                Telephone</label>
                        </td>
                        <td>
                            <ucUtil:TextBox ID="txtTelephone" runat="server" Width="200" /> <asp:CheckBox ID="chkTel" runat="server"  data-merge="tel"  />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            
                            <asp:CheckBoxList ID="rptInterests" runat="server" DataTextField="Text" DataValueField="Value" RepeatLayout="Table" RepeatColumns="2" CssClass="interest-list" />
                                                         
                        </td>
                    </tr>
                    
                    <tr>     
                        <td>
                            <label>Basket</label>
                        </td>                 
                        <td>
                            <label><%= Entity.BasketContents %></label>
                        </td>     
                    </tr>
                    <tr>
                        <td colspan="2">
                            
                            <asp:Repeater ID="rptConstituent" runat="server" OnItemDataBound="rptConstituent_ItemDataBound">

                                <ItemTemplate>
                                    <p>
                                    <asp:CheckBox ID="chkOption" runat="server"  TextAlign="Right"/>
                                        <label ID="lblOption" runat="server" style="float:none;"/>
                                       </p>
                                </ItemTemplate>

                            </asp:Repeater>
                                                      
                        </td>
                    </tr>
                    <tr>         
                        <td>
                            <label>Mail Preference</label>
                        </td>              
                        <td>
                            <label><%= Entity.MailPreference %></label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>E-Mail Preference</label>
                        </td>                 
                        <td>
                            <label><%= Entity.EMailPreference %></label>
                        </td>
                    </tr>
                       
                </table>
            </div>

            
            <div class="fr merge">

                

                <h2>Top 3 Potential Matches</h2>
                <table class="sTable dupes" style="max-width:450px;">
                <asp:ListView ID="lvItems" runat="server">
                    
                <LayoutTemplate>      
                    
                    <thead>
                        <tr class="header">
                            <th style="width: 100px;">Record</th>                            
                            <th style="text-align:left;width:100px;"><strong>Compared Fields</strong>
                            <th style="text-align:center;  width:80px"></th>
                        </tr>
                    </thead>
                        
                    <div id="itemPlaceholder" runat="server" />                        
                    
                </LayoutTemplate>
                <ItemTemplate>
                   
                    <tr id="record-<%# Eval("ID") %>">
                        <td><%# Eval("Reference") %></td>
                        <td><%# Eval("Fullname") %>
                        <%# Eval("PrimaryEmail") %>
                        <%# Eval("Postcode") %></td>
                        <td><asp:LinkButton ID="lnkUpdate" runat="server" OnClick="lnkUpdate_Click" CommandArgument='<%# Eval("ID") %>' >Update preview</asp:LinkButton></td>
                    </tr>     
                   
                </ItemTemplate>
                    
                <EmptyDataTemplate>
                    
                    <tr><td>No potential duplicates to merge could be found</td></tr>
                    
                </EmptyDataTemplate>
                    
            </asp:ListView>
            </table>

                

            </div>


            <asp:Panel ID="pnlMerge" runat="server" Visible="false">
            <div class="merge fr">
            
                <h2>Current record<br />
                Before</h2>
            
                <asp:Label ID="litBefore" runat="server" CssClass="box" />
                <asp:Label ID="lblConstituentTypesBefore" runat="server" CssClass="box" />

            </div>
            <div class="merge fr">
            
                <h2>Merged record<br />
                After</h2>
                
                <asp:Label ID="litAfter" runat="server" CssClass="box" />
                <asp:Label ID="lblConstituentTypesAfter" runat="server" CssClass="box" />

                <div class="buttons fr">
                <asp:LinkButton ID="btnSubmitChanges" runat="server" OnClick="btnSubmitChanges_Click">Submit this record change</asp:LinkButton>
            </div>
            </div></asp:Panel>
        </div>
    </div>
</asp:Content>
