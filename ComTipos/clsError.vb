<Serializable()> Public Class clsError
    Public nExito As Integer
    Public nInicioRespuesta As Integer
    Public nErrorConexionBaseDatos As Integer
    Public nErrorNegocio As Integer
    Public cMensajeBaseDatos As String
    Public nExitoServicioLista As Integer
    Public nRegistroSinActualizarVentas As Integer



    Public Sub New()
        nExito = 1
        nErrorConexionBaseDatos = -3
        nErrorNegocio = -1
        cMensajeBaseDatos = ""
        nInicioRespuesta = 0
        nRegistroSinActualizarVentas = 2

    End Sub

End Class
