Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000765 RID: 1893
Public Class RetroArcadeUFOMole
	Inherits RetroArcadeEnemy

	' Token: 0x06002941 RID: 10561 RVA: 0x00180A04 File Offset: 0x0017EE04
	Public Function Create(properties As LevelProperties.RetroArcade.UFO) As RetroArcadeUFOMole
		Dim retroArcadeUFOMole As RetroArcadeUFOMole = Me.InstantiatePrefab(Of RetroArcadeUFOMole)()
		retroArcadeUFOMole.properties = properties
		retroArcadeUFOMole.direction = If((Not Rand.Bool()), RetroArcadeUFOMole.Direction.Right, RetroArcadeUFOMole.Direction.Left)
		retroArcadeUFOMole.hp = properties.hp
		retroArcadeUFOMole.StartCoroutine(retroArcadeUFOMole.main_cr())
		Return retroArcadeUFOMole
	End Function

	' Token: 0x06002942 RID: 10562 RVA: 0x00180A50 File Offset: 0x0017EE50
	Private Iterator Function main_cr() As IEnumerator
		MyBase.transform.SetPosition(New Single?(Global.UnityEngine.Random.Range(-200F, 200F)), New Single?(-167F), Nothing)
		Me.direction = If((Not Rand.Bool()), RetroArcadeUFOMole.Direction.Right, RetroArcadeUFOMole.Direction.Left)
		Dim sprite As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		Dim col As Collider2D = MyBase.GetComponent(Of Collider2D)()
		sprite.sortingOrder = 90
		col.enabled = False
		MyBase.MoveY(-66F - MyBase.transform.position.y, Me.properties.moleAttackSpeed)
		While Me.movingY
			Yield New WaitForFixedUpdate()
		End While
		Dim leftOfPlayer As Boolean() = New Boolean(1) {}
		Dim firstCheck As Boolean = True
		While True
			Dim shouldAttack As Boolean = False
			While Not shouldAttack
				MyBase.transform.AddPosition(CSng(If((Me.direction <> RetroArcadeUFOMole.Direction.Left), 1, (-1))) * Me.properties.moleSpeed * CupheadTime.FixedDelta, 0F, 0F)
				If(Me.direction = RetroArcadeUFOMole.Direction.Left AndAlso MyBase.transform.position.x < -200F) OrElse (Me.direction = RetroArcadeUFOMole.Direction.Right AndAlso MyBase.transform.position.x > 200F) Then
					Me.direction = If((Me.direction <> RetroArcadeUFOMole.Direction.Left), RetroArcadeUFOMole.Direction.Left, RetroArcadeUFOMole.Direction.Right)
				End If
				For i As Integer = 0 To 2 - 1
					Dim arcadePlayerController As ArcadePlayerController = TryCast(If((i <> 0), PlayerManager.GetPlayer(PlayerId.PlayerTwo), PlayerManager.GetPlayer(PlayerId.PlayerOne)), ArcadePlayerController)
					If Not(arcadePlayerController Is Nothing) Then
						Dim flag As Boolean = leftOfPlayer(i)
						leftOfPlayer(i) = arcadePlayerController.center.x < MyBase.transform.position.x
						If leftOfPlayer(i) <> flag AndAlso Mathf.Abs(MyBase.transform.position.x) < 150F AndAlso arcadePlayerController.motor.Grounded AndAlso Not firstCheck Then
							shouldAttack = True
						End If
					End If
				Next
				firstCheck = False
				Yield New WaitForFixedUpdate()
			End While
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.moleWarningDelay)
			MyBase.MoveY(-167F - MyBase.transform.position.y, Me.properties.moleAttackSpeed)
			While Me.movingY
				Yield New WaitForFixedUpdate()
			End While
			sprite.sortingOrder = 200
			col.enabled = True
			MyBase.MoveY(-114F - MyBase.transform.position.y, Me.properties.moleAttackSpeed)
			While Me.movingY
				Yield New WaitForFixedUpdate()
			End While
			MyBase.MoveY(-167F - MyBase.transform.position.y, Me.properties.moleAttackSpeed)
			While Me.movingY
				Yield New WaitForFixedUpdate()
			End While
			sprite.sortingOrder = 90
			col.enabled = False
			MyBase.MoveY(-66F - MyBase.transform.position.y, Me.properties.moleAttackSpeed)
			While Me.movingY
				Yield New WaitForFixedUpdate()
			End While
			firstCheck = True
		End While
		Return
	End Function

	' Token: 0x06002943 RID: 10563 RVA: 0x00180A6B File Offset: 0x0017EE6B
	Public Sub OnWaveEnd()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.moveOffscreen_cr())
	End Sub

	' Token: 0x06002944 RID: 10564 RVA: 0x00180A80 File Offset: 0x0017EE80
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(-167F - MyBase.transform.position.y, Me.properties.moleAttackSpeed)
		While Me.movingY
			Yield New WaitForFixedUpdate()
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003234 RID: 12852
	Private Const BACKGROUND_Y As Single = -66F

	' Token: 0x04003235 RID: 12853
	Private Const UNDERGROUND_Y As Single = -167F

	' Token: 0x04003236 RID: 12854
	Private Const POPUP_Y As Single = -114F

	' Token: 0x04003237 RID: 12855
	Private Const TURNAROUND_X As Single = 200F

	' Token: 0x04003238 RID: 12856
	Private Const MAX_ATTACK_X As Single = 150F

	' Token: 0x04003239 RID: 12857
	Private Const BACKGROUND_SORT_ORDER As Integer = 90

	' Token: 0x0400323A RID: 12858
	Private Const ATTACK_SORT_ORDER As Integer = 200

	' Token: 0x0400323B RID: 12859
	Private direction As RetroArcadeUFOMole.Direction

	' Token: 0x0400323C RID: 12860
	Private properties As LevelProperties.RetroArcade.UFO

	' Token: 0x02000766 RID: 1894
	Private Enum Direction
		' Token: 0x0400323E RID: 12862
		Left
		' Token: 0x0400323F RID: 12863
		Right
	End Enum
End Class
