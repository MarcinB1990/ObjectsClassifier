<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="WebRole.Views.History" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <div runat="server" id="loggedOut">
        <h3>This page is only for registered users.</h3>
    </div>
    <div runat="server" id="loggedIn" visible="false">
        Empty page
    </div>
</asp:Content>
