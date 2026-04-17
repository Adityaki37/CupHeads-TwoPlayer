Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005A3 RID: 1443
Public Class DicePalaceBoozeLevelTumbler
	Inherits DicePalaceBoozeLevelBossBase

	' Token: 0x06001BC5 RID: 7109 RVA: 0x000FD30A File Offset: 0x000FB70A
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x06001BC6 RID: 7110 RVA: 0x000FD340 File Offset: 0x000FB740
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x06001BC7 RID: 7111 RVA: 0x000FD350 File Offset: 0x000FB750
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Dim health As Single = Me.health
		Me.health -= info.damage
		If health > 0F Then
			Level.Current.timeline.DealDamage(Mathf.Clamp(health - Me.health, 0F, health))
		End If
		If Me.health < 0F AndAlso Not MyBase.isDead Then
			Me.StartDying()
			Me.TumblerDeathSFX()
		End If
	End Sub

	' Token: 0x06001BC8 RID: 7112 RVA: 0x000FD3CC File Offset: 0x000FB7CC
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceBooze)
		Me.attackDelayIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.tumbler.beamDelayString.Split(New Char() { ","c }).Length)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntroEnd
		AddHandler Level.Current.OnWinEvent, AddressOf Me.HandleDead
		Me.health = properties.CurrentState.tumbler.tumblerHP
		AudioManager.Play("booze_tumbler_intro")
		Me.emitAudioFromObject.Add("booze_tumbler_intro")
		MyBase.LevelInit(properties)
	End Sub

	' Token: 0x06001BC9 RID: 7113 RVA: 0x000FD46B File Offset: 0x000FB86B
	Private Sub OnIntroEnd()
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06001BCA RID: 7114 RVA: 0x000FD47C File Offset: 0x000FB87C
	Private Iterator Function attack_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Parser.FloatParse(MyBase.properties.CurrentState.tumbler.beamDelayString.Split(New Char() { ","c })(Me.attackDelayIndex)) - DicePalaceBoozeLevelBossBase.ATTACK_DELAY)
			MyBase.animator.SetTrigger("OnAttack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack_Start", False, True)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.tumbler.beamWarningDuration)
			MyBase.animator.SetTrigger("Continue")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack", False)
			AudioManager.Play("booze_tumbler_attack")
			Me.emitAudioFromObject.Add("booze_tumbler_attack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
			Me.attackDelayIndex = (Me.attackDelayIndex + 1) Mod MyBase.properties.CurrentState.tumbler.beamDelayString.Split(New Char() { ","c }).Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001BCB RID: 7115 RVA: 0x000FD497 File Offset: 0x000FB897
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001BCC RID: 7116 RVA: 0x000FD4B5 File Offset: 0x000FB8B5
	Private Sub EnableSpray()
		AudioManager.Play("booze_tumbler_attack_spray")
		Me.emitAudioFromObject.Add("booze_tumbler_attack_spray")
		MyBase.animator.Play("Attack_Spray")
	End Sub

	' Token: 0x06001BCD RID: 7117 RVA: 0x000FD4E1 File Offset: 0x000FB8E1
	Private Sub TumblerDeathSFX()
		AudioManager.Play("tumbler_death_vox")
		Me.emitAudioFromObject.Add("tumbler_death_vox")
	End Sub

	' Token: 0x040024D4 RID: 9428
	<SerializeField()>
	Private sprayCollider As BoxCollider2D

	' Token: 0x040024D5 RID: 9429
	Private attackDelayIndex As Integer

	' Token: 0x040024D6 RID: 9430
	Private damageDealer As DamageDealer

	' Token: 0x040024D7 RID: 9431
	Private damageReceiver As DamageReceiver
End Class
