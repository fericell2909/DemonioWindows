
Imports System.Data
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Timers
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.ServiceProcess
Imports System.Text
Imports Capa_Datos
'Imports System.Xml.Linq
'Imports System.Xml
'Imports System.Data.SqlClient

Imports System.Configuration

Imports ComTipos

Public Class ServicePrueba
    Dim myTimer As System.Timers.Timer
    Dim contador As Integer = 0

    Private TiempoRestante As Integer


    Private olistadatos As New List(Of ComTipos.ClsGnv)
    Private oListadatosErrores As New List(Of ComTipos.ClsGnvError)

    Private oAcceso As New Capa_Datos.ClasGnv
    Private oLogErrores As New Capa_Datos.ManejadorLogs


    Private bValorRetornoServicioTaxi As Boolean

    Private oclsError As New ComTipos.clsError
    Private nTipoRespuesta As Integer
    Private cMensajeRespuesta As String = String.Empty

    Private nIntentosRealizados = 0

    Private nEnviaCorreo As Integer
    Private nMaximoIntentosConexion As Integer
    Private nMinimoRegistrosEnvio As Integer

    Private pcUsuarioEnviaCorrreo As String = String.Empty
    Private pUsuariosDestinatarios As String = String.Empty
    Private pcSubject As String = String.Empty

    Private pcRutaEventosLogs As String = String.Empty

  

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Agregue el código aquí para iniciar el servicio. Este método debería poner
        ' en movimiento los elementos para que el servicio pueda funcionar.
        Try

        
            EventLog.WriteEntry("Comienza")

            EventLog.WriteEntry("Empezando . . . ")

            nEnviaCorreo = System.Configuration.ConfigurationManager.AppSettings("nEnviaCorreo")
            nMaximoIntentosConexion = System.Configuration.ConfigurationManager.AppSettings("nMaximoIntentosConexion")
            nMinimoRegistrosEnvio = System.Configuration.ConfigurationManager.AppSettings("nMinimoRegistrosEnvio")

            pcUsuarioEnviaCorrreo = System.Configuration.ConfigurationManager.AppSettings("pUsuarioEnviaCorreo")
            pUsuariosDestinatarios = System.Configuration.ConfigurationManager.AppSettings("pUsuariosDestinatarios")
            pcSubject = System.Configuration.ConfigurationManager.AppSettings("pcSubject")

            pcRutaEventosLogs = System.Configuration.ConfigurationManager.AppSettings("pcRutaLog")

            myTimer = New System.Timers.Timer()
            ' pregunta cada n minutos

            myTimer.Interval = 60000 * getIntervalo("tiempo")

            'myTimer.Interval = 60000 * 1
            AddHandler myTimer.Elapsed, AddressOf Me.OnTimer
            myTimer.Enabled = True

            'Llenado de Variables
            myTimer.Start()

            EventLog.WriteEntry("Saliendo . . . ")

        Catch ex As Exception
            oLogErrores.CreateLogFiles()
            oLogErrores.ErrorLog(pcRutaEventosLogs, "" & ex.Message & " " & ex.StackTrace & " ")
        End Try


    End Sub
    Public Sub Service1()
        If Not System.Diagnostics.EventLog.SourceExists("MySource") Then
            System.Diagnostics.EventLog.CreateEventSource("MySource",
            "MyNewLog")
        End If

        EventLog.Source = "MySource"
        EventLog.Log = "MyNewLog"

        'EventLog.WriteEntry("Amen")
    
        'EventLog.WriteEntry("Amen 2 ")

    End Sub


    Public Function getIntervalo(ByVal psvalor As String) As Double
        Dim salida As Integer = 0
        'Dim xmlCadena As XElement
        'xmlCadena = XElement.Load(rutaVariables())

        'Dim xmlCad As Object = From el In xmlCadena.Descendants("intervalo") Select el

        'For Each _el In xmlCad
        '    salida = _el.Value
        'Next
        Try


            salida = System.Configuration.ConfigurationManager.AppSettings("" & psvalor)
            oLogErrores.CreateLogFiles()
            oLogErrores.ErrorLog(pcRutaEventosLogs, " " & CStr(salida) & " " & psvalor)

        Catch ex As Exception
            oLogErrores.CreateLogFiles()
            oLogErrores.ErrorLog(pcRutaEventosLogs, "" & ex.Message & " " & ex.StackTrace & " " & CStr(salida))

        End Try

        Return Convert.ToDouble(salida)
    End Function

    'Private Function getIntervalo2(ByVal psvalor As String) As Double
    '    Dim salida As Integer = 0
    '    'Dim xmlCadena As XElement
    '    'xmlCadena = XElement.Load(rutaVariables())

    '    'Dim xmlCad As Object = From el In xmlCadena.Descendants("intervalo") Select el

    '    'For Each _el In xmlCad
    '    '    salida = _el.Value
    '    'Next
    '    Try


    '        salida = System.Configuration.ConfigurationManager.AppSettings("" & psvalor)
    '        oLogErrores.CreateLogFiles()
    '        oLogErrores.ErrorLog("C:\LogVentas\", " " & CStr(salida) & " " & psvalor)

    '    Catch ex As Exception
    '        oLogErrores.CreateLogFiles()
    '        oLogErrores.ErrorLog("C:\LogVentas\", "" & ex.Message & " " & ex.StackTrace & " " & CStr(salida))

    '    End Try

    '    Return Convert.ToDouble(salida)
    'End Function
    Private Sub myTimer_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs)
        EventLog.WriteEntry("True False . . . ")
        myTimer.Enabled = False
        'CallServicioMail()
        TiempoRestante = 1
        myTimer.Enabled = True
    End Sub
    Protected Overrides Sub OnStop()
        ' Agregue el código aquí para realizar cualquier anulación necesaria para detener el servicio.
        EventLog.WriteEntry("Servicio Detenido . . . ")
    End Sub
    Private Sub OnTimer(ByVal sender As Object, ByVal e As Timers.ElapsedEventArgs)
        ' TODO: Insert monitoring activities here.
        'Dim eventId As Integer
        ' contador = contador + 1

        '
        'EventLog.WriteEntry("Amen " & contador.ToString(), EventLogEntryType.Information, eventId)

        'eventId = eventId + 1
        EventLog.WriteEntry("Entrando en la Busqueda . . .  ")
        'oLogErrores.CreateLogFiles()
        'oLogErrores.ErrorLog(pcRutaEventosLogs, " Entrando a Consultas . . .")

        EventLog.WriteEntry("Variable  Tiempo Restante : " & CStr(TiempoRestante))
        'EventLog.WriteEntry("Variable  Maximos Intentos de Conexion : " & CStr(nMaximoIntentosConexion))
        'EventLog.WriteEntry("Variable  MinimoRegistosenvio : " & CStr(nMinimoRegistrosEnvio))

        'EventLog.WriteEntry("Variable  usuario envia correo : " & CStr(pcUsuarioEnviaCorrreo))
        'EventLog.WriteEntry("Variable  Destinatarios : " & CStr(pUsuariosDestinatarios))
        'EventLog.WriteEntry("Variable  Subject: " & CStr(pcSubject))
        'EventLog.WriteEntry("Amen 2 ")


        Dim Valor_hash As String = String.Empty

        If TiempoRestante >= 0 Then

            nIntentosRealizados = nIntentosRealizados + 1

