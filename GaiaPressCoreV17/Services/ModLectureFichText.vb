Imports System.IO
Imports System.Threading
Imports GaiaPressCoreV17.Services
Imports iText.Kernel.Geom
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Services
    Module ModLectureFichText

        ' ******************************************************
        '  Lecture des anciens fichiers Gaiapress
        ' ******************************************************
        ' Passer l'instance de Probe (MySonde) à la méthode
        Dim essaiResistance = MySonde.GetEssaiResistanceParRef("ESSAI_001", 1)

        Public Function LectureEssaiTexte(FichierChantier As String, Essai As Essai_Forage) As Boolean
            On Error GoTo errorFind

            Dim i, l As Integer
            Dim TextLine As String
            'Dim MySonde As Probe

            Call MyListeSonde.NouvelleSonde(True, True)
            If MySonde Is Nothing Then MySonde = New Probe
            MySonde = MyListeSonde.GetSondeParIndex(MyListeSonde.GetSondeCount - 1)
            'Call MySonde.SuspendreEvents()
            Call Essai.SuspendreEvents()

            ' Ouverture du fichier
            Dim objStreamReader As StreamReader

            'Pass the file path and the file name to the StreamReader constructor.
            objStreamReader = New StreamReader(FichierChantier)

            ''Read the first line of text.
            'TextLine = objStreamReader.ReadLine

            ''Continue to read until you reach the end of the file.
            'Do While Not TextLine Is Nothing

            '    'Write the line to the Console window.
            '    Console.WriteLine(TextLine)

            '    'Read the next line.
            '    TextLine = objStreamReader.ReadLine
            'Loop

            'Console.ReadLine()

            ' Effectue la boucle jusqu'à la fin du fichier.
            'Read the first line of text.
            TextLine = objStreamReader.ReadLine

            'Continue to read until you reach the end of the file.
            Do While Not TextLine Is Nothing

                'Write the line to the Console window.
                Console.WriteLine(TextLine)

                'Read the next line.
                TextLine = objStreamReader.ReadLine


                Dim Chaine As String
                Chaine = TextLine
                l = 0
                If InStr(Chaine, ":") <> 0 Then
                    i = i + 1
                    'Chaine = RecupApres2Points(Chaine)
                    Chaine = RTrim(LTrim(Right(Chaine, Len(Chaine) - InStr(Chaine, ":"))))
                    With Essai
                        Select Case i
                            Case 3
                                .nomEssai = Chaine
                            Case 4
                                .dateEssai = StringToDate(Chaine)
                            Case 6
                                If Chaine = "TypeDeformee.nue" Then MySonde.TypeDeformee = TypeDeDeformee.BallonRugby
                                If Chaine = "LANTERNE" Then MySonde.TypeDeformee = TypeDeDeformee.Lanterne
                            Case 7
                                If Chaine = "MOYENNE" Then MySonde.TypeContrainte = TypeDeContrainte.Moyenne
                                If Chaine = "TypeDefo.lame" Then MySonde.TypeContrainte = TypeDeContrainte.Lame
                            Case 9
                                MySonde.lt = 10 * Val(Chaine)
                            Case 10
                                MySonde.Lg = 10 * Val(Chaine)
                            Case 11
                                MySonde.Ls = 10 * Val(Chaine)
                            Case 12
                                MySonde.Vc = Val(Chaine)
                            Case 13
                                MySonde.Ds = 10 * Val(Chaine)
                            Case 14
                                MySonde.EpMembraneGarde = 10 * Val(Chaine)
                            Case 15
                                MySonde.RetablirEvents()
                                MySonde.EpMembraneMesure = 10 * Val(Chaine) - MySonde.GetEpMembraneGarde
                            Case 16
                                .difPresEauAir = Val(Chaine)
                            Case 17
                                .presP0 = Val(Chaine)
                            Case 18
                                .presFluage = Val(Chaine)
                            Case 19
                                .presLimite = Math.Round(Val(Chaine))
                            Case 20
                                .poidsVol = Val(Chaine)
                            Case 21
                                .poidsVolDejauge = Val(Chaine)
                            Case 22
                                .coefPoisson = Val(Chaine)
                            Case 23
                                .modElastique = Math.Round(Val(Chaine))
                            Case 25
                                .Cohesion = Val(Chaine)
                            Case 26
                                .Frottement = Val(Chaine)
                            Case 27
                                .hauteurCpv = Val(Chaine)
                            Case 28
                                .profNappe = Val(Chaine)
                            Case 29
                                .profSonde = Val(Chaine)
                        End Select
                    End With
                End If

                Chaine = TextLine

                If i > 28 And Chaine <> "" Then
                    Dim indice As Single
                    Dim Pres As Single
                    Dim vol15 As Single
                    Dim vol30 As Single
                    Dim vol60 As Single
                    Dim cont As Single
                    Dim Def As Single

                    indice = Val(RecupColonne(Chaine, 1))
                    If indice <> 0 Then
                        Pres = Valeurs(RecupColonne(Chaine, 2))
                        vol15 = Valeurs(RecupColonne(Chaine, 3))
                        vol30 = Valeurs(RecupColonne(Chaine, 4))
                        vol60 = Valeurs(RecupColonne(Chaine, 5))
                        cont = Valeurs(RecupColonne(Chaine, 6))
                        Def = Valeurs(RecupColonne(Chaine, 7))

                        Call Essai.NouvelleMesure(False, l, l, Pres, vol15, vol30, vol60, cont, 0, Def, 0, 0, 0, 0, 0, 0)
                        l = l + 1
                    End If
                End If

            Loop 'fin du fichier EOF
            If i < 29 Then GoTo errorFind

            Call Essai.RetablirEvents()
            Call Essai.SetMySonde(MySonde)

            'Close the file.
            objStreamReader.Close()

            LectureEssaiTexte = True
            Exit Function

