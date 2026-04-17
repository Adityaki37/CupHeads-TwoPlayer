Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006BB RID: 1723
Public Class FrogsLevelShortClapBullet
	Inherits AbstractProjectile

	' Token: 0x06002495 RID: 9365 RVA: 0x0015702C File Offset: 0x0015542C
	Public Function Create(frogDir As FrogsLevelShort.Direction, dir As FrogsLevelShortClapBullet.Direction, pos As Vector2, velocity As Vector2) As FrogsLevelShortClapBullet
		Dim frogsLevelShortClapBullet As FrogsLevelShortClapBullet = TryCast(MyBase.Create(pos), FrogsLevelShortClapBullet)
		frogsLevelShortClapBullet.CollisionDeath.OnlyPlayer()
		frogsLevelShortClapBullet.DamagesType.OnlyPlayer()
		frogsLevelShortClapBullet.Init(frogDir, dir, pos, velocity)
		Return frogsLevelShortClapBullet
	End Function

	' Token: 0x06002496 RID: 9366 RVA: 0x00157069 File Offset: 0x00155469
	Private Sub Init(frogDir As FrogsLevelShort.Direction, dir As FrogsLevelShortClapBullet.Direction, pos As Vector2, velocity As Vector2)
		Me.frogDirection = frogDir
		Me.velocity = velocity
		Me.direction = dir
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x06002497 RID: 9367 RVA: 0x0015708E File Offset: 0x0015548E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002498 RID: 9368 RVA: 0x001570A4 File Offset: 0x001554A4
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x06002499 RID: 9369 RVA: 0x001570B4 File Offset: 0x001554B4
	Private Iterator Function go_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim upY As Single = Me.velocity.y
		Dim downY As Single = -Me.velocity.y
		Dim x As Single = If((Me.frogDirection <> FrogsLevelShort.Direction.Right), (-Me.velocity.x), Me.velocity.x)
		Dim y As Single = If((Me.direction <> FrogsLevelShortClapBullet.Direction.Up), downY, upY)
		If Me.direction = FrogsLevelShortClapBullet.Direction.Up Then
			MyBase.transform.LookAt2D(MyBase.transform.position + New Vector2(x, upY))
		Else
			MyBase.transform.LookAt2D(MyBase.transform.position + New Vector2(x, downY))
		End If
		While True
			If Me.direction = FrogsLevelShortClapBullet.Direction.Up Then
				If MyBase.transform.position.y >= 360F Then
					AudioManager.Play("level_frogs_short_clap_bounce")
					Me.emitAudioFromObject.Add("level_frogs_short_clap_bounce")
					Me.direction = FrogsLevelShortClapBullet.Direction.Down
					Me.bounceEffect.Create(MyBase.transform.position, New Vector3(1F, -1F, 1F))
					y = downY
					MyBase.transform.LookAt2D(MyBase.transform.position + New Vector2(x, y))
				End If
			ElseIf MyBase.transform.position.y <= CSng(Level.Current.Ground) Then
				AudioManager.Play("level_frogs_short_clap_bounce")
				Me.emitAudioFromObject.Add("level_frogs_short_clap_bounce")
				Me.direction = FrogsLevelShortClapBullet.Direction.Up
				Me.bounceEffect.Create(MyBase.transform.position)
				y = upY
				MyBase.transform.LookAt2D(MyBase.transform.position + New Vector2(x, y))
			End If
			If MyBase.transform.position.x > 640F + MyBase.GetComponent(Of SpriteRenderer)().bounds.size.x / 2F Then
				Exit For
			End If
			MyBase.transform.AddPosition(x * CupheadTime.FixedDelta, y * CupheadTime.FixedDelta, 0F)
			Yield wait
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x04002D40 RID: 11584
	Private Const MAX_Y As Single = 360F

	' Token: 0x04002D41 RID: 11585
	<SerializeField()>
	Private bounceEffect As Effect

	' Token: 0x04002D42 RID: 11586
	Private velocity As Vector2

	' Token: 0x04002D43 RID: 11587
	Private frogDirection As FrogsLevelShort.Direction

	' Token: 0x04002D44 RID: 11588
	Private direction As FrogsLevelShortClapBullet.Direction

	' Token: 0x020006BC RID: 1724
	Public Enum Direction
		' Token: 0x04002D46 RID: 11590
		Up
		' Token: 0x04002D47 RID: 11591
		Down
	End Enum
End Class
