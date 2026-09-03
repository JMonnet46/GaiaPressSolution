Namespace GaiaPressApi.Controllers

    ''' <summary>
    ''' Contrôleur API pour la gestion des sondes (Probe).
    ''' </summary>
    <ApiController>
    <Route("api/[controller]")>
    Public Class SondeController
        Inherits ControllerBase

        ' ✅ Déclarer les dépendances
        Public ReadOnly _context As GaiaPressDbContext
        'PUBLIC ReadOnly _chantierService As ChantierService


        ''' <summary>
        ''' Constructeur avec injection de dépendances.
        ''' </summary>
        Public Sub New(context As GaiaPressDbContext)
            _context = context
            '_chantierService = chantierService
        End Sub

        ' ======================
        ' GET: api/Sonde
        ' ======================
        ''' <summary>
        ''' Récupère toutes les sondes.
        <HttpGet>
        Public Async Function GetAllSondes() As Task(Of IActionResult)
            Try
                ' ✅ Utilise _context.Probes (maintenant défini)
                Dim sondes = Await _context.Sondes.ToListAsync()
                Return Ok(sondes)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération des sondes: {ex.Message}")
            End Try
        End Function

        ' ======================
        ' GET: api/Sonde/{id}
        ' ======================
        ''' <summary>
        ''' Récupère une sonde par son ID (P_refSonde).
        <HttpGet("{id}")>
        Public Async Function GetSondeById(id As String) As Task(Of IActionResult)
            Try
                ' ✅ Utilise _context.Probes
                Dim sonde = Await _context.Sondes.FirstOrDefaultAsync(Function(s) s.P_refSonde = id)
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
                If Await _context.Sondes.AnyAsync(Function(s) s.P_refSonde = sonde.P_refSonde) Then
                    Return Conflict($"Une sonde avec l'ID {sonde.P_refSonde} existe déjà.")
                End If

                _context.Sondes.Add(sonde)
                Await _context.SaveChangesAsync()
                Return CreatedAtAction(NameOf(GetSondeById), New With {.id = sonde.P_refSonde}, sonde)
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
                If id <> sonde.P_refSonde Then
                    Return BadRequest("L'ID de la sonde ne correspond pas à l'ID dans l'URL.")
                End If

                Dim existingSonde = Await _context.Sondes.FirstOrDefaultAsync(Function(s) s.P_refSonde = id)
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
                Dim sonde = Await _context.Sondes.FirstOrDefaultAsync(Function(s) s.P_refSonde = id)
                If sonde Is Nothing Then
                    Return NotFound($"La sonde avec l'ID {id} n'existe pas.")
                End If

                _context.Sondes.Remove(sonde)
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la suppression de la sonde: {ex.Message}")
            End Try
        End Function
        ' ================================================
        ' GET: api/Sonde/ByChantier/{chantierId}
        ' ================================================
        '''' <summary>
        '''' Récupère toutes les sondes associées à un chantier.
        '''' </summary>
        '<HttpGet("ByChantier/{chantierId}")>
        'Public Async Function GetSondesByChantier(chantierId As String) As Task(Of IActionResult)
        '    Try
        '        Dim chantier = Await _context.LesChantiers
        '        .Include(Function(c) c.C_ListSondes)
        '        .FirstOrDefaultAsync(Function(c) c.C_refChantier = chantierId)

        '        If chantier Is Nothing Then
        '            Return NotFound($"Le chantier avec l'ID {chantierId} n'existe pas.")
        '        End If

        '        Return Ok(chantier.C_ListSondes)
        '    Catch ex As Exception
        '        Return StatusCode(500, $"Erreur lors de la récupération des sondes du chantier: {ex.Message}")
        '    End Try
        'End Function
    End Class
End Namespace
