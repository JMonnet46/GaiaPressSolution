Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Imports GaiaPressCoreV17.Models
Public Class ClasseListeMesureEssaiForage
    Public Property ListeMesureEssaiForageSession As New List(Of MesureEssaiForage)

    Private _items As List(Of MesureEssaiForage)

    ' =========================================================
    ' Constructeur
    '==========================================================
    Public Sub New()
        _items = New List(Of MesureEssaiForage)
    End Sub
End Class
