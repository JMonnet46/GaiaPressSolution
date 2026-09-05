Namespace GaiaPress.core.V17.Services
    'Imports System.Windows.Forms


    Module ModBibliotheque
        ' ******************************************************************************
        ' Bibliothèque de type de programme et fonctions communes
        ' *****************************************************************************

        Public Structure Sols
            Public Dmax As Single
            Public Tamisat50mm As Single
            Public Tamisat2mm As Single
            Public Tamisat80m As Single
            Public VBS As Single
            Public Ip As Single
            Public Es As Single
            Public ClasseSol As String
            Public SousClasse As Single
            Public ClasseInf As String
            Public SousClasseInf As Single
            Public Phimu As Single
        End Structure

        Public Structure ClasseDuSol
            Public Classe1 As String
            Public SousClasse1 As Integer
            Public Classe2 As String
            Public SousClasse2 As Integer
            Public NomClasse As String
            Public Phimu As Single
            Public Erreur As Boolean
        End Structure

        Public Structure SousClasseDuSol
            Public classe As String
            Public SousClasse As Integer
            Public NomClasse As String
            Public Phimu As Single
            Public Erreur As Boolean
        End Structure

        Public Structure Inertie
            Public PresInertie As Single
            Public Erreur As Boolean
        End Structure

        Public Structure PerteCharge
            Public dH As Single
            Public Erreur As Boolean
        End Structure

        ' La structure de donnée Courbe
        Public Structure Courbe
            Public Pres As Single
            Public Def As Single
            Public Pf As Single
            Public Plim As Single
            Public Pmax As Single
            Public UaMax As Single
            Public Erreur As Single
        End Structure

        Public Structure Solution
            Public X1 As Single
            Public X2 As Single
            Public Erreur As Boolean
        End Structure

        Public Structure Solution3D
            Public x As Single
            Public X1 As Single
            Public X2 As Single
            Public Erreur As Boolean
        End Structure

        Public Structure Polynome
            Public A0 As Single
            Public A1 As Single
            Public A2 As Single
            Public A3 As Single
            Public A4 As Single
            Public A5 As Single
            Public Erreur As Boolean
        End Structure

        Public Structure Diagonalisation
            Public A(,) As Double
            Public B() As Double
            Public N As Integer
        End Structure

        Public Enum NoeudType
            NoeudChantier = 0
            NoeudForage = 1
            NoeudEssai = 2
        End Enum

        Public Structure Cisaillement
            Public Phi As Single
            Public Coh As Single
            Public Plim As Single
            Public Pf As Single
            Public Erreur As Boolean
        End Structure

        Public Structure Statistique
            Public Moyenne As Single
            Public Variance As Single
            Public Erreur As Boolean
        End Structure

        Public Structure Regression
            Public A As Single
            Public B As Single
            Public SigA As Single
            Public SigB As Single
            Public SigR As Single
            Public Erreur As Boolean
        End Structure

        Public Structure Minimum
            Public Valeur As Single
            Public Indice As Integer
        End Structure

        Public Structure ValeurPression
            Public Valeur As Single
            Public Precis As Single
        End Structure

        Public Structure Optimise
            Public Eelast As Single
            Public Phi As Single
            Public Coh As Single
            Public U0 As Single
            Public Plim As Single
            Public Erreur As Boolean
        End Structure

        ' Variable de la classe Sonde
        Public Enum TypeDeSonde
            SoupleTypeE = 0
            SoupleTypeG = 1
            TubeFenduCourte = 2
            TubeFenduLongue = 3
            Autre = 4
        End Enum

        Public Enum EssaiCalib
            Expansion = 0
            Resistance = 1
        End Enum

        Public Enum TypeDeDeformee
            BallonRugby = 0
            Lanterne = 1
        End Enum

        Public Enum TypeDeContrainte
            Moyenne = 0
            Lame = 1
        End Enum

        Public Enum TypeDePrecision
            A = 0
            B = 1
            C = 2
        End Enum

        Public Structure PrecisionElast
            Public PMax As Single
            Public PMin As Single
            Public VMax As Single
            Public VMin As Single
            Public DEm_P As Single
            Public DEm_V As Single
            Public DEm_Vs As Single
        End Structure


        '*******************************************************
        'Déclarer des variables communes pour tous les modules
        '*******************************************************
        Public isLoading As Boolean = False

        Public Const Pi = 3.14159265358979
        Public Const GammaW = 9.81
        Public sFile As String
        Public Lang As Integer = 1  ' 1 = Français par défaut; 0 = Anglais

        ' Variable d'aide
        Public helpfile, helpcontent, helpindex As String
        'Public fMainForm As FrmMenu 'Référence de la feuille principale

        ' Définition des DataAdapter

        ' Variable de modification du DataAdaptateur
        Public DaVi As DataView

        ' Variables de Chantier
        'Public MyChantier As New LesChantiers    ' Référence du chantier ouvert
        Public MonChantier As New DataSet
        Public MesSondes As New DataSet
        Public ObjetDataChantier As New System.Data.DataTable
        Public ObjetDataForage As New System.Data.DataTable
        Public ObjetDataEssai As New System.Data.DataTable
        Public ObjetDataMesures As New System.Data.DataTable
        Public AncienChantier As Boolean
        'Public MyChantierLogic As New ChantierLogic
        Public FichierListResistance As String

        ' Variables de Sol
        Public MySol As Sol
        Public MyListeSols As ListeSols   ' Référence de la liste de sols
        'Public MaListeSols As New Sols0DataSet
        Public ObjetDataSols As New System.Data.DataTable
        Public MaListeSols As New DataSet
        'Public MaListeSols As New ListeSols
        Public IsInitializingSols As Boolean
        'Public MyFrmSols As New FrmSols

        ' Variable de Forage
        Public MyForage As Forage
        Public MyListeForages As List(Of Forage)
        'Public MyFrmForage As FrmForage

        ' Variables de Sondes
        Public MySonde As Probe
        Public MyListeSondes As ListeSondes   ' Référence de la liste de sondes
        Public MaListeSondes As New DataSet
        Public ObjetDataListeSondes As New System.Data.DataTable
        Public ObjetDataEssaisResistancePropre As New System.Data.DataTable
        Public ObjetDataMesuresResistancePropre As New System.Data.DataTable
        Public ObjetDataEssaisExpansionPropre As New System.Data.DataTable
        Public ObjetDataMesuresExpansionPropre As New System.Data.DataTable
        Public MyListeSondeCount As Integer
        Public AncienneSonde As Boolean
        'Public MyFrmSonde As FrmSonde
        Public m_FrmSondeIsShowing As Boolean = False

        ' Variables d'Essai
        Public MyListeEssaisForage As List(Of Essai_Forage)
        Public MyListeEssaisExpansion As List(Of Essai_Expansion)
        Public MyListeEssaisResistance As List(Of Essai_Resistance)
        Public MyEssai, MyTest, MonEssai As Essai_Forage
        Public MyResistance As Essai_Resistance
        Public MyExpansion As Essai_Expansion
        'Public MyEssaiC_ListForages.Count As Integer
        Public MyRefEssai As String
        Public refEssaiResistancePropre As String
        Public refEssaiExpansionPropre As String
        Public MyEssaiExpansionCount As Integer
        Public MyEssaiResistanceCount As Integer
        'Public ImageListMenu As ImageList
        Public TypeRefEssai As Integer
        Public Fso As Object
        'Public MyFrmEssai As New FrmEssai

        ' Variables de Resistance
        Public A0, A1, A2, A3, Pel, Pe As Single

        ' Variables d'Expansion
        Public Di, A As Single
        Public Dilatap, CompMemb As Boolean

        ' Variables de Sonde
        Public Ls, Ds, Lg, Lt, Vs, Vc As Single

        ' Variable d'eaai
        Public PoidsVol As Single

        ' Variables de Précision
        Public PrLong, PrDiam, PrPV, PrProf, PrTemps, PrVs, PrVc, PrRegres, PrecisVs, PrRegresPl, PrA, PrB, PrP, PrV, PrEps, PrEMenard, PrEPressio, PrEelast, PrEelastNF As Single
        Public PrPlMenard, PrPl, PrVariance As Single
        Public ClassePrecision As TypeDePrecision

        ' Variables de Fichier
        Public FichierSondes As String      ' Nom du fichier de sondes
        Public FichierSols As String      ' Nom du fichier de sols
        Public FichierChantier As String      ' Nom du fichier de chantier
        Public ChargeChantier, Chargeforage, ChargeEssai, ChargeMesure, ChargeListeSondes, ChargeListeSols As Boolean
        Public Onglet() As String           ' Nom des Onglets des Fichiers Apageo
        Public iTable, Tab, NbTab As Integer
        Public NouveauChantier, NouveauForage, NouvelEssai, NouvelleMesure As Boolean
        ' Connection à un serveur local
        Public ChaineConnection As String =
    "Server=tcp:gaiatech-2026.database.windows.net,1433;" _
    & "Database=GaiaPressBD;" _
    & "User ID=CloudSA785f9c67;" _
    & "Password=VotreMotDePasse;" _
    & "Encrypt=True;" _
    & "TrustServerCertificate=False;" _
    & "Connection Timeout=30;"
        ' Connection à un serveur SQL distant
        'Public ChaineConnection As String = "Server=mon-serveur.database.windows.net;Database=GaiaPressDB;User Id=mon_user;Password=mon_mot_de_passe;")"

        Public Const HH_DISPLAY_TOPIC = &H0
        Public Const HH_HELP_CONTEXT = &HF

        Public Declare Function HtmlHelp Lib "Hhctrl.ocx" _
     Alias "HtmlHelpA" (ByVal hWndCaller As Long,
     ByVal pszFile As String, ByVal uCommand As Long,
     ByVal dwData As Long) As Long

        ' ****************************************************************************************************************
        ' Fin de déclaration de variable communes
        ' ******************************************************************************************************

        'Public Sub RadioButtonFrancais_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonFrancais.CheckedChanged
        '    If isLoading Then Return
        '    isLoading = True
        '    ' Votre code ici
        '    isLoading = False
        'End Sub

        'Public Sub RadioButtonAnglais_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButtonAnglais.CheckedChanged
        '    If isLoading Then Return
        '    isLoading = True
        '    ' Votre code ici
        '    isLoading = False
        'End Sub

        Public Function LaunchHTMLHelp(HelpFile As String, Optional WindowHandle As Long = 0, Optional Topic As Long = 0) As Boolean

            'HelpFile - correspond au chemin du fichier d'aide.
            'WindowHandle - Fenêtre contenant la fenêtre d'aide
            'Topic - le numéro du sujet d'aide à afficher

            Dim lngReturn As Long

            If Len(System.IO.Directory.GetFiles(HelpFile)) > 0 Then

                If Topic = 0 Then
                    ' Si le numéro de sujet est 0, affichage de la tables des matières
                    lngReturn = HtmlHelp(WindowHandle, HelpFile, HH_DISPLAY_TOPIC, 0)
                Else
                    ' Math.sinon, affichage du sujet correspondant
                    lngReturn = HtmlHelp(WindowHandle, HelpFile, HH_HELP_CONTEXT, Topic)
                End If

                LaunchHTMLHelp = CBool(lngReturn)
            Else
                LaunchHTMLHelp = False
            End If

        End Function


        Public Function GetFichierSondes()
            Dim Directory As String
            Dim i, j As Integer
            GetFichierSondes = ""
            If FichierSondes = "" Then
                'LireFichier.ShowDialog()
                Directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
                'Directory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase)

                ' recherche l'occurence du premier caractère \ à partir de la droite
                i = InStrRev(Directory, "\Projet GaiaPressRef")
                If i = 0 Then i = InStrRev(Directory, "\GaiaPressRef")
                j = Len("\GaiaPressRef")

                ' retourne la partie gauche du chemin d'accès au programme
                Directory = Strings.Left(Directory, i + j)

                i = Len(Directory)

                'If InStrRev(Directory, "\GaiaPressV16") > 0 Then
                If InStrRev(Directory, "\GaiaPressRef") > 0 Then
                    '    i = InStrRev(Directory, "\GaiaPressV16")
                    '    j = Len("\GaiaPressV16")

                    '    ' retourne la partie gauche du chemin d'accès au programme
                    '    Directory = Strings.Left(Directory, i - 1)
                    '    i = Len(Directory)
                    'End If

                    ' Enlève les caractères parasites du début
                    Directory = Strings.Right(Directory, i - 6)

                    GetFichierSondes = Directory & "Sondes\Sondes0.mdb"
                Else
                    GetFichierSondes = FichierSondes
                End If

            End If
        End Function

        Public Function GetSafeString(value As Object) As String
            If value Is DBNull.Value OrElse value Is Nothing Then
                Return String.Empty
            Else
                Return value.ToString()
            End If
        End Function

        Public Function Maximum10(ByVal Nombre As Single) As Single
            Dim i As Integer
            Maximum10 = 0.001
            For i = 0 To 1000
                If Nombre < Maximum10 Then Exit Function
                Maximum10 = Maximum10 * 10
            Next i
        End Function

        Public Function MinimumTableau(ByRef X() As Single, ByVal N As Integer) As Minimum
            Dim i, ind As Integer
            Dim Min As Single
            Min = X(0)
            ind = 0
            For i = 1 To N
                If X(i) < Min Then
                    Min = X(i)
                    ind = i
                End If
            Next i
            MinimumTableau.Indice = ind
            MinimumTableau.Valeur = Min
        End Function

        Public Function GetFichierSols()
            Dim Directory As String
            Dim i, j As Integer

            'LireFichier.ShowDialog()
            Directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)
            'Directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)

            ' recherche l'occurence du premier caractère \ à partir de la droite
            i = InStrRev(Directory, "\Projet GaiaPressRef")
            If i = 0 Then i = InStrRev(Directory, "\GaiaPressRef")
            j = Len("\GaiaPressRef")

            ' retourne la partie gauche du chemin d'accès au programme
            Directory = Strings.Left(Directory, i + j)

            i = Len(Directory)

            'LireFichier.ShowDialog()
            'Directory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().ToString)
            'Directory = Assembly.GetExecutingAssembly().Location

            '' recherche l'occurence du premier caractère \ à partir de la droite
            'i = InStrRev(Directory, "\WinFormsApp3\")
            'i = InStrRev(Directory, "\Sols")
            'If i = 0 Then i = InStrRev(Directory, "\Sols")
            'j = Len("\Projet GaiaPress")

            ' retourne la partie gauche du chemin d'accès au programme
            'Directory = Strings.Left(Directory, i + j)
            'i = Len(Directory)

            If InStrRev(Directory, "\GaiaPressV16") > 0 Then
                i = InStrRev(Directory, "\GaiaPressV16")
                If i = 0 Then i = InStrRev(Directory, "\GaiaPressV15")
                j = Len("\GaiaPressV16")

                ' retourne la partie gauche du chemin d'accès au programme
                Directory = Strings.Left(Directory, i - 1)
                i = Len(Directory)
            End If

            ' Enlève les caractères parasites du début
            Directory = Strings.Right(Directory, i - 6)

            If FichierSols = "" Then
                GetFichierSols = Directory & "Sols\Sols0.mdb"
                FichierSols = Directory & "Sols\Sols0.mdb"
            Else
                GetFichierSols = FichierSols
            End If
        End Function

        Public Function StringToDate(Chaine As String) As Date
            On Error GoTo fin
            Dim dat As Date

            If InStr(Chaine, ".") <> 0 Then
                Dim S1, S2 As String
                Dim i As Integer

                S1 = Chaine
                S2 = ""
                For i = 0 To 1
                    S2 = S2 & Val(Left(S1, InStr(Chaine, "."))) & "/"
                    S1 = (Right(S1, Len(S1) - InStr(Chaine, ".")))
                Next
                S2 = S2 & S1
                dat = CDate(S2)
            Else
                If Chaine <> "" Then
                    dat = CDate(Chaine)
                Else
                    dat = Today
                End If
            End If
            StringToDate = dat

            Exit Function

