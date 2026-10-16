Imports System.Collections.Generic
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Services
Imports Microsoft.AspNetCore.Mvc

Namespace GaiaPressCoreV17.Controllers

    ''' <summary>
    ''' Contrôleur API pour la gestion des essais de forage.
    ''' </summary>
    <ApiController>
    <Route("api/[controller]")>
    Public Class MesuresEssaiForageDbController
        Inherits ControllerBase

        ' Déclaration des dépendances avec injection de dépendances
        Public ReadOnly _essaiForageService As EssaiForageService

        ''' <summary>
        ''' Constructeur avec injection de dépendances.
        ''' </summary>
        Public Sub New(essaiForageService As EssaiForageService)
            _essaiForageService = essaiForageService
        End Sub

        ' ======================
        ' ESSAIS DE FORAGE
        ' ======================

        ''' <summary>
        ''' Récupère tous les essais de forage.
        ''' </summary>
        <HttpGet>
        Public Function GetAllEssaiForages() As IActionResult
            Try
                Dim essais = _essaiForageService.GetAllEssaiForages()
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des essais de forage : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Récupère un essai de forage par sa référence.
        ''' </summary>
        <HttpGet("{refEssai}")>
        Public Function GetEssaiForageByRef(refEssai As String) As IActionResult
            Try
                Dim essai = _essaiForageService.GetEssaiForageByRefEssai(refEssai)
                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai de forage avec la référence {refEssai} introuvable."})
                End If
                Return Ok(essai)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération de l'essai de forage : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Ajoute un nouvel essai de forage.
        ''' </summary>
        ' ✅ Correction
        Public Async Function AddEssaiForage(<FromBody> essaiModel As Essai_Forage) As Task(Of IActionResult)
            Try
                If essaiModel Is Nothing Then
                    Return BadRequest(New With {Key .Message = "Le modèle d'essai de forage est vide."})
                End If

                Await _essaiForageService.AddEssaiForage(essaiModel)  ' ✅ Await ajouté
                Return CreatedAtAction(NameOf(GetEssaiForageByRef), New With {Key .refEssai = essaiModel.refEssai}, essaiModel)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de l'ajout de l'essai de forage : {ex.Message}"})
            End Try
        End Function
        ''' <summary>
        ''' Met à jour un essai de forage existant.
        ''' </summary>
        <HttpPut("{refEssai}")>
        Public Function UpdateEssaiForage(refEssai As String, <FromBody> essaiModel As Essai_Forage) As IActionResult
            Try
                If essaiModel Is Nothing OrElse refEssai <> essaiModel.refEssai Then
                    Return BadRequest(New With {Key .Message = "Données d'essai de forage invalides."})
                End If

                Dim existingEssai = _essaiForageService.GetEssaiForageByRefEssai(refEssai)
                If existingEssai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai de forage avec la référence {refEssai} introuvable."})
                End If

                _essaiForageService.UpDateEssaiForage(essaiModel)
                Return Ok(New With {Key .Message = "Essai de forage mis à jour avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la mise à jour de l'essai de forage : {ex.Message}"})
            End Try
        End Function

        ''' <summary>
        ''' Supprime un essai de forage par sa référence.
        ''' </summary>
        <HttpDelete("{refEssai}")>
        Public Async Function DeleteEssaiForage(refEssai As String) As Task(Of IActionResult)  ' ✅ Async + Task(Of IActionResult)
            Try
                Dim essai = _essaiForageService.GetEssaiForageByRefEssai(refEssai)  ' ✅ Await ajouté ici aussi
                If essai Is Nothing Then
                    Return NotFound(New With {Key .Message = $"Essai de forage avec la référence {refEssai} introuvable."})
                End If

                Await _essaiForageService.DeleteEssaiForage(refEssai)  ' ✅ Await ajouté ici (ligne 108 corrigée)
                Return Ok(New With {Key .Message = "Essai de forage supprimé avec succès."})
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la suppression de l'essai de forage : {ex.Message}"})
            End Try
        End Function

        ' =====================================
        ' ESSAIS DE FORAGE PAR FORAGE
        ' ====================================

        ''' <summary>
        ''' Récupère les essais de forage pour un forage spécifique.
        ''' </summary>
        <HttpGet("byForage/{refForage}")>
        Public Function GetEssaiForagesByForage(refForage As String) As IActionResult
            Try
                Dim essais = _essaiForageService.GetEssaiForageByRefForage(refForage)
                Return Ok(essais)
            Catch ex As Exception
                Return BadRequest(New With {Key .Message = $"Erreur lors de la récupération des essais pour le forage {refForage} : {ex.Message}"})
            End Try
        End Function
    End Class
End Namespace