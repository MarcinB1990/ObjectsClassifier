<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebRole._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Object classifier</h1>
        <p class="lead">Object classifier is a web application, that allows you to classify your own collection of objects, reuse your training sets in the future and go back to history of your clasiifications.</p>
    </div>
    <div id="orderToRegister" runat="server" visible="true">
        <h2>Register now!</h2>
        <p>
            Register your account for free in order to use the full functionality of the application.
            Without the account you will not be able to return to your results.
        </p>
        To register a new account <a runat="server" href="~/Account/Register">click here</a>
    </div>
</asp:Content>
