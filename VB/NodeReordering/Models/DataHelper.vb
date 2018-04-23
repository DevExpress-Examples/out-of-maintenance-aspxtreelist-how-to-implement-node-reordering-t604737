Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web

Namespace NodeReordering.Models
    Public Class DataHelper

        Private _sessionKey As String = "ExampleDataSource"
        Public Property Data() As List(Of SampleDataItem)
            Get
                If HttpContext.Current.Session(_sessionKey) Is Nothing Then
                    HttpContext.Current.Session(_sessionKey) = Me.GetSampleData()
                End If
                Return DirectCast(HttpContext.Current.Session(_sessionKey), List(Of SampleDataItem))
            End Get
            Set(ByVal value As List(Of SampleDataItem))
            End Set
        End Property

        Private Function GetSampleData() As List(Of SampleDataItem)
            Dim result As New List(Of SampleDataItem)()
            result.Add(New SampleDataItem("root", 0, Nothing))
            result.Add(New SampleDataItem("a", 1, 0))
            result.Add(New SampleDataItem("b", 2, 0))
            result.Add(New SampleDataItem("a1", 3, 1))
            result.Add(New SampleDataItem("a2", 4, 1))
            result.Add(New SampleDataItem("a3", 5, 1))
            result.Add(New SampleDataItem("b1", 6, 2))
            result.Add(New SampleDataItem("b2", 7, 2))
            result.Add(New SampleDataItem("b1a", 8, 6))
            result.Add(New SampleDataItem("b1b", 9, 6))
            result.Add(New SampleDataItem("b1c", 10, 6))
            Return result
        End Function

        Public Sub SwapDataItems(ByVal item1 As SampleDataItem, ByVal item2 As SampleDataItem)
            If item1 Is Nothing OrElse item2 Is Nothing Then
                Return
            End If
            Dim index1 As Integer = Data.IndexOf(item1)
            Dim index2 As Integer = Data.IndexOf(item2)
            Data(index1) = item2
            Data(index2) = item1
        End Sub

    End Class

    Public Class SampleDataItem
        Public Property Title() As String
        Public Property Key() As Integer
        Public Property ParentKey() As Integer?

        Public Sub New(ByVal title As String, ByVal key As Integer, ByVal parentKey? As Integer)
            Me.Title = title
            Me.Key = key
            Me.ParentKey = parentKey
        End Sub
    End Class
End Namespace