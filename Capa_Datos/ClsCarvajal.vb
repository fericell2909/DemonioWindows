Imports System.Data.SqlClient
Imports ComConecta.ClsConecta
Imports ComTipos.ClsCarvajal
Public Class ClsCarvajal
    Dim objCon As New Conexion
    Dim cn As SqlConnection
    Public Function RecuperaLista() As List(Of ComTipos.ClsCarvajal)
        'Dim oConecta As New ComConecta.ClsConecta
        Dim oDataReader As SqlDataReader
        Dim oListaData As New List(Of ComTipos.ClsCarvajal)
        'oConecta.CadenaConexion()
        'oConecta.AbrirConexion()
        cn = objCon.conectar
        cn.Open()
        Dim oComand As New SqlCommand("USP_OBT_DOCUMENTO", cn)
        oComand.CommandType = CommandType.StoredProcedure
        oDataReader = oComand.ExecuteReader
        While (oDataReader.Read)
            Dim oClsExcel As New ComTipos.ClsCarvajal
            'oClsExcel.ListaCarvajal(oDataReader.Item(0), _
            '                     Convert.ToInt64(oDataReader.Item(1)), _
            '                     Convert.ToInt16(oDataReader.Item(2)), oDataReader.Item(3))

            oClsExcel.ListaCarvajal(oDataReader.Item(0), _
                                   oDataReader.Item(1), _
                                   oDataReader.Item(2), _
                                   oDataReader.Item(3), _
                                   oDataReader.Item(4), _
                                   oDataReader.Item(5), _
                                   oDataReader.Item(6), _
                                   oDataReader.Item(7), _
                                   oDataReader.Item(8), _
                                   oDataReader.Item(9), _
                                   oDataReader.Item(10), _
                                   oDataReader.Item(11), _
                                   oDataReader.Item(12), _
                                   oDataReader.Item(13), _
                                   oDataReader.Item(14), _
                                   oDataReader.Item(15), _
                                   oDataReader.Item(16), _
                                   oDataReader.Item(17), _
                                   oDataReader.Item(18), _
                                   oDataReader.Item(19), _
                                   oDataReader.Item(20), _
                                   oDataReader.Item(21), _
                                   oDataReader.Item(22), _
                                   oDataReader.Item(23), _
                                   oDataReader.Item(24), _
                                   oDataReader.Item(25), _
                                   oDataReader.Item(26), _
                                   oDataReader.Item(27), _
                                   oDataReader.Item(28), _
                                   oDataReader.Item(29), _
                                   oDataReader.Item(30), _
                                   oDataReader.Item(31), _
                                   oDataReader.Item(32), _
                                   oDataReader.Item(33), _
                                   oDataReader.Item(34), _
                                   oDataReader.Item(35), _
                                   oDataReader.Item(36), _
                                   oDataReader.Item(37), _
                                   oDataReader.Item(38), _
                                   oDataReader.Item(39), _
                                   oDataReader.Item(40), _
                                   oDataReader.Item(41), _
                                   oDataReader.Item(42), _
                                   oDataReader.Item(43), _
                                   oDataReader.Item(44), _
                                   oDataReader.Item(45), _
                                   oDataReader.Item(46), _
                                   oDataReader.Item(47), _
                                   oDataReader.Item(48),
                                   oDataReader.Item(50), _
                                   oDataReader.Item(51), _
                                   oDataReader.Item(52))







            oListaData.Add(oClsExcel)
            oClsExcel = Nothing
        End While
        oDataReader.Close()
        oDataReader = Nothing
        oComand.Dispose()
        oComand = Nothing
        'oConecta.CerrarConexion()
        'oConecta = Nothing
        cn.Dispose()
        RecuperaLista = oListaData

    End Function

    Public Sub CerrarTransac_Hash(ByVal pnNBRDOCUMENT As String, _
                                  ByVal pnDOCTYPEID As String, _
                            ByVal pnSITE As String, _
                            ByVal pnVALORHASH As String)

        'Dim oConecta As New ComConecta.ClsConecta
        'Dim oDataReader As SqlDataReader
        ' Dim oListaData As New List(Of ComTipos.ClsCarvajal)
        'oConecta.CadenaConexion()
        'oConecta.AbrirConexion()
        cn = objCon.conectar
        cn.Open()
        Dim oComand As New SqlCommand("USP_ACTUALIZA_HASH", cn)
        oComand.CommandType = CommandType.StoredProcedure

        oComand.Parameters.Add("@NBRDOCUMENT", SqlDbType.NVarChar, 12).Value = Trim(pnNBRDOCUMENT)
        oComand.Parameters.Add("@DOCTYPEID", SqlDbType.Char, 3).Value = Trim(pnDOCTYPEID)
        oComand.Parameters.Add("@SITEID", SqlDbType.Char, 3).Value = Trim(pnSITE)
        oComand.Parameters.Add("@HASH", SqlDbType.Text).Value = pnVALORHASH


        oComand.ExecuteNonQuery()

        'oComand.Parameters.Clear()

        'oComand.Dispose()
        oComand = Nothing
        'oConecta.CerrarConexion()
        'oConecta = Nothing
        cn.Dispose()

        'prm.ParameterName = "@ID"
        'prm.SqlDbType = SqlDbType.Int
        'prm.Direction = ParameterDirection.Output
        'cmd.Parameters.Add(prm)

        'Dim adp As SqlDataAdapter = New SqlDataAdapter(oComand)

        'Dim DataSet As DataSet = New DataSet("Lineas")

        'adp.Fill(DataSet)
        'GridLineas.DataSource = DataSet.Tables(0)



    End Sub
    Public Sub CerrarTransac_Hash_Anulada(ByVal pnNBRDOCUMENT As String, _
                              ByVal pnDOCTYPEID As String, _
                        ByVal pnSITE As String)

        'Dim oConecta As New ComConecta.ClsConecta
        'Dim oDataReader As SqlDataReader
        ' Dim oListaData As New List(Of ComTipos.ClsCarvajal)
        'oConecta.CadenaConexion()
        'oConecta.AbrirConexion()
        cn = objCon.conectar
        cn.Open()
        Dim oComand As New SqlCommand("USP_ACTUALIZA_HASH_ANULADA", cn)
        oComand.CommandType = CommandType.StoredProcedure

        oComand.Parameters.Add("@NBRDOCUMENT", SqlDbType.NVarChar, 12).Value = Trim(pnNBRDOCUMENT)
        oComand.Parameters.Add("@DOCTYPEID", SqlDbType.Char, 3).Value = Trim(pnDOCTYPEID)
        oComand.Parameters.Add("@SITEID", SqlDbType.Char, 3).Value = Trim(pnSITE)



        oComand.ExecuteNonQuery()

        'oComand.Parameters.Clear()

        'oComand.Dispose()
        oComand = Nothing
        'oConecta.CerrarConexion()
        'oConecta = Nothing
        cn.Dispose()

        'prm.ParameterName = "@ID"
        'prm.SqlDbType = SqlDbType.Int
        'prm.Direction = ParameterDirection.Output
        'cmd.Parameters.Add(prm)

        'Dim adp As SqlDataAdapter = New SqlDataAdapter(oComand)

        'Dim DataSet As DataSet = New DataSet("Lineas")

        'adp.Fill(DataSet)
        'GridLineas.DataSource = DataSet.Tables(0)



    End Sub




End Class



