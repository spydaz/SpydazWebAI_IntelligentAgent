Namespace AI_AGENT



    ''' <summary>
    ''' MAIN BRAIN RESPONSE Needs to be OverWritten
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Artificial_Intelligence_Brain

        Private MResponse As String = ""
        Public ReadOnly Property Response As String
            Get
                Return MResponse
            End Get
        End Property

        Public Sub New()
            MResponse = ""

        End Sub


        ''' <summary>
        ''' This is the main Function Called by the Agent 
        ''' </summary>
        ''' <param name="Userinput"></param>
        ''' <param name="Response"></param>
        ''' <returns></returns>
        Public Function GetResponse(ByRef Userinput As String, Response As String) As Boolean
            GetResponse = False
        End Function



    End Class
End Namespace

