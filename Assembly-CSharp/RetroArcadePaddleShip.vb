Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000747 RID: 1863
Public Class RetroArcadePaddleShip
	Inherits RetroArcadeEnemy

	' Token: 0x06002897 RID: 10391 RVA: 0x0017AAD8 File Offset: 0x00178ED8
	Public Sub LevelInit(properties As LevelProperties.RetroArcade)
		Me.properties = properties
	End Sub

	' Token: 0x06002898 RID: 10392 RVA: 0x0017AAE4 File Offset: 0x00178EE4
	Public Sub StartPaddleShip()
		MyBase.gameObject.SetActive(True)
		Me.p = Me.properties.CurrentState.paddleShip
		Me.ySpeed = Me.p.ySpeed.RandomFloat()
		MyBase.PointsBonus = Me.p.pointsBonus
		MyBase.PointsWorth = Me.p.pointsGained
		MyBase.transform.SetPosition(New Single?(Global.UnityEngine.Random.Range(-300F, 300F)), New Single?(350F), Nothing)
		Me.moveDir = New Trilean2(If((Not Rand.Bool()), 1, (-1)), -1)
		Me.hp = Me.p.hp
		MyBase.StartCoroutine(Me.moveY_cr())
		MyBase.StartCoroutine(Me.moveX_cr())
	End Sub

	' Token: 0x06002899 RID: 10393 RVA: 0x0017ABC8 File Offset: 0x00178FC8
	Private Iterator Function moveY_cr() As IEnumerator
		While MyBase.transform.position.y > CSng(Level.Current.Ceiling) - 80F
			Yield New WaitForFixedUpdate()
			MyBase.transform.AddPosition(0F, -Me.ySpeed * CupheadTime.FixedDelta, 0F)
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(CSng(Level.Current.Ceiling) - 80F), Nothing)
		While Me.paddle.position.y > CSng(Level.Current.Ground) + 20F
			Yield New WaitForFixedUpdate()
			Me.paddle.AddPosition(0F, -Me.ySpeed * CupheadTime.FixedDelta, 0F)
		End While
		Me.paddle.SetPosition(Nothing, New Single?(CSng(Level.Current.Ground) + 20F), Nothing)
		While True
			Yield New WaitForFixedUpdate()
			If(Me.moveDir.y > 0 AndAlso MyBase.transform.position.y > CSng(Level.Current.Ceiling) - 80F) OrElse (Me.moveDir.y < 0 AndAlso MyBase.transform.position.y < CSng(Level.Current.Ground) + 80F) Then
				Me.moveDir.y = Me.moveDir.y * -1
			End If
			MyBase.transform.AddPosition(0F, Me.moveDir.y * Me.ySpeed * CupheadTime.FixedDelta, 0F)
			Me.paddle.SetPosition(Nothing, New Single?(CSng(Level.Current.Ground) + 20F), Nothing)
		End While
		Return
	End Function

	' Token: 0x0600289A RID: 10394 RVA: 0x0017ABE4 File Offset: 0x00178FE4
	Private Iterator Function moveX_cr() As IEnumerator
		While True
			Yield New WaitForFixedUpdate()
			If(Me.moveDir.x > 0 AndAlso MyBase.transform.position.x > 300F) OrElse (Me.moveDir.x < 0 AndAlso MyBase.transform.position.x < -300F) Then
				Me.moveDir.x = Me.moveDir.x * -1
			End If
			MyBase.transform.AddPosition(Me.moveDir.x * Me.p.xSpeed * CupheadTime.FixedDelta, 0F, 0F)
		End While
		Return
	End Function

	' Token: 0x0600289B RID: 10395 RVA: 0x0017AC00 File Offset: 0x00179000
	Public Overrides Sub Dead()
		Me.StopAllCoroutines()
		For Each collider2D As Collider2D In MyBase.GetComponentsInChildren(Of Collider2D)()
			collider2D.enabled = False
		Next
		MyBase.IsDead = True
		For Each spriteRenderer As SpriteRenderer In MyBase.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.color = New Color(0F, 0F, 0F, 0.25F)
		Next
		Me.properties.DealDamageToNextNamedState()
		MyBase.StartCoroutine(Me.moveOffscreen_cr())
	End Sub

	' Token: 0x0600289C RID: 10396 RVA: 0x0017ACA0 File Offset: 0x001790A0
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(350F - (CSng(Level.Current.Ground) + 20F), 500F)
		While Me.movingY
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x0400316C RID: 12652
	Private Const OFFSCREEN_Y As Single = 350F

	' Token: 0x0400316D RID: 12653
	Private Const TURNAROUND_X As Single = 300F

	' Token: 0x0400316E RID: 12654
	Private Const PADDLE_PADDING As Single = 20F

	' Token: 0x0400316F RID: 12655
	Private Const SHIP_PADDING As Single = 80F

	' Token: 0x04003170 RID: 12656
	Private Const MOVE_OFFSCREEN_SPEED As Single = 500F

	' Token: 0x04003171 RID: 12657
	<SerializeField()>
	Private paddle As Transform

	' Token: 0x04003172 RID: 12658
	Private properties As LevelProperties.RetroArcade

	' Token: 0x04003173 RID: 12659
	Private p As LevelProperties.RetroArcade.PaddleShip

	' Token: 0x04003174 RID: 12660
	Private ySpeed As Single

	' Token: 0x04003175 RID: 12661
	Private moveDir As Trilean2
End Class
