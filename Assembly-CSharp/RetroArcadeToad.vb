Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200075E RID: 1886
Public Class RetroArcadeToad
	Inherits RetroArcadeEnemy

	' Token: 0x06002919 RID: 10521 RVA: 0x0017F4D0 File Offset: 0x0017D8D0
	Public Function Create(parent As RetroArcadeToadManager, properties As LevelProperties.RetroArcade.Toad, onLeft As Boolean) As RetroArcadeToad
		Dim retroArcadeToad As RetroArcadeToad = Me.InstantiatePrefab(Of RetroArcadeToad)()
		retroArcadeToad.transform.SetPosition(New Single?(If((Not onLeft), 330F, (-330F))), New Single?(200F), Nothing)
		retroArcadeToad.properties = properties
		retroArcadeToad.parent = parent
		retroArcadeToad.hp = properties.hp
		retroArcadeToad.onLeft = onLeft
		Return retroArcadeToad
	End Function

	' Token: 0x0600291A RID: 10522 RVA: 0x0017F53E File Offset: 0x0017D93E
	Protected Overrides Sub Start()
		MyBase.StartCoroutine(Me.jump_cr())
	End Sub

	' Token: 0x0600291B RID: 10523 RVA: 0x0017F550 File Offset: 0x0017D950
	Private Iterator Function jump_cr() As IEnumerator
		Dim speedY As Single = Me.properties.jumpVerticalSpeedRange.RandomFloat()
		Dim speedX As Single = Me.properties.jumpHorizontalSpeedRange.RandomFloat()
		Dim velocityX As Single = speedX
		Dim velocityY As Single = speedY
		Dim ground As Single = CSng(Level.Current.Ground) + 50F
		Dim jumping As Boolean = False
		Dim goingUp As Boolean = False
		Me.gravity = Me.properties.jumpGravity
		While MyBase.transform.position.y > ground
			velocityY -= Me.gravity * CupheadTime.Delta
			MyBase.transform.AddPosition(0F, velocityY * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Dim pos As Vector3 = MyBase.transform.position
		pos.y = ground
		MyBase.transform.position = pos
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.jumpDelay.RandomFloat())
			velocityY = speedY
			velocityX = If((Not Me.onLeft), (-speedX), speedX)
			jumping = True
			goingUp = True
			While jumping
				velocityY -= Me.gravity * CupheadTime.Delta
				MyBase.transform.AddPosition(velocityX * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0F)
				If velocityY < 0F AndAlso goingUp Then
					goingUp = False
				End If
				If velocityY < 0F AndAlso jumping AndAlso MyBase.transform.position.y <= ground Then
					jumping = False
					pos = MyBase.transform.position
					pos.y = ground
					MyBase.transform.position = pos
				End If
				If(MyBase.transform.position.x < -330F AndAlso Not Me.onLeft) OrElse (MyBase.transform.position.x > 330F AndAlso Me.onLeft) Then
					If Me.onLeft Then
						MyBase.transform.SetPosition(New Single?(330F), Nothing, Nothing)
						Me.onLeft = False
					Else
						MyBase.transform.SetPosition(New Single?(-330F), Nothing, Nothing)
						Me.onLeft = True
					End If
					velocityX = If((Not Me.onLeft), (-speedX), speedX)
				End If
				Yield Nothing
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600291C RID: 10524 RVA: 0x0017F56B File Offset: 0x0017D96B
	Public Overrides Sub Dead()
		MyBase.Dead()
		Me.parent.OnToadDie()
	End Sub

	' Token: 0x04003209 RID: 12809
	Private Const TOAD_MAX_X_POS As Single = 330F

	' Token: 0x0400320A RID: 12810
	Private Const OFFSET_Y As Single = 50F

	' Token: 0x0400320B RID: 12811
	Private Const OFFSCREEN_Y As Single = 200F

	' Token: 0x0400320C RID: 12812
	Private Const BASE_Y As Single = 250F

	' Token: 0x0400320D RID: 12813
	Private Const MOVE_Y_SPEED As Single = 500F

	' Token: 0x0400320E RID: 12814
	Private properties As LevelProperties.RetroArcade.Toad

	' Token: 0x0400320F RID: 12815
	Private parent As RetroArcadeToadManager

	' Token: 0x04003210 RID: 12816
	Private gravity As Single

	' Token: 0x04003211 RID: 12817
	Private onLeft As Boolean
End Class
