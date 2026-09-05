Imports System.Data
Imports System.IO
Imports Microsoft.AspNetCore.Mvc
Imports GaiaPress.core.V17
Imports GaiaPress
Imports GaiaPress.core.V17.Models ' Si vos classes sont dans un espace de noms comme Models
Imports GaiaPress.Core.V17.Services ' Si vos classes sont dans un espace de noms comme Services
Imports GaiaPress.Core.V17.Data ' Si vos classes sont dans un espace de noms comme Services

Namespace GaiaPress.core.V17.Controllers



    ''' <summary>
    ''' Contrôleur API pour la gestion des chantiers.
    ''' Utilise DataService et ChantierLogic pour manipuler les données.
    ''' </summary>
    <ApiController>
    <Route("api/[controller]")>
    Public Class ChantierControleur

        Inherits ControllerBase

        ' ✅ Déclarer les dépendances comme membres
        Private ReadOnly _context As GaiaPressDbContext
        Private ReadOnly _dataService As DataService

        '_chantierLogic = New GaiaPress.core.V17.Services.ChantierLogic(context)

        ''' <summary>
        ' ✅ Constructeur avec les BONS paramètres
        Public Sub New(context As GaiaPressDbContext, dataService As DataService)
            _context = context
            _dataService = dataService
        End Sub

        '' ======================
        '' CHANTIERS
        '' ======================

        '''' <summary>
        '''' Récupère tous les chantiers.
        '''' </summary>
        '<HttpGet>
        'Public Function GetAllChantiers() As IActionResult
        '    Try
        '        Dim chantiers As New List(Of LesChantiers)
        '        For Each row As DataRow In _dataService.ObjetDataChantier.Rows
        '            Dim chantier As New LesChantiers With {
        '                .C_refChantier = row("refChantier").ToString(),
        '                .C_nomChantier = row("nomChantier").ToString(),
        '                .C_descriptifChantier = row("descriptifChantier").ToString(),
        '                .C_refClient = If(row("refClient") Is DBNull.Value, Nothing, CInt(row("refClient"))),
        '                .C_entreprise = row("entreprise").ToString(),
        '                .C_adresse = row("adresse").ToString(),
        '                .C_telephone = row("telephone").ToString(),
        '                .C_reference = row("reference").ToString(),
        '                .C_FichierChantier = String.Empty,
        '                .C_MyFenetreChantier = New ChantierLogic,
        '                .C_MyFile = String.Empty,
        '                .C_MyFileTitle = String.Empty,
        '                .C_DataSetName = String.Empty,
        '                .C_ListForages = New List(Of Forage)(),
        '                .C_ListEssaisForages = New List(Of Essai_Forage)(),
        '                .C_ListMesuresForage = New List(Of MesuresEssaiForage)(),
        '                .C_ListSols = New List(Of Sol)(),
        '                .C_ListSondes = New List(Of Probe)(),
        '                .C_IsSaved = False,
        '                .C_FilePath = String.Empty,
        '                .C_FileTitle = String.Empty
        '            }
        '            chantiers.Add(chantier)
        '        Next
        '        Return Ok(chantiers)
        '    Catch ex As Exception
        '        Return StatusCode(500, $"Erreur lors de la récupération des chantiers: {ex.Message}")
        '    End Try
        'End Function

        ''' <summary>
        ''' Récupère un chantier par son ID.
        ''' </summary>
        '<HttpGet("{id}")>
        'Public Function GetChantierById(id As String) As IActionResult
        '    Try
        '        Dim chantier As LesChantiers = Nothing
        '        For Each row As DataRow In _dataService.ObjetDataChantier.Rows
        '            If row("refChantier").ToString() = id Then
        '                chantier = New LesChantiers With {
        '                    .C_refChantier = row("refChantier").ToString(),
        '                    .C_nomChantier = row("nomChantier").ToString(),
        '                    .C_descriptifChantier = row("descriptifChantier").ToString(),
        '                    .C_refClient = If(row("refClient") Is DBNull.Value, Nothing, CInt(row("refClient"))),
        '                    .C_entreprise = row("entreprise").ToString(),
        '                    .C_adresse = row("adresse").ToString(),
        '                    .C_telephone = row("telephone").ToString(),
        '                    .C_reference = row("reference").ToString(),
        '                    .C_FichierChantier = String.Empty,
        '                    .C_MyFenetreChantier = New ChantierLogic,
        '                    .C_MyFile = String.Empty,
        '                    .C_MyFileTitle = String.Empty,
        '                    .C_DataSetName = String.Empty,
        '                    .C_ListForages = New List(Of Forage)(),
        '                    .C_ListEssaisForages = New List(Of Essai_Forage)(),
        '                    .C_ListMesuresForage = New List(Of MesuresEssaiForage)(),
        '                    .C_ListSols = New List(Of Sol)(),
        '                    .C_ListSondes = New List(Of Probe)(),
        '                    .C_IsSaved = False,
        '                    .C_FilePath = String.Empty,
        '                    .C_FileTitle = String.Empty
        '                }
        '                Exit For
        '            End If
        '        Next

        '        If chantier Is Nothing Then
        '            Return NotFound($"Le chantier avec l'ID {id} n'existe pas.")
        '        End If
        '        Return Ok(chantier)
        '    Catch ex As Exception
        '        Return StatusCode(500, $"Erreur lors de la récupération du chantier: {ex.Message}")
        '    End Try
        'End Function



        ''' <summary>
        ''' Ajoute un nouveau chantier.
        '''' </summary>
        '<HttpPost>
        'Public Function AddChantier(<FromBody> chantierModel As LesChantiers) As IActionResult
        '    Try
        '        ' Initialiser les listes si elles sont Nothing
        '        If chantierModel.C_ListForages Is Nothing Then
        '            chantierModel.C_ListForages = New List(Of Forage)()
        '        End If
        '        If chantierModel.C_ListEssaisForages Is Nothing Then
        '            chantierModel.C_ListEssaisForages = New List(Of Essai_Forage)()
        '        End If
        '        If chantierModel.C_ListMesuresForage Is Nothing Then
        '            chantierModel.C_ListMesuresForage = New List(Of MesuresEssaiForage)()
        '        End If
        '        If chantierModel.C_ListSols Is Nothing Then
        '            chantierModel.C_ListSols = New List(Of Sol)()
        '        End If
        '        If chantierModel.C_ListSondes Is Nothing Then
        '            chantierModel.C_ListSondes = New List(Of Probe)()
        '        End If

        '        ' Initialiser ChantierLogic si nécessaire
        '        If chantierModel.C_MyFenetreChantier Is Nothing Then
        '            chantierModel.C_MyFenetreChantier = New ChantierLogic
        '        End If

        '        Dim chantier As New LesChantiers With {
        '            .C_refChantier = chantierModel.C_refChantier,
        '            .C_nomChantier = chantierModel.C_nomChantier,
        '            .C_descriptifChantier = chantierModel.C_descriptifChantier,
        '            .C_refClient = chantierModel.C_refClient,
        '            .C_entreprise = chantierModel.C_entreprise,
        '            .C_adresse = chantierModel.C_adresse,
        '            .C_telephone = chantierModel.C_telephone,
        '            .C_reference = chantierModel.C_reference,
        '            .C_FichierChantier = chantierModel.C_FichierChantier,
        '            .C_MyFenetreChantier = chantierModel.C_MyFenetreChantier,
        '            .C_MyFile = chantierModel.C_MyFile,
        '            .C_MyFileTitle = chantierModel.C_MyFileTitle,
        '            .C_DataSetName = chantierModel.C_DataSetName,
        '            .C_ListForages = chantierModel.C_ListForages,
        '            .C_ListEssaisForages = chantierModel.C_ListEssaisForages,
        '            .C_ListMesuresForage = chantierModel.C_ListMesuresForage,
        '            .C_ListSols = chantierModel.C_ListSols,
        '            .C_ListSondes = chantierModel.C_ListSondes,
        '            .C_IsSaved = chantierModel.C_IsSaved,
        '            .C_FilePath = chantierModel.C_FilePath,
        '            .C_FileTitle = chantierModel.C_FileTitle
        '        }
        '        _dataService.AddChantier(chantier)
        '        Return Ok(New With {Key .Message = "Chantier ajouté avec succès !", .RefChantier = chantier.C_refChantier})
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout du chantier : {ex.Message}"})
        '    End Try
        'End Function

        '''' <summary>
        '''' Met à jour un chantier existant.
        '''' </summary>
        '<HttpPut("{id}")>
        'Public Function UpdateChantier(id As String, <FromBody> chantierModel As LesChantiers) As IActionResult
        '    Try
        '        ' Initialiser les listes si elles sont Nothing
        '        If chantierModel.C_ListForages Is Nothing Then
        '            chantierModel.C_ListForages = New List(Of Forage)()
        '        End If
        '        If chantierModel.C_ListEssaisForages Is Nothing Then
        '            chantierModel.C_ListEssaisForages = New List(Of Essai_Forage)()
        '        End If
        '        If chantierModel.C_ListMesuresForage Is Nothing Then
        '            chantierModel.C_ListMesuresForage = New List(Of MesuresEssaiForage)()
        '        End If
        '        If chantierModel.C_ListSols Is Nothing Then
        '            chantierModel.C_ListSols = New List(Of Sol)()
        '        End If
        '        If chantierModel.C_ListSondes Is Nothing Then
        '            chantierModel.C_ListSondes = New List(Of Probe)()
        '        End If

        '        ' Initialiser ChantierLogic si nécessaire
        '        If chantierModel.C_MyFenetreChantier Is Nothing Then
        '            chantierModel.C_MyFenetreChantier = New ChantierLogic
        '        End If

        '        Dim chantier As New LesChantiers With {
        '            .C_refChantier = id,
        '            .C_nomChantier = chantierModel.C_nomChantier,
        '            .C_descriptifChantier = chantierModel.C_descriptifChantier,
        '            .C_refClient = chantierModel.C_refClient,
        '            .C_entreprise = chantierModel.C_entreprise,
        '            .C_adresse = chantierModel.C_adresse,
        '            .C_telephone = chantierModel.C_telephone,
        '            .C_reference = chantierModel.C_reference,
        '            .C_FichierChantier = chantierModel.C_FichierChantier,
        '            .C_MyFenetreChantier = chantierModel.C_MyFenetreChantier,
        '            .C_MyFile = chantierModel.C_MyFile,
        '            .C_MyFileTitle = chantierModel.C_MyFileTitle,
        '            .C_DataSetName = chantierModel.C_DataSetName,
        '            .C_ListForages = chantierModel.C_ListForages,
        '            .C_ListEssaisForages = chantierModel.C_ListEssaisForages,
        '            .C_ListMesuresForage = chantierModel.C_ListMesuresForage,
        '            .C_ListSols = chantierModel.C_ListSols,
        '            .C_ListSondes = chantierModel.C_ListSondes,
        '            .C_IsSaved = chantierModel.C_IsSaved,
        '            .C_FilePath = chantierModel.C_FilePath,
        '            .C_FileTitle = chantierModel.C_FileTitle
        '        }
        '        _dataService.UpdateChantier(chantier)
        '        Return Ok(New With {Key .Message = "Chantier mis à jour avec succès !"})
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour : {ex.Message}"})
        '    End Try
        'End Function

        '''' <summary>
        '''' Supprime un chantier.
        '''' </summary>
        '<HttpDelete("{id}")>
        'Public Function DeleteChantier(id As String) As IActionResult
        '    Try
        '        _dataService.DeleteChantier(id)
        '        Return Ok(New With {Key .Message = "Chantier supprimé avec succès !"})
        '    Catch ex As Exception
        '        Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression : {ex.Message}"})
        '    End Try
        'End Function

        ' ======================
        ' FORAGES
        ' ======================

        ' ======================
        ' ESSAIS
        ' ======================

        ''' <summary>
        ''' Ajoute un essai à un forage.
        ''' </summary>
        <HttpPost("essais")>
        Public Function AddEssai(<FromBody> essaiModel As Essai_Forage) As IActionResult
            Try
                Dim essai As New Essai_Forage With {
                    .E_MySol = New Sol(),
                    .E_RefForage = essaiModel.E_RefForage,
                    .MonForage = New Forage(),
                    .E_RefEssai = essaiModel.E_RefEssai,
                    .E_refSonde = essaiModel.E_refSonde,
                    .E_MyResistance = New Essai_Resistance(),
                    .E_MyExpansion = New Essai_Expansion(),
                    .E_MySonde = New Probe(),
                    .E_refEssaiResistancePropre = essaiModel.E_refEssaiResistancePropre,
                    .E_refEssaiExpansionPropre = essaiModel.E_refEssaiExpansionPropre,
                    .E_refEssaiPerteDeCharge = essaiModel.E_refEssaiPerteDeCharge,
                    .E_RefSol = essaiModel.E_RefSol,
                    .E_Enregistreur = essaiModel.E_Enregistreur,
                    .E_Operateur = essaiModel.E_Operateur,
                    .E_VolSonde = essaiModel.E_VolSonde,
                    .E_ClassPrecis = essaiModel.E_ClassPrecis,
                    .E_PrecisVolSonde = essaiModel.E_PrecisVolSonde,
                    .E_nomEssai = essaiModel.E_nomEssai,
                    .E_dateEssai = essaiModel.E_dateEssai,
                    .E_heureEssai = essaiModel.E_heureEssai,
                    .E_difPresEauAir = essaiModel.E_difPresEauAir,
                    .E_hauteurCpv = essaiModel.E_hauteurCpv,
                    .E_presP0 = essaiModel.E_presP0,
                    .E_presFluage = essaiModel.E_presFluage,
                    .E_presLimite = essaiModel.E_presLimite,
                    .E_presLimiteNF = essaiModel.E_presLimiteNF,
                    .E_presLimiteTheo = essaiModel.E_presLimiteTheo,
                    .E_PrecisVolume = essaiModel.E_PrecisVolume,
                    .E_PrecisPression = essaiModel.E_PrecisPression,
                    .E_poidsVol = essaiModel.E_poidsVol,
                    .E_poidsVolDejauge = essaiModel.E_poidsVolDejauge,
                    .E_coefPoisson = essaiModel.E_coefPoisson,
                    .E_modPressio = essaiModel.E_modPressio,
                    .E_modPressioNf = essaiModel.E_modPressioNf,
                    .E_modElastique = essaiModel.E_modElastique,
                    .E_modElastiqueNf = essaiModel.E_modElastiqueNf,
                    .E_Cohesion = essaiModel.E_Cohesion,
                    .E_Frottement = essaiModel.E_Frottement,
                    .E_FrottementIntergranulaire = essaiModel.E_FrottementIntergranulaire,
                    .E_U0 = essaiModel.E_U0,
                    .E_profNappe = essaiModel.E_profNappe,
                    .E_profSonde = essaiModel.E_profSonde,
                    .E_coteZeroGeomeca = essaiModel.E_coteZeroGeomeca,
                    .E_Pe = essaiModel.E_Pe,
                    .E_Ie = essaiModel.E_Ie,
                    .E_Sonde = New Probe(),
                    .Mesures_Essai_Forages = New List(Of MesuresEssaiForage)()
                }
                _dataService.AddEssaisForage(essai)
                Return Ok(New With {Key .Message = "Essai ajouté avec succès !", .RefEssai = essai.E_RefEssai})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout de l'essai : {ex.Message}"})
            End Try
        End Function

        ' ======================
        ' MESURES
        ' ======================

        ''' <summary>
        ''' Récupère les mesures d'un essai.
        ''' </summary>
        ''' <summary>
        ''' Récupère une mesure spécifique d'un essai depuis la liste C_ListMesuresForage d'un chantier.
        ''' </summary>
        <HttpGet("chantiers/{chantierId}/essais/{essaiId}/mesures/{indiceMesure}")>
        Public Function GetMesureByEssaiAndIndice(chantierId As String, essaiId As String, indiceMesure As Integer) As IActionResult
            Try
                ' Récupérer le chantier depuis DataService
                ''Dim myChantier As LesChantiers = GetChantierById(chantierId)
                'If myChantier Is Nothing Then
                '    Return NotFound($"Chantier {chantierId} introuvable.")
                'End If

                ' Rechercher la mesure dans C_ListMesuresForage
                Dim mesure As MesuresEssaiForage = MyEssai.Mesures_Essai_Forages.FirstOrDefault(
            Function(m) m.RefEssai = essaiId AndAlso m.indiceMesure = indiceMesure
        )

                If mesure Is Nothing Then
                    Return NotFound($"Aucune mesure trouvée pour l'essai {essaiId} et l'indice {indiceMesure}.")
                End If

                ' Retourner les valeurs demandées
                Return Ok(New With {
            .RefEssai = mesure.RefEssai,
            .IndiceMesure = mesure.indiceMesure,
            .Pression = mesure.pression,
            .Volume15 = mesure.volume15,
            .Volume30 = mesure.volume30,
            .Volume60 = mesure.volume60
        })

            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération de la mesure: {ex.Message}")
            End Try
        End Function
        ''' <summary>
        ''' Ajoute une mesure à un essai.
        ''' </summary>
        <HttpPost("mesures")>
        Public Function AddMesure(<FromBody> mesureModel As MesuresEssaiForage) As IActionResult
            Try
                Dim mesure As New MesuresEssaiForage With {
                    .RefEssai = mesureModel.RefEssai,
                    .pression = mesureModel.pression,
                    .volume15 = mesureModel.volume15,
                    .volume30 = mesureModel.volume30,
                    .volume60 = mesureModel.volume60
                }
                _dataService.AddMesureEssaisForage(mesure)
                Return Ok(New With {Key .Message = "Mesure ajoutée avec succès !", .RefMesure = mesure.RefEssai})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout de la mesure : {ex.Message}"})
            End Try
        End Function

        ' ======================
        ' ARBORESCENCE
        ' ======================

        ''' <summary>
        ''' Récupère l'arborescence d'un chantier (pour jsTree).
        ''' </summary>
        <HttpGet("{id}/arborescence")>
        Public Function GetArborescence(id As String) As IActionResult
            Try
                Dim arborescence As New List(Of Object)

                ' Ajouter l'élément racine pour le chantier
                arborescence.Add(New With {
            .id = id,
            .parent = "#",
            .text = "" ' GetChantierName(id)
        })

                '    ' Récupérer les forages associés au chantier
                '    Dim forages As DataRow() = _dataService.ObjetDataForage.Select($"refChantier = '{id}'")

                '    ' Ajouter les forages à l'arborescence
                '    For Each forage As DataRow In forages
                '        arborescence.Add(New With {
                '    .id = forage("refForage").ToString(),
                '    .parent = id,
                '    .text = forage("nomForage").ToString()
                '})

                ' Supposons que MyForage est une instance de Forage
                ' Vous devez d'abord récupérer l'instance de Forage correspondante à ce DataRow
                'Dim myForage As Forage = GetForageByRef(forage("refForage").ToString())

                ' Parcourir les essais de MyForage
                If MyForage IsNot Nothing Then
                    For Each essai As Essai_Forage In MyForage.F_ListEssais
                        arborescence.Add(New With {
                        .id = essai.E_RefEssai, ' Accès à la propriété RefEssai de Essai_Forage
                        .parent = essai.MonForage.F_RefForage.ToString(),
                        .text = $"Essai { essai.E_RefEssai}"
                    })
                    Next
                End If
                'Next

                Return Ok(arborescence)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la génération de l'arborescence : {ex.Message}")
            End Try
        End Function
        '''' <summary>
        '''' Récupère le nom d'un chantier à partir de son ID.
        '''' </summary>
        'Public Function GetChantierName(refChantier As String) As String
        '    Dim rows() As DataRow = _dataService.ObjetDataChantier.Select($"refChantier = '{refChantier}'")
        '    If rows.Length > 0 Then
        '        Return rows(0)("nomChantier").ToString()
        '    Else
        '        Return "Chantier inconnu"
        '    End If
        'End Function

        '' ======================
        '' CALCULS (Pressiométrique)
        '' ======================

        '''' <summary>
        '''' Calcule les résultats pressiométriques pour un chantier.
        '''' </summary>
        '<HttpGet("{id}/pressio")>
        'Public Function GetPressioResults(id As String) As IActionResult
        '    Try
        '        Dim result As Object = CalculerPressio(id)
        '        Return Ok(result)
        '    Catch ex As Exception
        '        Return BadRequest(New With {key .Message = $"Erreur lors du calcul pressiométrique : {ex.Message}"})
        '    End Try
        'End Function

        ' ======================
        ' CAPTURE D'ÉCRAN
        ' ======================

        ''' <summary>
        ''' Recevoir une capture d'écran du client.
        ''' </summary>
        <HttpPost("screenshot")>
        Public Function SaveScreenshot(<FromBody> model As ScreenshotModel) As IActionResult
            Try
                If model Is Nothing OrElse model.ImageData Is Nothing OrElse model.ImageData.Length = 0 Then
                    Return BadRequest("Aucune donnée d'image reçue.")
                End If

                Dim tempPath As String = Path.Combine(Path.GetTempPath(), $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.png")
                System.IO.File.WriteAllBytes(tempPath, model.ImageData)
                Return Ok(New With {Key .Message = "Capture d'écran enregistrée avec succès.", .Path = tempPath})
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de l'enregistrement de la capture d'écran: {ex.Message}")
            End Try
        End Function
    End Class

    ' ======================
    ' MODÈLES (DTOs)
    ' ======================

    Public Class ScreenshotModel
        Public Property ImageData As Byte()
    End Class
End Namespace