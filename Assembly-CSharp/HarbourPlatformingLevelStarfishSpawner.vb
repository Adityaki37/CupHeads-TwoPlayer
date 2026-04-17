Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008D3 RID: 2259
Public Class HarbourPlatformingLevelStarfishSpawner
	Inherits AbstractPausableComponent

	' Token: 0x060034E0 RID: 13536 RVA: 0x001EBFDC File Offset: 0x001EA3DC
	Private Sub Start()
		Me.speedX = Me.speedXString.Split(New Char() { ","c })
		Me.speedXIndex = Global.UnityEngine.Random.Range(0, Me.speedX.Length)
		Me.typeIndex = Global.UnityEngine.Random.Range(0, Me.typeString.Split(New Char() { ","c }).Length)
		MyBase.StartCoroutine(Me.spawn_cr())
	End Sub

	' Token: 0x060034E1 RID: 13537 RVA: 0x001EC04C File Offset: 0x001EA44C
	Private Iterator Function spawn_cr() As IEnumerator
		Dim hashadSuccessfulSpawn As Boolean = False
		While True
			If hashadSuccessfulSpawn Then
				Yield CupheadTime.WaitForSeconds(Me, Me.spawnTime.RandomFloat())
			Else
				Yield CupheadTime.WaitForSeconds(Me, Me.initialSpawnTime.RandomFloat())
			End If
			Dim spawnPos As Vector2 = MyBase.transform.position
			spawnPos.y = MyBase.transform.position.y
			spawnPos.x += Global.UnityEngine.Random.Range(-Me.xRange, Me.xRange)
			If CupheadLevelCamera.Current.ContainsPoint(spawnPos, New Vector2(0F, 1000F)) Then
				Me.starfishPrefab.Spawn(spawnPos).Init(90F, Parser.FloatParse(Me.speedX(Me.speedXIndex)), Me.speedYRange.RandomFloat(), Me.loopSize, Me.typeString.Split(New Char() { ","c })(Me.typeIndex))
				hashadSuccessfulSpawn = True
				Me.speedXIndex = (Me.speedXIndex + 1) Mod Me.speedX.Length
				Me.typeIndex = (Me.typeIndex + 1) Mod Me.typeString.Split(New Char() { ","c }).Length
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060034E2 RID: 13538 RVA: 0x001EC068 File Offset: 0x001EA468
	Protected Overrides Sub OnDrawGizmos()
		Gizmos.color = New Color(1F, 1F, 0F, 1F)
		Gizmos.DrawLine(MyBase.baseTransform.position - New Vector3(Me.xRange, 0F, 0F), MyBase.baseTransform.position + New Vector3(Me.xRange, 0F, 0F))
	End Sub

	' Token: 0x04003D0A RID: 15626
	<SerializeField()>
	Private initialSpawnTime As MinMax

	' Token: 0x04003D0B RID: 15627
	<SerializeField()>
	Private spawnTime As MinMax

	' Token: 0x04003D0C RID: 15628
	<SerializeField()>
	Private starfishPrefab As HarbourPlatformingLevelStarfish

	' Token: 0x04003D0D RID: 15629
	<SerializeField()>
	Private speedXString As String

	' Token: 0x04003D0E RID: 15630
	<SerializeField()>
	Private typeString As String

	' Token: 0x04003D0F RID: 15631
	<SerializeField()>
	Private speedYRange As MinMax

	' Token: 0x04003D10 RID: 15632
	<SerializeField()>
	Private xRange As Single

	' Token: 0x04003D11 RID: 15633
	<SerializeField()>
	Private loopSize As Single

	' Token: 0x04003D12 RID: 15634
	Private typeIndex As Integer

	' Token: 0x04003D13 RID: 15635
	Private speedXIndex As Integer

	' Token: 0x04003D14 RID: 15636
	Private speedX As String()
End Class
