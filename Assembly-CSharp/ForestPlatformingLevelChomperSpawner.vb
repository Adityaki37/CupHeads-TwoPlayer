Imports System
Imports UnityEngine

' Token: 0x0200087D RID: 2173
Public Class ForestPlatformingLevelChomperSpawner
	Inherits AbstractPausableComponent

	' Token: 0x06003274 RID: 12916 RVA: 0x001D628C File Offset: 0x001D468C
	Private Sub Start()
		Me.started = False
		Dim vector As Vector2 = MyBase.transform.position
		Me.startRect = RectUtils.NewFromCenter(Me.startTrigger.Position.x + vector.x, Me.startTrigger.Position.y + vector.y, Me.startTrigger.Size.x + Global.UnityEngine.Random.Range(-Me.startTrigger.xVariation, Me.startTrigger.xVariation), Me.startTrigger.Size.y)
	End Sub

	' Token: 0x06003275 RID: 12917 RVA: 0x001D632C File Offset: 0x001D472C
	Private Sub Update()
		If Me.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) OrElse (PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing AndAlso Me.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)) Then
			Me.OnStartTriggerHit()
		End If
	End Sub

	' Token: 0x06003276 RID: 12918 RVA: 0x001D6388 File Offset: 0x001D4788
	Private Sub OnStartTriggerHit()
		If Me.started Then
			Return
		End If
		Me.started = True
		For Each forestPlatformingLevelChomper As ForestPlatformingLevelChomper In Me.chompers
			If forestPlatformingLevelChomper IsNot Nothing Then
				forestPlatformingLevelChomper.StartAttacking()
			End If
		Next
	End Sub

	' Token: 0x06003277 RID: 12919 RVA: 0x001D63D9 File Offset: 0x001D47D9
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003278 RID: 12920 RVA: 0x001D63EC File Offset: 0x001D47EC
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06003279 RID: 12921 RVA: 0x001D6400 File Offset: 0x001D4800
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = New Color(0F, 1F, 0F, a)
		Gizmos.DrawWireCube(MyBase.baseTransform.position + Me.startTrigger.Position, Me.startTrigger.Size)
	End Sub

	' Token: 0x04003AD7 RID: 15063
	<Header("Triggers")>
	Public startTrigger As ForestPlatformingLevelChomperSpawner.TriggerProperties = New ForestPlatformingLevelChomperSpawner.TriggerProperties(New Vector2(-200F, 0F))

	' Token: 0x04003AD8 RID: 15064
	<Header("Chompers")>
	Public chompers As ForestPlatformingLevelChomper()

	' Token: 0x04003AD9 RID: 15065
	Private started As Boolean

	' Token: 0x04003ADA RID: 15066
	Private startRect As Rect

	' Token: 0x0200087E RID: 2174
	<Serializable()>
	Public Class TriggerProperties
		' Token: 0x0600327A RID: 12922 RVA: 0x001D645C File Offset: 0x001D485C
		Public Sub New(position As Vector2)
			Me.Position = position
		End Sub

		' Token: 0x04003ADB RID: 15067
		Public Position As Vector2 = Vector2.zero

		' Token: 0x04003ADC RID: 15068
		Public Size As Vector2 = Vector2.one * 100F

		' Token: 0x04003ADD RID: 15069
		Public xVariation As Single
	End Class
End Class
