Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007CC RID: 1996
Public Class SaltbakerLevelJumper
	Inherits AbstractProjectile

	' Token: 0x06002D4A RID: 11594 RVA: 0x001AAB40 File Offset: 0x001A8F40
	Public Function Create(position As Vector3, parent As SaltbakerLevel, swooperProperties As LevelProperties.Saltbaker.Swooper, jumperProperties As LevelProperties.Saltbaker.Jumper, firstDelay As Single, isSwooper As Boolean) As SaltbakerLevelJumper
		Dim saltbakerLevelJumper As SaltbakerLevelJumper = Me.InstantiatePrefab(Of SaltbakerLevelJumper)()
		saltbakerLevelJumper.transform.position = position
		If isSwooper Then
			saltbakerLevelJumper.transform.position += Vector3.up * -94F
		End If
		saltbakerLevelJumper.count = If((Not isSwooper), jumperProperties.numberFireJumpers, swooperProperties.numberFireSwoopers)
		saltbakerLevelJumper.apexHeight = If((Not isSwooper), (jumperProperties.apexHeight - 68F), (swooperProperties.apexHeight + -94F))
		saltbakerLevelJumper.apexTime = If((Not isSwooper), jumperProperties.apexTime, swooperProperties.apexTime)
		saltbakerLevelJumper.initialFallDelay = If((Not isSwooper), jumperProperties.initialFallDelay, swooperProperties.initialFallDelay)
		saltbakerLevelJumper.jumpDelay = If((Not isSwooper), jumperProperties.jumpDelay, swooperProperties.jumpDelay)
		saltbakerLevelJumper.levelEdgeOffset = If((Not isSwooper), 260F, 75F)
		saltbakerLevelJumper.parent = parent
		saltbakerLevelJumper.firstDelay = firstDelay
		saltbakerLevelJumper.isSwooper = isSwooper
		saltbakerLevelJumper.coll = saltbakerLevelJumper.GetComponent(Of CircleCollider2D)()
		saltbakerLevelJumper.FXbottom.transform.parent = Nothing
		saltbakerLevelJumper.aimPosition = saltbakerLevelJumper.transform.position
		If saltbakerLevelJumper.isSwooper Then
			saltbakerLevelJumper.StartCoroutine(saltbakerLevelJumper.arc_cr())
		Else
			saltbakerLevelJumper.GetComponent(Of SpriteRenderer)().sortingLayerName = "Player"
			saltbakerLevelJumper.StartCoroutine(saltbakerLevelJumper.fall_cr())
		End If
		Return saltbakerLevelJumper
	End Function

	' Token: 0x06002D4B RID: 11595 RVA: 0x001AACCF File Offset: 0x001A90CF
	Protected Overrides Sub OnDieLifetime()
	End Sub

	' Token: 0x06002D4C RID: 11596 RVA: 0x001AACD1 File Offset: 0x001A90D1
	Public Function GetAimPos() As Vector3
		Return Me.aimPosition
	End Function

	' Token: 0x06002D4D RID: 11597 RVA: 0x001AACD9 File Offset: 0x001A90D9
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002D4E RID: 11598 RVA: 0x001AACF8 File Offset: 0x001A90F8
	Private Iterator Function arc_cr() As IEnumerator
		Dim animHelper As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		If Me.isSwooper Then
			MyBase.animator.Play("SwooperIntro")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "SwooperIntro", False, True)
		Else
			Me.FXbottom.GetComponent(Of SpriteRenderer)().sortingLayerName = "Player"
			Me.FXbottom.GetComponent(Of SpriteRenderer)().sortingOrder = -2
		End If
		Dim root As Single = CSng((Level.Current.Left + Level.Current.Right)) / 2F
		Yield CupheadTime.WaitForSeconds(Me, Me.firstDelay)
		While Not Me.dead
			If Me.isSwooper Then
				MyBase.animator.Play("SwooperAntic")
				While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.888F
					Yield Nothing
				End While
			End If
			Dim t As Single = 0F
			Dim endPosY As Single = If((Not Me.isSwooper), (CSng(Level.Current.Ground) + 68F), (CupheadLevelCamera.Current.Bounds.yMax + -94F))
			Me.aimPosition = New Vector3(Mathf.Clamp(PlayerManager.GetNext().center.x, CSng(Level.Current.Left) + Me.levelEdgeOffset, CSng(Level.Current.Right) - Me.levelEdgeOffset), endPosY)
			Dim offset As Single = Mathf.Sign(root - Me.aimPosition.x) * Me.coll.bounds.size.x
			Dim foundPos As Boolean = False
			While Not foundPos
				If Me.aimPosition.x < CSng(Level.Current.Left) + Me.levelEdgeOffset OrElse Me.aimPosition.x > CSng(Level.Current.Right) - Me.levelEdgeOffset Then
					Me.aimPosition = MyBase.transform.position
					foundPos = True
				ElseIf Me.parent.IsPositionAvailable(Me.aimPosition, Me) Then
					foundPos = True
				Else
					Me.aimPosition.x = Me.aimPosition.x + offset
				End If
			End While
			Dim x As Single = Me.aimPosition.x - MyBase.transform.position.x
			Dim y As Single = Me.aimPosition.y - MyBase.transform.position.y
			Dim apexTime2 As Single = Me.apexTime * Me.apexTime
			Dim g As Single = -2F * Me.apexHeight / apexTime2
			Dim viY As Single = 2F * Me.apexHeight / Me.apexTime
			Dim viX2 As Single = viY * viY
			Dim sqrtRooted As Single = viX2 + 2F * g * y
			Dim tEnd As Single = (-viY + Mathf.Sqrt(sqrtRooted)) / g
			Dim tEnd2 As Single = (-viY - Mathf.Sqrt(sqrtRooted)) / g
			Dim tEnd3 As Single = Mathf.Max(tEnd, tEnd2)
			Dim velocityX As Single = x / tEnd3
			If Me.isSwooper Then
				viY = -viY
			End If
			Dim vel As Vector3 = New Vector3(velocityX, viY)
			MyBase.animator.SetInteger("ArcWidth", Mathf.Clamp(CInt((Mathf.Abs(velocityX) / 250F)), 0, 2))
			Dim jumpLoopHash As Integer = Animator.StringToHash(MyBase.animator.GetLayerName(0) + "." + If((Not Me.isSwooper), "Jumper", "Swooper") + "JumpLoop")
			Dim animTimeOnLand As Single = -1F
			MyBase.animator.SetInteger("Variant", Global.UnityEngine.Random.Range(0, 2))
			If Me.isSwooper Then
				MyBase.transform.localScale = New Vector3(Mathf.Sign(velocityX), 1F)
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "SwooperAntic", False, True)
				tEnd3 -= 0.375F
			Else
				MyBase.animator.SetTrigger("StartJumperAntic")
				Yield MyBase.animator.WaitForAnimationToStart(Me, "JumperAntic", False)
				MyBase.transform.localScale = New Vector3(Mathf.Sign(-velocityX), 1F)
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "JumperAntic", False, True)
			End If
			Me.FXbottom.transform.position = MyBase.transform.position + Vector3.up * If((Not Me.isSwooper), (-20F), 27F)
			Me.FXbottom.transform.localScale = New Vector3(MyBase.transform.localScale.x, CSng(If((Not Me.isSwooper), 1, (-1))))
			Me.FXbottom.Play(Me.FXanimNames(MyBase.animator.GetInteger("ArcWidth")), 0, 0F)
			Dim stillMoving As Boolean = True
			Dim wait As YieldInstruction = New WaitForFixedUpdate()
			While stillMoving
				If animTimeOnLand < 0F AndAlso MyBase.animator.GetCurrentAnimatorStateInfo(0).fullPathHash = jumpLoopHash Then
					Dim num As Single = tEnd3 / 0.41666666F
					animTimeOnLand = (num + MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime) Mod 1F
					Dim num2 As Single = animTimeOnLand - If((Not Me.isSwooper), 0.75F, 0.65F)
					Dim num3 As Single = num - num2
					animHelper.Speed = num3 / num
				End If
				If Me.isSwooper Then
					vel.y -= g * CupheadTime.FixedDelta
				Else
					vel.y += g * CupheadTime.FixedDelta
				End If
				MyBase.transform.Translate(vel * CupheadTime.FixedDelta)
				tEnd3 -= CupheadTime.FixedDelta
				Yield wait
				t += CupheadTime.FixedDelta
				If t > Me.apexTime Then
					If Me.isSwooper Then
						If MyBase.transform.position.y >= endPosY Then
							stillMoving = False
						End If
						If tEnd3 <= 0F Then
							MyBase.animator.SetBool("EndJump", True)
							animHelper.Speed = 1F
						End If
					ElseIf MyBase.transform.position.y <= endPosY Then
						stillMoving = False
						MyBase.animator.SetBool("EndJump", True)
						animHelper.Speed = 1F
					End If
				End If
			End While
			MyBase.transform.SetPosition(Nothing, New Single?(endPosY), Nothing)
			If Not Me.isSwooper Then
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "JumperJumpLoop", False, True)
			End If
			MyBase.animator.SetBool("EndJump", False)
			If Not Me.isSwooper Then
				Yield MyBase.animator.WaitForAnimationToStart(Me, "JumperIdle", False)
			End If
			If Not Me.dead Then
				Yield CupheadTime.WaitForSeconds(Me, Me.jumpDelay)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002D4F RID: 11599 RVA: 0x001AAD14 File Offset: 0x001A9114
	Private Sub AniEvent_FlipX()
		MyBase.transform.localScale = New Vector3(-MyBase.transform.localScale.x, 1F)
	End Sub

	' Token: 0x06002D50 RID: 11600 RVA: 0x001AAD4C File Offset: 0x001A914C
	Private Iterator Function fall_cr() As IEnumerator
		MyBase.animator.Play("JumperFallLoop")
		Dim endPosY As Single = CSng(Level.Current.Ground) + 68F
		Dim apexTime2 As Single = Me.apexTime * Me.apexTime
		Dim g As Single = -2F * Me.apexHeight / apexTime2
		Dim vel As Vector3 = Vector3.zero
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.y > 200F
			vel.y += g * CupheadTime.FixedDelta
			MyBase.transform.Translate(vel * CupheadTime.FixedDelta)
			Yield wait
		End While
		MyBase.animator.SetTrigger("StartJumperIntro")
		While MyBase.transform.position.y > endPosY
			Yield wait
			vel.y += g * CupheadTime.FixedDelta
			MyBase.transform.Translate(vel * CupheadTime.FixedDelta)
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(endPosY), Nothing)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "JumperIdle", False)
		MyBase.StartCoroutine(Me.arc_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002D51 RID: 11601 RVA: 0x001AAD67 File Offset: 0x001A9167
	Public Sub Die()
		Me.dead = True
		MyBase.animator.SetTrigger("Die")
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06002D52 RID: 11602 RVA: 0x001AAD8C File Offset: 0x001A918C
	Public Sub AniEvent_DeathComplete()
		Global.UnityEngine.[Object].Destroy(Me.FXbottom.gameObject)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002D53 RID: 11603 RVA: 0x001AADA9 File Offset: 0x001A91A9
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		If Me.coll IsNot Nothing Then
			Gizmos.DrawWireSphere(Me.aimPosition, Me.coll.radius)
		End If
	End Sub

	' Token: 0x040035C6 RID: 13766
	Private Const SWOOPER_DIVE_LENGTH As Single = 0.375F

	' Token: 0x040035C7 RID: 13767
	Private Const JUMPER_INTRO_LENGTH As Single = 0.41666666F

	' Token: 0x040035C8 RID: 13768
	Private Const JUMPER_GROUND_OFFSET As Single = 68F

	' Token: 0x040035C9 RID: 13769
	Private Const SWOOPER_CEILING_OFFSET As Single = -94F

	' Token: 0x040035CA RID: 13770
	Private Const SWOOPER_FX_OFFSET As Single = 27F

	' Token: 0x040035CB RID: 13771
	Private Const JUMPER_ENTRANCE_Y_POS As Single = 200F

	' Token: 0x040035CC RID: 13772
	Private Const JUMP_LOOP_LENGTH As Single = 0.41666666F

	' Token: 0x040035CD RID: 13773
	Private Const SWOOPER_LOOP_EXIT_TIME As Single = 0.65F

	' Token: 0x040035CE RID: 13774
	Private Const JUMPER_LOOP_EXIT_TIME As Single = 0.75F

	' Token: 0x040035CF RID: 13775
	Private Const LEVEL_EDGE_OFFSET_SWOOPER As Single = 75F

	' Token: 0x040035D0 RID: 13776
	Private Const LEVEL_EDGE_OFFSET_JUMPER As Single = 260F

	' Token: 0x040035D1 RID: 13777
	<SerializeField()>
	Private FXbottom As Animator

	' Token: 0x040035D2 RID: 13778
	Private FXanimNames As String() = New String() { "Thin", "Medium", "Wide" }

	' Token: 0x040035D3 RID: 13779
	Protected aimPosition As Vector3

	' Token: 0x040035D4 RID: 13780
	Private levelEdgeOffset As Single = 75F

	' Token: 0x040035D5 RID: 13781
	Protected parent As SaltbakerLevel

	' Token: 0x040035D6 RID: 13782
	Protected firstDelay As Single

	' Token: 0x040035D7 RID: 13783
	Protected isSwooper As Boolean

	' Token: 0x040035D8 RID: 13784
	Private coll As CircleCollider2D

	' Token: 0x040035D9 RID: 13785
	Private count As Integer

	' Token: 0x040035DA RID: 13786
	Private apexHeight As Single

	' Token: 0x040035DB RID: 13787
	Private apexTime As Single

	' Token: 0x040035DC RID: 13788
	Private initialFallDelay As Single

	' Token: 0x040035DD RID: 13789
	Private jumpDelay As Single

	' Token: 0x040035DE RID: 13790
	Private dead As Boolean
End Class
