Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007AC RID: 1964
Public Class SallyStagePlayLevelFianceDeity
	Inherits LevelProperties.SallyStagePlay.Entity

	' Token: 0x06002C24 RID: 11300 RVA: 0x0019F5C0 File Offset: 0x0019D9C0
	Private Sub Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06002C25 RID: 11301 RVA: 0x0019F5F1 File Offset: 0x0019D9F1
	Public Sub Attack()
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06002C26 RID: 11302 RVA: 0x0019F600 File Offset: 0x0019DA00
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If SallyStagePlayLevelAngel.extraHP > 0F Then
			SallyStagePlayLevelAngel.extraHP -= info.damage
		Else
			MyBase.properties.DealDamage(info.damage)
		End If
	End Sub

	' Token: 0x06002C27 RID: 11303 RVA: 0x0019F638 File Offset: 0x0019DA38
	Private Iterator Function attack_cr() As IEnumerator
		Dim p As LevelProperties.SallyStagePlay.Husband = MyBase.properties.CurrentState.husband
		While Not Me.isDead
			Yield CupheadTime.WaitForSeconds(Me, p.shotDelayRange.RandomFloat())
			MyBase.GetComponent(Of Animator)().SetBool("OnAttack", True)
			Yield MyBase.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Puppet_Attack_Start", False, True)
			Me.cherubProjectile.Create(Me.husbandRoot.position, 0F, p.shotSpeed)
			MyBase.GetComponent(Of Animator)().SetBool("OnAttack", False)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002C28 RID: 11304 RVA: 0x0019F653 File Offset: 0x0019DA53
	Public Sub Dead()
		Me.isDead = True
		Me.StopAllCoroutines()
		Me.damageReceiver.enabled = False
		MyBase.GetComponent(Of Animator)().SetTrigger("OnDeath")
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002C29 RID: 11305 RVA: 0x0019F68C File Offset: 0x0019DA8C
	Public Iterator Function move_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 3F
		Dim endPos As Vector3 = New Vector3(-1140F, MyBase.transform.position.y)
		Dim start As Vector2 = MyBase.transform.position
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
		Yield CupheadTime.WaitForSeconds(Me, 0.8F)
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(start, endPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Return
	End Function

	' Token: 0x06002C2A RID: 11306 RVA: 0x0019F6A7 File Offset: 0x0019DAA7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.cherubProjectile = Nothing
	End Sub

	' Token: 0x040034D6 RID: 13526
	<SerializeField()>
	Private cherubProjectile As SallyStagePlayLevelCherubProjectile

	' Token: 0x040034D7 RID: 13527
	<SerializeField()>
	Private husbandRoot As Transform

	' Token: 0x040034D8 RID: 13528
	Private isDead As Boolean

	' Token: 0x040034D9 RID: 13529
	Private health As Single

	' Token: 0x040034DA RID: 13530
	Private damageReceiver As DamageReceiver
End Class
