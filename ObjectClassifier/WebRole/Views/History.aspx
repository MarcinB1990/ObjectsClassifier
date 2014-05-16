<%@ Page Title="My History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="WebRole.Views.History" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <div runat="server" id="loggedOut">
        <h3>This page is only for registered users.</h3>
    </div>
    <div runat="server" id="loggedIn" visible="false">
        <div runat="server" id="listNotEmpty" visible="false">
            <h3>It's a history of all your classifications.</h3>
        <asp:GridView ID="myResultSetsView" runat="server" AutoGenerateColumns="false" PageSize="25" AllowPaging="true" OnPageIndexChanging="myResultSetsView_PageIndexChanging">
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="No." HeaderStyle-BackColor="Wheat"> 
                    <ItemTemplate> 
                        <%# Container.DataItemIndex + 1 %>. 
                    </ItemTemplate> 
                </asp:TemplateField> 
                <asp:BoundField DataField="NumberOfClasses" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="No. of classes" HeaderStyle-BackColor="Wheat" />
                <asp:BoundField DataField="NumberOfAttributes" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="No. of attributes" HeaderStyle-BackColor="Wheat" />
                <asp:BoundField DataField="DateOfEntry" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Date of entry" HeaderStyle-BackColor="Wheat"/>
                <asp:BoundField DataField="Comment" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Comment" HeaderStyle-BackColor="Wheat"/>
                <asp:BoundField DataField="MethodOfClassification" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Method" HeaderStyle-BackColor="Wheat"/>
                <asp:HyperLinkField DataNavigateUrlFields="TrainingSetFileSource" Text="Download" HeaderText="Training Set" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat"/>
                <asp:HyperLinkField DataNavigateUrlFields="InputFileSource" Text="Download" HeaderText="Input File" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat"/>
                <asp:HyperLinkField DataNavigateUrlFields="ResultSetFileSource" Text="Download" HeaderText="Result Set" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat"/>
                <asp:BoundField DataField="FileExtension" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="File Extension" HeaderStyle-BackColor="Wheat"/>
                <asp:BoundField DataField="Progress" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Progress" HeaderStyle-BackColor="Wheat"/>
            </Columns>
        </asp:GridView>
        </div>
        <div runat="server" id="listEmpty" visible="true">
            <h3>You didn't do any classification yet.</h3>
        </div>
    </div>
</asp:Content>
