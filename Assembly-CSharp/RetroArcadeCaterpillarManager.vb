Imports System
Imports UnityEngine

' Token: 0x0200073F RID: 1855
Public Class RetroArcadeCaterpillarManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x170003D8 RID: 984
	' (get) Token: 0x06002871 RID: 10353 RVA: 0x0017907C File Offset: 0x0017747C
	' (set) Token: 0x06002872 RID: 10354 RVA: 0x00179084 File Offset: 0x00177484
	Public Property moveSpeed As Single

	' Token: 0x06002873 RID: 10355 RVA: 0x00179090 File Offset: 0x00177490
	Public Sub StartCaterpillar()
		Me.p = MyBase.properties.CurrentState.caterpillar
		Me.bodyParts = New RetroArcadeCaterpillarBodyPart(Me.p.bodyParts.Length + 1 - 1) {}
		Dim direction As RetroArcadeCaterpillarBodyPart.Direction = If((Not Rand.Bool()), RetroArcadeCaterpillarBodyPart.Direction.Right, RetroArcadeCaterpillarBodyPart.Direction.Left)
		Me.bodyParts(0) = Me.bodyPartPrefabs(0).Create(0, direction, Me, Me.p)
		For i As Integer = 0 To Me.p.bodyParts.Length - 1
			Me.bodyParts(i + 1) = Me.bodyPartPrefabs(Me.p.bodyParts(i)).Create(i + 1, direction, Me, Me.p)
		Next
		Me.numDied = 0
		Me.numSpidersSpawned = 0
		Me.moveSpeed = 640F / Me.p.moveTime
	End Sub

	' Token: 0x06002874 RID: 10356 RVA: 0x00179170 File Offset: 0x00177570
	Public Sub OnBodyPartDie(alien As RetroArcadeCaterpillarBodyPart)
		Me.numDied += 1
		Me.moveSpeed = 640F / (Me.p.moveTime - CSng(Me.numDied) * Me.p.moveTimeDecrease)
		If Me.numDied >= Me.bodyParts.Length - 1 Then
			Me.StopAllCoroutines()
			Me.bodyParts(0).Dead()
			For Each retroArcadeCaterpillarBodyPart As RetroArcadeCaterpillarBodyPart In Me.bodyParts
				retroArcadeCaterpillarBodyPart.OnWaveEnd()
			Next
			MyBase.properties.DealDamageToNextNamedState()
		End If
	End Sub

	' Token: 0x06002875 RID: 10357 RVA: 0x00179210 File Offset: 0x00177610
	Public Sub OnReachBottom()
		If Me.numSpidersSpawned < Me.p.spiderCount Then
			Me.numSpidersSpawned += 1
			Me.spiderPrefab.Create(If((Not Rand.Bool()), RetroArcadeCaterpillarSpider.Direction.Right, RetroArcadeCaterpillarSpider.Direction.Left), Me.p)
		End If
	End Sub

	' Token: 0x04003139 RID: 12601
	<SerializeField()>
	Private bodyPartPrefabs As RetroArcadeCaterpillarBodyPart()

	' Token: 0x0400313A RID: 12602
	<SerializeField()>
	Private spiderPrefab As RetroArcadeCaterpillarSpider

	' Token: 0x0400313B RID: 12603
	Private bodyParts As RetroArcadeCaterpillarBodyPart()

	' Token: 0x0400313D RID: 12605
	Private p As LevelProperties.RetroArcade.Caterpillar

	' Token: 0x0400313E RID: 12606
	Private numDied As Integer

	' Token: 0x0400313F RID: 12607
	Private numSpidersSpawned As Integer
End Class
