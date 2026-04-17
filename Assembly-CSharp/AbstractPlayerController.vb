Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x020009D2 RID: 2514
Public MustInherit Class AbstractPlayerController
	Inherits AbstractPausableComponent

	' Token: 0x06003B0F RID: 15119 RVA: 0x00213868 File Offset: 0x00211C68
	Public Shared Function Create(id As PlayerId, pos As Vector2, mode As PlayerMode) As AbstractPlayerController
		Dim abstractPlayerController As AbstractPlayerController
		Select Case mode
			Case Else
				abstractPlayerController = Global.UnityEngine.[Object].Instantiate(Of LevelPlayerController)(Level.Current.LevelResources.levelPlayer)
			Case PlayerMode.Plane
				abstractPlayerController = Global.UnityEngine.[Object].Instantiate(Of PlanePlayerController)(Level.Current.LevelResources.planePlayer)
			Case PlayerMode.Arcade
				abstractPlayerController = Global.UnityEngine.[Object].Instantiate(Of ArcadePlayerController)(CType(Level.Current, RetroArcadeLevel).playerPrefab)
		End Select
		abstractPlayerController.transform.position = pos
		abstractPlayerController.mode = mode
		abstractPlayerController.LevelInit(id)
		Return abstractPlayerController
	End Function

	' Token: 0x170004D5 RID: 1237
	' (get) Token: 0x06003B10 RID: 15120 RVA: 0x002138FA File Offset: 0x00211CFA
	Public ReadOnly Property input As PlayerInput
		Get
			If Me._input Is Nothing Then
				Me._input = MyBase.GetComponent(Of PlayerInput)()
			End If
			Return Me._input
		End Get
	End Property

	' Token: 0x170004D6 RID: 1238
	' (get) Token: 0x06003B11 RID: 15121 RVA: 0x0021391F File Offset: 0x00211D1F
	Public ReadOnly Property stats As PlayerStatsManager
		Get
			If Me._stats Is Nothing Then
				Me._stats = MyBase.GetComponent(Of PlayerStatsManager)()
			End If
			Return Me._stats
		End Get
	End Property

	' Token: 0x170004D7 RID: 1239
	' (get) Token: 0x06003B12 RID: 15122 RVA: 0x00213944 File Offset: 0x00211D44
	Public ReadOnly Property damageReceiver As PlayerDamageReceiver
		Get
			If Me._damageReceiver Is Nothing Then
				Me._damageReceiver = MyBase.GetComponent(Of PlayerDamageReceiver)()
			End If
			Return Me._damageReceiver
		End Get
	End Property

	' Token: 0x170004D8 RID: 1240
	' (get) Token: 0x06003B13 RID: 15123 RVA: 0x00213969 File Offset: 0x00211D69
	Public ReadOnly Property cameraController As PlayerCameraController
		Get
			If Me._cameraController Is Nothing Then
				Me._cameraController = MyBase.GetComponent(Of PlayerCameraController)()
			End If
			Return Me._cameraController
		End Get
	End Property

	' Token: 0x170004D9 RID: 1241
	' (get) Token: 0x06003B14 RID: 15124 RVA: 0x0021398E File Offset: 0x00211D8E
	' (set) Token: 0x06003B15 RID: 15125 RVA: 0x00213996 File Offset: 0x00211D96
	Public Property id As PlayerId

	' Token: 0x170004DA RID: 1242
	' (get) Token: 0x06003B16 RID: 15126 RVA: 0x0021399F File Offset: 0x00211D9F
	' (set) Token: 0x06003B17 RID: 15127 RVA: 0x002139A7 File Offset: 0x00211DA7
	Public Property mode As PlayerMode

	' Token: 0x170004DB RID: 1243
	' (get) Token: 0x06003B18 RID: 15128 RVA: 0x002139B0 File Offset: 0x00211DB0
	Public ReadOnly Property IsDead As Boolean
		Get
			Return Not Me._isReviving AndAlso Me.stats.Health <= 0
		End Get
	End Property

	' Token: 0x170004DC RID: 1244
	' (get) Token: 0x06003B19 RID: 15129 RVA: 0x002139D0 File Offset: 0x00211DD0
	' (set) Token: 0x06003B1A RID: 15130 RVA: 0x002139D8 File Offset: 0x00211DD8
	Public Property levelStarted As Boolean

	' Token: 0x170004DD RID: 1245
	' (get) Token: 0x06003B1B RID: 15131 RVA: 0x002139E1 File Offset: 0x00211DE1
	' (set) Token: 0x06003B1C RID: 15132 RVA: 0x002139E9 File Offset: 0x00211DE9
	Public Property levelEnded As Boolean

	' Token: 0x170004DE RID: 1246
	' (get) Token: 0x06003B1D RID: 15133 RVA: 0x002139F2 File Offset: 0x00211DF2
	Public ReadOnly Property collider As BoxCollider2D
		Get
			If Me._collider Is Nothing Then
				Me._collider = MyBase.GetComponent(Of BoxCollider2D)()
			End If
			Return Me._collider
		End Get
	End Property

	' Token: 0x170004DF RID: 1247
	' (get) Token: 0x06003B1E RID: 15134 RVA: 0x00213A17 File Offset: 0x00211E17
	Public ReadOnly Property collider2D As BoxCollider2D
		Get
			Return Me.collider
		End Get
	End Property

	' Token: 0x170004E0 RID: 1248
	' (get) Token: 0x06003B1F RID: 15135 RVA: 0x00213A1F File Offset: 0x00211E1F
	Public Overridable ReadOnly Property center As Vector3
		Get
			If MyBase.transform Is Nothing Then
				Return Vector3.zero
			End If
			Return MyBase.transform.position + Me.collider.offset
		End Get
	End Property

	' Token: 0x170004E1 RID: 1249
	' (get) Token: 0x06003B20 RID: 15136 RVA: 0x00213A58 File Offset: 0x00211E58
	Public Overridable ReadOnly Property CameraCenter As Vector3
		Get
			Return Me.cameraController.center
		End Get
	End Property

	' Token: 0x170004E2 RID: 1250
	' (get) Token: 0x06003B21 RID: 15137 RVA: 0x00213A6C File Offset: 0x00211E6C
	Public ReadOnly Property left As Single
		Get
			Return Me.center.x - Me.collider.size.x * 0.5F
		End Get
	End Property

	' Token: 0x170004E3 RID: 1251
	' (get) Token: 0x06003B22 RID: 15138 RVA: 0x00213AA4 File Offset: 0x00211EA4
	Public ReadOnly Property right As Single
		Get
			Return Me.center.x + Me.collider.size.x * 0.5F
		End Get
	End Property

	' Token: 0x170004E4 RID: 1252
	' (get) Token: 0x06003B23 RID: 15139 RVA: 0x00213ADC File Offset: 0x00211EDC
	Public ReadOnly Property top As Single
		Get
			Return Me.center.y + Me.collider.size.y * 0.5F
		End Get
	End Property

	' Token: 0x170004E5 RID: 1253
	' (get) Token: 0x06003B24 RID: 15140 RVA: 0x00213B14 File Offset: 0x00211F14
	Public ReadOnly Property bottom As Single
		Get
			Return Me.center.y - Me.collider.size.y * 0.5F
		End Get
	End Property

	' Token: 0x170004E6 RID: 1254
	' (get) Token: 0x06003B25 RID: 15141 RVA: 0x00213B49 File Offset: 0x00211F49
	Public ReadOnly Property width As Single
		Get
			Return Me.right - Me.left
		End Get
	End Property

	' Token: 0x170004E7 RID: 1255
	' (get) Token: 0x06003B26 RID: 15142 RVA: 0x00213B58 File Offset: 0x00211F58
	Public ReadOnly Property height As Single
		Get
			Return Me.top - Me.bottom
		End Get
	End Property

	' Token: 0x170004E8 RID: 1256
	' (get) Token: 0x06003B27 RID: 15143
	Public MustOverride ReadOnly Property CanTakeDamage As Boolean

	' Token: 0x14000074 RID: 116
	' (add) Token: 0x06003B28 RID: 15144 RVA: 0x00213B68 File Offset: 0x00211F68
	' (remove) Token: 0x06003B29 RID: 15145 RVA: 0x00213BA0 File Offset: 0x00211FA0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayIntroEvent As Action

	' Token: 0x06003B2A RID: 15146 RVA: 0x00213BD6 File Offset: 0x00211FD6
	Public Overridable Sub PlayIntro()
		If Me.OnPlayIntroEvent IsNot Nothing Then
			Me.OnPlayIntroEvent()
		End If
	End Sub

	' Token: 0x14000075 RID: 117
	' (add) Token: 0x06003B2B RID: 15147 RVA: 0x00213BF0 File Offset: 0x00211FF0
	' (remove) Token: 0x06003B2C RID: 15148 RVA: 0x00213C28 File Offset: 0x00212028
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlatformingLevelAwakeEvent As Action

	' Token: 0x06003B2D RID: 15149 RVA: 0x00213C5E File Offset: 0x0021205E
	Public Overridable Sub OnPlatformingLevelAwake()
		If Me.OnPlatformingLevelAwakeEvent IsNot Nothing Then
			Me.OnPlatformingLevelAwakeEvent()
		End If
	End Sub

	' Token: 0x14000076 RID: 118
	' (add) Token: 0x06003B2E RID: 15150 RVA: 0x00213C78 File Offset: 0x00212078
	' (remove) Token: 0x06003B2F RID: 15151 RVA: 0x00213CB0 File Offset: 0x002120B0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnReviveEvent As AbstractPlayerController.OnReviveHandler

	' Token: 0x06003B30 RID: 15152 RVA: 0x00213CE8 File Offset: 0x002120E8
	Protected Overrides Sub Awake()
		Dim array As AbstractPlayerController() = Global.UnityEngine.[Object].FindObjectsOfType(Of AbstractPlayerController)()
		For i As Integer = 0 To array.Length - 1
			If array(i).name.Contains("PlayerTwo") Then
				Global.UnityEngine.[Object].Destroy(array(i).gameObject)
			End If
		Next
		MyBase.Awake()
		If Level.Current Is Nothing OrElse Not Level.Current.PlayersCreated Then
			If MyBase.gameObject IsNot Nothing Then
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			Return
		End If
	End Sub

	' Token: 0x06003B31 RID: 15153 RVA: 0x00213D78 File Offset: 0x00212178
	Protected Overridable Sub LevelInit(id As PlayerId)
		Me.id = id
		MyBase.name = id.ToString()
		PlayerManager.SetPlayer(id, Me)
		Me.input.Init(Me.id)
		Me.cameraController.LevelInit()
		Me.stats.LevelInit()
		AddHandler Me.stats.OnPlayerDeathEvent, AddressOf Me.OnDeath
	End Sub

	' Token: 0x06003B32 RID: 15154 RVA: 0x00213DE5 File Offset: 0x002121E5
	Public Overridable Sub OnLevelWin()
	End Sub

	' Token: 0x06003B33 RID: 15155 RVA: 0x00213DE8 File Offset: 0x002121E8
	Public Overridable Sub LevelStart()
		For Each abstractPlayerComponent As AbstractPlayerComponent In MyBase.GetComponentsInChildren(Of AbstractPlayerComponent)()
			abstractPlayerComponent.OnLevelStart()
		Next
		Me.levelStarted = True
	End Sub

	' Token: 0x06003B34 RID: 15156 RVA: 0x00213E21 File Offset: 0x00212221
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
		Me.levelEnded = True
	End Sub

	' Token: 0x06003B35 RID: 15157 RVA: 0x00213E30 File Offset: 0x00212230
	Protected Overridable Sub OnDeath(playerId As PlayerId)
		Me._isReviving = False
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06003B36 RID: 15158 RVA: 0x00213E45 File Offset: 0x00212245
	Public Overridable Sub OnLeave(playerId As PlayerId)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003B37 RID: 15159 RVA: 0x00213E52 File Offset: 0x00212252
	Protected Overridable Sub OnPreRevive(pos As Vector3)
		Me.stats.OnPreRevive()
		Me._isReviving = True
		MyBase.transform.position = pos
	End Sub

	' Token: 0x06003B38 RID: 15160 RVA: 0x00213E74 File Offset: 0x00212274
	Public Overridable Sub LevelJoin(pos As Vector3)
		Me.LevelStart()
		MyBase.gameObject.SetActive(False)
		Dim position As Vector3 = MyBase.transform.position
		Dim playerJoinEffect As PlayerJoinEffect = PlayerJoinEffect.Create(Me.id, MyBase.transform.position, Me.mode, Me.stats.isChalice)
		AddHandler playerJoinEffect.OnPreReviveEvent, AddressOf Me.OnPreRevive
		AddHandler playerJoinEffect.OnReviveEvent, AddressOf Me.OnRevive
		Me.OnPreRevive(pos)
	End Sub

	' Token: 0x06003B39 RID: 15161 RVA: 0x00213EF9 File Offset: 0x002122F9
	Public Overridable Sub BufferInputs()
	End Sub

	' Token: 0x06003B3A RID: 15162 RVA: 0x00213EFC File Offset: 0x002122FC
	Protected Overridable Sub OnRevive(pos As Vector3)
		Me.reviveHelper = New GameObjectHelper("Revive Helper")
		Me.reviveHelper.events.StartCoroutine(Me.reviveDelay_cr(1, pos))
		Me.stats.OnRevive()
		MyBase.transform.position = pos
	End Sub

	' Token: 0x06003B3B RID: 15163 RVA: 0x00213F4C File Offset: 0x0021234C
	Private Iterator Function reviveDelay_cr(frameDelay As Integer, pos As Vector3) As IEnumerator
		For i As Integer = 0 To frameDelay - 1
			Yield Nothing
		Next
		Me._isReviving = False
		MyBase.gameObject.SetActive(True)
		If Me.OnReviveEvent IsNot Nothing Then
			Me.OnReviveEvent(pos)
		End If
		Return
	End Function

	' Token: 0x040042C7 RID: 17095
	Private _input As PlayerInput

	' Token: 0x040042C8 RID: 17096
	Private _stats As PlayerStatsManager

	' Token: 0x040042C9 RID: 17097
	Private _damageReceiver As PlayerDamageReceiver

	' Token: 0x040042CA RID: 17098
	Private _cameraController As PlayerCameraController

	' Token: 0x040042CD RID: 17101
	Private _isReviving As Boolean

	' Token: 0x040042D0 RID: 17104
	Private _collider As BoxCollider2D

	' Token: 0x040042D4 RID: 17108
	Private reviveHelper As GameObjectHelper

	' Token: 0x020009D3 RID: 2515
	' (Invoke) Token: 0x06003B3D RID: 15165
	Public Delegate Sub OnReviveHandler(pos As Vector3)
End Class
