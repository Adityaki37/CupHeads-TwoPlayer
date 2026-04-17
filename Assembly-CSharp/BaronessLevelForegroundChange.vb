Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020004FC RID: 1276
Public Class BaronessLevelForegroundChange
	Inherits AbstractPausableComponent

	' Token: 0x06001678 RID: 5752 RVA: 0x000C9C98 File Offset: 0x000C8098
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.bossNotDead = True
		Me.currentClones = New List(Of OneTimeScrollingSprite)()
		MyBase.StartCoroutine(Me.start_phase2_cr())
	End Sub

	' Token: 0x06001679 RID: 5753 RVA: 0x000C9CC0 File Offset: 0x000C80C0
	Private Iterator Function start_phase2_cr() As IEnumerator
		For i As Integer = 0 To Me.sprites.Length - 1
			Me.sprites(i).speed = 0F
		Next
		For Each oneTimeScrollingSprite As OneTimeScrollingSprite In Me.currentClones
			If oneTimeScrollingSprite IsNot Nothing Then
				oneTimeScrollingSprite.GetComponent(Of OneTimeScrollingSprite)().speed = 0F
			End If
		Next
		While Me.baroness.state <> BaronessLevelCastle.State.Chase
			Yield Nothing
		End While
		Me.StartLoop()
		While True
			If Not Me.baroness.pauseScrolling Then
				For j As Integer = 0 To Me.sprites.Length - 1
					Me.sprites(j).speed = Me.speed
				Next
				For Each oneTimeScrollingSprite2 As OneTimeScrollingSprite In Me.currentClones
					If oneTimeScrollingSprite2 IsNot Nothing Then
						oneTimeScrollingSprite2.GetComponent(Of OneTimeScrollingSprite)().speed = Me.speed
					End If
				Next
			Else
				For k As Integer = 0 To Me.sprites.Length - 1
					Me.sprites(k).speed = 0F
				Next
				For Each oneTimeScrollingSprite3 As OneTimeScrollingSprite In Me.currentClones
					If oneTimeScrollingSprite3 IsNot Nothing Then
						oneTimeScrollingSprite3.GetComponent(Of OneTimeScrollingSprite)().speed = 0F
					End If
				Next
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600167A RID: 5754 RVA: 0x000C9CDB File Offset: 0x000C80DB
	Private Sub StartLoop()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x0600167B RID: 5755 RVA: 0x000C9CEC File Offset: 0x000C80EC
	Private Iterator Function loop_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim leadTime As Single = 0F / Me.speed
		Dim totalWeight As Single = 0F
		For Each scrollingSpriteInfo As BaronessLevelForegroundChange.ScrollingSpriteInfo In Me.spritePrefabs
			totalWeight += scrollingSpriteInfo.weight
		Next
		Dim spacing As Single = Global.UnityEngine.Random.Range(0F, Me.minSpacing) + MathUtils.ExpRandom(Me.averageSpacing - Me.minSpacing)
		Dim lastSpawned As BaronessLevelForegroundChange.ScrollingSpriteInfo = Nothing
		While True
			If Me.bossNotDead AndAlso Not Me.baroness.pauseScrolling Then
				Dim waitTime As Single = spacing / Me.speed
				If leadTime > waitTime Then
					leadTime -= waitTime
					Yield Nothing
				Else
					If leadTime > 0F Then
						waitTime -= leadTime
						leadTime = 0F
						Yield Nothing
					End If
					Yield CupheadTime.WaitForSeconds(Me, waitTime)
				End If
				Dim maxP As Single = totalWeight
				If lastSpawned IsNot Nothing Then
					maxP -= lastSpawned.weight
					Yield Nothing
				End If
				Dim p As Single = Global.UnityEngine.Random.Range(0F, maxP)
				Dim cumulativeWeight As Single = 0F
				Dim toSpawn As BaronessLevelForegroundChange.ScrollingSpriteInfo = lastSpawned
				For Each scrollingSpriteInfo2 As BaronessLevelForegroundChange.ScrollingSpriteInfo In Me.spritePrefabs
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
				Dim x As Single = -1280F - leadTime * Me.speed + sprite.bounds.size.x / 2F
				obj.transform.position = New Vector2(x, Me.spawnY)
				obj.transform.SetParent(MyBase.transform, False)
				sprite.sortingOrder = Me.sortingOrder
				Dim scrollingSprite As OneTimeScrollingSprite = obj.AddComponent(Of OneTimeScrollingSprite)()
				scrollingSprite.speed = Me.speed
				spacing = Me.minSpacing + MathUtils.ExpRandom(Me.averageSpacing - Me.minSpacing) - sprite.bounds.size.x
				Me.OnSpawn(obj)
				lastSpawned = toSpawn
				Me.currentClones.Add(scrollingSprite)
				Yield Nothing
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600167C RID: 5756 RVA: 0x000C9D07 File Offset: 0x000C8107
	Protected Overridable Sub OnSpawn(obj As GameObject)
	End Sub

	' Token: 0x04001FCC RID: 8140
	Public Const X_IN As Single = -1280F

	' Token: 0x04001FCD RID: 8141
	Public Const X_OUT As Single = 1280F

	' Token: 0x04001FCE RID: 8142
	<SerializeField()>
	Private spawnY As Single

	' Token: 0x04001FCF RID: 8143
	<SerializeField()>
	<Range(0F, -2000F)>
	Private speed As Single

	' Token: 0x04001FD0 RID: 8144
	<SerializeField()>
	<Range(0F, -2000F)>
	Private minSpacing As Single

	' Token: 0x04001FD1 RID: 8145
	<SerializeField()>
	<Range(0F, -2000F)>
	Private averageSpacing As Single

	' Token: 0x04001FD2 RID: 8146
	<SerializeField()>
	Private sortingOrder As Integer

	' Token: 0x04001FD3 RID: 8147
	<SerializeField()>
	Private spritePrefabs As BaronessLevelForegroundChange.ScrollingSpriteInfo()

	' Token: 0x04001FD4 RID: 8148
	<SerializeField()>
	Private baroness As BaronessLevelCastle

	' Token: 0x04001FD5 RID: 8149
	<SerializeField()>
	Private sprites As OneTimeScrollingSprite()

	' Token: 0x04001FD6 RID: 8150
	Private currentClones As List(Of OneTimeScrollingSprite)

	' Token: 0x04001FD7 RID: 8151
	Private bossNotDead As Boolean

	' Token: 0x020004FD RID: 1277
	<Serializable()>
	Public Class ScrollingSpriteInfo
		' Token: 0x04001FD8 RID: 8152
		Public sprite As SpriteRenderer

		' Token: 0x04001FD9 RID: 8153
		Public weight As Single = 1F
	End Class
End Class
