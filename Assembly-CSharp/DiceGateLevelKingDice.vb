Imports System
Imports UnityEngine

' Token: 0x0200059A RID: 1434
Public Class DiceGateLevelKingDice
	Inherits MonoBehaviour

	' Token: 0x06001B7C RID: 7036 RVA: 0x000FB282 File Offset: 0x000F9682
	Public Sub SetDisappearBool()
		PlayerData.Data.CurrentMapData.hasKingDiceDisappeared = True
		PlayerData.SaveCurrentFile()
	End Sub

	' Token: 0x06001B7D RID: 7037 RVA: 0x000FB299 File Offset: 0x000F9699
	Private Sub SoundKingDiceExitAnim()
		AudioManager.Play("dicegate_kingdice_exit")
	End Sub
End Class
