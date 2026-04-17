Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007D8 RID: 2008
Public Class SaltbakerLevelSugarcube
	Inherits SaltbakerLevelPhaseOneProjectile

	' Token: 0x06002DD8 RID: 11736 RVA: 0x001B0AAF File Offset: 0x001AEEAF
	Protected Overrides Sub OnDieDistance()
	End Sub

	' Token: 0x06002DD9 RID: 11737 RVA: 0x001B0AB1 File Offset: 0x001AEEB1
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x06002DDA RID: 11738 RVA: 0x001B0AB4 File Offset: 0x001AEEB4
	Public Overridable Function Init(pos As Vector2, onLeft As Boolean, properties As LevelProperties.Saltbaker.Sugarcubes, phase As Single, parent As SaltbakerLevelSaltbaker, anim As Integer, parryable As Boolean) As SaltbakerLevelSugarcube
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Me.root = pos
		MyBase.transform.position = pos
		Me.properties = properties
		Me.onLeft = onLeft
		Me.Move()
		Me.phase = phase * 0.017453292F
		MyBase.animator.Play(If((Not parryable), anim.ToString(), "Pink"))
		Me.SetParryable(parryable)
		Return Me
	End Function

	' Token: 0x06002DDB RID: 11739 RVA: 0x001B0B3E File Offset: 0x001AEF3E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002DDC RID: 11740 RVA: 0x001B0B5C File Offset: 0x001AEF5C
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002DDD RID: 11741 RVA: 0x001B0B6C File Offset: 0x001AEF6C
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim xVelocity As Single = Me.properties.sineFreq * Me.properties.sineWavelength
		xVelocity = If((Not Me.onLeft), (-xVelocity), xVelocity)
		MyBase.transform.localScale = New Vector3(Mathf.Sign(xVelocity), 1F)
		Dim t As Single = 0F
		Dim ismoving As Boolean = True
		While ismoving
			t += CupheadTime.FixedDelta
			Dim pos As Vector3 = MyBase.transform.position
			Dim yAbsolute As Single = Me.properties.sineAmplitude * Mathf.Sin(Me.properties.sineFreq * t + Me.phase)
			pos.y = Me.root.y + yAbsolute
			pos.x += xVelocity * CupheadTime.FixedDelta
			MyBase.transform.position = pos
			MyBase.HandleShadow(50F, 0F)
			If(Me.onLeft AndAlso MyBase.transform.position.x > CSng(Level.Current.Right) + 50F) OrElse (Not Me.onLeft AndAlso MyBase.transform.position.x < CSng(Level.Current.Left) - 50F) Then
				ismoving = False
			End If
			Yield wait
		End While
		Me.Death()
		Yield Nothing
		Return
	End Function

	' Token: 0x06002DDE RID: 11742 RVA: 0x001B0B87 File Offset: 0x001AEF87
	Private Sub Death()
		Me.Recycle()
	End Sub

	' Token: 0x0400365E RID: 13918
	Private properties As LevelProperties.Saltbaker.Sugarcubes

	' Token: 0x0400365F RID: 13919
	Private root As Vector3

	' Token: 0x04003660 RID: 13920
	Private onLeft As Boolean

	' Token: 0x04003661 RID: 13921
	Private phase As Single
End Class
