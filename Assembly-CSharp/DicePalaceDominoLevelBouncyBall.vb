Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005B3 RID: 1459
Public Class DicePalaceDominoLevelBouncyBall
	Inherits AbstractProjectile

	' Token: 0x06001C4A RID: 7242 RVA: 0x0010334E File Offset: 0x0010174E
	Public Sub InitBouncyBall(speed As Single, direction As Vector3)
		Me.deltaPosition = direction * speed
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.checkCollisions_cr())
	End Sub

	' Token: 0x06001C4B RID: 7243 RVA: 0x00103377 File Offset: 0x00101777
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		If parryable Then
			MyBase.animator.SetInteger("Variation", 3)
		Else
			MyBase.animator.SetInteger("Variation", Global.UnityEngine.Random.Range(1, 3))
		End If
	End Sub

	' Token: 0x06001C4C RID: 7244 RVA: 0x001033B4 File Offset: 0x001017B4
	Private Iterator Function move_cr() As IEnumerator
		While True
			MyBase.transform.position += Me.deltaPosition * CupheadTime.Delta
			For i As Integer = 0 To Me.toRotate.Length - 1
				Me.toRotate(i).Rotate(Vector3.forward, 180F * CupheadTime.Delta)
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001C4D RID: 7245 RVA: 0x001033D0 File Offset: 0x001017D0
	Private Iterator Function checkCollisions_cr() As IEnumerator
		While True
			If MyBase.transform.position.y > CSng(Level.Current.Ceiling) Then
				Me.deltaPosition.y = Me.deltaPosition.y * -1F
				Me.BounceSFX()
				MyBase.animator.SetTrigger("Bounce")
				Me.hitEffectPrefab.Create(MyBase.transform.position, New Vector3(1F, -1F, 1F))
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			End If
			If MyBase.transform.position.y < CSng(Level.Current.Ground) Then
				Me.deltaPosition.y = Me.deltaPosition.y * -1F
				Me.BounceSFX()
				MyBase.animator.SetTrigger("Bounce")
				Me.hitEffectPrefab.Create(MyBase.transform.position)
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			End If
			If MyBase.transform.position.x > CSng(Level.Current.Right) Then
				Me.deltaPosition.x = Me.deltaPosition.x * -1F
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			End If
			If MyBase.transform.position.x < CSng(Level.Current.Left) Then
				Me.deltaPosition.x = Me.deltaPosition.x * -1F
				Yield CupheadTime.WaitForSeconds(Me, 1F)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001C4E RID: 7246 RVA: 0x001033EB File Offset: 0x001017EB
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionWalls(hit, phase)
		If hit.GetComponent(Of BasicDamageDealingObject)() Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001C4F RID: 7247 RVA: 0x0010340B File Offset: 0x0010180B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001C50 RID: 7248 RVA: 0x00103429 File Offset: 0x00101829
	Protected Overrides Sub OnDestroy()
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
		Me.hitEffectPrefab = Nothing
		Me.explosion = Nothing
	End Sub

	' Token: 0x06001C51 RID: 7249 RVA: 0x00103445 File Offset: 0x00101845
	Protected Overrides Sub Die()
		Me.BounceSFX()
		Me.explosion.Create(MyBase.transform.position)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001C52 RID: 7250 RVA: 0x0010346F File Offset: 0x0010186F
	Private Sub BounceSFX()
		AudioManager.Play("dice_projectile_bounce")
		Me.emitAudioFromObject.Add("dice_projectile_bounce")
	End Sub

	' Token: 0x0400254A RID: 9546
	Private Const RotationFactor As Single = 180F

	' Token: 0x0400254B RID: 9547
	<SerializeField()>
	Private hitEffectPrefab As Effect

	' Token: 0x0400254C RID: 9548
	<SerializeField()>
	Private explosion As Effect

	' Token: 0x0400254D RID: 9549
	<SerializeField()>
	Private toRotate As Transform()

	' Token: 0x0400254E RID: 9550
	Private deltaPosition As Vector3

	' Token: 0x020005B4 RID: 1460
	Public Enum Colour
		' Token: 0x04002550 RID: 9552
		blue
		' Token: 0x04002551 RID: 9553
		green
		' Token: 0x04002552 RID: 9554
		red
		' Token: 0x04002553 RID: 9555
		yellow
		' Token: 0x04002554 RID: 9556
		none
	End Enum
End Class
