Imports GaiaPressCoreV17.Services

Module ModComportPressio
    Public Function ComptPres(ByVal Dext As Single,
                ByVal A As Single, ByVal Nu As Single,
                ByVal Eelast As Single, ByVal Sz As Single,
                ByVal Phimu As Single,
                ByVal Coh As Single, ByVal Phi As Single,
                ByVal U0 As Single, ByVal UaMax As Single,
                ByVal p As Single, ByVal Pmax As Single,
                ByVal K0 As Single, ByVal P0 As Single,
                ByVal Lambda As Single, ByVal Mu As Single,
                ByVal Phie As Single, ByVal N As Single,
                ByVal Np As Single, ByVal Theo As Boolean) As Courbe

        Dim Uap, C1, Deno, Ub, b, Ua As Single
        Dim PfTheo, PlimTheo, Alpha As Single
        Dim X1 As Single
        Dim k As Integer

        ' Les équations de référence du calcul sont dans les documents
        ' Etude théorique de l'équilibre élasto-plastique d'un sol pulvérulent
        ' autour du pressiomètre
        ' J.Monnet
        ' Revue Française de Géotechnique
        ' n°65, janvier 1994, p.71-80

        ' The use of Pressuremeter for the determination of in situ mechanical characteristics
        ' J.Monnet, S.M.Senouci
        ' Summer Scholl Gdansk
        ' p.   1999

        'Mise à zéro des pressions théoriques
        PfTheo = 0
        PlimTheo = 0

        'Calcul des pressions de fluage


        ' Cas du sable
        If (Phi > 0) Then

            If Phi <= Phimu Then
                Phi = Phimu + 0.1
                Phie = 0.1
                N = (1 - Math.Sin(DTOR(Phi))) / (1 + Math.Sin(DTOR(Phi)))
                Np = (1 - Math.Sin(DTOR(Phie))) / (1 + Math.Sin(DTOR(Phie)))
            End If


            If K0 < (1 / (1 + Math.Sin(DTOR(Phi)))) Then

                ' calcul de la pression de fluage avec deux zones plastiques
                PfTheo = (2 * K0 + N) * Sz

            Else

                ' calcul de la pression de fluage avec une zone plastique
                PfTheo = K0 * Sz * (1 + Math.Sin(DTOR(Phi)))
            End If
        Else
            'Cas de l'argile
            If Coh <= (1 - K0) * Sz Then

                'Cas de l'argile avec deux zones plastiques
                PfTheo = Sz * (2 * K0 - 1) + 2 * Coh

            Else

                'Cas de l'argile avec une zone plastique
                PfTheo = K0 * Sz + Coh

            End If

        End If

        'Mémorisation de Pmax
        If p > Pmax Then
            Pmax = p
        End If

        '  Tracé de la courbe pressiométrique théorique au chargement
        If (Math.Abs(p - Pmax) < 0.00001) And (p > 0) And (Eelast > 0) Then

            ' Cas du sable
            If (Phi > 0) Then

                Alpha = (1 + Np) / (1 - N)

                If Coh > 0 Then
                    p = p + Coh / Math.Tan(DTOR(Phi))
                    Sz = Sz + Coh / Math.Tan(DTOR(Phi))
                    PfTheo = PfTheo + Coh / Math.Tan(DTOR(Phi))
                End If

                ' Sable deux zones plastiques
                If K0 < (1 / (1 + Math.Sin(DTOR(Phi)))) Then

                    If p > PfTheo Then
                        Uap = 0

                        ' Calcul du rayon de la première zone plastique
                        ' éq. 20 de Gdansk 1999
                        b = A * (p / Sz) ^ (1 / (1 - N))

                        ' Calcul du déplacement au niveau du rayon plastique b
                        ' éq.37 de RFG 1994
                        Ub = Sz / 2 / Mu * (1 - K0) * b

                        ' Calcul de la constante C1
                        ' éq. 30 de Gdansk 1999
                        Deno = 1 + Np * (Sz / p) ^ Alpha

                        For k = 1 To 50
                            Ua = Uap

                            ' éq. 28, p.15 Canadian Geotechnical Journal
                            'Vérifié le 30/07/02
                            C1 = (Np * Ua / A * (1 + Np) * (Sz / p) ^ Alpha + (1 + Np) * (N - K0) * Sz / 2 / Mu) / Deno

                            ' Calcul du déplacement au niveau du forage
                            ' éq. 32 de Gdansk 1999
                            ' éq. 30, p.15 Canadian Geotechnical Journal
                            'Vérifié le 30/07/02
                            Uap = A / (1 + Np) * ((p / Sz) ^ (Alpha) * (Ub / b * (1 + Np) - C1) + C1)

                            'Eq.30 du Canadian Geotechnical Journal
                            'Uap =math.Log((1 - K0) * Sz * (1 + Np) / (2 * Mu) - C1)
                            'Uap = Alpha *math.Log(p) - Alpha *math.Log(Sz) + Uap
                            'Uap = A / (1 + Np) * (Exp(Uap) + C1)
                            If (Math.Abs(Ua - Uap) < 0.000001) Then
                                Exit For
                            End If
                        Next k ' For K = 1 To 50

                        If Theo Then

                            ' calcul de la pression limite conventionnelle
                            ' éq. 40 de Gdansk 1999
                            PlimTheo = Sz * (((1 + Np) * (Math.Sqrt(2) - 1) - C1) * 2 * Mu) ^ (1 / Alpha) /
                              ((1 - K0) * Sz * (1 + Np) - C1 * 2 * Mu) ^ (1 / Alpha)
                            '                Deno = Sz * (((1 + Np) * (Sqr(2) - 1) - C1) * 2 * Mu) ^ (1 / Alpha)
                            '                Deno = ((1 - K0) * Sz * (1 + Np) - C1 * 2 * Mu)
                            '                Deno = ((1 - K0) * Sz * (1 + Np) - C1 * 2 * Mu) ^ (1 / Alpha)
                            If Coh > 0 Then
                                PlimTheo = PlimTheo - Coh / Math.Tan(DTOR(Phi))
                            End If
                        End If

                        Uap = Uap + A - Dext / 2000
                        X1 = Uap / Dext * 2000 + U0

                    End If 'If P > PfTheo Then

                End If 'If K0 < (1 / (1 + Math.sin(DTOR(Phi)))) Then

                ' Sable une zone plastique
                If K0 >= (1 / (1 + Math.Sin(DTOR(Phi)))) Then

                    If p >= PfTheo Then

                        ' Calcul du rayon de la première zone plastique
                        ' éq. 43 de RFG 1994
                        b = A * ((1 + N) * p / 2 / P0) ^ (1 / (1 - N))

                        ' Calcul du déplacement au niveau du rayon plastique b
                        ' éq. du manuscript
                        Ub = P0 * (1 - N) * b / 2 / Mu / (1 + N)

                        ' éq. 31, p.16 Canadian Geotechnical Journal
                        'Vérifié le 30/07/02
                        C1 = P0 * (1 - N) * (Np - 1) / (1 + N) / 2 / Mu

                        ' éq. 32, p.16 Canadian Geotechnical Journal
                        'Vérifié le 30/07/02
                        Uap = A / (1 + Np) * ((p * (1 + N) / 2 / K0 / Sz) ^ Alpha _
                                       * (K0 * Sz * (1 - N) * (1 + Np) / 2 / Mu / (1 + N) - C1) + C1)
                        Uap = Uap + A - Dext / 2000
                        X1 = Uap / Dext * 2000 + U0
                    End If

                    If Theo Then

                        ' Calcul du rayon de la première zone plastique
                        ' éq. 43 de RFG 1994
                        b = A * ((1 + N) * p / 2 / P0) ^ (1 / (1 - N))

                        ' Calcul du déplacement au niveau du rayon plastique b
                        ' éq. du manuscript
                        Ub = P0 * (1 - N) * b / 2 / Mu / (1 + N)

                        ' éq. 31, p.16 Canadian Geotechnical Journal
                        'Vérifié le 30/07/02
                        C1 = P0 * (1 - N) * (Np - 1) / (1 + N) / 2 / Mu

                        ' calcul de la pression limite conventionnelle
                        ' éq. 38, p.17 Canadian Geotechnical Journal
                        PlimTheo = 2 * K0 * Sz / (1 + N) * (((1 + Np) * (Math.Sqrt(2) - 1) - C1) * 2 * Mu * (1 + N)) ^ (1 / Alpha) /
                                  ((K0 * Sz * (1 - N) * (1 + Np) - 2 * Mu * C1 * (1 + N))) ^ (1 / Alpha)
                        If Coh > 0 Then
                            PlimTheo = PlimTheo - Coh / Math.Tan(DTOR(Phi))
                        End If
                    End If

                End If 'If K0 >= (1 / (1 + Math.sin(DTOR(Phi)))) Then

            End If ' Fin du cas du sable et du sol frottant

            ' Cas de l'argile
            If (Phi = 0) And (Coh > 0) Then

                ' Argile deux zones plastiques
                If Coh <= (1 - K0) * Sz Then

                    If p > PfTheo Then
                        'C1 = -Coh / Mu
                        'b = A * Exp((p - Sz) / (2 * Coh))
                        'Ub = (Sz - P0) / 2 / Mu * b
                        'UbsB = (Sz - P0) / 2 / Mu
                        'Uap = A * (Exp((p - Sz) / Coh) * (UbsB - C1 / 2) + C1 / 2)

                        ' Relations du programme Delphi 3
                        '
                        '                    C1:=-Coh/Mu;
                        '        B:=A*Exp((Szlocal-p)/(2*Coh));
                        '        Ub:=(P0-Szlocal)/2/Mu*B;
                        '        UbsB:=(P0-Szlocal)/2/Mu;
                        '        Uap:=A*(Exp((Szlocal-P)/Coh)*(UbsB-C1/2)+C1/2);
                        '        X1:=Uap/Dext*2000+U0;


                        'Relation 58, p.22 Canadian Geotechnical Journal
                        C1 = -Coh / Mu
                        Uap = (1 - K0) * Sz / (2 * Mu) + C1 / 2
                        Uap = (p - Sz) / Coh + Math.Log(Uap)
                        Uap = A * (Math.Exp(Uap) - C1 / 2)
                        X1 = Uap / Dext * 2000 + U0
                    End If

                    If Theo Then

                        ' Relation 65,p.24 Canadian Geotechnical Journal
                        PlimTheo = Sz + Coh * Math.Log((2 * Mu * (Math.Sqrt(2) - 1) + Coh) / ((1 - K0) * Sz + Coh))
                    End If

                End If 'If Coh <= (K0 - 1) * Sz Then

                ' Argile une zone plastique
                If Coh > (1 - K0) * Sz Then

                    If p > PfTheo Then

                        ' Relation du programme Delphi 3.
                        'Uap = A / (1 / (Exp(p / Coh) * Exp(P0 / Coh - 1))) * (Coh / (2 * Mu))

                        'Relation 60, p.23 du Canadian Geotechnical Journal
                        Uap = Coh / (2 * Mu)
                        Uap = (p - K0 * Sz) / Coh - 1 + Math.Log(Uap)
                        Uap = A * Math.Exp(Uap)
                        X1 = Uap / Dext * 2000 + U0
                    End If

                    If Theo Then

                        ' Relation 68,p.25 Canadian Geotechnical Journal
                        PlimTheo = K0 * Sz + Coh * (1 + Math.Log(2 * Mu * (Math.Sqrt(2) - 1) / Coh))
                    End If

                End If 'If Coh > (K0 - 1) * Sz Then

            End If ' Fin du cas de l'argile

        End If ' Fin du calcul au chargement

        '  Calcul de la courbe pressiométrique avant le fluage
        If (p < PfTheo) And (Eelast > 0) And (p = Pmax) Then
            Ua = A / 2 / Mu * (p - P0)
            Uap = Ua
            X1 = Uap / Dext * 2000 + U0
        End If

        'Mémorisation du déplacement maximum à partir de U0
        If UaMax < Uap Then
            UaMax = Uap
        End If

        '  Calcul de la courbe pressiométrique théorique sur cycle
        If (p < Pmax) And (Eelast > 0) Then
            Ua = A / 2 / Mu * (Pmax - p)
            Uap = UaMax - Ua
            X1 = Uap / Dext * 2000 + U0
        End If

        If (Uap / A) <= 0.0001 Then
            Uap = 0
            X1 = 0 + U0
        End If

        ComptPres.Def = X1
        ComptPres.Pres = p
        ComptPres.Pf = PfTheo
        ComptPres.Plim = PlimTheo
        ComptPres.Pf = PfTheo
        ComptPres.Pmax = Pmax
        ComptPres.UaMax = UaMax
        ComptPres.Erreur = False

    End Function


End Module
