Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Service pour la gestion des sondes (Probes).
    ''' </summary>
    Public Class ProbesService

        Private ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Initialise une nouvelle instance de ProbesService.
        ''' </summary>
        ''' <param name="dbContext">Contexte de base de données.</param>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ''' <summary>
        ''' Récupère toutes les sondes.
        ''' </summary>
        ''' <returns>Liste de toutes les sondes.</returns>
        Public Async Function GetAllProbes() As Task(Of List(Of Probe))
            Return Await _dbContext.Sondes.ToListAsync()
        End Function

        ''' <summary>
        ''' Récupère une sonde par sa référence.
        ''' </summary>
        ''' <param name="refProbe">Référence de la sonde.</param>
        ''' <returns>Sonde correspondante ou Nothing.</returns>
        Public Async Function GetProbeByRef(refProbe As String) As Task(Of Probe)
            Return Await _dbContext.Sondes.FirstOrDefaultAsync(Function(p) p.P_refSonde = refProbe)
        End Function

        ''' <summary>
        ''' Récupère les sondes par référence de chantier.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>Liste des sondes pour le chantier spécifié.</returns>
        Public Async Function GetProbesByrefSonde(refSonde As String) As Task(Of List(Of Probe))
            Return Await _dbContext.Sondes.Where(Function(p) p.P_refSonde = refSonde).ToListAsync()
        End Function

        ''' <summary>
        ''' Ajoute une nouvelle sonde.
        ''' </summary>
        ''' <param name="probe">Sonde à ajouter.</param>
        ''' <returns>Sonde ajoutée.</returns>
        Public Async Function AddProbe(probe As Probe) As Task(Of Probe)
            ' Validation de base
            If String.IsNullOrEmpty(probe.P_refSonde) Then
                Throw New ArgumentException("La référence de la sonde ne peut pas être vide.")
            End If

            _dbContext.Sondes.Add(probe)
            Await _dbContext.SaveChangesAsync()
            Return probe
        End Function

        Public Sub AddMesuresExpansionPropre(mesure As MesuresExpansion)
            ' Exemple : Ajoutez la mesure à la base de données
            _dbContext.MesuresExpansion.Add(mesure)
            _dbContext.SaveChanges()
        End Sub

        '''' <summary>
        '''' Met à jour une sonde existante.
        '''' </summary>
        '''' <param name="refProbe">Référence de la sonde à mettre à jour.</param>
        '''' <param name="probe">Sonde avec les nouvelles valeurs.</param>
        '''' <returns>True si la mise à jour a réussi, False sinon.</returns>
        'Public Async Function UpdateProbe(refProbe As String, probe As Probe) As Task(Of Boolean)
        '    Dim existingProbe = Await _dbContext.sondes.FirstOrDefaultAsync(Function(p) p.P_refSonde = refProbe)

        '    If existingProbe Is Nothing Then
        '        Return False
        '    End If

        '    ' Mise à jour des propriétés
        '    existingProbe = probe
        '    'existingProbe.P_RefSonde = probe.P_refSonde
        '    'existingProbe.P_RefChantier = probe.P_RefChantier
        '    'existingProbe.P_typeSonde = probe.P_typeSonde
        '    'existingProbe.P_Profondeur = probe.P_Profondeur
        '    'existingProbe.P_DateInstallation = probe.P_DateInstallation

        '    Await _dbContext.SaveChangesAsync()
        '    Return True
        'End Function

        ''' <summary>
        ''' Supprime une sonde par sa référence.
        ''' </summary>
        ''' <param name="refProbe">Référence de la sonde à supprimer.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteProbe(refProbe As String) As Task(Of Boolean)
            Dim probeToDelete = Await _dbContext.Sondes.FirstOrDefaultAsync(Function(p) p.P_refSonde = refProbe)

            If probeToDelete Is Nothing Then
                Return False
            End If

            _dbContext.Sondes.Remove(probeToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Supprime toutes les sondes associées à un chantier.
        ''' </summary>
        ''' <param name="refChantier">Référence du chantier.</param>
        ''' <returns>True si la suppression a réussi, False sinon.</returns>
        Public Async Function DeleteProbesByrefSonde(refSonde As String) As Task(Of Boolean)
            Dim probesToDelete = Await _dbContext.Sondes.Where(Function(p) p.P_refSonde = refSonde).ToListAsync()

            If probesToDelete.Count = 0 Then
                Return False
            End If

            _dbContext.Sondes.RemoveRange(probesToDelete)
            Await _dbContext.SaveChangesAsync()
            Return True
        End Function

        ''' <summary>
        ''' Ajoute une sonde à un chantier.
        ''' </summary>
        ''' <param name="chantier">Chantier auquel ajouter la sonde.</param>
        ''' <param name="probe">Sonde à ajouter.</param>
        Public Sub AddProbeToListeSonde(ListeDesSondes As ListeSondes, Probe As Probe)
            ListeDesSondes.AddSonde(Probe)
            'Probe.P_RefChantier = chantier.C_refChantier
        End Sub

        ''' <summary>
        ''' Met à jour une sonde existante.
        ''' </summary>
        ''' <param name="refProbe">Référence de la sonde à mettre à jour.</param>
        ''' <param name="probe">Sonde avec les nouvelles valeurs.</param>
        ''' <returns>True si la mise à jour a réussi, False sinon.</returns>
        Public Async Function UpdateProbe(refProbe As String, probe As Probe) As Task(Of Boolean)
            Dim existingProbe = Await _dbContext.Sondes.FirstOrDefaultAsync(Function(p) p.P_refSonde = refProbe)

            If existingProbe Is Nothing Then
                Return False
            End If

            ' Mise à jour des propriétés
            existingProbe = probe

            'existingProbe.P_RefSonde = probe.P_refSonde
            'existingProbe.P_RefChantier = probe.P_RefChantier
            'existingProbe.P_typeSonde = probe.P_typeSonde
            'existingProbe.P_Profondeur = probe.P_Profondeur
            'existingProbe.P_DateInstallation = probe.P_DateInstallation

            Await _dbContext.SaveChangesAsync()
            Return True
        End Function


        ''' <summary>
        ''' Supprime une sonde d'un chantier.
        ''' </summary>
        ''' <param name="chantier">Chantier contenant la sonde.</param>
        ''' <param name="refProbe">Référence de la sonde à supprimer.</param>
        Public Sub DeleteProbeFromListeSondes(ListeProbe As ListeSondes, refProbe As String)
            Dim probe As Probe = ListeProbe.P_ListeSondes.FirstOrDefault(
                Function(s) s.P_refSonde = refProbe
            )
            If probe IsNot Nothing Then
                ListeProbe.P_ListeSondes.Remove(probe)
            End If
        End Sub
    End Class
End Namespace