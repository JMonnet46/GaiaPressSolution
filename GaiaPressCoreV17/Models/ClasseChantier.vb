Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Models

    Public Class Chantier

        ' Propriétés métiers (avec préfixe  )
        <Key>
        Public Property refChantier As String
        Public Property nomChantier As String
        Public Property descriptifChantier As String
        Public Property refClient As Integer?
        Public Property entreprise As String
        Public Property adresse As String
        Public Property telephone As String
        Public Property reference As String

        Public FichierChantier As String
        Public MyFenetreChantier As ChantierLogic
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
            ' Génère un événement indiquant que des modifications du Chantier ont eu lieu
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
            ' Lier le forage au chantier (si nécessaire)
            forage.RefChantier = Me.refChantier
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
            ' Lier le forage au chantier (si nécessaire)
            essai.RefChantier = Me.refChantier
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
                If E.refEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function


        ''' Met à jour un essai de forage avec toutes ses propriétés, y compris les objets complexes.
        Public Sub MettreAJourEssaiForage(EssaiModifie As Essai_Forage)

            Dim Essai As Essai_Forage = GetessaiForage(EssaiModifie.refEssai)

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
            ' Lier le forage au chantier (si nécessaire)
            essai.RefChantier = Me.refChantier
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
                If E.refEssai = refEssai Then
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
            ' Exemple pour une liste dans un chantier :
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
            Dim SondeAModifier As Probe = GetSonde(NouvelleSonde.refSonde)

            If SondeAModifier IsNot Nothing Then
                ' Mise à jour des propriétés de la sonde
                SondeAModifier.MaExpansion = NouvelleSonde.MaExpansion
                SondeAModifier.MaResistance = NouvelleSonde.MaResistance
                SondeAModifier.refSonde = NouvelleSonde.refSonde
                'SondeAModifier.NomSond = NouvelleSonde.NomSonde
                SondeAModifier.ClassPrecis = NouvelleSonde.ClassPrecis
                SondeAModifier.refEssai = NouvelleSonde.refEssai
                SondeAModifier.typeSonde = NouvelleSonde.typeSonde
                SondeAModifier.typeDeformee = NouvelleSonde.typeDeformee
                SondeAModifier.typeContrainte = NouvelleSonde.typeContrainte
                SondeAModifier.ls = NouvelleSonde.ls
                SondeAModifier.lg = NouvelleSonde.lg
                SondeAModifier.lt = NouvelleSonde.lt
                SondeAModifier.longInjectePompage = NouvelleSonde.longInjectePompage
                SondeAModifier.Vs = NouvelleSonde.Vs
                SondeAModifier.ds = NouvelleSonde.ds
                SondeAModifier.ClassPrecis = NouvelleSonde.ClassPrecis
                SondeAModifier.EpMembraneGarde = NouvelleSonde.EpMembraneGarde
                SondeAModifier.PrEpMembraneGarde = NouvelleSonde.PrEpMembraneGarde
                SondeAModifier.EpMembraneMesure = NouvelleSonde.EpMembraneMesure
                SondeAModifier.PrEpMembraneMesure = NouvelleSonde.PrEpMembraneMesure
                SondeAModifier.epTubeFendu = NouvelleSonde.epTubeFendu
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

        ' Constructeur de Chantier
        Public Sub New()
            ' Initialisation des collections avec List(Of T)
            ListForages = New List(Of Forage)
            ListEssaiForages = New List(Of Essai_Forage)
            ListMesuresForage = New List(Of MesureEssaiForage)
            ListSols = New List(Of Sol)
            ListSondes = New List(Of Probe)

            ' Initialisation des propriétés par défaut
            FichierChantier = "à remplir"
            refChantier = "à remplir"
            nomChantier = "Nouveau chantier"
            descriptifChantier = "à remplir"
            refClient = "à remplir"
            entreprise = "à remplir"
            adresse = "à remplir"
            telephone = "à remplir"
            reference = "à remplir"

            ' Initialisation des autres propriétés
            MyFile = ""
            MyFileTitle = "à remplir"
            DataSetName = "à remplir"
            EventsSupendus = True

            ' ❌ Supprimez cette ligne (référence à Windows Forms)
            '  MyNodeChantier = New TreeNode
        End Sub

        '        ' *******************************************************************************
        '        ' Lecture des Chantier
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
        '            Dim NewMesure As MesureEssaiForage
        '            Dim ReferenceSonde, Accu As String

        '            ' Dim Sonde, Sonde1, Sonde2 As Probe

        '            PasseForage = False
        '            PasseSonde = False

        '            On Error GoTo errFind   ' En cas d'erreur, aller à la section errFind (en fin de fonction)

        '            If MyChantier Is Nothing Then MyChantier = New Chantier
        '            If MyForage Is Nothing Then MyForage = New Forage

        '            ' Appel de la procédure de connection à la base de données

        '            If Not (ChargeChantier) Then

        '                ' Netoyage de l'AdaptateurChantier
        '                ' AdaptateurChantier.clear()

        '                ' Création d'une connection avec la base de données de type Access 2000

        '                ' Création d'une connection avec la base de données de type Access 2000

        '                ' Ouverture de la connection avec la BD

        '                ' *********************************************************************
        '                ' Lecture de la Table "Chantier"
        '                ' *********************************************************************

        '                'ma requete
        '                'Requete = "SELECT refChantier AS  refChantier, NomChantier AS  NomChantier, descriptifChantier AS  descriptifChantier, " _
        '                '    & "refClient AS  refClient, entreprise AS  entreprise, adresse AS  adresse, telephone AS  telephone, reference AS  reference FROM Chantier"
        '                Requete = "SELECT refChantier , NomChantier , descriptifChantier , " &
        '                          "refClient , entreprise , adresse , telephone , reference  FROM Chantier"

        '                'Requete = "SELECT * FROM Chantier"

        '                'definition de la commande

        '                ' Définition de l'AdaptateurChantier

        '                ' Nettoyage de MonChantier avant le chargement
        '                MonChantier.Tables("Chantier")?.Clear()

        '                ' rempli le dataset depuis la BD vers la Table "Chantier" de MonDataSet

        '                ' On remplit les ObjetTables avec les Tables de MonChantier
        '                ObjetDataLesChantiersDb = MonChantier.Tables("Chantier")

        '                ' Mise à jour du compteur d'enregistrements

        '                'Me. Chantier.Count = MonChantier.Tables("Chantier").Rows.Count

        '                Select Case Lang
        '                    Case 0
        '                        MsgBox("Reading of the Table 'Chantier' suceeded")
        '                    Case 1
        '                        MsgBox("Lecture de la Table 'Chantier' réussie")
        '                End Select
        '                Select Case Lang
        '                    Case 0
        '                        MsgBox("Reading of the Table 'Chantier' suceeded")
        '                    Case 1
        '                        MsgBox("Lecture de la Table 'Chantier' réussie")
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
        '                ObjetDataLesForages = MonChantier.Tables("LesForages")

        '                ' La Table "LesForages" est chargée dans MonChantier

        '                ' Mise à jour du compteur d'enregistrements
        '                ' ListForages.Count = MonChantier.Tables("LesForages").Rows.Count
        '                ' ForageInitCount = MonChantier.Tables("LesForages").Rows.Count
        '                'With MonChantier.Tables("Chantier").Rows(0)
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
        '                ' Lecture de la Table "LesEssaiForage"
        '                ' *********************************************************************
        '                'ma requete
        '                If Not (AncienChantier) Then
        '                    'Requete = "SELECT  refChantier,  refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, PrecisVolSonde, nomEssai, dateEssai, heureEssai,difPresEauAir, hauteurCpv, " _
        '                    '   & "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " _
        '                    '   & "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"

        '                    Requete = "SELECT refChantier , refForage , refEssai , refSonde , refEssaiResistancePropre , refEssaiExpansionPropre , refEssaiPerteDeCharge , " &
        '                         "refSol , Enregistreur , Operateur , VolSonde , ClassPrecis , PrecisVolSonde , nomEssai , dateEssai ,heureEssai , difPresEauAir , " &
        '                         "hauteurCpv , presP0 , presFluage , presLimite , presLimiteNf , presLimiteTheo , " &
        '"PrecisVolume , PrecisPression , poidsVol , poidsVolDejauge , coefPoisson , modPressio , " &
        '"modPressioNf , modElastique , modElastiqueNf , cohesion , frottement , frottementintergranulaire , " &
        '"U0 , profNappe , profSonde , coteZeroGeomeca FROM LesEssaiForage"
        '                Else
        '                    Requete = "SELECT refChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                        "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '"U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"
        '                End If
        '            Else
        '                Requete = "SELECT refChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                    "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                    "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"
        '            End If
        '            Else
        '            Requete = "SELECT refChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"
        '            End If

        '            'definition de la commande

        '            ' Définition de l'AdaptateurChantier

        '            ' On crée la Table "LesEssaiForage"
        '            'If Not MonChantier.Tables.Contains("LesEssaiForage") Then
        '            '    ' Dim table As New DataTable("LesEssaiForage")
        '            '    ' MonChantier.Tables.Add(table)
        '            '    Result = CreerTableLesEssaiForage(MonChantier, ConnChantier)
        '            '    If Not (Result) Then Exit Sub
        '            'End If

        '            ' Mise à jour du compteur d'enregistrements
        '            ' ListForages.Count = MonChantier.Tables("LesForages").Rows.Count

        '            ' Nettoyage de MonChantier avant le chargement
        '            MonChantier.Tables("LesEssaiForage")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaiForage" de MonDataSet

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaiForage" de MonDataSet

        '            ' AdaptateurChantier.FillSchema(MonChantier.Tables("LesEssaiForage"), SchemaType.Source)

        '            ' rempli le dataset depuis la BD vers la Table "LesEssaiForage" de MonDataSet
        '            ' AdaptateurChantier.Fill(MonChantier.Tables("LesEssaiForage"))

        '            ' AdaptateurChantier.FillSchema(MonChantier, SchemaType.Source, "LesEssaiForage")
        '            'AdaptateurChantier.Fill(MonChantier, "LesEssaiForage")
        '            ObjetDataEssai = MonChantier.Tables("LesEssaiForage")

        '            ' AdaptateurChantier.FillSchema(MonChantier, SchemaType.Source, "LesEssaiForage")
        '            'AdaptateurChantier.Fill(MonChantier, "LesEssaiForage")
        '            ObjetDataEssai = MonChantier.Tables("LesEssaiForage")

        '            ' Mise à jour du compteur d'enregistrements
        '            ' EssaiCount = MonChantier.Tables("LesEssaiForage").Rows.Count
        '            ' EssaiInitCount = MonChantier.Tables("LesEssaiForage").Rows.Count

        '            ' Mise à jour du compteur d'enregistrements
        '            ' EssaiCount = MonChantier.Tables("LesEssaiForage").Rows.Count
        '            ' EssaiInitCount = MonChantier.Tables("LesEssaiForage").Rows.Count

        '            'Vérificatio de la Table 
        '            For i = 0 To  ListEssaiForages.Count - 1
        '                'Debug.WriteLine("MonChantier.Tables(LesEssaiForage).Rows(" & i.ToString & "(RefForage)=" & MonChantier.Tables("LesEssaiForage").Rows(i).Item("refForage").ToString)
        '                'Debug.WriteLine("MonChantier.Tables(LesEssaiForage).Rows(" & i.ToString & "(RefEssai)=" & MonChantier.Tables("LesEssaiForage").Rows(i).Item("refEssai").ToString)
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

        '            ' Définition de l'AdaptateurChantier

        '            ' Nettoyage de MonChantier avant le chargement
        '            MonChantier.Tables("LesMesuresForage")?.Clear()

        '            ' Nettoyage de MonChantier avant le chargement
        '            MonChantier.Tables("LesMesuresForage")?.Clear()

        '            ' rempli le dataset depuis la BD vers la Table "LesForages" de MonDataSet

        '            ObjetDataMesures = MonChantier.Tables("LesMesuresForage")

        '            '' Mise à jour du compteur d'enregistrements
        '            '  Mesures.count = MonChantier.Tables("LesMesuresForage").Rows.Count
        '            ' MesureInitCount = MonChantier.Tables("LesMesuresForage").Rows.Count

        '            Select Case Lang

        '                Case 0
        '                    MsgBox("Reading of the Table 'LesMesuresForage' suceeded")
        '                Case 1
        '                    MsgBox("Lecture de la Table 'LesMesuresForage' réussie")
        '            End Select

        '            MyChantier = New Chantier

        '            '' **************************************************************************************
        '            '' Rangement dans MyChantier des objets de la Tables("Chantier") de MonChantier
        '            '' **************************************************************************************
        '            '' i est le numéro du Chantier
        '            ''For Each ChantierRow As DataRow In MonChantier.Tables("Chantier").Rows

        '            '' Nouveau Chantier
        '            'MyChantier = New Chantier

        '            '    MyChantier. refChantier = ChantierRow("refChantier").ToString()
        '            '    MyChantier. nomChantier = ChantierRow("NomChantier").ToString()
        '            '    MyChantier. descriptifChantier = ChantierRow("descriptifChantier").ToString
        '            '    MyChantier. refClient = ChantierRow("refClient").ToString
        '            '    MyChantier. entreprise = ChantierRow("entreprise").ToString
        '            '    MyChantier. adresse = ChantierRow("adresse").ToString
        '            '    MyChantier. telephone = ChantierRow("telephone").ToString

        '            '    'With MonChantier.Tables("Chantier").Rows(i)
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

        '            '        new_Forage. RefChantier = forageRow("refChantier").ToString()
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
        '            '        'MyForage = MyChantier. ListForages(forageIndex - 1)
        '            '        forageIndex += 1

        '            '        ' **************************************************************************************
        '            '        ' Rangement dans MyForage des objets de la Tables("LesEssaiForage") de MonChantier
        '            '        ' **************************************************************************************

        '            '        ' k est le numéro de l'EssaiForage
        '            '        kMax = MonChantier.Tables("LesEssaiForage").Rows.Count

        '            '        Dim EssaiIndex As Integer = 1

        '            '        For Each essaiRow As DataRow In MonChantier.Tables("LesEssaiForage").Rows

        '            '            If essaiRow("refForage").ToString() = new_Forage. RefForage Then
        '            '                Dim new_Essai As New Essai_Forage(False)
        '            '                Dim Verif As String
        '            '                Verif = essaiRow("refEssai").ToString()
        '            '                new_Essai. RefEssai = essaiRow("refEssai").ToString()
        '            '                new_Essai. RefChantier = essaiRow("refChantier").ToString
        '            '                new_Essai. RefForage = essaiRow("refForage").ToString
        '            '                new_Essai. RefEssai = essaiRow("refEssai").ToString
        '            '                'Accu = "MonChantier.Tables(LesEssaiForage).Rows(" & k.ToString & ")=" & essaiRow("refEssai").ToString
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

        '            '                'ReDim Preserve MyChantier. ListEssaiForages(k)
        '            '                'MyChantier. ListEssaiForages(k) = MyForage. ListEssais(l - 1)

        '            '                ' **************************************************************************************
        '            '                ' Rangement dans MyEssaiForage des objets de la Tables("LesMesuresForage") de MonChantier
        '            '                ' **************************************************************************************

        '            '                '' Affectation de la Liste des Sondes à MyChantier
        '            '                '    RowSonde = New Probe
        '            '                '    RowSonde = MyListeSonde.ListeSonde(i)
        '            '                '    If MyChantier. ListeSonde.Count - 1 <= i Then
        '            '                '        ReDim Preserve MyChantier. ListeSonde(i)
        '            '                '        MyChantier. ListeSonde(i) = New Probe
        '            '                '    End If
        '            '                '    MyChantier. ListeSonde(i) = RowSonde
        '            '                'Next i

        '            '                '' Affectation de la Liste des Sondes à MyChantier
        '            '                '    RowSonde = New Probe
        '            '                '    RowSonde = MyListeSonde.ListeSonde(i)
        '            '                '    If MyChantier. ListeSonde.Count - 1 <= i Then
        '            '                '        ReDim Preserve MyChantier. ListeSonde(i)
        '            '                '        MyChantier. ListeSonde(i) = New Probe
        '            '                '    End If
        '            '                '    MyChantier. ListeSonde(i) = RowSonde
        '            '                'Next i


        '            '                ' m est le numéro des MesuresForage
        '            '                m = 0
        '            '                For Each mesureRow As DataRow In MonChantier.Tables("LesMesuresForage").Rows

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
        '            '                            PressionNF = MonChantier.Tables("LesMesuresForage").Rows(m)("PNF")
        '            '                            VolumeNF = MonChantier.Tables("LesMesuresForage").Rows(m)("VNF")
        '            '                            'Debug.WriteLine("MonChantier.Tables(LesMesuresForage).Rows(" & m.ToString() & ").PNF= " & PressionNF.ToString())
        '            '                            'Debug.WriteLine("MonChantier.Tables(LesMesuresForage).Rows(" & m.ToString() & ").VNF= " & VolumeNF.ToString())
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

        '            '                Next mesureRow ' Fin de For m = 0 To MonChantier.Tables("LesMesuresForage").Rows().Count - 1
        '            '            End If ' fin du If essaiRow("refForage").ToString() = new_Forage.RefForage Then
        '            '        Next essaiRow
        '            '        ' --------------------------------------------
        '            '        ' MyEssaiForage. MesureEssaiForage est juste à ce niveau
        '            '        ' --------------------------------------------
        '            '    Next forageRow ' Fin du For Each forageRow As DataRow In MonChantier.Tables("LesForages").Rows
        '            'Next ChantierRow ' Fin du For Each row As DataRow In MonChantier.Tables("Chantier").Rows

        '            ChargeChantier = True

        '            'End If ' Fin de If Not (ChargeChantier) Then


        '            ' Sauvegarde du nom de fichier pour une future utilisation
        '            MyChantier. MyFile = FichierChantier

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

        '            ' Le chantier est considéré comme déjà sauvé
        '             IsSaved = True
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
            GetRefChantier = refChantier
        End Function

        Public Function GetNomChantier()
            GetNomChantier = nomChantier
        End Function


        Public Function GetDescriptifChantier()
            GetDescriptifChantier = descriptifChantier
        End Function

        Public Function GetRefClient()
            GetRefClient = refClient
        End Function

        Public Function GetEntreprise()
            GetEntreprise = entreprise
        End Function

        Public Function GetAdresse()
            GetAdresse = adresse
        End Function

        Public Function GetTelephone()
            GetTelephone = telephone
        End Function

        Public Function GetForagesCount()
            GetForagesCount = ListForages.Count
        End Function

        Public Sub SetRefChantier(ByVal ref As String)
            refChantier = ref
        End Sub


        Public Sub SetNomChantier(ByVal nom As String)
            nomChantier = nom
        End Sub

        Public Sub SetDescriptifChantier(ByVal descriptif As String)
            descriptifChantier = descriptif
        End Sub

        Public Sub SetRefClient(ByVal ref As String)
            refClient = ref
        End Sub

        Public Sub SetEntreprise(ByVal entrep As String)
            entreprise = entrep
        End Sub

        Public Sub SetAdresse(ByVal adr As String)
            adresse = adr

            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSupendus Then
                RaiseEvent HasBeenModified()
            End If
        End Sub

        Public Sub SetTelephone(ByVal tel As String)
            telephone = tel

            ' Génère un événement indiquant que des modifications ont eu lieu
            If Not EventsSupendus Then
                RaiseEvent HasBeenModified()
            End If
        End Sub


        ' Affichage du nouveau chantier dans l'arborescence des Chantier
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
        ''  ReDim Preserve  MyListeForageDb( ListForages.Count)
        '  ListForages.Count = MyChantier.  ListForages.Count
        'If   ListForages.Count > 0 Then
        '    For element = 0 To (MyChantier.  ListForages.Count - 1)
        '        MyForage = New Forage(True)
        '        MyForage = MyChantier.  ListForages(element)
        '        ' Ajout du noeud Parent Forage
        '        TreeViewArboParent = New TreeNode(MyChantier.  ListForages(element).NomForage)
        '        TreeViewArboParent.Name = MyChantier.  ListForages(element).NomForage
        '        TreeViewArboRoot.Nodes.Add(TreeViewArboParent)
        '        MyForage.RefForage = MyChantier.  ListForages(element).RefForage
        '        MyForage.NomForage = MyChantier.  ListForages(element).NomForage
        '        MyForage.RefChantier = MyChantier. refChantier
        '        Call   ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)
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
        '     nomChantier = MyChantier.NomChantier

        '    ' Ajout du noeud racine
        '    TreeViewArboRoot = New TreeNode( nomChantier)
        '    TreeViewArboRoot.Name =  nomChantier
        '    Arbo.Nodes.Add(TreeViewArboRoot)

        '    ' Affichage du Chantier
        '     ListForages.Count = MyChantier. ListForages.Count

        '    If  ListForages.Count > 0 Then
        '        For element As Integer = 0 To  ListForages.Count - 1
        '            ' Vérifiez que MyChantier.  ListForages(element) n'est pas Nothing
        '            If MyChantier. ListForages(element) IsNot Nothing Then
        '                ' Ajout du noeud Parent Forage
        '                Dim TreeViewArboParent As TreeNode = New TreeNode(MyChantier. ListForages(element).NomForage)
        '                MyForage = New Forage(True)
        '                MyForage = MyChantier. ListForages(element)
        '                ' Ajout du noeud Parent Forage
        '                TreeViewArboParent = New TreeNode(MyChantier. ListForages(element).NomForage)
        '                TreeViewArboParent.Name = MyChantier. ListForages(element).NomForage
        '                TreeViewArboRoot.Nodes.Add(TreeViewArboParent)
        '                MyForage.RefForage = MyChantier. ListForages(element).RefForage
        '                MyForage.NomForage = MyChantier. ListForages(element).NomForage
        '                MyForage. RefChantier = MyChantier. RefChantier
        '                'Call   ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)

        '                ' Appeler la méthode AfficheDansArbo de l'objet Forage
        '                MyChantier. ListForages(element).AfficheDansArbo(Arbo, TreeViewArboParent)
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
        '                    MsgBox(" Error during loading of borehole " &  ListForages(i).GetRefForage & " ", Err.Description)
        '                Case 1
        '                    MsgBox(" Erreur lors de l'ouverture du forage " &  ListForages(i).GetRefForage & " ", Err.Description)
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
        '        MyChantier. ListForages.Count = MyChantier. ListForages.Count + 1
        '    End If

        '    If MonChantier.Tables("LesForages") Is Nothing Then
        '        NewTable = True
        '    End If

        '    ' Redimensionnement du tableau dynamique de forages
        '    'ReDim Preserve MyChantier.  ListForages(  ListForages.Count - 1)

        '    ' On ajoute un Nouveau Forage à la Liste des Forages de MyChantier
        '    Dim nouveauForage As New Forage
        '    MyChantier.AddForage(nouveauForage,  ListForages.Count)
        '    ' Instanciation du nouveau forage
        '    If NewForage Is Nothing Then

        '        ' NewForage = nouveauForage
        '        If MyChantier. ListForages.Count > 0 And (MyChantier. ListForages( ListForages.Count - 1) IsNot Nothing) Then
        '            NewForage = MyChantier. ListForages( ListForages.Count - 1)
        '        Else
        '            NewForage = nouveauForage
        '            ' ListForages.Count =  ListForages.Count + 1
        '        End If
        '    End If

        '    ' Enregistrement du nouveau forage dans le tableau de forage
        '    MyChantier. ListForages( ListForages.Count - 1) = NewForage

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

        '    'MyChantier. ListFyorages(  ListForages.Count - 1) = NewForage
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
            Do While ((i < (MyChantier.ListForages.Count) - 1) And (MyChantier.ListForages(i).NomForage <> Nom))
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
            Do While ((i < MyChantier.ListForages.Count) And (ListForages(i).GetRefForage <> ref))
                'Do While (i <   ListForages.Count) And (MyChantier.GetForage(i).GetRefForage <> ref)
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
        '    MyChantier.refForage = MyForage. RefForage
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

        ' Création d'un nouveau fichier à partir de MyChantier
        Public Function NouveauFichier(FichierChantier As String) As Boolean
            Dim NomFichierInitial As String
            Dim Racine As String
            NouveauFichier = False
            Racine = AppDomain.CurrentDomain.BaseDirectory
            Racine = System.IO.Path.GetFullPath(System.IO.Path.Combine(Racine, "..\..\..\.."))
            FichierChantier = Racine & "\Chantier\" & FichierChantier & ".mdb"
            'NomFichierInitial = FichierChantierousDossier(FichierChantier, "Chantier", "Ressources", ".mdb")
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

        '            ' On crée une commanDebuilder pour renvoyer les données à la BD

        '            On Error GoTo errFind

        '            ' Ouverture de la console
        '            ' System.Console.Title = "Debug Console"

        '            ' Ouverture de la connection avec la BD

        '            ' *********************************************************************
        '            ' Enregistrement de la Table "Chantier"
        '            ' *********************************************************************

        '            ''ma requete
        '            ' exemple plasserre.developpez.com/cours/vb-net : strSql = "SELECT FICHEPATIENT.* FROM FICHEPATIENT"
        '            'Requete = "SELECT refChantier AS  refChantier, NomChantier AS  NomChantier, descriptifChantier as  descriptifChantier, refClient as  refClient, entreprise as  entreprise, adresse as  adresse , telephone as  telephone, reference as  reference FROM Chantier"
        '            Requete = "SELECT refChantier , NomChantier , descriptifChantier , refClient , entreprise , adresse , telephone , reference FROM Chantier"

        '            ' Définition de l'AdaptateurChantier

        '            iTable = 0

        '            ' definition de la commande

        '            iTable = 0
        '            ObjetDataLesChantiersDb = MonChantier.Tables("Chantier")

        '            ' Définition de l'AdaptateurChantier

        '            'Pour modifier les valeurs changées dans le DataAdapter

        '            columns(0) = MonChantier.Tables("Chantier").Columns("refChantier")
        '            MonChantier.Tables("Chantier").PrimaryKey = columns

        '            iTable = 0
        '            ObjetDataLesChantiersDb = MonChantier.Tables("Chantier")

        '            ' ChantierCount = MonChantier.Tables("Chantier").Rows.Count
        '            'If  ChantierInitCount <  ChantierCount Then
        '            '    ' On ajoute un enregistrement à la Table
        '            'End If

        '            ' Définition de la clef primaire de la Table "Chantier"
        '            Dim columns() As DataColumn
        '            ReDim columns(7)

        '            columns(0) = MonChantier.Tables("Chantier").Columns("refChantier")
        '            MonChantier.Tables("Leantiers").PrimaryKey = columns

        '            If MonChantier.Tables("Chantier") IsNot Nothing Then
        '                'AdaptateurChantier.Update(MonChantier, "Chantier")
        '                'ChantierInitCount = MonChantier.Tables("Chantier").Rows.Count

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonChantier.Tables("Chantier").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "Chantier")

        '                ' Mise à jour de MyChantier
        '                If MonChantier.Tables("Chantier").Rows.Count > 0 Then

        '                    MyChantier. refChantier = MonChantier.Tables("Chantier").Rows(0).Item("refChantier").ToString

        '                    'AdaptateurChantier.Update(MonChantier, "Chantier")

        '                    '' On vide la Table et on la remplit à nouveau
        '                    'MonChantier.Tables("Chantier").Clear()
        '                    'AdaptateurChantier.Fill(MonChantier, "Chantier")
        '                End If

        '                MsgBox("Update -Chantier- successful")
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
        '            ObjetDataLesForages = MonChantier.Tables("LesForages")

        '            For Each row In MonChantier.Tables("LesForages").Rows
        '                Debug.WriteLine("Row State: " & row.RowState.ToString())
        '            Next

        '            If MonChantier.Tables("LesForages") IsNot Nothing Then
        '                ' ListForages.Count = MonChantier.Tables("LesForages").Rows.Count
        '                'If  ForageInitCount <  ListForages.Count Then
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
        '            ' Enregistrement de la Table "LesEssaiForage"
        '            ' *********************************************************************
        '            iTable = 2
        '            ObjetDataEssai = MonChantier.Tables("LesEssaiForage")

        '            ' Mise à jour de la Table "LesEssaiForage"
        '            ''ma requete

        '            'Requete = "SELECT  refChantier,  refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, ClassPrecis, PrecisVolSonde, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " _
        '            '   & "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " _
        '            '   & "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"
        '            Requete = "SELECT refChantier, refForage, refEssai, refSonde, refEssaiResistancePropre, refEssaiExpansionPropre, refEssaiPerteDeCharge, refSol, Enregistreur, Operateur, VolSonde, ClassPrecis, PrecisVolSonde, nomEssai, dateEssai, heureEssai, difPresEauAir, hauteurCpv, " &
        '                    "presP0, presFluage, presLimite, presLimiteNf, poidsVol, poidsVolDejauge, coefPoisson, modPressio, modPressioNf, modElastique, modElastiqueNf, cohesion, frottement, frottementintergranulaire, " &
        '                    "U0, profNappe, profSonde, coteZeroGeomeca FROM LesEssaiForage"

        '            iTable = 2
        '            ' Vérification de la Table
        '            With MonChantier.Tables("LesEssaiForage")
        '                For i = 0 To .Rows.Count - 1

        '                    row = MonChantier.Tables("LesEssaiForage").Rows(i)
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

        '            ' Définition de la clef primaire de la Table "LesEssaiForage"
        '            ReDim columns(11)
        '            columns(0) = New DataColumn
        '            columns(0) = MonChantier.Tables("LesEssaiForage").Columns(2)
        '            MonChantier.Tables("LesEssaiForage").PrimaryKey = columns

        '            '' Remise à jour de la Table "LesEssaiForage"
        '            'FrmForage.UpDateLesForages(MyChantier.refForage)

        '            'Pour modifier les valeurs changées dans le DataAdapter

        '            ' Adaptation du fichier Accès à la dimension de la Table
        '            NbAcces = MonChantier.Tables("LesEssaiForage").Rows.Count

        '            MsgBox("Update -LesEssaiForage- successful")
        '            ' EssaiInitCount = MonChantier.Tables("LesEssaiForage").Rows.Count


        '            ' Mise à jour de la Base de données Access 
        '            iTable = 1
        '            ObjetDataLesForages = MonChantier.Tables("LesEssaiForage")

        '            If MonChantier.Tables("LesEssaiForage") IsNot Nothing Then
        '                ' ListForages.Count = MonChantier.Tables("LesEssaiForage").Rows.Count
        '                'If  ForageInitCount <  ListForages.Count Then
        '                '    ' On ajoute un enregistrement à la Table
        '                'End If

        '                ' Définition de la clef primaire de la Table "LesEssaiForage"
        '                columns(0) = New DataColumn
        '                columns(0) = MonChantier.Tables("LesEssaiForage").Columns("refEssai")
        '                MonChantier.Tables("LesEssaiForage").PrimaryKey = columns

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonChantier.Tables("LesEssaiForage").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "LesEssaiForage")

        '                'AdaptateurChantier.Update(MonChantier, "LesEssaiForage")
        '                'MsgBox("Update -LesEssaiForage- successful")
        '                'ForageInitCount = MonChantier.Tables("LesEssaiForage").Rows.Count

        '                '' On vide la Table et on la remplit à nouveau
        '                'MonChantier.Tables("LesEssaiForage").Clear()
        '                'AdaptateurChantier.Fill(MonChantier, "LesEssaiForage")
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
        '            'FrmEssai.UpDateLesMesuresForages(MyEssaiForage.RefEssai)

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

        '                ' Mesures.count = MonChantier.Tables("LesMesuresForage").Rows.Count
        '                'If  MesureInitCount <   Mesures.count Then
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

        '             IsSaved = True
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
        '            '    Case 1 : FrmTable = MyChantier.  ListForages
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

        'Public Function GiveMeANewRefForage()
        '    ' Détermination d'une nouvelle référence pour un Forage
        '    Dim k As Integer

        '    '    MsgBox "GiveMeANewRefForage"

        '    k = 1

        '    Do While Not MyChantier.IsRefUnique(" refForage " & k)
        '        k = k + 1
        '    Loop
        '    GiveMeANewRefForage = " refForage " & k

        'End Function


    End Class


    'Public Class MyForageList
    '    Inherits BindingList(Of Forage)
    '    Public Ind As Integer
    '    'Public  ListForages.Count As Integer

    '    Public Sub New()
    '        MyListeForageDbSession = New List(Of Forage)() ' Initialisation
    '    End Sub

    '    Protected Overrides ReadOnly Property SupportsSearchingCore() As Boolean
    '        Get
    '            Return True
    '        End Get
    '    End Property

    '    Protected Overrides Function FindCore(ByVal prop As PropertyDescriptor,
    'ByVal key As Object) As Integer
    '        ' Ignore the prop value and search by family name.
    '        While Ind < MyChantier.ListForages.Count
    '            If Items(Ind).GetNomForage = CStr(key).ToLower() Then
    '                Return Ind
    '            End If
    '            Ind += 1
    '        End While

    '        Return -1
    '    End Function


    'End Class

End Namespace

