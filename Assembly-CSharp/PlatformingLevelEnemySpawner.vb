Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200085D RID: 2141
Public MustInherit Class PlatformingLevelEnemySpawner
	Inherits AbstractPausableComponent

	' Token: 0x060031BE RID: 12734 RVA: 0x001D0FD8 File Offset: 0x001CF3D8
	Protected Overridable Sub Start()
		Me.started = False
		Me.ended = False
		Dim vector As Vector2 = MyBase.transform.position
		Me.startRect = RectUtils.NewFromCenter(Me.startTrigger.Position.x + vector.x, Me.startTrigger.Position.y + vector.y, Me.startTrigger.Size.x, Me.startTrigger.Size.y)
		Me.stopRect = RectUtils.NewFromCenter(Me.stopTrigger.Position.x + vector.x, Me.stopTrigger.Position.y + vector.y, Me.stopTrigger.Size.x, Me.stopTrigger.Size.y)
	End Sub

	' Token: 0x060031BF RID: 12735 RVA: 0x001D10BC File Offset: 0x001CF4BC
	Private Sub Update()
		If Me.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) OrElse (PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing AndAlso Me.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)) Then
			Me.OnStartTriggerHit()
		End If
		If Me.stopRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) OrElse (PlayerManager.GetPlayer(PlayerId.PlayerTwo) IsNot Nothing AndAlso Me.stopRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)) Then
			Me.OnStopTriggerHit()
		End If
	End Sub

	' Token: 0x060031C0 RID: 12736 RVA: 0x001D1163 File Offset: 0x001CF563
	Private Sub OnStartTriggerHit()
		If Me.started Then
			Return
		End If
		Me.started = True
		Me.StartSpawning()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x060031C1 RID: 12737 RVA: 0x001D118B File Offset: 0x001CF58B
	Private Sub OnStopTriggerHit()
		If Not Me.started Then
			Return
		End If
		If Me.ended Then
			Return
		End If
		Me.ended = True
		Me.EndSpawning()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x060031C2 RID: 12738 RVA: 0x001D11B8 File Offset: 0x001CF5B8
	Protected Overridable Sub EndSpawning()
	End Sub

	' Token: 0x060031C3 RID: 12739 RVA: 0x001D11BA File Offset: 0x001CF5BA
	Protected Overridable Sub StartSpawning()
	End Sub

	' Token: 0x060031C4 RID: 12740 RVA: 0x001D11BC File Offset: 0x001CF5BC
	Protected Overridable Sub Spawn()
	End Sub

	' Token: 0x060031C5 RID: 12741 RVA: 0x001D11C0 File Offset: 0x001CF5C0
	Private Iterator Function loop_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.initalSpawnDelay.RandomFloat())
		While True
			Me.Spawn()
			Yield CupheadTime.WaitForSeconds(Me, Me.spawnDelay.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x060031C6 RID: 12742 RVA: 0x001D11DB File Offset: 0x001CF5DB
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x060031C7 RID: 12743 RVA: 0x001D11EE File Offset: 0x001CF5EE
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x060031C8 RID: 12744 RVA: 0x001D1204 File Offset: 0x001CF604
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = New Color(0F, 1F, 0F, a)
		Gizmos.DrawWireCube(MyBase.baseTransform.position + Me.startTrigger.Position, Me.startTrigger.Size)
		Gizmos.color = New Color(1F, 0F, 0F, a)
		Gizmos.DrawWireCube(MyBase.baseTransform.position + Me.stopTrigger.Position, Me.stopTrigger.Size)
	End Sub

	' Token: 0x04003A1E RID: 14878
	Public destroyEnemyAfterLeavingScreen As Boolean = True

	' Token: 0x04003A1F RID: 14879
	<Header("Spawning Properties")>
	Public spawnDelay As MinMax = New MinMax(2F, 2F)

	' Token: 0x04003A20 RID: 14880
	Public initalSpawnDelay As MinMax = New MinMax(0F, 0F)

	' Token: 0x04003A21 RID: 14881
	<Header("Triggers")>
	Public startTrigger As PlatformingLevelEnemySpawner.TriggerProperties = New PlatformingLevelEnemySpawner.TriggerProperties(New Vector2(-200F, 0F))

	' Token: 0x04003A22 RID: 14882
	Public stopTrigger As PlatformingLevelEnemySpawner.TriggerProperties = New PlatformingLevelEnemySpawner.TriggerProperties(New Vector2(200F, 0F))

	' Token: 0x04003A23 RID: 14883
	Private started As Boolean

	' Token: 0x04003A24 RID: 14884
	Private ended As Boolean

	' Token: 0x04003A25 RID: 14885
	Private startRect As Rect

	' Token: 0x04003A26 RID: 14886
	Private stopRect As Rect

	' Token: 0x0200085E RID: 2142
	<Serializable()>
	Public Class TriggerProperties
		' Token: 0x060031C9 RID: 12745 RVA: 0x001D12AF File Offset: 0x001CF6AF
		Public Sub New(position As Vector2)
			Me.Position = position
		End Sub

		' Token: 0x04003A27 RID: 14887
		Public Position As Vector2 = Vector2.zero

		' Token: 0x04003A28 RID: 14888
		Public Size As Vector2 = Vector2.one * 100F
	End Class
End Class
