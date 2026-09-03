
Module ModMath

    '**********************************************
    'fonction qui calcule la regression polynomiale
    '**********************************************
    Public Function RegPolynome(ByVal NPD As Integer, ByRef XArray() As Single, _
                    ByRef YArray() As Single, ByVal N As Integer) As Polynome

        Dim AArray(,) As Double
        Dim BArray() As Double
        Dim i, j, k As Integer
        Dim poly As Polynome

        ' REGRESSION LINEAIRE PAR POLYNOME
        ' Le polynome est de degré N limité à 5 maximum
        ' CONSTITUTION DE LA MATRICE DES MOINDRES CARRES
        If N <= 5 And N < NPD Then
            ReDim AArray(N, N)
            ReDim BArray(N)

            For i = 0 To N
                For j = i To N
                    For k = 0 To NPD - 1
                        AArray(i, j) = AArray(i, j) + (XArray(k)) ^ (i + j)
                    Next k
                Next j
            Next i

            For i = 0 To N
                For k = 0 To NPD - 1
                    BArray(i) = BArray(i) + (YArray(k) * XArray(k) ^ (i))
                Next k
            Next i

            ' Remplissage de l'autre partie de la matrice
            For i = 1 To N
                For j = 0 To i - 1
                    AArray(i, j) = AArray(j, i)
                Next j
            Next i
            'With Resultat
            '    .A = AArray
            '    .B = BArray
            '    .N = N + 1
            'End With

            'Resultat = Diagonal(Resultat)
            ''**** PROCEDURE DE RESOLUTION DE SYSTEME LINEAIRE PAR DIAGONALISATION****
            'With Resultat
            '    AArray = .A
            '    BArray = .B
            'End With

            ' Diagonalisation du système
            For k = 0 To N
                For j = k + 1 To N
                    AArray(k, j) = AArray(k, j) / AArray(k, k)
                Next j

                BArray(k) = BArray(k) / AArray(k, k)
                For i = 0 To N
                    If i <> k Then
                        For j = (k + 1) To N
                            AArray(i, j) = AArray(i, j) - AArray(k, j) * AArray(i, k)
                        Next j
                        BArray(i) = BArray(i) - BArray(k) * AArray(i, k)
                    End If
                Next i
            Next k


            For i = 0 To N
                Select Case i
                    Case 0
                        poly.A0 = BArray(0)
                    Case 1
                        poly.A1 = BArray(1)
                    Case 2
                        poly.A2 = BArray(2)
                    Case 3
                        poly.A3 = BArray(3)
                    Case 4
                        poly.A4 = BArray(4)
                    Case 4
                        poly.A5 = BArray(5)
                End Select
            Next

            poly.Erreur = False
            RegPolynome = poly
        Else
            ' Le degré demandé ou le nombre de points est incorrect:
            RegPolynome.Erreur = True
        End If

    End Function

    '**********************************************
    'fonction de diagonalisation d'une matrice
    '**********************************************
    Public Function Diagonal(ByRef Donnee As Diagonalisation) As Diagonalisation

        '**** PROCEDURE DE RESOLUTION DE SYSTEME LINEAIRE PAR DIAGONALISATION****
        Dim i, j, k, N As Integer
        '**** DIAGONALISATION ****
        ' Passage des paramètres


        With Donnee
            N = .N
            For k = 0 To N - 1
                For j = k + 1 To N - 1
                    .A(k, j) = .A(k, j) / .A(k, k)
                Next j

                .B(k) = .B(k) / .A(k, k)
                For i = 0 To N - 1
                    If i <> k Then
                        For j = (k + 1) To N - 1
                            .A(i, j) = .A(i, j) - .A(k, j) * .A(i, k)
                        Next j
                        .B(i) = .B(i) - .B(k) * .A(i, k)
                    End If
                Next i
            Next k
        End With

        With Donnee
            Diagonal.A = .A
            Diagonal.B = .B
            Diagonal.N = .N
        End With
    End Function
    '**********************************************
    ' fonction de résolution d'une équation du second degré
    '**********************************************
    Public Function RACINE2(ByVal A2 As Single, ByVal A1 As Single, ByVal A0 As Single) As Solution
        Dim Delta As Single
        Dim sol As Solution
        sol.Erreur = 0
        sol.X1 = 0
        sol.X2 = 0
        With sol
            If A2 <> 0 Then

                Delta = A1 ^ 2 - 4 * A2 * A0
                If Delta > 0 Then

                    .X1 = (-A1 + Math.Sqrt(Delta)) / 2 / A2
                    .X2 = (-A1 - Math.Sqrt(Delta)) / 2 / A2
                    .Erreur = False
                    RACINE2 = sol
                End If
                If Delta = 0 Then
                    .X1 = -A1 / 2 / A2
                    .X2 = .X1
                    .Erreur = False

                End If
                If Delta < 0 Then
                    .Erreur = True
                End If
            End If

            If A2 = 0 And A0 <> 0 Then
                .Erreur = True
                .X1 = .X2 = -A1 / A0
            End If
        End With

    End Function

    '**********************************************
    'fonction qui convertie des degrés en radiant
    '*********************************************
    Public Function DTOR(ByVal Phi As Single) As Single
        DTOR = Phi * Pi / 180
    End Function

    '**********************************************
    'fonction construit un polynome
    '*********************************************
    Public Function Fonct(ByVal A3 As Single, ByVal A2 As Single, ByVal A1 As Single, ByVal A0 As Single, ByVal x As Single) As Single

        Fonct = A0 + A1 * x + A2 * x ^ 2 + A3 * x ^ 3

    End Function

    '**********************************************
    'fonction de résolution d'une équation
    'du 3éme degré par la méthode de dichotomie
    '*********************************************
    Public Function RACINE3D(ByVal A3 As Single, ByVal A2 As Single, ByVal A1 As Single, _
                            ByVal A0 As Single, ByVal X1 As Single, ByVal X2 As Single) As solution3D

        Dim Solution As solution3D
        'Dim X As Single
        Dim F2, F, F1 As Single
        Dim Epsilon As Single
        Dim Iter As Integer

        'Application de la méthode de dichotomie à la fonction

        Iter = 0
        Epsilon = 0.01

        Do
            Iter = Iter + 1
            F1 = Fonct(A3, A2, A1, A0, X1)
            F2 = Fonct(A3, A2, A1, A0, X2)

            Solution.x = (X1 + X2) / 2

            If F1 * F2 < 0 Then
                F = Fonct(A3, A2, A1, A0, Solution.x)
            Else
                If Iter = 1 Then
                    If Math.Abs(F1) >= Math.Abs(F2) Then
                        Solution.x = X2
                    Else
                        Solution.x = X1
                    End If
                    Exit Do
                End If

            End If
            If F * F1 > 0 Then
                X1 = Solution.x
            Else
                X2 = Solution.x
            End If

        Loop Until (Math.Abs(F) < Epsilon) Or (Iter > 100)

        If (Math.Abs(F) < Epsilon) Then
            Solution.Erreur = False
        Else
            Solution.Erreur = True
        End If
        RACINE3D.x = Solution.x
        RACINE3D.Erreur = Solution.Erreur

    End Function

    ''**********************************************
    ''fonction Loi de Student
    ''*********************************************
    'Public Function Student(ByVal A3 As Single, ByVal A2 As Single, ByVal A1 As Single, ByVal A0 As Single, ByVal x As Single) As Single

    '    Student = A0 + A1 * x + A2 * x ^ 2 + A3 * x ^ 3

    'End Function

End Module
