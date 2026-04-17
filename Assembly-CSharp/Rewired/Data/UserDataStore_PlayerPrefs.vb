Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports Rewired.Utils.Libraries.TinyJson
Imports UnityEngine

Namespace Rewired.Data
	' Token: 0x02000C53 RID: 3155
	Public Class UserDataStore_PlayerPrefs
		Inherits UserDataStore

		' Token: 0x170007E6 RID: 2022
		' (get) Token: 0x06004E09 RID: 19977 RVA: 0x0027838F File Offset: 0x0027678F
		' (set) Token: 0x06004E0A RID: 19978 RVA: 0x00278397 File Offset: 0x00276797
		Public Property IsEnabled As Boolean
			Get
				Return Me.isEnabled
			End Get
			Set(value As Boolean)
				Me.isEnabled = value
			End Set
		End Property

		' Token: 0x170007E7 RID: 2023
		' (get) Token: 0x06004E0B RID: 19979 RVA: 0x002783A0 File Offset: 0x002767A0
		' (set) Token: 0x06004E0C RID: 19980 RVA: 0x002783A8 File Offset: 0x002767A8
		Public Property LoadDataOnStart As Boolean
			Get
				Return Me.loadDataOnStart
			End Get
			Set(value As Boolean)
				Me.loadDataOnStart = value
			End Set
		End Property

		' Token: 0x170007E8 RID: 2024
		' (get) Token: 0x06004E0D RID: 19981 RVA: 0x002783B1 File Offset: 0x002767B1
		' (set) Token: 0x06004E0E RID: 19982 RVA: 0x002783B9 File Offset: 0x002767B9
		Public Property LoadJoystickAssignments As Boolean
			Get
				Return Me.loadJoystickAssignments
			End Get
			Set(value As Boolean)
				Me.loadJoystickAssignments = value
			End Set
		End Property

		' Token: 0x170007E9 RID: 2025
		' (get) Token: 0x06004E0F RID: 19983 RVA: 0x002783C2 File Offset: 0x002767C2
		' (set) Token: 0x06004E10 RID: 19984 RVA: 0x002783CA File Offset: 0x002767CA
		Public Property LoadKeyboardAssignments As Boolean
			Get
				Return Me.loadKeyboardAssignments
			End Get
			Set(value As Boolean)
				Me.loadKeyboardAssignments = value
			End Set
		End Property

		' Token: 0x170007EA RID: 2026
		' (get) Token: 0x06004E11 RID: 19985 RVA: 0x002783D3 File Offset: 0x002767D3
		' (set) Token: 0x06004E12 RID: 19986 RVA: 0x002783DB File Offset: 0x002767DB
		Public Property LoadMouseAssignments As Boolean
			Get
				Return Me.loadMouseAssignments
			End Get
			Set(value As Boolean)
				Me.loadMouseAssignments = value
			End Set
		End Property

		' Token: 0x170007EB RID: 2027
		' (get) Token: 0x06004E13 RID: 19987 RVA: 0x002783E4 File Offset: 0x002767E4
		' (set) Token: 0x06004E14 RID: 19988 RVA: 0x002783EC File Offset: 0x002767EC
		Public Property PlayerPrefsKeyPrefix As String
			Get
				Return Me.playerPrefsKeyPrefix
			End Get
			Set(value As String)
				Me.playerPrefsKeyPrefix = value
			End Set
		End Property

		' Token: 0x170007EC RID: 2028
		' (get) Token: 0x06004E15 RID: 19989 RVA: 0x002783F5 File Offset: 0x002767F5
		Private ReadOnly Property playerPrefsKey_controllerAssignments As String
			Get
				Return String.Format("{0}_{1}", Me.playerPrefsKeyPrefix, "ControllerAssignments")
			End Get
		End Property

		' Token: 0x170007ED RID: 2029
		' (get) Token: 0x06004E16 RID: 19990 RVA: 0x0027840C File Offset: 0x0027680C
		Private ReadOnly Property loadControllerAssignments As Boolean
			Get
				Return Me.loadKeyboardAssignments OrElse Me.loadMouseAssignments OrElse Me.loadJoystickAssignments
			End Get
		End Property

		' Token: 0x06004E17 RID: 19991 RVA: 0x0027842D File Offset: 0x0027682D
		Public Overrides Sub Save()
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SaveAll()
		End Sub

		' Token: 0x06004E18 RID: 19992 RVA: 0x0027844C File Offset: 0x0027684C
		Public Overrides Overloads Sub SaveControllerData(playerId As Integer, controllerType As ControllerType, controllerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SaveControllerDataNow(playerId, controllerType, controllerId)
		End Sub

		' Token: 0x06004E19 RID: 19993 RVA: 0x0027846E File Offset: 0x0027686E
		Public Overrides Overloads Sub SaveControllerData(controllerType As ControllerType, controllerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SaveControllerDataNow(controllerType, controllerId)
		End Sub

		' Token: 0x06004E1A RID: 19994 RVA: 0x0027848F File Offset: 0x0027688F
		Public Overrides Sub SavePlayerData(playerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SavePlayerDataNow(playerId)
		End Sub

		' Token: 0x06004E1B RID: 19995 RVA: 0x002784AF File Offset: 0x002768AF
		Public Overrides Sub SaveInputBehavior(playerId As Integer, behaviorId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SaveInputBehaviorNow(playerId, behaviorId)
		End Sub

		' Token: 0x06004E1C RID: 19996 RVA: 0x002784D0 File Offset: 0x002768D0
		Public Overrides Sub Load()
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadAll()
		End Sub

		' Token: 0x06004E1D RID: 19997 RVA: 0x002784FC File Offset: 0x002768FC
		Public Overrides Overloads Sub LoadControllerData(playerId As Integer, controllerType As ControllerType, controllerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadControllerDataNow(playerId, controllerType, controllerId)
		End Sub

		' Token: 0x06004E1E RID: 19998 RVA: 0x0027852C File Offset: 0x0027692C
		Public Overrides Overloads Sub LoadControllerData(controllerType As ControllerType, controllerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadControllerDataNow(controllerType, controllerId)
		End Sub

		' Token: 0x06004E1F RID: 19999 RVA: 0x0027855C File Offset: 0x0027695C
		Public Overrides Sub LoadPlayerData(playerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadPlayerDataNow(playerId)
		End Sub

		' Token: 0x06004E20 RID: 20000 RVA: 0x00278588 File Offset: 0x00276988
		Public Overrides Sub LoadInputBehavior(playerId As Integer, behaviorId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("Rewired: UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadInputBehaviorNow(playerId, behaviorId)
		End Sub

		' Token: 0x06004E21 RID: 20001 RVA: 0x002785B5 File Offset: 0x002769B5
		Protected Overrides Sub OnInitialize()
			If Me.loadDataOnStart Then
				Me.Load()
				If Me.loadControllerAssignments AndAlso ReInput.controllers.joystickCount > 0 Then
					Me.SaveControllerAssignments()
				End If
			End If
		End Sub

		' Token: 0x06004E22 RID: 20002 RVA: 0x002785EC File Offset: 0x002769EC
		Protected Overrides Sub OnControllerConnected(args As ControllerStatusChangedEventArgs)
			If Not Me.isEnabled Then
				Return
			End If
			If args.controllerType = ControllerType.Joystick Then
				Dim num As Integer = Me.LoadJoystickData(args.controllerId)
				If Me.loadDataOnStart AndAlso Me.loadJoystickAssignments AndAlso Not Me.wasJoystickEverDetected Then
					MyBase.StartCoroutine(Me.LoadJoystickAssignmentsDeferred())
				End If
				If Me.loadJoystickAssignments AndAlso Not Me.deferredJoystickAssignmentLoadPending Then
					Me.SaveControllerAssignments()
				End If
				Me.wasJoystickEverDetected = True
			End If
		End Sub

		' Token: 0x06004E23 RID: 20003 RVA: 0x00278670 File Offset: 0x00276A70
		Protected Overrides Sub OnControllerPreDiscconnect(args As ControllerStatusChangedEventArgs)
			If Not Me.isEnabled Then
				Return
			End If
			If args.controllerType = ControllerType.Joystick Then
				Me.SaveJoystickData(args.controllerId)
			End If
		End Sub

		' Token: 0x06004E24 RID: 20004 RVA: 0x00278696 File Offset: 0x00276A96
		Protected Overrides Sub OnControllerDisconnected(args As ControllerStatusChangedEventArgs)
			If Not Me.isEnabled Then
				Return
			End If
			If Me.loadControllerAssignments Then
				Me.SaveControllerAssignments()
			End If
		End Sub

		' Token: 0x06004E25 RID: 20005 RVA: 0x002786B8 File Offset: 0x00276AB8
		Private Function LoadAll() As Integer
			Dim num As Integer = 0
			If Me.loadControllerAssignments AndAlso Me.LoadControllerAssignmentsNow() Then
				num += 1
			End If
			Dim allPlayers As IList(Of Player) = ReInput.players.AllPlayers
			For i As Integer = 0 To allPlayers.Count - 1
				num += Me.LoadPlayerDataNow(allPlayers(i))
			Next
			Return num + Me.LoadAllJoystickCalibrationData()
		End Function

		' Token: 0x06004E26 RID: 20006 RVA: 0x0027871D File Offset: 0x00276B1D
		Private Function LoadPlayerDataNow(playerId As Integer) As Integer
			Return Me.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId))
		End Function

		' Token: 0x06004E27 RID: 20007 RVA: 0x00278730 File Offset: 0x00276B30
		Private Function LoadPlayerDataNow(player As Player) As Integer
			If player Is Nothing Then
				Return 0
			End If
			Dim num As Integer = 0
			num += Me.LoadInputBehaviors(player.id)
			num += Me.LoadControllerMaps(player.id, ControllerType.Keyboard, 0)
			num += Me.LoadControllerMaps(player.id, ControllerType.Mouse, 0)
			For Each joystick As Joystick In player.controllers.Joysticks
				num += Me.LoadControllerMaps(player.id, ControllerType.Joystick, joystick.id)
			Next
			Return num
		End Function

		' Token: 0x06004E28 RID: 20008 RVA: 0x002787DC File Offset: 0x00276BDC
		Private Function LoadAllJoystickCalibrationData() As Integer
			Dim num As Integer = 0
			Dim joysticks As IList(Of Joystick) = ReInput.controllers.Joysticks
			For i As Integer = 0 To joysticks.Count - 1
				num += Me.LoadJoystickCalibrationData(joysticks(i))
			Next
			Return num
		End Function

		' Token: 0x06004E29 RID: 20009 RVA: 0x0027881E File Offset: 0x00276C1E
		Private Function LoadJoystickCalibrationData(joystick As Joystick) As Integer
			If joystick Is Nothing Then
				Return 0
			End If
			Return If((Not joystick.ImportCalibrationMapFromXmlString(Me.GetJoystickCalibrationMapXml(joystick))), 0, 1)
		End Function

		' Token: 0x06004E2A RID: 20010 RVA: 0x00278841 File Offset: 0x00276C41
		Private Function LoadJoystickCalibrationData(joystickId As Integer) As Integer
			Return Me.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId))
		End Function

		' Token: 0x06004E2B RID: 20011 RVA: 0x00278854 File Offset: 0x00276C54
		Private Function LoadJoystickData(joystickId As Integer) As Integer
			Dim num As Integer = 0
			Dim allPlayers As IList(Of Player) = ReInput.players.AllPlayers
			For i As Integer = 0 To allPlayers.Count - 1
				Dim player As Player = allPlayers(i)
				If player.controllers.ContainsController(ControllerType.Joystick, joystickId) Then
					num += Me.LoadControllerMaps(player.id, ControllerType.Joystick, joystickId)
				End If
			Next
			Return num + Me.LoadJoystickCalibrationData(joystickId)
		End Function

		' Token: 0x06004E2C RID: 20012 RVA: 0x002788C0 File Offset: 0x00276CC0
		Private Function LoadControllerDataNow(playerId As Integer, controllerType As ControllerType, controllerId As Integer) As Integer
			Dim num As Integer = 0
			num += Me.LoadControllerMaps(playerId, controllerType, controllerId)
			Return num + Me.LoadControllerDataNow(controllerType, controllerId)
		End Function

		' Token: 0x06004E2D RID: 20013 RVA: 0x002788E8 File Offset: 0x00276CE8
		Private Function LoadControllerDataNow(controllerType As ControllerType, controllerId As Integer) As Integer
			Dim num As Integer = 0
			If controllerType = ControllerType.Joystick Then
				num += Me.LoadJoystickCalibrationData(controllerId)
			End If
			Return num
		End Function

		' Token: 0x06004E2E RID: 20014 RVA: 0x0027890C File Offset: 0x00276D0C
		Private Function LoadControllerMaps(playerId As Integer, controllerType As ControllerType, controllerId As Integer) As Integer
			Dim num As Integer = 0
			Dim player As Player = ReInput.players.GetPlayer(playerId)
			If player Is Nothing Then
				Return num
			End If
			Dim controller As Controller = ReInput.controllers.GetController(controllerType, controllerId)
			If controller Is Nothing Then
				Return num
			End If
			Dim allControllerMapsXml As List(Of UserDataStore_PlayerPrefs.SavedControllerMapData) = Me.GetAllControllerMapsXml(player, True, controller)
			If allControllerMapsXml.Count = 0 Then
				Return num
			End If
			num += player.controllers.maps.AddMapsFromXml(controllerType, controllerId, UserDataStore_PlayerPrefs.SavedControllerMapData.GetXmlStringList(allControllerMapsXml))
			Me.AddDefaultMappingsForNewActions(player, allControllerMapsXml, controllerType, controllerId)
			Return num
		End Function

		' Token: 0x06004E2F RID: 20015 RVA: 0x00278984 File Offset: 0x00276D84
		Private Function LoadInputBehaviors(playerId As Integer) As Integer
			Dim player As Player = ReInput.players.GetPlayer(playerId)
			If player Is Nothing Then
				Return 0
			End If
			Dim num As Integer = 0
			Dim inputBehaviors As IList(Of InputBehavior) = ReInput.mapping.GetInputBehaviors(player.id)
			For i As Integer = 0 To inputBehaviors.Count - 1
				num += Me.LoadInputBehaviorNow(player, inputBehaviors(i))
			Next
			Return num
		End Function

		' Token: 0x06004E30 RID: 20016 RVA: 0x002789E4 File Offset: 0x00276DE4
		Private Function LoadInputBehaviorNow(playerId As Integer, behaviorId As Integer) As Integer
			Dim player As Player = ReInput.players.GetPlayer(playerId)
			If player Is Nothing Then
				Return 0
			End If
			Dim inputBehavior As InputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId)
			If inputBehavior Is Nothing Then
				Return 0
			End If
			Return Me.LoadInputBehaviorNow(player, inputBehavior)
		End Function

		' Token: 0x06004E31 RID: 20017 RVA: 0x00278A24 File Offset: 0x00276E24
		Private Function LoadInputBehaviorNow(player As Player, inputBehavior As InputBehavior) As Integer
			If player Is Nothing OrElse inputBehavior Is Nothing Then
				Return 0
			End If
			Dim inputBehaviorXml As String = Me.GetInputBehaviorXml(player, inputBehavior.id)
			If inputBehaviorXml Is Nothing OrElse inputBehaviorXml = String.Empty Then
				Return 0
			End If
			Return If((Not inputBehavior.ImportXmlString(inputBehaviorXml)), 0, 1)
		End Function

		' Token: 0x06004E32 RID: 20018 RVA: 0x00278A78 File Offset: 0x00276E78
		Private Function LoadControllerAssignmentsNow() As Boolean
			Try
				Dim controllerAssignmentSaveInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo = Me.LoadControllerAssignmentData()
				If controllerAssignmentSaveInfo Is Nothing Then
					Return False
				End If
				If Me.loadKeyboardAssignments OrElse Me.loadMouseAssignments Then
					Me.LoadKeyboardAndMouseAssignmentsNow(controllerAssignmentSaveInfo)
				End If
				If Me.loadJoystickAssignments Then
					Me.LoadJoystickAssignmentsNow(controllerAssignmentSaveInfo)
				End If
			Catch
			End Try
			Return True
		End Function

		' Token: 0x06004E33 RID: 20019 RVA: 0x00278AE8 File Offset: 0x00276EE8
		Private Function LoadKeyboardAndMouseAssignmentsNow(data As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) As Boolean
			Try
				If data Is Nothing Then
					Dim controllerAssignmentSaveInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo = Me.LoadControllerAssignmentData()
					data = controllerAssignmentSaveInfo
					If controllerAssignmentSaveInfo Is Nothing Then
						Return False
					End If
				End If
				For Each player As Player In ReInput.players.AllPlayers
					If data.ContainsPlayer(player.id) Then
						Dim playerInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo = data.players(data.IndexOfPlayer(player.id))
						If Me.loadKeyboardAssignments Then
							player.controllers.hasKeyboard = playerInfo.hasKeyboard
						End If
						If Me.loadMouseAssignments Then
							player.controllers.hasMouse = playerInfo.hasMouse
						End If
					End If
				Next
			Catch
			End Try
			Return True
		End Function

		' Token: 0x06004E34 RID: 20020 RVA: 0x00278BD8 File Offset: 0x00276FD8
		Private Function LoadJoystickAssignmentsNow(data As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo) As Boolean
			Try
				If ReInput.controllers.joystickCount = 0 Then
					Return False
				End If
				If data Is Nothing Then
					Dim controllerAssignmentSaveInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo = Me.LoadControllerAssignmentData()
					data = controllerAssignmentSaveInfo
					If controllerAssignmentSaveInfo Is Nothing Then
						Return False
					End If
				End If
				For Each player As Player In ReInput.players.AllPlayers
					player.controllers.ClearControllersOfType(ControllerType.Joystick)
				Next
				Dim list As List(Of UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo) = If((Not Me.loadJoystickAssignments), Nothing, New List(Of UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo)())
				For Each player2 As Player In ReInput.players.AllPlayers
					If data.ContainsPlayer(player2.id) Then
						Dim playerInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo = data.players(data.IndexOfPlayer(player2.id))
						For i As Integer = 0 To playerInfo.joystickCount - 1
							Dim joystickInfo2 As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo = playerInfo.joysticks(i)
							If joystickInfo2 IsNot Nothing Then
								Dim joystick As Joystick = Me.FindJoystickPrecise(joystickInfo2)
								If joystick IsNot Nothing Then
									If list.Find(Function(x As UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo) x.joystick Is joystick) Is Nothing Then
										list.Add(New UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick, joystickInfo2.id))
									End If
									player2.controllers.AddController(joystick, False)
								End If
							End If
						Next
					End If
				Next
				If Me.allowImpreciseJoystickAssignmentMatching Then
					For Each player3 As Player In ReInput.players.AllPlayers
						If data.ContainsPlayer(player3.id) Then
							Dim playerInfo2 As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo = data.players(data.IndexOfPlayer(player3.id))
							For j As Integer = 0 To playerInfo2.joystickCount - 1
								Dim joystickInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo = playerInfo2.joysticks(j)
								If joystickInfo IsNot Nothing Then
									Dim joystick2 As Joystick = Nothing
									Dim num As Integer = list.FindIndex(Function(x As UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo) x.oldJoystickId = joystickInfo.id)
									If num >= 0 Then
										joystick2 = list(num).joystick
									Else
										Dim list2 As List(Of Joystick)
										If Not Me.TryFindJoysticksImprecise(joystickInfo, list2) Then
											GoTo IL_030F
										End If
										Using enumerator4 As List(Of Joystick).Enumerator = list2.GetEnumerator()
											While enumerator4.MoveNext()
												Dim match As Joystick = enumerator4.Current
												If list.Find(Function(x As UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo) x.joystick Is match) Is Nothing Then
													joystick2 = match
													Exit While
												End If
											End While
										End Using
										If joystick2 Is Nothing Then
											GoTo IL_030F
										End If
										list.Add(New UserDataStore_PlayerPrefs.JoystickAssignmentHistoryInfo(joystick2, joystickInfo.id))
									End If
									player3.controllers.AddController(joystick2, False)
								End If
								IL_030F:
							Next
						End If
					Next
				End If
			Catch
			End Try
			If ReInput.configuration.autoAssignJoysticks Then
				ReInput.controllers.AutoAssignJoysticks()
			End If
			Return True
		End Function

		' Token: 0x06004E35 RID: 20021 RVA: 0x00278FCC File Offset: 0x002773CC
		Private Function LoadControllerAssignmentData() As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo
			Dim controllerAssignmentSaveInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo
			Try
				If Not PlayerPrefs.HasKey(Me.playerPrefsKey_controllerAssignments) Then
					controllerAssignmentSaveInfo = Nothing
				Else
					Dim [string] As String = PlayerPrefs.GetString(Me.playerPrefsKey_controllerAssignments)
					If String.IsNullOrEmpty([string]) Then
						controllerAssignmentSaveInfo = Nothing
					Else
						Dim controllerAssignmentSaveInfo2 As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo = JsonParser.FromJson(Of UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo)([string])
						If controllerAssignmentSaveInfo2 Is Nothing OrElse controllerAssignmentSaveInfo2.playerCount = 0 Then
							controllerAssignmentSaveInfo = Nothing
						Else
							controllerAssignmentSaveInfo = controllerAssignmentSaveInfo2
						End If
					End If
				End If
			Catch
				controllerAssignmentSaveInfo = Nothing
			End Try
			Return controllerAssignmentSaveInfo
		End Function

		' Token: 0x06004E36 RID: 20022 RVA: 0x00279050 File Offset: 0x00277450
		Private Iterator Function LoadJoystickAssignmentsDeferred() As IEnumerator
			Me.deferredJoystickAssignmentLoadPending = True
			Yield New WaitForEndOfFrame()
			If Not ReInput.isReady Then
				Return
			End If
			If Me.LoadJoystickAssignmentsNow(Nothing) Then
			End If
			Me.SaveControllerAssignments()
			Me.deferredJoystickAssignmentLoadPending = False
			Return
		End Function

		' Token: 0x06004E37 RID: 20023 RVA: 0x0027906C File Offset: 0x0027746C
		Private Sub SaveAll()
			Dim allPlayers As IList(Of Player) = ReInput.players.AllPlayers
			For i As Integer = 0 To allPlayers.Count - 1
				Me.SavePlayerDataNow(allPlayers(i))
			Next
			Me.SaveAllJoystickCalibrationData()
			If Me.loadControllerAssignments Then
				Me.SaveControllerAssignments()
			End If
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004E38 RID: 20024 RVA: 0x002790C5 File Offset: 0x002774C5
		Private Sub SavePlayerDataNow(playerId As Integer)
			Me.SavePlayerDataNow(ReInput.players.GetPlayer(playerId))
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004E39 RID: 20025 RVA: 0x002790E0 File Offset: 0x002774E0
		Private Sub SavePlayerDataNow(player As Player)
			If player Is Nothing Then
				Return
			End If
			Dim saveData As PlayerSaveData = player.GetSaveData(True)
			Me.SaveInputBehaviors(player, saveData)
			Me.SaveControllerMaps(player, saveData)
		End Sub

		' Token: 0x06004E3A RID: 20026 RVA: 0x0027910C File Offset: 0x0027750C
		Private Sub SaveAllJoystickCalibrationData()
			Dim joysticks As IList(Of Joystick) = ReInput.controllers.Joysticks
			For i As Integer = 0 To joysticks.Count - 1
				Me.SaveJoystickCalibrationData(joysticks(i))
			Next
		End Sub

		' Token: 0x06004E3B RID: 20027 RVA: 0x00279148 File Offset: 0x00277548
		Private Sub SaveJoystickCalibrationData(joystickId As Integer)
			Me.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId))
		End Sub

		' Token: 0x06004E3C RID: 20028 RVA: 0x0027915C File Offset: 0x0027755C
		Private Sub SaveJoystickCalibrationData(joystick As Joystick)
			If joystick Is Nothing Then
				Return
			End If
			Dim calibrationMapSaveData As JoystickCalibrationMapSaveData = joystick.GetCalibrationMapSaveData()
			Dim joystickCalibrationMapPlayerPrefsKey As String = Me.GetJoystickCalibrationMapPlayerPrefsKey(joystick)
			PlayerPrefs.SetString(joystickCalibrationMapPlayerPrefsKey, calibrationMapSaveData.map.ToXmlString())
		End Sub

		' Token: 0x06004E3D RID: 20029 RVA: 0x00279190 File Offset: 0x00277590
		Private Sub SaveJoystickData(joystickId As Integer)
			Dim allPlayers As IList(Of Player) = ReInput.players.AllPlayers
			For i As Integer = 0 To allPlayers.Count - 1
				Dim player As Player = allPlayers(i)
				If player.controllers.ContainsController(ControllerType.Joystick, joystickId) Then
					Me.SaveControllerMaps(player.id, ControllerType.Joystick, joystickId)
				End If
			Next
			Me.SaveJoystickCalibrationData(joystickId)
		End Sub

		' Token: 0x06004E3E RID: 20030 RVA: 0x002791F3 File Offset: 0x002775F3
		Private Sub SaveControllerDataNow(playerId As Integer, controllerType As ControllerType, controllerId As Integer)
			Me.SaveControllerMaps(playerId, controllerType, controllerId)
			Me.SaveControllerDataNow(controllerType, controllerId)
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004E3F RID: 20031 RVA: 0x0027920B File Offset: 0x0027760B
		Private Sub SaveControllerDataNow(controllerType As ControllerType, controllerId As Integer)
			If controllerType = ControllerType.Joystick Then
				Me.SaveJoystickCalibrationData(controllerId)
			End If
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004E40 RID: 20032 RVA: 0x00279220 File Offset: 0x00277620
		Private Sub SaveControllerMaps(player As Player, playerSaveData As PlayerSaveData)
			For Each controllerMapSaveData As ControllerMapSaveData In playerSaveData.AllControllerMapSaveData
				Me.SaveControllerMap(player, controllerMapSaveData)
			Next
		End Sub

		' Token: 0x06004E41 RID: 20033 RVA: 0x0027927C File Offset: 0x0027767C
		Private Sub SaveControllerMaps(playerId As Integer, controllerType As ControllerType, controllerId As Integer)
			Dim player As Player = ReInput.players.GetPlayer(playerId)
			If player Is Nothing Then
				Return
			End If
			If Not player.controllers.ContainsController(controllerType, controllerId) Then
				Return
			End If
			Dim mapSaveData As ControllerMapSaveData() = player.controllers.maps.GetMapSaveData(controllerType, controllerId, True)
			If mapSaveData Is Nothing Then
				Return
			End If
			For i As Integer = 0 To mapSaveData.Length - 1
				Me.SaveControllerMap(player, mapSaveData(i))
			Next
		End Sub

		' Token: 0x06004E42 RID: 20034 RVA: 0x002792E8 File Offset: 0x002776E8
		Private Sub SaveControllerMap(player As Player, saveData As ControllerMapSaveData)
			Dim text As String = Me.GetControllerMapPlayerPrefsKey(player, saveData.controller, saveData.categoryId, saveData.layoutId)
			PlayerPrefs.SetString(text, saveData.map.ToXmlString())
			text = Me.GetControllerMapKnownActionIdsPlayerPrefsKey(player, saveData.controller, saveData.categoryId, saveData.layoutId)
			PlayerPrefs.SetString(text, Me.GetAllActionIdsString())
		End Sub

		' Token: 0x06004E43 RID: 20035 RVA: 0x00279348 File Offset: 0x00277748
		Private Sub SaveInputBehaviors(player As Player, playerSaveData As PlayerSaveData)
			If player Is Nothing Then
				Return
			End If
			Dim inputBehaviors As InputBehavior() = playerSaveData.inputBehaviors
			For i As Integer = 0 To inputBehaviors.Length - 1
				Me.SaveInputBehaviorNow(player, inputBehaviors(i))
			Next
		End Sub

		' Token: 0x06004E44 RID: 20036 RVA: 0x00279384 File Offset: 0x00277784
		Private Sub SaveInputBehaviorNow(playerId As Integer, behaviorId As Integer)
			Dim player As Player = ReInput.players.GetPlayer(playerId)
			If player Is Nothing Then
				Return
			End If
			Dim inputBehavior As InputBehavior = ReInput.mapping.GetInputBehavior(playerId, behaviorId)
			If inputBehavior Is Nothing Then
				Return
			End If
			Me.SaveInputBehaviorNow(player, inputBehavior)
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004E45 RID: 20037 RVA: 0x002793C8 File Offset: 0x002777C8
		Private Sub SaveInputBehaviorNow(player As Player, inputBehavior As InputBehavior)
			If player Is Nothing OrElse inputBehavior Is Nothing Then
				Return
			End If
			Dim inputBehaviorPlayerPrefsKey As String = Me.GetInputBehaviorPlayerPrefsKey(player, inputBehavior.id)
			PlayerPrefs.SetString(inputBehaviorPlayerPrefsKey, inputBehavior.ToXmlString())
		End Sub

		' Token: 0x06004E46 RID: 20038 RVA: 0x002793FC File Offset: 0x002777FC
		Private Function SaveControllerAssignments() As Boolean
			Try
				Dim controllerAssignmentSaveInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo = New UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo(ReInput.players.allPlayerCount)
				For i As Integer = 0 To ReInput.players.allPlayerCount - 1
					Dim player As Player = ReInput.players.AllPlayers(i)
					Dim playerInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo = New UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo()
					controllerAssignmentSaveInfo.players(i) = playerInfo
					playerInfo.id = player.id
					playerInfo.hasKeyboard = player.controllers.hasKeyboard
					playerInfo.hasMouse = player.controllers.hasMouse
					Dim array As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo() = New UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo(player.controllers.joystickCount - 1) {}
					playerInfo.joysticks = array
					For j As Integer = 0 To player.controllers.joystickCount - 1
						Dim joystick As Joystick = player.controllers.Joysticks(j)
						array(j) = New UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo() With { .instanceGuid = joystick.deviceInstanceGuid, .id = joystick.id, .hardwareIdentifier = joystick.hardwareIdentifier }
					Next
				Next
				PlayerPrefs.SetString(Me.playerPrefsKey_controllerAssignments, JsonWriter.ToJson(controllerAssignmentSaveInfo))
				PlayerPrefs.Save()
			Catch
			End Try
			Return True
		End Function

		' Token: 0x06004E47 RID: 20039 RVA: 0x00279548 File Offset: 0x00277948
		Private Function ControllerAssignmentSaveDataExists() As Boolean
			If Not PlayerPrefs.HasKey(Me.playerPrefsKey_controllerAssignments) Then
				Return False
			End If
			Dim [string] As String = PlayerPrefs.GetString(Me.playerPrefsKey_controllerAssignments)
			Return Not String.IsNullOrEmpty([string])
		End Function

		' Token: 0x06004E48 RID: 20040 RVA: 0x00279584 File Offset: 0x00277984
		Private Function GetBasePlayerPrefsKey(player As Player) As String
			Dim text As String = Me.playerPrefsKeyPrefix
			Return text + "|playerName=" + player.name
		End Function

		' Token: 0x06004E49 RID: 20041 RVA: 0x002795AC File Offset: 0x002779AC
		Private Function GetControllerMapPlayerPrefsKey(player As Player, controller As Controller, categoryId As Integer, layoutId As Integer) As String
			Dim text As String = Me.GetBasePlayerPrefsKey(player)
			text += "|dataType=ControllerMap"
			text = text + "|controllerMapType=" + controller.mapTypeString
			Dim text2 As String = text
			text = String.Concat(New Object() { text2, "|categoryId=", categoryId, "|layoutId=", layoutId })
			text = text + "|hardwareIdentifier=" + controller.hardwareIdentifier
			If controller.type = ControllerType.Joystick Then
				text = text + "|hardwareGuid=" + CType(controller, Joystick).hardwareTypeGuid.ToString()
			End If
			Return text
		End Function

		' Token: 0x06004E4A RID: 20042 RVA: 0x00279658 File Offset: 0x00277A58
		Private Function GetControllerMapKnownActionIdsPlayerPrefsKey(player As Player, controller As Controller, categoryId As Integer, layoutId As Integer) As String
			Dim text As String = Me.GetBasePlayerPrefsKey(player)
			text += "|dataType=ControllerMap_KnownActionIds"
			text = text + "|controllerMapType=" + controller.mapTypeString
			Dim text2 As String = text
			text = String.Concat(New Object() { text2, "|categoryId=", categoryId, "|layoutId=", layoutId })
			text = text + "|hardwareIdentifier=" + controller.hardwareIdentifier
			If controller.type = ControllerType.Joystick Then
				text = text + "|hardwareGuid=" + CType(controller, Joystick).hardwareTypeGuid.ToString()
			End If
			Return text
		End Function

		' Token: 0x06004E4B RID: 20043 RVA: 0x00279704 File Offset: 0x00277B04
		Private Function GetJoystickCalibrationMapPlayerPrefsKey(joystick As Joystick) As String
			Dim text As String = Me.playerPrefsKeyPrefix
			text += "|dataType=CalibrationMap"
			text = text + "|controllerType=" + joystick.type.ToString()
			text = text + "|hardwareIdentifier=" + joystick.hardwareIdentifier
			Return text + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString()
		End Function

		' Token: 0x06004E4C RID: 20044 RVA: 0x00279778 File Offset: 0x00277B78
		Private Function GetInputBehaviorPlayerPrefsKey(player As Player, inputBehaviorId As Integer) As String
			Dim text As String = Me.GetBasePlayerPrefsKey(player)
			text += "|dataType=InputBehavior"
			Return text + "|id=" + inputBehaviorId
		End Function

		' Token: 0x06004E4D RID: 20045 RVA: 0x002797AC File Offset: 0x00277BAC
		Private Function GetControllerMapXml(player As Player, controller As Controller, categoryId As Integer, layoutId As Integer) As String
			Dim controllerMapPlayerPrefsKey As String = Me.GetControllerMapPlayerPrefsKey(player, controller, categoryId, layoutId)
			If Not PlayerPrefs.HasKey(controllerMapPlayerPrefsKey) Then
				Return String.Empty
			End If
			Return PlayerPrefs.GetString(controllerMapPlayerPrefsKey)
		End Function

		' Token: 0x06004E4E RID: 20046 RVA: 0x002797DC File Offset: 0x00277BDC
		Private Function GetControllerMapKnownActionIds(player As Player, controller As Controller, categoryId As Integer, layoutId As Integer) As List(Of Integer)
			Dim list As List(Of Integer) = New List(Of Integer)()
			Dim controllerMapKnownActionIdsPlayerPrefsKey As String = Me.GetControllerMapKnownActionIdsPlayerPrefsKey(player, controller, categoryId, layoutId)
			If Not PlayerPrefs.HasKey(controllerMapKnownActionIdsPlayerPrefsKey) Then
				Return list
			End If
			Dim [string] As String = PlayerPrefs.GetString(controllerMapKnownActionIdsPlayerPrefsKey)
			If String.IsNullOrEmpty([string]) Then
				Return list
			End If
			Dim array As String() = [string].Split(New Char() { ","c })
			For i As Integer = 0 To array.Length - 1
				If Not String.IsNullOrEmpty(array(i)) Then
					Dim num As Integer
					If Integer.TryParse(array(i), num) Then
						list.Add(num)
					End If
				End If
			Next
			Return list
		End Function

		' Token: 0x06004E4F RID: 20047 RVA: 0x00279874 File Offset: 0x00277C74
		Private Function GetAllControllerMapsXml(player As Player, userAssignableMapsOnly As Boolean, controller As Controller) As List(Of UserDataStore_PlayerPrefs.SavedControllerMapData)
			Dim list As List(Of UserDataStore_PlayerPrefs.SavedControllerMapData) = New List(Of UserDataStore_PlayerPrefs.SavedControllerMapData)()
			Dim mapCategories As IList(Of InputMapCategory) = ReInput.mapping.MapCategories
			For i As Integer = 0 To mapCategories.Count - 1
				Dim inputMapCategory As InputMapCategory = mapCategories(i)
				If Not userAssignableMapsOnly OrElse inputMapCategory.userAssignable Then
					Dim list2 As IList(Of InputLayout) = ReInput.mapping.MapLayouts(controller.type)
					For j As Integer = 0 To list2.Count - 1
						Dim inputLayout As InputLayout = list2(j)
						Dim controllerMapXml As String = Me.GetControllerMapXml(player, controller, inputMapCategory.id, inputLayout.id)
						If Not(controllerMapXml = String.Empty) Then
							Dim controllerMapKnownActionIds As List(Of Integer) = Me.GetControllerMapKnownActionIds(player, controller, inputMapCategory.id, inputLayout.id)
							list.Add(New UserDataStore_PlayerPrefs.SavedControllerMapData(controllerMapXml, controllerMapKnownActionIds))
						End If
					Next
				End If
			Next
			Return list
		End Function

		' Token: 0x06004E50 RID: 20048 RVA: 0x00279954 File Offset: 0x00277D54
		Private Function GetJoystickCalibrationMapXml(joystick As Joystick) As String
			Dim joystickCalibrationMapPlayerPrefsKey As String = Me.GetJoystickCalibrationMapPlayerPrefsKey(joystick)
			If Not PlayerPrefs.HasKey(joystickCalibrationMapPlayerPrefsKey) Then
				Return String.Empty
			End If
			Return PlayerPrefs.GetString(joystickCalibrationMapPlayerPrefsKey)
		End Function

		' Token: 0x06004E51 RID: 20049 RVA: 0x00279980 File Offset: 0x00277D80
		Private Function GetInputBehaviorXml(player As Player, id As Integer) As String
			Dim inputBehaviorPlayerPrefsKey As String = Me.GetInputBehaviorPlayerPrefsKey(player, id)
			If Not PlayerPrefs.HasKey(inputBehaviorPlayerPrefsKey) Then
				Return String.Empty
			End If
			Return PlayerPrefs.GetString(inputBehaviorPlayerPrefsKey)
		End Function

		' Token: 0x06004E52 RID: 20050 RVA: 0x002799B0 File Offset: 0x00277DB0
		Private Sub AddDefaultMappingsForNewActions(player As Player, savedData As List(Of UserDataStore_PlayerPrefs.SavedControllerMapData), controllerType As ControllerType, controllerId As Integer)
			If player Is Nothing OrElse savedData Is Nothing Then
				Return
			End If
			Dim allActionIds As List(Of Integer) = Me.GetAllActionIds()
			For i As Integer = 0 To savedData.Count - 1
				Dim savedControllerMapData As UserDataStore_PlayerPrefs.SavedControllerMapData = savedData(i)
				If savedControllerMapData IsNot Nothing Then
					If savedControllerMapData.knownActionIds IsNot Nothing AndAlso savedControllerMapData.knownActionIds.Count <> 0 Then
						Dim controllerMap As ControllerMap = ControllerMap.CreateFromXml(controllerType, savedData(i).xml)
						If controllerMap IsNot Nothing Then
							Dim map As ControllerMap = player.controllers.maps.GetMap(controllerType, controllerId, controllerMap.categoryId, controllerMap.layoutId)
							If map IsNot Nothing Then
								Dim controllerMapInstance As ControllerMap = ReInput.mapping.GetControllerMapInstance(ReInput.controllers.GetController(controllerType, controllerId), controllerMap.categoryId, controllerMap.layoutId)
								If controllerMapInstance IsNot Nothing Then
									Dim list As List(Of Integer) = New List(Of Integer)()
									For Each num As Integer In allActionIds
										If Not savedControllerMapData.knownActionIds.Contains(num) Then
											list.Add(num)
										End If
									Next
									If list.Count <> 0 Then
										For Each actionElementMap As ActionElementMap In controllerMapInstance.AllMaps
											If list.Contains(actionElementMap.actionId) Then
												If Not map.DoesElementAssignmentConflict(actionElementMap) Then
													Dim elementAssignment As ElementAssignment = New ElementAssignment(controllerType, actionElementMap.elementType, actionElementMap.elementIdentifierId, actionElementMap.axisRange, actionElementMap.keyCode, actionElementMap.modifierKeyFlags, actionElementMap.actionId, actionElementMap.axisContribution, actionElementMap.invert)
													map.CreateElementMap(elementAssignment)
												End If
											End If
										Next
									End If
								End If
							End If
						End If
					End If
				End If
			Next
		End Sub

		' Token: 0x06004E53 RID: 20051 RVA: 0x00279BD4 File Offset: 0x00277FD4
		Private Function GetAllActionIds() As List(Of Integer)
			Dim list As List(Of Integer) = New List(Of Integer)()
			Dim actions As IList(Of InputAction) = ReInput.mapping.Actions
			For i As Integer = 0 To actions.Count - 1
				list.Add(actions(i).id)
			Next
			Return list
		End Function

		' Token: 0x06004E54 RID: 20052 RVA: 0x00279C1C File Offset: 0x0027801C
		Private Function GetAllActionIdsString() As String
			Dim text As String = String.Empty
			Dim allActionIds As List(Of Integer) = Me.GetAllActionIds()
			For i As Integer = 0 To allActionIds.Count - 1
				If i > 0 Then
					text += ","
				End If
				text += allActionIds(i)
			Next
			Return text
		End Function

		' Token: 0x06004E55 RID: 20053 RVA: 0x00279C74 File Offset: 0x00278074
		Private Function FindJoystickPrecise(joystickInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo) As Joystick
			If joystickInfo Is Nothing Then
				Return Nothing
			End If
			If joystickInfo.instanceGuid = Guid.Empty Then
				Return Nothing
			End If
			Dim joysticks As IList(Of Joystick) = ReInput.controllers.Joysticks
			For i As Integer = 0 To joysticks.Count - 1
				If joysticks(i).deviceInstanceGuid = joystickInfo.instanceGuid Then
					Return joysticks(i)
				End If
			Next
			Return Nothing
		End Function

		' Token: 0x06004E56 RID: 20054 RVA: 0x00279CE8 File Offset: 0x002780E8
		Private Function TryFindJoysticksImprecise(joystickInfo As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo, <System.Runtime.InteropServices.OutAttribute()> ByRef matches As List(Of Joystick)) As Boolean
			matches = Nothing
			If joystickInfo Is Nothing Then
				Return False
			End If
			If String.IsNullOrEmpty(joystickInfo.hardwareIdentifier) Then
				Return False
			End If
			Dim joysticks As IList(Of Joystick) = ReInput.controllers.Joysticks
			For i As Integer = 0 To joysticks.Count - 1
				If String.Equals(joysticks(i).hardwareIdentifier, joystickInfo.hardwareIdentifier, StringComparison.OrdinalIgnoreCase) Then
					If matches Is Nothing Then
						matches = New List(Of Joystick)()
					End If
					matches.Add(joysticks(i))
				End If
			Next
			Return matches IsNot Nothing
		End Function

		' Token: 0x040051C9 RID: 20937
		Private Const thisScriptName As String = "UserDataStore_PlayerPrefs"

		' Token: 0x040051CA RID: 20938
		Private Const editorLoadedMessage As String = vbLf & "If unexpected input issues occur, the loaded XML data may be outdated or invalid. Clear PlayerPrefs using the inspector option on the UserDataStore_PlayerPrefs component."

		' Token: 0x040051CB RID: 20939
		Private Const playerPrefsKeySuffix_controllerAssignments As String = "ControllerAssignments"

		' Token: 0x040051CC RID: 20940
		<Tooltip("Should this script be used? If disabled, nothing will be saved or loaded.")>
		<SerializeField()>
		Private isEnabled As Boolean = True

		' Token: 0x040051CD RID: 20941
		<Tooltip("Should saved data be loaded on start?")>
		<SerializeField()>
		Private loadDataOnStart As Boolean = True

		' Token: 0x040051CE RID: 20942
		<Tooltip("Should Player Joystick assignments be saved and loaded? This is not totally reliable for all Joysticks on all platforms. Some platforms/input sources do not provide enough information to reliably save assignments from session to session and reboot to reboot.")>
		<SerializeField()>
		Private loadJoystickAssignments As Boolean = True

		' Token: 0x040051CF RID: 20943
		<Tooltip("Should Player Keyboard assignments be saved and loaded?")>
		<SerializeField()>
		Private loadKeyboardAssignments As Boolean = True

		' Token: 0x040051D0 RID: 20944
		<Tooltip("Should Player Mouse assignments be saved and loaded?")>
		<SerializeField()>
		Private loadMouseAssignments As Boolean = True

		' Token: 0x040051D1 RID: 20945
		<Tooltip("The PlayerPrefs key prefix. Change this to change how keys are stored in PlayerPrefs. Changing this will make saved data already stored with the old key no longer accessible.")>
		<SerializeField()>
		Private playerPrefsKeyPrefix As String = "RewiredSaveData"

		' Token: 0x040051D2 RID: 20946
		Private allowImpreciseJoystickAssignmentMatching As Boolean = True

		' Token: 0x040051D3 RID: 20947
		Private deferredJoystickAssignmentLoadPending As Boolean

		' Token: 0x040051D4 RID: 20948
		Private wasJoystickEverDetected As Boolean

		' Token: 0x02000C54 RID: 3156
		Private Class SavedControllerMapData
			' Token: 0x06004E57 RID: 20055 RVA: 0x00279D75 File Offset: 0x00278175
			Public Sub New(xml As String, knownActionIds As List(Of Integer))
				Me.xml = xml
				Me.knownActionIds = knownActionIds
			End Sub

			' Token: 0x06004E58 RID: 20056 RVA: 0x00279D8C File Offset: 0x0027818C
			Public Shared Function GetXmlStringList(data As List(Of UserDataStore_PlayerPrefs.SavedControllerMapData)) As List(Of String)
				Dim list As List(Of String) = New List(Of String)()
				If data Is Nothing Then
					Return list
				End If
				For i As Integer = 0 To data.Count - 1
					If data(i) IsNot Nothing Then
						If Not String.IsNullOrEmpty(data(i).xml) Then
							list.Add(data(i).xml)
						End If
					End If
				Next
				Return list
			End Function

			' Token: 0x040051D5 RID: 20949
			Public xml As String

			' Token: 0x040051D6 RID: 20950
			Public knownActionIds As List(Of Integer)
		End Class

		' Token: 0x02000C55 RID: 3157
		Private Class ControllerAssignmentSaveInfo
			' Token: 0x06004E59 RID: 20057 RVA: 0x00279DFD File Offset: 0x002781FD
			Public Sub New()
			End Sub

			' Token: 0x06004E5A RID: 20058 RVA: 0x00279E08 File Offset: 0x00278208
			Public Sub New(playerCount As Integer)
				Me.players = New UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo(playerCount - 1) {}
				For i As Integer = 0 To playerCount - 1
					Me.players(i) = New UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo()
				Next
			End Sub

			' Token: 0x170007EE RID: 2030
			' (get) Token: 0x06004E5B RID: 20059 RVA: 0x00279E46 File Offset: 0x00278246
			Public ReadOnly Property playerCount As Integer
				Get
					Return If((Me.players Is Nothing), 0, Me.players.Length)
				End Get
			End Property

			' Token: 0x06004E5C RID: 20060 RVA: 0x00279E64 File Offset: 0x00278264
			Public Function IndexOfPlayer(playerId As Integer) As Integer
				For i As Integer = 0 To Me.playerCount - 1
					If Me.players(i) IsNot Nothing Then
						If Me.players(i).id = playerId Then
							Return i
						End If
					End If
				Next
				Return -1
			End Function

			' Token: 0x06004E5D RID: 20061 RVA: 0x00279EB0 File Offset: 0x002782B0
			Public Function ContainsPlayer(playerId As Integer) As Boolean
				Return Me.IndexOfPlayer(playerId) >= 0
			End Function

			' Token: 0x040051D7 RID: 20951
			Public players As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.PlayerInfo()

			' Token: 0x02000C56 RID: 3158
			Public Class PlayerInfo
				' Token: 0x170007EF RID: 2031
				' (get) Token: 0x06004E5F RID: 20063 RVA: 0x00279EC7 File Offset: 0x002782C7
				Public ReadOnly Property joystickCount As Integer
					Get
						Return If((Me.joysticks Is Nothing), 0, Me.joysticks.Length)
					End Get
				End Property

				' Token: 0x06004E60 RID: 20064 RVA: 0x00279EE4 File Offset: 0x002782E4
				Public Function IndexOfJoystick(joystickId As Integer) As Integer
					For i As Integer = 0 To Me.joystickCount - 1
						If Me.joysticks(i) IsNot Nothing Then
							If Me.joysticks(i).id = joystickId Then
								Return i
							End If
						End If
					Next
					Return -1
				End Function

				' Token: 0x06004E61 RID: 20065 RVA: 0x00279F30 File Offset: 0x00278330
				Public Function ContainsJoystick(joystickId As Integer) As Boolean
					Return Me.IndexOfJoystick(joystickId) >= 0
				End Function

				' Token: 0x040051D8 RID: 20952
				Public id As Integer

				' Token: 0x040051D9 RID: 20953
				Public hasKeyboard As Boolean

				' Token: 0x040051DA RID: 20954
				Public hasMouse As Boolean

				' Token: 0x040051DB RID: 20955
				Public joysticks As UserDataStore_PlayerPrefs.ControllerAssignmentSaveInfo.JoystickInfo()
			End Class

			' Token: 0x02000C57 RID: 3159
			Public Class JoystickInfo
				' Token: 0x040051DC RID: 20956
				Public instanceGuid As Guid

				' Token: 0x040051DD RID: 20957
				Public hardwareIdentifier As String

				' Token: 0x040051DE RID: 20958
				Public id As Integer
			End Class
		End Class

		' Token: 0x02000C58 RID: 3160
		Private Class JoystickAssignmentHistoryInfo
			' Token: 0x06004E63 RID: 20067 RVA: 0x00279F47 File Offset: 0x00278347
			Public Sub New(joystick As Joystick, oldJoystickId As Integer)
				If joystick Is Nothing Then
					Throw New ArgumentNullException("joystick")
				End If
				Me.joystick = joystick
				Me.oldJoystickId = oldJoystickId
			End Sub

			' Token: 0x040051DF RID: 20959
			Public joystick As Joystick

			' Token: 0x040051E0 RID: 20960
			Public oldJoystickId As Integer
		End Class
	End Class
End Namespace
