<%@ Control Language="C#" AutoEventWireup="True" Inherits="Website.Controls.Admin.SharedObjects.Meta.Meta" Codebehind="Meta.ascx.cs" %>


<table class="details">
    <tr>
        <td><label><span title="<div style='padding:10px;'><p>The title of the page is the most important part of meta data. Unlike the other two,<br/>the title is actually visible to users and also ranks the highest in search engines.</p><p><br/>The image below show the page title appearing in the top left corner.</p><p><br/><img src='/_assets/images/admin/help/title.jpg' alt='title'></p><p><br/>Page titles are also used for when people bookmark a page in their browser.</p><p><br/>This will be inserted on the browser tab/toolbar in the format [Page Title] | Website Name</p></div>" class="help">Page Title</span>:</label></td>
        <td><ucUtil:TextBox ID="txtTitle" runat="server" Width="767" Name="Name" /></td>
    </tr>
    <tr>
        <td><label><span title="<div style='padding:10px;'><p>Keywords are a series of words or phrases that you would like your page to rank for.<br/>These are hidden and only displayed within the code of the page so that search engines can see them.</p><p><br/>Keywords are seperated by a comma, as shown in the following example ...</p><p><i>BBC, Sport, BBC Sport, bbc.co.uk, world, uk, international, foreign, british, online, service</i></p><p><br/>It's best to keep the keywords fairly short and within 10 to 15 words/phrases.</p></div>" class="help">Keywords</span>:</label></td>
        <td><ucUtil:TextBox ID="txtKeywords" runat="server" Width="767" Name="Keywords" /></td>
    </tr>
    <tr>
        <td><label><span title="<div style='padding:10px;'><p>Similar to meta keywords, the description is also hidden.</p><p>The following shows an example ...</p><p><br/><i>The latest BBC Formula 1 news plus live video, audio, results, standings, calendar, blogs, analysis, photos and circuit guides.</i></p><p><br/>The description should be in the form of a sentence, still trying to fit as many keyword in as possible.</p></div>" class="help">Description</span>:</label></td>
        <td><ucUtil:TextBox ID="txtDescription" runat="server" Width="767" Name="Description" /></td>
    </tr>
</table>

<div class="buttons">

    <ucUtil:Button ID="btnContinue" runat="server" ButtonText="Update Meta Details" ImagePath="tick.png" Class="positive" />

</div>