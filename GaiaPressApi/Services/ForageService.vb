Namespace GaiaPress.Core.V17.Services

    ''' <summary>
    ''' Service pour la gestion des forages et de leurs essais.
    ''' </summary>
    Public Class ForageService
        Public ReadOnly _dbContext As GaiaPressDbContext

        ''' <summary>
        ''' Constructeur avec injection de dépendances.
        ''' </summary>
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        ' ======================
        ' Gestion des Forages
        ' ======================

        ''' <summary>
        ''' Récupère tous les forages.
        ''' </summary>
        Public Function GetAllForages() As List(Of Forage)
            Return _dbContext.Forages.ToList()
        End Function

        ''' <summary>
        ''' Récupère un forage par sa référence.
        ''' </summary>
        Public Function GetForageByRef(refForage As String) As Forage
            Return _dbContext.Forages.FirstOrDefault(Function(f) f.F_RefForage = refForage)
        End Function

        ''' <summary>
        ''' Ajoute un nouveau forage.
        ''' </summary>
        Public Sub AddForage(forage As Forage)
            _dbContext.Forages.Add(forage)
            _dbContext.SaveChanges()
        End Sub

        ''' <summary>
        ''' Met à jour un forage existant.
        ''' </summary>
        Public Sub UpdateForage(forage As Forage)
            Dim existingForage = _dbContext.Forages.FirstOrDefault(Function(f) f.F_RefForage = forage.F_RefForage)
            If existingForage IsNot Nothing Then
                _dbContext.Entry(existingForage).CurrentValues.SetValues(forage)
                _dbContext.SaveChanges()
            End If
        End Sub

        ''' <summary>
        ''' Supprime un forage par sa référence.
        ''' </summary>
        Public Sub DeleteForage(refForage As String)
            Dim forage = _dbContext.Forages.FirstOrDefault(Function(f) f.F_RefForage = refForage)
            If forage IsNot Nothing Then
                _dbContext.Forages.Remove(forage)
                _dbContext.SaveChanges()
            End If
        End Sub

        ' ======================
        ' Gestion des Essais de Forage
        ' ======================

        ''' <summary>
        ''' Récupère tous les essais d'un forage.
        ''' </summary>
        Public Function GetEssaisByForage(refForage As String) As List(Of Essai_Forage)
            Return _dbContext.EssaisForage.Where(Function(e) e.E_RefForage = refForage).ToList()
        End Function

        ''' <summary>
        ''' Ajoute un essai à un forage.
        ''' </summary>
        Public Sub AddEssaiToForage(refForage As String, essai As Essai_Forage)
            essai.E_RefForage = refForage
            _dbContext.EssaisForage.Add(essai)
            _dbContext.SaveChanges()
        End Sub

        ''' <summary>
        ''' Supprime un essai par sa référence.
        ''' </summary>
        Public Sub DeleteEssai(refEssai As String)
            Dim essai = _dbContext.EssaisForage.FirstOrDefault(Function(e) e.E_RefEssai = refEssai)
            If essai IsNot Nothing Then
                _dbContext.EssaisForage.Remove(essai)
                _dbContext.SaveChanges()
            End If
        End Sub
    End Class
End Namespace
