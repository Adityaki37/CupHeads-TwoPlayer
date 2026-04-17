Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000755 RID: 1877
Public Class RetroArcadeSheriff
	Inherits RetroArcadeEnemy

	' Token: 0x060028E8 RID: 10472 RVA: 0x0017CDC8 File Offset: 0x0017B1C8
	Public Function Create(pos As Vector3, speed As Single, clockwise As Boolean, offset As Single, properties As LevelProperties.RetroArcade.Sheriff) As RetroArcadeSheriff
		Dim retroArcadeSheriff As RetroArcadeSheriff = Me.InstantiatePrefab(Of RetroArcadeSheriff)()
		retroArcadeSheriff.transform.position = pos
		retroArcadeSheriff.properties = properties
		retroArcadeSheriff.speed = speed
		retroArcadeSheriff.clockwise = clockwise
		retroArcadeSheriff.offset = offset
		Return retroArcadeSheriff
	End Function

	' Token: 0x060028E9 RID: 10473 RVA: 0x0017CE07 File Offset: 0x0017B207
	Protected Overrides Sub Start()
		Me.side = RetroArcadeSheriff.Side.Right
		Me.SelectDirection()
	End Sub

	' Token: 0x060028EA RID: 10474 RVA: 0x0017CE18 File Offset: 0x0017B218
	Private Sub SelectDirection()
		If MyBase.transform.position.y > 200F Then
			Me.side = RetroArcadeSheriff.Side.Top
			Me.direction = If((Not Me.clockwise), Vector3.left, Vector3.right)
		ElseIf MyBase.transform.position.y < -100F Then
			Me.side = RetroArcadeSheriff.Side.Bottom
			Me.direction = If((Not Me.clockwise), Vector3.right, Vector3.left)
		ElseIf MyBase.transform.position.x < 0F Then
			Me.side = RetroArcadeSheriff.Side.Left
			Me.direction = If((Not Me.clockwise), Vector3.down, Vector3.up)
		Else
			Me.side = RetroArcadeSheriff.Side.Right
			Me.direction = If((Not Me.clockwise), Vector3.up, Vector3.down)
		End If
	End Sub

	' Token: 0x060028EB RID: 10475 RVA: 0x0017CF27 File Offset: 0x0017B327
	Public Sub StartMoving()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060028EC RID: 10476 RVA: 0x0017CF38 File Offset: 0x0017B338
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			Select Case Me.side
				Case RetroArcadeSheriff.Side.Top
					If Me.clockwise AndAlso MyBase.transform.position.x >= CSng(Level.Current.Right) - Me.offset Then
						Me.side = RetroArcadeSheriff.Side.Right
						Me.direction = Vector3.down
					ElseIf Not Me.clockwise AndAlso MyBase.transform.position.x <= CSng(Level.Current.Left) + Me.offset Then
						Me.side = RetroArcadeSheriff.Side.Left
						Me.direction = Vector3.down
					End If
				Case RetroArcadeSheriff.Side.Bottom
					If Me.clockwise AndAlso MyBase.transform.position.x <= CSng(Level.Current.Left) + Me.offset Then
						Me.side = RetroArcadeSheriff.Side.Left
						Me.direction = Vector3.up
					ElseIf Not Me.clockwise AndAlso MyBase.transform.position.x >= CSng(Level.Current.Right) - Me.offset Then
						Me.side = RetroArcadeSheriff.Side.Right
						Me.direction = Vector3.up
					End If
				Case RetroArcadeSheriff.Side.Left
					If Me.clockwise AndAlso MyBase.transform.position.y >= CSng(Level.Current.Ceiling) - Me.offset Then
						Me.side = RetroArcadeSheriff.Side.Top
						Me.direction = Vector3.right
					ElseIf Not Me.clockwise AndAlso MyBase.transform.position.y <= CSng(Level.Current.Ground) + Me.offset Then
						Me.side = RetroArcadeSheriff.Side.Bottom
						Me.direction = Vector3.right
					End If
				Case RetroArcadeSheriff.Side.Right
					If Not Me.clockwise AndAlso MyBase.transform.position.y >= CSng(Level.Current.Ceiling) - Me.offset Then
						Me.side = RetroArcadeSheriff.Side.Top
						Me.direction = Vector3.left
					ElseIf Me.clockwise AndAlso MyBase.transform.position.y <= CSng(Level.Current.Ground) + Me.offset Then
						Me.side = RetroArcadeSheriff.Side.Bottom
						Me.direction = Vector3.left
					End If
			End Select
			Dim pos As Vector3 = MyBase.transform.position
			pos.x = Mathf.Clamp(MyBase.transform.position.x, CSng(Level.Current.Left) + Me.offset, CSng(Level.Current.Right) - Me.offset)
			pos.y = Mathf.Clamp(MyBase.transform.position.y, CSng(Level.Current.Ground) + Me.offset, CSng(Level.Current.Ceiling) - Me.offset)
			MyBase.transform.position = pos
			MyBase.transform.position += Me.direction * Me.speed * CupheadTime.FixedDelta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060028ED RID: 10477 RVA: 0x0017CF54 File Offset: 0x0017B354
	Public Sub Shoot(player As AbstractPlayerController)
		Dim vector As Vector3 = player.transform.position - MyBase.transform.position
		Me.projectile.Create(MyBase.transform.position, MathUtils.DirectionToAngle(vector), Me.properties.shotSpeed)
	End Sub

	' Token: 0x040031C5 RID: 12741
	Public speed As Single

	' Token: 0x040031C6 RID: 12742
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x040031C7 RID: 12743
	Private Const Y_TOP_THRESHOLD As Single = 200F

	' Token: 0x040031C8 RID: 12744
	Private Const Y_BOTTOM_THRESHOLD As Single = -100F

	' Token: 0x040031C9 RID: 12745
	Private Const X_THRESHOLD As Single = 290F

	' Token: 0x040031CA RID: 12746
	Public side As RetroArcadeSheriff.Side

	' Token: 0x040031CB RID: 12747
	Private properties As LevelProperties.RetroArcade.Sheriff

	' Token: 0x040031CC RID: 12748
	Private offset As Single

	' Token: 0x040031CD RID: 12749
	Private clockwise As Boolean

	' Token: 0x040031CE RID: 12750
	Private direction As Vector3

	' Token: 0x040031CF RID: 12751
	Private targetPos As Vector2

	' Token: 0x02000756 RID: 1878
	Public Enum Side
		' Token: 0x040031D1 RID: 12753
		Top
		' Token: 0x040031D2 RID: 12754
		Bottom
		' Token: 0x040031D3 RID: 12755
		Left
		' Token: 0x040031D4 RID: 12756
		Right
	End Enum
End Class
