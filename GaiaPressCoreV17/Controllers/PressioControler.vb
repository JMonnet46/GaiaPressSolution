Imports Microsoft.AspNetCore.Mvc

Namespace GaiaPressCoreV17.Controllers
    <ApiController>
    <Route("api/[controller]")>
    Public Class PressioController
        Inherits ControllerBase

        <HttpGet>
        Public Function GetValue() As String
            Return "GaiaPress API is running!"
        End Function
    End Class
End Namespace