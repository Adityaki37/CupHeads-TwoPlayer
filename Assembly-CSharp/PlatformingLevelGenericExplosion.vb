Imports System
Imports UnityEngine

' Token: 0x02000860 RID: 2144
Public Class PlatformingLevelGenericExplosion
	Inherits Effect

	' Token: 0x060031D2 RID: 12754 RVA: 0x001D1650 File Offset: 0x001CFA50
	Public Overrides Function Create(position As Vector3, scale As Vector3) As Effect
		Dim num As Single = Global.UnityEngine.Random.Range(0F, 1F)
		If num < Me.lightningOnlyChance + Me.starOnlyChance + Me.starsPlusLightningChance Then
			If num < Me.lightningOnlyChance OrElse num > Me.lightningOnlyChance + Me.starOnlyChance Then
				Me.lightningPrefab.Create(position, scale)
			End If
			If num > Me.lightningOnlyChance Then
				Me.starsPrefab.Create(position, scale)
			End If
		End If
		Return MyBase.Create(position, scale)
	End Function

	' Token: 0x060031D3 RID: 12755 RVA: 0x001D16D6 File Offset: 0x001CFAD6
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.lightningPrefab = Nothing
		Me.starsPrefab = Nothing
	End Sub

	' Token: 0x04003A2D RID: 14893
	<SerializeField()>
	Private lightningPrefab As Effect

	' Token: 0x04003A2E RID: 14894
	<SerializeField()>
	Private starsPrefab As Effect

	' Token: 0x04003A2F RID: 14895
	<SerializeField()>
	Private lightningOnlyChance As Single

	' Token: 0x04003A30 RID: 14896
	<SerializeField()>
	Private starOnlyChance As Single

	' Token: 0x04003A31 RID: 14897
	<SerializeField()>
	Private starsPlusLightningChance As Single
End Class
