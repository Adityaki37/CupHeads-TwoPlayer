Imports System
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000A4E RID: 2638
Public MustInherit Class AbstractPlayerSuper
	Inherits AbstractCollidableObject

	' Token: 0x1400009E RID: 158
	' (add) Token: 0x06003ECC RID: 16076 RVA: 0x00226754 File Offset: 0x00224B54
	' (remove) Token: 0x06003ECD RID: 16077 RVA: 0x0022678C File Offset: 0x00224B8C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnEndedEvent As Action

	' Token: 0x1400009F RID: 159
	' (add) Token: 0x06003ECE RID: 16078 RVA: 0x002267C4 File Offset: 0x00224BC4
	' (remove) Token: 0x06003ECF RID: 16079 RVA: 0x002267FC File Offset: 0x00224BFC
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnStartedEvent As Action

	' Token: 0x1700056D RID: 1389
	' (get) Token: 0x06003ED0 RID: 16080 RVA: 0x00226832 File Offset: 0x00224C32
	Protected Overrides ReadOnly Property allowCollisionPlayer As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x06003ED1 RID: 16081 RVA: 0x00226835 File Offset: 0x00224C35
	Protected Overrides Sub Awake()
		MyBase.tag = "PlayerProjectile"
		MyBase.Awake()
	End Sub

	' Token: 0x06003ED2 RID: 16082 RVA: 0x00226848 File Offset: 0x00224C48
	Protected Overridable Sub Start()
		Me.animHelper = MyBase.GetComponent(Of AnimationHelper)()
		MyBase.transform.position = Me.player.transform.position
	End Sub

	' Token: 0x06003ED3 RID: 16083 RVA: 0x00226871 File Offset: 0x00224C71
	Protected Overridable Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003ED4 RID: 16084 RVA: 0x00226889 File Offset: 0x00224C89
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06003ED5 RID: 16085 RVA: 0x002268AB File Offset: 0x00224CAB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.player IsNot Nothing Then
			RemoveHandler Me.player.weaponManager.OnSuperInterrupt, AddressOf Me.Interrupt
		End If
	End Sub

	' Token: 0x06003ED6 RID: 16086 RVA: 0x002268E4 File Offset: 0x00224CE4
	Public Function Create(player As LevelPlayerController) As AbstractPlayerSuper
		Dim abstractPlayerSuper As AbstractPlayerSuper = Me.InstantiatePrefab(Of AbstractPlayerSuper)()
		abstractPlayerSuper.player = player
		Dim id As PlayerId = player.id
		If id = PlayerId.PlayerOne OrElse id <> PlayerId.PlayerTwo Then
			If Not Me.isChaliceSuper Then
				abstractPlayerSuper.spriteRenderer = If((Not PlayerManager.player1IsMugman), Me.cuphead, Me.mugman)
				abstractPlayerSuper.cuphead.gameObject.SetActive(Not PlayerManager.player1IsMugman)
				abstractPlayerSuper.mugman.gameObject.SetActive(PlayerManager.player1IsMugman)
			End If
		ElseIf Not Me.isChaliceSuper Then
			abstractPlayerSuper.spriteRenderer = If((Not PlayerManager.player1IsMugman), Me.mugman, Me.cuphead)
			abstractPlayerSuper.cuphead.gameObject.SetActive(PlayerManager.player1IsMugman)
			abstractPlayerSuper.mugman.gameObject.SetActive(Not PlayerManager.player1IsMugman)
		End If
		Me.interrupted = False
		AddHandler player.weaponManager.OnSuperInterrupt, AddressOf abstractPlayerSuper.Interrupt
		abstractPlayerSuper.StartSuper()
		Return abstractPlayerSuper
	End Function

	' Token: 0x06003ED7 RID: 16087 RVA: 0x00226A04 File Offset: 0x00224E04
	Public Overridable Sub Interrupt()
		Me.interrupted = True
	End Sub

	' Token: 0x06003ED8 RID: 16088 RVA: 0x00226A10 File Offset: 0x00224E10
	Protected Overridable Sub StartSuper()
		Dim component As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		component.IgnoreGlobal = True
		PauseManager.Pause()
		AudioManager.HandleSnapshot(AudioManager.Snapshots.SuperStart.ToString(), 0.2F)
		AudioManager.ChangeBGMPitch(1.3F, 1.5F)
		MyBase.transform.SetScale(New Single?(Me.player.transform.localScale.x), New Single?(Me.player.transform.localScale.y), New Single?(1F))
		MyBase.transform.position = Me.player.transform.position
		If Me.OnStartedEvent IsNot Nothing Then
			Me.OnStartedEvent()
		End If
		Me.OnStartedEvent = Nothing
	End Sub

	' Token: 0x06003ED9 RID: 16089 RVA: 0x00226AE0 File Offset: 0x00224EE0
	Protected Overridable Sub Fire()
		PauseManager.Unpause()
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Super.ToString(), 0.2F)
		If Me.player Is Nothing Then
			Me.Interrupt()
		Else
			Me.player.PauseAll()
		End If
		Dim component As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		component.IgnoreGlobal = False
	End Sub

	' Token: 0x06003EDA RID: 16090 RVA: 0x00226B40 File Offset: 0x00224F40
	Protected Overridable Sub EndSuper(Optional changePitch As Boolean = True)
		AudioManager.SnapshotReset(SceneLoader.SceneName, 1F)
		If changePitch Then
			AudioManager.ChangeBGMPitch(1F, 2F)
		End If
		If Me.player IsNot Nothing Then
			Me.player.UnpauseAll(False)
		End If
		If Me.OnEndedEvent IsNot Nothing Then
			Me.OnEndedEvent()
		End If
		Me.OnEndedEvent = Nothing
	End Sub

	' Token: 0x040045D5 RID: 17877
	<SerializeField()>
	<Header("Player Sprites")>
	Private cuphead As SpriteRenderer

	' Token: 0x040045D6 RID: 17878
	<SerializeField()>
	Private mugman As SpriteRenderer

	' Token: 0x040045D7 RID: 17879
	<SerializeField()>
	Private isChaliceSuper As Boolean

	' Token: 0x040045D8 RID: 17880
	Protected spriteRenderer As SpriteRenderer

	' Token: 0x040045D9 RID: 17881
	Protected player As LevelPlayerController

	' Token: 0x040045DA RID: 17882
	Protected damageDealer As DamageDealer

	' Token: 0x040045DB RID: 17883
	Protected animHelper As AnimationHelper

	' Token: 0x040045DC RID: 17884
	Protected interrupted As Boolean
End Class
