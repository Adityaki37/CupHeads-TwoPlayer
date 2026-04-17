Imports System
Imports System.Collections.Generic
Imports UnityEngine

Namespace Rewired.Data
	' Token: 0x02000C52 RID: 3154
	Public Class UserDataStore_Cuphead
		Inherits UserDataStore

		' Token: 0x06004DD7 RID: 19927 RVA: 0x0027767A File Offset: 0x00275A7A
		Public Overrides Sub Save()
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SaveAll()
		End Sub

		' Token: 0x06004DD8 RID: 19928 RVA: 0x00277699 File Offset: 0x00275A99
		Public Overrides Overloads Sub SaveControllerData(playerId As Integer, controllerType As ControllerType, controllerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SaveControllerDataNow(playerId, controllerType, controllerId)
		End Sub

		' Token: 0x06004DD9 RID: 19929 RVA: 0x002776BB File Offset: 0x00275ABB
		Public Overrides Overloads Sub SaveControllerData(controllerType As ControllerType, controllerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SaveControllerDataNow(controllerType, controllerId)
		End Sub

		' Token: 0x06004DDA RID: 19930 RVA: 0x002776DC File Offset: 0x00275ADC
		Public Overrides Sub SavePlayerData(playerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SavePlayerDataNow(playerId)
		End Sub

		' Token: 0x06004DDB RID: 19931 RVA: 0x002776FC File Offset: 0x00275AFC
		Public Overrides Sub SaveInputBehavior(playerId As Integer, behaviorId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not save any data.", Me)
				Return
			End If
			Me.SaveInputBehaviorNow(playerId, behaviorId)
		End Sub

		' Token: 0x06004DDC RID: 19932 RVA: 0x00277720 File Offset: 0x00275B20
		Public Overrides Sub Load()
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadAll()
		End Sub

		' Token: 0x06004DDD RID: 19933 RVA: 0x0027774C File Offset: 0x00275B4C
		Public Overrides Overloads Sub LoadControllerData(playerId As Integer, controllerType As ControllerType, controllerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadControllerDataNow(playerId, controllerType, controllerId)
		End Sub

		' Token: 0x06004DDE RID: 19934 RVA: 0x0027777C File Offset: 0x00275B7C
		Public Overrides Overloads Sub LoadControllerData(controllerType As ControllerType, controllerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadControllerDataNow(controllerType, controllerId)
		End Sub

		' Token: 0x06004DDF RID: 19935 RVA: 0x002777AC File Offset: 0x00275BAC
		Public Overrides Sub LoadPlayerData(playerId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadPlayerDataNow(playerId)
		End Sub

		' Token: 0x06004DE0 RID: 19936 RVA: 0x002777D8 File Offset: 0x00275BD8
		Public Overrides Sub LoadInputBehavior(playerId As Integer, behaviorId As Integer)
			If Not Me.isEnabled Then
				Global.UnityEngine.Debug.LogWarning("UserDataStore_PlayerPrefs is disabled and will not load any data.", Me)
				Return
			End If
			Dim num As Integer = Me.LoadInputBehaviorNow(playerId, behaviorId)
		End Sub

		' Token: 0x06004DE1 RID: 19937 RVA: 0x00277805 File Offset: 0x00275C05
		Protected Overrides Sub OnInitialize()
			If Me.loadDataOnStart Then
				Me.Load()
			End If
		End Sub

		' Token: 0x06004DE2 RID: 19938 RVA: 0x00277818 File Offset: 0x00275C18
		Protected Overrides Sub OnControllerConnected(args As ControllerStatusChangedEventArgs)
			If Not Me.isEnabled Then
				Return
			End If
			If args.controllerType = ControllerType.Joystick Then
				Dim num As Integer = Me.LoadJoystickData(args.controllerId)
			End If
		End Sub

		' Token: 0x06004DE3 RID: 19939 RVA: 0x0027784A File Offset: 0x00275C4A
		Protected Overrides Sub OnControllerPreDiscconnect(args As ControllerStatusChangedEventArgs)
			If Not Me.isEnabled Then
				Return
			End If
			If args.controllerType = ControllerType.Joystick Then
				Me.SaveJoystickData(args.controllerId)
			End If
		End Sub

		' Token: 0x06004DE4 RID: 19940 RVA: 0x00277870 File Offset: 0x00275C70
		Protected Overrides Sub OnControllerDisconnected(args As ControllerStatusChangedEventArgs)
			If Not Me.isEnabled Then
				Return
			End If
		End Sub

		' Token: 0x06004DE5 RID: 19941 RVA: 0x00277880 File Offset: 0x00275C80
		Private Function LoadAll() As Integer
			Dim num As Integer = 0
			Dim allPlayers As IList(Of Player) = ReInput.players.AllPlayers
			For i As Integer = 0 To allPlayers.Count - 1
				num += Me.LoadPlayerDataNow(allPlayers(i))
			Next
			Return num + Me.LoadAllJoystickCalibrationData()
		End Function

		' Token: 0x06004DE6 RID: 19942 RVA: 0x002778CB File Offset: 0x00275CCB
		Private Function LoadPlayerDataNow(playerId As Integer) As Integer
			Return Me.LoadPlayerDataNow(ReInput.players.GetPlayer(playerId))
		End Function

		' Token: 0x06004DE7 RID: 19943 RVA: 0x002778E0 File Offset: 0x00275CE0
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

		' Token: 0x06004DE8 RID: 19944 RVA: 0x0027798C File Offset: 0x00275D8C
		Private Function LoadAllJoystickCalibrationData() As Integer
			Dim num As Integer = 0
			Dim joysticks As IList(Of Joystick) = ReInput.controllers.Joysticks
			For i As Integer = 0 To joysticks.Count - 1
				num += Me.LoadJoystickCalibrationData(joysticks(i))
			Next
			Return num
		End Function

		' Token: 0x06004DE9 RID: 19945 RVA: 0x002779CE File Offset: 0x00275DCE
		Private Function LoadJoystickCalibrationData(joystick As Joystick) As Integer
			If joystick Is Nothing Then
				Return 0
			End If
			Return If((Not joystick.ImportCalibrationMapFromXmlString(Me.GetJoystickCalibrationMapXml(joystick))), 0, 1)
		End Function

		' Token: 0x06004DEA RID: 19946 RVA: 0x002779F1 File Offset: 0x00275DF1
		Private Function LoadJoystickCalibrationData(joystickId As Integer) As Integer
			Return Me.LoadJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId))
		End Function

		' Token: 0x06004DEB RID: 19947 RVA: 0x00277A04 File Offset: 0x00275E04
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

		' Token: 0x06004DEC RID: 19948 RVA: 0x00277A70 File Offset: 0x00275E70
		Private Function LoadControllerDataNow(playerId As Integer, controllerType As ControllerType, controllerId As Integer) As Integer
			Dim num As Integer = 0
			num += Me.LoadControllerMaps(playerId, controllerType, controllerId)
			Return num + Me.LoadControllerDataNow(controllerType, controllerId)
		End Function

		' Token: 0x06004DED RID: 19949 RVA: 0x00277A98 File Offset: 0x00275E98
		Private Function LoadControllerDataNow(controllerType As ControllerType, controllerId As Integer) As Integer
			Dim num As Integer = 0
			If controllerType = ControllerType.Joystick Then
				num += Me.LoadJoystickCalibrationData(controllerId)
			End If
			Return num
		End Function

		' Token: 0x06004DEE RID: 19950 RVA: 0x00277ABC File Offset: 0x00275EBC
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
			Dim allControllerMapsXml As List(Of String) = Me.GetAllControllerMapsXml(player, True, controllerType, controller)
			If allControllerMapsXml.Count = 0 Then
				Return num
			End If
			Return num + player.controllers.maps.AddMapsFromXml(controllerType, controllerId, allControllerMapsXml)
		End Function

		' Token: 0x06004DEF RID: 19951 RVA: 0x00277B24 File Offset: 0x00275F24
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

		' Token: 0x06004DF0 RID: 19952 RVA: 0x00277B84 File Offset: 0x00275F84
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

		' Token: 0x06004DF1 RID: 19953 RVA: 0x00277BC4 File Offset: 0x00275FC4
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

		' Token: 0x06004DF2 RID: 19954 RVA: 0x00277C18 File Offset: 0x00276018
		Private Sub SaveAll()
			Dim allPlayers As IList(Of Player) = ReInput.players.AllPlayers
			For i As Integer = 0 To allPlayers.Count - 1
				Me.SavePlayerDataNow(allPlayers(i))
			Next
			Me.SaveAllJoystickCalibrationData()
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004DF3 RID: 19955 RVA: 0x00277C5F File Offset: 0x0027605F
		Private Sub SavePlayerDataNow(playerId As Integer)
			Me.SavePlayerDataNow(ReInput.players.GetPlayer(playerId))
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004DF4 RID: 19956 RVA: 0x00277C78 File Offset: 0x00276078
		Private Sub SavePlayerDataNow(player As Player)
			If player Is Nothing Then
				Return
			End If
			Dim saveData As PlayerSaveData = player.GetSaveData(True)
			Me.SaveInputBehaviors(player, saveData)
			Me.SaveControllerMaps(player, saveData)
		End Sub

		' Token: 0x06004DF5 RID: 19957 RVA: 0x00277CA4 File Offset: 0x002760A4
		Private Sub SaveAllJoystickCalibrationData()
			Dim joysticks As IList(Of Joystick) = ReInput.controllers.Joysticks
			For i As Integer = 0 To joysticks.Count - 1
				Me.SaveJoystickCalibrationData(joysticks(i))
			Next
		End Sub

		' Token: 0x06004DF6 RID: 19958 RVA: 0x00277CE0 File Offset: 0x002760E0
		Private Sub SaveJoystickCalibrationData(joystickId As Integer)
			Me.SaveJoystickCalibrationData(ReInput.controllers.GetJoystick(joystickId))
		End Sub

		' Token: 0x06004DF7 RID: 19959 RVA: 0x00277CF4 File Offset: 0x002760F4
		Private Sub SaveJoystickCalibrationData(joystick As Joystick)
			If joystick Is Nothing Then
				Return
			End If
			Dim calibrationMapSaveData As JoystickCalibrationMapSaveData = joystick.GetCalibrationMapSaveData()
			Dim joystickCalibrationMapPlayerPrefsKey As String = Me.GetJoystickCalibrationMapPlayerPrefsKey(calibrationMapSaveData)
			PlayerPrefs.SetString(joystickCalibrationMapPlayerPrefsKey, calibrationMapSaveData.map.ToXmlString())
		End Sub

		' Token: 0x06004DF8 RID: 19960 RVA: 0x00277D28 File Offset: 0x00276128
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

		' Token: 0x06004DF9 RID: 19961 RVA: 0x00277D8B File Offset: 0x0027618B
		Private Sub SaveControllerDataNow(playerId As Integer, controllerType As ControllerType, controllerId As Integer)
			Me.SaveControllerMaps(playerId, controllerType, controllerId)
			Me.SaveControllerDataNow(controllerType, controllerId)
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004DFA RID: 19962 RVA: 0x00277DA3 File Offset: 0x002761A3
		Private Sub SaveControllerDataNow(controllerType As ControllerType, controllerId As Integer)
			If controllerType = ControllerType.Joystick Then
				Me.SaveJoystickCalibrationData(controllerId)
			End If
			PlayerPrefs.Save()
		End Sub

		' Token: 0x06004DFB RID: 19963 RVA: 0x00277DB8 File Offset: 0x002761B8
		Private Sub SaveControllerMaps(player As Player, playerSaveData As PlayerSaveData)
			For Each controllerMapSaveData As ControllerMapSaveData In playerSaveData.AllControllerMapSaveData
				Dim controllerMapPlayerPrefsKey As String = Me.GetControllerMapPlayerPrefsKey(player, controllerMapSaveData)
				PlayerPrefs.SetString(controllerMapPlayerPrefsKey, controllerMapSaveData.map.ToXmlString())
			Next
		End Sub

		' Token: 0x06004DFC RID: 19964 RVA: 0x00277E28 File Offset: 0x00276228
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
				Dim controllerMapPlayerPrefsKey As String = Me.GetControllerMapPlayerPrefsKey(player, mapSaveData(i))
				PlayerPrefs.SetString(controllerMapPlayerPrefsKey, mapSaveData(i).map.ToXmlString())
			Next
		End Sub

		' Token: 0x06004DFD RID: 19965 RVA: 0x00277EA8 File Offset: 0x002762A8
		Private Sub SaveInputBehaviors(player As Player, playerSaveData As PlayerSaveData)
			If player Is Nothing Then
				Return
			End If
			Dim inputBehaviors As InputBehavior() = playerSaveData.inputBehaviors
			For i As Integer = 0 To inputBehaviors.Length - 1
				Me.SaveInputBehaviorNow(player, inputBehaviors(i))
			Next
		End Sub

		' Token: 0x06004DFE RID: 19966 RVA: 0x00277EE4 File Offset: 0x002762E4
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

		' Token: 0x06004DFF RID: 19967 RVA: 0x00277F28 File Offset: 0x00276328
		Private Sub SaveInputBehaviorNow(player As Player, inputBehavior As InputBehavior)
			If player Is Nothing OrElse inputBehavior Is Nothing Then
				Return
			End If
			Dim inputBehaviorPlayerPrefsKey As String = Me.GetInputBehaviorPlayerPrefsKey(player, inputBehavior)
			PlayerPrefs.SetString(inputBehaviorPlayerPrefsKey, inputBehavior.ToXmlString())
		End Sub

		' Token: 0x06004E00 RID: 19968 RVA: 0x00277F58 File Offset: 0x00276358
		Private Function GetBasePlayerPrefsKey(player As Player) As String
			Dim text As String = Me.playerPrefsKeyPrefix
			Return text + "|playerName=" + player.name
		End Function

		' Token: 0x06004E01 RID: 19969 RVA: 0x00277F80 File Offset: 0x00276380
		Private Function GetControllerMapPlayerPrefsKey(player As Player, saveData As ControllerMapSaveData) As String
			Dim text As String = Me.GetBasePlayerPrefsKey(player)
			text += "|dataType=ControllerMap"
			text = text + "|controllerMapType=" + saveData.mapTypeString
			Dim text2 As String = text
			text = String.Concat(New Object() { text2, "|categoryId=", saveData.map.categoryId, "|layoutId=", saveData.map.layoutId })
			text = text + "|hardwareIdentifier=" + saveData.controllerHardwareIdentifier
			If saveData.mapType Is GetType(JoystickMap) Then
				text = text + "|hardwareGuid=" + CType(saveData, JoystickMapSaveData).joystickHardwareTypeGuid.ToString()
			End If
			Return text
		End Function

		' Token: 0x06004E02 RID: 19970 RVA: 0x00278048 File Offset: 0x00276448
		Private Function GetControllerMapXml(player As Player, controllerType As ControllerType, categoryId As Integer, layoutId As Integer, controller As Controller) As String
			Dim text As String = Me.GetBasePlayerPrefsKey(player)
			text += "|dataType=ControllerMap"
			text = text + "|controllerMapType=" + controller.mapTypeString
			Dim text2 As String = text
			text = String.Concat(New Object() { text2, "|categoryId=", categoryId, "|layoutId=", layoutId })
			text = text + "|hardwareIdentifier=" + controller.hardwareIdentifier
			If controllerType = ControllerType.Joystick Then
				Dim joystick As Joystick = CType(controller, Joystick)
				text = text + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString()
			End If
			If Not PlayerPrefs.HasKey(text) Then
				Return String.Empty
			End If
			Return PlayerPrefs.GetString(text)
		End Function

		' Token: 0x06004E03 RID: 19971 RVA: 0x0027810C File Offset: 0x0027650C
		Private Function GetAllControllerMapsXml(player As Player, userAssignableMapsOnly As Boolean, controllerType As ControllerType, controller As Controller) As List(Of String)
			Dim list As List(Of String) = New List(Of String)()
			Dim mapCategories As IList(Of InputMapCategory) = ReInput.mapping.MapCategories
			For i As Integer = 0 To mapCategories.Count - 1
				Dim inputMapCategory As InputMapCategory = mapCategories(i)
				If Not userAssignableMapsOnly OrElse inputMapCategory.userAssignable Then
					Dim list2 As IList(Of InputLayout) = ReInput.mapping.MapLayouts(controllerType)
					For j As Integer = 0 To list2.Count - 1
						Dim inputLayout As InputLayout = list2(j)
						Dim controllerMapXml As String = Me.GetControllerMapXml(player, controllerType, inputMapCategory.id, inputLayout.id, controller)
						If Not(controllerMapXml = String.Empty) Then
							list.Add(controllerMapXml)
						End If
					Next
				End If
			Next
			Return list
		End Function

		' Token: 0x06004E04 RID: 19972 RVA: 0x002781CC File Offset: 0x002765CC
		Private Function GetJoystickCalibrationMapPlayerPrefsKey(saveData As JoystickCalibrationMapSaveData) As String
			Dim text As String = Me.playerPrefsKeyPrefix
			text += "|dataType=CalibrationMap"
			text = text + "|controllerType=" + saveData.controllerType.ToString()
			text = text + "|hardwareIdentifier=" + saveData.hardwareIdentifier
			Return text + "|hardwareGuid=" + saveData.joystickHardwareTypeGuid.ToString()
		End Function

		' Token: 0x06004E05 RID: 19973 RVA: 0x00278240 File Offset: 0x00276640
		Private Function GetJoystickCalibrationMapXml(joystick As Joystick) As String
			Dim text As String = Me.playerPrefsKeyPrefix
			text += "|dataType=CalibrationMap"
			text = text + "|controllerType=" + joystick.type.ToString()
			text = text + "|hardwareIdentifier=" + joystick.hardwareIdentifier
			text = text + "|hardwareGuid=" + joystick.hardwareTypeGuid.ToString()
			If Not PlayerPrefs.HasKey(text) Then
				Return String.Empty
			End If
			Return PlayerPrefs.GetString(text)
		End Function

		' Token: 0x06004E06 RID: 19974 RVA: 0x002782CC File Offset: 0x002766CC
		Private Function GetInputBehaviorPlayerPrefsKey(player As Player, saveData As InputBehavior) As String
			Dim text As String = Me.GetBasePlayerPrefsKey(player)
			text += "|dataType=InputBehavior"
			Return text + "|id=" + saveData.id
		End Function

		' Token: 0x06004E07 RID: 19975 RVA: 0x00278308 File Offset: 0x00276708
		Private Function GetInputBehaviorXml(player As Player, id As Integer) As String
			Dim text As String = Me.GetBasePlayerPrefsKey(player)
			text += "|dataType=InputBehavior"
			text = text + "|id=" + id
			If Not PlayerPrefs.HasKey(text) Then
				Return String.Empty
			End If
			Return PlayerPrefs.GetString(text)
		End Function

		' Token: 0x040051C4 RID: 20932
		Private Const thisScriptName As String = "UserDataStore_PlayerPrefs"

		' Token: 0x040051C5 RID: 20933
		Private Const editorLoadedMessage As String = vbLf & "If unexpected input issues occur, the loaded XML data may be outdated or invalid. Clear PlayerPrefs using the inspector option on the UserDataStore_PlayerPrefs component."

		' Token: 0x040051C6 RID: 20934
		<SerializeField()>
		Private isEnabled As Boolean = True

		' Token: 0x040051C7 RID: 20935
		<SerializeField()>
		Private loadDataOnStart As Boolean = True

		' Token: 0x040051C8 RID: 20936
		<SerializeField()>
		Private playerPrefsKeyPrefix As String = "RewiredSaveData"
	End Class
End Namespace
