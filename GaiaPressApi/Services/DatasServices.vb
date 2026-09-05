Namespace GaiaPress.core.V17.Services
    Public Class DataService
        ' Déclaration des DataTables
        Public ObjetDataChantier As New DataTable()
        Public ObjetDataForage As New DataTable()
        Public ObjetDataEssaisForage As New DataTable()
        Public ObjetDataMesuresForage As New DataTable()
        Public ObjetDataSols As New DataTable()
        Public ObjetDataListeSondes As New DataTable()
        Public ObjetDataEssaisResistancePropre As New DataTable()
        Public ObjetDataMesuresResistancePropre As New DataTable()
        Public ObjetDataEssaisExpansionPropre As New DataTable()
        Public ObjetDataMesuresExpansionPropre As New DataTable()

        ' Constructeur
        Public Sub New()
            InitDataTables()
        End Sub

        ' Initialiser les DataTables avec les colonnes nécessaires
        PUBLIC Sub InitDataTables()
            ' ObjetDataChantier
            With ObjetDataChantier
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

            ' ObjetDataForage
            With ObjetDataForage
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
            With ObjetDataEssaisForage
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

            ' ObjetDataListeSondes
            With ObjetDataListeSondes
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

            ' ObjetDataEssaisResistancePropre
            With ObjetDataEssaisResistancePropre
                .Columns.Clear() ' Effacer les colonnes existantes si nécessaire
                .Columns.Add("refEssai", GetType(String))
                .Columns.Add("refSonde", GetType(String))
                .Columns.Add("ClassPrecis", GetType(Integer))
                .Columns.Add("nomEssai", GetType(String))
                .Columns.Add("dateEssai", GetType(Date))
                .Columns.Add("difPresEauAir", GetType(Single))
                .Columns.Add("hauteurCpv", GetType(Single))
                .Columns.Add("A0", GetType(Single))
                .Columns.Add("A1", GetType(Single))
                .Columns.Add("A2", GetType(Single))
                .Columns.Add("A3", GetType(Single))
                .PrimaryKey = New DataColumn() { .Columns("refEssai")}
            End With

            ' ObjetDataMesuresResistancePropre
            With ObjetDataMesuresResistancePropre
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
            Using conn As New SqliteConnection("Data Source=GaiaPress.db")
                conn.Open()

                ' Charger les chantiers
                Using cmd As New SqliteCommand("SELECT * FROM Chantiers WHERE refChantier = @refChantier", conn)
                    cmd.Parameters.AddWithValue("@refChantier", chantierId)
                    Using reader As SqliteDataReader = cmd.ExecuteReader()
                        ObjetDataChantier.Load(reader)
                    End Using
                End Using

                ' Charger les forages
                Using cmd As New SqliteCommand("SELECT * FROM Forages WHERE refChantier = @refChantier", conn)
                    cmd.Parameters.AddWithValue("@refChantier", chantierId)
                    Using reader As SqliteDataReader = cmd.ExecuteReader()
                        ObjetDataForage.Load(reader)
                    End Using
                End Using

                ' Charger les essais
                Using cmd As New SqliteCommand("SELECT * FROM Essais", conn)
                    Using reader As SqliteDataReader = cmd.ExecuteReader()
                        ObjetDataEssai.Load(reader)
                    End Using
                End Using
            End Using
        End Sub

        ''**************************************************************************
        '' Gestion des Chantiers
        ''**************************************************************************
        '' Méthodes pour manipuler les DataTables
        'Public Sub AddChantier(chantier As LesChantiers)
        '    Dim row As DataRow = ObjetDataChantier.NewRow()
        '    row("refChantier") = chantier.C_refChantier
        '    row("nomChantier") = chantier.C_nomChantier
        '    row("descriptifChantier") = chantier.C_descriptifChantier
        '    row("refClient") = chantier.C_refClient
        '    row("entreprise") = chantier.C_entreprise
        '    row("adresse") = chantier.C_adresse
        '    row("telephone") = chantier.C_telephone
        '    row("reference") = chantier.C_reference
        '    ObjetDataChantier.Rows.Add(row)
        'End Sub

        'Public Sub UpdateChantier(chantier As LesChantiers)
        '    For Each row As DataRow In ObjetDataChantier.Rows
        '        If row("refChantier").ToString() = chantier.C_refChantier Then
        '            row("nomChantier") = chantier.C_nomChantier
        '            row("descriptifChantier") = chantier.C_descriptifChantier
        '            row("refClient") = chantier.C_refClient
        '            row("entreprise") = chantier.C_entreprise
        '            row("adresse") = chantier.C_adresse
        '            row("telephone") = chantier.C_telephone
        '            row("reference") = chantier.C_reference
        '            Exit For
        '        End If
        '    Next
        'End Sub

        'Public Sub DeleteChantier(refChantier As String)
        '    Dim rows() As DataRow = ObjetDataChantier.Select("refChantier = '" & refChantier & "'")
        '    If rows.Length > 0 Then
        '        ObjetDataChantier.Rows.Remove(rows(0))
        '    End If
        'End Sub

        'Public Function GetChantierByRef(refChantier As String) As LesChantiers
        '    Dim rows() As DataRow = ObjetDataChantier.Select("refChantier = '" & refChantier & "'")
        '    If rows.Length = 0 Then
        '        Return Nothing
        '    End If
        '    Return New LesChantiers With {
        '        .C_refChantier = rows(0)("refChantier").ToString(),
        '        .C_nomChantier = rows(0)("nomChantier").ToString(),
        '        .C_descriptifChantier = rows(0)("descriptifChantier").ToString(),
        '        .C_refClient = rows(0)("refClient").ToString(),
        '        .C_entreprise = rows(0)("entreprise").ToString(),
        '        .C_adresse = rows(0)("adresse").ToString(),
        '        .C_telephone = rows(0)("telephone").ToString(),
        '        .C_reference = rows(0)("reference").ToString()
        '    }
        'End Function

        '*************************************************************************
        '  Gestion des forages
        '*************************************************************************

        ' Méthodes pour Forage
        Public Sub AddForage(forage As Forage)
            Dim row As DataRow = ObjetDataForage.NewRow()
            row("refChantier") = forage.F_RefChantier
            row("refForage") = forage.F_RefForage
            row("numeroForage") = forage.F_numeroForage
            row("nomForage") = forage.F_NomForage
            row("dateForage") = forage.F_dateForage
            row("materiel") = forage.F_materiel
            row("outil") = forage.F_outil
            row("abscisseX") = forage.F_abscisseX
            row("ordonneeY") = forage.F_ordonneeY
            row("altitudeZ") = forage.F_altitudeZ
            row("coteInit") = forage.F_coteInit
            row("coteFinale") = forage.F_coteFinale
            ObjetDataForage.Rows.Add(row)
        End Sub
        ''' <summary>

        Public Sub UpdateForage(forage As Forage)
            'Dim rows() As DataRow = ObjetDataForage.Select("refForage = '" & forage.F_RefForage & "'")
            For Each row As DataRow In ObjetDataForage.Rows
                If row("refForage").ToString() = forage.F_RefForage Then
                    row("refChantier") = forage.F_RefChantier
                    row("refForage") = forage.F_RefForage
                    row("numeroForage") = forage.F_numeroForage
                    row("nomForage") = forage.F_NomForage
                    row("dateForage") = forage.F_dateForage
                    row("materiel") = forage.F_materiel
                    row("outil") = forage.F_outil
                    row("abscisseX") = forage.F_abscisseX
                    row("ordonneeY") = forage.F_ordonneeY
                    row("altitudeZ") = forage.F_altitudeZ
                    row("coteInit") = forage.F_coteInit
                    row("coteFinale") = forage.F_coteFinale
                    Exit For
                End If
            Next
        End Sub

        Public Sub DeleteForage(refForage As String)
            Dim rows() As DataRow = ObjetDataForage.Select("refForage = '" & refForage & "'")
            If rows.Length > 0 Then
                ObjetDataForage.Rows.Remove(rows(0))
            End If
        End Sub

        Public Function GetForageByRef(refForage As String) As Forage
            Dim rows() As DataRow = ObjetDataForage.Select("refForage = '" & refForage & "'")
            If rows.Length = 0 Then
                Return Nothing
            End If
            Return New Forage With {
                .F_RefChantier = rows(0)("refChantier").ToString(),
                .F_RefForage = rows(0)("refForage").ToString(),
                .F_numeroForage = rows(0)("numeroForage").ToString(),
                .F_NomForage = rows(0)("nomForage").ToString(),
                .F_dateForage = If(rows(0)("dateForage") Is DBNull.Value, Nothing, CDate(rows(0)("dateForage"))),
                .F_materiel = rows(0)("materiel").ToString(),
                .F_outil = rows(0)("outil").ToString(),
                .F_abscisseX = rows(0)("abscisseX").ToString(),
                .F_ordonneeY = rows(0)("ordonneeY").ToString(),
                .F_altitudeZ = rows(0)("altitudeZ").ToString(),
                .F_coteInit = rows(0)("coteInit").ToString(),
                .F_coteFinale = rows(0)("coteFinale").ToString()
            }
        End Function

        '*************************************************************************
        '  Gestion des EssaisForage
        '*************************************************************************

        ' Méthodes pour EssaisForage
        Public Sub AddEssaisForage(essai As Essai_Forage)
            Dim row As DataRow = ObjetDataEssai.NewRow()
            row("refChantier") = essai.E_RefChantier
            row("refForage") = essai.E_RefForage
            row("refEssai") = essai.E_RefEssai
            row("refSonde") = essai.E_refSonde
            row("refEssaiResistancePropre") = essai.E_refEssaiResistancePropre
            row("refEssaiExpansionPropre") = essai.E_refEssaiExpansionPropre
            row("refEssaiPerteDeCharge") = essai.E_refEssaiPerteDeCharge
            row("refSol") = essai.E_RefSol
            row("Enregistreur") = essai.E_Enregistreur
            row("Operateur") = essai.E_Operateur
            row("VolSonde") = essai.E_VolSonde
            row("ClassPrecis") = essai.E_ClassPrecis
            row("PrecisVolSonde") = essai.E_PrecisVolSonde
            row("nomEssai") = essai.E_nomEssai
            row("dateEssai") = essai.E_dateEssai
            row("heureEssai") = essai.E_heureEssai
            row("difPresEauAir") = essai.E_difPresEauAir
            row("hauteurCpv") = essai.E_hauteurCpv
            row("presP0") = essai.E_presP0
            row("presFluage") = essai.E_presFluage
            row("presLimite") = essai.E_presLimite
            row("presLimiteNf") = essai.E_presLimiteNF
            row("presLimiteTheo") = essai.E_presLimiteTheo
            row("PrecisVolume") = essai.E_PrecisVolume
            row("PrecisPression") = essai.E_PrecisPression
            row("poidsVol") = essai.E_poidsVol
            row("poidsVolDejauge") = essai.E_poidsVolDejauge
            row("coefPoisson") = essai.E_coefPoisson
            row("modPressio") = essai.E_modPressio
            row("modPressioNf") = essai.E_modPressioNf
            row("modElastique") = essai.E_modElastique
            row("modElastiqueNf") = essai.E_modElastiqueNf
            row("cohesion") = essai.E_Cohesion
            row("frottement") = essai.E_Frottement
            row("frottementintergranulaire") = essai.E_FrottementIntergranulaire
            row("U0") = essai.E_U0
            row("profNappe") = essai.E_profNappe
            row("profSonde") = essai.E_profSonde
            row("coteZeroGeomeca") = essai.E_coteZeroGeomeca
            ObjetDataEssai.Rows.Add(row)

        End Sub


        ' Mettre à jour un EssaisForage

        Public Sub UpDateEssaisForage(NouvelEssai As Essai_Forage)
            'Dim Essai As Essai_Forage = GetEssaiForageByRef(refEssai)
            For Each row As DataRow In ObjetDataEssaisForage.Rows
                If row("refEssai").ToString() = NouvelEssai.E_RefEssai Then
                    row("refChantier") = NouvelEssai.E_RefChantier
                    row("RefForage") = NouvelEssai.E_RefForage
                    row("refEssai") = NouvelEssai.E_RefEssai
                    row("refSonde") = NouvelEssai.E_refSonde
                    row("refEssaiResistancePropre") = NouvelEssai.E_refEssaiResistancePropre
                    row("refEssaiExpansionPropre") = NouvelEssai.E_refEssaiExpansionPropre
                    row("refEssaiPerteDeCharge") = NouvelEssai.E_refEssaiPerteDeCharge
                    row("refSol") = NouvelEssai.E_RefSol
                    row("Enregistreur") = NouvelEssai.E_Enregistreur
                    row("Operateur") = NouvelEssai.E_Operateur
                    row("VolSonde") = NouvelEssai.E_VolSonde
                    row("ClassPrecis") = NouvelEssai.E_ClassPrecis
                    row("PrecisVolSonde") = NouvelEssai.E_PrecisVolSonde
                    row("nomEssai") = NouvelEssai.E_nomEssai
                    row("dateEssai") = NouvelEssai.E_dateEssai
                    row("heureEssai") = NouvelEssai.E_heureEssai
                    row("difPresEauAir") = NouvelEssai.E_difPresEauAir
                    row("hauteurCpv") = NouvelEssai.E_hauteurCpv
                    row("presP0") = NouvelEssai.E_presP0
                    row("presFluage") = NouvelEssai.E_presFluage
                    row("presLimite") = NouvelEssai.E_presLimite
                    row("presLimiteNf") = NouvelEssai.E_presLimiteNF
                    row("presLimiteTheo") = NouvelEssai.E_presLimiteTheo
                    row("PrecisVolume") = NouvelEssai.E_PrecisVolume
                    row("PrecisPression") = NouvelEssai.E_PrecisPression
                    row("poidsVol") = NouvelEssai.E_poidsVol
                    row("poidsVolDejauge") = NouvelEssai.E_poidsVolDejauge
                    row("coefPoisson") = NouvelEssai.E_coefPoisson
                    row("modPressio") = NouvelEssai.E_modPressio
                    row("modPressioNf") = NouvelEssai.E_modPressioNf
                    row("modElastique") = NouvelEssai.E_modElastique
                    row("modElastiqueNf") = NouvelEssai.E_modElastiqueNf
                    row("cohesion") = NouvelEssai.E_Cohesion
                    row("frottement") = NouvelEssai.E_Frottement
                    row("frottementintergranulaire") = NouvelEssai.E_FrottementIntergranulaire
                    row("U0") = NouvelEssai.E_U0
                    row("profNappe") = NouvelEssai.E_profNappe
                    row("profSonde") = NouvelEssai.E_profSonde
                    row("coteZeroGeomeca") = NouvelEssai.E_coteZeroGeomeca
                    Exit For
                End If
            Next
        End Sub

        '     Public Function GetEssaiForageByRef(refEssai As String) As Essai_Forage
        '         Dim rows() As DataRow = ObjetDataEssai.Select("refEssai = '" & refEssai & "'")
        '         If rows.Length = 0 Then
        '             Return Nothing
        '         End If
        '         Return New Essai_Forage With {
        '             .E_RefChantier = rows(0)("refChantier").ToString(),
        '             .E_RefForage = rows(0)("refForage").ToString(),
        '             .E_RefEssai = rows(0)("refEssai").ToString(),
        '             .E_refSonde = rows(0)("refSonde").ToString(),
        '             .E_refEssaiResistancePropre = rows(0)("refEssaiResistancePropre").ToString(),
        '             .E_refEssaiExpansionPropre = rows(0)("refEssaiExpansionPropre").ToString(),
        '             .E_refEssaiPerteDeCharge = rows(0)("refEssaiPerteDeCharge").ToString(),
        '             .E_RefSol = rows(0)("refSol").ToString(),
        '             .E_Enregistreur = rows(0)("Enregistreur").ToString(),
        '             .E_Operateur = rows(0)("Operateur").ToString(),
        '             .E_VolSonde = rows(0)("VolSonde").ToString(),
        '             .E_ClassPrecis = rows(0)("ClassPrecis").ToString(),
        '             .E_PrecisVolSonde = rows(0)("PrecisVolSonde").ToString(),
        '             .E_nomEssai = rows(0)("nomEssai").ToString(),
        '             .E_dateEssai = If(rows(0)("dateEssai") Is DBNull.Value, Nothing, CDate(rows(0)("dateEssai"))),
        '             .E_heureEssai = If(rows(0)("heureEssai") Is DBNull.Value, Nothing, CDate(rows(0)("heureEssai"))),
        '             .E_difPresEauAir = CDbl(rows(0)("difPresEauAir")),
        '             .E_hauteurCpv = CDbl(rows(0)("hauteurCpv")),
        '             .E_presP0 = CDbl(rows(0)("presP0")),
        '             .E_presFluage = CDbl(rows(0)("presFluage")),
        '         .E_presLimite = CDbl(rows(0)("presLimite")),
        '         .E_presLimiteNF = CDbl(rows(0)("presLimiteNf")),
        '         .E_presLimiteTheo = CDbl(rows(0)("presLimiteTheo")),
        '         .E_PrecisVolume = CDbl(rows(0)("PrecisVolume")),
        '         .E_PrecisPression = CDbl(rows(0)("PrecisPression")),
        '         .E_poidsVol = CDbl(rows(0)("poidsVol")),
        '         .E_poidsVolDejauge = CDbl(rows(0)("poidsVolDejauge")),
        '         .E_coefPoisson = CDbl(rows(0)("coefPoisson")),
        '         .E_modPressio = CDbl(rows(0)("modPressio")),
        '         .E_modPressioNf = CDbl(rows(0)("modPressioNf")),
        '         .E_modElastique = CDbl(rows(0)("modElastique")),
        '         .E_modElastiqueNf = CDbl(rows(0)("modElastiqueNf")),
        '         .E_Cohesion = CDbl(rows(0)("cohesion")),
        '         .E_Frottement = CDbl(rows(0)("frottement")),
        '         .E_FrottementIntergranulaire = CDbl(rows(0)("frottementintergranulaire")),
        '         .E_U0 = CDbl(rows(0)("U0")),
        '         .E_profNappe = CDbl(rows(0)("profNappe")),
        '         .E_profSonde = CDbl(rows(0)("profSonde")),
        '         .E_coteZeroGeomeca = CDbl(rows(0)("coteZeroGeomeca"))
        '}
        '     End Function

        ' Mettre à jour un EssaisForage
        Public Sub MettreAJourEssaisForage(refEssai As String, nouveauNom As String, nouvelleDate As String)
            Dim Essai As Essai_Forage = GetEssaiForage(refEssai)
            If Essai IsNot Nothing Then
                Essai.E_nomEssai = nouveauNom
                Essai.E_dateEssai = nouvelleDate
            End If
        End Sub

        Public Function GetForagesByChantier(chantierId As String) As List(Of Forage)
            Try
                Dim forages As New List(Of Forage)
                Dim rows() As DataRow = ObjetDataForage.Select("$refChantier = '" & chantierId & "'")

                For Each row As DataRow In rows
                    Dim forage As New Forage With {
                .F_RefForage = If(row("refForage") Is DBNull.Value, String.Empty, row("refForage").ToString()),
                .F_NomForage = If(row("nomForage") Is DBNull.Value, String.Empty, row("nomForage").ToString()),
                .F_dateForage = If(row("dateForage") Is DBNull.Value, Nothing, CDate(row("dateForage")))
            }
                    forages.Add(forage)
                Next

                Return forages
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
            Dim rows() As DataRow = ObjetDataEssaisForage.Select($"refEssai = '{refEssai}'")

            ' Si aucune ligne n'est trouvée, retourner Nothing
            If rows.Length = 0 Then
                Return Nothing
            End If

            ' Créer et retourner un nouvel objet Essai_Forage avec les valeurs de la DataRow
            Return New Essai_Forage With {
            .E_RefChantier = If(rows(0).IsNull("RefChantier"), String.Empty, rows(0).Field(Of String)("RefChantier")),
        .E_RefForage = If(rows(0).IsNull("refForage"), String.Empty, rows(0).Field(Of String)("refForage")),
        .E_RefEssai = If(rows(0).IsNull("refEssai"), String.Empty, rows(0).Field(Of String)("refEssai")),
        .E_refSonde = If(rows(0).IsNull("refSonde"), String.Empty, rows(0).Field(Of String)("refSonde")),
        .E_refEssaiResistancePropre = If(rows(0).IsNull("refEssaiResistancePropre"), String.Empty, rows(0).Field(Of String)("refEssaiResistancePropre")),
        .E_refEssaiExpansionPropre = If(rows(0).IsNull("refEssaiExpansionPropre"), String.Empty, rows(0).Field(Of String)("refEssaiExpansionPropre")),
        .E_refEssaiPerteDeCharge = If(rows(0).IsNull("refEssaiPerteDeCharge"), String.Empty, rows(0).Field(Of String)("refEssaiPerteDeCharge")),
        .E_RefSol = If(rows(0).IsNull("refSol"), String.Empty, rows(0).Field(Of String)("refSol")),
        .E_Enregistreur = If(rows(0).IsNull("Enregistreur"), String.Empty, rows(0).Field(Of String)("Enregistreur")),
        .E_Operateur = If(rows(0).IsNull("Operateur"), String.Empty, rows(0).Field(Of String)("Operateur")),
        .E_VolSonde = If(rows(0).IsNull("VolSonde"), 0, CSng(rows(0).Field(Of Single)("VolSonde"))),
        .E_nomEssai = If(rows(0).IsNull("nomEssai"), String.Empty, rows(0).Field(Of String)("nomEssai")),
        .E_ClassPrecis = If(rows(0).IsNull("ClassPrecis"), 0, CInt(rows(0).Field(Of Integer)("ClassPrecis"))),
        .E_PrecisVolSonde = If(rows(0).IsNull("PrecisVolSonde"), 0, CSng(rows(0).Field(Of Single)("PrecisVolSonde"))),
            .E_dateEssai = If(rows(0).IsNull("dateEssai"), Nothing, rows(0).Field(Of Date)("dateEssai")),
        .E_heureEssai = If(rows(0).IsNull("heureEssai"), Nothing, rows(0).Field(Of DateTime)("heureEssai")),
            .E_difPresEauAir = If(rows(0).IsNull("difPresEauAir"), 0, CSng(rows(0).Field(Of Single)("difPresEauAir"))),
        .E_hauteurCpv = If(rows(0).IsNull("hauteurCpv"), 0, CSng(rows(0).Field(Of Single)("hauteurCpv"))),
        .E_presP0 = If(rows(0).IsNull("presP0"), 0, CSng(rows(0).Field(Of Single)("presP0"))),
        .E_presFluage = If(rows(0).IsNull("presFluage"), 0, CSng(rows(0).Field(Of Single)("presFluage"))),
        .E_presLimite = If(rows(0).IsNull("presLimite"), 0, CSng(rows(0).Field(Of Single)("presLimite"))),
        .E_presLimiteNF = If(rows(0).IsNull("presLimiteNf"), 0, CSng(rows(0).Field(Of Single)("presLimiteNf"))),
        .E_presLimiteTheo = If(rows(0).IsNull("presLimiteTheo"), 0, CSng(rows(0).Field(Of Single)("presLimiteTheo"))),
        .E_PrecisVolume = If(rows(0).IsNull("PrecisVolume"), 0, CSng(rows(0).Field(Of Single)("PrecisVolume"))),
        .E_PrecisPression = If(rows(0).IsNull("PrecisPression"), 0, CSng(rows(0).Field(Of Single)("PrecisPression"))),
        .E_poidsVol = If(rows(0).IsNull("poidsVol"), 0, CSng(rows(0).Field(Of Single)("poidsVol"))),
        .E_poidsVolDejauge = If(rows(0).IsNull("poidsVolDejauge"), 0, CSng(rows(0).Field(Of Single)("poidsVolDejauge"))),
        .E_coefPoisson = If(rows(0).IsNull("coefPoisson"), 0, CSng(rows(0).Field(Of Single)("coefPoisson"))),
        .E_modPressio = If(rows(0).IsNull("modPressio"), 0, CSng(rows(0).Field(Of Single)("modPressio"))),
        .E_modPressioNf = If(rows(0).IsNull("modPressioNf"), 0, CSng(rows(0).Field(Of Single)("modPressioNf"))),
        .E_modElastique = If(rows(0).IsNull("modElastique"), 0, CSng(rows(0).Field(Of Single)("modElastique"))),
        .E_modElastiqueNf = If(rows(0).IsNull("modElastiqueNf"), 0, CSng(rows(0).Field(Of Single)("modElastiqueNf"))),
        .E_Cohesion = If(rows(0).IsNull("cohesion"), 0, CSng(rows(0).Field(Of Single)("cohesion"))),
        .E_Frottement = If(rows(0).IsNull("frottement"), 0, CSng(rows(0).Field(Of Single)("frottement"))),
        .E_FrottementIntergranulaire = If(rows(0).IsNull("frottementIntergranulaire"), 0, CSng(rows(0).Field(Of Single)("frottementIntergranulaire"))),
        .E_U0 = If(rows(0).IsNull("U0"), 0, CSng(rows(0).Field(Of Single)("U0"))),
        .E_profNappe = If(rows(0).IsNull("profNappe"), 0, CSng(rows(0).Field(Of Single)("profNappe"))),
        .E_profSonde = If(rows(0).IsNull("profSonde"), 0, CSng(rows(0).Field(Of Single)("profSonde"))),
        .E_coteZeroGeomeca = If(rows(0).IsNull("coteZeroGeomeca"), 0, CSng(rows(0).Field(Of Single)("coteZeroGeomeca")))
    }
        End Function
        Public Function GetEssaiForage(refEssai As String) As Essai_Forage
            For Each E As Essai_Forage In MyForage.F_ListEssais
                If E.E_RefEssai = refEssai Then
                    Return E
                End If
            Next
            Return Nothing
        End Function

        ' Delete un EssaisForage par référence
        Public Sub DeleteEssaisForage(refEssai As String)
            Dim EssaiADelete As Essai_Forage = GetEssaiForage(refEssai)
            If EssaiADelete IsNot Nothing Then
                MyForage.F_ListEssais.Remove(EssaiADelete)
            End If
        End Sub


        ' ********************************************************************
        '     Gestion des MesuresEssaisForage
        ' ********************************************************************

        ' Méthodes pour Mesure
        Public Sub AddMesureEssaisForage(mesure As MesuresEssaiForage)
            Dim row As DataRow = ObjetDataMesuresForage.NewRow()
            row("ind") = mesure.RefEssai
            row("refEssai") = mesure.RefEssai
            row("indiceMesure") = mesure.indiceMesure
            row("pression") = mesure.pression
            row("volume15") = mesure.volume15
            row("volume30") = mesure.volume30
            row("volume60") = mesure.volume60
            row("contrainteRadiale") = mesure.contrainteRadiale
            row("contrainteVerticale") = mesure.contrainteVerticale
            row("deformation") = mesure.deformation
            row("PNF") = mesure.PNF
            row("VNF") = mesure.VNF
            row("Qi") = mesure.Qi
            row("Pi") = mesure.Pi
            row("Qp") = mesure.Qp
            row("Pp") = mesure.Pp
            ObjetDataMesuresForage.Rows.Add(row)
        End Sub

        'Public Function GetMesuresByEssai(refEssai As String) As DbSet((Of MesuresEssaiForage)
        Public Function GetMesuresByEssai(refEssai As String) As List(Of MesuresEssaiForage)
            Dim mesures As New List(Of MesuresEssaiForage)
            Dim rows() As DataRow = ObjetDataMesuresForage.Select("refEssai = '" & refEssai & "'")
            For Each row As DataRow In rows
                mesures.Add(New MesuresEssaiForage With {
                    .RefEssai = row("refEssai").ToString(),
                    .indiceMesure = CInt(row("indiceMesure")),
                    .pression = CDbl(row("pression")),
                    .volume15 = CDbl(row("volume15")),
                    .volume30 = CDbl(row("volume30")),
                    .volume60 = CDbl(row("volume")),
                    .contrainteRadiale = CDbl(row("contrainteRadiale")),
                    .contrainteVerticale = CDbl(row("contrainteVerticale")),
                    .deformation = CDbl(row("deformation")),
                    .PNF = CDbl(row("PNF")),
                    .VNF = CDbl(row("VNF")),
                    .Qi = CDbl(row("Qi")),
                    .Pi = CDbl(row("Pi")),
                    .Qp = CDbl(row("Qp"))
                })
            Next
            Return mesures
        End Function

        Public Sub UpDateMesureEssaisForage(NouvelleMesure As MesuresEssaiForage)
            'Dim Essai As Essai_Forage = GetEssaiForageByRef(refEssai)
            For Each row As DataRow In ObjetDataMesuresForage.Rows
                If row("refEssai").ToString() = NouvelleMesure.RefEssai And row("indiceMesure").ToString() = NouvelleMesure.indiceMesure Then
                    row("pression") = NouvelleMesure.pression
                    row("volume15") = NouvelleMesure.volume15
                    row("volume30") = NouvelleMesure.volume30
                    row("volume60") = NouvelleMesure.volume60
                    row("contrainteRadiale") = NouvelleMesure.contrainteRadiale
                    row("contrainteVerticale") = NouvelleMesure.contrainteVerticale
                    row("deformation") = NouvelleMesure.deformation
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