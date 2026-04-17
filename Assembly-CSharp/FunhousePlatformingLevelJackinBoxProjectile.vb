Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008B8 RID: 2232
Public Class FunhousePlatformingLevelJackinBoxProjectile
	Inherits BasicProjectile

	' Token: 0x06003417 RID: 13335 RVA: 0x001E3E5F File Offset: 0x001E225F
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.move = False
		MyBase.StartCoroutine(Me.animation_cr())
	End Sub

	' Token: 0x06003418 RID: 13336 RVA: 0x001E3E7C File Offset: 0x001E227C
	Public Function Create(pos As Vector3, speed As Single, delay As Single, player As AbstractPlayerController, direction As Integer) As FunhousePlatformingLevelJackinBoxProjectile
		Dim funhousePlatformingLevelJackinBoxProjectile As FunhousePlatformingLevelJackinBoxProjectile = TryCast(MyBase.Create(pos, 0F, speed), FunhousePlatformingLevelJackinBoxProjectile)
		funhousePlatformingLevelJackinBoxProjectile.delay = delay
		funhousePlatformingLevelJackinBoxProjectile.player = player
		funhousePlatformingLevelJackinBoxProjectile.StartAnimation(direction)
		Return funhousePlatformingLevelJackinBoxProjectile
	End Function

	' Token: 0x06003419 RID: 13337 RVA: 0x001E3EBC File Offset: 0x001E22BC
	Private Sub StartAnimation(direction As Integer)
		Select Case direction
			Case 1
				MyBase.animator.Play("Top_Start")
			Case 2
				MyBase.animator.Play("Left_Start")
			Case 3
				MyBase.animator.Play("Bottom_Start")
			Case 4
				MyBase.animator.Play("Right_Start")
		End Select
	End Sub

	' Token: 0x0600341A RID: 13338 RVA: 0x001E3F3C File Offset: 0x001E233C
	Private Iterator Function animation_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Projectile", False)
		Yield CupheadTime.WaitForSeconds(Me, Me.delay)
		MyBase.animator.SetTrigger("Move")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Projectile_Move_Start", False, True)
		Dim dir As Vector3 = Me.player.transform.position - MyBase.transform.position
		Dim start As Single = MyBase.transform.rotation.z
		Dim [end] As Single = MathUtils.DirectionToAngle(dir)
		Dim t As Single = 0F
		Dim time As Single = 0.1F
		While t < time
			t += CupheadTime.Delta
			MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(Mathf.Lerp(start, [end], t / time)))
			Yield Nothing
		End While
		Me.move = True
		Yield Nothing
		Return
	End Function

	' Token: 0x04003C5D RID: 15453
	Private player As AbstractPlayerController

	' Token: 0x04003C5E RID: 15454
	Private delay As Single
End Class
