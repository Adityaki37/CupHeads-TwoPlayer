Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000722 RID: 1826
Public Class PirateLevelDogFish
	Inherits AbstractProjectile

	' Token: 0x060027C0 RID: 10176 RVA: 0x0017451C File Offset: 0x0017291C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.position = PirateLevelDogFish.START_POS
		AddHandler Me.normalHitBox.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		AddHandler Me.normalHitBox.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.onDamageTaken
		AddHandler Me.secretHitBox.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTakenFromBehind
	End Sub

	' Token: 0x060027C1 RID: 10177 RVA: 0x0017459C File Offset: 0x0017299C
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.state = PirateLevelDogFish.State.Slide Then
			MyBase.transform.AddPosition(-Me.speedY * CupheadTime.Delta, 0F, 0F)
			Dim num As Single = Me.slideTime / Me.dogfish.speedFalloffTime
			If num < 1F Then
				Me.speedY = EaseUtils.EaseOutQuart(Me.dogfish.startSpeed, Me.dogfish.endSpeed, num)
				Me.slideTime += CupheadTime.Delta
			Else
				Me.speedY = Me.dogfish.endSpeed
			End If
			If MyBase.transform.position.x < -1000F Then
				RemoveHandler Me.properties.OnBossDeath, AddressOf Me.OnBossDeath
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			End If
			If Me.bossDied Then
				Me.Die()
			End If
			If PirateLevelDogFish.dogKilled AndAlso Me.isSecret Then
				Me.isSecret = False
				Me.OnEnableCollider()
			End If
		End If
	End Sub

	' Token: 0x060027C2 RID: 10178 RVA: 0x001746C1 File Offset: 0x00172AC1
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.state <> PirateLevelDogFish.State.Death Then
			MyBase.OnCollisionPlayer(hit, phase)
			If phase <> CollisionPhase.[Exit] Then
				Me.damageDealer.DealDamage(hit)
			End If
		End If
	End Sub

	' Token: 0x060027C3 RID: 10179 RVA: 0x001746EC File Offset: 0x00172AEC
	Public Sub Init(properties As LevelProperties.Pirate, isSecret As Boolean)
		Me.properties = properties
		Me.dogfish = properties.CurrentState.dogFish
		Me.isSecret = isSecret
		Me.hp = CSng(Me.dogfish.hp)
		Me.state = PirateLevelDogFish.State.Jump
		Me.normalHitBox.GetComponent(Of DamageReceiver)().enabled = False
		AudioManager.Play("level_pirate_dogfish_jump")
		Me.emitAudioFromObject.Add("level_pirate_dogfish_jump")
		Me.splashEffect.Create(Me.splashRoot.position)
		AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x060027C4 RID: 10180 RVA: 0x00174788 File Offset: 0x00172B88
	Private Sub onDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If(Me.hp < 0F) And (Me.state <> PirateLevelDogFish.State.Death) Then
			Me.OnDying()
			Me.secretHitBox.GetComponent(Of Collider2D)().enabled = False
			Me.Die()
		End If
	End Sub

	' Token: 0x060027C5 RID: 10181 RVA: 0x001747E4 File Offset: 0x00172BE4
	Private Sub OnDamageTakenFromBehind(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Me.state <> PirateLevelDogFish.State.Death Then
			Me.OnDying()
			Me.SetParryable(True)
			Me.Die()
		End If
	End Sub

	' Token: 0x060027C6 RID: 10182 RVA: 0x00174834 File Offset: 0x00172C34
	Private Sub OnDying()
		AudioManager.[Stop]("level_pirate_dogfish_jump")
		AudioManager.Play("level_pirate_dogfish_death_poof")
		Me.emitAudioFromObject.Add("level_pirate_dogfish_death_poof")
		PirateLevelDogFish.dogKilled = True
		Me.normalHitBox.GetComponent(Of Collider2D)().enabled = False
		Me.secretHitBox.GetComponent(Of DamageReceiver)().enabled = False
	End Sub

	' Token: 0x060027C7 RID: 10183 RVA: 0x0017488D File Offset: 0x00172C8D
	Private Sub OnEnableCollider()
		Me.normalHitBox.GetComponent(Of DamageReceiver)().enabled = True
		Me.secretHitBox.GetComponent(Of DamageReceiver)().enabled = False
		MyBase.gameObject.layer = 0
	End Sub

	' Token: 0x060027C8 RID: 10184 RVA: 0x001748C0 File Offset: 0x00172CC0
	Private Sub OnJumpAnimationComplete()
		If Me.state <> PirateLevelDogFish.State.Death Then
			Me.state = PirateLevelDogFish.State.Slide
			AudioManager.Play("level_pirate_dogfish_slide")
			Me.emitAudioFromObject.Add("level_pirate_dogfish_slide")
			Me.slideTime = 0F
			Me.speedY = Me.dogfish.startSpeed
		End If
	End Sub

	' Token: 0x060027C9 RID: 10185 RVA: 0x00174918 File Offset: 0x00172D18
	Protected Overrides Sub Die()
		Me.state = PirateLevelDogFish.State.Death
		RemoveHandler Me.properties.OnBossDeath, AddressOf Me.OnBossDeath
		MyBase.animator.SetTrigger("OnDeath")
		Me.deathEffect.Create(MyBase.transform.position)
		MyBase.StartCoroutine(Me.deathFloat_cr())
	End Sub

	' Token: 0x060027CA RID: 10186 RVA: 0x00174977 File Offset: 0x00172D77
	Private Sub OnBossDeath()
		Me.bossDied = True
		If Me.state = PirateLevelDogFish.State.Slide Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060027CB RID: 10187 RVA: 0x00174994 File Offset: 0x00172D94
	Private Iterator Function deathFloat_cr() As IEnumerator
		AudioManager.Play("level_pirate_dogfish_death_flap")
		Me.emitAudioFromObject.Add("level_pirate_dogfish_death_flap")
		While MyBase.transform.position.y < 360F
			MyBase.transform.AddPosition(0F, Me.properties.CurrentState.dogFish.deathSpeed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x060027CC RID: 10188 RVA: 0x001749AF File Offset: 0x00172DAF
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.splashEffect = Nothing
		Me.deathEffect = Nothing
	End Sub

	' Token: 0x0400307D RID: 12413
	Private Shared START_POS As Vector2 = New Vector2(235F, -245F)

	' Token: 0x0400307E RID: 12414
	Private Const DEATH_Y As Single = 450F

	' Token: 0x0400307F RID: 12415
	<SerializeField()>
	Private secretHitBox As Collider2D

	' Token: 0x04003080 RID: 12416
	<SerializeField()>
	Private normalHitBox As Collider2D

	' Token: 0x04003081 RID: 12417
	<SerializeField()>
	Private splashEffect As Effect

	' Token: 0x04003082 RID: 12418
	<SerializeField()>
	Private splashRoot As Transform

	' Token: 0x04003083 RID: 12419
	<SerializeField()>
	Private deathEffect As Effect

	' Token: 0x04003084 RID: 12420
	Private state As PirateLevelDogFish.State

	' Token: 0x04003085 RID: 12421
	Private hp As Single

	' Token: 0x04003086 RID: 12422
	Private speedY As Single

	' Token: 0x04003087 RID: 12423
	Private slideTime As Single

	' Token: 0x04003088 RID: 12424
	Private properties As LevelProperties.Pirate

	' Token: 0x04003089 RID: 12425
	Private dogfish As LevelProperties.Pirate.DogFish

	' Token: 0x0400308A RID: 12426
	Private bossDied As Boolean

	' Token: 0x0400308B RID: 12427
	Private isSecret As Boolean

	' Token: 0x0400308C RID: 12428
	Public Shared dogKilled As Boolean = False

	' Token: 0x02000723 RID: 1827
	Public Enum State
		' Token: 0x0400308E RID: 12430
		Init
		' Token: 0x0400308F RID: 12431
		Jump
		' Token: 0x04003090 RID: 12432
		Slide
		' Token: 0x04003091 RID: 12433
		Death
	End Enum
End Class
