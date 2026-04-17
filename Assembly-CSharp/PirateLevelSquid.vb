Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000727 RID: 1831
Public Class PirateLevelSquid
	Inherits LevelProperties.Pirate.Entity

	' Token: 0x170003D2 RID: 978
	' (get) Token: 0x060027E3 RID: 10211 RVA: 0x001753D3 File Offset: 0x001737D3
	' (set) Token: 0x060027E4 RID: 10212 RVA: 0x001753DB File Offset: 0x001737DB
	Public Property state As PirateLevelSquid.State

	' Token: 0x060027E5 RID: 10213 RVA: 0x001753E4 File Offset: 0x001737E4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.position = PirateLevelSquid.START_POS
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.onDamageTaken
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x060027E6 RID: 10214 RVA: 0x00175424 File Offset: 0x00173824
	Private Sub Update()
		If Me.state = PirateLevelSquid.State.Attack Then
			If Me.attackTime > Me.squid.maxTime Then
				Me.[Exit]()
			Else
				Me.attackTime += CupheadTime.Delta
			End If
		End If
		Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, Mathf.PingPong(Me.bobTime, 1F))
		MyBase.transform.SetPosition(Nothing, New Single?(Mathf.Lerp(Me.startY, Me.endY, num)), Nothing)
		Me.bobTime += CupheadTime.Delta
	End Sub

	' Token: 0x060027E7 RID: 10215 RVA: 0x001754E4 File Offset: 0x001738E4
	Public Overrides Sub LevelInit(properties As LevelProperties.Pirate)
		MyBase.LevelInit(properties)
		Me.squid = properties.CurrentState.squid
		Dim num As Single = Me.squid.xPos.RandomFloat()
		MyBase.transform.SetPosition(New Single?(num), Nothing, Nothing)
		Me.splashPrefab.Create(MyBase.transform.position + New Vector3(0F, -40F, 0F))
		AudioManager.Play("level_pirate_squid_splash")
		Me.hp = Me.squid.hp
		Me.startY = MyBase.transform.position.y
		Me.endY = Me.startY + -20F
		Me.state = PirateLevelSquid.State.Enter
		AudioManager.Play("level_pirate_squid_enter")
		AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x060027E8 RID: 10216 RVA: 0x001755DB File Offset: 0x001739DB
	Private Sub PlayPopSFX()
		AudioManager.Play("level_pirate_squid_attack_pop")
	End Sub

	' Token: 0x060027E9 RID: 10217 RVA: 0x001755E7 File Offset: 0x001739E7
	Private Sub onDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060027EA RID: 10218 RVA: 0x00175614 File Offset: 0x00173A14
	Private Sub [Exit]()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.state = PirateLevelSquid.State.[Exit]
		AudioManager.Play("level_pirate_squid_exit")
		MyBase.animator.SetTrigger("OnExit")
		RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x060027EB RID: 10219 RVA: 0x00175668 File Offset: 0x00173A68
	Public Sub Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.state = PirateLevelSquid.State.Die
		AudioManager.Play("level_pirate_squid_death")
		MyBase.animator.SetTrigger("OnDeath")
		RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x060027EC RID: 10220 RVA: 0x001756B9 File Offset: 0x00173AB9
	Private Sub OnBossDeath()
		Me.Die()
	End Sub

	' Token: 0x060027ED RID: 10221 RVA: 0x001756C4 File Offset: 0x00173AC4
	Private Iterator Function attack_cr() As IEnumerator
		If Not Me.InkAttackSoundActive Then
			AudioManager.PlayLoop("level_pirate_squid_attack_loop")
			Me.InkAttackSoundActive = True
		End If
		While Me.state = PirateLevelSquid.State.Attack
			Dim v As Vector2 = Vector2.zero
			v.y = Me.squid.blobVelY
			v.x = Me.squid.blobVelX
			Me.inkBlob.Create(Me.inkOrigin.position, v, Me.squid.blobGravity)
			Yield CupheadTime.WaitForSeconds(Me, Me.squid.blobDelay)
		End While
		AudioManager.[Stop]("level_pirate_squid_attack_loop")
		Me.InkAttackSoundActive = False
		Return
	End Function

	' Token: 0x060027EE RID: 10222 RVA: 0x001756DF File Offset: 0x00173ADF
	Private Sub OnEnterAnimationComplete()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.state = PirateLevelSquid.State.Attack
		Me.attackTime = 0F
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x060027EF RID: 10223 RVA: 0x0017570C File Offset: 0x00173B0C
	Private Sub OnExitAnimationComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060027F0 RID: 10224 RVA: 0x00175719 File Offset: 0x00173B19
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.inkBlob = Nothing
		Me.splashPrefab = Nothing
	End Sub

	' Token: 0x040030A2 RID: 12450
	Private Shared START_POS As Vector2 = New Vector2(-200F, -220F)

	' Token: 0x040030A3 RID: 12451
	Private Const BOB_OFFSET As Single = -20F

	' Token: 0x040030A4 RID: 12452
	<SerializeField()>
	Private inkOrigin As Transform

	' Token: 0x040030A5 RID: 12453
	<SerializeField()>
	Private inkBlob As PirateLevelSquidProjectile

	' Token: 0x040030A6 RID: 12454
	<SerializeField()>
	Private splashPrefab As Effect

	' Token: 0x040030A8 RID: 12456
	Private hp As Single

	' Token: 0x040030A9 RID: 12457
	Private startY As Single

	' Token: 0x040030AA RID: 12458
	Private endY As Single

	' Token: 0x040030AB RID: 12459
	Private bobTime As Single

	' Token: 0x040030AC RID: 12460
	Private attackTime As Single

	' Token: 0x040030AD RID: 12461
	Private squid As LevelProperties.Pirate.Squid

	' Token: 0x040030AE RID: 12462
	Private InkAttackSoundActive As Boolean

	' Token: 0x02000728 RID: 1832
	Public Enum State
		' Token: 0x040030B0 RID: 12464
		Init
		' Token: 0x040030B1 RID: 12465
		Enter
		' Token: 0x040030B2 RID: 12466
		Attack
		' Token: 0x040030B3 RID: 12467
		[Exit]
		' Token: 0x040030B4 RID: 12468
		Die
	End Enum
End Class
