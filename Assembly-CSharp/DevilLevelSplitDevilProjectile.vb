Imports System
Imports UnityEngine

' Token: 0x02000597 RID: 1431
Public Class DevilLevelSplitDevilProjectile
	Inherits BasicProjectile

	' Token: 0x06001B6C RID: 7020 RVA: 0x000FB020 File Offset: 0x000F9420
	Public Function Create(position As Vector2, rotation As Single, speed As Single, devil As DevilLevelSplitDevil) As DevilLevelSplitDevilProjectile
		Dim devilLevelSplitDevilProjectile As DevilLevelSplitDevilProjectile = TryCast(MyBase.Create(position, rotation, speed), DevilLevelSplitDevilProjectile)
		devilLevelSplitDevilProjectile.devil = devil
		Return devilLevelSplitDevilProjectile
	End Function

	' Token: 0x06001B6D RID: 7021 RVA: 0x000FB045 File Offset: 0x000F9445
	Protected Overrides Sub Update()
		MyBase.Update()
		If MyBase.dead Then
			Return
		End If
		If Me.devil Is Nothing Then
			Me.Die()
			Return
		End If
		Me.UpdateColor()
	End Sub

	' Token: 0x06001B6E RID: 7022 RVA: 0x000FB077 File Offset: 0x000F9477
	Private Sub UpdateColor()
	End Sub

	' Token: 0x06001B6F RID: 7023 RVA: 0x000FB079 File Offset: 0x000F9479
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x040024A3 RID: 9379
	Private devil As DevilLevelSplitDevil
End Class
