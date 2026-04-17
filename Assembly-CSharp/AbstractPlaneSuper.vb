Imports System
Imports UnityEngine

' Token: 0x02000AAB RID: 2731
Public MustInherit Class AbstractPlaneSuper
	Inherits AbstractCollidableObject

	' Token: 0x170005BC RID: 1468
	' (get) Token: 0x06004182 RID: 16770 RVA: 0x00237C85 File Offset: 0x00236085
	Public ReadOnly Property State As PlanePlayerWeaponManager.States.Super
		Get
			Return Me.state
		End Get
	End Property

	' Token: 0x170005BD RID: 1469
	' (get) Token: 0x06004183 RID: 16771 RVA: 0x00237C8D File Offset: 0x0023608D
	Protected Overrides ReadOnly Property allowCollisionPlayer As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x06004184 RID: 16772 RVA: 0x00237C90 File Offset: 0x00236090
	Protected Overrides Sub Awake()
		MyBase.tag = "PlayerProjectile"
		MyBase.Awake()
	End Sub

	' Token: 0x06004185 RID: 16773 RVA: 0x00237CA3 File Offset: 0x002360A3
	Protected Overridable Sub Start()
		Me.animHelper = MyBase.GetComponent(Of AnimationHelper)()
		MyBase.transform.position = Me.player.transform.position
	End Sub

	' Token: 0x06004186 RID: 16774 RVA: 0x00237CCC File Offset: 0x002360CC
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06004187 RID: 16775 RVA: 0x00237CE4 File Offset: 0x002360E4
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06004188 RID: 16776 RVA: 0x00237D08 File Offset: 0x00236108
	Public Function Create(player As PlanePlayerController) As AbstractPlaneSuper
		Dim abstractPlaneSuper As AbstractPlaneSuper = Me.InstantiatePrefab(Of AbstractPlaneSuper)()
		abstractPlaneSuper.player = player
		If player.stats.isChalice Then
			abstractPlaneSuper.spriteRenderer = Me.chalice
			abstractPlaneSuper.chalice.gameObject.SetActive(True)
			If abstractPlaneSuper.cuphead Then
				abstractPlaneSuper.cuphead.gameObject.SetActive(False)
			End If
			If abstractPlaneSuper.mugman Then
				abstractPlaneSuper.mugman.gameObject.SetActive(False)
			End If
		Else
			Dim id As PlayerId = player.id
			If id = PlayerId.PlayerOne OrElse id <> PlayerId.PlayerTwo Then
				abstractPlaneSuper.spriteRenderer = If((Not PlayerManager.player1IsMugman), Me.cuphead, Me.mugman)
				abstractPlaneSuper.cuphead.gameObject.SetActive(Not PlayerManager.player1IsMugman)
				abstractPlaneSuper.mugman.gameObject.SetActive(PlayerManager.player1IsMugman)
			Else
				abstractPlaneSuper.spriteRenderer = If((Not PlayerManager.player1IsMugman), Me.mugman, Me.cuphead)
				abstractPlaneSuper.cuphead.gameObject.SetActive(PlayerManager.player1IsMugman)
				abstractPlaneSuper.mugman.gameObject.SetActive(Not PlayerManager.player1IsMugman)
			End If
		End If
		abstractPlaneSuper.StartSuper()
		Return abstractPlaneSuper
	End Function

	' Token: 0x06004189 RID: 16777 RVA: 0x00237E60 File Offset: 0x00236260
	Protected Overridable Sub StartSuper()
		Me.animHelper = MyBase.GetComponent(Of AnimationHelper)()
		Me.animHelper.IgnoreGlobal = True
		PauseManager.Pause()
		Me.player.PauseAll()
		Me.player.SetSpriteVisible(False)
		AudioManager.SnapshotTransition(New String() { "Super", "Unpaused", "Unpaused_1920s" }, New Single() { 1F, 0F, 0F }, 0.1F)
		AudioManager.ChangeBGMPitch(1.3F, 1.5F)
		AudioManager.Play("player_super_beam_start")
		MyBase.transform.SetScale(New Single?(Me.player.transform.localScale.x), New Single?(Me.player.transform.localScale.y), New Single?(1F))
		MyBase.transform.position = Me.player.transform.position
	End Sub

	' Token: 0x0600418A RID: 16778 RVA: 0x00237F6E File Offset: 0x0023636E
	Protected Overridable Sub Fire()
		Me.state = PlanePlayerWeaponManager.States.Super.Ending
	End Sub

	' Token: 0x0600418B RID: 16779 RVA: 0x00237F78 File Offset: 0x00236378
	Protected Sub SnapshotAudio()
		Dim array As String() = New String(1) {}
		array(0) = "Super"
		If SettingsData.Data.vintageAudioEnabled Then
			array(1) = "Unpaused_1920s"
		Else
			array(1) = "Unpaused"
		End If
		AudioManager.SnapshotTransition(array, New Single() { 0F, 1F }, 4F)
		AudioManager.ChangeBGMPitch(1F, 4F)
	End Sub

	' Token: 0x0600418C RID: 16780 RVA: 0x00237FEA File Offset: 0x002363EA
	Protected Overridable Sub StartCountdown()
		Me.SnapshotAudio()
		PauseManager.Unpause()
		Me.player.UnpauseAll(False)
		Me.player.SetSpriteVisible(True)
		Me.animHelper.IgnoreGlobal = False
		Me.state = PlanePlayerWeaponManager.States.Super.Countdown
	End Sub

	' Token: 0x04004807 RID: 18439
	Protected state As PlanePlayerWeaponManager.States.Super = PlanePlayerWeaponManager.States.Super.Intro

	' Token: 0x04004808 RID: 18440
	<SerializeField()>
	<Header("Player Sprites")>
	Private cuphead As SpriteRenderer

	' Token: 0x04004809 RID: 18441
	<SerializeField()>
	Private mugman As SpriteRenderer

	' Token: 0x0400480A RID: 18442
	<SerializeField()>
	Protected chalice As SpriteRenderer

	' Token: 0x0400480B RID: 18443
	Protected spriteRenderer As SpriteRenderer

	' Token: 0x0400480C RID: 18444
	Protected player As PlanePlayerController

	' Token: 0x0400480D RID: 18445
	Protected damageDealer As DamageDealer

	' Token: 0x0400480E RID: 18446
	Protected animHelper As AnimationHelper
End Class
