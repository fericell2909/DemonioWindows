Imports System.Configuration
Imports System.Data.SqlClient
Public Class Conexion
    Dim conexion As SqlConnection
    Public Function conectar()
        conexion = New SqlConnection(ConfigurationManager.ConnectionStrings("cn").ConnectionString)
        Return conexion
        
    End Function

    Public Function conectar(ByVal oCadenaConexion As String)
        conexion = New SqlConnection(ConfigurationManager.ConnectionStrings(oCadenaConexion).ConnectionString)
        Return conexion

    End Function
End Class
