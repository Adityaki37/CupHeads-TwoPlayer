Imports System
Imports UnityEngine

' Token: 0x02000951 RID: 2385
Public Class MapNPCFishgirl
	Inherits MonoBehaviour

	' Token: 0x060037B8 RID: 14264 RVA: 0x001FFBCB File Offset: 0x001FDFCB
	Private Sub Start()
		If PlayerData.Data.CheckLevelsHaveMinDifficulty(New Levels() { Levels.Mausoleum }, Level.Mode.Easy) Then
			Dialoguer.SetGlobalFloat(12, 1F)
		End If
	End Sub
End Class
