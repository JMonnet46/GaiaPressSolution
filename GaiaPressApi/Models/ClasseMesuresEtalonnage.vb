Imports Microsoft.EntityFrameworkCore

Namespace GaiaPress.core.V17.Models

    Public Class MesuresExpansion
        Inherits DbContext
        Public RefEssai As String
        Public IndiceMesure As Integer
        Public Pression As Single
        Public Volume15 As Single
        Public Volume30 As Single
        Public Volume60 As Single
    End Class
End Namespace
