Imports System
Imports UnityEngine

' Token: 0x02000947 RID: 2375
Public Class MapNPCAxeman
	Inherits MonoBehaviour

	' Token: 0x0600377A RID: 14202 RVA: 0x001FE38B File Offset: 0x001FC78B
	Private Sub Start()
		If PlayerData.Data.CheckLevelsCompleted(Level.world1BossLevels) Then
			Dialoguer.SetGlobalFloat(Me.dialoguerVariableID, 1F)
			MyBase.transform.position = Me.positionAfterWorld1
		End If
	End Sub

	' Token: 0x0600377B RID: 14203 RVA: 0x001FE3C2 File Offset: 0x001FC7C2
	Protected Sub OnDrawGizmosSelected()
		Gizmos.color = Color.green
		Gizmos.DrawWireSphere(Me.positionAfterWorld1, 0.5F)
	End Sub

	' Token: 0x04003F8F RID: 16271
	Public positionAfterWorld1 As Vector3

	' Token: 0x04003F90 RID: 16272
	<SerializeField()>
	Private dialoguerVariableID As Integer = 3
End Class
