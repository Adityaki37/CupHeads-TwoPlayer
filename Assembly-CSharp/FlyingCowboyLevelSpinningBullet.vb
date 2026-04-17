Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200065F RID: 1631
Public Class FlyingCowboyLevelSpinningBullet
	Inherits AbstractProjectile

	' Token: 0x060021F5 RID: 8693 RVA: 0x0013C558 File Offset: 0x0013A958
	Public Function Create(pos As Vector2, speed As Single, rotationSpeed As Single, rotationRadius As Single, direction As Vector3, clockwise As Boolean, parryable As Boolean) As FlyingCowboyLevelSpinningBullet
		Dim flyingCowboyLevelSpinningBullet As FlyingCowboyLevelSpinningBullet = TryCast(Me.Create(), FlyingCowboyLevelSpinningBullet)
		flyingCowboyLevelSpinningBullet.child.localPosition = New Vector3(rotationRadius, 0F)
		flyingCowboyLevelSpinningBullet.StartCoroutine(flyingCowboyLevelSpinningBullet.bullet_cr(pos, speed, rotationSpeed, direction, clockwise))
		flyingCowboyLevelSpinningBullet.StartCoroutine(flyingCowboyLevelSpinningBullet.scale_cr())
		flyingCowboyLevelSpinningBullet.SetParryable(parryable)
		Return flyingCowboyLevelSpinningBullet
	End Function

	' Token: 0x060021F6 RID: 8694 RVA: 0x0013C5B2 File Offset: 0x0013A9B2
	Protected Overrides Sub Update()
		MyBase.Update()
		Me.child.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
	End Sub

	' Token: 0x060021F7 RID: 8695 RVA: 0x0013C5E3 File Offset: 0x0013A9E3
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060021F8 RID: 8696 RVA: 0x0013C5F9 File Offset: 0x0013A9F9
	Protected Overrides Sub Die()
		Me.child.SetLocalEulerAngles(New Single?(0F), New Single?(0F), New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		MyBase.Die()
	End Sub

	' Token: 0x060021F9 RID: 8697 RVA: 0x0013C634 File Offset: 0x0013AA34
	Private Iterator Function scale_cr() As IEnumerator
		Dim initialScale As Vector3 = MyBase.transform.localScale
		MyBase.transform.localScale = initialScale * 0.75F
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.3F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			Dim scale As Vector3 = Vector3.Lerp(initialScale * 0.75F, initialScale, elapsedTime / 0.3F)
			MyBase.transform.localScale = scale
		End While
		Return
	End Function

	' Token: 0x060021FA RID: 8698 RVA: 0x0013C650 File Offset: 0x0013AA50
	Private Iterator Function bullet_cr(pos As Vector2, speed As Single, rotationSpeed As Single, direction As Vector3, clockwise As Boolean) As IEnumerator
		If Not clockwise Then
			MyBase.animator.SetFloat("Speed", -1F)
		End If
		MyBase.transform.position = pos - Me.child.localPosition
		While True
			MyBase.transform.position += direction * speed * CupheadTime.Delta
			MyBase.transform.AddEulerAngles(0F, 0F, CSng(If((Not clockwise), 1, (-1))) * rotationSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002AAC RID: 10924
	<SerializeField()>
	Private child As Transform
End Class
