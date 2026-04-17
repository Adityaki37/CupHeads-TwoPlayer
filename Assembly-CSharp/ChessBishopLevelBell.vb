Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000535 RID: 1333
Public Class ChessBishopLevelBell
	Inherits AbstractProjectile

	' Token: 0x17000331 RID: 817
	' (get) Token: 0x0600181B RID: 6171 RVA: 0x000D9E03 File Offset: 0x000D8203
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x0600181C RID: 6172 RVA: 0x000D9E06 File Offset: 0x000D8206
	Public Overridable Function Init(pos As Vector3, player As AbstractPlayerController, properties As LevelProperties.ChessBishop.Bishop) As ChessBishopLevelBell
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		Me.properties = properties
		Me.player = player
		MyBase.StartCoroutine(Me.move_cr())
		Return Me
	End Function

	' Token: 0x0600181D RID: 6173 RVA: 0x000D9E3C File Offset: 0x000D823C
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x0600181E RID: 6174 RVA: 0x000D9E5C File Offset: 0x000D825C
	Private Iterator Function move_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.projectileDelayRange.RandomFloat())
		MyBase.animator.SetTrigger("Attack")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", 0, False, True)
		Dim direction As Vector3 = (Me.player.transform.position - MyBase.transform.position).normalized
		If MyBase.animator.GetInteger(AbstractProjectile.[Variant]) = 0 Then
			MyBase.animator.Play("A", 1)
			MyBase.animator.Play("A", 2)
			MyBase.animator.Play("IntroA", 3)
			For Each transform As Transform In Me.smokeTransforms
				transform.rotation = Quaternion.Euler(0F, 0F, 45F)
			Next
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(90F + MathUtils.DirectionToAngle(direction)))
		Else
			MyBase.animator.Play("B", 1)
		End If
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			MyBase.transform.position += direction * Me.properties.projectileSpeed * CupheadTime.FixedDelta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x04002148 RID: 8520
	Private Const AnimatorBaseLayer As Integer = 0

	' Token: 0x04002149 RID: 8521
	Private Const AnimatorSmokeTopLayer As Integer = 1

	' Token: 0x0400214A RID: 8522
	Private Const AnimatorSmokeMiddleLayer As Integer = 2

	' Token: 0x0400214B RID: 8523
	Private Const AnimatorSmokeBottomLayer As Integer = 3

	' Token: 0x0400214C RID: 8524
	<SerializeField()>
	Private smokeTransforms As Transform()

	' Token: 0x0400214D RID: 8525
	Private properties As LevelProperties.ChessBishop.Bishop

	' Token: 0x0400214E RID: 8526
	Private player As AbstractPlayerController
End Class
