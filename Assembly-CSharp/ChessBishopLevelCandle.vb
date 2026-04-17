Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000539 RID: 1337
Public Class ChessBishopLevelCandle
	Inherits AbstractCollidableObject

	' Token: 0x17000332 RID: 818
	' (get) Token: 0x06001845 RID: 6213 RVA: 0x000DBD12 File Offset: 0x000DA112
	' (set) Token: 0x06001846 RID: 6214 RVA: 0x000DBD1A File Offset: 0x000DA11A
	Public Property isLit As Boolean

	' Token: 0x06001847 RID: 6215 RVA: 0x000DBD24 File Offset: 0x000DA124
	Public Sub Init(distToBlowout As Single)
		Me.glow.SetActive(False)
		Me.distToBlowout = distToBlowout
		Me.basePos = MyBase.transform.position
		Me.shadowPos = Me.shadow.transform.position
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001848 RID: 6216 RVA: 0x000DBD78 File Offset: 0x000DA178
	Private Function EaseOvershoot(start As Single, [end] As Single, value As Single, overshoot As Single) As Single
		Dim num As Single = Mathf.Lerp(start, [end], value)
		Return num + Mathf.Sin(value * 3.1415927F) * (([end] - start) * overshoot)
	End Function

	' Token: 0x06001849 RID: 6217 RVA: 0x000DBDA8 File Offset: 0x000DA1A8
	Private Iterator Function intro_cr() As IEnumerator
		Me.introPos = Me.introCandle.transform.position
		Yield Nothing
		While Not Me.introCandle.moving
			Yield Nothing
		End While
		Dim t As Single = 0F
		While t < 1F
			Me.introCandle.transform.position = Me.introPos + Vector3.up * 800F * EaseUtils.EaseOutSine(0F, 1F, Mathf.InverseLerp(0F, 1F, t))
			t += 0.041666668F
			Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		End While
		MyBase.transform.position = Me.basePos + Vector3.up * 800F
		MyBase.animator.Play("IntroToIdle")
		t = 0F
		While t < 1F
			MyBase.transform.position = Me.basePos + (Vector3.up * 800F * Me.EaseOvershoot(1F, 0F, t, Me.introOvershoot) + Me.floatAmplitude * Vector3.up)
			t += 0.041666668F
			Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		End While
		Me.introPlaying = False
		Return
	End Function

	' Token: 0x0600184A RID: 6218 RVA: 0x000DBDC4 File Offset: 0x000DA1C4
	Private Function PlayerInRange() As Boolean
		If Me.player1 AndAlso Not Me.player1.IsDead Then
			Dim num As Single = Vector3.SqrMagnitude(Me.blowoutRoot.position - Me.player1.center)
			If num < Me.distToBlowout * Me.distToBlowout AndAlso Me.player1.center <> Me.lastPlayer1Position Then
				Return True
			End If
		End If
		If Me.player2 AndAlso Not Me.player2.IsDead Then
			Dim num2 As Single = Vector3.SqrMagnitude(Me.blowoutRoot.position - Me.player2.center)
			If num2 < Me.distToBlowout * Me.distToBlowout AndAlso Me.player2.center <> Me.lastPlayer2Position Then
				Return True
			End If
		End If
		Return False
	End Function

	' Token: 0x0600184B RID: 6219 RVA: 0x000DBEB4 File Offset: 0x000DA2B4
	Private Sub Update()
		Me.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne)
		Me.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
		If Me.PlayerInRange() Then
			If Me.isLit Then
				Me.StopAllCoroutines()
				MyBase.StartCoroutine(Me.light_out_cr())
			ElseIf MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") Then
				MyBase.animator.Play("Walkby", 0, 0F)
			End If
		End If
		If Me.player1 AndAlso Not Me.player1.IsDead Then
			Me.lastPlayer1Position = Me.player1.center
		End If
		If Me.player2 AndAlso Not Me.player2.IsDead Then
			Me.lastPlayer2Position = Me.player2.center
		End If
		Me.stepTimer += CupheadTime.Delta
		While Me.stepTimer > 0.041666668F
			Me.[Step]()
			Me.stepTimer -= 0.041666668F
		End While
	End Sub

	' Token: 0x0600184C RID: 6220 RVA: 0x000DBFE4 File Offset: 0x000DA3E4
	Private Sub [Step]()
		Me.shadow.transform.position = Me.shadowPos
		If Me.introPlaying Then
			Return
		End If
		MyBase.transform.position = Me.basePos
		If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") OrElse MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Lit") Then
			Me.easeToFloat = Mathf.Clamp(0F, 1F, Me.easeToFloat + 0.041666668F)
			Dim num As Single = If((Not Me.offsetFloat), MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime, (MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.25F))
			MyBase.transform.position += Vector3.up * Mathf.Cos(num * 3.1415927F * 2F) * Me.floatAmplitude * Me.easeToFloat
		Else
			Me.easeToFloat = 0F
		End If
	End Sub

	' Token: 0x0600184D RID: 6221 RVA: 0x000DC119 File Offset: 0x000DA519
	Public Sub LightUp()
		Me.isLit = True
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.light_up_cr())
	End Sub

	' Token: 0x0600184E RID: 6222 RVA: 0x000DC138 File Offset: 0x000DA538
	Private Iterator Function light_up_cr() As IEnumerator
		MyBase.animator.Play(If((Not Rand.Bool()), "ReigniteB", "ReigniteA"), 1)
		Me.glow.SetActive(True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "None", 1, False)
		MyBase.animator.Play("Lit", 0, MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
		Me.SFX_KOG_Bishop_CandlesLightUp()
		Return
	End Function

	' Token: 0x0600184F RID: 6223 RVA: 0x000DC154 File Offset: 0x000DA554
	Private Iterator Function light_out_cr() As IEnumerator
		Me.isLit = False
		Me.glow.SetActive(False)
		MyBase.animator.Play("Stagger")
		Me.SFX_KOG_Bishop_CandleSnuff()
		Me.smoke.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(-5, 5)))
		Me.vanquishFX.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		Me.vanquishSpark.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		MyBase.animator.Play(If((Not Rand.Bool()), "SmokeB", "SmokeA"), 2)
		Yield CupheadTime.WaitForSeconds(Me, Me.staggerLoopTime)
		MyBase.animator.SetTrigger("EndStaggerLoop")
		Return
	End Function

	' Token: 0x06001850 RID: 6224 RVA: 0x000DC16F File Offset: 0x000DA56F
	Private Sub SFX_KOG_Bishop_CandlesLightUp()
		AudioManager.Play("sfx_dlc_kog_bishop_candleslightup")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_bishop_candleslightup")
	End Sub

	' Token: 0x06001851 RID: 6225 RVA: 0x000DC18B File Offset: 0x000DA58B
	Private Sub SFX_KOG_Bishop_CandleSnuff()
		AudioManager.Play("sfx_dlc_kog_bishop_candlesnuff")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_bishop_candlesnuff")
	End Sub

	' Token: 0x04002171 RID: 8561
	<SerializeField()>
	Private blowoutRoot As Transform

	' Token: 0x04002172 RID: 8562
	<SerializeField()>
	Private smoke As GameObject

	' Token: 0x04002173 RID: 8563
	<SerializeField()>
	Private vanquishFX As GameObject

	' Token: 0x04002174 RID: 8564
	<SerializeField()>
	Private vanquishSpark As GameObject

	' Token: 0x04002175 RID: 8565
	<SerializeField()>
	Private shadow As GameObject

	' Token: 0x04002176 RID: 8566
	<SerializeField()>
	Private staggerLoopTime As Single

	' Token: 0x04002177 RID: 8567
	<SerializeField()>
	Private floatAmplitude As Single

	' Token: 0x04002178 RID: 8568
	<SerializeField()>
	Private offsetFloat As Boolean

	' Token: 0x04002179 RID: 8569
	Private easeToFloat As Single = 1F

	' Token: 0x0400217A RID: 8570
	Private basePos As Vector3

	' Token: 0x0400217B RID: 8571
	Private shadowPos As Vector3

	' Token: 0x0400217C RID: 8572
	Private introPos As Vector3

	' Token: 0x0400217E RID: 8574
	Private distToBlowout As Single

	' Token: 0x0400217F RID: 8575
	Private player1 As AbstractPlayerController

	' Token: 0x04002180 RID: 8576
	Private player2 As AbstractPlayerController

	' Token: 0x04002181 RID: 8577
	Private lastPlayer1Position As Vector3 = Vector3.zero

	' Token: 0x04002182 RID: 8578
	Private lastPlayer2Position As Vector3 = Vector3.zero

	' Token: 0x04002183 RID: 8579
	Private stepTimer As Single

	' Token: 0x04002184 RID: 8580
	<SerializeField()>
	Private glow As GameObject

	' Token: 0x04002185 RID: 8581
	<SerializeField()>
	Private introCandle As ChessBishopLevelIntroCandle

	' Token: 0x04002186 RID: 8582
	Private introPlaying As Boolean = True

	' Token: 0x04002187 RID: 8583
	<SerializeField()>
	Private isLastIntro As Boolean

	' Token: 0x04002188 RID: 8584
	<SerializeField()>
	Private introOvershoot As Single
End Class
