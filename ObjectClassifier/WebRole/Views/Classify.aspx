<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Classify.aspx.cs" Inherits="WebRole.Views.Classify" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Complete the form to classify your set</h3>
    <div style="border:solid;border-width:1px;border-radius:10px 10px; padding:10px 10px 10px 10px">
        <fieldset>
        <legend>Choose the training set</legend>
            <asp:RadioButtonList ID="radioNewOrOldTrainingSet" runat="server" Visible="false">
                <asp:ListItem Text="Use new training set" Value="NW" Selected="True" />
                <asp:ListItem Text="Choose training set from MyTrainingSets" Value="CFM" />
            </asp:RadioButtonList>
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
                <asp:Label ID="error" runat="server" Visible="false" Font-Bold="true" ForeColor="Red">Error during uploading. Make sure, that everything is OK and try again.</asp:Label>
            <asp:GridView ID="myTrainingSetsView" runat="server" AutoGenerateColumns="false" Visible="true">
            <Columns> 
                <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat">
                    <ItemTemplate>
                        <asp:RadioButton runat="server" id="radioButton" onclick="SelectOne(this, 'myTrainingSetsView')" VALUE="rdo" />
                        <asp:HiddenField ID="HiddenField" runat="server" Value = '<%#Eval("TrainingSetId")%>' />
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat"/>
                <asp:BoundField DataField="NumberOfClasses" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Number of classes" HeaderStyle-BackColor="Wheat" />
                <asp:BoundField DataField="NumberOfAttributes" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Number of attributes" HeaderStyle-BackColor="Wheat" />
                <asp:BoundField DataField="DateOfEntry" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Date of entry" HeaderStyle-BackColor="Wheat"/>
                <asp:BoundField DataField="Comment" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Comment" HeaderStyle-BackColor="Wheat"/>
                <asp:BoundField DataField="NumberOfUses" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Number of Uses" HeaderStyle-BackColor="Wheat"/>
                <asp:HyperLinkField DataNavigateUrlFields="TrainingSetFileSource" Text="Download" HeaderText="Source" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat"/>
            </Columns>
        </asp:GridView>
        <asp:Label ID="noTrainingSets" runat="server" Visible="false" Font-Bold="true" ForeColor="Red">You haven't got any training sets yet.</asp:Label>
        </fieldset>
        </div>
</asp:Content>
