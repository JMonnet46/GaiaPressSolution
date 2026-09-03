Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPress.Core.V17.Models

    ''' <summary>
    ''' Représente une mesure associée à un essai pressiométrique.
    ''' </summary>
    Public Class MesuresEssaiForage

        ' Définition de la clef primaire de la Table LesMesuresEssaiForage
        <Key>
        Public Property Id As Integer

        <Required>
        Public Property RefEssai As String

        <Required>
        Public Property IndiceMesure As Integer

        Public Property Pression As Single?

        Public Property Volume15 As Single?

        Public Property Volume30 As Single?

        Public Property Volume60 As Single?

        Public Property ContrainteRadiale As Single?

        Public Property ContrainteVerticale As Single?

        Public Property Deformation As Single?

        Public Property PNF As Single?

        Public Property VNF As Single?

        Public Property Qi As Single?

        Public Property Pi As Single?

        Public Property Qp As Single?

        Public Property Pp As Single?

        ' === NAVIGATION VERS L'ESSAI PARENT ===
        <ForeignKey("RefEssai")>
        Public Overridable Property Essai_Forage As Essai_Forage

        ''' <summary>
        ''' Constructeur par défaut.
        ''' </summary>
        Public Sub New()
            ' Initialisation par défaut
        End Sub

        ''' <summary>
        ''' Constructeur avec paramètres.
        ''' </summary>
        Public Sub New(refEssai As String, indiceMesure As Integer)
            Me.RefEssai = refEssai
            Me.IndiceMesure = indiceMesure
        End Sub
    End Class
End Namespace
