Imports Microsoft.EntityFrameworkCore

Namespace GaiaPress.core.V17.Models

    ''' Représente une mesure associée à un essai pressiométrique.
    Public Class MesuresEssaiForage
        ' Propriétés de base

        Public Property ind As Integer     ' indice 1
        Public Property RefEssai As String           ' indice 2
        Public Property indiceMesure As Integer     ' indice 3
        Public Property pression As Nullable(Of Single)  ' ✅ Déclaré comme Nullable ' indice 4
        Public Property volume15 As Nullable(Of Single)   ' ✅ Déclaré comme Nullable    ' indice 5
        Public Property volume30 As Nullable(Of Single)   ' ✅ Déclaré comme Nullable    ' indice 6
        Public Property volume60 As Nullable(Of Single)   ' ✅ Déclaré comme Nullable    ' indice 7
        Public Property contrainteRadiale As Nullable(Of Single)  ' ✅ Déclaré comme Nullable    ' indice 8
        Public Property contrainteVerticale As Nullable(Of Single)  ' ✅ Déclaré comme Nullable    ' indice 8
        Public Property deformation As Nullable(Of Single)  ' ✅ Déclaré comme Nullable    ' indice 9
        Public Property PNF As Nullable(Of Single)       ' ✅ Déclaré comme Nullable    ' indice 10
        Public Property VNF As Nullable(Of Single)       ' ✅ Déclaré comme Nullable    ' indice 11
        Public Property Qi As Nullable(Of Single)                ' indice 12
        Public Property Pi As Nullable(Of Single)                ' indice 13
        Public Property Qp As Nullable(Of Single)                ' indice 14 
        Public Property Pp As Nullable(Of Single)                ' indice 15

        ' ✅ Propriété de navigation vers l'essai parent

        ' ✅ Propriété pour suivre les modifications (si nécessaire)
        Public Property _HasBeenModified As Boolean = False

        ' ✅ Vérifiez que ces propriétés existent
        Public Property Essai As Essai_Forage  ' Propriété de navigation vers l essai parent

        ''' <summary>
        ''' Constructeur par défaut.
        ''' </summary>
        'Public Sub New()
        'End Sub

        ''' <summary>
        ''' Constructeur avec paramètres.
        ''' </summary>
        Public Sub New(refEssai As String, pression As Single)
            Me.RefEssai = refEssai  ' Pas d'erreur : String est compatible
            Me.pression = pression
        End Sub
        ' ✅ Méthode pour supprimer une mesure
        Public Sub DeleteMesuresForage(Indice As Integer)
            Dim mesureToDelete As MesuresEssaiForage = GetMesuresEssaiForage(Indice)
            If mesureToDelete IsNot Nothing Then
                If Me.Essai IsNot Nothing AndAlso Me.Essai.E_MesuresEssaiForage IsNot Nothing Then
                    Me.Essai.E_MesuresEssaiForage.Remove(mesureToDelete)
                End If
                _HasBeenModified = True
            End If
        End Sub

        ''' <summary>
        ''' Calcule le module pressiométrique (exemple de méthode de calcul).
        ''' </summary>
        Public Function CalculerModulePressio() As Double
            ' Exemple de calcul : à adapter selon votre logique métier
            If Me.pression > 0 Then
                Return pression * 1000.0 ' Exemple simplifié
            Else
                Return 0.0
            End If
        End Function

        ''' <summary>
        ''' Calcule l'angle de frottement Phi (exemple).
        ''' </summary>
        Public Function CalculerPhi() As Double
            ' Exemple de calcul : à adapter selon votre logique métier
            If pression > 0 Then
                Return Math.Atan(pression / 1000.0) * (180.0 / Math.PI)
            Else
                Return 0.0
            End If
        End Function

        ' ===============================================================================
        ' Gestion de C_ListMesuresForage
        ' ===============================================================================

        ' ✅ Méthode pour mettre à jour une mesure
        Public Sub MesuresForage(Indice As Integer, NouvelleMesure As MesuresEssaiForage)
            Dim Mesure As MesuresEssaiForage = GetMesuresEssaiForage(indiceMesure)
            If Mesure IsNot Nothing Then
                ' ✅ Adaptez selon les propriétés réelles de MesuresEssaiForage
                Mesure.pression = NouvelleMesure.pression
                Mesure.volume15 = NouvelleMesure.volume15
                Mesure.volume30 = NouvelleMesure.volume30
                Mesure.volume60 = NouvelleMesure.volume60
                Mesure.contrainteRadiale = NouvelleMesure.contrainteRadiale
                Mesure.contrainteVerticale = NouvelleMesure.contrainteVerticale
                Mesure.deformation = NouvelleMesure.deformation
                Mesure.PNF = NouvelleMesure.PNF
                Mesure.VNF = NouvelleMesure.VNF
                Mesure.Qi = NouvelleMesure.Qi
                Mesure.Pi = NouvelleMesure.Pi
                Mesure.Qp = NouvelleMesure.Qp
                Mesure.Pp = NouvelleMesure.Pp

                _HasBeenModified = True
            End If
        End Sub

        '' Mettre à jour une Mesure
        'Public Sub UpdateMesureEssaiForage(IndiceMesure As Integer, NouvelleMesure As MesuresEssaiForage)
        '    Dim Mesure As MesuresEssaiForage = GetMesuresEssaiForage(IndiceMesure)
        '    If Mesure IsNot Nothing Then
        '        Mesure.pression = NouvelleMesure.pression
        '        Mesure.volume15 = NouvelleMesure.volume15
        '        Mesure.volume30 = NouvelleMesure.volume30
        '        Mesure.volume60 = NouvelleMesure.volume60
        '        Mesure.contrainteRadiale = NouvelleMesure.contrainteRadiale
        '        Mesure.contrainteVerticale = NouvelleMesure.contrainteVerticale
        '        Mesure.deformation = NouvelleMesure.deformation
        '        Mesure.PNF = NouvelleMesure.PNF
        '        Mesure.VNF = NouvelleMesure.VNF
        '        Mesure.Qi = NouvelleMesure.Qi
        '        Mesure.Pi = NouvelleMesure.Pi
        '        Mesure.Qp = NouvelleMesure.Qp
        '        Mesure.Pp = NouvelleMesure.Pp
        '    End If
        'End Sub


        ''' <summary>
        ''' Add une mesure d'essai de forage au chantier
        ''' </summary>
        'Public Sub AddMesuresEssaisForage(Mesure As MesuresEssaiForage)
        '    ' ✅ Utiliser la liste du chantier (C_ListMesuresForage)
        '    _context.C_ListMesuresForage.Add(Mesure)
        '    ' Lier la mesure au chantier (si nécessaire)
        '    ' Mesure.E_RefChantier = _chantier.C_refChantier
        'End Sub

        ''' <summary>
        ''' Get une mesure d'essai de forage par indice
        ''' </summary>
       ' ✅ Méthode pour récupérer une mesure par indice
        Public Function GetMesuresEssaiForage(Indice As Integer) As MesuresEssaiForage
            ' ✅ Utilisez la propriété de navigation Essai pour accéder aux mesures
            If Me.Essai IsNot Nothing AndAlso Me.Essai.Mesures_Essai_Forages IsNot Nothing Then
                Return Me.Essai.Mesures_Essai_Forages.FirstOrDefault(Function(m) m.indiceMesure = Indice)
            End If
            Return Nothing
        End Function

        '''' <summary>
        '''' Delete une mesure d'essai de forage par indice
        '''' </summary>
        'Public Sub DeleteMesuresForage(Indice As Integer)
        '    Dim MesureADelete As MesuresEssaiForage = GetMesuresEssaiForage(Indice)
        '    If MesureADelete IsNot Nothing Then
        '        ' ✅ Utiliser la liste du chantier (C_ListMesuresForage)
        '        _chantier.C_ListMesuresForage.Remove(MesureADelete)
        '        _HasBeenModified = True
        '    End If
        'End Sub

        ''' <summary>
        ''' Mettre à jour une mesure d'essai de forage
        ''' </summary>
        'Public Sub MettreAJourMesuresForage(Indice As Integer, nouveauNom As String, nouvelleDate As String)
        '    Dim Mesure As MesuresEssaiForage = GetMesuresEssaiForage(Indice)
        '    If Mesure IsNot Nothing Then
        '        'Adaptez selon les propriétés réelles de MesuresEssaiForage
        '        Mesure.E_nomEssai = nouveauNom
        '        Mesure.E_dateEssai = nouvelleDate
        '        _HasBeenModified = True
        '    End If
        'End Sub

        ''' <summary>
        ''' Récupère toutes les mesures d'essais de forage.
        ''' </summary>


        ''' <summary>
        ''' Récupère une mesure d'essai de forage par son indice
        ''' </summary>
        Public Function GetMesuresForageByIndice(Indice As Integer) As MesuresEssaiForage
            Return GetMesuresEssaiForage(Indice)
        End Function

    End Class
End Namespace
