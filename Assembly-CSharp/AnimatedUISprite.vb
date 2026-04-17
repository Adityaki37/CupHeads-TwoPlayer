Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000B0D RID: 2829
Public Class AnimatedUISprite
	Inherits MonoBehaviour

	' Token: 0x060044A7 RID: 17575 RVA: 0x00246165 File Offset: 0x00244565
	Public Sub ResetAnimation()
		Me._currentSpriteIndex = 0
	End Sub

	' Token: 0x060044A8 RID: 17576 RVA: 0x0024616E File Offset: 0x0024456E
	Private Sub OnEnable()
		Me.ResetAnimation()
	End Sub

	' Token: 0x060044A9 RID: 17577 RVA: 0x00246176 File Offset: 0x00244576
	Private Sub OnDisable()
		Me.ResetAnimation()
	End Sub

	' Token: 0x060044AA RID: 17578 RVA: 0x00246180 File Offset: 0x00244580
	Private Sub Update()
		If Not Me.[Loop] AndAlso Me._currentSpriteIndex >= Me.Sprites.Length - 1 Then
			Return
		End If
		If Me.Animating AndAlso Time.time >= Me._lastRefreshTime + 1F / CSng(Me.FrameRate) Then
			Me._currentSpriteIndex += 1
			If Me._currentSpriteIndex >= Me.Sprites.Length Then
				Me._currentSpriteIndex = 0
			End If
			Me.UIImage.sprite = Me.Sprites(Me._currentSpriteIndex)
			Me._lastRefreshTime = Time.time
		End If
	End Sub

	' Token: 0x04004A60 RID: 19040
	Public Animating As Boolean

	' Token: 0x04004A61 RID: 19041
	Public [Loop] As Boolean

	' Token: 0x04004A62 RID: 19042
	Public UIImage As Image

	' Token: 0x04004A63 RID: 19043
	Public Sprites As Sprite()

	' Token: 0x04004A64 RID: 19044
	Public FrameRate As Integer = 24

	' Token: 0x04004A65 RID: 19045
	Private _currentSpriteIndex As Integer

	' Token: 0x04004A66 RID: 19046
	Private _lastRefreshTime As Single
End Class
