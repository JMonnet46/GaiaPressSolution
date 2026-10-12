'Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17
Imports System.Threading

Namespace GaiaPressCoreV17.Services
    Module ModStatProba

        'Dim XlApp As Excel.Application

        'Degrés de liberté maximum pour ProbFisher (permet une précision d'au moins 4 chiffres sur p)
        PUBLIC Const gclMaxDF As Long = 16384

        ' Type retourné par la fonction RegLinDom [Calcul de la droite de régression y = f(x)]
        Public Structure tRegLin
            Public bErreur As Boolean               ' Vrai en cas d'erreur
            Public sMsgErreur As String             ' Message d'erreur
            Public dPente As Double                 ' Valeur de la pente de la droite de régression
            Public dStDevPente As Double            ' Ecart-type de la pente
            Public dOrdonnee As Double              ' Ordonnée à l'origine de la droite
            Public dStDevOrigine As Double          ' Ecart-type de l'origine
            Public dErreurTypeY As Double           ' Erreur-type des y estimés par la droite
            Public fr2 As Single                    ' Coefficient de détermination (r x r)
            Public lCompte As Long                  ' Nombre de couples (x,y) ou enregistrements utilisés
            Public lCompteX As Long                 ' Nombre de valeurs uniques de x
            Public vMinX As Object                 ' Valeur minimum de x
            Public vMaxX As Object                 ' Valeur maximum de x
            Public fProbaFisherLinearite As Single  ' Probabilité du test de linéarité
            Public fProbaStudentPente As Single     ' Probabilité du test de liaison
        End Structure

        '************************************************************************************************
        '* Fonction    : RegLinDom [Calcul de la droite de régression linéaire de y en x (y = ax + b)]
        '* Auteur      : PhilBen (© dans le cas d'un usage professionnel)
        '* Version     : 1.0
        '* Publication : 22/07/2006 (www.developpez.com)
        '* Dépendances : RegLinDom |-> IsStatDomErr -> StatExpressionErr
        '*                         |-> ProbStudent  -> ProbFisher
        '*                         |-> ProbFisher
        '* Rappel      : La régression porte sur la variation d'une quantité y en fonction d'une
        '*               quantité x. Elle exprime la liaison entre un facteur x contrôlé
        '*               (valeurs connues a priori) et une variable aléatoire y liée (ne pas confondre
        '*               avec la corrélation qui étudie la liaison entre 2 variables totalement
        '*               aléatoires).
        '* Objectifs   : - Déterminer la forme de la liaison linéaire y = ax + b, en calculant par
        '*                 la méthode des moindres carrés, la pente (a) et l'ordonnée à l'origine (b)
        '*                 de la droite de régression ajustée au plus près aux données observées;
        '*               - Tester la linéarité de la courbe de régression et l'existence d'une liaison
        '*                 significative entre la cause (x) et l'effet (y).
        '* Paramètres  : - sExpressionX : Identifiant obligatoire de la variable X qui peut être le
        '*                                nom d'un champ numérique ou une expression calculée issus
        '*                                du domaine étudié;
        '*               - sExpressionY : Identifiant obligatoire de la variable Y qui peut être le
        '*                                nom d'un champ numérique ou une expression calculée issus
        '*                                du domaine étudié;
        '*               - sDomaine     : Identifiant obligatoire du nom de la table ou de la requête
        '*                                qui porte les enregistrements du domaine étudié;
        '*               - sCritere     : Expression facultative permettant de restreindre l'étendue
        '*                                du domaine étudié (équivalent à l'argument de la clause WHERE
        '*                                d'une requête SQL)
        '*               - bMsgBoxErr   : Valeur boléenne facultative (Vrai par défaut) indiquant
        '*                                si la fonction affiche ou non un message en cas d'erreur
        '* Type retourné: Contenu du type tRegLin :
        '*               - bErreur      : Valeur (type boolean) à Vrai si une erreur est survenue
        '*               - sMsgErreur   : Chaîne de caractère décrivant l'éventuelle erreur rencontrée
        '*               - dPente       : Valeur (type Double) de la pente de la droite de régression
        '*               - dStDevPente  : Valeur (type Double) de l'écart-type de la pente
        '*               - dOrdonnee    : Valeur (type Double) de l'ordonnée à l'origine de la droite
        '*               - dStDevOrigine: Valeur (type Double) de l'écart-type de l'ordonnée
        '*               - dErreurTypeY : Valeur (type Double) de l'erreur-type associée aux y estimés
        '*                                par la droite de régression. Permet de calculer les intervalles
        '*                                de prévision et de confiance des y autour de la droite
        '*               - fr2          : Valeur (type Single) du coefficient de détermination (r au carré)
        '*                                Peut varier dans l'intervalle [0 à 1]. Plus sa valeur est grande
        '*                                plus la droite de régression explique la variabilité des y
        '*               - lCompte      : Valeur (type Long) du nombre de couples (x,y) ou enregistrements
        '*                                utilisés pour le calcul de la droite de régression
        '*               - lCompteX     : Valeur (type Long) du nombre de valeurs uniques de x utilisées
        '*               - vMinX        : Valeur (type Variant) minimum des x
        '*               - vMaxX        : Valeur (type Variant) maximum des x
        '*               - fProbaFisherLinearite As Double : Valeur (type Single) de la probabilité (p)
        '*                                [0 à 1], de l'hypothèse nulle de linéarité. La probabilité est
        '*                                calculée d'après la loi F de Fisher-Snedecor et nous indique
        '*                                si l'écart des y le long de la droite peut être expliqué par
        '*                                l'écart dû aux fluctuations d'échantillonnage des y entre eux.
        '*                                Si p > 0,05 l'hypothèse de linéarité est admissible (à 5 %)
        '*                                Si p <= 0,05 la linéarité est rejetée au risque 5 %
        '*               - fProbaStudentPente : Valeur (type Single) de la probabilité [0 à 1], calculée
        '*                                d'après la loi t de Student-Fisher, que la la pente ne diffère
        '*                                pas de zéro, en d'autres termes que la variable y est indépendante
        '*                                de x. Si la probabilité du test est <= au risque de 1ère espèce
        '*                                alpha consenti (en général 5 %), on dira que la pente diffère
        '*                                significativement de 0 au seuil 5 % et l'hypothèse d'indépendance
        '*                                est rejetée.
        '*                                En résumé, si p > 0,05 la liaison n'est pas significative (à 5%)
        '*                                et si p <= 0,05 la liaison est significative et p mesure son
        '*                                degré de signification.
        '*                                Rem. : Ce type de test peut être utilisé pour comparer 2 pentes
        '* Remarques   : A) On considère que la droite de régression représente convenablement la courbe
        '*                  de régression de y en x si :
        '*                   - l'hypothèse de linéarité est admise (p > 0,05)
        '*                   - l'hypothèse d'indépendance (non liaison) est rejetée (p <= 0,05)
        '*               B) Une droite de régression statistiquement linéaire ne prétend représenter
        '*                  la variation de y en fonction de x que sur l'intervalle étudié [xMin à xMax].
        '* Exemple     : Dim tRL as tRegLin
        '*               tRL = RegLinDom("MonChampX", "MonChampY", "MaTable")
        '*               If Not tRL.bErreur then
        '*                   ' Si la courbe de régression des données est bien estimée par notre droite
        '*                   ' et si la liaison x <-> y est significative, c'est tout bon !
        '*                   If dProbaFisherLinearite > 0.05 And dProbaStudentPente <= 0.05 then
        '*                      ...utilisation de la droite
        '*                   endif
        '*               endif
        '************************************************************************************************
        '    Public Function RegLinDom(ByVal sExpressionX As String, ByVal sExpressionY As String, _
        '                ByVal sDomaine As String, Optional ByVal sCritere As String = vbNullString, _
        '                Optional ByVal bMsgBoxErr As Boolean = True) As tRegLin
        '        On Error GoTo RLErr
        '        Dim oDb As DAO.Database
        '        Dim oRs As DAO.Recordset
        '        Dim dSommeX As Double, dSommeCarreX As Double, dSommeY As Double, dSommeCarreY As Double
        '        Dim dSommeXY As Double, dDeltaX As Double, dDeltaY As Double, dDeltaXY As Double
        '        Dim dSommeCarreYSurX As Double, dVarDevDroite As Double, dVarIntraY As Double
        '        Dim dEcartEntreY As Double
        '        Dim sSql As String, sTmp As String

        '        If Not IsStatDomErr(RegLinDom.sMsgErreur, "RegLin", sDomaine, sExpressionX, sExpressionY) Then
        '            sTmp = "(" & sExpressionX & ") Is Not Null AND (" & sExpressionY & ") Is Not Null"
        '            sCritere = IIf(Len(Trim$(sCritere)) > 0, sCritere & " AND " & sTmp, sTmp)

        '            sTmp = "((" & sExpressionX & ") * (" & sExpressionY & "))"
        '            sSql = "SELECT Sum(" & sExpressionX & ") AS SommeX, " & _
        '                     "Sum(" & sExpressionY & ") AS SommeY, " & _
        '                     "Sum(" & sTmp & ") AS SommeXY, " & _
        '                     "Sum((" & sExpressionX & ") ^ 2) AS SommeCarreX, " & _
        '                     "Sum((" & sExpressionY & ") ^ 2) AS SommeCarreY, " & _
        '                     "Min(" & sExpressionX & ") AS MinX, " & _
        '                     "Max(" & sExpressionX & ") AS MaxX, " & _
        '                     "Count(*) AS Compte " & _
        '                     "FROM " & sDomaine & " WHERE " & sCritere & ";"

        '            oDb = CurrentDb
        '            oRs = oDb.OpenRecordset(sSql, dbOpenForwardOnly)

        '            If Not oRs.EOF Then
        '                RegLinDom.lCompte = oRs.Fields("Compte")
        '                RegLinDom.vMinX = oRs.Fields("MinX")
        '                RegLinDom.vMaxX = oRs.Fields("MaxX")
        '                dSommeX = oRs.Fields("SommeX")
        '                dSommeY = oRs.Fields("SommeY")
        '                dSommeXY = oRs.Fields("SommeXY")
        '                dSommeCarreX = oRs.Fields("SommeCarreX")
        '                dSommeCarreY = oRs.Fields("SommeCarreY")
        '                dDeltaX = RegLinDom.lCompte * dSommeCarreX - dSommeX ^ 2
        '                dDeltaY = RegLinDom.lCompte * dSommeCarreY - dSommeY ^ 2
        '                dDeltaXY = RegLinDom.lCompte * dSommeXY - dSommeX * dSommeY

        '                If RegLinDom.lCompte > 2 And dDeltaX > 0 And dDeltaY > 0 Then
        '                    RegLinDom.dPente = dDeltaXY / dDeltaX
        '                    RegLinDom.dOrdonnee = (dSommeCarreX * dSommeY - dSommeX * dSommeXY) / dDeltaX

        '                    RegLinDom.dStDevPente = ((dDeltaY / dDeltaX - RegLinDom.dPente ^ 2) / (RegLinDom.lCompte - 2)) ^ 0.5
        '                    RegLinDom.dStDevOrigine = RegLinDom.dStDevPente * (dSommeCarreX / RegLinDom.lCompte) ^ 0.5

        '                    RegLinDom.dErreurTypeY = ((dDeltaY - dDeltaXY ^ 2 / dDeltaX) / (RegLinDom.lCompte * (RegLinDom.lCompte - 2))) ^ 0.5
        '                    RegLinDom.fr2 = dDeltaXY ^ 2 / (dDeltaX * dDeltaY)

        '                    RegLinDom.fProbaStudentPente = ProbStudent(RegLinDom.dPente / RegLinDom.dStDevPente, RegLinDom.lCompte - 2)

        '                    sSql = "SELECT Count(*) AS CompteUniqueX, " & _
        '                           "Count(" & sExpressionX & ") AS CompteX, " & _
        '                           "Sum(" & sExpressionY & ") AS SommeY, " & _
        '                           "[SommeY] ^ 2 / [CompteX] AS SommeCarreYSurX " & _
        '                           "FROM " & sDomaine & " WHERE " & sCritere & " GROUP BY " & sExpressionX & ";"

        '                    oRs = oDb.OpenRecordset(sSql, dbOpenForwardOnly)

        '                    If Not oRs.EOF Then
        '                        RegLinDom.lCompteX = oRs.Fields("CompteUniqueX")
        '                        Do Until oRs.EOF
        '                            dSommeCarreYSurX = dSommeCarreYSurX + oRs.Fields("SommeCarreYSurX")
        '                            oRs.MoveNext()
        '                        Loop

        '                        If RegLinDom.lCompteX > 2 And RegLinDom.lCompte > RegLinDom.lCompteX Then
        '                            dEcartEntreY = dSommeCarreYSurX - (dSommeY ^ 2 / RegLinDom.lCompte)
        '                            dVarDevDroite = (dEcartEntreY - (RegLinDom.dPente ^ 2 * dDeltaX / RegLinDom.lCompte)) / _
        '                                            (RegLinDom.lCompteX - 2) 'd.d.l. = Nb Colonnes - 1 - 1(pour la Régression)
        '                            dVarIntraY = (dSommeCarreY - dSommeCarreYSurX) / _
        '                                         (RegLinDom.lCompte - 1 - (RegLinDom.lCompteX - 1))
        '                            RegLinDom.fProbaFisherLinearite = ProbFisher(dVarDevDroite / dVarIntraY, _
        '                                           RegLinDom.lCompteX - 2, RegLinDom.lCompte - RegLinDom.lCompteX)
        '                        Else
        '                            RegLinDom.bErreur = True
        '                            RegLinDom.sMsgErreur = "Test de linéarité non réalisable..." & vbCrLf
        '                            If RegLinDom.lCompteX <= 2 Then
        '                                RegLinDom.sMsgErreur = RegLinDom.sMsgErreur & _
        '                                            "Le nombre de valeurs uniques de la variable x doit être > à 2."
        '                            Else
        '                                RegLinDom.sMsgErreur = RegLinDom.sMsgErreur & _
        '                                "Le nombre de couples de valeurs (x,y) doit être > au nombre de valeurs uniques de x."
        '                            End If
        '                        End If
        '                    Else
        '                        RegLinDom.bErreur = True
        '                        RegLinDom.sMsgErreur = "Test de linéarité non réalisé..." & vbCrLf & _
        '                                               "Aucun enregistrement retourné par le domaine."
        '                    End If
        '                Else
        '                    RegLinDom.bErreur = True
        '                    RegLinDom.sMsgErreur = "Droite de régression non calculable..." & vbCrLf
        '                    If RegLinDom.lCompte <= 2 Then
        '                        RegLinDom.sMsgErreur = RegLinDom.sMsgErreur & _
        '                                               "Le nombre de couples de valeurs (x,y) doit être > à 2."
        '                    Else
        '                        RegLinDom.sMsgErreur = RegLinDom.sMsgErreur & "La variance des x et des y doivent être > à zéro."
        '                    End If
        '                End If
        '            Else
        '                RegLinDom.bErreur = True
        '                RegLinDom.sMsgErreur = "Droite de régression non calculée..." & vbCrLf & _
        '                                       "Aucun enregistrement retourné par le domaine."
        '            End If
        '        End If
        'fin:
        '        oRs = Nothing
        '        oDb = Nothing
        '        If bMsgBoxErr And RegLinDom.bErreur Then
        '            MsgBox(RegLinDom.sMsgErreur)
        '        End If
        '        Exit Function
        'RLErr:
        '        RegLinDom.bErreur = True
        '        RegLinDom.sMsgErreur = "Erreur n°" & Err.Number & vbCrLf & "Description :" & Err.Description
        '        Resume fin
        '    End Function

        ' Vérifie sommairement les paramètres de la fonction de statistiques
        PUBLIC Function IsStatDomErr(sMsgErr As String, sNomFunc As String,
                                      sDomaine As String, sExpression1 As String,
                                      Optional sExpression2 As String = vbNullString) As Boolean
            IsStatDomErr = False
            sMsgErr = vbNullString
            If Len(Trim$(sDomaine)) = 0 Then
                sMsgErr = "<Domaine> ne peut être vide..."
            Else
                sMsgErr = StatExpressionErr(sNomFunc, Trim$(sExpression1))
                If Len(sMsgErr) = 0 And sExpression2 <> vbNullString Then
                    sMsgErr = StatExpressionErr(sNomFunc, Trim$(sExpression2))
                End If
            End If
            If Len(sMsgErr) > 0 Then IsStatDomErr = True
        End Function

        ' Vérifie sommairement les expressions
        PUBLIC Function StatExpressionErr(sNomFunc As String, sExpression As String) As String
            StatExpressionErr = ""
            If Len(sExpression) = 0 Then
                StatExpressionErr = "<Expression> ne peut être vide..."
            ElseIf sExpression = "*" Or InStr(1, sExpression, ".*", vbBinaryCompare) > 0 Then
                StatExpressionErr = "Le " & sNomFunc & " ne peut être calculé sur l'ensemble des colonnes (*)..."
            ElseIf InStr(1, sExpression, ",", vbBinaryCompare) > 0 Then
                StatExpressionErr = "<Expression> ne doit pas retourner plus d'un champ..."
            ElseIf InStr(1, sExpression, " AS ", vbTextCompare) > 0 Then
                StatExpressionErr = "Le champ de <Expression> ne doit pas être aliasé..."
            End If
        End Function

        ' ProbFisher renvoie la probabilité suivant la loi F de Fisher
        ' Permet de calculer indirectement la probabilité de la loi t, normale et Khi2
        ' Remarque : - Code légèrement modifié par moi (PB) pour éviter une erreur de calcul (voir source)
        Public Function ProbFisher(ByVal dFRatio As Double,
                                    ByVal lDF1 As Long,
                                    ByVal lDF2 As Long) As Single

            ' From : Egon Dorrer, "Algorithm 322: F-Distribution [S14]", Communications of the
            '        Association for Computing Machinery 11:2:116-117 (1968).
            On Error GoTo errortag
            Const cdEpsilon As Double = 0.000001
            Dim dInvPi As Double
            Dim w As Double, y As Double, z As Double, zk As Double, d As Double, p As Double
            Dim i As Long, j As Long
            Dim a As Integer, b As Integer

            If dFRatio < cdEpsilon Or lDF1 < 1 Or lDF2 < 1 Then
                ProbFisher = 1
                GoTo fin
            End If
            If lDF1 > gclMaxDF Then lDF1 = gclMaxDF 'clMaxDF ~ infini
            If lDF2 > gclMaxDF Then lDF2 = gclMaxDF

            dInvPi = 1 / (4 * Math.Atan(1)) ' 1 / Pi
            dInvPi = 1 / Pi

            a = IIf(lDF1 Mod 2 = 0, 2, 1)
            b = IIf(lDF2 Mod 2 = 0, 2, 1)
            w = dFRatio * lDF1 / lDF2
            z = 1 / (1 + w)

            If a = 1 Then
                If b = 1 Then
                    p = w ^ 0.5
                    d = dInvPi * z / p
                    p = 2 * dInvPi * Math.Atan(p)
                Else
                    p = (w * z) ^ 0.5
                    d = 0.5 * p * z / w
                End If
            ElseIf b = 1 Then
                p = z ^ 0.5
                d = 0.5 * p * z
                p = 1 - p
            Else
                d = z * z
                p = w * z
            End If

            y = 2 * w / z
            If a = 1 Then
                For i = b + 2 To lDF2 Step 2
                    d = d * (1 + a / (i - 2)) * z
                    p = p + (d * y / (i - 1))
                Next i
            Else
                'zk = z ^ ((lDF2 - 2) / 2) ' Code original mais zk faux si b = 2
                ' -> Code modifié par PB :
                If b = 2 Then
                    zk = z ^ ((lDF2 - 2) / 2)
                Else
                    zk = z ^ ((lDF2 - 1) / 2)
                End If
                ' <- Fin du code modifié
                d = d * zk * lDF2 / b
                p = p * zk + w * z * (zk - 1) / (z - 1)
            End If

            y = w * z
            z = 2 / z
            b = lDF2 - 2
            For i = a + 2 To lDF1 Step 2
                j = i + b
                d = d * y * j / (i - 2)
                p = p - (z * d / j)
            Next i

            If p < 0 Then
                p = 0
            ElseIf p > 1 Then
                p = 1
            End If
            ProbFisher = 1 - p