RecuperarVentas:
            oAcceso.RecuperaLista(olistadatos, nTipoRespuesta, cMensajeRespuesta)

            If nTipoRespuesta = oclsError.nExito Then

                If olistadatos.Count > nMinimoRegistrosEnvio Then

                    nTipoRespuesta = oclsError.nInicioRespuesta

                    oAcceso.LlamadaServicio_Taxi_Registro_Log(olistadatos, nTipoRespuesta, cMensajeRespuesta, oListadatosErrores, 1)


                End If

                nIntentosRealizados = 0

            Else


                If nTipoRespuesta = oclsError.nErrorConexionBaseDatos Then
                    If nIntentosRealizados = nMaximoIntentosConexion Then
                        If nEnviaCorreo = oclsError.nExito Then

                            If oAcceso.EnvioCorreo(pcUsuarioEnviaCorrreo, pUsuariosDestinatarios, pcSubject, cMensajeRespuesta) Then
                                nIntentosRealizados = 0
                                GoTo TiempoEjecutar
                            End If

                        End If

                    Else

                        nIntentosRealizados = nIntentosRealizados + 1
                        GoTo RecuperarVentas

                    End If
                End If

                nIntentosRealizados = 0


            End If

TiempoEjecutar:
            'Call TiempoEjecutar(10)

            'oClaseEmail.of_Enviar_Correo(olistadatos)


        End If

Salir:

    End Sub
    Protected Overrides Sub OnContinue()
        EventLog.WriteEntry("In OnContinue.")
        myTimer.Start()
    End Sub

End Class
