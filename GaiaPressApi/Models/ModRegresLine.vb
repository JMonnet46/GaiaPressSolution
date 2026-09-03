Module ModRegresLine

    '*********************************************
    'fonction qui calcule une regression linéaire
    'd'une série de mesure
    '*********************************************
    Public Function RegresLine(ByVal N As Integer, ByRef X() As Single, ByRef Y() As Single) As Regression
        Dim i, J As Integer
        Dim SomX, SomY, SomX2, SomXY, MoyX, MoyY, Exi, Eyi As Double
        Dim Precis, PrecisA, PrecisB As Single
        Dim AArray(0 To 1, 0 To 1) As Double
        Dim BArray(0 To 1) As Double
        Dim Resultat As Diagonalisation

        SomX = 0
        SomY = 0
        SomX2 = 0
        SomXY = 0
        Exi = 0
        Eyi = 0

        If N >= 1 Then

            For i = 0 To N
                SomY = SomY + Y(i)
                SomX = SomX + X(i)
            Next i

            MoyX = SomX / (N + 1)
            MoyY = SomY / (N + 1)
            SomX = 0
            SomY = 0

            For i = 0 To N
                SomX = SomX + X(i)
                SomY = SomY + Y(i)
                SomXY = SomXY + X(i) * Y(i)
                SomX2 = SomX2 + X(i) ^ 2
                Exi = Exi + (X(i) - MoyX) ^ 2
                Eyi = Eyi + (Y(i) - MoyY) ^ 2
            Next i

            For i = 0 To 1
                For J = 0 To 1
                    AArray(i, J) = 0
                Next J
                BArray(i) = 0
            Next i

            AArray(0, 0) = SomX2
            AArray(0, 1) = SomX
            AArray(1, 0) = SomX
            AArray(1, 1) = N + 1
            BArray(0) = SomXY
            BArray(1) = SomY
            Precis = Math.Sqrt(Eyi / (N - 1))
            PrecisA = Precis * Math.Sqrt(SomX2 / ((N + 1) * Exi))
            PrecisB = Precis / Math.Sqrt(Exi)

            ' passage de parametres
            With Resultat
                .A = AArray
                .B = BArray
                .N = 2
            End With

            Resultat = Diagonal(Resultat)

            'passage de paramètres
            With Resultat
                RegresLine.b = .B(1) 'ordonnée à l'origine
                RegresLine.A = .B(0) 'pente
                RegresLine.SigR = Precis ' Précision de la régression linéaire
                RegresLine.SigA = PrecisA
                RegresLine.SigB = PrecisB
                RegresLine.Erreur = False
            End With

        Else
            RegresLine.Erreur = True
        End If

    End Function
End Module
