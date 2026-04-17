Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020007F1 RID: 2033
Public Class SnowCultLevelPlatformManager
	Inherits AbstractCollidableObject

	' Token: 0x06002EAA RID: 11946 RVA: 0x001B87B3 File Offset: 0x001B6BB3
	Private Sub Start()
		Me.platforms = New List(Of SnowCultLevelPlatform)()
		Me.InstantiatePlatforms()
	End Sub

	' Token: 0x06002EAB RID: 11947 RVA: 0x001B87C8 File Offset: 0x001B6BC8
	Private Sub InstantiatePlatforms()
		For i As Integer = 0 To 20 - 1
			Dim snowCultLevelPlatform As SnowCultLevelPlatform = Global.UnityEngine.[Object].Instantiate(Of SnowCultLevelPlatform)(Me.platformPrefab)
			snowCultLevelPlatform.gameObject.SetActive(False)
			snowCultLevelPlatform.transform.parent = MyBase.transform
			Me.platforms.Add(snowCultLevelPlatform)
		Next
	End Sub

	' Token: 0x0400374C RID: 14156
	Private Const NUM_OF_PLATFORMS As Integer = 20

	' Token: 0x0400374D RID: 14157
	<SerializeField()>
	Private platformPrefab As SnowCultLevelPlatform

	' Token: 0x0400374E RID: 14158
	Private platforms As List(Of SnowCultLevelPlatform)
End Class
