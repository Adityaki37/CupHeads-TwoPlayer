Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.Linq
Imports UnityEngine

' Token: 0x02000536 RID: 1334
Public Class ChessBishopLevelBishop
	Inherits LevelProperties.ChessBishop.Entity

	' Token: 0x06001820 RID: 6176 RVA: 0x000DA13C File Offset: 0x000D853C
	Private Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		For i As Integer = 0 To Me.candles.Length - 1
			Me.candles(i).Init(MyBase.properties.CurrentState.candle.candleDistToBlowout)
		Next
	End Sub

	' Token: 0x06001821 RID: 6177 RVA: 0x000DA18F File Offset: 0x000D858F
	Public Overrides Sub LevelInit(properties As LevelProperties.ChessBishop)
		MyBase.LevelInit(properties)
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.onIntroEventHandler
		Me.setupPatternStrings()
	End Sub

	' Token: 0x06001822 RID: 6178 RVA: 0x000DA1B4 File Offset: 0x000D85B4
	Private Sub UpdateBodyFade()
		Dim num As Single = Mathf.Clamp(Me.bodyOpacity, 0F, 1F)
		Me.bodyRenderer.color = New Color(1F, 1F, 1F, num)
		Me.bodyRenderer.material.SetFloat("_BlurAmount", (1F - num) * 5F)
		Me.bodyRenderer.material.SetFloat("_BlurLerp", (1F - num) * 5F)
	End Sub

	' Token: 0x06001823 RID: 6179 RVA: 0x000DA23C File Offset: 0x000D863C
	Private Sub FixedUpdate()
		If PlayerManager.GetPlayer(PlayerId.PlayerOne) AndAlso Not PlayerManager.GetPlayer(PlayerId.PlayerOne).IsDead Then
			Me.playerMask(0).transform.position = PlayerManager.GetPlayer(PlayerId.PlayerOne).transform.position + Vector3.up * 50F
		End If
		If PlayerManager.GetPlayer(PlayerId.PlayerTwo) AndAlso Not PlayerManager.GetPlayer(PlayerId.PlayerTwo).IsDead Then
			Me.playerMask(1).transform.position = PlayerManager.GetPlayer(PlayerId.PlayerTwo).transform.position + Vector3.up * 50F
		End If
		If Me.introPlaying OrElse Me.dead Then
			Return
		End If
		Me.bodyOpacity -= CupheadTime.FixedDelta * Me.fadeRate
		Me.UpdateBodyFade()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.FixedUpdate()
		End If
	End Sub

	' Token: 0x06001824 RID: 6180 RVA: 0x000DA341 File Offset: 0x000D8741
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06001825 RID: 6181 RVA: 0x000DA358 File Offset: 0x000D8758
	Public Sub StartNewPhase()
		Me.stateDidChange = True
		Me.cancelShoot()
		Me.StopAllCoroutines()
		Me.candleOrderMainIndex = Me.candleOrderMainIndex Mod MyBase.properties.CurrentState.candle.candleOrder.Length
		Me.setupPatternStrings()
		MyBase.StartCoroutine(Me.disappear_cr())
	End Sub

	' Token: 0x06001826 RID: 6182 RVA: 0x000DA3B0 File Offset: 0x000D87B0
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.OnParry(player)
		Me.cancelShoot()
		MyBase.properties.DealDamage(If((Not PlayerManager.BothPlayersActive()), 10F, ChessKingLevelKing.multiplayerDamageNerf))
		If MyBase.properties.CurrentHealth <= 0F Then
			Me.die()
		Else
			Me.bodyOpacity = 1.75F
			Me.bodyAnimator.SetTrigger("Hit")
			Me.bodyExplosion.Create(Me.bodyExplosionSpawnPoint.position)
			Me.turnDormant(Me.stateDidChange)
			Me.stateDidChange = False
		End If
	End Sub

	' Token: 0x06001827 RID: 6183 RVA: 0x000DA453 File Offset: 0x000D8853
	Private Sub onIntroEventHandler()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001828 RID: 6184 RVA: 0x000DA464 File Offset: 0x000D8864
	Private Iterator Function intro_cr() As IEnumerator
		Me.bodyAnimator.SetTrigger("StartIntro")
		Yield Me.bodyAnimator.WaitForAnimationToEnd(Me, "Intro.End", False, True)
		Me.isPathTwo = MathUtils.RandomBool()
		Me.startPath()
		Yield CupheadTime.WaitForSeconds(Me, 0.55F)
		MyBase.animator.SetBool("CanParry", True)
		MyBase.animator.SetTrigger("Appear")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "AppearActive", False, True)
		Me.candleOrderMainIndex = Global.UnityEngine.Random.Range(0, MyBase.properties.CurrentState.candle.candleOrder.Length)
		Me.candlesHolder.SetActive(True)
		Me.canMove = True
		Me.introPlaying = False
		Return
	End Function

	' Token: 0x06001829 RID: 6185 RVA: 0x000DA480 File Offset: 0x000D8880
	Private Sub turnDormant(willDisappear As Boolean)
		Me._canParry = False
		If MyBase.properties.CurrentHealth > 0F Then
			MyBase.StartCoroutine(Me.candles_cr())
		End If
		If Not willDisappear Then
			MyBase.animator.SetTrigger("ToDormant")
			MyBase.StartCoroutine(Me.postHitToggleCollider_cr())
		End If
	End Sub

	' Token: 0x0600182A RID: 6186 RVA: 0x000DA4DC File Offset: 0x000D88DC
	Private Iterator Function postHitToggleCollider_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.bishop.colliderOffTime)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Return
	End Function

	' Token: 0x0600182B RID: 6187 RVA: 0x000DA4F8 File Offset: 0x000D88F8
	Private Iterator Function candles_cr() As IEnumerator
		While Me.disappearingState = ChessBishopLevelBishop.DisappearingState.Disappearing
			Yield Nothing
		End While
		Dim p As LevelProperties.ChessBishop.Candle = MyBase.properties.CurrentState.candle
		Dim candleOrder As String() = p.candleOrder(Me.candleOrderMainIndex).Split(New Char() { ","c })
		Dim length As Integer = candleOrder.Length + If((Not PlayerManager.BothPlayersActive()), 0, 3)
		Dim activeCandles As ChessBishopLevelCandle() = New ChessBishopLevelCandle(length - 1) {}
		Dim index As Integer = 0
		For i As Integer = 0 To candleOrder.Length - 1
			Parser.IntTryParse(candleOrder(i), index)
			Me.candles(index).LightUp()
			activeCandles(i) = Me.candles(index)
		Next
		If PlayerManager.BothPlayersActive() Then
			Dim list As List(Of ChessBishopLevelCandle) = Me.candles.Where(Function(c As ChessBishopLevelCandle) Not c.isLit).ToList()
			For j As Integer = 0 To 3 - 1
				If list.Count > 0 Then
					index = Global.UnityEngine.Random.Range(0, list.Count)
					list(index).LightUp()
					activeCandles(candleOrder.Length + j) = list(index)
					list.RemoveAt(index)
				End If
			Next
		End If
		Me.candleOrderMainIndex = (Me.candleOrderMainIndex + 1) Mod p.candleOrder.Length
		Yield Nothing
		Dim candlesStillLit As Boolean = True
		While candlesStillLit
			candlesStillLit = False
			For k As Integer = 0 To activeCandles.Length - 1
				If activeCandles(k) IsNot Nothing AndAlso activeCandles(k).isLit Then
					candlesStillLit = True
					Exit For
				End If
			Next
			Yield Nothing
		End While
		Me.cancelShoot()
		If Me.disappearingState = ChessBishopLevelBishop.DisappearingState.Disappearing Then
			Me._canParry = True
			Return
		End If
		While Me.disappearingState = ChessBishopLevelBishop.DisappearingState.Reappearing
			Yield Nothing
		End While
		Me._canParry = True
		MyBase.animator.SetTrigger("ToActive")
		Return
	End Function

	' Token: 0x0600182C RID: 6188 RVA: 0x000DA514 File Offset: 0x000D8914
	Private Iterator Function disappear_cr() As IEnumerator
		Me.disappearingState = ChessBishopLevelBishop.DisappearingState.Disappearing
		Me.isFirstPhase = False
		Me.canMove = False
		Dim collider As Collider2D = MyBase.GetComponent(Of Collider2D)()
		collider.enabled = False
		MyBase.animator.SetTrigger("HitDisappear")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "HitDisappear", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.invisibleTime.PopFloat())
		Me.disappearingState = ChessBishopLevelBishop.DisappearingState.Reappearing
		Me.isPathTwo = Not Me.isPathTwo
		Me.startPath()
		MyBase.animator.SetBool("CanParry", Me._canParry)
		MyBase.animator.SetTrigger("Appear")
		Dim animationName As String = If((Not MyBase.canParry), "AppearDormant", "AppearActive")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, animationName, False, True)
		Me.canMove = True
		collider.enabled = True
		Me.disappearingState = ChessBishopLevelBishop.DisappearingState.None
		Return
	End Function

	' Token: 0x0600182D RID: 6189 RVA: 0x000DA52F File Offset: 0x000D892F
	Private Sub startPath()
		If Me.isPathTwo Then
			MyBase.StartCoroutine(Me.moveHorizontal_cr())
		Else
			MyBase.StartCoroutine(Me.moveVertical_cr())
		End If
	End Sub

	' Token: 0x0600182E RID: 6190 RVA: 0x000DA55C File Offset: 0x000D895C
	Private Iterator Function moveVertical_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim pivotOffset As Vector3 = Vector3.up * 2F * 150F
		Me.invert = True
		Dim value As Single = -1F
		Dim speed As Single = MyBase.properties.CurrentState.bishop.movementSpeed
		Dim angle As Single = 3.9269907F
		If Not Me.isFirstPhase Then
			Dim num As Single = MyBase.GetComponent(Of CircleCollider2D)().radius * 1.5F
			angle = ChessBishopLevelBishop.findMoveVerticalInitialAngle(num, value, Me.invert, speed, Me.pivotPoint, pivotOffset)
		End If
		MyBase.StartCoroutine(Me.spawnProjectiles_cr())
		While True
			MyBase.transform.position = ChessBishopLevelBishop.calculateMoveVerticalPosition(angle, value, Me.invert, speed, Me.pivotPoint, pivotOffset)
			While Not Me.canMove
				Yield Nothing
			End While
			Yield wait
		End While
		Return
	End Function

	' Token: 0x0600182F RID: 6191 RVA: 0x000DA578 File Offset: 0x000D8978
	Private Shared Function findMoveVerticalInitialAngle(minimumDistance As Single, value As Single, invert As Boolean, speed As Single, pivotPoint As Transform, pivotOffset As Vector3) As Single
		Dim num As Single = minimumDistance * minimumDistance
		Dim list As List(Of Vector3) = New List(Of Vector3)()
		If PlayerManager.DoesPlayerExist(PlayerId.PlayerOne) Then
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerOne).center)
		End If
		If PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo) Then
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)
		End If
		Dim i As Integer = 0
		Dim num2 As Single = 0F
		While i < 20
			i += 1
			num2 = Global.UnityEngine.Random.Range(0F, 6.2831855F)
			Dim num3 As Single = num2
			Dim num4 As Single = value
			Dim flag As Boolean = invert
			Dim vector As Vector3 = ChessBishopLevelBishop.calculateMoveVerticalPosition(num3, num4, flag, speed, pivotPoint, pivotOffset)
			Dim flag2 As Boolean = False
			For Each vector2 As Vector3 In list
				If(vector - vector2).sqrMagnitude < num Then
					flag2 = True
				End If
			Next
			If Not flag2 Then
				Exit While
			End If
		End While
		Return num2
	End Function

	' Token: 0x06001830 RID: 6192 RVA: 0x000DA67C File Offset: 0x000D8A7C
	Private Shared Function calculateMoveVerticalPosition(ByRef angle As Single, ByRef value As Single, ByRef invert As Boolean, speed As Single, pivotPoint As Transform, pivotOffset As Vector3) As Vector3
		angle += speed * CupheadTime.FixedDelta
		If angle > 6.2831855F Then
			invert = Not invert
			angle -= 6.2831855F
		End If
		If angle < 0F Then
			angle += 6.2831855F
		End If
		Dim vector As Vector3
		If invert Then
			vector = pivotPoint.position + pivotOffset
			value = -1F
		Else
			vector = pivotPoint.position
			value = 1F
		End If
		Dim vector2 As Vector3 = New Vector3(-Mathf.Sin(angle) * 500F, Mathf.Cos(angle) * value * 150F, 0F)
		Return vector + vector2
	End Function

	' Token: 0x06001831 RID: 6193 RVA: 0x000DA72C File Offset: 0x000D8B2C
	Private Iterator Function moveHorizontal_cr() As IEnumerator
		Dim p As LevelProperties.ChessBishop.Bishop = MyBase.properties.CurrentState.bishop
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.invert = True
		Dim xSpeed As Single = p.xSpeed
		Dim amplitude As Single = p.amplitude
		Dim frequency As Single = p.freqMultiplier * 2F * 3.1415927F / (p.maxDistance * 2F)
		If Me.isFirstPhase Then
			MyBase.transform.position = New Vector3(500F, MyBase.transform.position.y)
		Else
			Dim num As Single = MyBase.GetComponent(Of CircleCollider2D)().radius * 1.5F
			MyBase.transform.position = ChessBishopLevelBishop.findMoveHorizontalInitialPosition(num, MyBase.transform.position.y, p.maxDistance, xSpeed, amplitude, frequency)
		End If
		MyBase.StartCoroutine(Me.spawnProjectiles_cr())
		Dim goalPos As Vector3 = MyBase.transform.position
		Dim distanceTraveled As Single = 1.5707964F
		While True
			MyBase.transform.position = ChessBishopLevelBishop.calculateMoveHorizontalPosition(goalPos, xSpeed, distanceTraveled, amplitude, frequency, p.maxDistance)
			While Not Me.canMove
				Yield Nothing
			End While
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001832 RID: 6194 RVA: 0x000DA748 File Offset: 0x000D8B48
	Private Shared Function findMoveHorizontalInitialPosition(minimumDistance As Single, yPosition As Single, maxDistance As Single, xSpeed As Single, amplitude As Single, frequency As Single) As Vector3
		Dim num As Single = minimumDistance * minimumDistance
		Dim list As List(Of Vector3) = New List(Of Vector3)()
		If PlayerManager.DoesPlayerExist(PlayerId.PlayerOne) Then
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerOne).center)
		End If
		If PlayerManager.DoesPlayerExist(PlayerId.PlayerTwo) Then
			list.Add(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)
		End If
		Dim i As Integer = 0
		Dim zero As Vector3 = Vector3.zero
		While i < 20
			i += 1
			zero = New Vector3(Global.UnityEngine.Random.Range(-maxDistance, maxDistance), yPosition)
			Dim vector As Vector3 = zero
			Dim num2 As Single = 1.5707964F
			Dim num3 As Single = xSpeed
			ChessBishopLevelBishop.calculateMoveHorizontalPosition(vector, num3, num2, amplitude, frequency, maxDistance)
			Dim flag As Boolean = False
			For Each vector2 As Vector3 In list
				If(zero - vector2).sqrMagnitude < num Then
					flag = True
				End If
			Next
			If Not flag Then
				Exit While
			End If
		End While
		Return zero
	End Function

	' Token: 0x06001833 RID: 6195 RVA: 0x000DA850 File Offset: 0x000D8C50
	Private Shared Function calculateMoveHorizontalPosition(ByRef goalPosition As Vector3, ByRef xSpeed As Single, ByRef distanceTravelled As Single, amplitude As Single, frequency As Single, maxDistance As Single) As Vector3
		Dim vector As Vector3 = goalPosition
		vector.x += xSpeed * CupheadTime.FixedDelta
		distanceTravelled += Mathf.Abs(xSpeed) * CupheadTime.FixedDelta
		If vector.x > maxDistance OrElse vector.x < -maxDistance Then
			xSpeed *= -1F
		End If
		vector.y = amplitude * Mathf.Sin(frequency * distanceTravelled)
		goalPosition = vector
		If vector.x < -maxDistance + 100F OrElse vector.x > maxDistance - 100F Then
			Dim num As Single = Mathf.InverseLerp(maxDistance - 100F, maxDistance, Mathf.Abs(vector.x))
			num *= 1.5707964F
			num = Mathf.Sin(num) * 100F / 2F
			vector.x = (maxDistance - 100F + num) * Mathf.Sign(vector.x)
		End If
		Return vector
	End Function

	' Token: 0x06001834 RID: 6196 RVA: 0x000DA94C File Offset: 0x000D8D4C
	Private Iterator Function spawnProjectiles_cr() As IEnumerator
		While Not Me.canMove
			Yield Nothing
		End While
		Dim p As LevelProperties.ChessBishop.Bishop = MyBase.properties.CurrentState.bishop
		Dim delayPattern As PatternString = New PatternString(p.attackDelayString, True, True)
		While True
			Dim delay As Single = delayPattern.PopFloat()
			Yield CupheadTime.WaitForSeconds(Me, delay)
			Me.bulletSpawnCoroutine = MyBase.StartCoroutine(Me.shoot_cr())
			While Me.bulletSpawnCoroutine IsNot Nothing
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x06001835 RID: 6197 RVA: 0x000DA968 File Offset: 0x000D8D68
	Private Iterator Function shoot_cr() As IEnumerator
		Dim previousTime As Single = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		Dim currentTime As Single = previousTime
		While previousTime >= 0.625F OrElse currentTime <= 0.625F
			Yield Nothing
			previousTime = currentTime
			currentTime = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		End While
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("IdleActive") OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("IsDormant") Then
			Me.mainRenderer.enabled = False
		End If
		Me.summonOverlayRenderer.enabled = True
		Dim num As Single = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		Dim num2 As Single = num
		currentTime = num
		previousTime = num2
		While previousTime <= currentTime
			Yield Nothing
			previousTime = currentTime
			currentTime = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		End While
		Dim bell As ChessBishopLevelBell = Me.bellProjectile.Spawn()
		Me.SFX_KOG_Bishop_Shoot()
		bell.Init(Me.projectileSpawnPoint.position, PlayerManager.GetNext(), MyBase.properties.CurrentState.bishop)
		Dim num3 As Single = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		num2 = num3
		currentTime = num3
		previousTime = num2
		While previousTime >= 0.525F OrElse currentTime <= 0.525F
			Yield Nothing
			previousTime = currentTime
			currentTime = MathUtilities.DecimalPart(MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		End While
		Me.mainRenderer.enabled = True
		Me.summonOverlayRenderer.enabled = False
		Me.bulletSpawnCoroutine = Nothing
		Return
	End Function

	' Token: 0x06001836 RID: 6198 RVA: 0x000DA983 File Offset: 0x000D8D83
	Private Sub cancelShoot()
		If Me.bulletSpawnCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.bulletSpawnCoroutine)
			Me.bulletSpawnCoroutine = Nothing
		End If
		Me.mainRenderer.enabled = True
		Me.summonOverlayRenderer.enabled = False
	End Sub

	' Token: 0x06001837 RID: 6199 RVA: 0x000DA9BC File Offset: 0x000D8DBC
	Private Sub die()
		If Me.dead Then
			Return
		End If
		Me.dead = True
		Me.bodyOpacity = 1F
		Me.UpdateBodyFade()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.SFX_KOG_Bishop_Death()
		Me.bodyAnimator.Play("Death")
		MyBase.animator.Play("Death")
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06001838 RID: 6200 RVA: 0x000DAA35 File Offset: 0x000D8E35
	Private Sub setupPatternStrings()
		Me.invisibleTime = New PatternString(MyBase.properties.CurrentState.bishop.invisibleTimeString, True)
	End Sub

	' Token: 0x06001839 RID: 6201 RVA: 0x000DAA58 File Offset: 0x000D8E58
	Private Sub AnimationEvent_SFX_KOG_Bishop_Wakeup()
		AudioManager.Play("sfx_dlc_kog_bishop_wakeup")
	End Sub

	' Token: 0x0600183A RID: 6202 RVA: 0x000DAA64 File Offset: 0x000D8E64
	Private Sub AnimationEvent_SFX_KOG_Bishop_HeadDisappearsFromBody()
		AudioManager.Play("sfx_dlc_kog_bishop_headdisappearsfrombody")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_bishop_headdisappearsfrombody")
	End Sub

	' Token: 0x0600183B RID: 6203 RVA: 0x000DAA80 File Offset: 0x000D8E80
	Private Sub AnimationEvent_SFX_KOG_Bishop_HeadReappears()
		AudioManager.Play("sfx_dlc_kog_bishop_headreappears")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_bishop_headreappears")
	End Sub

	' Token: 0x0600183C RID: 6204 RVA: 0x000DAA9C File Offset: 0x000D8E9C
	Private Sub SFX_KOG_Bishop_Shoot()
		AudioManager.Play("sfx_dlc_kog_bishop_shoot")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_bishop_shoot")
	End Sub

	' Token: 0x0600183D RID: 6205 RVA: 0x000DAAB8 File Offset: 0x000D8EB8
	Private Sub SFX_KOG_Bishop_Death()
		AudioManager.Play("sfx_dlc_kog_bishop_death")
		AudioManager.Play("sfx_level_knockout_boom")
	End Sub

	' Token: 0x0600183E RID: 6206 RVA: 0x000DAACE File Offset: 0x000D8ECE
	Private Sub AnimationEvent_SFX_KOG_Bishop_Vocal()
		MyBase.StartCoroutine(Me.SFX_KOG_Bihop_Vocal_cr())
	End Sub

	' Token: 0x0600183F RID: 6207 RVA: 0x000DAAE0 File Offset: 0x000D8EE0
	Private Iterator Function SFX_KOG_Bihop_Vocal_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		AudioManager.Play("sfx_dlc_kog_bishop_vocal")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_bishop_vocal")
		Return
	End Function

	' Token: 0x0400214F RID: 8527
	Private Const MULTIPLAYER_CANDLE_DIFFERENCE As Integer = 3

	' Token: 0x04002150 RID: 8528
	Private Const VER_LOOPSIZEY As Single = 150F

	' Token: 0x04002151 RID: 8529
	Private Const VER_LOOPSIZEX As Single = 500F

	' Token: 0x04002152 RID: 8530
	Private Const H_EASE_DISTANCE As Single = 100F

	' Token: 0x04002153 RID: 8531
	<SerializeField()>
	Private bellProjectile As ChessBishopLevelBell

	' Token: 0x04002154 RID: 8532
	<SerializeField()>
	Private mainRenderer As SpriteRenderer

	' Token: 0x04002155 RID: 8533
	<SerializeField()>
	Private summonOverlayRenderer As SpriteRenderer

	' Token: 0x04002156 RID: 8534
	<SerializeField()>
	Private projectileSpawnPoint As Transform

	' Token: 0x04002157 RID: 8535
	<SerializeField()>
	Private pivotPoint As Transform

	' Token: 0x04002158 RID: 8536
	<SerializeField()>
	Private candlesHolder As GameObject

	' Token: 0x04002159 RID: 8537
	<SerializeField()>
	Private candles As ChessBishopLevelCandle()

	' Token: 0x0400215A RID: 8538
	<SerializeField()>
	Private bodyAnimator As Animator

	' Token: 0x0400215B RID: 8539
	<SerializeField()>
	Private bodyExplosion As Effect

	' Token: 0x0400215C RID: 8540
	<SerializeField()>
	Private bodyExplosionSpawnPoint As Transform

	' Token: 0x0400215D RID: 8541
	<SerializeField()>
	Private bodyRenderer As SpriteRenderer

	' Token: 0x0400215E RID: 8542
	Private bodyOpacity As Single = 1F

	' Token: 0x0400215F RID: 8543
	<SerializeField()>
	Private fadeRate As Single = 0.75F

	' Token: 0x04002160 RID: 8544
	<SerializeField()>
	Private playerMask As GameObject()

	' Token: 0x04002161 RID: 8545
	Private candleOrderMainIndex As Integer

	' Token: 0x04002162 RID: 8546
	Private invert As Boolean

	' Token: 0x04002163 RID: 8547
	Private isPathTwo As Boolean

	' Token: 0x04002164 RID: 8548
	Private damageDealer As DamageDealer

	' Token: 0x04002165 RID: 8549
	Private invisibleTime As PatternString

	' Token: 0x04002166 RID: 8550
	Private canMove As Boolean

	' Token: 0x04002167 RID: 8551
	Private isFirstPhase As Boolean = True

	' Token: 0x04002168 RID: 8552
	Private stateDidChange As Boolean

	' Token: 0x04002169 RID: 8553
	Private disappearingState As ChessBishopLevelBishop.DisappearingState

	' Token: 0x0400216A RID: 8554
	Private dead As Boolean

	' Token: 0x0400216B RID: 8555
	Private introPlaying As Boolean = True

	' Token: 0x0400216C RID: 8556
	Private bulletSpawnCoroutine As Coroutine

	' Token: 0x02000537 RID: 1335
	Private Enum DisappearingState
		' Token: 0x0400216E RID: 8558
		None
		' Token: 0x0400216F RID: 8559
		Disappearing
		' Token: 0x04002170 RID: 8560
		Reappearing
	End Enum
End Class
