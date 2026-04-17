Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008EF RID: 2287
Public Class MountainPlatformingLevelScale
	Inherits AbstractPausableComponent

	' Token: 0x060035A4 RID: 13732 RVA: 0x001F4074 File Offset: 0x001F2474
	Private Sub Start()
		Me.scaleLeftStart = Me.ScaleLeft.transform.position
		Me.scaleRightStart = Me.ScaleRight.transform.position
		MyBase.StartCoroutine(Me.check_scale_cr())
	End Sub

	' Token: 0x060035A5 RID: 13733 RVA: 0x001F40C4 File Offset: 0x001F24C4
	Private Iterator Function check_scale_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If Me.ScaleRight.steppedOn Then
				If Not Me.ScaleLeft.steppedOn Then
					If Me.ScaleRight.transform.position.y > Me.scaleRightStart.y - Me.scaleChangeAmount Then
						Me.ScaleRight.transform.AddPosition(0F, -Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
						Me.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown)
					End If
					If Me.ScaleLeft.transform.position.y < Me.scaleLeftStart.y + Me.scaleChangeAmount Then
						Me.ScaleLeft.transform.AddPosition(0F, Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
					End If
				ElseIf PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
					If Me.ScaleRight.transform.position.y < Me.scaleRightStart.y Then
						Me.ScaleRight.transform.AddPosition(0F, Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
						Me.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown)
					ElseIf Me.ScaleLeft.steppedOn Then
						Me.ChangeState(MountainPlatformingLevelScale.ScaleState.still)
					End If
					If Me.ScaleLeft.transform.position.y > Me.scaleLeftStart.y Then
						Me.ScaleLeft.transform.AddPosition(0F, -Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
					End If
				End If
			Else
				If Me.ScaleRight.transform.position.y < Me.scaleRightStart.y Then
					Me.ScaleRight.transform.AddPosition(0F, Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
					Me.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown)
				ElseIf Not Me.ScaleLeft.steppedOn Then
					Me.ChangeState(MountainPlatformingLevelScale.ScaleState.still)
				End If
				If Me.ScaleLeft.transform.position.y > Me.scaleLeftStart.y Then
					Me.ScaleLeft.transform.AddPosition(0F, -Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
				End If
			End If
			If Me.ScaleLeft.steppedOn Then
				If Not Me.ScaleRight.steppedOn Then
					If Me.ScaleLeft.transform.position.y > Me.scaleLeftStart.y - Me.scaleChangeAmount Then
						Me.ScaleLeft.transform.AddPosition(0F, -Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
						Me.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown)
					End If
					If Me.ScaleRight.transform.position.y < Me.scaleRightStart.y + Me.scaleChangeAmount Then
						Me.ScaleRight.transform.AddPosition(0F, Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
					End If
				ElseIf PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing Then
					If Me.ScaleLeft.transform.position.y < Me.scaleLeftStart.y Then
						Me.ScaleLeft.transform.AddPosition(0F, Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
						Me.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown)
					ElseIf Me.ScaleRight.steppedOn Then
						Me.ChangeState(MountainPlatformingLevelScale.ScaleState.still)
					End If
					If Me.ScaleRight.transform.position.y > Me.scaleRightStart.y Then
						Me.ScaleRight.transform.AddPosition(0F, -Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
					End If
				End If
			Else
				If Me.ScaleLeft.transform.position.y < Me.scaleLeftStart.y Then
					Me.ScaleLeft.transform.AddPosition(0F, Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
					Me.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown)
				ElseIf Not Me.ScaleRight.steppedOn Then
					Me.ChangeState(MountainPlatformingLevelScale.ScaleState.still)
				End If
				If Me.ScaleRight.transform.position.y > Me.scaleRightStart.y Then
					Me.ScaleRight.transform.AddPosition(0F, -Me.scaleSpeed * CupheadTime.FixedDelta, 0F)
				End If
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060035A6 RID: 13734 RVA: 0x001F40E0 File Offset: 0x001F24E0
	Private Sub ChangeState(state As MountainPlatformingLevelScale.ScaleState)
		If Me.scaleState <> state Then
			Me.scaleState = state
			Dim text As String = "goingDown"
			Dim text2 As String = "goingUp"
			Dim scaleState As MountainPlatformingLevelScale.ScaleState = Me.scaleState
			If scaleState <> MountainPlatformingLevelScale.ScaleState.leftDown Then
				If scaleState <> MountainPlatformingLevelScale.ScaleState.rightDown Then
					If scaleState = MountainPlatformingLevelScale.ScaleState.still Then
						Me.ScaleLeft.animator.SetBool(text, False)
						Me.ScaleLeft.animator.SetBool(text2, False)
						Me.ScaleRight.animator.SetBool(text, False)
						Me.ScaleRight.animator.SetBool(text2, False)
						If Me.ScaleLeft.animator.GetCurrentAnimatorStateInfo(0).IsName("Scale_Idle") Then
							AudioManager.[Stop]("castle_scales_tip_up")
							AudioManager.[Stop]("castle_scales_tip_down")
						End If
					End If
				Else
					Me.ScaleLeft.animator.SetBool(text, False)
					Me.ScaleLeft.animator.SetBool(text2, True)
					Me.ScaleRight.animator.SetBool(text, True)
					Me.ScaleRight.animator.SetBool(text2, False)
					Me.ScalesUpSound()
					Me.ScalesDownSound()
				End If
			Else
				Me.ScaleLeft.animator.SetBool(text, True)
				Me.ScaleLeft.animator.SetBool(text2, False)
				Me.ScaleRight.animator.SetBool(text, False)
				Me.ScaleRight.animator.SetBool(text2, True)
				Me.ScalesUpSound()
				Me.ScalesDownSound()
			End If
		End If
	End Sub

	' Token: 0x060035A7 RID: 13735 RVA: 0x001F4262 File Offset: 0x001F2662
	Private Sub ScalesUpSound()
		If Not AudioManager.CheckIfPlaying("castle_scales_tip_up") Then
			AudioManager.Play("castle_scales_tip_up")
			Me.emitAudioFromObject.Add("castle_scales_tip_up")
		End If
	End Sub

	' Token: 0x060035A8 RID: 13736 RVA: 0x001F428D File Offset: 0x001F268D
	Private Sub ScalesDownSound()
		If Not AudioManager.CheckIfPlaying("castle_scales_tip_down") Then
			AudioManager.Play("castle_scales_tip_down")
			Me.emitAudioFromObject.Add("castle_scales_tip_down")
		End If
	End Sub

	' Token: 0x04003DB8 RID: 15800
	<SerializeField()>
	Private scaleSpeed As Single

	' Token: 0x04003DB9 RID: 15801
	<SerializeField()>
	Private scaleChangeAmount As Single

	' Token: 0x04003DBA RID: 15802
	<SerializeField()>
	Private ScaleLeft As MountainPlatformingLevelScalePart

	' Token: 0x04003DBB RID: 15803
	<SerializeField()>
	Private ScaleRight As MountainPlatformingLevelScalePart

	' Token: 0x04003DBC RID: 15804
	Private scaleLeftStart As Vector2

	' Token: 0x04003DBD RID: 15805
	Private scaleRightStart As Vector2

	' Token: 0x04003DBE RID: 15806
	Public scaleState As MountainPlatformingLevelScale.ScaleState

	' Token: 0x020008F0 RID: 2288
	Public Enum ScaleState
		' Token: 0x04003DC0 RID: 15808
		rightDown
		' Token: 0x04003DC1 RID: 15809
		leftDown
		' Token: 0x04003DC2 RID: 15810
		still
	End Enum
End Class
