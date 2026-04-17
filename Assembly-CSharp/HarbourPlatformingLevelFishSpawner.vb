Imports System
Imports UnityEngine

' Token: 0x020008C8 RID: 2248
Public Class HarbourPlatformingLevelFishSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x0600348D RID: 13453 RVA: 0x001E8358 File Offset: 0x001E6758
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.delayIndex = Global.UnityEngine.Random.Range(0, Me.spawnDelayString.Split(New Char() { ","c }).Length)
		Me.posIndex = Global.UnityEngine.Random.Range(0, Me.spawnPositionString.Split(New Char() { ","c }).Length)
		Me.sideIndex = Global.UnityEngine.Random.Range(0, Me.spawnSideString.Split(New Char() { ","c }).Length)
		Me.typeIndex = Global.UnityEngine.Random.Range(0, Me.typeString.Split(New Char() { ","c }).Length)
	End Sub

	' Token: 0x0600348E RID: 13454 RVA: 0x001E83FC File Offset: 0x001E67FC
	Protected Overrides Sub Spawn()
		MyBase.Spawn()
		Me.spawnDelay.min = Parser.FloatParse(Me.spawnDelayString.Split(New Char() { ","c })(Me.delayIndex))
		Me.spawnDelay.max = Parser.FloatParse(Me.spawnDelayString.Split(New Char() { ","c })(Me.delayIndex))
		If Me.spawnSideString.Split(New Char() { ","c })(Me.sideIndex)(0) = "L"c Then
			Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMin - 50F
			Me.rotation = 0F
		ElseIf Me.spawnSideString.Split(New Char() { ","c })(Me.sideIndex)(0) = "R"c Then
			Me.spawnPosition.x = CupheadLevelCamera.Current.Bounds.xMax + 50F
			Me.rotation = 180F
		End If
		Me.spawnPosition.y = CupheadLevelCamera.Current.transform.position.y + Parser.FloatParse(Me.spawnPositionString.Split(New Char() { ","c })(Me.posIndex))
		Dim harbourPlatformingLevelFish As HarbourPlatformingLevelFish = Global.UnityEngine.[Object].Instantiate(Of HarbourPlatformingLevelFish)(Me.fishPrefab)
		harbourPlatformingLevelFish.Init(Me.spawnPosition, Me.rotation, Me.typeString.Split(New Char() { ","c })(Me.typeIndex))
		Me.sideIndex = (Me.sideIndex + 1) Mod Me.spawnSideString.Split(New Char() { ","c }).Length
		Me.posIndex = (Me.posIndex + 1) Mod Me.spawnPositionString.Split(New Char() { ","c }).Length
		Me.delayIndex = (Me.delayIndex + 1) Mod Me.spawnDelayString.Split(New Char() { ","c }).Length
		Me.typeIndex = (Me.typeIndex + 1) Mod Me.typeString.Split(New Char() { ","c }).Length
	End Sub

	' Token: 0x04003CB0 RID: 15536
	<SerializeField()>
	Private fishPrefab As HarbourPlatformingLevelFish

	' Token: 0x04003CB1 RID: 15537
	<SerializeField()>
	Private spawnDelayString As String

	' Token: 0x04003CB2 RID: 15538
	<SerializeField()>
	Private spawnPositionString As String

	' Token: 0x04003CB3 RID: 15539
	<SerializeField()>
	Private spawnSideString As String

	' Token: 0x04003CB4 RID: 15540
	<SerializeField()>
	Private typeString As String

	' Token: 0x04003CB5 RID: 15541
	<SerializeField()>
	Private movementSpeed As Single

	' Token: 0x04003CB6 RID: 15542
	<SerializeField()>
	Private sineSpeed As Single

	' Token: 0x04003CB7 RID: 15543
	<SerializeField()>
	Private sineSize As Single

	' Token: 0x04003CB8 RID: 15544
	Private rotation As Single

	' Token: 0x04003CB9 RID: 15545
	Private delayIndex As Integer

	' Token: 0x04003CBA RID: 15546
	Private posIndex As Integer

	' Token: 0x04003CBB RID: 15547
	Private sideIndex As Integer

	' Token: 0x04003CBC RID: 15548
	Private typeIndex As Integer

	' Token: 0x04003CBD RID: 15549
	Private spawnPosition As Vector3
End Class
