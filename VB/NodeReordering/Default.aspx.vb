Imports DevExpress.Web.ASPxTreeList
Imports NodeReordering.Models
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace NodeReordering
    Partial Public Class [Default]
        Inherits System.Web.UI.Page

        Private dataHelper As New DataHelper()
        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            TreeList.DataSource = dataHelper.Data
            TreeList.DataBind()
            TreeList.ClientSideEvents.Init = String.Format("function (s, e) {{ s.reorderHelper = new ReorderHelper(s, '{0}', '{1}'); }}", Page.ResolveClientUrl("~/Content/Images/004_54.gif"), Page.ResolveClientUrl("~/Content/Images/004_32.gif"))
        End Sub

        Protected Sub TreeList_DataBinding(ByVal sender As Object, ByVal e As EventArgs)

        End Sub

        Protected Sub TreeList_CustomCallback(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxTreeList.TreeListCustomCallbackEventArgs)
            Dim callbackParams() As String = e.Argument.Split("|"c)
            Dim isSwap As Boolean = False
            Dim draggedKey As Integer = -1
            Dim targetKey As Integer = -1
            If callbackParams.Length = 3 AndAlso Int32.TryParse(callbackParams(0), draggedKey) AndAlso Int32.TryParse(callbackParams(1), targetKey) AndAlso Boolean.TryParse(callbackParams(2), isSwap) AndAlso isSwap Then
                dataHelper.SwapDataItems(dataHelper.Data.Find(Function(i) i.Key = draggedKey), dataHelper.Data.Find(Function(i) i.Key = targetKey))

                TreeList.DataSource = dataHelper.Data
                DirectCast(sender, ASPxTreeList).DataBind()
            End If
        End Sub

        Protected Sub TreeList_CustomJSProperties(ByVal sender As Object, ByVal e As DevExpress.Web.ASPxTreeList.TreeListCustomJSPropertiesEventArgs)
            Dim tree As ASPxTreeList = DirectCast(sender, ASPxTreeList)
            e.Properties("cpSiblingKeys") = tree.GetAllNodes().ToDictionary(Function(n) n.Key, Function(n) n.ParentNode.ChildNodes.OfType(Of TreeListNode)().Select(Function(c) c.Key))
        End Sub

        Protected Sub TreeList_ProcessDragNode(ByVal sender As Object, ByVal e As TreeListNodeDragEventArgs)
            If e.Node IsNot Nothing AndAlso e.NewParentNode IsNot Nothing Then
                Dim child As SampleDataItem = dataHelper.Data.Find(Function(i) i.Key = Convert.ToInt32(e.Node.Key))
                Dim newParent As SampleDataItem = dataHelper.Data.Find(Function(i) i.Key = Convert.ToInt32(e.NewParentNode.Key))
                child.ParentKey = newParent.Key

                TreeList.DataSource = dataHelper.Data
                DirectCast(sender, ASPxTreeList).DataBind()
                e.Handled = True
            End If

        End Sub
    End Class
End Namespace