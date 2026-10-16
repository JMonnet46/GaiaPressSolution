Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic
Module ModTrigo

    Public Function Sec(ByVal X As Single) As Single
        ' Sécante
        Sec = 1 / Math.Cos(X)
    End Function
    Public Function Cosec(ByVal X As Single) As Single
        ' Cosécante
        Cosec = 1 / Math.Sin(X)
    End Function
    Public Function Cotan(ByVal X As Single) As Single
        ' Cotangente
        Cotan = 1 / Math.Tan(X)
    End Function

    Public Function Arcsin(ByVal X As Single) As Single
        ' Arcsinus
        Arcsin = Math.Atan(X / Math.Sqrt(-X * X + 1))
    End Function
    Public Function Arccos(ByVal X As Single) As Single
        ' Arccosinus
        Arccos = Math.Atan(-X / Math.Sqrt(-X * X + 1)) + 2 * Math.Atan(1)
    End Function
    Public Function Arcsec(X As Single) As Single
        ' Arcsécante
        Arcsec = Math.Atan(X / Math.Sqrt(X * X - 1)) + Math.Sign(X - 1) * (2 * Math.Atan(1))
    End Function

    Public Function Arccosec(ByVal X As Single) As Single
        ' Arccosécante
        Arccosec = Math.Atan(X / Math.Sqrt(X * X - 1)) + (Math.Sign(X) - 1) * (2 * Math.Atan(1))
    End Function

    Public Function Arccotan(ByVal X As Single) As Single
        ' Arccotangente
        Arccotan = Math.Atan(X) + 2 * Math.Atan(1)
    End Function
    Public Function HSin(ByVal X As Single) As Single
        'Sinus hyperbolique
        HSin = (Math.Exp(X) - Math.Exp(-X)) / 2
    End Function
    Public Function HCos(ByVal X As Single) As Single
        ' Cosinus hyperbolique
        HCos = (Math.Exp(X) + Math.Exp(-X)) / 2
    End Function
    Public Function HTan(ByVal X As Single) As Single
        'Tangente hyperbolique
        HTan = (Math.Exp(X) - Math.Exp(-X)) / (Math.Exp(X) + Math.Exp(-X))
    End Function

    Public Function HSec(ByVal X As Single) As Single
        ' Sécante hyperbolique
        HSec = 2 / (Math.Exp(X) + Math.Exp(-X))
    End Function

    Public Function HCosec(ByVal X As Single) As Single
        ' Cosécante hyperbolique
        HCosec = 2 / (Math.Exp(X) - Math.Exp(-X))
    End Function

    Public Function HCotan(ByVal X As Single) As Single
        ' Cotangente hyperbolique
        HCotan = (Math.Exp(X) + Math.Exp(-X)) / (Math.Exp(X) - Math.Exp(-X))
    End Function

    Public Function HArcsin(ByVal X As Single) As Single
        ' Arcsinus hyperbolique
        HArcsin = Math.Log(X + Math.Sqrt(X * X + 1))
    End Function

    Public Function HArccos(ByVal X As Single) As Single
        ' Arccosinus hyperbolique
        HArccos = Math.Log(X + Math.Sqrt(X * X - 1))
    End Function

    Public Function HArctan(ByVal X As Single) As Single
        ' Arctangente hyperbolique
        HArctan = Math.Log((1 + X) / (1 - X)) / 2
    End Function

    Public Function HArcsec(ByVal X As Single) As Single
        ' Arcsécante hyperbolique
        HArcsec = Math.Log((Math.Sqrt(-X * X + 1) + 1) / X)
    End Function

    Public Function HArccosec(ByVal X As Single) As Single
        ' Arccosécante hyperbolique
        HArccosec = Math.Log((Math.Sign(X) * Math.Sqrt(X * X + 1) + 1) / X)
    End Function

    Public Function HArccotan(ByVal X As Single) As Single
        ' Arccotangente hyperbolique
        HArccotan = Math.Log((X + 1) / (X - 1)) / 2
    End Function

    Public Function Adeg(ByVal Angle As Single) As Single
        ' Conversion de l'angle en radian vers un angle en degré
        Const Pi = 3.14159265358979
        Adeg = Angle / Pi * 180
    End Function

    Public Function Arad(ByVal Angle As Single) As Single
        ' Conversion de l'angle en degré vers un angle en radian
        Const Pi = 3.14159265358979
        Arad = Angle * Pi / 180
    End Function

    'Public Function Inf(ByRef Tableau() As Single, ByVal nb As Integer) As VarInf
    '    ' fonction de calcul du minimum d'un tableau
    '    Dim i As Integer
    '    Inf.Valeur = Tableau(0)
    '    For i = 0 To nb - 1
    '        If Tableau(i) < Inf.Valeur Then
    '            Inf.Valeur = Tableau(i)
    '            Inf.indice = i
    '        End If
    '    Next i
    'End Function

End Module
