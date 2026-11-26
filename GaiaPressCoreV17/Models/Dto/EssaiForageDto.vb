
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPressCoreV17.Models.Dto
    Public Class EssaiForageDto

        Public Property RefEssai As String
        Public Property RefForage As String
        Public Property RefChantier As String
        Public Property RefSol As String
        Public Property RefSonde As String
        Public Property refEssaiResistancePropre As String
        Public Property refEssaiExpansionPropre As String
        Public Property refEssaiPerteDeCharge As String
        Public Property NomEssai As String
        Public Property dateEssai As Date

        ' ← RIEN DE PLUS !***


    End Class

End Namespace
