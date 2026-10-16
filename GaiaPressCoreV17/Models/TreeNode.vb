Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Public Class TreeNode
    Public Property Value As Object
    Public Property Children As New List(Of TreeNode)
    Public Property Text As String
    Public Property Tag As Object

    Public Sub New()
        ' Constructeur par défaut
    End Sub

    Public Sub New(value As Object)
        Me.Value = value
        Me.Text = value.ToString()
    End Sub

    Public Sub New(value As Object, text As String)
        Me.Value = value
        Me.Text = text
    End Sub

    Public Function AddChild(child As TreeNode) As TreeNode
        Me.Children.Add(child)
        Return child
    End Function
End Class
