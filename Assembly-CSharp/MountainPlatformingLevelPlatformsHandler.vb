Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008EB RID: 2283
Public Class MountainPlatformingLevelPlatformsHandler
	Inherits AbstractPausableComponent

	' Token: 0x0600358A RID: 13706 RVA: 0x001F30F8 File Offset: 0x001F14F8
	Private Sub Start()
		Me.platforms = New Transform(Me.platformHolder.GetComponentsInChildren(Of Transform)().Length - 1) {}
		Me.platforms = Me.platformHolder.GetComponentsInChildren(Of Transform)()
		Me.platformsStartPos = New Vector3(Me.platformHolder.GetComponentsInChildren(Of Transform)().Length - 1) {}
		For i As Integer = 0 To Me.platforms.Length - 1
			Me.platformsStartPos(i) = Me.platforms(i).position
		Next
		For j As Integer = 0 To Me.platforms.Length - 1
			Me.OffScreen(Me.platforms(j))
		Next
		AddHandler Me.parrySwitch.OnActivate, AddressOf Me.MovePlatforms
	End Sub

	' Token: 0x0600358B RID: 13707 RVA: 0x001F31BB File Offset: 0x001F15BB
	Private Sub MovePlatforms()
		If Not Me.hasSwitched Then
			MyBase.StartCoroutine(Me.moving_cr())
			Me.hasSwitched = True
		End If
	End Sub

	' Token: 0x0600358C RID: 13708 RVA: 0x001F31DC File Offset: 0x001F15DC
	Private Iterator Function moving_cr() As IEnumerator
		For i As Integer = 0 To Me.platforms.Length - 1
			MyBase.StartCoroutine(Me.move_platform_cr(Me.platforms(i), Me.platformsStartPos(i)))
			Yield CupheadTime.WaitForSeconds(Me, Me.platformAppearDelay)
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x0600358D RID: 13709 RVA: 0x001F31F7 File Offset: 0x001F15F7
	Private Sub OffScreen(platform As Transform)
		platform.transform.position += Vector3.down * Me.lowerAmount
	End Sub

	' Token: 0x0600358E RID: 13710 RVA: 0x001F3220 File Offset: 0x001F1620
	Private Iterator Function move_platform_cr(platform As Transform, startPos As Vector3) As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = Me.platformMoveTime
		Dim start As Vector2 = platform.transform.position
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, t / time)
			platform.transform.position = Vector2.Lerp(start, startPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		platform.transform.position = startPos
		Yield Nothing
		Return
	End Function

	' Token: 0x04003DA1 RID: 15777
	<SerializeField()>
	Private platformHolder As Transform

	' Token: 0x04003DA2 RID: 15778
	<SerializeField()>
	Private parrySwitch As ParrySwitch

	' Token: 0x04003DA3 RID: 15779
	<SerializeField()>
	Private platformMoveTime As Single

	' Token: 0x04003DA4 RID: 15780
	<SerializeField()>
	Private platformAppearDelay As Single

	' Token: 0x04003DA5 RID: 15781
	Private hasSwitched As Boolean

	' Token: 0x04003DA6 RID: 15782
	Private platforms As Transform()

	' Token: 0x04003DA7 RID: 15783
	Private platformsStartPos As Vector3()

	' Token: 0x04003DA8 RID: 15784
	Private lowerAmount As Single = 1000F
End Class
