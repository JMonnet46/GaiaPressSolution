

Imports System.Collections.Generic
Imports System.Data
Imports System.Data.Common
Imports GaiaPressCoreV17.Models
Imports Microsoft.Data.SqlClient
Imports Microsoft.EntityFrameworkCore


Namespace GaiaPressCoreV17.Services
    Public Class DataService

        Inherits DbContext

        Private ReadOnly _dbContext As GaiaPressDbContext

        ' Déclaration des DataTables
        Public ObjetDataLesChantiers As New DataTable()
        Public ObjetDataLesForages As New DataTable()
        Public ObjetDataLesEssaisForage As New DataTable()
        Public ObjetDataMesuresForage As New DataTable()
        Public ObjetDataSols As New DataTable()
        Public ObjetDataListeSonde As New DataTable()
        Public ObjetDataLesEssaisResistancePropre As New DataTable()
        Public ObjetDataLesMesuresResistancePropre As New DataTable()
        Public ObjetDataLesEssaisExpansionPropre As New DataTable()
        Public ObjetDataLesMesuresExpansionPropre As New DataTable()

        ' Déclaration des variables

        ' Constructeur
        ' Injecter le DbContext via le constructeur
        Public Sub New(dbContext As GaiaPressDbContext)
            _dbContext = dbContext
        End Sub

        '----------------------------------------------------------
        ' Lecture des données sur la Base de données externe
        '----------------------------------------------------------

        Private Shared Function LireTable(
        connexion As DbConnection,
        requeteSql As String
    ) As DataTable

            Dim resultat As New DataTable()

            Using commande As DbCommand = connexion.CreateCommand()
                commande.CommandText = requeteSql

                Using lecteur As DbDataReader = commande.ExecuteReader()
                    resultat.Load(lecteur)
                End Using
            End Using

            Return resultat
        End Function

        Public Sub ChargerDonneesSondes(connexion As DbConnection)
            If connexion.State <> ConnectionState.Open Then
                Throw New InvalidOperationException(
                "La connexion à la base externe doit être ouverte.")
            End If

            ' Remplacer ces noms SQL par ceux réellement présents
            ' dans GaiaPressSchema.sql.
            ObjetDataListeSonde =
            LireTable(connexion, "SELECT * FROM LesSondes")

            ObjetDataLesEssaisResistancePropre =
            LireTable(connexion, "SELECT * FROM LesEssaisResistancePropre")

            ObjetDataLesEssaisExpansionPropre =
            LireTable(connexion, "SELECT * FROM LesEssaisExpansionPropre")

            ObjetDataLesMesuresResistancePropre =
            LireTable(connexion, "SELECT * FROM LesMesuresResistancePropre")

            ObjetDataLesMesuresExpansionPropre =
            LireTable(connexion, "SELECT * FROM LesMesuresExpansionPropre")
        End Sub

        Public Sub ChargerDonneesSols(connexion As DbConnection)
            If connexion.State <> ConnectionState.Open Then
                Throw New InvalidOperationException(
                "La connexion à la base externe doit être ouverte.")
            End If

            ' Remplacer ces noms SQL par ceux réellement présents
            ' dans GaiaPressSchema.sql.
            ObjetDataListeSol =
            LireTable(connexion, "SELECT * FROM LesSols")

        End Sub

        Public Sub ChargerDonneesChantiers(connexion As DbConnection)
            If connexion.State <> ConnectionState.Open Then
                Throw New InvalidOperationException(
                "La connexion à la base externe doit être ouverte.")
            End If

            ' Remplacer ces noms SQL par ceux réellement présents
            ' dans GaiaPressSchema.sql.
            ObjetDataLesChantiers =
            LireTable(connexion, "SELECT * FROM LesChantiers")

            ' Remplacer ces noms SQL par ceux réellement présents
            ' dans GaiaPressSchema.sql.
            ObjetDataLesForages =
            LireTable(connexion, "SELECT * FROM LesForages")

            ' Remplacer ces noms SQL par ceux réellement présents
            ' dans GaiaPressSchema.sql.
            ObjetDataLesEssaisForage =
            LireTable(connexion, "SELECT * FROM LesEssaisForage")

            ' Remplacer ces noms SQL par ceux réellement présents
            ' dans GaiaPressSchema.sql.
            ObjetDataMesures =
            LireTable(connexion, "SELECT * FROM LesMesuresForage")

        End Sub
        ''' <summary>
        ''' Récupère tous les chantiers depuis la base de données.
        ''' </summary>
        Public Function GetAllChantier() As List(Of Chantier)
            Return _dbContext.LesChantiers.ToList()
        End Function

        Public Function SaveAllChantiers() As List(Of Chantier)
            Return _dbContext.LesChantiers.ToList()
        End Function

        Public Function GenerateReferenceEssai(typeEssai As Integer) As String
            Dim random As New Random()
            Return $"ESSAI-{typeEssai}-{random.Next(1000, 9999)}"
        End Function

        ''' <summary>
        ''' Valide un modèle avant sauvegarde.
        ''' </summary>
        Public Function ValidateModel(model As Object) As Boolean
            ' Logique de validation générique (ex : vérifier les champs obligatoires)
            Return True ' À adapter selon vos besoins
        End Function

        ' --- Méthodes pour accéder directement aux DbSet (si nécessaire) ---
        ''' <summary>
        ''' Récupère tous les chantiers (délègue à ChantierService si possible).
        ''' </summary>
        Public Function GetAllChantiers() As List(Of Chantier)
            Return GetAllChantier()
        End Function

        Public Function GetAllForage() As List(Of Forage)
            Return _dbContext.ListeForageDb.ToList()
        End Function


        Protected Overrides Sub OnConfiguring(optionsBuilder As DbContextOptionsBuilder)
            optionsBuilder.UseSqlServer(DatabaseConnection.ConnectionString)
        End Sub

        ' Initialiser les DataTables avec les colonnes nécessaires
        Public Sub InitDataTables()
            ' ObjetDataLesChantiers
            With ObjetDataLesChantiers
                .Columns.Add("refChantier", GetType(String))
                .Columns.Add("nomChantier", GetType(String))
                .Columns.Add("descriptifChantier", GetType(String))
                .Columns.Add("refClient", GetType(String))
                .Columns.Add("entreprise", GetType(String))
                .Columns.Add("adresse", GetType(String))
                .Columns.Add("telephone", GetType(String))
                .Columns.Add("reference", GetType(String))
                .PrimaryKey = New DataColumn() { .Columns("refChantier")}
            End With

            ' ObjetDataLesForages
            With ObjetDataLesForages
                .Columns.Add("refChantier", GetType(String))
                .Columns.Add("refForage", GetType(String))
                .Columns.Add("numeroForage", GetType(Integer))
                .Columns.Add("dateForage", GetType(DateTime))
                .Columns.Add("numeroForage", GetType(Integer))
                .Columns.Add("materiel", GetType(String))
                .Columns.Add("outil", GetType(String))
                .Columns.Add("abscisseX", GetType(Single))
                .Columns.Add("ordonnéeY", GetType(Single))
                .Columns.Add("altitudeZ", GetType(Single))
                .Columns.Add("coteInit", GetType(Single))
                .Columns.Add("coteFinale", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("refForage")}
            End With

            ' ObjetDataEssai
            With ObjetDataLesEssaisForage
                .Columns.Add("refChantier", GetType(String))
                .Columns.Add("refForage", GetType(String))
                .Columns.Add("refEssai", GetType(String))
                .Columns.Add("refSonde", GetType(String))
                .Columns.Add("refEssaiResistancePropre", GetType(String))
                .Columns.Add("refEssaiExpansionPropre", GetType(String))
                .Columns.Add("refEssaiPerteDeCharge", GetType(String))
                .Columns.Add("refSol", GetType(String))
                .Columns.Add("Enregistreur", GetType(String))
                .Columns.Add("Operateur", GetType(String))
                .Columns.Add("VolSonde", GetType(Single))
                .Columns.Add("ClassPrecis", GetType(Integer))
                .Columns.Add("PrecisVolSonde", GetType(Single))
                .Columns.Add("nomEssai", GetType(String))
                .Columns.Add("dateEssai", GetType(DateTime))
                .Columns.Add("heureEssai", GetType(DateTime))
                .Columns.Add("difPresEauAir", GetType(Single))
                .Columns.Add("hauteurCpv", GetType(Single))
                .Columns.Add("presP0", GetType(Single))
                .Columns.Add("presFluage", GetType(Single))
                .Columns.Add("presLimite", GetType(Single))
                .Columns.Add("presLimiteNf", GetType(Single))
                .Columns.Add("presLimiteTheo", GetType(Single))
                .Columns.Add("PrecisVolume", GetType(Single))
                .Columns.Add("PrecisPression", GetType(Single))
                .Columns.Add("poidsVol", GetType(Single))
                .Columns.Add("poidsVolDejauge", GetType(Single))
                .Columns.Add("coefPoisson", GetType(Single))
                .Columns.Add("modPressio", GetType(Single))
                .Columns.Add("modPressioNf", GetType(Single))
                .Columns.Add("modElastique", GetType(Single))
                .Columns.Add("modElastiqueNf", GetType(Single))
                .Columns.Add("cohesion", GetType(Single))
                .Columns.Add("frottement", GetType(Single))
                .Columns.Add("frottementintergranulaire", GetType(Single))
                .Columns.Add("U0", GetType(Single))
                .Columns.Add("profNappe", GetType(Single))
                .Columns.Add("profSonde", GetType(Single))
                .Columns.Add("coteZeroGeomeca", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("refEssai")}
            End With

            ' ObjetDataMesuresForage
            With ObjetDataMesuresForage
                .Columns.Add("ind", GetType(Integer))
                .Columns.Add("refEssai", GetType(String))
                .Columns.Add("indiceMesure", GetType(Integer))
                .Columns.Add("pression", GetType(Single))
                .Columns.Add("volume15", GetType(Single))
                .Columns.Add("volume30", GetType(Single))
                .Columns.Add("volume60", GetType(Single))
                .Columns.Add("contrainteRadiale", GetType(Single))
                .Columns.Add("contrainteVerticale", GetType(Single))
                .Columns.Add("deformation", GetType(Single))
                .Columns.Add("PNF", GetType(Single))
                .Columns.Add("VNF", GetType(Single))
                .Columns.Add("Qi", GetType(Single))
                .Columns.Add("Pi", GetType(Single))
                .Columns.Add("Qp", GetType(Single))
                .Columns.Add("Pp", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("ind")}
            End With

            ' ObjetDataSols
            With ObjetDataSols
                .Columns.Add("refSol", GetType(String))
                .Columns.Add("nomSol", GetType(String))
                .Columns.Add("zoneGeo", GetType(String))
                .Columns.Add("famille", GetType(String))
                .Columns.Add("classeSol", GetType(String))
                .Columns.Add("sousClasse", GetType(Integer))
                .Columns.Add("classeInférieure", GetType(String))
                .Columns.Add("sousClasseInférieure", GetType(Integer))
                .Columns.Add("nomSolNormalise", GetType(String))
                .Columns.Add("dmax", GetType(Single))
                .Columns.Add("tamisat50mm", GetType(Single))
                .Columns.Add("tamisat2mm", GetType(Single))
                .Columns.Add("tamisat80microns", GetType(Single))
                .Columns.Add("VBS", GetType(Single))
                .Columns.Add("IP", GetType(Single))
                .Columns.Add("Es", GetType(Single))
                .Columns.Add("phiMu", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("refSol")}
            End With

            ' ObjetDataListeSonde
            With ObjetDataListeSonde
                .Columns.Add("refSonde", GetType(String))
                .Columns.Add("typeSonde", GetType(Integer))
                .Columns.Add("typeDeformee", GetType(Integer))
                .Columns.Add("typeContrainte", GetType(Integer))
                .Columns.Add("ls", GetType(Single))
                .Columns.Add("lg", GetType(Single))
                .Columns.Add("lt", GetType(Single))
                .Columns.Add("longInjectPompage", GetType(Single))
                .Columns.Add("Vs", GetType(Single))
                .Columns.Add("ds", GetType(Single))
                .Columns.Add("ClassPrecis", GetType(Integer))
                .Columns.Add("epMembraneGarde", GetType(Single))
                .Columns.Add("PrEpMembraneGarde", GetType(Single))
                .Columns.Add("epMembraneMesure", GetType(Single))
                .Columns.Add("PrEpMembraneMesure", GetType(Single))
                .Columns.Add("epTubeFendu", GetType(Single))
                .Columns.Add("PrEpTubeFendu", GetType(Single))
                .Columns.Add("epaisInitMembrane", GetType(Single))
                .Columns.Add("PrEpaisInitMembrane", GetType(Single))
                .Columns.Add("PrLong", GetType(Single))
                .Columns.Add("PrDiam", GetType(Single))
                .Columns.Add("PrP", GetType(Single))
                .Columns.Add("PrV", GetType(Single))
                .Columns.Add("PrPV", GetType(Single))
                .Columns.Add("PrZ", GetType(Single))
                .Columns.Add("PrT", GetType(Single))
                .Columns.Add("Pm", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("refSonde")}
            End With

            ' ObjetDataLesEssaisResistancePropre
            With ObjetDataLesEssaisResistancePropre
                .Columns.Clear() ' Effacer les colonnes existantes si nécessaire
                .Columns.Add("refEssai", GetType(String))
                .Columns.Add("refSonde", GetType(String))
                .Columns.Add("ClassPrecis", GetType(Integer))
                .Columns.Add("nomEssai", GetType(String))
                .Columns.Add("dateEssai", GetType(Date))
                .Columns.Add("difPresEauAir", GetType(Single))
                .Columns.Add("hauteurCpv", GetType(Single))
                .Columns.Add("a0", GetType(Single))
                .Columns.Add("a1", GetType(Single))
                .Columns.Add("a2", GetType(Single))
                .Columns.Add("a3", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("refEssai")}
            End With

            ' ObjetDataLesMesuresResistancePropre
            With ObjetDataLesMesuresResistancePropre
                .Columns.Add("Ind", GetType(Integer))
                .Columns.Add("refEssai", GetType(String))
                .Columns.Add("indiceMesure", GetType(Integer))
                .Columns.Add("pression", GetType(Single))
                .Columns.Add("volume", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("Ind")}
            End With

            ' ObjetDataLesEssaisExpansionPropre
            With ObjetDataLesEssaisExpansionPropre
                .Columns.Add("refEssai", GetType(String))
                .Columns.Add("refSonde", GetType(String))
                .Columns.Add("ClassPrecis", GetType(Integer))
                .Columns.Add("nomEssai", GetType(String))
                .Columns.Add("dateEssai", GetType(Date))
                .Columns.Add("difPresEauAir", GetType(Single))
                .Columns.Add("hauteurCpv", GetType(Single))
                .Columns.Add("Di", GetType(Single))
                .Columns.Add("Vc", GetType(Single))
                .Columns.Add("Vs", GetType(Single))
                .Columns.Add("EpMembrane", GetType(Single))
                .Columns.Add("Prp", GetType(Single))
                .Columns.Add("PrV", GetType(Single))
                .Columns.Add("coefficientA", GetType(Single))
                .Columns.Add("coefficientB", GetType(Single))
                .Columns.Add("PrecisVc", GetType(Single))
                .Columns.Add("PrecisVs", GetType(Single))
                .Columns.Add("PrEpMembrane", GetType(Single))
                .Columns.Add("PrecisA", GetType(Single))
                .Columns.Add("PrecisB", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("refEssai")}
            End With

            ' ObjetDataLesMesuresExpansionPropre
            With ObjetDataLesMesuresExpansionPropre
                .Columns.Add("Ind", GetType(Integer))
                .Columns.Add("refEssai", GetType(String))
                .Columns.Add("indiceMesure", GetType(Integer))
                .Columns.Add("pression", GetType(Single))
                .Columns.Add("volume", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("Ind")}
            End With


        End Sub

        ' Méthode pour charger les données depuis SQLite
        Public Sub LoadDataFromDatabase(chantierId As String)
            ' Connexion SQL Server : réutilise la configuration centralisée existante.
            Using conn As New SqlConnection(DatabaseConnection.ConnectionString)
                conn.Open()

                ' Le chantier et ses données dépendantes restent filtrés par refChantier.
                Using cmd As New SqlCommand("SELECT * FROM [LesChantiers] WHERE [refChantier] = @refChantier", conn)
                    cmd.Parameters.AddWithValue("@refChantier", chantierId)
                    ObjetDataLesChantiers.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataLesChantiers.Load(reader)
                    End Using
                End Using

                ' Charge LesForages
                Using cmd As New SqlCommand("SELECT * FROM [LesForages] WHERE [refChantier] = @refChantier", conn)
                    cmd.Parameters.AddWithValue("@refChantier", chantierId)
                    ObjetDataLesForages.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataLesForages.Load(reader)
                    End Using
                End Using

                ' Charge LesEssaisForage
                Using cmd As New SqlCommand(
                    "SELECT e.* FROM [LesEssaisForage] AS e " &
                    "INNER JOIN [LesForages] AS f ON f.[refForage] = e.[refForage] " &
                    "WHERE f.[refChantier] = @refChantier", conn)
                    cmd.Parameters.AddWithValue("@refChantier", chantierId)
                    ObjetDataLesEssaisForage.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataLesEssaisForage.Load(reader)
                    End Using
                End Using

                ' ChargeChantier "LesMesuresForage"
                Using cmd As New SqlCommand(
                    "SELECT m.* FROM [LesMesuresForage] AS m " &
                    "INNER JOIN [LesEssaisForage] AS e ON e.[refEssai] = m.[refEssai] " &
                    "INNER JOIN [LesForages] AS f ON f.[refForage] = e.[refForage] " &
                    "WHERE f.[refChantier] = @refChantier", conn)
                    cmd.Parameters.AddWithValue("@refChantier", chantierId)
                    ObjetDataMesuresForage.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataMesuresForage.Load(reader)
                    End Using
                End Using

                ' Charge LesSondes
                ' Ces tables ne portent pas de filtre chantier attesté : elles sont chargées intégralement.
                Using cmd As New SqlCommand("SELECT * FROM [LesSondes]", conn)
                    ObjetDataListeSonde.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataListeSonde.Load(reader)
                    End Using
                End Using

                ' Charge LesEssaisExpansionPropre
                Using cmd As New SqlCommand("SELECT * FROM [LesEssaisExpansionPropre]", conn)
                    ObjetDataLesEssaisExpansionPropre.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataLesEssaisExpansionPropre.Load(reader)
                    End Using
                End Using

                ' Charge LesEssaisResistancePropre
                Using cmd As New SqlCommand("SELECT * FROM [LesEssaisResistancePropre]", conn)
                    ObjetDataLesEssaisResistancePropre.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataLesEssaisResistancePropre.Load(reader)
                    End Using
                End Using

                ' Charge LesMesuresExpansionPropre
                Using cmd As New SqlCommand("SELECT * FROM [LesMesuresExpansionPropre]", conn)
                    ObjetDataLesMesuresExpansionPropre.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataLesMesuresExpansionPropre.Load(reader)
                    End Using
                End Using

                ' Charge LesMesuresResistancePropre
                Using cmd As New SqlCommand("SELECT * FROM [LesMesuresResistancePropre]", conn)
                    ObjetDataLesMesuresResistancePropre.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataLesMesuresResistancePropre.Load(reader)
                    End Using
                End Using

                ' Charge LesSols
                Using cmd As New SqlCommand("SELECT * FROM [LesSols]", conn)
                    ObjetDataSols.Clear()
                    Using reader As SqlDataReader = cmd.ExecuteReader()
                        ObjetDataSols.Load(reader)
                    End Using
                End Using
            End Using
        End Sub

        '**************************************************************************
        ' Gestion des LesChantier
        '**************************************************************************
        ' Méthodes pour manipuler les DataTables
        Public Sub AddChantier(ChantierLocal As Chantier)
            Dim row As DataRow = ObjetDataLesChantiers.NewRow()
            row("refChantier") = ChantierLocal.refChantier
            row("nomChantier") = ChantierLocal.nomChantier
            row("descriptifChantier") = ChantierLocal.descriptifChantier
            row("refClient") = ChantierLocal.refClient
            row("entreprise") = ChantierLocal.entreprise
            row("adresse") = ChantierLocal.adresse
            row("telephone") = ChantierLocal.telephone
            row("reference") = ChantierLocal.reference
            ObjetDataLesChantiers.Rows.Add(row)
        End Sub

        Public Sub UpdateChantier(ChantierLocal As Chantier)
            For Each row As DataRow In ObjetDataLesChantiers.Rows
                If row("refChantier").ToString() = ChantierLocal.refChantier Then
                    row("nomChantier") = ChantierLocal.nomChantier
                    row("descriptifChantier") = ChantierLocal.descriptifChantier
                    row("refClient") = ChantierLocal.refClient
                    row("entreprise") = ChantierLocal.entreprise
                    row("adresse") = ChantierLocal.adresse
                    row("telephone") = ChantierLocal.telephone
                    row("reference") = ChantierLocal.reference
                    Exit For
                End If
            Next
        End Sub

        Public Sub DeleteChantier(refChantier As String)
            Dim rows() As DataRow = ObjetDataLesChantiers.Select("refChantier = '" & refChantier & "'")
            If rows.Length > 0 Then
                ObjetDataLesChantiers.Rows.Remove(rows(0))
            End If
        End Sub

        Public Function GetChantierByRef(refChantier As String) As Chantier
            Dim rows() As DataRow = ObjetDataLesChantiers.Select("refChantier = '" & refChantier & "'")
            If rows.Length = 0 Then
                Return Nothing
            End If
            Return New Chantier With {
                .refChantier = rows(0)("refChantier").ToString(),
                .nomChantier = rows(0)("nomChantier").ToString(),
                .descriptifChantier = rows(0)("descriptifChantier").ToString(),
                .refClient = rows(0)("refClient").ToString(),
                .entreprise = rows(0)("entreprise").ToString(),
                .adresse = rows(0)("adresse").ToString(),
                .telephone = rows(0)("telephone").ToString(),
                .reference = rows(0)("reference").ToString()
            }
        End Function

        '*************************************************************************
        '  Gestion des LesForages
        '*************************************************************************

        ' Méthodes pour Forage
        Public Sub AddForage(forage As Forage)
            Dim row As DataRow = ObjetDataLesForages.NewRow()
            row("refChantier") = forage.RefChantier
            row("refForage") = forage.RefForage
            row("numeroForage") = forage.numeroForage
            row("nomForage") = forage.NomForage
            row("dateForage") = forage.dateForage
            row("materiel") = forage.materiel
            row("outil") = forage.outil
            row("abscisseX") = forage.abscisseX
            row("ordonneeY") = forage.ordonneeY
            row("altitudeZ") = forage.altitudeZ
            row("coteInit") = forage.coteInit
            row("coteFinale") = forage.coteFinale
            ObjetDataLesForages.Rows.Add(row)
        End Sub
        ''' <summary>

        Public Sub UpdateForage(forage As Forage)
            'Dim rows() As DataRow = ObjetDataLesForages.Select("refForage = '" & forage. RefForage & "'")
            For Each row As DataRow In ObjetDataLesForages.Rows
                If row("refForage").ToString() = forage.RefForage Then
                    row("refChantier") = forage.RefChantier
                    row("refForage") = forage.RefForage
                    row("numeroForage") = forage.numeroForage
                    row("nomForage") = forage.NomForage
                    row("dateForage") = forage.dateForage
                    row("materiel") = forage.materiel
                    row("outil") = forage.outil
                    row("abscisseX") = forage.abscisseX
                    row("ordonneeY") = forage.ordonneeY
                    row("altitudeZ") = forage.altitudeZ
                    row("coteInit") = forage.coteInit
                    row("coteFinale") = forage.coteFinale
                    Exit For
                End If
            Next
        End Sub

        Public Sub DeleteForage(refForage As String)
            Dim rows() As DataRow = ObjetDataLesForages.Select("refForage = '" & refForage & "'")
            If rows.Length > 0 Then
                ObjetDataLesForages.Rows.Remove(rows(0))
            End If
        End Sub

        Public Function GetForageByRef(refForage As String) As Forage
            Dim rows() As DataRow = ObjetDataLesForages.Select("refForage = '" & refForage & "'")
            If rows.Length = 0 Then
                Return Nothing
            End If
            Return New Forage With {
                .RefChantier = rows(0)("refChantier").ToString(),
                .RefForage = rows(0)("refForage").ToString(),
                .numeroForage = rows(0)("numeroForage").ToString(),
                .NomForage = rows(0)("nomForage").ToString(),
                .dateForage = If(rows(0)("dateForage") Is DBNull.Value, Nothing, CDate(rows(0)("dateForage"))),
                .materiel = rows(0)("materiel").ToString(),
                .outil = rows(0)("outil").ToString(),
                .abscisseX = rows(0)("abscisseX").ToString(),
                .ordonneeY = rows(0)("ordonneeY").ToString(),
                .altitudeZ = rows(0)("altitudeZ").ToString(),
                .coteInit = rows(0)("coteInit").ToString(),
                .coteFinale = rows(0)("coteFinale").ToString()
            }
        End Function

        '*************************************************************************
        '  Gestion des LesEssaisForage
        '*************************************************************************

        ' Méthodes pour LesEssaisForage
        Public Sub AddEssaiForage(essai As Essai_Forage)
            Dim row As DataRow = ObjetDataEssai.NewRow()
            row("refChantier") = essai.RefChantier
            row("refForage") = essai.RefForage
            row("refEssai") = essai.refEssai
            row("refSonde") = essai.refSonde
            row("refEssaiResistancePropre") = essai.refEssaiResistancePropre
            row("refEssaiExpansionPropre") = essai.refEssaiExpansionPropre
            row("refEssaiPerteDeCharge") = essai.refEssaiPerteDeCharge
            row("refSol") = essai.RefSol
            row("Enregistreur") = essai.Enregistreur
            row("Operateur") = essai.Operateur
            row("VolSonde") = essai.VolSonde
            row("ClassPrecis") = essai.ClassPrecis
            row("PrecisVolSonde") = essai.PrecisVolSonde
            row("nomEssai") = essai.nomEssai
            row("dateEssai") = essai.dateEssai
            row("heureEssai") = essai.heureEssai
            row("difPresEauAir") = essai.difPresEauAir
            row("hauteurCpv") = essai.hauteurCpv
            row("presP0") = essai.presP0
            row("presFluage") = essai.presFluage
            row("presLimite") = essai.presLimite
            row("presLimiteNf") = essai.presLimiteNF
            row("presLimiteTheo") = essai.presLimiteTheo
            row("PrecisVolume") = essai.PrecisVolume
            row("PrecisPression") = essai.PrecisPression
            row("poidsVol") = essai.poidsVol
            row("poidsVolDejauge") = essai.poidsVolDejauge
            row("coefPoisson") = essai.coefPoisson
            row("modPressio") = essai.modPressio
            row("modPressioNf") = essai.modPressioNf
            row("modElastique") = essai.modElastique
            row("modElastiqueNf") = essai.modElastiqueNf
            row("cohesion") = essai.Cohesion
            row("frottement") = essai.Frottement
            row("frottementintergranulaire") = essai.FrottementIntergranulaire
            row("U0") = essai.U0
            row("profNappe") = essai.profNappe
            row("profSonde") = essai.profSonde
            row("coteZeroGeomeca") = essai.coteZeroGeomeca
            ObjetDataEssai.Rows.Add(row)

        End Sub


        ' Mettre à jour un LesEssaisLesForages

        Public Sub UpDateLesEssaisLesForages(NouvelEssai As Essai_Forage)
            'Dim Essai As Essai_Forage = GetEssaiForageByRef(refEssai)
            For Each row As DataRow In ObjetDataLesEssaisForage.Rows
                If row("refEssai").ToString() = NouvelEssai.refEssai Then
                    row("refChantier") = NouvelEssai.RefChantier
                    row("RefForage") = NouvelEssai.RefForage
                    row("refEssai") = NouvelEssai.refEssai
                    row("refSonde") = NouvelEssai.refSonde
                    row("refEssaiResistancePropre") = NouvelEssai.refEssaiResistancePropre
                    row("refEssaiExpansionPropre") = NouvelEssai.refEssaiExpansionPropre
                    row("refEssaiPerteDeCharge") = NouvelEssai.refEssaiPerteDeCharge
                    row("refSol") = NouvelEssai.RefSol
                    row("Enregistreur") = NouvelEssai.Enregistreur
                    row("Operateur") = NouvelEssai.Operateur
                    row("VolSonde") = NouvelEssai.VolSonde
                    row("ClassPrecis") = NouvelEssai.ClassPrecis
                    row("PrecisVolSonde") = NouvelEssai.PrecisVolSonde
                    row("nomEssai") = NouvelEssai.nomEssai
                    row("dateEssai") = NouvelEssai.dateEssai
                    row("heureEssai") = NouvelEssai.heureEssai
                    row("difPresEauAir") = NouvelEssai.difPresEauAir
                    row("hauteurCpv") = NouvelEssai.hauteurCpv
                    row("presP0") = NouvelEssai.presP0
                    row("presFluage") = NouvelEssai.presFluage
                    row("presLimite") = NouvelEssai.presLimite
                    row("presLimiteNf") = NouvelEssai.presLimiteNF
                    row("presLimiteTheo") = NouvelEssai.presLimiteTheo
                    row("PrecisVolume") = NouvelEssai.PrecisVolume
                    row("PrecisPression") = NouvelEssai.PrecisPression
                    row("poidsVol") = NouvelEssai.poidsVol
                    row("poidsVolDejauge") = NouvelEssai.poidsVolDejauge
                    row("coefPoisson") = NouvelEssai.coefPoisson
                    row("modPressio") = NouvelEssai.modPressio
                    row("modPressioNf") = NouvelEssai.modPressioNf
                    row("modElastique") = NouvelEssai.modElastique
                    row("modElastiqueNf") = NouvelEssai.modElastiqueNf
                    row("cohesion") = NouvelEssai.Cohesion
                    row("frottement") = NouvelEssai.Frottement
                    row("frottementintergranulaire") = NouvelEssai.FrottementIntergranulaire
                    row("U0") = NouvelEssai.U0
                    row("profNappe") = NouvelEssai.profNappe
                    row("profSonde") = NouvelEssai.profSonde
                    row("coteZeroGeomeca") = NouvelEssai.coteZeroGeomeca
                    Exit For
                End If
            Next
        End Sub

        ' Add un LesEssaisLesForages
        Public Sub AddLesEssaisLesForages(Essai As Essai_Forage)
            MyForage.ListEssais.Add(Essai)
            ' Lier l essai au chantier (si nécessaire)
            Essai.RefChantier = MyForage.RefChantier
        End Sub

        ' Mettre à jour un LesEssaisLesForages
        Public Sub MettreAJourLesEssaisLesForages(refEssai As String, nouveauNom As String, nouvelleDate As String)
            Dim Essai As Essai_Forage = GetEssaiForage(refEssai)
            If Essai IsNot Nothing Then
                Essai.nomEssai = nouveauNom
                Essai.dateEssai = nouvelleDate
            End If
        End Sub

        Public Function GetLesForagesByChantier(chantierId As String) As List(Of Forage)
            Try
                Dim LesForages As New List(Of Forage)
                Dim rows() As DataRow = ObjetDataLesForages.Select("$refChantier = '" & chantierId & "'")

                For Each row As DataRow In rows
                    Dim forage As New Forage With {
                .RefForage = If(row("refForage") Is DBNull.Value, String.Empty, row("refForage").ToString()),
                .NomForage = If(row("nomForage") Is DBNull.Value, String.Empty, row("nomForage").ToString()),
                .dateForage = If(row("dateForage") Is DBNull.Value, Nothing, CDate(row("dateForage")))
            }
                    LesForages.Add(forage)
                Next

                Return LesForages
            Catch ex As Exception
                ' Gestion des erreurs
                Return New List(Of Forage)
            End Try
        End Function






        ''' <summary>
        ''' Récupère un essai de forage par sa référence depuis ObjetDataEssai.
        ''' <summary>
        ''' Récupère un essai de forage par sa référence depuis ObjetDataEssaiForage.
        ''' </summary>
        ''' <summary>
        ''' Récupère un essai de forage par sa référence depuis ObjetDataEssaiForage.
        ''' </summary>
        ''' <summary>
        ''' <summary>
        ''' Récupère un essai de forage par sa référence depuis ObjetDataEssaiForage.
        ''' </summary>
        Public Function GetEssaiForageByRef(refEssai As String) As Essai_Forage
            ' Filtrer les lignes correspondantes dans ObjetDataEssaiForage
            Dim rows() As DataRow = ObjetDataLesEssaisForage.Select($"refEssai = '{refEssai}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet EssaiForage avec les valeurs de la DataRow
            Return New Essai_Forage With {
            .RefChantier = If(rows(0).IsNull("RefChantier"), String.Empty, rows(0).Field(Of String)("RefChantier")),
        .RefForage = If(rows(0).IsNull("refForage"), String.Empty, rows(0).Field(Of String)("refForage")),
        .refEssai = If(rows(0).IsNull("refEssai"), String.Empty, rows(0).Field(Of String)("refEssai")),
        .refSonde = If(rows(0).IsNull("refSonde"), String.Empty, rows(0).Field(Of String)("refSonde")),
        .refEssaiResistancePropre = If(rows(0).IsNull("refEssaiResistancePropre"), String.Empty, rows(0).Field(Of String)("refEssaiResistancePropre")),
        .refEssaiExpansionPropre = If(rows(0).IsNull("refEssaiExpansionPropre"), String.Empty, rows(0).Field(Of String)("refEssaiExpansionPropre")),
        .refEssaiPerteDeCharge = If(rows(0).IsNull("refEssaiPerteDeCharge"), String.Empty, rows(0).Field(Of String)("refEssaiPerteDeCharge")),
        .RefSol = If(rows(0).IsNull("refSol"), String.Empty, rows(0).Field(Of String)("refSol")),
        .Enregistreur = If(rows(0).IsNull("Enregistreur"), String.Empty, rows(0).Field(Of String)("Enregistreur")),
        .Operateur = If(rows(0).IsNull("Operateur"), String.Empty, rows(0).Field(Of String)("Operateur")),
        .VolSonde = If(rows(0).IsNull("VolSonde"), 0, CSng(rows(0).Field(Of Single)("VolSonde"))),
        .nomEssai = If(rows(0).IsNull("nomEssai"), String.Empty, rows(0).Field(Of String)("nomEssai")),
        .ClassPrecis = If(rows(0).IsNull("ClassPrecis"), 0, CInt(rows(0).Field(Of Integer)("ClassPrecis"))),
        .PrecisVolSonde = If(rows(0).IsNull("PrecisVolSonde"), 0, CSng(rows(0).Field(Of Single)("PrecisVolSonde"))),
            .dateEssai = If(rows(0).IsNull("dateEssai"), Nothing, rows(0).Field(Of Date)("dateEssai")),
        .heureEssai = If(rows(0).IsNull("heureEssai"), Nothing, rows(0).Field(Of DateTime)("heureEssai")),
            .difPresEauAir = If(rows(0).IsNull("difPresEauAir"), 0, CSng(rows(0).Field(Of Single)("difPresEauAir"))),
        .hauteurCpv = If(rows(0).IsNull("hauteurCpv"), 0, CSng(rows(0).Field(Of Single)("hauteurCpv"))),
        .presP0 = If(rows(0).IsNull("presP0"), 0, CSng(rows(0).Field(Of Single)("presP0"))),
        .presFluage = If(rows(0).IsNull("presFluage"), 0, CSng(rows(0).Field(Of Single)("presFluage"))),
        .presLimite = If(rows(0).IsNull("presLimite"), 0, CSng(rows(0).Field(Of Single)("presLimite"))),
        .presLimiteNF = If(rows(0).IsNull("presLimiteNf"), 0, CSng(rows(0).Field(Of Single)("presLimiteNf"))),
        .presLimiteTheo = If(rows(0).IsNull("presLimiteTheo"), 0, CSng(rows(0).Field(Of Single)("presLimiteTheo"))),
        .PrecisVolume = If(rows(0).IsNull("PrecisVolume"), 0, CSng(rows(0).Field(Of Single)("PrecisVolume"))),
        .PrecisPression = If(rows(0).IsNull("PrecisPression"), 0, CSng(rows(0).Field(Of Single)("PrecisPression"))),
        .poidsVol = If(rows(0).IsNull("poidsVol"), 0, CSng(rows(0).Field(Of Single)("poidsVol"))),
        .poidsVolDejauge = If(rows(0).IsNull("poidsVolDejauge"), 0, CSng(rows(0).Field(Of Single)("poidsVolDejauge"))),
        .coefPoisson = If(rows(0).IsNull("coefPoisson"), 0, CSng(rows(0).Field(Of Single)("coefPoisson"))),
        .modPressio = If(rows(0).IsNull("modPressio"), 0, CSng(rows(0).Field(Of Single)("modPressio"))),
        .modPressioNf = If(rows(0).IsNull("modPressioNf"), 0, CSng(rows(0).Field(Of Single)("modPressioNf"))),
        .modElastique = If(rows(0).IsNull("modElastique"), 0, CSng(rows(0).Field(Of Single)("modElastique"))),
        .modElastiqueNf = If(rows(0).IsNull("modElastiqueNf"), 0, CSng(rows(0).Field(Of Single)("modElastiqueNf"))),
        .Cohesion = If(rows(0).IsNull("cohesion"), 0, CSng(rows(0).Field(Of Single)("cohesion"))),
        .Frottement = If(rows(0).IsNull("frottement"), 0, CSng(rows(0).Field(Of Single)("frottement"))),
        .FrottementIntergranulaire = If(rows(0).IsNull("frottementIntergranulaire"), 0, CSng(rows(0).Field(Of Single)("frottementIntergranulaire"))),
        .U0 = If(rows(0).IsNull("U0"), 0, CSng(rows(0).Field(Of Single)("U0"))),
        .profNappe = If(rows(0).IsNull("profNappe"), 0, CSng(rows(0).Field(Of Single)("profNappe"))),
        .profSonde = If(rows(0).IsNull("profSonde"), 0, CSng(rows(0).Field(Of Single)("profSonde"))),
        .coteZeroGeomeca = If(rows(0).IsNull("coteZeroGeomeca"), 0, CSng(rows(0).Field(Of Single)("coteZeroGeomeca")))
    }
        End Function
        Public Function GetEssaiForage(refEssai As String) As Essai_Forage
            For Each E As Essai_Forage In MyForage.ListEssais
                If E.refEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        ' Delete un LesEssaisLesForages par référence
        Public Sub DeleteLesEssaisLesForages(refEssai As String)
            Dim EssaiADelete As Essai_Forage = GetEssaiForage(refEssai)
            If EssaiADelete IsNot Nothing Then
                MyForage.ListEssais.Remove(EssaiADelete)
            End If
        End Sub


        ' ********************************************************************
        '     Gestion des MesuresLesEssaisForage
        ' ********************************************************************

        ' Méthodes pour Mesure
        Public Sub AddMesureLesEssaisForage(mesure As MesureEssaiForage)
            Dim row As DataRow = ObjetDataMesuresForage.NewRow()
            row("ind") = mesure.RefEssai
            row("refEssai") = mesure.RefEssai
            row("indiceMesure") = mesure.IndiceMesure
            row("pression") = mesure.Pression
            row("volume15") = mesure.Volume15
            row("volume30") = mesure.Volume30
            row("volume60") = mesure.Volume60
            row("contrainteRadiale") = mesure.ContrainteRadiale
            row("contrainteVerticale") = mesure.ContrainteVerticale
            row("deformation") = mesure.Deformation
            row("PNF") = mesure.PNF
            row("VNF") = mesure.VNF
            row("Qi") = mesure.Qi
            row("Pi") = mesure.Pi
            row("Qp") = mesure.Qp
            row("Pp") = mesure.Pp
            ObjetDataMesuresForage.Rows.Add(row)
        End Sub

        ' Donne la liste des Mesures pour un EssaiForage défini par refEssai
        Public Function GetMesuresByEssai(refEssai As String) As List(Of MesureEssaiForage)
            Dim Mesures As New List(Of MesureEssaiForage)
            Dim rows() As DataRow = ObjetDataMesuresForage.Select("refEssai = '" & refEssai & "'")
            For Each row As DataRow In rows
                Mesures.Add(New MesureEssaiForage With {
                    .RefEssai = row("refEssai").ToString(),
                    .IndiceMesure = CInt(row("indiceMesure")),
                    .Pression = CDbl(row("pression")),
                    .Volume15 = CDbl(row("volume15")),
                    .Volume30 = CDbl(row("volume30")),
                    .Volume60 = CDbl(row("volume"))
                })
            Next
            Return Mesures
        End Function

        Public Sub UpDateMesureLesEssaisLesForages(NouvelleMesure As MesureEssaiForage)
            'Dim Essai As Essai_Forage = GetEssaiForageByRef(refEssai)
            For Each row As DataRow In ObjetDataMesuresForage.Rows
                If row("refEssai").ToString() = NouvelleMesure.RefEssai And row("indiceMesure").ToString() = NouvelleMesure.IndiceMesure Then
                    row("pression") = NouvelleMesure.Pression
                    row("volume15") = NouvelleMesure.Volume15
                    row("volume30") = NouvelleMesure.Volume30
                    row("volume60") = NouvelleMesure.Volume60
                    row("contrainteRadiale") = NouvelleMesure.ContrainteRadiale
                    row("contrainteVerticale") = NouvelleMesure.ContrainteVerticale
                    row("deformation") = NouvelleMesure.Deformation
                    row("PNF") = NouvelleMesure.PNF
                    row("VNF") = NouvelleMesure.VNF
                    row("Qi") = NouvelleMesure.Qi
                    row("Pi") = NouvelleMesure.Pi
                    row("Qp") = NouvelleMesure.Qp
                    row("Pp") = NouvelleMesure.Pp
                    Exit For
                End If
            Next
        End Sub

    End Class
End Namespace