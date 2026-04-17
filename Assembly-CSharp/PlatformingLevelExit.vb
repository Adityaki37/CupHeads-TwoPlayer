Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000904 RID: 2308
Public Class PlatformingLevelExit
	Inherits AbstractCollidableObject

	' Token: 0x14000063 RID: 99
	' (add) Token: 0x0600361F RID: 13855 RVA: 0x001F7014 File Offset: 0x001F5414
	' (remove) Token: 0x06003620 RID: 13856 RVA: 0x001F7048 File Offset: 0x001F5448
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnWinStartEvent As Action

	' Token: 0x14000064 RID: 100
	' (add) Token: 0x06003621 RID: 13857 RVA: 0x001F707C File Offset: 0x001F547C
	' (remove) Token: 0x06003622 RID: 13858 RVA: 0x001F70B0 File Offset: 0x001F54B0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Shared Event OnWinCompleteEvent As Action

	' Token: 0x06003623 RID: 13859 RVA: 0x001F70E4 File Offset: 0x001F54E4
	Private Sub FixedUpdate()
		If Me._activated Then
			If Not Me._exited Then
				For i As Integer = 0 To 2 - 1
					Dim player As AbstractPlayerController = PlayerManager.GetPlayer(If((i <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne))
					If Not(player Is Nothing) Then
						If player.center.x > MyBase.transform.position.x + Me._exitDistance Then
							Me._exited = True
							If PlatformingLevelExit.OnWinCompleteEvent IsNot Nothing Then
								PlatformingLevelExit.OnWinCompleteEvent()
								PlatformingLevelExit.OnWinCompleteEvent = Nothing
							End If
							Exit For
						End If
					End If
				Next
			End If
		Else
			For j As Integer = 0 To 2 - 1
				Dim player2 As AbstractPlayerController = PlayerManager.GetPlayer(If((j <> 0), PlayerId.PlayerTwo, PlayerId.PlayerOne))
				If Not(player2 Is Nothing) AndAlso Not player2.IsDead Then
					If player2.center.x > MyBase.transform.position.x Then
						Me._activated = True
						If PlatformingLevelExit.OnWinStartEvent IsNot Nothing Then
							PlatformingLevelExit.OnWinStartEvent()
							PlatformingLevelExit.OnWinStartEvent = Nothing
						End If
						PlatformingLevelEnd.Win()
						MyBase.StartCoroutine(Me.on_win_complete_cr())
						Exit For
					End If
				End If
			Next
		End If
	End Sub

	' Token: 0x06003624 RID: 13860 RVA: 0x001F723F File Offset: 0x001F563F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		PlatformingLevelExit.OnWinStartEvent = Nothing
		PlatformingLevelExit.OnWinCompleteEvent = Nothing
	End Sub

	' Token: 0x06003625 RID: 13861 RVA: 0x001F7254 File Offset: 0x001F5654
	Private Iterator Function on_win_complete_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.onCompleteWaitTime)
		If PlatformingLevelExit.OnWinCompleteEvent IsNot Nothing Then
			PlatformingLevelExit.OnWinCompleteEvent()
			PlatformingLevelExit.OnWinCompleteEvent = Nothing
		End If
		Return
	End Function

	' Token: 0x06003626 RID: 13862 RVA: 0x001F726F File Offset: 0x001F566F
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.5F)
	End Sub

	' Token: 0x06003627 RID: 13863 RVA: 0x001F7282 File Offset: 0x001F5682
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06003628 RID: 13864 RVA: 0x001F7298 File Offset: 0x001F5698
	Private Sub DrawGizmos(a As Single)
		Gizmos.color = New Color(0F, 1F, 0F, a)
		Dim vector As Vector3 = MyBase.baseTransform.position + New Vector3(Me._exitDistance, 0F, 0F)
		Gizmos.DrawLine(MyBase.baseTransform.position, vector)
		Gizmos.DrawLine(vector + New Vector3(0F, -5000F, 0F), vector + New Vector3(0F, 5000F, 0F))
		Gizmos.color = Color.white
	End Sub

	' Token: 0x04003E27 RID: 15911
	<SerializeField()>
	<Range(200F, 1500F)>
	Private _exitDistance As Single = 500F

	' Token: 0x04003E28 RID: 15912
	<SerializeField()>
	Private onCompleteWaitTime As Single = 2F

	' Token: 0x04003E29 RID: 15913
	Private _activated As Boolean

	' Token: 0x04003E2A RID: 15914
	Private _exited As Boolean
End Class
