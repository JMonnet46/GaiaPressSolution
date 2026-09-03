Namespace GaiaPressApi.Controllers

    ''' <summary>
    ''' Contrôleur API pour la gestion des sols.
    ''' </summary>
    <ApiController>
    <Route("api/[controller]")>
    Public Class SolController

        Inherits ControllerBase

        ' ✅ Déclarer les dépendances
        Public ReadOnly _context As GaiaPressDbContext
        'PUBLIC ReadOnly _chantierService As ChantierService

        ''' <summary>
        ''' Constructeur avec injection de dépendances.
        'Public Sub New(context As GaiaPressDbContext, chantierService As ChantierService)
        '    _context = context
        '    _chantierService = chantierService
        'End Sub


        ' GET: api/Sol
        <HttpGet>
        Public Async Function GetAllSols() As Task(Of IActionResult)
            Try
                Dim sols = Await _context.Sols.ToListAsync()
                Return Ok(sols)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération des sols: {ex.Message}")
            End Try
        End Function


        ' ======================
        ' POST: api/Sol
        ' ======================
        ''' <summary>
        ''' Ajoute un nouveau sol.
        ''' </summary>
        ' POST: api/Sol

        <HttpGet("{id}")>
        Public Async Function GetSolById(id As String) As Task(Of IActionResult)
            Try
                Dim sol = Await _context.Sols.FirstOrDefaultAsync(Function(s) s.RefSol = id)
                If sol Is Nothing Then
                    Return NotFound($"Le sol avec l'ID {id} n'existe pas.")
                End If
                Return Ok(sol)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la récupération du sol: {ex.Message}")
            End Try
        End Function

        ' POST: api/Sol
        <HttpPost>
        Public Async Function CreateSol(sol As Sol) As Task(Of IActionResult)
            Try
                If sol Is Nothing Then
                    Return BadRequest("Le sol ne peut pas être null.")
                End If

                ' ✅ Correction : AnyAsync avec prédicat valide
                If Await _context.Sols.AnyAsync(Function(s) s.RefSol = sol.RefSol) Then
                    Return Conflict($"Un sol avec l'ID {sol.RefSol} existe déjà.")
                End If

                _context.Sols.Add(sol)
                Await _context.SaveChangesAsync()
                Return CreatedAtAction(NameOf(GetSolById), New With {.id = sol.RefSol}, sol)
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la création du sol: {ex.Message}")
            End Try
        End Function


        <HttpDelete("{id}")>
        Public Async Function DeleteSol(id As String) As Task(Of IActionResult)
            Try
                Dim sol = Await _context.Sols.FirstOrDefaultAsync(Function(s) s.RefSol = id)
                If sol Is Nothing Then
                    Return NotFound($"Le sol avec l'ID {id} n'existe pas.")
                End If

                _context.Sols.Remove(sol)
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la suppression du sol: {ex.Message}")
            End Try
        End Function

        ' ======================
        ' PUT: api/Sol/{id}
        ' ======================
        ''' <summary>
        ''' Met à jour un sol existant.
        ' PUT: api/Sol/{id}
        <HttpPut("{id}")>
        Public Async Function UpdateSol(id As String, sol As Sol) As Task(Of IActionResult)
            Try
                If id <> sol.RefSol Then
                    Return BadRequest("L'ID du sol ne correspond pas à l'ID dans l'URL.")
                End If

                Dim existingSol = Await _context.Sols.FirstOrDefaultAsync(Function(s) s.RefSol = id)
                If existingSol Is Nothing Then
                    Return NotFound($"Le sol avec l'ID {id} n'existe pas.")
                End If

                _context.Entry(existingSol).CurrentValues.SetValues(sol)
                Await _context.SaveChangesAsync()
                Return NoContent()
            Catch ex As Exception
                Return StatusCode(500, $"Erreur lors de la mise à jour du sol: {ex.Message}")
            End Try
        End Function

        '' ======================
        '' GET: api/Sol/ByChantier/{chantierId}
        '' ======================
        '''' <summary>
        '''' Récupère tous les sols associés à un chantier.
        '''' </summary>
        '<HttpGet("ByChantier/{chantierId}")>
        'Public Async Function GetSolsByChantier(chantierId As String) As Task(Of IActionResult)
        '    Try
        '        ' ✅ Syntaxe correcte pour votre modèle (C_ListSols, C_refChantier)
        '        Dim chantier = Await _context.LesChantiers
        '        .Include(Function(c) c.C_ListSols)
        '        .FirstOrDefaultAsync(Function(c) c.C_refChantier = chantierId)

        '        If chantier Is Nothing Then
        '            Return NotFound($"Le chantier avec l'ID {chantierId} n'existe pas.")
        '        End If

        '        Return Ok(chantier.C_ListSols)
        '    Catch ex As Exception
        '        Return StatusCode(500, $"Erreur lors de la récupération des sols du chantier: {ex.Message}")
        '    End Try
        'End Function




    End Class
End Namespace
