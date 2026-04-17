Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000636 RID: 1590
Public Class FlyingBlimpLevelEnemyProjectile
	Inherits BasicProjectile

	' Token: 0x0600209A RID: 8346 RVA: 0x0012C8D3 File Offset: 0x0012ACD3
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.spawn_fx_cr())
	End Sub

	' Token: 0x0600209B RID: 8347 RVA: 0x0012C8E8 File Offset: 0x0012ACE8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.animator.SetTrigger("dead")
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x0600209C RID: 8348 RVA: 0x0012C902 File Offset: 0x0012AD02
	Private Sub Destroy()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600209D RID: 8349 RVA: 0x0012C910 File Offset: 0x0012AD10
	Private Iterator Function spawn_fx_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.17F)
		While True
			Me.FX.Create(Me.root.transform.position).transform.SetEulerAngles(Nothing, Nothing, New Single?(MyBase.transform.eulerAngles.z))
			Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		End While
		Return
	End Function

	' Token: 0x0400291F RID: 10527
	<SerializeField()>
	Private FX As Effect

	' Token: 0x04002920 RID: 10528
	<SerializeField()>
	Private root As Transform
End Class
