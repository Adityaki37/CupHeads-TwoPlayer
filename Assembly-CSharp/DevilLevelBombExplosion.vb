Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200058A RID: 1418
Public Class DevilLevelBombExplosion
	Inherits Effect

	' Token: 0x06001B16 RID: 6934 RVA: 0x000F9057 File Offset: 0x000F7457
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001B17 RID: 6935 RVA: 0x000F906A File Offset: 0x000F746A
	Private Sub Start()
		MyBase.StartCoroutine(Me.timer_cr())
		AudioManager.Play("bat_bomb_explo")
	End Sub

	' Token: 0x06001B18 RID: 6936 RVA: 0x000F9083 File Offset: 0x000F7483
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001B19 RID: 6937 RVA: 0x000F909B File Offset: 0x000F749B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001B1A RID: 6938 RVA: 0x000F90C4 File Offset: 0x000F74C4
	Private Iterator Function timer_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.loopTime)
		MyBase.animator.SetTrigger("Continue")
		Return
	End Function

	' Token: 0x04002455 RID: 9301
	<SerializeField()>
	Private loopTime As Single

	' Token: 0x04002456 RID: 9302
	Private damageDealer As DamageDealer
End Class
