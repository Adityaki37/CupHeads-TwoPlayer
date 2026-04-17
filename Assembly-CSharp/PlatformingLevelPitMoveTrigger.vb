Imports System
Imports UnityEngine

' Token: 0x0200086D RID: 2157
Public Class PlatformingLevelPitMoveTrigger
	Inherits AbstractPausableComponent

	' Token: 0x0600322C RID: 12844 RVA: 0x001D47B0 File Offset: 0x001D2BB0
	Private Sub Start()
		Dim vector As Vector2 = MyBase.transform.position
		Me.rect = RectUtils.NewFromCenter(Me.trigger.Position.x + vector.x, Me.trigger.Position.y + vector.y, Me.trigger.Size.x, Me.trigger.Size.y)
	End Sub

	' Token: 0x0600322D RID: 12845 RVA: 0x001D482C File Offset: 0x001D2C2C
	Private Sub Update()
		If Me.rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) OrElse (PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing AndAlso Me.rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)) Then
			Me.OnTriggerHit()
		End If
	End Sub

	' Token: 0x0600322E RID: 12846 RVA: 0x001D4886 File Offset: 0x001D2C86
	Private Sub OnTriggerHit()
		LevelPit.Instance.ExtraOffset = Me.pitOffset
	End Sub

	' Token: 0x0600322F RID: 12847 RVA: 0x001D4898 File Offset: 0x001D2C98
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003230 RID: 12848 RVA: 0x001D48AB File Offset: 0x001D2CAB
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06003231 RID: 12849 RVA: 0x001D48C0 File Offset: 0x001D2CC0
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = New Color(0F, 1F, 0F, a)
		Gizmos.DrawWireCube(MyBase.baseTransform.position + Me.trigger.Position, Me.trigger.Size)
	End Sub

	' Token: 0x04003A85 RID: 14981
	<SerializeField()>
	Private pitOffset As Single

	' Token: 0x04003A86 RID: 14982
	<Header("Triggers")>
	Public trigger As PlatformingLevelPitMoveTrigger.TriggerProperties = New PlatformingLevelPitMoveTrigger.TriggerProperties(New Vector2(-200F, 0F))

	' Token: 0x04003A87 RID: 14983
	Private rect As Rect

	' Token: 0x0200086E RID: 2158
	<Serializable()>
	Public Class TriggerProperties
		' Token: 0x06003232 RID: 12850 RVA: 0x001D491C File Offset: 0x001D2D1C
		Public Sub New(position As Vector2)
			Me.Position = position
		End Sub

		' Token: 0x04003A88 RID: 14984
		Public Position As Vector2 = Vector2.zero

		' Token: 0x04003A89 RID: 14985
		Public Size As Vector2 = Vector2.one * 100F
	End Class
End Class