errorFind:
            LectureEssaiTexte = False

        End Function

        ' ******************************************************
        '  Lecture des fichiers texte Jean Lutz
        ' ******************************************************

        Public Function LectureEssaiJeanLutzTexte(ByVal FichierChantier As String) As Boolean
            '    Dim ListeDesEssais() As EssaiForage
            Dim Essai As Essai_Forage
            Dim EssaiResistance As EssaiResistance
            Dim EssaiExpansion As EssaiExpansion
            Dim Sonde As Probe
            Dim MyExpansion As EssaiExpansion
            Dim MyResistance As EssaiResistance

            On Error GoTo errorFind

            Dim i, J, k, Temps, Index As Integer
            Dim TextLine, Chaine, NomForage, RefSonde, NomEssai As String
            Dim RefEssai As String
            '    Dim MySonde As Probe
            Dim vol15, vol30, vol60, Pres, cont, Def As Single
            Dim FinEssai As Boolean
            Dim MyTypeEssai As Integer

            ' Ouverture du fichier
            Dim objStreamReader As StreamReader

            ''Pass the file path and the file name to the StreamReader constructor.
            'objStreamReader = New StreamReader(FichierChantier)

            ''Read the first line of text.
            'TextLine = objStreamReader.ReadLine

            ''Continue to read until you reach the end of the file.
            'Do While Not TextLine Is Nothing

            '    'Write the line to the Console window.
            '    Console.WriteLine(TextLine)

            '    'Read the next line.
            '    TextLine = objStreamReader.ReadLine
            'Loop

            ' Initialisation
            Essai = Nothing
            EssaiResistance = Nothing

            ' Création d'une nouvelle sonde
            Call MyListeSonde.NouvelleSonde(True, False)

            ' assignation à MySonde de la nouvelle sonde
            If MySonde IsNot Nothing Then MySonde = New Probe
            MySonde = MyListeSonde.GetSondeParIndex(MyListeSonde.GetSondeCount - 1)

            ' Définition de la nouvelle référence de sonde
            '    frmSondeFr.Show
            '    FrmDialogSonde.Show
            '    FrmDialogSonde.SetFocus

            '    Set Sonde = New Probe

            Call MySonde.SuspendreEvents()
            Call MyEssaiForage.SuspendreEvents()

            'Pass the file path and the file name to the StreamReader constructor.
            objStreamReader = New StreamReader(FichierChantier)

            'Read the first line of text.
            TextLine = objStreamReader.ReadLine

            'Continue to read until you reach the end of the file.
            Do While Not TextLine Is Nothing

                'Read the next line.
                TextLine = objStreamReader.ReadLine

                Chaine = TextLine

                ' Détection d'un nouveau Forage --- Forage --- Forage --- Forage --- Forage --- Forage --- Forage --- Forage ---
                If InStr(Chaine, "Forage") <> 0 Then

                    Do While Not EOF(1)
                        i = i + 1

                        'Read the next line.
                        TextLine = objStreamReader.ReadLine

                        Chaine = TextLine

                        If i = 1 Then

                            ' Mémorisation du nom du Forage
                            NomForage = RecupAvantTab(Chaine)
                            Call MyForage.SetNomForage(NomForage)

                            ' Mémorisation de la Référence de la sonde
                            RefSonde = "Ref" & NomForage
                            Call MySonde.SetRefSonde(RefSonde)
                        End If

                        ' Détection d'une calibration, d'un étalonnage, d'un nouvel essai -- calibratoion  -- étalonnage -- nouvel essai --
                        If InStr(Chaine, "Type essai") <> 0 Then
                            J = J + 1
                            'TextLine = objStreamReader.ReadLine
                            'Read the next line.
                            TextLine = objStreamReader.ReadLine
                            Chaine = TextLine

                            ' Le calibrage correspond à un essai d'expansion propre--- Début Expansion propre -- Début Expansion propre -
                            ' Détection d'un nouveau essai Calibrage --- Calibrage --- Calibrage --- Calibrage --- Calibrage -- Calibrage -- Calibrage
                            If InStr(Chaine, "Calibrage") <> 0 Then

                                ' Création d'un nouvel essai d'expansion
                                'MyExpansion = MySonde.P_Expansion.NouvelEssai(EssaiCalib.Expansion)
                                ' ? Utilisation simple
                                'MyExpansion = MySonde.P_MaExpansion.NouvelEssai(EssaiCalib)
                                ' ? Corrigé : Utiliser une instance de EssaiExpansion
                                Dim essaiCalibInstance As New EssaiExpansion()
                                essaiCalibInstance.RefEssai = "Calibrag 001" ' Exemple
                                essaiCalibInstance.nomEssai = "Essai de calibrage"
                                ' ✅ Corrigé : Utiliser EssaiExpansion.NouvelEssaiExpansion
                                'Dim essaiCalibInstance As New EssaiExpansion()
                                essaiCalibInstance.RefEssai = "Expansion_001"
                                essaiCalibInstance.nomEssai = "Essai de calibrage en Expansion"
                                'MyExpansion = EssaiExpansion.NouvelEssaiExpansion(essaiCalibInstance, MySonde.P_Expansion, True)
                                ' ✅ Corrigé : Passer MySonde (Probe) au lieu de MySonde.P_Expansion (List)
                                MyExpansion = EssaiExpansion.NouvelEssai(True)

                                ' Enregistrement du nom de l'essai d'expansion
                                Chaine = TextLine
                                NomEssai = RecupAvantTab(Chaine)
                                Call MyExpansion.SetNomEssai(NomEssai)

                                ' Il faut déclarer cette nouvelle sonde dans la liste des sondes
                                '                        Call MySonde.NouvelEssai(MyExpansion)

                                ' Enregistrement de la date de l'essai d'expansion
                                '                        Chaine = TextLine
                                '                        Chaine = RecupApres5Tab(Chaine)
                                '                        Chaine = RecupAvantTab(Chaine)
                                '                        Call MyExpansion.SetDateEssai(CDate(Chaine))

                                ' Enregistrement de la date de l'MyExpansion
                                Chaine = TextLine
                                Chaine = RecupApres5Tab(Chaine)
                                Chaine = RecupAvantTab(Chaine)
                                Chaine = RecupAvantTab1(Chaine)
                                Call MyExpansion.SetDateEssai(CDate(Chaine))


                                ' On saute 1 ligne pour lire les paramètres de la sonde
                                '                        J = J + 1
                                'TextLine = objStreamReader.ReadLine
                                'Read the next line.
                                TextLine = objStreamReader.ReadLine
                                Chaine = TextLine
                                J = J + 1

                                'TextLine = objStreamReader.ReadLine
                                'Read the next line.
                                TextLine = objStreamReader.ReadLine

                                ' On déclare un nouvel essai de type ExpansionPropre

                                ' Récupération du diamètre du tube de calibration
                                Chaine = TextLine
                                Chaine = RecupApresTab(Chaine)
                                Chaine = RecupAvantTab(Chaine)
                                MyExpansion.SetDi(Valeurs(Chaine))

                                '                        MyExpansion = MySonde.GetEssai("TFC", MyExpansion)


                                ' Récupération de la longueur de la sonde
                                Chaine = TextLine
                                Chaine = RecupApres2Tab(Chaine)
                                Chaine = RecupAvantTab(Chaine)
                                Call MySonde.Setls(Valeurs(Chaine))

                                ' On saute 1 ligne pour lire les mesures
                                J = J + 1

                                'TextLine = objStreamReader.ReadLine
                                'Read the next line.
                                TextLine = objStreamReader.ReadLine
                                Chaine = TextLine

                                FinEssai = False
                                k = 0
                                ' Boucle de lecture des différentes pressions d'essai
                                Do While Not (FinEssai)

                                    ' Boucle de lecture de volume aux temps de 1, 15, 30 et 60s

                                    Do While Not EOF(1)

                                        k = k + 1
                                        'TextLine = objStreamReader.ReadLine
                                        'Read the next line.
                                        TextLine = objStreamReader.ReadLine

                                        ' Critère de fin d'essai
                                        If TextLine = "" Then
                                            FinEssai = True
                                            Exit Do
                                        End If

                                        ' Récupération de la pression différentielle
                                        Chaine = TextLine
                                        Chaine = RecupApres5Tab(Chaine)
                                        Chaine = RecupAvantTab(Chaine)
                                        Call MyExpansion.SetDifPresEauAir(-100 * Valeurs(Chaine))

                                        ' Récupération du temps de la mesure
                                        Chaine = TextLine
                                        Chaine = RecupApres2Tab(Chaine)
                                        Chaine = RecupAvantTab(Chaine)
                                        Temps = Valeurs(Chaine)

                                        Select Case Temps
                                            Case 15
                                                ' Enregistrement du volume à 15s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol15 = Valeurs(Chaine)

                                            Case 30
                                                ' Enregistrement du volume à 30s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol30 = Valeurs(Chaine)

                                            Case 60
                                                ' Enregistrement du volume à 60s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol60 = Valeurs(Chaine)

                                                ' Enregistrement de la pression à 60s
                                                Chaine = TextLine
                                                Chaine = RecupApres3Tab(Chaine)
                                                Chaine = RecupAvantTab(Chaine)
                                                Pres = Valeurs(Chaine) * 100

                                                Exit Do
                                        End Select

                                    Loop 'fin de lecture d'une pression d'essai

                                    ' enregistrement de la nouvelle mesure d'expansion propre
                                    If Not (FinEssai) Then
                                        Call MySonde.MaExpansion.SetNouvelleMesureExpansion(k - 1, Pres, vol15, vol30, vol60)
                                    End If

                                Loop

                                ' Enregistrement du nouveau Essai dans le tableau d'essais de MySonde
                                Call MySonde.SetEssai(EssaiCalib.Expansion, MyExpansion, MyEssaiForageExpansionCount - 1)

                                ' enregistrement de la sonde dans la liste des sondes
                                Index = MyListeSonde.GetIndexSonde(MySonde.GetRefSonde)
                                Call MyListeSonde.UpdateProbe(MySonde)

                            End If ' Fin de lecture d'un essai d'expression propre --- Fin Expansion propre ---- Fin Expansion propre -

                            ' Le calibrage correspond à un essai de Résistancepropre--- Début Résistance propre -- Début Résistance propre -
                            If InStr(Chaine, "Etalonnage") <> 0 Then

                                ' L'étalonnage correspond à un essai de résistance propre

                                ' On utilise la sonde crée lors de l'essai d'expansion propre
                                ' on ne crée pas de nouvelle sonde

                                ' déclaration d'un nouvel essai de resistance propre
                                Dim NewEssaiResistance As New EssaiResistance

                                MyListeEssaiResistanceSession.Add(NewEssaiResistance)
                                'MyResistance.AddEssaiResistance(NewEssaiResistance)

                                ' Enregistrement du nom de l'essai de résistance propre
                                Chaine = TextLine
                                Chaine = RecupAvantTab(Chaine)
                                '                        Chaine = RecupApres5Tab(Chaine)
                                '                        Chaine = RecupAvantTab(Chaine)
                                Call EssaiResistance.SetNomEssai(Chaine)

                                ' Il faut déclarer cette nouvelle sonde dans la liste des sondes
                                '                        Call MySonde.NouvelEssai(MyExpansion)

                                ' Enregistrement de la date de l'essai d'expansion
                                '                        Chaine = TextLine
                                '                        Chaine = RecupApres5Tab(Chaine)
                                '                        Chaine = RecupAvantTab(Chaine)
                                '                        Call MyResistance.SetDateEssai(CDate(Chaine))

                                ' Enregistrement de la date de l'MyExpansion
                                Chaine = TextLine
                                Chaine = RecupApres5Tab(Chaine)
                                Chaine = RecupAvantTab(Chaine)
                                Chaine = RecupAvantTab1(Chaine)
                                Call MyResistance.SetDateEssai(CDate(Chaine))


                                ' On saute 1 ligne pour lire les paramètres de la sonde
                                '                        J = J + 1
                                'Read the next line.
                                TextLine = objStreamReader.ReadLine
                                Chaine = TextLine
                                J = J + 1
                                TextLine = objStreamReader.ReadLine
                                Chaine = TextLine


                                ' Récupération et enregistrement du nom de la sonde
                                '                        Chaine = TextLine
                                '                        Chaine = RecupAvantTab(Chaine)
                                '                        Call MySonde.SetRefSonde(Chaine)
                                '
                                ' Enregistrement du nom de l'essai d'Expansion propre ou Calibrage
                                '                        Call MyEssaiForage.SetMySonde(MySonde)


                                '                        If MySonde.GetRefSonde = "TFC" Then
                                '                            MySonde.SetTypeContrainte (Moyenne)
                                '                            MySonde.SetTypeDeformee (Lanternee)
                                '                            MySonde.SetTypeSonde (TubeFenduCourte)
                                '                        End If

                                ' On déclare un nouvel essai de type ExpansionPropre

                                ' Récupération du diamètre extérieur de la sonde
                                Chaine = TextLine
                                Chaine = RecupApresTab(Chaine)
                                Chaine = RecupAvantTab(Chaine)
                                MySonde.Setds(Valeurs(Chaine))

                                '                        MyResistance = MySonde.GetEssai("TFC", MyExpansion)

                                ' set mysonde.

                                ' Enregistrement du diamètre intérieur du tube de calibration
                                ' Call MySonde.(Valeurs(Chaine))

                                ' Récupération de la longueur de la sonde
                                Chaine = TextLine
                                Chaine = RecupApres2Tab(Chaine)
                                Chaine = RecupAvantTab(Chaine)
                                Call MySonde.Setls(Valeurs(Chaine))

                                ' On saute 1 ligne pour lire les mesures
                                J = J + 1
                                TextLine = objStreamReader.ReadLine
                                Chaine = TextLine

                                Chaine = TextLine

                                FinEssai = False
                                ' Boucle de lecture des différentes pressions d'essai
                                k = 0
                                Do While Not (FinEssai)

                                    ' Boucle de lecture de volume aux temps de 1, 15, 30 et 60s

                                    Do While Not EOF(1)

                                        k = k + 1
                                        TextLine = objStreamReader.ReadLine

                                        ' Critère de fin d'essai
                                        If TextLine = "" Then
                                            FinEssai = True
                                            Exit Do
                                        End If

                                        ' Récupération de la pression différentielle
                                        Chaine = TextLine
                                        Chaine = RecupApres5Tab(Chaine)
                                        Chaine = RecupAvantTab(Chaine)
                                        Call MyResistance.SetDifPresEauAir(-100 * Valeurs(Chaine))

                                        ' Récupération du temps de la mesure
                                        Chaine = TextLine
                                        Chaine = RecupApres2Tab(Chaine)
                                        Chaine = RecupAvantTab(Chaine)
                                        Temps = Valeurs(Chaine)

                                        Select Case Temps
                                            Case 15
                                                ' Enregistrement du volume à 15s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol15 = Valeurs(Chaine)

                                            Case 30
                                                ' Enregistrement du volume à 30s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol30 = Valeurs(Chaine)

                                            Case 60
                                                ' Enregistrement du volume à 60s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol60 = Valeurs(Chaine)

                                                ' Enregistrement de la pression à 60s
                                                Chaine = TextLine
                                                Chaine = RecupApres3Tab(Chaine)
                                                Chaine = RecupAvantTab(Chaine)
                                                Pres = Valeurs(Chaine) * 100

                                                Exit Do
                                        End Select

                                    Loop 'fin de lecture d'une pression d'essai

                                    ' enregistrement de la nouvelle mesure d'expansion propre
                                    If Not (FinEssai) Then
                                        Call MyResistance.MyResistanceNouvelleMesure(k, Pres, vol60)
                                    End If
                                    ' --- Fin Résistance propre -- --- Fin Résistance propre -- --- Fin Résistance propre -- --- Fin Résistance propre --
                                Loop

                                ' Enregistrement du nouveau Essai dans le tableau d'essais de MySonde
                                Call MySonde.SetEssai(EssaiCalib.Resistance, MyResistance, MyEssaiForageResistanceCount - 1)

                                ' enregistrement de la sonde dans la liste des sondes
                                Index = MyListeSonde.GetIndexSonde(MySonde.GetRefSonde)
                                Call MyListeSonde.UpdateProbe(MySonde)



                            End If ' Fin de lecture d'un essai d'expression propre

                            ' Détection d'un nouvel Essai --- Essai --- Essai --- Essai --- Essai --- Essai --- Essai --- Essai ---
                            If InStr(Chaine, "Essai") <> 0 Then

                                ' Creation nouvel objet Essai
                                ' Essai = New Essai_Forage(True)
                                Essai = New Essai_Forage ' True pour créer la Table

                                Index = 0
                                ' Déclaration d'un nouvel essai sur MonForage
                                If MonChantier.Tables("LesEssaisForage") Is Nothing Then
                                    Call MyForage.NouveauEssai(True)
                                Else
                                    Call MyForage.NouveauEssai(False)
                                End If

                                Essai.RefForage = MyForage.RefForage
                                Essai.nomEssai = ""
                                Essai.refEssai = MyRefEssai
                                Essai.profSonde = 0
                                ' Call Essai.Set Mesures.count(0)
                                'Essai. Mesures.Count = 0

                                ' Enregistrement du nom de l'essai
                                ' Attribution d'une référence au nouvel essai

                                Chaine = RecupAvantTab(Chaine)
                                Essai.nomEssai = Chaine

                                ' Essai. RefEssai = MyForage. ListEssais(MyEssaiForageCount - 1).RefEssai
                                ' MyForage ne porte pas de propriété ListEssais dans le modèle fourni.
                                ' La référence importée est déjà affectée à partir de MyRefEssai ci-dessus.

                                ' Enregistrement de la profondeur de l'essai
                                Chaine = TextLine
                                Chaine = RecupApres4Tab(Chaine)
                                Chaine = RecupAvantTab(Chaine)
                                Essai.profSonde = Valeurs(Chaine)

                                ' Enregistrement de la date de l'MyEssaiForage
                                Chaine = TextLine
                                Chaine = RecupApres5Tab(Chaine)
                                Chaine = RecupAvantTab(Chaine)
                                Chaine = RecupAvantTab1(Chaine)
                                Essai.dateEssai = CDate(Chaine)

                                ' Enregistrement de l'heure de l'MyEssaiForage
                                Chaine = TextLine
                                Chaine = RecupApres5Tab(Chaine)
                                Chaine = RecupApresTab(Chaine)
                                Chaine = RecupApresTab1(Chaine)
                                Essai.heureEssai = CDate(Chaine)

                                ' On saute 2 ligne pour lire les paramètres de la sonde
                                J = J + 1
                                TextLine = objStreamReader.ReadLine
                                Chaine = TextLine

                                J = J + 1
                                TextLine = objStreamReader.ReadLine
                                Chaine = TextLine

                                ' Récupération du nom de la sonde
                                '                        Chaine = TextLine
                                '                        Chaine = RecupAvantTab(Chaine)

                                If MySonde.GetRefSonde = "TFC" Then MySonde.SetTypeDeformee(TypeDeDeformee.Lanterne)

                                ' Récupération du diamètre de la sonde
                                '                        Chaine = TextLine
                                '                        Chaine = RecupApresTab(Chaine)
                                '                        Chaine = RecupAvantTab(Chaine)
                                '                        Call MySonde.SetDs(Valeurs(Chaine))

                                ' Récupération de la longueur de la sonde
                                '                        Chaine = TextLine
                                '                        Chaine = RecupApres2Tab(Chaine)
                                '                        Chaine = RecupAvantTab(Chaine)
                                '                        Call MySonde.Setls(Valeurs(Chaine))

                                ' On saute 1 ligne pour lire les mesures
                                J = J + 1

                                TextLine = objStreamReader.ReadLine
                                Chaine = TextLine

                                FinEssai = False
                                k = 0
                                ' Boucle de lecture des différentes pressions d'MyEssaiForage
                                Do While Not (FinEssai)

                                    ' Boucle de lecture de volume aux temps de 1, 15, 30 et 60s

                                    Do While Not EOF(1)

                                        k = k + 1
                                        TextLine = objStreamReader.ReadLine

                                        ' Critère de fin d'essai
                                        If TextLine = "" Then
                                            FinEssai = True
                                            Exit Do
                                        End If

                                        ' Récupération de la pression différentielle
                                        Chaine = TextLine
                                        Chaine = RecupApres5Tab(Chaine)
                                        Chaine = RecupAvantTab(Chaine)
                                        Essai.difPresEauAir = -100 * Valeurs(Chaine)


                                        ' Récupération du temps de la mesure
                                        Chaine = TextLine
                                        Chaine = RecupApres2Tab(Chaine)
                                        Chaine = RecupAvantTab(Chaine)
                                        Temps = Valeurs(Chaine)

                                        Select Case Temps
                                            Case 15
                                                ' Enregistrement du volume à 15s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol15 = Valeurs(Chaine)

                                            Case 30
                                                ' Enregistrement du volume à 30s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol30 = Valeurs(Chaine)

                                            Case 60
                                                ' Enregistrement du volume à 60s
                                                Chaine = TextLine
                                                Chaine = RecupApres6Tab(Chaine)
                                                ' Chaine = RecupAvantTab(Chaine)
                                                vol60 = Valeurs(Chaine)

                                                ' Enregistrement de la pression à 60s
                                                Chaine = TextLine
                                                Chaine = RecupApres3Tab(Chaine)
                                                Chaine = RecupAvantTab(Chaine)
                                                Pres = Valeurs(Chaine) * 100

                                                Exit Do
                                        End Select

                                    Loop 'fin de lecture d'une pression d'essai

                                    ' enregistrement de la nouvelle mesure
                                    If Not (FinEssai) Then
                                        Call Essai.NouvelleMesure(True, k, k, Pres, vol15, vol30, vol60, cont, 0, Def, 0, 0, 0, 0, 0, 0)
                                        Index = Index + 1
                                    End If

                                Loop
                                '                        Call MyEssaiForageSet Mesures.count(k)
                                ' Assignation de MySonde à l'Essai courant
                                Call Essai.SetMySonde(MySonde)

                                ' assignation de l'Essai Courant à la liste des essais du forage
                                Call MyForage.UpdateEssaiForage(Essai)

                                ' Sauveagrde de la liste des essais du forage
                                '                        ReDim Preserve ListeDesEssais(MyEssaiForage. . ForageCount - 1)
                                '                        Set ListeDesEssais(MyEssaiForageCount - 1) = Essai

                            End If ' Fin de lecture d'un essai

                            ' ajout de l'essai au forage
                            '                    set Myforage.
                        End If
                    Loop 'fin du fichier EOF en Forage
                End If

            Loop 'fin du fichier EOF

            Call Essai.RetablirEvents()
            Call Essai.SetMySonde(MySonde)

            ' libération mémoire
            '    Set ListeDesEssais is Nothing

            'Close the file.
            objStreamReader.Close()
            LectureEssaiJeanLutzTexte = True

            If Lang = 1 Then
                MsgBox("chargement Fichier Jean-Lutz terminé avec succès")
            Else
                MsgBox("Loading Jean-Lutz file succeed")
            End If

            Exit Function

