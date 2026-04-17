Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006A4 RID: 1700
Public Class FlyingMermaidLevelYellProjectile
	Inherits AbstractProjectile

	' Token: 0x0600240B RID: 9227 RVA: 0x0015279C File Offset: 0x00150B9C
	Public Function Create(pos As Vector2, trackSpeed As Single, angle As Single, target As AbstractPlayerController) As FlyingMermaidLevelYellProjectile
		Dim flyingMermaidLevelYellProjectile As FlyingMermaidLevelYellProjectile = TryCast(MyBase.Create(), FlyingMermaidLevelYellProjectile)
		flyingMermaidLevelYellProjectile.trackSpeed = trackSpeed
		flyingMermaidLevelYellProjectile.target = target
		flyingMermaidLevelYellProjectile.direction = MathUtils.AngleToDirection(angle)
		flyingMermaidLevelYellProjectile.transform.position = pos
		Return flyingMermaidLevelYellProjectile
	End Function

	' Token: 0x0600240C RID: 9228 RVA: 0x001527E2 File Offset: 0x00150BE2
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x0600240D RID: 9229 RVA: 0x001527F7 File Offset: 0x00150BF7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600240E RID: 9230 RVA: 0x00152818 File Offset: 0x00150C18
	Private Iterator Function move_cr() As IEnumerator
		Dim speed As Single = Me.launchSpeed
		Dim t As Single = 0F
		While True
			t += CupheadTime.FixedDelta
			Dim state As FlyingMermaidLevelYellProjectile.State = Me.state
			If state <> FlyingMermaidLevelYellProjectile.State.Slowing Then
				If state <> FlyingMermaidLevelYellProjectile.State.Stopped Then
					If state = FlyingMermaidLevelYellProjectile.State.Tracking Then
						If t < Me.attackEaseTime Then
							speed = EaseUtils.EaseInSine(0F, Me.trackSpeed, t / Me.attackEaseTime)
						Else
							speed = Me.trackSpeed
						End If
					End If
				ElseIf t >= Me.waitTime Then
					Me.state = FlyingMermaidLevelYellProjectile.State.Tracking
					t = 0F
					If Me.target Is Nothing OrElse Me.target.IsDead Then
						Me.target = PlayerManager.GetNext()
					End If
					If Me.target IsNot Nothing Then
						Me.direction = (Me.target.center - MyBase.transform.position).normalized
						MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(MathUtils.DirectionToAngle(Me.direction) + 180F))
						MyBase.animator.SetTrigger("Continue")
					End If
				End If
			ElseIf t < Me.stopTime Then
				speed = EaseUtils.EaseOutSine(Me.launchSpeed, 0F, t / Me.stopTime)
			Else
				speed = 0F
				Me.state = FlyingMermaidLevelYellProjectile.State.Stopped
				t = 0F
			End If
			Dim pos As Vector2 = MyBase.transform.localPosition
			pos += speed * CupheadTime.FixedDelta * Me.direction
			MyBase.transform.localPosition = pos
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x04002CD6 RID: 11478
	Public launchSpeed As Single

	' Token: 0x04002CD7 RID: 11479
	Public stopTime As Single

	' Token: 0x04002CD8 RID: 11480
	Public waitTime As Single

	' Token: 0x04002CD9 RID: 11481
	Public attackEaseTime As Single

	' Token: 0x04002CDA RID: 11482
	Private state As FlyingMermaidLevelYellProjectile.State

	' Token: 0x04002CDB RID: 11483
	Private trackSpeed As Single

	' Token: 0x04002CDC RID: 11484
	Private target As AbstractPlayerController

	' Token: 0x04002CDD RID: 11485
	Private direction As Vector2

	' Token: 0x020006A5 RID: 1701
	Public Enum State
		' Token: 0x04002CDF RID: 11487
		Slowing
		' Token: 0x04002CE0 RID: 11488
		Stopped
		' Token: 0x04002CE1 RID: 11489
		Tracking
	End Enum
End Class
