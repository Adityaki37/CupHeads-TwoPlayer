Imports System
Imports UnityEngine

' Token: 0x0200051C RID: 1308
Public Class BeeLevelQueenBlackHole
	Inherits AbstractProjectile

	' Token: 0x06001762 RID: 5986 RVA: 0x000D2A94 File Offset: 0x000D0E94
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.direction = If((MyBase.transform.position.x >= 0F), (-1), 1)
	End Sub

	' Token: 0x06001763 RID: 5987 RVA: 0x000D2AD4 File Offset: 0x000D0ED4
	Protected Overrides Sub Update()
		MyBase.Update()
		MyBase.transform.AddPosition(Me.speed * CSng(Me.direction) * CupheadTime.Delta, 0F, 0F)
		Me.timer += CupheadTime.Delta
		If Me.timer >= Me.childDelay Then
			Me.childPrefab.Create(MyBase.transform.position, 90F, Me.childSpeed)
			Me.childPrefab.Create(MyBase.transform.position, -90F, Me.childSpeed).GetComponent(Of Animator)().Play("Reverse")
			Me.timer = 0F
		End If
	End Sub

	' Token: 0x06001764 RID: 5988 RVA: 0x000D2BA4 File Offset: 0x000D0FA4
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.childPrefab = Nothing
	End Sub

	' Token: 0x04002099 RID: 8345
	<HideInInspector()>
	Public health As Single

	' Token: 0x0400209A RID: 8346
	<HideInInspector()>
	Public speed As Single

	' Token: 0x0400209B RID: 8347
	<HideInInspector()>
	Public childDelay As Single

	' Token: 0x0400209C RID: 8348
	<HideInInspector()>
	Public childSpeed As Single

	' Token: 0x0400209D RID: 8349
	<SerializeField()>
	Private childPrefab As BasicProjectile

	' Token: 0x0400209E RID: 8350
	Private direction As Integer

	' Token: 0x0400209F RID: 8351
	Private timer As Single
End Class
