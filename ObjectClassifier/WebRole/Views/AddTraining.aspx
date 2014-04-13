<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddTraining.aspx.cs" Inherits="WebRole.Views.AddTraining" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="loggedOut">
        <h3>This page is only for registered users.</h3>
    </div>
    <div runat="server" id="loggedIn" visible="false">
        <h3>Complete the form to upload you own training set</h3>
        <asp:Table runat="server">
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server">Name*:</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="name" runat="server"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell>
                        <asp:RequiredFieldValidator id="requiredFieldValidatorName" runat="server" ControlToValidate="name" ErrorMessage="Name is a required field." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="regExpValidatorName" runat="server" ControlToValidate="name" ErrorMessage="Name has a wrong format." ForeColor="Red" Display="Dynamic" ValidationExpression="^\s{1,40}$" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server">Number of classes*:</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="numberOfClasses" runat="server"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell>
                        <asp:RequiredFieldValidator id="requiredFieldValidatorNumberOfClasses" runat="server" ControlToValidate="numberOfClasses" ErrorMessage="Number of classes is a required field." ForeColor="Red" Display="Dynamic"/>
                        <asp:RegularExpressionValidator ID="regExpValidatorNumberOfClasses" runat="server" ControlToValidate="numberOfClasses" ErrorMessage="Nummer of classes should be in the range 1-999." ForeColor="Red" Display="Dynamic" ValidationExpression="^[1-9]\d{0,2}$" />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server">Number of attributes*:</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="numberOfAttributes" runat="server"></asp:TextBox>
                </asp:TableCell>
                <asp:TableCell>
                        <asp:RequiredFieldValidator id="requiredFieldValidatorNumberOfAttributes" runat="server" ControlToValidate="numberOfAttributes" ErrorMessage="Number of attributes is a required field." ForeColor="Red" Display="Dynamic"/>
                        <asp:RegularExpressionValidator ID="regExpValidatorNumberOfAttributes" runat="server" ControlToValidate="numberOfAttributes" ErrorMessage="Nummer of attributes should be in the range 1-999." ForeColor="Red" Display="Dynamic" ValidationExpression="^[1-9]\d{0,2}$" />           
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server">Comment:</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:TextBox ID="comment" runat="server"></asp:TextBox>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell>
                    <asp:Label runat="server">File*:</asp:Label>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:FileUpload ID="fileUploader" runat="server"></asp:FileUpload>
                </asp:TableCell>
                <asp:TableCell>
                        <asp:RequiredFieldValidator id="requiredFieldValidatorFileUploaded" runat="server" ControlToValidate="fileUploader" ErrorMessage="Select a file." ForeColor="Red" Display="Dynamic"/>
                        <asp:RegularExpressionValidator ID="regExpValidatorFileUpload" runat="server" ControlToValidate="fileUploader" ErrorMessage="Invalid extension. Choose .txt file" ForeColor="Red" Display="Dynamic" ValidationExpression="^.*\.txt$" />           
                </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        Fields with * are required
        <asp:Button Text="Upload" runat="server" Style="margin-left: 50px"/>
    </div>
</asp:Content>
