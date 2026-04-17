Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200063D RID: 1597
Public Class FlyingBlimpLevelScrollingSpriteSpawnerBase
	Inherits AbstractPausableComponent

	' Token: 0x060020CD RID: 8397 RVA: 0x0012CF37 File Offset: 0x0012B337
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x060020CE RID: 8398 RVA: 0x0012CF4C File Offset: 0x0012B34C
	Protected Iterator Function loop_cr() As IEnumerator
		Dim leadTime As Single = 2560F / Me.speed
		Dim totalWeight As Single = 0F
		For Each scrollingSpriteInfo As FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo In Me.spritePrefabs
			totalWeight += scrollingSpriteInfo.weight
		Next
		Dim spacing As Single = Global.UnityEngine.Random.Range(0F, Me.minSpacing) + MathUtils.ExpRandom(Me.averageSpacing - Me.minSpacing)
		Dim lastSpawned As FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo = Nothing
		While True
			Dim waitTime As Single = spacing / Me.speed
			If leadTime > waitTime Then
				leadTime -= waitTime
			Else
				If leadTime > 0F Then
					waitTime -= leadTime
					leadTime = 0F
				End If
				Yield CupheadTime.WaitForSeconds(Me, waitTime)
			End If
			Dim maxP As Single = totalWeight
			If lastSpawned IsNot Nothing Then
				maxP -= lastSpawned.weight
			End If
			Dim p As Single = Global.UnityEngine.Random.Range(0F, maxP)
			Dim cumulativeWeight As Single = 0F
			Dim toSpawn As FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo = lastSpawned
			For Each scrollingSpriteInfo2 As FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo In Me.spritePrefabs
				toSpawn = scrollingSpriteInfo2
				If scrollingSpriteInfo2 IsNot lastSpawned Then
					cumulativeWeight += scrollingSpriteInfo2.weight
					If cumulativeWeight >= p Then
						Exit For
					End If
				End If
			Next
			Dim sprite As SpriteRenderer = Global.UnityEngine.[Object].Instantiate(Of SpriteRenderer)(toSpawn.sprite)
			Dim obj As GameObject = sprite.gameObject
			Dim x As Single = 1280F - leadTime * Me.speed + sprite.bounds.size.x / 2F
			obj.transform.position = New Vector2(x, Me.spawnY)
			obj.transform.SetParent(MyBase.transform, False)
			sprite.sortingOrder = Me.sortingOrder
			Dim scrollingSprite As OneTimeScrollingSprite = obj.AddComponent(Of OneTimeScrollingSprite)()
			scrollingSprite.speed = Me.speed
			spacing = Me.minSpacing + MathUtils.ExpRandom(Me.averageSpacing - Me.minSpacing) + sprite.bounds.size.x
			Me.OnSpawn(obj)
			lastSpawned = toSpawn
		End While
		Return
	End Function

	' Token: 0x060020CF RID: 8399 RVA: 0x0012CF67 File Offset: 0x0012B367
	Protected Overridable Sub OnSpawn(obj As GameObject)
	End Sub

	' Token: 0x060020D0 RID: 8400 RVA: 0x0012CF69 File Offset: 0x0012B369
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.spritePrefabs = Nothing
	End Sub

	' Token: 0x04002962 RID: 10594
	Public Const X_IN As Single = 1280F

	' Token: 0x04002963 RID: 10595
	Public Const X_OUT As Single = -1280F

	' Token: 0x04002964 RID: 10596
	<SerializeField()>
	Protected spawnY As Single

	' Token: 0x04002965 RID: 10597
	<SerializeField()>
	<Range(0F, 2000F)>
	Protected speed As Single = 100F

	' Token: 0x04002966 RID: 10598
	<SerializeField()>
	Protected minSpacing As Single = 50F

	' Token: 0x04002967 RID: 10599
	<SerializeField()>
	Protected averageSpacing As Single = 100F

	' Token: 0x04002968 RID: 10600
	<SerializeField()>
	Protected sortingOrder As Integer

	' Token: 0x04002969 RID: 10601
	<SerializeField()>
	Protected spritePrefabs As FlyingBlimpLevelScrollingSpriteSpawnerBase.ScrollingSpriteInfo()

	' Token: 0x0200063E RID: 1598
	<Serializable()>
	Public Class ScrollingSpriteInfo
		' Token: 0x0400296A RID: 10602
		Public sprite As SpriteRenderer

		' Token: 0x0400296B RID: 10603
		Public weight As Single = 1F
	End Class
End Class
