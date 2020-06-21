Namespace AI_AGENT



    ''' <summary>
    ''' MAIN BRAIN RESPONSE Needs to be OverWritten
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Artificial_Intelligence_Brain

        Private MResponse As String = ""
        Private AI As New IntelligentAgent
        Public ReadOnly Property Response As String
            Get
                Return MResponse
            End Get
        End Property

        Public Sub New()

        End Sub
        Public Function GetResponse(ByRef Userinput As String, Response As String) As Boolean
            GetResponse = False


        End Function


    End Class
End Namespace

