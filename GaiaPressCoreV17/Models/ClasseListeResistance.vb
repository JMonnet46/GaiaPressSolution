Imports GaiaPressCoreV17.Services
Imports GaiaPressCoreV17.Logic

Namespace GaiaPressCoreV17.Models

    Public Class ListeEssaiResistance

        Implements ICollection(Of EssaiResistance)

        ' ======= Propriétés de Liste ===========
        Public Property ListeResistance As New List(Of EssaiResistance)

        ' ✅ Propriété HasBeenModified (événement)
        Public Event HasBeenModified As EventHandler

        ' ✅ Liste interne
        Private _items As List(Of EssaiResistance)

        ' ✅ Propriété Count
        Public ReadOnly Property Count As Integer Implements ICollection(Of EssaiResistance).Count
            Get
                Return _items.Count
            End Get
        End Property

        ' ✅ Propriété IsReadOnly
        Public ReadOnly Property IsReadOnly As Boolean Implements ICollection(Of EssaiResistance).IsReadOnly
            Get
                Return False
            End Get
        End Property

        ' =====================================================================================
        ' Constructeur
        ' =====================================================================================
        Public Sub New()
            _items = New List(Of EssaiResistance)
        End Sub

        ' ====================================================================================
        ' Méthodes
        ' ====================================================================================

        ' ✅ Méthode Add
        Public Sub Add(item As EssaiResistance) Implements ICollection(Of EssaiResistance).Add
            _items.Add(item)
            RaiseEvent HasBeenModified(Me, EventArgs.Empty)
        End Sub

        ' ✅ Méthode Clear
        Public Sub Clear() Implements ICollection(Of EssaiResistance).Clear
            _items.Clear()
            RaiseEvent HasBeenModified(Me, EventArgs.Empty)
        End Sub

        ' ✅ Méthode Contains
        Public Function Contains(item As EssaiResistance) As Boolean Implements ICollection(Of EssaiResistance).Contains
            Return _items.Contains(item)
        End Function

        ' ✅ Méthode CopyTo
        Public Sub CopyTo(array As EssaiResistance(), arrayIndex As Integer) Implements ICollection(Of EssaiResistance).CopyTo
            _items.CopyTo(array, arrayIndex)
        End Sub

        ' ✅ Méthode Remove
        Public Function Remove(item As EssaiResistance) As Boolean Implements ICollection(Of EssaiResistance).Remove
            Dim result = _items.Remove(item)
            If result Then
                RaiseEvent HasBeenModified(Me, EventArgs.Empty)
            End If
            Return result
        End Function

        ' ✅ Méthode GetEnumerator
        Public Function GetEnumerator() As IEnumerator(Of EssaiResistance) Implements IEnumerable(Of EssaiResistance).GetEnumerator
            Return _items.GetEnumerator()
        End Function

        ' ✅ Méthode GetEnumerator (IEnumerable)
        Public Function GetEnumerator1() As IEnumerator Implements IEnumerable.GetEnumerator
            Return _items.GetEnumerator()
        End Function


        ' Indexeur (si nécessaire)
        Default Public Property Item(index As Integer) As EssaiResistance
            Get
                Return _items(index)
            End Get
            Set(value As EssaiResistance)
                _items(index) = value
            End Set
        End Property


        'Public Sub AddResistance(EssaiResistance As EssaiResistance)
        '    Me.ListResistance.Add(EssaiResistance)
        '    ' Logique pour lier au Chantier si nécessaire
        '    EssaiResistance.SetRefEssaiResistance(EssaiResistance.GetRefEssai)
        'End Sub

        ' ================================================================================
        ' Gestion de ListeResistance
        ' ================================================================================

        Public Sub AddResistance(EssaiResistance As EssaiResistance)
            _items.Add(EssaiResistance)  ' ✅ Utiliser _items au lieu de ListResistance
        End Sub

        Public Function GetEssaiResistance(refEssai As String) As EssaiResistance
            For Each E As EssaiResistance In _items  ' Utiliser _items
                If E.GetRefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        Public Function GetEssaiResistanceParRefEssai(refEssai As String) As EssaiResistance
            For Each E As EssaiResistance In Me.ListeResistance
                If E.GetRefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        Public Function GetEssaiResistanceParRefSonde(refSonde As String, rang As Integer) As EssaiResistance
            Dim i As Integer
            For Each E As EssaiResistance In Me.ListeResistance
                If E.GetRefSonde = refSonde And i >= rang Then
                    Return E
                End If
                i = i + 1
            Next
            Return Nothing
        End Function

        ' Delete un Essai par référence
        Public Sub DeleteEssaiResistance(refEssai As String)
            Dim EssaiADelete As EssaiResistance = GetEssaiResistance(refEssai)
            If EssaiADelete IsNot Nothing Then
                Me.Remove(EssaiADelete)
            End If
        End Sub

        ' Mettre à jour un Essai
        Public Sub MettreAJourEssaiResistance(refEssai As String, nouvelEssai As EssaiResistance)
            Dim EssaiResistance As EssaiResistance = GetEssaiResistance(refEssai)
            If EssaiResistance IsNot Nothing Then
                EssaiResistance = nouvelEssai
                'Sol. dateEssai = nouvelleDate
            End If
        End Sub

        Public Sub CloseListResistance()
            ' Fermeture et nettoyage
            _items.Clear()
        End Sub


        Public Function GetFichierName() As String
            ' À implémenter selon vos besoins
            GetFichierName = MyResistance.RefEssai.tostring
            Return ""
        End Function

        Public Sub IHaveBeenModified()
            ' Génère un événement indiquant que des modifications ont eu lieu
            RaiseEvent HasBeenModified(Me, EventArgs.Empty)
        End Sub

    End Class
End Namespace