Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000522 RID: 1314
Public Class BeeLevelSecurityGuard
	Inherits LevelProperties.Bee.Entity

	' Token: 0x1700032C RID: 812
	' (get) Token: 0x06001793 RID: 6035 RVA: 0x000D47EC File Offset: 0x000D2BEC
	' (set) Token: 0x06001794 RID: 6036 RVA: 0x000D47F4 File Offset: 0x000D2BF4
	Public Property state As BeeLevelSecurityGuard.State

	' Token: 0x06001795 RID: 6037 RVA: 0x000D4800 File Offset: 0x000D2C00
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnTakeDamage
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.circleCollider = MyBase.GetComponent(Of CircleCollider2D)()
	End Sub

	' Token: 0x06001796 RID: 6038 RVA: 0x000D484D File Offset: 0x000D2C4D
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001797 RID: 6039 RVA: 0x000D4865 File Offset: 0x000D2C65
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001798 RID: 6040 RVA: 0x000D488E File Offset: 0x000D2C8E
	Private Sub OnTakeDamage(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001799 RID: 6041 RVA: 0x000D48A4 File Offset: 0x000D2CA4
	Public Sub StartSecurityGuard()
		Me.ResetGuard()
		Me.p = MyBase.properties.CurrentState.securityGuard
		AddHandler MyBase.properties.OnStateChange, AddressOf Me.OnStateChange
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x0600179A RID: 6042 RVA: 0x000D48F1 File Offset: 0x000D2CF1
	Private Sub OnStateChange()
		RemoveHandler MyBase.properties.OnStateChange, AddressOf Me.OnStateChange
		Me.Die()
	End Sub

	' Token: 0x0600179B RID: 6043 RVA: 0x000D4910 File Offset: 0x000D2D10
	Private Sub Die()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.leave_cr())
	End Sub

	' Token: 0x0600179C RID: 6044 RVA: 0x000D4925 File Offset: 0x000D2D25
	Private Sub ResetGuard()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x0600179D RID: 6045 RVA: 0x000D492D File Offset: 0x000D2D2D
	Private Sub SfxThrow()
		AudioManager.Play("bee_guard_attack")
		Me.emitAudioFromObject.Add("bee_guard_attack")
	End Sub

	' Token: 0x0600179E RID: 6046 RVA: 0x000D494C File Offset: 0x000D2D4C
	Private Sub Attack()
		Me.bombPrefab.Create(Me.bombRoot.position, -CInt(MyBase.transform.localScale.x), Me.p.idleTime, Me.p.warningTime, Me.p.childSpeed, Me.p.childCount)
	End Sub

	' Token: 0x0600179F RID: 6047 RVA: 0x000D49B6 File Offset: 0x000D2DB6
	Private Sub AttackComplete()
	End Sub

	' Token: 0x060017A0 RID: 6048 RVA: 0x000D49B8 File Offset: 0x000D2DB8
	Private Sub FlipX()
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
	End Sub

	' Token: 0x060017A1 RID: 6049 RVA: 0x000D49FD File Offset: 0x000D2DFD
	Private Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x060017A2 RID: 6050 RVA: 0x000D4A20 File Offset: 0x000D2E20
	Private Iterator Function go_cr() As IEnumerator
		AudioManager.Play("bee_guard_spawn")
		Me.emitAudioFromObject.Add("bee_guard_spawn")
		AudioManager.PlayLoop("bee_guard_flying_loop")
		Me.emitAudioFromObject.Add("bee_guard_flying_loop")
		While True
			Yield MyBase.StartCoroutine(Me.move_cr())
			Yield MyBase.StartCoroutine(Me.attack_cr())
		End While
		Return
	End Function

	' Token: 0x060017A3 RID: 6051 RVA: 0x000D4A3C File Offset: 0x000D2E3C
	Private Iterator Function move_cr() As IEnumerator
		Me.state = BeeLevelSecurityGuard.State.Move
		Dim t As Single = 0F
		Dim time As Single = Me.p.attackDelay.RandomFloat()
		While t < time
			MyBase.transform.AddPositionForward2D(-Me.p.speed * CupheadTime.Delta * MyBase.transform.localScale.x * Me.hitPauseCoefficient())
			If(MyBase.transform.localScale.x > 0F AndAlso MyBase.transform.position.x <= -490F) OrElse (MyBase.transform.localScale.x < 0F AndAlso MyBase.transform.position.x >= 490F) Then
				Yield MyBase.StartCoroutine(Me.turn_cr())
			End If
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060017A4 RID: 6052 RVA: 0x000D4A58 File Offset: 0x000D2E58
	Private Iterator Function attack_cr() As IEnumerator
		Me.state = BeeLevelSecurityGuard.State.Attack
		MyBase.animator.SetTrigger("OnAttack")
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Attack", False, True)
		Return
	End Function

	' Token: 0x060017A5 RID: 6053 RVA: 0x000D4A74 File Offset: 0x000D2E74
	Private Iterator Function leave_cr() As IEnumerator
		Dim exploder As LevelBossDeathExploder = MyBase.GetComponent(Of LevelBossDeathExploder)()
		Me.state = BeeLevelSecurityGuard.State.Leaving
		exploder.StartExplosion()
		If MyBase.transform.localScale.x < 0F AndAlso MyBase.transform.position.x < 0F Then
			MyBase.transform.SetScale(New Single?(-1F), New Single?(1F), New Single?(1F))
		End If
		If MyBase.transform.localScale.x > 0F AndAlso MyBase.transform.position.x > 0F Then
			MyBase.transform.SetScale(New Single?(1F), New Single?(1F), New Single?(1F))
		End If
		MyBase.animator.Play("Leave")
		AudioManager.[Stop]("bee_guard_flying_loop")
		AudioManager.Play("bee_guard_leave")
		Me.emitAudioFromObject.Add("bee_guard_leave")
		Me.circleCollider.enabled = False
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		exploder.StopExplosions()
		AudioManager.Play("bee_guard_death")
		Me.emitAudioFromObject.Add("bee_guard_death")
		Dim leave As Boolean = True
		While leave
			MyBase.transform.AddPositionForward2D(-Me.p.speed * CupheadTime.Delta * MyBase.transform.localScale.x * Me.hitPauseCoefficient())
			Yield Nothing
			If MyBase.transform.position.x > 1280F OrElse MyBase.transform.position.x < -1280F Then
				leave = False
			End If
		End While
		Me.state = BeeLevelSecurityGuard.State.Ready
		Return
	End Function

	' Token: 0x060017A6 RID: 6054 RVA: 0x000D4A90 File Offset: 0x000D2E90
	Private Iterator Function turn_cr() As IEnumerator
		MyBase.animator.Play("Turn")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Turn", False, True)
		MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
		Return
	End Function

	' Token: 0x060017A7 RID: 6055 RVA: 0x000D4AAB File Offset: 0x000D2EAB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bombPrefab = Nothing
	End Sub

	' Token: 0x040020CC RID: 8396
	<SerializeField()>
	Private bombRoot As Transform

	' Token: 0x040020CD RID: 8397
	<SerializeField()>
	Private bombPrefab As BeeLevelSecurityGuardBomb

	' Token: 0x040020CE RID: 8398
	Private p As LevelProperties.Bee.SecurityGuard

	' Token: 0x040020CF RID: 8399
	Private damageReceiver As DamageReceiver

	' Token: 0x040020D0 RID: 8400
	Private damageDealer As DamageDealer

	' Token: 0x040020D1 RID: 8401
	Private circleCollider As CircleCollider2D

	' Token: 0x02000523 RID: 1315
	Public Enum State
		' Token: 0x040020D3 RID: 8403
		Ready
		' Token: 0x040020D4 RID: 8404
		Move
		' Token: 0x040020D5 RID: 8405
		Attack
		' Token: 0x040020D6 RID: 8406
		Leaving
	End Enum
End Class
