Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008E1 RID: 2273
Public Class MountainPlatformingLevelElevatorHandler
	Inherits AbstractPausableComponent

	' Token: 0x17000450 RID: 1104
	' (get) Token: 0x06003532 RID: 13618 RVA: 0x001EF50F File Offset: 0x001ED90F
	' (set) Token: 0x06003533 RID: 13619 RVA: 0x001EF516 File Offset: 0x001ED916
	Public Shared Property elevatorIsMoving As Boolean

	' Token: 0x06003534 RID: 13620 RVA: 0x001EF520 File Offset: 0x001ED920
	Private Sub Start()
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = False
		MyBase.StartCoroutine(Me.wait_cr())
		Me.cameraLockRoutine = MyBase.StartCoroutine(Me.lock_camera_cr())
		Me.invisibleWall.SetActive(False)
		For Each platformingLevelParallax As PlatformingLevelParallax In Me.bottomBackground.GetComponentsInChildren(Of PlatformingLevelParallax)()
			platformingLevelParallax.enabled = False
		Next
	End Sub

	' Token: 0x06003535 RID: 13621 RVA: 0x001EF58C File Offset: 0x001ED98C
	Private Iterator Function lock_camera_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		While True
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			CupheadLevelCamera.Current.LockCamera(CupheadLevelCamera.Current.transform.position.x > Me.triggerPoint.transform.position.x)
			If player2 IsNot Nothing Then
				If Not player2.IsDead AndAlso Not player.IsDead Then
					If player.transform.position.x < Me.triggerPoint.transform.position.x AndAlso player2.transform.position.x < Me.triggerPoint.transform.position.x Then
						CupheadLevelCamera.Current.LockCamera(False)
					End If
				ElseIf player2.IsDead Then
					If player.transform.position.x < Me.triggerPoint.transform.position.x Then
						CupheadLevelCamera.Current.LockCamera(False)
					End If
				ElseIf player.IsDead AndAlso player2.transform.position.x < Me.triggerPoint.transform.position.x Then
					CupheadLevelCamera.Current.LockCamera(False)
				End If
			ElseIf player.transform.position.x < Me.triggerPoint.transform.position.x Then
				CupheadLevelCamera.Current.LockCamera(False)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003536 RID: 13622 RVA: 0x001EF5A8 File Offset: 0x001ED9A8
	Private Iterator Function wait_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		While True
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			If player2 IsNot Nothing Then
				If Not player2.IsDead AndAlso Not player.IsDead Then
					If player.transform.position.x > Me.triggerPoint.transform.position.x AndAlso player2.transform.position.x > Me.triggerPoint2.transform.position.x Then
						Exit For
					End If
					If player.transform.position.x > Me.triggerPoint2.transform.position.x AndAlso player2.transform.position.x > Me.triggerPoint.transform.position.x Then
						Exit For
					End If
				ElseIf player2.IsDead Then
					If player.transform.position.x > Me.triggerPoint.transform.position.x Then
						Exit For
					End If
				ElseIf player.IsDead AndAlso player2.transform.position.x > Me.triggerPoint.transform.position.x Then
					Exit For
				End If
			ElseIf player.transform.position.x > Me.triggerPoint.transform.position.x Then
				Exit For
			End If
			Yield Nothing
		End While
		MyBase.StopCoroutine(Me.cameraLockRoutine)
		CupheadLevelCamera.Current.LockCamera(True)
		Me.invisibleWall.SetActive(True)
		AudioManager.Play("castle_lift_start")
		CupheadLevelCamera.Current.Shake(10F, 1F, False)
		Yield CupheadTime.WaitForSeconds(Me, 0.9F)
		MyBase.StartCoroutine(Me.move_cr())
		Return
	End Function

	' Token: 0x06003537 RID: 13623 RVA: 0x001EF5C4 File Offset: 0x001ED9C4
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim t As Single = 0F
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = True
		Dim startPos As Vector3 = Me.scrollingObject.transform.position
		Dim pos As Vector3 = Me.scrollingObject.transform.position
		Dim dir As Vector3 = Me.pointA.transform.position - Me.scrollingObject.transform.position
		Dim middle As Vector3 = Me.scrollingObject.transform.position + dir.normalized * (Vector3.Distance(Me.scrollingObject.transform.position, Me.pointA.transform.position) / 2F)
		For Each scrollingBackgroundElevator As ScrollingBackgroundElevator In Me.midgroundSprites
			scrollingBackgroundElevator.SetUp(MathUtils.AngleToDirection(-45F), Me.speed)
		Next
		Me.backgroundSprite.SetUp(MathUtils.AngleToDirection(-45F), Me.speed - Me.speed / 4F)
		Me.foregroundSprite.SetUp(MathUtils.AngleToDirection(-45F), Me.speed + Me.speed / 4F)
		Me.cloudSprite.SetUp(MathUtils.AngleToDirection(-45F), Me.speed + Me.speed / 4F)
		For Each platformingLevelParallax As PlatformingLevelParallax In Me.topBackground.GetComponentsInChildren(Of PlatformingLevelParallax)()
			platformingLevelParallax.enabled = False
		Next
		Me.bottomBackground.parent = Me.scrollingObject.transform
		Me.mudmanSpawner.SpawnMudmen()
		AudioManager.PlayLoop("castle_lift_loop")
		While Me.scrollingObject.transform.position.y < middle.y
			pos -= MathUtils.AngleToDirection(-45F) * Me.speed * CupheadTime.FixedDelta
			Me.scrollingObject.transform.position = pos
			Yield wait
		End While
		While t < Me.time
			t += CupheadTime.FixedDelta
			Yield Nothing
		End While
		Dim midPos As Vector3 = Me.scrollingObject.transform.position
		Dim endTime As Single = Vector3.Distance(startPos, Me.pointA.position) / Me.speed
		t = 0F
		For Each scrollingBackgroundElevator2 As ScrollingBackgroundElevator In Me.midgroundSprites
			scrollingBackgroundElevator2.EaseoutSpeed(endTime)
		Next
		Me.cloudSprite.EaseoutSpeed(endTime)
		Me.foregroundSprite.EaseoutSpeed(endTime)
		Me.backgroundSprite.EaseoutSpeed(endTime)
		MyBase.StartCoroutine(Me.easeTime(endTime))
		Me.cloudSprite.ending = True
		While Me.easeingTime < endTime
			pos -= MathUtils.AngleToDirection(-45F) * Me.speed * CupheadTime.FixedDelta
			Me.scrollingObject.transform.position = pos
			Yield wait
		End While
		AudioManager.[Stop]("castle_lift_loop")
		AudioManager.Play("castle_lift_end")
		CupheadLevelCamera.Current.Shake(10F, 0.5F, False)
		For Each platformingLevelParallax2 As PlatformingLevelParallax In Me.bottomBackground.GetComponentsInChildren(Of PlatformingLevelParallax)()
			platformingLevelParallax2.enabled = True
			platformingLevelParallax2.UpdateBasePosition()
		Next
		For Each scrollingBackgroundElevator3 As ScrollingBackgroundElevator In Me.midgroundSprites
			scrollingBackgroundElevator3.ending = True
		Next
		Me.backgroundSprite.ending = True
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = False
		If PlayerManager.GetFirst().transform.position.x < CupheadLevelCamera.Current.transform.position.x Then
			CupheadLevelCamera.Current.OffsetCamera(True, True)
		Else
			CupheadLevelCamera.Current.OffsetCamera(True, False)
		End If
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		CupheadLevelCamera.Current.OffsetCamera(False, False)
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		CupheadLevelCamera.Current.LockCamera(False)
		Yield Nothing
		Return
	End Function

	' Token: 0x06003538 RID: 13624 RVA: 0x001EF5E0 File Offset: 0x001ED9E0
	Private Iterator Function easeTime(time As Single) As IEnumerator
		Dim startSpeed As Single = Me.speed
		Me.easeingTime = 0F
		While Me.easeingTime < time
			Me.easeingTime += CupheadTime.Delta
			Me.speed = Mathf.Lerp(startSpeed, 0F, Me.easeingTime / time)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003539 RID: 13625 RVA: 0x001EF604 File Offset: 0x001EDA04
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = New Color(0F, 0F, 1F, 1F)
		Gizmos.DrawLine(Me.triggerPoint.transform.position, New Vector3(Me.triggerPoint.transform.position.x, 5000F, 0F))
		Gizmos.DrawLine(Me.triggerPoint2.transform.position, New Vector3(Me.triggerPoint2.transform.position.x, 5000F, 0F))
		Gizmos.color = New Color(0F, 1F, 0F, 1F)
		Gizmos.DrawWireSphere(Me.pointA.transform.position, 100F)
		Gizmos.DrawLine(Me.pointA.transform.position, Me.scrollingObject.transform.position)
	End Sub

	' Token: 0x04003D5B RID: 15707
	<SerializeField()>
	Private mudmanSpawner As MountainPlatformingLevelMudmanSpawner

	' Token: 0x04003D5C RID: 15708
	<SerializeField()>
	Private topBackground As Transform

	' Token: 0x04003D5D RID: 15709
	<SerializeField()>
	Private bottomBackground As Transform

	' Token: 0x04003D5E RID: 15710
	<SerializeField()>
	Private speed As Single

	' Token: 0x04003D5F RID: 15711
	<SerializeField()>
	Private scrollingObject As GameObject

	' Token: 0x04003D60 RID: 15712
	<SerializeField()>
	Private triggerPoint As Transform

	' Token: 0x04003D61 RID: 15713
	<SerializeField()>
	Private triggerPoint2 As Transform

	' Token: 0x04003D62 RID: 15714
	<SerializeField()>
	Private pointA As Transform

	' Token: 0x04003D63 RID: 15715
	<SerializeField()>
	Private invisibleWall As GameObject

	' Token: 0x04003D64 RID: 15716
	<SerializeField()>
	Private cloudSprite As ScrollingBackgroundElevator

	' Token: 0x04003D65 RID: 15717
	<SerializeField()>
	Private foregroundSprite As ScrollingBackgroundElevator

	' Token: 0x04003D66 RID: 15718
	<SerializeField()>
	Private backgroundSprite As ScrollingBackgroundElevator

	' Token: 0x04003D67 RID: 15719
	<SerializeField()>
	Private midgroundSprites As ScrollingBackgroundElevator()

	' Token: 0x04003D68 RID: 15720
	<SerializeField()>
	Private time As Single

	' Token: 0x04003D69 RID: 15721
	Private easeingTime As Single

	' Token: 0x04003D6A RID: 15722
	Private cameraLockRoutine As Coroutine
End Class
