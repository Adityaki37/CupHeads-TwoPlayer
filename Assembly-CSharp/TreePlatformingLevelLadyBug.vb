Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200088E RID: 2190
Public Class TreePlatformingLevelLadyBug
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x060032EE RID: 13038 RVA: 0x001D9874 File Offset: 0x001D7C74
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.manuallySetJumpX = True
	End Sub

	' Token: 0x060032EF RID: 13039 RVA: 0x001D9883 File Offset: 0x001D7C83
	Protected Overrides Sub Start()
		Me.Setup()
		MyBase.Start()
	End Sub

	' Token: 0x060032F0 RID: 13040 RVA: 0x001D9894 File Offset: 0x001D7C94
	Public Function Spawn(pos As Vector3, dir As PlatformingLevelGroundMovementEnemy.Direction, destroy As Boolean, type As TreePlatformingLevelLadyBug.Type) As TreePlatformingLevelLadyBug
		Dim treePlatformingLevelLadyBug As TreePlatformingLevelLadyBug = TryCast(MyBase.Spawn(pos, dir, destroy), TreePlatformingLevelLadyBug)
		treePlatformingLevelLadyBug.type = type
		Return treePlatformingLevelLadyBug
	End Function

	' Token: 0x060032F1 RID: 13041 RVA: 0x001D98BC File Offset: 0x001D7CBC
	Public Sub Setup()
		Select Case Me.type
			Case TreePlatformingLevelLadyBug.Type.GroundFast
				MyBase.GoToGround(True, "Fast_Ground")
				AudioManager.PlayLoop("level_platform_ladybug_ground_fast_loop")
				Me.emitAudioFromObject.Add("level_platform_ladybug_ground_fast_loop")
				Me.SetMoveSpeed(MyBase.Properties.fastMovement)
				Me.noTurn = True
				MyBase.StartCoroutine(Me.no_y_cr())
			Case TreePlatformingLevelLadyBug.Type.GroundSlow
				MyBase.GoToGround(True, "Slow_Ground")
				AudioManager.PlayLoop("level_platform_ladybug_ground_slow_loop")
				Me.emitAudioFromObject.Add("level_platform_ladybug_ground_slow_loop")
				Me.SetMoveSpeed(MyBase.Properties.slowMovement)
				Me.noTurn = True
				MyBase.StartCoroutine(Me.no_y_cr())
			Case TreePlatformingLevelLadyBug.Type.BounceFast
				MyBase.animator.Play("Fast_Bounce")
				AudioManager.PlayLoop("level_platform_ladybug_bounce_fast_loop")
				Me.emitAudioFromObject.Add("level_platform_ladybug_bounce_fast_loop")
				Me.SetMoveSpeed(MyBase.Properties.fastMovement)
				MyBase.StartCoroutine(Me.y_cr())
				Me.noTurn = True
			Case TreePlatformingLevelLadyBug.Type.BounceSlow
				MyBase.animator.Play("Slow_Bounce")
				AudioManager.PlayLoop("level_platform_ladybug_bounce_slow_loop")
				Me.emitAudioFromObject.Add("level_platform_ladybug_bounce_slow_loop")
				Me.SetMoveSpeed(MyBase.Properties.slowMovement)
				MyBase.StartCoroutine(Me.y_cr())
				Me.noTurn = True
			Case TreePlatformingLevelLadyBug.Type.BouncePink
				Me._canParry = True
				MyBase.animator.Play("Pink_Slow_Ground")
				AudioManager.PlayLoop("level_platform_ladybug_ground_slow_loop")
				Me.emitAudioFromObject.Add("level_platform_ladybug_ground_slow_loop")
				Me.SetMoveSpeed(MyBase.Properties.slowMovement)
				MyBase.StartCoroutine(Me.y_cr())
				Me.noTurn = True
		End Select
	End Sub

	' Token: 0x060032F2 RID: 13042 RVA: 0x001D9A98 File Offset: 0x001D7E98
	Private Iterator Function y_cr() As IEnumerator
		Me.floating = False
		Yield Nothing
		While True
			While Not MyBase.Grounded
				Yield Nothing
			End While
			MyBase.Jump()
			AudioManager.Play("level_platform_ladybug_bounce")
			Me.emitAudioFromObject.Add("level_platform_ladybug_bounce")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032F3 RID: 13043 RVA: 0x001D9AB4 File Offset: 0x001D7EB4
	Private Iterator Function no_y_cr() As IEnumerator
		While True
			If Not MyBase.Grounded Then
				Me.fallInPit = True
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060032F4 RID: 13044 RVA: 0x001D9AD0 File Offset: 0x001D7ED0
	Protected Overrides Sub Die()
		MyBase.Die()
		AudioManager.Play("level_platform_ladybug_death")
		Me.emitAudioFromObject.Add("level_platform_ladybug_death")
		Select Case Me.type
			Case TreePlatformingLevelLadyBug.Type.GroundFast
				AudioManager.[Stop]("level_platform_ladybug_ground_fast_loop")
			Case TreePlatformingLevelLadyBug.Type.GroundSlow
				AudioManager.[Stop]("level_platform_ladybug_ground_slow_loop")
			Case TreePlatformingLevelLadyBug.Type.BounceFast
				AudioManager.[Stop]("level_platform_ladybug_bounce_fast_loop")
			Case TreePlatformingLevelLadyBug.Type.BounceSlow
				AudioManager.[Stop]("level_platform_ladybug_bounce_slow_loop")
			Case TreePlatformingLevelLadyBug.Type.BouncePink
				AudioManager.[Stop]("level_platform_ladybug_ground_slow_loop")
		End Select
	End Sub

	' Token: 0x04003B13 RID: 15123
	Public type As TreePlatformingLevelLadyBug.Type

	' Token: 0x0200088F RID: 2191
	Public Enum Type
		' Token: 0x04003B15 RID: 15125
		GroundFast
		' Token: 0x04003B16 RID: 15126
		GroundSlow
		' Token: 0x04003B17 RID: 15127
		BounceFast
		' Token: 0x04003B18 RID: 15128
		BounceSlow
		' Token: 0x04003B19 RID: 15129
		BouncePink
	End Enum
End Class
