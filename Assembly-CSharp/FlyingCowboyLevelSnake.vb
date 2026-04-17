Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200065E RID: 1630
Public Class FlyingCowboyLevelSnake
	Inherits AbstractProjectile

	' Token: 0x060021F1 RID: 8689 RVA: 0x0013C37A File Offset: 0x0013A77A
	Public Sub Move(position As Vector3, speedX As Single, speedY As Single, stopPosX As Single, gravity As Single, properties As LevelProperties.FlyingCowboy.SnakeAttack)
		MyBase.transform.position = position
		Me.properties = properties
		Me.speed = New Vector3(speedX, speedY)
		Me.stopPosX = stopPosX
		Me.gravity = gravity
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060021F2 RID: 8690 RVA: 0x0013C3BA File Offset: 0x0013A7BA
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060021F3 RID: 8691 RVA: 0x0013C3D8 File Offset: 0x0013A7D8
	Private Iterator Function move_cr() As IEnumerator
		While MyBase.transform.position.x < Me.stopPosX
			Me.speed += New Vector3(Me.gravity * CupheadTime.FixedDelta, 0F)
			MyBase.transform.Translate(Me.speed * CupheadTime.FixedDelta)
			Yield New WaitForFixedUpdate()
		End While
		Dim snake As BasicProjectile = Me.snakeLine.Create(MyBase.transform.position, 0F, -Me.properties.snakeSpeed)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04002AA7 RID: 10919
	<SerializeField()>
	Private snakeLine As BasicProjectile

	' Token: 0x04002AA8 RID: 10920
	Private properties As LevelProperties.FlyingCowboy.SnakeAttack

	' Token: 0x04002AA9 RID: 10921
	Private speed As Vector3

	' Token: 0x04002AAA RID: 10922
	Private gravity As Single

	' Token: 0x04002AAB RID: 10923
	Private stopPosX As Single
End Class
