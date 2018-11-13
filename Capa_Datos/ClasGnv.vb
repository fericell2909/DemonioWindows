Imports System.Data.SqlClient
Imports ComTipos.ClsGnv
Imports ComTipos.clsError
Imports System
Imports System.Configuration

Public Class ClasGnv
    Dim objCon As New Conexion
    Dim cn As SqlClient.SqlConnection
    Private oclsErrores As New ComTipos.clsError
    Private aServicio As ServicioTaxi.ServicioTaxiClient
    Private cRutaLog As String = String.Empty
    Private sMensajeErrorCorreo As String = String.Empty
    Private oLogErroresRutaLocal As Capa_Datos.ManejadorLogs
    Public Sub New()
        Try

            cRutaLog = System.Configuration.ConfigurationManager.AppSettings("pcRutaLog")

            oLogErroresRutaLocal = New Capa_Datos.ManejadorLogs()

            oLogErroresRutaLocal.CreateFolderNombre(cRutaLog)


        Catch ex As Exception

        End Try



    End Sub

    Public Sub RecuperaLista(
                                    ByRef OListaDatosEnviar As List(Of ComTipos.ClsGnv), _
                                    ByRef nTipoResultado As Integer, _
                                    ByRef cTipoMensajeError As String)


        Dim oListaData As New List(Of ComTipos.ClsGnv)

        nTipoResultado = oclsErrores.nInicioRespuesta


        Try

            Dim oDataReader As SqlDataReader
            cn = objCon.conectar
            cn.Open()

            'Dim oComand As New SqlCommand("usp_gnv_lista_ventas_enviar", cn)
            Dim oComand As New SqlCommand("PC_SP_VENTAS_GNV", cn)
            oComand.CommandType = CommandType.StoredProcedure
            oDataReader = oComand.ExecuteReader

            While (oDataReader.Read)
                Dim oClsGnv As New ComTipos.ClsGnv

                oClsGnv.AgregaClsGnv(oDataReader.Item(0), _
                           oDataReader.Item(1), _
                            oDataReader.Item(2), _
                            oDataReader.Item(3), _
                            oDataReader.Item(4), _
                            oDataReader.Item(5), _
                            oDataReader.Item(6), _
                            oDataReader.Item(7), _
                            oDataReader.Item(8), Int(oDataReader.Item(9)), False)

                oListaData.Add(oClsGnv)
                oClsGnv = Nothing

            End While

            oDataReader.Close()
            oDataReader = Nothing
            oComand.Dispose()
            oComand = Nothing

            cn.Dispose()


        Catch ex As Exception

            nTipoResultado = oclsErrores.nErrorConexionBaseDatos
            cTipoMensajeError = ex.Message.ToString() & " " & "PC_SP_VENTAS_GNV" & " " + ConfigurationManager.ConnectionStrings("cn").ConnectionString
            oLogErroresRutaLocal.CreateLogFiles()
            oLogErroresRutaLocal.ErrorLog(cRutaLog, ex.Message.ToString() & " " & "PC_SP_VENTAS_GNV" & " " + ConfigurationManager.ConnectionStrings("cn").ConnectionString & " " & ex.StackTrace)

        End Try

        If nTipoResultado = oclsErrores.nInicioRespuesta Then
            nTipoResultado = oclsErrores.nExito
            OListaDatosEnviar = oListaData

            If oListaData.Count = 0 Then

                oLogErroresRutaLocal.CreateLogFiles()
                oLogErroresRutaLocal.ErrorLog(cRutaLog, "No Existen registros Disponibles al llamar al procedure :  PC_SP_VENTAS_GNV")

            Else
                oLogErroresRutaLocal.CreateLogFiles()
                oLogErroresRutaLocal.ErrorLog(cRutaLog, "Registros Disponibles al llamar al procedure :  PC_SP_VENTAS_GNV " & " " & CStr(oListaData.Count))

            End If
        Else
            OListaDatosEnviar = Nothing

        End If




    End Sub

    Public Sub LlamadaServicio_Taxi_Registro_Log(
                                    ByVal oListaDatosEnviar As List(Of ComTipos.ClsGnv), _
                                    ByRef nTIpoResultado As Integer, _
                                    ByRef cMensajeError As String, ByRef OlistaErrores As List(Of ComTipos.ClsGnvError), ByVal nIntentosConexionServicio As Integer)

        Dim pnIdLogAuditoria As Long = 0 ' nos Devuelve el registro insertado en el Log.

        Dim bValorRetornoServicioTaxi As Boolean ' Valor que devuelve la llamada al seervicio web.

        Dim nNumeroContadorRegistros As Integer = 1 ' registros de recorrido de la lista de datos a enviar

        Dim sCadenaRetorno_Xml As String = String.Empty

        Dim oObjError As New ComTipos.ClsGnvError

        Dim oListaErrorMensaje As New List(Of ComTipos.ClsGnvError) 'Todos los Que tengan Errores : Ya sea por error al instanciar servicio o error al enviar'

        Dim nNumeroIntentosConexionServicio As Integer = 0 'La cantidad de Intentos viene por el parametro : nIntentosConexionServicio'

        nTIpoResultado = oclsErrores.nInicioRespuesta

        If oListaDatosEnviar.Count > 0 Then

            For Each ObjLista As ComTipos.ClsGnv In oListaDatosEnviar

                ' Los que estan bien y los otros deben enviar correo para validacion

                oObjError.cPuntoCompra = ObjLista.cPuntoCompra
                oObjError.cClave = ObjLista.cClave
                oObjError.cFactura = ObjLista.cFactura
                oObjError.cPlaca = ObjLista.cPlaca
                oObjError.cTaxista = ObjLista.cTaxista
                oObjError.cFecha = ObjLista.cFecha
                oObjError.cHora = ObjLista.cHora
                oObjError.cArticulo = ObjLista.cArticulo
                oObjError.cMonto = ObjLista.cMonto

                nNumeroIntentosConexionServicio = 0
                Try