fin:
            Err.Raise(vbObjectError + 513)
        End Function

        Public Function OnlyNumbers(ByVal Chaine As String) As Boolean
            OnlyNumbers = (Len(Chaine) = Len(CStr(Valeurs(Chaine))))
        End Function


        Public Function Inc(ByVal Valeur As Integer) As Integer
            Inc = Valeur + 1
        End Function


        Public Function Valeurs(ByVal Chaine As String) As Single
            If Chaine = "," Then
                Valeurs = 0
                Exit Function
            End If
            If InStr(Chaine, ",") = 0 Then
                Valeurs = Val(Chaine)
            Else
                Valeurs = CDbl(Chaine)
            End If
        End Function

        ' Conversion d'un nombre en chaine de caractère
        Public Function FormatNombre(ByVal Nombre As Single, ByVal decimale As Integer) As Single
            Nombre = Nombre * 10 ^ decimale
            Nombre = Int(Nombre)
            Nombre = Nombre / 10 ^ decimale
            FormatNombre = Nombre
        End Function

        ' Logarithme décimal
        Function Log10(ByVal x As Single) As Single
            Log10 = Math.Log(x) / Math.Log(10)
        End Function

        '
        ' Fonction de définition de nom de fichier
        ' Transforme le nom du fichier d'essai en ajoutant un sousDossier, un NomChantier et une extension
        '

        Public Function FichierChantierSousDossier(ByVal FichierChantier As String, ByVal NomChantier As String, ByVal SousDossier As String, ByVal TypeFichier As String) As String
            ' FichierChantier est le nom du fichier du chantier
            ' NomChantier est le nom du fichier à créer
            ' SousDossier est le nom du sous Dossier dans lequel on veut travailler
            ' TypeFichier est l'extension du fichier à créer
            Dim i, j As Integer
            Dim Ls As String
            NomChantier = NomChantier & TypeFichier ' Ajoute l'extension choisie à NomChantier
            j = Len(FichierChantier)                     ' Mesure la longueur de la chaine FichierChantier qui est l'adresse du fichier d 'essai actuel
            '    i = InStr(FichierChantier, NomChantier) - 1
            i = InStrRev(FichierChantier, "\") - 1       ' Retourne la première occurence du caractère \ dans FichierChantier
            If i < 0 Then i = 0
            Ls = Left(FichierChantier, i)                ' prend la partie gauche avant le caractère \ dans FichierChantier
            i = InStrRev(Ls, "\")                   ' Insère un caractère \ à droite de la chaine Ls
            If i < 0 Then i = 0
            Ls = Left(Ls, i)                ' sélectionne la partie à gauche de la chaine Ls
            FichierChantierSousDossier = Ls & SousDossier & "\" & NomChantier    ' Ajoute à Ls le SousDossier et le NomChantier"
        End Function

        ' myfilenameWithoutExtension:  Return filename without extension from complete path
        Public Function myfilenameWithoutExtension(ByVal path As String) As String
            Dim pos As Integer
            Dim filename As String
            pos = InStrRev(path, "\")
            If pos > 0 Then
                filename = Mid$(path, pos + 1, Len(path))
                myfilenameWithoutExtension = Left(filename, Len(filename) - Len(Mid$(filename, InStrRev(filename, "."), Len(filename))))
            Else
                myfilenameWithoutExtension = ""
            End If
        End Function

        ' myfilenameWithExtension: Return filename with extension from complete path
        Public Function myfilenameWithExtension(ByVal path As String) As String
            Dim pos As Integer
            pos = InStrRev(path, "\")
            If pos > 0 Then
                myfilenameWithExtension = Mid$(path, pos + 1, Len(path))
            Else
                myfilenameWithExtension = ""
            End If
        End Function


        ' GetDirectoryFromPathFilename: Return directory path contain filename
        Public Function GetDirectoryFromPathFilename(ByVal path As String) As String
            Dim pos As Integer
            pos = InStrRev(path, "\")
            If pos > 0 Then
                GetDirectoryFromPathFilename = Left$(path, pos)
            Else
                GetDirectoryFromPathFilename = ""
            End If
        End Function

        Function Nettoyage(ByVal str As String) As Single

            str = Replace(str, ".", vbNullString)
            str = Replace(str, ".", ",")
            If Right(str, 1) = "-" Then
                Nettoyage = CDbl("-" & Left(str, Len(str) - 1))
            Else
                Nettoyage = CDbl(str)
            End If

        End Function

        Public Function Viscosite(ByVal Theta As Single) As Single
            ' Calculation of the water viscosity in Poiseuille (Pa.s)
            Viscosite = 0.00179 / (1 + 0.03368 * Theta + 0.00022 * Theta ^ 2)
        End Function

        Public Function RhoW(ByVal Theta As Single) As Single
            ' Calculation of the density of the water (g/cm3)
            RhoW = (2 * 10 ^ -5 * Theta ^ 3 - 0.0058 * Theta ^ 2 + 0.0237 * Theta + 1000) / 1000
        End Function

        'Public Sub LibereTxtBox(ByRef TextBoxAppli As System.Windows.Forms.TextBox, ByRef Couleur As System.Drawing.Color, ByVal Visible As Boolean)
        '    With TextBoxAppli
        '        .BackColor = Couleur
        '        .BorderStyle = 1
        '        .Visible = Visible
        '        .Enabled = Visible
        '    End With
        'End Sub

        'Public Sub VideTxtBox(ByRef TextBoxAppli As System.Windows.Forms.TextBox, ByRef Couleur As System.Drawing.Color, ByVal Visible As Boolean)
        '    With TextBoxAppli
        '        .BackColor = Couleur
        '        .BorderStyle = 1
        '        .Visible = Visible
        '        .Text = ""
        '    End With
        'End Sub
    End Module
End Namespace