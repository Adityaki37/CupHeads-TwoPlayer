Imports System

' Token: 0x020007A3 RID: 1955
Public Class SallyStagePlayHusbandExplosion
	Inherits LevelBossDeathExploder

	' Token: 0x06002BCB RID: 11211 RVA: 0x001992A6 File Offset: 0x001976A6
	Protected Overrides Sub Start()
		Me.effectPrefab = Level.Current.LevelResources.levelBossDeathExplosion
	End Sub
End Class
