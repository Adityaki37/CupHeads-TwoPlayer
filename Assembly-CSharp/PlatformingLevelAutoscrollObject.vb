Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008FD RID: 2301
Public Class PlatformingLevelAutoscrollObject
	Inherits AbstractCollidableObject

	' Token: 0x17000461 RID: 1121
	' (get) Token: 0x060035FA RID: 13818 RVA: 0x001EA4C1 File Offset: 0x001E88C1
	' (set) Token: 0x060035FB RID: 13819 RVA: 0x001EA4C9 File Offset: 0x001E88C9
	Protected Private Property isMoving As Boolean

	' Token: 0x060035FC RID: 13820 RVA: 0x001EA4D2 File Offset: 0x001E88D2
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.isMoving = False
		Me.isLocked = False
	End Sub

	' Token: 0x060035FD RID: 13821 RVA: 0x001EA4E8 File Offset: 0x001E88E8
	Protected Overridable Sub Start()
		If Me.checkToLock Then
			MyBase.StartCoroutine(Me.check_to_lock_cr())
		End If
	End Sub

	' Token: 0x060035FE RID: 13822 RVA: 0x001EA504 File Offset: 0x001E8904
	Protected Overridable Sub Update()
		If Me.isMoving AndAlso MyBase.transform.position.x > Me.endPosition.transform.position.x Then
			Me.StartEndingAutoscroll()
			Me.isMoving = False
		End If
	End Sub

	' Token: 0x060035FF RID: 13823 RVA: 0x001EA55C File Offset: 0x001E895C
	Protected Overridable Iterator Function check_to_lock_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Dim dist As Single = PlayerManager.Center.x - MyBase.transform.position.x
		While dist < -Me.lockDistance
			dist = PlayerManager.Center.x - MyBase.transform.position.x
			Yield Nothing
		End While
		CupheadLevelCamera.Current.LockCamera(True)
		Me.isLocked = True
		Yield Nothing
		Return
	End Function

	' Token: 0x06003600 RID: 13824 RVA: 0x001EA577 File Offset: 0x001E8977
	Protected Overridable Sub StartAutoscroll()
		Me.isMoving = True
		Me.isLocked = False
		CupheadLevelCamera.Current.LockCamera(False)
		CupheadLevelCamera.Current.SetAutoScroll(True)
	End Sub

	' Token: 0x06003601 RID: 13825 RVA: 0x001EA59D File Offset: 0x001E899D
	Protected Overridable Sub StartEndingAutoscroll()
		MyBase.StartCoroutine(Me.end_autoscroll())
	End Sub

	' Token: 0x06003602 RID: 13826 RVA: 0x001EA5AC File Offset: 0x001E89AC
	Protected Overridable Sub EndAutoscroll()
	End Sub

	' Token: 0x06003603 RID: 13827 RVA: 0x001EA5B0 File Offset: 0x001E89B0
	Private Iterator Function end_autoscroll() As IEnumerator
		CupheadLevelCamera.Current.SetAutoScroll(False)
		Me.EndAutoscroll()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003604 RID: 13828 RVA: 0x001EA5CC File Offset: 0x001E89CC
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Me.endPosition.transform IsNot Nothing Then
			Gizmos.DrawLine(New Vector3(Me.endPosition.transform.position.x, Me.endPosition.transform.position.y + 1500F), New Vector3(Me.endPosition.transform.position.x, Me.endPosition.transform.position.y - 1500F))
		End If
	End Sub

	' Token: 0x04003E08 RID: 15880
	<SerializeField()>
	Private endPosition As Transform

	' Token: 0x04003E09 RID: 15881
	Protected lockDistance As Single = 600F

	' Token: 0x04003E0A RID: 15882
	Protected endDelay As Single = 1F

	' Token: 0x04003E0C RID: 15884
	Protected checkToLock As Boolean

	' Token: 0x04003E0D RID: 15885
	Protected isLocked As Boolean
End Class
