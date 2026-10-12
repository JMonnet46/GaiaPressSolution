Imports System.ComponentModel.DataAnnotations

Namespace GaiaPressCoreV17.Models

    Public Class Sol

        ' Définition de la clef primaire de la Table Sols
        <Key>
        Public Property RefSol As String

        Public Property nomSol As String
        Public Property zoneGeo As String
        Public Property famille As String
        Public Property ClasseDuSol As String
        Public Property SousClasse As Integer
        Public Property ClasseInférieure As String
        Public Property SousClasseInférieure As Integer
        Public Property nomSolNormalise As String
        Public Property Dmax As Single
        Public Property Tamisat50mm As Single
        Public Property Tamisat2mm As Single
        Public Property tamisat80microns As Single
        Public Property VBS As Single
        Public Property Ip As Single
        Public Property Es As Single
        Public Property Phimu As Single
    End Class
End Namespace