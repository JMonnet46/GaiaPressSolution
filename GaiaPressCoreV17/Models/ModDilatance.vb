Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic

Module ModDilatance
    Public Function Dilatance(ByVal Phi As Single, ByVal Phimu As Single, ByVal TypeDilatance As Integer) As Single
        Dim R, Sinus As Single

        Dilatance = 0
        If Phimu <= 0 Then
            If Lang = 1 Then
                MsgBox("L'angle de frottement intergranulaire est nul")
                Phimu = 1
            Else
                MsgBox("The interparticle angle of friction is null")
                Phimu = 1
            End If
        End If
        Select Case TypeDilatance
            Case 1 ' Type dilatance Triaxiale
                R = 2 * Math.Sin(Arad(Phi)) * Math.Sqrt(2) / (3 - Math.Sin(Arad(Phi))) - Math.Tan(Arad(Phimu))
                Sinus = Math.Sqrt(3 * R ^ 2 / (2 - R ^ 2))
                Dilatance = Math.Asin(Sinus)
                Dilatance = Adeg(Dilatance)
                If Phi < Phimu Then Dilatance = 0

            Case 2  ' Type dilatance pressiomètre
                R = 2 * Math.Sin(Arad(Phi)) * Math.Sqrt(2) / (3 - Math.Sin(Arad(Phi))) - Math.Tan(Arad(Phimu))
                Sinus = Math.Sqrt(3 * R ^ 2 / (2 - R ^ 2))
                Dilatance = Math.Asin(Sinus)
                Dilatance = Adeg(Dilatance)
                If Phi < Phimu Then Dilatance = 0

            Case 3  ' Type dilatance approchée
                Dilatance = Phi - Phimu

        End Select

    End Function


End Module
