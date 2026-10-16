Imports System
Imports System.Data
Imports GaiaPressCoreV17.Models
Imports Microsoft.Data.SqlClient

Namespace GaiaPressCoreV17.Services
    Public Class DatabaseWriteService

        Private ReadOnly _connectionString As String

        Public Sub New()
            _connectionString = DatabaseConnection.ConnectionString
        End Sub

        ''' <summary>
        ''' Insère une ligne dans une table (générique)
        ''' </summary>
        Public Function InsertRow(tableName As String, values As Dictionary(Of String, Object)) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim columns As String = String.Join(", ",
                        values.Keys.Select(Function(k) $"[{k}]"))
                    Dim parameters As String = String.Join(", ",
                        values.Keys.Select(Function(k) $"@{k}"))

                    Dim query As String = $"INSERT INTO [{tableName}] ({columns}) VALUES ({parameters})"
                    Dim command As New SqlCommand(query, connection)

                    For Each kvp In values
                        command.Parameters.AddWithValue($"@{kvp.Key}", If(kvp.Value, DBNull.Value))
                    Next
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur insertion {tableName} : {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Met à jour une ligne dans une table (clé String)
        ''' </summary>
        Public Function UpdateRowByRef(tableName As String, idColumnName As String, id As String,
                                       values As Dictionary(Of String, Object)) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim setClause As String = String.Join(", ",
                        values.Keys.Select(Function(k) $"[{k}] = @{k}"))

                    Dim query As String = $"UPDATE [{tableName}] SET {setClause} WHERE [{idColumnName}] = @ID"
                    Dim command As New SqlCommand(query, connection)

                    For Each kvp In values
                        command.Parameters.AddWithValue($"@{kvp.Key}", If(kvp.Value, DBNull.Value))
                    Next
                    command.Parameters.AddWithValue("@ID", id)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur mise à jour {tableName} : {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Met à jour une ligne dans une table (clé Integer)
        ''' </summary>
        Public Function UpdateRow(tableName As String, idColumnName As String, id As Integer,
                                 values As Dictionary(Of String, Object)) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim setClause As String = String.Join(", ",
                        values.Keys.Select(Function(k) $"[{k}] = @{k}"))

                    Dim query As String = $"UPDATE [{tableName}] SET {setClause} WHERE [{idColumnName}] = @ID"
                    Dim command As New SqlCommand(query, connection)

                    For Each kvp In values
                        command.Parameters.AddWithValue($"@{kvp.Key}", If(kvp.Value, DBNull.Value))
                    Next
                    command.Parameters.AddWithValue("@ID", id)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur mise à jour {tableName} : {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Supprime une ligne d'une table (clé String)
        ''' </summary>
        Public Function DeleteRowByRef(tableName As String, idColumnName As String, id As String) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = $"DELETE FROM [{tableName}] WHERE [{idColumnName}] = @ID"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ID", id)

                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur suppression {tableName} : {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Supprime une ligne d'une table (clé Integer)
        ''' </summary>
        Public Function DeleteRow(tableName As String, idColumnName As String, id As Integer) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = $"DELETE FROM [{tableName}] WHERE [{idColumnName}] = @ID"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@ID", id)

                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur suppression {tableName} : {ex.Message}")
                Return False
            End Try
        End Function

        ' ============================================
        ' MÉTHODES SPÉCIFIQUES - Chantier
        ' ============================================

        ''' <summary>
        ''' Ajoute un nouveau chantier
        ''' </summary>
        Public Function InsertChantier(refChantier As String, nomChantier As String, descriptifChantier As String,
                                       refClient As String, entreprise As String, adresse As String,
                                       telephone As String, reference As String) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refChantier", refChantier},
                {"nomChantier", nomChantier},
                {"descriptifChantier", descriptifChantier},
                {"refClient", refClient},
                {"entreprise", entreprise},
                {"adresse", adresse},
                {"telephone", telephone},
                {"reference", reference}
            }
            Return InsertRow("LesChantier", values)
        End Function

        ''' <summary>
        ''' Met à jour un chantier
        ''' </summary>
        Public Function UpdateChantier(refChantier As String, nomChantier As String, descriptifChantier As String,
                                       refClient As String, entreprise As String, adresse As String,
                                       telephone As String, reference As String) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"nomChantier", nomChantier},
                {"descriptifChantier", descriptifChantier},
                {"refClient", refClient},
                {"entreprise", entreprise},
                {"adresse", adresse},
                {"telephone", telephone},
                {"reference", reference}
            }
            Return UpdateRowByRef("LesChantier", "refChantier", refChantier, values)
        End Function

        ''' <summary>
        ''' Supprime un chantier
        ''' </summary>
        Public Function DeleteChantier(refChantier As String) As Boolean
            Return DeleteRowByRef("LesChantier", "refChantier", refChantier)
        End Function

        ' ============================================
        ' MÉTHODES SPÉCIFIQUES - FORAGES
        ' ============================================

        ''' <summary>
        ''' Ajoute un nouveau forage
        ''' </summary>
        Public Function InsertForage(refForage As String, refChantier As String, nomForage As String,
                                     profondeur As Single, description As String) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refForage", refForage},
                {"refChantier", refChantier},
                {"nomForage", nomForage},
                {"profondeur", profondeur},
                {"description", description}
            }
            Return InsertRow("LesForages", values)
        End Function

        ''' <summary>
        ''' Met à jour un forage
        ''' </summary>
        Public Function UpdateForage(refForage As String, nomForage As String, profondeur As Single,
                                     description As String) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"nomForage", nomForage},
                {"profondeur", profondeur},
                {"description", description}
            }
            Return UpdateRowByRef("LesForages", "refForage", refForage, values)
        End Function

        ''' <summary>
        ''' Supprime un forage
        ''' </summary>
        Public Function DeleteForage(refForage As String) As Boolean
            Return DeleteRowByRef("LesForages", "refForage", refForage)
        End Function

        ' ============================================
        ' ESSAIS DE FORAGE - MÉTHODES SPÉCIFIQUES
        ' ============================================

        ''' <summary>
        ''' Ajoute un nouvel essai de forage
        ''' </summary>
        Public Function InsertEssaiForage(refEssai As String, refChantier As String, refForage As String,
                                          nomEssai As String, dateEssai As Date, heureEssai As DateTime,
                                          operateur As String, enregistreur As String) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refEssai", refEssai},
                {"refChantier", refChantier},
                {"refForage", refForage},
                {"nomEssai", nomEssai},
                {"dateEssai", dateEssai},
                {"heureEssai", heureEssai},
                {"operateur", operateur},
                {"enregistreur", enregistreur}
            }
            Return InsertRow("LesEssaiForages", values)
        End Function

        ''' <summary>
        ''' Met à jour un essai de forage
        ''' </summary>
        Public Function UpdateEssaiForage(refEssai As String, nomEssai As String, dateEssai As Date,
                                          heureEssai As DateTime, operateur As String) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"nomEssai", nomEssai},
                {"dateEssai", dateEssai},
                {"heureEssai", heureEssai},
                {"operateur", operateur}
            }
            Return UpdateRowByRef("LesEssaiForages", "refEssai", refEssai, values)
        End Function

        ''' <summary>
        ''' Supprime un essai de forage
        ''' </summary>
        Public Function DeleteEssaiForage(refEssai As String) As Boolean
            Return DeleteRowByRef("LesEssaiForages", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Ajoute une mesure de forage
        ''' </summary>
        Public Function InsertMesureForage(refEssai As String, indiceMesure As Integer, pression As Single,
                                           volume15 As Single, volume30 As Single, volume60 As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refEssai", refEssai},
                {"indiceMesure", indiceMesure},
                {"pression", pression},
                {"volume15", volume15},
                {"volume30", volume30},
                {"volume60", volume60}
            }
            Return InsertRow("LesMesuresForages", values)
        End Function

        ''' <summary>
        ''' Met à jour une mesure de forage
        ''' </summary>
        Public Function UpdateMesureForage(refEssai As String, indiceMesure As Integer, pression As Single,
                                           volume15 As Single, volume30 As Single, volume60 As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"pression", pression},
                {"volume15", volume15},
                {"volume30", volume30},
                {"volume", volume60}
            }
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "UPDATE [LesMesuresForages] SET [pression]=@pression, [volume15]=@volume15, [volume30]=@volume30, [volume]=@volume WHERE [refEssai]=@refEssai AND [indiceMesure]=@indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)
                    command.Parameters.AddWithValue("@pression", pression)
                    command.Parameters.AddWithValue("@volume15", volume15)
                    command.Parameters.AddWithValue("@volume30", volume30)
                    command.Parameters.AddWithValue("@volume", volume60)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur mise à jour mesure forage : {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Supprime une mesure de forage
        ''' </summary>
        Public Function DeleteMesureForage(refEssai As String, indiceMesure As Integer) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "DELETE FROM [LesMesuresForages] WHERE [refEssai]=@refEssai AND [indiceMesure]=@indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur suppression mesure forage : {ex.Message}")
                Return False
            End Try
        End Function

        ' ============================================
        ' MÉTHODES SPÉCIFIQUES - SONDES
        ' ============================================

        ''' <summary>
        ''' Ajoute une nouvelle sonde
        ''' </summary>
        Public Function InsertSonde(refSonde As String, nomSonde As String, typeSonde As String,
                                    volSonde As Single, precisVolSonde As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refSonde", refSonde},
                {"nomSonde", nomSonde},
                {"typeSonde", typeSonde},
                {"volSonde", volSonde},
                {"precisVolSonde", precisVolSonde}
            }
            Return InsertRow("LesSondes", values)
        End Function

        ''' <summary>
        ''' Met à jour une sonde
        ''' </summary>
        Public Function UpdateSonde(refSonde As String, nomSonde As String, typeSonde As String,
                                    volSonde As Single, precisVolSonde As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"nomSonde", nomSonde},
                {"typeSonde", typeSonde},
                {"volSonde", volSonde},
                {"precisVolSonde", precisVolSonde}
            }
            Return UpdateRowByRef("LesSondes", "refSonde", refSonde, values)
        End Function

        ''' <summary>
        ''' Supprime une sonde
        ''' </summary>
        Public Function DeleteSonde(refSonde As String) As Boolean
            Return DeleteRowByRef("LesSondes", "refSonde", refSonde)
        End Function

        ' ============================================
        ' ESSAIS D'EXPANSION - MÉTHODES SPÉCIFIQUES
        ' ============================================

        ''' <summary>
        ''' Ajoute un nouvel essai d'expansion
        ''' </summary>
        Public Function InsertEssaiExpansion(refEssai As String, refSonde As String, nomEssai As String,
                                             dateEssai As Date, heureEssai As DateTime, classPrecis As Integer,
                                             difPresEauAir As Single, hauteurCpv As Single,
                                             a0 As Single, a1 As Single, a2 As Single, a3 As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refEssai", refEssai},
                {"refSonde", refSonde},
                {"nomEssai", nomEssai},
                {"dateEssai", dateEssai},
                {"heureEssai", heureEssai},
                {"ClassPrecis", classPrecis},
                {"difPresEauAir", difPresEauAir},
                {"hauteurCpv", hauteurCpv},
                {"A0", a0},
                {"A1", a1},
                {"A2", a2},
                {"A3", a3}
            }
            Return InsertRow("LesEssaisExpansionPropre", values)
        End Function

        ''' <summary>
        ''' Met à jour un essai d'expansion
        ''' </summary>
        Public Function UpdateEssaiExpansion(refEssai As String, nomEssai As String, dateEssai As Date,
                                             modElastique As Single, modElastiqueNf As Single,
                                             coefPoisson As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"nomEssai", nomEssai},
                {"dateEssai", dateEssai},
                {"modElastique", modElastique},
                {"modElastiqueNf", modElastiqueNf},
                {"coefPoisson", coefPoisson}
            }
            Return UpdateRowByRef("LesEssaisExpansionPropre", "refEssai", refEssai, values)
        End Function

        ''' <summary>
        ''' Supprime un essai d'expansion
        ''' </summary>
        Public Function DeleteEssaiExpansion(refEssai As String) As Boolean
            Return DeleteRowByRef("LesEssaisExpansionPropre", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Ajoute une mesure d'expansion
        ''' </summary>
        Public Function InsertMesureExpansion(refEssai As String, indiceMesure As Integer,
                                              pression As Single, volume As Single, expansion As Single,
                                              deformation As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refEssai", refEssai},
                {"indiceMesure", indiceMesure},
                {"pression", pression},
                {"volume", volume},
                {"expansion", expansion},
                {"deformation", deformation}
            }
            Return InsertRow("LesMesureExpansionPropre", values)
        End Function

        ''' <summary>
        ''' Met à jour une mesure d'expansion
        ''' </summary>
        Public Function UpdateMesureExpansion(refEssai As String, indiceMesure As Integer,
                                              pression As Single, volume As Single, expansion As Single,
                                              deformation As Single) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "UPDATE [LesMesureExpansionPropre] SET [pression]=@pression, [volume]=@volume, [expansion]=@expansion, [deformation]=@deformation WHERE [refEssai]=@refEssai AND [indiceMesure]=@indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)
                    command.Parameters.AddWithValue("@pression", pression)
                    command.Parameters.AddWithValue("@volume", volume)
                    command.Parameters.AddWithValue("@expansion", expansion)
                    command.Parameters.AddWithValue("@deformation", deformation)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur mise à jour mesure expansion : {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Supprime une mesure d'expansion
        ''' </summary>
        Public Function DeleteMesureExpansion(refEssai As String, indiceMesure As Integer) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "DELETE FROM [LesMesureExpansionPropre] WHERE [refEssai]=@refEssai AND [indiceMesure]=@indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur suppression mesure expansion : {ex.Message}")
                Return False
            End Try
        End Function

        ' ============================================
        ' ESSAIS DE RÉSISTANCE - MÉTHODES SPÉCIFIQUES
        ' ============================================

        ''' <summary>
        ''' Ajoute un nouvel essai de résistance
        ''' </summary>
        Public Function InsertEssaiResistance(refEssai As String, refSonde As String, nomEssai As String,
                                              dateEssai As Date, heureEssai As DateTime, classPrecis As Integer,
                                              difPresEauAir As Single, hauteurCpv As Single,
                                              a0 As Single, a1 As Single, a2 As Single, a3 As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refEssai", refEssai},
                {"refSonde", refSonde},
                {"nomEssai", nomEssai},
                {"dateEssai", dateEssai},
                {"heureEssai", heureEssai},
                {"ClassPrecis", classPrecis},
                {"difPresEauAir", difPresEauAir},
                {"hauteurCpv", hauteurCpv},
                {"a0", a0},
                {"a1", a1},
                {"a2", a2},
                {"a3", a3}
            }
            Return InsertRow("LesEssaisResistanceDbPropre", values)
        End Function

        ''' <summary>
        ''' Met à jour un essai de résistance
        ''' </summary>
        Public Function UpdateEssaiResistance(refEssai As String, nomEssai As String, dateEssai As Date,
                                              modElastique As Single, coefPoisson As Single,
                                              cohesion As Single, frottement As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"nomEssai", nomEssai},
                {"dateEssai", dateEssai},
                {"modElastique", modElastique},
                {"coefPoisson", coefPoisson},
                {"cohesion", cohesion},
                {"frottement", frottement}
            }
            Return UpdateRowByRef("LesEssaisResistanceDbPropre", "refEssai", refEssai, values)
        End Function

        ''' <summary>
        ''' Supprime un essai de résistance
        ''' </summary>
        Public Function DeleteEssaiResistance(refEssai As String) As Boolean
            Return DeleteRowByRef("LesEssaisResistanceDbPropre", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Ajoute une mesure de résistance
        ''' </summary>
        Public Function InsertMesureResistance(refEssai As String, indiceMesure As Integer,
                                               pression As Single, volume As Single,
                                               contrainteRadiale As Single, contrainteVerticale As Single,
                                               deformation As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refEssai", refEssai},
                {"indiceMesure", indiceMesure},
                {"pression", pression},
                {"volume", volume},
                {"contrainteRadiale", contrainteRadiale},
                {"contrainteVerticale", contrainteVerticale},
                {"deformation", deformation}
            }
            Return InsertRow("LesMesuresResistanceDbPropre", values)
        End Function

        ''' <summary>
        ''' Met à jour une mesure de résistance
        ''' </summary>
        Public Function UpdateMesureResistance(refEssai As String, indiceMesure As Integer,
                                               pression As Single, volume As Single,
                                               contrainteRadiale As Single, contrainteVerticale As Single,
                                               deformation As Single) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "UPDATE [LesMesuresResistanceDbPropre] SET [pression]=@pression, [volume]=@volume, [contrainteRadiale]=@contrainteRadiale, [contrainteVerticale]=@contrainteVerticale, [deformation]=@deformation WHERE [refEssai]=@refEssai AND [indiceMesure]=@indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)
                    command.Parameters.AddWithValue("@pression", pression)
                    command.Parameters.AddWithValue("@volume", volume)
                    command.Parameters.AddWithValue("@contrainteRadiale", contrainteRadiale)
                    command.Parameters.AddWithValue("@contrainteVerticale", contrainteVerticale)
                    command.Parameters.AddWithValue("@deformation", deformation)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur mise à jour mesure résistance : {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Supprime une mesure de résistance
        ''' </summary>
        Public Function DeleteMesureResistance(refEssai As String, indiceMesure As Integer) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "DELETE FROM [LesMesuresResistanceDbPropre] WHERE [refEssai]=@refEssai AND [indiceMesure]=@indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur suppression mesure résistance : {ex.Message}")
                Return False
            End Try
        End Function

        ' ============================================
        ' ESSAIS DE PERTE DE CHARGE - MÉTHODES SPÉCIFIQUES
        ' ============================================

        ''' <summary>
        ''' Ajoute un nouvel essai de perte de charge
        ''' </summary>
        Public Function InsertEssaiPerteDeCharge(refEssai As String, refSonde As String, nomEssai As String,
                                                 dateEssai As Date, heureEssai As DateTime,
                                                 difPresEauAir As Single, hauteurCpv As Single,
                                                 permeabilite As Single, classPrecis As Integer) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refEssai", refEssai},
                {"refSonde", refSonde},
                {"nomEssai", nomEssai},
                {"dateEssai", dateEssai},
                {"heureEssai", heureEssai},
                {"difPresEauAir", difPresEauAir},
                {"hauteurCpv", hauteurCpv},
                {"permeabilite", permeabilite},
                {"ClassPrecis", classPrecis}
            }
            Return InsertRow("LesEssaisPerteDeCharge", values)
        End Function

        ''' <summary>
        ''' Met à jour un essai de perte de charge
        ''' </summary>
        Public Function UpdateEssaiPerteDeCharge(refEssai As String, nomEssai As String, dateEssai As Date,
                                                 permeabilite As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"nomEssai", nomEssai},
                {"dateEssai", dateEssai},
                {"permeabilite", permeabilite}
            }
            Return UpdateRowByRef("LesEssaisPerteDeCharge", "refEssai", refEssai, values)
        End Function

        ''' <summary>
        ''' Supprime un essai de perte de charge
        ''' </summary>
        Public Function DeleteEssaiPerteDeCharge(refEssai As String) As Boolean
            Return DeleteRowByRef("LesEssaisPerteDeCharge", "refEssai", refEssai)
        End Function

        ''' <summary>
        ''' Ajoute une mesure de perte de charge
        ''' </summary>
        Public Function InsertMesurePerteDeCharge(refEssai As String, indiceMesure As Integer,
                                                  debit As Single, perteCharge As Single,
                                                  gradient As Single) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refEssai", refEssai},
                {"indiceMesure", indiceMesure},
                {"debit", debit},
                {"perteCharge", perteCharge},
                {"gradient", gradient}
            }
            Return InsertRow("LesMesuresPerteDeCharge", values)
        End Function

        ''' <summary>
        ''' Met à jour une mesure de perte de charge
        ''' </summary>
        Public Function UpdateMesurePerteDeCharge(refEssai As String, indiceMesure As Integer,
                                                  debit As Single, perteCharge As Single,
                                                  gradient As Single) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "UPDATE [LesMesuresPerteDeCharge] SET [debit]=@debit, [perteCharge]=@perteCharge, [gradient]=@gradient WHERE [refEssai]=@refEssai AND [indiceMesure]=@indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)
                    command.Parameters.AddWithValue("@debit", debit)
                    command.Parameters.AddWithValue("@perteCharge", perteCharge)
                    command.Parameters.AddWithValue("@gradient", gradient)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur mise à jour mesure perte de charge : {ex.Message}")
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Supprime une mesure de perte de charge
        ''' </summary>
        Public Function DeleteMesurePerteDeCharge(refEssai As String, indiceMesure As Integer) As Boolean
            Try
                Using connection As New SqlConnection(_connectionString)
                    Dim query As String = "DELETE FROM [LesMesuresPerteDeCharge] WHERE [refEssai]=@refEssai AND [indiceMesure]=@indiceMesure"
                    Dim command As New SqlCommand(query, connection)
                    command.Parameters.AddWithValue("@refEssai", refEssai)
                    command.Parameters.AddWithValue("@indiceMesure", indiceMesure)
                    connection.Open()
                    command.ExecuteNonQuery()
                    Return True
                End Using
            Catch ex As Exception
                Console.WriteLine($"❌ Erreur suppression mesure perte de charge : {ex.Message}")
                Return False
            End Try
        End Function

        ' ============================================
        ' MÉTHODES SPÉCIFIQUES - SOLS
        ' ============================================

        ''' <summary>
        ''' Ajoute un nouveau sol
        ''' </summary>
        Public Function InsertSol(refSol As String, nomSol As String, classe As String, famille As String) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"refSol", refSol},
                {"nomSol", nomSol},
                {"classe", classe},
                {"famille", famille}
            }
            Return InsertRow("LesSols", values)
        End Function

        ''' <summary>
        ''' Met à jour un sol
        ''' </summary>
        Public Function UpdateSol(refSol As String, nomSol As String, classe As String, famille As String) As Boolean
            Dim values As New Dictionary(Of String, Object) From {
                {"nomSol", nomSol},
                {"classe", classe},
                {"famille", famille}
            }
            Return UpdateRowByRef("LesSols", "refSol", refSol, values)
        End Function

        ''' <summary>
        ''' Supprime un sol
        ''' </summary>
        Public Function DeleteSol(refSol As String) As Boolean
            Return DeleteRowByRef("LesSols", "refSol", refSol)
        End Function

    End Class
End Namespace