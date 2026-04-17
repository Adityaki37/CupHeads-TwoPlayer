Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008E8 RID: 2280
Public Class MountainPlatformingLevelMudman
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x0600356C RID: 13676 RVA: 0x001F23DD File Offset: 0x001F07DD
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.come_up_cr())
		MyBase.StartCoroutine(Me.check_cr())
	End Sub

	' Token: 0x0600356D RID: 13677 RVA: 0x001F240B File Offset: 0x001F080B
	Protected Overrides Sub FixedUpdate()
		If Not Me.melting Then
			MyBase.FixedUpdate()
		End If
	End Sub

	' Token: 0x0600356E RID: 13678 RVA: 0x001F2420 File Offset: 0x001F0820
	Public Sub Init(pos As Vector3, direction As PlatformingLevelGroundMovementEnemy.Direction)
		MyBase.transform.position = pos
		Me._direction = direction
		MyBase.transform.SetScale(New Single?(CSng(If((direction <> PlatformingLevelGroundMovementEnemy.Direction.Right), 1, (-1)))), Nothing, Nothing)
	End Sub

	' Token: 0x0600356F RID: 13679 RVA: 0x001F2474 File Offset: 0x001F0874
	Private Iterator Function come_up_cr() As IEnumerator
		MyBase.animator.SetTrigger("Intro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Me.melting = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06003570 RID: 13680 RVA: 0x001F2490 File Offset: 0x001F0890
	Private Iterator Function check_cr() As IEnumerator
		While MountainPlatformingLevelElevatorHandler.elevatorIsMoving
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Outro")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Outro", False)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		Return
	End Function

	' Token: 0x06003571 RID: 13681 RVA: 0x001F24AB File Offset: 0x001F08AB
	Protected Overrides Sub CalculateDirection()
	End Sub

	' Token: 0x06003572 RID: 13682 RVA: 0x001F24AD File Offset: 0x001F08AD
	Protected Overrides Function Turn() As Coroutine
		Me.StopAllCoroutines()
		Return MyBase.StartCoroutine(Me.despawn_cr())
	End Function

	' Token: 0x06003573 RID: 13683 RVA: 0x001F24C4 File Offset: 0x001F08C4
	Private Iterator Function despawn_cr() As IEnumerator
		Me.melting = True
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.SetTrigger("Outro")
		Yield Nothing
		Return
	End Function

	' Token: 0x06003574 RID: 13684 RVA: 0x001F24E0 File Offset: 0x001F08E0
	Private Iterator Function explode_cr() As IEnumerator
		For i As Integer = 0 To Me.explodeSpawns.Length - 1
			Me.splash.Create(Me.explodeSpawns(i).position)
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x06003575 RID: 13685 RVA: 0x001F24FB File Offset: 0x001F08FB
	Protected Overrides Sub Die()
		MyBase.FrameDelayedCallback(Sub()
			Me.otherExplosion.Create(MyBase.GetComponent(Of Collider2D)().bounds.center)
			Me.<Die>__BaseCallProxy0()
		End Sub, 1)
		MyBase.StartCoroutine(Me.explode_cr())
		If Me.isBig Then
			Me.MudmanBigDeathSFX()
		Else
			Me.MudmanSmallDeathSFX()
		End If
	End Sub

	' Token: 0x06003576 RID: 13686 RVA: 0x001F253A File Offset: 0x001F093A
	Private Sub Delete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003577 RID: 13687 RVA: 0x001F2547 File Offset: 0x001F0947
	Private Sub MudmanBigSpawnSFX()
		AudioManager.Play("castle_mudman_small_spawn")
		Me.emitAudioFromObject.Add("castle_mudman_small_spawn")
	End Sub

	' Token: 0x06003578 RID: 13688 RVA: 0x001F2563 File Offset: 0x001F0963
	Private Sub MudmanBigDeathSFX()
		AudioManager.Play("castle_mudman_large_death")
		Me.emitAudioFromObject.Add("castle_mudman_large_death")
	End Sub

	' Token: 0x06003579 RID: 13689 RVA: 0x001F257F File Offset: 0x001F097F
	Private Sub MudmanSmallSpawnSFX()
		AudioManager.Play("castle_mudman_large_spawn")
		Me.emitAudioFromObject.Add("castle_mudman_large_spawn")
	End Sub

	' Token: 0x0600357A RID: 13690 RVA: 0x001F259B File Offset: 0x001F099B
	Private Sub MudmanSmallDeathSFX()
		AudioManager.Play("castle_mudman_small_death")
		Me.emitAudioFromObject.Add("castle_mudman_small_death")
	End Sub

	' Token: 0x04003D8F RID: 15759
	<SerializeField()>
	Private splash As PlatformingLevelGenericExplosion

	' Token: 0x04003D90 RID: 15760
	<SerializeField()>
	Private explodeSpawns As Transform()

	' Token: 0x04003D91 RID: 15761
	<SerializeField()>
	Private otherExplosion As PlatformingLevelGenericExplosion

	' Token: 0x04003D92 RID: 15762
	<SerializeField()>
	Private isBig As Boolean

	' Token: 0x04003D93 RID: 15763
	Private melting As Boolean = True
End Class
