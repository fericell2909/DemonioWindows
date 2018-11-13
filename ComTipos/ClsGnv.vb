Imports System.Reflection

<Serializable()> Public Class ClsGnv
    Public cPuntoCompra As String = String.Empty
    Public cClave As String = String.Empty
    Public cFactura As String = String.Empty
    Public cPlaca As String = String.Empty

    Public cTaxista As String = String.Empty
    Public cFecha As String = String.Empty
    Public cHora As String = String.Empty
    Public cArticulo As String = String.Empty
    Public cMonto As String = String.Empty
    Public nEstadoDocumento As Integer = 0
    Public bRetornoValorEnvio As Boolean
    Public dFechaConsultaTablaVenta As Date

    Public cPuntoCompraNombre As String = String.Empty
    Public cClaveNombre As String = String.Empty
    Public cFacturaNombre As String = String.Empty
    Public cPlacaNombre As String = String.Empty
    Public cTaxistaNombre As String = String.Empty
    Public cFechaNombre As String = String.Empty
    Public cHoraNombre As String = String.Empty
    Public cArticuloNombre As String = String.Empty
    Public cMontoNombre As String = String.Empty
    Public nEstadoDocumentoNombre As String = String.Empty
    Public bRetornoValorEnvioNombre As String = String.Empty
    Public dFechaConsultaTablaVentaNombre As String = String.Empty

    Public Sub New()
        Me.cPuntoCompraNombre = "cPuntoCompra"
        Me.cClaveNombre = "cClave"
        Me.cFacturaNombre = "cFactura"
        Me.cPlacaNombre = "cPlaca"
        Me.cTaxistaNombre = "cTaxista"
        Me.cFechaNombre = "cFecha"
        Me.cHoraNombre = "cHora"
        Me.cArticuloNombre = "cArticulo"
        Me.cMontoNombre = "cMonto"
        Me.nEstadoDocumentoNombre = "nEstadoDocumento"
        Me.bRetornoValorEnvioNombre = "bRetornoValorEnvio"
        Me.dFechaConsultaTablaVentaNombre = "dFechaConsultaTablaVenta"

    End Sub


    Public Sub AgregaClsGnv(
                            ByVal pcPuntoCompra As String, _
                            ByVal pcClave As String, _
                            ByVal pcFactura As String, _
                            ByVal pcPlaca As String, _
                            ByVal pcTaxista As String, _
                            ByVal pcFecha As String, _
                            ByVal pcHora As String, _
                            ByVal pcArticulo As String, _
                            ByVal pcMonto As String, _
                            ByVal pnEstadoDocumento As Integer, _
                            ByVal pbRetornoValorEnvio As Boolean
                            )

        Me.cPuntoCompra = pcPuntoCompra
        Me.cClave = pcClave
        Me.cFactura = pcFactura
        Me.cPlaca = pcPlaca
        Me.cTaxista = pcTaxista
        Me.cFecha = pcFecha
        Me.cHora = pcHora
        Me.cArticulo = pcArticulo
        Me.cMonto = pcMonto
        Me.nEstadoDocumento = pnEstadoDocumento
        Me.bRetornoValorEnvio = pbRetornoValorEnvio
        Me.dFechaConsultaTablaVenta = DateTime.Now.ToShortDateString().ToString() & " " & DateTime.Now.ToLongTimeString().ToString()
    End Sub

    Public Function of_devuelve_nombre_campo(ByVal oClaseTipo As Type, ByVal nOrdenCampo As Integer) As String


        Dim flags As BindingFlags = BindingFlags.Instance _
                         Or BindingFlags.Public _
                         Or BindingFlags.DeclaredOnly _
                         Or BindingFlags.Static
        '
        Dim OFNombreCampo As FieldInfo() = oClaseTipo.GetFields(flags)

        Return OFNombreCampo(nOrdenCampo).Name


    End Function

End Class
