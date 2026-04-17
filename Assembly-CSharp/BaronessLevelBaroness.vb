Imports System
Imports UnityEngine

' Token: 0x020004DC RID: 1244
Public Class BaronessLevelBaroness
	Inherits AbstractCollidableObject

	' Token: 0x0600154A RID: 5450 RVA: 0x000BF04D File Offset: 0x000BD44D
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isEasyFinal = False
		Me.damageReceiver = Me.shootPoint.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x0600154B RID: 5451 RVA: 0x000BF084 File Offset: 0x000BD484
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.isEasyFinal Then
			Me.properties.DealDamage(info.damage)
			If Me.properties.CurrentHealth <= 0F AndAlso Me.isEasyFinal Then
				Me.isEasyFinal = False
			End If
		ElseIf Me.health < 0F AndAlso Not Me.shotEnough Then
			Me.shotEnough = True
		End If
	End Sub

	' Token: 0x0600154C RID: 5452 RVA: 0x000BF10E File Offset: 0x000BD50E
	Private Sub Update()
		If Me.shotEnough Then
			Me.health = Me.maxHealth
		End If
	End Sub

	' Token: 0x0600154D RID: 5453 RVA: 0x000BF127 File Offset: 0x000BD527
	Public Sub getProperties(properties As LevelProperties.Baroness, health As Single, parent As BaronessLevelCastle)
		Me.properties = properties
		Me.maxHealth = health
		Me.parent = parent
		health = Me.maxHealth
	End Sub

	' Token: 0x0600154E RID: 5454 RVA: 0x000BF146 File Offset: 0x000BD546
	Public Sub ShootCounter()
		Me.FireProjectileBunch()
		Me.shootCounter += 1
	End Sub

	' Token: 0x0600154F RID: 5455 RVA: 0x000BF15C File Offset: 0x000BD55C
	Public Sub PopUpCounter()
		Me.popUpCounter += 1
	End Sub

	' Token: 0x06001550 RID: 5456 RVA: 0x000BF16C File Offset: 0x000BD56C
	Public Sub TransformCounter()
		Me.transformCounter += 1
	End Sub

	' Token: 0x06001551 RID: 5457 RVA: 0x000BF17C File Offset: 0x000BD57C
	Private Sub FireProjectileBunch()
		AudioManager.Play("level_baroness_gun_fire")
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim num As Single = [next].transform.position.x - MyBase.transform.position.x
		Dim num2 As Single = [next].transform.position.y - MyBase.transform.position.y
		Dim num3 As Single = Mathf.Atan2(num2, num) * 57.29578F
		Dim baronessLevelBaronessProjectileBunch As BaronessLevelBaronessProjectileBunch = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelBaronessProjectileBunch)(Me.baronessProjectileBunch)
		baronessLevelBaronessProjectileBunch.Init(Me.baronessProjectileShootPoint.position, CSng(Me.properties.CurrentState.baronessVonBonbon.projectileSpeed), num3, Me.properties.CurrentState.baronessVonBonbon, Me.parent)
	End Sub

	' Token: 0x06001552 RID: 5458 RVA: 0x000BF250 File Offset: 0x000BD650
	Public Sub FireFinalProjectile()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim baronessLevelFollowingProjectile As BaronessLevelFollowingProjectile = Global.UnityEngine.[Object].Instantiate(Of BaronessLevelFollowingProjectile)(Me.baronessFollowProjectile)
		baronessLevelFollowingProjectile.Init(Me.baronessTossPoint.position, [next].transform.position, Me.properties.CurrentState.baronessVonBonbon, [next], Me.parent)
	End Sub

	' Token: 0x06001553 RID: 5459 RVA: 0x000BF2A7 File Offset: 0x000BD6A7
	Private Sub SoundVoiceAngry()
		AudioManager.Play("level_baroness_voice_angry")
		Me.emitAudioFromObject.Add("level_baroness_voice_angry")
	End Sub

	' Token: 0x06001554 RID: 5460 RVA: 0x000BF2C3 File Offset: 0x000BD6C3
	Private Sub SoundVoiceEffort()
		AudioManager.Play("level_baroness_voice_effort")
		Me.emitAudioFromObject.Add("level_baroness_voice_effort")
	End Sub

	' Token: 0x06001555 RID: 5461 RVA: 0x000BF2DF File Offset: 0x000BD6DF
	Private Sub SoundVoiceCastleyank()
		AudioManager.Play("level_baroness_voice_castleyank")
		Me.emitAudioFromObject.Add("level_baroness_voice_castleyank")
	End Sub

	' Token: 0x06001556 RID: 5462 RVA: 0x000BF2FB File Offset: 0x000BD6FB
	Private Sub SoundVoiceIntroA()
		AudioManager.Play("level_baroness_voice_intro_a")
		Me.emitAudioFromObject.Add("level_baroness_voice_intro_a")
	End Sub

	' Token: 0x06001557 RID: 5463 RVA: 0x000BF317 File Offset: 0x000BD717
	Private Sub SoundVoiceIntroB()
		AudioManager.Play("level_baroness_voice_intro_b")
		Me.emitAudioFromObject.Add("level_baroness_voice_intro_b")
	End Sub

	' Token: 0x06001558 RID: 5464 RVA: 0x000BF333 File Offset: 0x000BD733
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.baronessProjectileBunch = Nothing
		Me.baronessFollowProjectile = Nothing
	End Sub

	' Token: 0x04001EA4 RID: 7844
	<SerializeField()>
	Private baronessTossPoint As Transform

	' Token: 0x04001EA5 RID: 7845
	<SerializeField()>
	Private baronessProjectileShootPoint As Transform

	' Token: 0x04001EA6 RID: 7846
	<SerializeField()>
	Private baronessProjectileBunch As BaronessLevelBaronessProjectileBunch

	' Token: 0x04001EA7 RID: 7847
	<SerializeField()>
	Private baronessFollowProjectile As BaronessLevelFollowingProjectile

	' Token: 0x04001EA8 RID: 7848
	<SerializeField()>
	Public shootPoint As Transform

	' Token: 0x04001EA9 RID: 7849
	Private properties As LevelProperties.Baroness

	' Token: 0x04001EAA RID: 7850
	Private parent As BaronessLevelCastle

	' Token: 0x04001EAB RID: 7851
	Public isEasyFinal As Boolean

	' Token: 0x04001EAC RID: 7852
	Public shootCounter As Integer

	' Token: 0x04001EAD RID: 7853
	Public popUpCounter As Integer

	' Token: 0x04001EAE RID: 7854
	Public transformCounter As Integer

	' Token: 0x04001EAF RID: 7855
	Public shotEnough As Boolean

	' Token: 0x04001EB0 RID: 7856
	Private health As Single

	' Token: 0x04001EB1 RID: 7857
	Private maxHealth As Single

	' Token: 0x04001EB2 RID: 7858
	Private damageReceiver As DamageReceiver
End Class
