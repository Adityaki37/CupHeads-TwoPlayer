Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008B4 RID: 2228
Public Class FunhousePlatformingLevelDuckCarSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x060033EF RID: 13295 RVA: 0x001E1ED7 File Offset: 0x001E02D7
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.duckPinkIndex = Global.UnityEngine.Random.Range(0, Me.duckPinkString.Split(New Char() { ","c }).Length)
	End Sub

	' Token: 0x060033F0 RID: 13296 RVA: 0x001E1F03 File Offset: 0x001E0303
	Protected Overrides Sub StartSpawning()
		MyBase.StartSpawning()
		MyBase.StartCoroutine(Me.start_spawning_cr())
	End Sub

	' Token: 0x060033F1 RID: 13297 RVA: 0x001E1F18 File Offset: 0x001E0318
	Protected Overrides Sub EndSpawning()
		MyBase.StopCoroutine(Me.start_spawning_cr())
		MyBase.EndSpawning()
	End Sub

	' Token: 0x060033F2 RID: 13298 RVA: 0x001E1F2C File Offset: 0x001E032C
	Private Iterator Function start_spawning_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.initalSpawnDelay.RandomFloat())
		While True
			If Me.firstTime Then
				Me.ducksTop = False
				Me.firstTime = False
			Else
				Me.ducksTop = Not Me.ducksTop
			End If
			MyBase.StartCoroutine(Me.spawn_ducks_cr())
			MyBase.StartCoroutine(Me.spawn_cars_cr())
			Me.ducksSpawning = True
			Me.carsSpawning = True
			While Me.ducksSpawning OrElse Me.carsSpawning
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, Me.spawnDelay.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060033F3 RID: 13299 RVA: 0x001E1F48 File Offset: 0x001E0348
	Private Iterator Function spawn_ducks_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.duckDelay)
		Dim bigDuckSize As Single = Me.bigDuckPrefab.GetComponentInChildren(Of SpriteRenderer)().bounds.size.x
		Dim smallDuckSize As Single = Me.smallDuckPrefab.GetComponentInChildren(Of SpriteRenderer)().bounds.size.x
		Dim lastDuck As FunhousePlatformingLevelDuck = Nothing
		Dim startPos As Vector2 = Vector2.zero
		startPos.x = CupheadLevelCamera.Current.Bounds.xMax + bigDuckSize + Me.duckSpacing
		startPos.y = If((Not Me.ducksTop), Me.bottomSpawnRoot.position.y, Me.topSpawnRoot.position.y)
		Dim bigDuck As FunhousePlatformingLevelDuck = Me.bigDuckPrefab.Spawn(startPos)
		bigDuck.transform.SetScale(Nothing, New Single?(If((Not Me.ducksTop), bigDuck.transform.localScale.y, (-bigDuck.transform.localScale.y))), Nothing)
		Dim num As Integer = 1
		While CSng(num) < Me.duckCount
			startPos.x = CupheadLevelCamera.Current.Bounds.xMax + bigDuckSize + Me.duckSpacing + (smallDuckSize + Me.duckSpacing) * CSng(num)
			startPos.y = If((Not Me.ducksTop), Me.bottomSpawnRoot.position.y, Me.topSpawnRoot.position.y)
			If Me.duckPinkString.Split(New Char() { ","c })(Me.duckPinkIndex)(0) = "P"c Then
				Dim funhousePlatformingLevelDuck As FunhousePlatformingLevelDuck = Me.smallDuckPrefab.Spawn(startPos)
				funhousePlatformingLevelDuck.transform.SetScale(Nothing, New Single?(If((Not Me.ducksTop), funhousePlatformingLevelDuck.transform.localScale.y, (-funhousePlatformingLevelDuck.transform.localScale.y))), Nothing)
				funhousePlatformingLevelDuck.smallFirst = num = 1
				If CSng(num) = Me.duckCount - 1F Then
					funhousePlatformingLevelDuck.smallLast = True
					lastDuck = funhousePlatformingLevelDuck
				Else
					funhousePlatformingLevelDuck.smallLast = False
				End If
			ElseIf Me.duckPinkString.Split(New Char() { ","c })(Me.duckPinkIndex)(0) = "R"c Then
				Dim funhousePlatformingLevelDuck2 As FunhousePlatformingLevelDuck = Me.smallDuckPinkPrefab.Spawn(startPos)
				funhousePlatformingLevelDuck2.transform.SetScale(Nothing, New Single?(If((Not Me.ducksTop), funhousePlatformingLevelDuck2.transform.localScale.y, (-funhousePlatformingLevelDuck2.transform.localScale.y))), Nothing)
				funhousePlatformingLevelDuck2.smallFirst = num = 1
				If CSng(num) = Me.duckCount - 1F Then
					funhousePlatformingLevelDuck2.smallLast = True
					lastDuck = funhousePlatformingLevelDuck2
				Else
					funhousePlatformingLevelDuck2.smallLast = False
				End If
			End If
			Me.duckPinkIndex = (Me.duckPinkIndex + 1) Mod Me.duckPinkString.Split(New Char() { ","c }).Length
			num += 1
		End While
		While lastDuck IsNot Nothing
			If lastDuck.transform.position.x < CupheadLevelCamera.Current.transform.position.x Then
				Exit While
			End If
			Yield Nothing
		End While
		Me.ducksSpawning = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060033F4 RID: 13300 RVA: 0x001E1F64 File Offset: 0x001E0364
	Private Iterator Function spawn_cars_cr() As IEnumerator
		Me.SpawnHonk(If((Not Me.ducksTop), Me.topSpawnRoot.position.y, Me.bottomSpawnRoot.position.y), CSng(If((Not Me.ducksTop), (-1), 1)), If((Not Me.ducksTop), (-100F), 100F))
		Yield CupheadTime.WaitForSeconds(Me, Me.carDelay)
		Dim carSize As Single = Me.carPrefabNormal.GetComponentInChildren(Of SpriteRenderer)().bounds.size.x
		Dim index As Integer = 0
		Dim lastCar As FunhousePlatformingLevelCar = Nothing
		Dim startPos As Vector2 = Vector2.zero
		For i As Integer = 0 To Me.carCount - 1
			startPos.x = CupheadLevelCamera.Current.Bounds.xMax + (carSize + Me.carSpacing * CSng(i))
			startPos.y = If((Not Me.ducksTop), Me.topSpawnRoot.position.y, Me.bottomSpawnRoot.position.y)
			Dim funhousePlatformingLevelCar As FunhousePlatformingLevelCar = Global.UnityEngine.[Object].Instantiate(Of FunhousePlatformingLevelCar)(Me.carPrefabNormal)
			funhousePlatformingLevelCar.Init(startPos, 180F, Me.carSpeed, index, i = 0, i = Me.carCount - 1)
			funhousePlatformingLevelCar.transform.SetScale(Nothing, New Single?(If((Not Me.ducksTop), funhousePlatformingLevelCar.transform.localScale.y, (-funhousePlatformingLevelCar.transform.localScale.y))), Nothing)
			If i = Me.carCount - 1 Then
				lastCar = funhousePlatformingLevelCar
			End If
			index = (index + 1) Mod 4
		Next
		While lastCar.transform.position.x > CupheadLevelCamera.Current.transform.position.x
			Yield Nothing
		End While
		Me.carsSpawning = False
		Yield Nothing
		Return
	End Function

	' Token: 0x060033F5 RID: 13301 RVA: 0x001E1F80 File Offset: 0x001E0380
	Private Sub SpawnHonk(rootY As Single, yScale As Single, offset As Single)
		AudioManager.Play("funhouse_car_honk_sweet")
		Dim vector As Vector2 = New Vector2(CupheadLevelCamera.Current.Bounds.xMax, rootY + offset)
		Dim effect As Effect = Me.honkEffect.Create(vector)
		effect.transform.parent = CupheadLevelCamera.Current.transform
		effect.transform.SetScale(Nothing, New Single?(yScale), Nothing)
	End Sub

	' Token: 0x060033F6 RID: 13302 RVA: 0x001E2000 File Offset: 0x001E0400
	Protected Overrides Sub OnDrawGizmos()
		Gizmos.color = New Color(1F, 1F, 0F, 1F)
		Gizmos.DrawWireSphere(Me.topSpawnRoot.transform.position, 50F)
		Gizmos.color = New Color(1F, 0F, 1F, 1F)
		Gizmos.DrawWireSphere(Me.bottomSpawnRoot.transform.position, 50F)
	End Sub

	' Token: 0x04003C31 RID: 15409
	<SerializeField()>
	Private honkEffect As Effect

	' Token: 0x04003C32 RID: 15410
	<SerializeField()>
	Private topSpawnRoot As Transform

	' Token: 0x04003C33 RID: 15411
	<SerializeField()>
	Private bottomSpawnRoot As Transform

	' Token: 0x04003C34 RID: 15412
	<Header("Cars")>
	<SerializeField()>
	Private carPrefabNormal As FunhousePlatformingLevelCar

	' Token: 0x04003C35 RID: 15413
	<SerializeField()>
	Private carSpeed As Single

	' Token: 0x04003C36 RID: 15414
	<SerializeField()>
	Private carDelay As Single

	' Token: 0x04003C37 RID: 15415
	<SerializeField()>
	Private carSpacing As Single

	' Token: 0x04003C38 RID: 15416
	<SerializeField()>
	Private carCount As Integer

	' Token: 0x04003C39 RID: 15417
	<Header("Ducks")>
	<SerializeField()>
	Private bigDuckPrefab As FunhousePlatformingLevelDuck

	' Token: 0x04003C3A RID: 15418
	<SerializeField()>
	Private smallDuckPrefab As FunhousePlatformingLevelDuck

	' Token: 0x04003C3B RID: 15419
	<SerializeField()>
	Private smallDuckPinkPrefab As FunhousePlatformingLevelDuck

	' Token: 0x04003C3C RID: 15420
	<SerializeField()>
	Private duckDelay As Single

	' Token: 0x04003C3D RID: 15421
	<SerializeField()>
	Private duckCount As Single

	' Token: 0x04003C3E RID: 15422
	<SerializeField()>
	Private duckSpacing As Single

	' Token: 0x04003C3F RID: 15423
	<SerializeField()>
	Private duckPinkString As String

	' Token: 0x04003C40 RID: 15424
	Private duckPinkIndex As Integer

	' Token: 0x04003C41 RID: 15425
	Private carsSpawning As Boolean

	' Token: 0x04003C42 RID: 15426
	Private ducksSpawning As Boolean

	' Token: 0x04003C43 RID: 15427
	Private ducksTop As Boolean

	' Token: 0x04003C44 RID: 15428
	Private firstTime As Boolean = True
End Class
