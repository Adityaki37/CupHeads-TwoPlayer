Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006D6 RID: 1750
Public Class MausoleumLevelDelayGhost
	Inherits MausoleumLevelGhostBase

	' Token: 0x06002542 RID: 9538 RVA: 0x0015D460 File Offset: 0x0015B860
	Public Function Create(position As Vector2, rotation As Single, speed As Single, properties As LevelProperties.Mausoleum.DelayGhost) As MausoleumLevelDelayGhost
		Dim mausoleumLevelDelayGhost As MausoleumLevelDelayGhost = TryCast(MyBase.Create(position, rotation, speed), MausoleumLevelDelayGhost)
		mausoleumLevelDelayGhost.properties = properties
		Return mausoleumLevelDelayGhost
	End Function

	' Token: 0x06002543 RID: 9539 RVA: 0x0015D485 File Offset: 0x0015B885
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.wait_cr())
	End Sub

	' Token: 0x06002544 RID: 9540 RVA: 0x0015D49C File Offset: 0x0015B89C
	Private Iterator Function wait_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.dashDelay)
		Me.Speed = Me.properties.speed
		Yield Nothing
		Return
	End Function

	' Token: 0x04002DE2 RID: 11746
	Private properties As LevelProperties.Mausoleum.DelayGhost
End Class
