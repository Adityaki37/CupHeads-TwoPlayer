Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000906 RID: 2310
Public Class PlatformingLevelMovingPlatform
	Inherits AbstractPausableComponent

	' Token: 0x17000464 RID: 1124
	' (get) Token: 0x06003630 RID: 13872 RVA: 0x001F74A4 File Offset: 0x001F58A4
	' (set) Token: 0x06003631 RID: 13873 RVA: 0x001F74AC File Offset: 0x001F58AC
	Protected Private Property allValues As Single()

	' Token: 0x06003632 RID: 13874 RVA: 0x001F74B8 File Offset: 0x001F58B8
	Protected Overridable Sub Start()
		Me._offset = MyBase.transform.position
		MyBase.StartCoroutine(Me.pingpong_cr())
		AudioManager.PlayLoop("level_platform_propellor_loop")
		Me.emitAudioFromObject.Add("level_platform_propellor_loop")
		MyBase.StartCoroutine(Me.check_sound_cr())
	End Sub

	' Token: 0x06003633 RID: 13875 RVA: 0x001F750C File Offset: 0x001F590C
	Private Iterator Function check_sound_cr() As IEnumerator
		Dim inRange As Boolean = False
		While True
			If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 1000F)) Then
				If Not inRange Then
					AudioManager.PlayLoop("level_platform_propellor_loop")
					Me.emitAudioFromObject.Add("level_platform_propellor_loop")
					inRange = True
				End If
			ElseIf inRange Then
				AudioManager.[Stop]("level_platform_propellor_loop")
				inRange = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003634 RID: 13876 RVA: 0x001F7528 File Offset: 0x001F5928
	Private Function CalculateTime() As Single
		Return Me.path.Distance / Me.speed
	End Function

	' Token: 0x06003635 RID: 13877 RVA: 0x001F754C File Offset: 0x001F594C
	Private Function CalculateRemainingTime(t As Single) As Single
		Dim num As Single = Me.CalculateTime()
		Return If((Not Me.goingUp), (t * num), ((1F - t) * num))
	End Function

	' Token: 0x06003636 RID: 13878 RVA: 0x001F757C File Offset: 0x001F597C
	Private Sub MoveCallback(value As Single)
		MyBase.transform.position = Me._offset + Me.path.Lerp(value)
	End Sub

	' Token: 0x06003637 RID: 13879 RVA: 0x001F75A0 File Offset: 0x001F59A0
	Protected Overridable Iterator Function pingpong_cr() As IEnumerator
		While True
			If Me.goingUp Then
				Yield MyBase.TweenValue(0F, 1F, Me.CalculateTime(), Me._easeType, AddressOf Me.MoveCallback)
			Else
				Yield MyBase.TweenValue(1F, 0F, Me.CalculateTime(), Me._easeType, AddressOf Me.MoveCallback)
			End If
			Yield CupheadTime.WaitForSeconds(Me, Me.loopRepeatDelay)
			Me.goingUp = Not Me.goingUp
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003638 RID: 13880 RVA: 0x001F75BB File Offset: 0x001F59BB
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003639 RID: 13881 RVA: 0x001F75CE File Offset: 0x001F59CE
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x0600363A RID: 13882 RVA: 0x001F75E4 File Offset: 0x001F59E4
	Private Sub DrawGizmos(a As Single)
		If Application.isPlaying Then
			Me.path.DrawGizmos(a, Me._offset)
			Return
		End If
		Me.path.DrawGizmos(a, MyBase.baseTransform.position)
		Gizmos.color = New Color(1F, 0F, 0F, a)
		Gizmos.DrawSphere(Me.path.Lerp(0F) + MyBase.baseTransform.position, 10F)
		Gizmos.DrawWireSphere(Me.path.Lerp(0F) + MyBase.baseTransform.position, 11F)
	End Sub

	' Token: 0x04003E2C RID: 15916
	Private Const ON_SCREEN_SOUND_PADDING As Single = 100F

	' Token: 0x04003E2E RID: 15918
	Protected pathIndex As Integer

	' Token: 0x04003E2F RID: 15919
	Public loopRepeatDelay As Single

	' Token: 0x04003E30 RID: 15920
	Public speed As Single = 100F

	' Token: 0x04003E31 RID: 15921
	Public path As VectorPath

	' Token: 0x04003E32 RID: 15922
	Public goingUp As Boolean

	' Token: 0x04003E33 RID: 15923
	Public sprite As SpriteRenderer

	' Token: 0x04003E34 RID: 15924
	Private _easeType As EaseUtils.EaseType = EaseUtils.EaseType.linear

	' Token: 0x04003E35 RID: 15925
	Protected _offset As Vector3
End Class
