Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Imports GaiaPressCoreV17.Models
Public Class ClasseListeMesureExpansion
    Public Property ListeMesureExpansion As New List(Of MesureExpansion)

    Private _items As List(Of MesureExpansion)

    ' =========================================================
    ' Constructeur
    '==========================================================
    Public Sub New()
        _items = New List(Of MesureExpansion)
    End Sub
End Class
