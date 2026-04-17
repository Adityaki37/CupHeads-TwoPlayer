Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007B1 RID: 1969
Public Class SallyStagePlayLevelMeteor
	Inherits AbstractProjectile

	' Token: 0x17000406 RID: 1030
	' (get) Token: 0x06002C46 RID: 11334 RVA: 0x001A08D4 File Offset: 0x0019ECD4
	' (set) Token: 0x06002C47 RID: 11335 RVA: 0x001A08DC File Offset: 0x0019ECDC
	Public Property spawnPosition As Single

	' Token: 0x17000407 RID: 1031
	' (get) Token: 0x06002C48 RID: 11336 RVA: 0x001A08E5 File Offset: 0x0019ECE5
	' (set) Token: 0x06002C49 RID: 11337 RVA: 0x001A08ED File Offset: 0x0019ECED
	Public Property state As SallyStagePlayLevelMeteor.State

	' Token: 0x17000408 RID: 1032
	' (get) Token: 0x06002C4A RID: 11338 RVA: 0x001A08F6 File Offset: 0x0019ECF6
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0.25F
		End Get
	End Property

	' Token: 0x17000409 RID: 1033
	' (get) Token: 0x06002C4B RID: 11339 RVA: 0x001A08FD File Offset: 0x0019ECFD
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06002C4C RID: 11340 RVA: 0x001A0904 File Offset: 0x0019ED04
	Public Function Create(pos As Single, hp As Single, properties As LevelProperties.SallyStagePlay.Meteor) As SallyStagePlayLevelMeteor
		Dim sallyStagePlayLevelMeteor As SallyStagePlayLevelMeteor = TryCast(MyBase.Create(), SallyStagePlayLevelMeteor)
		sallyStagePlayLevelMeteor.properties = properties
		sallyStagePlayLevelMeteor.spawnPosition = pos
		sallyStagePlayLevelMeteor.hp = hp
		Return sallyStagePlayLevelMeteor
	End Function

	' Token: 0x06002C4D RID: 11341 RVA: 0x001A0934 File Offset: 0x0019ED34
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AddHandler Me.star.OnActivate, AddressOf Me.ParryStar
		Me.star.GetComponent(Of Collider2D)().enabled = False
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002C4E RID: 11342 RVA: 0x001A0992 File Offset: 0x0019ED92
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.transform.position = New Vector2(-640F + Me.spawnPosition, 360F)
		MyBase.StartCoroutine(Me.move_down_cr())
	End Sub

	' Token: 0x06002C4F RID: 11343 RVA: 0x001A09CD File Offset: 0x0019EDCD
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002C50 RID: 11344 RVA: 0x001A09EB File Offset: 0x0019EDEB
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F AndAlso Me.state = SallyStagePlayLevelMeteor.State.Meteor Then
			Me.state = SallyStagePlayLevelMeteor.State.Hook
			Me.OnMeteorDie()
		End If
	End Sub

	' Token: 0x06002C51 RID: 11345 RVA: 0x001A0A28 File Offset: 0x0019EE28
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.state = SallyStagePlayLevelMeteor.State.Meteor AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002C52 RID: 11346 RVA: 0x001A0A54 File Offset: 0x0019EE54
	Private Iterator Function move_down_cr() As IEnumerator
		AudioManager.Play("sally_meteor_ascend_decend")
		Me.emitAudioFromObject.Add("sally_meteor_ascend_decend")
		Me.state = SallyStagePlayLevelMeteor.State.Meteor
		While MyBase.transform.position.y > CSng(Level.Current.Ground) + 100F
			MyBase.transform.position -= MyBase.transform.up * Me.properties.meteorSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C53 RID: 11347 RVA: 0x001A0A70 File Offset: 0x0019EE70
	Private Iterator Function move_up_cr() As IEnumerator
		AudioManager.Play("sally_meteor_ascend_decend")
		Me.emitAudioFromObject.Add("sally_meteor_ascend_decend")
		While Me.star.transform.position.y < 360F - Me.properties.hookMaxHeight
			Me.star.transform.position += Me.star.transform.up * Me.properties.meteorSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C54 RID: 11348 RVA: 0x001A0A8C File Offset: 0x0019EE8C
	Private Iterator Function leave_cr() As IEnumerator
		While Me.star.transform.position.y < 460F
			Me.star.transform.position += Me.star.transform.up * Me.properties.meteorSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C55 RID: 11349 RVA: 0x001A0AA8 File Offset: 0x0019EEA8
	Private Iterator Function leave_all_cr() As IEnumerator
		While MyBase.transform.position.y < 460F
			MyBase.transform.position += MyBase.transform.up * Me.properties.meteorSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C56 RID: 11350 RVA: 0x001A0AC4 File Offset: 0x0019EEC4
	Private Iterator Function timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.hookParryExitDelay)
		MyBase.StartCoroutine(Me.leave_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C57 RID: 11351 RVA: 0x001A0AE0 File Offset: 0x0019EEE0
	Private Sub OnMeteorDie()
		Me.star.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of Animator)().SetTrigger("OpenMeteor")
		AudioManager.Play("sally_meteor_open")
		Me.emitAudioFromObject.Add("sally_meteor_open")
		Me.damageReceiver.enabled = False
		MyBase.StartCoroutine(Me.move_up_cr())
		MyBase.StartCoroutine(Me.slide_meteor_cr())
	End Sub

	' Token: 0x06002C58 RID: 11352 RVA: 0x001A0B5C File Offset: 0x0019EF5C
	Public Sub ParryStar()
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If player2 IsNot Nothing AndAlso Not player2.IsDead AndAlso Not player.IsDead AndAlso Me.parryCounter < 1 Then
			Me.parryCounter += 1
			Return
		End If
		MyBase.GetComponent(Of Animator)().SetTrigger("SpinStar")
		Me.state = SallyStagePlayLevelMeteor.State.Leaving
		MyBase.StartCoroutine(Me.leave_cr())
		Me.star.StartParryCooldown()
	End Sub

	' Token: 0x06002C59 RID: 11353 RVA: 0x001A0BE4 File Offset: 0x0019EFE4
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.state = SallyStagePlayLevelMeteor.State.Leaving
		Me.spawnPosition = 0F
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
		For Each spriteRenderer As SpriteRenderer In MyBase.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.enabled = False
		Next
	End Sub

	' Token: 0x06002C5A RID: 11354 RVA: 0x001A0C47 File Offset: 0x0019F047
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
		If hit.GetComponent(Of SallyStagePlayLevelWave)() Then
			MyBase.StartCoroutine(Me.leave_all_cr())
		End If
	End Sub

	' Token: 0x06002C5B RID: 11355 RVA: 0x001A0C70 File Offset: 0x0019F070
	Private Iterator Function slide_meteor_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1F
		Dim start As Vector3 = Me.meteor.transform.position
		Dim [end] As Vector3 = New Vector3(Me.meteor.transform.position.x, Me.meteor.transform.position.y + 700F)
		While t < time
			t += CupheadTime.Delta
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / time)
			Me.meteor.transform.position = Vector3.Lerp(start, [end], val)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002C5C RID: 11356 RVA: 0x001A0C8B File Offset: 0x0019F08B
	Public Sub MeteorChangePhase()
		MyBase.StartCoroutine(Me.change_phase_cr())
	End Sub

	' Token: 0x06002C5D RID: 11357 RVA: 0x001A0C9C File Offset: 0x0019F09C
	Private Iterator Function change_phase_cr() As IEnumerator
		AudioManager.Play("sally_meteor_ascend_decend")
		Me.emitAudioFromObject.Add("sally_meteor_ascend_decend")
		Me.state = SallyStagePlayLevelMeteor.State.Meteor
		While MyBase.transform.position.y < CSng(Level.Current.Ceiling) + 100F
			MyBase.transform.position += MyBase.transform.up * Me.properties.meteorSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x040034ED RID: 13549
	<SerializeField()>
	Private meteor As GameObject

	' Token: 0x040034EE RID: 13550
	<SerializeField()>
	Private star As ParrySwitch

	' Token: 0x040034F0 RID: 13552
	Private damageReceiver As DamageReceiver

	' Token: 0x040034F1 RID: 13553
	Private properties As LevelProperties.SallyStagePlay.Meteor

	' Token: 0x040034F2 RID: 13554
	Private hp As Single

	' Token: 0x040034F3 RID: 13555
	Private parryCounter As Integer

	' Token: 0x020007B2 RID: 1970
	Public Enum State
		' Token: 0x040034F5 RID: 13557
		Meteor
		' Token: 0x040034F6 RID: 13558
		Hook
		' Token: 0x040034F7 RID: 13559
		Leaving
	End Enum
End Class
