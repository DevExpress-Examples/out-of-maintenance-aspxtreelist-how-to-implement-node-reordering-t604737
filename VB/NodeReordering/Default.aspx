<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="NodeReordering.Default" %>

<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v16.1, Version=16.1.17.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Node Reorder Example</title>

</head>
<body>
    <form id="form1" runat="server">
        <script type="text/javascript" src="<%=ResolveClientUrl("~/Scripts/ReorderHelper.js")%>"></script>


        <h2>Drag a node to the first 1/3 of a sibling node</h2>
        <dx:ASPxTreeList ID="TreeList" ClientInstanceName="TreeList" runat="server" 
            Width="300px" KeyFieldName="Key" ParentFieldName="ParentKey"

            OnDataBinding="TreeList_DataBinding"
            OnCustomCallback="TreeList_CustomCallback"
            OnCustomJSProperties="TreeList_CustomJSProperties"
            OnProcessDragNode="TreeList_ProcessDragNode">

            <SettingsPager Visible="true"></SettingsPager>
            <SettingsSelection Enabled="false" />
            <Settings GridLines="Both" />
            <SettingsBehavior AllowSort="false" />
            <SettingsEditing AllowNodeDragDrop="true" />
            <Styles>
                <Node CssClass="tree-node"></Node>
            </Styles>
        </dx:ASPxTreeList>
    </form>
</body>
</html>