aperturar_conexion:
                    Try

                        aServicio = New ServicioTaxi.ServicioTaxiClient


                    Catch ex As Exception
                        cMensajeError = ex.Message.ToString()

                        oLogErroresRutaLocal.CreateLogFiles()
                        oLogErroresRutaLocal.ErrorLog(cRutaLog, ex.Message.ToString() & " " & ex.StackTrace)
                        nNumeroIntentosConexionServicio = nNumeroIntentosConexionServicio + 1



                        If nIntentosConexionServicio = nNumeroIntentosConexionServicio Then

                            oObjError.cMensajeErrorEnvio = ex.Message.ToString()

                            oListaErrorMensaje.Add(oObjError)

                            GoTo siguiente

                        Else
                            oObjError.cMensajeErrorEnvio = ex.Message.ToString()
                            oListaErrorMensaje.Add(oObjError)
                            GoTo aperturar_conexion

                        End If

                    End Try

                    bValorRetornoServicioTaxi = aServicio.enviarVenta(
                                          ObjLista.cPuntoCompra, _
                                          ObjLista.cClave, _
                                          ObjLista.cFactura, _
                                          ObjLista.cPlaca, _
                                          ObjLista.cTaxista, _
                                          ObjLista.cFecha, _
                                          ObjLista.cHora, _
                                          ObjLista.cArticulo, _
                                          ObjLista.cMonto)

                    ObjLista.bRetornoValorEnvio = bValorRetornoServicioTaxi
                Catch ex As Exception
                    cMensajeError = ex.Message.ToString()
                    oObjError.cMensajeErrorEnvio = ex.Message.ToString()
                    oListaErrorMensaje.Add(oObjError)
                    oLogErroresRutaLocal.CreateLogFiles()
                    oLogErroresRutaLocal.ErrorLog(cRutaLog, ex.Message.ToString() & " " & ex.StackTrace)

                    GoTo siguiente
                End Try
