Imports System
Imports UnityEngine

' Token: 0x020006DA RID: 1754
Public Class MausoleumLevelUrn
	Inherits AbstractCollidableObject

	' Token: 0x170003BF RID: 959
	' (get) Token: 0x0600255F RID: 9567 RVA: 0x0015D97D File Offset: 0x0015BD7D
	' (set) Token: 0x06002560 RID: 9568 RVA: 0x0015D984 File Offset: 0x0015BD84
	Public Shared Property URN_POS As Vector3

	' Token: 0x06002561 RID: 9569 RVA: 0x0015D98C File Offset: 0x0015BD8C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MausoleumLevelUrn.URN_POS = MyBase.transform.position
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06002562 RID: 9570 RVA: 0x0015D9AF File Offset: 0x0015BDAF
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x04002DF1 RID: 11761
	Private damageDealer As DamageDealer
End Class
