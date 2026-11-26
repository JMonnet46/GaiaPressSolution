Imports System.Data
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Controllers
Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Imports GaiaPressCoreV17.Models
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Models

    Public Class ListeSonde

        ' ------Tables -----------------------------------------
        Public Property ListeSonde As New List(Of Probe)
        'Public Sondes() As Probe
        'Public SondesSup() As Probe

        ' ------------------------------------------------------
        Public FichierListeSonde As String
        Public EventsSuspendus As Boolean
        Public IsSaved As Boolean
        Public Event HasBeenModified()


        ' ================================================================================
        ' Gestion de   ListeSonde
        ' ================================================================================
        ' Add une Sonde
        Public Sub AddSonde(Sonde As Probe)
            Me.ListeSonde.Add(Sonde)
            ' Lier l Esai a la sonde (si nécessaire)
            MyEssaiForage.refEssai = Sonde.RefSonde
        End Sub


        Public Function GetSonde(refSonde As String) As Probe
            For Each S As Probe In Me.ListeSonde
                If S.RefSonde = refSonde Then
                    Return S
                End If
            Next
            Return Nothing
        End Function

        ' Delete une Sonde par référence
        Public Sub DeleteSonde(refSonde As String)
            Dim SondeADelete As Probe = GetSonde(refSonde)
            If SondeADelete IsNot Nothing Then
                Me.ListeSonde.Remove(SondeADelete)
            End If
        End Sub

        ''' <summary>
        ''' Met à jour une sonde dans la liste  ListeSonde.
        ''' Si la sonde n'existe pas, elle est ajoutée.
        ''' </summary>
        ''' <param name="sondeToUpdate">Instance de Probe à mettre à jour ou Add.</param>
        Public Sub SetSonde(ByRef Sonde As Probe, ByVal index As Integer)
            If Sonde Is Nothing Then Exit Sub
            If index >= 0 AndAlso index < Me.ListeSonde.Count Then
                Me.ListeSonde(index) = Sonde
            ElseIf index = Me.ListeSonde.Count Then
                Me.ListeSonde.Add(Sonde)
            End If
        End Sub

        Public Sub UpdateProbe(sondeToUpdate As Probe)
            ' Vérifier si la sonde existe déjà dans  ListeSonde
            Dim existingSonde As Probe = Me.ListeSonde.FirstOrDefault(Function(s) s.RefSonde = sondeToUpdate.RefSonde)

            If existingSonde IsNot Nothing Then
                ' ✅ Mettre à jour les propriétés de la sonde existante
                existingSonde.RefSonde = sondeToUpdate.RefSonde
                ' Copier les autres propriétés de sondeToUpdate vers existingSonde
                ' Exemple :
                ' existingSonde. typeSonde = sondeToUpdate.sondeToUpdate. typeSonde

                existingSonde = sondeToUpdate

                'existingSonde. refSonde = sondeToUpdate. refSonde
                'existingSonde. NomSonde = sondeToUpdate. NomSonde
                'existingSonde. ClassPrecis = sondeToUpdate. ClassPrecis
                'existingSonde. PrecisVolSonde = sondeToUpdate. PrecisVolSonde
                'existingSonde. typeSonde = sondeToUpdate. typeSonde
                'existingSonde. typeDeformee = sondeToUpdate. typeDeformee
                'existingSonde. typeContrainte = sondeToUpdate. typeContrainte
                'existingSonde. ls = sondeToUpdate. ls  ' Longueur de la cellule de mesure
                'existingSonde. lg = sondeToUpdate. lg   ' Longueur des cellules de garde
                'existingSonde. lt = sondeToUpdate. lt  ' Longueur totale de la partie déformable de la sonde
                'existingSonde. longInjectePompage = sondeToUpdate. longInjectePompage
                'existingSonde. Vs = sondeToUpdate. Vs    ' Volume de la sonde
                'existingSonde. ds = sondeToUpdate. ds   ' Diamètre intérieur du Tube de calibration
                'existingSonde. EpMembraneGarde = sondeToUpdate. EpMembraneGarde
                'existingSonde. PrEpMembraneGarde = sondeToUpdate. PrEpMembraneGarde
                'existingSonde. EpMembraneMesure = sondeToUpdate. EpMembraneMesure
                'existingSonde. PrEpMembraneMesure = sondeToUpdate. PrEpMembraneMesure
                'existingSonde. epTubeFendu = sondeToUpdate. epTubeFendu
                'existingSonde. PrEpTubeFendu = sondeToUpdate. PrEpTubeFendu
                'existingSonde. EpInitMembrane = sondeToUpdate. EpInitMembrane
                'existingSonde. PrEpInitMembrane = sondeToUpdate. PrEpInitMembrane
                'existingSonde. PrLong = sondeToUpdate. PrLong
                'existingSonde. PrDiam = sondeToUpdate. PrDiam
                'existingSonde. PrP = sondeToUpdate. PrP
                'existingSonde. PrV = sondeToUpdate. PrV
                'existingSonde. PrPV = sondeToUpdate. PrPV
                'existingSonde. PrZ = sondeToUpdate. PrZ
                'existingSonde. Prt = sondeToUpdate. Prt
                'existingSonde. Pm = sondeToUpdate. Pm
                'existingSonde. Vs = sondeToUpdate. Vs
                'existingSonde. PrecisA = sondeToUpdate. PrecisA
                'existingSonde. PrecisB = sondeToUpdate. PrecisB
                'existingSonde. PrecisVs = sondeToUpdate. PrecisVs
                'existingSonde. PrecisVc = sondeToUpdate. PrecisVc

            Else
                ' ✅ Add la nouvelle sonde à la liste
                Me.ListeSonde.Add(sondeToUpdate)
            End If
        End Sub


        ' Propriété pour obtenir le nombre de sondes
        Public ReadOnly Property Count() As Integer
            Get
                Return ListeSonde.Count
            End Get
        End Property

        Default Public Property Item(ByVal index As Integer) As Probe
            Get
                Return MyListeSonde.ListeSonde(index)
            End Get
            Set(ByVal value As Probe)
                MyListeSonde.ListeSonde(index) = value
            End Set
        End Property



        Public Sub NouvelleSonde(ByVal NewProbe As Boolean, ByVal NewTable As Boolean)
            ' NewProbe est Vrai quand on Crée une Nouvelle Sonde; sinon Faux
            ' NewTable est Vrai quand on Crée une Nouvelle Table MaListeSonde.Tables("LesSondes"); sinon Faux

            ' Déclaration d'un forage

            Dim NouvelleSonde As Probe

            NouvelleSonde = Nothing

            ' Incrémentation du compteur des sondes si Newprobe est Vrai

            ' Redimensionnement du tableau dynamique des Sondes
            ' Instanciation d
            If NouvelleSonde Is Nothing Then
                NouvelleSonde = New Probe
            End If

            ' Enregistrement de la nouvelle sonde dans le tableau de sondes
            MyListeSonde.ListeSonde(MyListeSonde.ListeSonde.Count - 1) = NouvelleSonde

            ' Enregistrement de la liste auprès de sa nouvelle sonde
            Call NouvelleSonde.SetMyListe(Me)

            ' Attribution d'une référence à la nouvelle sonde
            NouvelleSonde.SuspendreEvents()
            NouvelleSonde.SetRefSonde(GiveMeANewRefSonde)
            NouvelleSonde.RetablirEvents()
            If MySonde IsNot Nothing Then MySonde = New Probe
            MySonde = NouvelleSonde

            'If NewTable Then ' On ajoute une mesure à MonFichier.Tables("LesSondes") seulement si Newprobe est vrai
            '    If MaListeSonde.Tables("LesSondes") Is Nothing Then

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

        End Sub

        Public Function SupSonde(ByVal ref As String) As Boolean
            Dim i As Integer    ' Index de la sonde à Delete
            Dim J As Integer

            On Error GoTo errFind

            ' Récupération de l'indice de la sonde d'après sa référence
            i = GetIndexSonde(ref)

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

            SupSonde = True
            Exit Function

errFind:
            SupSonde = False
            MsgBox(Err.Description)
        End Function

        Public Function GetEssaiExpansionParRef(ByVal RefSonde As String, ByVal refExpansion As String) As EssaiExpansion
            ' Recherche de la sonde ayant pour référence 'ref' avec arrêt dès que possible
            Dim i, j As Integer
            Dim Trouve As Boolean

            Trouve = False
            For i = 0 To MyListeSonde.ListeSonde.Count - 1

                ' On cherche dans la liste des sondes celle qui correspond
                If MyListeSonde.ListeSonde(i).RefSonde = RefSonde Then

                    ' On a trouvé la sonde avec la bonne référence
                    With MyListeSonde.ListeSonde(i)

                        If .MaExpansion IsNot Nothing Then
                            For j = 0 To MyListeSonde.ListeSonde.Count - 1

                                ' On cherche dans la liste des essais d'expansion celui qui corrrespond
                                If .Expansion(j) IsNot Nothing Then
                                    If .Expansion(j).RefEssai.ToString = refExpansion Then
                                        GetEssaiExpansionParRef = .Expansion(j)
                                        MyExpansion = .Expansion(j)
                                        .MaExpansion = .Expansion(j)
                                        Trouve = True
                                        Exit Function
                                    End If
                                End If
                            Next j
                        End If
                    End With
                End If
            Next i

            '' Si la sonde n'a pas été trouvée, la valeur nothing est renvoyée
            GetEssaiExpansionParRef = Nothing
        End Function

        Public Function GetEssaiResistanceParRef(ByVal RefSonde As String, ByVal refResistance As String) As EssaiResistance
            ' Recherche de la sonde ayant pour référence 'ref' avec arrêt dès que possible
            Dim i, j As Integer
            Dim Trouve As Boolean

            Trouve = False
            For i = 0 To MyListeSonde.ListeSonde.Count - 1

                ' On cherche dans la liste des sondes celle qui correspond
                If MyListeSonde.ListeSonde(i).RefSonde = RefSonde Then

                    ' on a trouvé la sonde avec la bonne référence
                    With MyListeSonde.ListeSonde(i)

                        If .MaResistance IsNot Nothing Then

                            ' On cherche l'essai avec labonne référence de résistance
                            For j = 0 To .Resistance.Count - 1

                                If .Resistance(j) IsNot Nothing Then
                                    ' On cherche dans la liste des essais d'Resistance celui qui corrrespond
                                    If .Resistance(j).RefEssai = refEssaiResistancePropre Then
                                        .Resistance(j).RefEssai = refResistance
                                        GetEssaiResistanceParRef = .Resistance(j)
                                        MyResistance = .Resistance(j)
                                        .MaResistance = .Resistance(j)
                                        Trouve = True
                                        Exit Function
                                    End If
                                End If
                            Next j
                        End If
                    End With
                End If
            Next i

            '' Si la sonde n'a pas été trouvée, la valeur nothing est renvoyée
            GetEssaiResistanceParRef = Nothing
        End Function

        Public Function GetIndexSonde(ByVal ref As String) As Integer
            ' Recherche de la sonde ayant pour référence 'ref' avec arrêt dès que possible
            Dim i As Integer

            Do While (i < MyListeSonde.ListeSonde.Count - 1)
                If (MyListeSonde.ListeSonde(i).RefSonde = ref) Then Exit Do
                i = i + 1
            Loop

            ' Si la sonde n'a pas été trouvée, la valeur -1 est renvoyée
            If i = MyListeSonde.ListeSonde.Count Then i = -1
            GetIndexSonde = i
        End Function


        Public Function GiveMeANewRefSonde()
            ' Détermination d'une nouvelle référence pour un forage
            Dim k As Integer
            k = 1

            Do While Not IsRefUnique("RefSonde " & k)
                k = k + 1
            Loop
            GiveMeANewRefSonde = "RefSonde " & k

        End Function

        Public Function IsRefUnique(ByVal ref As String) As Boolean
            ' Vérification de l'unicité d'une référence
            Dim i As Integer
            Dim unique As Boolean

            unique = True

            Do While (i < ref.Count) And unique
                unique = (MyListeSonde.ListeSonde(i).GetRefSonde <> ref)
                i = i + 1
            Loop

            IsRefUnique = unique
        End Function

        'Public Sub ShowSonde(ByVal ref As String)
        '    Dim i As Integer    ' Index de la sonde à ouvrir
        '    i = GetIndexSonde(ref)
        '    If i = -1 Then
        '        Select Case Lang
        '            Case 0
        '                MsgBox("Probe not found")
        '            Case 1
        '                MsgBox("Sonde pas trouvée")
        '        End Select
        '        Exit Sub
        '    End If
        '    If MySonde IsNot Nothing Then MySonde = New Probe
        '    MySonde = Sondes(i)
        '    Sondes(i).Show()
        'End Sub

        'Public Sub ShowSondeParIndice(ByVal i As Integer)
        '    If MySonde IsNot Nothing Then MySonde = New Probe
        '    MySonde = Sondes(i)
        '    Sondes(i).Show()
        'End Sub

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSuspendus Then
                RaiseEvent HasBeenModified()
                IsSaved = False
            End If
        End Sub

        Public Function GetSondeCount()
            GetSondeCount = MyListeSonde.ListeSonde.Count
        End Function

        Public Function GetFichierName()
            GetFichierName = FichierListeSonde
        End Function

        Public Function IsRefEssaiUnique(ByVal ref As String, ByVal TypeEssai As Integer) As Boolean
            ' Vérification de l'unicité d'une référence d'Essai d'Expansion ou de Résistance

            Dim NbEssai As Integer
            Dim unique As Boolean
            Dim RefEssaiCourant As String

            RefEssaiCourant = ""
            unique = True
            Select Case TypeEssai
                Case 0 ' Expansion
                    NbEssai = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count
                Case 1 ' Resistance
                    NbEssai = MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows.Count
            End Select

            ' Lecture des références de chaque essai dans la liste des Esais
            For i = 0 To NbEssai - 1
                Select Case TypeEssai
                    Case 0 ' Expansion
                        If MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(i).RowState <> DataRowState.Deleted Then
                            RefEssaiCourant = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(i).Item("refEssai").ToString
                            unique = (RefEssaiCourant <> ref)
                            If Not (unique) Then Exit For
                        End If
                    Case 1 ' Resistance
                        If MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows(i).RowState <> DataRowState.Deleted Then
                            RefEssaiCourant = MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows(i).Item("refEssai").ToString
                            unique = (RefEssaiCourant <> ref)
                            If Not (unique) Then Exit For
                        End If
                End Select
            Next i

            IsRefEssaiUnique = unique
        End Function

        Public Function IsNomEssaiUnique(ByVal Nom As String, ByVal TypeEssai As Integer) As Boolean
            ' Vérification de l'unicité d'une référence d'Essai d'Expansion ou de Résistance

            Dim NbEssai As Integer
            Dim unique As Boolean
            Dim NomEssaiCourant As String

            NomEssaiCourant = ""
            unique = True
            Select Case TypeEssai
                Case 0 ' Expansion
                    NbEssai = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count
                Case 1 ' Resistance
                    NbEssai = MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows.Count
            End Select

            ' Lecture des références de chaque essai dans la liste des Esais
            For i = 0 To NbEssai - 1
                Select Case TypeEssai
                    Case 0 ' Expansion
                        If MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(i).RowState <> DataRowState.Deleted Then
                            NomEssaiCourant = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(i).Item("NomEssai").ToString
                            unique = (NomEssaiCourant <> Nom)
                            If Not (unique) Then Exit For
                        End If
                    Case 1 ' Resistance
                        If MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows(i).RowState <> DataRowState.Deleted Then
                            NomEssaiCourant = MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows(i).Item("NomEssai").ToString
                            unique = (NomEssaiCourant <> Nom)
                            If Not (unique) Then Exit For
                        End If
                End Select
            Next i

            IsNomEssaiUnique = unique
        End Function


        Public Sub Show()
            MyListeSonde.Show()
        End Sub

        Public Sub MaskListeSonde()
            'FrmListeSonde.Close()
            Return
        End Sub

        Public Function GetSondeParIndex(ByVal Index As Integer) As Probe
            GetSondeParIndex = MyListeSonde.ListeSonde(Index)
        End Function

        Public Function GetSondeParRef(ByVal ref As String) As Probe
            Dim i As Integer
            GetSondeParRef = Nothing
            i = GetIndexSonde(ref)
            If i = -1 Then
                Select Case Lang
                    Case 0 : MsgBox("Probe not found")
                        GetSondeParRef = Nothing
                        Exit Function
                    Case 1 : MsgBox("La sonde n'a pas été trouvée")
                        GetSondeParRef = Nothing
                        Exit Function
                End Select
            End If
            GetSondeParRef = ListeSonde(i)
        End Function

    End Class
End Namespace
