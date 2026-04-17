Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008C6 RID: 2246
Public Class HarbourPlatformingLevelCrab
	Inherits PlatformingLevelGroundMovementEnemy

	' Token: 0x0600347D RID: 13437 RVA: 0x001E7AE4 File Offset: 0x001E5EE4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.animator.SetBool("goingLeft", MyBase.direction <> PlatformingLevelGroundMovementEnemy.Direction.Right)
		MyBase.GetComponent(Of DamageReceiver)().enabled = False
		Me.walkingBack = False
		Me.SetTurnTarget(Me.target)
	End Sub

	' Token: 0x0600347E RID: 13438 RVA: 0x001E7B39 File Offset: 0x001E5F39
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.play_loop_SFX())
	End Sub

	' Token: 0x0600347F RID: 13439 RVA: 0x001E7B4E File Offset: 0x001E5F4E
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If phase = CollisionPhase.Enter AndAlso hit.GetComponent(Of HarbourPlatformingLevelCrab)() Then
			MyBase.StartCoroutine(Me.prepare_turn_cr(hit))
		End If
	End Sub

	' Token: 0x06003480 RID: 13440 RVA: 0x001E7B7C File Offset: 0x001E5F7C
	Protected Overrides Sub CalculateDirection()
	End Sub

	' Token: 0x06003481 RID: 13441 RVA: 0x001E7B80 File Offset: 0x001E5F80
	Private Iterator Function prepare_turn_cr(hit As GameObject) As IEnumerator
		Dim dist As Single = Vector3.Distance(hit.transform.position, MyBase.transform.position)
		While dist > 670F
			dist = Vector3.Distance(hit.transform.position, MyBase.transform.position)
			Yield Nothing
		End While
		Me.Turn()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003482 RID: 13442 RVA: 0x001E7BA4 File Offset: 0x001E5FA4
	Protected Overrides Function Turn() As Coroutine
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(1000F, 1000F)) Then
			AudioManager.Play("harbour_crab_turn")
			Me.emitAudioFromObject.Add("harbour_crab_turn")
		End If
		Me.walkingBack = Not Me.walkingBack
		MyBase.animator.SetBool("walkingBack", Me.walkingBack)
		MyBase.animator.SetBool("goingLeft", MyBase.direction <> PlatformingLevelGroundMovementEnemy.Direction.Right)
		Me.target = If((MyBase.direction <> PlatformingLevelGroundMovementEnemy.Direction.Right), "Turn_Right", "Turn_Left")
		Me.SetTurnTarget(Me.target)
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, AbstractPlatformingLevelEnemy.CAMERA_DEATH_PADDING) Then
			CupheadLevelCamera.Current.Shake(10F, 0.4F, False)
		End If
		Return MyBase.Turn()
	End Function

	' Token: 0x06003483 RID: 13443 RVA: 0x001E7CB0 File Offset: 0x001E60B0
	Private Iterator Function play_loop_SFX() As IEnumerator
		Dim playerLeft As Boolean = False
		While True
			If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(1000F, 1000F)) Then
				playerLeft = False
				If Not AudioManager.CheckIfPlaying("harbour_crab_walk") AndAlso Not AudioManager.CheckIfPlaying("harbour_crab_turn") Then
					AudioManager.PlayLoop("harbour_crab_walk")
					Me.emitAudioFromObject.Add("harbour_crab_walk")
				End If
			ElseIf Not playerLeft Then
				AudioManager.[Stop]("harbour_crab_walk")
				playerLeft = True
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003CA5 RID: 15525
	Private Const ON_SCREEN_SOUND_PADDING As Single = 1000F

	' Token: 0x04003CA6 RID: 15526
	Private target As String

	' Token: 0x04003CA7 RID: 15527
	Private walkingBack As Boolean
End Class
