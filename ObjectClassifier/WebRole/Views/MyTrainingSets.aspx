<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MyTrainingSets.aspx.cs" Inherits="WebRole.Views.MyTrainingSets" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:GridView ID="myTrainingSetsView" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="Name" HeaderText="Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat"/>
            <asp:BoundField DataField="NumberOfClasses" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Number of classes" HeaderStyle-BackColor="Wheat" />
            <asp:BoundField DataField="NumberOfAttributes" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Number of attributes" HeaderStyle-BackColor="Wheat" />
            <asp:BoundField DataField="Comment" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Comment" HeaderStyle-BackColor="Wheat"/>
            <asp:BoundField DataField="DateOfEntry" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Date of entry" HeaderStyle-BackColor="Wheat"/>
            <asp:BoundField DataField="NumberOfUses" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderText="Number of Uses" HeaderStyle-BackColor="Wheat"/>
            <asp:HyperLinkField DataNavigateUrlFields="TrainingSetFileSource" Text="Download" HeaderText="Source" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-BackColor="Wheat"/>
        </Columns>
    </asp:GridView>
</asp:Content>
