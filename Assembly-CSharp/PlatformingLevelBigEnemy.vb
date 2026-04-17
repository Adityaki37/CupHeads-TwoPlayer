Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200085C RID: 2140
Public Class PlatformingLevelBigEnemy
	Inherits PlatformingLevelShootingEnemy

	' Token: 0x060031B6 RID: 12726 RVA: 0x001D0CE9 File Offset: 0x001CF0E9
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.FrameDelayedCallback(AddressOf Me.StartLockCheck, 1)
	End Sub

	' Token: 0x060031B7 RID: 12727 RVA: 0x001D0D05 File Offset: 0x001CF105
	Private Sub StartLockCheck()
		MyBase.StartCoroutine(Me.camera_locking_cr())
	End Sub

	' Token: 0x060031B8 RID: 12728 RVA: 0x001D0D14 File Offset: 0x001CF114
	Private Iterator Function camera_locking_cr() As IEnumerator
		While Not Me.isDead
			If Not Me.bigEnemyCameraLock Then
				Me.dist = PlayerManager.Center.x - MyBase.transform.position.x
				If Me.dist > -Me.LockDistance Then
					Me.bigEnemyCameraLock = True
					CupheadLevelCamera.Current.LockCamera(True)
					Me.OnLock()
				End If
			ElseIf Me.bigEnemyCameraLock Then
				Me.dist = PlayerManager.Center.x - Me.passDistance.transform.position.x
				If Me.dist > 0F Then
					Me.bigEnemyCameraLock = False
					CupheadLevelCamera.Current.LockCamera(False)
					Me.OnPass()
					Exit While
				End If
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060031B9 RID: 12729 RVA: 0x001D0D2F File Offset: 0x001CF12F
	Protected Overridable Sub OnPass()
	End Sub

	' Token: 0x060031BA RID: 12730 RVA: 0x001D0D31 File Offset: 0x001CF131
	Protected Overridable Sub OnLock()
	End Sub

	' Token: 0x060031BB RID: 12731 RVA: 0x001D0D33 File Offset: 0x001CF133
	Protected Overrides Sub Die()
		Me.bigEnemyCameraLock = False
		CupheadLevelCamera.Current.LockCamera(False)
		MyBase.Die()
	End Sub

	' Token: 0x060031BC RID: 12732 RVA: 0x001D0D50 File Offset: 0x001CF150
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.color = Color.cyan
		Gizmos.DrawLine(Me.passDistance.transform.position, New Vector3(Me.passDistance.transform.position.x, Me.passDistance.transform.position.y - 1000F))
	End Sub

	' Token: 0x04003A19 RID: 14873
	<SerializeField()>
	Private passDistance As Transform

	' Token: 0x04003A1A RID: 14874
	Protected LockDistance As Single = 500F

	' Token: 0x04003A1B RID: 14875
	Protected bigEnemyCameraLock As Boolean

	' Token: 0x04003A1C RID: 14876
	Protected isDead As Boolean

	' Token: 0x04003A1D RID: 14877
	Private dist As Single
End Class
