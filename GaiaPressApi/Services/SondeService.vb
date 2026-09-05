Namespace GaiaPress.core.V17.Services

    Public Class SondeService

        ' ✅ Déclarer EventsSuspendus comme variable de classe
        Public Shared EventsSuspendus As Boolean = False

        '**************************************************************************
        ' Gestion des Sondes
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables
        Public Sub AddSonde(NouvelleSonde As Probe)
            Dim row As DataRow = ObjetDataListeSondes.NewRow()
            row("refSonde") = NouvelleSonde.P_refSonde
            row("typeSonde") = NouvelleSonde.P_typeSonde
            row("typeDeformee") = NouvelleSonde.P_typeDeformee
            row("typeContrainte") = NouvelleSonde.P_typeContrainte
            row("ls") = NouvelleSonde.P_ls
            row("lg") = NouvelleSonde.P_lg
            row("lt") = NouvelleSonde.P_lt
            row("longInjectPompage") = NouvelleSonde.P_longInjectePompage
            row("Vs") = NouvelleSonde.P_Vs
            row("ds") = NouvelleSonde.P_ds
            row("ClassPrecis") = NouvelleSonde.P_ClassPrecis
            row("epMembraneGarde") = NouvelleSonde.P_EpMembraneGarde
            row("PrEpMembraneGarde") = NouvelleSonde.P_PrEpMembraneGarde
            row("epMembraneMesure") = NouvelleSonde.P_EpMembraneMesure
            row("PrEpMembraneMesure") = NouvelleSonde.P_PrEpMembraneMesure
            row("epTubeFendu") = NouvelleSonde.P_epTubeFendu
            row("PrEpTubeFendu") = NouvelleSonde.P_PrEpTubeFendu
            row("epaisInitMembrane") = NouvelleSonde.P_EpInitMembrane
            row("PrEpaisInitMembrane") = NouvelleSonde.P_PrEpInitMembrane
            row("PrLong") = NouvelleSonde.P_PrLong
            row("PrDiam") = NouvelleSonde.P_PrDiam
            row("PrP") = NouvelleSonde.P_PrP
            row("PrV") = NouvelleSonde.P_PrV
            row("PrPV") = NouvelleSonde.P_PrPV
            row("PrZ") = NouvelleSonde.P_PrZ
            row("PrT") = NouvelleSonde.P_Prt
            row("Pm") = NouvelleSonde.P_Pm
            ObjetDataListeSondes.Rows.Add(row)
        End Sub

        Public Sub UpdateSonde(NouvelleSonde As Probe)
            For Each row As DataRow In ObjetDataListeSondes.Rows
                If row("refSonde").ToString() = NouvelleSonde.P_refSonde Then
                    row("refSonde") = NouvelleSonde.P_refSonde
                    row("typeSonde") = NouvelleSonde.P_typeSonde
                    row("typeDeformee") = NouvelleSonde.P_typeDeformee
                    row("typeContrainte") = NouvelleSonde.P_typeContrainte
                    row("ls") = NouvelleSonde.P_ls
                    row("lg") = NouvelleSonde.P_lg
                    row("lt") = NouvelleSonde.P_lt
                    row("longInjectPompage") = NouvelleSonde.P_longInjectePompage
                    row("Vs") = NouvelleSonde.P_Vs
                    row("ds") = NouvelleSonde.P_ds
                    row("ClassPrecis") = NouvelleSonde.P_ClassPrecis
                    row("epMembraneGarde") = NouvelleSonde.P_EpMembraneGarde
                    row("PrEpMembraneGarde") = NouvelleSonde.P_PrEpMembraneGarde
                    row("epMembraneMesure") = NouvelleSonde.P_EpMembraneMesure
                    row("PrEpMembraneMesure") = NouvelleSonde.P_PrEpMembraneMesure
                    row("epTubeFendu") = NouvelleSonde.P_epTubeFendu
                    row("PrEpTubeFendu") = NouvelleSonde.P_PrEpTubeFendu
                    row("epaisInitMembrane") = NouvelleSonde.P_EpInitMembrane
                    row("PrEpaisInitMembrane") = NouvelleSonde.P_PrEpInitMembrane
                    row("PrLong") = NouvelleSonde.P_PrLong
                    row("PrDiam") = NouvelleSonde.P_PrDiam
                    row("PrP") = NouvelleSonde.P_PrP
                    row("PrV") = NouvelleSonde.P_PrV
                    row("PrPV") = NouvelleSonde.P_PrPV
                    row("PrZ") = NouvelleSonde.P_PrZ
                    row("PrT") = NouvelleSonde.P_Prt
                    row("Pm") = NouvelleSonde.P_Pm
                    Exit For
                End If
            Next
        End Sub

        Public Sub DeleteSonde(refSonde As String)
            Dim rows() As DataRow = ObjetDataListeSondes.Select("refSonde = '" & refSonde & "'")
            If rows.Length > 0 Then
                ObjetDataListeSondes.Rows.Remove(rows(0))
            End If
        End Sub
        ''' <summary>
        ''' Récupère une sonde par sa référence depuis ObjetDataListeSondes.
        ''' </summary>
        Public Function GetSondeByRef(refSonde As String) As Probe
            ' Filtrer les lignes correspondantes dans ObjetDataListeSondes
            Dim rows() As DataRow = ObjetDataListeSondes.Select($"refSonde = '{refSonde}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Probe avec les valeurs de la DataRow
            Return New Probe With {
            .P_refSonde = If(rows(0)("refSonde") Is DBNull.Value, String.Empty, rows(0)("refSonde").ToString()),
            .P_typeSonde = If(rows(0)("typeSonde") Is DBNull.Value, 0, CInt(rows(0)("typeSonde"))),
            .P_typeDeformee = If(rows(0)("typeDeformee") Is DBNull.Value, 0, CInt(rows(0)("typeDeformee"))),
            .P_typeContrainte = If(rows(0)("typeContrainte") Is DBNull.Value, 0, CInt(rows(0)("typeContrainte"))),
            .P_ls = If(rows(0)("ls") Is DBNull.Value, 0, CSng(rows(0)("ls"))),
            .P_lg = If(rows(0)("lg") Is DBNull.Value, 0, CSng(rows(0)("lg"))),
            .P_lt = If(rows(0)("lt") Is DBNull.Value, 0, CSng(rows(0)("lt"))),
            .P_longInjectePompage = If(rows(0)("longInjectPompage") Is DBNull.Value, 0, CSng(rows(0)("longInjectPompage"))),
            .P_Vs = If(rows(0)("Vs") Is DBNull.Value, 0, CSng(rows(0)("Vs"))),
            .P_ds = If(rows(0)("ds") Is DBNull.Value, 0, CSng(rows(0)("ds"))),
            .P_ClassPrecis = If(rows(0)("ClassPrecis") Is DBNull.Value, 0, CInt(rows(0)("ClassPrecis"))),
            .P_EpMembraneGarde = If(rows(0)("epMembraneGarde") Is DBNull.Value, 0, CSng(rows(0)("epMembraneGarde"))),
            .P_PrEpMembraneGarde = If(rows(0)("PrEpMembraneGarde") Is DBNull.Value, 0, CSng(rows(0)("PrEpMembraneGarde"))),
            .P_EpMembraneMesure = If(rows(0)("epMembraneMesure") Is DBNull.Value, 0, CSng(rows(0)("epMembraneMesure"))),
            .P_PrEpMembraneMesure = If(rows(0)("PrEpMembraneMesure") Is DBNull.Value, 0, CSng(rows(0)("PrEpMembraneMesure"))),
            .P_epTubeFendu = If(rows(0)("epTubeFendu") Is DBNull.Value, 0, CSng(rows(0)("epTubeFendu"))),
            .P_PrEpTubeFendu = If(rows(0)("PrEpTubeFendu") Is DBNull.Value, 0, CSng(rows(0)("PrEpTubeFendu"))),
            .P_EpInitMembrane = If(rows(0)("epaisInitMembrane") Is DBNull.Value, 0, CSng(rows(0)("epaisInitMembrane"))),
            .P_PrEpInitMembrane = If(rows(0)("PrEpaisInitMembrane") Is DBNull.Value, 0, CSng(rows(0)("PrEpaisInitMembrane"))),
            .P_PrLong = If(rows(0)("PrLong") Is DBNull.Value, 0, CSng(rows(0)("PrLong"))),
            .P_PrDiam = If(rows(0)("PrDiam") Is DBNull.Value, 0, CSng(rows(0)("PrDiam"))),
            .P_PrP = If(rows(0)("PrP") Is DBNull.Value, 0, CSng(rows(0)("PrP"))),
            .P_PrV = If(rows(0)("PrV") Is DBNull.Value, 0, CSng(rows(0)("PrV"))),
            .P_PrPV = If(rows(0)("PrPV") Is DBNull.Value, 0, CSng(rows(0)("PrPV"))),
            .P_PrZ = If(rows(0)("PrZ") Is DBNull.Value, 0, CSng(rows(0)("PrZ"))),
            .P_Prt = If(rows(0)("PrT") Is DBNull.Value, 0, CSng(rows(0)("PrT"))),
            .P_Pm = If(rows(0)("Pm") Is DBNull.Value, 0, CSng(rows(0)("Pm")))
        }
        End Function

        '**************************************************************************
        ' Gestion des EssaisExpansion
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables de  ObjetDataEssaisExpansionPropre
        Public Sub AddEssaiExpansion(NouvelEssai As Essai_Expansion)
            Dim row As DataRow = ObjetDataListeSondes.NewRow()
            row("refEssai") = NouvelEssai.E_RefEssai
            row("refSonde") = NouvelEssai.E_RefSonde
            row("ClassPrecis") = NouvelEssai.E_ClassPrecis
            row("nomEssai") = NouvelEssai.E_nomEssai
            row("dateEssai") = NouvelEssai.E_dateEssai
            row("difPresEauAir") = NouvelEssai.E_DifPresEauAir
            row("hauteurCpv") = NouvelEssai.E_HauteurCpv
            row("Di") = NouvelEssai.E_Di
            row("Vc") = NouvelEssai.E_Vc
            row("Vs") = NouvelEssai.E_Vs
            row("EpMembrane") = NouvelEssai.E_EpMembrane
            row("Prp") = NouvelEssai.E_Prp
            row("PrV") = NouvelEssai.E_PrV
            row("coefficientA") = NouvelEssai.E_coefficientA
            row("coefficientB") = NouvelEssai.E_coefficientB
            row("PrecisVc") = NouvelEssai.E_PrecisVc
            row("PrecisVs") = NouvelEssai.E_PrecisVs
            row("PrEpMembrane") = NouvelEssai.E_PrEpMembrane
            row("PrecisA") = NouvelEssai.E_PrecisA
            row("PrecisB") = NouvelEssai.E_PrecisB
            ObjetDataListeSondes.Rows.Add(row)
        End Sub

        '' Récupère une sonde par sa référence depuis ObjetDataListeSondes.
        ''' </summary>
        Public Function GetEssaiExpansionByRef(refEssai As String) As Essai_Expansion
            ' Filtrer les lignes correspondantes dans ObjetDataListeSondes
            Dim rows() As DataRow = ObjetDataEssaisExpansionPropre.Select($"refEssai = '{refEssai}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Probe avec les valeurs de la DataRow
            Return New Essai_Expansion With {
            .E_RefSonde = If(rows(0)("refSonde") Is DBNull.Value, String.Empty, rows(0)("refSonde").ToString()),
            .E_ClassPrecis = If(rows(0)("ClassPrecis") Is DBNull.Value, 0, CInt(rows(0)("ClassPrecis"))),
            .E_nomEssai = If(rows(0)("nomEssai") Is DBNull.Value, 0, CInt(rows(0)("nomEssai"))),
            .E_dateEssai = If(rows(0)("dateEssai") Is DBNull.Value, 0, CInt(rows(0)("dateEssai"))),
            .E_DifPresEauAir = If(rows(0)("difPresEauAir") Is DBNull.Value, 0, CSng(rows(0)("difPresEauAir"))),
            .E_HauteurCpv = If(rows(0)("hauteurCpv") Is DBNull.Value, 0, CSng(rows(0)("hauteurCpv"))),
            .E_Di = If(rows(0)("Di") Is DBNull.Value, 0, CSng(rows(0)("Di"))),
            .E_Vc = If(rows(0)("Vc") Is DBNull.Value, 0, CSng(rows(0)("Vc"))),
            .E_Vs = If(rows(0)("Vs") Is DBNull.Value, 0, CSng(rows(0)("Vs"))),
            .E_EpMembrane = If(rows(0)("EpMembrane") Is DBNull.Value, 0, CSng(rows(0)("EpMembrane"))),
            .E_Prp = If(rows(0)("Prp") Is DBNull.Value, 0, CInt(rows(0)("Prp"))),
            .E_PrV = If(rows(0)("PrV") Is DBNull.Value, 0, CSng(rows(0)("PrV"))),
            .E_coefficientA = If(rows(0)("coefficientA") Is DBNull.Value, 0, CSng(rows(0)("coefficientA"))),
            .E_coefficientB = If(rows(0)("coefficientB") Is DBNull.Value, 0, CSng(rows(0)("coefficientB"))),
            .E_PrecisVc = If(rows(0)("PrecisVc") Is DBNull.Value, 0, CSng(rows(0)("PrecisVc"))),
            .E_PrecisVs = If(rows(0)("PrecisVs") Is DBNull.Value, 0, CSng(rows(0)("PrecisVs"))),
            .E_PrEpMembrane = If(rows(0)("PrEpMembrane") Is DBNull.Value, 0, CSng(rows(0)("PrEpMembrane"))),
            .E_PrecisA = If(rows(0)("PrecisA") Is DBNull.Value, 0, CSng(rows(0)("PrecisA"))),
            .E_PrecisB = If(rows(0)("PrecisB") Is DBNull.Value, 0, CSng(rows(0)("PrecisB")))
        }
        End Function

        Public Sub UpdateEssaiExpansion(NouvelEssai As Essai_Expansion)
            For Each row As DataRow In ObjetDataListeSondes.Rows
                If row("refSonde").ToString() = NouvelEssai.E_RefSonde Then
                    row("refEssai") = NouvelEssai.E_RefEssai
                    row("refSonde") = NouvelEssai.E_RefSonde
                    row("ClassPrecis") = NouvelEssai.E_ClassPrecis
                    row("nomEssai") = NouvelEssai.E_nomEssai
                    row("dateEssai") = NouvelEssai.E_dateEssai
                    row("difPresEauAir") = NouvelEssai.E_DifPresEauAir
                    row("hauteurCpv") = NouvelEssai.E_HauteurCpv
                    row("Di") = NouvelEssai.E_Di
                    row("Vc") = NouvelEssai.E_Vc
                    row("Vs") = NouvelEssai.E_Vs
                    row("EpMembrane") = NouvelEssai.E_EpMembrane
                    row("Prp") = NouvelEssai.E_Prp
                    row("PrV") = NouvelEssai.E_PrV
                    row("coefficientA") = NouvelEssai.E_coefficientA
                    row("coefficientB") = NouvelEssai.E_coefficientB
                    row("PrecisVc") = NouvelEssai.E_PrecisVc
                    row("PrecisVs") = NouvelEssai.E_PrecisVs
                    row("PrEpMembrane") = NouvelEssai.E_PrEpMembrane
                    row("PrecisA") = NouvelEssai.E_PrecisA
                    row("PrecisB") = NouvelEssai.E_PrecisB
                    Exit For
                End If
            Next
        End Sub


        '**************************************************************************
        ' Gestion des EssaisResistance
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables de  ObjetDataEssaisResistancePropre
        Public Sub AddEssaiResistance(NouvelEssai As Essai_Resistance)
            Dim row As DataRow = ObjetDataListeSondes.NewRow()
            row("refEssai") = NouvelEssai.R_refEssai
            row("refSonde") = NouvelEssai.R_refSonde
            row("ClassPrecis") = NouvelEssai.R_ClassPrecis
            row("nomEssai") = NouvelEssai.R_nomEssai
            row("dateEssai") = NouvelEssai.R_dateEssai
            row("difPresEauAir") = NouvelEssai.R_difPresEauAir
            row("hauteurCpv") = NouvelEssai.R_hauteurCpv
            row("A0") = NouvelEssai.R_A0
            row("A1") = NouvelEssai.R_A1
            row("A2") = NouvelEssai.R_A2
            row("A3") = NouvelEssai.R_A3
            row("Pel") = NouvelEssai.R_Pel
            row("Pe") = NouvelEssai.R_Pe
            row("MySondeResistance") = NouvelEssai.R_MySondeResistance
            row("Mesures") = NouvelEssai.R_Mesures
            row("IsSaved") = NouvelEssai.IsSaved
            row("EventsSuspendus") = NouvelEssai.EventsSuspendus
            ObjetDataListeSondes.Rows.Add(row)
        End Sub

        ''' <summary>
        ''' Récupère un essai de résistance par sa référence depuis ObjetDataEssaiResistancePropre.
        ' Exemple de méthode modifiée pour accepter une Probe
        Public Function GetEssaiResistanceByRef(refEssai As String, probe As Probe) As Essai_Resistance
            Try
                ' ✅ Accéder à ObjetDataEssaiResistancePropre via l'instance de Probe

                Dim rows() As DataRow = probe.ObjetDataEssaiResistancePropre.Select($"refEssai = '{refEssai}'")

                ' ✅ Accéder à EventsSuspendus via la DataRow
                Dim eventsSuspendusValue As Boolean = If(rows(0).IsNull("EventsSuspendus"), False, rows(0).Field(Of Boolean)("EventsSuspendus"))

                If rows.Length = 0 Then
                    Return Nothing
                End If

                ' Créer et retourner un nouvel objet Essai_Resistance
                Return New Essai_Resistance With {
            .R_refEssai = If(rows(0).IsNull("refEssai"), String.Empty, rows(0).Field(Of String)("refEssai")),
            .R_refSonde = If(rows(0).IsNull("refSonde"), String.Empty, rows(0).Field(Of String)("refSonde")),
            .EventsSuspendus = If(rows(0).IsNull("EventsSuspendus"), False, rows(0).Field(Of Boolean)("EventsSuspendus"))
        }
            Catch ex As Exception
                Console.WriteLine($"Erreur dans GetEssaiResistanceByRef: {ex.Message}")
                Return Nothing
            End Try
        End Function



        Public Sub UpdateEssaiResistance(NouvelEssai As Essai_Resistance)
            For Each row As DataRow In ObjetDataListeSondes.Rows
                If row("refSonde").ToString() = NouvelEssai.R_refSonde Then
                    row("refEssai") = NouvelEssai.R_refEssai
                    row("refSonde") = NouvelEssai.R_refSonde
                    row("ClassPrecis") = NouvelEssai.R_ClassPrecis
                    row("nomEssai") = NouvelEssai.R_nomEssai
                    row("dateEssai") = NouvelEssai.R_dateEssai
                    row("difPresEauAir") = NouvelEssai.R_difPresEauAir
                    row("hauteurCpv") = NouvelEssai.R_hauteurCpv
                    row("A0") = NouvelEssai.R_A0
                    row("A1") = NouvelEssai.R_A1
                    row("A2") = NouvelEssai.R_A2
                    row("A3") = NouvelEssai.R_A3
                    row("Pel") = NouvelEssai.R_Pel
                    row("Pe") = NouvelEssai.R_Pe
                    row("MySondeResistance") = NouvelEssai.R_MySondeResistance
                    row("Mesures") = NouvelEssai.R_Mesures
                    row("IsSaved") = NouvelEssai.IsSaved
                    row("EventsSuspendus") = NouvelEssai.EventsSuspendus
                    Exit For
                End If
            Next
        End Sub

        ' *******************************************************************************************************
        ' Charge les Mesures depuis MaListeSondes.Tables("LesMesuresResistancePropre") vers MyResistance.Mesures
        ' *******************************************************************************************************

        '**************************************************************************
        ' Gestion des EssaisMesuresResistancePropre
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables de  ObjetDataMesuresResistancePropre
        Public Sub AddEssaiMesuresResistancePropre(NouvelleMesure As MesuresExpansion)
            Dim row As DataRow = ObjetDataMesuresResistancePropre.NewRow()
            row("refEssai") = NouvelleMesure.RefEssai
            row("IndiceMesure") = NouvelleMesure.IndiceMesure
            row("pression") = NouvelleMesure.Pression
            row("volume15") = NouvelleMesure.Volume15
            row("volume30") = NouvelleMesure.Volume30
            row("volume60") = NouvelleMesure.Volume60
            ObjetDataMesuresResistancePropre.Rows.Add(row)
        End Sub

        '' Récupère une sonde par sa référence depuis ObjetDataListeSondes.
        ''' </summary>
        Public Function GetEssaiMesuresResistancePropreByRef(refEssai As String, IndiceMesure As Integer) As MesuresExpansion
            ' Filtrer les lignes correspondantes dans ObjetDataListeSondes
            Dim rows() As DataRow = ObjetDataMesuresResistancePropre.Select($"refEssai = '{refEssai}'" And $"IndiceMesure = '{IndiceMesure}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Probe avec les valeurs de la DataRow
            Return New MesuresExpansion With {
            .RefEssai = If(rows(0)("refEssai") Is DBNull.Value, String.Empty, rows(0)("refEssai").ToString()),
            .IndiceMesure = If(rows(0)("IndiceMesure") Is DBNull.Value, String.Empty, rows(0)("IndiceMesure").ToString()),
            .Pression = If(rows(0)("pression") Is DBNull.Value, 0, CInt(rows(0)("pression"))),
            .Volume15 = If(rows(0)("volume15") Is DBNull.Value, 0, CInt(rows(0)("volume15"))),
            .Volume30 = If(rows(0)("volume30") Is DBNull.Value, 0, CInt(rows(0)("volume30"))),
            .Volume60 = If(rows(0)("volume60") Is DBNull.Value, 0, CSng(rows(0)("volume60")))
        }
        End Function

        Public Sub UpdateEssaiMesuresResistancePropre(NouvelleMesure As MesuresExpansion)
            For Each row As DataRow In ObjetDataMesuresResistancePropre.Rows
                If row("refEssai").ToString() = NouvelleMesure.RefEssai And row("IndiceMesure").ToString() = NouvelleMesure.IndiceMesure Then
                    row("pression") = NouvelleMesure.Pression
                    row("volume15") = NouvelleMesure.Volume15
                    row("volume30") = NouvelleMesure.Volume30
                    row("volume60") = NouvelleMesure.Volume60
                    Exit For
                End If
            Next
        End Sub

        Public Sub SuspendreEvents()
            SondeService.EventsSuspendus = True
        End Sub

        Public Sub RetablirEvents()
            SondeService.EventsSuspendus = False
        End Sub

        ' *******************************************************************************************************
        ' Charge les Mesures depuis MaListeSondes.Tables("LesMesuresExpansionPropre") vers MyExpansion.Mesures
        ' *******************************************************************************************************

        '**************************************************************************
        ' Gestion des EssaisMesuresExpansionPropre
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables de  ObjetDataMesuresExpansionPropre
        Public Sub AddEssaiMesuresExpansionPropre(NouvelleMesure As MesuresExpansion)
            Dim row As DataRow = ObjetDataMesuresExpansionPropre.NewRow()
            row("refEssai") = NouvelleMesure.RefEssai
            row("IndiceMesure") = NouvelleMesure.IndiceMesure
            row("pression") = NouvelleMesure.Pression
            row("volume15") = NouvelleMesure.Volume15
            row("volume30") = NouvelleMesure.Volume30
            row("volume60") = NouvelleMesure.Volume60
            ObjetDataMesuresExpansionPropre.Rows.Add(row)
        End Sub

        '' Récupère une sonde par sa référence depuis ObjetDataListeSondes.
        ''' </summary>
        Public Function GetEssaiMesuresExpansionPropreByRef(refEssai As String, IndiceMesure As Integer) As MesuresExpansion
            ' Filtrer les lignes correspondantes dans ObjetDataListeSondes
            Dim rows() As DataRow = ObjetDataMesuresExpansionPropre.Select($"refEssai = '{refEssai}'" And $"IndiceMesure = '{IndiceMesure}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Probe avec les valeurs de la DataRow
            Return New MesuresExpansion With {
            .RefEssai = If(rows(0)("refEssai") Is DBNull.Value, String.Empty, rows(0)("refEssai").ToString()),
            .IndiceMesure = If(rows(0)("IndiceMesure") Is DBNull.Value, String.Empty, rows(0)("IndiceMesure").ToString()),
            .Pression = If(rows(0)("pression") Is DBNull.Value, 0, CInt(rows(0)("pression"))),
            .Volume15 = If(rows(0)("volume15") Is DBNull.Value, 0, CInt(rows(0)("volume15"))),
            .Volume30 = If(rows(0)("volume30") Is DBNull.Value, 0, CInt(rows(0)("volume30"))),
            .Volume60 = If(rows(0)("volume60") Is DBNull.Value, 0, CSng(rows(0)("volume60")))
        }
        End Function

        Public Sub UpdateEssaiMesuresExpansionPropre(NouvelleMesure As MesuresExpansion)
            For Each row As DataRow In ObjetDataMesuresExpansionPropre.Rows
                If row("refEssai").ToString() = NouvelleMesure.RefEssai And row("IndiceMesure").ToString() = NouvelleMesure.IndiceMesure Then
                    row("pression") = NouvelleMesure.Pression
                    row("volume15") = NouvelleMesure.Volume15
                    row("volume30") = NouvelleMesure.Volume30
                    row("volume60") = NouvelleMesure.Volume60
                    Exit For
                End If
            Next
        End Sub




    End Class
End Namespace