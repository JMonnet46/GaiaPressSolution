Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports GaiaPressCoreV17.Models

Namespace GaiaPressCoreV17.Logic
    ''' <summary>Extraits métier autonomes, avec dépendances passées explicitement.</summary>
    Public Module EssaiForageLogicExtraits

        Public Function IsRefUnique(ByVal ref As String, ByVal essais As IEnumerable(Of Essai_Forage)) As Boolean
            If essais Is Nothing Then Return True
            Return Not essais.Any(Function(e) e IsNot Nothing AndAlso String.Equals(e.RefEssai, ref, StringComparison.OrdinalIgnoreCase))
        End Function

        Public Function IsNomUnique(ByVal nom As String, ByVal essais As IEnumerable(Of Essai_Forage)) As Boolean
            If essais Is Nothing Then Return True
            Return Not essais.Any(Function(e) e IsNot Nothing AndAlso String.Equals(e.nomEssai, nom, StringComparison.OrdinalIgnoreCase))
        End Function

        Public Function GetIndexEssai(ByVal ref As String, ByVal essais As IList(Of Essai_Forage)) As Integer
            If essais Is Nothing Then Return -1
            For i As Integer = 0 To essais.Count - 1
                If essais(i) IsNot Nothing AndAlso String.Equals(essais(i).RefEssai, ref, StringComparison.OrdinalIgnoreCase) Then Return i
            Next
            Return -1
        End Function

        Public Function GetEssaiParRef(ByVal ref As String, ByVal essais As IEnumerable(Of Essai_Forage)) As Essai_Forage
            If essais Is Nothing Then Return Nothing
            Return essais.FirstOrDefault(Function(e) e IsNot Nothing AndAlso String.Equals(e.RefEssai, ref, StringComparison.OrdinalIgnoreCase))
        End Function

        Public Function GetEssaiCount(ByVal essais As IEnumerable(Of Essai_Forage)) As Integer
            If essais Is Nothing Then Return 0
            Return essais.Count(Function(e) e IsNot Nothing)
        End Function

        Public Sub SupEssaiDansListe(ByVal ref As String, ByVal essais As IList(Of Essai_Forage))
            Dim index As Integer = GetIndexEssai(ref, essais)
            If index >= 0 Then essais.RemoveAt(index)
        End Sub

        Public Sub SetEssai(ByVal essai As Essai_Forage, ByVal essais As IList(Of Essai_Forage), ByVal index As Integer)
            If essai Is Nothing Then Throw New ArgumentNullException(NameOf(essai))
            If essais Is Nothing Then Throw New ArgumentNullException(NameOf(essais))
            If index < 0 OrElse index >= essais.Count Then Throw New ArgumentOutOfRangeException(NameOf(index))
            essais(index) = essai
        End Sub

        Public Sub AjouterNouvelEssai(ByVal essai As Essai_Forage, ByVal essais As IList(Of Essai_Forage))
            If essai Is Nothing Then Throw New ArgumentNullException(NameOf(essai))
            If essais Is Nothing Then Throw New ArgumentNullException(NameOf(essais))
            essais.Add(essai)
        End Sub

        Public Function GiveMeANewRefEssai(ByVal prefixe As String, ByVal referencesExistantes As IEnumerable(Of String)) As String
            Return ProchaineValeur(prefixe, referencesExistantes, 1)
        End Function

        Public Function GiveMeANewNomEssai(ByVal prefixe As String, ByVal nomsExistants As IEnumerable(Of String)) As String
            Return ProchaineValeur(prefixe, nomsExistants, 0)
        End Function

        Public Function GetIndexSondeEssai(ByVal ref As String, ByVal essais As IEnumerable(Of Essai_Forage)) As Integer
            If essais Is Nothing Then Return -1
            Dim index As Integer = 0
            For Each essai As Essai_Forage In essais
                If essai IsNot Nothing AndAlso String.Equals(essai.RefEssai, ref, StringComparison.OrdinalIgnoreCase) Then Return index
                index += 1
            Next
            Return -1
        End Function

        Public Sub SupSondeEssai(ByVal index As Integer, ByVal essais As IList(Of Essai_Forage))
            If essais IsNot Nothing AndAlso index >= 0 AndAlso index < essais.Count Then essais.RemoveAt(index)
        End Sub

        Private Function ProchaineValeur(ByVal prefixe As String, ByVal valeursExistantes As IEnumerable(Of String), ByVal depart As Integer) As String
            If String.IsNullOrWhiteSpace(prefixe) Then Throw New ArgumentException("Le préfixe est requis.", NameOf(prefixe))
            Dim existants As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            If valeursExistantes IsNot Nothing Then
                For Each valeur As String In valeursExistantes
                    If valeur IsNot Nothing Then existants.Add(valeur)
                Next
            End If
            Dim suffixe As Integer = depart
            Do
                Dim candidate As String = prefixe & " " & suffixe.ToString()
                If Not existants.Contains(candidate) Then Return candidate
                suffixe += 1
            Loop
        End Function

        Public Function CalculeVolNF(ByVal longueurSonde As Single, ByVal diametreInterieur As Single, ByVal volumeCalibre As Single) As Single
            Return CSng(Math.Round(0.001R * Math.PI * CDbl(longueurSonde) * Math.Pow(CDbl(diametreInterieur) / 2.0R, 2), 1) - volumeCalibre)
        End Function

    End Module
End Namespace
