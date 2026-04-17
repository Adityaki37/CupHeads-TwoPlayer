Imports System
Imports UnityEngine

' Token: 0x02000618 RID: 1560
Public Class FlyingBirdLevelBirdEgg
	Inherits AbstractProjectile

	' Token: 0x06001FB5 RID: 8117 RVA: 0x00123870 File Offset: 0x00121C70
	Public Overridable Function Create(speed As Single, pos As Vector2) As AbstractProjectile
		Dim flyingBirdLevelBirdEgg As FlyingBirdLevelBirdEgg = TryCast(Me.Create(pos, 0F), FlyingBirdLevelBirdEgg)
		flyingBirdLevelBirdEgg.speed = -speed
		flyingBirdLevelBirdEgg.CollisionDeath.OnlyPlayer()
		flyingBirdLevelBirdEgg.DamagesType.OnlyPlayer()
		Return flyingBirdLevelBirdEgg
	End Function

	' Token: 0x06001FB6 RID: 8118 RVA: 0x001238B0 File Offset: 0x00121CB0
	Protected Overrides Sub Start()
		MyBase.Start()
		Dim mode As Level.Mode = Level.Current.mode
		If mode <> Level.Mode.Easy Then
			If mode <> Level.Mode.Normal Then
				If mode = Level.Mode.Hard Then
					Me.maxProjectiles = 5
				End If
			Else
				Me.maxProjectiles = 3
			End If
		Else
			Me.maxProjectiles = 2
		End If
	End Sub

	' Token: 0x06001FB7 RID: 8119 RVA: 0x0012390C File Offset: 0x00121D0C
	Protected Overrides Sub Update()
		MyBase.Update()
		MyBase.transform.position += MyBase.transform.right * Me.speed * CupheadTime.Delta
		If Me.state = FlyingBirdLevelBirdEgg.State.Idle AndAlso MyBase.transform.position.x < -640F Then
			Me.Explode()
			Me.Die()
		End If
	End Sub

	' Token: 0x06001FB8 RID: 8120 RVA: 0x0012398E File Offset: 0x00121D8E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
			Me.Die()
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001FB9 RID: 8121 RVA: 0x001239B4 File Offset: 0x00121DB4
	Private Sub Explode()
		AudioManager.Play("level_flying_bird_egg_explode")
		Me.emitAudioFromObject.Add("level_flying_bird_egg_explode")
		AudioManager.Play("level_flying_bird_egg_break")
		Me.emitAudioFromObject.Add("level_flying_bird_egg_break")
		If Me.state <> FlyingBirdLevelBirdEgg.State.Idle Then
			Return
		End If
		Me.state = FlyingBirdLevelBirdEgg.State.Exploded
		Me.effectPrefab.Create(MyBase.transform.position)
		If Me.maxProjectiles = 0 Then
			Return
		End If
		Dim position As Vector3 = MyBase.transform.position
		position.x += 42F
		If Me.maxProjectiles = 2 Then
			Me.childPrefab.Create(position, 90F, Vector2.one, -Me.speed)
			Me.childPrefab.Create(position, -90F, Vector2.one, -Me.speed)
		Else
			For i As Integer = 0 To Me.maxProjectiles - 1
				Dim num As Single
				Select Case i
					Case Else
						num = 0F
					Case 1
						num = -45F
					Case 2
						num = 45F
					Case 3
						num = 90F
					Case 4
						num = -90F
				End Select
				Me.childPrefab.Create(position, num, Vector2.one, -Me.speed)
			Next
		End If
	End Sub

	' Token: 0x0400283E RID: 10302
	Private Const ANGLE As Single = 45F

	' Token: 0x0400283F RID: 10303
	<SerializeField()>
	Private childPrefab As BasicProjectile

	' Token: 0x04002840 RID: 10304
	<SerializeField()>
	Private effectPrefab As Effect

	' Token: 0x04002841 RID: 10305
	Private speed As Single

	' Token: 0x04002842 RID: 10306
	Private state As FlyingBirdLevelBirdEgg.State

	' Token: 0x04002843 RID: 10307
	Private maxProjectiles As Integer

	' Token: 0x02000619 RID: 1561
	Private Enum State
		' Token: 0x04002845 RID: 10309
		Idle
		' Token: 0x04002846 RID: 10310
		Exploded
	End Enum
End Class