errorFind:
            LectureEssaiJeanLutzTexte = False
            MsgBox(Err.Description)

        End Function












        Public Function RecupColonne(ByVal Ligne As String, num As Integer) As String
            Dim i As Integer
            Dim chaine1 As String
            Dim chaine2 As String

            chaine1 = Ligne
            chaine2 = ""

            For i = 0 To num - 1
                chaine2 = ""
                chaine1 = LTrim(chaine1)

                If InStr(chaine1, " ") = 0 Then
                    chaine2 = chaine1
                Else
                    chaine2 = Left(chaine1, InStr(chaine1, " ") - 1)
                    chaine1 = Right(chaine1, Len(chaine1) - Len(chaine2))
                End If
            Next

            RecupColonne = chaine2
        End Function

        ' Conserver une seule version
        'Public Function RecupColonne(ByVal Ligne As String, ByVal num As Integer) As String
        '    ' Logique unique ici
        '    Dim colonnes() As String = Ligne.Split(";"c)
        '    If num > 0 AndAlso num <= colonnes.Length Then
        '        Return colonnes(num - 1).Trim()
        '    Else
        '        Return String.Empty
        '    End If
        'End Function

        ' Conserver une seule version
        Public Function RecupApres2Points(ByVal Ligne As String) As String
            ' Logique unique ici
            Return Ligne.Substring(Ligne.IndexOf(":") + 1).Trim()
        End Function

        'Public Function RecupApres2Points(ByVal Ligne As String) As String
        '    RecupApres2Points = RTrim(LTrim(Right(Ligne, Len(Ligne) - InStr(Ligne, ":"))))
        'End Function

        Public Function RecupApresTab(ByVal Ligne As String) As String
            RecupApresTab = Right(Ligne, Len(Ligne) - InStr(Ligne, Chr(9)))
        End Function

        Public Function RecupApres2Tab(ByVal Ligne As String) As String
            RecupApres2Tab = Right(RecupApresTab(Ligne), Len(Ligne) - InStr(Ligne, Chr(9)))
            RecupApres2Tab = Right(RecupApresTab(RecupApres2Tab), Len(RecupApres2Tab) - InStr(RecupApres2Tab, Chr(9)))
        End Function

        Public Function RecupApres3Tab(ByVal Ligne As String) As String
            RecupApres3Tab = Right(RecupApresTab(Ligne), Len(Ligne) - InStr(Ligne, Chr(9)))
            RecupApres3Tab = Right(RecupApresTab(RecupApres3Tab), Len(RecupApres3Tab) - InStr(RecupApres3Tab, Chr(9)))
            RecupApres3Tab = Right(RecupApresTab(RecupApres3Tab), Len(RecupApres3Tab) - InStr(RecupApres3Tab, Chr(9)))
        End Function

        Public Function RecupApres4Tab(ByVal Ligne As String) As String
            RecupApres4Tab = Right(RecupApresTab(Ligne), Len(Ligne) - InStr(Ligne, Chr(9)))
            RecupApres4Tab = Right(RecupApresTab(RecupApres4Tab), Len(RecupApres4Tab) - InStr(RecupApres4Tab, Chr(9)))
            RecupApres4Tab = Right(RecupApresTab(RecupApres4Tab), Len(RecupApres4Tab) - InStr(RecupApres4Tab, Chr(9)))
            RecupApres4Tab = Right(RecupApresTab(RecupApres4Tab), Len(RecupApres4Tab) - InStr(RecupApres4Tab, Chr(9)))
        End Function

        Public Function RecupApres5Tab(ByVal Ligne As String) As String
            RecupApres5Tab = Right(RecupApresTab(Ligne), Len(Ligne) - InStr(Ligne, Chr(9)))
            RecupApres5Tab = Right(RecupApresTab(RecupApres5Tab), Len(RecupApres5Tab) - InStr(RecupApres5Tab, Chr(9)))
            RecupApres5Tab = Right(RecupApresTab(RecupApres5Tab), Len(RecupApres5Tab) - InStr(RecupApres5Tab, Chr(9)))
            RecupApres5Tab = Right(RecupApresTab(RecupApres5Tab), Len(RecupApres5Tab) - InStr(RecupApres5Tab, Chr(9)))
            RecupApres5Tab = Right(RecupApresTab(RecupApres5Tab), Len(RecupApres5Tab) - InStr(RecupApres5Tab, Chr(9)))
        End Function

        Public Function RecupApres6Tab(ByVal Ligne As String) As String
            RecupApres6Tab = Right(RecupApresTab(Ligne), Len(Ligne) - InStr(Ligne, Chr(9)))
            RecupApres6Tab = Right(RecupApresTab(RecupApres6Tab), Len(RecupApres6Tab) - InStr(RecupApres6Tab, Chr(9)))
            RecupApres6Tab = Right(RecupApresTab(RecupApres6Tab), Len(RecupApres6Tab) - InStr(RecupApres6Tab, Chr(9)))
            RecupApres6Tab = Right(RecupApresTab(RecupApres6Tab), Len(RecupApres6Tab) - InStr(RecupApres6Tab, Chr(9)))
            RecupApres6Tab = Right(RecupApresTab(RecupApres6Tab), Len(RecupApres6Tab) - InStr(RecupApres6Tab, Chr(9)))
            RecupApres6Tab = Right(RecupApresTab(RecupApres6Tab), Len(RecupApres6Tab) - InStr(RecupApres6Tab, Chr(9)))
        End Function

        Public Function RecupAvantTab(ByVal Ligne As String) As String
            '    Dim Chaine As String
            '    Chaine = Left(Ligne, InStr(Ligne, Chr(9)))
            '    Chaine = Right(Ligne, Len(Ligne) - InStr(Ligne, "    "))
            RecupAvantTab = Left(Ligne, InStr(Ligne, Chr(9)) - 1)
        End Function

        Public Function RecupApresTab1(ByVal Ligne As String) As String
            '    Dim Chaine As String
            '    Chaine = Left(Ligne, InStr(Ligne, Chr(9)))
            '    Chaine = Right(Ligne, Len(Ligne) - InStr(Ligne, "    "))
            '    RecupAvantTab1 = Left(Ligne, InStr(Ligne, Chr(9)) - 1)
            RecupApresTab1 = Right(Ligne, InStr(Ligne, Chr(32)) - 2)
        End Function

        Public Function RecupAvantTab1(ByVal Ligne As String) As String
            '    Dim Chaine As String
            '    Chaine = Left(Ligne, InStr(Ligne, Chr(9)))
            '    Chaine = Right(Ligne, Len(Ligne) - InStr(Ligne, "    "))
            '    RecupAvantTab1 = Left(Ligne, InStr(Ligne, Chr(9)) - 1)
            RecupAvantTab1 = Left(Ligne, InStr(Ligne, Chr(32)) - 1)
        End Function

    End Module
End Namespace