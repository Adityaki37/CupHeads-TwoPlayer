Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000500 RID: 1280
Public Class BatLevelBat
	Inherits LevelProperties.Bat.Entity

	' Token: 0x17000325 RID: 805
	' (get) Token: 0x0600168B RID: 5771 RVA: 0x000CA9B1 File Offset: 0x000C8DB1
	' (set) Token: 0x0600168C RID: 5772 RVA: 0x000CA9B9 File Offset: 0x000C8DB9
	Public Property state As BatLevelBat.State

	' Token: 0x0600168D RID: 5773 RVA: 0x000CA9C4 File Offset: 0x000C8DC4
	Public Overrides Sub LevelInit(properties As LevelProperties.Bat)
		MyBase.LevelInit(properties)
		AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.speed = properties.CurrentState.movement.movementSpeed
		Me.originalSpeed = Me.speed
		Me.inMovingPhase = True
		Me.moving = True
		Me.startPosition = MyBase.transform.position
		Me.startPosition.y = properties.CurrentState.movement.startPosY
		MyBase.transform.position = Me.startPosition
		Me.damageDealer = New DamageDealer(1F, 0.2F, True, False, False)
		Me.damageDealer.SetDirection(DamageDealer.Direction.Left, MyBase.transform)
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x0600168E RID: 5774 RVA: 0x000CAA94 File Offset: 0x000C8E94
	Private Iterator Function intro_cr() As IEnumerator
		Dim p As LevelProperties.Bat.State = MyBase.properties.CurrentState
		Me.anglePattern = p.batBouncer.bounceAngleString.GetRandom().Split(New Char() { ","c })
		Me.angleIndex = Global.UnityEngine.Random.Range(0, Me.anglePattern.Length)
		Yield CupheadTime.WaitForSeconds(Me, 5F)
		MyBase.animator.SetTrigger("OnIntro")
		MyBase.StartCoroutine(Me.bat_movement_cr())
		Me.state = BatLevelBat.State.Idle
		Return
	End Function

	' Token: 0x0600168F RID: 5775 RVA: 0x000CAAAF File Offset: 0x000C8EAF
	Public Sub Die()
		MyBase.animator.SetTrigger("OnDeath")
	End Sub

	' Token: 0x06001690 RID: 5776 RVA: 0x000CAAC1 File Offset: 0x000C8EC1
	Private Sub Update()
		Me.damageDealer.Update()
		If Me.state <> BatLevelBat.State.Phase2 Then
			Me.VaryingSpeed()
		End If
	End Sub

	' Token: 0x06001691 RID: 5777 RVA: 0x000CAAE0 File Offset: 0x000C8EE0
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06001692 RID: 5778 RVA: 0x000CAAF7 File Offset: 0x000C8EF7
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001693 RID: 5779 RVA: 0x000CAB0C File Offset: 0x000C8F0C
	Private Sub OnTurnAnimComplete()
		MyBase.transform.SetScale(New Single?(MyBase.transform.localScale.x * -1F), Nothing, Nothing)
	End Sub

	' Token: 0x06001694 RID: 5780 RVA: 0x000CAB54 File Offset: 0x000C8F54
	Private Iterator Function bat_movement_cr() As IEnumerator
		Dim offset As Single = 200F
		Dim stopDist As Single = 100F
		Dim pos As Vector3 = MyBase.transform.position
		While True
			If Me.direction = BatLevelBat.Direction.Left Then
				While MyBase.transform.position.x > -640F + offset
					If Me.moving Then
						Dim num As Single = -640F + offset - MyBase.transform.position.x
						num = Mathf.Abs(num)
						pos.x = Mathf.MoveTowards(MyBase.transform.position.x, -640F + offset, Me.speed * CupheadTime.Delta)
						If num < stopDist Then
							Me.slowDown = True
						End If
						MyBase.transform.position = pos
					End If
					Yield Nothing
				End While
				MyBase.animator.SetTrigger("OnTurn")
				If Not Me.inMovingPhase Then
					Exit For
				End If
				Me.direction = BatLevelBat.Direction.Right
				Yield Nothing
			ElseIf Me.direction = BatLevelBat.Direction.Right Then
				While MyBase.transform.position.x < 640F - offset
					If Me.moving Then
						Dim num2 As Single = 640F - offset - MyBase.transform.position.x
						num2 = Mathf.Abs(num2)
						pos.x = Mathf.MoveTowards(MyBase.transform.position.x, 640F - offset, Me.speed * CupheadTime.Delta)
						If num2 < stopDist Then
							Me.slowDown = True
						End If
						MyBase.transform.position = pos
					End If
					Yield Nothing
				End While
				MyBase.animator.SetTrigger("OnTurn")
				If Not Me.inMovingPhase Then
					GoTo IL_0336
				End If
				Me.direction = BatLevelBat.Direction.Left
				Yield Nothing
			End If
			Yield Nothing
		End While
		Me.onRight = False
		MyBase.StartCoroutine(Me.phase_2_handler_cr())
		GoTo IL_0399
		IL_0336:
		Me.onRight = True
		MyBase.StartCoroutine(Me.phase_2_handler_cr())
		IL_0399:
		Return
	End Function

	' Token: 0x06001695 RID: 5781 RVA: 0x000CAB70 File Offset: 0x000C8F70
	Private Sub VaryingSpeed()
		Dim num As Single = 10F
		If Me.slowDown Then
			If Me.speed <= 50F Then
				Me.slowDown = False
			Else
				Me.speed -= num
			End If
		ElseIf Me.speed < Me.originalSpeed Then
			Me.speed += num
		End If
	End Sub

	' Token: 0x06001696 RID: 5782 RVA: 0x000CABDC File Offset: 0x000C8FDC
	Public Sub StartBouncer()
		If Me.pattern IsNot Nothing Then
			MyBase.StopCoroutine(Me.pattern)
		End If
		Me.pattern = MyBase.StartCoroutine(Me.bouncer_cr())
	End Sub

	' Token: 0x06001697 RID: 5783 RVA: 0x000CAC08 File Offset: 0x000C9008
	Private Sub SpawnBouncer()
		Dim num As Single = 0F
		Parser.FloatTryParse(Me.anglePattern(Me.angleIndex), num)
		num = If((Me.direction <> BatLevelBat.Direction.Right), num, (num + 90F))
		Dim batLevelBouncer As BatLevelBouncer = Global.UnityEngine.[Object].Instantiate(Of BatLevelBouncer)(Me.bouncerPrefab)
		batLevelBouncer.Init(MyBase.properties.CurrentState.batBouncer, Me.bouncerRoot.position, num)
		Me.angleIndex = (Me.angleIndex + 1) Mod Me.anglePattern.Length
	End Sub

	' Token: 0x06001698 RID: 5784 RVA: 0x000CAC94 File Offset: 0x000C9094
	Private Iterator Function bouncer_cr() As IEnumerator
		Dim p As LevelProperties.Bat.BatBouncer = MyBase.properties.CurrentState.batBouncer
		Me.state = BatLevelBat.State.Bouncer
		Me.moving = False
		Yield CupheadTime.WaitForSeconds(Me, p.stopDelay)
		Me.SpawnBouncer()
		Me.moving = True
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = BatLevelBat.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06001699 RID: 5785 RVA: 0x000CACAF File Offset: 0x000C90AF
	Public Sub StartGoblin()
		MyBase.StartCoroutine(Me.goblin_cr())
	End Sub

	' Token: 0x0600169A RID: 5786 RVA: 0x000CACC0 File Offset: 0x000C90C0
	Private Iterator Function goblin_cr() As IEnumerator
		Dim p As LevelProperties.Bat.Goblins = MyBase.properties.CurrentState.goblins
		Dim delayPattern As String() = p.appearDelayString.GetRandom().Split(New Char() { ","c })
		Dim entrancePattern As String() = p.entranceString.GetRandom().Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayPattern.Length)
		Dim entranceIndex As Integer = Global.UnityEngine.Random.Range(0, entrancePattern.Length)
		Dim counter As Integer = 0
		Dim delay As Single = 0F
		Dim startX As Single = 0F
		Dim pickShooter As Single = CSng(p.shooterOccuranceRange.RandomInt())
		Dim startY As Single = CSng(Level.Current.Ground) + 100F
		Dim isShooter As Boolean = False
		While True
			Parser.FloatTryParse(delayPattern(delayIndex), delay)
			Yield CupheadTime.WaitForSeconds(Me, delay)
			If entrancePattern(entranceIndex)(0) = "R"c Then
				startX = 640F
				Dim startPos As Vector2 = New Vector2(startX, startY)
				Me.SpawnGoblin(False, startPos, isShooter)
			ElseIf entrancePattern(entranceIndex)(0) = "L"c Then
				startX = -640F
				Dim startPos As Vector2 = New Vector2(startX, startY)
				Me.SpawnGoblin(True, startPos, isShooter)
			End If
			isShooter = False
			counter += 1
			If CSng(counter) = pickShooter Then
				isShooter = True
				counter = 0
			End If
			entranceIndex = (entranceIndex + 1) Mod entrancePattern.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600169B RID: 5787 RVA: 0x000CACDC File Offset: 0x000C90DC
	Private Sub SpawnGoblin(leftSide As Boolean, startPos As Vector2, isShooter As Boolean)
		Dim goblins As LevelProperties.Bat.Goblins = MyBase.properties.CurrentState.goblins
		Dim batLevelGoblin As BatLevelGoblin = Global.UnityEngine.[Object].Instantiate(Of BatLevelGoblin)(Me.goblinPrefab)
		batLevelGoblin.Init(goblins, startPos, leftSide, isShooter, goblins.HP)
	End Sub

	' Token: 0x0600169C RID: 5788 RVA: 0x000CAD16 File Offset: 0x000C9116
	Public Sub StartLightning()
		If Me.pattern IsNot Nothing Then
			MyBase.StopCoroutine(Me.pattern)
		End If
		Me.pattern = MyBase.StartCoroutine(Me.lightning_cr())
	End Sub

	' Token: 0x0600169D RID: 5789 RVA: 0x000CAD41 File Offset: 0x000C9141
	Private Sub SpawnCloud(startPos As Vector2)
		Me.lightning = Global.UnityEngine.[Object].Instantiate(Of BatLevelLightning)(Me.lightningPrefab)
		Me.lightning.Init(MyBase.properties.CurrentState.batLightning, startPos)
	End Sub

	' Token: 0x0600169E RID: 5790 RVA: 0x000CAD70 File Offset: 0x000C9170
	Private Iterator Function lightning_cr() As IEnumerator
		Me.state = BatLevelBat.State.Lightning
		Me.moving = False
		Dim p As LevelProperties.Bat.BatLightning = MyBase.properties.CurrentState.batLightning
		Dim offsetString As String() = p.centerOffset.GetRandom().Split(New Char() { ","c })
		Dim offsetIndex As Integer = 0
		Dim offset As Single = 0F
		Dim pos As Vector2 = Vector2.zero
		pos.y = p.cloudHeight
		Dim num As Integer = 0
		While CSng(num) < p.cloudCount
			Parser.FloatTryParse(offsetString(offsetIndex), offset)
			pos.x = p.cloudDistance * CSng(num) + offset - CSng((Level.Current.Right / 2))
			Me.SpawnCloud(pos)
			offsetIndex = offsetIndex Mod offsetString.Length
			num += 1
		End While
		While Me.lightning IsNot Nothing
			Yield Nothing
		End While
		Me.moving = True
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = BatLevelBat.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x0600169F RID: 5791 RVA: 0x000CAD8B File Offset: 0x000C918B
	Public Sub StartPhase2()
		Me.inMovingPhase = False
	End Sub

	' Token: 0x060016A0 RID: 5792 RVA: 0x000CAD94 File Offset: 0x000C9194
	Private Iterator Function phase_2_handler_cr() As IEnumerator
		Dim yPos As Single = MyBase.transform.position.y - 100F
		Me.state = BatLevelBat.State.Phase2
		Me.speed = Me.originalSpeed
		While MyBase.transform.position.y <> yPos
			Dim pos As Vector3 = MyBase.transform.position
			pos.y = Mathf.MoveTowards(MyBase.transform.position.y, yPos, Me.speed * CupheadTime.Delta)
			MyBase.transform.position = pos
			Yield Nothing
		End While
		Me.StartMiniBats()
		Me.StartPentagram()
		Me.StartCross()
		Yield Nothing
		Return
	End Function

	' Token: 0x060016A1 RID: 5793 RVA: 0x000CADAF File Offset: 0x000C91AF
	Public Sub StartMiniBats()
		MyBase.StartCoroutine(Me.mini_bats_cr())
	End Sub

	' Token: 0x060016A2 RID: 5794 RVA: 0x000CADC0 File Offset: 0x000C91C0
	Private Sub SpawnMiniBat(angle As Single)
		Dim miniBats As LevelProperties.Bat.MiniBats = MyBase.properties.CurrentState.miniBats
		Dim num As Single = If((Not Me.onRight), angle, (-angle))
		Dim num2 As Single = If((Not Me.onRight), miniBats.speedX, (-miniBats.speedX))
		Me.minibatPrefab.Create(Me.coffinRoot.position, num, num2, miniBats.speedY, miniBats.yMinMax, miniBats.HP)
	End Sub

	' Token: 0x060016A3 RID: 5795 RVA: 0x000CAE4C File Offset: 0x000C924C
	Private Iterator Function mini_bats_cr() As IEnumerator
		Dim p As LevelProperties.Bat.MiniBats = MyBase.properties.CurrentState.miniBats
		Dim angleString As String() = p.batAngleString.GetRandom().Split(New Char() { ","c })
		Dim angleIndex As Integer = Global.UnityEngine.Random.Range(0, angleString.Length)
		Dim angle As Single = 0F
		While MyBase.properties.CurrentState.stateName = LevelProperties.Bat.States.Coffin
			Parser.FloatTryParse(angleString(angleIndex), angle)
			Me.SpawnMiniBat(angle)
			Yield CupheadTime.WaitForSeconds(Me, p.delay)
			angleIndex = (angleIndex + 1) Mod angleString.Length
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060016A4 RID: 5796 RVA: 0x000CAE67 File Offset: 0x000C9267
	Public Sub StartPentagram()
		MyBase.StartCoroutine(Me.pentagram_cr())
	End Sub

	' Token: 0x060016A5 RID: 5797 RVA: 0x000CAE78 File Offset: 0x000C9278
	Private Sub SpawnPentagram()
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim position As Vector3 = Me.pentagramRoot.position
		position.y = CSng(Level.Current.Ground) - 10F
		Dim batLevelPentagram As BatLevelPentagram = Global.UnityEngine.[Object].Instantiate(Of BatLevelPentagram)(Me.pentagramPrefab)
		batLevelPentagram.Init(position, MyBase.properties.CurrentState.pentagrams, [next], Me.onRight)
	End Sub

	' Token: 0x060016A6 RID: 5798 RVA: 0x000CAEE0 File Offset: 0x000C92E0
	Private Iterator Function pentagram_cr() As IEnumerator
		Dim p As LevelProperties.Bat.Pentagrams = MyBase.properties.CurrentState.pentagrams
		Dim delayString As String() = p.pentagramDelayString.GetRandom().Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayString.Length)
		Dim delay As Single = 0F
		While MyBase.properties.CurrentState.stateName = LevelProperties.Bat.States.Coffin
			Parser.FloatTryParse(delayString(delayIndex), delay)
			Yield CupheadTime.WaitForSeconds(Me, delay)
			Me.SpawnPentagram()
			delayIndex = delayIndex Mod delayString.Length
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060016A7 RID: 5799 RVA: 0x000CAEFB File Offset: 0x000C92FB
	Public Sub StartCross()
		MyBase.StartCoroutine(Me.cross_cr())
	End Sub

	' Token: 0x060016A8 RID: 5800 RVA: 0x000CAF0C File Offset: 0x000C930C
	Private Sub SpawnCross(count As Integer)
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Me.cross = Global.UnityEngine.[Object].Instantiate(Of BatLevelCross)(Me.crossPrefab)
		Me.cross.Init(Me.bouncerRoot.position, MyBase.properties.CurrentState.crossToss, count, [next])
	End Sub

	' Token: 0x060016A9 RID: 5801 RVA: 0x000CAF60 File Offset: 0x000C9360
	Private Iterator Function cross_cr() As IEnumerator
		Dim p As LevelProperties.Bat.CrossToss = MyBase.properties.CurrentState.crossToss
		Dim delayString As String() = p.crossDelayString.GetRandom().Split(New Char() { ","c })
		Dim countString As String() = p.attackCount.GetRandom().Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayString.Length)
		Dim countIndex As Integer = Global.UnityEngine.Random.Range(0, countString.Length)
		Dim delay As Single = 0F
		Dim count As Integer = 0
		While MyBase.properties.CurrentState.stateName = LevelProperties.Bat.States.Coffin
			If Me.cross Is Nothing Then
				Parser.FloatTryParse(delayString(delayIndex), delay)
				Parser.IntTryParse(countString(countIndex), count)
				Yield CupheadTime.WaitForSeconds(Me, delay)
				Me.SpawnCross(count)
			End If
			delayIndex = delayIndex Mod delayString.Length
			count = count Mod countString.Length
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060016AA RID: 5802 RVA: 0x000CAF7B File Offset: 0x000C937B
	Public Sub StartPhase3()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.shoot_cr())
		MyBase.StartCoroutine(Me.soul_cr())
	End Sub

	' Token: 0x060016AB RID: 5803 RVA: 0x000CAFA0 File Offset: 0x000C93A0
	Private Iterator Function shoot_cr() As IEnumerator
		Dim p As LevelProperties.Bat.WolfFire = MyBase.properties.CurrentState.wolfFire
		Dim player As AbstractPlayerController = PlayerManager.GetNext()
		Dim counter As Single = 0F
		While MyBase.properties.CurrentState.stateName = LevelProperties.Bat.States.Wolf
			Yield CupheadTime.WaitForSeconds(Me, p.bulletDelay)
			Me.ShootBullet(p.bulletSpeed, player)
			counter += 1F
			If counter >= p.bulletAimCount Then
				player = PlayerManager.GetNext()
				counter = 0F
			End If
		End While
		Return
	End Function

	' Token: 0x060016AC RID: 5804 RVA: 0x000CAFBC File Offset: 0x000C93BC
	Private Sub ShootBullet(speed As Single, player As AbstractPlayerController)
		Dim num As Single = player.transform.position.x - Me.bouncerRoot.position.x
		Dim num2 As Single = player.transform.position.y - Me.bouncerRoot.position.y
		Dim num3 As Single = Mathf.Atan2(num2, num) * 57.29578F
		Me.wolfProjectile.Create(Me.bouncerRoot.position, num3, speed)
	End Sub

	' Token: 0x060016AD RID: 5805 RVA: 0x000CB04C File Offset: 0x000C944C
	Private Sub SpawnSoul()
		Dim wolfSoul As LevelProperties.Bat.WolfSoul = MyBase.properties.CurrentState.wolfSoul
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim batLevelHomingSoul As BatLevelHomingSoul = Global.UnityEngine.[Object].Instantiate(Of BatLevelHomingSoul)(Me.soulPrefab)
		batLevelHomingSoul.Init(Me.bouncerRoot.position, [next], wolfSoul)
	End Sub

	' Token: 0x060016AE RID: 5806 RVA: 0x000CB094 File Offset: 0x000C9494
	Private Iterator Function soul_cr() As IEnumerator
		Me.SpawnSoul()
		Yield Nothing
		Return
	End Function

	' Token: 0x04001FDC RID: 8156
	<SerializeField()>
	Private bouncerRoot As Transform

	' Token: 0x04001FDD RID: 8157
	<SerializeField()>
	Private coffinRoot As Transform

	' Token: 0x04001FDE RID: 8158
	<SerializeField()>
	Private pentagramRoot As Transform

	' Token: 0x04001FDF RID: 8159
	<SerializeField()>
	Private bouncerPrefab As BatLevelBouncer

	' Token: 0x04001FE0 RID: 8160
	<SerializeField()>
	Private goblinPrefab As BatLevelGoblin

	' Token: 0x04001FE1 RID: 8161
	<SerializeField()>
	Private minibatPrefab As BatLevelMiniBat

	' Token: 0x04001FE2 RID: 8162
	<SerializeField()>
	Private lightningPrefab As BatLevelLightning

	' Token: 0x04001FE3 RID: 8163
	Private lightning As BatLevelLightning

	' Token: 0x04001FE4 RID: 8164
	<SerializeField()>
	Private pentagramPrefab As BatLevelPentagram

	' Token: 0x04001FE5 RID: 8165
	<SerializeField()>
	Private wolfProjectile As BasicProjectile

	' Token: 0x04001FE6 RID: 8166
	<SerializeField()>
	Private crossPrefab As BatLevelCross

	' Token: 0x04001FE7 RID: 8167
	Private cross As BatLevelCross

	' Token: 0x04001FE8 RID: 8168
	<SerializeField()>
	Private soulPrefab As BatLevelHomingSoul

	' Token: 0x04001FE9 RID: 8169
	Private direction As BatLevelBat.Direction = BatLevelBat.Direction.Left

	' Token: 0x04001FEB RID: 8171
	Private damageDealer As DamageDealer

	' Token: 0x04001FEC RID: 8172
	Private slowDown As Boolean

	' Token: 0x04001FED RID: 8173
	Private onRight As Boolean

	' Token: 0x04001FEE RID: 8174
	Private inMovingPhase As Boolean

	' Token: 0x04001FEF RID: 8175
	Private moving As Boolean

	' Token: 0x04001FF0 RID: 8176
	Private anglePattern As String()

	' Token: 0x04001FF1 RID: 8177
	Private angleIndex As Integer

	' Token: 0x04001FF2 RID: 8178
	Private speed As Single

	' Token: 0x04001FF3 RID: 8179
	Private originalSpeed As Single

	' Token: 0x04001FF4 RID: 8180
	Private startPosition As Vector3

	' Token: 0x04001FF5 RID: 8181
	Private pattern As Coroutine

	' Token: 0x02000501 RID: 1281
	Public Enum Direction
		' Token: 0x04001FF7 RID: 8183
		Right
		' Token: 0x04001FF8 RID: 8184
		Left
	End Enum

	' Token: 0x02000502 RID: 1282
	Public Enum State
		' Token: 0x04001FFA RID: 8186
		Idle
		' Token: 0x04001FFB RID: 8187
		Bouncer
		' Token: 0x04001FFC RID: 8188
		Lightning
		' Token: 0x04001FFD RID: 8189
		Phase2
	End Enum
End Class
