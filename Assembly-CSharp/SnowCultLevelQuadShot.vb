Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007F2 RID: 2034
Public Class SnowCultLevelQuadShot
	Inherits AbstractProjectile

	' Token: 0x17000416 RID: 1046
	' (get) Token: 0x06002EAD RID: 11949 RVA: 0x001B8830 File Offset: 0x001B6C30
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06002EAE RID: 11950 RVA: 0x001B8838 File Offset: 0x001B6C38
	Public Overridable Function Init(startPos As Vector3, destPos As Vector3, speed As Single, hazardDirectionInstruction As String, properties As LevelProperties.SnowCult.QuadShot, rowPosition As Integer, delay As Single, distanceBetween As Single, targetPlayer As AbstractPlayerController) As SnowCultLevelQuadShot
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		AddHandler CType(Level.Current, SnowCultLevel).OnYetiHitGround, AddressOf Me.WhaleDeath
		Me.id = If((rowPosition Mod 2 <> 0), "B"c, "A"c)
		MyBase.animator.Play("Emerge" + Me.id)
		MyBase.transform.position = startPos
		Me.startPos = startPos
		Me.destPos = destPos
		Me.properties = properties
		Me.speed = speed
		Me.hazardDirectionInstruction = hazardDirectionInstruction
		Me.rowPosition = rowPosition
		Me.distanceBetween = distanceBetween
		Me.targetPlayer = targetPlayer
		Me.delay = delay
		MyBase.transform.localScale = New Vector3(CSng(If((Me.rowPosition <= 1), 1, (-1))), 1F)
		MyBase.StartCoroutine(Me.move_to_launch_pos_cr())
		MyBase.tag = "EnemyProjectile"
		Return Me
	End Function

	' Token: 0x06002EAF RID: 11951 RVA: 0x001B8937 File Offset: 0x001B6D37
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002EB0 RID: 11952 RVA: 0x001B8958 File Offset: 0x001B6D58
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If Not Me.grounded Then
			Return
		End If
		MyBase.OnCollisionEnemy(hit, phase)
		If phase <> CollisionPhase.[Exit] AndAlso (hit.GetComponent(Of SnowCultLevelWhaleCollision)() OrElse hit.GetComponent(Of SnowCultLevelQuadShot)()) AndAlso phase = CollisionPhase.Enter Then
			MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.position.x - hit.gameObject.transform.position.x), 1F)
			Me.WhaleDeath()
		End If
	End Sub

	' Token: 0x06002EB1 RID: 11953 RVA: 0x001B89F4 File Offset: 0x001B6DF4
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Not Me.grounded Then
			Return
		End If
		Me.health -= info.damage
		If Me.health < 0F Then
			If Me.running Then
				If Not MyBase.dead Then
					Level.Current.RegisterMinionKilled()
					MyBase.transform.localScale = New Vector3(CSng(MathUtils.PlusOrMinus()), 1F)
					Me.rend.flipX = Rand.Bool()
					Me.Dead()
				End If
			Else
				Me.running = True
				Me.health = Me.properties.hazardHealth
			End If
		End If
	End Sub

	' Token: 0x06002EB2 RID: 11954 RVA: 0x001B8AA0 File Offset: 0x001B6EA0
	Private Iterator Function move_to_launch_pos_cr() As IEnumerator
		Dim t As Single = 0F
		While t < 0.33333334F
			Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
			t += 0.041666668F
			MyBase.transform.position = Vector3.Lerp(Me.startPos, Me.destPos, Mathf.InverseLerp(0F, 0.33333334F, t))
		End While
		Return
	End Function

	' Token: 0x06002EB3 RID: 11955 RVA: 0x001B8ABC File Offset: 0x001B6EBC
	Public Sub Shoot(angle As Single)
		Me.angle = angle
		MyBase.transform.localScale = New Vector3(CSng(If((angle >= -90F), (-1), 1)), 1F)
		MyBase.animator.Play("Launch" + Me.id)
		Me.sparkEffect.Create(MyBase.transform.position)
		If Me.rowPosition Mod 2 = 0 Then
			Me.rend.sortingOrder = 3
		End If
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x06002EB4 RID: 11956 RVA: 0x001B8B58 File Offset: 0x001B6F58
	Private Iterator Function shoot_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.delay)
		While MyBase.transform.position.y > -240F
			MyBase.transform.position += MathUtils.AngleToDirection(Me.angle) * Me.speed * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		MyBase.transform.position = New Vector3(MyBase.transform.position.x, -240F)
		MyBase.StartCoroutine(Me.run_away_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002EB5 RID: 11957 RVA: 0x001B8B74 File Offset: 0x001B6F74
	Private Iterator Function run_away_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim animHelper As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.tag = "Enemy"
		Me.grounded = True
		Me.health = Me.properties.groundHealth
		MyBase.animator.Play("HitGround" + Me.id)
		Me.SFX_SNOWCULT_QuadshotMinionStuckInGround()
		Me.snowLandEffect.Create(New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground)))
		Dim t As Single = (Me.properties.hazardMoveDelay - Me.delay) * Me.popOutWarningTimeNormalized
		While t > 0F AndAlso Not Me.running
			t -= CupheadTime.Delta
			Yield Nothing
		End While
		animHelper.Speed = 1.5F
		t = (Me.properties.hazardMoveDelay - Me.delay) * (1F - Me.popOutWarningTimeNormalized)
		While t > 0F AndAlso Not Me.running
			t -= CupheadTime.Delta
			Yield Nothing
		End While
		animHelper.Speed = 1F
		Me.running = True
		Me.health = Me.properties.hazardHealth
		Dim direction As Single = 0F
		Dim text As String = Me.hazardDirectionInstruction
		If text IsNot Nothing Then
			If Not(text = "L") Then
				If Not(text = "R") Then
					If Not(text = "F") Then
						If Not(text = "G") Then
							If text = "P" Then
								direction = CSng(If((MyBase.transform.position.x - Me.targetPlayer.transform.position.x <= 0F), 1, (-1)))
							End If
						Else
							Dim num As Single = MyBase.transform.position.x + (CSng((2 - Me.rowPosition)) - 0.5F) * Me.distanceBetween
							direction = CSng(If((Mathf.Abs(num - CSng(Level.Current.Left)) <= Mathf.Abs(num - CSng(Level.Current.Right))), 1, (-1)))
						End If
					Else
						direction = CSng(If((Mathf.Abs(MyBase.transform.position.x - CSng(Level.Current.Left)) <= Mathf.Abs(MyBase.transform.position.x - CSng(Level.Current.Right))), 1, (-1)))
					End If
				Else
					direction = 1F
				End If
			Else
				direction = -1F
			End If
		End If
		MyBase.transform.localScale = New Vector3(-direction, 1F)
		MyBase.animator.Play("PopOut" + Me.id)
		Me.SFX_SNOWCULT_QuadshotMinionFlipUp()
		Me.snowPopOutEffect.Create(New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground)))
		Yield Nothing
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.16666667F
			Yield Nothing
		End While
		Me.health = Me.properties.hazardHealth
		Me.rend.sortingOrder = 1
		t = 0F
		While MyBase.transform.position.x > CSng((Level.Current.Left - 200)) AndAlso MyBase.transform.position.x < CSng((Level.Current.Right + 200))
			t = Mathf.Clamp(t + CupheadTime.FixedDelta * 2F, 0F, 1F)
			MyBase.transform.position += Vector3.right * direction * Me.properties.hazardSpeed * CupheadTime.FixedDelta * t
			Yield wait
		End While
		Me.Recycle()
		Return
	End Function

	' Token: 0x06002EB6 RID: 11958 RVA: 0x001B8B8F File Offset: 0x001B6F8F
	Private Sub WhaleDeath()
		Me.Dead()
		Me.rend.sortingLayerName = "Foreground"
		MyBase.animator.Play("WhaleDeath" + Me.id)
	End Sub

	' Token: 0x06002EB7 RID: 11959 RVA: 0x001B8BC8 File Offset: 0x001B6FC8
	Private Sub Dead()
		Me.StopAllCoroutines()
		Me.deathPuff.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.deathPuff.flipX = Rand.Bool()
		Me.deathPuff.flipY = Rand.Bool()
		MyBase.animator.Play("Death")
		Me.SFX_SNOWCULT_QuadshotMinionDie()
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06002EB8 RID: 11960 RVA: 0x001B8C48 File Offset: 0x001B7048
	Private Sub aniEvent_Dead()
		Me.Recycle()
	End Sub

	' Token: 0x06002EB9 RID: 11961 RVA: 0x001B8C50 File Offset: 0x001B7050
	Protected Overrides Sub OnDestroy()
		If Level.Current Then
			RemoveHandler CType(Level.Current, SnowCultLevel).OnYetiHitGround, AddressOf Me.WhaleDeath
		End If
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06002EBA RID: 11962 RVA: 0x001B8C82 File Offset: 0x001B7082
	Private Sub SFX_SNOWCULT_QuadshotMinionStuckInGround()
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_stuckinground")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_stuckinground")
	End Sub

	' Token: 0x06002EBB RID: 11963 RVA: 0x001B8C9E File Offset: 0x001B709E
	Private Sub SFX_SNOWCULT_QuadshotMinionFlipUp()
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_flipup")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_flipup")
	End Sub

	' Token: 0x06002EBC RID: 11964 RVA: 0x001B8CBA File Offset: 0x001B70BA
	Private Sub SFX_SNOWCULT_QuadshotMinionDie()
		AudioManager.Play("sfx_dlc_snowcult_p1_minion_death_explode")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p1_minion_death_explode")
	End Sub

	' Token: 0x0400374F RID: 14159
	Private Const GROUND_Y As Single = -240F

	' Token: 0x04003750 RID: 14160
	<SerializeField()>
	Private popOutWarningTimeNormalized As Single = 0.8F

	' Token: 0x04003751 RID: 14161
	<SerializeField()>
	Private sparkEffect As Effect

	' Token: 0x04003752 RID: 14162
	<SerializeField()>
	Private snowLandEffect As Effect

	' Token: 0x04003753 RID: 14163
	<SerializeField()>
	Private snowPopOutEffect As Effect

	' Token: 0x04003754 RID: 14164
	<SerializeField()>
	Private deathPuff As SpriteRenderer

	' Token: 0x04003755 RID: 14165
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04003756 RID: 14166
	Private properties As LevelProperties.SnowCult.QuadShot

	' Token: 0x04003757 RID: 14167
	Private damageReceiver As DamageReceiver

	' Token: 0x04003758 RID: 14168
	Private speed As Single

	' Token: 0x04003759 RID: 14169
	Private delay As Single

	' Token: 0x0400375A RID: 14170
	Private angle As Single

	' Token: 0x0400375B RID: 14171
	Private hazardDirectionInstruction As String

	' Token: 0x0400375C RID: 14172
	Private rowPosition As Integer

	' Token: 0x0400375D RID: 14173
	Private distanceBetween As Single

	' Token: 0x0400375E RID: 14174
	Private targetPlayer As AbstractPlayerController

	' Token: 0x0400375F RID: 14175
	Private startPos As Vector3

	' Token: 0x04003760 RID: 14176
	Private destPos As Vector3

	' Token: 0x04003761 RID: 14177
	Private health As Single

	' Token: 0x04003762 RID: 14178
	Private isDead As Boolean

	' Token: 0x04003763 RID: 14179
	Private grounded As Boolean

	' Token: 0x04003764 RID: 14180
	Private running As Boolean

	' Token: 0x04003765 RID: 14181
	Private id As Char
End Class