siguiente:
                nNumeroContadorRegistros = nNumeroContadorRegistros + 1

            Next

            'Registro de Log que Actualiza Estados 
            Registro_Log(of_Devuelve_Cadena_Xml(oListaDatosEnviar), nTIpoResultado, cMensajeError, oclsErrores.nExito, pnIdLogAuditoria)

            

            If nTIpoResultado <> oclsErrores.nExito Then

                EnvioCorreo(System.Configuration.ConfigurationManager.AppSettings("pUsuarioEnviaCorreo"), System.Configuration.ConfigurationManager.AppSettings("pUsuariosDestinatarios"), System.Configuration.ConfigurationManager.AppSettings("pcSubjectErrorEnvioCorreo") & " " & CStr(pnIdLogAuditoria), of_Devuelve_Cadena_Xml(oListaDatosEnviar))
            Else
                oLogErroresRutaLocal.CreateLogFiles()
                oLogErroresRutaLocal.ErrorLog(cRutaLog, System.Configuration.ConfigurationManager.AppSettings("pcMensajeCorrectoLog") & " " & CStr(pnIdLogAuditoria))

            End If

            If oListaErrorMensaje.Count > 0 Then
                ''Mensaje'

                Registro_Log(of_Devuelve_Cadena_Xml(oListaErrorMensaje), nTIpoResultado, cMensajeError, oclsErrores.nRegistroSinActualizarVentas, pnIdLogAuditoria)

                EnvioCorreo(System.Configuration.ConfigurationManager.AppSettings("pUsuarioEnviaCorreo"), System.Configuration.ConfigurationManager.AppSettings("pUsuariosDestinatarios"), System.Configuration.ConfigurationManager.AppSettings("pcSubjectErrorMensaje") & " Codigo Log: " & CStr(pnIdLogAuditoria), of_Devuelve_Cadena_Xml(oListaErrorMensaje))

                GoTo salir



            Else

                oLogErroresRutaLocal.CreateLogFiles()
                oLogErroresRutaLocal.ErrorLog(cRutaLog, System.Configuration.ConfigurationManager.AppSettings("pcMensajeCorrectoLogFalse") & " " & CStr(pnIdLogAuditoria))

                GoTo salir
            End If

        Else
            nTIpoResultado = 1
            cMensajeError = String.Empty
            GoTo salir


        End If

