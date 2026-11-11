Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports GaiaPressCoreV17.Logic
Imports GaiaPressCoreV17.Services
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Models

    Public Class ConstructionSite

        ' Propriétés métiers (avec préfixe  )
        <Key>       ' Column("RefChantier")
        Public Property RefChantier As String

        ' Column("NomConstructionSite")
        Public Property NameConstructionSite As String

        ' Column("DescriptionConstructionSite")
        Public Property DescriptionConstructionSite As String
        Public Property DateConstructionSite As DateTime
        Public Property RefClient As String
        Public Property Entreprise As String
        Public Property Adresse As String
        Public Property Telephone As String
        Public Property Reference As String

        Public FichierConstructionSite As String
        Public MyFenetreConstructionSite As ConstructionSiteLogic
        Public MyFile As String
        Public MyFileTitle As String
        Public DataSetName As String

        ' Collections (remplacez les tableaux par des List)
        Public Property ListForages As New List(Of Forage)
        Public Property ListEssaiForages As New List(Of Essai_Forage)
        Public Property ListMesuresForage As New List(Of MesureEssaiForage)
        Public Property ListSols As New List(Of Sol)
        Public Property ListSondes As New List(Of Probe)

        ' Propriétés de gestion (optionnel, avec préfixe  )
        Public Property IsSaved As Boolean = False
        Public Property FilePath As String
        Public Property FileTitle As String

        Public Property EventsSupendus As Boolean

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications du ConstructionSite ont eu lieu
            If Not EventsSupendus Then
                RaiseEvent HasBeenModified()
                IsSaved = False
            End If
        End Sub

        ' ===============================================================================
        ' Gestion de   ListForages
        ' ===============================================================================

        Public Sub AddForage(forage As Forage)
            Me.ListForages.Add(forage)
            ' Lier le forage au ConstructionSite (si nécessaire)
            forage.RefChantier = Me.RefChantier
        End Sub

        ' Delete un forage par référence
        Public Sub DeleteForage(refForage As String)
            Dim forageADelete As Forage = GetForageParRef(refForage)
            If forageADelete IsNot Nothing Then
                Me.ListForages.Remove(forageADelete)
            End If
        End Sub


        Public Function GetForageParRef(refForage As String) As Forage
            For Each f As Forage In ListForages
                If f.RefForage = refForage Then
                    Return f
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Met à jour un forage dans la liste  ListForage, y compris ses objets complexes.
        ''' </summary>
        Public Sub MettreAJourForage(ForageModifie As Forage, context As GaiaPressDbContext)
            ' 1. Récupérer le forage existant depuis la base de données
            Dim forage As Forage = context.ListeForageDb.Find(ForageModifie.RefForage)

            If forage IsNot Nothing Then
                ' 2. Mise à jour des propriétés (SAUF la clé primaire !)
                forage.RefChantier = ForageModifie.RefChantier
                forage.numeroForage = ForageModifie.numeroForage
                forage.NomForage = ForageModifie.NomForage
                forage.dateForage = ForageModifie.dateForage
                forage.materiel = ForageModifie.materiel
                forage.outil = ForageModifie.outil
                forage.abscisseX = ForageModifie.abscisseX
                forage.ordonneeY = ForageModifie.ordonneeY
                forage.altitudeZ = ForageModifie.altitudeZ
                forage.coteInit = ForageModifie.coteInit
                forage.coteFinale = ForageModifie.coteFinale
                forage.EventsSuspendus = ForageModifie.EventsSuspendus

                ' 3. Sauvegarde en base de données
                context.SaveChanges()

                ' 4. Mise à jour de IsSaved (si nécessaire)
                forage.IsSaved = True
            Else
                Throw New Exception($"Forage avec RefForage={ForageModifie.RefForage} introuvable en base de données.")
            End If
        End Sub


        ' ===============================================================================
        ' Gestion de   ListEssaiForages
        ' ===============================================================================

        Public Sub AddEssaiForage(essai As Essai_Forage)
            Me.ListEssaiForages.Add(essai)
            ' Lier le forage au ConstructionSite (si nécessaire)
            essai.RefChantier = Me.RefChantier
        End Sub

        ' Delete un forage par référence
        Public Sub DeleteessaiForage(refessai As String)
            Dim EssaiADelete As Essai_Forage = GetessaiForage(refessai)
            If EssaiADelete IsNot Nothing Then
                Me.ListEssaiForages.Remove(EssaiADelete)
            End If
        End Sub

        Public Function GetessaiForage(refEssai As String) As Essai_Forage
            For Each E As Essai_Forage In Me.ListEssaiForages
                If E.RefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function


        ''' Met à jour un essai de forage avec toutes ses propriétés, y compris les objets complexes.
        Public Sub MettreAJourEssaiForage(EssaiModifie As Essai_Forage)

            Dim Essai As Essai_Forage = GetessaiForage(EssaiModifie.RefEssai)

            If EssaiModifie IsNot Nothing Then
                ' Mise à jour des objets complexes
                MySonde = EssaiModifie.MySonde
                MySol = EssaiModifie.MySol
                'MyForage = EssaiModifie.MyForage
                MyResistance = EssaiModifie.MaResistance
                MyExpansion = EssaiModifie.MaExpansion

                ' Mise à jour des propriétés simples
                Essai.ClassPrecis = EssaiModifie.ClassPrecis
                Essai.coefPoisson = EssaiModifie.coefPoisson
                Essai.coteZeroGeomeca = EssaiModifie.coteZeroGeomeca
                Essai.Cohesion = EssaiModifie.Cohesion
                Essai.dateEssai = EssaiModifie.dateEssai
                Essai.heureEssai = EssaiModifie.heureEssai
                Essai.difPresEauAir = EssaiModifie.difPresEauAir
                Essai.Enregistreur = EssaiModifie.Enregistreur
                Essai.Frottement = EssaiModifie.Frottement
                Essai.FrottementIntergranulaire = EssaiModifie.FrottementIntergranulaire
                Essai.hauteurCpv = EssaiModifie.hauteurCpv
                Essai.modElastique = EssaiModifie.modElastique
                Essai.modElastiqueNf = EssaiModifie.modElastiqueNf
                Essai.modPressio = EssaiModifie.modPressio
                Essai.modPressioNf = EssaiModifie.modPressioNf
                Essai.nomEssai = EssaiModifie.nomEssai
                Essai.Operateur = EssaiModifie.Operateur
                Essai.Pe = EssaiModifie.Pe
                Essai.Ie = EssaiModifie.Ie
                Essai.presFluage = EssaiModifie.presFluage
                Essai.presLimite = EssaiModifie.presLimite
                Essai.presLimiteNF = EssaiModifie.presLimiteNF
                Essai.presLimiteTheo = EssaiModifie.presLimiteTheo
                Essai.presP0 = EssaiModifie.presP0
                Essai.PrecisPression = EssaiModifie.PrecisPression
                Essai.PrecisVolume = EssaiModifie.PrecisVolume
                Essai.poidsVol = EssaiModifie.poidsVol
                Essai.poidsVolDejauge = EssaiModifie.poidsVolDejauge
                Essai.profNappe = EssaiModifie.profNappe
                Essai.profSonde = EssaiModifie.profSonde
                Essai.U0 = EssaiModifie.U0
                Essai.VolSonde = EssaiModifie.VolSonde
                Essai.PrecisVolSonde = EssaiModifie.PrecisVolSonde

                ' Mise à jour des références
                Essai.refSonde = EssaiModifie.refSonde
                Essai.refEssaiResistancePropre = EssaiModifie.refEssaiResistancePropre
                Essai.refEssaiExpansionPropre = EssaiModifie.refEssaiExpansionPropre
                Essai.refEssaiPerteDeCharge = EssaiModifie.refEssaiPerteDeCharge
                Essai.RefSol = EssaiModifie.RefSol
            End If
        End Sub


        ' ===============================================================================
        ' Gestion de   ListMesureEssaiForage
        ' ===============================================================================

        Public Sub AddMesureEssaiForage(essai As Essai_Forage)
            Me.ListEssaiForages.Add(essai)
            ' Lier le forage au ConstructionSite (si nécessaire)
            essai.RefChantier = Me.RefChantier
        End Sub

        ' Delete un forage par référence
        Public Sub DeleteMesureEssaiForage(refessai As String)
            Dim EssaiADelete As Essai_Forage = GetessaiForage(refessai)
            If EssaiADelete IsNot Nothing Then
                Me.ListEssaiForages.Remove(EssaiADelete)
            End If
        End Sub

        Public Function GetMesureEssaiForage(refEssai As String) As Essai_Forage
            For Each E As Essai_Forage In Me.ListEssaiForages
                If E.RefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        ''' <summary>
        ''' Met à jour une mesure d'essai de forage dans une liste de mesures.
        ''' </summary>
        Public Sub UpdateMesureEssaiForage(MesureModifiee As MesureEssaiForage)
            ' Get la mesure dans la liste (ex:  ListMesuresForage ou ObjetDataMesures)
            ' Exemple pour une liste dans un ConstructionSite :
            Dim mesure As MesureEssaiForage = ListMesuresForage.FirstOrDefault(
        Function(m) m.RefEssai = MesureModifiee.RefEssai AndAlso m.IndiceMesure = MesureModifiee.IndiceMesure
    )

            If mesure IsNot Nothing Then
                ' Mise à jour des propriétés de la mesure
                mesure.RefEssai = MesureModifiee.RefEssai
                mesure.IndiceMesure = MesureModifiee.IndiceMesure
                mesure.Pression = MesureModifiee.Pression
                mesure.Volume15 = MesureModifiee.Volume15
                mesure.Volume30 = MesureModifiee.Volume30
                mesure.Volume60 = MesureModifiee.Volume60
                mesure.ContrainteRadiale = MesureModifiee.ContrainteRadiale
                mesure.ContrainteVerticale = MesureModifiee.ContrainteVerticale
                mesure.Deformation = MesureModifiee.Deformation
                mesure.PNF = MesureModifiee.PNF
                mesure.VNF = MesureModifiee.VNF
                mesure.Qi = MesureModifiee.Qi
                mesure.Pi = MesureModifiee.Pi
                mesure.Qp = MesureModifiee.Qp
                mesure.Pp = MesureModifiee.Pp
            End If
        End Sub

        ' ===============================================================================
        ' Gestion de   ListSondes
        ' ===============================================================================
        ' Add une sonde
        Public Sub AddSonde(Sonde As Probe)
            Me.ListSondes.Add(Sonde)
        End Sub

        ' Get une Sonde
        Public Function GetSonde(RefSonde As String) As Probe
            For Each P As Probe In Me.ListSondes
                If RefSonde = RefSonde Then
                    Return P
                End If
            Next
            Return Nothing
        End Function

        ' Delete une Sonde par référence
        Public Sub DeleteSonde(refsonde As String)
            Dim SondeADelete As Probe = GetSonde(refsonde)
            If SondeADelete IsNot Nothing Then
                Me.ListSondes.Remove(SondeADelete)
            End If
        End Sub

        ' Mettre à jour une Sonde
        ''' <summary>
        ''' Met à jour une sonde dans la liste  ListSondes.
        ''' </summary>
        Public Sub MettreAJourSonde(NouvelleSonde As Probe)
            ' Get la sonde à modifier dans  ListSondes
            Dim SondeAModifier As Probe = GetSonde(NouvelleSonde.RefSonde)

            If SondeAModifier IsNot Nothing Then
                ' Mise à jour des propriétés de la sonde
                SondeAModifier.MaExpansion = NouvelleSonde.MaExpansion
                SondeAModifier.MaResistance = NouvelleSonde.MaResistance
                SondeAModifier.RefSonde = NouvelleSonde.RefSonde
                'SondeAModifier.NomSond = NouvelleSonde.NomSonde
                SondeAModifier.ClassPrecis = NouvelleSonde.ClassPrecis
                SondeAModifier.RefEssai = NouvelleSonde.RefEssai
                SondeAModifier.TypeSonde = NouvelleSonde.TypeSonde
                SondeAModifier.TypeDeformee = NouvelleSonde.TypeDeformee
                SondeAModifier.TypeContrainte = NouvelleSonde.TypeContrainte
                SondeAModifier.Ls = NouvelleSonde.Ls
                SondeAModifier.Lg = NouvelleSonde.Lg
                SondeAModifier.lt = NouvelleSonde.lt
                SondeAModifier.LongInjectePompage = NouvelleSonde.LongInjectePompage
                SondeAModifier.Vs = NouvelleSonde.Vs
                SondeAModifier.Ds = NouvelleSonde.Ds
                SondeAModifier.ClassPrecis = NouvelleSonde.ClassPrecis
                SondeAModifier.EpMembraneGarde = NouvelleSonde.EpMembraneGarde
                SondeAModifier.PrEpMembraneGarde = NouvelleSonde.PrEpMembraneGarde
                SondeAModifier.EpMembraneMesure = NouvelleSonde.EpMembraneMesure
                SondeAModifier.PrEpMembraneMesure = NouvelleSonde.PrEpMembraneMesure
                SondeAModifier.EpTubeFendu = NouvelleSonde.EpTubeFendu
                SondeAModifier.PrEpTubeFendu = NouvelleSonde.PrEpTubeFendu
                SondeAModifier.EpInitMembrane = NouvelleSonde.EpInitMembrane
                SondeAModifier.PrEpInitMembrane = NouvelleSonde.PrEpInitMembrane
                SondeAModifier.PrLong = NouvelleSonde.PrLong
                SondeAModifier.PrDiam = NouvelleSonde.PrDiam
                SondeAModifier.PrP = NouvelleSonde.PrP
                SondeAModifier.PrV = NouvelleSonde.PrV
                SondeAModifier.PrPV = NouvelleSonde.PrPV
                SondeAModifier.PrZ = NouvelleSonde.PrZ
                SondeAModifier.Prt = NouvelleSonde.Prt
                SondeAModifier.Pm = NouvelleSonde.Pm
            End If
        End Sub


        ' ==============================================================================
        ' Gestion de  ListSols
        ' ==============================================================================
        ' Add un sol
        Public Sub AddSol(NouveauSol As Sol)
            Me.ListSols.Add(NouveauSol)
        End Sub

        ' Get un sol
        Public Function GetSol(RefSol As String) As Sol
            For Each S As Sol In Me.ListSols
                If S.RefSol() = RefSol Then
                    Return S
                End If
            Next
            Return Nothing
        End Function

        ' Delete un Sol par référence
        Public Sub DeleteSol(refSol As String)
            Dim SolADelete As Sol = GetSol(refSol)
            If SolADelete IsNot Nothing Then
                Me.ListSols.Remove(SolADelete)
            End If
        End Sub

        ' ==============================================================================
        ' Mettre à jour un Sol
        ' ==============================================================================
        Public Sub MettreAJourSol(NouveauSol As Sol)
            Dim SolAModifier As Sol = GetSol(NouveauSol.RefSol)
            If SolAModifier IsNot Nothing Then
                ' Mise à jour des propriétés de la Sonde
                SolAModifier.RefSol = NouveauSol.RefSol
                'SolAModifier. NomSol=NouveauSol.GetNomSol
                ' ✅ Corrigé : Passer une instance de Sol
                SolAModifier.nomSol = SolAModifier.nomSol ' ou GetNomSol=NouveauSol
                SolAModifier.zoneGeo = NouveauSol.zoneGeo
                SolAModifier.famille = NouveauSol.famille
                SolAModifier.ClasseDuSol = NouveauSol.ClasseDuSol
                SolAModifier.SousClasse = NouveauSol.SousClasse
                SolAModifier.ClasseInférieure = NouveauSol.ClasseInférieure
                SolAModifier.SousClasseInférieure = NouveauSol.SousClasseInférieure
                SolAModifier.nomSolNormalise = NouveauSol.nomSolNormalise
                SolAModifier.Dmax = NouveauSol.Dmax
                SolAModifier.Tamisat50mm = NouveauSol.Tamisat50mm
                SolAModifier.Tamisat2mm = NouveauSol.Tamisat2mm
                SolAModifier.tamisat80microns = NouveauSol.tamisat80microns
                SolAModifier.VBS = NouveauSol.VBS
                SolAModifier.Ip = NouveauSol.Ip
                SolAModifier.Es = NouveauSol.Es
                SolAModifier.Phimu = NouveauSol.Phimu
            End If
        End Sub


        Public Event HasBeenModified()

        ' Constructeur de ConstructionSite
        Public Sub New()
            ' Initialisation des collections avec List(Of T)
            ListForages = New List(Of Forage)
            ListEssaiForages = New List(Of Essai_Forage)
            ListMesuresForage = New List(Of MesureEssaiForage)
            ListSols = New List(Of Sol)
            ListSondes = New List(Of Probe)

            ' Initialisation des propriétés par défaut
            FichierConstructionSite = "à remplir"
            RefChantier = "à remplir"
            NameConstructionSite = "Nouveau ConstructionSite"
            DescriptionConstructionSite = "à remplir"
            RefClient = "à remplir"
            Entreprise = "à remplir"
            Adresse = "à remplir"
            Telephone = "à remplir"
            Reference = "à remplir"

            ' Initialisation des autres propriétés
            MyFile = ""
            MyFileTitle = "à remplir"
            DataSetName = "à remplir"
            EventsSupendus = True

            ' ❌ Supprimez cette ligne (référence à Windows Forms)
            '  MyNodeConstructionSite = New TreeNode
        End Sub

        '        ' *******************************************************************************
        '        ' Lecture des ConstructionSite
        '        ' ************************************************************************
        '        Public Sub ChargeAvecBD(ByVal FichierConstructionSite As String)
        '            Dim RowNumber, i, j, k, kMax, kRef, l, m, n, IndSonde, IndEssai, Ind, IndMesures, IndexForage As Integer
        '            Dim Pres, V15, V30, V60, SigR, SigV, Defo, Pnf, Vnf, Qi, Pi, Qp, Pp, PressionNF, VolumeNF As Single
        '            Dim Requete, RefForage, RefEssai, RefChantier As String
        '            Dim Result, PasseForage, PasseEssai, PasseSonde As Boolean
        '            Dim refSondes As New Dictionary(Of String, Boolean)
        '            Dim RowSonde As Probe
        '            Dim NewForage As Forage
        '            Dim NewEssai As Essai_Forage
        '            Dim NewMesure As MesureEssaiForage
        '            Dim ReferenceSonde, Accu As String

        '            ' Dim Sonde, Sonde1, Sonde2 As Probe

        '            PasseForage = False
        '            PasseSonde = False

        '            On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            If MyConstructionSite Is Nothing Then MyConstructionSite = New ConstructionSite
        '            If MyForage Is Nothing Then MyForage = New Forage

        '            ' Appel de la procédure de connection à la base de données

        '            If Not (ChargeConstructionSite) Then

        '                ' Netoyage de l'AdaptateurConstructionSite
        '                ' AdaptateurConstructionSite.clear()

        '                ' Création d'une connection avec la base de données de type Access 2000

        '                ' Création d'une connection avec la base de données de type Access 2000

        '                ' Ouverture de la connection avec la BD

        '                ' *********************************************************************
        '                ' Lecture de la Table "ConstructionSite"
        '                ' *********************************************************************

        '                'ma requete
        '                'Requete = "SELECT RefChantier AS  RefChantier, NomConstructionSite AS  NomConstructionSite, DescriptifConstructionSite AS  DescriptifConstructionSite, " _
        '                '    & "RefClient AS  RefClient, Entreprise AS  Entreprise, Adresse AS  Adresse, Telephone AS  Telephone, Reference AS  Reference FROM ConstructionSite"
        '                Requete = "SELECT RefChantier , NomConstructionSite , DescriptifConstructionSite , " &
        '                          "RefClient , Entreprise , Adresse , Telephone , Reference  FROM ConstructionSite"

        '                'Requete = "SELECT * FROM ConstructionSite"

        '                'definition de la commande

        '                ' Définition de l'AdaptateurConstructionSite

        '                ' Nettoyage de MonConstructionSite avant le chargement
        '                MonConstructionSite.Tables("ConstructionSite")?.Clear()

        '                ' rempli le dataset depuis la BD vers la Table "ConstructionSite" de MonDataSet

        '                ' On remplit les ObjetTables avec les Tables de MonConstructionSite
        '                ObjetDataConstructionsSitesDb = MonConstructionSite.Tables("ConstructionSite")

        '                ' Mise à jour du compteur d'enregistrements

        '                'Me. ConstructionSite.Count = MonConstructionSite.Tables("ConstructionSite").Rows.Count

        '                Select Case Lang
        '                    Case 0
        '                        MsgBox("Reading of the Table 'ConstructionSite' suceeded")
        '                    Case 1
        '                        MsgBox("Lecture de la Table 'ConstructionSite' réussie")
        '                End Select
        '                Select Case Lang
        '                    Case 0
        '                        MsgBox("Reading of the Table 'ConstructionSite' suceeded")
        '                    Case 1
        '                        MsgBox("Lecture de la Table 'ConstructionSite' réussie")
        '                End Select

        '                ' *********************************************************************
        '                ' Lecture de la Table "LesForages"
        '                ' *********************************************************************
        '                'ma requete
        '                Requete = "SELECT RefChantier, refForage, NumeroForage, NomForage, dateForage, materiel, outil, abscisseX, ordonneeY, altitudeZ, coteInit, coteFinale FROM LesForages"

        '                'definition de la commande

        '                ' Définition de l'AdaptateurConstructionSite

        '                ' Nettoyage de MonConstructionSite avant le chargement
        '                MonConstructionSite.Tables("LesForages")?.Clear()

        '                ' rempli le dataset depuis la BD vers la Table "LesForages" de MonDataSet
        '                ObjetDataLesForages = MonConstructionSite.Tables("LesForages")

        '                ' La Table "LesForages" est chargée dans MonConstructionSite

        '                ' Mise à jour du compteur d'enregistrements
        '                ' ListForages.Count = MonConstructionSite.Tables("LesForages").Rows.Count
        '                ' ForageInitCount = MonConstructionSite.Tables("LesForages").Rows.Count
        '                'With MonConstructionSite.Tables("ConstructionSite").Rows(0)
        '                '     RefChantier = .Item("RefChantier").ToString
        '                '     NomConstructionSite = .Item("NomConstructionSite").ToString
        '                'End With

        '                ' refForage = MonConstructionSite.Tables("LesForages").Rows(0).Item("refForage")

        '                ' refForage = MonConstructionSite.Tables("LesForages").Rows(0).Item("refForage")

        '                Select Case Lang
        '                    Case 0
        '                        MsgBox("Reading of the Table 'LesForages' suceeded")
        '                    Case 1
        '                        MsgBox("Lecture de la Table 'LesForages' réussie")
        '                End Select

        '                ' *********************************************************************
        '                ' Lecture de la Table "LesEssaiForage"
        '                ' *********************************************************************
        '                'ma requete
        '                If Not (AncienConstructionSite) Then
        '                    'Requete = "SELECT  RefChantier,  refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, PrecisVolSonde, nomEssai, dateEssai, heureEssai,difPresEauAir, hauteurCpv, " _
        '                    '   & "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " _
        '                    '   & "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"

        '                    Requete = "SELECT RefChantier , refForage , refEssai , refSonde , refEssaiResistancePropre , refEssaiExpansionPropre , refEssaiPerteDeCharge , " &
        '                         "refSol , Enregistreur , Operateur , VolSonde , ClassPrecis , PrecisVolSonde , nomEssai , dateEssai ,heureEssai , difPresEauAir , " &
        '                         "hauteurCpv , presP0 , presFluage , presLimite , presLimiteNf , presLimiteTheo , " &
        '"PrecisVolume , PrecisPression , poidsVol , poidsVolDejauge , coefPoisson , modPressio , " &
        '"modPressioNf , modElastique , modElastiqueNf , cohesion , frottement , frottementintergranulaire , " &
        '"U0 , profNappe , profSonde , coteZeroGeomeca FROM LesEssaiForage"
        '                Else
        '                    Requete = "SELECT RefChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                        "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '"U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"
        '                End If
        '            Else
        '                Requete = "SELECT RefChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                    "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                    "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"
        '            End If
        '            Else
        '            Requete = "SELECT RefChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"
        '            End If

        '            'definition de la commande

        '            ' Définition de l'AdaptateurConstructionSite

        '            ' On crée la Table "LesEssaiForage"
        '            'If Not MonConstructionSite.Tables.Contains("LesEssaiForage") Then
        '            '    ' Dim table As New DataTable("LesEssaiForage")
        '            '    ' MonConstructionSite.Tables.Add(table)
        '            '    Result = CreerTableLesEssaiForage(MonConstructionSite, ConnConstructionSite)
        '            '    If Not (Result) Then Exit Sub
        '            'End If

        '            ' Mise à jour du compteur d'enregistrements
        '            ' ListForages.Count = MonConstructionSite.Tables("LesForages").Rows.Count

        '            ' Nettoyage de MonConstructionSite avant le chargement
        '            MonConstructionSite.Tables("LesEssaiForage")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaiForage" de MonDataSet

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaiForage" de MonDataSet

        '            ' AdaptateurConstructionSite.FillSchema(MonConstructionSite.Tables("LesEssaiForage"), SchemaType.Source)

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaiForage" de MonDataSet
        '            ' AdaptateurConstructionSite.Fill(MonConstructionSite.Tables("LesEssaiForage"))

        '            ' AdaptateurConstructionSite.FillSchema(MonConstructionSite, SchemaType.Source, "LesEssaiForage")
        '            'AdaptateurConstructionSite.Fill(MonConstructionSite, "LesEssaiForage")
        '            ObjetDataEssai = MonConstructionSite.Tables("LesEssaiForage")

        '            ' AdaptateurConstructionSite.FillSchema(MonConstructionSite, SchemaType.Source, "LesEssaiForage")
        '            'AdaptateurConstructionSite.Fill(MonConstructionSite, "LesEssaiForage")
        '            ObjetDataEssai = MonConstructionSite.Tables("LesEssaiForage")

        '            ' Mise à jour du compteur d'enregistrements
        '            ' EssaiCount = MonConstructionSite.Tables("LesEssaiForage").Rows.Count
        '            ' EssaiInitCount = MonConstructionSite.Tables("LesEssaiForage").Rows.Count

        '            ' Mise à jour du compteur d'enregistrements
        '            ' EssaiCount = MonConstructionSite.Tables("LesEssaiForage").Rows.Count
        '            ' EssaiInitCount = MonConstructionSite.Tables("LesEssaiForage").Rows.Count

        '            'Vérificatio de la Table 
        '            For i = 0 To  ListEssaiForages.Count - 1
        '                'Debug.WriteLine("MonConstructionSite.Tables(LesEssaiForage).Rows(" & i.ToString & "(RefForage)=" & MonConstructionSite.Tables("LesEssaiForage").Rows(i).Item("refForage").ToString)
        '                'Debug.WriteLine("MonConstructionSite.Tables(LesEssaiForage).Rows(" & i.ToString & "(RefEssai)=" & MonConstructionSite.Tables("LesEssaiForage").Rows(i).Item("refEssai").ToString)
        '            Next i

        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading of the Table 'LesEssaiForage' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesEssaiForage' réussie")
        '            End Select

        '            Select Case Lang
        '                Case 0
        '                    MsgBox("Reading of the Table 'LesEssaiForage' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesEssaiForage' réussie")
        '            End Select


        '            ' *********************************************************************
        '            ' Lecture de la Table "LesMesuresForage"
        '            ' *********************************************************************
        '            ' Lecture des mesures de l'essai
        '            ' Requete = "SELECT Ind, refEssai, indiceMesure, pression, volume15, volume30, volume60, contrainteRadiale, contrainteVerticale, deformation, PNF, VNF, Qi, Pi, Qp, Pp FROM LesMesuresForage"
        '            Requete = "SELECT ind, refEssai, indiceMesure, pression, volume15, volume30, volume60, contrainteRadiale, contrainteVerticale, deformation, PNF, VNF, Qi, Pi, Qp, Pp FROM LesMesuresForage"

        '            'definition du DataAdapter

        '            'definition du DataAdapter

        '            ' Définition de l'AdaptateurConstructionSite

        '            ' Nettoyage de MonConstructionSite avant le chargement
        '            MonConstructionSite.Tables("LesMesuresForage")?.Clear()

        '            ' Nettoyage de MonConstructionSite avant le chargement
        '            MonConstructionSite.Tables("LesMesuresForage")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesForages" de MonDataSet

        '            ObjetDataMesures = MonConstructionSite.Tables("LesMesuresForage")

        '            '' Mise à jour du compteur d'enregistrements
        '            '  Mesures.count = MonConstructionSite.Tables("LesMesuresForage").Rows.Count
        '            ' MesureInitCount = MonConstructionSite.Tables("LesMesuresForage").Rows.Count

        '            Select Case Lang

        '                Case 0
        '                    MsgBox("Reading of the Table 'LesMesuresForage' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesMesuresForage' réussie")
        '            End Select

        '            MyConstructionSite = New ConstructionSite

        '            '' **************************************************************************************
        '            '' Rangement dans MyConstructionSite des objets de la Tables("ConstructionSite") de MonConstructionSite
        '            '' **************************************************************************************
        '            '' i est le numéro du ConstructionSite
        '            ''For Each ConstructionSiteRow As DataRow In MonConstructionSite.Tables("ConstructionSite").Rows

        '            '' Nouveau ConstructionSite
        '            'MyConstructionSite = New ConstructionSite

        '            '    MyConstructionSite. RefChantier = ConstructionSiteRow("RefChantier").ToString()
        '            '    MyConstructionSite. NomConstructionSite = ConstructionSiteRow("NomConstructionSite").ToString()
        '            '    MyConstructionSite. DescriptifConstructionSite = ConstructionSiteRow("DescriptifConstructionSite").ToString
        '            '    MyConstructionSite. RefClient = ConstructionSiteRow("RefClient").ToString
        '            '    MyConstructionSite. Entreprise = ConstructionSiteRow("Entreprise").ToString
        '            '    MyConstructionSite. Adresse = ConstructionSiteRow("Adresse").ToString
        '            '    MyConstructionSite. Telephone = ConstructionSiteRow("Telephone").ToString

        '            '    'With MonConstructionSite.Tables("ConstructionSite").Rows(i)
        '            '    '    MyConstructionSite. RefChantier = .Item("RefChantier").ToString
        '            '    '    MyConstructionSite. NomConstructionSite = .Item("NomConstructionSite").ToString
        '            '    '    MyConstructionSite. DescriptifConstructionSite = .Item("DescriptifConstructionSite").ToString
        '            '    '    MyConstructionSite. RefClient = .Item("RefClient").ToString
        '            '    '    MyConstructionSite. Entreprise = .Item("Entreprise").ToString
        '            '    '    MyConstructionSite. Adresse = .Item("Adresse").ToString
        '            '    '    MyConstructionSite. Telephone = .Item("Telephone").ToString
        '            '    'End With

        '            '    ' **************************************************************************************
        '            '    ' Rangement dans  ListForages des objets de la Tables("LesForages") de MonConstructionSite
        '            '    ' **************************************************************************************

        '            '    ' j est le numéro du Forage
        '            '    Dim forageIndex As Integer = 1

        '            '    For Each forageRow As DataRow In MonConstructionSite.Tables("LesForages").Rows

        '            '        Dim new_Forage As New Forage

        '            '        If forageRow("RefChantier") Is Nothing Then Exit For
        '            '        ' Nouveau Forage
        '            '        MyForage = New Forage
        '            '        'MyForage = MyConstructionSite. ListForages(forageIndex - 1)

        '            '        new_Forage. RefChantier = forageRow("RefChantier").ToString()
        '            '        new_Forage. RefForage = forageRow("refForage").ToString()
        '            '        new_Forage. NomForage = forageRow("NomForage").ToString
        '            '        new_Forage. dateForage = forageRow("dateForage").ToString
        '            '        new_Forage. materiel = forageRow("materiel").ToString
        '            '        new_Forage. outil = forageRow("outil").ToString
        '            '        new_Forage. abscisseX = forageRow("abscisseX").ToString
        '            '        new_Forage. ordonneeY = forageRow("ordonneeY").ToString
        '            '        new_Forage. altitudeZ = forageRow("altitudeZ").ToString
        '            '        new_Forage. coteInit = forageRow("coteInit").ToString
        '            '        new_Forage. coteFinale = forageRow("coteFinale").ToString

        '            '        ' Redimensionner le tableau et Add le nouveau forage
        '            '        MyForage = new_Forage
        '            '        'MyForage = MyConstructionSite. ListForages(forageIndex - 1)
        '            '        forageIndex += 1

        '            '        ' **************************************************************************************
        '            '        ' Rangement dans MyForage des objets de la Tables("LesEssaiForage") de MonConstructionSite
        '            '        ' **************************************************************************************

        '            '        ' k est le numéro de l'EssaiForage
        '            '        kMax = MonConstructionSite.Tables("LesEssaiForage").Rows.Count

        '            '        Dim EssaiIndex As Integer = 1

        '            '        For Each essaiRow As DataRow In MonConstructionSite.Tables("LesEssaiForage").Rows

        '            '            If essaiRow("refForage").ToString() = new_Forage. RefForage Then
        '            '                Dim new_Essai As New Essai_Forage(False)
        '            '                Dim Verif As String
        '            '                Verif = essaiRow("refEssai").ToString()
        '            '                new_Essai. RefEssai = essaiRow("refEssai").ToString()
        '            '                new_Essai. RefChantier = essaiRow("RefChantier").ToString
        '            '                new_Essai. RefForage = essaiRow("refForage").ToString
        '            '                new_Essai. RefEssai = essaiRow("refEssai").ToString
        '            '                'Accu = "MonConstructionSite.Tables(LesEssaiForage).Rows(" & k.ToString & ")=" & essaiRow("refEssai").ToString
        '            '                'Debug.WriteLine(Accu)
        '            '                new_Essai. refSonde = essaiRow("refSonde").ToString
        '            '                'new_Essai.Sonde = essaiRow("nomSonde").ToString
        '            '                new_Essai. refEssaiResistancePropre = essaiRow("refEssaiResistancePropre").ToString
        '            '                new_Essai. refEssaiExpansionPropre = essaiRow("refEssaiExpansionPropre").ToString
        '            '                new_Essai. RefSol = essaiRow("refSol").ToString
        '            '                If essaiRow("ClassPrecis").ToString <> "" Then new_Essai. ClassPrecis = essaiRow("ClassPrecis").ToString
        '            '                new_Essai. Enregistreur = essaiRow("Enregistreur").ToString
        '            '                new_Essai. Operateur = essaiRow("Operateur").ToString
        '            '                new_Essai. VolSonde = essaiRow("VolSonde").ToString
        '            '                new_Essai. ClassPrecis = Valeurs(essaiRow("ClassPrecis").ToString)
        '            '                new_Essai. PrecisVolSonde = essaiRow("PrecisVolSonde").ToString
        '            '                new_Essai. nomEssai = essaiRow("nomEssai").ToString
        '            '                new_Essai. dateEssai = essaiRow("dateEssai").ToString
        '            '                new_Essai. heureEssai = essaiRow("heureEssai").ToString
        '            '                new_Essai. difPresEauAir = essaiRow("difPresEauAir").ToString
        '            '                new_Essai. hauteurCpv = essaiRow("hauteurCpv").ToString
        '            '                new_Essai. presP0 = essaiRow("presP0").ToString
        '            '                new_Essai. presFluage = essaiRow("presFluage").ToString
        '            '                new_Essai. presLimite = essaiRow("presLimite").ToString
        '            '                new_Essai. presLimiteNF = essaiRow("presLimiteNf").ToString
        '            '                new_Essai. poidsVol = essaiRow("poidsVol").ToString
        '            '                new_Essai. poidsVolDejauge = essaiRow("poidsVolDejauge").ToString
        '            '                new_Essai. coefPoisson = essaiRow("coefPoisson").ToString
        '            '                new_Essai. modPressio = essaiRow("modPressio").ToString
        '            '                new_Essai. modPressioNf = essaiRow("modPressioNf").ToString
        '            '                new_Essai. modElastique = essaiRow("modElastique").ToString
        '            '                new_Essai. modElastiqueNf = essaiRow("modElastiqueNf").ToString
        '            '                new_Essai. Cohesion = essaiRow("cohesion").ToString
        '            '                new_Essai. Frottement = essaiRow("frottement").ToString
        '            '                new_Essai. FrottementIntergranulaire = essaiRow("frottementintergranulaire").ToString
        '            '                new_Essai. U0 = essaiRow("U0").ToString
        '            '                new_Essai. profNappe = essaiRow("profNappe").ToString
        '            '                new_Essai. profSonde = essaiRow("profSonde").ToString
        '            '                new_Essai. coteZeroGeomeca = essaiRow("coteZeroGeomeca").ToString

        '            '                EssaiIndex += 1

        '            '                MyEssaiForage = Essai_Forage.CreateNewEssai(False)
        '            '                MyEssaiForage = new_Essai

        '            '                For IndSonde = 0 To MyListeSonde.P_ListeSonde.Count - 1
        '            '                    If MyEssaiForage. refSonde = MyListeSonde.P_ListeSonde(IndSonde).GetRefSonde Then Exit For
        '            '                Next IndSonde

        '            '                MySonde = MyListeSonde.GetSondeParIndex(IndSonde)

        '            '                ' MAJ de MyEssaiForage.refEssaiResistancePropre
        '            '                If MySonde.P_Resistance.Count > 0 Then
        '            '                    kMax = MySonde.P_Resistance.Count
        '            '                    If kMax = 0 Then kMax = 1
        '            '                    For IndEssai = 0 To kMax
        '            '                        If MySonde.P_Resistance(IndEssai) IsNot (Nothing) Then
        '            '                            If MyEssaiForage. refEssaiResistancePropre = MySonde.P_Resistance(IndEssai). refEssai Then
        '            '                                MyResistance = New EssaiResistance
        '            '                                MyResistance = MySonde.P_Resistance(IndEssai)
        '            '                                Exit For
        '            '                            End If
        '            '                        End If
        '            '                    Next IndEssai
        '            '                End If

        '            '                MyEssaiForage. MyResistance = MyResistance
        '            '                ' MAJ de MyEssaiForage.refEssaiExpansionPropre
        '            '                If MySonde.P_Expansion.Count > 0 Then
        '            '                    For IndEssai = 0 To MySonde.P_Expansion.Count - 1
        '            '                        If MyEssaiForage. refEssaiExpansionPropre = MySonde.P_Expansion(IndEssai). RefEssai Then
        '            '                            MyExpansion = New EssaiExpansion
        '            '                            MyExpansion = MySonde.P_Expansion(IndEssai)
        '            '                            Exit For
        '            '                        End If
        '            '                    Next IndEssai
        '            '                End If

        '            '                MyEssaiForage. MyExpansion = MyExpansion

        '            '                'End If ' Fin de If essaiRow("refForage").ToString() = NewForage.RefForage Then

        '            '                'End If ' Fin de If essaiRow("refForage").ToString() = NewForage.RefForage Then

        '            '                'ReDim Preserve MyConstructionSite. ListEssaiForages(k)
        '            '                'MyConstructionSite. ListEssaiForages(k) = MyForage. ListEssais(l - 1)

        '            '                ' **************************************************************************************
        '            '                ' Rangement dans MyEssaiForage des objets de la Tables("LesMesuresForage") de MonConstructionSite
        '            '                ' **************************************************************************************

        '            '                '' Affectation de la Liste des Sondes à MyConstructionSite
        '            '                '    RowSonde = New Probe
        '            '                '    RowSonde = MyListeSonde.ListeSonde(i)
        '            '                '    If MyConstructionSite. ListeSonde.Count - 1 <= i Then
        '            '                '        ReDim Preserve MyConstructionSite. ListeSonde(i)
        '            '                '        MyConstructionSite. ListeSonde(i) = New Probe
        '            '                '    End If
        '            '                '    MyConstructionSite. ListeSonde(i) = RowSonde
        '            '                'Next i

        '            '                '' Affectation de la Liste des Sondes à MyConstructionSite
        '            '                '    RowSonde = New Probe
        '            '                '    RowSonde = MyListeSonde.ListeSonde(i)
        '            '                '    If MyConstructionSite. ListeSonde.Count - 1 <= i Then
        '            '                '        ReDim Preserve MyConstructionSite. ListeSonde(i)
        '            '                '        MyConstructionSite. ListeSonde(i) = New Probe
        '            '                '    End If
        '            '                '    MyConstructionSite. ListeSonde(i) = RowSonde
        '            '                'Next i


        '            '                ' m est le numéro des MesuresForage
        '            '                m = 0
        '            '                For Each mesureRow As DataRow In MonConstructionSite.Tables("LesMesuresForage").Rows

        '            '                    If mesureRow("refEssai").ToString() = new_Essai. RefEssai Then

        '            '                        Dim new_Mesure As New MesureEssaiForage()
        '            '                        new_Mesure.Pression = mesureRow("pression").ToString()

        '            '                        ' On ajoute une nouvelle Mesure à l'Essai actuel
        '            '                        If mesureRow("refEssai").ToString() = new_Essai. RefEssai Then

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
        '            '                            PressionNF = MonConstructionSite.Tables("LesMesuresForage").Rows(m)("PNF")
        '            '                            VolumeNF = MonConstructionSite.Tables("LesMesuresForage").Rows(m)("VNF")
        '            '                            'Debug.WriteLine("MonConstructionSite.Tables(LesMesuresForage).Rows(" & m.ToString() & ").PNF= " & PressionNF.ToString())
        '            '                            'Debug.WriteLine("MonConstructionSite.Tables(LesMesuresForage).Rows(" & m.ToString() & ").VNF= " & VolumeNF.ToString())
        '            '                            Qi = 0
        '            '                            Pi = 0
        '            '                            Qp = 0
        '            '                            Pp = 0

        '            '                            MyEssaiForage.NouvelleMesure(False, m, n, new_Mesure.Pression, new_Mesure.Volume15, new_Mesure.Volume30, new_Mesure.Volume60,
        '            '                                                    new_Mesure.ContrainteRadiale, SigV, new_Mesure.Deformation, new_Mesure.PNF, new_Mesure.VNF, Qi, Pi, Qp, Pp)
        '            '                            IndMesures = IndMesures + 1
        '            '                        End If ' fin du If mesureRow("refEssai").ToString() = new_Essai. RefEssai Then

        '            '                        'If IndMesures > 1 Then
        '            '                        'IndMesures = IndMesures + 1
        '            '                    End If ' fin du If mesureRow("refEssai").ToString() = new_Essai. RefEssai Then

        '            '                Next mesureRow ' Fin de For m = 0 To MonConstructionSite.Tables("LesMesuresForage").Rows().Count - 1
        '            '            End If ' fin du If essaiRow("refForage").ToString() = new_Forage.RefForage Then
        '            '        Next essaiRow
        '            '        ' --------------------------------------------
        '            '        ' MyEssaiForage. MesureEssaiForage est juste à ce niveau
        '            '        ' --------------------------------------------
        '            '    Next forageRow ' Fin du For Each forageRow As DataRow In MonConstructionSite.Tables("LesForages").Rows
        '            'Next ConstructionSiteRow ' Fin du For Each row As DataRow In MonConstructionSite.Tables("ConstructionSite").Rows

        '            ChargeConstructionSite = True

        '            'End If ' Fin de If Not (ChargeConstructionSite) Then


        '            ' Sauvegarde du nom de fichier pour une future utilisation
        '            MyConstructionSite. MyFile = FichierConstructionSite

        '            ' Extraction du nom de fichier 

        '            '' Vérification de MYListeSonde.Tables("LesEssaiForage")
        '            'refSondes.Clear()
        '            'For i = 0 To MyListeSonde.ListeSonde.Count - 1
        '            '    RowSonde = MyListeSonde.ListeSonde(i)
        '            '    ReferenceSonde = RowSonde.refSonde.ToString()
        '            '    If refSondes.ContainsKey(ReferenceSonde) Then
        '            '        MsgBox("Doublon détecté dans MyListeSonde pour refSonde : " & i & ReferenceSonde)
        '            '        Exit For
        '            '    Else
        '            '        refSondes.Add(ReferenceSonde, True)
        '            '    End If
        '            'Next i

        '            ' Le ConstructionSite est considéré comme déjà sauvé
        '             IsSaved = True
        '            Exit Sub

        '            ' Traitement des erreurs lors du chargement de la base de données:
        'errFind:
        '            ' Fermeture de la connection
        '            ' ConnConstructionSite = Nothing

        '            ' Chargement de l'objet d'erreur
        '            Select Case Lang
        '                Case 0
        '                    MsgBox(" Error during loading of building site " & FichierConstructionSite & " ", Information.Err.Description)
        '                Case 1
        '                    MsgBox(" Erreur lors de l'ouverture du ConstructionSite référencé " & FichierConstructionSite)
        '                    MsgBox(" Erreur lors de l'ouverture du ConstructionSite référencé " & FichierConstructionSite & " ", Err.Description)
        '            End Select
        '            End
        '            Resume Next
        '        End Sub
        Public Function GetRefChantier()
            GetRefChantier = RefChantier
        End Function

        Public Function GetNomConstructionSite()
            GetNomConstructionSite = NameConstructionSite
        End Function


        Public Function GetDescriptifConstructionSite()
            GetDescriptifConstructionSite = DescriptionConstructionSite
        End Function

        Public Function GetRefClient()
            GetRefClient = RefClient
        End Function

        Public Function GetEntreprise()
            GetEntreprise = Entreprise
        End Function

        Public Function GetAdresse()
            GetAdresse = Adresse
        End Function

        Public Function GetTelephone()
            GetTelephone = Telephone
        End Function

        Public Function GetForagesCount()
            GetForagesCount = ListForages.Count
        End Function

        Public Sub SetRefChantier(ByVal ref As String)
            RefChantier = ref
        End Sub


        Public Sub SetNomConstructionSite(ByVal nom As String)
            NameConstructionSite = nom
        End Sub

        Public Sub SetDescriptifConstructionSite(ByVal descriptif As String)
            DescriptionConstructionSite = descriptif
        End Sub

        Public Sub SetRefClient(ByVal ref As String)
            RefClient = ref
        End Sub

        Public Sub SetEntreprise(ByVal entrep As String)
            Entreprise = entrep
        End Sub

        Public Sub SetAdresse(ByVal adr As String)
            Adresse = adr

            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSupendus Then
                RaiseEvent HasBeenModified()
            End If
        End Sub

        Public Sub SetTelephone(ByVal tel As String)
            Telephone = tel

            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSupendus Then
                RaiseEvent HasBeenModified()
            End If
        End Sub


        ' Affichage du nouveau ConstructionSite dans l'arborescence des ConstructionSite
        'Public Sub AfficheDansArbo(ByRef Arbo As TreeView)
        '    MsgBox "AfficheDansArbo"

        ' Vidage de l'arborescence
        'Arbo.Nodes.Clear()
        '' NomConstructionSite = MyConstructionSite.GetNomConstructionSite
        ' NomConstructionSite = MyConstructionSite. NomConstructionSite

        '' Ajout du noeud racine
        'TreeViewArboRoot = New TreeNode( NomConstructionSite)
        'TreeViewArboRoot.Name =  NomConstructionSite
        'Arbo.Nodes.Add(TreeViewArboRoot)

        '' Affichage du ConstructionSite
        'Dim element As Integer
        ''  ReDim Preserve  MyListeForageDb( ListForages.Count)
        '  ListForages.Count = MyConstructionSite.  ListForages.Count
        'If   ListForages.Count > 0 Then
        '    For element = 0 To (MyConstructionSite.  ListForages.Count - 1)
        '        MyForage = New Forage(True)
        '        MyForage = MyConstructionSite.  ListForages(element)
        '        ' Ajout du noeud Parent Forage
        '        TreeViewArboParent = New TreeNode(MyConstructionSite.  ListForages(element).NomForage)
        '        TreeViewArboParent.Name = MyConstructionSite.  ListForages(element).NomForage
        '        TreeViewArboRoot.Nodes.Add(TreeViewArboParent)
        '        MyForage.RefForage = MyConstructionSite.  ListForages(element).RefForage
        '        MyForage.NomForage = MyConstructionSite.  ListForages(element).NomForage
        '        MyForage.RefChantier = MyConstructionSite. RefChantier
        '        Call   ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)
        '    Next

        'End If
        'Arbo.ExpandAll()

        ' Vidage de l'arborescence
        'Arbo.Nodes.Clear()

        '' Vérifiez que MyConstructionSite n'est pas Nothing
        'If MyConstructionSite Is Nothing Then
        '        Exit Sub
        '    End If

        '    ' Récupération du nom du ConstructionSite
        '     NomConstructionSite = MyConstructionSite.NomConstructionSite

        '    ' Ajout du noeud racine
        '    TreeViewArboRoot = New TreeNode( NomConstructionSite)
        '    TreeViewArboRoot.Name =  NomConstructionSite
        '    Arbo.Nodes.Add(TreeViewArboRoot)

        '    ' Affichage du ConstructionSite
        '     ListForages.Count = MyConstructionSite. ListForages.Count

        '    If  ListForages.Count > 0 Then
        '        For element As Integer = 0 To  ListForages.Count - 1
        '            ' Vérifiez que MyConstructionSite.  ListForages(element) n'est pas Nothing
        '            If MyConstructionSite. ListForages(element) IsNot Nothing Then
        '                ' Ajout du noeud Parent Forage
        '                Dim TreeViewArboParent As TreeNode = New TreeNode(MyConstructionSite. ListForages(element).NomForage)
        '                MyForage = New Forage(True)
        '                MyForage = MyConstructionSite. ListForages(element)
        '                ' Ajout du noeud Parent Forage
        '                TreeViewArboParent = New TreeNode(MyConstructionSite. ListForages(element).NomForage)
        '                TreeViewArboParent.Name = MyConstructionSite. ListForages(element).NomForage
        '                TreeViewArboRoot.Nodes.Add(TreeViewArboParent)
        '                MyForage.RefForage = MyConstructionSite. ListForages(element).RefForage
        '                MyForage.NomForage = MyConstructionSite. ListForages(element).NomForage
        '                MyForage. RefChantier = MyConstructionSite. RefChantier
        '                'Call   ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)

        '                ' Appeler la méthode AfficheDansArbo de l'objet Forage
        '                MyConstructionSite. ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)
        '            End If
        '        Next
        '    End If

        '    ' Développer tous les noeuds
        '    Arbo.ExpandAll()
        'End Sub

        ' Affiche les modifications dans la DataGrid)
        ' Public Sub AfficheDansTab(ByRef Table As DataGridView, ByVal FichierConstructionSite As String)
        '        Public Sub AfficheDansTab(ByRef DataGrid As DataGridView, ByRef BindingData As BindingSource, ByVal FichierConstructionSite As String)
        '            Dim i As Integer
        '            'Dim fonts As New MyFontList()
        '            Dim ForageList As MyForageList

        '            On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            ' affectation de la bindingdata à la Listbox
        '            With DataGrid
        '                .DataSource = MonConstructionSite.Tables("LesForages")
        '            End With

        '            Exit Sub
        'errFind:
        '            ' Chargement de l'objet d'erreur
        '            Select Case Lang
        '                Case 0
        '                    MsgBox(" Error during loading of borehole " &  ListForages(i).GetRefForage & " ", Err.Description)
        '                Case 1
        '                    MsgBox(" Erreur lors de l'ouverture du forage " &  ListForages(i).GetRefForage & " ", Err.Description)
        '            End Select
        '            Resume Next

        '        End Sub

        'Public Sub Show()
        '    MyFenetreConstructionSite.Show()
        'End Sub

        'Public Sub MaskConstructionSite()
        '    'Unload(MyFenetreConstructionSite)
        '    MyFenetreConstructionSite?.Close()
        'End Sub

        Public Sub CloseConstructionSite()

            'Call MaskConstructionSite()
            Dim i As Integer
            If ListForages IsNot Nothing Then
                For i = 0 To ListForages.Count - 1
                    ' ListForages(i).CloseForage()
                    ListForages(i) = Nothing
                Next
            End If
            ' ListForages.Count = 0
        End Sub

        'Public Sub NouveauForage(ByVal NouvForage As Boolean, ByVal NewTable As Boolean)

        '    ' Déclaration d'un Nouveau Forage
        '    Dim NewForage As Forage

        '    NewForage = Nothing

        '    If NouvForage Then
        '        ' Incrémentation du compteur de forages
        '        MyConstructionSite. ListForages.Count = MyConstructionSite. ListForages.Count + 1
        '    End If

        '    If MonConstructionSite.Tables("LesForages") Is Nothing Then
        '        NewTable = True
        '    End If

        '    ' Redimensionnement du tableau dynamique de forages
        '    'ReDim Preserve MyConstructionSite.  ListForages(  ListForages.Count - 1)

        '    ' On ajoute un Nouveau Forage à la Liste des Forages de MyConstructionSite
        '    Dim nouveauForage As New Forage
        '    MyConstructionSite.AddForage(nouveauForage,  ListForages.Count)
        '    ' Instanciation du nouveau forage
        '    If NewForage Is Nothing Then

        '        ' NewForage = nouveauForage
        '        If MyConstructionSite. ListForages.Count > 0 And (MyConstructionSite. ListForages( ListForages.Count - 1) IsNot Nothing) Then
        '            NewForage = MyConstructionSite. ListForages( ListForages.Count - 1)
        '        Else
        '            NewForage = nouveauForage
        '            ' ListForages.Count =  ListForages.Count + 1
        '        End If
        '    End If

        '    ' Enregistrement du nouveau forage dans le tableau de forage
        '    MyConstructionSite. ListForages( ListForages.Count - 1) = NewForage

        '    If NewTable Then  ' On ajoute un Forage à MonFichier.Tables("LesForage") seulement si Table est vrai
        '        If MonConstructionSite.Tables("LesForages") Is Nothing Then
        '            MonConstructionSite.Tables.Add("LesForages")
        '            With MonConstructionSite.Tables("LesForages")

        '                Dim columns() As DataColumn
        '                ReDim columns(12)

        '                .Columns.Add(" RefChantier", GetType(String))
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
        '            MonConstructionSite.Tables("LesForages").NewRow()
        '        End If
        '    End If

        '    'MyConstructionSite. ListFyorages(  ListForages.Count - 1) = NewForage
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

            ListForages(Ind) = Nothing

            ' Décalage des forages suivant d'une case
            For J = Ind To ListForages.Count - 1
                ListForages(J) = ListForages(J + 1)
            Next

            ' Redimensionnement du tableau   ListForages
            If ListForages.Count > 1 Then
                'ReDim Preserve  ListForages( ListForages.Count - 1)
                '  ListForages.RemoveAt(  ListForages.Count - 1)
            Else
                'ReDim  ListForages(0)
                '  ListForages.RemoveAt(0)
            End If

            ' Décrémentation du compteur de forages
            ' ListForages.Count =  ListForages.Count - 1

            ' Nouveau tableau à afficher
            'MyFenetreConstructionSite.Close()
            'MyFenetreConstructionSite.Show()

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
            Do While ((i < (MyConstructionSite.ListForages.Count) - 1) And (MyConstructionSite.ListForages(i).NomForage <> Nom))
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
            Do While ((i < MyConstructionSite.ListForages.Count) And (ListForages(i).GetRefForage <> ref))
                'Do While (i <   ListForages.Count) And (MyConstructionSite.GetForage(i).GetRefForage <> ref)
                i = i + 1
            Loop
            GetIndexRefForage = i
        End Function

        'Public Function GetIndexRefEssai(ByVal refForage As String, ByVal refEssai As String) As Integer
        '    ' Recherche du forage ayant pour référence 'ref' avec arrêt dès que possible
        '    Dim iForage, iEssai As Integer
        '    iForage = 0
        '    iEssai = 0
        '    GetIndexRefEssai = 0
        '    '    MsgBox "GetIndexForage"
        '    If refForage = "" Then Exit Function
        '    If refEssai = "" Then Exit Function

        '    ' Recherche du Forage dans la Table des forages
        '    Do While ((iForage < ListForages.Count) And (ListForages(iForage).GetRefForage <> refForage))
        '        iForage = iForage + 1
        '    Loop
        '    If iForage = ListForages.Count Then Exit Function

        '    ' Recherche de l'Essai dans la table des Essais
        '    Do While ((iEssai < ListForages(iForage).ListEssais.Count) And (ListForages(iForage).ListEssais(iEssai).refEssai <> refEssai))
        '        iEssai = iEssai + 1
        '    Loop
        '    GetIndexRefEssai = iEssai
        'End Function

        'Public Sub ShowForage(ByVal i As Integer)
        '    MyForage =  ListForages(i)
        '    MyConstructionSite.refForage = MyForage. RefForage
        '     ListForages(i).Show(MyForage)
        '    'FrmForage.Show()
        'End Sub

        'Public Sub MaskForage(ByVal ref As String)
        '    Dim i As Integer    ' Index du forage à fermer
        '    i = GetIndexRefForage(ref)
        '     ListForages(i).MaskForage()
        'End Sub

        Public Sub SuspendreEvents()
            EventsSupendus = True
        End Sub
        Public Sub RetablirEvents()
            EventsSupendus = False
        End Sub

        ' Création d'un nouveau fichier à partir de MyConstructionSite
        Public Function NouveauFichier(FichierConstructionSite As String) As Boolean
            Dim NomFichierInitial As String
            Dim Racine As String
            NouveauFichier = False
            Racine = AppDomain.CurrentDomain.BaseDirectory
            Racine = System.IO.Path.GetFullPath(System.IO.Path.Combine(Racine, "..\..\..\.."))
            FichierConstructionSite = Racine & "\ConstructionSite\" & FichierConstructionSite & ".mdb"
            'NomFichierInitial = FichierConstructionSiteousDossier(FichierConstructionSite, "ConstructionSite", "Ressources", ".mdb")
            Racine = AppDomain.CurrentDomain.BaseDirectory
            Racine = System.IO.Path.GetFullPath(System.IO.Path.Combine(Racine, "..\..\..\.."))
            NomFichierInitial = Racine & "\Ressources\ConstructionSite.mdb"
            Call FileCopy(NomFichierInitial, FichierConstructionSite)
            NouveauFichier = True
        End Function

        '        Public Function EnregistreConstructionSite(FichierConstructionSite As String, ByVal Nouveau As Boolean) As Boolean
        '            On Error GoTo errFind

        '            Dim RowNumber As Integer
        '            Dim Requete, refForage As String
        '            Dim MonModifConstructionSite As DataSet
        '            Dim i, jMax, kMax, NbAcces, NbTable, iTable As Integer
        '            Dim Essai As Essai_Forage
        '            Dim row As DataRow
        '            Dim RowEssai As Essai_Forage
        '            Dim Found As Boolean
        '            Dim refEssais As New Dictionary(Of String, Boolean)

        '            On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            ' On crée une commanDebuilder pour renvoyer les données à la BD

        '            On Error GoTo errFind

        '            ' Ouverture de la console
        '            ' System.Console.Title = "Debug Console"

        '            ' Ouverture de la connection avec la BD

        '            ' *********************************************************************
        '            ' Enregistrement de la Table "ConstructionSite"
        '            ' *********************************************************************

        '            ''ma requete
        '            ' exemple plasserre.developpez.com/cours/vb-net : strSql = "SELECT FICHEPATIENT.* FROM FICHEPATIENT"
        '            'Requete = "SELECT RefChantier AS  RefChantier, NomConstructionSite AS  NomConstructionSite, DescriptifConstructionSite as  DescriptifConstructionSite, RefClient as  RefClient, Entreprise as  Entreprise, Adresse as  Adresse , Telephone as  Telephone, Reference as  Reference FROM ConstructionSite"
        '            Requete = "SELECT RefChantier , NomConstructionSite , DescriptifConstructionSite , RefClient , Entreprise , Adresse , Telephone , Reference FROM ConstructionSite"

        '            ' Définition de l'AdaptateurConstructionSite

        '            iTable = 0

        '            ' definition de la commande

        '            iTable = 0
        '            ObjetDataConstructionsSitesDb = MonConstructionSite.Tables("ConstructionSite")

        '            ' Définition de l'AdaptateurConstructionSite

        '            'Pour modifier les valeurs changées dans le DataAdapter

        '            columns(0) = MonConstructionSite.Tables("ConstructionSite").Columns("RefChantier")
        '            MonConstructionSite.Tables("ConstructionSite").PrimaryKey = columns

        '            iTable = 0
        '            ObjetDataConstructionsSitesDb = MonConstructionSite.Tables("ConstructionSite")

        '            ' ConstructionSiteCount = MonConstructionSite.Tables("ConstructionSite").Rows.Count
        '            'If  ConstructionSiteInitCount <  ConstructionSiteCount Then
        '            '    ' On ajoute un enregistrement à la Table
        '            'End If

        '            ' Définition de la clef primaire de la Table "ConstructionSite"
        '            Dim columns() As DataColumn
        '            ReDim columns(7)

        '            columns(0) = MonConstructionSite.Tables("ConstructionSite").Columns("RefChantier")
        '            MonConstructionSite.Tables("Leantiers").PrimaryKey = columns

        '            If MonConstructionSite.Tables("ConstructionSite") IsNot Nothing Then
        '                'AdaptateurConstructionSite.Update(MonConstructionSite, "ConstructionSite")
        '                'ConstructionSiteInitCount = MonConstructionSite.Tables("ConstructionSite").Rows.Count

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonConstructionSite.Tables("ConstructionSite").Clear()
        '                'AdaptateurConstructionSite.Fill(MonConstructionSite, "ConstructionSite")

        '                ' Mise à jour de MyConstructionSite
        '                If MonConstructionSite.Tables("ConstructionSite").Rows.Count > 0 Then

        '                    MyConstructionSite. RefChantier = MonConstructionSite.Tables("ConstructionSite").Rows(0).Item("RefChantier").ToString

        '                    'AdaptateurConstructionSite.Update(MonConstructionSite, "ConstructionSite")

        '                    '' On vide la Table et on la remplit à nouveau
        '                    'MonConstructionSite.Tables("ConstructionSite").Clear()
        '                    'AdaptateurConstructionSite.Fill(MonConstructionSite, "ConstructionSite")
        '                End If

        '                MsgBox("Update -ConstructionSite- successful")
        '            End If
        '            ' *********************************************************************
        '            ' Enregistrement de la Table "LesForages"
        '            ' *********************************************************************

        '            'ma requete
        '            'Requete = "SELECT RefChantier AS  RefChantier, refForage as  refForage, NumeroForage as  NumeroForage, NomForage as  NomForage, " _
        '            '       & "dateForage as  dateForage, materiel as  materiel, outil as  outil, abscisseX as  abscisseX, ordonneeY as  ordonneeY, altitudeZ as  altitudeZ, " _
        '            '       & "coteInit as  coteInit, coteFinale as  coteFinale FROM LesForages"
        '            Requete = "SELECT RefChantier , refForage , NumeroForage , NomForage , " &
        '                "dateForage , materiel , outil , abscisseX , ordonneeY , altitudeZ , " &
        '                "coteInit , coteFinale  FROM LesForages"

        '            iTable = 1
        '            'definition de la commande

        '            ' Définition de l'AdaptateurConstructionSite

        '            ' Définition de l'AdaptateurConstructionSite

        '            ' Définition de la clef primaire de la Table "LesForages"
        '            ReDim columns(0)

        '            columns(0) = MonConstructionSite.Tables("LesForages").Columns("refForage")
        '            MonConstructionSite.Tables("LesForages").PrimaryKey = columns

        '            '' Remise à jour de la Table "LesForages"
        '            'FrmForage.UpDateLesForages(MyConstructionSite.refForage)

        '            'Pour modifier les valeurs changées dans le DataAdapter

        '            ' Adaptation du fichier Accès à la dimension de la Table
        '            NbAcces = MonConstructionSite.Tables("LesForages").Rows.Count


        '            ' Mise à jour de la Base de données Access 
        '            iTable = 1
        '            ObjetDataLesForages = MonConstructionSite.Tables("LesForages")

        '            For Each row In MonConstructionSite.Tables("LesForages").Rows
        '                Debug.WriteLine("Row State: " & row.RowState.ToString())
        '            Next

        '            If MonConstructionSite.Tables("LesForages") IsNot Nothing Then
        '                ' ListForages.Count = MonConstructionSite.Tables("LesForages").Rows.Count
        '                'If  ForageInitCount <  ListForages.Count Then
        '                ' On ajoute un enregistrement à la Table
        '                'End If

        '                '' Définition de la clef primaire de la Table "LesForages"
        '                'columns(0) = New DataColumn
        '                'columns(0) = MonConstructionSite.Tables("LesForages").Columns("refForage")
        '                'MonConstructionSite.Tables("LesForages").PrimaryKey = columns

        '                ' Mettre à jour la base de données
        '                'AdaptateurConstructionSite.Update(MonConstructionSite, "LesForages")
        '                'MsgBox("Update -LesForages- successful")

        '                '' Recharger la table après la mise à jour
        '                'MonConstructionSite.Tables("LesForages").Clear()
        '                'AdaptateurConstructionSite.Fill(MonConstructionSite, "LesForages")
        '            End If

        '            ' *********************************************************************
        '            ' Enregistrement de la Table "LesEssaiForage"
        '            ' *********************************************************************
        '            iTable = 2
        '            ObjetDataEssai = MonConstructionSite.Tables("LesEssaiForage")

        '            ' Mise à jour de la Table "LesEssaiForage"
        '            ''ma requete

        '            'Requete = "SELECT  RefChantier,  refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, ClassPrecis, PrecisVolSonde, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " _
        '            '   & "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " _
        '            '   & "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"
        '            Requete = "SELECT RefChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, ClassPrecis, PrecisVolSonde, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                    "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                    "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"

        '            iTable = 2
        '            ' Vérification de la Table
        '            With MonConstructionSite.Tables("LesEssaiForage")
        '                For i = 0 To .Rows.Count - 1

        '                    row = MonConstructionSite.Tables("LesEssaiForage").Rows(i)
        '                    'Debug.WriteLine("refEssai = " & row("refEssai").ToString)
        '                    'Debug.WriteLine("RefChantier = " & row("RefChantier").ToString)
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

        '            ' Définition de l'AdaptateurConstructionSite

        '            'definition de la commande

        '            ' Définition de l'AdaptateurConstructionSite

        '            ' Définition de l'AdaptateurConstructionSite

        '            ' Définition de la clef primaire de la Table "LesEssaiForage"
        '            ReDim columns(11)
        '            columns(0) = New DataColumn
        '            columns(0) = MonConstructionSite.Tables("LesEssaiForage").Columns(2)
        '            MonConstructionSite.Tables("LesEssaiForage").PrimaryKey = columns

        '            '' Remise à jour de la Table "LesEssaiForage"
        '            'FrmForage.UpDateLesForages(MyConstructionSite.refForage)

        '            'Pour modifier les valeurs changées dans le DataAdapter

        '            ' Adaptation du fichier Accès à la dimension de la Table
        '            NbAcces = MonConstructionSite.Tables("LesEssaiForage").Rows.Count

        '            MsgBox("Update -LesEssaiForage- successful")
        '            ' EssaiInitCount = MonConstructionSite.Tables("LesEssaiForage").Rows.Count


        '            ' Mise à jour de la Base de données Access 
        '            iTable = 1
        '            ObjetDataLesForages = MonConstructionSite.Tables("LesEssaiForage")

        '            If MonConstructionSite.Tables("LesEssaiForage") IsNot Nothing Then
        '                ' ListForages.Count = MonConstructionSite.Tables("LesEssaiForage").Rows.Count
        '                'If  ForageInitCount <  ListForages.Count Then
        '                '    ' On ajoute un enregistrement à la Table
        '                'End If

        '                ' Définition de la clef primaire de la Table "LesEssaiForage"
        '                columns(0) = New DataColumn
        '                columns(0) = MonConstructionSite.Tables("LesEssaiForage").Columns("refEssai")
        '                MonConstructionSite.Tables("LesEssaiForage").PrimaryKey = columns

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonConstructionSite.Tables("LesEssaiForage").Clear()
        '                'AdaptateurConstructionSite.Fill(MonConstructionSite, "LesEssaiForage")

        '                'AdaptateurConstructionSite.Update(MonConstructionSite, "LesEssaiForage")
        '                'MsgBox("Update -LesEssaiForage- successful")
        '                'ForageInitCount = MonConstructionSite.Tables("LesEssaiForage").Rows.Count

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonConstructionSite.Tables("LesEssaiForage").Clear()
        '                'AdaptateurConstructionSite.Fill(MonConstructionSite, "LesEssaiForage")
        '            End If


        '            ' *********************************************************************
        '            ' Enregistrement de la Table "LesMesuresForage"
        '            ' *********************************************************************

        '            With MonConstructionSite.Tables("LesMesuresForage")
        '                For i = 0 To .Rows.Count - 1

        '                    row = MonConstructionSite.Tables("LesMesuresForage").Rows(i)
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
        '            'AdaptateurConstructionSite.SelectCommand = Commande

        '            ' Définition de l'AdaptateurConstructionSite

        '            '' Remise à jour de la Table "LesMesuresForage"
        '            'FrmEssai.UpDateLesMesuresForages(MyEssaiForage.RefEssai)

        '            ' 1. Définition de la clef primaire de la Table "LesMesuresForage" une seule fois
        '            ReDim columns(35)
        '            columns(0) = New DataColumn

        '            columns(0) = MonConstructionSite.Tables("LesMesuresForage").Columns("Ind") ' Exemple : colonne "Ind" comme clé primaire
        '            MonConstructionSite.Tables("LesMesuresForage").PrimaryKey = columns

        '            ''Pour modifier les valeurs changées dans le DataAdapter

        '            ' 2. Créer l'OleDbCommandBuilder et l'adaptateur

        '            If MonConstructionSite.Tables("LesMesuresForage") IsNot Nothing Then

        '                ' Adaptation du fichier Accès à la dimension de la Table
        '                NbAcces = MonConstructionSite.Tables("LesMesuresForage").Rows.Count

        '                'AdaptateurConstructionSite.AcceptChangesDuringUpdate = True

        '                '' Mise à jour de la Base de données Access 
        '                'iTable = 3
        '                'ObjetDataMesures = MonConstructionSite.Tables("LesMesuresForage")

        '                '' 3. Mettre à jour la base de données AVANT de vider ou recharger
        '                'AdaptateurConstructionSite.Update(MonConstructionSite, "LesMesuresForage")

        '                ' Mesures.count = MonConstructionSite.Tables("LesMesuresForage").Rows.Count
        '                'If  MesureInitCount <   Mesures.count Then
        '                '    ' On ajoute un enregistrement à la Table
        '                'End If


        '                'AdaptateurConstructionSite.Update(MonConstructionSite, "LesMesuresForage")
        '                'MsgBox("Update -LesMesuresForage- successful")
        '                'MesureInitCount = MonConstructionSite.Tables("LesMesuresForage").Rows.Count


        '                'AdaptateurConstructionSite.Fill(MonConstructionSite, "LesMesuresForage")

        '                ' 4. Optionnel : recharger la table après la mise à jour
        '                MonConstructionSite.Tables("LesMesuresForage").Clear()
        '                'AdaptateurConstructionSite.Fill(MonConstructionSite, "LesMesuresForage")

        '                'AdaptateurConstructionSite.Update(MonConstructionSite, "LesMesuresForage")
        '                'MsgBox("Update -LesMesuresForage- successful")
        '                ' MesureInitCount = MonConstructionSite.Tables("LesMesuresForage").Rows.Count

        '                ''MonConstructionSite.Tables(3).Clear()
        '                ''AdaptateurConstructionSite.Fill(MonConstructionSite, "LesMesuresForage")

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonConstructionSite.Tables("LesMesuresForage").Clear()
        '                'AdaptateurConstructionSite.Fill(MonConstructionSite, "LesMesuresForage")
        '            End If

        '             IsSaved = True
        '            EnregistreConstructionSite = True


        '            RaiseEvent HasBeenModified()
        '            Exit Function

        'errFind:

        '            ' En cas d'erreur on édite la Table
        '            EnregistreConstructionSite = False
        '            MsgBox(Err.Description)
        '            'Dim Message As String
        '            'Debug.Print(Message)
        '            ''MsgBox(Erreur.Message)
        '            'Select Case iTable
        '            '    'Case 0 : FrmTable = MyConstructionSite.
        '            '    Case 1 : FrmTable = MyConstructionSite.  ListForages
        '            '    Case 2
        '            'End Select
        '            'FrmTable = New FrmTable
        '            'FrmTable.ShowDialog()
        '        End Function

        'Public Sub New()
        '    ' Enregistrement de la référence du ConstructionSite auprès de sa feuille
        '    'Call MyFenetreConstructionSite.SetMyConstructionSite(Me)
        '    ' MyConstructionSite = Me

        '     NomConstructionSite = "Nouveau ConstructionSite"
        '     RefChantier = "à remplir"
        '     DescriptifConstructionSite = "à remplir"
        '     RefClient = "à remplir"
        '     Entreprise = "à remplir"
        '     Adresse = "à remplir"
        '     Telephone = "à remplir"
        '    IsSaved = True
        '    MyFile = ""
        '    MyFileTitle = "ConstructionSite"

        'End Sub



        'Public Sub SelectMyNode()
        '    MyNodeConstructionSite.Checked = True
        'End Sub

        'Public Function GiveMeANewRefForage()
        '    ' Détermination d'une nouvelle référence pour un Forage
        '    Dim k As Integer

        '    '    MsgBox "GiveMeANewRefForage"

        '    k = 1

        '    Do While Not MyConstructionSite.IsRefUnique(" refForage " & k)
        '        k = k + 1
        '    Loop
        '    GiveMeANewRefForage = " refForage " & k

        'End Function


    End Class



End Namespace

