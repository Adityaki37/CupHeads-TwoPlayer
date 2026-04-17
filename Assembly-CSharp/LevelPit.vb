Imports System
Imports UnityEngine

' Token: 0x020004A8 RID: 1192
Public Class LevelPit
	Inherits AbstractCollidableObject

	' Token: 0x1700030B RID: 779
	' (get) Token: 0x0600136F RID: 4975 RVA: 0x000AB643 File Offset: 0x000A9A43
	' (set) Token: 0x06001370 RID: 4976 RVA: 0x000AB64A File Offset: 0x000A9A4A
	Public Shared Property Instance As LevelPit

	' Token: 0x1700030C RID: 780
	' (get) Token: 0x06001371 RID: 4977 RVA: 0x000AB652 File Offset: 0x000A9A52
	' (set) Token: 0x06001372 RID: 4978 RVA: 0x000AB65A File Offset: 0x000A9A5A
	Public Property ExtraOffset As Single
		Get
			Return Me.extraOffset
		End Get
		Set(value As Single)
			Me.extraOffset = value
		End Set
	End Property

	' Token: 0x06001373 RID: 4979 RVA: 0x000AB664 File Offset: 0x000A9A64
	Private Sub Start()
		If Level.Current.LevelType = Level.Type.Platforming Then
			LevelPit.Instance = Me
			MyBase.transform.SetParent(CupheadLevelCamera.Current.transform)
			MyBase.transform.ResetLocalTransforms()
			MyBase.transform.SetLocalPosition(New Single?(0F), New Single?(-500F), New Single?(0F))
		End If
	End Sub

	' Token: 0x06001374 RID: 4980 RVA: 0x000AB6D0 File Offset: 0x000A9AD0
	Private Sub FixedUpdate()
		Me.CheckPlayer(TryCast(PlayerManager.GetPlayer(PlayerId.PlayerOne), LevelPlayerController))
		Me.CheckPlayer(TryCast(PlayerManager.GetPlayer(PlayerId.PlayerTwo), LevelPlayerController))
	End Sub

	' Token: 0x06001375 RID: 4981 RVA: 0x000AB6F4 File Offset: 0x000A9AF4
	Private Sub CheckPlayer(player As LevelPlayerController)
		If player Is Nothing OrElse player.IsDead Then
			Return
		End If
		Dim num As Single = 1F
		If Level.Current.LevelType = Level.Type.Platforming Then
			num *= 1.3F
		End If
		num *= Me.forceMultiplier
		If player.motor.GravityReversed AndAlso Level.Current.LevelType = Level.Type.Platforming Then
			Dim num2 As Single = MyBase.transform.parent.position.y - MyBase.transform.localPosition.y
			If player.transform.position.y >= num2 - Me.extraOffset Then
				player.OnPitKnockUp(num2 - Me.extraOffset, num)
			End If
		ElseIf player.transform.position.y <= MyBase.transform.position.y + Me.extraOffset Then
			player.OnPitKnockUp(MyBase.transform.position.y + Me.extraOffset, num)
		End If
	End Sub

	' Token: 0x06001376 RID: 4982 RVA: 0x000AB818 File Offset: 0x000A9C18
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.3F)
	End Sub

	' Token: 0x06001377 RID: 4983 RVA: 0x000AB82B File Offset: 0x000A9C2B
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06001378 RID: 4984 RVA: 0x000AB840 File Offset: 0x000A9C40
	Private Sub DrawGizmos(a As Single)
		Dim rect As Rect = New Rect(MyBase.baseTransform.position.x + -1000F, MyBase.baseTransform.position.y, 2000F, 0F)
		Gizmos.color = New Color(1F, 0F, 0F, a)
		Gizmos.DrawLine(New Vector2(rect.xMin, rect.y), New Vector2(rect.xMax, rect.y))
		For i As Integer = 0 To 20 - 1
			Dim num As Single = 100F
			Dim rect2 As Rect = New Rect(rect.xMin + num * CSng(i), rect.y, num, -20F)
			Gizmos.DrawLine(New Vector2(rect2.xMin, rect2.y), New Vector2(rect2.center.x, rect2.yMax))
			Gizmos.DrawLine(New Vector2(rect2.xMax, rect2.y), New Vector2(rect2.center.x, rect2.yMax))
		Next
	End Sub

	' Token: 0x04001C84 RID: 7300
	Private Const PLATFORMING_LEVEL_CAMERA_OFFSET_Y As Single = -500F

	' Token: 0x04001C85 RID: 7301
	<SerializeField()>
	Private extraOffset As Single

	' Token: 0x04001C86 RID: 7302
	<SerializeField()>
	Private forceMultiplier As Single = 1F
End Class
