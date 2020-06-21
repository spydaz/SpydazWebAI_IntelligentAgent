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
        ''' <summary>
        ''' Indicates Userinput Populated
        ''' </summary>
        ''' <param name="Sender"></param>
        ''' <param name="Changed"></param>
        Public Event UserInputChanged(ByRef Sender As InteractiveConsumer, ByRef Changed As Boolean)
        ''' <summary>
        ''' Indicates Response Populated with change
        ''' </summary>
        ''' <param name="Sender"></param>
        ''' <param name="Changed"></param>
        Public Event ResponseChanged(ByRef Sender As InteractiveConsumer, ByRef Changed As Boolean)

        Private WithEvents Agent As New IntelligentAgent

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
            RaiseEvent ResponseChanged(Me, True)
        End Sub

        ''' <summary>
        ''' Sends UserInput to the Intelligent Agent
        ''' </summary>
        ''' <param name="UserInputStr"></param>
        ''' <remarks></remarks>
        Public Sub SendUserInput(ByRef UserInputStr As String)
            Agent.InputText(UserInputStr)
            UpdateInput(UserInputStr)
        End Sub


        ''' <summary>
        ''' Called By the Intelligent Agent to Activate UserInput Change
        ''' </summary>
        ''' <param name="Data"></param>
        Public Sub UpdateInput(Data As String)
            PublishUserInput(Data)
            RaiseEvent UserInputChanged(Me, True)
        End Sub
    End Class

    ''' <summary>
    ''' This Class Enables for Userinput updates and Response Updates Published by the
    ''' intelligent agent To be Displayed or Republished for display or use
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class Subscriber
        Implements IntelligentAgentResponseObserver
        ''' <summary>
        ''' Indicates Userinput Populated
        ''' </summary>
        ''' <param name="Sender"></param>
        ''' <param name="Changed"></param>
        Public Event UserInputChanged(ByRef Sender As Subscriber, ByRef Changed As Boolean)
        ''' <summary>
        ''' Indicates Response Populated with change
        ''' </summary>
        ''' <param name="Sender"></param>
        ''' <param name="Changed"></param>
        Public Event ResponseChanged(ByRef Sender As Subscriber, ByRef Changed As Boolean)

        ''' <summary>
        ''' Used to publish response to Subscriber to be overWritten by subscriber UI
        ''' </summary>
        ''' <param name="Response"></param>
        Public MustOverride Sub PublishResponse(ByRef Response As String)
        ''' <summary>
        ''' Publishes the user input to the subscriber client to be over written by the subscriber ui
        ''' </summary>
        ''' <param name="UserInput"></param>
        Public MustOverride Sub PublishUserInput(ByRef UserInput As String)
        ''' <summary>
        ''' Called By the Intelligent Agent to Activate UserInput Change
        ''' </summary>
        ''' <param name="Data"></param>
        Public Sub UpdateInput(Data As String) Implements IntelligentAgentResponseObserver.UpdateInput
            PublishUserInput(Data)
            RaiseEvent UserInputChanged(Me, True)
        End Sub
        ''' <summary>
        ''' Called by Intelligent agent to Update response events
        ''' </summary>
        ''' <param name="Data"></param>
        Public Sub UpdateResponse(Data As String) Implements IntelligentAgentResponseObserver.UpdateResponse
            PublishResponse(Data)
            RaiseEvent ResponseChanged(Me, True)
        End Sub

    End Class

End Namespace