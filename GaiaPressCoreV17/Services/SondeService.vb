Imports System.Data
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports GaiaPressCoreV17
Imports System.Threading


Namespace GaiaPressCoreV17.Services

    Public Class SondeService

        ' ✅ Déclarer EventsSuspendus comme variable de classe
        Public Shared EventsSuspendus As Boolean = False

        '**************************************************************************
        ' Gestion des Sondes
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables
        Public Sub AddSonde(NouvelleSonde As Probe)
            Dim row As DataRow = ObjetDataListeSonde.NewRow()
            row("refSonde") = NouvelleSonde.refSonde
            row("typeSonde") = NouvelleSonde.typeSonde
            row("typeDeformee") = NouvelleSonde.typeDeformee
            row("typeContrainte") = NouvelleSonde.typeContrainte
            row("ls") = NouvelleSonde.ls
            row("lg") = NouvelleSonde.lg
            row("lt") = NouvelleSonde.lt
            row("longInjectPompage") = NouvelleSonde.longInjectePompage
            row("Vs") = NouvelleSonde.Vs
            row("ds") = NouvelleSonde.ds
            row("ClassPrecis") = NouvelleSonde.ClassPrecis
            row("epMembraneGarde") = NouvelleSonde.EpMembraneGarde
            row("PrEpMembraneGarde") = NouvelleSonde.PrEpMembraneGarde
            row("epMembraneMesure") = NouvelleSonde.EpMembraneMesure
            row("PrEpMembraneMesure") = NouvelleSonde.PrEpMembraneMesure
            row("epTubeFendu") = NouvelleSonde.epTubeFendu
            row("PrEpTubeFendu") = NouvelleSonde.PrEpTubeFendu
            row("epaisInitMembrane") = NouvelleSonde.EpInitMembrane
            row("PrEpaisInitMembrane") = NouvelleSonde.PrEpInitMembrane
            row("PrLong") = NouvelleSonde.PrLong
            row("PrDiam") = NouvelleSonde.PrDiam
            row("PrP") = NouvelleSonde.PrP
            row("PrV") = NouvelleSonde.PrV
            row("PrPV") = NouvelleSonde.PrPV
            row("PrZ") = NouvelleSonde.PrZ
            row("PrT") = NouvelleSonde.Prt
            row("Pm") = NouvelleSonde.Pm
            ObjetDataListeSonde.Rows.Add(row)
        End Sub

        'Public Sub UpdateSonde(NouvelleSonde As Probe)
        '    For Each row As DataRow In ObjetDataListeSonde.Rows
        '        If row("refSonde").ToString() = NouvelleSonde.refSonde Then
        '            row("refSonde") = NouvelleSonde.refSonde
        '            row("typeSonde") = NouvelleSonde.typeSonde
        '            row("typeDeformee") = NouvelleSonde.typeDeformee
        '            row("typeContrainte") = NouvelleSonde.typeContrainte
        '            row("ls") = NouvelleSonde.ls
        '            row("lg") = NouvelleSonde.lg
        '            row("lt") = NouvelleSonde.lt
        '            row("longInjectPompage") = NouvelleSonde.longInjectePompage
        '            row("Vs") = NouvelleSonde.Vs
        '            row("ds") = NouvelleSonde.ds
        '            row("ClassPrecis") = NouvelleSonde.ClassPrecis
        '            row("epMembraneGarde") = NouvelleSonde.EpMembraneGarde
        '            row("PrEpMembraneGarde") = NouvelleSonde.PrEpMembraneGarde
        '            row("epMembraneMesure") = NouvelleSonde.EpMembraneMesure
        '            row("PrEpMembraneMesure") = NouvelleSonde.PrEpMembraneMesure
        '            row("epTubeFendu") = NouvelleSonde.epTubeFendu
        '            row("PrEpTubeFendu") = NouvelleSonde.PrEpTubeFendu
        '            row("epaisInitMembrane") = NouvelleSonde.EpInitMembrane
        '            row("PrEpaisInitMembrane") = NouvelleSonde.PrEpInitMembrane
        '            row("PrLong") = NouvelleSonde.PrLong
        '            row("PrDiam") = NouvelleSonde.PrDiam
        '            row("PrP") = NouvelleSonde.PrP
        '            row("PrV") = NouvelleSonde.PrV
        '            row("PrPV") = NouvelleSonde.PrPV
        '            row("PrZ") = NouvelleSonde.PrZ
        '            row("PrT") = NouvelleSonde.Prt
        '            row("Pm") = NouvelleSonde.Pm
        '            Exit For
        '        End If
        '    Next
        'End Sub

        'Public Sub DeleteSonde(refSonde As String)
        '    Dim rows() As DataRow = ObjetDataListeSonde.Select("refSonde = '" & refSonde & "'")
        '    If rows.Length > 0 Then
        '        ObjetDataListeSonde.Rows.Remove(rows(0))
        '    End If
        'End Sub
        ''' <summary>
        ''' Récupère une sonde par sa référence depuis ObjetDataListeSonde.
        ''' </summary>
        Public Function GetSondeByRef(refSonde As String) As Probe
            ' Filtrer les lignes correspondantes dans ObjetDataListeSonde
            Dim rows() As DataRow = ObjetDataListeSonde.Select($"refSonde = '{refSonde}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Probe avec les valeurs de la DataRow
            Return New Probe With {
            .RefSonde = If(rows(0)("refSonde") Is DBNull.Value, String.Empty, rows(0)("refSonde").ToString()),
            .TypeSonde = If(rows(0)("typeSonde") Is DBNull.Value, 0, CInt(rows(0)("typeSonde"))),
            .TypeDeformee = If(rows(0)("typeDeformee") Is DBNull.Value, 0, CInt(rows(0)("typeDeformee"))),
            .TypeContrainte = If(rows(0)("typeContrainte") Is DBNull.Value, 0, CInt(rows(0)("typeContrainte"))),
            .Ls = If(rows(0)("ls") Is DBNull.Value, 0, CSng(rows(0)("ls"))),
            .Lg = If(rows(0)("lg") Is DBNull.Value, 0, CSng(rows(0)("lg"))),
            .lt = If(rows(0)("lt") Is DBNull.Value, 0, CSng(rows(0)("lt"))),
            .LongInjectePompage = If(rows(0)("longInjectPompage") Is DBNull.Value, 0, CSng(rows(0)("longInjectPompage"))),
            .Vs = If(rows(0)("Vs") Is DBNull.Value, 0, CSng(rows(0)("Vs"))),
            .Ds = If(rows(0)("ds") Is DBNull.Value, 0, CSng(rows(0)("ds"))),
            .ClassPrecis = If(rows(0)("ClassPrecis") Is DBNull.Value, 0, CInt(rows(0)("ClassPrecis"))),
            .EpMembraneGarde = If(rows(0)("epMembraneGarde") Is DBNull.Value, 0, CSng(rows(0)("epMembraneGarde"))),
            .PrEpMembraneGarde = If(rows(0)("PrEpMembraneGarde") Is DBNull.Value, 0, CSng(rows(0)("PrEpMembraneGarde"))),
            .EpMembraneMesure = If(rows(0)("epMembraneMesure") Is DBNull.Value, 0, CSng(rows(0)("epMembraneMesure"))),
            .PrEpMembraneMesure = If(rows(0)("PrEpMembraneMesure") Is DBNull.Value, 0, CSng(rows(0)("PrEpMembraneMesure"))),
            .EpTubeFendu = If(rows(0)("epTubeFendu") Is DBNull.Value, 0, CSng(rows(0)("epTubeFendu"))),
            .PrEpTubeFendu = If(rows(0)("PrEpTubeFendu") Is DBNull.Value, 0, CSng(rows(0)("PrEpTubeFendu"))),
            .EpInitMembrane = If(rows(0)("epaisInitMembrane") Is DBNull.Value, 0, CSng(rows(0)("epaisInitMembrane"))),
            .PrEpInitMembrane = If(rows(0)("PrEpaisInitMembrane") Is DBNull.Value, 0, CSng(rows(0)("PrEpaisInitMembrane"))),
            .PrLong = If(rows(0)("PrLong") Is DBNull.Value, 0, CSng(rows(0)("PrLong"))),
            .PrDiam = If(rows(0)("PrDiam") Is DBNull.Value, 0, CSng(rows(0)("PrDiam"))),
            .PrP = If(rows(0)("PrP") Is DBNull.Value, 0, CSng(rows(0)("PrP"))),
            .PrV = If(rows(0)("PrV") Is DBNull.Value, 0, CSng(rows(0)("PrV"))),
            .PrPV = If(rows(0)("PrPV") Is DBNull.Value, 0, CSng(rows(0)("PrPV"))),
            .PrZ = If(rows(0)("PrZ") Is DBNull.Value, 0, CSng(rows(0)("PrZ"))),
            .Prt = If(rows(0)("PrT") Is DBNull.Value, 0, CSng(rows(0)("PrT"))),
            .Pm = If(rows(0)("Pm") Is DBNull.Value, 0, CSng(rows(0)("Pm")))
        }
        End Function

        '**************************************************************************
        ' Gestion des EssaisExpansion
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables de  ObjetDataEssaisExpansionPropre
        Public Sub AddEssaiExpansion(NouvelEssai As EssaiExpansion)
            Dim row As DataRow = ObjetDataLesEssaisExpansionPropre.NewRow()
            row("refEssai") = NouvelEssai.RefEssai
            row("refSonde") = NouvelEssai.RefSonde
            row("ClassPrecis") = NouvelEssai.ClassPrecis
            row("nomEssai") = NouvelEssai.nomEssai
            row("dateEssai") = NouvelEssai.dateEssai
            row("difPresEauAir") = NouvelEssai.DifPresEauAir
            row("hauteurCpv") = NouvelEssai.HauteurCpv
            row("Di") = NouvelEssai.Di
            row("Vc") = NouvelEssai.Vc
            row("Vs") = NouvelEssai.Vs
            row("EpMembrane") = NouvelEssai.EpMembrane
            row("Prp") = NouvelEssai.Prp
            row("PrV") = NouvelEssai.PrV
            row("coefficientA") = NouvelEssai.coefficientA
            row("coefficientB") = NouvelEssai.coefficientB
            row("PrecisVc") = NouvelEssai.PrecisVc
            row("PrecisVs") = NouvelEssai.PrecisVs
            row("PrEpMembrane") = NouvelEssai.PrEpMembrane
            row("PrecisA") = NouvelEssai.PrecisA
            row("PrecisB") = NouvelEssai.PrecisB
            ObjetDataLesEssaisExpansionPropre.Rows.Add(row)
        End Sub

        '' Récupère une sonde par sa référence depuis ObjetDataListeSonde.
        ''' </summary>
        Public Function GetEssaiExpansionByRef(refEssai As String) As EssaiExpansion

            ' Filtrer les lignes correspondantes dans ObjetDataListeSonde
            Dim rows() As DataRow = ObjetDataLesEssaisExpansionPropre.Select($"refEssai = '{refEssai}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Probe avec les valeurs de la DataRow
            Return New EssaiExpansion With {
            .RefSonde = If(rows(0)("refSonde") Is DBNull.Value, String.Empty, rows(0)("refSonde").ToString()),
            .ClassPrecis = If(rows(0)("ClassPrecis") Is DBNull.Value, 0, CInt(rows(0)("ClassPrecis"))),
            .nomEssai = If(rows(0)("nomEssai") Is DBNull.Value, 0, CInt(rows(0)("nomEssai"))),
            .dateEssai = If(rows(0).IsNull("dateEssai"), Date.MinValue, Convert.ToDateTime(rows(0)("dateEssai"))),
            .difPresEauAir = If(rows(0)("difPresEauAir") Is DBNull.Value, 0, CSng(rows(0)("difPresEauAir"))),
            .hauteurCpv = If(rows(0)("hauteurCpv") Is DBNull.Value, 0, CSng(rows(0)("hauteurCpv"))),
            .Di = If(rows(0)("Di") Is DBNull.Value, 0, CSng(rows(0)("Di"))),
            .Vc = If(rows(0)("Vc") Is DBNull.Value, 0, CSng(rows(0)("Vc"))),
            .Vs = If(rows(0)("Vs") Is DBNull.Value, 0, CSng(rows(0)("Vs"))),
            .EpMembrane = If(rows(0)("EpMembrane") Is DBNull.Value, 0, CSng(rows(0)("EpMembrane"))),
            .Prp = If(rows(0)("Prp") Is DBNull.Value, 0, CInt(rows(0)("Prp"))),
            .PrV = If(rows(0)("PrV") Is DBNull.Value, 0, CSng(rows(0)("PrV"))),
            .coefficientA = If(rows(0)("coefficientA") Is DBNull.Value, 0, CSng(rows(0)("coefficientA"))),
            .coefficientB = If(rows(0)("coefficientB") Is DBNull.Value, 0, CSng(rows(0)("coefficientB"))),
            .PrecisVc = If(rows(0)("PrecisVc") Is DBNull.Value, 0, CSng(rows(0)("PrecisVc"))),
            .PrecisVs = If(rows(0)("PrecisVs") Is DBNull.Value, 0, CSng(rows(0)("PrecisVs"))),
            .PrEpMembrane = If(rows(0)("PrEpMembrane") Is DBNull.Value, 0, CSng(rows(0)("PrEpMembrane"))),
            .PrecisA = If(rows(0)("PrecisA") Is DBNull.Value, 0, CSng(rows(0)("PrecisA"))),
            .PrecisB = If(rows(0)("PrecisB") Is DBNull.Value, 0, CSng(rows(0)("PrecisB")))
        }
        End Function

        Public Sub UpdateEssaiExpansion(NouvelEssai As EssaiExpansion)
            For Each row As DataRow In ObjetDataListeSonde.Rows
                If row("refSonde").ToString() = NouvelEssai.RefSonde Then
                    row("refEssai") = NouvelEssai.RefEssai
                    row("refSonde") = NouvelEssai.RefSonde
                    row("ClassPrecis") = NouvelEssai.ClassPrecis
                    row("nomEssai") = NouvelEssai.nomEssai
                    row("dateEssai") = NouvelEssai.dateEssai
                    row("difPresEauAir") = NouvelEssai.DifPresEauAir
                    row("hauteurCpv") = NouvelEssai.HauteurCpv
                    row("Di") = NouvelEssai.Di
                    row("Vc") = NouvelEssai.Vc
                    row("Vs") = NouvelEssai.Vs
                    row("EpMembrane") = NouvelEssai.EpMembrane
                    row("Prp") = NouvelEssai.Prp
                    row("PrV") = NouvelEssai.PrV
                    row("coefficientA") = NouvelEssai.coefficientA
                    row("coefficientB") = NouvelEssai.coefficientB
                    row("PrecisVc") = NouvelEssai.PrecisVc
                    row("PrecisVs") = NouvelEssai.PrecisVs
                    row("PrEpMembrane") = NouvelEssai.PrEpMembrane
                    row("PrecisA") = NouvelEssai.PrecisA
                    row("PrecisB") = NouvelEssai.PrecisB
                    Exit For
                End If
            Next
        End Sub


        '**************************************************************************
        ' Gestion des EssaisResistanceDb
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables de  ObjetDataEssaisResistanceDbPropre
        Public Sub AddEssaiResistance(NouvelEssai As EssaiResistance)
            Dim row As DataRow = ObjetDataLesEssaisResistanceDbPropre.NewRow()
            row("refEssai") = NouvelEssai.refEssai
            row("refSonde") = NouvelEssai.refSonde
            row("ClassPrecis") = NouvelEssai.ClassPrecis
            row("nomEssai") = NouvelEssai.nomEssai
            row("dateEssai") = NouvelEssai.dateEssai
            row("difPresEauAir") = NouvelEssai.difPresEauAir
            row("hauteurCpv") = NouvelEssai.hauteurCpv
            row("A0") = NouvelEssai.A0
            row("A1") = NouvelEssai.A1
            row("A2") = NouvelEssai.A2
            row("A3") = NouvelEssai.A3
            row("Pel") = NouvelEssai.Pel
            row("Pe") = NouvelEssai.Pe
            row("MySondeResistance") = NouvelEssai.MySondeResistance
            'row("Mesures") = NouvelEssai. Mesure
            ObjetDataLesEssaisResistanceDbPropre.Rows.Add(row)
        End Sub

        ''' <summary>
        ''' Récupère un essai de résistance par sa référence depuis ObjetDataEssaiResistancePropre.
        ' Exemple de méthode modifiée pour accepter une Probe
        Public Function GetEssaiResistanceByRef(refEssai As String, probe As Probe) As EssaiResistance
            Try

                ' ✅ Accéder à ObjetDataEssaiResistancePropre via l'instance de Probe
                Dim tableEssais As DataTable = ObjetDataLesEssaisResistanceDbPropre

                Dim rows() As DataRow = tableEssais.Select("refEssai = '" & refEssai.Replace("'", "''") & "'")

                If rows.Length = 0 Then
                    Return Nothing
                End If

                Dim row As DataRow = rows(0)

                ' ✅ Accéder à EventsSuspendus via la DataRow
                Dim eventsSuspendusValue As Boolean = If(rows(0).IsNull("EventsSuspendus"), False, rows(0).Field(Of Boolean)("EventsSuspendus"))

                If rows.Length = 0 Then
                    Return Nothing
                End If

                ' Créer et retourner un nouvel objet EssaiResistance
                Return New EssaiResistance With {
            .RefEssai = If(rows(0).IsNull("refEssai"), String.Empty, rows(0).Field(Of String)("refEssai")),
            .RefSonde = If(rows(0).IsNull("refSonde"), String.Empty, rows(0).Field(Of String)("refSonde")),
            .EventsSuspendus = If(rows(0).IsNull("EventsSuspendus"), False, rows(0).Field(Of Boolean)("EventsSuspendus"))
        }
            Catch ex As Exception
                Console.WriteLine($"Erreur dans GetEssaiResistanceByRef: {ex.Message}")
                Return Nothing
            End Try
        End Function



        Public Sub UpdateEssaiResistance(NouvelEssai As EssaiResistance)
            For Each row As DataRow In ObjetDataLesEssaisResistanceDbPropre.Rows
                If row("refEssai").ToString() = NouvelEssai.RefEssai Then
                    row("refEssai") = NouvelEssai.RefEssai
                    row("refSonde") = NouvelEssai.RefSonde
                    row("ClassPrecis") = NouvelEssai.ClassPrecis
                    row("nomEssai") = NouvelEssai.nomEssai
                    row("dateEssai") = NouvelEssai.dateEssai
                    row("difPresEauAir") = NouvelEssai.difPresEauAir
                    row("hauteurCpv") = NouvelEssai.hauteurCpv
                    row("A0") = NouvelEssai.A0
                    row("A1") = NouvelEssai.A1
                    row("A2") = NouvelEssai.A2
                    row("A3") = NouvelEssai.A3
                    row("Pel") = NouvelEssai.Pel
                    row("Pe") = NouvelEssai.Pe
                    row("MySondeResistance") = NouvelEssai.MySondeResistance
                    Exit For
                End If
            Next
        End Sub

        ' *******************************************************************************************************
        ' Charge les Mesures depuis MaListeSonde.Tables("LesMesuresResistanceDbPropre") vers MyResistance.Mesures
        ' *******************************************************************************************************

        '**************************************************************************
        ' Gestion des EssaisMesuresResistanceDbPropre
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables de  ObjetDataMesuresResistanceDbPropre
        Public Sub AddEssaiMesuresResistanceDbPropre(NouvelleMesure As MesureExpansion)
            Dim row As DataRow = ObjetDataLesMesuresResistanceDbPropre.NewRow()
            row("refEssai") = NouvelleMesure.RefEssai
            row("IndiceMesure") = NouvelleMesure.IndiceMesure
            row("pression") = NouvelleMesure.Pression
            row("volume") = NouvelleMesure.Volume
            ObjetDataLesMesuresResistanceDbPropre.Rows.Add(row)
        End Sub

        '' Récupère une sonde par sa référence depuis ObjetDataListeSonde.
        ''' </summary>
        Public Function GetEssaiMesuresResistanceDbPropreByRef(refEssai As String, IndiceMesure As Integer) As MesureExpansion
            ' Filtrer les lignes correspondantes dans ObjetDataListeSonde
            Dim rows() As DataRow = ObjetDataLesMesuresResistanceDbPropre.Select($"refEssai = '{refEssai}'" And $"IndiceMesure = '{IndiceMesure}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Probe avec les valeurs de la DataRow
            Return New MesureExpansion With {
            .RefEssai = If(rows(0)("refEssai") Is DBNull.Value, String.Empty, rows(0)("refEssai").ToString()),
            .IndiceMesure = If(rows(0)("IndiceMesure") Is DBNull.Value, String.Empty, rows(0)("IndiceMesure").ToString()),
            .Pression = If(rows(0)("pression") Is DBNull.Value, 0, CInt(rows(0)("pression"))),
            .Volume = If(rows(0)("volume") Is DBNull.Value, 0, CInt(rows(0)("volume")))
        }
        End Function

        Public Sub UpdateEssaiMesuresResistanceDbPropre(NouvelleMesure As MesureExpansion)
            For Each row As DataRow In ObjetDataLesMesuresResistanceDbPropre.Rows
                If row("refEssai").ToString() = NouvelleMesure.RefEssai And row("IndiceMesure").ToString() = NouvelleMesure.IndiceMesure Then
                    row("pression") = NouvelleMesure.Pression
                    row("volume") = NouvelleMesure.Volume
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
        ' Charge les Mesures depuis MaListeSonde.Tables("LesMesureExpansionPropre") vers MyExpansion.Mesures
        ' *******************************************************************************************************

        '**************************************************************************
        ' Gestion des EssaisMesureExpansionPropre
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables de  ObjetDataLesMesuresExpansionPropre
        Public Sub AddEssaiMesureExpansionPropre(NouvelleMesure As MesureExpansion)
            Dim row As DataRow = ObjetDataLesMesuresExpansionPropre.NewRow()
            row("refEssai") = NouvelleMesure.RefEssai
            row("IndiceMesure") = NouvelleMesure.IndiceMesure
            row("pression") = NouvelleMesure.Pression
            row("volume") = NouvelleMesure.Volume
            ObjetDataLesMesuresExpansionPropre.Rows.Add(row)
        End Sub

        '' Récupère une sonde par sa référence depuis ObjetDataListeSonde.
        ''' </summary>
        Public Function GetEssaiMesureExpansionPropreByRef(refEssai As String, IndiceMesure As Integer) As MesureExpansion
            ' Filtrer les lignes correspondantes dans ObjetDataListeSonde
            Dim rows() As DataRow = ObjetDataMesures.Select($"refEssai = '{refEssai}'" And $"IndiceMesure = '{IndiceMesure}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Probe avec les valeurs de la DataRow
            Return New MesureExpansion With {
            .RefEssai = If(rows(0)("refEssai") Is DBNull.Value, String.Empty, rows(0)("refEssai").ToString()),
            .IndiceMesure = If(rows(0)("IndiceMesure") Is DBNull.Value, String.Empty, rows(0)("IndiceMesure").ToString()),
            .Pression = If(rows(0)("Pression") Is DBNull.Value, 0, CInt(rows(0)("Pression"))),
            .Volume = If(rows(0)("Volume") Is DBNull.Value, 0, CInt(rows(0)("Volume")))
            }
        End Function

        ' ✅ Ajoutez ces méthodes à SondeService.vb
        'Public Function UpdateSonde(refSonde As String, probe As Probe) As IActionResult
        '            Try
        '                ' Logique pour mettre à jour la sonde dans la DataTable
        '                ' (Utilisez votre logique existante ici)
        '                For Each row As DataRow In ObjetDataListeSonde.Rows
        '                    If row("refSonde").ToString() = refSonde Then
        '                        ' Mettez à jour les champs de la sonde
        '                        row("typeSonde") = probe.TypeSonde
        '                        ' Ajoutez ici les autres champs à mettre à jour
        '                        Return Ok(New With {Key .Message = "Sonde mise à jour avec succès."})
        '                    End If
        '                Next
        '                Return NotFound(New With {Key .Message = $"Sonde {refSonde} introuvable."})
        '            Catch ex As Exception
        '                Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour : {ex.Message}"})
        '            End Try
        '        End Function

        'Public Function DeleteSonde(refSonde As String) As IActionResult
        '    Try
        '        ' Logique pour supprimer la sonde dans la DataTable
        '        Dim rows() As DataRow = ObjetDataListeSonde.Select("refSonde = '" & refSonde & "'")
        '        If rows.Length > 0 Then
        '            ObjetDataListeSonde.Rows.Remove(rows(0))
        '            Return Ok(New With {Key .Message = $"Sonde {refSonde} supprimée avec succès."})
        '        End If
        '        Return NotFound(New With {Key .Message = $"Sonde {refSonde} introuvable."})
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression : {ex.Message}"})
        '    End Try
        'End Function      .Volume = If(rows(0)("volume") Is DBNull.Value, 0, CInt(rows(0)("volume")))
        '}


        Public Sub UpdateEssaiMesureExpansionPropre(NouvelleMesure As MesureExpansion)
            For Each row As DataRow In ObjetDataLesMesuresExpansionPropre.Rows
                If row("refEssai").ToString() = NouvelleMesure.RefEssai And row("IndiceMesure").ToString() = NouvelleMesure.IndiceMesure Then
                    row("pression") = NouvelleMesure.Pression
                    row("volume") = NouvelleMesure.Volume
                    Exit For
                End If
            Next
        End Sub

        ' ✅ Ajoutez ces méthodes à SondeService.vb (avant End Class)
        Public Sub UpdateSonde(NouvelleSonde As Probe)
            For Each row As DataRow In ObjetDataListeSonde.Rows
                If row("refSonde").ToString() = NouvelleSonde.RefSonde Then
                    row("refSonde") = NouvelleSonde.RefSonde
                    row("typeSonde") = NouvelleSonde.TypeSonde
                    row("typeDeformee") = NouvelleSonde.TypeDeformee
                    row("typeContrainte") = NouvelleSonde.TypeContrainte
                    row("ls") = NouvelleSonde.Ls
                    row("lg") = NouvelleSonde.Lg
                    row("lt") = NouvelleSonde.lt
                    row("longInjectPompage") = NouvelleSonde.LongInjectePompage
                    row("Vs") = NouvelleSonde.Vs
                    row("ds") = NouvelleSonde.Ds
                    row("ClassPrecis") = NouvelleSonde.ClassPrecis
                    row("epMembraneGarde") = NouvelleSonde.EpMembraneGarde
                    row("PrEpMembraneGarde") = NouvelleSonde.PrEpMembraneGarde
                    row("epMembraneMesure") = NouvelleSonde.EpMembraneMesure
                    row("PrEpMembraneMesure") = NouvelleSonde.PrEpMembraneMesure
                    row("epTubeFendu") = NouvelleSonde.EpTubeFendu
                    row("PrEpTubeFendu") = NouvelleSonde.PrEpTubeFendu
                    row("epaisInitMembrane") = NouvelleSonde.EpInitMembrane
                    row("PrEpaisInitMembrane") = NouvelleSonde.PrEpInitMembrane
                    row("PrLong") = NouvelleSonde.PrLong
                    row("PrDiam") = NouvelleSonde.PrDiam
                    row("PrP") = NouvelleSonde.PrP
                    row("PrV") = NouvelleSonde.PrV
                    row("PrPV") = NouvelleSonde.PrPV
                    row("PrZ") = NouvelleSonde.PrZ
                    row("PrT") = NouvelleSonde.Prt
                    row("Pm") = NouvelleSonde.Pm
                    Exit For

                End If
            Next
        End Sub

        Public Sub DeleteSonde(refSonde As String)
            Dim rows() As DataRow = ObjetDataListeSonde.Select("refSonde = '" & refSonde & "'")
            If rows.Length > 0 Then
                ObjetDataListeSonde.Rows.Remove(rows(0))
            End If
        End Sub

    End Class
End Namespace