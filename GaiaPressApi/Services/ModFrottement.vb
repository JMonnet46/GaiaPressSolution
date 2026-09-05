Namespace GaiaPress.Core.V17.Services

    Module ModFrottement

        '*****************************************************
        ' Calcul de la pente de la droite bi-logarithmique
        '*****************************************************
        Public Function CalcPente(ByVal Phimu As Single, ByVal Phi As Single) As Single

            Dim N1, N2, Phip As Single

            Phip = Phi - Phimu
            N1 = (1 - Math.Sin(DTOR(Phi))) / (1 + Math.Sin(DTOR(Phi)))
            N2 = (1 - Math.Sin(DTOR(Phip))) / (1 + Math.Sin(DTOR(Phip)))
            CalcPente = (1 - N1) / (1 + N2)
        End Function

        '********************************************************************************
        '    Calcul de la pression limite théorique sur les valeurs totalement corrigées
        '********************************************************************************
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
                        ByVal Phi As Single,
                        ByVal Pmax As Single,
                        ByVal Ndp As Integer) As Cisaillement

            Dim Uap, Ua, Ub As Single
            Dim PL, UaMax, C1, Deno, Mu, Phip, A, b, N, Np As Single
            Dim i As Integer


            A = Dext0 / 200
            Mu = Eelast / 2 / (1 + Nu)

            If (Eelast > 0) And (Phi > 0) And (P0 > 0) And (A > 0) And (Sz < 0) And (Pmax > 0) Then

                PL = Eelast / 4 / (1 - Math.Sqrt(Nu)) / Math.Sin(DTOR(Phi)) / (P0 + CTanPhi)
                PL = PL ^ (Math.Sin(DTOR(Phi)) / (1 + Math.Sin(DTOR(Phi))))
                PL = (1 + Math.Sin(DTOR(Phi))) * (P0 + CTanPhi) * PL - CTanPhi


                If (Phi > 0) And (Coh = 0) Then

                    N = (1 - Math.Sin(DTOR(Phi))) / (1 + Math.Sin(DTOR(Phi)))
                    Phip = Phi - Phimu
                    Np = (1 - Math.Sin(DTOR(Phip))) / (1 + Math.Sin(DTOR(Phip)))

                    If K0 < 1 / (1 + Math.Sin(DTOR(Phi))) Then
                        Uap = 0
                        For i = 0 To 100
                            Ua = Uap
                            b = A * (-Pmax / Sz) ^ (1 / (1 - N))
                            Ub = Sz / 2 / Mu * (K0 - 1) * b
                            Deno = 1 + Np * (-Sz / Pmax) ^ ((1 + Np) / (1 - N))
                            C1 = (Np * Ua / A * (1 + Np) * (-Sz / Pmax) ^ ((1 + Np) / (1 - N)) _
                             - (1 + Np) * (N - K0) * Sz / 2 / Mu) / Deno
                            Uap = A / (1 + Np) * ((b / A) ^ (1 + Np)) * (Ub / b * (1 + Np) - C1) + C1
                            UaMax = Uap

                            If (Math.Abs(Ua - Uap) < 0.000001) Then
                                Exit For
                            End If
                        Next i 'For I = 0 To 100

                        PL = -Sz * ((1 + Np - 2 * C1) * Mu) ^ ((1 - N) / (1 + Np)) /
                           ((K0 - 1) * Sz * (1 + Np) - C1 * 2 * Mu) ^ ((1 - N) / (1 + Np))


                    End If 'If K0 < 1 / (1 + Math.sin(Phir)) Then
                End If 'If (Phi > 0) And (Coh = 0) Then

                If K0 >= 1 / (1 + Math.Sin(DTOR(Phi))) Then

                    C1 = -P0 / 2 / Mu * (1 - N) / (1 + N) * (Np - 1)

                    PL = -2 * K0 * Sz / (1 + N) * ((1 + Np - 2 * C1) * Mu * (1 + N)) ^ ((1 - N) / (1 + Np)) /
                     ((-K0 * Sz * (1 - N) * (1 + Np) - 2 * Mu * C1 * (1 + N))) ^ ((1 - N) / (1 + Np))

                End If 'If K0 >= 1 / (1 + Math.sin(Phir)) Then

                CalculPlim.Plim = PL
                CalculPlim.Erreur = False

            Else
                ' Si le module élastique est inférieur à 0
                CalculPlim.Erreur = True
            End If
        End Function



        Public Function ModuleFrotte(ByVal Pfexp As Single,
            Essai As Essai_Forage) As Cisaillement

            Dim i, J, k As Integer
            Dim X() As Single
            Dim Y() As Single
            Dim Phi0, Phi1, Phi2, Pres, Pmax, Def, N, Phir, PfTheo, Sz, K0, P0 As Single
            Dim PenteTheo, P1, P2, Plim, Phi, Phimu, CTanPhi, A0L, A1L, Nu As Single
            Dim Dext, Coh, HSonde, Hnappe, Gammadej, Gamma, Eelast As Single
            Dim Ndp As Integer
            Dim Erreur As Boolean
            Dim result As Cisaillement
            Dim Regres As Regression

            J = 10
            ReDim X(0 To 10)
            ReDim Y(0 To 10)

            With Essai
                HSonde = .E_profSonde
                Hnappe = .E_profNappe
                Gamma = .E_poidsVol
                Gammadej = .E_poidsVolDejauge
                Eelast = .E_modElastique
                Nu = .E_coefPoisson
                ' Dext = .MySonde.Ds
                Dext = .E_MySonde.P_ds
                Coh = 0
                Phimu = .E_MySol.Phimu
                Ndp = .E_Mesures.Count
            End With

            If HSonde >= Hnappe Then Sz = -(HSonde - Hnappe) * Gammadej - (Hnappe * Gamma)
            If HSonde < Hnappe Then Sz = -HSonde * Gamma

            K0 = Nu / (1 - Nu)
            P0 = -K0 * Sz

            k = -1
            Pmax = 0

            If Ndp > 0 Then
                For i = 0 To Ndp - 1
                    ' E_Mesures(Ligne, Colonne, valeur)
                    Pres = Essai.E_Mesures(7).ContrainteRadiale
                    Def = Essai.E_Mesures(9).Deformation
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
                        Y(k) = Math.Log(Pres)
                    End If
                Next i

                If k > 1 Then

                    Regres = RegresLine(k, X, Y)

                    A1L = Regres.A
                    A0L = Regres.B
                    Erreur = Regres.Erreur

                    Phi1 = Phimu
                    Phi2 = 90
                    P1 = CalcPente(Phimu, Phi1)
                    P2 = CalcPente(Phimu, Phi2)


                    If (A1L >= P1) And (A1L <= P2) Then

                        'Calcul de l'angle de frottement pas dichotomie
                        For i = 0 To 5000
                            If (A1L >= P1) And (A1L <= P2) Then
                                Phi0 = (Phi1 + Phi2) / 2
                                PenteTheo = CalcPente(Phimu, Phi0)
                                If PenteTheo > A1L Then Phi2 = Phi0
                                If PenteTheo < A1L Then Phi1 = Phi0
                                If Math.Abs(PenteTheo) < 0.01 Then
                                    Exit For
                                End If
                            End If
                        Next i

                        Phi = Phi1

                        result = CalculPlim(A1L, Nu, CTanPhi, Eelast, P0, Phimu, Dext,
                                    K0, Sz, Coh, Phi, Pmax, Ndp)

                        If Not (result.Erreur) Then
                            Coh = result.Coh
                            Phi = result.Phi
                            Plim = result.Plim

                            N = (1 - Math.Sin(DTOR(Phi))) / (1 + Math.Sin(DTOR(Phi)))

                            If K0 < 1 / (1 + Math.Sin(DTOR(Phi))) Then PfTheo = (2 * K0 - N) * Sz
                            If K0 >= 1 / (1 + Math.Sin(DTOR(Phir))) Then PfTheo = -2 * K0 * Sz / (1 + N)

                            ModuleFrotte.Coh = Coh
                            ModuleFrotte.Phi = Phi
                            ModuleFrotte.Pf = PfTheo
                            ModuleFrotte.Plim = Plim
                            ModuleFrotte.Erreur = False
                        Else
                            ModuleFrotte.Erreur = True
                        End If
                    Else
                        ModuleFrotte.Erreur = True
                        ModuleFrotte.Phi = Phimu
                    End If
                Else
                    ModuleFrotte.Erreur = True
                    ModuleFrotte.Phi = Phimu
                End If
            Else
                ModuleFrotte.Erreur = True
                ModuleFrotte.Phi = Phimu
            End If

        End Function



    End Module
End Namespace