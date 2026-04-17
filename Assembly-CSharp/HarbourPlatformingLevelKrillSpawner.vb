Imports System
Imports UnityEngine

' Token: 0x020008CC RID: 2252
Public Class HarbourPlatformingLevelKrillSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x060034A1 RID: 13473 RVA: 0x001E8B14 File Offset: 0x001E6F14
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.posIndex = Global.UnityEngine.Random.Range(0, Me.posString.Split(New Char() { ","c }).Length)
		Me.typeIndex = Global.UnityEngine.Random.Range(0, Me.typeString.Split(New Char() { ","c }).Length)
		Me.delayIndex = Global.UnityEngine.Random.Range(0, Me.delayString.Split(New Char() { ","c }).Length)
	End Sub

	' Token: 0x060034A2 RID: 13474 RVA: 0x001E8B94 File Offset: 0x001E6F94
	Protected Overrides Sub Spawn()
		MyBase.Spawn()
		Me.spawnDelay.min = Parser.FloatParse(Me.delayString.Split(New Char() { ","c })(Me.delayIndex))
		Me.spawnDelay.max = Parser.FloatParse(Me.delayString.Split(New Char() { ","c })(Me.delayIndex))
		Dim vector As Vector2 = CupheadLevelCamera.Current.transform.position
		vector.x = CupheadLevelCamera.Current.transform.position.x + CSng(Parser.IntParse(Me.posString.Split(New Char() { ","c })(Me.posIndex)))
		vector.y = CupheadLevelCamera.Current.Bounds.yMin - 50F
		Me.parryable = Me.typeString.Split(New Char() { ","c })(Me.typeIndex)(0) = "A"c
		Dim harbourPlatformingLevelKrill As HarbourPlatformingLevelKrill = Me.krillPrefab.Spawn(Nothing, vector)
		harbourPlatformingLevelKrill.isParryable = Me.parryable
		harbourPlatformingLevelKrill.SetType(Me.typeString.Split(New Char() { ","c })(Me.typeIndex))
		Me.posIndex = (Me.posIndex + 1) Mod Me.posString.Split(New Char() { ","c }).Length
		Me.typeIndex = (Me.typeIndex + 1) Mod Me.typeString.Split(New Char() { ","c }).Length
		Me.delayIndex = (Me.delayIndex + 1) Mod Me.delayString.Split(New Char() { ","c }).Length
	End Sub

	' Token: 0x04003CC8 RID: 15560
	<SerializeField()>
	Private krillPrefab As HarbourPlatformingLevelKrill

	' Token: 0x04003CC9 RID: 15561
	<SerializeField()>
	Private posString As String = "305,640,356"

	' Token: 0x04003CCA RID: 15562
	<SerializeField()>
	Private typeString As String

	' Token: 0x04003CCB RID: 15563
	<SerializeField()>
	Private delayString As String

	' Token: 0x04003CCC RID: 15564
	Private posIndex As Integer

	' Token: 0x04003CCD RID: 15565
	Private typeIndex As Integer

	' Token: 0x04003CCE RID: 15566
	Private delayIndex As Integer

	' Token: 0x04003CCF RID: 15567
	Private parryable As Boolean
End Class
