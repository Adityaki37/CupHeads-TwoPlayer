Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000740 RID: 1856
Public Class RetroArcadeCaterpillarSpider
	Inherits RetroArcadeEnemy

	' Token: 0x06002877 RID: 10359 RVA: 0x0017926C File Offset: 0x0017766C
	Public Function Create(direction As RetroArcadeCaterpillarSpider.Direction, properties As LevelProperties.RetroArcade.Caterpillar) As RetroArcadeCaterpillarSpider
		Dim retroArcadeCaterpillarSpider As RetroArcadeCaterpillarSpider = Me.InstantiatePrefab(Of RetroArcadeCaterpillarSpider)()
		retroArcadeCaterpillarSpider.transform.SetPosition(New Single?(If((direction <> RetroArcadeCaterpillarSpider.Direction.Right), 320F, (-320F))), New Single?(300F), Nothing)
		retroArcadeCaterpillarSpider.direction = direction
		retroArcadeCaterpillarSpider.properties = properties
		retroArcadeCaterpillarSpider.targetPos = New Vector2(retroArcadeCaterpillarSpider.transform.position.x, properties.spiderPathY.max)
		retroArcadeCaterpillarSpider.state = RetroArcadeCaterpillarSpider.State.Entering
		retroArcadeCaterpillarSpider.hp = 1F
		Return retroArcadeCaterpillarSpider
	End Function

	' Token: 0x06002878 RID: 10360 RVA: 0x00179304 File Offset: 0x00177704
	Protected Overrides Sub FixedUpdate()
		If MyBase.IsDead Then
			Return
		End If
		Dim num As Single = Me.properties.spiderSpeed * CupheadTime.FixedDelta
		Dim magnitude As Single = (Me.targetPos - MyBase.transform.position).magnitude
		If magnitude > num Then
			Me.move(num)
		Else
			MyBase.transform.position = Me.targetPos
			Select Case Me.state
				Case RetroArcadeCaterpillarSpider.State.Entering
					Me.state = RetroArcadeCaterpillarSpider.State.ZigZagDown
				Case RetroArcadeCaterpillarSpider.State.ZigZagDown, RetroArcadeCaterpillarSpider.State.ZigZagUp
					If Me.numZigZags >= Me.properties.spiderNumZigZags Then
						Me.state = RetroArcadeCaterpillarSpider.State.Leaving
					Else
						Me.state = If((Me.state <> RetroArcadeCaterpillarSpider.State.ZigZagUp), RetroArcadeCaterpillarSpider.State.ZigZagUp, RetroArcadeCaterpillarSpider.State.ZigZagDown)
						Me.numZigZags += 1
					End If
				Case RetroArcadeCaterpillarSpider.State.Leaving
					Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
					Return
			End Select
			Dim state As RetroArcadeCaterpillarSpider.State = Me.state
			If state <> RetroArcadeCaterpillarSpider.State.ZigZagUp AndAlso state <> RetroArcadeCaterpillarSpider.State.ZigZagDown Then
				If state = RetroArcadeCaterpillarSpider.State.Leaving Then
					Me.targetPos.y = 300F
				End If
			Else
				Me.targetPos.x = CSng(If((Me.direction <> RetroArcadeCaterpillarSpider.Direction.Right), (-1), 1)) * Mathf.Lerp(-320F, 320F, CSng(Me.numZigZags) / CSng(Me.properties.spiderNumZigZags))
				Me.targetPos.y = If((Me.state <> RetroArcadeCaterpillarSpider.State.ZigZagUp), Me.properties.spiderPathY.min, Me.properties.spiderPathY.max)
			End If
			Me.move(num - magnitude)
		End If
	End Sub

	' Token: 0x06002879 RID: 10361 RVA: 0x001794CC File Offset: 0x001778CC
	Private Sub move(distance As Single)
		MyBase.transform.position = MyBase.transform.position + (Me.targetPos - MyBase.transform.position).normalized * distance
	End Sub

	' Token: 0x0600287A RID: 10362 RVA: 0x00179527 File Offset: 0x00177927
	Public Overrides Sub Dead()
		MyBase.Dead()
		MyBase.StartCoroutine(Me.moveOffscreen_cr())
	End Sub

	' Token: 0x0600287B RID: 10363 RVA: 0x0017953C File Offset: 0x0017793C
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(300F - MyBase.transform.position.y, 500F)
		While Me.movingY
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003140 RID: 12608
	Private Const OFFSCREEN_Y As Single = 300F

	' Token: 0x04003141 RID: 12609
	Private Const MOVE_OFFSCREEN_SPEED As Single = 500F

	' Token: 0x04003142 RID: 12610
	Public Const MAX_X As Single = 320F

	' Token: 0x04003143 RID: 12611
	Private properties As LevelProperties.RetroArcade.Caterpillar

	' Token: 0x04003144 RID: 12612
	Private direction As RetroArcadeCaterpillarSpider.Direction

	' Token: 0x04003145 RID: 12613
	Private targetPos As Vector2

	' Token: 0x04003146 RID: 12614
	Private state As RetroArcadeCaterpillarSpider.State

	' Token: 0x04003147 RID: 12615
	Private numZigZags As Integer

	' Token: 0x02000741 RID: 1857
	Public Enum Direction
		' Token: 0x04003149 RID: 12617
		Left
		' Token: 0x0400314A RID: 12618
		Right
	End Enum

	' Token: 0x02000742 RID: 1858
	Public Enum State
		' Token: 0x0400314C RID: 12620
		Entering
		' Token: 0x0400314D RID: 12621
		ZigZagDown
		' Token: 0x0400314E RID: 12622
		ZigZagUp
		' Token: 0x0400314F RID: 12623
		Leaving
	End Enum
End Class
