Imports System.ComponentModel
Imports GaiaPress.core.V17.Services
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPress.core.V17.Models


    Public Class LesChantiers
        Inherits DbContext
        ' ...

        ' Propriétés métiers (avec préfixe C_)
        Public Property C_refChantier As String
        Public Property C_nomChantier As String
        Public Property C_descriptifChantier As String
        Public Property C_refClient As Integer?
        Public Property C_entreprise As String
        Public Property C_adresse As String
        Public Property C_telephone As String
        Public Property C_reference As String

        Public C_FichierChantier As String
        Public C_MyFenetreChantier As ChantierLogic
        Public C_MyFile As String
        Public C_MyFileTitle As String
        Public C_DataSetName As String

        ' Collections (remplacez les tableaux par des List)
        Public Property C_ListForages As New List(Of Forage)
        Public Property C_ListEssaisForages As New List(Of Essai_Forage)
        Public Property C_ListMesuresForage As New List(Of MesuresEssaiForage)
        Public Property C_ListSols As New List(Of Sol)
        Public Property C_ListSondes As New List(Of Probe)

        ' Propriétés de gestion (optionnel, avec préfixe C_)
        Public Property C_IsSaved As Boolean = False
        Public Property C_FilePath As String
        Public Property C_FileTitle As String

        Public Property C_EventsSupendus As Boolean


        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications du Chantier ont eu lieu
            If Not C_EventsSupendus Then
                RaiseEvent HasBeenModified()
                C_IsSaved = False
            End If
        End Sub

        ' ===============================================================================
        ' Gestion de  C_ListForages
        ' ===============================================================================

        Public Sub AddForage(forage As Forage)
            Me.C_ListForages.Add(forage)
            ' Lier le forage au chantier (si nécessaire)
            forage.F_RefChantier = Me.C_refChantier
        End Sub

        ' Delete un forage par référence
        Public Sub DeleteForage(refForage As String)
            Dim forageADelete As Forage = GetForage(refForage)
            If forageADelete IsNot Nothing Then
                Me.C_ListForages.Remove(forageADelete)
            End If
        End Sub


        Public Function GetForage(refForage As String) As Forage
            For Each f As Forage In Me.C_ListForages
                If f.F_RefForage = refForage Then
                    Return f
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Met à jour un forage dans la liste C_ListForage, y compris ses objets complexes.
        ''' </summary>
        Public Sub MettreAJourForage(ForageModifie As Forage)
            Dim forage As Forage = GetForage(ForageModifie.F_RefForage)

            If forage IsNot Nothing Then
                ' Mise à jour des propriétés simples
                forage.F_RefChantier = ForageModifie.F_RefChantier
                forage.F_RefForage = ForageModifie.F_RefForage
                forage.F_numeroForage = ForageModifie.F_numeroForage
                forage.F_NomForage = ForageModifie.F_NomForage
                forage.F_dateForage = ForageModifie.F_dateForage
                forage.F_materiel = ForageModifie.F_materiel
                forage.F_outil = ForageModifie.F_outil
                forage.F_abscisseX = ForageModifie.F_abscisseX
                forage.F_ordonneeY = ForageModifie.F_ordonneeY
                forage.F_altitudeZ = ForageModifie.F_altitudeZ
                forage.F_coteInit = ForageModifie.F_coteInit
                forage.F_coteFinale = ForageModifie.F_coteFinale
                forage.F_EventsSuspendus = ForageModifie.F_EventsSuspendus
                forage.F_IsSaved = ForageModifie.F_IsSaved
                forage.F_FichierChantier = ForageModifie.F_FichierChantier

                ' Mise à jour des objets complexes (ex: F_ListEssais)
                ' Copie profonde pour éviter les références partagées
                forage.F_ListEssais = New List(Of Essai_Forage)(ForageModifie.F_ListEssais)
            End If
        End Sub


        ' ===============================================================================
        ' Gestion de  C_ListEssaiForages
        ' ===============================================================================

        Public Sub AddEssaiForage(essai As Essai_Forage)
            Me.C_ListEssaisForages.Add(essai)
            ' Lier le forage au chantier (si nécessaire)
            essai.E_RefChantier = Me.C_refChantier
        End Sub

        ' Delete un forage par référence
        Public Sub DeleteessaiForage(refessai As String)
            Dim EssaiADelete As Essai_Forage = GetessaiForage(refessai)
            If EssaiADelete IsNot Nothing Then
                Me.C_ListEssaisForages.Remove(EssaiADelete)
            End If
        End Sub

        Public Function GetessaiForage(refEssai As String) As Essai_Forage
            For Each E As Essai_Forage In Me.C_ListEssaisForages
                If E.E_RefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function


        ''' Met à jour un essai de forage avec toutes ses propriétés, y compris les objets complexes.
        Public Sub MettreAJourEssaiForage(EssaiModifie As Essai_Forage)

            Dim Essai As Essai_Forage = GetessaiForage(EssaiModifie.E_RefEssai)

            If EssaiModifie IsNot Nothing Then
                ' Mise à jour des objets complexes
                Essai.E_MySonde = EssaiModifie.E_MySonde
                Essai.E_MySol = EssaiModifie.E_MySol
                Essai.E_MyForage = EssaiModifie.E_MyForage
                Essai.E_MyResistance = EssaiModifie.E_MyResistance
                Essai.E_MyExpansion = EssaiModifie.E_MyExpansion

                ' Mise à jour des propriétés simples
                Essai.E_ClassPrecis = EssaiModifie.E_ClassPrecis
                Essai.E_coefPoisson = EssaiModifie.E_coefPoisson
                Essai.E_coteZeroGeomeca = EssaiModifie.E_coteZeroGeomeca
                Essai.E_Cohesion = EssaiModifie.E_Cohesion
                Essai.E_dateEssai = EssaiModifie.E_dateEssai
                Essai.E_heureEssai = EssaiModifie.E_heureEssai
                Essai.E_difPresEauAir = EssaiModifie.E_difPresEauAir
                Essai.E_Enregistreur = EssaiModifie.E_Enregistreur
                Essai.E_Frottement = EssaiModifie.E_Frottement
                Essai.E_FrottementIntergranulaire = EssaiModifie.E_FrottementIntergranulaire
                Essai.E_hauteurCpv = EssaiModifie.E_hauteurCpv
                Essai.E_modElastique = EssaiModifie.E_modElastique
                Essai.E_modElastiqueNf = EssaiModifie.E_modElastiqueNf
                Essai.E_modPressio = EssaiModifie.E_modPressio
                Essai.E_modPressioNf = EssaiModifie.E_modPressioNf
                Essai.E_nomEssai = EssaiModifie.E_nomEssai
                Essai.E_Operateur = EssaiModifie.E_Operateur
                Essai.E_Pe = EssaiModifie.E_Pe
                Essai.E_Ie = EssaiModifie.E_Ie
                Essai.E_presFluage = EssaiModifie.E_presFluage
                Essai.E_presLimite = EssaiModifie.E_presLimite
                Essai.E_presLimiteNF = EssaiModifie.E_presLimiteNF
                Essai.E_presLimiteTheo = EssaiModifie.E_presLimiteTheo
                Essai.E_presP0 = EssaiModifie.E_presP0
                Essai.E_PrecisPression = EssaiModifie.E_PrecisPression
                Essai.E_PrecisVolume = EssaiModifie.E_PrecisVolume
                Essai.E_poidsVol = EssaiModifie.E_poidsVol
                Essai.E_poidsVolDejauge = EssaiModifie.E_poidsVolDejauge
                Essai.E_profNappe = EssaiModifie.E_profNappe
                Essai.E_profSonde = EssaiModifie.E_profSonde
                Essai.E_U0 = EssaiModifie.E_U0
                Essai.E_VolSonde = EssaiModifie.E_VolSonde
                Essai.E_PrecisVolSonde = EssaiModifie.E_PrecisVolSonde

                ' Mise à jour des références
                Essai.E_refSonde = EssaiModifie.E_refSonde
                Essai.E_refEssaiResistancePropre = EssaiModifie.E_refEssaiResistancePropre
                Essai.E_refEssaiExpansionPropre = EssaiModifie.E_refEssaiExpansionPropre
                Essai.E_refEssaiPerteDeCharge = EssaiModifie.E_refEssaiPerteDeCharge
                Essai.E_RefSol = EssaiModifie.E_RefSol
            End If
        End Sub


        ' ===============================================================================
        ' Gestion de  C_ListMesuresEssaiForages
        ' ===============================================================================

        Public Sub AddMesuresEssaiForage(essai As Essai_Forage)
            Me.C_ListEssaisForages.Add(essai)
            ' Lier le forage au chantier (si nécessaire)
            essai.E_RefChantier = Me.C_refChantier
        End Sub

        ' Delete un forage par référence
        Public Sub DeleteMesuresessaiForage(refessai As String)
            Dim EssaiADelete As Essai_Forage = GetessaiForage(refessai)
            If EssaiADelete IsNot Nothing Then
                Me.C_ListEssaisForages.Remove(EssaiADelete)
            End If
        End Sub

        Public Function GetMesuresessaiForage(refEssai As String) As Essai_Forage
            For Each E As Essai_Forage In Me.C_ListEssaisForages
                If E.E_RefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Met à jour une mesure d'essai de forage dans une liste de mesures.
        ''' </summary>
        Public Sub UpdateMesuresEssaiForage(MesureModifiee As MesuresEssaiForage)
            ' Get la mesure dans la liste (ex: C_ListMesuresForage ou ObjetDataMesures)
            ' Exemple pour une liste dans un chantier :
            Dim mesure As MesuresEssaiForage = C_ListMesuresForage.FirstOrDefault(
        Function(m) m.RefEssai = MesureModifiee.RefEssai AndAlso m.indiceMesure = MesureModifiee.indiceMesure
    )

            If mesure IsNot Nothing Then
                ' Mise à jour des propriétés de la mesure
                mesure.RefEssai = MesureModifiee.RefEssai
                mesure.indiceMesure = MesureModifiee.indiceMesure
                mesure.pression = MesureModifiee.pression
                mesure.volume15 = MesureModifiee.volume15
                mesure.volume30 = MesureModifiee.volume30
                mesure.volume60 = MesureModifiee.volume60
                mesure.contrainteRadiale = MesureModifiee.contrainteRadiale
                mesure.contrainteVerticale = MesureModifiee.contrainteVerticale
                mesure.deformation = MesureModifiee.deformation
                mesure.PNF = MesureModifiee.PNF
                mesure.VNF = MesureModifiee.VNF
                mesure.Qi = MesureModifiee.Qi
                mesure.Pi = MesureModifiee.Pi
                mesure.Qp = MesureModifiee.Qp
                mesure.Pp = MesureModifiee.Pp
            End If
        End Sub

        ' ===============================================================================
        ' Gestion de  C_ListSondes
        ' ===============================================================================
        ' Add une sonde
        Public Sub AddSonde(Sonde As Probe)
            Me.C_ListSondes.Add(Sonde)
        End Sub

        ' Get une Sonde
        Public Function GetSonde(RefSonde As String) As Probe
            For Each P As Probe In Me.C_ListSondes
                If P.P_refSonde = RefSonde Then
                    Return P
                End If
            Next
            Return Nothing
        End Function

        ' Delete une Sonde par référence
        Public Sub DeleteSonde(refsonde As String)
            Dim SondeADelete As Probe = GetSonde(refsonde)
            If SondeADelete IsNot Nothing Then
                Me.C_ListSondes.Remove(SondeADelete)
            End If
        End Sub

        ' Mettre à jour une Sonde
        ''' <summary>
        ''' Met à jour une sonde dans la liste C_ListSondes.
        ''' </summary>
        Public Sub MettreAJourSonde(NouvelleSonde As Probe)
            ' Get la sonde à modifier dans C_ListSondes
            Dim SondeAModifier As Probe = GetSonde(NouvelleSonde.P_refSonde)

            If SondeAModifier IsNot Nothing Then
                ' Mise à jour des propriétés de la sonde
                SondeAModifier.P_MaExpansion = NouvelleSonde.P_MaExpansion
                SondeAModifier.P_MaResistance = NouvelleSonde.P_MaResistance
                SondeAModifier.P_refSonde = NouvelleSonde.P_refSonde
                SondeAModifier.P_NomSonde = NouvelleSonde.P_NomSonde
                SondeAModifier.P_ClassPrecis = NouvelleSonde.P_ClassPrecis
                SondeAModifier.P_refEssai = NouvelleSonde.P_refEssai
                SondeAModifier.P_typeSonde = NouvelleSonde.P_typeSonde
                SondeAModifier.P_typeDeformee = NouvelleSonde.P_typeDeformee
                SondeAModifier.P_typeContrainte = NouvelleSonde.P_typeContrainte
                SondeAModifier.P_ls = NouvelleSonde.P_ls
                SondeAModifier.P_lg = NouvelleSonde.P_lg
                SondeAModifier.P_lt = NouvelleSonde.P_lt
                SondeAModifier.P_longInjectePompage = NouvelleSonde.P_longInjectePompage
                SondeAModifier.P_Vs = NouvelleSonde.P_Vs
                SondeAModifier.P_ds = NouvelleSonde.P_ds
                SondeAModifier.P_ClassPrecis = NouvelleSonde.P_ClassPrecis
                SondeAModifier.P_EpMembraneGarde = NouvelleSonde.P_EpMembraneGarde
                SondeAModifier.P_PrEpMembraneGarde = NouvelleSonde.P_PrEpMembraneGarde
                SondeAModifier.P_EpMembraneMesure = NouvelleSonde.P_EpMembraneMesure
                SondeAModifier.P_PrEpMembraneMesure = NouvelleSonde.P_PrEpMembraneMesure
                SondeAModifier.P_epTubeFendu = NouvelleSonde.P_epTubeFendu
                SondeAModifier.P_PrEpTubeFendu = NouvelleSonde.P_PrEpTubeFendu
                SondeAModifier.P_EpInitMembrane = NouvelleSonde.P_EpInitMembrane
                SondeAModifier.P_PrEpInitMembrane = NouvelleSonde.P_PrEpInitMembrane
                SondeAModifier.P_PrLong = NouvelleSonde.P_PrLong
                SondeAModifier.P_PrDiam = NouvelleSonde.P_PrDiam
                SondeAModifier.P_PrP = NouvelleSonde.P_PrP
                SondeAModifier.P_PrV = NouvelleSonde.P_PrV
                SondeAModifier.P_PrPV = NouvelleSonde.P_PrPV
                SondeAModifier.P_PrZ = NouvelleSonde.P_PrZ
                SondeAModifier.P_Prt = NouvelleSonde.P_Prt
                SondeAModifier.P_Pm = NouvelleSonde.P_Pm
            End If
        End Sub


        ' ==============================================================================
        ' Gestion de C_ListSols
        ' ==============================================================================
        ' Add un sol
        Public Sub AddSol(NouveauSol As Sol)
            Me.C_ListSols.Add(NouveauSol)
        End Sub

        ' Get un sol
        Public Function GetSol(RefSol As String) As Sol
            For Each S As Sol In Me.C_ListSols
                If S.GetRefSol() = RefSol Then
                    Return S
                End If
            Next
            Return Nothing
        End Function

        ' Delete un Sol par référence
        Public Sub DeleteSol(refSol As String)
            Dim SolADelete As Sol = GetSol(refSol)
            If SolADelete IsNot Nothing Then
                Me.C_ListSols.Remove(SolADelete)
            End If
        End Sub

        ' ==============================================================================
        ' Mettre à jour un Sol
        ' ==============================================================================
        Public Sub MettreAJourSol(NouveauSol As Sol)
            Dim SolAModifier As Sol = GetSol(NouveauSol.GetRefSol)
            If SolAModifier IsNot Nothing Then
                ' Mise à jour des propriétés de la Sonde
                SolAModifier.SetRefSol(NouveauSol.RefSol)
                'SolAModifier.SetNomSol(NouveauSol.GetNomSol)
                ' ✅ Corrigé : Passer une instance de Sol
                SolAModifier.SetNomSol(SolAModifier.nomSol) ' ou GetNomSol(NouveauSol
                SolAModifier.SetZoneGeo(NouveauSol.GetZoneGeo)
                SolAModifier.SetFamille(NouveauSol.GetFamille)
                SolAModifier.SetClasseSol(NouveauSol.GetClasseSol)
                SolAModifier.SetSousClasse(NouveauSol.GetSousClasse)
                SolAModifier.SetClasseInférieure(NouveauSol.GetClasseInférieure)
                SolAModifier.SetSousClasseInférieure(NouveauSol.GetSousClasseInférieure)
                SolAModifier.SetNomSolNormalise(NouveauSol.GetNomSolNormalise)
                SolAModifier.SetDmax(NouveauSol.GetDmax)
                SolAModifier.SetTamisat50mm(NouveauSol.GetTamisat50mm)
                SolAModifier.SetTamisat2mm(NouveauSol.GetTamisat2mm)
                SolAModifier.SetTamisat80microns(NouveauSol.GetTamisat80microns)
                SolAModifier.SetVBS(NouveauSol.GetVBS)
                SolAModifier.SetIP(NouveauSol.GetIP)
                SolAModifier.SetEs(NouveauSol.GetEs)
                SolAModifier.SetPhiMu(NouveauSol.GetPhiMu)
            End If
        End Sub



        Public Event HasBeenModified()

        ' Constructeur de Chantier
        Public Sub New()
            ' Initialisation des collections avec List(Of T)
            C_ListForages = New List(Of Forage)
            C_ListEssaisForages = New List(Of Essai_Forage)
            C_ListMesuresForage = New List(Of MesuresEssaiForage)
            C_ListSols = New List(Of Sol)
            C_ListSondes = New List(Of Probe)

            ' Initialisation des propriétés par défaut
            C_FichierChantier = "à remplir"
            C_refChantier = "à remplir"
            C_nomChantier = "Nouveau chantier"
            C_descriptifChantier = "à remplir"
            C_refClient = "à remplir"
            C_entreprise = "à remplir"
            C_adresse = "à remplir"
            C_telephone = "à remplir"
            C_reference = "à remplir"

            ' Initialisation des autres propriétés
            C_MyFile = ""
            C_MyFileTitle = "à remplir"
            C_DataSetName = "à remplir"
            C_EventsSupendus = True

            ' ❌ Supprimez cette ligne (référence à Windows Forms)
            ' C_MyNodeChantier = New TreeNode
        End Sub

        '        ' *******************************************************************************
        '        ' Lecture des chantiers
        '        ' ************************************************************************
        '        Public Sub ChargeAvecBD(ByVal FichierChantier As String)
        '            Dim RowNumber, i, j, k, kMax, kRef, l, m, n, IndSonde, IndEssai, Ind, IndMesures, IndexForage As Integer
        '            Dim Pres, V15, V30, V60, SigR, SigV, Defo, Pnf, Vnf, Qi, Pi, Qp, Pp, PressionNF, VolumeNF As Single
        '            Dim Requete, RefForage, RefEssai, RefChantier As String
        '            Dim Result, PasseForage, PasseEssai, PasseSonde As Boolean
        '            Dim refSondes As New Dictionary(Of String, Boolean)
        '            Dim RowSonde As Probe
        '            Dim NewForage As Forage
        '            Dim NewEssai As Essai_Forage
        '            Dim NewMesure As MesuresEssaiForage
        '            Dim ReferenceSonde, Accu As String

        '            ' Dim Sonde, Sonde1, Sonde2 As Probe

        '            PasseForage = False
        '            PasseSonde = False

        '            On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            If MyChantier Is Nothing Then MyChantier = New LesChantiers
        '            If MyForage Is Nothing Then MyForage = New Forage

        '            ' Appel de la procédure de connection à la base de données

        '            If Not (ChargeChantier) Then

        '                ' Netoyage de l'AdaptateurChantier
        '                ' AdaptateurChantier.clear()

        '                ' Création d'une connection avec la base de données de type Access 2000

        '                ' Création d'une connection avec la base de données de type Access 2000

        '                ' Ouverture de la connection avec la BD

        '                ' *********************************************************************
        '                ' Lecture de la Table "LesChantiers"
        '                ' *********************************************************************

        '                'ma requete
        '                'Requete = "SELECT refChantier AS  refChantier, NomChantier AS  NomChantier, descriptifChantier AS  descriptifChantier, " _
        '                '    & "refClient AS  refClient, entreprise AS  entreprise, adresse AS  adresse, telephone AS  telephone, reference AS  reference FROM LesChantiers"
        '                Requete = "SELECT refChantier , NomChantier , descriptifChantier , " &
        '                          "refClient , entreprise , adresse , telephone , reference  FROM LesChantiers"

        '                'Requete = "SELECT * FROM LesChantiers"

        '                'definition de la commande

        '                ' Définition de l'AdaptateurChantier

        '                ' Nettoyage de MonChantier avant le chargement
        '                MonChantier.Tables("LesChantiers")?.Clear()

        '                ' rempli le dataset depuis la BD vers la Table "LesChantiers" de MonDataSet

        '                ' On remplit les ObjetTables avec les Tables de MonChantier
        '                ObjetDataChantier = MonChantier.Tables("LesChantiers")

        '                ' Mise à jour du compteur d'enregistrements

        '                'Me.C_Chantier.Count = MonChantier.Tables("LesChantiers").Rows.Count

        '                Select Case Lang
        '                    Case 0
        '                        MsgBox("Reading of the Table 'LesChantiers' suceeded")
        '                    Case 1
        '                        MsgBox("Lecture de la Table 'LesChantiers' réussie")
        '                End Select
        '                Select Case Lang
        '                    Case 0
        '                        MsgBox("Reading of the Table 'LesChantiers' suceeded")
        '                    Case 1
        '                        MsgBox("Lecture de la Table 'LesChantiers' réussie")
        '                End Select

        '                ' *********************************************************************
        '                ' Lecture de la Table "LesForages"
        '                ' *********************************************************************
        '                'ma requete
        '                Requete = "SELECT refChantier, refForage, NumeroForage, NomForage, dateForage, materiel, outil, abscisseX, ordonneeY, altitudeZ, coteInit, coteFinale FROM LesForages"

        '                'definition de la commande

        '                ' Définition de l'AdaptateurChantier

        '                ' Nettoyage de MonChantier avant le chargement
        '                MonChantier.Tables("LesForages")?.Clear()

        '                ' rempli le dataset depuis la BD vers la Table "LesForages" de MonDataSet
        '                ObjetDataForage = MonChantier.Tables("LesForages")

        '                ' La Table "LesForages" est chargée dans MonChantier

        '                ' Mise à jour du compteur d'enregistrements
        '                'C_ListForages.Count = MonChantier.Tables("LesForages").Rows.Count
        '                'C_ForageInitCount = MonChantier.Tables("LesForages").Rows.Count
        '                'With MonChantier.Tables("LesChantiers").Rows(0)
        '                '     refChantier = .Item("RefChantier").ToString
        '                '     NomChantier = .Item("nomChantier").ToString
        '                'End With

        '                ' refForage = MonChantier.Tables("LesForages").Rows(0).Item("refForage")

        '                ' refForage = MonChantier.Tables("LesForages").Rows(0).Item("refForage")

        '                Select Case Lang
        '                    Case 0
        '                        MsgBox("Reading of the Table 'LesForages' suceeded")
        '                    Case 1
        '                        MsgBox("Lecture de la Table 'LesForages' réussie")
        '                End Select

        '                ' *********************************************************************
        '                ' Lecture de la Table "LesEssaisForage"
        '                ' *********************************************************************
        '                'ma requete
        '                If Not (AncienChantier) Then
        '                    'Requete = "SELECT  refChantier,  refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, PrecisVolSonde, nomEssai, dateEssai, heureEssai,difPresEauAir, hauteurCpv, " _
        '                    '   & "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " _
        '                    '   & "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaisForage"

        '                    Requete = "SELECT refChantier , refForage , refEssai , refSonde , refEssaiResistancePropre , refEssaiExpansionPropre , refEssaiPerteDeCharge , " &
        '                         "refSol , Enregistreur , Operateur , VolSonde , ClassPrecis , PrecisVolSonde , nomEssai , dateEssai ,heureEssai , difPresEauAir , " &
        '                         "hauteurCpv , presP0 , presFluage , presLimite , presLimiteNf , presLimiteTheo , " &
        '"PrecisVolume , PrecisPression , poidsVol , poidsVolDejauge , coefPoisson , modPressio , " &
        '"modPressioNf , modElastique , modElastiqueNf , cohesion , frottement , frottementintergranulaire , " &
        '"U0 , profNappe , profSonde , coteZeroGeomeca FROM LesEssaisForage"
        '                Else
        '                    Requete = "SELECT refChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                        "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '"U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaisForage"
        '                End If
        '            Else
        '                Requete = "SELECT refChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                    "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                    "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaisForage"
        '            End If
        '            Else
        '            Requete = "SELECT refChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaisForage"
        '            End If

        '            'definition de la commande

        '            ' Définition de l'AdaptateurChantier

        '            ' On crée la Table "LesEssaisForage"
        '            'If Not MonChantier.Tables.Contains("LesEssaisForage") Then
        '            '    ' Dim table As New DataTable("LesEssaisForage")
        '            '    ' MonChantier.Tables.Add(table)
        '            '    Result = CreerTableLesEssaisForage(MonChantier, ConnChantier)
        '            '    If Not (Result) Then Exit Sub
        '            'End If

        '            ' Mise à jour du compteur d'enregistrements
        '            'C_ListForages.Count = MonChantier.Tables("LesForages").Rows.Count

        '            ' Nettoyage de MonChantier avant le chargement
        '            MonChantier.Tables("LesEssaisForage")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaisForage" de MonDataSet

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaisForage" de MonDataSet

        '            ' AdaptateurChantier.FillSchema(MonChantier.Tables("LesEssaisForage"), SchemaType.Source)

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaisForage" de MonDataSet
        '            ' AdaptateurChantier.Fill(MonChantier.Tables("LesEssaisForage"))

        '            ' AdaptateurChantier.FillSchema(MonChantier, SchemaType.Source, "LesEssaisForage")
        '            'AdaptateurChantier.Fill(MonChantier, "LesEssaisForage")
        '            ObjetDataEssai = MonChantier.Tables("LesEssaisForage")

        '            ' AdaptateurChantier.FillSchema(MonChantier, SchemaType.Source, "LesEssaisForage")
        '            'AdaptateurChantier.Fill(MonChantier, "LesEssaisForage")
        '            ObjetDataEssai = MonChantier.Tables("LesEssaisForage")

        '            ' Mise à jour du compteur d'enregistrements
        '            'C_EssaiCount = MonChantier.Tables("LesEssaisForage").Rows.Count
        '            'C_EssaiInitCount = MonChantier.Tables("LesEssaisForage").Rows.Count

        '            ' Mise à jour du compteur d'enregistrements
        '            'C_EssaiCount = MonChantier.Tables("LesEssaisForage").Rows.Count
        '            'C_EssaiInitCount = MonChantier.Tables("LesEssaisForage").Rows.Count

        '            'Vérificatio de la Table 
        '            For i = 0 To C_ListEssaisForages.Count - 1
        '                'Debug.WriteLine("MonChantier.Tables(LesEssaisForage).Rows(" & i.ToString & "(RefForage)=" & MonChantier.Tables("LesEssaisForage").Rows(i).Item("refForage").ToString)
        '                'Debug.WriteLine("MonChantier.Tables(LesEssaisForage).Rows(" & i.ToString & "(RefEssai)=" & MonChantier.Tables("LesEssaisForage").Rows(i).Item("refEssai").ToString)
        '            Next i

        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading of the Table 'LesEssaisForage' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesEssaisForage' réussie")
        '            End Select

        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading of the Table 'LesEssaisForage' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesEssaisForage' réussie")
        '            End Select


        '            ' *********************************************************************
        '            ' Lecture de la Table "LesMesuresForage"
        '            ' *********************************************************************
        '            ' Lecture des mesures de l'essai
        '            ' Requete = "SELECT Ind, refEssai, indiceMesure, pression, volume15, volume30, volume60, contrainteRadiale, contrainteVerticale, deformation, PNF, VNF, Qi, Pi, Qp, Pp FROM LesMesuresForage"
        '            Requete = "SELECT ind, refEssai, indiceMesure, pression, volume15, volume30, volume60, contrainteRadiale, contrainteVerticale, deformation, PNF, VNF, Qi, Pi, Qp, Pp FROM LesMesuresForage"

        '            'definition du DataAdapter

        '            'definition du DataAdapter

        '            ' Définition de l'AdaptateurChantier

        '            ' Nettoyage de MonChantier avant le chargement
        '            MonChantier.Tables("LesMesuresForage")?.Clear()

        '            ' Nettoyage de MonChantier avant le chargement
        '            MonChantier.Tables("LesMesuresForage")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesForages" de MonDataSet

        '            ObjetDataMesures = MonChantier.Tables("LesMesuresForage")

        '            '' Mise à jour du compteur d'enregistrements
        '            'C_R_Mesures.count = MonChantier.Tables("LesMesuresForage").Rows.Count
        '            'C_MesureInitCount = MonChantier.Tables("LesMesuresForage").Rows.Count

        '            Select Case Lang

        '                Case 0
        '                    MsgBox("Reading of the Table 'LesMesuresForage' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesMesuresForage' réussie")
        '            End Select

        '            MyChantier = New LesChantiers

        '            '' **************************************************************************************
        '            '' Rangement dans MyChantier des objets de la Tables("LesChantiers") de MonChantier
        '            '' **************************************************************************************
        '            '' i est le numéro du Chantier
        '            ''For Each ChantierRow As DataRow In MonChantier.Tables("LesChantiers").Rows

        '            '' Nouveau Chantier
        '            'MyChantier = New LesChantiers

        '            '    MyChantier.C_refChantier = ChantierRow("refChantier").ToString()
        '            '    MyChantier.C_nomChantier = ChantierRow("NomChantier").ToString()
        '            '    MyChantier.C_descriptifChantier = ChantierRow("descriptifChantier").ToString
        '            '    MyChantier.C_refClient = ChantierRow("refClient").ToString
        '            '    MyChantier.C_entreprise = ChantierRow("entreprise").ToString
        '            '    MyChantier.C_adresse = ChantierRow("adresse").ToString
        '            '    MyChantier.C_telephone = ChantierRow("telephone").ToString

        '            '    'With MonChantier.Tables("LesChantiers").Rows(i)
        '            '    '    MyChantier. refChantier = .Item("refChantier").ToString
        '            '    '    MyChantier. NomChantier = .Item("NomChantier").ToString
        '            '    '    MyChantier. descriptifChantier = .Item("descriptifChantier").ToString
        '            '    '    MyChantier. refClient = .Item("refClient").ToString
        '            '    '    MyChantier. entreprise = .Item("entreprise").ToString
        '            '    '    MyChantier. adresse = .Item("adresse").ToString
        '            '    '    MyChantier. telephone = .Item("telephone").ToString
        '            '    'End With

        '            '    ' **************************************************************************************
        '            '    ' Rangement dans  ListForages des objets de la Tables("LesForages") de MonChantier
        '            '    ' **************************************************************************************

        '            '    ' j est le numéro du Forage
        '            '    Dim forageIndex As Integer = 1

        '            '    For Each forageRow As DataRow In MonChantier.Tables("LesForages").Rows

        '            '        Dim new_Forage As New Forage

        '            '        If forageRow("refChantier") Is Nothing Then Exit For
        '            '        ' Nouveau Forage
        '            '        MyForage = New Forage
        '            '        'MyForage = MyChantier. ListForages(forageIndex - 1)

        '            '        new_Forage.F_RefChantier = forageRow("refChantier").ToString()
        '            '        new_Forage.F_RefForage = forageRow("refForage").ToString()
        '            '        new_Forage.F_NomForage = forageRow("NomForage").ToString
        '            '        new_Forage.F_dateForage = forageRow("dateForage").ToString
        '            '        new_Forage.F_materiel = forageRow("materiel").ToString
        '            '        new_Forage.F_outil = forageRow("outil").ToString
        '            '        new_Forage.F_abscisseX = forageRow("abscisseX").ToString
        '            '        new_Forage.F_ordonneeY = forageRow("ordonneeY").ToString
        '            '        new_Forage.F_altitudeZ = forageRow("altitudeZ").ToString
        '            '        new_Forage.F_coteInit = forageRow("coteInit").ToString
        '            '        new_Forage.F_coteFinale = forageRow("coteFinale").ToString

        '            '        ' Redimensionner le tableau et Add le nouveau forage
        '            '        MyForage = new_Forage
        '            '        'MyForage = MyChantier. ListForages(forageIndex - 1)
        '            '        forageIndex += 1

        '            '        ' **************************************************************************************
        '            '        ' Rangement dans MyForage des objets de la Tables("LesEssaisForage") de MonChantier
        '            '        ' **************************************************************************************

        '            '        ' k est le numéro de l'EssaisForage
        '            '        kMax = MonChantier.Tables("LesEssaisForage").Rows.Count

        '            '        Dim EssaiIndex As Integer = 1

        '            '        For Each essaiRow As DataRow In MonChantier.Tables("LesEssaisForage").Rows

        '            '            If essaiRow("refForage").ToString() = new_Forage.F_RefForage Then
        '            '                Dim new_Essai As New Essai_Forage(False)
        '            '                Dim Verif As String
        '            '                Verif = essaiRow("refEssai").ToString()
        '            '                new_Essai.E_RefEssai = essaiRow("refEssai").ToString()
        '            '                new_Essai.E_RefChantier = essaiRow("refChantier").ToString
        '            '                new_Essai.E_RefForage = essaiRow("refForage").ToString
        '            '                new_Essai.E_RefEssai = essaiRow("refEssai").ToString
        '            '                'Accu = "MonChantier.Tables(LesEssaisForage).Rows(" & k.ToString & ")=" & essaiRow("refEssai").ToString
        '            '                'Debug.WriteLine(Accu)
        '            '                new_Essai.E_refSonde = essaiRow("refSonde").ToString
        '            '                'new_Essai.Sonde = essaiRow("nomSonde").ToString
        '            '                new_Essai.E_refEssaiResistancePropre = essaiRow("refEssaiResistancePropre").ToString
        '            '                new_Essai.E_refEssaiExpansionPropre = essaiRow("refEssaiExpansionPropre").ToString
        '            '                new_Essai.E_RefSol = essaiRow("refSol").ToString
        '            '                If essaiRow("ClassPrecis").ToString <> "" Then new_Essai.E_ClassPrecis = essaiRow("ClassPrecis").ToString
        '            '                new_Essai.E_Enregistreur = essaiRow("Enregistreur").ToString
        '            '                new_Essai.E_Operateur = essaiRow("Operateur").ToString
        '            '                new_Essai.E_VolSonde = essaiRow("VolSonde").ToString
        '            '                new_Essai.E_ClassPrecis = Valeurs(essaiRow("ClassPrecis").ToString)
        '            '                new_Essai.E_PrecisVolSonde = essaiRow("PrecisVolSonde").ToString
        '            '                new_Essai.E_nomEssai = essaiRow("nomEssai").ToString
        '            '                new_Essai.E_dateEssai = essaiRow("dateEssai").ToString
        '            '                new_Essai.E_heureEssai = essaiRow("heureEssai").ToString
        '            '                new_Essai.E_difPresEauAir = essaiRow("difPresEauAir").ToString
        '            '                new_Essai.E_hauteurCpv = essaiRow("hauteurCpv").ToString
        '            '                new_Essai.E_presP0 = essaiRow("presP0").ToString
        '            '                new_Essai.E_presFluage = essaiRow("presFluage").ToString
        '            '                new_Essai.E_presLimite = essaiRow("presLimite").ToString
        '            '                new_Essai.E_presLimiteNF = essaiRow("presLimiteNf").ToString
        '            '                new_Essai.E_poidsVol = essaiRow("poidsVol").ToString
        '            '                new_Essai.E_poidsVolDejauge = essaiRow("poidsVolDejauge").ToString
        '            '                new_Essai.E_coefPoisson = essaiRow("coefPoisson").ToString
        '            '                new_Essai.E_modPressio = essaiRow("modPressio").ToString
        '            '                new_Essai.E_modPressioNf = essaiRow("modPressioNf").ToString
        '            '                new_Essai.E_modElastique = essaiRow("modElastique").ToString
        '            '                new_Essai.E_modElastiqueNf = essaiRow("modElastiqueNf").ToString
        '            '                new_Essai.E_Cohesion = essaiRow("cohesion").ToString
        '            '                new_Essai.E_Frottement = essaiRow("frottement").ToString
        '            '                new_Essai.E_FrottementIntergranulaire = essaiRow("frottementintergranulaire").ToString
        '            '                new_Essai.E_U0 = essaiRow("U0").ToString
        '            '                new_Essai.E_profNappe = essaiRow("profNappe").ToString
        '            '                new_Essai.E_profSonde = essaiRow("profSonde").ToString
        '            '                new_Essai.E_coteZeroGeomeca = essaiRow("coteZeroGeomeca").ToString

        '            '                EssaiIndex += 1

        '            '                MyEssai = Essai_Forage.CreateNewEssai(False)
        '            '                MyEssai = new_Essai

        '            '                For IndSonde = 0 To MyListeSondes.P_ListeSondes.Count - 1
        '            '                    If MyEssai.E_refSonde = MyListeSondes.P_ListeSondes(IndSonde).GetRefSonde Then Exit For
        '            '                Next IndSonde

        '            '                MySonde = MyListeSondes.GetSondeParIndex(IndSonde)

        '            '                ' MAJ de MyEssai.refEssaiResistancePropre
        '            '                If MySonde.P_Resistance.Count > 0 Then
        '            '                    kMax = MySonde.P_Resistance.Count
        '            '                    If kMax = 0 Then kMax = 1
        '            '                    For IndEssai = 0 To kMax
        '            '                        If MySonde.P_Resistance(IndEssai) IsNot (Nothing) Then
        '            '                            If MyEssai.E_refEssaiResistancePropre = MySonde.P_Resistance(IndEssai).R_refEssai Then
        '            '                                MyResistance = New Essai_Resistance
        '            '                                MyResistance = MySonde.P_Resistance(IndEssai)
        '            '                                Exit For
        '            '                            End If
        '            '                        End If
        '            '                    Next IndEssai
        '            '                End If

        '            '                MyEssai.E_MyResistance = MyResistance
        '            '                ' MAJ de MyEssai.refEssaiExpansionPropre
        '            '                If MySonde.P_Expansion.Count > 0 Then
        '            '                    For IndEssai = 0 To MySonde.P_Expansion.Count - 1
        '            '                        If MyEssai.E_refEssaiExpansionPropre = MySonde.P_Expansion(IndEssai).E_RefEssai Then
        '            '                            MyExpansion = New Essai_Expansion
        '            '                            MyExpansion = MySonde.P_Expansion(IndEssai)
        '            '                            Exit For
        '            '                        End If
        '            '                    Next IndEssai
        '            '                End If

        '            '                MyEssai.E_MyExpansion = MyExpansion

        '            '                'End If ' Fin de If essaiRow("refForage").ToString() = NewForage.RefForage Then

        '            '                'End If ' Fin de If essaiRow("refForage").ToString() = NewForage.RefForage Then

        '            '                'ReDim Preserve MyChantier. ListEssaisForages(k)
        '            '                'MyChantier. ListEssaisForages(k) = MyForage.F_ListEssais(l - 1)

        '            '                ' **************************************************************************************
        '            '                ' Rangement dans MyEssai des objets de la Tables("LesMesuresForage") de MonChantier
        '            '                ' **************************************************************************************

        '            '                '' Affectation de la Liste des Sondes à MyChantier
        '            '                '    RowSonde = New Probe
        '            '                '    RowSonde = MyListeSondes.ListeSondes(i)
        '            '                '    If MyChantier. ListeSondes.Count - 1 <= i Then
        '            '                '        ReDim Preserve MyChantier. ListeSondes(i)
        '            '                '        MyChantier. ListeSondes(i) = New Probe
        '            '                '    End If
        '            '                '    MyChantier. ListeSondes(i) = RowSonde
        '            '                'Next i

        '            '                '' Affectation de la Liste des Sondes à MyChantier
        '            '                '    RowSonde = New Probe
        '            '                '    RowSonde = MyListeSondes.ListeSondes(i)
        '            '                '    If MyChantier. ListeSondes.Count - 1 <= i Then
        '            '                '        ReDim Preserve MyChantier. ListeSondes(i)
        '            '                '        MyChantier. ListeSondes(i) = New Probe
        '            '                '    End If
        '            '                '    MyChantier. ListeSondes(i) = RowSonde
        '            '                'Next i


        '            '                ' m est le numéro des MesuresForage
        '            '                m = 0
        '            '                For Each mesureRow As DataRow In MonChantier.Tables("LesMesuresForage").Rows

        '            '                    If mesureRow("refEssai").ToString() = new_Essai.E_RefEssai Then

        '            '                        Dim new_Mesure As New MesureEssaiForage()
        '            '                        new_Mesure.Pression = mesureRow("pression").ToString()

        '            '                        ' On ajoute une nouvelle Mesure à l'Essai actuel
        '            '                        If mesureRow("refEssai").ToString() = new_Essai.E_RefEssai Then

        '            '                            new_Mesure = New MesureEssaiForage
        '            '                            m += 1
        '            '                            ' Déclaration d'une mesure
        '            '                            new_Mesure.Pression = mesureRow("pression").ToString
        '            '                            new_Mesure.Volume15 = mesureRow("volume15").ToString
        '            '                            new_Mesure.Volume30 = mesureRow("volume30").ToString
        '            '                            new_Mesure.Volume60 = mesureRow("volume60").ToString
        '            '                            new_Mesure.ContrainteRadiale = mesureRow("contrainteRadiale").ToString
        '            '                            SigV = mesureRow("contrainteVerticale").ToString
        '            '                            new_Mesure.Deformation = mesureRow("deformation").ToString
        '            '                            new_Mesure.PNF = mesureRow("PNF").ToString
        '            '                            new_Mesure.VNF = mesureRow("VNF").ToString
        '            '                            PressionNF = MonChantier.Tables("LesMesuresForage").Rows(m)("PNF")
        '            '                            VolumeNF = MonChantier.Tables("LesMesuresForage").Rows(m)("VNF")
        '            '                            'Debug.WriteLine("MonChantier.Tables(LesMesuresForage).Rows(" & m.ToString() & ").PNF= " & PressionNF.ToString())
        '            '                            'Debug.WriteLine("MonChantier.Tables(LesMesuresForage).Rows(" & m.ToString() & ").VNF= " & VolumeNF.ToString())
        '            '                            Qi = 0
        '            '                            Pi = 0
        '            '                            Qp = 0
        '            '                            Pp = 0

        '            '                            MyEssai.NouvelleMesure(False, m, n, new_Mesure.Pression, new_Mesure.Volume15, new_Mesure.Volume30, new_Mesure.Volume60,
        '            '                                                    new_Mesure.ContrainteRadiale, SigV, new_Mesure.Deformation, new_Mesure.PNF, new_Mesure.VNF, Qi, Pi, Qp, Pp)
        '            '                            IndMesures = IndMesures + 1
        '            '                        End If ' fin du If mesureRow("refEssai").ToString() = new_Essai.E_RefEssai Then

        '            '                        'If IndMesures > 1 Then
        '            '                        'IndMesures = IndMesures + 1
        '            '                    End If ' fin du If mesureRow("refEssai").ToString() = new_Essai.E_RefEssai Then

        '            '                Next mesureRow ' Fin de For m = 0 To MonChantier.Tables("LesMesuresForage").Rows().Count - 1
        '            '            End If ' fin du If essaiRow("refForage").ToString() = new_Forage.RefForage Then
        '            '        Next essaiRow
        '            '        ' --------------------------------------------
        '            '        ' MyEssai.E_MesuresEssaiForage est juste à ce niveau
        '            '        ' --------------------------------------------
        '            '    Next forageRow ' Fin du For Each forageRow As DataRow In MonChantier.Tables("LesForages").Rows
        '            'Next ChantierRow ' Fin du For Each row As DataRow In MonChantier.Tables("LesChantiers").Rows

        '            ChargeChantier = True

        '            'End If ' Fin de If Not (ChargeChantier) Then


        '            ' Sauvegarde du nom de fichier pour une future utilisation
        '            MyChantier.C_MyFile = FichierChantier

        '            ' Extraction du nom de fichier 

        '            '' Vérification de MYListeSondes.Tables("LesEssaisForage")
        '            'refSondes.Clear()
        '            'For i = 0 To MyListeSondes.ListeSondes.Count - 1
        '            '    RowSonde = MyListeSondes.ListeSondes(i)
        '            '    ReferenceSonde = RowSonde.refSonde.ToString()
        '            '    If refSondes.ContainsKey(ReferenceSonde) Then
        '            '        MsgBox("Doublon détecté dans MyListeSondes pour refSonde : " & i & ReferenceSonde)
        '            '        Exit For
        '            '    Else
        '            '        refSondes.Add(ReferenceSonde, True)
        '            '    End If
        '            'Next i

        '            ' Le chantier est considéré comme déjà sauvé
        '            C_IsSaved = True
        '            Exit Sub

        '            ' Traitement des erreurs lors du chargement de la base de données:
        'errFind:
        '            ' Fermeture de la connection
        '            ' ConnChantier = Nothing

        '            ' Chargement de l'objet d'erreur
        '            Select Case Lang
        '                Case 0
        '                    MsgBox(" Error during loading of building site " & FichierChantier & " ", Information.Err.Description)
        '                Case 1
        '                    MsgBox(" Erreur lors de l'ouverture du chantier référencé " & FichierChantier)
        '                    MsgBox(" Erreur lors de l'ouverture du chantier référencé " & FichierChantier & " ", Err.Description)
        '            End Select
        '            End
        '            Resume Next
        '        End Sub
        Public Function GetRefChantier()
            GetRefChantier = C_refChantier
        End Function

        Public Function GetNomChantier()
            GetNomChantier = C_nomChantier
        End Function

        '        Public Function RemplirTableLesEssaisForage(ByRef MonChantier As DataSet, ByVal C_Forage As String, ByVal C_Essai As String) As Boolean

        '            RemplirTableLesEssaisForage = False
        '            Dim essaiIndex, l, l1, m, n, n0, Ind As Integer
        '            Dim Pres, V15, V30, V60, SigR, SigV, Defo, PnF, PnV, VnF, Qi, Pi, Qp, Pp As Single
        '            Dim currentRefForage, currentRefEssai As String
        '            Dim Passe, PasseForage, PasseEssai As Boolean

        '            On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            ' **************************************************************************************
        '            ' Boucle pour les essais du forage courant (essaiIndex)
        '            ' **************************************************************************************

        '            ' ======== Boucle pour les essais du forage courant ==============
        '            For essaiIndex = 0 To MonChantier.Tables("LesEssaisForage").Rows.Count - 1
        '                currentRefForage = MonChantier.Tables("LesEssaisForage").Rows(essaiIndex).Item("refForage").ToString()
        '                System.Diagnostics.Debug.Print("Essai " & essaiIndex & " : refForage = " & currentRefForage)
        '            Next

        '            PasseForage = False
        '            PasseEssai = False

        '            ' On recherche dans la MonChantier.Tables("LesEssaisForage") ceux qui correspondent a Re Forage
        '            For essaiIndex = 0 To MonChantier.Tables("LesEssaisForage").Rows.Count - 1

        '                currentRefForage = MonChantier.Tables("LesEssaisForage").Rows(essaiIndex).Item("refForage").ToString()
        '                currentRefEssai = MonChantier.Tables("LesEssaisForage").Rows(essaiIndex).Item("refEssai").ToString()
        '                Debug.WriteLine("Essai " & essaiIndex & " : refForage = " & currentRefForage)

        '                ' Mise à jour de refForage si nécessaire
        '                If currentRefForage = MyForage.F_RefForage And currentRefEssai = MyEssai.E_RefEssai And Not (PasseForage) Then
        '                    PasseForage = True

        '                    MonChantier.Tables("LesEssaisForage").Rows(essaiIndex).Item("refForage") = MyForage.F_RefForage
        '                    'currentRefForage = "RefSP2" ' Mise à jour de la variable locale

        '                    Debug.WriteLine("Ajout d un Essai : --- Essai " & essaiIndex & " : refForage = " & currentRefForage)

        '                    ' Vérifiez que currentRefForage correspond à MyForage.RefForage
        '                    'If currentRefForage = MyForage.RefForage Then

        '                    ' Ajoutez un nouvel essai
        '                    For k = 0 To MyForage.F_ListEssais.Count - 1
        '                        If MyForage.F_ListEssais(k).E_RefEssai = MyEssai.E_RefEssai Then
        '                            PasseEssai = True
        '                            l1 = k + 1
        '                            Exit For
        '                        End If
        '                    Next k

        '                    If Not (PasseEssai) Then
        '                        If MonChantier.Tables("LesEssaisForage") IsNot Nothing Then
        '                            Debug.WriteLine("Appel de MyForage.NouveauEssai(False) essaiIndex=" & essaiIndex.ToString)

        '                            ' La Table MonChantier.Tables("LesEssaisForage") nexiste pas,il faut la créer
        '                            MyForage.NouveauEssai(False)
        '                        Else
        '                            Debug.WriteLine("Appel de MyForage.NouveauEssai(False) essaiIndex=" & essaiIndex.ToString)

        '                            ' La Table MonChantier.Tables("LesEssaisForage") existe, on met simplement a jour 
        '                            MyForage.NouveauEssai(True)
        '                        End If
        '                    End If ' If Not (PasseEssai) Then

        '                    If Not (PasseEssai) Then l1 = MyForage.F_ListEssais.Count
        '                    ' Mise à jour des propriétés de l'essai
        '                    With MonChantier.Tables("LesEssaisForage").Rows(essaiIndex)
        '                        MyForage.F_ListEssais(l1 - 1).E_RefEssai = .Item("RefEssai").ToString
        '                        MyForage.F_ListEssais(l1 - 1).E_RefForage = currentRefForage ' Utilisez la valeur mise à jour
        '                        MyForage.F_ListEssais(l1 - 1).E_RefEssai = .Item("RefEssai")
        '                        MyForage.F_ListEssais(l1 - 1).E_refSonde = .Item("RefSonde")
        '                        MyForage.F_ListEssais(l1 - 1).E_refEssaiResistancePropre = .Item("refEssaiResistancePropre")
        '                        MyForage.F_ListEssais(l1 - 1).E_refEssaiExpansionPropre = .Item("refEssaiExpansionPropre")
        '                        'MyForage.F_ListEssais(l1 - 1).F_refEssaiPerteDeCharge = .Item("refEssaiPerteDeCharge")
        '                        MyForage.F_ListEssais(l1 - 1).E_RefSol = .Item("refSol")
        '                        MyForage.F_ListEssais(l1 - 1).E_Enregistreur = .Item("Enregistreur")
        '                        MyForage.F_ListEssais(l1 - 1).E_Operateur = .Item("Operateur")
        '                        MyForage.F_ListEssais(l1 - 1).E_VolSonde = Convert.ToSingle(.Item("VolSonde"))
        '                        MyForage.F_ListEssais(l1 - 1).E_ClassPrecis = Convert.ToInt32(.Item("ClassPrecis"))
        '                        MyForage.F_ListEssais(l1 - 1).E_PrecisVolSonde = Convert.ToSingle(.Item("PrecisVolSonde"))
        '                        MyForage.F_ListEssais(l1 - 1).E_nomEssai = .Item("nomEssai")
        '                        MyForage.F_ListEssais(l1 - 1).E_dateEssai = .Item("dateEssai")
        '                        MyForage.F_ListEssais(l1 - 1).E_heureEssai = .Item("heureEssai")
        '                        MyForage.F_ListEssais(l1 - 1).E_difPresEauAir = Convert.ToSingle(.Item("difPresEauAir"))
        '                        MyForage.F_ListEssais(l1 - 1).E_hauteurCpv = Convert.ToSingle(.Item("hauteurCpv"))
        '                        MyForage.F_ListEssais(l1 - 1).E_presP0 = Convert.ToSingle(.Item("presP0"))
        '                        MyForage.F_ListEssais(l1 - 1).E_presFluage = Convert.ToSingle(.Item("presFluage"))
        '                        MyForage.F_ListEssais(l1 - 1).E_presLimite = Convert.ToSingle(.Item("preslimite"))
        '                        MyForage.F_ListEssais(l1 - 1).E_presLimiteNF = Convert.ToSingle(.Item("presLimiteNf"))
        '                        MyForage.F_ListEssais(l1 - 1).E_presLimiteTheo = Convert.ToSingle(.Item("presLimiteTheo"))
        '                        MyForage.F_ListEssais(l1 - 1).E_PrecisVolume = Convert.ToSingle(.Item("PrecisVolume"))
        '                        MyForage.F_ListEssais(l1 - 1).E_PrecisPression = Convert.ToSingle(.Item("PrecisPression"))
        '                        MyForage.F_ListEssais(l1 - 1).E_poidsVol = Convert.ToSingle(.Item("poidsVol"))
        '                        MyForage.F_ListEssais(l1 - 1).E_poidsVolDejauge = Convert.ToSingle(.Item("poidsVolDejauge"))
        '                        MyForage.F_ListEssais(l1 - 1).E_coefPoisson = Convert.ToSingle(.Item("coefPoisson"))
        '                        MyForage.F_ListEssais(l1 - 1).E_modPressio = Convert.ToSingle(.Item("modPressio"))
        '                        MyForage.F_ListEssais(l1 - 1).E_modPressioNf = Convert.ToSingle(.Item("modPressioNf"))
        '                        MyForage.F_ListEssais(l1 - 1).E_modElastique = Convert.ToSingle(.Item("modElastique"))
        '                        MyForage.F_ListEssais(l1 - 1).E_modElastiqueNf = Convert.ToSingle(.Item("modElastiqueNf"))
        '                        MyForage.F_ListEssais(l1 - 1).E_Cohesion = Convert.ToSingle(.Item("cohesion"))
        '                        MyForage.F_ListEssais(l1 - 1).E_Frottement = Convert.ToSingle(.Item("frottement"))
        '                        MyForage.F_ListEssais(l1 - 1).E_FrottementIntergranulaire = Convert.ToSingle(.Item("frottementintergranulaire"))
        '                        ' MyForage.F_ListEssais(l1 - 1).E_U0 = .Item("U0").col(0).ToString
        '                        MyForage.F_ListEssais(l1 - 1).E_U0 = Convert.ToSingle(.Item("U0"))
        '                        'MyForage.F_ListEssais(l1 - 1).F_profNappe = .Item("profNappe").col(0).ToString
        '                        MyForage.F_ListEssais(l1 - 1).E_profNappe = Convert.ToSingle(.Item("profNappe"))
        '                        ' MyForage.F_ListEssais(l1 - 1).E_profSonde = .Item("profSonde").col(0).ToString
        '                        MyForage.F_ListEssais(l1 - 1).E_profSonde = Convert.ToSingle(.Item("profSonde"))
        '                        ' MyForage.F_ListEssais(l1 - 1).E_coteZeroGeomeca = .Item("coteZeroGeomeca").col(0).ToString
        '                        MyForage.F_ListEssais(l1 - 1).E_coteZeroGeomeca = Convert.ToSingle(.Item("coteZeroGeomeca"))
        '                        MyEssai = MyForage.F_ListEssais(l1 - 1)
        '                        MyEssai.E_RefEssai = GetSafeString(.Item("RefEssai"))
        '                        Dim refSondeValue As Object = .Item("refSonde")
        '                        If refSondeValue IsNot Nothing AndAlso Not DBNull.Value.Equals(refSondeValue) Then
        '                            MyForage.F_ListEssais(l1 - 1).E_refSonde = .Item("RefSonde")
        '                        Else
        '                            MyForage.F_ListEssais(l1 - 1).E_refSonde = ""  ' Valeur par défaut si NULL
        '                        End If
        '                        If MySonde IsNot Nothing Then MySonde = New Probe
        '                        MySonde.P_refSonde = MyForage.F_ListEssais(l1 - 1).E_refSonde
        '                        MyEssai.SetMySonde(MySonde)

        '                        ' **************************************************************************************
        '                        ' Rangement dans MyEssai des objets de la Tables("LesMesuresForage") de MonChantier
        '                        ' **************************************************************************************

        '                        ' l est le numéro de l'EssaisForage
        '                        ' on décrit avec lêindice m la Tables("LesMesuresForage")
        '                        Passe = False

        '                        For m = 0 To MonChantier.Tables("LesMesuresForage").Rows().Count - 1


        '                            If MonChantier.Tables("LesMesuresForage").Rows(m) IsNot Nothing Then

        '                                ' On ajoute une nouvelle Mesure à l'Essai actuel
        '                                If MyEssai.E_RefEssai = MonChantier.Tables("LesMesuresForage").Rows(m).Item("refEssai").ToString Then

        '                                    With MonChantier.Tables("LesMesuresForage").Rows(m)

        '                                        'Mesures(0, R_Mesures.count - 1) = pression
        '                                        'Mesures(1, R_Mesures.count - 1) = volume15
        '                                        'Mesures(2, R_Mesures.count - 1) = volume30
        '                                        'Mesures(3, R_Mesures.count - 1) = volume60
        '                                        'Mesures(4, R_Mesures.count - 1) = contrainteR
        '                                        'Mesures(5, R_Mesures.count - 1) = deformation
        '                                        'Mesures(6, R_Mesures.count - 1) = PNF
        '                                        'Mesures(7, R_Mesures.count - 1) = VNF
        '                                        'Mesures(8, R_Mesures.count - 1) = Qi
        '                                        'Mesures(9, R_Mesures.count - 1) = Pi
        '                                        'Mesures(10, R_Mesures.count - 1) = Qp
        '                                        'Mesures(11, R_Mesures.count - 1) = Pp
        '                                        'Mesures(12, R_Mesures.count - 1) = contrainteV
        '                                        ' Déclaration d'un2 mesure
        '                                        Ind = m
        '                                        n = .Item("indiceMesure").ToString
        '                                        Pres = .Item("pression").ToString
        '                                        V15 = .Item("volume15").ToString
        '                                        V30 = .Item("volume30").ToString
        '                                        V60 = .Item("volume60").ToString
        '                                        SigR = .Item("contrainteRadiale").ToString
        '                                        SigV = .Item("contrainteVerticale").ToString
        '                                        Defo = .Item("deformation").ToString
        '                                        PnF = .Item("PNF").ToString
        '                                        VnF = .Item("VNF").ToString
        '                                        Qi = 0
        '                                        Pi = 0
        '                                        Qp = 0
        '                                        Pp = 0

        '                                        ' MAJ de MyEssai.E_MesuresEssaiForage
        '                                        If Not (Passe) Then
        '                                            n0 = n
        '                                            n = .Item("indiceMesure").ToString - n0
        '                                            Passe = True
        '                                        End If
        '                                        'ReDim Preserve MyEssai.E_MesuresEssaiForage(15, MyEssai.E_MesuresEssaiForage.Count + 1)
        '                                        MyEssai.NouvelleMesure(False, Ind, n, Pres, V15, V30, V60, SigR, SigV, Defo, PnF, VnF, Qi, Pi, Qp, Pp)
        '                                        Debug.WriteLine("MyEssai.E_MesuresEssaiForage(" & "Ind=" & Ind.ToString & "n=" & n.ToString & "Pres=" & Pres.ToString & "V15=" & V15.ToString)
        '                                        ' MAJ de MyChantier. MyListeForages.
        '                                        ' ReDim Preserve MyChantier. MyListeForages.ListEssais(E_Mesures.count+ 1)
        '                                        'MyChantier. MyListeForages.
        '                                    End With
        '                                End If
        '                            End If
        '                        Next m
        '                        RemplirTableLesEssaisForage = True
        '                        Exit Function
        '                    End With '  With MonChantier.Tables("LesEssaisForage").Rows(essaiIndex)
        '                End If '  If currentRefForage = MyForage.RefForage
        '            Next essaiIndex

        '            ' ======== Fin de Boucle pour les essais du forage courant ==============



        '            '' **************************************************************************************
        '            '' Rangement dans MyEssai des objets de la Tables("LesMesuresForage") de MonChantier
        '            '' **************************************************************************************

        '            '' l est le numéro de l'EssaisForage
        '            '' on décrit avec lêindice m la Tables("LesMesuresForage")
        '            'Passe = False

        '            RemplirTableLesEssaisForage = True

        '            Exit Function
        'errFind:
        '            ' Fermeture de la connection
        '            ' ConnChantier = Nothing

        '            ' Chargement de l'objet d'erreur
        '            Select Case Lang
        '                Case 0
        '                    MsgBox(" Error during loading of building site " & FichierChantier & " ", Information.Err.Description)
        '                Case 1
        '                    MsgBox(" Erreur lors de l'ouverture du chantier référencé " & FichierChantier)
        '                    MsgBox(" Erreur lors de l'ouverture du chantier référencé " & FichierChantier & " ", Err.Description)
        '            End Select
        '            End
        '            Resume Next
        '        End Function


        'Public Sub AddForage(forage As Forage, Indice As Integer)

        '    If MyChantier.C_ListForages Is Nothing Then
        '        ReDim MyChantier.C_ListForages(1)
        '        'Else
        '        '    ReDim Preserve MyChantier. ListForages(Indice)
        '    End If
        '    'MyChantier. ForageCount = MyChantier. ListForages.Count
        '    If MyChantier.C_ListForages.Count > 0 Then
        '        ReDim Preserve MyChantier.C_ListForages(C_ListForages.Count - 1)
        '    Else
        '        ReDim Preserve MyChantier.C_ListForages(0)
        '    End If
        '    MyChantier.C_ListForages(C_ListForages.Count - 1) = forage

        '    If MyListeForages Is Nothing Then
        '        MyListeForages = New List(Of Forage)
        '    End If

        '    If Indice > MyListeForages.Count And Indice > 0 Then

        '        ' Add un nouvel élément à la liste
        '        MyListeForages.Add(New Forage)
        '        'ReDim Preserve C_ListForages(Indice - 1)
        '        C_ListForages(Indice - 1) = New Forage
        '        MyListeForages(Indice - 1) = C_ListForages(Indice - 1)
        '        MyChantier.C_ListForages(Indice - 1) = C_ListForages(Indice - 1)

        '        'Next i
        '    End If

        '    ' le compteur d'enregistrement est incrémenté automatiquement
        '    'C_ListForages.Count = C_ListForages.Count
        'End Sub


        Public Function GetForage(index As Integer) As Forage
            If index >= 0 AndAlso index < C_ListForages.Count Then
                GetForage = C_ListForages(index)
                MyForage = C_ListForages(index)
                Exit Function
            Else
                Return Nothing
            End If
        End Function

        Public Function GetDescriptifChantier()
            GetDescriptifChantier = C_descriptifChantier
        End Function

        Public Function GetRefClient()
            GetRefClient = C_refClient
        End Function

        Public Function GetEntreprise()
            GetEntreprise = C_entreprise
        End Function

        Public Function GetAdresse()
            GetAdresse = C_adresse
        End Function

        Public Function GetTelephone()
            GetTelephone = C_telephone
        End Function

        Public Function GetForagesCount()
            GetForagesCount = C_ListForages.Count
        End Function

        Public Sub SetRefChantier(ByVal ref As String)
            C_refChantier = ref
        End Sub


        Public Sub SetNomChantier(ByVal nom As String)
            C_nomChantier = nom
        End Sub

        Public Sub SetDescriptifChantier(ByVal descriptif As String)
            C_descriptifChantier = descriptif
        End Sub

        Public Sub SetRefClient(ByVal ref As String)
            C_refClient = ref
        End Sub

        Public Sub SetEntreprise(ByVal entrep As String)
            C_entreprise = entrep
        End Sub

        Public Sub SetAdresse(ByVal adr As String)
            C_adresse = adr

            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not C_EventsSupendus Then
                RaiseEvent HasBeenModified()
            End If
        End Sub

        Public Sub SetTelephone(ByVal tel As String)
            C_telephone = tel

            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not C_EventsSupendus Then
                RaiseEvent HasBeenModified()
            End If
        End Sub


        ' Affichage du nouveau chantier dans l'arborescence des chantiers
        'Public Sub AfficheDansArbo(ByRef Arbo As TreeView)
        '    MsgBox "AfficheDansArbo"

        ' Vidage de l'arborescence
        'Arbo.Nodes.Clear()
        '' NomChantier = MyChantier.GetNomChantier
        ' NomChantier = MyChantier. NomChantier

        '' Ajout du noeud racine
        'TreeViewArboRoot = New TreeNode( NomChantier)
        'TreeViewArboRoot.Name =  NomChantier
        'Arbo.Nodes.Add(TreeViewArboRoot)

        '' Affichage du Chantier
        'Dim element As Integer
        ''  ReDim Preserve  MyListeForages(C_ListForages.Count)
        ' C_ListForages.Count = MyChantier. C_ListForages.Count
        'If  C_ListForages.Count > 0 Then
        '    For element = 0 To (MyChantier. C_ListForages.Count - 1)
        '        MyForage = New Forage(True)
        '        MyForage = MyChantier. C_ListForages(element)
        '        ' Ajout du noeud Parent Forage
        '        TreeViewArboParent = New TreeNode(MyChantier. C_ListForages(element).NomForage)
        '        TreeViewArboParent.Name = MyChantier. C_ListForages(element).NomForage
        '        TreeViewArboRoot.Nodes.Add(TreeViewArboParent)
        '        MyForage.RefForage = MyChantier. C_ListForages(element).RefForage
        '        MyForage.NomForage = MyChantier. C_ListForages(element).NomForage
        '        MyForage.RefChantier = MyChantier. refChantier
        '        Call  C_ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)
        '    Next

        'End If
        'Arbo.ExpandAll()

        ' Vidage de l'arborescence
        'Arbo.Nodes.Clear()

        '' Vérifiez que MyChantier n'est pas Nothing
        'If MyChantier Is Nothing Then
        '        Exit Sub
        '    End If

        '    ' Récupération du nom du chantier
        '    C_nomChantier = MyChantier.NomChantier

        '    ' Ajout du noeud racine
        '    TreeViewArboRoot = New TreeNode(C_nomChantier)
        '    TreeViewArboRoot.Name = C_nomChantier
        '    Arbo.Nodes.Add(TreeViewArboRoot)

        '    ' Affichage du Chantier
        '    C_ListForages.Count = MyChantier.C_ListForages.Count

        '    If C_ListForages.Count > 0 Then
        '        For element As Integer = 0 To C_ListForages.Count - 1
        '            ' Vérifiez que MyChantier. C_ListForages(element) n'est pas Nothing
        '            If MyChantier.C_ListForages(element) IsNot Nothing Then
        '                ' Ajout du noeud Parent Forage
        '                Dim TreeViewArboParent As TreeNode = New TreeNode(MyChantier.C_ListForages(element).NomForage)
        '                MyForage = New Forage(True)
        '                MyForage = MyChantier.C_ListForages(element)
        '                ' Ajout du noeud Parent Forage
        '                TreeViewArboParent = New TreeNode(MyChantier.C_ListForages(element).NomForage)
        '                TreeViewArboParent.Name = MyChantier.C_ListForages(element).NomForage
        '                TreeViewArboRoot.Nodes.Add(TreeViewArboParent)
        '                MyForage.RefForage = MyChantier.C_ListForages(element).RefForage
        '                MyForage.NomForage = MyChantier.C_ListForages(element).NomForage
        '                MyForage.F_RefChantier = MyChantier.C_RefChantier
        '                'Call  C_ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)

        '                ' Appeler la méthode AfficheDansArbo de l'objet Forage
        '                MyChantier.C_ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)
        '            End If
        '        Next
        '    End If

        '    ' Développer tous les noeuds
        '    Arbo.ExpandAll()
        'End Sub

        ' Affiche les modifications dans la DataGrid)
        ' Public Sub AfficheDansTab(ByRef Table As DataGridView, ByVal FichierChantier As String)
        '        Public Sub AfficheDansTab(ByRef DataGrid As DataGridView, ByRef BindingData As BindingSource, ByVal FichierChantier As String)
        '            Dim i As Integer
        '            'Dim fonts As New MyFontList()
        '            Dim ForageList As MyForageList

        '            On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            ' affectation de la bindingdata à la Listbox
        '            With DataGrid
        '                .DataSource = MonChantier.Tables("LesForages")
        '            End With

        '            Exit Sub
        'errFind:
        '            ' Chargement de l'objet d'erreur
        '            Select Case Lang
        '                Case 0
        '                    MsgBox(" Error during loading of borehole " & C_ListForages(i).GetRefForage & " ", Err.Description)
        '                Case 1
        '                    MsgBox(" Erreur lors de l'ouverture du forage " & C_ListForages(i).GetRefForage & " ", Err.Description)
        '            End Select
        '            Resume Next

        '        End Sub

        'Public Sub Show()
        '    MyFenetreChantier.Show()
        'End Sub

        'Public Sub MaskChantier()
        '    'Unload(MyFenetreChantier)
        '    MyFenetreChantier?.Close()
        'End Sub

        Public Sub CloseChantier()

            'Call MaskChantier()
            Dim i As Integer
            If C_ListForages IsNot Nothing Then
                For i = 0 To C_ListForages.Count - 1
                    'C_ListForages(i).CloseForage()
                    C_ListForages(i) = Nothing
                Next
            End If
            'C_ListForages.Count = 0
        End Sub

        'Public Sub NouveauForage(ByVal NouvForage As Boolean, ByVal NewTable As Boolean)

        '    ' Déclaration d'un Nouveau Forage
        '    Dim NewForage As Forage

        '    NewForage = Nothing

        '    If NouvForage Then
        '        ' Incrémentation du compteur de forages
        '        MyChantier.C_ListForages.Count = MyChantier.C_ListForages.Count + 1
        '    End If

        '    If MonChantier.Tables("LesForages") Is Nothing Then
        '        NewTable = True
        '    End If

        '    ' Redimensionnement du tableau dynamique de forages
        '    'ReDim Preserve MyChantier. C_ListForages( C_ListForages.Count - 1)

        '    ' On ajoute un Nouveau Forage à la Liste des Forages de MyChantier
        '    Dim nouveauForage As New Forage
        '    MyChantier.AddForage(nouveauForage, C_ListForages.Count)
        '    ' Instanciation du nouveau forage
        '    If NewForage Is Nothing Then

        '        ' NewForage = nouveauForage
        '        If MyChantier.C_ListForages.Count > 0 And (MyChantier.C_ListForages(C_ListForages.Count - 1) IsNot Nothing) Then
        '            NewForage = MyChantier.C_ListForages(C_ListForages.Count - 1)
        '        Else
        '            NewForage = nouveauForage
        '            'C_ListForages.Count = C_ListForages.Count + 1
        '        End If
        '    End If

        '    ' Enregistrement du nouveau forage dans le tableau de forage
        '    MyChantier.C_ListForages(C_ListForages.Count - 1) = NewForage

        '    If NewTable Then  ' On ajoute un Forage à MonFichier.Tables("LesForage") seulement si Table est vrai
        '        If MonChantier.Tables("LesForages") Is Nothing Then
        '            MonChantier.Tables.Add("LesForages")
        '            With MonChantier.Tables("LesForages")

        '                Dim columns() As DataColumn
        '                ReDim columns(12)

        '                .Columns.Add(" refChantier", GetType(String))
        '                .Columns.Add(" refForage", GetType(String))
        '                columns(1) = .Columns(" refForage")
        '                .PrimaryKey = columns
        '                .Columns.Add(" NumeroForage", GetType(Integer))
        '                .Columns.Add(" NomForage", GetType(String))
        '                .Columns.Add(" dateForage", GetType(Date))
        '                .Columns.Add(" materiel", GetType(String))
        '                .Columns.Add(" outil", GetType(String))
        '                .Columns.Add(" abscisseX", GetType(Single))
        '                .Columns.Add(" ordonneeY", GetType(Single))
        '                .Columns.Add(" altitudeZ", GetType(Single))
        '                .Columns.Add(" coteInit", GetType(Single))
        '                .Columns.Add(" coteFinale", GetType(Single))

        '            End With
        '        Else
        '            MonChantier.Tables("LesForages").NewRow()
        '        End If
        '    End If

        '    'MyChantier. ListFyorages( C_ListForages.Count - 1) = NewForage
        '    ' Attribution d'une référence au nouveau forage
        '    NewForage.SetRefForage(GiveMeANewRefForage)

        '    ' Appel de la procédure indiquant que des modifications ont eu lieu

        '    ' Affectation du nouveau forage à MyForage
        '    MyForage = NewForage

        '    Call IHaveBeenModified()

        'End Sub

        Public Sub SupForage(ByVal Ind As Integer)
            Dim J As Integer

            'i = GetIndRefForage(ref)

            ' Effaçage du forage

            C_ListForages(Ind) = Nothing

            ' Décalage des forages suivant d'une case
            For J = Ind To C_ListForages.Count - 1
                C_ListForages(J) = C_ListForages(J + 1)
            Next

            ' Redimensionnement du tableau  C_ListForages
            If C_ListForages.Count > 1 Then
                'ReDim Preserve C_ListForages(C_ListForages.Count - 1)
                ' C_ListForages.RemoveAt( C_ListForages.Count - 1)
            Else
                'ReDim C_ListForages(0)
                ' C_ListForages.RemoveAt(0)
            End If

            ' Décrémentation du compteur de forages
            'C_ListForages.Count = C_ListForages.Count - 1

            ' Nouveau tableau à afficher
            'MyFenetreChantier.Close()
            'MyFenetreChantier.Show()

            ' Appel de la procédure indiquant que des modifications ont eu lieu
            Call IHaveBeenModified()

        End Sub

        Public Function GetIndexNomForage(ByVal Nom As String) As Integer
            ' Recherche du forage ayant pour référence 'ref' avec arrêt dès que possible
            Dim i As Integer
            i = 0
            GetIndexNomForage = 0
            '    MsgBox "GetIndexForage"
            If Nom = "" Then Exit Function
            Do While ((i < (MyChantier.C_ListForages.Count) - 1) And (MyChantier.C_ListForages(i).F_NomForage <> Nom))
                i = i + 1
            Loop
            GetIndexNomForage = i
        End Function

        Public Function GetIndexRefForage(ByVal ref As String) As Integer
            ' Recherche du forage ayant pour référence 'ref' avec arrêt dès que possible
            Dim i As Integer
            i = 0
            GetIndexRefForage = 0
            '    MsgBox "GetIndexForage"
            If ref = "" Then Exit Function
            Do While ((i < MyChantier.C_ListForages.Count) And (C_ListForages(i).GetRefForage <> ref))
                'Do While (i <  C_ListForages.Count) And (MyChantier.GetForage(i).GetRefForage <> ref)
                i = i + 1
            Loop
            GetIndexRefForage = i
        End Function

        Public Function GetIndexRefEssai(ByVal refForage As String, ByVal refEssai As String) As Integer
            ' Recherche du forage ayant pour référence 'ref' avec arrêt dès que possible
            Dim iForage, iEssai As Integer
            iForage = 0
            iEssai = 0
            GetIndexRefEssai = 0
            '    MsgBox "GetIndexForage"
            If refForage = "" Then Exit Function
            If refEssai = "" Then Exit Function

            ' Recherche du Forage dans la Table des forages
            Do While ((iForage < C_ListForages.Count) And (C_ListForages(iForage).GetRefForage <> refForage))
                iForage = iForage + 1
            Loop
            If iForage = C_ListForages.Count Then Exit Function

            ' Recherche de l'Essai dans la table des Essais
            Do While ((iEssai < C_ListForages(iForage).F_ListEssais.Count) And (C_ListForages(iForage).F_ListEssais(iEssai).E_RefEssai <> refEssai))
                iEssai = iEssai + 1
            Loop
            GetIndexRefEssai = iEssai
        End Function

        'Public Sub ShowForage(ByVal i As Integer)
        '    MyForage = C_ListForages(i)
        '    MyChantier.refForage = MyForage.F_RefForage
        '    C_ListForages(i).Show(MyForage)
        '    'FrmForage.Show()
        'End Sub

        'Public Sub MaskForage(ByVal ref As String)
        '    Dim i As Integer    ' Index du forage à fermer
        '    i = GetIndexRefForage(ref)
        '    C_ListForages(i).MaskForage()
        'End Sub

        Public Sub SuspendreEvents()
            C_EventsSupendus = True
        End Sub
        Public Sub RetablirEvents()
            C_EventsSupendus = False
        End Sub

        ' Création d'un nouveau fichier à partir de MyChantier
        Public Function NouveauFichier(FichierChantier As String) As Boolean
            Dim NomFichierInitial As String
            Dim Racine As String
            NouveauFichier = False
            Racine = AppDomain.CurrentDomain.BaseDirectory
            Racine = System.IO.Path.GetFullPath(System.IO.Path.Combine(Racine, "..\..\..\.."))
            FichierChantier = Racine & "\Chantiers\" & FichierChantier & ".mdb"
            'NomFichierInitial = FichierChantierSousDossier(FichierChantier, "Chantier", "Ressources", ".mdb")
            Racine = AppDomain.CurrentDomain.BaseDirectory
            Racine = System.IO.Path.GetFullPath(System.IO.Path.Combine(Racine, "..\..\..\.."))
            NomFichierInitial = Racine & "\Ressources\Chantier.mdb"
            Call FileCopy(NomFichierInitial, FichierChantier)
            NouveauFichier = True
        End Function

        '        Public Function EnregistreChantier(FichierChantier As String, ByVal Nouveau As Boolean) As Boolean
        '            On Error GoTo errFind

        '            Dim RowNumber As Integer
        '            Dim Requete, refForage As String
        '            Dim MonModifChantier As DataSet
        '            Dim i, jMax, kMax, NbAcces, NbTable, iTable As Integer
        '            Dim Essai As Essai_Forage
        '            Dim row As DataRow
        '            Dim RowEssai As Essai_Forage
        '            Dim Found As Boolean
        '            Dim refEssais As New Dictionary(Of String, Boolean)

        '            On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            ' On crée une commande Builder pour renvoyer les données à la BD

        '            On Error GoTo errFind

        '            ' Ouverture de la console
        '            ' System.Console.Title = "Debug Console"

        '            ' Ouverture de la connection avec la BD

        '            ' *********************************************************************
        '            ' Enregistrement de la Table "LesChantiers"
        '            ' *********************************************************************

        '            ''ma requete
        '            ' exemple plasserre.developpez.com/cours/vb-net : strSql = "SELECT FICHEPATIENT.* FROM FICHEPATIENT"
        '            'Requete = "SELECT refChantier AS  refChantier, NomChantier AS  NomChantier, descriptifChantier as  descriptifChantier, refClient as  refClient, entreprise as  entreprise, adresse as  adresse , telephone as  telephone, reference as  reference FROM LesChantiers"
        '            Requete = "SELECT refChantier , NomChantier , descriptifChantier , refClient , entreprise , adresse , telephone , reference FROM LesChantiers"

        '            ' Définition de l'AdaptateurChantier

        '            iTable = 0

        '            ' definition de la commande

        '            iTable = 0
        '            ObjetDataChantier = MonChantier.Tables("LesChantiers")

        '            ' Définition de l'AdaptateurChantier

        '            'Pour modifier les valeurs changées dans le DataAdapter

        '            columns(0) = MonChantier.Tables("LesChantiers").Columns("refChantier")
        '            MonChantier.Tables("LesChantiers").PrimaryKey = columns

        '            iTable = 0
        '            ObjetDataChantier = MonChantier.Tables("LesChantiers")

        '            'C_ChantierCount = MonChantier.Tables("LesChantiers").Rows.Count
        '            'If C_ChantierInitCount < C_ChantierCount Then
        '            '    ' On ajoute un enregistrement à la Table
        '            'End If

        '            ' Définition de la clef primaire de la Table "LesChantiers"
        '            Dim columns() As DataColumn
        '            ReDim columns(7)

        '            columns(0) = MonChantier.Tables("LesChantiers").Columns("refChantier")
        '            MonChantier.Tables("LesChantiers").PrimaryKey = columns

        '            If MonChantier.Tables("LesChantiers") IsNot Nothing Then
        '                'AdaptateurChantier.Update(MonChantier, "LesChantiers")
        '                'ChantierInitCount = MonChantier.Tables("LesChantiers").Rows.Count

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonChantier.Tables("LesChantiers").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "LesChantiers")

        '                ' Mise à jour de MyChantier
        '                If MonChantier.Tables("LesChantiers").Rows.Count > 0 Then

        '                    MyChantier.C_refChantier = MonChantier.Tables("LesChantiers").Rows(0).Item("refChantier").ToString

        '                    'AdaptateurChantier.Update(MonChantier, "LesChantiers")

        '                    '' On vide la Table et on la remplit à nouveau
        '                    'MonChantier.Tables("LesChantiers").Clear()
        '                    'AdaptateurChantier.Fill(MonChantier, "LesChantiers")
        '                End If

        '                MsgBox("Update -LesChantiers- successful")
        '            End If
        '            ' *********************************************************************
        '            ' Enregistrement de la Table "LesForages"
        '            ' *********************************************************************

        '            'ma requete
        '            'Requete = "SELECT refChantier AS  refChantier, refForage as  refForage, NumeroForage as  NumeroForage, NomForage as  NomForage, " _
        '            '       & "dateForage as  dateForage, materiel as  materiel, outil as  outil, abscisseX as  abscisseX, ordonneeY as  ordonneeY, altitudeZ as  altitudeZ, " _
        '            '       & "coteInit as  coteInit, coteFinale as  coteFinale FROM LesForages"
        '            Requete = "SELECT refChantier , refForage , NumeroForage , NomForage , " &
        '                "dateForage , materiel , outil , abscisseX , ordonneeY , altitudeZ , " &
        '                "coteInit , coteFinale  FROM LesForages"

        '            iTable = 1
        '            'definition de la commande

        '            ' Définition de l'AdaptateurChantier

        '            ' Définition de l'AdaptateurChantier

        '            ' Définition de la clef primaire de la Table "LesForages"
        '            ReDim columns(0)

        '            columns(0) = MonChantier.Tables("LesForages").Columns("refForage")
        '            MonChantier.Tables("LesForages").PrimaryKey = columns

        '            '' Remise à jour de la Table "LesForages"
        '            'FrmForage.UpDateLesForages(MyChantier.refForage)

        '            'Pour modifier les valeurs changées dans le DataAdapter

        '            ' Adaptation du fichier Accès à la dimension de la Table
        '            NbAcces = MonChantier.Tables("LesForages").Rows.Count


        '            ' Mise à jour de la Base de données Access 
        '            iTable = 1
        '            ObjetDataForage = MonChantier.Tables("LesForages")

        '            For Each row In MonChantier.Tables("LesForages").Rows
        '                Debug.WriteLine("Row State: " & row.RowState.ToString())
        '            Next

        '            If MonChantier.Tables("LesForages") IsNot Nothing Then
        '                'C_ListForages.Count = MonChantier.Tables("LesForages").Rows.Count
        '                'If C_ForageInitCount < C_ListForages.Count Then
        '                ' On ajoute un enregistrement à la Table
        '                'End If

        '                '' Définition de la clef primaire de la Table "LesForages"
        '                'columns(0) = New DataColumn
        '                'columns(0) = MonChantier.Tables("LesForages").Columns("refForage")
        '                'MonChantier.Tables("LesForages").PrimaryKey = columns

        '                ' Mettre à jour la base de données
        '                'AdaptateurChantier.Update(MonChantier, "LesForages")
        '                'MsgBox("Update -LesForages- successful")

        '                '' Recharger la table après la mise à jour
        '                'MonChantier.Tables("LesForages").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "LesForages")
        '            End If

        '            ' *********************************************************************
        '            ' Enregistrement de la Table "LesEssaisForage"
        '            ' *********************************************************************
        '            iTable = 2
        '            ObjetDataEssai = MonChantier.Tables("LesEssaisForage")

        '            ' Mise à jour de la Table "LesEssaisForage"
        '            ''ma requete

        '            'Requete = "SELECT  refChantier,  refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, ClassPrecis, PrecisVolSonde, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " _
        '            '   & "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " _
        '            '   & "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaisForage"
        '            Requete = "SELECT refChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, ClassPrecis, PrecisVolSonde, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                    "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                    "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaisForage"

        '            iTable = 2
        '            ' Vérification de la Table
        '            With MonChantier.Tables("LesEssaisForage")
        '                For i = 0 To .Rows.Count - 1

        '                    row = MonChantier.Tables("LesEssaisForage").Rows(i)
        '                    'Debug.WriteLine("refEssai = " & row("refEssai").ToString)
        '                    'Debug.WriteLine("refChantier = " & row("refChantier").ToString)
        '                    'Debug.WriteLine("refForage = " & row("refForage").ToString)
        '                    'Debug.WriteLine("refSonde = " & row("refSonde").ToString)
        '                    'Debug.WriteLine("refEssaiResistancePropre = " & row("refEssaiResistancePropre").ToString)
        '                    'Debug.WriteLine("refEssaiExpansionPropre = " & row("refEssaiExpansionPropre").ToString)
        '                    'Debug.WriteLine("refEssaiPerteDeCharge = " & row("refEssaiPerteDeCharge").ToString)
        '                    'Debug.WriteLine("refSol = " & row("refSol").ToString)
        '                    'Debug.WriteLine("Enregistreur = " & row("Enregistreur").ToString)
        '                    'Debug.WriteLine("Operateur = " & row("Operateur").ToString)
        '                    'Debug.WriteLine("VolSonde = " & row("VolSonde").ToString)
        '                    'Debug.WriteLine("ClassPrecis = " & row("ClassPrecis").ToString)
        '                    'Debug.WriteLine("PrecisVolSonde = " & row("PrecisVolSonde").ToString)
        '                    'Debug.WriteLine("dateEssai = " & row("dateEssai").ToString)
        '                    'Debug.WriteLine("heureEssai = " & row("heureEssai").ToString)
        '                    'Debug.WriteLine("difPresEauAir = " & row("difPresEauAir").ToString)
        '                    'Debug.WriteLine("hauteurCpv = " & row("hauteurCpv").ToString)
        '                    'Debug.WriteLine("presP0 = " & row("presP0").ToString)
        '                    'Debug.WriteLine("presFluage = " & row("presFluage").ToString)
        '                    'Debug.WriteLine("presLimite = " & row("presLimite").ToString)
        '                    'Debug.WriteLine("presLimiteNf = " & row("presLimiteNf").ToString)
        '                    'Debug.WriteLine("presLimiteTheo = " & row("presLimiteTheo").ToString)
        '                    'Debug.WriteLine("PrecisVolume = " & row("PrecisVolume").ToString)
        '                    'Debug.WriteLine("PrecisPression = " & row("PrecisPression").ToString)
        '                    'Debug.WriteLine("poidsVolDejauge = " & row("poidsVolDejauge").ToString)
        '                    'Debug.WriteLine("coefPoisson = " & row("coefPoisson").ToString)
        '                    'Debug.WriteLine("modPressio = " & row("modPressio").ToString)
        '                    'Debug.WriteLine("modPressioNf = " & row("modElastique").ToString)
        '                    'Debug.WriteLine("modElastique = " & row("refEssai").ToString)
        '                    'Debug.WriteLine("modElastiqueNf = " & row("modElastiqueNf").ToString)
        '                    'Debug.WriteLine("cohesion = " & row("cohesion").ToString)
        '                    'Debug.WriteLine("frottement = " & row("frottement").ToString)
        '                    'Debug.WriteLine("frottementintergranulaire = " & row("frottementintergranulaire").ToString)
        '                    'Debug.WriteLine("U0 = " & row("U0").ToString)
        '                    'Debug.WriteLine("profNappe = " & row("profNappe").ToString)
        '                    'Debug.WriteLine("profSonde = " & row("profSonde").ToString)
        '                    'Debug.WriteLine("coteZeroGeomeca = " & row("coteZeroGeomeca").ToString)
        '                Next i
        '            End With

        '            'definition de la commande

        '            ' Définition de l'AdaptateurChantier

        '            'definition de la commande

        '            ' Définition de l'AdaptateurChantier

        '            ' Définition de l'AdaptateurChantier

        '            ' Définition de la clef primaire de la Table "LesEssaisForage"
        '            ReDim columns(11)
        '            columns(0) = New DataColumn
        '            columns(0) = MonChantier.Tables("LesEssaisForage").Columns(2)
        '            MonChantier.Tables("LesEssaisForage").PrimaryKey = columns

        '            '' Remise à jour de la Table "LesEssaisForage"
        '            'FrmForage.UpDateLesForages(MyChantier.refForage)

        '            'Pour modifier les valeurs changées dans le DataAdapter

        '            ' Adaptation du fichier Accès à la dimension de la Table
        '            NbAcces = MonChantier.Tables("LesEssaisForage").Rows.Count

        '            MsgBox("Update -LesEssaisForage- successful")
        '            'C_EssaiInitCount = MonChantier.Tables("LesEssaisForage").Rows.Count


        '            ' Mise à jour de la Base de données Access 
        '            iTable = 1
        '            ObjetDataForage = MonChantier.Tables("LesEssaisForage")

        '            If MonChantier.Tables("LesEssaisForage") IsNot Nothing Then
        '                'C_ListForages.Count = MonChantier.Tables("LesEssaisForage").Rows.Count
        '                'If C_ForageInitCount < C_ListForages.Count Then
        '                '    ' On ajoute un enregistrement à la Table
        '                'End If

        '                ' Définition de la clef primaire de la Table "LesEssaisForage"
        '                columns(0) = New DataColumn
        '                columns(0) = MonChantier.Tables("LesEssaisForage").Columns("refEssai")
        '                MonChantier.Tables("LesEssaisForage").PrimaryKey = columns

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonChantier.Tables("LesEssaisForage").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "LesEssaisForage")

        '                'AdaptateurChantier.Update(MonChantier, "LesEssaisForage")
        '                'MsgBox("Update -LesEssaisForage- successful")
        '                'ForageInitCount = MonChantier.Tables("LesEssaisForage").Rows.Count

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonChantier.Tables("LesEssaisForage").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "LesEssaisForage")
        '            End If


        '            ' *********************************************************************
        '            ' Enregistrement de la Table "LesMesuresForage"
        '            ' *********************************************************************

        '            With MonChantier.Tables("LesMesuresForage")
        '                For i = 0 To .Rows.Count - 1

        '                    row = MonChantier.Tables("LesMesuresForage").Rows(i)
        '                    'Debug.WriteLine("refEssai = " & row("refEssai").ToString)
        '                    'Debug.WriteLine("indiceMesure = " & row("indiceMesure").ToString)
        '                    'Debug.WriteLine("pression = " & row("pression").ToString)
        '                    'Debug.WriteLine("volume15 = " & row("volume15").ToString)
        '                    'Debug.WriteLine("volume30 = " & row("volume30").ToString)
        '                    'Debug.WriteLine("volume60 = " & row("volume60").ToString)
        '                    'Debug.WriteLine("contrainteRadiale = " & row("contrainteRadiale").ToString)
        '                    'Debug.WriteLine("contrainteVerticale = " & row("contrainteVerticale").ToString)
        '                    'Debug.WriteLine("deformation = " & row("deformation").ToString)
        '                    'Debug.WriteLine("PNF = " & row("PNF").ToString)
        '                    'Debug.WriteLine("VNF = " & row("VNF").ToString)

        '                Next i
        '            End With

        '            ' Lecture des mesures de l'essai
        '            Requete = "SELECT Ind, refEssai, indiceMesure, pression, volume15, volume30, volume60, contrainteRadiale, contrainteVerticale, deformation, PNF, VNF, Qi, Pi, Qp, Pp FROM LesMesuresForage"

        '            iTable = 3
        '            'definition de la commande
        '            'AdaptateurChantier.SelectCommand = Commande

        '            ' Définition de l'AdaptateurChantier

        '            '' Remise à jour de la Table "LesMesuresForage"
        '            'FrmEssai.UpDateLesMesuresForages(MyEssai.RefEssai)

        '            ' 1. Définition de la clef primaire de la Table "LesMesuresForage" une seule fois
        '            ReDim columns(35)
        '            columns(0) = New DataColumn

        '            columns(0) = MonChantier.Tables("LesMesuresForage").Columns("Ind") ' Exemple : colonne "Ind" comme clé primaire
        '            MonChantier.Tables("LesMesuresForage").PrimaryKey = columns

        '            ''Pour modifier les valeurs changées dans le DataAdapter

        '            ' 2. Créer l'OleDbCommandBuilder et l'adaptateur

        '            If MonChantier.Tables("LesMesuresForage") IsNot Nothing Then

        '                ' Adaptation du fichier Accès à la dimension de la Table
        '                NbAcces = MonChantier.Tables("LesMesuresForage").Rows.Count

        '                'AdaptateurChantier.AcceptChangesDuringUpdate = True

        '                '' Mise à jour de la Base de données Access 
        '                'iTable = 3
        '                'ObjetDataMesures = MonChantier.Tables("LesMesuresForage")

        '                '' 3. Mettre à jour la base de données AVANT de vider ou recharger
        '                'AdaptateurChantier.Update(MonChantier, "LesMesuresForage")

        '                'R_Mesures.count = MonChantier.Tables("LesMesuresForage").Rows.Count
        '                'If C_MesureInitCount < C_R_Mesures.count Then
        '                '    ' On ajoute un enregistrement à la Table
        '                'End If


        '                'AdaptateurChantier.Update(MonChantier, "LesMesuresForage")
        '                'MsgBox("Update -LesMesuresForage- successful")
        '                'MesureInitCount = MonChantier.Tables("LesMesuresForage").Rows.Count


        '                'AdaptateurChantier.Fill(MonChantier, "LesMesuresForage")

        '                ' 4. Optionnel : recharger la table après la mise à jour
        '                MonChantier.Tables("LesMesuresForage").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "LesMesuresForage")

        '                'AdaptateurChantier.Update(MonChantier, "LesMesuresForage")
        '                'MsgBox("Update -LesMesuresForage- successful")
        '                ' MesureInitCount = MonChantier.Tables("LesMesuresForage").Rows.Count

        '                ''MonChantier.Tables(3).Clear()
        '                ''AdaptateurChantier.Fill(MonChantier, "LesMesuresForage")

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonChantier.Tables("LesMesuresForage").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "LesMesuresForage")
        '            End If

        '            C_IsSaved = True
        '            EnregistreChantier = True


        '            RaiseEvent HasBeenModified()
        '            Exit Function

        'errFind:

        '            ' En cas d'erreur on édite la Table
        '            EnregistreChantier = False
        '            MsgBox(Err.Description)
        '            'Dim Message As String
        '            'Debug.Print(Message)
        '            ''MsgBox(Erreur.Message)
        '            'Select Case iTable
        '            '    'Case 0 : FrmTable = MyChantier.
        '            '    Case 1 : FrmTable = MyChantier. C_ListForages
        '            '    Case 2
        '            'End Select
        '            'FrmTable = New FrmTable
        '            'FrmTable.ShowDialog()
        '        End Function

        'Public Sub New()
        '    ' Enregistrement de la référence du chantier auprès de sa feuille
        '    'Call MyFenetreChantier.SetMyChantier(Me)
        '    ' MyChantier = Me

        '     NomChantier = "Nouveau chantier"
        '     refChantier = "à remplir"
        '     descriptifChantier = "à remplir"
        '     refClient = "à remplir"
        '     entreprise = "à remplir"
        '     adresse = "à remplir"
        '     telephone = "à remplir"
        '    IsSaved = True
        '    MyFile = ""
        '    MyFileTitle = "Chantier"

        'End Sub



        'Public Sub SelectMyNode()
        '    MyNodeChantier.Checked = True
        'End Sub

        Public Function GiveMeANewRefForage()
            ' Détermination d'une nouvelle référence pour un Forage
            Dim k As Integer

            '    MsgBox "GiveMeANewRefForage"

            k = 1

            Do While Not MyChantier.IsRefUnique(" refForage " & k)
                k = k + 1
            Loop
            GiveMeANewRefForage = " refForage " & k

        End Function

        Public Function IsRefUnique(ByVal ref As String) As Boolean
            ' Vérification de l'unicité d'une référence
            ' Donne une référence unique au nouveau forage
            Dim i As Integer
            Dim unique As Boolean

            '    MsgBox "IsRefUnique"

            unique = True

            ' i est le Numéro du Forage à vérifier
            Do While (i < C_ListForages.Count) And unique
                unique = (C_ListForages(i).GetRefForage <> ref)
                If unique Then Exit Do
                'k = MyChantier. C_ListForages(i).GetEssaiCount
                'J = 0  ' J est le numéro de l'essai dans le forage i
                'Do While J < k And unique
                '    unique = (MyChantier. C_ListForages(i).GetEssai(J).RefEssai <> ref)
                '    J = J + 1
                'Loop
                i = i + 1
            Loop
            If MyForage Is Nothing Then
                MyForage = New Forage
                'ReDim Preserve MyForage.F_ListEssais(i)
                MyForage = MyChantier.C_ListForages(i)
            End If

            IsRefUnique = unique
        End Function


    End Class


    Public Class MyForageList
        Inherits BindingList(Of Forage)
        Public Ind As Integer
        'Public C_ListForages.Count As Integer

        Public Sub New()
            MyListeForages = New List(Of Forage)() ' Initialisation
        End Sub

        Protected Overrides ReadOnly Property SupportsSearchingCore() As Boolean
            Get
                Return True
            End Get
        End Property

        Protected Overrides Function FindCore(ByVal prop As PropertyDescriptor,
    ByVal key As Object) As Integer
            ' Ignore the prop value and search by family name.
            While Ind < MyChantier.C_ListForages.Count
                If Items(Ind).GetNomForage = CStr(key).ToLower() Then
                    Return Ind
                End If
                Ind += 1
            End While

            Return -1
        End Function


    End Class

        'Public Class MyFontList
        '    Inherits BindingList(Of Microsoft.office.interop.excel.Font)

        '    Protected Overrides ReadOnly Property SupportsSearchingCore() As Boolean
        '        Get
        '            Return True
        '        End Get
        '    End Property

        '    Protected Overrides Function FindCore(ByVal prop As PropertyDescriptor, _
        '        ByVal key As Object) As Integer
        '        ' Ignore the prop value and search by family name.
        '        Dim i As Integer
        '        While i < Count
        '            If Items(i).FontFamily.Name.ToLower() = CStr(key).ToLower() Then
        '                Return i
        '            End If
        '            i += 1
        '        End While

        '        Return -1
        '    End Function


        'End Class

        'Public Class MyFontList
        '    Inherits BindingList(Of Microsoft.Office.Interop.Excel.Font)

        '    Protected Overrides Function SupportsSearchingCore() As Boolean
        '        Return True
        '    End Function

        '    Protected Overrides Function FindCore(ByVal prop As PropertyDescriptor, ByVal key As Object) As Integer
        '        ' Ignore the prop value and search by family name.
        '        Dim fontKey As String = CStr(key).ToLower()

        '        For i As Integer = 0 To Count - 1
        '            Dim fontFamilyName As String = Items(i).Name.ToLower()
        '            If fontFamilyName = fontKey Then
        '                Return i
        '            End If
        '        Next

        '        Return -1 ' Retourne -1 si non trouvé
        '    End Function
        'End Class

    End Namespace

