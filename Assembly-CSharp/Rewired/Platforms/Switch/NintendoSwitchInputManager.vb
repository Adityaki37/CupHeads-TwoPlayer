Imports System
Imports System.Collections.Generic
Imports Rewired.Data
Imports Rewired.Utils.Interfaces
Imports UnityEngine

Namespace Rewired.Platforms.Switch
	' Token: 0x02000C5B RID: 3163
	<AddComponentMenu("Rewired/Nintendo Switch Input Manager")>
	<RequireComponent(GetType(InputManager))>
	Public NotInheritable Class NintendoSwitchInputManager
		Inherits MonoBehaviour
		Implements IExternalInputManager

		' Token: 0x06004E95 RID: 20117 RVA: 0x0027A2A7 File Offset: 0x002786A7
		Function Initialize(platform As Platform, configVars As ConfigVars) As Object Implements Rewired.Utils.Interfaces.IExternalInputManager.Initialize
			Return Nothing
		End Function

		' Token: 0x06004E96 RID: 20118 RVA: 0x0027A2AA File Offset: 0x002786AA
		Sub Deinitialize() Implements Rewired.Utils.Interfaces.IExternalInputManager.Deinitialize
		End Sub

		' Token: 0x040051E2 RID: 20962
		<SerializeField()>
		Private _userData As NintendoSwitchInputManager.UserData = New NintendoSwitchInputManager.UserData()

		' Token: 0x02000C5C RID: 3164
		<Serializable()>
		Private Class UserData
			Implements IKeyedData(Of Integer)

			' Token: 0x170007F1 RID: 2033
			' (get) Token: 0x06004E98 RID: 20120 RVA: 0x0027A353 File Offset: 0x00278753
			' (set) Token: 0x06004E99 RID: 20121 RVA: 0x0027A35B File Offset: 0x0027875B
			Public Property allowedNpadStyles As Integer
				Get
					Return Me._allowedNpadStyles
				End Get
				Set(value As Integer)
					Me._allowedNpadStyles = value
				End Set
			End Property

			' Token: 0x170007F2 RID: 2034
			' (get) Token: 0x06004E9A RID: 20122 RVA: 0x0027A364 File Offset: 0x00278764
			' (set) Token: 0x06004E9B RID: 20123 RVA: 0x0027A36C File Offset: 0x0027876C
			Public Property joyConGripStyle As Integer
				Get
					Return Me._joyConGripStyle
				End Get
				Set(value As Integer)
					Me._joyConGripStyle = value
				End Set
			End Property

			' Token: 0x170007F3 RID: 2035
			' (get) Token: 0x06004E9C RID: 20124 RVA: 0x0027A375 File Offset: 0x00278775
			' (set) Token: 0x06004E9D RID: 20125 RVA: 0x0027A37D File Offset: 0x0027877D
			Public Property adjustIMUsForGripStyle As Boolean
				Get
					Return Me._adjustIMUsForGripStyle
				End Get
				Set(value As Boolean)
					Me._adjustIMUsForGripStyle = value
				End Set
			End Property

			' Token: 0x170007F4 RID: 2036
			' (get) Token: 0x06004E9E RID: 20126 RVA: 0x0027A386 File Offset: 0x00278786
			' (set) Token: 0x06004E9F RID: 20127 RVA: 0x0027A38E File Offset: 0x0027878E
			Public Property handheldActivationMode As Integer
				Get
					Return Me._handheldActivationMode
				End Get
				Set(value As Integer)
					Me._handheldActivationMode = value
				End Set
			End Property

			' Token: 0x170007F5 RID: 2037
			' (get) Token: 0x06004EA0 RID: 20128 RVA: 0x0027A397 File Offset: 0x00278797
			' (set) Token: 0x06004EA1 RID: 20129 RVA: 0x0027A39F File Offset: 0x0027879F
			Public Property assignJoysticksByNpadId As Boolean
				Get
					Return Me._assignJoysticksByNpadId
				End Get
				Set(value As Boolean)
					Me._assignJoysticksByNpadId = value
				End Set
			End Property

			' Token: 0x170007F6 RID: 2038
			' (get) Token: 0x06004EA2 RID: 20130 RVA: 0x0027A3A8 File Offset: 0x002787A8
			Private ReadOnly Property npadNo1 As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadNo1
				End Get
			End Property

			' Token: 0x170007F7 RID: 2039
			' (get) Token: 0x06004EA3 RID: 20131 RVA: 0x0027A3B0 File Offset: 0x002787B0
			Private ReadOnly Property npadNo2 As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadNo2
				End Get
			End Property

			' Token: 0x170007F8 RID: 2040
			' (get) Token: 0x06004EA4 RID: 20132 RVA: 0x0027A3B8 File Offset: 0x002787B8
			Private ReadOnly Property npadNo3 As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadNo3
				End Get
			End Property

			' Token: 0x170007F9 RID: 2041
			' (get) Token: 0x06004EA5 RID: 20133 RVA: 0x0027A3C0 File Offset: 0x002787C0
			Private ReadOnly Property npadNo4 As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadNo4
				End Get
			End Property

			' Token: 0x170007FA RID: 2042
			' (get) Token: 0x06004EA6 RID: 20134 RVA: 0x0027A3C8 File Offset: 0x002787C8
			Private ReadOnly Property npadNo5 As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadNo5
				End Get
			End Property

			' Token: 0x170007FB RID: 2043
			' (get) Token: 0x06004EA7 RID: 20135 RVA: 0x0027A3D0 File Offset: 0x002787D0
			Private ReadOnly Property npadNo6 As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadNo6
				End Get
			End Property

			' Token: 0x170007FC RID: 2044
			' (get) Token: 0x06004EA8 RID: 20136 RVA: 0x0027A3D8 File Offset: 0x002787D8
			Private ReadOnly Property npadNo7 As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadNo7
				End Get
			End Property

			' Token: 0x170007FD RID: 2045
			' (get) Token: 0x06004EA9 RID: 20137 RVA: 0x0027A3E0 File Offset: 0x002787E0
			Private ReadOnly Property npadNo8 As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadNo8
				End Get
			End Property

			' Token: 0x170007FE RID: 2046
			' (get) Token: 0x06004EAA RID: 20138 RVA: 0x0027A3E8 File Offset: 0x002787E8
			Private ReadOnly Property npadHandheld As NintendoSwitchInputManager.NpadSettings_Internal
				Get
					Return Me._npadHandheld
				End Get
			End Property

			' Token: 0x170007FF RID: 2047
			' (get) Token: 0x06004EAB RID: 20139 RVA: 0x0027A3F0 File Offset: 0x002787F0
			Public ReadOnly Property debugPad As NintendoSwitchInputManager.DebugPadSettings_Internal
				Get
					Return Me._debugPad
				End Get
			End Property

			' Token: 0x17000800 RID: 2048
			' (get) Token: 0x06004EAC RID: 20140 RVA: 0x0027A3F8 File Offset: 0x002787F8
			Private ReadOnly Property delegates As Dictionary(Of Integer, Object())
				Get
					If Me.__delegates IsNot Nothing Then
						Return Me.__delegates
					End If
					Dim dictionary As Dictionary(Of Integer, Object()) = New Dictionary(Of Integer, Object())()
					dictionary.Add(0, New Object() { AddressOf Me.get_allowedNpadStyles, New Action(Of Integer)(Sub(x As Integer)
						Me.allowedNpadStyles = x
					End Sub) })
					dictionary.Add(1, New Object() { AddressOf Me.get_joyConGripStyle, New Action(Of Integer)(Sub(x As Integer)
						Me.joyConGripStyle = x
					End Sub) })
					dictionary.Add(2, New Object() { AddressOf Me.get_adjustIMUsForGripStyle, New Action(Of Boolean)(Sub(x As Boolean)
						Me.adjustIMUsForGripStyle = x
					End Sub) })
					dictionary.Add(3, New Object() { AddressOf Me.get_handheldActivationMode, New Action(Of Integer)(Sub(x As Integer)
						Me.handheldActivationMode = x
					End Sub) })
					dictionary.Add(4, New Object() { AddressOf Me.get_assignJoysticksByNpadId, New Action(Of Boolean)(Sub(x As Boolean)
						Me.assignJoysticksByNpadId = x
					End Sub) })
					Dim dictionary2 As Dictionary(Of Integer, Object()) = dictionary
					Dim num As Integer = 5
					Dim array As Object() = New Object(1) {}
					array(0) = AddressOf Me.get_npadNo1
					dictionary2.Add(num, array)
					Dim dictionary3 As Dictionary(Of Integer, Object()) = dictionary
					Dim num2 As Integer = 6
					Dim array2 As Object() = New Object(1) {}
					array2(0) = AddressOf Me.get_npadNo2
					dictionary3.Add(num2, array2)
					Dim dictionary4 As Dictionary(Of Integer, Object()) = dictionary
					Dim num3 As Integer = 7
					Dim array3 As Object() = New Object(1) {}
					array3(0) = AddressOf Me.get_npadNo3
					dictionary4.Add(num3, array3)
					Dim dictionary5 As Dictionary(Of Integer, Object()) = dictionary
					Dim num4 As Integer = 8
					Dim array4 As Object() = New Object(1) {}
					array4(0) = AddressOf Me.get_npadNo4
					dictionary5.Add(num4, array4)
					Dim dictionary6 As Dictionary(Of Integer, Object()) = dictionary
					Dim num5 As Integer = 9
					Dim array5 As Object() = New Object(1) {}
					array5(0) = AddressOf Me.get_npadNo5
					dictionary6.Add(num5, array5)
					Dim dictionary7 As Dictionary(Of Integer, Object()) = dictionary
					Dim num6 As Integer = 10
					Dim array6 As Object() = New Object(1) {}
					array6(0) = AddressOf Me.get_npadNo6
					dictionary7.Add(num6, array6)
					Dim dictionary8 As Dictionary(Of Integer, Object()) = dictionary
					Dim num7 As Integer = 11
					Dim array7 As Object() = New Object(1) {}
					array7(0) = AddressOf Me.get_npadNo7
					dictionary8.Add(num7, array7)
					Dim dictionary9 As Dictionary(Of Integer, Object()) = dictionary
					Dim num8 As Integer = 12
					Dim array8 As Object() = New Object(1) {}
					array8(0) = AddressOf Me.get_npadNo8
					dictionary9.Add(num8, array8)
					Dim dictionary10 As Dictionary(Of Integer, Object()) = dictionary
					Dim num9 As Integer = 13
					Dim array9 As Object() = New Object(1) {}
					array9(0) = AddressOf Me.get_npadHandheld
					dictionary10.Add(num9, array9)
					Dim dictionary11 As Dictionary(Of Integer, Object()) = dictionary
					Dim num10 As Integer = 14
					Dim array10 As Object() = New Object(1) {}
					array10(0) = AddressOf Me.get_debugPad
					dictionary11.Add(num10, array10)
					Dim dictionary12 As Dictionary(Of Integer, Object()) = dictionary
					dictionary = dictionary12
					Me.__delegates = dictionary12
					Return dictionary
				End Get
			End Property

			' Token: 0x06004EAD RID: 20141 RVA: 0x0027A61C File Offset: 0x00278A1C
			Function TryGetValue(Of T)(key As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef value As T) As Boolean Implements Rewired.Utils.Interfaces.IKeyedData(Of Integer).TryGetValue
				Dim array As Object()
				If Not Me.delegates.TryGetValue(key, array) Then
					value = Nothing
					Return False
				End If
				Dim func As Func(Of T) = TryCast(array(0), Func(Of T))
				If func Is Nothing Then
					value = Nothing
					Return False
				End If
				value = func()
				Return True
			End Function

			' Token: 0x06004EAE RID: 20142 RVA: 0x0027A67C File Offset: 0x00278A7C
			Function TrySetValue(Of T)(key As Integer, value As T) As Boolean Implements Rewired.Utils.Interfaces.IKeyedData(Of Integer).TrySetValue
				Dim array As Object()
				If Not Me.delegates.TryGetValue(key, array) Then
					Return False
				End If
				Dim action As Action(Of T) = TryCast(array(1), Action(Of T))
				If action Is Nothing Then
					Return False
				End If
				action(value)
				Return True
			End Function

			' Token: 0x040051E3 RID: 20963
			<SerializeField()>
			Private _allowedNpadStyles As Integer = -1

			' Token: 0x040051E4 RID: 20964
			<SerializeField()>
			Private _joyConGripStyle As Integer = 1

			' Token: 0x040051E5 RID: 20965
			<SerializeField()>
			Private _adjustIMUsForGripStyle As Boolean = True

			' Token: 0x040051E6 RID: 20966
			<SerializeField()>
			Private _handheldActivationMode As Integer

			' Token: 0x040051E7 RID: 20967
			<SerializeField()>
			Private _assignJoysticksByNpadId As Boolean = True

			' Token: 0x040051E8 RID: 20968
			<SerializeField()>
			Private _npadNo1 As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(0)

			' Token: 0x040051E9 RID: 20969
			<SerializeField()>
			Private _npadNo2 As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(1)

			' Token: 0x040051EA RID: 20970
			<SerializeField()>
			Private _npadNo3 As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(2)

			' Token: 0x040051EB RID: 20971
			<SerializeField()>
			Private _npadNo4 As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(3)

			' Token: 0x040051EC RID: 20972
			<SerializeField()>
			Private _npadNo5 As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(4)

			' Token: 0x040051ED RID: 20973
			<SerializeField()>
			Private _npadNo6 As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(5)

			' Token: 0x040051EE RID: 20974
			<SerializeField()>
			Private _npadNo7 As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(6)

			' Token: 0x040051EF RID: 20975
			<SerializeField()>
			Private _npadNo8 As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(7)

			' Token: 0x040051F0 RID: 20976
			<SerializeField()>
			Private _npadHandheld As NintendoSwitchInputManager.NpadSettings_Internal = New NintendoSwitchInputManager.NpadSettings_Internal(0)

			' Token: 0x040051F1 RID: 20977
			<SerializeField()>
			Private _debugPad As NintendoSwitchInputManager.DebugPadSettings_Internal = New NintendoSwitchInputManager.DebugPadSettings_Internal(0)

			' Token: 0x040051F2 RID: 20978
			Private __delegates As Dictionary(Of Integer, Object())
		End Class

		' Token: 0x02000C5D RID: 3165
		<Serializable()>
		Private NotInheritable Class NpadSettings_Internal
			Implements IKeyedData(Of Integer)

			' Token: 0x06004EB4 RID: 20148 RVA: 0x0027A6E4 File Offset: 0x00278AE4
			Friend Sub New(playerId As Integer)
				Me._rewiredPlayerId = playerId
			End Sub

			' Token: 0x17000801 RID: 2049
			' (get) Token: 0x06004EB5 RID: 20149 RVA: 0x0027A701 File Offset: 0x00278B01
			' (set) Token: 0x06004EB6 RID: 20150 RVA: 0x0027A709 File Offset: 0x00278B09
			Private Property isAllowed As Boolean
				Get
					Return Me._isAllowed
				End Get
				Set(value As Boolean)
					Me._isAllowed = value
				End Set
			End Property

			' Token: 0x17000802 RID: 2050
			' (get) Token: 0x06004EB7 RID: 20151 RVA: 0x0027A712 File Offset: 0x00278B12
			' (set) Token: 0x06004EB8 RID: 20152 RVA: 0x0027A71A File Offset: 0x00278B1A
			Private Property rewiredPlayerId As Integer
				Get
					Return Me._rewiredPlayerId
				End Get
				Set(value As Integer)
					Me._rewiredPlayerId = value
				End Set
			End Property

			' Token: 0x17000803 RID: 2051
			' (get) Token: 0x06004EB9 RID: 20153 RVA: 0x0027A723 File Offset: 0x00278B23
			' (set) Token: 0x06004EBA RID: 20154 RVA: 0x0027A72B File Offset: 0x00278B2B
			Private Property joyConAssignmentMode As Integer
				Get
					Return Me._joyConAssignmentMode
				End Get
				Set(value As Integer)
					Me._joyConAssignmentMode = value
				End Set
			End Property

			' Token: 0x17000804 RID: 2052
			' (get) Token: 0x06004EBB RID: 20155 RVA: 0x0027A734 File Offset: 0x00278B34
			Private ReadOnly Property delegates As Dictionary(Of Integer, Object())
				Get
					If Me.__delegates IsNot Nothing Then
						Return Me.__delegates
					End If
					Dim dictionary As Dictionary(Of Integer, Object()) = New Dictionary(Of Integer, Object())() From { { 0, New Object() { AddressOf Me.get_isAllowed, New Action(Of Boolean)(Sub(x As Boolean)
						Me.isAllowed = x
					End Sub) } }, { 1, New Object() { AddressOf Me.get_rewiredPlayerId, New Action(Of Integer)(Sub(x As Integer)
						Me.rewiredPlayerId = x
					End Sub) } }, { 2, New Object() { AddressOf Me.get_joyConAssignmentMode, New Action(Of Integer)(Sub(x As Integer)
						Me.joyConAssignmentMode = x
					End Sub) } } }
					Dim dictionary2 As Dictionary(Of Integer, Object()) = dictionary
					Me.__delegates = dictionary
					Return dictionary2
				End Get
			End Property

			' Token: 0x06004EBC RID: 20156 RVA: 0x0027A7E4 File Offset: 0x00278BE4
			Function TryGetValue(Of T)(key As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef value As T) As Boolean Implements Rewired.Utils.Interfaces.IKeyedData(Of Integer).TryGetValue
				Dim array As Object()
				If Not Me.delegates.TryGetValue(key, array) Then
					value = Nothing
					Return False
				End If
				Dim func As Func(Of T) = TryCast(array(0), Func(Of T))
				If func Is Nothing Then
					value = Nothing
					Return False
				End If
				value = func()
				Return True
			End Function

			' Token: 0x06004EBD RID: 20157 RVA: 0x0027A844 File Offset: 0x00278C44
			Function TrySetValue(Of T)(key As Integer, value As T) As Boolean Implements Rewired.Utils.Interfaces.IKeyedData(Of Integer).TrySetValue
				Dim array As Object()
				If Not Me.delegates.TryGetValue(key, array) Then
					Return False
				End If
				Dim action As Action(Of T) = TryCast(array(1), Action(Of T))
				If action Is Nothing Then
					Return False
				End If
				action(value)
				Return True
			End Function

			' Token: 0x040051F3 RID: 20979
			<Tooltip("Determines whether this Npad id is allowed to be used by the system.")>
			<SerializeField()>
			Private _isAllowed As Boolean = True

			' Token: 0x040051F4 RID: 20980
			<Tooltip("The Rewired Player Id assigned to this Npad id.")>
			<SerializeField()>
			Private _rewiredPlayerId As Integer

			' Token: 0x040051F5 RID: 20981
			<Tooltip("Determines how Joy-Cons should be handled." & vbLf & vbLf & "Unmodified: Joy-Con assignment mode will be left at the system default." & vbLf & "Dual: Joy-Cons pairs are handled as a single controller." & vbLf & "Single: Joy-Cons are handled as individual controllers.")>
			<SerializeField()>
			Private _joyConAssignmentMode As Integer = -1

			' Token: 0x040051F6 RID: 20982
			Private __delegates As Dictionary(Of Integer, Object())
		End Class

		' Token: 0x02000C5E RID: 3166
		<Serializable()>
		Private NotInheritable Class DebugPadSettings_Internal
			Implements IKeyedData(Of Integer)

			' Token: 0x06004EC1 RID: 20161 RVA: 0x0027A89A File Offset: 0x00278C9A
			Friend Sub New(playerId As Integer)
				Me._rewiredPlayerId = playerId
			End Sub

			' Token: 0x17000805 RID: 2053
			' (get) Token: 0x06004EC2 RID: 20162 RVA: 0x0027A8A9 File Offset: 0x00278CA9
			' (set) Token: 0x06004EC3 RID: 20163 RVA: 0x0027A8B1 File Offset: 0x00278CB1
			Private Property rewiredPlayerId As Integer
				Get
					Return Me._rewiredPlayerId
				End Get
				Set(value As Integer)
					Me._rewiredPlayerId = value
				End Set
			End Property

			' Token: 0x17000806 RID: 2054
			' (get) Token: 0x06004EC4 RID: 20164 RVA: 0x0027A8BA File Offset: 0x00278CBA
			' (set) Token: 0x06004EC5 RID: 20165 RVA: 0x0027A8C2 File Offset: 0x00278CC2
			Private Property enabled As Boolean
				Get
					Return Me._enabled
				End Get
				Set(value As Boolean)
					Me._enabled = value
				End Set
			End Property

			' Token: 0x17000807 RID: 2055
			' (get) Token: 0x06004EC6 RID: 20166 RVA: 0x0027A8CC File Offset: 0x00278CCC
			Private ReadOnly Property delegates As Dictionary(Of Integer, Object())
				Get
					If Me.__delegates IsNot Nothing Then
						Return Me.__delegates
					End If
					Dim dictionary As Dictionary(Of Integer, Object()) = New Dictionary(Of Integer, Object())() From { { 0, New Object() { AddressOf Me.get_enabled, New Action(Of Boolean)(Sub(x As Boolean)
						Me.enabled = x
					End Sub) } }, { 1, New Object() { AddressOf Me.get_rewiredPlayerId, New Action(Of Integer)(Sub(x As Integer)
						Me.rewiredPlayerId = x
					End Sub) } } }
					Dim dictionary2 As Dictionary(Of Integer, Object()) = dictionary
					Me.__delegates = dictionary
					Return dictionary2
				End Get
			End Property

			' Token: 0x06004EC7 RID: 20167 RVA: 0x0027A954 File Offset: 0x00278D54
			Function TryGetValue(Of T)(key As Integer, <System.Runtime.InteropServices.OutAttribute()> ByRef value As T) As Boolean Implements Rewired.Utils.Interfaces.IKeyedData(Of Integer).TryGetValue
				Dim array As Object()
				If Not Me.delegates.TryGetValue(key, array) Then
					value = Nothing
					Return False
				End If
				Dim func As Func(Of T) = TryCast(array(0), Func(Of T))
				If func Is Nothing Then
					value = Nothing
					Return False
				End If
				value = func()
				Return True
			End Function

			' Token: 0x06004EC8 RID: 20168 RVA: 0x0027A9B4 File Offset: 0x00278DB4
			Function TrySetValue(Of T)(key As Integer, value As T) As Boolean Implements Rewired.Utils.Interfaces.IKeyedData(Of Integer).TrySetValue
				Dim array As Object()
				If Not Me.delegates.TryGetValue(key, array) Then
					Return False
				End If
				Dim action As Action(Of T) = TryCast(array(1), Action(Of T))
				If action Is Nothing Then
					Return False
				End If
				action(value)
				Return True
			End Function

			' Token: 0x040051F7 RID: 20983
			<Tooltip("Determines whether the Debug Pad will be enabled.")>
			<SerializeField()>
			Private _enabled As Boolean

			' Token: 0x040051F8 RID: 20984
			<Tooltip("The Rewired Player Id to which the Debug Pad will be assigned.")>
			<SerializeField()>
			Private _rewiredPlayerId As Integer

			' Token: 0x040051F9 RID: 20985
			Private __delegates As Dictionary(Of Integer, Object())
		End Class
	End Class
End Namespace
