' =============================================
' Fichier : ClasseModuleElasticite.vb
' Description : Module de calcul du module élastique avec accès aux données depuis la base SQL internet.
' Namespace : GaiaPressCoreV17.Models
' =============================================
Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Models.Dto ' ← IMPORT CRUCIAL POUR Dto.PrecisionElastDto
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Imports Microsoft.EntityFrameworkCore

''' <summary>
''' Module de calcul du module élastique avec accès aux données depuis la base SQL internet.
''' Toutes les fonctionnalités originales sont conservées.
''' </summary>
''' 
Namespace GaiaPressCoreV17.Models
    Public Class ClasseModuleElasticite

        ' =============================================
        ' 🔹 DÉPENDANCES (À CONSERVER)
        ' =============================================

        Private ReadOnly _context As GaiaPressDbContext
        Private ReadOnly _mesureForageService As ClasseListeMesureEssaiForage

        ''' <summary>
        ''' Service pour accéder aux mesures de forage en session
        ''' </summary>
        Public ReadOnly Property MesureForageService As ClasseListeMesureEssaiForage
            Get
                Return _mesureForageService
            End Get
        End Property

        ' =============================================
        ' 🔹 PROPRIÉTÉ POUR PRECISION (CORRIGÉE ICI)
        ' =============================================

        ''' <summary>
        ''' Variables de précision pour les calculs élastiques (DTO au lieu du type interne)
        ''' </summary>
        Public _precisionVariablesUnitaires As Dto.PrecisionElastDto

        ''' <summary>
        ''' Variables de précision pour les calculs élastiques
        ''' </summary>

        ' =============================================
        ' 🔹 CONSTRUCTEUR (CORRIGÉ ICI)
        ' =============================================

        ''' <summary>
        ''' Crée le calculateur et injecte le contexte EF Core.
        ''' </summary>
        ''' <param name="context">Contexte Entity Framework</param>
        ''' <param name="mesureForageService">Service pour les mesures de forage</param>
        Public Sub New(context As GaiaPressDbContext, mesureForageService As ClasseListeMesureEssaiForage)
            If context Is Nothing Then Throw New ArgumentNullException(NameOf(context))
            If mesureForageService Is Nothing Then Throw New ArgumentNullException(NameOf(mesureForageService))

            _context = context
            _mesureForageService = mesureForageService

            ' Initialisation de la précision avec des valeurs par défaut
            _precisionVariablesUnitaires = New Dto.PrecisionElastDto With {
            .PMax = 0.0F,
            .PMin = 0.0F,
            .VMax = 0.0F,
            .VMin = 0.0F
        }
        End Sub

        ' =============================================
        ' 🔹 MÉTHODES EXISTANTES (À CONSERVER)
        ' =============================================

        ''' <summary>Retourne les mesures de l'essai demandé.</summary>
        Public Function GetMesuresByRefEssai(refEssai As String) As List(Of MesureEssaiForage)
            If String.IsNullOrWhiteSpace(refEssai) Then
                Return New List(Of MesureEssaiForage)()
            End If

            Return _context.ListeMesureEssaiForageDb _
            .Where(Function(m) m.RefEssai = refEssai) _
            .ToList()
        End Function

        ''' <summary>Retourne le nombre de mesures de l'essai demandé.</summary>
        Public Function GetMesureCount(refEssai As String) As Integer
            If String.IsNullOrWhiteSpace(refEssai) Then Return 0
            Return _context.ListeMesureEssaiForageDb.Count(Function(m) m.RefEssai = refEssai)
        End Function

        '*********************************************
        'fonction de calcul du module pressiométrique
        '*********************************************
        Public Function ModuleEPressio(ByVal Pini As Single, ByVal Pfexp As Single, Essai As Essai_Forage) As Single

            Dim i, J, J1, k, Ndp As Integer
            Dim A0L, A1L, Moy, Sigma, Pmax, Dmoy As Single
            Dim PMin, VMax, VMin, DEm_db, DEm_V, PrecisB As Single
            Dim Dext, Nu, Pres, Def As Single
            Dim Erreur As Boolean
            Dim Resultat As Statistique
            Dim Regres As Regression
            Dim x() As Single
            Dim Y() As Single
            Dim essaiRef As String = Essai.refEssai
            Dim mesures As List(Of MesureEssaiForage) = GetMesuresByRefEssai(essaiRef)

            ' Initialisation
            ModuleEPressio = 0

            ' Création des tableaux de variables
            J = 10
            ReDim x(0 To J)
            ReDim Y(0 To J)
            J1 = 10

            Ndp = mesures.Count
            k = -1
            Pmax = 0
            ' Dext = Essai.MySonde.ds
            Dext = MySonde.ds
            Nu = Essai.coefPoisson

            'Calcul du module pressiométrique
            If Ndp > 1 Then

                For Each mesure As MesureEssaiForage In mesures

                    ' SetMesures(  Colonne, Ligne, Valeur)
                    '  Mesures(  Colonne, Ligne, Valeur)
                    Pres = mesure.ContrainteRadiale   ' Sigmar
                    Def = mesure.Deformation    ' Epsr

                    If Pmax < Pres Then
                        Pmax = Pres

                        If ((Pres <= Pfexp) _
                     And (Pres >= Pini)) Then
                            k = k + 1

                            If k > J Then
                                J = J + 10
                                ReDim Preserve x(0 To J)
                                ReDim Preserve Y(0 To J)
                            End If

                            x(k) = Def
                            Y(k) = Pres

                        End If '{ fin de IF (PMAX=Essai.mesures(4,i)) AND (Essai.Mesures(4,i)<=PF) AND (Essai.Mesures(4,i)>=Pini) THEN BEGIN }
                    End If
                Next mesure

                If k < 1 Then
                    Err.Raise(vbObjectError + 600, "Module Pressiométrique", "Calcul impossible: mesures insuffisantes dans l'intervalle de pression!")
                End If

                Pmax = Y(k)
                PMin = Y(0)
                VMax = x(k)
                VMin = x(0)

                If k > 1 Then

                    ' Calcul de la déformation moyenne
                    Resultat = MoyVariance(0, k, x)
                    Moy = Resultat.Moyenne
                    Sigma = Resultat.Variance

                    ' Calcul de la pente moyenne
                    Regres = RegresLine(k, x, Y)
                    A1L = Regres.A
                    A0L = Regres.B
                    PrecisB = Regres.SigB
                    Erreur = Regres.Erreur

                    ' Calcul du diamètre moyen
                    Dmoy = (1 + Moy) * Dext

                    'Calcul du module pressiométrique
                    ModuleEPressio = (1 + Nu) * A1L * (1 + Moy)

                    ' Calcul de la Précision sur les variables uniataires
                    'PrVs = (0.0625 * Pi ^ 2 * (Di / 10) ^ 4 * PrLong ^ 2 + 0.25 * (Pi * (Ls / 10) * (Di / 10) * PrDiam) ^ 2 + PrVol ^ 2) ^ 0.5
                    PrP = 0
                    If A1 * A2 * A3 <> 0 Then
                        PrP = (PrP ^ 2 + PrV ^ 2 / (A1 + 2 * A2 * Pe + 3 * A3 * Pe ^ 2) + (PoidsVol * PrProf) ^ 2) ^ 0.5
                    End If
                    'PrV = (PrV ^ 2 + (A * PrP) ^ 2) ^ 0.5
                    DEm_db = 1.33 * (1 + Moy)
                    DEm_V = 1.33 * (1 + A1L)

                    ' Calcul de la précision sur le module
                    PrEPressio = (DEm_db ^ 2 * PrecisB ^ 2 + DEm_V ^ 2 * (PrEps ^ 2) ^ 2 / k) ^ 0.5

                    ModuleEPressio = Math.Round(ModuleEPressio, 1)
                    PrEPressio = Math.Round(PrEPressio, 1)




                Else
                    Err.Raise(vbObjectError + 600, "Module Pressiométrique", "Calcul impossible: Nombre de points insuffisants pour une régréssion linéaire!")
                End If ' { fin de If K>1 Then Begin }
            Else
                Err.Raise(vbObjectError + 601, "Module pressiométrique", "Calcul impossible: Nombre de points insuffisants!")
            End If
        End Function

        '******************************************
        'fonction de calcul du module élastique
        '******************************************
        Public Function ModuleEelast(ByVal PDebC As Single, ByVal PFinC As Single, ByVal Pfexp As Single, refEssai As String) As Single
            Dim ModuleD, ModuleR, PMax, Pres, Def, PMin, Moy, Sigma, A0L, A1L, Precis, VMax, VMin, Deb, DEEpsr As Single
            Dim i, J1, K1, NbCycle, L, iMax, iMin As Integer
            Dim DebutDecharge, FinDecharge, Cycle, Erreur As Boolean
            Dim X(), Y(), X1(), Y1() As Single
            Dim Resultat As Statistique
            Dim Regres As Regression

            ' ✅ Remplacez les tableaux locaux par les données depuis la base
            Dim essais = _context.ListeEssaiForageDb.Where(Function(e) e.refEssai = refEssai).ToList()

            If essais.Count = 0 Then
                Err.Raise(vbObjectError + 604, "Module élastique", "Essai non trouvé!")
                Return 0
            End If

            Dim Essai As Essai_Forage = essais(0)
            Dim Ndp As Integer = essais.Count

            ' ✅ Remplacez les tableaux locaux par des requêtes LINQ
            Dim mesures = _context.ListeEssaiForageDb.Where(Function(m) m.refEssai = refEssai).ToList()


            ' Initialisation
            Dim Eelast As Single = 0
            Dim EelastD As Single = 0
            Dim EelastR As Single = 0
            Dim Dext As Single = Essai.MySonde.ds ' Assurez-vous que MySonde est accessible
            Dim Nu As Single = Essai.coefPoisson
            Dim k As Integer = -1
            Dim J As Integer = 10

            ' ✅ Utilisez les mesures depuis la base
            ReDim X(J)
            ReDim Y(J)

            ' Initialisation
            ModuleEelast = 0
            ModuleD = Nothing
            ModuleR = Nothing

            Ndp = mesures.Count

            ' Création des tableaux de variables
            J = 10
            J1 = 10
            DebutDecharge = False
            FinDecharge = False

            'Pression et déformation au déchargement
            ReDim X(0 To J)
            ReDim Y(0 To J)

            'Pression et déformation au rechargement
            ReDim X1(0 To J1)
            ReDim Y1(0 To J1)

            'Initialisations
            Ndp = GetMesureCount(Essai.refEssai)
            k = -1
            K1 = -1
            PMax = 0

            ' Dext = Essai.MySonde.Ds
            Dext = MySonde.ds

            Nu = Essai.coefPoisson

            NbCycle = -1
            Cycle = False
            EelastD = 0
            EelastR = 0


            L = 0

            If Ndp > 1 Then

                'Etude du module élastique au déchargement
                DebutDecharge = False
                i = 0
                'On commence au second point pour pouvoir comparer au précédent
                For Each mesure As MesureEssaiForage In _mesureForageService.ListeMesureEssaiForageSession
                    i = i + 1
                    ' Pression
                    ' SetMesures( Ligne, Colonne, Valeur)
                    Pres = mesure.ContrainteRadiale

                    ' Volumes
                    ' SetMesures( Ligne, Colonne, Valeur)
                    Def = mesure.Deformation

                    If Pres >= PFinC Then
                        DebutDecharge = True
                    End If

                    If i > 1 Then
                        If Pres >= Valeurs(Essai.Mesures(i).ContrainteRadiale) And Valeurs(Essai.Mesures(i - 1).ContrainteRadiale) < Valeurs(Essai.Mesures(i - 2).ContrainteRadiale) And DebutDecharge Then
                            FinDecharge = True
                            DebutDecharge = False
                        End If
                    End If

                    If PMax <= Pres Then

                        ' Cas du chargement
                        ' Mémorisation du maximum
                        PMax = Pres
                        iMax = i

                    Else

                        ' Cas du déchargement
                        'Est ce que la pression est comprise entre PdebC et PfinC en décharement
                        ' GetMesures( Ligne, Colonne, Valeur)
                        If (Valeurs(Essai.Mesures(i - 1).ContrainteRadiale) > Pres) And (Pres <= PFinC) And (Pres >= PDebC) _
                        And DebutDecharge And Not (FinDecharge) Then

                            'Initialisation du premier minimum
                            If k < 0 Then
                                PMin = Pres
                                iMin = i
                            End If

                            If PMin >= Pres Then

                                k = k + 1

                                'Redimensionnement éventuel
                                If k > J Then
                                    J = J + 10
                                    ReDim Preserve X(0 To J)
                                    ReDim Preserve Y(0 To J)
                                End If

                                'Mémorisation du minimum du cycle calculé
                                PMin = Pres
                                iMin = i

                                'On mémorise les valeurs à calculer
                                X(k) = Def
                                Y(k) = Pres
                                Cycle = True
                            End If

                        Else

                            ' Cas du rechargement

                            If (Essai.Mesures(i - 1).ContrainteRadiale > Pres) And (Pres > PFinC) And Cycle Then DebutDecharge = False

                            If (Valeurs(Essai.Mesures(i - 1).ContrainteRadiale) <= Pres) And (Pres <= PFinC) And (Pres >= PDebC) _
                           And FinDecharge Then

                                k = k + 1
                                'Redimensionnement éventuel
                                If k > J Then
                                    J = J + 10
                                    ReDim Preserve X(0 To J)
                                    ReDim Preserve Y(0 To J)
                                End If

                                'On mémorise les valeurs à calculer

                                X(k) = Def
                                Y(k) = Pres

                                '' A la fin d'un cycle on remet k = -1
                                'k = -1
                                'PMax = PMin
                                'iMax = iMin
                            End If '{ fin de If K>=2 Then Begin }


                        End If ' fin de IF (P(i-1)>Essai.Mesures(4,i)) And (Essai.Mesures(4,i)<=PDEBC) And (Essai.Mesures(4,i)>=PFINC)
                    End If ' Fin du test de chargement
                Next mesure ' { fin du For i:=1 to Ndp-1 Do Begin }

                'Fin du déchargement
                If k > -1 Then
                    If k > 0 Then
                        ' Calcul de la déformation moyenne
                        Resultat = MoyVariance(0, k, X)
                        Moy = Resultat.Moyenne
                        Sigma = Resultat.Variance

                        If Not (Resultat.Erreur) Then
                            ' Calcul de la pente moyenne
                            Regres = RegresLine(k, X, Y)
                            A1L = Regres.A
                            A0L = Regres.B
                            Precis = Regres.SigR
                            PrA = Regres.SigA
                            PrB = Regres.SigB
                            Erreur = Regres.Erreur

                            'Calcul du module élastique 
                            EelastD = (1 + Nu) * A1L * (1 + Moy)

                            'NbCycle = NbCycle + 1
                            'ReDim Preserve ModuleD(0 To NbCycle)
                            'ModuleD(NbCycle) = EelastD
                        End If
                    End If
                End If

                If Cycle Then
                    'Calcul de la déformation moyenne
                    'Resultat = MoyVariance(0, Cycle, ModuleD)
                    'EelastD = Resultat.Moyenne
                    Resultat = MoyVariance(0, k, X)
                    Moy = Resultat.Moyenne
                Else
                    Select Case Lang
                        Case 0
                            MsgBox("No cycle on loading")
                        Case 1
                            MsgBox("Pas de cycle au chargement")
                    End Select

                End If

                ' Calcul de la Précision sur les variables unitaires
                PMax = Y(k)
                PMin = Y(0)
                VMax = X(k)
                VMin = X(0)

                'PrVs = (0.0625 * Pi ^ 2 * MyExpansion.Di ^ 4 * PrLong ^ 2 + 0.25 * (Pi * MySonde.ls * MyExpansion.Di * PrDiam) ^ 2 + PrVol ^ 2) ^ 0.5
                PrP = 0
                If MyResistance.A1 * MyResistance.A2 * MyResistance.A3 <> 0 Then
                    PrP = (PrP ^ 2 + PrV ^ 2 / (MyResistance.A1 + 2 * MyResistance.A2 * MyResistance.Pe + 3 * MyResistance.A3 * MyResistance.Pe ^ 2) + (MyEssaiForage.poidsVol * PrProf) ^ 2) ^ 0.5
                    'PrEps = 0.001
                End If
                'PrV = (PrVol ^ 2 + (MyExpansion.coefficientA * PrPression) ^ 2) ^ 0.5
                Deb = 1.33 * (1 + Moy)
                DEEpsr = 1.33 * A1L
                ModuleEelast = 1.33 * (1 + Moy) * A1L

                ' Calcul de la précision sur le module
                PrEelast = (Deb ^ 2 * PrB ^ 2 + DEEpsr ^ 2 * PrEps ^ 2 / k) ^ 0.5


                PrEelast = Math.Round(PrEelast, 1)
                Essai.modElastique = Math.Round(ModuleEelast, 1)

            Else
                Err.Raise(vbObjectError + 604, "Module élastique", "Calcul impossible: Nombre de points insuffisants!")
            End If
        End Function

        '******************************************
        'fonction de calcul du module élastique NF
        '******************************************

        ' Calcul du diamètre moyen
        '
        '
        '   Vs = Pi * ((essai.GetMySonde.GetDs / 10) ^ 2 / 4) * (essai.GetMySonde.Getls / 10)
        '
        '                            Dmoy1 = (1 + Moy) * Dext
        '
        '                            'Calcul du module élastique de décharge
        '
        '                            EelastD = 2 * (1 + Nu) * (Vs + Moy) * A1L

        '
        '                            Dmoy1 = (1 + Moy) * Dext
        '
        '                            'Calcul du module élastique de Recharge
        '
        '                            Eelastr = 2 * (1 + Nu) * (Vs + Moy) * A1L

        Public Function ModuleEelastNF(ByVal PDebC As Single, ByVal PFinC As Single,
                Essai As Essai_Forage) As Single

            Dim i, J, J1, k, K1, L, NbCycle, Ndp, iMax, iMin As Integer
            Dim A0L, A1L, Moy, Sigma, Pmax, Pmin, Vs, Deb, DEEpsr, Precis As Single
            Dim EelastD, EelastR, Dext, Nu, Pres, Def, Vol As Single
            Dim VMax, VMin As Single
            Dim Erreur, Cycle As Boolean
            Dim DebutDecharge As Boolean
            Dim Resultat As Statistique
            Dim Regres As Regression
            Dim x() As Single
            Dim Y() As Single
            Dim X1() As Single
            Dim Y1() As Single
            Dim ModuleD() As Single
            Dim ModuleR() As Single

            ' Initialisation
            ModuleEelastNF = 0
            ModuleD = Nothing
            ModuleR = Nothing

            ' Création des tableaux de variables
            J = 10
            J1 = 10
            DebutDecharge = False

            'Pression et déformation au déchargement
            ReDim x(0 To J)
            ReDim Y(0 To J)

            'Pression et déformation au rechargement
            ReDim X1(0 To J1)
            ReDim Y1(0 To J1)

            'Initialisations
            Ndp = GetMesureCount(Essai.refEssai)
            k = -1
            K1 = -1
            Pmax = 0

            ' Dext = Essai.GetMySonde.GetDs
            Dext = MySonde.Getds
            Nu = Essai.coefPoisson

            NbCycle = -1
            Cycle = False
            EelastD = 0
            EelastR = 0

            L = 0

            'Vs = Pi * ((Essai.GetMySonde.GetDs / 10) ^ 2 / 4) * (Essai.GetMySonde.Getls / 10)
            MyExpansion = Essai.MaExpansion
            Vs = MyExpansion.Vs

            If Ndp > 1 Then

                'Etude du module élastique au déchargement
                DebutDecharge = False

                'On commence au second point pour pouvoir comparer au précédent
                For i = 1 To Ndp - 1

                    ' Pression NF
                    ' SetMesures( Ligne, Colonne, Valeur)
                    Pres = Essai.Mesures(i).ContrainteRadiale

                    ' Volumes NF
                    ' SetMesures( Ligne, Colonne, Valeur)
                    Def = Essai.Mesures(i).Deformation
                    Vol = Def

                    ' GetMesures( Ligne, Colonne, Valeur)
                    If (Essai.Mesures(i - 1).ContrainteRadiale > Pres) And (Pres < Pmax) And (Pres > PDebC) Then
                        DebutDecharge = True
                    End If

                    If Pmax <= Pres Then

                        ' Cas du chargement
                        ' Mémorisation du maximum
                        Pmax = Pres
                        iMax = i

                    End If

                    ' Cas du déchargement
                    ' Est ce que la pression est comprise entre PdebC et PfinC en déchargement
                    '  Mesures( Colonne, Ligne,  Valeur)
                    If (Essai.Mesures(i - 1).ContrainteRadiale > Pres) And (Pres <= PFinC) And (Pres >= PDebC) _
                    And DebutDecharge Then

                        'Initialisation du premier minimum
                        If k < 0 Then
                            Pmin = Pres
                            iMin = i
                        End If

                        If Pmin >= Pres Then

                            k = k + 1

                            'Redimensionnement éventuel
                            If k > J Then
                                J = J + 10
                                ReDim Preserve x(0 To J)
                                ReDim Preserve Y(0 To J)
                            End If

                            'Mémorisation du minimum du cycle calculé
                            Pmin = Pres
                            iMin = i

                            'On mémorise les valeurs à calculer
                            x(k) = Def
                            Y(k) = Pres
                        End If

                    Else

                        If (Essai.Mesures(i - 1).ContrainteRadiale <= Pres) And (Pres <= PFinC) And (Pres >= PDebC) And DebutDecharge Then

                            k = k + 1
                            'Redimensionnement éventuel
                            If k > J Then
                                J = J + 10
                                ReDim Preserve x(0 To J)
                                ReDim Preserve Y(0 To J)
                            End If

                            'On mémorise les valeurs à calculer

                            x(k) = Def
                            Y(k) = Pres

                            '' A la fin d'un cycle on remet k = -1
                            'k = -1
                            'PMax = PMin
                            'iMax = iMin
                        End If '{ fin de If K>=2 Then Begin }


                    End If ' Fin du test de chargement
                Next i ' { fin du For i:=1 to Ndp-1 Do Begin }

                'Fin du déchargement
                If k > -1 Then
                    If k > 0 Then
                        ' Calcul de la déformation moyenne
                        Resultat = MoyVariance(0, k, x)
                        Moy = Resultat.Moyenne
                        Sigma = Resultat.Variance

                        If Not (Resultat.Erreur) Then
                            ' Calcul de la pente moyenne
                            Regres = RegresLine(k, x, Y)
                            A1L = Regres.A
                            A0L = Regres.B
                            Precis = Regres.SigR
                            PrA = Regres.SigA
                            PrB = Regres.SigB
                            Erreur = Regres.Erreur

                            'Calcul du module élastique 
                            EelastD = (1 + Nu) * A1L * (1 + Moy)

                            'NbCycle = NbCycle + 1
                            'ReDim Preserve ModuleD(0 To NbCycle)
                            'ModuleD(NbCycle) = EelastD
                        End If
                    End If
                End If

                '    End If ' fin de IF (P(i-1)>Essai.Mesures(6,i)) And (Essai.Mesures(6,i)<=PDEBC) And (Essai.Mesures(6,i)>=PFINC)

                'Next i ' { fin du For i:=1 to Ndp-1 Do Begin }

                Pmax = Y(k)
                Pmin = Y(0)
                VMax = x(k)
                VMin = x(0)

                'PrVs = (0.0625 * Pi ^ 2 * MyExpansion.Di ^ 4 * PrLong ^ 2 + 0.25 * (Pi * MySonde.ls * MyExpansion.Di * PrDiam) ^ 2 + PrVol ^ 2) ^ 0.5
                PrP = 0
                If MyResistance.A1 * MyResistance.A2 * MyResistance.A3 <> 0 Then
                    PrP = (PrP ^ 2 + PrV ^ 2 / (MyResistance.A1 + 2 * MyResistance.A2 * MyResistance.Pe + 3 * MyResistance.A3 * MyResistance.Pe ^ 2) + (MyEssaiForage.poidsVol * PrProf) ^ 2) ^ 0.5
                    'PrEps = 0.001
                End If
                PrV = (PrV ^ 2 + (MyExpansion.coefficientA * PrP) ^ 2) ^ 0.5
                Deb = 1.33 * (1 + Moy)
                DEEpsr = 1.33 * A1L
                ModuleEelastNF = 1.33 * (1 + Moy) * A1L

                ' Calcul de la précision sur le module
                PrEelastNF = (Deb ^ 2 * PrB ^ 2 + DEEpsr ^ 2 * PrEps ^ 2 / k) ^ 0.5

                PrEelastNF = Math.Round(PrEelastNF, 1)
                Essai.modElastique = Math.Round(ModuleEelastNF, 1)

            Else
                Err.Raise(vbObjectError + 604, "Module élastique", "Calcul impossible: Nombre de points insuffisants!")
            End If
        End Function

        ' Calcul des dérivées partielles nécessaires au calcul de la précision du module pressio et de la pression limite
        ' iDebut est l'indice correspondant à V1
        ' iFin est l'indice correspondant à V2
        ' k est le nombre de points de mesure dans la partie linéaire de la relation V-1 = A.p + B

        Public Function PrecisionVariablesUnitaires(ByVal iDebut As Integer, ByVal iFin As Integer) As PrecisionElastDto

            ' Déclaration de toutes les variables nécessaires
            Dim Di, Ls, A0, A1, A2, A3, A, Pr1, Pr2, PV, Vr1, Vr2, Pe, PrVp As Single
            Dim Pmax, Pmin, Vmax, Vmin, VL, PrVl As Single
            Dim PrV, PrP, PrVs, PrLong, PrDiam, PrProf, PrA, PoidsVol As Single

            ' Récupération des mesures
            MyExpansion = MyEssaiForage.MaExpansion

            ' Vérification des indices
            If iDebut < 0 OrElse iDebut >= MyEssaiForage.Mesures.Count OrElse
       iFin < 0 OrElse iFin >= MyEssaiForage.Mesures.Count OrElse
       iDebut > iFin Then
                Throw New ArgumentException("Indices de mesures invalides")
            End If

            ' Récupération des valeurs de mesure
            Pmax = MyEssaiForage.Mesures(iFin).PNF
            Pmin = MyEssaiForage.Mesures(iDebut).PNF
            Vmax = MyEssaiForage.Mesures(iFin).VNF
            Vmin = MyEssaiForage.Mesures(iDebut).VNF
            Pr1 = MyEssaiForage.Mesures(iDebut).Pression
            Vr1 = MyEssaiForage.Mesures(iDebut).Volume60
            Pr2 = MyEssaiForage.Mesures(iFin).Pression
            Vr2 = MyEssaiForage.Mesures(iFin).Volume60

            ' Récupération des paramètres de l'essai
            Di = MyEssaiForage.MaExpansion.Di / 10
            Ls = MySonde.ls / 10
            Vs = MyExpansion.Vs
            A0 = MyEssaiForage.MaResistance.A0
            A1 = MyEssaiForage.MaResistance.A1
            A2 = MyEssaiForage.MaResistance.A2
            A3 = MyEssaiForage.MaResistance.A3
            A = MyEssaiForage.MaExpansion.coefficientA

            MyEssaiForage.MaResistance.CalculePel(Vr1)
            Pe = MyEssaiForage.MaResistance.Pe

            ' Poids volumique
            If MyEssaiForage.profSonde < MyEssaiForage.profNappe Then
                PoidsVol = MyEssaiForage.poidsVol
            Else
                PoidsVol = MyEssaiForage.poidsVolDejauge
            End If

            ' Paramètres de précision (valeurs par défaut ou à adapter selon votre contexte)
            PrLong = MySonde.PrLong
            PrDiam = MySonde.PrDiam
            PrProf = MySonde.PrZ
            PrA = MySonde.PrecisA
            PrVp = MySonde.PrecisVc

            ' Calcul de la précision sur Vs
            PrVs = CSng((0.0625 * Math.PI ^ 2 * Di ^ 4 * (PrLong / 10) ^ 2 + 0.25 * (Math.PI * Ls * Di * (PrDiam / 10)) ^ 2) ^ 0.5 + PrVp)

            ' Initialisation des variables de précision
            PrV = 0
            PrP = 0

            ' Calcul de la précision sur P
            If A1 * A2 * A3 <> 0 Then
                PrP = CSng((PrP ^ 2 + PrV ^ 2 / (A1 + 2 * A2 * Pe + 3 * A3 * Pe ^ 2) + (PoidsVol * PrProf) ^ 2) ^ 0.5)
            End If

            ' Calcul de la précision sur V
            PrV = CSng((PrV ^ 2 + (A * PrP) ^ 2 + Pr1 ^ 2 * PrA ^ 2) ^ 0.5)

            ' Calcul de la Précision sur les variables unitaires 
            PrVs = (0.0625 * Pi ^ 2 * Di ^ 4 * (PrLong / 10) ^ 2 + 0.25 * (Pi * Ls * Di * (PrDiam / 10)) ^ 2) ^ 0.5 + PrVp
            PrP = 0
            If A1 * A2 * A3 <> 0 Then
                PrP = (PrP ^ 2 + PrV ^ 2 / (A1 + 2 * A2 * Pe + 3 * A3 * Pe ^ 2) + (PoidsVol * PrProf) ^ 2) ^ 0.5
            End If
            PrV = (PrV ^ 2 + (A * PrP) ^ 2 + Pr1 ^ 2 * PrA ^ 2) ^ 0.5

            ' Calcul des variables pour le module élastique
            Dim DE_mVs As Single = 2.66 * (Pmax - Pmin) / (Vmax - Vmin)
            Dim DE_mV As Single = 1.33 * (Pmax - Pmin) / (Vmax - Vmin) * (1 + 2 / (Vmax - Vmin) * (Vs + (Vmax + Vmin) / 2))
            Dim DE_mP As Single = 2.66 * (Vs + (Vmax + Vmin) / 2) / (Vmax - Vmin)

            ' Calcul du volume limite
            VL = Vs + 2 * Vmin
            PrVl = PrVs + 2 * PrV

            ' Création et retour du DTO
            Dim result As New PrecisionElastDto() With {
                .PMax = Pmax,
                .PMin = Pmin,
                .VMax = Vmax,
                .VMin = Vmin,
                .DEm_Vs = DE_mVs,
                .DEm_V = DE_mV,
                .DEm_P = DE_mP,
                .VL = VL,
                .PrVl = PrVl
            }

            Return result
        End Function


        'Public Function PrecisionVariablesUnitaires(ByVal iDebut As Integer, iFin As Integer) As PrecisionElast
        '    Dim Di, Ls, A0, A1, A2, A3, A, Pr1, Pr2, PV, Vr1, Vr2, Pe, PrVp As Single
        '    Dim Pmax, Pmin, Vmax, Vmin, VL, PrVl As Single
        '    '  Mesures(  Colonne, Ligne, Valeur)
        '    MyExpansion = MyEssaiForage.MaExpansion
        '    Pmax = MyEssaiForage.Mesures(iFin).PNF
        '    Pmin = MyEssaiForage.Mesures(iDebut).PNF
        '    Vmax = MyEssaiForage.Mesures(iFin).VNF
        '    Vmin = MyEssaiForage.Mesures(iDebut).VNF
        '    Pr1 = MyEssaiForage.Mesures(iDebut).Pression
        '    Vr1 = MyEssaiForage.Mesures(iDebut).Volume60
        '    Pr2 = MyEssaiForage.Mesures(iFin).Pression
        '    Vr2 = MyEssaiForage.Mesures(iFin).Volume60
        '    Di = MyEssaiForage.MaExpansion.Di / 10
        '    Ls = MySonde.ls / 10
        '    Vs = MyExpansion.Vs
        '    A0 = MyEssaiForage.MaResistance.A0
        '    A1 = MyEssaiForage.MaResistance.A1
        '    A2 = MyEssaiForage.MaResistance.A2
        '    A3 = MyEssaiForage.MaResistance.A3
        '    A = MyEssaiForage.MaExpansion.coefficientA
        '    MyEssaiForage.MaResistance.CalculePel(Vr1)
        '    Pe = MyEssaiForage.MaResistance.Pe
        '    If MyEssaiForage.profSonde < MyEssaiForage.profNappe Then
        '        PV = MyEssaiForage.poidsVol
        '    Else
        '        PV = MyEssaiForage.poidsVolDejauge
        '    End If

        '    ' Calcul de la précision sur Vp


        '    ' Calcul de la Précision sur les variables unitaires 
        '    PrVs = (0.0625 * Pi ^ 2 * Di ^ 4 * (PrLong / 10) ^ 2 + 0.25 * (Pi * Ls * Di * (PrDiam / 10)) ^ 2) ^ 0.5 + PrVp
        '    PrP = 0
        '    If A1 * A2 * A3 <> 0 Then
        '        PrP = (PrP ^ 2 + PrV ^ 2 / (A1 + 2 * A2 * Pe + 3 * A3 * Pe ^ 2) + (PoidsVol * PrProf) ^ 2) ^ 0.5
        '    End If
        '    PrV = (PrV ^ 2 + (A * PrP) ^ 2 + Pr1 ^ 2 * PrA ^ 2) ^ 0.5
        '    With PrecisionVariablesUnitaires
        '        .PMax = Pmax
        '        .PMin = Pmin
        '        .VMax = Vmax
        '        .VMin = Vmin

        '        ' Précision pour la détermination du Module Em
        '        .DEm_Vs = 2.66 * (Pmax - Pmin) / (Vmax - Vmin)
        '        .DEm_V = 1.33 * (Pmax - Pmin) / (Vmax - Vmin) * (1 + 2 / (Vmax - Vmin) * (Vs + (Vmax + Vmin) / 2))
        '        .DEm_P = 2.66 * (Vs + (Vmax + Vmin) / 2) / (Vmax - Vmin)

        '        ' Précsion pour la détermination de pLM
        '        ' Valeur du volume limite Vl

        '        VL = (Vs + 2 * Vmin)
        '        PrVl = PrVs + 2 * PrV
        '        'PrVariance = 1 - 0.341

        '    End With
        'End Function

        'Public Function SetPrecisionVariablesUnitaires(ByVal iDebut As Integer, iFin As Integer) As PrecisionElast
        '    Dim Di, Ls, A0, A1, A2, A3, A, Pr1, Pr2, PV, Vr1, Vr2, Pe, PrVp As Single
        '    Dim Pmax, Pmin, Vmax, Vmin, VL, PrVl As Single
        '    '  Mesures(  Colonne, Ligne, Valeur)
        '    MyExpansion = MyEssaiForage.MaExpansion
        '    Pmax = MyEssaiForage.Mesures(iFin).PNF
        '    Pmin = MyEssaiForage.Mesures(iDebut).PNF
        '    Vmax = MyEssaiForage.Mesures(iFin).VNF
        '    Vmin = MyEssaiForage.Mesures(iDebut).VNF
        '    Pr1 = MyEssaiForage.Mesures(iDebut).Pression
        '    Vr1 = MyEssaiForage.Mesures(iDebut).Volume60
        '    Pr2 = MyEssaiForage.Mesures(iFin).Pression
        '    Vr2 = MyEssaiForage.Mesures(iFin).Volume60
        '    Di = MyEssaiForage.MaExpansion.Di / 10
        '    Ls = MySonde.ls / 10
        '    Vs = MyExpansion.Vs
        '    A0 = MyEssaiForage.MaResistance.A0
        '    A1 = MyEssaiForage.MaResistance.A1
        '    A2 = MyEssaiForage.MaResistance.A2
        '    A3 = MyEssaiForage.MaResistance.A3
        '    A = MyEssaiForage.MaExpansion.coefficientA
        '    MyEssaiForage.MaResistance.CalculePel(Vr1)
        '    Pe = MyEssaiForage.MaResistance.Pe
        '    If MyEssaiForage.profSonde < MyEssaiForage.profNappe Then
        '        PV = MyEssaiForage.poidsVol
        '    Else
        '        PV = MyEssaiForage.poidsVolDejauge
        '    End If

        '    ' Calcul de la précision sur Vp


        '    ' Calcul de la Précision sur les variables unitaires 
        '    PrVs = (0.0625 * Pi ^ 2 * Di ^ 4 * (PrLong / 10) ^ 2 + 0.25 * (Pi * Ls * Di * (PrDiam / 10)) ^ 2) ^ 0.5 + PrVp
        '    PrP = 0
        '    If A1 * A2 * A3 <> 0 Then
        '        PrP = (PrP ^ 2 + PrV ^ 2 / (A1 + 2 * A2 * Pe + 3 * A3 * Pe ^ 2) + (PoidsVol * PrProf) ^ 2) ^ 0.5
        '    End If
        '    PrV = (PrV ^ 2 + (A * PrP) ^ 2 + Pr1 ^ 2 * PrA ^ 2) ^ 0.5
        '    With PrecisionVariablesUnitaires()
        '        .PMax = Pmax
        '        .PMin = Pmin
        '        .VMax = Vmax
        '        .VMin = Vmin

        '        ' Précision pour la détermination du Module Em
        '        .DEm_Vs = 2.66 * (Pmax - Pmin) / (Vmax - Vmin)
        '        .DEm_V = 1.33 * (Pmax - Pmin) / (Vmax - Vmin) * (1 + 2 / (Vmax - Vmin) * (Vs + (Vmax + Vmin) / 2))
        '        .DEm_P = 2.66 * (Vs + (Vmax + Vmin) / 2) / (Vmax - Vmin)

        '        ' Précsion pour la détermination de pLM
        '        ' Valeur du volume limite Vl

        '        VL = (Vs + 2 * Vmin)
        '        PrVl = PrVs + 2 * PrV
        '        'PrVariance = 1 - 0.341

        '    End With
        'End Function


        ' *******************************************************************
        ' Fonction de calcul de la précision sur Vp
        ' *******************************************************************

        '***********************************************************
        'fonction de calcul du module pressiométrique Ménard NF
        '***********************************************************
        Public Function ModuleMenard(ByVal Pini As Single, ByVal Pfin As Single, ByVal Pfexp As Single, Essai As Essai_Forage) As Single

            Dim i, J, J1, k, Ndp, iMax, iEprime, iDebut, iFin As Integer
            Dim EMenard, EMenardNF, Dext, Nu, Mi, MiEprime, Beta, Pmax, Pmin, Vmax, VMin, Vs As Single
            Dim Presi, Presj, Voli, Volj, P1, P2, V1, V2 As Single
            Dim Precision As PrecisionElast
            Dim Debut, fin, Trouve As Boolean
            Dim x() As Single
            Dim Y() As Single

            ' Création des tableaux de variables
            J = 10
            ReDim Preserve x(0 To J)
            ReDim Preserve Y(0 To J)
            J1 = 10

            Ndp = GetMesureCount(Essai.refEssai)
            k = -1
            Pmax = 0
            ' Dext = Essai.MySonde.ds
            Dext = MySonde.ds

            Nu = Essai.coefPoisson
            MiEprime = 10000000000.0#
            'Vs = Pi * ((Essai.MySonde.ds / 10) ^ 2 / 4) * (Essai.MySonde.ls / 10)
            MyExpansion = Essai.MaExpansion
            Vs = MyExpansion.Vs

            iEprime = 0
            ' Calcul du module pressiométrique:
            ' Recherche de la pente minimum MiEprime de la courbe pressiométrique pour le rang iEprime
            If Ndp > 1 Then
                Pmax = 0
                For i = 1 To Ndp - 1
                    '  Mesures( Ligne, Colonne, Valeur)
                    Presi = Essai.Mesures(i).PNF
                    Voli = Essai.Mesures(i).VNF
                    Presj = Essai.Mesures(i - 1).PNF
                    Volj = Essai.Mesures(i - 1).VNF
                    If Pmax <= Presi Then
                        Pmax = Presi
                        iMax = i
                        If ((Presi - Presj) > 0) And (Presj >= Pini) And ((Voli - Volj) > 0) And (Presi <= Pfin) Then
                            'Calcul de la pente de la courbe pressiométrique NF
                            Mi = (Voli - Volj) / (Presi - Presj)

                            'Mémorisation de la pente minimum de la courbe pressiométrique NF
                            If MiEprime >= Mi Then
                                MiEprime = Mi
                                iEprime = i
                            End If
                        End If
                    End If
                Next i

                ' Calcul de la pente Béta de la courbe pressiométrique entre iEprime-1 et iEprime
                Pmax = 0
                iMax = 0
                If iEprime > 0 Then
                    For i = iEprime - 1 To Ndp - 1
                        '  Mesures(  Colonne, Ligne, Valeur)
                        P2 = Essai.Mesures(iEprime).PNF
                        P1 = Essai.Mesures(i).PNF
                        If Pmax < P1 Then
                            Pmax = P1
                            iMax = i
                            'Essai.Pe = P1
                            Essai.Pe = P1
                            Essai.Ie = iEprime

                            '  Mesures(  Colonne, Ligne, Valeur)
                            V2 = Essai.Mesures(iEprime).VNF
                            V1 = Essai.Mesures(i).VNF

                            'Calcul Debéta
                            Beta = 0
                            If (V2 - V1) <> 0 Then Beta = 1 + (P2 + P1) / (100 * (P2 - P1)) + 6 / (V2 - V1)
                            If Beta <> 0 Then Exit For
                        End If
                    Next i
                End If

                'Calcul des extrémités de la plage de calcul du module Ménard
                Debut = False
                fin = False
                iDebut = iEprime - 1
                iFin = 0
                Pmax = 0
                iMax = 0
                For i = 1 To Ndp - 1

                    '  Mesures( Ligne, Colonne, Valeur)
                    Presi = Essai.Mesures(i).PNF
                    Voli = Essai.Mesures(i).VNF
                    Presj = Essai.Mesures(i - 1).PNF
                    Volj = Essai.Mesures(i - 1).VNF

                    ' Recherche du chargement vierge
                    If Pmax <= Presi Then
                        Pmax = Presi
                        iMax = i

                        ' Recherche de la plage de valeur dans les bornes Pini et Pfin
                        If (Presi - Presj) > 0 And Presj >= Pini And Presi <= Pfin Then
                            'Calcul de la pente de la courbe pressiométrique NF
                            Mi = (Voli - Volj) / (Presi - Presj)

                            'Mémorisation de la limite basse pour la pente de la courbe pressiométrique NF
                            If (Mi <= MiEprime * Beta) And Not (Debut) Then
                                iDebut = i - 1
                                Pmin = Presj
                                VMin = Volj
                                iFin = i
                                Debut = True
                            End If

                            ' la limite de calcul de la norme NF P 74-110 à Béta*Mi' induit une imprécision des résultats trop importante
                            'If (Mi  >= MiEprime * Beta) And Debut And i - iDebut >= 3 Then
                            If (Mi <= MiEprime * Beta) And Debut Then
                                iFin = i
                                Pmax = Presi
                                Vmax = Voli
                                'fin = True
                                'Exit For
                            End If
                        Else
                            'vIf fin Then Exit For
                        End If
                    End If
                Next i

                ' Mémorisation des deux bornes du calcul
                '        If iDebut = iFin Then
                '            iDebut = iDebut - 1
                '        End If


                If Vmax > VMin Then
                    Select Case MySonde.GetTypeSonde
                        Case 0 To 4
                            ' Calcul du module pressiométrique Ménard en fonction des pentes minimum et maximum
                            EMenard = 2 * (1 + Nu) * (Vs + (Vmax + VMin) / 2) * (Pmax - Pmin) / (Vmax - VMin)
                            'Case 2 To 3
                            '    ' Calcul du module pressiométrique Ménard en fonction des pentes minimum et maximum
                            '    Vm = (Vmax + VMin) / 2
                            '    Dci = MySonde.Getds - 2 * MySonde.GetEpaisTubeFendu
                            '    Vt = Pi / 4000 * MySonde.Getls * Dci ^ 2
                            '    EMenard = 2 * (1 + Nu) * Math.Sqrt((Vs + Vm) * (Vm + Vt)) * (Pmax - Pmin) / (Vmax - VMin)
                    End Select
                    ' Calcul du module pressiométrique Ménard en fonction des pentes minimum et maximum
                    EMenard = 2 * (1 + Nu) * (Vs + (Precision.VMax + Precision.VMin) / 2) * (Precision.PMax - Precision.PMin) / (Precision.VMax - Precision.VMin)
                Else
                    Err.Raise(vbObjectError + 602, "Module Ménard", "Calcul impossible: Vmax < VMin")
                End If

                ' Détermination des bornes inférieures et supérieures du calcul de module pressiométrique
                '        Pmin = 0
                Trouve = False

                ' Recherche de la borne inférieure
                For i = 0 To Ndp - 1
                    '  Mesures( Ligne, Colonne, Valeur)
                    If (Essai.Mesures(i).PNF >= Pini) And (Not (Trouve)) And (i >= iEprime - 1) Then
                        '  Mesures(  Colonne, Ligne, Valeur)
                        Pmin = Essai.Mesures(i).PNF
                        iEprime = i
                        Trouve = True
                    Else
                        '                Trouve = True
                    End If
                Next i

                Pmax = 0
                iMax = 0
                Trouve = False
                ' Recherche de la borne supérieure
                For i = 1 To Ndp - 1
                    ' GetMesures( Ligne, Colonne, Valeur)
                    If (Essai.Mesures(i).PNF <= Pfin) And (Essai.Mesures(i).PNF >= Pmax) And (i > iEprime) Then

                        ' GetMesures( Ligne, Colonne, Valeur)
                        Pmax = Essai.Mesures(i).PNF
                        iMax = i
                        Trouve = True
                    Else
                        '                Trouve = True
                        '                Exit For
                    End If
                Next i

                ' GetMesures( Ligne, Colonne, Valeur)
                Pmax = Essai.Mesures(iFin).PNF
                Vmax = Essai.Mesures(iFin).VNF
                Pmin = Essai.Mesures(iDebut).PNF
                VMin = Essai.Mesures(iDebut).VNF

                If Vmax > VMin Then
                    Select Case MySonde.GetTypeSonde
                        Case 0 To 4
                            ' Calcul du module pressiométrique Ménard en fonction des pentes minimum et maximum
                            EMenardNF = 2 * (1 + Nu) * (Vs + (Vmax + VMin) / 2) * (Pmax - Pmin) / (Vmax - VMin)
                            'Case 2 To 3
                            '    ' Calcul du module pressiométrique Ménard en fonction des pentes minimum et maximum
                            '    Vm = (Vmax + VMin) / 2
                            '    Dci = MySonde.Getds - 2 * MySonde.GetEpaisTubeFendu
                            '    Vt = Pi / 4000 * MySonde.Getls * Dci ^ 2
                            '    EMenardNF = 2 * (1 + Nu) * Math.Sqrt((Vs + Vm) * (Vm + Vt)) * (Pmax - Pmin) / (Vmax - VMin)
                    End Select
                Else
                    Err.Raise(vbObjectError + 602, "Module Ménard", "Calcul impossible: Vmax < VMin")
                End If

                Essai.modPressioNf = Math.Round(EMenardNF, 1)
            Else
                Err.Raise(vbObjectError + 603, "Module Ménard", "Calcul impossible: Nombre de points insuffisants!")
            End If

            ' Calcul de la precision des variables unitaires et des dérivées partielles
            Precision.DEm_P = PrecisionVariablesUnitaires(iDebut, iFin).DEm_P
            Precision.DEm_V = PrecisionVariablesUnitaires(iDebut, iFin).DEm_V
            Precision.DEm_Vs = PrecisionVariablesUnitaires(iDebut, iFin).DEm_Vs
            Precision.vl = PrecisionVariablesUnitaires(iDebut, iFin).VL
            Precision.prvl = PrecisionVariablesUnitaires(iDebut, iFin).PrVl

            ' Calcul de la précision sur le module
            PrVs = MyEssaiForage.PrecisVolSonde
            PrP = MyEssaiForage.PrecisPression
            PrV = MyEssaiForage.PrecisVolume
            PrEMenard = (Precision.DEm_Vs ^ 2 * PrVs ^ 2 + 2 * Precision.DEm_V ^ 2 * PrV ^ 2 + 2 * Precision.DEm_P ^ 2 * PrP ^ 2) ^ 0.5

            ModuleMenard = Math.Round(EMenardNF, 1)
            PrEMenard = Math.Round(PrEMenard, 1)
        End Function

    End Class
End Namespace