Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200062F RID: 1583
Public Class FlyingBlimpLevelBlimpLady
	Inherits LevelProperties.FlyingBlimp.Entity

	' Token: 0x17000381 RID: 897
	' (get) Token: 0x0600203D RID: 8253 RVA: 0x00128520 File Offset: 0x00126920
	' (set) Token: 0x0600203E RID: 8254 RVA: 0x00128528 File Offset: 0x00126928
	Public Property state As FlyingBlimpLevelBlimpLady.State

	' Token: 0x17000382 RID: 898
	' (get) Token: 0x0600203F RID: 8255 RVA: 0x00128531 File Offset: 0x00126931
	' (set) Token: 0x06002040 RID: 8256 RVA: 0x00128539 File Offset: 0x00126939
	Public Property moving As Boolean

	' Token: 0x17000383 RID: 899
	' (get) Token: 0x06002041 RID: 8257 RVA: 0x00128542 File Offset: 0x00126942
	' (set) Token: 0x06002042 RID: 8258 RVA: 0x0012854A File Offset: 0x0012694A
	Public Property fading As Boolean

	' Token: 0x14000044 RID: 68
	' (add) Token: 0x06002043 RID: 8259 RVA: 0x00128554 File Offset: 0x00126954
	' (remove) Token: 0x06002044 RID: 8260 RVA: 0x0012858C File Offset: 0x0012698C
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x06002045 RID: 8261 RVA: 0x001285C4 File Offset: 0x001269C4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.state = FlyingBlimpLevelBlimpLady.State.Intro
		Me.pivotOffset = Vector3.up * 2F * Me.loopSize
		Me.pivotPoint.position = MyBase.transform.position
		Me.startPos = MyBase.transform.position
		Me.ResetPivotPos(MyBase.transform.position)
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.constellationHandler.color = New Color(1F, 1F, 1F, 0F)
	End Sub

	' Token: 0x06002046 RID: 8262 RVA: 0x00128688 File Offset: 0x00126A88
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingBlimp)
		MyBase.LevelInit(properties)
		Me.originalSpeed = properties.CurrentState.move.pathSpeed
		Me.moving = True
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06002047 RID: 8263 RVA: 0x001286BB File Offset: 0x00126ABB
	Public Overrides Sub OnLevelEnd()
		MyBase.OnLevelEnd()
	End Sub

	' Token: 0x06002048 RID: 8264 RVA: 0x001286C4 File Offset: 0x00126AC4
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> FlyingBlimpLevelBlimpLady.State.Death Then
			Me.state = FlyingBlimpLevelBlimpLady.State.Death
			Me.StartDeath()
		End If
	End Sub

	' Token: 0x06002049 RID: 8265 RVA: 0x00128710 File Offset: 0x00126B10
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600204A RID: 8266 RVA: 0x00128728 File Offset: 0x00126B28
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600204B RID: 8267 RVA: 0x00128748 File Offset: 0x00126B48
	Private Sub ResetPivotPos(newPos As Vector3)
		Me.pivotPoint.position = newPos
		Dim position As Vector3 = MyBase.transform.position
		position.y = newPos.y + Me.loopSize
		MyBase.transform.position = position
	End Sub

	' Token: 0x0600204C RID: 8268 RVA: 0x00128790 File Offset: 0x00126B90
	Private Iterator Function intro_cr() As IEnumerator
		AudioManager.Play("level_flying_blimp_intro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro_End", False, True)
		AudioManager.PlayLoop("level_flying_blimp_pedal_loop")
		MyBase.StartCoroutine(Me.move_cr())
		While Me.movementSpeed < Me.originalSpeed
			Me.movementSpeed += 0.2F
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.move.initalAttackDelayRange.RandomFloat())
		Me.state = FlyingBlimpLevelBlimpLady.State.Idle
		Return
	End Function

	' Token: 0x0600204D RID: 8269 RVA: 0x001287AC File Offset: 0x00126BAC
	Private Iterator Function move_cr() As IEnumerator
		Me.angle = If((Not Rand.Bool()), 0F, 6.2831855F)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If Me.moving Then
				Me.PathMovement()
				Yield wait
			Else
				Yield wait
			End If
		End While
		Return
	End Function

	' Token: 0x0600204E RID: 8270 RVA: 0x001287C8 File Offset: 0x00126BC8
	Public Sub PathMovement()
		Me.angle += Me.movementSpeed * CupheadTime.FixedDelta
		If Me.angle > 6.2831855F Then
			Me.invert = Not Me.invert
			Me.angle -= 6.2831855F
		End If
		If Me.angle < 0F Then
			Me.angle += 6.2831855F
		End If
		Dim num As Single
		If Me.invert Then
			MyBase.transform.position = Me.pivotPoint.position + Me.pivotOffset
			num = -1F
		Else
			MyBase.transform.position = Me.pivotPoint.position
			num = 1F
		End If
		Dim vector As Vector3 = New Vector3(-Mathf.Sin(Me.angle) * Me.loopSize, Mathf.Cos(Me.angle) * num * Me.loopSize, 0F)
		MyBase.transform.position += vector
	End Sub

	' Token: 0x0600204F RID: 8271 RVA: 0x001288DE File Offset: 0x00126CDE
	Private Sub ChangeMat(mat As Material)
		MyBase.GetComponent(Of SpriteRenderer)().material = mat
	End Sub

	' Token: 0x06002050 RID: 8272 RVA: 0x001288EC File Offset: 0x00126CEC
	Public Sub StartDash()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.dash_cr())
	End Sub

	' Token: 0x06002051 RID: 8273 RVA: 0x00128918 File Offset: 0x00126D18
	Private Iterator Function dash_cr() As IEnumerator
		Dim startedClouds As Boolean = False
		Me.smallClouds = True
		Me.transitionToSummon = True
		Dim constAnimator As Animator = Me.constellationHandler.GetComponent(Of Animator)()
		Me.state = FlyingBlimpLevelBlimpLady.State.Dash
		Dim p As LevelProperties.FlyingBlimp.DashSummon = MyBase.properties.CurrentState.dashSummon
		Dim pattern As String() = p.patternString.GetRandom().Split(New Char() { ","c })
		Me.moving = False
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i)(0) = "D"c Then
				Dim waitTime As Single = 0F
				Parser.FloatTryParse(pattern(i).Substring(1), waitTime)
				Yield CupheadTime.WaitForSeconds(Me, waitTime)
			Else
				Yield CupheadTime.WaitForSeconds(Me, 0.1F)
				AudioManager.[Stop]("level_flying_blimp_pedal_loop")
				AudioManager.Play("level_flying_blimp_inhale")
				MyBase.animator.Play("Dash_Start")
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Dash_Start", False, True)
				Yield CupheadTime.WaitForSeconds(Me, p.hold)
				AudioManager.Play("level_flying_blimp_exhale")
				MyBase.animator.SetBool("Deflate", True)
				Yield CupheadTime.WaitForSeconds(Me, 0.2F)
				Me.fading = True
				MyBase.StartCoroutine(Me.fade_constellation_cr(True))
				AudioManager.Play("level_flying_blimp_lady_constellation_loop")
				Select Case Me.constellation
					Case FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus
						constAnimator.Play("Taurus")
					Case FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius
						constAnimator.Play("Sagittarius")
					Case FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini
						constAnimator.Play("Gemini")
				End Select
				MyBase.animator.SetTrigger("Move")
				While MyBase.transform.position.x >= -1280F
					If Me.state <> FlyingBlimpLevelBlimpLady.State.Death Then
						MyBase.transform.position += MyBase.transform.right * -p.dashSpeed * CupheadTime.Delta
					End If
					Yield Nothing
				End While
				Me.dashExplosions = False
				MyBase.animator.SetTrigger("ToOff")
				Yield CupheadTime.WaitForSeconds(Me, p.reeentryDelay)
				MyBase.animator.SetTrigger("StartSummon")
				AudioManager.PlayLoop("level_flying_blimp_pedal_loop")
				AudioManager.Play("level_flying_blimp_lady_constellation_transform")
				Dim endPos As Vector3 = Me.startPos
				If Me.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini Then
					endPos.x = Me.startPos.x + 100F
					Me.ResetPivotPos(endPos)
				End If
				Dim pos As Vector3 = MyBase.transform.position
				pos.y = Me.startPos.y
				MyBase.transform.position = pos
				While MyBase.transform.position.x <= endPos.x
					If MyBase.transform.position.x >= Me.transformationPoint.position.x AndAlso Not startedClouds Then
						Me.fading = False
						MyBase.StartCoroutine(Me.fade_constellation_cr(False))
						MyBase.StartCoroutine(Me.spawn_clouds_cr())
						startedClouds = True
					End If
					If Me.state <> FlyingBlimpLevelBlimpLady.State.Death Then
						MyBase.transform.position += MyBase.transform.right * p.summonSpeed * CupheadTime.FixedDelta
					End If
					Yield New WaitForFixedUpdate()
				End While
				If MyBase.properties.CurrentState.stateName = LevelProperties.FlyingBlimp.States.Generic Then
					Me.transitionToSummon = False
				Else
					Me.transitionToSummon = True
				End If
				AudioManager.[Stop]("level_flying_blimp_pedal_loop")
				MyBase.animator.Play("Big_Cloud")
				Me.smallClouds = False
				MyBase.animator.SetBool("Deflate", False)
			End If
		Next
		Return
	End Function

	' Token: 0x06002052 RID: 8274 RVA: 0x00128934 File Offset: 0x00126D34
	Private Iterator Function select_constellation_cr() As IEnumerator
		If Me.transitionToSummon Then
			Select Case Me.constellation
				Case FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus
					Me.ToTaurus()
					MyBase.animator.Play("Taurus_Idle")
					Me.ChangeMat(Me.taurusMat)
				Case FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius
					Me.ToSagittarius()
					MyBase.animator.Play("Sag_Cloud")
					MyBase.animator.Play("Sagittarius_Idle")
					Me.ChangeMat(Me.sagittariusMat)
				Case FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini
					Me.ToGemini()
					MyBase.animator.Play("Gemini")
					MyBase.animator.Play("Sphere_Idle")
					Me.ChangeMat(Me.geminiMat)
			End Select
			Me.transitionToSummon = False
		Else
			Me.ResetPivotPos(Me.startPos)
			AudioManager.Play("level_flying_blimp_lady_constellation_transform_end")
			MyBase.animator.Play("Appear")
			Me.ChangeMat(Me.blimpMat)
			If Me.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius Then
				MyBase.animator.Play("Sag_Off")
			End If
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Appear", False, True)
			Me.moving = True
			AudioManager.PlayLoop("level_flying_blimp_pedal_loop")
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.dashSummon.summonHesitate)
			Me.state = FlyingBlimpLevelBlimpLady.State.Idle
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002053 RID: 8275 RVA: 0x00128950 File Offset: 0x00126D50
	Private Iterator Function check_state_cr(currentState As LevelProperties.FlyingBlimp.States) As IEnumerator
		Me.isLooping = True
		While MyBase.properties.CurrentState.stateName = currentState
			Yield Nothing
		End While
		Me.isLooping = False
		Me.waitLoopTime = 0F
		Return
	End Function

	' Token: 0x06002054 RID: 8276 RVA: 0x00128972 File Offset: 0x00126D72
	Private Sub StartSmoke()
		MyBase.animator.Play("Dash_Smoke")
		Me.dashExplosions = True
		MyBase.StartCoroutine(Me.spawn_explosions_cr())
	End Sub

	' Token: 0x06002055 RID: 8277 RVA: 0x00128998 File Offset: 0x00126D98
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.yellow
		Gizmos.DrawWireSphere(MyBase.baseTransform.position + Me.explosionOffset, Me.explosionRadius)
	End Sub

	' Token: 0x06002056 RID: 8278 RVA: 0x001289D8 File Offset: 0x00126DD8
	Private Iterator Function spawn_explosions_cr() As IEnumerator
		While Me.dashExplosions
			Dim explosion As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.dashExplosionEffect)
			explosion.transform.position = MyBase.transform.position
			Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		End While
		Return
	End Function

	' Token: 0x06002057 RID: 8279 RVA: 0x001289F4 File Offset: 0x00126DF4
	Private Iterator Function spawn_clouds_cr() As IEnumerator
		While Me.smallClouds
			Dim cloud As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.cloudEffect)
			Dim scale As Vector3 = New Vector3(1F, 1F, 1F)
			scale.x = If((Not Rand.Bool()), (-scale.x), scale.x)
			scale.y = If((Not Rand.Bool()), (-scale.y), scale.y)
			cloud.transform.SetScale(New Single?(scale.x), New Single?(scale.y), New Single?(1F))
			cloud.transform.eulerAngles = New Vector3(0F, 0F, Global.UnityEngine.Random.Range(0F, 360F))
			cloud.GetComponent(Of SpriteRenderer)().sortingOrder = Global.UnityEngine.Random.Range(0, 3)
			cloud.transform.position = Me.GetRandomPoint()
			MyBase.StartCoroutine(Me.delete_cloud_cr(cloud))
			Yield CupheadTime.WaitForSeconds(Me, 0.05F)
		End While
		Return
	End Function

	' Token: 0x06002058 RID: 8280 RVA: 0x00128A10 File Offset: 0x00126E10
	Private Iterator Function delete_cloud_cr(cloud As GameObject) As IEnumerator
		Yield cloud.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, "Cloud", False, True)
		Global.UnityEngine.[Object].Destroy(cloud)
		Return
	End Function

	' Token: 0x06002059 RID: 8281 RVA: 0x00128A34 File Offset: 0x00126E34
	Private Function GetRandomPoint() As Vector2
		Dim vector As Vector2 = MyBase.transform.position + Me.explosionOffset
		Dim vector2 As Vector2 = New Vector2(CSng(Global.UnityEngine.Random.Range(-1, 1)), CSng(Global.UnityEngine.Random.Range(-1, 1)))
		Dim vector3 As Vector2 = vector2.normalized * (Me.explosionRadius * Global.UnityEngine.Random.value) * 2F
		Return vector + vector3
	End Function

	' Token: 0x0600205A RID: 8282 RVA: 0x00128AA0 File Offset: 0x00126EA0
	Private Iterator Function fade_constellation_cr(fadeIn As Boolean) As IEnumerator
		Dim fadeTime As Single = 0.5F
		Dim blackMaxFade As Single = 0.25F
		Dim blackMidFade As Single = 0.13F
		Dim blackCurrentFade As Single = 0F
		If fadeIn Then
			Dim t As Single = 0F
			While t < fadeTime
				Me.constellationHandler.color = New Color(1F, 1F, 1F, t / fadeTime)
				If blackCurrentFade < blackMaxFade Then
					Me.blackDim.color = New Color(0F, 0F, 0F, blackCurrentFade + t / 3F)
				End If
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Me.constellationHandler.color = New Color(1F, 1F, 1F, 1F)
			Me.blackDim.color = New Color(0F, 0F, 0F, blackMaxFade)
		Else
			Dim t2 As Single = 0F
			blackCurrentFade = blackMaxFade
			While t2 < fadeTime
				Me.constellationHandler.color = New Color(1F, 1F, 1F, 1F - t2 / fadeTime)
				If blackCurrentFade > blackMidFade Then
					Me.blackDim.color = New Color(0F, 0F, 0F, blackCurrentFade - t2 / 3F)
				End If
				t2 += CupheadTime.Delta
				Yield Nothing
			End While
			Me.constellationHandler.color = New Color(1F, 1F, 1F, 0F)
			Me.blackDim.color = New Color(0F, 0F, 0F, blackMidFade)
			Yield Nothing
		End If
		Return
	End Function

	' Token: 0x0600205B RID: 8283 RVA: 0x00128AC4 File Offset: 0x00126EC4
	Private Iterator Function final_fade_cr() As IEnumerator
		Dim fadeTime As Single = 0.5F
		Dim blackMidFade As Single = 0.13F
		Dim t As Single = 0F
		While t < fadeTime
			Me.blackDim.color = New Color(0F, 0F, 0F, blackMidFade - t / 3F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.constellationHandler.color = New Color(1F, 1F, 1F, 0F)
		Yield Nothing
		Return
	End Function

	' Token: 0x0600205C RID: 8284 RVA: 0x00128ADF File Offset: 0x00126EDF
	Public Sub StartTaurus()
		Me.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Taurus
		Me.StartDash()
		MyBase.StartCoroutine(Me.check_state_cr(MyBase.properties.CurrentState.stateName))
	End Sub

	' Token: 0x0600205D RID: 8285 RVA: 0x00128B0B File Offset: 0x00126F0B
	Private Sub ToTaurus()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.taurus_cr())
	End Sub

	' Token: 0x0600205E RID: 8286 RVA: 0x00128B38 File Offset: 0x00126F38
	Private Iterator Function taurus_cr() As IEnumerator
		Dim p As LevelProperties.FlyingBlimp.Taurus = MyBase.properties.CurrentState.taurus
		Me.waitLoopTime = p.attackDelayRange.RandomFloat()
		Me.moving = True
		Me.state = FlyingBlimpLevelBlimpLady.State.Taurus
		Me.movementSpeed = p.movementSpeed
		Do
			Dim t As Single = 0F
			While t < Me.waitLoopTime
				t += CupheadTime.Delta
				If Not Me.isLooping Then
					Exit While
				End If
				Yield Nothing
			End While
			t = 0F
			Me.moving = False
			MyBase.animator.SetTrigger("TaurusATK")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Taurus_Attack", False)
			AudioManager.Play("level_flying_blimp_taurus_attack")
			AudioManager.[Stop]("level_flying_blimp_taurus_idle")
			Me.emitAudioFromObject.Add("level_flying_blimp_taurus_attack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Taurus_Attack", False, True)
			Me.moving = True
			Yield Nothing
		Loop While Me.isLooping
		MyBase.animator.Play("Big_Cloud")
		Me.movementSpeed = Me.originalSpeed
		MyBase.StartCoroutine(Me.final_fade_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x0600205F RID: 8287 RVA: 0x00128B53 File Offset: 0x00126F53
	Public Sub StartSagittarius()
		Me.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Sagittarius
		Me.StartDash()
		MyBase.StartCoroutine(Me.check_state_cr(MyBase.properties.CurrentState.stateName))
	End Sub

	' Token: 0x06002060 RID: 8288 RVA: 0x00128B7F File Offset: 0x00126F7F
	Private Sub ToSagittarius()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.sagittarius_cr())
	End Sub

	' Token: 0x06002061 RID: 8289 RVA: 0x00128BAC File Offset: 0x00126FAC
	Private Iterator Function sagittarius_cr() As IEnumerator
		Dim p As LevelProperties.FlyingBlimp.Sagittarius = MyBase.properties.CurrentState.sagittarius
		Me.waitLoopTime = p.attackDelayRange.RandomFloat()
		Me.moving = True
		Me.state = FlyingBlimpLevelBlimpLady.State.Sagittarius
		Me.movementSpeed = CSng(p.movementSpeed)
		Do
			MyBase.animator.SetTrigger("SagittariusATK")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Sagittarius_Attack_Loop", False)
			AudioManager.Play("level_flying_blimp_sagittarius_anticipation")
			Yield CupheadTime.WaitForSeconds(Me, p.arrowWarning)
			MyBase.animator.SetTrigger("Continue")
			AudioManager.[Stop]("level_flying_blimp_sagittarius_anticipation")
			Dim t As Single = 0F
			While t < Me.waitLoopTime
				t += CupheadTime.Delta
				If Not Me.isLooping Then
					Exit While
				End If
				Yield Nothing
			End While
			t = 0F
			Yield Nothing
		Loop While Me.isLooping
		MyBase.animator.Play("Big_Cloud")
		Me.movementSpeed = Me.originalSpeed
		MyBase.StartCoroutine(Me.final_fade_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002062 RID: 8290 RVA: 0x00128BC8 File Offset: 0x00126FC8
	Private Sub FireArrowsStars()
		Dim sagittarius As LevelProperties.FlyingBlimp.Sagittarius = MyBase.properties.CurrentState.sagittarius
		Dim num As Integer = 3
		For i As Integer = 0 To num - 1
			Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
			Dim num2 As Single = sagittarius.homingSpreadAngle.GetFloatAt(CSng(i) / (CSng(num) - 1F))
			Dim num3 As Single = sagittarius.homingSpreadAngle.max / 2F
			num2 -= num3
			Dim num4 As Single = Mathf.Atan2(0F, -360F) * 57.29578F
			Me.sagittariusStarPrefab.Create(Me.arrowEffectRoot.transform.position, num4 + num2, sagittarius.arrowInitialSpeed, sagittarius.homingSpeed, sagittarius.homingRotation, sagittarius.homingDurationRange.RandomFloat(), sagittarius.homingDelay, [next], CSng(sagittarius.arrowHP))
		Next
		Me.arrowEffect.Create(Me.arrowEffectRoot.transform.position)
		Me.sagittariusArrowPrefab.Create(Me.arrowRoot.position, 180F, sagittarius.arrowInitialSpeed)
	End Sub

	' Token: 0x06002063 RID: 8291 RVA: 0x00128CEA File Offset: 0x001270EA
	Public Sub StartGemini()
		Me.constellation = FlyingBlimpLevelBlimpLady.constellationPossibility.Gemini
		Me.StartDash()
		MyBase.StartCoroutine(Me.check_state_cr(MyBase.properties.CurrentState.stateName))
	End Sub

	' Token: 0x06002064 RID: 8292 RVA: 0x00128D16 File Offset: 0x00127116
	Private Sub ToGemini()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.gemini_cr())
	End Sub

	' Token: 0x06002065 RID: 8293 RVA: 0x00128D44 File Offset: 0x00127144
	Private Iterator Function gemini_cr() As IEnumerator
		Dim p As LevelProperties.FlyingBlimp.Gemini = MyBase.properties.CurrentState.gemini
		Me.waitLoopTime = MyBase.properties.CurrentState.gemini.spawnerDelay.RandomFloat()
		Me.pivotPoint.position = MyBase.transform.position
		Me.moving = True
		Dim repeat As Boolean = False
		Me.state = FlyingBlimpLevelBlimpLady.State.Gemini
		Do
			If Me.geminiObject Is Nothing Then
				If repeat Then
					AudioManager.Play("level_flying_blimp_gemini_sphere_reappear")
					MyBase.animator.Play("Sphere_Reappear")
				End If
				Dim t As Single = 0F
				While t < Me.waitLoopTime
					t += CupheadTime.Delta
					If Not Me.isLooping Then
						Exit While
					End If
					Yield Nothing
				End While
				t = 0F
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Gemini", False, True)
				MyBase.animator.SetTrigger("GeminiATK")
				AudioManager.Play("level_flying_blimp_gemini_attack")
				MyBase.animator.Play("Gemini_Attack")
				Yield CupheadTime.WaitForSeconds(Me, p.spawnerSpeed)
				Me.SpawnGemini()
				repeat = True
				Yield Nothing
			End If
			Yield Nothing
		Loop While Me.isLooping
		MyBase.animator.Play("Big_Cloud")
		Me.movementSpeed = Me.originalSpeed
		MyBase.StartCoroutine(Me.final_fade_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002066 RID: 8294 RVA: 0x00128D60 File Offset: 0x00127160
	Private Sub SpawnGemini()
		Me.geminiTarget = Me.objectSpawnRoot.transform.position
		Dim vector As Vector2 = Me.geminiTarget
		Dim vector2 As Vector2 = New Vector2(Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)), Global.UnityEngine.Random.value * CSng(If((Not Rand.Bool()), (-1), 1)))
		Me.geminiTarget = vector + vector2.normalized * Me.objectSpawnRoot.radius * Global.UnityEngine.Random.value
		Me.geminiObject = Global.UnityEngine.[Object].Instantiate(Of FlyingBlimpLevelGeminiShoot)(Me.geminiObjectPrefab)
		Me.geminiObject.Init(MyBase.properties.CurrentState.gemini, Me.geminiTarget)
	End Sub

	' Token: 0x06002067 RID: 8295 RVA: 0x00128E23 File Offset: 0x00127223
	Private Sub SwitchCloneBottomLayer()
		Me.geminiClone.sortingOrder = 1
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 3
	End Sub

	' Token: 0x06002068 RID: 8296 RVA: 0x00128E3D File Offset: 0x0012723D
	Private Sub SwitchCloneTopLayer()
		Me.geminiClone.sortingOrder = 3
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = 1
	End Sub

	' Token: 0x06002069 RID: 8297 RVA: 0x00128E57 File Offset: 0x00127257
	Private Sub SwitchSphereLayer(layer As Integer)
		Me.sphere.sortingOrder = layer
	End Sub

	' Token: 0x0600206A RID: 8298 RVA: 0x00128E68 File Offset: 0x00127268
	Public Sub SummonTornado()
		Dim tornado As LevelProperties.FlyingBlimp.Tornado = MyBase.properties.CurrentState.tornado
		Me.tornado = Global.UnityEngine.[Object].Instantiate(Of FlyingBlimpLevelTornado)(Me.tornadoPrefab)
		Me.tornado.Init(Me.projectileRoot.transform.position, PlayerManager.GetNext(), tornado)
	End Sub

	' Token: 0x0600206B RID: 8299 RVA: 0x00128EBD File Offset: 0x001272BD
	Private Sub MoveTornado()
		If Me.tornadoPrefab IsNot Nothing Then
			MyBase.StartCoroutine(Me.tornado.move_cr())
		End If
	End Sub

	' Token: 0x0600206C RID: 8300 RVA: 0x00128EE2 File Offset: 0x001272E2
	Public Sub StartTornado()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.tornado_cr())
	End Sub

	' Token: 0x0600206D RID: 8301 RVA: 0x00128F10 File Offset: 0x00127310
	Public Iterator Function tornado_cr() As IEnumerator
		Me.state = FlyingBlimpLevelBlimpLady.State.Tornado
		Dim p As LevelProperties.FlyingBlimp.Tornado = MyBase.properties.CurrentState.tornado
		Me.moving = False
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		AudioManager.[Stop]("level_flying_blimp_pedal_loop")
		AudioManager.Play("level_flying_blimp_tornado")
		Me.SummonTornado()
		MyBase.animator.Play("Tornado_Start")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Tornado_Start", False, True)
		Yield CupheadTime.WaitForSeconds(Me, p.loopDuration)
		MyBase.animator.SetTrigger("FinishTornado")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Tornado_Finish", False, True)
		AudioManager.PlayLoop("level_flying_blimp_pedal_loop")
		Me.moving = True
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateAfterAttack)
		Me.state = FlyingBlimpLevelBlimpLady.State.Idle
		Return
	End Function

	' Token: 0x0600206E RID: 8302 RVA: 0x00128F2B File Offset: 0x0012732B
	Public Sub StartShoot()
		If Me.patternCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.patternCoroutine)
		End If
		Me.patternCoroutine = MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x0600206F RID: 8303 RVA: 0x00128F58 File Offset: 0x00127358
	Private Iterator Function shoot_cr() As IEnumerator
		Me.state = FlyingBlimpLevelBlimpLady.State.Shoot
		Dim p As LevelProperties.FlyingBlimp.Shoot = MyBase.properties.CurrentState.shoot
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		AudioManager.Play("level_flying_blimp_fire")
		MyBase.animator.Play("Shoot_Start")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Shoot_Start", False, True)
		Me.spawnProjectile()
		Yield CupheadTime.WaitForSeconds(Me, 0.7F)
		Yield CupheadTime.WaitForSeconds(Me, p.hesitateAfterAttackRange.RandomFloat())
		Me.state = FlyingBlimpLevelBlimpLady.State.Idle
		Return
	End Function

	' Token: 0x06002070 RID: 8304 RVA: 0x00128F73 File Offset: 0x00127373
	Private Sub spawnProjectile()
		Me.shootProjectilePrefab.Create(Me.projectileRoot.position, 0F, MyBase.properties.CurrentState.shoot)
	End Sub

	' Token: 0x06002071 RID: 8305 RVA: 0x00128FA8 File Offset: 0x001273A8
	Private Sub SummonEnemy(prefab As FlyingBlimpLevelEnemy, startPoint As Vector3, stopPoint As Single, type As Boolean)
		Dim flyingBlimpLevelEnemy As FlyingBlimpLevelEnemy = Global.UnityEngine.[Object].Instantiate(Of FlyingBlimpLevelEnemy)(prefab)
		Dim vector As Vector2 = flyingBlimpLevelEnemy.transform.position
		vector.y = 360F - startPoint.y
		vector.x = 740F
		stopPoint = MyBase.properties.CurrentState.enemy.stopDistance.RandomFloat()
		flyingBlimpLevelEnemy.transform.position = vector
		flyingBlimpLevelEnemy.Init(MyBase.properties, startPoint, stopPoint, type, Me)
	End Sub

	' Token: 0x06002072 RID: 8306 RVA: 0x0012902C File Offset: 0x0012742C
	Public Iterator Function spawnEnemy_cr() As IEnumerator
		Dim p As LevelProperties.FlyingBlimp.Enemy = MyBase.properties.CurrentState.enemy
		Dim spawnPattern As String() = p.spawnString.GetRandom().Split(New Char() { ","c })
		Dim typePattern As String() = p.typeString.GetRandom().Split(New Char() { ","c })
		Dim AParryable As Boolean = False
		Dim waitTime As Single = 0F
		Dim counter As Integer = 0
		Dim typeIndex As Integer = 0
		Dim spawnIndex As Integer = Global.UnityEngine.Random.Range(0, spawnPattern.Length)
		Dim spawnPos As Vector3 = Vector3.zero
		While True
			For i As Integer = spawnIndex To spawnPattern.Length - 1
				If waitTime > 0F Then
					Yield CupheadTime.WaitForSeconds(Me, waitTime)
				End If
				If spawnPattern(i)(0) = "D"c Then
					Parser.FloatTryParse(spawnPattern(i).Substring(1), waitTime)
				Else
					Dim array As String() = spawnPattern(i).Split(New Char() { "-"c })
					For Each text As String In array
						Dim num As Single = 0F
						Dim num2 As Single = 0F
						Parser.FloatTryParse(text, num)
						Parser.FloatTryParse(text, num2)
						Dim flyingBlimpLevelEnemy As FlyingBlimpLevelEnemy = Nothing
						If typePattern(typeIndex)(0) = "A"c Then
							flyingBlimpLevelEnemy = Me.enemyPrefabA
							If CSng(counter) >= p.APinkOccurance.RandomFloat() Then
								AParryable = True
								counter = 0
							Else
								AParryable = False
								counter += 1
							End If
						ElseIf typePattern(typeIndex)(0) = "B"c Then
							flyingBlimpLevelEnemy = Me.enemyPrefabB
							AParryable = False
						End If
						spawnPos.y = num
						If Me.state <> FlyingBlimpLevelBlimpLady.State.Death Then
							Me.SummonEnemy(flyingBlimpLevelEnemy, spawnPos, num2, AParryable)
						End If
						typeIndex = (typeIndex + 1) Mod typePattern.Length
					Next
					waitTime = p.stringDelay
				End If
				i = i Mod spawnPattern.Length
			Next
			spawnIndex = 0
		End While
		Return
	End Function

	' Token: 0x06002073 RID: 8307 RVA: 0x00129047 File Offset: 0x00127447
	Public Sub StartDeath()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x06002074 RID: 8308 RVA: 0x0012905C File Offset: 0x0012745C
	Private Iterator Function die_cr() As IEnumerator
		MyBase.animator.SetTrigger("Death")
		Me.moving = False
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield Nothing
		Return
	End Function

	' Token: 0x06002075 RID: 8309 RVA: 0x00129077 File Offset: 0x00127477
	Public Sub SpawnMoonLady()
		MyBase.StartCoroutine(Me.spawn_moon_lady_cr())
	End Sub

	' Token: 0x06002076 RID: 8310 RVA: 0x00129088 File Offset: 0x00127488
	Private Iterator Function spawn_moon_lady_cr() As IEnumerator
		While Me.angle > 0.2617994F OrElse Me.angle < -0.2617994F
			Yield Nothing
		End While
		Me.moving = False
		Me.moonLady.StartIntro()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002077 RID: 8311 RVA: 0x001290A3 File Offset: 0x001274A3
	Private Sub SagAttackSFX()
		AudioManager.Play("level_flying_blimp_sagittarius_attack")
		Me.emitAudioFromObject.Add("level_flying_blimp_sagittarius_attack")
	End Sub

	' Token: 0x06002078 RID: 8312 RVA: 0x001290BF File Offset: 0x001274BF
	Private Sub TaurusIdleSFX()
		AudioManager.Play("level_flying_blimp_taurus_idle")
		Me.emitAudioFromObject.Add("level_flying_blimp_taurus_idle")
	End Sub

	' Token: 0x040028BD RID: 10429
	<Header("Phase Materials")>
	<SerializeField()>
	Private blimpMat As Material

	' Token: 0x040028BE RID: 10430
	<SerializeField()>
	Private taurusMat As Material

	' Token: 0x040028BF RID: 10431
	<SerializeField()>
	Private sagittariusMat As Material

	' Token: 0x040028C0 RID: 10432
	<SerializeField()>
	Private geminiMat As Material

	' Token: 0x040028C1 RID: 10433
	<Space(10F)>
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x040028C2 RID: 10434
	<SerializeField()>
	Private transformationPoint As Transform

	' Token: 0x040028C3 RID: 10435
	<SerializeField()>
	Private dashExplosionEffect As Effect

	' Token: 0x040028C4 RID: 10436
	<SerializeField()>
	Private cloudEffect As GameObject

	' Token: 0x040028C5 RID: 10437
	<SerializeField()>
	Private bigCloud As GameObject

	' Token: 0x040028C6 RID: 10438
	<SerializeField()>
	Private constellationHandler As SpriteRenderer

	' Token: 0x040028C7 RID: 10439
	<SerializeField()>
	Private blackDim As SpriteRenderer

	' Token: 0x040028C8 RID: 10440
	<SerializeField()>
	Private enemyPrefabA As FlyingBlimpLevelEnemy

	' Token: 0x040028C9 RID: 10441
	<SerializeField()>
	Private enemyPrefabB As FlyingBlimpLevelEnemy

	' Token: 0x040028CA RID: 10442
	<SerializeField()>
	Private tornadoPrefab As FlyingBlimpLevelTornado

	' Token: 0x040028CB RID: 10443
	Private tornado As FlyingBlimpLevelTornado

	' Token: 0x040028CC RID: 10444
	<SerializeField()>
	Private shootProjectilePrefab As FlyingBlimpLevelShootProjectile

	' Token: 0x040028CD RID: 10445
	<SerializeField()>
	Private sagittariusStarPrefab As FlyingBlimpLevelArrowProjectile

	' Token: 0x040028CE RID: 10446
	<SerializeField()>
	Private sagittariusArrowPrefab As BasicProjectile

	' Token: 0x040028CF RID: 10447
	<SerializeField()>
	Private geminiObjectPrefab As FlyingBlimpLevelGeminiShoot

	' Token: 0x040028D0 RID: 10448
	Private geminiObject As FlyingBlimpLevelGeminiShoot

	' Token: 0x040028D1 RID: 10449
	<SerializeField()>
	Private geminiClone As SpriteRenderer

	' Token: 0x040028D2 RID: 10450
	<SerializeField()>
	Private sphere As SpriteRenderer

	' Token: 0x040028D3 RID: 10451
	<SerializeField()>
	Private objectSpawnRoot As FlyingBlimpLevelSpawnRadius

	' Token: 0x040028D4 RID: 10452
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x040028D5 RID: 10453
	<SerializeField()>
	Private arrowRoot As Transform

	' Token: 0x040028D6 RID: 10454
	<SerializeField()>
	Private arrowEffectRoot As Transform

	' Token: 0x040028D7 RID: 10455
	<SerializeField()>
	Private moonLady As FlyingBlimpLevelMoonLady

	' Token: 0x040028D8 RID: 10456
	<SerializeField()>
	Private explosionOffset As Vector2 = Vector2.zero

	' Token: 0x040028D9 RID: 10457
	<SerializeField()>
	Private arrowEffect As Effect

	' Token: 0x040028DA RID: 10458
	<SerializeField()>
	Private explosionRadius As Single = 100F

	' Token: 0x040028DB RID: 10459
	Private angle As Single

	' Token: 0x040028DC RID: 10460
	Private originalSpeed As Single

	' Token: 0x040028DD RID: 10461
	Private loopSize As Single = 80F

	' Token: 0x040028DE RID: 10462
	Private movementSpeed As Single

	' Token: 0x040028DF RID: 10463
	Private waitLoopTime As Single

	' Token: 0x040028E0 RID: 10464
	Private startPos As Vector3

	' Token: 0x040028E1 RID: 10465
	Private pivotOffset As Vector3

	' Token: 0x040028E2 RID: 10466
	Private getPos As Vector3

	' Token: 0x040028E3 RID: 10467
	Private geminiTarget As Vector2

	' Token: 0x040028E4 RID: 10468
	Private invert As Boolean

	' Token: 0x040028E5 RID: 10469
	Private isLooping As Boolean

	' Token: 0x040028E6 RID: 10470
	Private smallClouds As Boolean

	' Token: 0x040028E7 RID: 10471
	Private dashExplosions As Boolean

	' Token: 0x040028E8 RID: 10472
	Private transitionToSummon As Boolean = True

	' Token: 0x040028E9 RID: 10473
	Private damageDealer As DamageDealer

	' Token: 0x040028EA RID: 10474
	Private damageReceiver As DamageReceiver

	' Token: 0x040028EB RID: 10475
	Private patternCoroutine As Coroutine

	' Token: 0x040028EC RID: 10476
	Private constellation As FlyingBlimpLevelBlimpLady.constellationPossibility

	' Token: 0x02000630 RID: 1584
	Public Enum State
		' Token: 0x040028EE RID: 10478
		Intro
		' Token: 0x040028EF RID: 10479
		Idle
		' Token: 0x040028F0 RID: 10480
		Dash
		' Token: 0x040028F1 RID: 10481
		Tornado
		' Token: 0x040028F2 RID: 10482
		Shoot
		' Token: 0x040028F3 RID: 10483
		Taurus
		' Token: 0x040028F4 RID: 10484
		Sagittarius
		' Token: 0x040028F5 RID: 10485
		Gemini
		' Token: 0x040028F6 RID: 10486
		Death
	End Enum

	' Token: 0x02000631 RID: 1585
	Public Enum constellationPossibility
		' Token: 0x040028F8 RID: 10488
		Taurus = 1
		' Token: 0x040028F9 RID: 10489
		Sagittarius
		' Token: 0x040028FA RID: 10490
		Gemini
	End Enum
End Class
