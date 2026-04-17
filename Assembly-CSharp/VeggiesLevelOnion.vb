Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200084B RID: 2123
Public Class VeggiesLevelOnion
	Inherits LevelProperties.Veggies.Entity

	' Token: 0x17000427 RID: 1063
	' (get) Token: 0x06003128 RID: 12584 RVA: 0x001CD074 File Offset: 0x001CB474
	' (set) Token: 0x06003129 RID: 12585 RVA: 0x001CD07C File Offset: 0x001CB47C
	Public Property state As VeggiesLevelOnion.State

	' Token: 0x17000428 RID: 1064
	' (get) Token: 0x0600312A RID: 12586 RVA: 0x001CD085 File Offset: 0x001CB485
	' (set) Token: 0x0600312B RID: 12587 RVA: 0x001CD08D File Offset: 0x001CB48D
	Public Property HappyLeave As Boolean

	' Token: 0x1400005D RID: 93
	' (add) Token: 0x0600312C RID: 12588 RVA: 0x001CD098 File Offset: 0x001CB498
	' (remove) Token: 0x0600312D RID: 12589 RVA: 0x001CD0D0 File Offset: 0x001CB4D0
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As VeggiesLevelOnion.OnDamageTakenHandler

	' Token: 0x1400005E RID: 94
	' (add) Token: 0x0600312E RID: 12590 RVA: 0x001CD108 File Offset: 0x001CB508
	' (remove) Token: 0x0600312F RID: 12591 RVA: 0x001CD140 File Offset: 0x001CB540
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnHappyLeave As Action

	' Token: 0x06003130 RID: 12592 RVA: 0x001CD178 File Offset: 0x001CB578
	Private Sub Start()
		If Me.properties Is Nothing Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			Return
		End If
		Me.noSecret = True
		Me.circleCollider = MyBase.GetComponent(Of CircleCollider2D)()
		Me.state = VeggiesLevelOnion.State.Idle
		MyBase.StartCoroutine(Me.happyTimer_cr())
		Me.SfxGround()
	End Sub

	' Token: 0x06003131 RID: 12593 RVA: 0x001CD1C9 File Offset: 0x001CB5C9
	Private Sub SfxGround()
		AudioManager.Play("level_veggies_onion_rise")
	End Sub

	' Token: 0x06003132 RID: 12594 RVA: 0x001CD1D5 File Offset: 0x001CB5D5
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003133 RID: 12595 RVA: 0x001CD1F0 File Offset: 0x001CB5F0
	Public Overrides Sub LevelInitWithGroup(propertyGroup As AbstractLevelPropertyGroup)
		MyBase.LevelInitWithGroup(propertyGroup)
		Me.properties = TryCast(propertyGroup, LevelProperties.Veggies.Onion)
		Me.hp = CSng(Me.properties.hp)
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = New DamageDealer(1F, 0.2F, True, False, False)
		Me.damageDealer.SetDirection(DamageDealer.Direction.Neutral, MyBase.transform)
	End Sub

	' Token: 0x06003134 RID: 12596 RVA: 0x001CD264 File Offset: 0x001CB664
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.state = VeggiesLevelOnion.State.Idle Then
			Me.state = VeggiesLevelOnion.State.Crying
			Me.noSecret = False
			MyBase.animator.SetTrigger("SadStart")
			Return
		End If
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06003135 RID: 12597 RVA: 0x001CD2E0 File Offset: 0x001CB6E0
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003136 RID: 12598 RVA: 0x001CD2FE File Offset: 0x001CB6FE
	Private Sub OnDeathAnimComplete()
		Me.state = VeggiesLevelOnion.State.Complete
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003137 RID: 12599 RVA: 0x001CD312 File Offset: 0x001CB712
	Private Sub Die()
		Me.StopCrying()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x06003138 RID: 12600 RVA: 0x001CD32D File Offset: 0x001CB72D
	Private Sub StartExplosions()
		MyBase.GetComponent(Of LevelBossDeathExploder)().StartExplosion()
	End Sub

	' Token: 0x06003139 RID: 12601 RVA: 0x001CD33A File Offset: 0x001CB73A
	Private Sub StopExplosions()
		MyBase.GetComponent(Of LevelBossDeathExploder)().StopExplosions()
	End Sub

	' Token: 0x0600313A RID: 12602 RVA: 0x001CD348 File Offset: 0x001CB748
	Private Sub StartCrying()
		AudioManager.Play("level_veggies_onion_crying")
		Me.rightStream = Me.tearStreamPrefab.Create(Me.rightRoot.position, 1)
		Me.leftStream = Me.tearStreamPrefab.Create(Me.leftRoot.position, -1)
		Me.StartTearCoroutines()
	End Sub

	' Token: 0x0600313B RID: 12603 RVA: 0x001CD3AC File Offset: 0x001CB7AC
	Private Sub StopCrying()
		AudioManager.[Stop]("level_veggies_onion_crying")
		Me.currentCryLoop = 0
		Me.targetCryLoops = Me.properties.cryLoops.RandomInt()
		MyBase.animator.SetBool("ContinueCrying", True)
		If Me.rightStream IsNot Nothing Then
			Me.rightStream.[End]()
			Me.rightStream = Nothing
		End If
		If Me.leftStream IsNot Nothing Then
			Me.leftStream.[End]()
			Me.leftStream = Nothing
		End If
		Me.StopTearCoroutines()
	End Sub

	' Token: 0x0600313C RID: 12604 RVA: 0x001CD43D File Offset: 0x001CB83D
	Private Sub CryLoop()
		Me.currentCryLoop += 1
		If Me.currentCryLoop >= Me.targetCryLoops Then
			MyBase.animator.SetBool("ContinueCrying", False)
		End If
	End Sub

	' Token: 0x0600313D RID: 12605 RVA: 0x001CD470 File Offset: 0x001CB870
	Private Sub BashfulAnimComplete()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector2
		Dim flag As Boolean
		If [next].transform.position.x > MyBase.transform.position.x Then
			vector = Me.radishRootRight.position
			flag = False
		Else
			vector = Me.radishRootLeft.position
			flag = True
		End If
		Me.homingHeartPrefab.CreateRadish(vector, Me.properties.heartMaxSpeed, Me.properties.heartAcceleration, Me.properties.heartHP, flag)
		Me.state = VeggiesLevelOnion.State.Complete
	End Sub

	' Token: 0x0600313E RID: 12606 RVA: 0x001CD510 File Offset: 0x001CB910
	Private Iterator Function happyTimer_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.properties.happyTime
			t += CupheadTime.Delta
			Yield Nothing
		End While
		If Me.noSecret Then
			If Me.OnHappyLeave IsNot Nothing Then
				Me.OnHappyLeave()
			End If
			If Me.state = VeggiesLevelOnion.State.Idle Then
				Me.HappyLeave = True
				MyBase.animator.SetTrigger("HappyExit")
				MyBase.StartCoroutine(Me.handle_dirt_cr())
			End If
		End If
		Return
	End Function

	' Token: 0x0600313F RID: 12607 RVA: 0x001CD52B File Offset: 0x001CB92B
	Public Sub StopTearCoroutines()
		If Me.rightTearsCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.rightTearsCoroutine)
		End If
		If Me.leftTearsCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.leftTearsCoroutine)
		End If
		Me.rightTearsCoroutine = Nothing
		Me.leftTearsCoroutine = Nothing
	End Sub

	' Token: 0x06003140 RID: 12608 RVA: 0x001CD56C File Offset: 0x001CB96C
	Public Sub StartTearCoroutines()
		Me.StopTearCoroutines()
		Dim text As String = Me.properties.tearPatterns.GetRandom().ToUpper()
		Me.rightTearsCoroutine = Me.tears_cr(VeggiesLevelOnion.Side.Right, text)
		Me.leftTearsCoroutine = Me.tears_cr(VeggiesLevelOnion.Side.Left, text)
		MyBase.StartCoroutine(Me.rightTearsCoroutine)
		MyBase.StartCoroutine(Me.leftTearsCoroutine)
	End Sub

	' Token: 0x06003141 RID: 12609 RVA: 0x001CD5CC File Offset: 0x001CB9CC
	Private Iterator Function tears_cr(side As VeggiesLevelOnion.Side, pattern As String) As IEnumerator
		Dim tearDelay As Single = Global.UnityEngine.Random.Range(0.3F, 0.7F)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.tearAnticipate)
		Dim patterns As String() = pattern.Split(New Char() { ","c })
		Dim currentPattern As Integer = 0
		Dim numUntilPink As Integer = Me.properties.pinkTearRange.RandomInt()
		While True
			If patterns(currentPattern)(0) = "D"c Then
				Dim wait As Single = 0F
				Dim success As Boolean = Parser.FloatTryParse(patterns(currentPattern).Replace("D", String.Empty), wait)
				If success Then
					Yield CupheadTime.WaitForSeconds(Me, wait)
				End If
			Else
				Dim destinations As String() = patterns(currentPattern).Split(New Char() { "-"c })
				For i As Integer = 0 To destinations.Length - 1
					Dim x As Single = 0F
					Dim success2 As Boolean = Parser.FloatTryParse(destinations(i), x)
					If success2 Then
						numUntilPink -= 1
						If numUntilPink <= 0 Then
							Yield CupheadTime.WaitForSeconds(Me, tearDelay)
							numUntilPink = Me.properties.pinkTearRange.RandomInt()
							Me.pinkProjectilePrefab.Create(Me.properties.tearTime, If((side <> VeggiesLevelOnion.Side.Right), (-x), x))
						Else
							Yield CupheadTime.WaitForSeconds(Me, tearDelay)
							Me.projectilePrefab.Create(Me.properties.tearTime, If((side <> VeggiesLevelOnion.Side.Right), (-x), x))
						End If
						tearDelay = Global.UnityEngine.Random.Range(0.3F, 0.7F)
					End If
				Next
			End If
			currentPattern = CInt(Mathf.Repeat(CSng((currentPattern + 1)), CSng(patterns.Length)))
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.tearCommaDelay)
		End While
		Return
	End Function

	' Token: 0x06003142 RID: 12610 RVA: 0x001CD5F8 File Offset: 0x001CB9F8
	Private Iterator Function die_cr() As IEnumerator
		Me.circleCollider.enabled = False
		AudioManager.Play("level_veggies_onion_die")
		MyBase.animator.Play("Sad_Die")
		Me.StartExplosions()
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.StopExplosions()
		Yield Nothing
		Return
	End Function

	' Token: 0x06003143 RID: 12611 RVA: 0x001CD614 File Offset: 0x001CBA14
	Private Iterator Function handle_dirt_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		MyBase.animator.SetTrigger("FadeDirt")
		Return
	End Function

	' Token: 0x06003144 RID: 12612 RVA: 0x001CD62F File Offset: 0x001CBA2F
	Private Sub OnionVoiceExisBashfulSFX()
		AudioManager.Play("level_veggies_onion_exit_bashful")
		Me.emitAudioFromObject.Add("level_veggies_onion_exit_bashful")
	End Sub

	' Token: 0x040039BE RID: 14782
	Private Const START_SHOOTING_TIME As Single = 0.6F

	' Token: 0x040039C1 RID: 14785
	<SerializeField()>
	Private leftRoot As Transform

	' Token: 0x040039C2 RID: 14786
	<SerializeField()>
	Private rightRoot As Transform

	' Token: 0x040039C3 RID: 14787
	<SerializeField()>
	Private radishRootRight As Transform

	' Token: 0x040039C4 RID: 14788
	<SerializeField()>
	Private radishRootLeft As Transform

	' Token: 0x040039C5 RID: 14789
	<SerializeField()>
	Private tearStreamPrefab As VeggiesLevelOnionTearsStream

	' Token: 0x040039C6 RID: 14790
	<SerializeField()>
	Private projectilePrefab As VeggiesLevelOnionTearProjectile

	' Token: 0x040039C7 RID: 14791
	<SerializeField()>
	Private pinkProjectilePrefab As VeggiesLevelOnionTearProjectile

	' Token: 0x040039C8 RID: 14792
	<SerializeField()>
	Private homingHeartPrefab As VeggiesLevelOnionHomingHeart

	' Token: 0x040039C9 RID: 14793
	Private properties As LevelProperties.Veggies.Onion

	' Token: 0x040039CA RID: 14794
	Private circleCollider As CircleCollider2D

	' Token: 0x040039CB RID: 14795
	Private hp As Single

	' Token: 0x040039CC RID: 14796
	Private damageDealer As DamageDealer

	' Token: 0x040039CD RID: 14797
	Private rightStream As VeggiesLevelOnionTearsStream

	' Token: 0x040039CE RID: 14798
	Private leftStream As VeggiesLevelOnionTearsStream

	' Token: 0x040039CF RID: 14799
	Private currentCryLoop As Integer

	' Token: 0x040039D0 RID: 14800
	Private targetCryLoops As Integer = 8

	' Token: 0x040039D3 RID: 14803
	Private noSecret As Boolean

	' Token: 0x040039D4 RID: 14804
	Private rightTearsCoroutine As IEnumerator

	' Token: 0x040039D5 RID: 14805
	Private leftTearsCoroutine As IEnumerator

	' Token: 0x0200084C RID: 2124
	Public Enum Side
		' Token: 0x040039D7 RID: 14807
		Left = -1
		' Token: 0x040039D8 RID: 14808
		Right = 1
	End Enum

	' Token: 0x0200084D RID: 2125
	Public Enum State
		' Token: 0x040039DA RID: 14810
		Idle
		' Token: 0x040039DB RID: 14811
		Crying
		' Token: 0x040039DC RID: 14812
		Complete
	End Enum

	' Token: 0x0200084E RID: 2126
	' (Invoke) Token: 0x06003146 RID: 12614
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
