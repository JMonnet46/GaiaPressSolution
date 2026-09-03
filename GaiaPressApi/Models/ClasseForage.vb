Namespace GaiaPress.core.V17.Models

    Public Class Forage
        Inherits DbContext

        Public Property F_ListEssais As New List(Of Essai_Forage)

        ' les proprietes
        Public F_RefChantier As String
        Public F_RefForage As String
        Public F_numeroForage As Integer
        Public F_NomForage As String
        Public F_dateForage As Date
        Public F_materiel As String
        Public F_outil As String
        Public F_abscisseX As Single
        Public F_ordonneeY As Single
        Public F_altitudeZ As Single
        Public F_coteInit As Single
        Public F_coteFinale As Single
        Public F_EventsSuspendus As Boolean
        Public F_IsSaved As Boolean
        Public Event F_HasBeenModified()
        Public F_FichierChantier As String

        ' Puis utilisez :
        Dim column As DataColumn = New DataColumn("NomColonne", GetType(String))

        ' ================================================================================
        ' Gestion de  F_ListEssais
        ' ================================================================================
        ' Add un Essai

        Public Sub AddEssai(Essai As Essai_Forage)
            Me.F_ListEssais.Add(Essai)
            ' Lier le forage au chantier (si nécessaire)
            MyForage.F_RefChantier = Me.F_RefChantier
        End Sub

        Public Function GetEssai(refEssai As String) As Essai_Forage
            For Each E As Essai_Forage In Me.F_ListEssais
                If E.E_RefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        ' Delete un Essai par référence
        Public Sub DeleteEssai(refEssai As String)
            Dim EssaiADelete As Essai_Forage = GetEssai(refEssai)
            If EssaiADelete IsNot Nothing Then
                Me.F_ListEssais.Remove(EssaiADelete)
            End If
        End Sub


        ' Mettre à jour un Essai
        Public Sub MettreAJourEssai(refEssai As String, nouveauNom As String, nouvelleDate As String)
            Dim Essai As Essai_Forage = GetEssai(refEssai)
            If Essai IsNot Nothing Then
                Essai.E_nomEssai = nouveauNom
                Essai.E_dateEssai = nouvelleDate
            End If
        End Sub

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications du Chantier ont eu lieu
            If Not F_EventsSuspendus Then
                RaiseEvent F_HasBeenModified()
                F_IsSaved = False
            End If
        End Sub

        ''' <summary>
        ''' Met à jour un essai de forage dans F_ListEssais.
        ''' Si l'essai n'existe pas, il est ajouté.
        ''' </summary>
        ''' <param name="essaiToUpdate">Essai de forage à mettre à jour ou Add.</param>
        Public Sub UpdateEssaiForage(essaiToUpdate As Essai_Forage)
            ' Vérifier si l'essai existe déjà dans F_ListEssais
            Dim existingEssai As Essai_Forage = Me.F_ListEssais.FirstOrDefault(
                Function(e) e.E_RefEssai = essaiToUpdate.E_RefEssai
            )

            If existingEssai IsNot Nothing Then
                ' ✅ Mettre à jour les propriétés de l'essai existant
                existingEssai.E_RefEssai = essaiToUpdate.E_RefEssai
                existingEssai.E_nomEssai = essaiToUpdate.E_nomEssai
                existingEssai.E_dateEssai = essaiToUpdate.E_dateEssai
                ' ... copier les autres propriétés de essaiToUpdate vers existingEssai
            Else
                ' ✅ Add le nouvel essai à la liste
                Me.F_ListEssais.Add(essaiToUpdate)
            End If
        End Sub


        Public Function GetRefForage()
            GetRefForage = F_RefForage
        End Function

        Public Function GetNumeroForage()
            GetNumeroForage = F_numeroForage
        End Function

        Public Function GetNomForage()
            GetNomForage = F_NomForage
        End Function

        Public Function GetDateForage()
            GetDateForage = F_dateForage
        End Function

        Public Function GetMateriel()
            GetMateriel = F_materiel
        End Function

        Public Function GetOutil()
            GetOutil = F_outil
        End Function

        Public Function GetabscisseX()
            GetabscisseX = F_abscisseX
        End Function

        Public Function GetOrdonneeY()
            GetOrdonneeY = F_ordonneeY
        End Function

        Public Function GetAltitudeZ()
            GetAltitudeZ = F_altitudeZ
        End Function

        Public Function GetCoteInit()
            GetCoteInit = F_coteInit
        End Function

        Public Function GetCoteFinale()
            GetCoteFinale = F_coteFinale
        End Function

        'Public Function GetEssai(NumeroEssai As Integer) As Essai_Forage
        '    GetEssai =  ListEssais(NumeroEssai).ChargeAvecBD(ref, rsEssais.Fields("RefEssai").value)
        'End Function

        'Public Function GetMyChantier() As LesChantiers
        '    GetMyChantier = MyChantier
        'End Function

        Public Function GetEssai(ByVal Index As Integer) As Essai_Forage
            Dim i As Integer
            'If MyForage.F_ListEssais Is Nothing Then
            '    ReDim Preserve MyForage.F_ListEssais(0)
            'End If
            GetEssai = Nothing
            If Index >= MyForage.F_ListEssais.Count - 1 Then Exit Function
            GetEssai = MyForage.F_ListEssais(Index)
            MyEssai = MyForage.F_ListEssais(Index)
            MyEssai.E_refSonde = MyForage.F_ListEssais(Index).E_refSonde
            For i = 0 To MyListeSondes.P_ListeSondes.Count - 1
                If MyEssai.E_refSonde = MyListeSondes.P_ListeSondes(i).P_refSonde Then
                    MySonde = MyListeSondes.P_ListeSondes(i)
                    MyEssai.E_MySonde = MySonde
                    GetEssai = MyEssai
                    Exit For
                End If
            Next i
            MyEssai.E_MySonde = MySonde
        End Function

        'Public Sub SetMyChantier(ByRef chant As Chantier)
        '    MyChantier = chant
        'End Sub

        Public Sub SetNomForage(ByVal nom As String)
            F_NomForage = nom

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetRefForage(ByVal ref As String)
            ' Attribution d'une référence au Forage
            F_RefForage = ref

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            'Call IHaveBeenModified
        End Sub

        Public Sub SetNumeroForage(ByVal numero As Integer)
            F_numeroForage = numero

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetDateForage(ByVal dateFora As String)
            F_dateForage = dateFora

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetMateriel(ByVal typeFora As String)
            F_materiel = typeFora

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetOutil(ByVal outi As String)
            F_outil = outi

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        'Public Sub SelectMyNode()
        '    'MyNodeForage.Selected = True
        '    MyNodeForage.EnsureVisible()
        'End Sub

        Public Sub SetAbscisseX(ByVal abscisse As Single)
            F_abscisseX = abscisse

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetOrdonneeY(ByVal ordonnee As Single)
            F_ordonneeY = ordonnee

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetAltitudeZ(ByVal altitude As Single)
            F_altitudeZ = altitude

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub


        Public Sub SetCoteInit(ByVal cote As Single)
            F_coteInit = cote

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        Public Sub SetCoteFinale(ByVal cote As Single)
            F_coteFinale = cote

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()
        End Sub

        'Public Sub SetEssai(ByRef Essai As Object, ByVal indice As Integer)

        '    '    MsgBox "NouvelEssai"

        '    ' vérification de l'indice de l'essai
        '    If indice > EssaiCount - 1 Then Exit Sub

        '    ' ******************* SAUVEGARDE ***************************
        '    ' Enregistrement du nouveau Essai dans le tableau de Essai
        '    ListEssais(indice) = Essai
        '    ' **********************************************************

        '    ' Enregistrement du forage auprès de son nouvel Essai
        '    Call Essai.SetMyForage(Me)

        '    ' Ouverture du noeud de forage
        '    'MyNodeForage.IsExpanded = True

        'End Sub

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

        'Public Sub Show(fora As Forage)
        '    If Not (MyFenetreForage Is Nothing) Then
        '        MyFenetreForage.ShowDialog()
        '    Else
        '        MyFenetreForage = New FrmForage(fora)
        '    End If
        'End Sub

        'Public Sub ShowEssai(ByVal index As Integer)
        '    Dim i As Integer
        '    ' Sélection de MyEssai à partir de Index dans Forage.ListEssais
        '    MyEssai = MyForage.F_ListEssais(index)
        '    'MyForage.F_ListEssais(index).Show()

        '    ' Sélection de MySonde, MyResistance, MyExpansion

        '    If MySonde Is Nothing Then MySonde = New Probe
        '    'If MyEssai.MySonde Is Nothing Then
        '    'End If
        '    MySonde = MyEssai.MySonde
        '    If MySonde.Resistance IsNot (Nothing) Then MyResistance = MySonde.MaResistance
        '    If MySonde.MaExpansion IsNot (Nothing) Then MyExpansion = MySonde.MaExpansion
        '    FrmEssai.Show()
        'End Sub

        'Public Sub MaskForage()
        '    MyFenetreForage.Close()
        'End Sub

        'Public Sub CloseForage()
        '    Call MaskForage()
        '    Dim i As Integer

        '    For i = 0 To MyForage.EssaiCount - 1
        '        If MyForage.F_ListEssais(i) IsNot (Nothing) Then
        '            MyForage.F_ListEssais(i).CloseEssaiForage()
        '            MyForage.F_ListEssais(i) = Nothing
        '        End If
        '    Next

        '    'If MyForage.F_ListEssais IsNot (Nothing) Then
        '    '    For i = 0 To MyForage.EssaiCount - 1
        '    '        MyForage.F_ListEssais(i).CloseEssaiForage()
        '    '        MyForage.F_ListEssais(i) = Nothing
        '    '    Next
        '    'End If
        'End Sub

        'Public Sub SuspendreEvents()
        '    EventsSuspendus = True
        'End Sub

        'Public Sub RetablirEvents()
        '    EventsSuspendus = False
        'End Sub

        ' Constructeur de Forage
        'Public Sub New(Retour As Boolean)
        '    If Retour Then
        '        Dim fora As Forage
        '        Retour = Not (Retour)
        '        fora = New Forage(Retour)
        '        MyFenetreForage = New FrmForage(fora)
        '    End If
        '    'fora = New Forage
        '    ' Création de la feuille de forage
        '    ' Is Created est True à la fin de l'initialisation

        '    'MyChantier. C_ListForages.Count = MyChantier. ForageCount + 1
        '    ' Enregistrement de la référence du forage auprès de sa feuille
        '    If MyFenetreForage IsNot (Nothing) Then Call MyFenetreForage.SetMyForage(Me)
        '    numeroForage = 0
        '    NomForage = "Nouveau forage"
        '    DateForage = DateValue(Now)
        '    materiel = "à remplir"
        '    outil = "à remplir"
        '    abscisseX = 0
        '    ordonneeY = 0
        '    altitudeZ = 0
        '    coteInit = 0
        '    coteFinale = 0

        'End Sub

        Public Sub NouveauEssai(ByVal NewTable As Boolean)
            ' Déclaration d'un essai

            '    MsgBox "NewEssai"
            If NewTable Then  ' On ajoute un Essai à MonFichier.Tables("LesEssaisForage") seulement si Table est vrai
                If MonChantier.Tables("LesEssaisForage") Is Nothing Then
                    MonChantier.Tables.Add("LesEssaisForage")
                    With MonChantier.Tables("LesEssaisForage")
                        .Columns.Add("refChantier", GetType(String))
                        .Columns.Add("refForage", GetType(String))

                        Dim columns() As Datacolumn
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
                    MonChantier.Tables("LesEssaisForage").NewRow()
                End If
            End If

            ' Incrémentation du compteur d'essais
            'EssaiCount = EssaiCount + 1

            ' Redimensionnement du tableau dynamique de Essais
            'ReDim Preserve ListEssais(EssaiCount - 1)

            ' Instanciation du nouveau Essai
            'ListEssais(EssaiCount - 1) = New Essai_Forage(False)
            Debug.WriteLine("Instanciation du nouveau Essai")

            'If MyForage.F_ListEssais IsNot (Nothing) Then
            '     EssaiCount = MyForage.F_ListEssais.Count
            'Else
            '     EssaiCount = 0
            'End If
            'Debug.WriteLine("Incrémentation du compteur d'essais  EssaiCount =" & Essai.Count.ToString)
            'Debug.WriteLine("Incrémentation du compteur d'essais  RefForage =" & RefForage.ToString)
            ' Debug.WriteLine("Incrémentation du compteur d'essais MyEssai.RefEssai =" & Str(MyEssai.RefEssai))

            ' Redimensionnement du tableau dynamique de Essais
            ' Add un nouvel essai à la liste (sans ReDim)
            ' Vérification que NewEssai est bien instancié

            ' Instanciation du nouveau Essai
            Debug.WriteLine("Instanciation du nouveau Essai")
            'NewEssai = New Essai_Forage(False)

            ' Initialisation de la liste si elle est Nothing
            'If  ListEssais Is Nothing Then
            '    Debug.WriteLine("Initialisation de la liste si elle est Nothing Me.RefForage = " &  RefForage.ToString)
            '    ReDim  ListEssais(10)
            'End If

            'If  EssaiCount > 0 Then
            'If NewEssai IsNot Nothing Then

            '    ' Incrémentation du compteur d'essais
            '    ' EssaiCount =  ListEssais.Count + 1

            '    ' Ajout du nouvel essai à la liste
            '    ReDim Preserve  ListEssais( EssaiCount)
            '     ListEssais( EssaiCount) = New Essai_Forage(False)
            '    Debug.WriteLine("Incrémentation du compteur d'essais  EssaiCount =" &  EssaiCount.ToString)

            'Else
            '    Debug.Print("Erreur : NewEssai est Nothing.")
            'End If

            ' Enregistrement du nouveau Essai dans le tableau de Essai

            Debug.WriteLine("Enregistrement du nouveau Essai dans le tableau de Essai")

            ' ListEssais( EssaiCount - 1) = NewEssai

            '' Enregistrement du forage auprès de son nouvel Essai
            'Call NewEssai.SetMyForage(Me)

            '' Attribution d'une référence au nouvel essai
            'With MyForage.F_ListEssais(EssaiCount - 1)
            '    .RefEssai = GiveMeANewRefEssai()
            '    .RefChantier = MyChantier.RefChantier
            '    .RefForage = MyForage.RefForage

            '    ' Attribution d'une sonde au nouvel essai
            '    If MySonde IsNot Nothing Then MySonde = New Probe
            '    MySonde = MyListeSondes.GetSondeParIndex(0)
            '    .SetMySonde(MySonde)

            'End With

            MyEssai = F_ListEssais(F_ListEssais.Count - 1)

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

            ' Ouverture du noeud de forage
            'MyNodeForage.Expand()

        End Sub ' Fin de NouveauEssai

        'Public Sub SupEssai(ByVal ref As String)
        '    Dim i As Integer    ' Index de l'essai à Delete
        '    Dim J As Integer

        '    i = GetIndexEssai(ref)

        '    ' Effaçage du Essai
        '    ListEssais(i) = Nothing

        '    ' Décalage des Essais suivant d'une case
        '    For J = i To EssaiCount - 1
        '        ListEssais(J) = ListEssais(J + 1)
        '    Next

        '    ' Redimensionnement du tableau  ListEssais
        '    If EssaiCount > 1 Then
        '        ReDim Preserve ListEssais(EssaiCount - 1)
        '    Else
        '        ReDim ListEssais(0)

        '        '' Redimensionnement du tableau ListEssais
        '        'If  EssaiCount > 1 Then
        '        '    ' ReDim Preserve  ListEssais( EssaiCount - 1)
        '        '    ReDim  ListEssais.RemoveAt( EssaiCount - 1) ' Supprime l'élément à l'indice spécifié


        '        '    'Else
        '        '    '    ' ReDim  ListEssais(0)
        '        '    '     ListEssais.Clear()
        '        'End If

        '        '' Décrémentation du compteur de Essais
        '        ' EssaiCount =  EssaiCount - 1
        '    End If

        '    MyFrmEssai.UpDateLesEssaisForage(ref)

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu
        '    Call IHaveBeenModified()

        'End Sub

        Public Function GetIndexEssai(ref As String) As Integer
            ' Recherche de l'essai ayant pour Nom 'ref' avec arrêt dès que possible
            Dim i As Integer
            For i = 0 To F_ListEssais.Count - 1
                If F_ListEssais(i).E_RefEssai = ref Then Exit For
            Next i
            If i = F_ListEssais.Count Then
                Select Case Lang
                    Case 0
                        MsgBox("Test not found")
                    Case 1
                        MsgBox("L'Essai n'est pas trouvé")
                End Select
            End If
            GetIndexEssai = i
        End Function

        Public Function GetIndexEssaiParNom(Nom As String) As Integer
            ' Recherche de l'essai ayant pour référence 'ref' avec arrêt dès que possible
            Dim i As Integer
            For i = 0 To F_ListEssais.Count - 1
                If F_ListEssais(i).E_nomEssai = Nom Then Exit For
            Next i
            If i = F_ListEssais.Count Then
                Select Case Lang
                    Case 0
                        MsgBox("Test not found")
                    Case 1
                        MsgBox("L'Essai n'est pas trouvé")
                End Select
            End If
            GetIndexEssaiParNom = i
        End Function

        'Public Sub IHaveBeenModified()
        '    ' Génère un événement indiquant que des modifications du Forage ont eu lieu
        '    If Not EventsSuspendus Then
        '        Call MyChantier.IHaveBeenModified()
        '        'RaiseEvent HasBeenModified
        '    End If
        'End Sub

        Public Function GiveMeANewRefEssai()
            ' Détermination d'une nouvelle référence pour un essai
            Dim k As Integer
            '    MsgBox "GiveMeANewRefEssai"
            k = 1
            Do While Not MyListeEssaisForage.IsRefUnique("RefEssai " & k)
                k = k + 1
            Loop
            GiveMeANewRefEssai = "RefEssai " & k
        End Function

        'Public Sub SelectMyNode()
        '    MyNodeForage.IsSelected = True
        'End Sub


    End Class
End Namespace