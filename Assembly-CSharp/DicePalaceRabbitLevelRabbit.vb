Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005E0 RID: 1504
Public Class DicePalaceRabbitLevelRabbit
	Inherits LevelProperties.DicePalaceRabbit.Entity

	' Token: 0x17000370 RID: 880
	' (get) Token: 0x06001DBB RID: 7611 RVA: 0x00111389 File Offset: 0x0010F789
	' (set) Token: 0x06001DBC RID: 7612 RVA: 0x00111391 File Offset: 0x0010F791
	Public Property state As DicePalaceRabbitLevelRabbit.State

	' Token: 0x06001DBD RID: 7613 RVA: 0x0011139C File Offset: 0x0010F79C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.idle_voice_sfx_cr())
		MyBase.StartCoroutine(Me.idle_sfx_cr())
	End Sub

	' Token: 0x06001DBE RID: 7614 RVA: 0x001113F7 File Offset: 0x0010F7F7
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001DBF RID: 7615 RVA: 0x0011140A File Offset: 0x0010F80A
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001DC0 RID: 7616 RVA: 0x00111422 File Offset: 0x0010F822
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001DC1 RID: 7617 RVA: 0x00111440 File Offset: 0x0010F840
	Public Overrides Sub LevelInit(properties As LevelProperties.DicePalaceRabbit)
		MyBase.LevelInit(properties)
		Me.attacking = False
		Me.playerOneCircleIndex = Global.UnityEngine.Random.Range(0, properties.CurrentState.magicWand.safeZoneString.Split(New Char() { ","c }).Length)
		Dim zero As Vector2 = Vector2.zero
		Dim zero2 As Vector2 = Vector2.zero
		zero.x = CSng(Parser.IntParse(properties.CurrentState.general.platformOnePosition.Split(New Char() { ","c })(0)))
		zero.y = CSng(Parser.IntParse(properties.CurrentState.general.platformOnePosition.Split(New Char() { ","c })(1)))
		Me.platform1.transform.position = zero
		Me.platform1.YPositionUp = zero.y
		zero2.x = CSng(Parser.IntParse(properties.CurrentState.general.platformTwoPosition.Split(New Char() { ","c })(0)))
		zero2.y = CSng(Parser.IntParse(properties.CurrentState.general.platformTwoPosition.Split(New Char() { ","c })(1)))
		Me.platform2.transform.position = zero2
		Me.platform2.YPositionUp = zero2.y
		Me.isMagicParryTop = Rand.Bool()
		Me.state = DicePalaceRabbitLevelRabbit.State.Idle
		AddHandler Level.Current.OnWinEvent, AddressOf Me.Death
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001DC2 RID: 7618 RVA: 0x001115D8 File Offset: 0x0010F9D8
	Private Iterator Function intro_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro_Continue", False, True)
		MyBase.animator.Play("Off")
		Yield Nothing
		Return
	End Function

	' Token: 0x06001DC3 RID: 7619 RVA: 0x001115F4 File Offset: 0x0010F9F4
	Private Iterator Function idle_voice_sfx_cr() As IEnumerator
		Dim delay As MinMax = New MinMax(1F, 4F)
		While True
			While Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle")
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, delay)
			AudioManager.Play("dice_palace_rabbit_idle_vox")
			Me.emitAudioFromObject.Add("dice_palace_rabbit_idle_vox")
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001DC4 RID: 7620 RVA: 0x00111610 File Offset: 0x0010FA10
	Private Iterator Function idle_sfx_cr() As IEnumerator
		Dim loopingIdle As Boolean = False
		While True
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") Then
				If Not loopingIdle Then
				End If
			ElseIf loopingIdle Then
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001DC5 RID: 7621 RVA: 0x0011162B File Offset: 0x0010FA2B
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.orbPrefab = Nothing
		Me.magicPrefab = Nothing
		Me.explosionPrefab = Nothing
	End Sub

	' Token: 0x06001DC6 RID: 7622 RVA: 0x00111648 File Offset: 0x0010FA48
	Public Sub OnMagicWand()
		MyBase.StartCoroutine(Me.magicwand_cr())
	End Sub

	' Token: 0x06001DC7 RID: 7623 RVA: 0x00111658 File Offset: 0x0010FA58
	Private Iterator Function magicwand_cr() As IEnumerator
		Me.attacking = True
		Me.state = DicePalaceRabbitLevelRabbit.State.MagicWand
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.magicWand.initialAttackDelay)
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		MyBase.animator.SetTrigger("OnAttack")
		MyBase.StartCoroutine(Me.orbs_cr(player.id, Parser.IntParse(MyBase.properties.CurrentState.magicWand.safeZoneString.Split(New Char() { ","c })(Me.playerOneCircleIndex))))
		Me.playerOneCircleIndex += 1
		If Me.playerOneCircleIndex >= MyBase.properties.CurrentState.magicWand.safeZoneString.Split(New Char() { ","c }).Length Then
			Me.playerOneCircleIndex = 0
		End If
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.magicWand.attackDelayRange.RandomFloat())
		Me.attacking = False
		MyBase.animator.SetTrigger("OnAttackEnd")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.magicWand.hesitate)
		Me.state = DicePalaceRabbitLevelRabbit.State.Idle
		Return
	End Function

	' Token: 0x06001DC8 RID: 7624 RVA: 0x00111674 File Offset: 0x0010FA74
	Private Iterator Function orbs_cr(target As PlayerId, safeZone As Integer) As IEnumerator
		Dim centerPoint As GameObject = New GameObject()
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(target)
		centerPoint.transform.position = player.center
		Me.currentCenterPoint = centerPoint
		Dim dir As Vector3 = Vector3.up
		Dim dist As Single = MyBase.properties.CurrentState.magicWand.circleDiameter / 2F
		safeZone = Me.GetSafeZone(safeZone)
		Dim orbs As Transform() = New Transform(6) {}
		Dim orbsIndex As Integer = 0
		Dim initialRotation As Single = CSng(Global.UnityEngine.Random.Range(0, 350))
		For i As Integer = 0 To 8 - 1
			If i <> safeZone Then
				Dim dicePalaceRabbitLevelOrb As DicePalaceRabbitLevelOrb = TryCast(Me.orbPrefab.Create(player.center + dir * dist, 0F, Vector2.one), DicePalaceRabbitLevelOrb)
				dicePalaceRabbitLevelOrb.transform.parent = centerPoint.transform
				dicePalaceRabbitLevelOrb.transform.Rotate(Vector3.forward, -initialRotation)
				dicePalaceRabbitLevelOrb.SetAsGold(i Mod 2 = 1)
				Dim color As Color = dicePalaceRabbitLevelOrb.GetComponent(Of SpriteRenderer)().color
				color.a = 0.2F
				dicePalaceRabbitLevelOrb.GetComponent(Of SpriteRenderer)().color = color
				orbs(orbsIndex) = dicePalaceRabbitLevelOrb.transform
				orbsIndex += 1
			End If
			dir = Quaternion.AngleAxis(-45F, Vector3.forward) * dir
		Next
		centerPoint.transform.Rotate(Vector3.forward, initialRotation)
		While Me.attacking
			If player IsNot Nothing AndAlso Not player.IsDead Then
				centerPoint.transform.position = player.center
			End If
			centerPoint.transform.Rotate(Vector3.forward * CupheadTime.FixedDelta, -MyBase.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta)
			For j As Integer = 0 To orbs.Length - 1
				Dim component As SpriteRenderer = orbs(j).GetComponent(Of SpriteRenderer)()
				Dim color2 As Color = component.color
				color2.a += CupheadTime.Delta / 2F
				component.color = color2
				If color2.a >= 1F Then
					orbs(j).GetComponent(Of Collider2D)().enabled = True
				End If
				orbs(j).Rotate(Vector3.forward * CupheadTime.FixedDelta, MyBase.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta)
			Next
			Yield New WaitForFixedUpdate()
		End While
		For k As Integer = 0 To orbs.Length - 1
			orbs(k).GetComponent(Of Collider2D)().enabled = True
		Next
		While Vector3.Angle(Vector3.up, centerPoint.transform.up) > 5F
			If player IsNot Nothing AndAlso Not player.IsDead Then
				centerPoint.transform.position = player.center
			End If
			centerPoint.transform.Rotate(Vector3.forward * CupheadTime.FixedDelta, -MyBase.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta)
			For l As Integer = 0 To orbs.Length - 1
				orbs(l).Rotate(Vector3.forward * CupheadTime.FixedDelta, MyBase.properties.CurrentState.magicWand.spinningSpeed * CupheadTime.FixedDelta)
			Next
			Yield New WaitForFixedUpdate()
		End While
		centerPoint.transform.up = Vector3.up
		MyBase.StartCoroutine(Me.collapse_cr(centerPoint))
		Return
	End Function

	' Token: 0x06001DC9 RID: 7625 RVA: 0x001116A0 File Offset: 0x0010FAA0
	Private Iterator Function collapse_cr(centerPoint As GameObject) As IEnumerator
		Dim dist As Single = MyBase.properties.CurrentState.magicWand.circleDiameter / 2F
		Dim explodeDist As Single = MyBase.properties.CurrentState.magicWand.circleDiameter * 0.1F
		While dist >= explodeDist
			For i As Integer = 0 To 7 - 1
				Dim vector As Vector3 = (centerPoint.transform.GetChild(i).position - centerPoint.transform.position).normalized * dist
				centerPoint.transform.GetChild(i).position = centerPoint.transform.position + vector
			Next
			dist -= MyBase.properties.CurrentState.magicWand.bulletSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		AudioManager.Play("projectile_explo")
		Me.explosionPrefab.Create(centerPoint.transform.position)
		Me.currentCenterPoint = Nothing
		Global.UnityEngine.[Object].Destroy(centerPoint)
		Return
	End Function

	' Token: 0x06001DCA RID: 7626 RVA: 0x001116C4 File Offset: 0x0010FAC4
	Private Function GetSafeZone(index As Integer) As Integer
		Dim num As Integer = 0
		Select Case index
			Case 1
				num = 5
			Case 2
				num = 4
			Case 3
				num = 3
			Case 4
				num = 6
			Case 6
				num = 2
			Case 7
				num = 7
			Case 8
				num = 0
			Case 9
				num = 1
		End Select
		Return num
	End Function

	' Token: 0x06001DCB RID: 7627 RVA: 0x00111744 File Offset: 0x0010FB44
	Private Iterator Function kill_orbs_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = 1F
		Dim speed As Single = 2500F
		Dim angles As Single() = New Single(6) {}
		For i As Integer = 0 To 7 - 1
			Me.currentCenterPoint.transform.GetChild(i).GetComponent(Of Collider2D)().enabled = False
			angles(i) = CSng(Global.UnityEngine.Random.Range(0, 360))
		Next
		While t < time
			t += CupheadTime.Delta
			For j As Integer = 0 To 7 - 1
				Me.currentCenterPoint.transform.GetChild(j).position += MathUtils.AngleToDirection(angles(j)) * speed * CupheadTime.FixedDelta
			Next
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(Me.currentCenterPoint)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001DCC RID: 7628 RVA: 0x0011175F File Offset: 0x0010FB5F
	Public Sub OnMagicParry()
		MyBase.StartCoroutine(Me.magicparry_cr())
	End Sub

	' Token: 0x06001DCD RID: 7629 RVA: 0x00111770 File Offset: 0x0010FB70
	Private Iterator Function magicparry_cr() As IEnumerator
		Me.attacking = True
		Me.state = DicePalaceRabbitLevelRabbit.State.MagicParry
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.magicParry.initialAttackDelay)
		MyBase.animator.SetTrigger("OnAttack")
		Dim positionsSplits As String() = MyBase.properties.CurrentState.magicParry.magicPositions.Split(New Char() { "-"c })
		Dim magicOrbs As DicePalaceRabbitLevelMagic() = New DicePalaceRabbitLevelMagic(positionsSplits.Length - 1) {}
		Dim parryPattern As String() = MyBase.properties.CurrentState.magicParry.pinkString.Split(New Char() { ","c })
		Dim parryIndexes As String() = parryPattern(Me.parryCurrentIndex).Split(New Char() { "-"c })
		Dim yOffset As Single = MyBase.properties.CurrentState.magicParry.yOffset
		Dim posY As Single = If((Not Me.isMagicParryTop), (-360F + yOffset), (360F - yOffset))
		Dim suit As Integer = 0
		For i As Integer = 0 To magicOrbs.Length - 1
			Dim num As Single = 0F
			Parser.FloatTryParse(positionsSplits(i), num)
			num += -640F
			magicOrbs(i) = CType(Me.magicPrefab.Create(New Vector3(num, posY)), DicePalaceRabbitLevelMagic)
			magicOrbs(i).IsOffset(i Mod 2 = 1)
			magicOrbs(i).AppearTime = MyBase.properties.CurrentState.magicParry.attackDelayRange
			Dim flag As Boolean = False
			For j As Integer = 0 To parryIndexes.Length - 1
				Dim num2 As Integer = 0
				If Parser.IntTryParse(parryIndexes(j), num2) AndAlso num2 - 1 = i Then
					magicOrbs(i).SetParryable(True)
					flag = True
				End If
			Next
			If Not flag Then
				magicOrbs(i).SetSuit(suit)
				suit = (suit + 1) Mod 3
			End If
		Next
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.magicParry.attackDelayRange)
		For k As Integer = 0 To magicOrbs.Length - 1
			magicOrbs(k).ActivateOrb()
			magicOrbs(k).Move(posY, Me.isMagicParryTop, MyBase.properties.CurrentState.magicParry.speed)
		Next
		MyBase.animator.SetTrigger("OnAttackEnd")
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.magicParry.hesitate)
		Me.attacking = False
		Me.isMagicParryTop = Not Me.isMagicParryTop
		Me.parryCurrentIndex = (Me.parryCurrentIndex + 1) Mod parryPattern.Length
		Me.state = DicePalaceRabbitLevelRabbit.State.Idle
		Return
	End Function

	' Token: 0x06001DCE RID: 7630 RVA: 0x0011178B File Offset: 0x0010FB8B
	Private Sub AttackSFX()
		MyBase.StartCoroutine(Me.attack_sfx_cr())
	End Sub

	' Token: 0x06001DCF RID: 7631 RVA: 0x0011179C File Offset: 0x0010FB9C
	Private Iterator Function attack_sfx_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack", False)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Attack_End", False)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001DD0 RID: 7632 RVA: 0x001117B8 File Offset: 0x0010FBB8
	Private Sub Death()
		AudioManager.[Stop]("dice_palace_rabbit_idle_loop")
		AudioManager.[Stop]("dice_palace_rabbit_attack_loop")
		Me.SFX_StickTwirlStop()
		MyBase.animator.SetTrigger("Death")
		Me.StopAllCoroutines()
		If Me.currentCenterPoint IsNot Nothing Then
			MyBase.StartCoroutine(Me.kill_orbs_cr())
		End If
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06001DD1 RID: 7633 RVA: 0x00111819 File Offset: 0x0010FC19
	Private Sub SFX_IntroContinue()
		AudioManager.Play("intro_continue")
		Me.emitAudioFromObject.Add("intro_continue")
	End Sub

	' Token: 0x06001DD2 RID: 7634 RVA: 0x00111835 File Offset: 0x0010FC35
	Private Sub SFX_Death()
		AudioManager.Play("dice_palace_rabbit_death")
		Me.emitAudioFromObject.Add("dice_palace_rabbit_death")
	End Sub

	' Token: 0x06001DD3 RID: 7635 RVA: 0x00111851 File Offset: 0x0010FC51
	Private Sub SFX_AttackStart()
		AudioManager.Play("dice_palace_rabbit_attack_start")
		Me.emitAudioFromObject.Add("dice_palace_rabbit_attack_start")
	End Sub

	' Token: 0x06001DD4 RID: 7636 RVA: 0x0011186D File Offset: 0x0010FC6D
	Private Sub SFX_Attack()
		If Not Me.AttackSFXPlaying Then
			AudioManager.PlayLoop("dice_palace_rabbit_attack_loop")
			Me.emitAudioFromObject.Add("dice_palace_rabbit_attack_loop")
			Me.AttackSFXPlaying = True
		End If
	End Sub

	' Token: 0x06001DD5 RID: 7637 RVA: 0x0011189B File Offset: 0x0010FC9B
	Private Sub SFX_AttackEnd()
		AudioManager.[Stop]("dice_palace_rabbit_attack_loop")
		AudioManager.Play("dice_palace_rabbit_attack_end")
		Me.emitAudioFromObject.Add("dice_palace_rabbit_attack_end")
		Me.AttackSFXPlaying = False
	End Sub

	' Token: 0x06001DD6 RID: 7638 RVA: 0x001118C8 File Offset: 0x0010FCC8
	Private Sub SFX_IdleRock()
		AudioManager.Play("idle_rock")
		Me.emitAudioFromObject.Add("idle_rock")
	End Sub

	' Token: 0x06001DD7 RID: 7639 RVA: 0x001118E4 File Offset: 0x0010FCE4
	Private Sub SFX_StickTwirl()
		If Not Me.StickTwirlActive Then
			Me.StickTwirlActive = True
			AudioManager.PlayLoop("stick_twirl")
			Me.emitAudioFromObject.Add("stick_twirl")
		End If
	End Sub

	' Token: 0x06001DD8 RID: 7640 RVA: 0x00111912 File Offset: 0x0010FD12
	Private Sub SFX_StickTwirlStop()
		Me.StickTwirlActive = False
		AudioManager.[Stop]("stick_twirl")
	End Sub

	' Token: 0x04002694 RID: 9876
	Private Const OrbAppearTime As Single = 2F

	' Token: 0x04002695 RID: 9877
	<SerializeField()>
	Private orbPrefab As AbstractProjectile

	' Token: 0x04002696 RID: 9878
	<SerializeField()>
	Private magicPrefab As DicePalaceRabbitLevelMagic

	' Token: 0x04002697 RID: 9879
	<SerializeField()>
	Private platform1 As FlowerLevelPlatform

	' Token: 0x04002698 RID: 9880
	<SerializeField()>
	Private platform2 As FlowerLevelPlatform

	' Token: 0x04002699 RID: 9881
	<SerializeField()>
	Private explosionPrefab As Effect

	' Token: 0x0400269A RID: 9882
	Private attacking As Boolean

	' Token: 0x0400269B RID: 9883
	Private isDying As Boolean

	' Token: 0x0400269C RID: 9884
	Private playerOneCircleIndex As Integer

	' Token: 0x0400269D RID: 9885
	Private damageDealer As DamageDealer

	' Token: 0x0400269E RID: 9886
	Private damageReceiver As DamageReceiver

	' Token: 0x0400269F RID: 9887
	Private isMagicParryTop As Boolean

	' Token: 0x040026A0 RID: 9888
	Private parryCurrentIndex As Integer

	' Token: 0x040026A1 RID: 9889
	Private currentCenterPoint As GameObject

	' Token: 0x040026A2 RID: 9890
	Private AttackSFXPlaying As Boolean

	' Token: 0x040026A3 RID: 9891
	Private StickTwirlActive As Boolean

	' Token: 0x020005E1 RID: 1505
	Public Enum State
		' Token: 0x040026A6 RID: 9894
		Idle
		' Token: 0x040026A7 RID: 9895
		MagicWand
		' Token: 0x040026A8 RID: 9896
		MagicParry
	End Enum
End Class