fin:
            Exit Function
errortag:
            ProbFisher = -1
            Resume fin
        End Function

        ' ProbStudent renvoie la probabilité suivant la loi t de Student (bilatéral)
        '   q : quantile choisie
        '   lDF : nombre de variables
        ' donne la probabilité pour que le quantile choisit soit atteint
        Public Function ProbStudent(q As Double, lDF As Long) As Single
            ProbStudent = ProbFisher(q ^ 2, 1, lDF)
        End Function

        ' ProbStudent renvoie la probabilité suivant la loi t de Student (bilatéral)
        '   q : quantilechoisie
        '   lDF : nombre de variables
        ' donne la probabilité pour que le quantile choisit soit atteint
        Public Function ProbStudent_Inverse(p As Double, lDF As Long) As Single
            Dim q, qMax, qMin, pMax, pMin, pMoy As Single
            Dim i As Integer

            qMax = 318
            qMin = 0.674
            q = (qMax + qMin) / 2
            pMax = ProbFisher(qMax ^ 2, 1, lDF)
            pMin = ProbFisher(qMin ^ 2, 1, lDF)
            For i = 0 To 10000
                pMoy = ProbFisher(q ^ 2, 1, lDF)
                If Math.Abs(pMoy - p) < 0.001 Then Exit For
                If pMoy > p Then
                    qMin = q
                Else
                    qMax = q
                End If
                q = (qMax + qMin) / 2
            Next i
            ProbStudent_Inverse = q
        End Function

        ' ProbStudent renvoie la probabilité suivant la loi t de Student (bilatéral)
        '   q : quantilechoisie
        '   lDF : nombre de variables
        ' donne la probabilité pour que le quantile choisit soit atteint
        Public Function ProbStudent_InverseTab(i As Integer) As Single
            Dim Res(50) As Single
            Dim Precision As Single

            ' Appel à la fonction Excell
            'XlApp = CreateObject("Excel.Application")
            Precision = 1 - 0.682
            'ResStudent = XlApp.loi.Student.inverse(Precision, i)

            If i < 1 Or i > 50 Then
                MsgBox("erreur sur le nombre de variables de la Loi Student")
                ''Res(i) = 0
                'ProbStudent_InverseTab = Res(i)
                ProbStudent_InverseTab = 0
                Exit Function
            End If
            Res(1) = 1.832607384
            Res(2) = 1.318781408
            Res(3) = 1.19483604
            Res(4) = 1.139767749
            Res(5) = 1.108748712
            Res(6) = 1.088874346
            Res(7) = 1.075061826
            Res(8) = 1.064908212
            Res(9) = 1.057131235
            Res(10) = 1.050984641
            Res(11) = 1.04600478
            Res(12) = 1.041888475
            Res(13) = 1.038429159
            Res(14) = 1.03548125
            Res(15) = 1.032939201
            Res(16) = 1.030724629
            Res(17) = 1.028778106
            Res(18) = 1.027053759
            Res(19) = 1.025515616
            Res(20) = 1.024135068
            Res(21) = 1.022889082
            Res(22) = 1.021758903
            Res(23) = 1.020729106
            Res(24) = 1.019786889
            Res(25) = 1.018921536
            Res(26) = 1.018124013
            Res(27) = 1.017386646
            Res(28) = 1.016702878
            Res(29) = 1.016067069
            Res(30) = 1.015474347
            Res(31) = 1.014920475
            Res(32) = 1.014401755
            Res(33) = 1.013914944
            Res(34) = 1.013457186
            Res(35) = 1.013025955
            Res(36) = 1.012619012
            Res(37) = 1.01223436
            Res(38) = 1.011870217
            Res(39) = 1.011524985
            Res(40) = 1.011197228
            Res(41) = 1.010885653
            Res(42) = 1.010589091
            Res(43) = 1.01030648
            Res(44) = 1.010036861
            Res(45) = 1.009779357
            Res(46) = 1.009533169
            Res(47) = 1.009297568
            Res(48) = 1.009071886
            Res(49) = 1.008855508
            Res(50) = 1.008647871
            ProbStudent_InverseTab = 1
            If i > 0 And i < 51 Then
                ProbStudent_InverseTab = Res(i)
            ElseIf i < 1 Then
                ProbStudent_InverseTab = 1.832607384
            ElseIf i > 50 Then
                ProbStudent_InverseTab = 1
            End If
        End Function

        ' ProbKhi2 renvoie la probabilité du Khi2
        Public Function ProbKhi2(dKhi2 As Double, lDF As Long) As Single
            ProbKhi2 = IIf(lDF > 0, ProbFisher(dKhi2 / lDF, lDF, gclMaxDF), -1)
        End Function
    End Module

End Namespace