salir:


    End Sub


    Private Sub Registro_Log(ByVal sCadenaRetorno As String, ByRef pnTipoResultado As Integer, ByRef pcMensajeBaseDatos As String, ByVal pnTipoRegistro As Integer, ByRef pnIdLogAuditoria As Long)
        Try

            Dim oDataReader As SqlDataReader
            Dim oListaData As New List(Of ComTipos.ClsGnv)
            'oConecta.CadenaConexion()
            'oConecta.AbrirConexion()
            cn = objCon.conectar("cn")
            cn.Open()
            Dim oComand As New SqlCommand("usp_gnv_auditoria_log", cn)
            oComand.CommandType = CommandType.StoredProcedure
            oComand.Parameters.Add("@cParametroXml", System.Data.SqlDbType.Text).Value = sCadenaRetorno
            oComand.Parameters.Add("@pnTipoRegistro", SqlDbType.Int).Value = pnTipoRegistro
            oComand.Parameters.Add("@pnTipoResultado", SqlDbType.Int).Value = pnTipoResultado
            oComand.Parameters("@pnTipoResultado").Direction = ParameterDirection.Output
            oComand.Parameters.Add("@pcMensajeBaseDatos", SqlDbType.VarChar, 100).Value = pcMensajeBaseDatos
            oComand.Parameters("@pcMensajeBaseDatos").Direction = ParameterDirection.Output

            oComand.Parameters.Add("@pnIdLogAuditoria", SqlDbType.BigInt).Value = pnIdLogAuditoria
            oComand.Parameters("@pnIdLogAuditoria").Direction = ParameterDirection.Output

            oComand.ExecuteNonQuery()
            pnTipoResultado = oComand.Parameters("@pnTipoResultado").Value
            pcMensajeBaseDatos = oComand.Parameters("@pcMensajeBaseDatos").Value
            pnIdLogAuditoria = oComand.Parameters("@pnIdLogAuditoria").Value
            oDataReader = Nothing
            oComand.Dispose()
            oComand = Nothing
            cn.Dispose()

        Catch ex As Exception
            'Dim cadena As String = ex.Message
            pnTipoResultado = -2
            pcMensajeBaseDatos = ex.Message.ToString()

            oLogErroresRutaLocal.CreateLogFiles()
            oLogErroresRutaLocal.ErrorLog(cRutaLog, " "& CStr(pnIdLogAuditoria) &  ex.Message.ToString() & "usp_gnv_auditoria_log" & " " + ConfigurationManager.ConnectionStrings("cn").ConnectionString & " " & ex.StackTrace)


        End Try

    End Sub

    Private Function of_Devuelve_Cadena_Xml(ByVal oListaDatosEnviar As List(Of ComTipos.ClsGnvError)) As String
        Dim sCadenaXml_Retorno As String = String.Empty
        Try



            Dim sEspacio As String = " "
            Dim sComilla As String = """"

            Dim sTagEnvio_cierre As String = sEspacio & "/>"

            Dim sTagEnvio_cierre_xml As String = "</GNV>"

            Dim sTagVenta As String = "<VENTA" & sEspacio

            Dim ln_cotador As Integer = 1


            sCadenaXml_Retorno = "<GNV><VENTA " & sEspacio


            For Each ObjListaXml As ComTipos.ClsGnvError In oListaDatosEnviar

                sCadenaXml_Retorno = sCadenaXml_Retorno & _
                                                ObjListaXml.cPuntoCompraNombre & "=" & sComilla & ObjListaXml.cPuntoCompra & sComilla & sEspacio & _
                                                ObjListaXml.cClaveNombre & "=" & sComilla & ObjListaXml.cClave & sComilla & sEspacio & _
                                                ObjListaXml.cFacturaNombre & "=" & sComilla & ObjListaXml.cFactura & sComilla & sEspacio & _
                                                ObjListaXml.cPlacaNombre & "=" & sComilla & ObjListaXml.cPlaca & sComilla & sEspacio & _
                                                ObjListaXml.cTaxistaNombre & "=" & sComilla & ObjListaXml.cTaxista & sComilla & sEspacio & _
                                                ObjListaXml.cFechaNombre & "=" & sComilla & ObjListaXml.cFecha & sComilla & sEspacio & _
                                                ObjListaXml.cHoraNombre & "=" & sComilla & ObjListaXml.cHora & sComilla & sEspacio & _
                                                ObjListaXml.cArticuloNombre & "=" & sComilla & ObjListaXml.cArticulo & sComilla & sEspacio & _
                                                ObjListaXml.cMontoNombre & "=" & sComilla & ObjListaXml.cMonto & sComilla & sEspacio & _
                                                ObjListaXml.nEstadoDocumentoNombre & "=" & sComilla & ObjListaXml.nEstadoDocumento & sComilla & sEspacio & _
                                                ObjListaXml.bRetornoValorEnvioNombre & "=" & sComilla & ObjListaXml.bRetornoValorEnvio & sComilla & sEspacio & _
                                                ObjListaXml.cMensajeErrorEnvioNombre & "=" & sComilla & ObjListaXml.cMensajeErrorEnvio & sComilla & sEspacio & _
                                                sTagEnvio_cierre & IIf(oListaDatosEnviar.Count = ln_cotador, sTagEnvio_cierre_xml, sTagVenta)


                ln_cotador = ln_cotador + 1
            Next

        Catch ex As Exception
            oLogErroresRutaLocal.CreateLogFiles()
            oLogErroresRutaLocal.ErrorLog(cRutaLog, ex.Message.ToString() & " " & ex.StackTrace)
        End Try

        Return sCadenaXml_Retorno

    End Function

    Private Function of_Devuelve_Cadena_Xml(ByVal oListaDatosEnviar As List(Of ComTipos.ClsGnv)) As String

        Dim sCadenaXml_Retorno As String = String.Empty

        Try

            Dim sEspacio As String = " "
            Dim sComilla As String = """"

            Dim sTagEnvio_cierre As String = sEspacio & "/>"

            Dim sTagEnvio_cierre_xml As String = "</GNV>"

            Dim sTagVenta As String = "<VENTA" & sEspacio

            Dim ln_cotador As Integer = 1


            sCadenaXml_Retorno = "<GNV><VENTA " & sEspacio


            For Each ObjListaXml As ComTipos.ClsGnv In oListaDatosEnviar

                sCadenaXml_Retorno = sCadenaXml_Retorno & _
                                                ObjListaXml.cPuntoCompraNombre & "=" & sComilla & ObjListaXml.cPuntoCompra & sComilla & sEspacio & _
                                                ObjListaXml.cClaveNombre & "=" & sComilla & ObjListaXml.cClave & sComilla & sEspacio & _
                                                ObjListaXml.cFacturaNombre & "=" & sComilla & ObjListaXml.cFactura & sComilla & sEspacio & _
                                                ObjListaXml.cPlacaNombre & "=" & sComilla & ObjListaXml.cPlaca & sComilla & sEspacio & _
                                                ObjListaXml.cTaxistaNombre & "=" & sComilla & ObjListaXml.cTaxista & sComilla & sEspacio & _
                                                ObjListaXml.cFechaNombre & "=" & sComilla & ObjListaXml.cFecha & sComilla & sEspacio & _
                                                ObjListaXml.cHoraNombre & "=" & sComilla & ObjListaXml.cHora & sComilla & sEspacio & _
                                                ObjListaXml.cArticuloNombre & "=" & sComilla & ObjListaXml.cArticulo & sComilla & sEspacio & _
                                                ObjListaXml.cMontoNombre & "=" & sComilla & ObjListaXml.cMonto & sComilla & sEspacio & _
                                                ObjListaXml.nEstadoDocumentoNombre & "=" & sComilla & ObjListaXml.nEstadoDocumento & sComilla & sEspacio & _
                                                ObjListaXml.bRetornoValorEnvioNombre & "=" & sComilla & ObjListaXml.bRetornoValorEnvio & sComilla & sEspacio & _
                                                ObjListaXml.dFechaConsultaTablaVentaNombre & "=" & sComilla & ObjListaXml.dFechaConsultaTablaVenta & sComilla & sEspacio & _
                                                sTagEnvio_cierre & IIf(oListaDatosEnviar.Count = ln_cotador, sTagEnvio_cierre_xml, sTagVenta)


                ln_cotador = ln_cotador + 1
            Next
        Catch ex As Exception
            oLogErroresRutaLocal.CreateLogFiles()
            oLogErroresRutaLocal.ErrorLog(cRutaLog, ex.Message.ToString() & " " & ex.StackTrace)
        End Try

        Return sCadenaXml_Retorno

    End Function

    Public Function EnvioCorreo(
                                ByVal pUsuarioEnviaCorreo As String, _
                                ByVal pUsuariosDestinatarios As String, _
                                ByVal pcSubject As String, _
                                ByVal pcMensaje As String) As Boolean


        Dim bvalorRetorno As Boolean = False

        Try

            Dim oDataReader As SqlDataReader
            cn = objCon.conectar("cn")
            cn.Open()
            Dim oComand As New SqlCommand("sp_send_cdosysmail", cn)
            oComand.CommandType = CommandType.StoredProcedure
            oComand.Parameters.Add("@From", System.Data.SqlDbType.VarChar, 100).Value = pUsuarioEnviaCorreo
            oComand.Parameters.Add("@To", System.Data.SqlDbType.VarChar, 100).Value = pUsuariosDestinatarios
            oComand.Parameters.Add("@Subject", System.Data.SqlDbType.VarChar, 100).Value = pcSubject
            oComand.Parameters.Add("@Body", System.Data.SqlDbType.VarChar, 4000).Value = pcMensaje

            oComand.ExecuteNonQuery()
            oDataReader = Nothing
            oComand.Dispose()
            oComand = Nothing
            cn.Dispose()

            bvalorRetorno = True

        Catch ex As Exception
            'Dim cadena As String = ex.Message
            oLogErroresRutaLocal.CreateLogFiles()
            oLogErroresRutaLocal.ErrorLog(cRutaLog, ex.Message.ToString() & "sp_send_cdosysmail" & " " + ConfigurationManager.ConnectionStrings("cn").ConnectionString & " " & ex.StackTrace)
            bvalorRetorno = False

        End Try

        Return bvalorRetorno

    End Function
End Class
