Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200074A RID: 1866
Public Class RetroArcadeQShipTentacle
	Inherits RetroArcadeEnemy

	' Token: 0x060028AD RID: 10413 RVA: 0x0017BAB0 File Offset: 0x00179EB0
	Public Function Create(direction As RetroArcadeQShipTentacle.Direction, properties As LevelProperties.RetroArcade.QShip) As RetroArcadeQShipTentacle
		Dim retroArcadeQShipTentacle As RetroArcadeQShipTentacle = Me.InstantiatePrefab(Of RetroArcadeQShipTentacle)()
		retroArcadeQShipTentacle.transform.position = New Vector2(If((direction <> RetroArcadeQShipTentacle.Direction.Left), (-400F), 400F), -140F)
		retroArcadeQShipTentacle.properties = properties
		retroArcadeQShipTentacle.direction = direction
		retroArcadeQShipTentacle.hp = 1F
		retroArcadeQShipTentacle.transform.SetScale(New Single?(CSng(If((direction <> RetroArcadeQShipTentacle.Direction.Right), (-1), 1))), New Single?(1F), New Single?(1F))
		Return retroArcadeQShipTentacle
	End Function

	' Token: 0x060028AE RID: 10414 RVA: 0x0017BB40 File Offset: 0x00179F40
	Protected Overrides Sub FixedUpdate()
		MyBase.transform.AddPosition(CSng(If((Me.direction <> RetroArcadeQShipTentacle.Direction.Right), (-1), 1)) * Me.properties.tentacleSpeed * CupheadTime.FixedDelta, 0F, 0F)
		If If((Me.direction <> RetroArcadeQShipTentacle.Direction.Left), (MyBase.transform.position.x > 400F), (MyBase.transform.position.x < -400F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060028AF RID: 10415 RVA: 0x0017BBDC File Offset: 0x00179FDC
	Public Overrides Sub Dead()
		Me.StopAllCoroutines()
		For Each collider2D As Collider2D In MyBase.GetComponentsInChildren(Of Collider2D)()
			collider2D.enabled = False
		Next
		MyBase.IsDead = True
		For Each spriteRenderer As SpriteRenderer In MyBase.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.color = New Color(0F, 0F, 0F, 0.25F)
		Next
		MyBase.StartCoroutine(Me.moveOffscreen_cr())
	End Sub

	' Token: 0x060028B0 RID: 10416 RVA: 0x0017BC70 File Offset: 0x0017A070
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(-250F - MyBase.transform.position.y, 500F)
		While Me.movingY
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04003183 RID: 12675
	Private Const SPAWN_X As Single = 400F

	' Token: 0x04003184 RID: 12676
	Private Const SPAWN_Y As Single = -140F

	' Token: 0x04003185 RID: 12677
	Private Const OFFSCREEN_Y As Single = -250F

	' Token: 0x04003186 RID: 12678
	Private Const MOVE_Y_SPEED As Single = 500F

	' Token: 0x04003187 RID: 12679
	Private properties As LevelProperties.RetroArcade.QShip

	' Token: 0x04003188 RID: 12680
	Private direction As RetroArcadeQShipTentacle.Direction

	' Token: 0x0200074B RID: 1867
	Public Enum Direction
		' Token: 0x0400318A RID: 12682
		Left
		' Token: 0x0400318B RID: 12683
		Right
	End Enum
End Class
