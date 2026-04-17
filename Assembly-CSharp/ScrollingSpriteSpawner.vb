Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200043F RID: 1087
Public Class ScrollingSpriteSpawner
	Inherits AbstractPausableComponent

	' Token: 0x06000FF9 RID: 4089 RVA: 0x0009E074 File Offset: 0x0009C474
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Not Me.customStart Then
			MyBase.StartCoroutine(Me.loop_cr(False))
		End If
	End Sub

	' Token: 0x06000FFA RID: 4090 RVA: 0x0009E095 File Offset: 0x0009C495
	Public Sub StartLoop(Optional ensureInitialOffscreenSpawn As Boolean = False)
		MyBase.StartCoroutine(Me.loop_cr(ensureInitialOffscreenSpawn))
	End Sub

	' Token: 0x06000FFB RID: 4091 RVA: 0x0009E0A8 File Offset: 0x0009C4A8
	Private Iterator Function loop_cr(ensureInitialOffscreenSpawn As Boolean) As IEnumerator
		Dim leadTime As Single = 2560F / Me.speed
		Dim totalWeight As Single = 0F
		For Each scrollingSpriteInfo As ScrollingSpriteSpawner.ScrollingSpriteInfo In Me.spritePrefabs
			totalWeight += scrollingSpriteInfo.weight
		Next
		Dim spacing As Single = Global.UnityEngine.Random.Range(0F, Me.minSpacing) + MathUtils.ExpRandom(Me.averageSpacing - Me.minSpacing)
		Dim lastSpawned As ScrollingSpriteSpawner.ScrollingSpriteInfo = Nothing
		While True
			While Me.pauseScrolling
				Yield Nothing
			End While
			Dim waitTime As Single = spacing / Me.speed
			If ensureInitialOffscreenSpawn Then
				waitTime = Mathf.Max(waitTime, leadTime * 1.1F)
			End If
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
			Dim toSpawn As ScrollingSpriteSpawner.ScrollingSpriteInfo = lastSpawned
			For Each scrollingSpriteInfo2 As ScrollingSpriteSpawner.ScrollingSpriteInfo In Me.spritePrefabs
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
			If Me.usePrefabY Then
				obj.transform.position = New Vector3(x, toSpawn.sprite.transform.position.y)
			Else
				obj.transform.position = New Vector2(x, Me.spawnY)
			End If
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

	' Token: 0x06000FFC RID: 4092 RVA: 0x0009E0CA File Offset: 0x0009C4CA
	Public Sub HandlePausing(pause As Boolean)
		Me.pauseScrolling = pause
	End Sub

	' Token: 0x06000FFD RID: 4093 RVA: 0x0009E0D3 File Offset: 0x0009C4D3
	Protected Overridable Sub OnSpawn(obj As GameObject)
	End Sub

	' Token: 0x0400198F RID: 6543
	Public Const X_IN As Single = 1280F

	' Token: 0x04001990 RID: 6544
	Public Const X_OUT As Single = -1280F

	' Token: 0x04001991 RID: 6545
	<SerializeField()>
	Private customStart As Boolean

	' Token: 0x04001992 RID: 6546
	<SerializeField()>
	Private spawnY As Single

	' Token: 0x04001993 RID: 6547
	<SerializeField()>
	Private usePrefabY As Boolean

	' Token: 0x04001994 RID: 6548
	<SerializeField()>
	<Range(0F, 2000F)>
	Private speed As Single = 100F

	' Token: 0x04001995 RID: 6549
	<SerializeField()>
	Private minSpacing As Single = 50F

	' Token: 0x04001996 RID: 6550
	<SerializeField()>
	Private averageSpacing As Single = 100F

	' Token: 0x04001997 RID: 6551
	<SerializeField()>
	Private sortingOrder As Integer

	' Token: 0x04001998 RID: 6552
	<SerializeField()>
	Private spritePrefabs As ScrollingSpriteSpawner.ScrollingSpriteInfo()

	' Token: 0x04001999 RID: 6553
	Private pauseScrolling As Boolean

	' Token: 0x02000440 RID: 1088
	<Serializable()>
	Public Class ScrollingSpriteInfo
		' Token: 0x0400199A RID: 6554
		Public sprite As SpriteRenderer

		' Token: 0x0400199B RID: 6555
		Public weight As Single = 1F
	End Class
End Class
