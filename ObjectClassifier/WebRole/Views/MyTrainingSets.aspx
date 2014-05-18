<%@ Page Title="My Training Sets" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyTrainingSets.aspx.cs" Inherits="WebRole.Views.MyTrainingSets" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div runat="server" id="loggedOut">
        <h3>This page is only for registered users.</h3>
    </div>
    <div runat="server" id="loggedIn" visible="false">
        <div runat="server" id="listNotEmpty" visible="false">
            <h3>It's a list of all your training sets.</h3>
            <asp:GridView PageSize="25" AllowPaging="true" OnPageIndexChanging="myTrainingSetsView_PageIndexChanging" ID="myTrainingSetsView" runat="server" AutoGenerateColumns="false" OnRowDeleting="myTrainingSetsView_RowDeleting">
                <Columns>
                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="No." HeaderStyle-BackColor="Wheat">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>. 
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat" />
                    <asp:BoundField DataField="NumberOfClasses" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="No. of classes" HeaderStyle-BackColor="Wheat" />
                    <asp:BoundField DataField="NumberOfAttributes" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="No. of attributes" HeaderStyle-BackColor="Wheat" />
                    <asp:BoundField DataField="DateOfEntry" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Date of entry" HeaderStyle-BackColor="Wheat" />
                    <asp:BoundField DataField="Comment" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Comment" HeaderStyle-BackColor="Wheat" />
                    <asp:BoundField DataField="AccessRights" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Access Rights" HeaderStyle-BackColor="Wheat" />
                    <asp:BoundField DataField="NumberOfUses" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="No. of Uses" HeaderStyle-BackColor="Wheat" />
                    <asp:HyperLinkField DataNavigateUrlFields="TrainingSetFileSource" Text="Download" HeaderText="Source" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat" />
                    <asp:CommandField DeleteText="Delete" ShowDeleteButton="true" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat" />
                </Columns>
            </asp:GridView>
        </div>
        <div runat="server" id="listEmpty" visible="true">
            <h3>You haven't got any training sets yet.</h3>
        </div>
        <asp:HyperLink ID="hl" NavigateUrl="~/Views/AddTraining.aspx" Text="Add new" runat="server" Font-Size="Large"></asp:HyperLink>
    </div>
</asp:Content>
