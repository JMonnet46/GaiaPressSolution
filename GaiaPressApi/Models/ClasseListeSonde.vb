Namespace GaiaPress.core.V17.Models

    Public Class ListeSondes
        Inherits DbContext

        ' ------Tables -----------------------------------------
        Public Property P_ListeSondes As New List(Of Probe)
        'Public Sondes() As Probe
        'Public SondesSup() As Probe

        ' ------------------------------------------------------
        Public FichierListeSondes As String
        Public EventsSuspendus As Boolean
        Public IsSaved As Boolean
        Public Event HasBeenModified()


        ' ================================================================================
        ' Gestion de  P_ListeSondes
        ' ================================================================================
        ' Add une Sonde
        Public Sub AddSonde(Sonde As Probe)
            Me.P_ListeSondes.Add(Sonde)
            ' Lier l Esai a la sonde (si nécessaire)
            MyEssai.E_RefEssai = Sonde.P_refSonde
        End Sub


        Public Function GetSonde(refSonde As String) As Probe
            For Each S As Probe In Me.P_ListeSondes
                If S.P_refSonde = refSonde Then
                    Return S
                End If
            Next
            Return Nothing
        End Function

        ' Delete une Sonde par référence
        Public Sub DeleteSonde(refSonde As String)
            Dim SondeADelete As Probe = GetSonde(refSonde)
            If SondeADelete IsNot Nothing Then
                Me.P_ListeSondes.Remove(SondeADelete)
            End If
        End Sub

        ''' <summary>
        ''' Met à jour une sonde dans la liste P_ListeSondes.
        ''' Si la sonde n'existe pas, elle est ajoutée.
        ''' </summary>
        ''' <param name="sondeToUpdate">Instance de Probe à mettre à jour ou Add.</param>
        Public Sub UpdateProbe(sondeToUpdate As Probe)
            ' Vérifier si la sonde existe déjà dans P_ListeSondes
            Dim existingSonde As Probe = Me.P_ListeSondes.FirstOrDefault(Function(s) s.P_refSonde = sondeToUpdate.P_refSonde)

            If existingSonde IsNot Nothing Then
                ' ✅ Mettre à jour les propriétés de la sonde existante
                existingSonde.P_refSonde = sondeToUpdate.P_refSonde
                ' Copier les autres propriétés de sondeToUpdate vers existingSonde
                ' Exemple :
                ' existingSonde.P_typeSonde = sondeToUpdate.sondeToUpdate.P_typeSonde
                existingSonde = sondeToUpdate
                'existingSonde.P_refSonde = sondeToUpdate.P_refSonde
                'existingSonde.P_NomSonde = sondeToUpdate.P_NomSonde
                'existingSonde.P_ClassPrecis = sondeToUpdate.P_ClassPrecis
                'existingSonde.P_PrecisVolSonde = sondeToUpdate.P_PrecisVolSonde
                'existingSonde.P_typeSonde = sondeToUpdate.P_typeSonde
                'existingSonde.P_typeDeformee = sondeToUpdate.P_typeDeformee
                'existingSonde.P_typeContrainte = sondeToUpdate.P_typeContrainte
                'existingSonde.P_ls = sondeToUpdate.P_ls  ' Longueur de la cellule de mesure
                'existingSonde.P_lg = sondeToUpdate.P_lg   ' Longueur des cellules de garde
                'existingSonde.P_lt = sondeToUpdate.P_lt  ' Longueur totale de la partie déformable de la sonde
                'existingSonde.P_longInjectePompage = sondeToUpdate.P_longInjectePompage
                'existingSonde.P_Vs = sondeToUpdate.P_Vs    ' Volume de la sonde
                'existingSonde.P_ds = sondeToUpdate.P_ds   ' Diamètre intérieur du Tube de calibration
                'existingSonde.P_EpMembraneGarde = sondeToUpdate.P_EpMembraneGarde
                'existingSonde.P_PrEpMembraneGarde = sondeToUpdate.P_PrEpMembraneGarde
                'existingSonde.P_EpMembraneMesure = sondeToUpdate.P_EpMembraneMesure
                'existingSonde.P_PrEpMembraneMesure = sondeToUpdate.P_PrEpMembraneMesure
                'existingSonde.P_epTubeFendu = sondeToUpdate.P_epTubeFendu
                'existingSonde.P_PrEpTubeFendu = sondeToUpdate.P_PrEpTubeFendu
                'existingSonde.P_EpInitMembrane = sondeToUpdate.P_EpInitMembrane
                'existingSonde.P_PrEpInitMembrane = sondeToUpdate.P_PrEpInitMembrane
                'existingSonde.P_PrLong = sondeToUpdate.P_PrLong
                'existingSonde.P_PrDiam = sondeToUpdate.P_PrDiam
                'existingSonde.P_PrP = sondeToUpdate.P_PrP
                'existingSonde.P_PrV = sondeToUpdate.P_PrV
                'existingSonde.P_PrPV = sondeToUpdate.P_PrPV
                'existingSonde.P_PrZ = sondeToUpdate.P_PrZ
                'existingSonde.P_Prt = sondeToUpdate.P_Prt
                'existingSonde.P_Pm = sondeToUpdate.P_Pm
                'existingSonde.P_Vs = sondeToUpdate.P_Vs
                'existingSonde.P_PrecisA = sondeToUpdate.P_PrecisA
                'existingSonde.P_PrecisB = sondeToUpdate.P_PrecisB
                'existingSonde.P_PrecisVs = sondeToUpdate.P_PrecisVs
                'existingSonde.P_PrecisVc = sondeToUpdate.P_PrecisVc

            Else
                ' ✅ Add la nouvelle sonde à la liste
                Me.P_ListeSondes.Add(sondeToUpdate)
            End If
        End Sub







        'Public Sub New()
        '    Dim Index As Integer
        '    Index = 0
        '    ' Création de la feuille de liste
        '    If MyListeSondes Is Nothing Then
        '        Index = 0
        '        MyListeSondes = Me
        '        ' Initialize other properties or perform other setup tasks here
        '    Else
        '        Index = -1
        '        If MySonde IsNot Nothing Then Index = GetIndexSonde(MySonde.GetNomTypeSonde)
        '        If Index < 0 Then
        '            ' Index = 0
        '            MyListeSondeCount = Nothing
        '            Exit Sub
        '        End If
        '        'Call MyListeSondes.MettreAJourSonde(MySonde, Index)
        '        Call MyListeSondes.MettreAJourSonde(MySonde.P_refSonde, "nouveauNom", "nouvelledate")
        '    End If

        '    ' Enregistrement de la référence du chantier auprès de sa feuille
        '    ' Call MyListeSondes.SetSonde(MySonde, Index)

        'End Sub

        ' Propriété pour obtenir le nombre de sondes
        Public ReadOnly Property Count() As Integer
            Get
                Return P_ListeSondes.Count
            End Get
        End Property

        '' Méthode pour Add une sonde
        '' Méthode pour Add une sonde
        'Public Sub AddSonde(ByVal sonde As Probe)
        '    Dim iMax As Integer
        '    iMax = P_ListeSondes.Count
        '    'ReDim Preserve MyListeSondes.P_ListeSondes(iMax)
        '    MyListeSondes.P_ListeSondes(iMax) = New Probe
        '    MyListeSondes.P_ListeSondes(iMax) = sonde
        'End Sub


        'Public Function SetSonde(ByRef Sonde As Probe, ByVal Index As Integer) As Probe
        '    Dim iMax As Integer
        '    iMax = P_ListeSondes.Count

        '    If Index = -1 Then
        '        'ReDim Preserve MyListeSondes.p_ListeSondes(0)
        '        MyListeSondes.p_ListeSondes(0) = New Probe
        '        MyListeSondes.p_ListeSondes(0) = Sonde

        '    ElseIf Index < MyListeSondes.p_ListeSondes.Count Then
        '        MyListeSondes.p_ListeSondes(Index) = Sonde
        '        MyListeSondes.p_ListeSondes(Index) = Sonde
        '    ElseIf Index >= iMax Then
        '        ReDim Preserve MyListeSondes.p_ListeSondes(iMax)
        '        MyListeSondes.p_ListeSondes(iMax) = New Probe
        '        MyListeSondes.p_ListeSondes(iMax) = Sonde
        '    End If
        '    SetSonde = Sonde
        'End Function

        Default Public Property Item(ByVal index As Integer) As Probe
            Get
                Return MyListeSondes.P_ListeSondes(index)
            End Get
            Set(ByVal value As Probe)
                MyListeSondes.P_ListeSondes(index) = value
            End Set
        End Property

        '' *****************************************************************************************************************************************
        ''     On enregistre dans MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j) les caracteristiques de l'essai d Expansion refEssai
        ''     On enregistre dans MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j) les caracteristiques de l'essai de Resistance refEssai
        '' ****************************************************************************************************************************************
        'Public Sub ChargeMesures(ByVal ReferenceEssai As String, ByVal i As Integer, ByVal TypeEssai As Integer)
        '    Dim j, k As Integer

        '    'MaListeSondes.Tables("LesEssaisResistancePropre").AcceptChanges()

        '    Select Case TypeEssai
        '        Case EssaiCalib.Resistance
        '            k = 0
        '            For j = 0 To MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Count - 1

        '                ' Si la RefProbe de la Table("LesSondes") est la même quand dans la table("LesEssaisResistancePropre") 
        '                ' alors on a trouvé un nouvel essai de Resistance
        '                If ReferenceEssai = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("RefEssai").ToString Then
        '                    ' On déclare un nouvel essai de Résistance propre
        '                    MyListeSondes.P_ListeSondes(i).NouvelEssai(EssaiCalib.Resistance)
        '                    ' On affecte à l'item créé, l'adresse de l'essai de résistance correspondant
        '                    MyListeSondes.P_ListeSondes(i).P_refEssai = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("refEssai").ToString
        '                    MyListeSondes.P_ListeSondes(i).P_refSonde = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("refSonde").ToString
        '                    MyListeSondes.P_ListeSondes(i).P_ClassPrecis = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("ClassPrecis")
        '                    MyListeSondes.P_ListeSondes(i).P_refEssai = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("refEssai").ToString
        '                    MyListeSondes.P_ListeSondes(i).P_Resistance(k).R_dateEssai = Convert.ToDateTime(MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("dateEssai").ToString)
        '                    MyListeSondes.P_ListeSondes(i).P_Resistance(k).R_hauteurCpv = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("hauteurCpv").ToString
        '                    'MyListeSondes.P_ListeSondes(i).P_Resistance(k).R_Mesures.Count = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("R_Mesures.count").ToString

        '                    MyListeSondes.P_ListeSondes(i).P_Resistance(k).R_A0 = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("A0").ToString
        '                    MyListeSondes.P_ListeSondes(i).P_Resistance(k).R_A1 = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("A1").ToString
        '                    MyListeSondes.P_ListeSondes(i).P_Resistance(k).R_A2 = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("A2").ToString
        '                    MyListeSondes.P_ListeSondes(i).P_Resistance(k).R_A3 = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j).Item("A3").ToString
        '                    k = k + 1
        '                End If
        '            Next

        '        Case EssaiCalib.Expansion
        '            k = 0
        '            For j = 0 To MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count - 1

        '                ' Si la RefProbe de la Table("LesSondes") est la même quand dans la table("LesEssaisExpansionPropre") 
        '                ' alors on a trouvé un nouvel essai d'expansion
        '                If ReferenceEssai = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("RefEssai").ToString Then
        '                    ' On déclare un nouvel essai d'Expansion propre
        '                    MyListeSondes.P_ListeSondes(i).NouvelEssai(EssaiCalib.Expansion)
        '                    ' On affecte à l'item créé, l'adresse de l'essai d'expansion correspondant
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_RefEssai = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("RefEssai").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_RefSonde = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("RefSonde").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_ClassPrecis = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("ClassPrecis")
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_nomEssai = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("NomEssai").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_dateEssai = Convert.ToDateTime(MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("DateEssai").ToString)
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_DifPresEauAir = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("DifPresEauAir").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_HauteurCpv = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("HauteurCpv").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_Di = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("Di").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_Vc = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("Vc").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_Vs = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("Vs").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_EpMembrane = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("EpMembrane").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_PrEpMembrane = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("PrEpMembrane").ToString
        '                    'MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_Mesures.Count = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("R_Mesures.count").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_Prp = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("Prp").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_PrV = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("PrV").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_coefficientA = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("coefficientA").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_coefficientB = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("coefficientB").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_PrecisVc = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("PrecisVc").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_PrecisVs = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("PrecisVs").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_PrecisA = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("PrecisA").ToString
        '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(k).E_PrecisB = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j).Item("PrecisB").ToString

        '                    k = k + 1
        '                End If
        '                '  MyListeSondes.p_ListeSondes(i)P_MaExpansion = True
        '            Next

        '    End Select

        '    'MaListeSondes.Tables("LesEssaisResistancePropre").AcceptChanges()

        'End Sub

        Public Function ChargeAvecBD(ByVal nomFichier As String) As Boolean
            Dim Requete, RefProbe As String
            Dim i, j, iResistance, iExpansion As Integer
            'Dim typeSonde, typeDeformee, typeContrainte, ClassPrecis As Int32
            'Dim ls, Lt, Lg, longInjectPompage, Vs, ds, EpInitMembrane, PrEpInitMembrane As Single
            'Dim epMembraneGarde, PrEpMembraneGarde, epMembraneMesure, PrepMembraneMesure, epTubeFendu, PrEpTubeFendu, Pm As Single
            'Dim epMembraneMesureBytes, PrEpMembraneMesureBytes As Byte()
            Dim RefSonde, RefEssai, Chaine As String
            'Dim newRow As DataRow
            Dim MyExpansion As New Essai_Expansion
            Dim MyResistance As New Essai_Resistance
            Dim refSondes As New Dictionary(Of String, Boolean)
            Dim RowSonde As Probe
            Dim ReferenceSonde As String

            ' On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

            ' ***************************************************************************
            ' Connexion avec la Base de Données
            ' ***************************************************************************

            ' Création d'une connection avec la base de données de type Access 2000


            ' *********************************************************************
            ' Lecture de la Table "LesSondes"
            ' *********************************************************************
            'ma requete pour lire les enregistrements du fichier Sondes.mdb
            ' Requete = "SELECT refSonde, typeSonde, typeDeformee, typeContrainte, ls, lg, lt, longInjectPompage, Vs, ds, " &
            '"epMembraneGarde,  PrEpMembraneGarde, epMembraneMesure, PrEpMembraneMesure, epMembraneMesure, PrepMembraneMesure,epTubeFendu, PrEpTubeFendu, " &
            '" Pm FROM LesSondes"

            Requete = "SELECT refSonde, typeSonde, typeDeformee, typeContrainte, ls, lg, lt, longInjectPompage, Vs, ds, " &
              "ClassPrecis, epMembraneGarde, PrEpMembraneGarde, epMembraneMesure, PrEpMembraneMesure, epTubeFendu, PrEpTubeFendu, " &
              "epaisInitMembrane, PrEpaisInitMembrane, PrLong, PrDiam, PrP, PrV, PrPV, PrZ, PrT, Pm FROM LesSondes"

            ' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            'MaListeSondes.AcceptChanges()

            ' Nettoyage de MaListeSondes avant le chargement
            If MaListeSondes.Tables("LesSondes") IsNot Nothing Then
                MaListeSondes.Tables("LesSondes").Clear()
            End If

            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSondes
            ' AdaptateurSondes.Fill(MesSondes, "LesEssaisExpansionPropre")

            ' Enregistre la nouvelle Table dans le Mapping
            'AdaptateurSondes.TableMappings.Add("LesSondes")

            '' Mise à jour du compteur d'enregistrements
            'SondeCount = MaListeSondes.Tables("LesSondes").Rows.Count
            'SondeCountInit = MaListeSondes.Tables("LesSondes").Rows.Count

            ' MaListeSondes contient toute la table "LesSondes"
            Select Case Lang
                Case 0
                    MsgBox("Reading of the Table 'LesSondes' suceeded")
                Case 1
                    MsgBox("Lecture de la Table 'LesSondes' réussie")
            End Select


            ' *********************************************************************
            ' Lecture de la Table "LesEssaisExpansionPropre"
            ' *********************************************************************
            'ma requete
            If Not (AncienneSonde) Then
                Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai, difPresEauAir, hauteurCpv" &
                ", Di, Vc, Vs, EpMembrane, Prp, PrV, coefficientA, coefficientB, PrecisVc, PrecisVs, PrEpMembrane, PrecisA, PrecisB" &
                " FROM LesEssaisExpansionPropre"
                'Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai" &
                '" FROM LesEssaisExpansionPropre"
            Else
                Requete = "Select refEssai, refSonde, nomEssai, dateEssai, difPresEauAir, hauteurCpv, Di, Vc, coefficientA" &
               " FROM LesEssaisExpansionPropre"
            End If


            ' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            'MaListeSondes.AcceptChanges()

            ' Nettoyage de MaListeSondes avant le chargement
            MaListeSondes.Tables("LesEssaisExpansionPropre")?.Clear()

            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSondes

            'If MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count = 0 Then
            '    MsgBox("Aucune donnée trouvée dans la table LesEssaisExpansionPropre.")
            'Else
            '    MsgBox("Les données ont été correctement chargées.")
            'End If

            ' Enregistre la nouvelle Table dans le Mapping
            'AdaptateurSondes.TableMappings.Add("LesEssaisExpansionPropre")

            '' Mise à jour du compteur d'enregistrements
            'ExpansionCount = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count
            'ExpansionCountInit = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count

            ' MaListeSondes contient toute la table "LesEssaisExpansionPropre"
            Select Case Lang
                Case 0
                    MsgBox("Reading Of the Table 'LesEssaisExpansionPropre' suceeded")
                Case 1
                    MsgBox("Lecture de la Table 'LesEssaisExpansionPropre' réussie")
            End Select


            ' *********************************************************************
            ' Lecture de la Table "LesEssaisResistance"
            ' *********************************************************************
            'ma requete
            Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai, difPresEauAir, hauteurCpv, A0, A1, A2, A3" &
                 " FROM LesEssaisResistancePropre"

            'Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai, difPresEauAir, hauteurCpv, A0, A1, A2, A3" & _
            '     " FROM LesEssaisResistancePropre"

            ' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            'MaListeSondes.AcceptChanges()

            ' Nettoyage de MaListeSondes avant le chargement
            MaListeSondes.Tables("LesEssaisResistancePropre")?.Clear()

            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSondes
            ' MaListeSondes contient toute la table "LesEssaisResistancePropre"

            '' Mise à jour du compteur d'enregistrements
            'ResistanceCount = MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Count
            'ResistanceCountInit = MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Count

            Select Case Lang
                Case 0
                    MsgBox("Reading of the Table 'LesEssaisResistancePropre' suceeded")
                Case 1
                    MsgBox("Lecture de la Table 'LesEssaisResistancePropre' réussie")
            End Select

            ' *********************************************************************
            ' Lecture de la Table "LesMesuresResistancePropre"
            ' *********************************************************************
            'ma requete
            Requete = "SELECT Ind, indiceMesure, refEssai, pression, volume FROM LesMesuresResistancePropre "

            ' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            'MaListeSondes.AcceptChanges()

            ' Nettoyage de MaListeSondes avant le chargement
            MaListeSondes.Tables("LesMesuresResistancePropre")?.Clear()

            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSondes
            ' MaListeSondes contient toute la table "LesMesuresResistancePropre"

            '' Mise à jour du compteur d'enregistrements
            'MesureResistanceCount = MaListeSondes.Tables("LesMesuresResistancePropre").Rows.Count
            'MesureResistanceCountInit = MaListeSondes.Tables("LesMesuresResistancePropre").Rows.Count

            Select Case Lang
                Case 0
                    MsgBox("Reading of the Table 'LesMesuresResistancePropre' suceeded")
                Case 1
                    MsgBox("Lecture de la Table 'LesMesuresResistancePropre' réussie")
            End Select



            ' *********************************************************************
            ' Lecture de la Table "LesMesuresExpansionPropre"
            ' *********************************************************************
            'ma requete
            Requete = "SELECT Ind, indiceMesure, refEssai, pression, volume FROM LesMesuresExpansionPropre "

            ' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            'MaListeSondes.AcceptChanges()

            ' Nettoyage de MaListeSondes avant le chargement
            MaListeSondes.Tables("LesMesuresExpansionPropre")?.Clear()

            ' rempli le dataset depuis la BD vers la Table "LesSondes" de MaListeSondes
            ' MaListeSondes contient toute la table "LesMesuresExpansionPropre"

            '' Mise à jour du compteur d'enregistrements
            'MesureExpansionCount = MaListeSondes.Tables("LesMesuresExpansionPropre").Rows.Count
            'MesureExpansionCountInit = MaListeSondes.Tables("LesMesuresExpansionPropre").Rows.Count

            Select Case Lang
                Case 0
                    MsgBox("Reading of the Table 'LesMesuresExpansionPropre' suceeded")
                Case 1
                    MsgBox("Lecture de la Table 'LesMesuresExpansionPropre' réussie")
            End Select


            ' *****************************************************
            ' Sauvegarde des données de connection
            ' *****************************************************
            'MyListeSondes.connSonde = connSonde
            With MyListeSondes
                '    '.AdaptateurSondes = AdaptateurSondes
                '    ' Initialisation de l'objet MyListeSonde
                '.Count = 0
                '.SondeCount = 0
                '.ExpansionCount = 0
                '.p_ResistanceCount = 0
                '.MesureExpansionCount = 0
                '.MesureResistanceCount = 0
            End With

            ' **************************************************************************************
            ' Rangement dans MyListeSondesdes objets de la Tables("LesSondes") de MaListeSondes.
            ' **************************************************************************************

            '    '' Création des objets Sondes
            '    MyListeSondes.p_ListeSondes(i).ChargeAvecBD(connSonde, MySonde, RefProbe, i)
            '    'MaListeSondes.Sondes(i).ChargeAvecBD(connSonde, MySonde, RefProbe, i)
            '    EventsSuspendus = False
            'End If

            ' Remplissage de MyListeSonde
            ' i est le numéro de la sonde
            ' For Each Ligne As DataRow In MesSondes.Tables("LesEssaisExpansionPropre").Rows()
            For i = 0 To MaListeSondes.Tables("LesSondes").Rows().Count - 1

                If MaListeSondes.Tables("LesSondes").Rows(i) IsNot Nothing Then
                    ' Nouvelle Sonde

                    MyListeSondes.NouvelleSonde(True, False)
                    'RefProbe = MesSondes.Tables("LesSondes").Rows(i).Item("refSonde").ToString
                    RefProbe = MaListeSondes.Tables("LesSondes").Rows(i).Item("refSonde").ToString
                    ' Création des objets Sondes
                    EventsSuspendus = True
                    MyListeSondes.P_ListeSondes(i).P_refSonde = MaListeSondes.Tables("LesSondes").Rows(i).Item("RefSonde").ToString
                    MyListeSondes.P_ListeSondes(i).P_typeSonde = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("typeSonde"))
                    MyListeSondes.P_ListeSondes(i).P_typeDeformee = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("typeDeformee"))
                    MyListeSondes.P_ListeSondes(i).P_typeContrainte = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("typeContrainte"))
                    MyListeSondes.P_ListeSondes(i).P_ls = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("ls").ToString)
                    MyListeSondes.P_ListeSondes(i).P_lg = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("lg").ToString)
                    MyListeSondes.P_ListeSondes(i).P_lt = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("lt").ToString)
                    'MyListeSondes.p_ListeSondes(i).P_longInjectePompage = MaListeSondes.Tables("LesSondes").Rows(i).Item("longInjectePompage").ToString)
                    MyListeSondes.P_ListeSondes(i).P_Vs = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("Vs").ToString)
                    MyListeSondes.P_ListeSondes(i).P_ds = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("ds").ToString)
                    If Not IsDBNull(MaListeSondes.Tables("LesSondes").Rows(i).Item("ClassPrecis")) Then
                        MyListeSondes.P_ListeSondes(i).P_ClassPrecis = MaListeSondes.Tables("LesSondes").Rows(i).Item("ClassPrecis").ToString
                    Else
                        ' Assigner une valeur par défaut si la valeur est DBNull
                        MyListeSondes.P_ListeSondes(i).P_ClassPrecis = "0" ' ou une autre valeur par défaut
                    End If

                    'MyListeSondes.p_ListeSondes(i).P_ClassPrecis = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("ClassPrecis"))
                    MyListeSondes.P_ListeSondes(i).P_EpMembraneGarde = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("EpMembraneGarde").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrEpMembraneGarde = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrEpMembraneGarde").ToString)
                    MyListeSondes.P_ListeSondes(i).P_EpMembraneMesure = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("epMembraneMesure").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrEpMembraneMesure = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrepMembraneMesure").ToString)
                    MyListeSondes.P_ListeSondes(i).P_epTubeFendu = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("epTubeFendu").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrEpTubeFendu = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrepTubeFendu").ToString)
                    MyListeSondes.P_ListeSondes(i).P_EpInitMembrane = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("epaisInitMembrane").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrEpInitMembrane = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrEpaisInitMembrane").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrLong = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrLong").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrDiam = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrDiam").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrP = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrP").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrV = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrV").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrPV = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrPV").ToString)
                    MyListeSondes.P_ListeSondes(i).P_PrZ = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("PrZ").ToString)
                    MyListeSondes.P_ListeSondes(i).P_Prt = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("Prt").ToString)
                    MyListeSondes.P_ListeSondes(i).P_Pm = Valeurs(MaListeSondes.Tables("LesSondes").Rows(i).Item("Pm").ToString)

                    ' i correspond à la Sonde RefSonde

                    RefSonde = MyListeSondes.P_ListeSondes(i).P_refSonde
                    iExpansion = 0
                    iResistance = 0

                    ' On lit toutes les lignes de la Table "LesEssaisExpansion" 
                    ' On cherche la premiere occurance de nom Re Essai
                    'For j = 0 To MyListeSondes.P_ListeSondes(i).Count - 1

                    '    ' On cherche les Essais de nom RefProbe dans la table "LesEssaisExpansion"
                    '    Chaine = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j)(1).ToString

                    '    If RefSonde = Chaine Then

                    '        ' On a trouvé un essai de la meme sonde
                    '        iExpansion = iExpansion + 1

                    '        RefEssai = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(j)(0).ToString

                    '        ' Vérifier si le tableau Expansion est initialisé
                    '        'If MyListeSondes.P_ListeSondes(i)P_MaExpansion Is Nothing Then
                    '        '    ReDim Preserve MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion)
                    '        'End If

                    '        ' Redimensionner et instancier Expansion(iExpansion) si nécessaire
                    '        'If iExpansion >= UBound(MyListeSondes.P_ListeSondes(i)P_MaExpansion) Then
                    '        '    ReDim Preserve MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion)
                    '        'End If

                    '        ' Instancier l'objet Expansion(iExpansion)
                    '        MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1) = New Essai_Expansion

                    '        ' Remplir les propriétés
                    '        With MaListeSondes.Tables("LesEssaisExpansionPropre")
                    '            'Debug.WriteLine("refEssai = " & .Rows(j)(0).ToString)
                    '            If (.Rows(j)(0)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_RefEssai = CStr(.Rows(j)(0))
                    '            'Debug.WriteLine("RefSonde = " & .Rows(j)(1).ToString)
                    '            If (.Rows(j)(1)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_RefSonde = CStr(.Rows(j)(1))
                    '            'Debug.WriteLine("ClassPrecis = " & .Rows(j)(2).ToString)
                    '            If (.Rows(j)(2)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_ClassPrecis = CStr(.Rows(j)(2))
                    '            'Debug.WriteLine("nomEssai = " & .Rows(j)(3))
                    '            If (.Rows(j)(3)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_nomEssai = CStr(.Rows(j)(3))
                    '            'Debug.WriteLine("dateEssai = " & .Rows(j)(4))E_
                    '            If (.Rows(j)(4)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_dateEssai = CStr(.Rows(j)(4))
                    '            'Debug.WriteLine("DifPresEauAir = " & .Rows(j)(5).ToString)
                    '            If (.Rows(j)(5)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_DifPresEauAir = CStr(.Rows(j)(5))
                    '            ''Debug.WriteLine("HauteurCpv = " & .Rows(j)(6).ToString)
                    '            If (.Rows(j)(6)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_HauteurCpv = CStr(.Rows(j)(6))
                    '            'Debug.WriteLine("Di = " & .Rows(j)(7).ToString)
                    '            If (.Rows(j)(7)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_Di = CStr(.Rows(j)(7))
                    '            'Debug.WriteLine("Vc = " & .Rows(j)(8).ToString)
                    '            If (.Rows(j)(8)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_Vc = CStr(.Rows(j)(8))
                    '            'Debug.WriteLine("Vs = " & .Rows(j)(9).ToString)
                    '            If (.Rows(j)(9)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_Vs = CStr(.Rows(j)(9))
                    '            'Debug.WriteLine("EpMembrane = " & .Rows(j)(10).ToString)
                    '            If (.Rows(j)(10)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_EpMembrane = CStr(.Rows(j)(10))
                    '            'Debug.WriteLine("Prp = " & .Rows(j)(11).ToString)
                    '            If (.Rows(j)(11)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_Prp = CStr(.Rows(j)(11))
                    '            'Debug.WriteLine("PrV = " & .Rows(j)(12).ToString)
                    '            If (.Rows(j)(12)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_PrV = CStr(.Rows(j)(12))
                    '            'Debug.WriteLine("coefficientA = " & .Rows(j)(13).ToString)
                    '            If (.Rows(j)(13)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_coefficientA = CStr(.Rows(j)(13))
                    '            'Debug.WriteLine("coefficientB = " & .Rows(j)(14).ToString)
                    '            If (.Rows(j)(14)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_coefficientB = CStr(.Rows(j)(14))
                    '            'Debug.WriteLine("PrecisVc = " & .Rows(j)(15).ToString)
                    '            If (.Rows(j)(15)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_PrecisVc = CStr(.Rows(j)(15))
                    '            'Debug.WriteLine("PrecisVs = " & .Rows(j)(16).ToString)
                    '            If (.Rows(j)(16)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_PrecisVs = CStr(.Rows(j)(16))
                    '            ''Debug.WriteLine("PrEpMembrane = " & .Rows(j)(17).ToString)
                    '            If (.Rows(j)(17)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_PrEpMembrane = CStr(.Rows(j)(17))
                    '            'Debug.WriteLine("PrecisA = " & .Rows(j)(18).ToString)
                    '            If (.Rows(j)(18)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_PrecisA = CStr(.Rows(j)(18))
                    '            'Debug.WriteLine("PrecisB = " & .Rows(j)(19).ToString)
                    '            If (.Rows(j)(19)) IsNot Nothing Then MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_PrecisB = CStr(.Rows(j)(19))
                    '        End With

                    '        iMesuresExpansion = 0
                    '        ' On charge mes Mesures correspondantes à cet essai RefEssai
                    '        For k = 0 To MyListeSondes.MesureExpansionCount - 1

                    '            ' On cherche les Essais de nom RefEssai dans la table "LesMesuresExpansionPropre"
                    '            Chaine2 = MaListeSondes.Tables("LesMesuresExpansionPropre").Rows(k)(2).ToString

                    '            If RefEssai = Chaine2 Then
                    '                ' On a trouvé un essai de la même RefEssai
                    '                iMesuresExpansion = iMesuresExpansion + 1

                    '                '' Vérifier si le tableau Expansion est initialisé
                    '                'If MyListeSondes.P_ListeSondes(i)P_MaExpansion Is Nothing Then
                    '                '    ReDim MyListeSondes.P_ListeSondes(i)P_MaExpansion(0)
                    '                'End If

                    '                ' Vérifier si Expansion(iExpansion) est instancié
                    '                If MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1) Is Nothing Then
                    '                    MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1) = New Essai_Expansion
                    '                End If

                    '                '' Redimensionner le tableau Mesures
                    '                'ReDim Preserve MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_Mesures(1, iMesuresExpansion)

                    '                '    ' Instancier l'objet Expansion(iExpansion)
                    '                '    '   MyListeSondes.p_ListeSondes(i)P_MaExpansion(iExpansion).Mesures(2, iMesuresExpansion) = New Mesure
                    '                With MaListeSondes.Tables("LesMesuresExpansionPropre")
                    '                    ' Colonne 0 désigne le numéro du chargement
                    '                    'MyListeSondes.p_ListeSondes(i)P_MaExpansion(iExpansion - 1).Mesures(0, iMesuresExpansion - 1) = iMesuresExpansion
                    '                    ' Colonne 0 désigne la pression
                    '                    'MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_Mesures(0, iMesuresExpansion - 1) = CDbl(.Rows(k)(3))
                    '                    ' Colonne 1 désigne le volume
                    '                    'MyListeSondes.P_ListeSondes(i)P_MaExpansion(iExpansion - 1).E_Mesures(1, iMesuresExpansion - 1) = CDbl(.Rows(k)(4))

                    '                    'MyListeSondes.p_ListeSondes(i)P_MaExpansion(iExpansion - 1).Mesures(0, iMesuresExpansion - 1) = CStr(.Rows(iMesuresExpansion - 1)(0))
                    '                    'MyListeSondes.p_ListeSondes(i)P_MaExpansion(iExpansion - 1).Mesures(1, iMesuresExpansion - 1) = CStr(.Rows(iMesuresExpansion - 1)(1))
                    '                    'MyListeSondes.p_ListeSondes(i)P_MaExpansion(iExpansion - 1).Mesures(2, iMesuresExpansion - 1) = CStr(.Rows(iMesuresExpansion - 1)(2))                              End With
                    '                End With
                    '            End If

                    '            ' Remplir les propriétés

                    '        Next k

                    '    End If

                    'Next j


                    RefSonde = MyListeSondes.P_ListeSondes(i).P_refSonde
                    'iResistance = 0

                    ' On lit toutes les lignes de la Table "LesEssaisResistance" 
                    ' On cherche la premiere occurance de nom Re Essai
                    For j = 0 To MyListeSondes.P_ListeSondes(i).P_MaResistance.Count - 1

                        ' On cherche les Essais de nom RefProbe dans la table "LesEssaisResistance"
                        Chaine = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j)(1).ToString

                        If RefSonde = Chaine Then

                            ' -------------------- On a trouvé un essai de la meme sonde ---------------------------------------------
                            iResistance = iResistance + 1

                            RefEssai = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(j)(0).ToString

                            '' Vérifier si le tableau Resistance est initialisé
                            'If MyListeSondes.P_ListeSondes(i).p_Resistance Is Nothing Then
                            '    ReDim Preserve MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance)
                            'End If

                            '' Redimensionner et instancier Resistance(iResistance) si nécessaire
                            'If iResistance >= UBound(MyListeSondes.P_ListeSondes(i).p_Resistance) Then
                            '    ReDim Preserve MyListeSondes.P_ListeSondes(i).p_Resistance(iResistance)
                            'End If

                            ' Instancier l'objet Resistance(iResistance)
                            'MyListeSondes.P_ListeSondes(i).p_Resistance(iResistance - 1) = New Essai_Resistance

                            ' Remplir les propriétés
                            'With MaListeSondes.Tables("LesEssaisResistancePropre")
                            '    MyListeSondes.P_ListeSondes(i).P_MaResistance(iResistance - 1).R_refEssai = CStr(.Rows(j)(0))
                            '    MyListeSondes.P_ListeSondes(i).P_MaResistance(iResistance - 1).R_refSonde = CStr(.Rows(j)(1))
                            '    If Not IsDBNull(.Rows(j)(2)) Then
                            '        MyListeSondes.P_ListeSondes(i).P_MaResistance(iResistance - 1).R_ClassPrecis = CStr(.Rows(j)(2))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_ClassPrecis = 0
                            '    End If
                            '    If Not IsDBNull(.Rows(j)(3)) Then
                            '        'MyListeSondes.P_ListeSondes(i).p_Resistance(iResistance - 1).ListeSondes(i).R_ResistancenomEssai = CStr(.Rows(j)(3))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_nomEssai = 0
                            '    End If

                            '    If Not IsDBNull(.Rows(j)(4)) Then
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_dateEssai = CStr(.Rows(j)(4))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_nomEssai = 0
                            '    End If

                            '    If Not IsDBNull(.Rows(j)(5)) Then
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_difPresEauAir = CStr(.Rows(j)(5))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_difPresEauAir = 0
                            '    End If
                            '    If Not IsDBNull(.Rows(j)(6)) Then
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_hauteurCpv = CStr(.Rows(j)(6))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_hauteurCpv = 0
                            '    End If

                            '    If Not IsDBNull(.Rows(j)(7)) Then
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_A0 = CStr(.Rows(j)(7))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_A0 = 0
                            '    End If
                            '    If Not IsDBNull(.Rows(j)(8)) Then
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_A1 = CStr(.Rows(j)(8))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_A1 = 0
                            '    End If
                            '    If Not IsDBNull(.Rows(j)(9)) Then
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_A2 = CStr(.Rows(j)(9))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_A2 = 0
                            '    End If
                            '    If Not IsDBNull(.Rows(j)(10)) Then
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_A3 = CStr(.Rows(j)(10))
                            '    Else
                            '        MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_A3 = 0
                            '    End If
                            'End With

                            'iMesuresResistance = 0
                            '' On charge mes Mesures correspondantes à cet essai RefEssai
                            'For k = 0 To MyListeSondes.MesureResistanceCount - 1

                            '    ' On cherche les Essais de nom RefEssai dans la table "LesMesuresResistancePropre"
                            '    Chaine2 = MaListeSondes.Tables("LesMesuresResistancePropre").Rows(k)(2).ToString

                            '    If RefEssai = Chaine2 Then
                            '        '---------------------  On a trouvé un essai de la même RefEssai  -----------------------
                            '        iMesuresResistance = iMesuresResistance + 1

                            '        '' Vérifier si le tableau Resistance est initialisé
                            '        'If MyListeSondes.P_ListeSondes(i).P_Resistance Is Nothing Then
                            '        '    ReDim MyListeSondes.P_ListeSondes(i).P_Resistance(0)
                            '        'End If

                            '        ' Vérifier si Resistance(iResistance) est instancié
                            '        If MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1) Is Nothing Then
                            '            MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1) = New Essai_Resistance
                            '        End If

                            '        '' Redimensionner le tableau Mesures
                            '        'ReDim Preserve MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).R_Mesures(2, iMesuresResistance)

                            '        '    ' Instancier l'objet Resistance(iResistance)
                            '        '    '   MyListeSondes.P_ListeSondes(i).p_Resistance(iResistance).Mesures(2, iMesuresResistance) = New Mesure
                            '        With MaListeSondes.Tables("LesMesuresResistancePropre")
                            '            ' Colonne 0 désigne le numéro du chargement

                            '            'MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).Mesures(0, iMesuresResistance - 1) = iMesuresResistance
                            '            ' Colonne 1 désigne la pression
                            '            If Not IsDBNull(.Rows(j)(3)) Then
                            '                'MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).Mesures(1, iMesuresResistance - 1) = CDbl(.Rows(k)(3))
                            '            Else
                            '                'MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).Mesures(1, iMesuresResistance - 1) = 0
                            '            End If

                            '            ' Colonne 2 désigne le volume
                            '            If Not IsDBNull(.Rows(j)(4)) Then
                            '                'MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).Mesures(2, iMesuresResistance - 1) = CDbl(.Rows(k)(4))
                            '            Else
                            '                'MyListeSondes.P_ListeSondes(i).P_Resistance(iResistance - 1).Mesures(2, iMesuresResistance - 1) = 0

                            '            End If
                            '            'MyListeSondes.P_ListeSondes(i).p_Resistance(iResistance - 1).Mesures(0, iMesuresResistance - 1) = CStr(.Rows(iMesuresResistance - 1)(0))
                            '            'MyListeSondes.P_ListeSondes(i).p_Resistance(iResistance - 1).Mesures(1, iMesuresResistance - 1) = CStr(.Rows(iMesuresResistance - 1)(1))
                            '            'MyListeSondes.P_ListeSondes(i).p_Resistance(iResistance - 1).Mesures(2, iMesuresResistance - 1) = CStr(.Rows(iMesuresResistance - 1)(2))                              End With
                            '        End With

                            '    End If ' Fin du If RefEssai = Chaine2 Then

                            '    ' Remplir les propriétés

                            'Next k ' Fin du For k = 0 To MyListeSondes.MesureResistanceCount - 1

                        End If ' Fin du  If RefSonde = Chaine Then

                    Next j ' Fin du For j = 0 To MyListeSondes.p_ResistanceCount - 1

                End If ' Fin du If MaListeSondes.Tables("LesSondes").Rows(i) IsNot Nothing Then

            Next i ' Fin du For i = 0 To MaListeSondes.Tables("LesSondes").Rows().Count - 1



            ' Le chargement  s'étant effectuer correctement, la fonction renvoie vrai
            ChargeAvecBD = True
            IsSaved = True

            ' Vérification initiale
            ' Vérification de MaListeSondes.Tables("LesEssaisResistancePropre")
            With MaListeSondes.Tables("LesEssaisResistancePropre")
                For i1 = 0 To .Rows.Count - 1
                    Debug.WriteLine("ClasseListeSondes - LesEssaisResistancePropre Init i = " & i & " RefEssai = " & .Rows(i1).Item("RefEssai").ToString)
                Next i1
            End With

            ' Vérification de MyListeSondes.Tables("LesEssaisForage")
            refSondes.Clear()
            For i1 = 0 To MyListeSondes.P_ListeSondes.Count - 1
                RowSonde = MyListeSondes.P_ListeSondes(i1)
                ReferenceSonde = RowSonde.P_refSonde.ToString()
                If refSondes.ContainsKey(ReferenceSonde) Then
                    MsgBox("Doublon détecté dans MyListeSondes pour refSonde: " & ReferenceSonde)
                    Exit Function
                Else
                    refSondes.Add(ReferenceSonde, True)
                End If
            Next i1

            ' Affectation de la Liste des Sondes à MyChantier
            For i1 = 0 To MyListeSondes.P_ListeSondes.Count - 1
                RowSonde = New Probe
                RowSonde = MyListeSondes.P_ListeSondes(i1)
                'If MyChantier.ListeSondes.Count - 1 <= i1 Then
                '    ReDim Preserve MyChantier.ListeSondes(i1)
                '    MyChantier.ListeSondes(i1) = New Probe
                'End If
                'MyChantier.ListeSondes(i1) = RowSonde
            Next i1
            Exit Function

            ' Traitement des erreurs lors du chargement de la base de données:
errFind:
            ' Fermeture de la connection
            ' La fonction renvoie faux
            MsgBox(Err.Description)
            ChargeAvecBD = False
        End Function

        Public Sub CloseListeSondes()

            ' Effacement de la référence vers le chantier
            MyListeSondes = Nothing

        End Sub

        ' Remplissage du tableau de la liste des sondes 
        'Public Sub AfficheDansTab(ByRef Table As DataGridView)
        '    Dim Taille As Integer

        '    Taille = MyListeSondes.p_ListeSondes.Count
        '    With Table
        '        .Rows.Clear()
        '        ' Remplissage du tableau rsEssaiTab de la liste des essais par sonde
        '        For i = 0 To Taille - 1
        '            .Rows.Add()
        '            '.Rows(i).Cells(0).Value = i
        '            .Rows(i).Cells(0).Value = MyListeSondes.p_ListeSondes(i).P_refSonde

        '            'SoupleTypeE = 0
        '            'SoupleTypeG = 1
        '            'TubeFenduCourte = 2
        '            'TubeFenduLongue = 3
        '            'Autre = 4
        '            Select Case MyListeSondes.p_ListeSondes(i).P_typeSonde
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
        '        For i = 0 To MyListeSondes.p_ListeSondes.Count - 1
        '            .Add(MyListeSondes.p_ListeSondes(i).P_refSonde)
        '        Next i
        '    End With
        'End Sub


        Public Sub NouvelleSonde(ByVal NewProbe As Boolean, ByVal NewTable As Boolean)
            ' NewProbe est Vrai quand on Crée une Nouvelle Sonde; sinon Faux
            ' NewTable est Vrai quand on Crée une Nouvelle Table MaListeSondes.Tables("LesSondes"); sinon Faux

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
            'If MyListeSondes.P_ListeSondes Is Nothing Then
            '    ReDim Preserve MyListeSondes.P_ListeSondes(0)
            '    MyListeSondes.P_ListeSondes(0) = New Probe
            'Else
            '    If SondeCount >= MyListeSondes.P_ListeSondes.Count Then
            '        Dim iMax As Integer
            '        iMax = MyListeSondes.P_ListeSondes.Count
            '        ReDim Preserve MyListeSondes.P_ListeSondes(iMax)
            '        MyListeSondes.P_ListeSondes(iMax) = New Probe
            '        'ListeSondes.Add(NouvelleSonde)
            '    Else
            '        MyListeSondes.P_ListeSondes(SondeCount - 1) = NouvelleSonde
            '    End If
            'End If

            ' Enregistrement de la nouvelle sonde dans le tableau de sondes
            MyListeSondes.P_ListeSondes(MyListeSondes.P_ListeSondes.Count - 1) = NouvelleSonde

            ' Enregistrement de la liste auprès de sa nouvelle sonde
            Call NouvelleSonde.SetMyListe(Me)

            ' Attribution d'une référence à la nouvelle sonde
            NouvelleSonde.SuspendreEvents()
            NouvelleSonde.SetRefSonde(GiveMeANewRefSonde)
            NouvelleSonde.RetablirEvents()
            If MySonde IsNot Nothing Then MySonde = New Probe
            MySonde = NouvelleSonde

            'If NewTable Then ' On ajoute une mesure à MonFichier.Tables("LesSondes") seulement si Newprobe est vrai
            '    If MaListeSondes.Tables("LesSondes") Is Nothing Then

            '        ' Création de la Table("LesSondes") dans MaListeSondes
            '        MonChantier.Tables.Add("LesSondes")
            '        With MaListeSondes.Tables("LesSondes")
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
            '        '    ObjetDataRow = MaListeSondes.Tables("LesSondes").NewRow()

            '        '    ObjetDataRow("refSonde") = MyListeSondes.GiveMeANewRefSonde
            '        '    ObjetDataRow("typeSonde") = MySonde.P_typeSonde
            '        '    ObjetDataRow("typeDeformee") = MySonde.P_typeDeformee
            '        '    ObjetDataRow("typeContrainte") = MySonde.P_typeContrainte
            '        '    ObjetDataRow("ls") = MySonde.P_ls
            '        '    ObjetDataRow("lg") = MySonde.P_lg
            '        '    ObjetDataRow("lt") = MySonde.P_lt
            '        '    ObjetDataRow("longInjectPompage") = MySonde.P_longInjectePompage
            '        '    ObjetDataRow("Vs") = MySonde.P_Vs
            '        '    ObjetDataRow("ds") = MySonde.P_ds
            '        '    ObjetDataRow("ClassPrecis") = MySonde.P_ClassPrecis
            '        '    ObjetDataRow("EpMembraneGarde") = MySonde.P_EpMembraneGarde
            '        '    ObjetDataRow("PrEpMembraneGarde") = MySonde.P_PrEpMembraneGarde
            '        '    ObjetDataRow("EpMembraneMesure") = MySonde.P_EpMembraneMesure
            '        '    ObjetDataRow("PrEpMembraneMesure") = MySonde.P_PrEpMembraneMesure

            '        '    ObjetDataRow("EpTubeFendu") = MySonde.P_epTubeFendu
            '        '    ObjetDataRow("PrEpTubeFendu") = MySonde.P_PrEpTubeFendu

            '        '    ObjetDataRow("EpInitMembrane") = MySonde.P_EpInitMembrane
            '        '    ObjetDataRow("PrEpInitMembrane") = MySonde.P_PrEpInitMembrane

            '        '    ObjetDataRow("PrLong") = MySonde.P_PrLong
            '        '    ObjetDataRow("PrDiam") = MySonde.P_PrDiam
            '        '    ObjetDataRow("PrP") = MySonde.P_PrP
            '        '    ObjetDataRow("PrV") = MySonde.P_PrV
            '        '    ObjetDataRow("PrPV") = MySonde.P_PrPV
            '        '    ObjetDataRow("PrZ") = MySonde.P_PrZ
            '        '    ObjetDataRow("Prt") = MySonde.P_Prt
            '        '    ' ObjetDataRow("EpMembraneMesure") = Mysonde.P_EpMembraneMesure
            '        '    ObjetDataRow("Pm") = MySonde.P_Pm

            '        '    MaListeSondes.Tables("LesSondes").Rows.Add(ObjetDataRow)

            '        '    'MaListeSondes.Tables("LesSondes").Rows.Add()
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

        Public Function GetEssaiExpansionParRef(ByVal RefSonde As String, ByVal refExpansion As String) As Essai_Expansion
            ' Recherche de la sonde ayant pour référence 'ref' avec arrêt dès que possible
            Dim i, j As Integer
            Dim Trouve As Boolean

            Trouve = False
            For i = 0 To MyListeSondes.P_ListeSondes.Count - 1

                ' On cherche dans la liste des sondes celle qui correspond
                If MyListeSondes.P_ListeSondes(i).P_refSonde = RefSonde Then

                    ' On a trouvé la sonde avec la bonne référence
                    With MyListeSondes.P_ListeSondes(i)

                        If .P_MaExpansion IsNot Nothing Then
                            For j = 0 To .P_MaExpansion.Count - 1

                                ' On cherche dans la liste des essais d'expansion celui qui corrrespond
                                If .P_MaExpansion(j) IsNot Nothing Then
                                    If .P_MaExpansion(j).E_RefEssai.ToString = refExpansion Then
                                        GetEssaiExpansionParRef = .P_MaExpansion(j)
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

        Public Function GetEssaiResistanceParRef(ByVal RefSonde As String, ByVal refResistance As String) As Essai_Resistance
            ' Recherche de la sonde ayant pour référence 'ref' avec arrêt dès que possible
            Dim i, j As Integer
            Dim Trouve As Boolean

            Trouve = False
            For i = 0 To MyListeSondes.P_ListeSondes.Count - 1

                ' On cherche dans la liste des sondes celle qui correspond
                If MyListeSondes.P_ListeSondes(i).P_refSonde = RefSonde Then

                    ' on a trouvé la sonde avec la bonne référence
                    With MyListeSondes.P_ListeSondes(i)

                        If .P_MaResistance IsNot Nothing Then

                            ' On cherche l'essai avec labonne référence de résistance
                            For j = 0 To .P_MaResistance.Count - 1

                                If .P_MaResistance(j) IsNot Nothing Then
                                    ' On cherche dans la liste des essais d'Resistance celui qui corrrespond
                                    If .P_MaResistance(j).R_refEssai = refEssaiResistancePropre Then
                                        .P_Resistance(j).R_refEssai = refResistance
                                        GetEssaiResistanceParRef = .P_Resistance(j)
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

            Do While (i < MyListeSondes.P_ListeSondes.Count - 1)
                If (MyListeSondes.P_ListeSondes(i).P_refSonde = ref) Then Exit Do
                i = i + 1
            Loop

            ' Si la sonde n'a pas été trouvée, la valeur -1 est renvoyée
            If i = MyListeSondes.P_ListeSondes.Count Then i = -1
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
                unique = (MyListeSondes.P_ListeSondes(i).GetRefSonde <> ref)
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

        Public Function EnregistreSondes(ByVal RefSonde As String) As Boolean
            Dim i, NbAcces, EssaiCount As Integer
            Dim Requete, ReferenceSonde As String
            ''Dim row As DataRow
            Dim RowSonde As Probe
            Dim refSondes As New Dictionary(Of String, Boolean)

            'Dim columns() As DataColumn

            'On Error GoTo errFind
            'ReDim columns(5)

            ' ------------------------------------------------------------------------
            ' *********************************************************************
            ' Enregistrement de la Table "LesSondes"
            ' *********************************************************************

            'ma requete pour lire les enregistrements du fichier Sondes.mdb

            ' Définition de la requête pour lire les enregistrements du fichier Sondes.mdb
            Requete = "SELECT refSonde, typeSonde, typeDeformee, typeContrainte, ls, lg, lt, longInjectPompage, Vs, ds, ClassPrecis, EpMembraneGarde, PrEpMembraneGarde, " &
                 "EpMembraneMesure, PrEpMembraneMesure, EpTubeFendu, PrEpTubeFendu, epaisInitMembrane, PrEpaisInitMembrane, PrLong, PrDiam, PrP, PrV, PrPV, PrZ, PrT, Pm FROM LesSondes"

            ' Génération automatique des commandes d'insertion/mise à jour/suppression

            ' Rafraîchir les données depuis la base de données
            MaListeSondes.Tables("LesSondes").Clear()

            ' Vérification des doublons dans MaListeSondes.Tables("LesSondes") AVANT toute modification
            ''For i = 0 To MaListeSondes.Tables("LesSondes").Rows.Count - 1
            ''    row = MaListeSondes.Tables("LesSondes").Rows(i)
            ''    ReferenceSonde = row("refSonde").ToString()
            ''    If refSondes.ContainsKey(ReferenceSonde) Then
            ''        MsgBox("Doublon détecté dans MaListeSondes pour refSonde: " & ReferenceSonde)
            ''        Return False
            ''    Else
            ''        refSondes.Add(ReferenceSonde, True)
            ''    End If
            ''Next i

            ' Vérification des doublons dans MyListeSondes.p_ListeSondes AVANT toute modification
            refSondes.Clear()
            For i = 0 To MyListeSondes.P_ListeSondes.Count - 1
                RowSonde = MyListeSondes.P_ListeSondes(i)
                ReferenceSonde = RowSonde.P_refSonde.ToString()
                If refSondes.ContainsKey(ReferenceSonde) Then
                    MsgBox("Doublon détecté dans MyListeSondes pour refSonde: " & ReferenceSonde)
                    Return False
                Else
                    refSondes.Add(ReferenceSonde, True)
                End If
            Next i

            ' Réinitialisation du dictionnaire pour vérifier les doublons après modification
            refSondes.Clear()

            ' Modification des lignes de MaListeSondes depuisMyListeSondes
            'For i = 0 To MaListeSondes.Tables("LesSondes").Rows.Count - 1
            '    row = MaListeSondes.Tables("LesSondes").Rows(i)
            '    Found = False
            '    ReferenceSonde = row("refSonde").ToString()

            '    For j = 0 To MyListeSondes.P_ListeSondes.Count - 1
            '        Sonde = MyListeSondes.P_ListeSondes(j)
            '        If row("refSonde").ToString = Sonde.P_refSonde.ToString Then
            '            Found = True
            '            Debug.WriteLine("Modif Line refSonde = " & Sonde.P_refSonde.ToString)
            '            row.BeginEdit()
            '            ' Copie des champs
            '            row("typeSonde") = Sonde.P_typeSonde
            '            row("typeDeformee") = Sonde.P_typeDeformee
            '            row("typeContrainte") = Sonde.P_typeContrainte
            '            row("ls") = Sonde.P_ls
            '            row("lg") = Sonde.P_lg
            '            row("lt") = Sonde.P_lt
            '            row("longInjectPompage") = Sonde.P_longInjectePompage
            '            row("Vs") = Sonde.P_Vs
            '            row("ds") = Sonde.P_ds
            '            row("ClassPrecis") = Sonde.P_ClassPrecis
            '            row("epMembraneGarde") = Sonde.P_EpMembraneGarde
            '            row("PrEpMembraneGarde") = Sonde.P_PrEpMembraneGarde
            '            row("epMembraneMesure") = Sonde.P_EpMembraneMesure
            '            row("PrEpMembraneMesure") = Sonde.P_PrEpMembraneMesure
            '            row("epTubeFendu") = Sonde.P_epTubeFendu
            '            row("PrEpTubeFendu") = Sonde.P_PrEpTubeFendu
            '            row("epaisInitMembrane") = Sonde.P_EpInitMembrane
            '            row("PrEpaisInitMembrane") = Sonde.P_PrEpInitMembrane
            '            row("PrLong") = Sonde.P_PrLong
            '            row("PrDiam") = Sonde.P_PrDiam
            '            row("PrP") = Sonde.P_PrP
            '            row("PrV") = Sonde.P_PrV
            '            row("PrPV") = Sonde.P_PrPV
            '            row("PrZ") = Sonde.P_PrZ
            '            row("Prt") = Sonde.P_Prt
            '            row("Pm") = Sonde.P_Pm
            '            row.EndEdit()
            '            Exit For
            '        End If
            '    Next j

            '    ' Si aucune correspondance n'est trouvée, mettez à jour les valeurs par défaut
            '    If Not Found Then
            '        row.BeginEdit()
            '        If row.IsNull("ClassPrecis") OrElse Convert.IsDBNull(row("ClassPrecis")) Then
            '            row("ClassPrecis") = 0
            '        End If
            '        row.EndEdit()
            '    End If
            'Next i

            ' Vérification des doublons après modification
            refSondes.Clear()
            'For i = 0 To MaListeSondes.Tables("LesSondes").Rows.Count - 1
            '    row = MaListeSondes.Tables("LesSondes").Rows(i)
            '    ReferenceSonde = row("refSonde").ToString()
            '    With MaListeSondes.Tables("LesSondes")
            '        Debug.WriteLine("Final - LesSondes Init i = " & i & " refSonde = " & .Rows(i).Item("refSonde").ToString)
            '    End With
            '    If refSondes.ContainsKey(ReferenceSonde) Then
            '        MsgBox("Doublon détecté après modification pour refSonde: " & ReferenceSonde)
            '        Return False
            '    Else
            '        refSondes.Add(ReferenceSonde, True)
            '    End If
            'Next i

            ' Mise à jour de la BD
            'Try
            'AdaptateurSondes.SelectCommand.Transaction = Transaction
            ' Mettre à jour la base de données
            'Dim rowsAffected As Integer = AdaptateurSondes.Update(MaListeSondes, "LesSondes")
            'If rowsAffected < MaListeSondes.Tables("LesSondes").Rows.Count Then
            MsgBox("Certaines lignes n'ont pas été mises à jour en raison de conflits de concurrency.")
            'End If
            ' Valider la transaction

            '    MsgBox("Enregistrement de la Table 'LesSondes' réussi")
            '    EnregistreSondes = True
            'Catch ex As DBConcurrencyException
            '    ' En cas de conflit de concurrency, annuler la transaction
            '    Transaction.Rollback()
            '    MsgBox("Conflit de concurrency lors de l'enregistrement des sondes: " & ex.Message)
            '    EnregistreSondes = False
            'Catch ex As Exception
            '    ' En cas d'erreur, annuler la transaction
            '    Tra            'Transaction.Commit()nsaction.Rollback()
            '    MsgBox("Erreur lors de l'enregistrement des sondes: " & ex.Message)
            '    EnregistreSondes = False
            'Finally
            '    ' S'assurer que la transaction est fermée
            '    If Transaction IsNot Nothing Then
            '        Transaction.Dispose()
            '    End If
            'End Try

            ' ----------------------------------------------------------------------------
            ' Vérification Initiale de MaListeSondes.Tables("LesEssaisExpansionPropre")
            ' ----------------------------------------------------------------------------
            With MaListeSondes.Tables("LesSondes")
                For i = 0 To .Rows.Count - 1
                    Debug.WriteLine("Final - LesSondes Fin i = " & i & " refSonde = " & .Rows(i).Item("refSonde").ToString)
                Next i
            End With

            MaListeSondes.Tables("LesSondes").AcceptChanges()
            'End Function


            ' *********************************************************************
            ' Enregistrement de la Table "LesEssaisExpansionPropre"
            ' *********************************************************************
            ' Mise à jour de la BD
            iTable = 6
            ObjetDataEssaisExpansionPropre = MaListeSondes.Tables("LesEssaisExpansionPropre")

            EssaiCount = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count

            ' ----------------------------------------------------------------------------
            ' Vérification Initiale de MaListeSondes.Tables("LesEssaisExpansionPropre")
            ' ----------------------------------------------------------------------------
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    Debug.WriteLine("Final - LesEssaisExpansionPropre Init i = " & i & " refEssai = " & .Rows(i).Item("refEssai").ToString)
                    Debug.WriteLine("Final - LesEssaisExpansionPropre Init i = " & i & " nomEssai = " & .Rows(i).Item("nomEssai").ToString)
                Next i
            End With

            'ma requete
            If Not (AncienneSonde) Then
                Requete = "SELECT refEssai, refSonde, ClassPrecis, nomEssai, dateEssai, difPresEauAir, hauteurCpv, Di, Vc, Vs, " &
                    "EpMembrane, PrP, PrV, coefficientA, coefficientB, PrecisVc, PrecisVs, PrEpMembrane, PrecisA, PrecisB" &
                    " FROM LesEssaisExpansionPropre"
            Else
                Requete = "SELECT  refEssai, refSonde, nomEssai, dateEssai, difPresEauAir, hauteurCpv, Di, Vc, coefficientA" &
               " FROM LesEssaisExpansionPropre"
            End If

            '' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            ' Sélection de la bonne commande
            'AdaptateurSondes.DeleteCommand = Commande

            'Pour modifier les valeurs changées dans le DataAdapter

            ' Effacement des commandes précédentes
            'AdaptateurSondes.DeleteCommand = CmdSondeBuild.GetDeleteCommand()

            'AdaptateurSondes.AcceptChangesDuringUpdate = True

            'AdaptateurSondes.Update(MaListeSondes, "LesEssaisExpansionPropre")
            'MsgBox("Enregistrement de la Table 'LesEssaisExpansionPropre' réussi")

            '' On vide la Table et on la remplit à nouveau
            'MaListeSondes.Tables("LesEssaisExpansionPropre").Clear()
            'AdaptateurSondes.Fill(MaListeSondes, "LesEssaisExpansionPropre")

            ' ----------------------------------------------------------------------------
            ' Vérification Finale de MaListeSondes.Tables("LesEssaisExpansionPropre")
            ' ----------------------------------------------------------------------------
            With MaListeSondes.Tables("LesEssaisExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    Debug.WriteLine("Final - LesEssaisExpansionPropre Fin i = " & i & " refEssai = " & .Rows(i).Item("refEssai").ToString)
                Next i
            End With

            ' *********************************************************************
            ' Enregistrement de la Table "LesMesuresExpansionPropre"
            ' *********************************************************************
            'ma requete
            Requete = "SELECT Ind, indiceMesure, refEssai, pression, volume FROM LesMesuresExpansionPropre"



            ' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            ' Sélection de la bonne commande

            'Pour modifier les valeurs changées dans le DataAdapter

            ' Effacement des commandes précédentes
            'AdaptateurSondes.DeleteCommand = CmdSondeBuild.GetDeleteCommand()


            '' Définition de la clef primaire de la Table "LesMesuresForage"
            'columns(0) = New DataColumn
            'columns(0) = MaListeSondes.Tables("LesMesuresExpansionPropre").Columns("Ind")
            'MaListeSondes.Tables("LesMesuresExpansionPropre").PrimaryKey = columns

            ' Mise à jour de la BD
            iTable = 7
            ObjetDataMesuresExpansionPropre = MaListeSondes.Tables("LesMesuresExpansionPropre")

            '' Redéfintition de la clef primaire
            'columns(0) = New DataColumn
            'columns(0) = MaListeSondes.Tables("LesMesuresExpansionPropre").Columns("Ind")
            'MaListeSondes.Tables("LesMesuresExpansionPropre").PrimaryKey = columns

            'AdaptateurSondes.Update(MaListeSondes, "LesMesuresExpansionPropre")
            'MsgBox("Enregistrement de la Table 'LesMesuresExpansionPropre' réussi")

            '' On vide la Table et on la remplit à nouveau
            'MaListeSondes.Tables("LesMesuresExpansionPropre").Clear()
            'AdaptateurSondes.Fill(MaListeSondes, "LesMesuresExpansionPropre")

            ' ----------------------------------------------------------------------------
            ' Vérification Finale de MaListeSondes.Tables("LesMesuresExpansionPropre")
            ' ----------------------------------------------------------------------------
            With MaListeSondes.Tables("LesMesuresExpansionPropre")
                For i = 0 To .Rows.Count - 1
                    Debug.WriteLine("Final - LesMesuresExpansionPropre Fin i = " & i & " refEssai = " & .Rows(i).Item("refEssai").ToString)
                Next i
            End With

            Requete = "SELECT  refEssai, refSonde, ClassPrecis, nomEssai, dateEssai, difPresEauAir, hauteurCpv, a0, a1, a2, a3" &
            " FROM LesEssaisResistancePropre"

            ' ----------------------------------------------------------------------------
            ' Vérification Finale de MaListeSondes.Tables("LesEssaisResistancePropre")
            ' ----------------------------------------------------------------------------
            With MaListeSondes.Tables("LesEssaisResistancePropre")
                For i = 0 To .Rows.Count - 1
                    Debug.WriteLine("INITIAL - LesEssaisResistancePropre Init i = " & i & " refEssai = " & .Rows(i).Item("refEssai").ToString)
                Next i


            End With

            ' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            ' Sélection de la bonne commande
            'AdaptateurSondes.DeleteCommand = Commande

            ' Adaptation du fichier Accès à la dimension de la Table
            NbAcces = MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Count

            ' Création de l'objetCommand nécessaire à l'enregistrement

            ' Effacement des commandes précédentes
            'AdaptateurSondes.DeleteCommand = CmdSondeBuild.GetDeleteCommand()


            ' Mise à jour de la BD
            iTable = 8
            ObjetDataEssaisResistancePropre = MaListeSondes.Tables("LesEssaisResistancePropre")

            ' ----------------------------------------------------------------------------
            ' Vérification Médiane de MaListeSondes.Tables("LesEssaisResistancePropre")
            ' ----------------------------------------------------------------------------
            With MaListeSondes.Tables("LesEssaisResistancePropre")
                For i = 0 To .Rows.Count - 1
                    Debug.WriteLine("Median - LesEssaisResistancePropre Init i = " & i & " refEssai = " & .Rows(i).Item("refEssai").ToString)
                Next i
            End With

            '' Redéfintition de la clef primaire
            'columns(0) = New DataColumn
            'columns(0) = MaListeSondes.Tables("LesEssaisResistancePropre").Columns("refEssai")
            'MaListeSondes.Tables("LesEssaisResistancePropre").PrimaryKey = columns

            ' On vide la Table et on la remplit à nouveau
            'MaListeSondes.Tables("LesEssaisResistancePropre").Clear()
            'AdaptateurSondes.Fill(MaListeSondes, "LesEssaisResistancePropre")


            'AdaptateurSondes.Update(MaListeSondes, "LesEssaisResistancePropre")
            'MsgBox("Enregistrement de la Table -LesEssaisResistancePropre- réussi")

            '' On vide la Table et on la remplit à nouveau
            'MaListeSondes.Tables("LesEssaisResistancePropre").Clear()
            'AdaptateurSondes.Fill(MaListeSondes, "LesEssaisResistancePropre")

            ' ----------------------------------------------------------------------------
            ' Vérification Finale de MaListeSondes.Tables("LesEssaisResistancePropre")
            ' ----------------------------------------------------------------------------
            With MaListeSondes.Tables("LesEssaisResistancePropre")
                For i = 0 To .Rows.Count - 1
                    Debug.WriteLine("Final - LesEssaisResistancePropre Init i = " & i & " refEssai = " & .Rows(i).Item("refEssai").ToString)
                Next i
            End With

            ' *********************************************************************
            ' Enregistrement de la Table "LesMesuresResistancePropre"
            ' ********************************************************************
            'ma requete
            Requete = "SELECT Ind, indiceMesure, refEssai, pression, volume FROM LesMesuresResistancePropre "

            ' Définition de la Commande

            ' Définition de l'AdaptateurSondes

            ' Sélection de la bonne commande
            'AdaptateurSondes.DeleteCommand = Commande

            'Pour modifier les valeurs changées dans le DataAdapter

            ' Effacement des commandes précédentes
            'AdaptateurSondes.DeleteCommand = CmdSondeBuild.GetDeleteCommand()

            'AdaptateurSondes.AcceptChangesDuringUpdate = True

            ' Mise à jour de la BD
            iTable = 9
            ObjetDataMesuresResistancePropre = MaListeSondes.Tables("LesMesuresResistancePropre")

            '' Redéfintition de la clef primaire
            'columns(0) = New DataColumn
            'columns(0) = MaListeSondes.Tables("LesMesuresResistancePropre").Columns("Ind")
            'MaListeSondes.Tables("LesMesuresResistancePropre").PrimaryKey = columns

            ' On vide la Table et on la remplit à nouveau
            'MaListeSondes.Tables("LesMesuresResistancePropre").Clear()
            'AdaptateurSondes.Fill(MaListeSondes, "LesMesuresResistancePropre")

            'AdaptateurSondes.Update(MaListeSondes, "LesMesuresResistancePropre")
            'MsgBox("Enregistrement de la Table -LesMesuresResistancePropre- réussi")

            '' On vide la Table et on la remplit à nouveau
            'MaListeSondes.Tables("LesMesuresResistancePropre").Clear()
            'AdaptateurSondes.Fill(MaListeSondes, "LesMesuresResistancePropre")

            ' ----------------------------------------------------------------------------
            ' Vérification Finale de MaListeSondes.Tables("LesMesuresResistancePropre")
            ' ----------------------------------------------------------------------------
            With MaListeSondes.Tables("LesMesuresResistancePropre")
                For i = 0 To .Rows.Count - 1
                    Debug.WriteLine("Final - LesMesuresResistancePropre Init i = " & i & " refEssai = " & .Rows(i).Item("refEssai").ToString)
                Next i
            End With

            EnregistreSondes = True
            IsSaved = True
            Exit Function
errFind:
            MsgBox(Err.Description)
            EnregistreSondes = False
            'FrmTable.Show()
            Exit Function
        End Function

        Public Function GetSondeCount()
            GetSondeCount = MyListeSondes.P_ListeSondes.Count
        End Function

        Public Function GetFichierName()
            GetFichierName = FichierListeSondes
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
                    NbEssai = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows.Count
                Case 1 ' Resistance
                    NbEssai = MaListeSondes.Tables("LesEssaisResistancePropre").Rows.Count
            End Select

            ' Lecture des références de chaque essai dans la liste des Esais
            'For i = 0 To NbEssai - 1
            '    Select Case TypeEssai
            '        Case 0 ' Expansion
            '            If MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(i).RowState <> DataRowState.Deleted Then
            '                RefEssaiCourant = MaListeSondes.Tables("LesEssaisExpansionPropre").Rows(i).Item("refEssai").ToString
            '                unique = (RefEssaiCourant <> ref)
            '                If Not (unique) Then Exit For
            '            End If
            '        Case 1 ' Resistance
            '            If MaListeSondes.Tables("LesEssaisResistancePropre").Rows(i).RowState <> DataRowState.Deleted Then
            '                RefEssaiCourant = MaListeSondes.Tables("LesEssaisResistancePropre").Rows(i).Item("refEssai").ToString
            '                unique = (RefEssaiCourant <> ref)
            '                If Not (unique) Then Exit For
            '            End If
            '    End Select
            'Next i

            IsRefEssaiUnique = unique
        End Function


        'Public Sub CloseListeSonde()
        '    Call MaskListeSondes
        '    Dim i As Integer
        '    For i = 0 To SondeCount - 1
        '        ListeSonde(i).CloseListeSonde
        '        Set ListeSonde(i) = Nothing
        '    Next
        'End Sub

        Public Sub Show()
            MyListeSondes.Show()
        End Sub

        Public Sub MaskListeSondes()
            'FrmListeSonde.Close()
            Return
        End Sub

        Public Function GetSondeParIndex(ByVal Index As Integer) As Probe
            GetSondeParIndex = MyListeSondes.P_ListeSondes(Index)
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
            GetSondeParRef = P_ListeSondes(i)
        End Function

    End Class
End Namespace
