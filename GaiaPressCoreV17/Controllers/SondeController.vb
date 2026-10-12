Imports GaiaPressCoreV17.Data
Imports GaiaPressCoreV17.Models
Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Models.Dto
Imports Microsoft.AspNetCore.Mvc
Imports Microsoft.EntityFrameworkCore
Imports System.Threading.Tasks
Imports System.Linq  ' Pour les méthodes LINQ

Namespace GaiaPressCoreV17.Controllers

    <ApiController>
    <Route("api/[controller]")>
    Public Class SondeController
        Inherits ControllerBase

        ' ✅ Déclarer les dépendances
        Public ReadOnly _context As GaiaPressDbContext
        PUBLIC ReadOnly _ChantierService As ChantierService


        ''' <summary>
        ''' Constructeur avec injection de dépendances.
        ''' </summary>
        Public Sub New(context As GaiaPressDbContext, ChantierService As ChantierService)
            _context = context
            _ChantierService = ChantierService
        End Sub

        ' ======================
        ' GET: api/Sonde
        ' ======================
        ''' <summary>
        ''' Récupère toutes les sondes.
        <HttpGet>
        Public Async Function GetAllSondes() As Task(Of IActionResult)
            Try
                ' ✅ Utilise _context.ListeSondeDb (maintenant défini)
                Dim sondes = Await _context.ListeSondeDb.ToListAsync()
                Return Ok(sondes)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération des sondes: {ex.Message}")
            End Try
        End Function

        ' ===================================
        ' GET: api/Sonde/{id}
        ' ===================================
        <HttpGet("{id}")>
        Public Async Function GetSondeById(id As String) As Task(Of IActionResult)
            Try
                ' ✅ Utilise _context.ListeSondeDb avec FirstOrDefaultAsync
                Dim sonde = Await _context.ListeSondeDb _
            .FirstOrDefaultAsync(Function(s) s.RefSonde = id)

                If sonde Is Nothing Then
                    Return NotFound($"La sonde avec l'ID {id} n'existe pas.")
                End If

                Return Ok(sonde)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération de la sonde: {ex.Message}")
            End Try
        End Function

        ' ======================
        ' POST: api/Sonde
        ' ======================
        ''' <summary>
        ''' Ajoute une nouvelle sonde.
        <HttpPost>
        Public Async Function CreateSonde(sonde As Probe) As Task(Of IActionResult)
            Try
                If sonde Is Nothing Then
                    Return BadRequest("La sonde ne peut pas être null.")
                End If

                ' ✅ Vérifie si la sonde existe déjà
                If Await _context.ListeSondeDb.AnyAsync(Function(s) s.RefSonde = sonde.RefSonde) Then
                    Return Conflict($"Une sonde avec l'ID {sonde.RefSonde} existe déjà.")
                End If

                _context.ListeSondeDb.Add(sonde)
                Await _context.SaveChangesAsync()
                Return CreatedAtAction(NameOf(GetSondeById), New With {.id = sonde.RefSonde}, sonde)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la création de la sonde: {ex.Message}")
            End Try
        End Function

        ' ======================
        ' PUT: api/Sonde/{id}
        ' ======================
        ''' <summary>
        <HttpPut("{id}")>
        Public Async Function UpdateSonde(id As String, sonde As Probe) As Task(Of IActionResult)
            Try
                If id <> sonde.RefSonde Then
                    Return BadRequest("L'ID de la sonde ne correspond pas à l'ID dans l'URL.")
                End If

                Dim existingSonde = Await _context.ListeSondeDb.FirstOrDefaultAsync(Function(s) s.RefSonde = id)
                If existingSonde Is Nothing Then
                    Return NotFound($"La sonde avec l'ID {id} n'existe pas.")
                End If

                _context.Entry(existingSonde).CurrentValues.SetValues(sonde)
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la mise à jour de la sonde: {ex.Message}")
            End Try
        End Function

        ' ======================
        ' DELETE: api/Sonde/{id}
        ' ======================
        ''' <summary>
        ''' Supprime une sonde par son ID.
        <HttpDelete("{id}")>
        Public Async Function DeleteSonde(id As String) As Task(Of IActionResult)
            Try
                Dim sonde = Await _context.ListeSondeDb.FirstOrDefaultAsync(Function(s) s.RefSonde = id)
                If sonde Is Nothing Then
                    Return NotFound($"La sonde avec l'ID {id} n'existe pas.")
                End If

                _context.ListeSondeDb.Remove(sonde)
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la suppression de la sonde: {ex.Message}")
            End Try
        End Function

        ' ================================================
        ' GET: api/Sonde/ByChantier/{chantierId}
        ' ================================================
        ''' <summary>
        ''' Récupère toutes les sondes associées à un chantier.
        ''' </summary>
        <HttpGet("ByChantier/{chantierId}")>
        Public Async Function GetSondesByChantier(chantierId As String) As Task(Of IActionResult)
            Try
                'Dim chantier = Await _context.LesChantierss
                '.Include(Function(c) c. ListSondes)
                '.FirstOrDefaultAsync(Function(c) c. refChantier = chantierId)
                Dim chantier = Await _context.LesChantiers _
                .Include(Function(c) c.ListSondes) _
                .FirstOrDefaultAsync(Function(c) c.refChantier = chantierId)

                If chantier Is Nothing Then
                    Return NotFound($"Le chantier avec l'ID {chantierId} n'existe pas.")
                End If

                Return Ok(chantier.ListSondes)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération des sondes du chantier: {ex.Message}")
            End Try
        End Function

        ' ═════════════════════════════════════════════════════════════
        ' ✅ NOUVELLES MÉTHODES : ESSAIS D'EXPANSION
        ' ═════════════════════════════════════════════════════════════

        ''' <summary>
        ''' Récupère tous les essais d'expansion d'une sonde.
        ''' GET: api/sonde/{id}/expansion
        ''' </summary>
        <HttpGet("{id}/expansion")>
        Public Async Function GetEssaisExpansion(id As String) As Task(Of IActionResult)
            Try
                ' Vérifier si la sonde existe
                Dim sonde = Await _context.ListeSondeDb.FirstOrDefaultAsync(Function(s) s.RefSonde = id)
                If sonde Is Nothing Then
                    Return NotFound($"Sonde {id} introuvable.")
                End If

                ' Charger les essais d'expansion associés
                'Dim essais = Await _context.ListeEssaiExpansionsDb _
                '.Where(Function(e) e. RefSonde = id) _
                '.Select(Function(e) New EssaiExpansionDto With {
                '        . RefEssai = e. RefEssai,
                '        . nomEssai = e. nomEssai,
                '        . dateEssai = e. dateEssai,
                '        . Expansion = e. expansion,
                '        . Statut = e. statut
                '    }) _
                '.ToListAsync()

                'Return Ok(essais)}
                Dim essais = Await _context.ListeEssaiExpansionsDb _
                .Where(Function(e) e.RefSonde = id) _
                .Select(Function(e) New EssaiExpansionDto With {
                    .RefEssai = e.RefEssai,
                    .nomEssai = e.nomEssai,
                    .dateEssai = e.dateEssai,
                    .ClassPrecis = e.ClassPrecis,
                    .typeEssai = e.typeEssai
                }) _
                .ToListAsync()
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération des essais d'expansion: {ex.Message}")
            End Try
        End Function

        ' ═════════════════════════════════════════════════════════════
        ' ✅ NOUVELLES MÉTHODES : ESSAIS DE RÉSISTANCE
        ' ═════════════════════════════════════════════════════════════

        ''' <summary>
        ''' Récupère tous les essais de résistance d'une sonde.
        ''' GET: api/sonde/{id}/resistance
        ''' </summary>
        <HttpGet("{id}/resistance")>
        Public Async Function GetEssaisResistance(id As String) As Task(Of IActionResult)
            Try
                ' Vérifier si la sonde existe
                Dim sonde = Await _context.ListeSondeDb.FirstOrDefaultAsync(Function(s) s.RefSonde = id)
                If sonde Is Nothing Then
                    Return NotFound($"Sonde {id} introuvable.")
                End If

                ' Charger les essais de résistance associés
                Dim essais = Await _context.ListeEssaiResistancesDb _
                .Where(Function(e) e.RefSonde = id) _
                .Select(Function(e) New EssaiResistanceDto With {
                        .RefEssai = e.RefEssai,
                        .nomEssai = e.nomEssai,
                        .dateEssai = e.dateEssai,
                        .ClassPrecis = e.ClassPrecis,
                        .RefSonde = e.RefSonde
                    }) _
                .ToListAsync()
                Return Ok(essais)

            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération des essais de résistance: {ex.Message}")
            End Try
        End Function

        ' ═════════════════════════════════════════════════════════════
        ' ✅ NOUVELLES MÉTHODES : ESSAI UNIQUE
        ' ═════════════════════════════════════════════════════════════

        ''' <summary>
        ''' Récupère un essai d'expansion spécifique.
        ''' GET: api/sonde/{id}/expansion/{essaiId}
        ''' </summary>
        <HttpGet("{id}/expansion/{essaiId}")>
        Public Async Function GetEssaiExpansionById(id As String, essaiId As String) As Task(Of IActionResult)
            Try
                Dim essai = Await _context.ListeEssaiExpansionsDb _
                .Where(Function(e) e.RefSonde = id And e.RefEssai = essaiId) _
                .FirstOrDefaultAsync()

                If essai Is Nothing Then
                    Return NotFound($"Essai d'expansion {essaiId} non trouvé pour la sonde {id}.")
                End If

                Dim essaiDto = New EssaiExpansionDto With {
                    .RefEssai = essai.RefEssai,
                    .nomEssai = essai.nomEssai,
                    .dateEssai = essai.dateEssai,
                    .ClassPrecis = essai.ClassPrecis,
                    .RefSonde = essai.RefSonde
                }
                Return Ok(essaiDto)

            Catch ex As Exception
                Return StatusCode(500, $"Erreur: {ex.Message}")
            End Try
        End Function

        ''' <summary>
        ''' Récupère un essai de résistance spécifique.
        ''' GET: api/sonde/{id}/resistance/{essaiId}
        ''' </summary>
        <HttpGet("{id}/resistance/{essaiId}")>
        Public Async Function GetEssaiResistanceById(id As String, essaiId As String) As Task(Of IActionResult)
            Try
                Dim essai = Await _context.ListeEssaiResistancesDb _
                .Where(Function(e) e.RefSonde = id And e.RefEssai = essaiId) _
                .FirstOrDefaultAsync()

                If essai Is Nothing Then
                    Return NotFound($"Essai de résistance {essaiId} non trouvé pour la sonde {id}.")
                End If

                Dim essaiDto = New EssaiResistanceDto With {
                    .RefEssai = essai.RefEssai,
                    .nomEssai = essai.nomEssai,
                    .dateEssai = essai.dateEssai,
                    .ClassPrecis = essai.ClassPrecis,
                    .RefSonde = essai.RefSonde
                }
                Return Ok(essaiDto)

            Catch ex As Exception
                Return StatusCode(500, $"Erreur: {ex.Message}")
            End Try
        End Function

    End Class
End Namespace
