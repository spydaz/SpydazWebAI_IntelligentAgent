Imports SpydazWebAI_IntelligentAgent.AI_AGENT
Imports SpydazWebAI_IntelligentAgent.AI_AGENT.Devices

Namespace Clients

    ''' <summary>
    ''' THis Class Enables for a Consumer of the Intelligent Agent Class To Interact With the
    ''' Agent By Sending a UserInput to the Agent On Receipt of the Response It can be
    ''' republished for use.
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class InteractiveConsumer

        Public WithEvents Agent As New IntelligentAgent

        ''' <summary>
        ''' Data Received from the Agent is Sent to this sub to be handled
        ''' </summary>
        ''' <param name="Response"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub PublishResponse(ByRef Response As String)

        ''' <summary>
        ''' UserInput Received from the Agent is sent to this sub to be handled
        ''' </summary>
        ''' <param name="UserInput"></param>
        ''' <remarks></remarks>
        Public MustOverride Sub PublishUserInput(ByRef UserInput As String)

        ''' <summary>
        ''' Handles Responses From the IntelligentAgent Class
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ResponseRecieved(ByVal Response As String) Handles Agent.AgentResponded
            PublishResponse(Response)
        End Sub

        ''' <summary>
        ''' Sends UserInput to the Intelligent Agent
        ''' </summary>
        ''' <param name="UserInputStr"></param>
        ''' <remarks></remarks>
        Public Sub SendUserInput(ByRef UserInputStr As String)
            Agent.InputText(UserInputStr)
        End Sub

    End Class

    ''' <summary>
    ''' This Class Enables for Userinput updates and Response Updates Published by the
    ''' intelligent agent To be Displayed or Republished for display or use
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Subscriber
        Implements IntelligentAgentResponseObserver

        Public MustOverride Sub PublishResponse(ByRef Response As String)

        Public MustOverride Sub PublishUserInput(ByRef UserInput As String)

        Public Sub UpdateInput(Data As String) Implements IntelligentAgentResponseObserver.UpdateInput
            PublishUserInput(Data)
        End Sub

        Public Sub UpdateResponse(Data As String) Implements IntelligentAgentResponseObserver.UpdateResponse
            PublishResponse(Data)
        End Sub

    End Class

End Namespace