Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005ED RID: 1517
Public Class DragonLevelDragon
	Inherits LevelProperties.Dragon.Entity

	' Token: 0x17000374 RID: 884
	' (get) Token: 0x06001E1A RID: 7706 RVA: 0x00114F46 File Offset: 0x00113346
	' (set) Token: 0x06001E1B RID: 7707 RVA: 0x00114F4E File Offset: 0x0011334E
	Public Property state As DragonLevelDragon.State

	' Token: 0x06001E1C RID: 7708 RVA: 0x00114F57 File Offset: 0x00113357
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06001E1D RID: 7709 RVA: 0x00114F8D File Offset: 0x0011338D
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.dead Then
			Return
		End If
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001E1E RID: 7710 RVA: 0x00114FAC File Offset: 0x001133AC
	Private Sub Start()
		AddHandler Level.Current.OnIntroEvent, AddressOf Me.OnIntro
	End Sub

	' Token: 0x06001E1F RID: 7711 RVA: 0x00114FC4 File Offset: 0x001133C4
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001E20 RID: 7712 RVA: 0x00114FDC File Offset: 0x001133DC
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001E21 RID: 7713 RVA: 0x00115005 File Offset: 0x00113405
	Private Sub OnBossDeath()
		If Me.dead Then
			Return
		End If
		Me.dead = True
		AudioManager.[Stop]("level_dragon_sucking_air")
		Me.StopAllCoroutines()
		MyBase.animator.Play("Death")
	End Sub

	' Token: 0x06001E22 RID: 7714 RVA: 0x0011503A File Offset: 0x0011343A
	Private Sub StartWingSFX()
		AudioManager.PlayLoop("level_dragon_left_dragon_peashot_idle_loop")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_idle_loop")
	End Sub

	' Token: 0x06001E23 RID: 7715 RVA: 0x00115056 File Offset: 0x00113456
	Private Sub StopWingSFX()
		AudioManager.[Stop]("level_dragon_left_dragon_peashot_idle_loop")
	End Sub

	' Token: 0x06001E24 RID: 7716 RVA: 0x00115062 File Offset: 0x00113462
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.meteorPrefab = Nothing
		Me.smokePrefab = Nothing
		Me.peashotPrefab = Nothing
	End Sub

	' Token: 0x06001E25 RID: 7717 RVA: 0x0011507F File Offset: 0x0011347F
	Private Sub OnIntro()
		MyBase.StartCoroutine(Me.intro_cr())
	End Sub

	' Token: 0x06001E26 RID: 7718 RVA: 0x00115090 File Offset: 0x00113490
	Private Iterator Function intro_cr() As IEnumerator
		Me.state = DragonLevelDragon.State.Init
		MyBase.animator.Play("Intro")
		AudioManager.Play("level_dragon_left_dragon_intro")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_intro")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		Yield CupheadTime.WaitForSeconds(Me, 0.6F)
		Me.state = DragonLevelDragon.State.Idle
		Return
	End Function

	' Token: 0x06001E27 RID: 7719 RVA: 0x001150AB File Offset: 0x001134AB
	Public Sub StartPeashot()
		Me.state = DragonLevelDragon.State.Peashot
		MyBase.StartCoroutine(Me.peashot_cr())
	End Sub

	' Token: 0x06001E28 RID: 7720 RVA: 0x001150C1 File Offset: 0x001134C1
	Private Sub PeashotInSFX()
		AudioManager.Play("level_dragon_left_dragon_peashot_in")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_in")
	End Sub

	' Token: 0x06001E29 RID: 7721 RVA: 0x001150E0 File Offset: 0x001134E0
	Private Iterator Function peashot_cr() As IEnumerator
		Dim p As LevelProperties.Dragon.Peashot = MyBase.properties.CurrentState.peashot
		Dim pattern As String() = p.patternString.GetRandom().Split(New Char() { ","c })
		MyBase.animator.SetBool("Peashot", True)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Peashot_In", False, True)
		MyBase.animator.Play("Peashot_Zinger")
		For i As Integer = 0 To pattern.Length - 1
			If pattern(i).ToLower() = "p" Then
				Me.peashotRoot.LookAt2D(PlayerManager.GetNext().center)
				For c As Integer = 0 To p.colorString.Length - 1
					Dim color As Integer = 0
					Dim c2 As Char = p.colorString(c)
					If c2 <> "O"c Then
						If c2 <> "P"c Then
							If c2 = "B"c Then
								color = 1
							End If
						Else
							color = 2
						End If
					Else
						color = 0
					End If
					AudioManager.Play("level_dragon_left_dragon_peashot_fire")
					Me.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_fire")
					TryCast(Me.peashotPrefab.Create(Me.peashotRoot.position, Me.peashotRoot.eulerAngles.z, p.speed), DragonLevelPeashot).color = color
					Yield CupheadTime.WaitForSeconds(Me, p.shotDelay)
				Next
			Else
				Dim delay As Single = 0F
				Parser.FloatTryParse(pattern(i), delay)
				Yield CupheadTime.WaitForSeconds(Me, delay)
			End If
		Next
		MyBase.animator.SetBool("Peashot", False)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Peashot_Out", False)
		AudioManager.Play("level_dragon_left_dragon_peashot_out")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_peashot_out")
		Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		Me.state = DragonLevelDragon.State.Idle
		Return
	End Function

	' Token: 0x06001E2A RID: 7722 RVA: 0x001150FC File Offset: 0x001134FC
	Private Sub SpawnSmokeFX()
		Dim effect As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.smokePrefab)
		effect.transform.position = Me.smokeRoot.transform.position
		effect.GetComponent(Of Animator)().Play(If((Not Rand.Bool()), "Smoke_FX_B", "Smoke_FX_A"))
	End Sub

	' Token: 0x06001E2B RID: 7723 RVA: 0x00115154 File Offset: 0x00113554
	Public Sub StartMeteor()
		Me.state = DragonLevelDragon.State.Meteor
		MyBase.StartCoroutine(Me.meteor_cr())
	End Sub

	' Token: 0x06001E2C RID: 7724 RVA: 0x0011516C File Offset: 0x0011356C
	Private Sub FireMeteor()
		AudioManager.Play("level_dragon_left_dragon_meteor_spit")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_spit")
		If Me.meteorState = DragonLevelMeteor.State.Both Then
			Me.meteorPrefab.Create(Me.mouthRoot.position, New DragonLevelMeteor.Properties(Me.currentMeteorProperties.timeY, Me.currentMeteorProperties.speedX, DragonLevelMeteor.State.Up))
			Me.meteorPrefab.Create(Me.mouthRoot.position, New DragonLevelMeteor.Properties(Me.currentMeteorProperties.timeY, Me.currentMeteorProperties.speedX, DragonLevelMeteor.State.Down))
		Else
			Me.meteorPrefab.Create(Me.mouthRoot.position, New DragonLevelMeteor.Properties(Me.currentMeteorProperties.timeY, Me.currentMeteorProperties.speedX, Me.meteorState))
		End If
	End Sub

	' Token: 0x06001E2D RID: 7725 RVA: 0x00115250 File Offset: 0x00113650
	Private Iterator Function meteor_cr() As IEnumerator
		Me.currentMeteorProperties = MyBase.properties.CurrentState.meteor
		Dim meteorPattern As Char() = Me.currentMeteorProperties.pattern.GetRandom().ToCharArray()
		MyBase.animator.SetTrigger("OnMeteor")
		MyBase.animator.SetBool("Repeat", True)
		Yield MyBase.animator.WaitForAnimationToStart(Me, "MeteorStart", False)
		AudioManager.Play("level_dragon_left_dragon_meteor_start")
		Me.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_start")
		For i As Integer = 0 To meteorPattern.Length - 1
			Dim c As Char = meteorPattern(i)
			Select Case c
				Case "B"c
					Me.meteorState = DragonLevelMeteor.State.Both
				Case Else
					If c <> "U"c Then
					End If
					Me.meteorState = DragonLevelMeteor.State.Up
				Case "D"c
					Me.meteorState = DragonLevelMeteor.State.Down
				Case "F"c
					Me.meteorState = DragonLevelMeteor.State.Forward
			End Select
			If i >= meteorPattern.Length - 1 Then
				MyBase.animator.SetBool("Repeat", False)
			End If
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Meteor_Anticipation_Loop", False)
			AudioManager.Play("level_dragon_left_dragon_meteor_anticipation_loop")
			Me.emitAudioFromObject.Add("level_dragon_left_dragon_meteor_anticipation_loop")
			Yield CupheadTime.WaitForSeconds(Me, Me.currentMeteorProperties.shotDelay)
			MyBase.animator.SetTrigger("OnMeteor")
			AudioManager.[Stop]("level_dragon_left_dragon_meteor_anticipation_loop")
			Yield MyBase.animator.WaitForAnimationToStart(Me, "Meteor_Attack", False)
			AudioManager.Play("level_dragon_left_dragon_meteor_attack")
			Yield MyBase.animator.WaitForAnimationToEnd(Me, "Meteor_Attack", False, True)
		Next
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Meteor_Attack_End", False, True)
		Yield CupheadTime.WaitForSeconds(Me, Me.currentMeteorProperties.hesitate)
		Me.state = DragonLevelDragon.State.Idle
		Return
	End Function

	' Token: 0x06001E2E RID: 7726 RVA: 0x0011526B File Offset: 0x0011366B
	Public Sub Leave()
		MyBase.StartCoroutine(Me.leave_cr())
	End Sub

	' Token: 0x06001E2F RID: 7727 RVA: 0x0011527C File Offset: 0x0011367C
	Private Iterator Function leave_cr() As IEnumerator
		While Me.state <> DragonLevelDragon.State.Idle
			Yield Nothing
		End While
		Dim endPos As Vector2 = MyBase.transform.position
		endPos.x += 500F
		Yield MyBase.StartCoroutine(Me.tween_cr(MyBase.transform, MyBase.transform.position, endPos, EaseUtils.EaseType.easeInSine, 1.5F))
		Me.damages.SetActive(False)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Dim dashEndPos As Vector2 = Me.dash.position
		dashEndPos.x = -1300F
		Me.StopWingSFX()
		AudioManager.Play("level_dragon_dash")
		Yield MyBase.StartCoroutine(Me.tween_cr(Me.dash, Me.dash.position, dashEndPos, EaseUtils.EaseType.linear, 1.3F))
		Yield CupheadTime.WaitForSeconds(Me, 0.75F)
		Me.leftSideDragon.StartIntro()
		Global.UnityEngine.[Object].Destroy(Me.dash.gameObject)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06001E30 RID: 7728 RVA: 0x00115298 File Offset: 0x00113698
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

	' Token: 0x040026E3 RID: 9955
	<Space(10F)>
	<SerializeField()>
	Private mouthRoot As Transform

	' Token: 0x040026E4 RID: 9956
	<SerializeField()>
	Private meteorPrefab As DragonLevelMeteor

	' Token: 0x040026E5 RID: 9957
	<SerializeField()>
	Private smokePrefab As Effect

	' Token: 0x040026E6 RID: 9958
	<SerializeField()>
	Private smokeRoot As Transform

	' Token: 0x040026E7 RID: 9959
	<Space(10F)>
	<SerializeField()>
	Private peashotPrefab As DragonLevelPeashot

	' Token: 0x040026E8 RID: 9960
	<SerializeField()>
	Private peashotRoot As Transform

	' Token: 0x040026E9 RID: 9961
	<Space(10F)>
	<SerializeField()>
	Private chargeRoot As Transform

	' Token: 0x040026EA RID: 9962
	<SerializeField()>
	Private dash As Transform

	' Token: 0x040026EB RID: 9963
	<SerializeField()>
	Private leftSideDragon As DragonLevelLeftSideDragon

	' Token: 0x040026EC RID: 9964
	<SerializeField()>
	Private damages As GameObject

	' Token: 0x040026ED RID: 9965
	Private currentMeteorProperties As LevelProperties.Dragon.Meteor

	' Token: 0x040026EE RID: 9966
	Private damageDealer As DamageDealer

	' Token: 0x040026EF RID: 9967
	Private damageReceiver As DamageReceiver

	' Token: 0x040026F0 RID: 9968
	Private dead As Boolean

	' Token: 0x040026F1 RID: 9969
	Private meteorState As DragonLevelMeteor.State

	' Token: 0x020005EE RID: 1518
	Public Enum State
		' Token: 0x040026F3 RID: 9971
		Init
		' Token: 0x040026F4 RID: 9972
		Idle
		' Token: 0x040026F5 RID: 9973
		Meteor
		' Token: 0x040026F6 RID: 9974
		Peashot
	End Enum
End Class
