Imports System
Imports UnityEngine

' Token: 0x0200077D RID: 1917
Public Class RobotLevelHatchBombBot
	Inherits HomingProjectile

	' Token: 0x06002A0C RID: 10764 RVA: 0x001894F3 File Offset: 0x001878F3
	Public Sub InitBombBot(properties As LevelProperties.Robot.BombBot)
		Me.properties = properties
		Me.health = CSng(properties.bombHP)
	End Sub

	' Token: 0x06002A0D RID: 10765 RVA: 0x00189509 File Offset: 0x00187909
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.Die()
	End Sub

	' Token: 0x06002A0E RID: 10766 RVA: 0x00189519 File Offset: 0x00187919
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If hit.GetComponent(Of RobotLevelRobotBodyPart)() IsNot Nothing Then
			Me.Die()
		ElseIf hit.GetComponent(Of RobotLevelHatchBombBot)() IsNot Nothing Then
			Me.Die()
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06002A0F RID: 10767 RVA: 0x00189556 File Offset: 0x00187956
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F AndAlso Not Me.isDead Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002A10 RID: 10768 RVA: 0x0018958C File Offset: 0x0018798C
	Protected Overrides Sub Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x06002A11 RID: 10769 RVA: 0x001895B7 File Offset: 0x001879B7
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageDealer.SetDamage(CSng(Me.properties.bombBossDamage))
		Me.damageDealer.SetRate(0F)
	End Sub

	' Token: 0x06002A12 RID: 10770 RVA: 0x001895E6 File Offset: 0x001879E6
	Protected Overrides Sub Update()
		Me.damageDealer.Update()
		MyBase.Update()
	End Sub

	' Token: 0x06002A13 RID: 10771 RVA: 0x001895F9 File Offset: 0x001879F9
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06002A14 RID: 10772 RVA: 0x00189604 File Offset: 0x00187A04
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.isDead = True
		Me.StopAllCoroutines()
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		MyBase.animator.Play("Explode")
		AudioManager.Play("robot_bombbot_death")
		Me.emitAudioFromObject.Add("robot_bombbot_death")
	End Sub

	' Token: 0x040032EF RID: 13039
	<SerializeField()>
	Private explosion As Sprite

	' Token: 0x040032F0 RID: 13040
	Private isDead As Boolean

	' Token: 0x040032F1 RID: 13041
	Private health As Single

	' Token: 0x040032F2 RID: 13042
	Private damageReceiver As DamageReceiver

	' Token: 0x040032F3 RID: 13043
	Private properties As LevelProperties.Robot.BombBot
End Class
