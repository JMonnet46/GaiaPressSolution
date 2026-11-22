Imports Microsoft.EntityFrameworkCore
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections.Generic
Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace GaiaPressCoreV17.Models

    <Table("LesForagesDb", Schema:="dbo")>
    Public Class Forage

        ' Tableau des Essais
        Public Property ListEssais As New List(Of Essai_Forage)()

        ' Clé primaire
        <Key>
        Public Property RefForage As String

        ' Clé étrangère : mappée sur la colonne SQL refChantier
        <Column("refChantier")>
        Public Property RefChantier As String

        ' Propriété de navigation vers le chantier
        Public Property Chantier As Chantier

        ' Propriétés du forage
        Public Property numeroForage As Integer
        Public Property NomForage As String
        Public Property dateForage As Date
        Public Property materiel As String
        Public Property outil As String
        Public Property abscisseX As Single
        Public Property ordonneeY As Single
        Public Property altitudeZ As Single
        Public Property coteInit As Single
        Public Property coteFinale As Single

        ' Propriétés NON mappées en base (uniquement en mémoire)
        <NotMapped>
        Public Property EventsSuspendus As Boolean

        <NotMapped>
        Public Property IsSaved As Boolean

        <NotMapped>
        Public Property FichierChantier As String

        Public Event HasBeenModified()

        '================================================================================
        ' Constructeur
        '================================================================================

        Public Sub New()

        End Sub

        ' ================================================================================
        ' Gestion des Forages
        ' ================================================================================

        Public Sub IHaveBeenModified()
            If Not EventsSuspendus Then
                RaiseEvent HasBeenModified()
                IsSaved = False
            End If
        End Sub

        ''' <summary>
        Public Function GetRefForage()
            GetRefForage = RefForage
        End Function

        Public Function GetNumeroForage()
            GetNumeroForage = numeroForage
        End Function

        Public Function GetNomForage()
            GetNomForage = NomForage
        End Function

        Public Function GetDateForage()
            GetDateForage = dateForage
        End Function

        Public Function GetMateriel()
            GetMateriel = materiel
        End Function

        Public Function GetOutil()
            GetOutil = outil
        End Function

        Public Function GetabscisseX()
            GetabscisseX = abscisseX
        End Function

        Public Function GetOrdonneeY()
            GetOrdonneeY = ordonneeY
        End Function

        Public Function GetAltitudeZ()
            GetAltitudeZ = altitudeZ
        End Function

        Public Function GetCoteInit()
            GetCoteInit = coteInit
        End Function

        Public Function GetCoteFinale()
            GetCoteFinale = coteFinale
        End Function

        Public Function GetForageParRef(refForage As String) As Forage
            If Me.Chantier Is Nothing Then
                Return Nothing
            End If

            Return Me.Chantier.GetForageParRef(refForage)
        End Function

        Public Sub SetNomForage(ByVal nom As String)
            NomForage = nom

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetRefForage(ByVal ref As String)
            ' Attribution d'une référence au Forage
            RefForage = ref

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            'Call IHaveBeenModified
        End Sub

        Public Sub SetNumeroForage(ByVal numero As Integer)
            numeroForage = numero

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetDateForage(ByVal dateFora As String)
            dateForage = dateFora

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetMateriel(ByVal typeFora As String)
            materiel = typeFora

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetOutil(ByVal outi As String)
            outil = outi

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        'Public Sub SelectMyNode()
        '    'MyNodeForage.Selected = True
        '    MyNodeForage.EnsureVisible()
        'End Sub

        Public Sub SetAbscisseX(ByVal abscisse As Single)
            abscisseX = abscisse

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetOrdonneeY(ByVal ordonnee As Single)
            ordonneeY = ordonnee

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetAltitudeZ(ByVal altitude As Single)
            altitudeZ = altitude

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub


        Public Sub SetCoteInit(ByVal cote As Single)
            coteInit = cote

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetCoteFinale(ByVal cote As Single)
            coteFinale = cote

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetEssaiCalib(ByRef Essai As Object, ByVal typeEssai As Integer)

            Select Case typeEssai
                Case EssaiCalib.Resistance
                    MyResistance = Essai
                    refEssaiResistancePropre = Essai.GetRefEssai
                Case EssaiCalib.Expansion
                    MyExpansion = Essai
                    refEssaiExpansionPropre = Essai.GetRefEssai
            End Select
            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub NouveauEssai(ByVal NewTable As Boolean)
            ' Déclaration d'un essai

            '    MsgBox "NewEssai"
            If NewTable Then  ' On ajoute un Essai à MonFichier.Tables("LesEssaiForage") seulement si Table est vrai
                If MonChantier.Tables("LesEssaiForage") Is Nothing Then
                    MonChantier.Tables.Add("LesEssaiForage")
                    With MonChantier.Tables("LesEssaiForage")
                        .Columns.Add("RefChantier", GetType(String))
                        .Columns.Add("refForage", GetType(String))

                        Dim columns() As DataColumn
                        ReDim columns(36)

                        .Columns.Add("refEssai", GetType(String))
                        columns(2) = .Columns("refEssai")
                        .PrimaryKey = columns

                        .Columns.Add("refSonde", GetType(String))
                        .Columns.Add("refEssaiResistancePropre", GetType(String))
                        .Columns.Add("refEssaiExpansionPropre", GetType(String))
                        .Columns.Add("refEssaiPerteDeCharge", GetType(String))
                        .Columns.Add("refSol", GetType(String))
                        .Columns.Add("Enregistreur", GetType(String))
                        .Columns.Add("Operateur", GetType(String))
                        .Columns.Add("VolSonde", GetType(Single))
                        .Columns.Add("ClassPrecis", GetType(Integer))
                        .Columns.Add("PrecisVolSonde", GetType(Single))
                        .Columns.Add("nomEssai", GetType(String))
                        .Columns.Add("dateEssai", GetType(String))
                        .Columns.Add("heureEssai", GetType(String))
                        .Columns.Add("difPresEauAir", GetType(Single))
                        .Columns.Add("hauteurCpv", GetType(Single))
                        .Columns.Add("presP0", GetType(Single))
                        .Columns.Add("presFluage", GetType(Single))
                        .Columns.Add("presLimite", GetType(Single))
                        .Columns.Add("presLimiteNF", GetType(Single))
                        .Columns.Add("presLimiteTheo", GetType(Single))
                        .Columns.Add("PrecisVolume", GetType(Single))
                        .Columns.Add("PrecisPression", GetType(Single))
                        .Columns.Add("poidsVol", GetType(Single))
                        .Columns.Add("poidsVolDejauge", GetType(Single))
                        .Columns.Add("coefPoisson", GetType(Single))
                        .Columns.Add("modPressio", GetType(Single))
                        .Columns.Add("modPressioNf", GetType(Single))
                        .Columns.Add("modElastique", GetType(Single))
                        .Columns.Add("modElastiqueNf", GetType(Single))
                        .Columns.Add("cohesion", GetType(Single))
                        .Columns.Add("frottement", GetType(Single))
                        .Columns.Add("frottementintergranulaire", GetType(Single))
                        .Columns.Add("U0", GetType(Single))
                        .Columns.Add("profNappe", GetType(Single))
                        .Columns.Add("profSonde", GetType(Single))
                        .Columns.Add("coteZeroGeomeca", GetType(Single))

                    End With
                Else
                    MonChantier.Tables("LesEssaiForage").NewRow()
                End If
            End If

            Debug.WriteLine("Instanciation du nouveau Essai")

            ' Enregistrement du nouveau Essai dans le tableau de Essai

            Debug.WriteLine("Enregistrement du nouveau Essai dans le tableau de Essai")

            'MyForage = ListForageSession(ListForageSession.Count - 1)

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

            ' Ouverture du noeud de forage
            'MyNodeForage.Expand()

        End Sub ' Fin de NouveauEssai

        Public Function GiveMeANewRefEssai()
            ' Détermination d'une nouvelle référence pour un essai
            Dim k As Integer
            '    MsgBox "GiveMeANewRefEssai"
            k = 1
            Do While k >= MyListeEssaiForageSession.Count
                MyEssaiForage = MyListeEssaiForageSession(k)
                If Not MyEssaiForage.IsRefUnique("RefEssai " & k) Then Exit Do
                k = k + 1
            Loop
            GiveMeANewRefEssai = "RefEssai " & k
        End Function

        Public Function GiveMeANewNomEssai()
            ' Détermination d'une nouvelle référence pour un essai
            Dim k As Integer
            '    MsgBox "GiveMeANewRefEssai"
            k = 1
            Do While k >= MyListeEssaiForageSession.Count
                MyEssaiForage = MyListeEssaiForageSession(k)
                If Not MyEssaiForage.IsNomUnique("NomEssai " & k) Then Exit Do
                k = k + 1
            Loop
            GiveMeANewNomEssai = "NomEssai " & k
        End Function


        Public Sub UpdateEssaiForage(ByVal essai As Essai_Forage)
            For i As Integer = 0 To Me.ListEssais.Count - 1
                If Me.ListEssais(i).RefEssai = essai.RefEssai Then
                    Me.ListEssais(i) = essai
                    Exit For
                End If
            Next
        End Sub

    End Class
End Namespace