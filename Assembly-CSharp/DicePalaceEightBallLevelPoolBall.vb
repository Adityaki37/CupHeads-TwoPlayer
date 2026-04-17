Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005C0 RID: 1472
Public Class DicePalaceEightBallLevelPoolBall
	Inherits AbstractProjectile

	' Token: 0x06001CA4 RID: 7332 RVA: 0x00105FA0 File Offset: 0x001043A0
	Public Function Create(pos As Vector2, horSpeed As Single, verSpeed As Single, gravity As Single, delay As Single, onLeft As Boolean, parent As DicePalaceEightBallLevelEightBall) As DicePalaceEightBallLevelPoolBall
		Dim dicePalaceEightBallLevelPoolBall As DicePalaceEightBallLevelPoolBall = TryCast(MyBase.Create(), DicePalaceEightBallLevelPoolBall)
		dicePalaceEightBallLevelPoolBall.transform.position = pos
		dicePalaceEightBallLevelPoolBall.horSpeed = horSpeed
		dicePalaceEightBallLevelPoolBall.verSpeed = verSpeed
		dicePalaceEightBallLevelPoolBall.gravity = gravity
		dicePalaceEightBallLevelPoolBall.delay = delay
		dicePalaceEightBallLevelPoolBall.onLeft = onLeft
		dicePalaceEightBallLevelPoolBall.parent = parent
		Return dicePalaceEightBallLevelPoolBall
	End Function

	' Token: 0x06001CA5 RID: 7333 RVA: 0x00105FFC File Offset: 0x001043FC
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.shadowInstance = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.shadowPrefab).transform
		Me.shadowInstance.gameObject.SetActive(False)
		Me.dustInstance = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.dustPrefab).transform
		Me.shadowInstance.gameObject.SetActive(False)
		MyBase.StartCoroutine(Me.jump_cr())
		MyBase.StartCoroutine(Me.check_dying_cr())
		Dim dicePalaceEightBallLevelEightBall As DicePalaceEightBallLevelEightBall = Me.parent
		dicePalaceEightBallLevelEightBall.OnEightBallDeath = CType([Delegate].Combine(dicePalaceEightBallLevelEightBall.OnEightBallDeath, AddressOf Me.EightBallDead), Action)
	End Sub

	' Token: 0x06001CA6 RID: 7334 RVA: 0x001060A0 File Offset: 0x001044A0
	Public Sub SetVariation(index As Integer)
		For i As Integer = 0 To Me.colorVariations.Length - 1
			Me.colorVariations(i).SetActive(False)
		Next
		If index >= 0 AndAlso index < Me.colorVariations.Length Then
			Me.colorVariations(index).SetActive(True)
		End If
	End Sub

	' Token: 0x06001CA7 RID: 7335 RVA: 0x001060F7 File Offset: 0x001044F7
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001CA8 RID: 7336 RVA: 0x00106115 File Offset: 0x00104515
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001CA9 RID: 7337 RVA: 0x00106134 File Offset: 0x00104534
	Private Iterator Function jump_cr() As IEnumerator
		Dim jumping As Boolean = False
		Dim goingUp As Boolean = False
		Dim upsideDown As Boolean = False
		Dim velocityY As Single = Me.verSpeed
		Dim velocityX As Single = Me.horSpeed
		Dim ground As Single = CSng(Level.Current.Ground) + 55F
		Me.dustInstance.gameObject.SetActive(False)
		While MyBase.transform.position.y > ground
			velocityY -= Me.gravity / 2F * CupheadTime.Delta
			MyBase.transform.AddPosition(0F, velocityY * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Dim p As Vector3 = MyBase.transform.position
		p.y = ground
		MyBase.transform.position = p
		Me.dustInstance.position = MyBase.transform.position
		Me.dustInstance.gameObject.SetActive(True)
		MyBase.animator.SetTrigger("Smash")
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.delay)
			jumping = True
			goingUp = True
			velocityY = Me.verSpeed
			velocityX = If((Not Me.onLeft), (-Me.horSpeed), Me.horSpeed)
			MyBase.animator.SetTrigger("Jump")
			Me.shadowInstance.gameObject.SetActive(False)
			If upsideDown Then
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "UpsideDownJump", True, True)
			Else
				Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jump", True, True)
			End If
			Me.shadowInstance.gameObject.SetActive(True)
			Me.dustInstance.gameObject.SetActive(False)
			While jumping
				Me.shadowInstance.position = New Vector3(MyBase.transform.position.x, ground, 0F)
				velocityY -= Me.gravity * CupheadTime.Delta
				MyBase.transform.AddPosition(velocityX * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0F)
				If velocityY < 0F AndAlso goingUp Then
					MyBase.animator.SetTrigger("Turn")
					goingUp = False
					If upsideDown Then
						Yield MyBase.animator.WaitForAnimationToEnd(Me, "RightSideUpSmash_start", True, True)
					Else
						Yield MyBase.animator.WaitForAnimationToEnd(Me, "JumpTurn", True, True)
					End If
				End If
				If velocityY < 0F AndAlso jumping AndAlso MyBase.transform.position.y <= ground Then
					MyBase.animator.SetTrigger("Smash")
					jumping = False
					upsideDown = Not upsideDown
					Dim position As Vector3 = MyBase.transform.position
					position.y = ground
					MyBase.transform.position = position
					Me.dustInstance.position = MyBase.transform.position
					Me.dustInstance.gameObject.SetActive(True)
				End If
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x06001CAA RID: 7338 RVA: 0x00106150 File Offset: 0x00104550
	Private Iterator Function check_dying_cr() As IEnumerator
		While True
			If Me.onLeft Then
				If MyBase.transform.position.x > 840F Then
					Exit For
				End If
			ElseIf MyBase.transform.position.x < -840F Then
				Exit For
			End If
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001CAB RID: 7339 RVA: 0x0010616B File Offset: 0x0010456B
	Private Sub EightBallDead()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.eight_ball_death_cr())
	End Sub

	' Token: 0x06001CAC RID: 7340 RVA: 0x00106180 File Offset: 0x00104580
	Private Iterator Function eight_ball_death_cr() As IEnumerator
		Dim speed As Single = 2500F
		Dim angle As Single = CSng(Global.UnityEngine.Random.Range(0, 360))
		Dim dir As Vector3 = MathUtils.AngleToDirection(angle)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		While True
			MyBase.transform.position += dir * speed * CupheadTime.FixedDelta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001CAD RID: 7341 RVA: 0x0010619C File Offset: 0x0010459C
	Protected Overrides Sub OnDestroy()
		If Me.shadowInstance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.shadowInstance.gameObject)
		End If
		If Me.dustInstance IsNot Nothing Then
			Global.UnityEngine.[Object].Destroy(Me.dustInstance.gameObject)
		End If
		MyBase.OnDestroy()
		Me.shadowPrefab = Nothing
		Me.dustPrefab = Nothing
	End Sub

	' Token: 0x06001CAE RID: 7342 RVA: 0x001061FF File Offset: 0x001045FF
	Protected Overrides Sub Die()
		MyBase.Die()
		Dim dicePalaceEightBallLevelEightBall As DicePalaceEightBallLevelEightBall = Me.parent
		dicePalaceEightBallLevelEightBall.OnEightBallDeath = CType([Delegate].Remove(dicePalaceEightBallLevelEightBall.OnEightBallDeath, AddressOf Me.EightBallDead), Action)
	End Sub

	' Token: 0x0400258D RID: 9613
	Private Const OffsetY As Single = 55F

	' Token: 0x0400258E RID: 9614
	<SerializeField()>
	Private shadowPrefab As GameObject

	' Token: 0x0400258F RID: 9615
	<SerializeField()>
	Private dustPrefab As GameObject

	' Token: 0x04002590 RID: 9616
	<SerializeField()>
	Private colorVariations As GameObject()

	' Token: 0x04002591 RID: 9617
	Private parent As DicePalaceEightBallLevelEightBall

	' Token: 0x04002592 RID: 9618
	Private horSpeed As Single

	' Token: 0x04002593 RID: 9619
	Private verSpeed As Single

	' Token: 0x04002594 RID: 9620
	Private gravity As Single

	' Token: 0x04002595 RID: 9621
	Private delay As Single

	' Token: 0x04002596 RID: 9622
	Private onLeft As Boolean

	' Token: 0x04002597 RID: 9623
	Private shadowInstance As Transform

	' Token: 0x04002598 RID: 9624
	Private dustInstance As Transform
End Class
