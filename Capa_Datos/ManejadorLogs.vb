Imports System.IO
Imports System.Configuration


Public Class ManejadorLogs


    Public sLogFormat As String
    Public sErrorTime As String
    Public Function CreateFolderNombre(ByVal cRutaLog As String) As Integer

        'Create Carpeta y/0 Create Carpeta y Ruta
        Try
            If Not Directory.Exists(cRutaLog) Then
                Directory.CreateDirectory(cRutaLog)

                Return 1
            Else
                Return 2
            End If
        Catch e As Exception
            Return 0
        End Try
    End Function

    Public Function DeleteFolderNombre(ByVal Nombre As String, ByRef strPathFinal As String) As Integer

        Dim strPath As String
        Dim strSeparator As String
        If Nombre.CompareTo("") = 0 Then
            Return -1
        Else
            strSeparator = System.Configuration.ConfigurationManager.AppSettings("PathSeparator") 'ConfigurationSettings.AppSettings.Get("PathSeparator")
            strPath = System.Configuration.ConfigurationManager.AppSettings("initialPath") 'ConfigurationSettings.AppSettings.Get("initialPath") & strSeparator & Nombre
            strPathFinal = strPath
            Try
                If Not Directory.Exists(strPath) Then
                    Return 1
                Else
                    Directory.Delete(strPath, True)
                    Return 2
                End If
            Catch e As Exception
                Return 0
            End Try
        End If
    End Function


    Public Sub CreateLogFiles()
        Try
            'sLogFormat used to create log files format :
            ' dd/MM/yyyy hh:mm:ss AM/PM ==> Log Message
            sLogFormat = DateTime.Now.ToShortDateString().ToString() & " " & DateTime.Now.ToLongTimeString().ToString() & " ==> "

            'this variable used to create log filename format "
            'for example filename : ErrorLogYYYYMMDD
            Dim sYear As String = DateTime.Now.Year.ToString()
            Dim sMonth As String = DateTime.Now.Month.ToString()
            Dim sDay As String = DateTime.Now.Day.ToString()
            sErrorTime = sYear & sMonth & sDay
        Catch ex As Exception

        End Try

    End Sub

    Public Sub ErrorLog(ByVal sPathName As String, ByVal sErrMsg As String)
        Try
            Dim sw As StreamWriter = New StreamWriter(sPathName & sErrorTime & ".txt", True)
            sw.WriteLine("")
            sw.WriteLine(sLogFormat & sErrMsg)
            sw.Flush()
            sw.Close()
        Catch ex As Exception

        End Try

    End Sub



End Class
