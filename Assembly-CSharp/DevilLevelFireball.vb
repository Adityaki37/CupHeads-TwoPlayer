Imports System
Imports UnityEngine

' Token: 0x0200058B RID: 1419
Public Class DevilLevelFireball
	Inherits AbstractProjectile

	' Token: 0x06001B1C RID: 6940 RVA: 0x000F91D0 File Offset: 0x000F75D0
	Public Function Create(xPos As Single, speed As Single, gravity As Single, xScale As Single) As DevilLevelFireball
		Dim devilLevelFireball As DevilLevelFireball = Me.InstantiatePrefab(Of DevilLevelFireball)()
		devilLevelFireball.transform.position = New Vector2(xPos, 500F)
		devilLevelFireball.yVelocity = -speed
		devilLevelFireball.gravity = gravity
		devilLevelFireball.transform.SetScale(New Single?(xScale), Nothing, Nothing)
		Return devilLevelFireball
	End Function

	' Token: 0x06001B1D RID: 6941 RVA: 0x000F9233 File Offset: 0x000F7633
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B1E RID: 6942 RVA: 0x000F9254 File Offset: 0x000F7654
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If MyBase.dead Then
			Return
		End If
		Me.yVelocity -= Me.gravity * CupheadTime.FixedDelta
		MyBase.transform.AddPosition(0F, Me.yVelocity * CupheadTime.FixedDelta, 0F)
	End Sub

	' Token: 0x06001B1F RID: 6943 RVA: 0x000F92B0 File Offset: 0x000F76B0
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.poofEffect.Create(MyBase.transform.position)
		For Each spriteDeathParts As SpriteDeathParts In Me.parts
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001B20 RID: 6944 RVA: 0x000F9316 File Offset: 0x000F7716
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.poofEffect = Nothing
		Me.parts = Nothing
	End Sub

	' Token: 0x04002457 RID: 9303
	<SerializeField()>
	Private poofEffect As Effect

	' Token: 0x04002458 RID: 9304
	<SerializeField()>
	Private parts As SpriteDeathParts()

	' Token: 0x04002459 RID: 9305
	Private Const SPAWN_Y As Single = 500F

	' Token: 0x0400245A RID: 9306
	Private yVelocity As Single

	' Token: 0x0400245B RID: 9307
	Private gravity As Single
End Class
