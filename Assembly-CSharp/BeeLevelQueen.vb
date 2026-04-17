Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000516 RID: 1302
Public Class BeeLevelQueen
	Inherits LevelProperties.Bee.Entity

	' Token: 0x17000327 RID: 807
	' (get) Token: 0x06001733 RID: 5939 RVA: 0x000D03D3 File Offset: 0x000CE7D3
	' (set) Token: 0x06001734 RID: 5940 RVA: 0x000D03DB File Offset: 0x000CE7DB
	Public Property state As BeeLevelQueen.State

	' Token: 0x06001735 RID: 5941 RVA: 0x000D03E4 File Offset: 0x000CE7E4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.RegisterCollisionChild(Me.head.gameObject)
		Me.EnableBody(False)
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001736 RID: 5942 RVA: 0x000D043D File Offset: 0x000CE83D
	Private Sub Start()
		AudioManager.Play("bee_queen_intro_vocal")
		Me.emitAudioFromObject.Add("bee_queen_intro_vocal")
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntro
	End Sub

	' Token: 0x06001737 RID: 5943 RVA: 0x000D046F File Offset: 0x000CE86F
	Private Sub OnIntro()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001738 RID: 5944 RVA: 0x000D047E File Offset: 0x000CE87E
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001739 RID: 5945 RVA: 0x000D0496 File Offset: 0x000CE896
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600173A RID: 5946 RVA: 0x000D04BF File Offset: 0x000CE8BF
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Gizmos.color = Color.yellow
		Gizmos.DrawWireSphere(Me.followerRoot.position, Me.followerRadius)
	End Sub

	' Token: 0x0600173B RID: 5947 RVA: 0x000D04F4 File Offset: 0x000CE8F4
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
		If MyBase.properties.CurrentHealth <= 0F AndAlso Me.state <> BeeLevelQueen.State.Death Then
			Me.state = BeeLevelQueen.State.Death
			Me.Death()
		End If
	End Sub

	' Token: 0x0600173C RID: 5948 RVA: 0x000D0540 File Offset: 0x000CE940
	Private Sub EnableBody(p As Boolean)
		Me.head.SetActive(p)
		Me.body.SetActive(p)
		Me.chain.SetActive(p)
	End Sub

	' Token: 0x0600173D RID: 5949 RVA: 0x000D0568 File Offset: 0x000CE968
	Private Sub MagicEffect()
		Dim transform As Transform = Me.dustEffect.Create(MyBase.transform.position).transform
		Dim transform2 As Transform = Me.sparkEffect.Create(MyBase.transform.position).transform
		transform.SetParent(MyBase.transform)
		transform2.SetParent(MyBase.transform)
		transform.ResetLocalTransforms()
		transform2.ResetLocalTransforms()
	End Sub

	' Token: 0x0600173E RID: 5950 RVA: 0x000D05D1 File Offset: 0x000CE9D1
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.puff = Nothing
		Me.spitPrefab = Nothing
		Me.blackHolePrefab = Nothing
		Me.trianglePrefab = Nothing
		Me.triangleInvinciblePrefab = Nothing
		Me.followerPrefab = Nothing
		Me.dustEffect = Nothing
		Me.sparkEffect = Nothing
	End Sub

	' Token: 0x0600173F RID: 5951 RVA: 0x000D0614 File Offset: 0x000CEA14
	Private Iterator Function intro_cr() As IEnumerator
		Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
		Me.state = BeeLevelQueen.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06001740 RID: 5952 RVA: 0x000D062F File Offset: 0x000CEA2F
	Private Sub SfxIntroKnife()
		AudioManager.Play("bee_queen_intro_cutlery")
		Me.emitAudioFromObject.Add("bee_queen_intro_cutlery")
	End Sub

	' Token: 0x06001741 RID: 5953 RVA: 0x000D064B File Offset: 0x000CEA4B
	Private Sub SfxIntroSnap()
		AudioManager.Play("bee_queen_intro_finger_click")
		Me.emitAudioFromObject.Add("bee_queen_intro_finger_click")
	End Sub

	' Token: 0x06001742 RID: 5954 RVA: 0x000D0667 File Offset: 0x000CEA67
	Public Sub StartChain()
		Me.state = BeeLevelQueen.State.Chain
		MyBase.StartCoroutine(Me.chain_cr())
	End Sub

	' Token: 0x06001743 RID: 5955 RVA: 0x000D067D File Offset: 0x000CEA7D
	Private Sub FireChainStartSFX()
		AudioManager.Play("bee_queen_chain_head_spit_start")
		Me.emitAudioFromObject.Add("bee_queen_chain_head_spit_start")
	End Sub

	' Token: 0x06001744 RID: 5956 RVA: 0x000D069C File Offset: 0x000CEA9C
	Private Sub FireChainProjectile()
		AudioManager.Play("bee_queen_chain_head_spit_attack")
		Me.emitAudioFromObject.Add("bee_queen_chain_head_spit_attack")
		Me.spitPrefab.Create(Me.spitRoot.position, New Vector2(MyBase.transform.localScale.x, 1F), Me.currentChain.speed, New Vector2(Me.currentChain.timeX, Me.currentChain.timeY))
	End Sub

	' Token: 0x06001745 RID: 5957 RVA: 0x000D0724 File Offset: 0x000CEB24
	Private Sub ChainFlip()
		MyBase.transform.SetScale(New Single?(MyBase.transform.localScale.x * -1F), New Single?(1F), New Single?(1F))
	End Sub

	' Token: 0x06001746 RID: 5958 RVA: 0x000D0770 File Offset: 0x000CEB70
	Private Iterator Function chain_cr() As IEnumerator
		Me.currentChain = MyBase.properties.CurrentState.chain
		MyBase.transform.ResetLocalTransforms()
		MyBase.transform.SetPosition(New Single?(-250F), New Single?(0F), New Single?(0F))
		MyBase.animator.Play("Warning")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Warning", False, True)
		MyBase.transform.SetPosition(New Single?(0F), New Single?(550F), New Single?(0F))
		Me.EnableBody(True)
		Me.SetBool(BeeLevelQueen.Bools.Repeat, True)
		MyBase.animator.Play("Chain_Idle")
		MyBase.animator.Play("Head_Closed_Idle", MyBase.animator.GetLayerIndex("Head"))
		Yield MyBase.StartCoroutine(Me.tween_cr(MyBase.transform, MyBase.transform.position, New Vector2(0F, 300F), EaseUtils.EaseType.easeOutQuart, 0.6F))
		AudioManager.Play("bee_queen_chain_ascend_vocal")
		Me.emitAudioFromObject.Add("bee_queen_chain_ascend_vocal")
		AudioManager.Play("bee_queen_chain_head_ascend")
		Me.emitAudioFromObject.Add("bee_queen_chain_head_ascend")
		MyBase.StartCoroutine(Me.tween_cr(Me.chain.transform, Me.chain.transform.position, New Vector2(0F, -100F), EaseUtils.EaseType.easeInQuart, 0.6F))
		Yield MyBase.StartCoroutine(Me.tween_cr(Me.head.transform, Me.head.transform.position, New Vector2(0F, -100F), EaseUtils.EaseType.easeInQuart, 0.6F))
		CupheadLevelCamera.Current.Shake(20F, 0.7F, False)
		Yield CupheadTime.WaitForSeconds(Me, 0.7F)
		MyBase.animator.Play("Spit_Start", MyBase.animator.GetLayerIndex("Head"))
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		If Not MyBase.properties.CurrentState.chain.chainForever Then
			For i As Integer = 0 To Me.currentChain.count - 1
				AudioManager.Play("bee_chain_head_spit_delay")
				Me.emitAudioFromObject.Add("bee_chain_head_spit_delay")
				Yield CupheadTime.WaitForSeconds(Me, Me.currentChain.delay)
				If i >= Me.currentChain.count - 1 Then
					Me.SetBool(BeeLevelQueen.Bools.Repeat, False)
				End If
				Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
			Next
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spit_Attack_End", MyBase.animator.GetLayerIndex("Head"), False, True)
			AudioManager.Play("bee_queen_chain_head_decend")
			Me.emitAudioFromObject.Add("bee_queen_chain_head_decend")
			MyBase.StartCoroutine(Me.tween_cr(Me.chain.transform, Me.chain.transform.position, New Vector2(0F, 300F), EaseUtils.EaseType.easeInQuart, 0.6F))
			Yield MyBase.StartCoroutine(Me.tween_cr(Me.head.transform, Me.head.transform.position, New Vector2(0F, 300F), EaseUtils.EaseType.easeInQuart, 0.6F))
			CupheadLevelCamera.Current.Shake(20F, 0.7F, False)
			Yield CupheadTime.WaitForSeconds(Me, 0.7F)
			Yield MyBase.StartCoroutine(Me.tween_cr(MyBase.transform, MyBase.transform.position, New Vector2(0F, 550F), EaseUtils.EaseType.easeInQuart, 0.6F))
			Me.EnableBody(False)
			Yield CupheadTime.WaitForSeconds(Me, Me.currentChain.hesitate)
			Me.state = BeeLevelQueen.State.Idle
			Return
		End If
		While True
			AudioManager.Play("bee_chain_head_spit_delay")
			Me.emitAudioFromObject.Add("bee_chain_head_spit_delay")
			Yield CupheadTime.WaitForSeconds(Me, Me.currentChain.delay)
			Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
			Yield Nothing
		End While
	End Function

	' Token: 0x06001747 RID: 5959 RVA: 0x000D078B File Offset: 0x000CEB8B
	Public Sub StartBlackHole()
		Me.state = BeeLevelQueen.State.BlackHole
		MyBase.StartCoroutine(Me.blackHole_cr())
	End Sub

	' Token: 0x06001748 RID: 5960 RVA: 0x000D07A4 File Offset: 0x000CEBA4
	Private Iterator Function blackHole_cr() As IEnumerator
		MyBase.transform.ResetLocalTransforms()
		MyBase.transform.SetScale(New Single?(CSng(MathUtils.PlusOrMinus())), New Single?(1F), New Single?(1F))
		MyBase.transform.SetPosition(New Single?(290F * MyBase.transform.localScale.x), Nothing, Nothing)
		MyBase.animator.Play("Warning")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Warning", False, True)
		Me.ClearTrigger(BeeLevelQueen.Triggers.[Continue])
		Me.SetAttackAnim(BeeLevelQueen.AttackAnimations.BlackHole)
		MyBase.animator.Play("Spell_Start")
		Dim properties As LevelProperties.Bee.BlackHole = MyBase.properties.CurrentState.blackHole
		Dim patternStrings As String() = properties.patterns(Global.UnityEngine.Random.Range(0, properties.patterns.Length)).Split(New Char() { ","c })
		Dim patternArray As Integer() = New Integer(patternStrings.Length - 1) {}
		For j As Integer = 0 To patternStrings.Length - 1
			Parser.IntTryParse(patternStrings(j), patternArray(j))
			patternArray(j) = Mathf.Clamp(patternArray(j), 0, 2)
		Next
		Dim i As Integer = 0
		Dim count As Integer = patternArray.Length
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Start", False, True)
		AudioManager.PlayLoop("bee_queen_spell_shake_loop")
		Me.emitAudioFromObject.Add("bee_queen_spell_shake_loop")
		While i < count
			Yield CupheadTime.WaitForSeconds(Me, properties.chargeTime)
			Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Charge_End", False, True)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Attack_Start", False, True)
			Dim b As BeeLevelQueenBlackHole = TryCast(Me.blackHolePrefab.Create(Me.blackHoleRoots(patternArray(i)).position), BeeLevelQueenBlackHole)
			b.speed = properties.speed
			b.health = properties.health
			b.childDelay = properties.childDelay
			b.childSpeed = CSng(properties.childSpeed)
			If properties.damageable Then
				b.gameObject.AddComponent(Of Rigidbody2D)()
			End If
			Yield CupheadTime.WaitForSeconds(Me, properties.attackTime)
			i += 1
			Me.SetBool(BeeLevelQueen.Bools.Repeat, i <> count)
			Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
		End While
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_End", False, True)
		AudioManager.[Stop]("bee_queen_spell_shake_loop")
		MyBase.transform.SetPosition(New Single?(0F), Nothing, Nothing)
		Yield CupheadTime.WaitForSeconds(Me, properties.hesitate)
		Me.state = BeeLevelQueen.State.Idle
		Return
	End Function

	' Token: 0x06001749 RID: 5961 RVA: 0x000D07BF File Offset: 0x000CEBBF
	Public Sub StartTriangle()
		Me.state = BeeLevelQueen.State.Triangle
		MyBase.StartCoroutine(Me.triangle_cr())
	End Sub

	' Token: 0x0600174A RID: 5962 RVA: 0x000D07D8 File Offset: 0x000CEBD8
	Private Iterator Function triangle_cr() As IEnumerator
		MyBase.transform.ResetLocalTransforms()
		MyBase.transform.SetScale(New Single?(CSng(MathUtils.PlusOrMinus())), New Single?(1F), New Single?(1F))
		MyBase.transform.SetPosition(New Single?(290F * MyBase.transform.localScale.x), Nothing, Nothing)
		MyBase.animator.Play("Warning")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Warning", False, True)
		Me.ClearTrigger(BeeLevelQueen.Triggers.[Continue])
		Me.SetAttackAnim(BeeLevelQueen.AttackAnimations.Triangle)
		MyBase.animator.Play("Spell_Start")
		Me.SetBool(BeeLevelQueen.Bools.Repeat, False)
		Dim properties As LevelProperties.Bee.Triangle = MyBase.properties.CurrentState.triangle
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Start", False, True)
		AudioManager.PlayLoop("bee_queen_spell_shake_loop")
		Me.emitAudioFromObject.Add("bee_queen_spell_shake_loop")
		Dim i As Integer = 0
		While i < properties.count
			Yield CupheadTime.WaitForSeconds(Me, properties.chargeTime)
			Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Charge_End", False, True)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Attack_Start", False, True)
			Dim p As BeeLevelQueenTriangle.Properties = New BeeLevelQueenTriangle.Properties(PlayerManager.GetNext(), properties.introTime, properties.speed, properties.rotationSpeed, properties.health, properties.childSpeed, properties.childDelay, properties.childHealth, properties.childCount, properties.damageable)
			If properties.damageable Then
				Me.trianglePrefab.Create(p)
			Else
				Me.triangleInvinciblePrefab.Create(p)
			End If
			Yield CupheadTime.WaitForSeconds(Me, properties.attackTime)
			i += 1
			Me.SetBool(BeeLevelQueen.Bools.Repeat, i <> properties.count)
			Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
		End While
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_End", False, True)
		AudioManager.[Stop]("bee_queen_spell_shake_loop")
		MyBase.transform.SetPosition(New Single?(0F), Nothing, Nothing)
		Yield CupheadTime.WaitForSeconds(Me, properties.hesitate)
		Me.state = BeeLevelQueen.State.Idle
		Return
	End Function

	' Token: 0x0600174B RID: 5963 RVA: 0x000D07F3 File Offset: 0x000CEBF3
	Public Sub StartMorph()
		MyBase.StartCoroutine(Me.morph_cr())
	End Sub

	' Token: 0x0600174C RID: 5964 RVA: 0x000D0804 File Offset: 0x000CEC04
	Private Iterator Function morph_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 2.5F
		Dim moveSpeed As Single = 0F
		MyBase.animator.Play("Warning_Trans")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Warning_Trans", False, True)
		AudioManager.PlayLoop("bee_queen_spell_antic")
		Me.emitAudioFromObject.Add("bee_queen_spell_antic")
		Dim endPos As Vector3 = New Vector3(0F, 230F)
		Dim startPos As Vector3 = MyBase.transform.position
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(startPos, endPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.position = endPos
		AudioManager.[Stop]("bee_queen_spell_antic")
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Morph_Morph", False, True)
		Yield CupheadTime.WaitForSeconds(Me, 0.54F)
		t = 0F
		While t < 0.76F
			moveSpeed = If((t >= 0.3F), 300F, 800F)
			MyBase.transform.position += Vector3.up * moveSpeed * CupheadTime.Delta
			t += CupheadTime.Delta
			Yield Nothing
		End While
		t = 0F
		time = 0.67F
		startPos = MyBase.transform.position
		endPos = New Vector3(0F, -960F)
		MyBase.StartCoroutine(Me.spawn_puffs_cr())
		While t < time
			Dim val2 As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			MyBase.transform.position = Vector2.Lerp(startPos, endPos, val2)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.airplane.StartIntro()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x0600174D RID: 5965 RVA: 0x000D0820 File Offset: 0x000CEC20
	Private Sub SnapPosition()
		MyBase.transform.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Player.ToString()
		MyBase.transform.GetComponent(Of SpriteRenderer)().sortingOrder = 100
		MyBase.transform.position = New Vector3(0F, 960F)
	End Sub

	' Token: 0x0600174E RID: 5966 RVA: 0x000D0878 File Offset: 0x000CEC78
	Private Sub MoveHoney()
		MyBase.StartCoroutine(Me.move_honey_cr())
		MyBase.StartCoroutine(Me.move_bee_cr())
	End Sub

	' Token: 0x0600174F RID: 5967 RVA: 0x000D0894 File Offset: 0x000CEC94
	Private Iterator Function move_honey_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 2.5F
		Dim startPos As Vector3 = Me.bottomHoney.transform.position
		Dim endPos As Vector3 = New Vector3(0F, -560F)
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / time)
			Me.bottomHoney.transform.position = Vector2.Lerp(startPos, endPos, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.bottomHoney.transform.position = endPos
		MyBase.StartCoroutine(CupheadLevelCamera.Current.change_zoom_cr(0.97F, 2F))
		Yield Nothing
		Return
	End Function

	' Token: 0x06001750 RID: 5968 RVA: 0x000D08B0 File Offset: 0x000CECB0
	Private Iterator Function move_bee_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 3F
		While t < time
			MyBase.transform.position += Vector3.up * 50F * CupheadTime.Delta
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001751 RID: 5969 RVA: 0x000D08CC File Offset: 0x000CECCC
	Private Iterator Function spawn_puffs_cr() As IEnumerator
		For Each root As Transform In Me.puffRoots
			Me.puff.Create(root.position)
			Yield CupheadTime.WaitForSeconds(Me, 0.134F)
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x06001752 RID: 5970 RVA: 0x000D08E7 File Offset: 0x000CECE7
	Public Sub StartFollower()
		Me.state = BeeLevelQueen.State.Triangle
		MyBase.StartCoroutine(Me.follower_cr())
	End Sub

	' Token: 0x06001753 RID: 5971 RVA: 0x000D0900 File Offset: 0x000CED00
	Private Iterator Function follower_cr() As IEnumerator
		MyBase.transform.ResetLocalTransforms()
		MyBase.transform.SetScale(New Single?(CSng(MathUtils.PlusOrMinus())), New Single?(1F), New Single?(1F))
		MyBase.transform.SetPosition(New Single?(290F * MyBase.transform.localScale.x), Nothing, Nothing)
		MyBase.animator.Play("Warning")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Warning", False, True)
		Me.ClearTrigger(BeeLevelQueen.Triggers.[Continue])
		Me.SetAttackAnim(BeeLevelQueen.AttackAnimations.Follower)
		MyBase.animator.Play("Spell_Start")
		Me.SetBool(BeeLevelQueen.Bools.Repeat, False)
		Dim properties As LevelProperties.Bee.Follower = MyBase.properties.CurrentState.follower
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Start", False, True)
		Dim i As Integer = 0
		While i < properties.count
			Yield CupheadTime.WaitForSeconds(Me, properties.chargeTime)
			Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Charge_End", False, True)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_Attack_Start", False, True)
			Dim vector As Vector2 = Me.followerRoot.position
			Dim vector2 As Vector2 = New Vector2(CSng(Global.UnityEngine.Random.Range(-1, 1)), CSng(Global.UnityEngine.Random.Range(-1, 1)))
			Dim pos As Vector2 = vector + vector2.normalized * (Me.followerRadius * Global.UnityEngine.Random.value)
			Dim p As BeeLevelQueenFollower.Properties = New BeeLevelQueenFollower.Properties(PlayerManager.GetNext(), properties.introTime, properties.homingSpeed, properties.homingRotation, properties.homingTime, properties.health, properties.childDelay, properties.childHealth, properties.parryable)
			If properties.damageable Then
				Me.followerPrefab.Create(pos, p).gameObject.AddComponent(Of Rigidbody2D)().isKinematic = True
			Else
				Me.followerPrefab.Create(pos, p)
			End If
			Yield CupheadTime.WaitForSeconds(Me, properties.attackTime)
			i += 1
			Me.SetBool(BeeLevelQueen.Bools.Repeat, i <> properties.count)
			Me.SetTrigger(BeeLevelQueen.Triggers.[Continue])
		End While
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Spell_End", False, True)
		MyBase.transform.SetPosition(New Single?(0F), Nothing, Nothing)
		Yield CupheadTime.WaitForSeconds(Me, properties.hesitate)
		Me.state = BeeLevelQueen.State.Idle
		Return
	End Function

	' Token: 0x06001754 RID: 5972 RVA: 0x000D091C File Offset: 0x000CED1C
	Private Iterator Function tween_cr(trans As Transform, start As Vector2, [end] As Vector2, ease As EaseUtils.EaseType, time As Single) As IEnumerator
		Dim t As Single = 0F
		trans.position = start
		While t < time
			Dim val As Single = EaseUtils.Ease(ease, 0F, 1F, t / time)
			trans.position = Vector2.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		trans.position = [end]
		Yield Nothing
		Return
	End Function

	' Token: 0x06001755 RID: 5973 RVA: 0x000D0955 File Offset: 0x000CED55
	Private Sub SetAttackAnim(a As BeeLevelQueen.AttackAnimations)
		Me.SetInt(BeeLevelQueen.Integers.Attack, CInt(a))
	End Sub

	' Token: 0x06001756 RID: 5974 RVA: 0x000D095F File Offset: 0x000CED5F
	Private Sub SetTrigger(t As BeeLevelQueen.Triggers)
		MyBase.animator.SetTrigger(t.ToString())
	End Sub

	' Token: 0x06001757 RID: 5975 RVA: 0x000D0979 File Offset: 0x000CED79
	Private Sub ClearTrigger(t As BeeLevelQueen.Triggers)
		MyBase.animator.ResetTrigger(t.ToString())
	End Sub

	' Token: 0x06001758 RID: 5976 RVA: 0x000D0993 File Offset: 0x000CED93
	Private Sub SetInt(i As BeeLevelQueen.Integers, value As Integer)
		MyBase.animator.SetInteger(i.ToString(), value)
	End Sub

	' Token: 0x06001759 RID: 5977 RVA: 0x000D09AE File Offset: 0x000CEDAE
	Private Sub SetBool(b As BeeLevelQueen.Bools, value As Boolean)
		MyBase.animator.SetBool(b.ToString(), value)
	End Sub

	' Token: 0x0600175A RID: 5978 RVA: 0x000D09C9 File Offset: 0x000CEDC9
	Private Sub Death()
		MyBase.animator.Play("Head_Closed_Idle")
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x0600175B RID: 5979 RVA: 0x000D09E1 File Offset: 0x000CEDE1
	Private Sub SpellTossSFX()
		AudioManager.Play("bee_queen_spell_toss")
		Me.emitAudioFromObject.Add("bee_queen_spell_toss")
	End Sub

	' Token: 0x0600175C RID: 5980 RVA: 0x000D09FD File Offset: 0x000CEDFD
	Private Sub SpellCastSFX()
		AudioManager.Play("bee_queen_spell_cast")
		Me.emitAudioFromObject.Add("bee_queen_spell_cast")
	End Sub

	' Token: 0x0600175D RID: 5981 RVA: 0x000D0A19 File Offset: 0x000CEE19
	Private Sub AttackStartSFX()
		AudioManager.Play("bee_queen_attack_start")
		Me.emitAudioFromObject.Add("bee_queen_attack_start")
		AudioManager.PlayLoop("bee_queen_attack_loop")
	End Sub

	' Token: 0x0600175E RID: 5982 RVA: 0x000D0A3F File Offset: 0x000CEE3F
	Private Sub AttackEndSFX()
		AudioManager.[Stop]("bee_queen_attack_loop")
		AudioManager.Play("bee_queen_attack_end")
		Me.emitAudioFromObject.Add("bee_queen_attack_end")
	End Sub

	' Token: 0x0600175F RID: 5983 RVA: 0x000D0A65 File Offset: 0x000CEE65
	Private Sub WarningSFX()
		AudioManager.Play("bee_queen_warning")
		Me.emitAudioFromObject.Add("bee_queen_warning")
	End Sub

	' Token: 0x06001760 RID: 5984 RVA: 0x000D0A81 File Offset: 0x000CEE81
	Private Sub FlyDownSFX()
		AudioManager.Play("bee_airplane_fly_down")
		Me.emitAudioFromObject.Add("bee_airplane_fly_down")
	End Sub

	' Token: 0x04002070 RID: 8304
	Public Const SPELL_X As Single = 290F

	' Token: 0x04002072 RID: 8306
	<SerializeField()>
	Private airplane As BeeLevelAirplane

	' Token: 0x04002073 RID: 8307
	<SerializeField()>
	Private bottomHoney As Transform

	' Token: 0x04002074 RID: 8308
	<SerializeField()>
	Private puff As Effect

	' Token: 0x04002075 RID: 8309
	<Space(5F)>
	<SerializeField()>
	Private puffRoots As Transform()

	' Token: 0x04002076 RID: 8310
	<Space(5F)>
	<SerializeField()>
	Private head As GameObject

	' Token: 0x04002077 RID: 8311
	<SerializeField()>
	Private body As GameObject

	' Token: 0x04002078 RID: 8312
	<SerializeField()>
	Private chain As GameObject

	' Token: 0x04002079 RID: 8313
	<Space(10F)>
	<SerializeField()>
	Private spitPrefab As BeeLevelQueenSpitProjectile

	' Token: 0x0400207A RID: 8314
	<SerializeField()>
	Private spitRoot As Transform

	' Token: 0x0400207B RID: 8315
	<Space(10F)>
	<SerializeField()>
	Private blackHolePrefab As BeeLevelQueenBlackHole

	' Token: 0x0400207C RID: 8316
	<SerializeField()>
	Private blackHoleRoots As Transform()

	' Token: 0x0400207D RID: 8317
	<Space(10F)>
	<SerializeField()>
	Private trianglePrefab As BeeLevelQueenTriangle

	' Token: 0x0400207E RID: 8318
	<SerializeField()>
	Private triangleInvinciblePrefab As BeeLevelQueenTriangle

	' Token: 0x0400207F RID: 8319
	<Space(10F)>
	<SerializeField()>
	Private followerRadius As Single = 200F

	' Token: 0x04002080 RID: 8320
	<SerializeField()>
	Private followerRoot As Transform

	' Token: 0x04002081 RID: 8321
	<SerializeField()>
	Private followerPrefab As BeeLevelQueenFollower

	' Token: 0x04002082 RID: 8322
	<Space(10F)>
	<SerializeField()>
	Private dustEffect As Effect

	' Token: 0x04002083 RID: 8323
	<SerializeField()>
	Private sparkEffect As Effect

	' Token: 0x04002084 RID: 8324
	Private damageReceiver As DamageReceiver

	' Token: 0x04002085 RID: 8325
	Private damageDealer As DamageDealer

	' Token: 0x04002086 RID: 8326
	Private currentChain As LevelProperties.Bee.Chain

	' Token: 0x02000517 RID: 1303
	Public Enum State
		' Token: 0x04002088 RID: 8328
		Intro
		' Token: 0x04002089 RID: 8329
		Idle
		' Token: 0x0400208A RID: 8330
		BlackHole
		' Token: 0x0400208B RID: 8331
		Triangle
		' Token: 0x0400208C RID: 8332
		Follower
		' Token: 0x0400208D RID: 8333
		Chain
		' Token: 0x0400208E RID: 8334
		Death
	End Enum

	' Token: 0x02000518 RID: 1304
	Private Enum AttackAnimations
		' Token: 0x04002090 RID: 8336
		BlackHole
		' Token: 0x04002091 RID: 8337
		Triangle
		' Token: 0x04002092 RID: 8338
		Follower
	End Enum

	' Token: 0x02000519 RID: 1305
	Private Enum Triggers
		' Token: 0x04002094 RID: 8340
		[Continue]
	End Enum

	' Token: 0x0200051A RID: 1306
	Private Enum Integers
		' Token: 0x04002096 RID: 8342
		Attack
	End Enum

	' Token: 0x0200051B RID: 1307
	Private Enum Bools
		' Token: 0x04002098 RID: 8344
		Repeat
	End Enum
End Class
