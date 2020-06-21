Imports System
Imports SpydazWebAI_IntelligentAgent.AI_AGENT.Devices

Namespace AI_AGENT




    ''' <summary>
    ''' This Class Enables for Subscribers to receive Updates from the class Inputs received from
    ''' sensors and Response generated From evaluation Process
    ''' 1. On Sensor input (Environment.Sensors)
    ''' 2. Check Rules (Evaluate)
    ''' 3. Effect Actuators (Environment.Actuators) This Class Can be used as a foundation For
    ''' Creating Artificial intelligent Interfaces
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IntelligentAgent

        Public Event AgentResponded(ByVal ResponseRecieved As String)

        Public Event SensorTextRecieved(ByRef Recieved As Boolean)

        '---------------------------------------------------------------------------------
        'Sensors - Input
        '---------------------------------------------------------------------------------
        Private WithEvents EnvironmentalSensors As New Environment.Sensors

        '---------------------------------------------------------------------------------
        'Evaluate
        '---------------------------------------------------------------------------------
        Private WithEvents EvaluateResponse As New Evaluate.Goals.Responder

        Private mAttactchedInputDevices As New List(Of InputDevice)

        ''' <summary>
        ''' OutputDevices Attached can be added to this list to be actuated
        ''' </summary>
        ''' <remarks></remarks>
        Private mAttatchedOutputDevices As New List(Of OutputDevice)

        Private MinputDevices As Boolean

        '---------------------------------------------------------------------------------------
        'DEVICES : Enable for New Devices to be Added to the AgentModel
        '---------------------------------------------------------------------------------------
        Private MOutputDevices As Boolean = False

        Private mSensorText As String

        '---------------------------------------------------------------------------------------
        'ResponseUpdate Clients
        'Subscriber/Publisher Interface (Publisher)
        '---------------------------------------------------------------------------------------
        ''' <summary>
        ''' Maintains a list of clients receiving Response Updates
        ''' </summary>
        Private ResponseObservers As New List(Of IntelligentAgentResponseObserver)

        'SpeechSynth : Generally the Actuator Functions would be private and automated
        '          But For Consumption they are shared and activated as required
        Private Speech As New Environment.Actuators.Actions.ComputerSpeechOut

        Public ReadOnly Property AttachedInputDevices As List(Of InputDevice)
            Get
                Return mAttactchedInputDevices

            End Get

        End Property

        Public Property AttachedOutputDevices As List(Of OutputDevice)
            Get
                Return mAttatchedOutputDevices
            End Get
            Set(value As List(Of OutputDevice))
                mAttatchedOutputDevices = value
            End Set
        End Property

        Public ReadOnly Property InputDevicesLoaded As Boolean
            Get
                Return MinputDevices
            End Get
        End Property

        Public ReadOnly Property OutputDevicesLoaded As Boolean
            Get
                Return MOutputDevices
            End Get
        End Property

        Public ReadOnly Property SensorText As String
            Get
                Return mSensorText
            End Get
        End Property

        Public ReadOnly Property SpeechLoaded As Boolean
            Get
                Return Speech.Req
            End Get
        End Property

        Public Sub New()
            DisableSpeechReco()


        End Sub

        ''' <summary>
        ''' Interactions with the environmental Products
        ''' </summary>
        ''' <remarks></remarks>
        Public Class Environment

            ''' <summary>
            ''' Actions to be performed in the environment
            ''' </summary>
            Public Class Actuators

                ''' <summary>
                ''' Actions can be modeled from this class
                ''' </summary>
                Public Class Actions

                    Public Class ComputerSpeechOut

                        Private mNeeded As Boolean = False

                        Public ReadOnly Property Req As Boolean
                            Get
                                Return mNeeded
                            End Get
                        End Property

                        Public Sub Activate()
                            mNeeded = True
                        End Sub

                        Public Sub DeActivate()
                            mNeeded = False
                        End Sub

                        Public Sub SpeakText(ByRef Txt As String)
                            If Req = True Then

                                Dim Speechout As New Speech.Synthesis.SpeechSynthesizer
                                Speechout.Speak(Txt)
                            Else

                            End If
                        End Sub

                    End Class

                End Class

            End Class

            ''' <summary>
            ''' Receives Signals from outside sensors, To send input a reference to the instance of
            ''' the agent model sensor class for each sensor in the class new sensors can be created
            ''' by shadowing this class
            ''' </summary>
            Public Class Sensors

                Public Event SensorsRecievedText()

                ''' <summary>
                ''' Speech recognition Sensor
                ''' </summary>
                ''' <remarks></remarks>
                Private WithEvents Speech As New SpeechSensor

                ''' <summary>
                ''' Text Sensor Which can be used as an input
                ''' </summary>
                ''' <remarks></remarks>
                Private WithEvents Text As New TextSensor

                Private NewSensorText As String

                Public ReadOnly Property SensorText As String
                    Get
                        Return NewSensorText
                    End Get
                End Property

                Public Property SpeechRecognitionEnabled As Boolean = False

                Public Class SpeechSensor

                    ''' <summary>
                    ''' Speech Sensor received text
                    ''' </summary>
                    Public Event OutputTextChanged()

                    ''' <summary>
                    ''' event handler for SPeech recognition interface
                    ''' </summary>
                    Private WithEvents Speech As SpeechRecognition

                    ''' <summary>
                    ''' Variable for Received text
                    ''' </summary>
                    Private mRecievedtext As String

                    ''' <summary>
                    ''' Text received
                    ''' </summary>
                    ''' <returns></returns>
                    Public Property RecievedText As String
                        Get
                            Return mRecievedtext
                        End Get
                        Set(value As String)
                            RaiseEvent OutputTextChanged()
                            mRecievedtext = value
                        End Set
                    End Property

                    Public Sub New()
                        StartListener()
                    End Sub

                    Private Class SpeechRecognition

                        Public Event OutputTextChanged(ByVal Output As Boolean)

                        ''' <summary>
                        ''' enables voice recognition engine
                        ''' </summary>
                        ''' <remarks></remarks>
                        Private WithEvents RecoEngine As New Speech.Recognition.SpeechRecognitionEngine()

                        Private mOutputText As String = ""

                        ''' <summary>
                        ''' Text recived from Speech recognition
                        ''' </summary>
                        ''' <returns></returns>
                        Public Property OutputText As String
                            Get
                                Return mOutputText
                            End Get
                            Set(value As String)
                                mOutputText = value
                                RaiseEvent OutputTextChanged(True)
                            End Set
                        End Property

                        Public Sub New()
                            SetupRecognition()
                        End Sub

                        ''' <summary>
                        ''' Activated on completion of recognition
                        ''' </summary>
                        ''' <param name="sender"></param>
                        ''' <param name="e">     </param>
                        ''' <remarks></remarks>
                        Private Sub RecoEngine_RecognizeCompleted(ByVal sender As Object, ByVal e As System.Speech.Recognition.RecognizeCompletedEventArgs) Handles RecoEngine.RecognizeCompleted
                            RecoEngine.RecognizeAsync()
                        End Sub

                        ''' <summary>
                        ''' On recognizing speech the input is sent to the response routine
                        ''' </summary>
                        ''' <param name="sender"></param>
                        ''' <param name="e">     </param>
                        ''' <remarks></remarks>
                        Private Sub RecoEngine_SpeechRecognized(ByVal sender As Object, ByVal e As System.Speech.Recognition.RecognitionEventArgs) Handles RecoEngine.SpeechRecognized
                            'a call to act is required here
                            OutputText = e.Result.Text
                        End Sub

                        Private Sub SetupRecognition()
                            RecoEngine.LoadGrammar(New Speech.Recognition.DictationGrammar())
                            RecoEngine.SetInputToDefaultAudioDevice()
                            RecoEngine.RecognizeAsync()
                        End Sub

                    End Class

                    ''' <summary>
                    ''' Event handler for new speech recognized as texts
                    ''' </summary>
                    Private Sub SpeechRecieved() Handles Speech.OutputTextChanged
                        RecievedText = Speech.OutputText

                    End Sub

                    ''' <summary>
                    ''' Starts the speech Sensor Receiver
                    ''' </summary>
                    Private Sub StartListener()
                        Speech = New SpeechRecognition
                    End Sub

                    ''' <summary>
                    ''' Outputs Speech to Sensor
                    ''' </summary>
                    ''' <param name="NewText"></param>
                    Public Shared Sub SpeakText(ByRef NewText As String)
                        Dim SpeechSynth As New Speech.Synthesis.SpeechSynthesizer
                        SpeechSynth.Speak(NewText)
                    End Sub

                End Class

                Public Class TextSensor

                    Public Event TextRecieved()

                    Private NewSensorText As String

                    Public Property SensorText As String
                        Get
                            Return NewSensorText
                        End Get
                        Set(value As String)
                            NewSensorText = value
                            RaiseEvent TextRecieved()
                        End Set
                    End Property

                End Class

                Private Sub SpeechRecieved() Handles Speech.OutputTextChanged
                    If SpeechRecognitionEnabled = True Then
                        NewSensorText = Speech.RecievedText
                        RaiseEvent SensorsRecievedText()
                    Else

                    End If

                End Sub

                Private Sub TextRecieved() Handles Text.TextRecieved
                    NewSensorText = Text.SensorText
                    RaiseEvent SensorsRecievedText()
                End Sub

                ''' <summary>
                ''' Enables for Manual TextInput
                ''' </summary>
                ''' <param name="InputStr"></param>
                ''' <remarks></remarks>
                Public Sub InputText(ByRef InputStr As String)
                    Text.SensorText = InputStr

                End Sub

            End Class

        End Class

        '----------------------------------------------------------------------------------------
        'AGENT MAIN
        '----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Evaluate sensor information , Return action to be completed evaluation has target goals
        ''' as well as preset goals percepts are potentially allowable tolerance levels
        ''' </summary>
        Public Class Evaluate
            '1. Get perceptions
            '   a. Define Properties
            '   b. Produce Requested Analysis
            '2. Check Goals
            '   a. Perform Actions
            '   b. Evaluate Responses
            '   c. Return Response

            ''' <summary>
            ''' Goals can be modeled using this class Agent Goals
            ''' </summary>
            Public Class Goals

                ''' <summary>
                ''' Generates Response Based on GetResponseFunction
                ''' </summary>
                ''' <remarks></remarks>
                Public Class Responder

                    Public Event ResponseFound()

                    Private mResponse As String

                    Public ReadOnly Property Response As String
                        Get
                            Return mResponse
                        End Get
                    End Property

                    ''' <summary>
                    ''' Brain Function:
                    '''
                    ''' Provides an Output in xResponse and Response in Class And Raises Event ResponseFound
                    ''' </summary>
                    ''' <param name="UserInput"></param>
                    ''' <remarks></remarks>
                    Public Sub GetResponse(ByRef UserInput As String)
                        'Response to be populated
                        Dim XResponse As String = ""

                        'SubMain Area to Create Brain Functions
                        '--------------------------------------

                        Dim AI As New Artificial_Intelligence_Brain()
                        AI.GetResponse(UserInput, XResponse)

                        '--------------------------------------
                        'Must KEEP:
                        '--------------------------------------
                        'Populate Response if none is Found
                        If XResponse = "" Then XResponse = UserInput & "?"
                        'Raise event that response is populated
                        mResponse = XResponse
                        RaiseEvent ResponseFound()

                    End Sub

                End Class

            End Class

            ''' <summary>
            ''' Percepts can be modeled from this class Comparative Sensor Markers/Milestones;
            ''' Analysis of input data to produce Results to be evaluated or displayed.
            ''' </summary>
            Public Class Percepts

                'Evaluate Text : Actions Learning
                'Respond

            End Class

        End Class

        ''' <summary>
        ''' Effects all Activated Actuators
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub EffectActuators(ByRef Response As String)
            'SpeechReQ?
            If Speech.Req = True Then
                EffectActuatorsSpeech(Response)
            Else
            End If

            EffectActuatorsDevices(Response)

            'Notify All Observers
            NotifyResponseObserver(Response)

        End Sub

        ''' <summary>
        ''' Effects the output devices Loaded
        ''' </summary>
        ''' <param name="mText"></param>
        ''' <remarks></remarks>
        Private Sub EffectActuatorsDevices(ByRef mText As String)
            For Each device As OutputDevice In AttachedOutputDevices
                If device.Loaded = True Then
                    device.EffectActuators(mText)
                End If
            Next
        End Sub

        ''' <summary>
        ''' Evaluates Text Received
        ''' </summary>
        ''' <param name="RecivedText">If set to <see langword="true"/>, then ; otherwise, .</param>
        ''' <remarks></remarks>
        Private Sub EvaluateText(ByRef RecivedText As Boolean) Handles Me.SensorTextRecieved
            '2. Check Rules         <Evaluate>
            'Notify Input
            NotifyInputObserver(SensorText)
            'Evaluate input get O
            Dim Response As String = ""
            EvaluateResponse.GetResponse(SensorText)

        End Sub

        Private Sub NotifyInputObserver(ByVal Data As String)
            If ResponseObservers IsNot Nothing Then
                For Each observer As IntelligentAgentResponseObserver In ResponseObservers
                    observer.UpdateInput(Data)
                Next
            Else
            End If

        End Sub

        Private Sub NotifyResponseObserver(ByVal Data As String)
            If ResponseObservers IsNot Nothing Then
                For Each observer As IntelligentAgentResponseObserver In ResponseObservers
                    observer.UpdateResponse(Data)
                Next
            Else
            End If
        End Sub

        Private Sub ResponseRecieved() Handles EvaluateResponse.ResponseFound
            EffectActuators(EvaluateResponse.Response)
            RaiseEvent AgentResponded(EvaluateResponse.Response)
        End Sub

        Private Sub UpdateSensorText() Handles EnvironmentalSensors.SensorsRecievedText
            mSensorText = EnvironmentalSensors.SensorText
            RaiseEvent SensorTextRecieved(True)
        End Sub

        Public Sub ActivateInputDevices()
            If mAttactchedInputDevices IsNot Nothing Then
                For Each device As InputDevice In mAttactchedInputDevices
                    device.Activate()
                Next
            Else
            End If

        End Sub

        Public Sub AddInputDevice(ByRef NewDevice As InputDevice)
            mAttactchedInputDevices.Add(NewDevice)
        End Sub

        Public Sub AddOutputDevice(ByRef mDevice As OutputDevice)
            mAttatchedOutputDevices.Add(mDevice)
        End Sub

        Public Sub DeActivateInputDevices()
            If mAttactchedInputDevices IsNot Nothing Then
                For Each device As InputDevice In mAttactchedInputDevices
                    device.Deactivate()
                Next
            Else
            End If

        End Sub

        Public Sub DisableSpeechReco()
            EnvironmentalSensors.SpeechRecognitionEnabled = False

        End Sub

        ''' <summary>
        ''' Activates speech
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EffectActuatorsActivateSpeech()
            Speech.Activate()
        End Sub

        ''' <summary>
        ''' DeActivates Speech
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub EffectActuatorsDeActivateSpeech()
            Speech.DeActivate()
        End Sub

        ''' <summary>
        ''' Must be activated
        ''' </summary>
        ''' <param name="InputStr"></param>
        ''' <remarks></remarks>
        Public Sub EffectActuatorsSpeech(ByRef InputStr As String)
            Speech.SpeakText(InputStr)
        End Sub

        Public Sub EnableSpeechReco()
            EnvironmentalSensors.SpeechRecognitionEnabled = True

        End Sub

        ''' <summary>
        ''' Activates TextSensor
        ''' </summary>
        ''' <param name="InputStr"></param>
        ''' <remarks></remarks>
        Public Sub InputText(ByRef InputStr As String)
            EnvironmentalSensors.InputText(InputStr)
            RaiseEvent SensorTextRecieved(True)
        End Sub

        Public Sub LoadOutputDevices()
            If AttachedOutputDevices IsNot Nothing Then
                For Each device As OutputDevice In AttachedOutputDevices
                    device.load()
                Next
            Else
            End If

        End Sub

        Public Sub RegisterResponseObserver(ByVal mObserver As IntelligentAgentResponseObserver)
            Try
                ResponseObservers.Add(mObserver)
            Catch ex As Exception

            End Try

        End Sub

        Public Sub RemoveResponseObserver(ByVal mObserver As IntelligentAgentResponseObserver)
            Try
                ResponseObservers.Remove(mObserver)
            Catch ex As Exception

            End Try

        End Sub

        Public Sub UnloadOutputDevices()
            If AttachedOutputDevices IsNot Nothing Then
                For Each device As OutputDevice In AttachedOutputDevices
                    device.Unload()
                Next
            Else
            End If

        End Sub

    End Class

End Namespace
