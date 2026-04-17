Imports System
Imports UnityEngine

' Token: 0x020005BC RID: 1468
Public Class DicePalaceDominoLevelRandomSpike
	Inherits AbstractMonoBehaviour

	' Token: 0x06001C8C RID: 7308 RVA: 0x0010501F File Offset: 0x0010341F
	Private Sub Start()
		Me.ChangeSpikes()
	End Sub

	' Token: 0x06001C8D RID: 7309 RVA: 0x00105027 File Offset: 0x00103427
	Public Sub ChangeSpikes()
		MyBase.animator.SetTrigger(Me.states(Global.UnityEngine.Random.Range(0, Me.states.Length)))
		Me.melt = False
	End Sub

	' Token: 0x06001C8E RID: 7310 RVA: 0x00105050 File Offset: 0x00103450
	Private Sub Update()
		If Me.melt Then
			Return
		End If
		If MyBase.transform.position.x <= -410F Then
			Me.melt = True
			MyBase.animator.SetTrigger("Melt")
		End If
	End Sub

	' Token: 0x0400257B RID: 9595
	<SerializeField()>
	Private states As String()

	' Token: 0x0400257C RID: 9596
	Private melt As Boolean
End Class
