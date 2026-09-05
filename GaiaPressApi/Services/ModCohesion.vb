Namespace GaiaPress.Core.V17.Services

    Module ModCohesion

        Public Function CalculPlim(ByVal Pente As Single,
                        ByVal Nu As Single,
                        ByVal CTanPhi As Single,
                        ByVal Eelast As Single,
                        ByVal P0 As Single,
                        ByVal Phimu As Single,
                        ByVal Dext0 As Single,
                        ByVal K0 As Single,
                        ByVal Sz As Single,
                        ByVal Coh As Single,
                        ByVal Pmax As Single,
                        ByVal Ndp As Integer) As Cisaillement

            Dim Uap, Ub, Phi As Single
            Dim PlimTheo, PfTheo, UbsB, C1, Mu, A, b As Single

            A = Dext0 / 200
            Mu = Eelast / 2 / (1 + Nu)

            If (Eelast > 0) And (Phi = 0) And (P0 > 0) And (A > 0) And (Sz < 0) _
           And (Pmax > 0) And (Coh > 0) Then

                ' Argile deux zones plastiques
                If Coh <= (K0 - 1) * Sz Then
                    C1 = -Coh / Mu
                    b = A * Math.Exp((Sz - Pmax) / (2 * Coh))
                    Ub = (P0 - Sz) / 2 / Mu * b
                    UbsB = (P0 - Sz) / 2 / Mu
                    Uap = A * (Math.Exp((Sz - Pmax) / Coh) * (UbsB - C1 / 2) + C1 / 2)
                    ' X1 = Uap / Dext0 * 2000
                    PfTheo = -Sz * (2 * K0 - 1) + 2 * Coh
                    PlimTheo = -Sz + Coh * Math.Log((2 * Mu * (Math.Sqrt(2) - 1) + Coh) / ((K0 - 1) * Sz + Coh))
                End If

                ' Argile une zone plastique
                If Coh > (K0 - 1) * Sz Then
                    ' C1p = 0
                    Uap = A / (1 / (Math.Exp(-Pmax / Coh) * Math.Exp(P0 / Coh - 1))) * (Coh / (2 * Mu))
                    ' X1 = Uap / Dext0 * 2000 + U0
                    PfTheo = -K0 * Sz + Coh
                    PlimTheo = -K0 * Sz + Coh * (1 + Math.Log(2 * Mu * (Math.Sqrt(2) - 1) / Coh))
                End If

                CalculPlim.Plim = PlimTheo
                CalculPlim.Pf = PfTheo
                CalculPlim.Erreur = False

            Else 'If Eelast > 0 Then
                CalculPlim.Erreur = True
            End If

        End Function


        Public Function ModuleCohe(ByVal Pfexp As Single, Essai As Essai_Forage) As Cisaillement

            Dim i, J, k As Integer
            Dim X() As Single
            Dim Y() As Single
            Dim Pres, Pmax, Def, N, Phir, PfTheo, Sz, K0, P0 As Single
            Dim Plim, Phi, Phimu, CTanPhi, A0L, A1L, Nu As Single
            Dim Dext, Coh, HSonde, Hnappe, Gammadej, Gamma, Eelast As Single
            Dim Ndp As Integer
            Dim Erreur As Boolean
            Dim result As Cisaillement
            Dim Regres As Regression

            J = 10
            ReDim X(0 To 10)
            ReDim Y(0 To 10)

            HSonde = Essai.E_profSonde
            Hnappe = Essai.E_profNappe
            Gamma = Essai.E_poidsVol
            Gammadej = Essai.E_poidsVolDejauge
            Eelast = Essai.E_modElastique
            Nu = Essai.E_coefPoisson
            Nu = 0.343
            '  Dext = Essai.MySonde.ds
            Dext = Essai.E_MySonde.P_ds

            If HSonde >= Hnappe Then Sz = -(HSonde - Hnappe) * Gammadej - (Hnappe * Gamma)
            If HSonde < Hnappe Then Sz = -HSonde * Gamma

            K0 = Nu / (1 - Nu)
            P0 = -K0 * Sz


            'Ndp = _chantier.C_ListMesuresForage.count
            Ndp = Essai.E_Mesures.Count

            k = -1
            Pmax = 0

            If Ndp > 0 Then
                For i = 0 To Ndp - 1
                    ' GetMesures(Ligne, Colonne, valeur)
                    Pres = Essai.E_Mesures(i).ContrainteRadiale
                    Def = Essai.E_Mesures(i).Deformation
                    If Pmax < Pres Then
                        Pmax = Pres
                    End If
                    If (Math.Abs(Pmax - Pres) < 0.0001) And (Pres >= Pfexp) _
                   And (Def > 0) And (Pres > 0) Then
                        k = k + 1
                        If k > J Then
                            J = J + 10
                            ReDim Preserve X(0 To J)
                            ReDim Preserve Y(0 To J)
                        End If
                        X(k) = Math.Log(Def)
                        Y(k) = Pres

                    End If
                Next i

                If k > 0 Then

                    Regres = RegresLine(k, X, Y)

                    If Not (Regres.Erreur) Then

                        A1L = Regres.A
                        Coh = A1L
                        ModuleCohe.Coh = Coh
                        ModuleCohe.Phi = 0
                        A0L = Regres.B
                        Erreur = Regres.Erreur


                        result = CalculPlim(A1L, Nu, CTanPhi, Eelast, P0, Phimu, Dext,
                                        K0, Sz, Coh, Pmax, Ndp)

                        If Not (result.Erreur) Then

                            Plim = result.Plim
                            PfTheo = result.Pf

                            If Phi > 0 Then
                                N = (1 - Math.Sin(DTOR(Phi))) / (1 + Math.Sin(DTOR(Phi)))
                                If K0 < 1 / (1 + Math.Sin(DTOR(Phi))) Then PfTheo = (2 * K0 - N) * Sz
                                If K0 >= 1 / (1 + Math.Sin(DTOR(Phir))) Then PfTheo = -2 * K0 * Sz / (1 + N)
                                ModuleCohe.Pf = PfTheo
                                ModuleCohe.Plim = Plim
                                ModuleCohe.Erreur = False
                            End If

                            ModuleCohe.Pf = PfTheo
                            ModuleCohe.Plim = Plim
                            ModuleCohe.Erreur = False

                            'Mémorisation du frottement
                            Essai.E_Cohesion = Math.Round(Coh, 1)
                        Else
                            ModuleCohe.Erreur = True
                        End If
                    Else
                        ModuleCohe.Erreur = True
                    End If
                Else
                    ModuleCohe.Erreur = True
                End If
            Else
                ModuleCohe.Erreur = True
            End If

        End Function




    End Module
End Namespace