Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200073D RID: 1853
Public Class RetroArcadeCaterpillarBodyPart
	Inherits RetroArcadeEnemy

	' Token: 0x06002868 RID: 10344 RVA: 0x00178BE8 File Offset: 0x00176FE8
	Public Function Create(index As Integer, direction As RetroArcadeCaterpillarBodyPart.Direction, manager As RetroArcadeCaterpillarManager, properties As LevelProperties.RetroArcade.Caterpillar) As RetroArcadeCaterpillarBodyPart
		Dim retroArcadeCaterpillarBodyPart As RetroArcadeCaterpillarBodyPart = Me.InstantiatePrefab(Of RetroArcadeCaterpillarBodyPart)()
		retroArcadeCaterpillarBodyPart.transform.SetPosition(New Single?(If((direction <> RetroArcadeCaterpillarBodyPart.Direction.Right), 320F, (-320F))), New Single?(300F + CSng(index) * 50F), Nothing)
		retroArcadeCaterpillarBodyPart.direction = direction
		retroArcadeCaterpillarBodyPart.manager = manager
		retroArcadeCaterpillarBodyPart.properties = properties
		retroArcadeCaterpillarBodyPart.manager = manager
		retroArcadeCaterpillarBodyPart.targetPos = New Vector2(retroArcadeCaterpillarBodyPart.transform.position.x, 230F)
		retroArcadeCaterpillarBodyPart.moveY = True
		retroArcadeCaterpillarBodyPart.hp = properties.hp
		Return retroArcadeCaterpillarBodyPart
	End Function

	' Token: 0x06002869 RID: 10345 RVA: 0x00178C93 File Offset: 0x00177093
	Protected Overrides Sub Start()
		MyBase.PointsWorth = Me.properties.pointsGained
		MyBase.PointsBonus = Me.properties.pointsBonus
	End Sub

	' Token: 0x0600286A RID: 10346 RVA: 0x00178CB8 File Offset: 0x001770B8
	Protected Overrides Sub FixedUpdate()
		If Me.movingY Then
			Return
		End If
		Dim num As Single = Me.manager.moveSpeed * CupheadTime.FixedDelta
		Dim magnitude As Single = (Me.targetPos - MyBase.transform.position).magnitude
		If magnitude > num Then
			Me.move(num)
		Else
			MyBase.transform.position = Me.targetPos
			If Me.moveY Then
				Me.moveY = False
				Me.targetPos = New Vector2(If((Me.direction <> RetroArcadeCaterpillarBodyPart.Direction.Left), 320F, (-320F)), MyBase.transform.position.y)
				If Me.atBottom AndAlso Me.isHead Then
					Me.manager.OnReachBottom()
				End If
			Else
				Me.moveY = True
				Me.direction = If((Me.direction <> RetroArcadeCaterpillarBodyPart.Direction.Left), RetroArcadeCaterpillarBodyPart.Direction.Left, RetroArcadeCaterpillarBodyPart.Direction.Right)
				If Me.atBottom Then
					Me.targetPos = New Vector2(MyBase.transform.position.x, 230F)
					Me.atBottom = False
				ElseIf Me.timesDropped >= Me.properties.dropCount Then
					Me.targetPos = New Vector2(MyBase.transform.position.x, -120F)
					Me.timesDropped = 0
					Me.atBottom = True
				Else
					Me.targetPos = New Vector2(MyBase.transform.position.x, MyBase.transform.position.y - 50F)
					Me.timesDropped += 1
					If Me.bulletPrefab IsNot Nothing Then
						Me.Shoot()
					End If
				End If
			End If
			Me.move(num - magnitude)
		End If
	End Sub

	' Token: 0x0600286B RID: 10347 RVA: 0x00178EB0 File Offset: 0x001772B0
	Private Sub move(distance As Single)
		MyBase.transform.position = MyBase.transform.position + (Me.targetPos - MyBase.transform.position).normalized * distance
	End Sub

	' Token: 0x0600286C RID: 10348 RVA: 0x00178F0B File Offset: 0x0017730B
	Public Overrides Sub Dead()
		MyBase.Dead()
		If Not Me.isHead Then
			Me.manager.OnBodyPartDie(Me)
		End If
	End Sub

	' Token: 0x0600286D RID: 10349 RVA: 0x00178F2C File Offset: 0x0017732C
	Public Sub Shoot()
		Dim num As Single = MathUtils.DirectionToAngle(PlayerManager.GetNext().transform.position - Me.bulletRoot.position)
		Me.bulletPrefab.Create(Me.bulletRoot.position, num, Me.properties.shotSpeed)
	End Sub

	' Token: 0x0600286E RID: 10350 RVA: 0x00178F8B File Offset: 0x0017738B
	Public Sub OnWaveEnd()
		MyBase.StartCoroutine(Me.moveOffscreen_cr())
	End Sub

	' Token: 0x0600286F RID: 10351 RVA: 0x00178F9C File Offset: 0x0017739C
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(420F, 500F)
		While Me.movingY
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003126 RID: 12582
	Private Const TOP_Y As Single = 230F

	' Token: 0x04003127 RID: 12583
	Private Const BOTTOM_Y As Single = -120F

	' Token: 0x04003128 RID: 12584
	Private Const SPACING As Single = 50F

	' Token: 0x04003129 RID: 12585
	Private Const OFFSCREEN_Y As Single = 300F

	' Token: 0x0400312A RID: 12586
	Private Const MOVE_OFFSCREEN_SPEED As Single = 500F

	' Token: 0x0400312B RID: 12587
	Public Const TURNAROUND_X As Single = 320F

	' Token: 0x0400312C RID: 12588
	<SerializeField()>
	Private bulletPrefab As BasicProjectile

	' Token: 0x0400312D RID: 12589
	<SerializeField()>
	Private bulletRoot As Transform

	' Token: 0x0400312E RID: 12590
	<SerializeField()>
	Private isHead As Boolean

	' Token: 0x0400312F RID: 12591
	Private properties As LevelProperties.RetroArcade.Caterpillar

	' Token: 0x04003130 RID: 12592
	Private manager As RetroArcadeCaterpillarManager

	' Token: 0x04003131 RID: 12593
	Private direction As RetroArcadeCaterpillarBodyPart.Direction

	' Token: 0x04003132 RID: 12594
	Private targetPos As Vector2

	' Token: 0x04003133 RID: 12595
	Private moveY As Boolean

	' Token: 0x04003134 RID: 12596
	Private timesDropped As Integer

	' Token: 0x04003135 RID: 12597
	Private atBottom As Boolean

	' Token: 0x0200073E RID: 1854
	Public Enum Direction
		' Token: 0x04003137 RID: 12599
		Left
		' Token: 0x04003138 RID: 12600
		Right
	End Enum
End Class
