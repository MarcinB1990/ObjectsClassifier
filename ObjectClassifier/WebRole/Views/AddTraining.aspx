<%@ Page Title="Add Training Set" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddTraining.aspx.cs" Inherits="WebRole.Views.AddTraining" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="loggedOut">
        <h3>This page is only for registered users.</h3>
    </div>
    <div runat="server" id="loggedIn" visible="false">
        <br />
        <fieldset>
            <legend>Complete the form to upload you own training set</legend>
            <asp:Table runat="server">
                <asp:TableRow>
                    <asp:TableCell>
                    <asp:Label runat="server">Name*:</asp:Label>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:TextBox ID="name" runat="server"></asp:TextBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:RequiredFieldValidator ID="requiredFieldValidatorName" runat="server" ControlToValidate="name" ErrorMessage="Name is a required field." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="regExpValidatorName" runat="server" ControlToValidate="name" ErrorMessage="Name has a wrong format." ForeColor="Red" Display="Dynamic" ValidationExpression="^\w{1,40}$" />
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
                        <asp:RequiredFieldValidator ID="requiredFieldValidatorNumberOfClasses" runat="server" ControlToValidate="numberOfClasses" ErrorMessage="Number of classes is a required field." ForeColor="Red" Display="Dynamic" />
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
                        <asp:RequiredFieldValidator ID="requiredFieldValidatorNumberOfAttributes" runat="server" ControlToValidate="numberOfAttributes" ErrorMessage="Number of attributes is a required field." ForeColor="Red" Display="Dynamic" />
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
                        <asp:RequiredFieldValidator ID="requiredFieldValidatorFileUploaded" runat="server" ControlToValidate="fileUploader" ErrorMessage="Select a file." ForeColor="Red" Display="Dynamic" />
                        <asp:RegularExpressionValidator ID="regExpValidatorFileUpload" runat="server" ControlToValidate="fileUploader" ErrorMessage="Invalid extension. Choose .txt file" ForeColor="Red" Display="Dynamic" ValidationExpression="^.*\.txt$" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
            <br />
        </fieldset>
        <fieldset id="accessRights" runat="server">
            <legend>Select access rights to the file:</legend>
            <asp:RadioButtonList ID="accessRightsList" runat="server">
                <asp:ListItem Selected="True">Public</asp:ListItem>
                <asp:ListItem>Private</asp:ListItem>
            </asp:RadioButtonList>
        </fieldset>
        Fields with * are required
        <asp:Button Text="Upload" runat="server" Style="margin-left: 50px" OnClick="UploadTrainingSet" />
        <asp:Label ID="error" runat="server" Visible="false" Font-Bold="true" ForeColor="Red">Error during uploading. Make sure, that everything is OK and try again.</asp:Label>
    </div>
    <div id="uploaded" runat="server" visible="false">
        <h3>File uploaded! Click <a href="AddTraining.aspx">here</a> to upload the another training set or back to <a href="MyTrainingSets.aspx">My Training Sets.</a></h3>
    </div>
</asp:Content>
