Public Class ClasseListeMesureResistance

    Private _items As List(Of MesureResistance)

    Public Property ListeMesureResistance As List(Of MesureResistance)
        Get
            Return _items
        End Get
        Set(value As List(Of MesureResistance))
            _items = value
        End Set
    End Property

    ' =========================================================
    ' Constructeur
    ' =========================================================
    Public Sub New()
        _items = New List(Of MesureResistance)
    End Sub

End Class
