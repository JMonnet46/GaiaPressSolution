Imports System.Data
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Controllers
Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
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

        '        Public Function ChargeAvecBD(ByVal nomFichier As String) As Boolean
        '            Dim Requete, RefProbe As String
        '            Dim i, j, iResistance, iExpansion As Integer
        '            Dim RefSonde, RefEssai, Chaine As String
        '            'Dim newRow As DataRow
        '            Dim MyExpansion As New EssaiExpansion
        '            Dim MyResistance As New EssaiResistance
        '            Dim refSondes As New Dictionary(Of String, Boolean)
        '            Dim RowSonde As Probe
        '            Dim ReferenceSonde As String

        '            ' On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            ' ***************************************************************************
        '            ' Connexion avec la Base de Données
        '            ' ***************************************************************************

        '            ' Création d'une connection avec la base de données de type Access 2000


        '            ' *********************************************************************
        '            ' Lecture de la Table "LesSondes"
        '            ' *********************************************************************
        '            'ma requete pour lire les enregistrements du fichier Sondes.mdb
        '            ' Requete = "SELECT refSonde, typeSonde, typeDeformee, typeContrainte, ls, lg, lt, longInjectPompage, Vs, ds, " &
        '            '"epMembraneGarde,  PrEpMembraneGarde, epMembraneMesure, PrEpMembraneMesure, epMembraneMesure, PrepMembraneMesure,epTubeFendu, PrEpTubeFendu, " &
        '            '" Pm FROM LesSondes"

        '            Requete = "SELECT refSonde, typeSonde, typeDeformee, typeContrainte, ls, lg, lt, longInjectPompage, Vs, ds, " &
        '              "ClassPrecis, epMembraneGarde, PrEpMembraneGarde, epMembraneMesure, PrEpMembraneMesure, epTubeFendu, PrEpTubeFendu, " &
        '              "epaisInitMembrane, PrEpaisInitMembrane, PrLong, PrDiam, PrP, PrV, PrPV, PrZ, PrT, Pm FROM LesSondes"

        '            ' Définition de la Commande

        '            ' Définition de l'AdaptateurSondes

        '            'MaListeSonde.AcceptChanges()

        '            ' Nettoyage de MaListeSonde avant le chargement
        '            If MaListeSonde.Tables("LesSondes") IsNot Nothing Then
        '                MaListeSonde.Tables("LesSondes").Clear()
        '            End If

        '            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSonde
        '            ' AdaptateurSondes.Fill(MesSondes, "LesEssaisExpansionPropre")

        '            ' Enregistre la nouvelle Table dans le Mapping
        '            'AdaptateurSondes.TableMappings.Add("LesSondes")

        '            '' Mise à jour du compteur d'enregistrements
        '            'SondeCount = MaListeSonde.Tables("LesSondes").Rows.Count
        '            'SondeCountInit = MaListeSonde.Tables("LesSondes").Rows.Count

        '            ' MaListeSonde contient toute la table "LesSondes"
        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading of the Table 'LesSondes' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesSondes' réussie")
        '            End Select


        '            ' *********************************************************************
        '            ' Lecture de la Table "LesEssaisExpansionPropre"
        '            ' *********************************************************************
        '            'ma requete
        '            If Not (AncienneSonde) Then
        '                Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai, difPresEauAir, hauteurCpv" &
        '                ", Di, Vc, Vs, EpMembrane, Prp, PrV, coefficientA, coefficientB, PrecisVc, PrecisVs, PrEpMembrane, PrecisA, PrecisB" &
        '                " FROM LesEssaisExpansionPropre"
        '                'Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai" &
        '                '" FROM LesEssaisExpansionPropre"
        '            Else
        '                Requete = "Select refEssai, refSonde, nomEssai, dateEssai, difPresEauAir, hauteurCpv, Di, Vc, coefficientA" &
        '               " FROM LesEssaisExpansionPropre"
        '            End If


        '            ' Définition de la Commande

        '            ' Définition de l'AdaptateurSondes

        '            'MaListeSonde.AcceptChanges()

        '            ' Nettoyage de MaListeSonde avant le chargement
        '            MaListeSonde.Tables("LesEssaisExpansionPropre")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSonde

        '            'If MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count = 0 Then
        '            '    MsgBox("Aucune donnée trouvée dans la table LesEssaisExpansionPropre.")
        '            'Else
        '            '    MsgBox("Les données ont été correctement chargées.")
        '            'End If

        '            ' Enregistre la nouvelle Table dans le Mapping
        '            'AdaptateurSondes.TableMappings.Add("LesEssaisExpansionPropre")

        '            '' Mise à jour du compteur d'enregistrements
        '            'ExpansionCount = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count
        '            'Expansion.CountInit = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows.Count

        '            ' MaListeSonde contient toute la table "LesEssaisExpansionPropre"
        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading Of the Table 'LesEssaisExpansionPropre' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesEssaisExpansionPropre' réussie")
        '            End Select


        '            ' *********************************************************************
        '            ' Lecture de la Table "LesEssaisResistanceDb"
        '            ' *********************************************************************
        '            'ma requete
        '            Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai, difPresEauAir, hauteurCpv, A0, A1, A2, A3" &
        '                 " FROM LesEssaisResistanceDbPropre"

        '            'Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai, difPresEauAir, hauteurCpv, A0, A1, A2, A3" & _
        '            '     " FROM LesEssaisResistanceDbPropre"

        '            ' Définition de la Commande

        '            ' Définition de l'AdaptateurSondes

        '            'MaListeSonde.AcceptChanges()

        '            ' Nettoyage de MaListeSonde avant le chargement
        '            MaListeSonde.Tables("LesEssaisResistanceDbPropre")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSonde
        '            ' MaListeSonde contient toute la table "LesEssaisResistanceDbPropre"

        '            '' Mise à jour du compteur d'enregistrements
        '            'ResistanceCount = MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows.Count
        '            'ResistanceCountInit = MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows.Count

        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading of the Table 'LesEssaisResistanceDbPropre' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesEssaisResistanceDbPropre' réussie")
        '            End Select

        '            ' *********************************************************************
        '            ' Lecture de la Table "LesMesuresResistanceDbPropre"
        '            ' *********************************************************************
        '            'ma requete
        '            Requete = "SELECT Ind, indiceMesure, refEssai, pression, volume FROM LesMesuresResistanceDbPropre "

        '            ' Définition de la Commande

        '            ' Définition de l'AdaptateurSondes

        '            'MaListeSonde.AcceptChanges()

        '            ' Nettoyage de MaListeSonde avant le chargement
        '            MaListeSonde.Tables("LesMesuresResistanceDbPropre")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSonde
        '            ' MaListeSonde contient toute la table "LesMesuresResistanceDbPropre"

        '            '' Mise à jour du compteur d'enregistrements
        '            'MesureResistanceCount = MaListeSonde.Tables("LesMesuresResistanceDbPropre").Rows.Count
        '            'MesureResistanceCountInit = MaListeSonde.Tables("LesMesuresResistanceDbPropre").Rows.Count

        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading of the Table 'LesMesuresResistanceDbPropre' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesMesuresResistanceDbPropre' réussie")
        '            End Select



        '            ' *********************************************************************
        '            ' Lecture de la Table "LesMesureExpansionPropre"
        '            ' *********************************************************************
        '            'ma requete
        '            Requete = "SELECT Ind, indiceMesure, refEssai, pression, volume FROM LesMesureExpansionPropre "

        '            ' Définition de la Commande

        '            ' Définition de l'AdaptateurSondes

        '            'MaListeSonde.AcceptChanges()

        '            ' Nettoyage de MaListeSonde avant le chargement
        '            MaListeSonde.Tables("LesMesureExpansionPropre")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSonde
        '            ' MaListeSonde contient toute la table "LesMesureExpansionPropre"

        '            '' Mise à jour du compteur d'enregistrements
        '            'MesureExpansionCount = MaListeSonde.Tables("LesMesureExpansionPropre").Rows.Count
        '            'MesureExpansion.CountInit = MaListeSonde.Tables("LesMesureExpansionPropre").Rows.Count

        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading of the Table 'LesMesureExpansionPropre' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesMesureExpansionPropre' réussie")
        '            End Select


        '            ' *****************************************************
        '            ' Sauvegarde des données de connection
        '            ' *****************************************************
        '            'MyListeSonde.connSonde = connSonde
        '            With MyListeSonde
        '                '    '.AdaptateurSondes = AdaptateurSondes
        '                '    ' Initialisation de l'objet MyListeSonde
        '                '.Count = 0
        '                '.SondeCount = 0
        '                '.ExpansionCount = 0
        '                '. ResistanceCount = 0
        '                '.MesureExpansion.Count = 0
        '                '.MesureResistanceCount = 0
        '            End With

        '            ' **************************************************************************************
        '            ' Rangement dans MyListeSondedes objets de la Tables("LesSondes") de MaListeSonde.
        '            ' **************************************************************************************

        '            '    '' Création des objets Sondes
        '            '    MyListeSonde. ListeSonde(i).ChargeAvecBD(connSonde, MySonde, RefProbe, i)
        '            '    'MaListeSonde.Sondes(i).ChargeAvecBD(connSonde, MySonde, RefProbe, i)
        '            '    EventsSuspendus = False
        '            'End If

        '            ' Remplissage de MyListeSonde
        '            ' i est le numéro de la sonde
        '            ' For Each Ligne As DataRow In MesSondes.Tables("LesEssaisExpansionPropre").Rows()
        '            For i = 0 To MaListeSonde.Tables("LesSondes").Rows().Count - 1

        '                If MaListeSonde.Tables("LesSondes").Rows(i) IsNot Nothing Then
        '                    ' Nouvelle Sonde

        '                    MyListeSonde.NouvelleSonde(True, False)
        '                    'RefProbe = MesSondes.Tables("LesSondes").Rows(i).Item("refSonde").ToString
        '                    RefProbe = MaListeSonde.Tables("LesSondes").Rows(i).Item("refSonde").ToString
        '                    ' Création des objets Sondes
        '                    EventsSuspendus = True
        '                    MyListeSonde.ListeSonde(i).refSonde = MaListeSonde.Tables("LesSondes").Rows(i).Item("RefSonde").ToString
        '                    MyListeSonde.ListeSonde(i).typeSonde = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("typeSonde"))
        '                    MyListeSonde.ListeSonde(i).typeDeformee = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("typeDeformee"))
        '                    MyListeSonde.ListeSonde(i).typeContrainte = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("typeContrainte"))
        '                    MyListeSonde.ListeSonde(i).ls = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("ls").ToString)
        '                    MyListeSonde.ListeSonde(i).lg = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("lg").ToString)
        '                    MyListeSonde.ListeSonde(i).lt = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("lt").ToString)
        '                    'MyListeSonde. ListeSonde(i). longInjectePompage = MaListeSonde.Tables("LesSondes").Rows(i).Item("longInjectePompage").ToString)
        '                    MyListeSonde.ListeSonde(i).Vs = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("Vs").ToString)
        '                    MyListeSonde.ListeSonde(i).ds = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("ds").ToString)
        '                    If Not IsDBNull(MaListeSonde.Tables("LesSondes").Rows(i).Item("ClassPrecis")) Then
        '                        MyListeSonde.ListeSonde(i).ClassPrecis = MaListeSonde.Tables("LesSondes").Rows(i).Item("ClassPrecis").ToString
        '                    Else
        '                        ' Assigner une valeur par défaut si la valeur est DBNull
        '                        MyListeSonde.ListeSonde(i).ClassPrecis = "0" ' ou une autre valeur par défaut
        '                    End If

        '                    'MyListeSonde. ListeSonde(i). ClassPrecis = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("ClassPrecis"))
        '                    MyListeSonde.ListeSonde(i).EpMembraneGarde = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("EpMembraneGarde").ToString)
        '                    MyListeSonde.ListeSonde(i).PrEpMembraneGarde = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrEpMembraneGarde").ToString)
        '                    MyListeSonde.ListeSonde(i).EpMembraneMesure = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("epMembraneMesure").ToString)
        '                    MyListeSonde.ListeSonde(i).PrEpMembraneMesure = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrepMembraneMesure").ToString)
        '                    MyListeSonde.ListeSonde(i).epTubeFendu = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("epTubeFendu").ToString)
        '                    MyListeSonde.ListeSonde(i).PrEpTubeFendu = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrepTubeFendu").ToString)
        '                    MyListeSonde.ListeSonde(i).EpInitMembrane = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("epaisInitMembrane").ToString)
        '                    MyListeSonde.ListeSonde(i).PrEpInitMembrane = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrEpaisInitMembrane").ToString)
        '                    MyListeSonde.ListeSonde(i).PrLong = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrLong").ToString)
        '                    MyListeSonde.ListeSonde(i).PrDiam = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrDiam").ToString)
        '                    MyListeSonde.ListeSonde(i).PrP = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrP").ToString)
        '                    MyListeSonde.ListeSonde(i).PrV = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrV").ToString)
        '                    MyListeSonde.ListeSonde(i).PrPV = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrPV").ToString)
        '                    MyListeSonde.ListeSonde(i).PrZ = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("PrZ").ToString)
        '                    MyListeSonde.ListeSonde(i).Prt = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("Prt").ToString)
        '                    MyListeSonde.ListeSonde(i).Pm = Valeurs(MaListeSonde.Tables("LesSondes").Rows(i).Item("Pm").ToString)

        '                    ' i correspond à la Sonde RefSonde

        '                    RefSonde = MyListeSonde.ListeSonde(i).refSonde
        '                    iExpansion = 0
        '                    iResistance = 0

        '                    ' On lit toutes les lignes de la Table "LesEssaisExpansion" 
        '                    ' On cherche la premiere occurance de nom Re Essai
        '                    'For j = 0 To MyListeSonde. ListeSonde(i).Count - 1

        '                    '    ' On cherche les Essais de nom RefProbe dans la table "LesEssaisExpansion"
        '                    '    Chaine = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(j)(1).ToString

        '                    '    If RefSonde = Chaine Then

        '                    '        ' On a trouvé un essai de la meme sonde
        '                    '        iExpansion = iExpansion + 1

        '                    '        RefEssai = MaListeSonde.Tables("LesEssaisExpansionPropre").Rows(j)(0).ToString

        '                    '        ' Vérifier si le tableau Expansion est initialisé
        '                    '        'If MyListeSonde. ListeSonde(i) MaExpansion Is Nothing Then
        '                    '        '    ReDim Preserve MyListeSonde. ListeSonde(i) MaExpansion(iExpansion)
        '                    '        'End If

        '                    '        ' Redimensionner et instancier Expansion(iExpansion) si nécessaire
        '                    '        'If iExpansion >= UBound(MyListeSonde. ListeSonde(i) MaExpansion) Then
        '                    '        '    ReDim Preserve MyListeSonde. ListeSonde(i) MaExpansion(iExpansion)
        '                    '        'End If

        '                    '        ' Instancier l'objet Expansion(iExpansion)
        '                    '        MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1) = New EssaiExpansion

        '                    '        ' Remplir les propriétés
        '                    '        With MaListeSonde.Tables("LesEssaisExpansionPropre")
        '                    '            'Debug.WriteLine("refEssai = " & .Rows(j)(0).ToString)
        '                    '            If (.Rows(j)(0)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). RefEssai = CStr(.Rows(j)(0))
        '                    '            'Debug.WriteLine("RefSonde = " & .Rows(j)(1).ToString)
        '                    '            If (.Rows(j)(1)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). RefSonde = CStr(.Rows(j)(1))
        '                    '            'Debug.WriteLine("ClassPrecis = " & .Rows(j)(2).ToString)
        '                    '            If (.Rows(j)(2)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). ClassPrecis = CStr(.Rows(j)(2))
        '                    '            'Debug.WriteLine("nomEssai = " & .Rows(j)(3))
        '                    '            If (.Rows(j)(3)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). nomEssai = CStr(.Rows(j)(3))
        '                    '            'Debug.WriteLine("dateEssai = " & .Rows(j)(4)) 
        '                    '            If (.Rows(j)(4)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). dateEssai = CStr(.Rows(j)(4))
        '                    '            'Debug.WriteLine("DifPresEauAir = " & .Rows(j)(5).ToString)
        '                    '            If (.Rows(j)(5)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). DifPresEauAir = CStr(.Rows(j)(5))
        '                    '            ''Debug.WriteLine("HauteurCpv = " & .Rows(j)(6).ToString)
        '                    '            If (.Rows(j)(6)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). HauteurCpv = CStr(.Rows(j)(6))
        '                    '            'Debug.WriteLine("Di = " & .Rows(j)(7).ToString)
        '                    '            If (.Rows(j)(7)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). Di = CStr(.Rows(j)(7))
        '                    '            'Debug.WriteLine("Vc = " & .Rows(j)(8).ToString)
        '                    '            If (.Rows(j)(8)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). Vc = CStr(.Rows(j)(8))
        '                    '            'Debug.WriteLine("Vs = " & .Rows(j)(9).ToString)
        '                    '            If (.Rows(j)(9)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). Vs = CStr(.Rows(j)(9))
        '                    '            'Debug.WriteLine("EpMembrane = " & .Rows(j)(10).ToString)
        '                    '            If (.Rows(j)(10)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). EpMembrane = CStr(.Rows(j)(10))
        '                    '            'Debug.WriteLine("Prp = " & .Rows(j)(11).ToString)
        '                    '            If (.Rows(j)(11)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). Prp = CStr(.Rows(j)(11))
        '                    '            'Debug.WriteLine("PrV = " & .Rows(j)(12).ToString)
        '                    '            If (.Rows(j)(12)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). PrV = CStr(.Rows(j)(12))
        '                    '            'Debug.WriteLine("coefficientA = " & .Rows(j)(13).ToString)
        '                    '            If (.Rows(j)(13)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). coefficientA = CStr(.Rows(j)(13))
        '                    '            'Debug.WriteLine("coefficientB = " & .Rows(j)(14).ToString)
        '                    '            If (.Rows(j)(14)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). coefficientB = CStr(.Rows(j)(14))
        '                    '            'Debug.WriteLine("PrecisVc = " & .Rows(j)(15).ToString)
        '                    '            If (.Rows(j)(15)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). PrecisVc = CStr(.Rows(j)(15))
        '                    '            'Debug.WriteLine("PrecisVs = " & .Rows(j)(16).ToString)
        '                    '            If (.Rows(j)(16)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). PrecisVs = CStr(.Rows(j)(16))
        '                    '            ''Debug.WriteLine("PrEpMembrane = " & .Rows(j)(17).ToString)
        '                    '            If (.Rows(j)(17)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). PrEpMembrane = CStr(.Rows(j)(17))
        '                    '            'Debug.WriteLine("PrecisA = " & .Rows(j)(18).ToString)
        '                    '            If (.Rows(j)(18)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). PrecisA = CStr(.Rows(j)(18))
        '                    '            'Debug.WriteLine("PrecisB = " & .Rows(j)(19).ToString)
        '                    '            If (.Rows(j)(19)) IsNot Nothing Then MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). PrecisB = CStr(.Rows(j)(19))
        '                    '        End With

        '                    '        iMesureExpansion = 0
        '                    '        ' On charge mes Mesures correspondantes à cet essai RefEssai
        '                    '        For k = 0 To MyListeSonde.MesureExpansionCount - 1

        '                    '            ' On cherche les Essais de nom RefEssai dans la table "LesMesureExpansionPropre"
        '                    '            Chaine2 = MaListeSonde.Tables("LesMesureExpansionPropre").Rows(k)(2).ToString

        '                    '            If RefEssai = Chaine2 Then
        '                    '                ' On a trouvé un essai de la même RefEssai
        '                    '                iMesureExpansion = iMesureExpansion + 1

        '                    '                '' Vérifier si le tableau Expansion est initialisé
        '                    '                'If MyListeSonde. ListeSonde(i) MaExpansion Is Nothing Then
        '                    '                '    ReDim MyListeSonde. ListeSonde(i) MaExpansion(0)
        '                    '                'End If

        '                    '                ' Vérifier si Expansion(iExpansion) est instancié
        '                    '                If MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1) Is Nothing Then
        '                    '                    MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1) = New EssaiExpansion
        '                    '                End If

        '                    '                '' Redimensionner le tableau Mesures
        '                    '                'ReDim Preserve MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). Mesures(1, iMesureExpansion)

        '                    '                '    ' Instancier l'objet Expansion(iExpansion)
        '                    '                '    '   MyListeSonde. ListeSonde(i) MaExpansion(iExpansion).Mesures(2, iMesureExpansion) = New Mesure
        '                    '                With MaListeSonde.Tables("LesMesureExpansionPropre")
        '                    '                    ' Colonne 0 désigne le numéro du chargement
        '                    '                    'MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1).Mesures(0, iMesureExpansion - 1) = iMesureExpansion
        '                    '                    ' Colonne 0 désigne la pression
        '                    '                    'MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). Mesures(0, iMesureExpansion - 1) = CDbl(.Rows(k)(3))
        '                    '                    ' Colonne 1 désigne le volume
        '                    '                    'MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1). Mesures(1, iMesureExpansion - 1) = CDbl(.Rows(k)(4))

        '                    '                    'MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1).Mesures(0, iMesureExpansion - 1) = CStr(.Rows(iMesureExpansion - 1)(0))
        '                    '                    'MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1).Mesures(1, iMesureExpansion - 1) = CStr(.Rows(iMesureExpansion - 1)(1))
        '                    '                    'MyListeSonde. ListeSonde(i) MaExpansion(iExpansion - 1).Mesures(2, iMesureExpansion - 1) = CStr(.Rows(iMesureExpansion - 1)(2))                              End With
        '                    '                End With
        '                    '            End If

        '                    '            ' Remplir les propriétés

        '                    '        Next k

        '                    '    End If

        '                    'Next j


        '                    RefSonde = MyListeSonde.ListeSonde(i).RefSonde
        '                    'iResistance = 0

        '                    ' On lit toutes les lignes de la Table "LesEssaisResistanceDb" 
        '                    ' On cherche la premiere occurance de nom Re Essai
        '                    For j = 0 To MyListeSonde.ListeSonde.Count - 1

        '                        ' On cherche les Essais de nom RefProbe dans la table "LesEssaisResistanceDb"
        '                        Chaine = MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows(j)(1).ToString

        '                        If RefSonde = Chaine Then

        '                            ' -------------------- On a trouvé un essai de la meme sonde ---------------------------------------------
        '                            iResistance = iResistance + 1

        '                            RefEssai = MaListeSonde.Tables("LesEssaisResistanceDbPropre").Rows(j)(0).ToString

        '                            '' Vérifier si le tableau Resistance est initialisé
        '                            'If MyListeSonde. ListeSonde(i). Resistance Is Nothing Then
        '                            '    ReDim Preserve MyListeSonde. ListeSonde(i). Resistance(iResistance)
        '                            'End If

        '                            '' Redimensionner et instancier Resistance(iResistance) si nécessaire
        '                            'If iResistance >= UBound(MyListeSonde. ListeSonde(i). Resistance) Then
        '                            '    ReDim Preserve MyListeSonde. ListeSonde(i). Resistance(iResistance)
        '                            'End If

        '                            ' Instancier l'objet Resistance(iResistance)
        '                            'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1) = New EssaiResistance

        '                            ' Remplir les propriétés
        '                            'With MaListeSonde.Tables("LesEssaisResistanceDbPropre")
        '                            '    MyListeSonde. ListeSonde(i). MaResistance(iResistance - 1). refEssai = CStr(.Rows(j)(0))
        '                            '    MyListeSonde. ListeSonde(i). MaResistance(iResistance - 1). refSonde = CStr(.Rows(j)(1))
        '                            '    If Not IsDBNull(.Rows(j)(2)) Then
        '                            '        MyListeSonde. ListeSonde(i). MaResistance(iResistance - 1). ClassPrecis = CStr(.Rows(j)(2))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). ClassPrecis = 0
        '                            '    End If
        '                            '    If Not IsDBNull(.Rows(j)(3)) Then
        '                            '        'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).ListeSonde(i). ResistancenomEssai = CStr(.Rows(j)(3))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). nomEssai = 0
        '                            '    End If

        '                            '    If Not IsDBNull(.Rows(j)(4)) Then
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). dateEssai = CStr(.Rows(j)(4))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). nomEssai = 0
        '                            '    End If

        '                            '    If Not IsDBNull(.Rows(j)(5)) Then
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). difPresEauAir = CStr(.Rows(j)(5))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). difPresEauAir = 0
        '                            '    End If
        '                            '    If Not IsDBNull(.Rows(j)(6)) Then
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). hauteurCpv = CStr(.Rows(j)(6))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). hauteurCpv = 0
        '                            '    End If

        '                            '    If Not IsDBNull(.Rows(j)(7)) Then
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). A0 = CStr(.Rows(j)(7))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). A0 = 0
        '                            '    End If
        '                            '    If Not IsDBNull(.Rows(j)(8)) Then
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). A1 = CStr(.Rows(j)(8))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). A1 = 0
        '                            '    End If
        '                            '    If Not IsDBNull(.Rows(j)(9)) Then
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). A2 = CStr(.Rows(j)(9))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). A2 = 0
        '                            '    End If
        '                            '    If Not IsDBNull(.Rows(j)(10)) Then
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). A3 = CStr(.Rows(j)(10))
        '                            '    Else
        '                            '        MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). A3 = 0
        '                            '    End If
        '                            'End With

        '                            'iMesuresResistanceDb = 0
        '                            '' On charge mes Mesures correspondantes à cet essai RefEssai
        '                            'For k = 0 To MyListeSonde.MesureResistanceCount - 1

        '                            '    ' On cherche les Essais de nom RefEssai dans la table "LesMesuresResistanceDbPropre"
        '                            '    Chaine2 = MaListeSonde.Tables("LesMesuresResistanceDbPropre").Rows(k)(2).ToString

        '                            '    If RefEssai = Chaine2 Then
        '                            '        '---------------------  On a trouvé un essai de la même RefEssai  -----------------------
        '                            '        iMesuresResistanceDb = iMesuresResistanceDb + 1

        '                            '        '' Vérifier si le tableau Resistance est initialisé
        '                            '        'If MyListeSonde. ListeSonde(i). Resistance Is Nothing Then
        '                            '        '    ReDim MyListeSonde. ListeSonde(i). Resistance(0)
        '                            '        'End If

        '                            '        ' Vérifier si Resistance(iResistance) est instancié
        '                            '        If MyListeSonde. ListeSonde(i). Resistance(iResistance - 1) Is Nothing Then
        '                            '            MyListeSonde. ListeSonde(i). Resistance(iResistance - 1) = New EssaiResistance
        '                            '        End If

        '                            '        '' Redimensionner le tableau Mesures
        '                            '        'ReDim Preserve MyListeSonde. ListeSonde(i). Resistance(iResistance - 1). Mesures(2, iMesuresResistanceDb)

        '                            '        '    ' Instancier l'objet Resistance(iResistance)
        '                            '        '    '   MyListeSonde. ListeSonde(i). Resistance(iResistance).Mesures(2, iMesuresResistanceDb) = New Mesure
        '                            '        With MaListeSonde.Tables("LesMesuresResistanceDbPropre")
        '                            '            ' Colonne 0 désigne le numéro du chargement

        '                            '            'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).Mesures(0, iMesuresResistanceDb - 1) = iMesuresResistanceDb
        '                            '            ' Colonne 1 désigne la pression
        '                            '            If Not IsDBNull(.Rows(j)(3)) Then
        '                            '                'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).Mesures(1, iMesuresResistanceDb - 1) = CDbl(.Rows(k)(3))
        '                            '            Else
        '                            '                'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).Mesures(1, iMesuresResistanceDb - 1) = 0
        '                            '            End If

        '                            '            ' Colonne 2 désigne le volume
        '                            '            If Not IsDBNull(.Rows(j)(4)) Then
        '                            '                'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).Mesures(2, iMesuresResistanceDb - 1) = CDbl(.Rows(k)(4))
        '                            '            Else
        '                            '                'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).Mesures(2, iMesuresResistanceDb - 1) = 0

        '                            '            End If
        '                            '            'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).Mesures(0, iMesuresResistanceDb - 1) = CStr(.Rows(iMesuresResistanceDb - 1)(0))
        '                            '            'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).Mesures(1, iMesuresResistanceDb - 1) = CStr(.Rows(iMesuresResistanceDb - 1)(1))
        '                            '            'MyListeSonde. ListeSonde(i). Resistance(iResistance - 1).Mesures(2, iMesuresResistanceDb - 1) = CStr(.Rows(iMesuresResistanceDb - 1)(2))                              End With
        '                            '        End With

        '                            '    End If ' Fin du If RefEssai = Chaine2 Then

        '                            '    ' Remplir les propriétés

        '                            'Next k ' Fin du For k = 0 To MyListeSonde.MesureResistanceCount - 1

        '                        End If ' Fin du  If RefSonde = Chaine Then

        '                    Next j ' Fin du For j = 0 To MyListeSonde. ResistanceCount - 1

        '                End If ' Fin du If MaListeSonde.Tables("LesSondes").Rows(i) IsNot Nothing Then

        '            Next i ' Fin du For i = 0 To MaListeSonde.Tables("LesSondes").Rows().Count - 1



        '            ' Le chargement  s'étant effectuer correctement, la fonction renvoie vrai
        '            ChargeAvecBD = True
        '            IsSaved = True

        '            ' Vérification initiale
        '            ' Vérification de MaListeSonde.Tables("LesEssaisResistanceDbPropre")
        '            With MaListeSonde.Tables("LesEssaisResistanceDbPropre")
        '                For i1 = 0 To .Rows.Count - 1
        '                    Debug.WriteLine("ClasseListeSonde - LesEssaisResistanceDbPropre Init i = " & i & " RefEssai = " & .Rows(i1).Item("RefEssai").ToString)
        '                Next i1
        '            End With

        '            ' Vérification de MyListeSonde.Tables("LesEssaiForage")
        '            refSondes.Clear()
        '            For i1 = 0 To MyListeSonde.ListeSonde.Count - 1
        '                RowSonde = MyListeSonde.ListeSonde(i1)
        '                ReferenceSonde = RowSonde.refSonde.ToString()
        '                If refSondes.ContainsKey(ReferenceSonde) Then
        '                    MsgBox("Doublon détecté dans MyListeSonde pour refSonde: " & ReferenceSonde)
        '                    Exit Function
        '                Else
        '                    refSondes.Add(ReferenceSonde, True)
        '                End If
        '            Next i1

        '            ' Affectation de la Liste des Sondes à MyChantier
        '            For i1 = 0 To MyListeSonde.ListeSonde.Count - 1
        '                RowSonde = New Probe
        '                RowSonde = MyListeSonde.ListeSonde(i1)
        '                'If MyChantier.ListeSonde.Count - 1 <= i1 Then
        '                '    ReDim Preserve MyChantier.ListeSonde(i1)
        '                '    MyChantier.ListeSonde(i1) = New Probe
        '                'End If
        '                'MyChantier.ListeSonde(i1) = RowSonde
        '            Next i1
        '            Exit Function

        '            ' Traitement des erreurs lors du chargement de la base de données:
        'errFind:
        '            ' Fermeture de la connection
        '            ' La fonction renvoie faux
        '            MsgBox(Err.Description)
        '            ChargeAvecBD = False
        '        End Function

        Public Sub CloseListeSonde()

            ' Effacement de la référence vers le chantier
            MyListeSonde = Nothing

        End Sub

        ' Remplissage du tableau de la liste des sondes 
        'Public Sub AfficheDansTab(ByRef Table As DataGridView)
        '    Dim Taille As Integer

        '    Taille = MyListeSonde. ListeSonde.Count
        '    With Table
        '        .Rows.Clear()
        '        ' Remplissage du tableau rsEssaiTab de la liste des essais par sonde
        '        For i = 0 To Taille - 1
        '            .Rows.Add()
        '            '.Rows(i).Cells(0).Value = i
        '            .Rows(i).Cells(0).Value = MyListeSonde. ListeSonde(i). refSonde

        '            'SoupleTypeE = 0
        '            'SoupleTypeG = 1
        '            'TubeFenduCourte = 2
        '            'TubeFenduLongue = 3
        '            'Autre = 4
        '            Select Case MyListeSonde. ListeSonde(i). typeSonde
        '                Case 0
        '                    .Rows(i).Cells(1).Value = "SoupleTypeE"
        '                Case 1
        '                    .Rows(i).Cells(1).Value = "SoupleTypeG"
        '                Case 2
        '                    .Rows(i).Cells(1).Value = "TubeFenduCourte"
        '                Case 3
        '                    .Rows(i).Cells(1).Value = "TubeFenduLongue"
        '                Case 4
        '                    .Rows(i).Cells(1).Value = "Autre"

        '            End Select

        '        Next i
        '    End With
        '    '         Remplissage des l'enregistrements

        'End Sub

        'Public Sub AfficheDansCombo(ByRef Combo As System.Windows.Forms.ComboBox)

        '    ' Remplissage de l'enregistrement
        '    Dim i As Integer
        '    With Combo.Items
        '        .Clear()
        '        For i = 0 To MyListeSonde. ListeSonde.Count - 1
        '            .Add(MyListeSonde. ListeSonde(i). refSonde)
        '        Next i
        '    End With
        'End Sub


        Public Sub NouvelleSonde(ByVal NewProbe As Boolean, ByVal NewTable As Boolean)
            ' NewProbe est Vrai quand on Crée une Nouvelle Sonde; sinon Faux
            ' NewTable est Vrai quand on Crée une Nouvelle Table MaListeSonde.Tables("LesSondes"); sinon Faux

            ' Déclaration d'un forage

            Dim NouvelleSonde As Probe
            'Dim ObjetDataRow As DataRow

            NouvelleSonde = Nothing

            ' Incrémentation du compteur des sondes si Newprobe est Vrai
            'If NewProbe Then
            '    SondeCount = SondeCount + 1
            'End If

            ' Redimensionnement du tableau dynamique des Sondes
            ' Instanciation d
            If NouvelleSonde Is Nothing Then
                NouvelleSonde = New Probe
            End If

            ' Enregistrement de la nouvelle sonde dans le tableau de sondes
            'If MyListeSonde. ListeSonde Is Nothing Then
            '    ReDim Preserve MyListeSonde. ListeSonde(0)
            '    MyListeSonde. ListeSonde(0) = New Probe
            'Else
            '    If SondeCount >= MyListeSonde. ListeSonde.Count Then
            '        Dim iMax As Integer
            '        iMax = MyListeSonde. ListeSonde.Count
            '        ReDim Preserve MyListeSonde. ListeSonde(iMax)
            '        MyListeSonde. ListeSonde(iMax) = New Probe
            '        'ListeSonde.Add(NouvelleSonde)
            '    Else
            '        MyListeSonde. ListeSonde(SondeCount - 1) = NouvelleSonde
            '    End If
            'End If

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

            '        ' Création de la Table("LesSondes") dans MaListeSonde
            '        MonChantier.Tables.Add("LesSondes")
            '        With MaListeSonde.Tables("LesSondes")
            '            .Columns.Add("refSonde", GetType(String))
            '            .Columns.Add("typeSonde", GetType(Int32))
            '            .Columns.Add("typeDeformee", GetType(Int32))
            '            .Columns.Add("typeContrainte", GetType(Int32))
            '            .Columns.Add("ls", GetType(Single))
            '            .Columns.Add("lg", GetType(Single))
            '            .Columns.Add("lt", GetType(Single))
            '            .Columns.Add("longInjectPompage", GetType(Single))
            '            .Columns.Add("Vs", GetType(Single))
            '            .Columns.Add("ds", GetType(Single))
            '            .Columns.Add("ClassPrecis", GetType(Int32))
            '            .Columns.Add("EpMembraneGarde", GetType(Single))
            '            .Columns.Add("PrEpMembraneGarde", GetType(Single))
            '            .Columns.Add("EpMembraneMesure", GetType(Single))
            '            .Columns.Add("PrEpMembraneMesure", GetType(Single))
            '            .Columns.Add("epTubeFendu", GetType(Single))
            '            .Columns.Add("PrEpTubeFendu", GetType(Single))
            '            .Columns.Add("EpInitMembrane", GetType(Single))
            '            .Columns.Add("PrEpInitMembrane", GetType(Single))
            '            .Columns.Add("PrLong", GetType(Single))
            '            .Columns.Add("PrDiam", GetType(Single))
            '            .Columns.Add("PrP", GetType(Single))
            '            .Columns.Add("PrV", GetType(Single))
            '            .Columns.Add("PrPV", GetType(Single))
            '            .Columns.Add("PrZ", GetType(Single))
            '            .Columns.Add("Prt", GetType(Single))
            '            .Columns.Add("Pm", GetType(Single))
            '        End With
            '    Else
            '        '    ' Ajout d'une ligne à la Table "LesSondes"
            '        '    ObjetDataRow = MaListeSonde.Tables("LesSondes").NewRow()

            '        '    ObjetDataRow("refSonde") = MyListeSonde.GiveMeANewRefSonde
            '        '    ObjetDataRow("typeSonde") = MySonde. typeSonde
            '        '    ObjetDataRow("typeDeformee") = MySonde. typeDeformee
            '        '    ObjetDataRow("typeContrainte") = MySonde. typeContrainte
            '        '    ObjetDataRow("ls") = MySonde. ls
            '        '    ObjetDataRow("lg") = MySonde. lg
            '        '    ObjetDataRow("lt") = MySonde. lt
            '        '    ObjetDataRow("longInjectPompage") = MySonde. longInjectePompage
            '        '    ObjetDataRow("Vs") = MySonde. Vs
            '        '    ObjetDataRow("ds") = MySonde. ds
            '        '    ObjetDataRow("ClassPrecis") = MySonde. ClassPrecis
            '        '    ObjetDataRow("EpMembraneGarde") = MySonde. EpMembraneGarde
            '        '    ObjetDataRow("PrEpMembraneGarde") = MySonde. PrEpMembraneGarde
            '        '    ObjetDataRow("EpMembraneMesure") = MySonde. EpMembraneMesure
            '        '    ObjetDataRow("PrEpMembraneMesure") = MySonde. PrEpMembraneMesure

            '        '    ObjetDataRow("EpTubeFendu") = MySonde. epTubeFendu
            '        '    ObjetDataRow("PrEpTubeFendu") = MySonde. PrEpTubeFendu

            '        '    ObjetDataRow("EpInitMembrane") = MySonde. EpInitMembrane
            '        '    ObjetDataRow("PrEpInitMembrane") = MySonde. PrEpInitMembrane

            '        '    ObjetDataRow("PrLong") = MySonde. PrLong
            '        '    ObjetDataRow("PrDiam") = MySonde. PrDiam
            '        '    ObjetDataRow("PrP") = MySonde. PrP
            '        '    ObjetDataRow("PrV") = MySonde. PrV
            '        '    ObjetDataRow("PrPV") = MySonde. PrPV
            '        '    ObjetDataRow("PrZ") = MySonde. PrZ
            '        '    ObjetDataRow("Prt") = MySonde. Prt
            '        '    ' ObjetDataRow("EpMembraneMesure") = Mysonde. EpMembraneMesure
            '        '    ObjetDataRow("Pm") = MySonde. Pm

            '        '    MaListeSonde.Tables("LesSondes").Rows.Add(ObjetDataRow)

            '        '    'MaListeSonde.Tables("LesSondes").Rows.Add()
            '        'End If


            '    End If


            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

        End Sub

        Public Function SupSonde(ByVal ref As String) As Boolean
            Dim i As Integer    ' Index de la sonde à Delete
            Dim J As Integer

            On Error GoTo errFind

            ' Récupération de l'indice de la sonde d'après sa référence
            i = GetIndexSonde(ref)

            ' Enregistrement de la sonde dans le tableau des sondes supprimées:
            ' Incrémentation du compteur de sondes à Delete
            'SondeSupCount = SondeSupCount + 1

            '' Redimensionnement du tableau dynamique des sondes à Delete
            'ReDim Preserve SondesSup(SondeSupCount - 1)

            '' Effaçage de la sonde
            'SondesSup(SondeSupCount - 1) = Sondes(i)

            '' Décalage des sondes suivantes d'une case
            'For J = i To SondeCount - 2
            '    Sondes(J) = Sondes(J + 1)
            'Next

            '' Redimensionnement du tableau Sondes
            'If SondeCount > 1 Then
            '    ReDim Preserve Sondes(SondeCount - 2)
            'Else
            '    ReDim Sondes(0)
            'End If

            ' Décrémentation du compteur de sondes
            'SondeCount = SondeCount - 1

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

        'Public Function GetIsSaved() As Boolean

        '    If Not IsSaved And SondeCount > 0 Then
        '        Dim i As Integer
        '        IsSaved = True
        '        For i = 0 To SondeCount - 1
        '            IsSaved = IsSaved And Sondes(i).GetIsSaved
        '            If Not IsSaved Then Exit For
        '        Next
        '    End If
        '    GetIsSaved = IsSaved
        'End Function

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
