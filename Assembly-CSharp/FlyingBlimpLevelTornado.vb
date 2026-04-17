Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000644 RID: 1604
Public Class FlyingBlimpLevelTornado
	Inherits AbstractCollidableObject

	' Token: 0x17000389 RID: 905
	' (get) Token: 0x060020EC RID: 8428 RVA: 0x00130329 File Offset: 0x0012E729
	' (set) Token: 0x060020ED RID: 8429 RVA: 0x00130331 File Offset: 0x0012E731
	Public Property state As FlyingBlimpLevelTornado.State

	' Token: 0x060020EE RID: 8430 RVA: 0x0013033A File Offset: 0x0012E73A
	Public Sub Init(pos As Vector2, player As AbstractPlayerController, properties As LevelProperties.FlyingBlimp.Tornado)
		MyBase.transform.position = pos
		Me.player = player
		Me.properties = properties
	End Sub

	' Token: 0x060020EF RID: 8431 RVA: 0x0013035B File Offset: 0x0012E75B
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.state = FlyingBlimpLevelTornado.State.Alive
	End Sub

	' Token: 0x060020F0 RID: 8432 RVA: 0x0013036F File Offset: 0x0012E76F
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060020F1 RID: 8433 RVA: 0x00130390 File Offset: 0x0012E790
	Public Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim t As Single = 0F
		While MyBase.transform.position.x >= -1280F
			t += CupheadTime.FixedDelta
			Dim homingDirection As Vector2 = Me.player.transform.position - MyBase.transform.position
			Dim homingVelocity As Vector2 = homingDirection * Me.properties.homingSpeed
			Dim velocity As Single = homingVelocity.y
			If MyBase.transform.position.x > Me.player.transform.position.x Then
				velocity = Mathf.Lerp(Me.properties.moveSpeed, homingVelocity.y, 1F)
			ElseIf Me.state <> FlyingBlimpLevelTornado.State.Dead Then
				velocity = homingVelocity.y
			Else
				velocity = 0F
			End If
			MyBase.transform.AddPosition(-Me.properties.moveSpeed * CupheadTime.FixedDelta, velocity * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x060020F2 RID: 8434 RVA: 0x001303AB File Offset: 0x0012E7AB
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002984 RID: 10628
	Private properties As LevelProperties.FlyingBlimp.Tornado

	' Token: 0x04002985 RID: 10629
	Private player As AbstractPlayerController

	' Token: 0x04002986 RID: 10630
	Private movementSpeed As Single

	' Token: 0x04002987 RID: 10631
	Private damageDealer As DamageDealer

	' Token: 0x02000645 RID: 1605
	Public Enum State
		' Token: 0x04002989 RID: 10633
		Alive
		' Token: 0x0400298A RID: 10634
		Dead
	End Enum
End Class
