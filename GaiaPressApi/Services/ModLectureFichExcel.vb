Namespace GaiaPress.core.V17.Services

    'Imports cli_uno
    'Imports cli_uno.com.sun.star.lang
    'Imports cli_uno.com.sun.star.frame
    'Imports cli_uno.com.sun.star.beans
    'Imports cli_uno.com.sun.star.sheet
    'Imports cli_uno.com.sun.star.table
    'Imports cli_uno.com.sun.star.uno
    'Imports UnoLib = cli_uno



    'Imports wkbObj = Microsoft.Office.Interop.Excel.Workbook

    Module ModLectureFichExcel
        'ReadOnly wkbObj As Excel.Workbook ' Déclare un objet variable.
        'Dim XlApp As Excel.Application
        'Public Sub ConnexionLibreOffice()
        '    Try
        '        ' Connexion à LibreOffice en utilisant UNO
        '        Dim localContext As Object = UnoLib.uno.util.Bootstrap.default_Bootstrap()


        '        ' Créer le service manager
        '        Dim serviceManager As XMultiComponentFactory = localContext.getServiceManager()

        '        ' Créer une instance de LibreOffice
        '        Dim desktop As XComponentLoader = CType(serviceManager.createInstanceWithContext("com.sun.star.frame.Desktop", localContext), XComponentLoader)

        '        ' Ouvrir un fichier Calc
        '        Dim fileURL As String = "file:///C:/Chemin/vers/votre_fichier.ods"
        '        Dim loadProps() As PropertyValue = New PropertyValue() {}

        '        ' Charger le fichier
        '        Dim document As XComponent = desktop.loadComponentFromURL(fileURL, "_blank", 0, loadProps)

        '        MessageBox.Show("Document ouvert avec succès !")
        '' ✅ Nouveau code (API web)
        'Return BadRequest("Erreur : " & ex.Message)

        'Catch ex As Exception
        ''        MessageBox.Show("Erreur : " & ex.Message)
        '' ✅ Nouveau code (API web)
        'Return BadRequest("Erreur : " & ex.Message)
        ''    End Try
        'End Sub


        '    Public Function LectureEssaiExcelGaiaPress(ByVal FichierChantier As String, ByRef Essai As Essai_Forage, ByRef MyForage As Forage) As Boolean
        '        Dim wkbObj As Excel.Workbook ' Déclare un objet variable.
        '        Dim XlApp As Excel.Application

        '        On Error GoTo errorFind

        '        Dim i, k As Integer
        '        Dim TextLine
        '        Dim MySonde As Probe
        '        'dim MyMesure as mesure
        '        Dim indice As Double
        '        Dim Pres As Double
        '        Dim vol15 As Double
        '        Dim vol30 As Double
        '        Dim vol60 As Double
        '        Dim cont As Double
        '        Dim Def As Double

        '        Call MyListeSondes.NouvelleSonde(True, True)
        '        If MySonde Is Nothing Then MySonde = New Probe
        '        MySonde = MyListeSondes.GetSondeParIndex(MyListeSondes.GetSondeCount - 1)
        '        Call MySonde.SuspendreEvents()
        '        Call Essai.SuspendreEvents()

        '        ' Chargement de Excel
        '        ' ouvrir l'application excel ; Ok
        '        XlApp = CreateObject("Excel.Application")

        '        XlApp.Visible = True  ' Rendre visible l'application

        '        ' Ouvrir le fichier Excel et affectation; Erreur 0x080010105
        '        wkbObj = XlApp.Workbooks.Open(FichierChantier)

        '        ' Affectation à un workbook
        '        ''wkbObj = xlApp.Workbooks

        '        i = 1
        '        k = 0

        '        With wkbObj.Worksheets(1)



        '            i = 1
        '            ' Recherche de la première feuille du dossier Excel
        '            Do While (.Range("A" & i).Value) <> "     NOM FICHIER                   :"

        '                i = i + 1
        '                If i > 50 Then
        '                    If Lang = 1 Then
        '                        MsgBox("La première ligne de mesures du classeur Excel est supérieure à 50")
        '                    Else
        '                        MsgBox("The range of the first line of measurements in the Excel spreadsheet is greater than 50")
        '                        GoTo errorFind
        '                    End If
        '                End If
        '            Loop ' Fin du Do While (wkbObj.Worksheets(1).Range("A" & i).value) <> "     NOM FICHIER                   :"

        '            Do While (.Range("A" & i).Value) <> "" Or i < 41

        '                With Essai
        '                    Select Case i
        '                        Case 13
        '                            .nomEssai = wkbObj.Worksheets(1).Range("G" & i).Value
        '                        Case 14
        '                            .dateEssai = StringToDate(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 16
        '                            '                    Call .TypeDeformee= wkbObj.Worksheets= 1).Range= "G" & i).value)
        '                        Case 17
        '                            '                    Call .TypeContrainte= wkbObj.Worksheets= 1).Range= "G" & i).value)
        '                        Case 20
        '                            Call MySonde.Setlt(10 * Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value))
        '                        Case 21
        '                            Call MySonde.Setlg(10 * Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value))
        '                        Case 22
        '                            Call MySonde.Setls(10 * Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value))
        '                        Case 23
        '                            '                    Call MySonde.VolumeInitial= Valeurs= wkbObj.Worksheets= 1).Range= "G" & i).value))
        '                        Case 24
        '                            Call MySonde.Setds(10 * Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value))
        '                        Case 25
        '                            Call MySonde.SetEpMembraneGarde(10 * Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value))
        '                        Case 26
        '                            Call MySonde.RetablirEvents()
        '                            Call MySonde.SetEpMembraneMesure(10 * Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value) - MySonde.GetEpMembraneGarde)
        '                        Case 27
        '                            .difPresEauAir = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 28
        '                            .presP0 = Valeurs(wkbObj.Worksheet(1).Range("G" & i).Value)
        '                        Case 29
        '                            .presFluage = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 30
        '                            .presLimite = Math.Round(Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value))
        '                        Case 31
        '                            .poidsVol = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 32
        '                            .poidsVolDejauge = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 33
        '                            .coefPoisson = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 34
        '                            .modElastique = Math.Round(Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value))
        '                        Case 36
        '                            .Cohesion = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 37
        '                            .Frottement = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 38
        '                            .hauteurCpv = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 39
        '                            .profNappe = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                        Case 40
        '                            .profSonde = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)
        '                    End Select

        '                    i = i + 1

        '                End With
        '            Loop ' Fin du Do While (wkbObj.Worksheets(1).Range("A" & i).VALEURue) <> "" or i > 39

        '            i = i + 5

        '            Do While (.Range("A" & i).Value) <> "" And i > 45

        '                With Essai

        '                    indice = Valeurs(wkbObj.Worksheets(1).Range("A" & i).Value)
        '                    If indice <> 0 Then
        '                        Pres = Valeurs(wkbObj.Worksheets(1).Range("B" & i).Value)
        '                        vol15 = Valeurs(wkbObj.Worksheets(1).Range("C" & i).Value)
        '                        vol30 = Valeurs(wkbObj.Worksheets(1).Range("D" & i).Value)
        '                        vol60 = Valeurs(wkbObj.Worksheets(1).Range("E" & i).Value)
        '                        cont = Valeurs(wkbObj.Worksheets(1).Range("F" & i).Value)
        '                        Def = Valeurs(wkbObj.Worksheets(1).Range("G" & i).Value)

        '                        Call Essai.NouvelleMesure(False, i, indice, Pres, vol15, vol30, vol60, cont, 0, Def, 0, 0, 0, 0, 0, 0)
        '                    End If
        '                    i = i + 1
        '                End With
        '            Loop

        '            Call Essai.RetablirEvents()
        '            Call Essai.SetMySonde(MySonde)

        '            ' Fermeture du fichier Excel
        '            'wkbObj.Close(SaveChanges:=False, Filename:=FichierChantier)

        '            wkbObj.Close(SaveChanges:=False)

        '            ''wkbObj(FichierChantier).close(savechanges:=False)
        '            ' Suppression des objets
        '            wkbObj.Close(FichierChantier)
        '            wkbObj = Nothing

        '            XlApp.Visible = False
        '            XlApp.Quit()

        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Loading Excel file succeed")
        '                Case 1
        '                    MsgBox("Chargement du Fichier Excel Réussi")
        '            End Select

        '            LectureEssaiExcelGaiaPress = True

        '        End With
        '        Exit Function
        'errorFind:
        '        LectureEssaiExcelGaiaPress = False

        '        If Lang = 1 Then
        '            MsgBox("le chargement du Fichier Excell a échoué")
        '        Else
        '            MsgBox("Loading Excell file failed")
        '        End If
        '    End Function

        '    Public Function LectureEssaiExcelApageo(ByVal FichierChantier As String, ByRef Essai As Essai_Forage, ByRef MyForage As Forage) As Boolean
        '        Dim wkbObj As Excel.Workbook ' Déclare un objet variable.
        '        Dim XlApp As Excel.Application

        '        On Error GoTo errorFind

        '        Dim i, k, Nb As Integer
        '        Dim TextLine
        '        Dim MySonde As Probe
        '        'dim MyMesure as mesure
        '        Dim indice As Single
        '        Dim Pres, vol15, vol30, vol60, PNF, VNF, Nombre As Single
        '        Dim Chaine As String
        '        Dim DateEssai As Date
        '        Dim cont, Def As Single

        '        ' Chargement de Excel
        '        ' ouvrir l'application excel ; Ok
        '        XlApp = CreateObject("Excel.Application")

        '        XlApp.Visible = True  ' Rendre visible l'application

        '        ' Ouvrir le fichier Excel et affectation; Erreur 0x080010105
        '        wkbObj = XlApp.Workbooks.Open(FichierChantier)

        '        ' Affectation à un workbook
        '        ''wkbObj = xlApp.Workbooks

        '        i = 1 ' i est le numéro de ligne
        '        k = 0

        '        ' Mémorisation des onglets du fichier lu
        '        NbTab = wkbObj.Worksheets.Count
        '        ReDim Onglet(NbTab)
        '        For i = 1 To NbTab
        '            Onglet(i) = wkbObj.Worksheets(i).name.ToString
        '        Next i

        '        ' Choix de l'onglet à lire
        '        Dim FrmLectureApageo = New FrmLectureApageo()
        '        FrmLectureApageo.ShowDialog()
        '        'MyForage.NouveauEssai()

        '        With wkbObj.Worksheets(Tab)

        '            i = 1
        '            ' Recherche de la première feuille du dossier Excel
        '            Do While (.Range("AG" & i).Value) <> " Dossier"

        '                i = i + 1
        '                If i > 50 Then
        '                    If Lang = 1 Then
        '                        MsgBox("La première ligne de mesures du classeur Excel est supérieure à 50")
        '                        GoTo errorFind
        '                    Else
        '                        MsgBox("The range of the first line of measurements in the Excel spreadsheet is greater than 50")
        '                        GoTo errorFind
        '                    End If
        '                End If
        '            Loop ' Fin du Do While (wkbObj.Worksheets(1).Range("A" & i).value) <> "     NOM FICHIER                   :"

        '            ' On a trouvé le nom du Chantier
        '            Do While i < 46

        '                With Essai
        '                    '<<<<<<< HEAD
        '                    '                    .RefEssai = MyEssai.RefEssai
        '                    '=======
        '                    .RefEssai = MyEssai.RefEssai

        '                    Select Case i
        '                        Case 2
        '                            'Call .RefChantier(wkbObj.Worksheets(Tab).Range("AJ" & i).Value)    ' Enregistrement de la Ref du Chantier
        '                        Case 4
        '                            'Call .RefChantier(wkbObj.Worksheets(Tab).Range("AJ" & i).Value)    ' Enregistrement du nom du Chantier
        '                        Case 6
        '                            'Call .RefForage(wkbObj.Worksheets(Tab).Range("AJ" & i).Value)    ' Enregistrement de la Ref du Forage
        '                        Case 8
        '                            Chaine = wkbObj.Worksheets(Tab).Range("AJ" & i).Value
        '                            .nomEssai = Chaine    ' Enregistrement de la Ref de l'Essai
        '                        Case 9
        '                            DateEssai = wkbObj.Worksheets(Tab).Range("AJ" & i).Value
        '                            .dateEssai = DateEssai   ' Enregistrement de la Date de l'Essai
        '                        Case 11
        '                            Chaine = wkbObj.Worksheets(Tab).Range("AJ" & i).Value
        '                            .Enregistreur = Chaine   ' Enregistrement du nom de l'enregistreur
        '                        Case 12
        '                            Chaine = wkbObj.Worksheets(Tab).Range("AJ" & i).Value
        '                            .Operateur = Chaine   ' Enregistrement du nom de l'operateur
        '                        Case 13
        '                            Nombre = wkbObj.Worksheets(Tab).Range("AJ" & i).Value
        '                            Nombre = Math.Round(-1000 * Nombre, 1)
        '                            .difPresEauAir = Nombre      ' Enregistrement de la pression différentielle
        '                        Case 20
        '                            Nombre = wkbObj.Worksheets(Tab).Range("AM" & i).Value
        '                            Nombre = Math.Round(Nombre, 2)
        '                            .hauteurCpv = Nombre        ' Enregistrement de la cote du CPV
        '                        Case 24
        '                            Nombre = wkbObj.Worksheets(Tab).Range("AM" & i).Value
        '                            Nombre = Math.Round(Nombre, 2)
        '                            .profNappe = Nombre       ' Enregistrement de la cote de la nappe phréatique
        '                            GoTo LectureLigne
        '                        Case 26
        '                            Nombre = wkbObj.Worksheets(Tab).Range("AM" & i).Value
        '                            Nombre = Math.Round(Nombre, 2)
        '                            .profSonde = Nombre      ' Enregistrement de la cote de la sonde
        '                            GoTo LectureLigne
        '                        Case Is > 21    ' Saisie des mesures
        'LectureLigne:               Pres = Valeurs(wkbObj.Worksheets(Tab).Range("J" & i).Value * 1000)
        '                            Pres = Math.Round(Pres, 1)
        '                            vol15 = Valeurs(wkbObj.Worksheets(Tab).Range("N" & i).Value)
        '                            vol15 = Math.Round(vol15, 1)
        '                            vol30 = Valeurs(wkbObj.Worksheets(Tab).Range("P" & i).Value)
        '                            vol30 = Math.Round(vol30, 1)
        '                            vol60 = Valeurs(wkbObj.Worksheets(Tab).Range("R" & i).Value)
        '                            vol60 = Math.Round(vol60, 1)
        '                            PNF = Valeurs(wkbObj.Worksheets(Tab).Range("T" & i).Value * 1000)
        '                            PNF = Math.Round(PNF, 1)
        '                            VNF = Valeurs(wkbObj.Worksheets(Tab).Range("V" & i).Value)
        '                            VNF = Math.Round(VNF, 1)
        '                            If Pres = 0 Then Exit Do
        '                            Call Essai.NouvelleMesure(False, i, k, Pres, vol15, vol30, vol60, cont, 0, 0, PNF, VNF, 0, 0, 0, 0)
        '                            k = k + 1
        '                    End Select
        '                    i = i + 1
        '                End With
        '            Loop ' Fin du Do While i < 46

        '        End With

        '        Call Essai.RetablirEvents()
        '        Call Essai.SetMySonde(MySonde)

        '        ' Fermeture du fichier Excel
        '        'wkbObj.Close(SaveChanges:=False, Filename:=FichierChantier)

        '        'wkbObj.Close(SaveChanges:=False)

        '        ''wkbObj(FichierChantier).close(savechanges:=False)
        '        ' Suppression des objets
        '        wkbObj.Close(FichierChantier)
        '        wkbObj = Nothing

        '        XlApp.Visible = False
        '        XlApp.Quit()

        '        ' Mise à jour de la Table "LesEssaisForage"
        '        '<<<<<<< HEAD
        '        '        FrmEssai.UpDateLesEssaisForage(MyEssai.RefEssai)

        '        '        ' Mise à jour de la Table "LesMesuresForages"
        '        '        FrmEssai.UpDateLesMesuresForages(MyEssai.RefEssai)
        '        '=======
        '        MyFrmEssai.UpDateLesEssaisForage(MyEssai.RefEssai)

        '        ' Mise à jour de la Table "LesMesuresForage"
        '        MyFrmEssai.UpDateLesMesuresForage(MyEssai.RefEssai)

        '        Select Case Lang
        '            Case 0
        '                MsgBox("Loading Excel file succeed")
        '            Case 1
        '                MsgBox("Chargement du Fichier Excel Réussi")
        '        End Select

        '        LectureEssaiExcelApageo = True

        '        Exit Function
        'errorFind:
        '        LectureEssaiExcelApageo = False

        '        If Lang = 1 Then
        '            MsgBox("le chargement du Fichier Excell a échoué")
        '        Else
        '            MsgBox("Loading Excell file failed")
        '        End If
        '    End Function

        '    Public Function LectureEssaiExcelFondaSol(ByVal FichierChantier As String, ByRef Essai As Essai_Forage, ByRef MyForage As Forage) As Boolean
        '        Dim wkbObj As Excel.Workbook ' Déclare un objet variable.
        '        Dim XlApp As Excel.Application

        '        On Error GoTo errorFind

        '        Dim i, j, k, l, Nb, Temps As Integer
        '        Dim TextLine
        '        Dim MySonde As Probe
        '        'dim MyMesure as mesure
        '        Dim indice As Single
        '        Dim Pres, vol15, vol30, vol60, PNF, VNF, Nombre, PAir, PEau, PDiff, VL As Single
        '        Dim Chaine As String
        '        Dim DateEssai As Date
        '        Dim cont, Def As Single

        '        ' Chargement de Excel
        '        ' ouvrir l'application excel ; Ok
        '        XlApp = CreateObject("Excel.Application")

        '        XlApp.Visible = True  ' Rendre visible l'application

        '        ' Ouvrir le fichier Excel et affectation; Erreur 0x080010105
        '        wkbObj = XlApp.Workbooks.Open(FichierChantier)

        '        ' Affectation à un workbook
        '        ''wkbObj = xlApp.Workbooks

        '        i = 1 ' i est le numéro de ligne
        '        k = 0

        '        ' Récupération du nombre de Mesures dans la Table "LesMesuresForage"
        '        l = MonChantier.Tables("LesMesuresForage").Rows.Count

        '        ' Mémorisation des onglets du fichier lu
        '        NbTab = wkbObj.Worksheets.Count
        '        ReDim Onglet(NbTab)
        '        For i = 1 To NbTab
        '            Onglet(i) = wkbObj.Worksheets(i).name.ToString
        '        Next i

        '        ' Choix de l'onglet à lire
        '        Dim FrmLectureApageo As New FrmLectureApageo()
        '        FrmLectureApageo.ShowDialog()
        '        'MyForage.NouveauEssai()

        '        With wkbObj.Worksheets(Tab)

        '            j = 1
        '            ' Recherche de la première feuille du dossier Excel
        '            Do While (.Range("A" & j).Value) <> "IDEssai"

        '                j = j + 1
        '                If j > 50 Then
        '                    If Lang = 1 Then
        '                        MsgBox("La première ligne de mesures du classeur Excel est supérieure à 50")
        '                        GoTo errorFind
        '                    Else
        '                        MsgBox("The range of the first line of measurements in the Excel spreadsheet is greater than 50")
        '                        GoTo errorFind
        '                    End If
        '                End If
        '            Loop ' Fin du Do While (wkbObj.Worksheets(1).Range("A" & i).value) <> "     NOM FICHIER     : "

        '            i = j
        '            ' On a trouvé l'identifiant de l'essai
        '            Do While i < 200

        '                With Essai
        '                    '<<<<<<< HEAD
        '                    '                    .RefEssai = MyEssai.RefEssai
        '                    '=======
        '                    .RefEssai = MyEssai.RefEssai

        '                    Select Case i
        '                        Case j + 1
        '                            Chaine = wkbObj.Worksheets(Tab).Range("A" & i).Value
        '                            .nomEssai = Chaine    ' Enregistrement de la Ref de l'Essai

        '                            Chaine = wkbObj.Worksheets(Tab).Range("E" & i).Value
        '                            .profSonde = Chaine    ' Enregistrement de la Ref de l'Essai

        '                            DateEssai = wkbObj.Worksheets(Tab).Range("F" & i).Value
        '                            .dateEssai = DateEssai    ' Enregistrement de la Date de l'Essai

        '                        Case Is > j + 4    ' Saisie des mesures
        'LectureLigne:               k = Valeurs(wkbObj.Worksheets(Tab).Range("A" & i).Value)
        '                            Temps = Valeurs(wkbObj.Worksheets(Tab).Range("C" & i).Value)
        '                            PAir = Valeurs(wkbObj.Worksheets(Tab).Range("D" & i).Value) * 100
        '                            PEau = Valeurs(wkbObj.Worksheets(Tab).Range("E" & i).Value) * 100
        '                            PDiff = Valeurs(wkbObj.Worksheets(Tab).Range("F" & i).Value) * 100
        '                            VL = Valeurs(wkbObj.Worksheets(Tab).Range("G" & i).Value)
        '                            If Temps = 15 Then vol15 = VL
        '                            If Temps = 30 Then vol30 = VL
        '                            If Temps = 60 Then
        '                                Pres = PEau
        '                                vol60 = VL
        '                                Call Essai.NouvelleMesure(False, k - 1, k - 1, Pres, vol15, vol30, vol60, cont, 0, 0, 0, 0, 0, 0, 0, 0)
        '                                l = l + 1
        '                            End If

        '                            If k = 0 Then Exit Do

        '                    End Select
        '                    i = i + 1
        '                End With
        '            Loop ' Fin du Do While i < 46

        '        End With

        '        Call Essai.RetablirEvents()
        '        Call Essai.SetMySonde(MySonde)

        '        ' Fermeture du fichier Excel
        '        'wkbObj.Close(SaveChanges:=False, Filename:=FichierChantier)

        '        'wkbObj.Close(SaveChanges:=False)

        '        ''wkbObj(FichierChantier).close(savechanges:=False)
        '        ' Suppression des objets
        '        wkbObj.Close(FichierChantier)
        '        wkbObj = Nothing

        '        XlApp.Visible = False
        '        XlApp.Quit()

        '        ' Mise à jour de la Table "LesEssaisForage"
        '        '<<<<<<< HEAD
        '        '        FrmEssai.UpDateLesEssaisForage(MyEssai.RefEssai)

        '        '        ' Mise à jour de la Table "LesMesuresForages"
        '        '        FrmEssai.UpDateLesMesuresForages(MyEssai.RefEssai)
        '        '=======
        '        MyFrmEssai.UpDateLesEssaisForage(MyEssai.RefEssai)

        '        ' Mise à jour de la Table "LesMesuresForage"
        '        MyFrmEssai.UpDateLesMesuresForage(MyEssai.RefEssai)

        '        Select Case Lang
        '            Case 0
        '                MsgBox("Loading Excel file succeed")
        '            Case 1
        '                MsgBox("Chargement du Fichier Excel Réussi")
        '        End Select

        '        LectureEssaiExcelFondaSol = True

        '        Exit Function
        'errorFind:
        '        LectureEssaiExcelFondaSol = False

        '        If Lang = 1 Then
        '            MsgBox("le chargement du Fichier Excell a échoué")
        '        Else
        '            MsgBox("Loading Excell file failed")
        '        End If
        '    End Function

    End Module

End Namespace
