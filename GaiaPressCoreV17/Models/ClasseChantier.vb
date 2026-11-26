Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Services
Imports Microsoft.EntityFrameworkCore

Namespace GaiaPressCoreV17.Models

    Public Class Chantier

        ' Propriétés métiers (avec préfixe  )
        <Key>
        Public Property RefChantier As String
        Public Property NomChantier As String
        Public Property DescriptifChantier As String
        Public Property DateChantier As DateTime
        Public Property RefClient As String
        Public Property Entreprise As String
        Public Property Adresse As String
        Public Property Telephone As String
        Public Property Reference As String

        Public FichierChantier As String
        Public MyFenetreChantier As Chantier
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
        Public Sub MettreAJourForage(ForageModifie As Forage, context As GaiaPress_Db_Context)
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
                'MyForage = EssaiModifie.MyForageh
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
            RefChantier = "à remplir"
            NomChantier = "Nouveau chantier"
            DescriptifChantier = "à remplir"
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

        End Sub

        Public Function GetRefChantier()
            GetRefChantier = RefChantier
        End Function

        Public Function GetNomChantier()
            GetNomChantier = NomChantier
        End Function


        Public Function GetDescriptifChantier()
            GetDescriptifChantier = DescriptifChantier
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


        Public Sub SetNomChantier(ByVal nom As String)
            NomChantier = nom
        End Sub

        Public Sub SetDescriptifChantier(ByVal descriptif As String)
            DescriptifChantier = descriptif
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


        Public Sub SupForage(ByVal Ind As Integer)
            Dim J As Integer

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
            Do While ((i < MyChantier.ListForages.Count) And (ListForages(i).RefForage <> ref))
                'Do While (i <   ListForages.Count) And (MyChantier.GetForage(i).GetRefForage <> ref)
                i = i + 1
            Loop
            GetIndexRefForage = i
        End Function

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

    End Class

End Namespace

