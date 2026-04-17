Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020009D5 RID: 2517
Public Class ArcadePlayerAnimationController
	Inherits AbstractArcadePlayerComponent

	' Token: 0x06003B45 RID: 15173 RVA: 0x002140AD File Offset: 0x002124AD
	Protected Overrides Sub OnAwake()
		MyBase.OnAwake()
		Me.SetSprites(MyBase.player.id = PlayerId.PlayerOne)
	End Sub

	' Token: 0x06003B46 RID: 15174 RVA: 0x002140CC File Offset: 0x002124CC
	Private Sub Start()
		AddHandler MyBase.basePlayer.OnPlayIntroEvent, AddressOf Me.PlayIntro
		AddHandler MyBase.player.motor.OnParryEvent, AddressOf Me.OnParryStart
		AddHandler MyBase.player.motor.OnGroundedEvent, AddressOf Me.OnGrounded
		AddHandler MyBase.player.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler MyBase.player.weaponManager.OnExStart, AddressOf Me.OnEx
		AddHandler MyBase.player.weaponManager.OnSuperStart, AddressOf Me.OnSuper
		AddHandler MyBase.player.weaponManager.OnSuperEnd, AddressOf Me.OnSuperEnd
		AddHandler MyBase.player.weaponManager.OnWeaponFire, AddressOf Me.OnShotFired
		AddHandler LevelPauseGUI.OnPauseEvent, AddressOf Me.OnGuiPause
		AddHandler LevelPauseGUI.OnPauseEvent, AddressOf Me.OnGuiUnpause
	End Sub

	' Token: 0x06003B47 RID: 15175 RVA: 0x002141D6 File Offset: 0x002125D6
	Private Sub OnEnable()
		MyBase.StartCoroutine(Me.flash_cr())
	End Sub

	' Token: 0x06003B48 RID: 15176 RVA: 0x002141E8 File Offset: 0x002125E8
	Private Sub Update()
		If MyBase.player.IsDead OrElse Not MyBase.player.levelStarted Then
			Return
		End If
		If Not Me.hitAnimation AndAlso MyBase.player.motor.LookDirection.x <> 0 AndAlso MyBase.player.motor.LookDirection.x <> Me.GetInt(ArcadePlayerAnimationController.Integers.LookX) Then
			Me.SetBool(ArcadePlayerAnimationController.Booleans.Turning, True)
		Else
			Me.SetBool(ArcadePlayerAnimationController.Booleans.Turning, False)
		End If
		Me.SetBool(ArcadePlayerAnimationController.Booleans.Grounded, MyBase.player.motor.Grounded)
		Me.SetBool(ArcadePlayerAnimationController.Booleans.NearLanding, MyBase.player.motor.GetTimeUntilLand() <= 0.15F AndAlso Not MyBase.player.motor.Parrying)
		Me.SetInt(ArcadePlayerAnimationController.Integers.MoveX, MyBase.player.motor.LookDirection.x)
		Me.SetInt(ArcadePlayerAnimationController.Integers.MoveY, MyBase.player.motor.MoveDirection.y)
		Me.SetInt(ArcadePlayerAnimationController.Integers.LookX, MyBase.player.motor.TrueLookDirection.x)
		Me.SetInt(ArcadePlayerAnimationController.Integers.LookY, MyBase.player.motor.TrueLookDirection.y)
		Me.SetBool(ArcadePlayerAnimationController.Booleans.Shooting, MyBase.player.weaponManager.IsShooting)
		Dim currentAnimatorStateInfo As AnimatorStateInfo = MyBase.animator.GetCurrentAnimatorStateInfo(0)
		Dim flag As Boolean = currentAnimatorStateInfo.IsName("Idle") OrElse currentAnimatorStateInfo.IsName("Run")
		If Me.shooting Then
			Me.timeSinceStoppedShooting = 0F
		Else
			Me.timeSinceStoppedShooting += CupheadTime.Delta
		End If
		Dim flag2 As Boolean = False
		If Me.fired AndAlso flag Then
			Me.SetTrigger(ArcadePlayerAnimationController.Triggers.OnFire)
			Me.SetInt(ArcadePlayerAnimationController.Integers.ArmVariant, If((Not Rand.Bool()), 1, 0))
			flag2 = True
		End If
		Me.fired = False
		Me.shooting = MyBase.player.weaponManager.IsShooting
		If Not Me.shooting AndAlso Not flag2 Then
			Me.ResetTrigger(ArcadePlayerAnimationController.Triggers.OnFire)
		End If
		Me.SetBool(ArcadePlayerAnimationController.Booleans.Dashing, MyBase.player.motor.Dashing)
		Me.SetBool(ArcadePlayerAnimationController.Booleans.NearDashEnd, MyBase.player.motor.GetTimeUntilDashEnd() < If((Not MyBase.player.motor.Grounded), 0.108333334F, 0.15F))
		If Not MyBase.player.motor.Dashing Then
			If MyBase.player.motor.LookDirection.x <> 0 Then
				MyBase.transform.SetScale(New Single?(MyBase.player.motor.LookDirection.x), Nothing, Nothing)
			End If
		Else
			MyBase.transform.SetScale(New Single?(CSng(MyBase.player.motor.DashDirection)), Nothing, Nothing)
		End If
		MyBase.animator.Update(Time.deltaTime)
		For i As Integer = 0 To 3 - 1
			MyBase.animator.Update(0F)
		Next
	End Sub

	' Token: 0x06003B49 RID: 15177 RVA: 0x00214588 File Offset: 0x00212988
	Public Sub ChangeToRocket()
		Me.prong.SetActive(False)
		Dim text As String = "Rocket"
		Me.Play(text)
	End Sub

	' Token: 0x06003B4A RID: 15178 RVA: 0x002145B0 File Offset: 0x002129B0
	Public Sub ChangeToJetpack()
		Me.prong.SetActive(False)
		Dim text As String = "Jetpack"
		Me.Play(text)
	End Sub

	' Token: 0x06003B4B RID: 15179 RVA: 0x002145D6 File Offset: 0x002129D6
	Public Overrides Sub OnPause()
		MyBase.OnPause()
		Me.SetAlpha(1F)
	End Sub

	' Token: 0x06003B4C RID: 15180 RVA: 0x002145E9 File Offset: 0x002129E9
	Private Sub OnGuiPause()
	End Sub

	' Token: 0x06003B4D RID: 15181 RVA: 0x002145EB File Offset: 0x002129EB
	Private Sub OnGuiUnpause()
	End Sub

	' Token: 0x06003B4E RID: 15182 RVA: 0x002145ED File Offset: 0x002129ED
	Public Sub OnShotFired()
		Me.fired = True
	End Sub

	' Token: 0x06003B4F RID: 15183 RVA: 0x002145F6 File Offset: 0x002129F6
	Public Sub OnLevelWin()
		MyBase.player.damageReceiver.OnWin()
		Me.SetTrigger(ArcadePlayerAnimationController.Triggers.OnWin)
	End Sub

	' Token: 0x06003B50 RID: 15184 RVA: 0x00214610 File Offset: 0x00212A10
	Public Sub PlayIntro()
		Me.SetBool(ArcadePlayerAnimationController.Booleans.Intro, True)
		Dim text As String = If((MyBase.player.id <> PlayerId.PlayerOne), "Mugman", "Cuphead")
		Me.Play("Intro_" + text)
		If text = "Cuphead" Then
			AudioManager.Play("player_intro_cuphead")
		Else
			AudioManager.Play("player_intro_mugman")
		End If
	End Sub

	' Token: 0x06003B51 RID: 15185 RVA: 0x0021467F File Offset: 0x00212A7F
	Public Sub LevelInit()
		Me.SetSprites(MyBase.player.id = PlayerId.PlayerOne)
	End Sub

	' Token: 0x06003B52 RID: 15186 RVA: 0x00214698 File Offset: 0x00212A98
	Public Sub SetSprites(isCuphead As Boolean)
		Me.cuphead.SetActive(isCuphead)
		Me.mugman.SetActive(Not isCuphead)
		If isCuphead Then
			Me.spriteRenderer = Me.cuphead.GetComponent(Of SpriteRenderer)()
			Me.armRenderer = Me.cupheadArm.GetComponent(Of SpriteRenderer)()
		Else
			Me.spriteRenderer = Me.mugman.GetComponent(Of SpriteRenderer)()
			Me.armRenderer = Me.mugmanArm.GetComponent(Of SpriteRenderer)()
		End If
	End Sub

	' Token: 0x06003B53 RID: 15187 RVA: 0x00214710 File Offset: 0x00212B10
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		CupheadLevelCamera.Current.Shake(20F, 0.6F, False)
		AudioManager.Play("player_hit")
		If MyBase.player.controlScheme = ArcadePlayerController.ControlScheme.Normal Then
			Me.Play("Hit")
			Me.hitAnimation = True
		End If
	End Sub

	' Token: 0x06003B54 RID: 15188 RVA: 0x0021475E File Offset: 0x00212B5E
	Private Sub OnRunDust()
		Me.runEffect.Create(Me.runDustRoot.position)
	End Sub

	' Token: 0x06003B55 RID: 15189 RVA: 0x00214777 File Offset: 0x00212B77
	Private Sub onHitAnimationComplete()
		Me.hitAnimation = False
	End Sub

	' Token: 0x06003B56 RID: 15190 RVA: 0x00214780 File Offset: 0x00212B80
	Private Sub SetSpriteProperties(layer As SpriteLayer, order As Integer)
		Me.spriteRenderer.sortingLayerName = layer.ToString()
		Me.spriteRenderer.sortingOrder = order
	End Sub

	' Token: 0x06003B57 RID: 15191 RVA: 0x002147A8 File Offset: 0x00212BA8
	Private Sub ResetSpriteProperties()
		Me.spriteRenderer.sortingLayerName = SpriteLayer.Player.ToString()
		Me.spriteRenderer.sortingOrder = If((MyBase.player.id <> PlayerId.PlayerOne), (-1), 1)
	End Sub

	' Token: 0x06003B58 RID: 15192 RVA: 0x002147F1 File Offset: 0x00212BF1
	Private Sub OnParryStart()
		If Me.super Then
			Return
		End If
		Me.SetTrigger(ArcadePlayerAnimationController.Triggers.OnParry)
	End Sub

	' Token: 0x06003B59 RID: 15193 RVA: 0x00214806 File Offset: 0x00212C06
	Public Sub OnParrySuccess()
		Me.SetAlpha(1F)
	End Sub

	' Token: 0x06003B5A RID: 15194 RVA: 0x00214813 File Offset: 0x00212C13
	Public Sub OnParryPause()
	End Sub

	' Token: 0x06003B5B RID: 15195 RVA: 0x00214815 File Offset: 0x00212C15
	Public Sub OnParryAnimEnd()
	End Sub

	' Token: 0x06003B5C RID: 15196 RVA: 0x00214817 File Offset: 0x00212C17
	Public Sub ResumeNormanAnim()
	End Sub

	' Token: 0x06003B5D RID: 15197 RVA: 0x00214819 File Offset: 0x00212C19
	Private Sub OnGrounded()
		If Not Level.Current.Started Then
			Return
		End If
		AudioManager.Play("player_grounded")
	End Sub

	' Token: 0x06003B5E RID: 15198 RVA: 0x00214838 File Offset: 0x00212C38
	Private Sub OnEx()
		Dim text As String = "Forward"
		If MyBase.player.motor.LookDirection.x = 0 AndAlso MyBase.player.motor.LookDirection.y > 0 Then
			text = "Up"
		ElseIf MyBase.player.motor.LookDirection.x <> 0 AndAlso MyBase.player.motor.LookDirection.y > 0 Then
			text = "Diagonal_Up"
		ElseIf MyBase.player.motor.LookDirection.x = 0 AndAlso MyBase.player.motor.LookDirection.y < 0 Then
			text = "Down"
		ElseIf MyBase.player.motor.LookDirection.x <> 0 AndAlso MyBase.player.motor.LookDirection.y < 0 Then
			text = "Diagonal_Down"
		End If
		If text = "Forward" Then
			AudioManager.Play("player_ex_forward_ground")
		End If
		Dim text2 As String = "Ex." + text + "_"
		If MyBase.player.motor.Grounded Then
			text2 += "Ground"
		Else
			text2 += "Air"
		End If
		Me.Play(text2)
	End Sub

	' Token: 0x06003B5F RID: 15199 RVA: 0x002149F8 File Offset: 0x00212DF8
	Private Sub OnSuper()
		Dim super As Super = PlayerData.Data.Loadouts.GetPlayerLoadout(MyBase.player.id).super
		Me.super = True
		Me.spriteRenderer.enabled = False
	End Sub

	' Token: 0x06003B60 RID: 15200 RVA: 0x00214A38 File Offset: 0x00212E38
	Private Sub OnSuperEnd()
		Me.super = False
		Me.spriteRenderer.enabled = True
		Me.ResetSpriteProperties()
	End Sub

	' Token: 0x06003B61 RID: 15201 RVA: 0x00214A53 File Offset: 0x00212E53
	Private Sub _OnSuperAnimEnd()
		MyBase.player.UnpauseAll(False)
		MyBase.player.motor.OnSuperEnd()
	End Sub

	' Token: 0x06003B62 RID: 15202 RVA: 0x00214A71 File Offset: 0x00212E71
	Protected Sub Play(animation As String)
		MyBase.animator.Play(animation, 0, 0F)
	End Sub

	' Token: 0x06003B63 RID: 15203 RVA: 0x00214A85 File Offset: 0x00212E85
	Protected Function GetBool(b As ArcadePlayerAnimationController.Booleans) As Boolean
		Return MyBase.animator.GetBool(b.ToString())
	End Function

	' Token: 0x06003B64 RID: 15204 RVA: 0x00214A9F File Offset: 0x00212E9F
	Protected Sub SetBool(b As ArcadePlayerAnimationController.Booleans, value As Boolean)
		MyBase.animator.SetBool(b.ToString(), value)
	End Sub

	' Token: 0x06003B65 RID: 15205 RVA: 0x00214ABA File Offset: 0x00212EBA
	Protected Function GetInt(i As ArcadePlayerAnimationController.Integers) As Integer
		Return MyBase.animator.GetInteger(i.ToString())
	End Function

	' Token: 0x06003B66 RID: 15206 RVA: 0x00214AD4 File Offset: 0x00212ED4
	Protected Sub SetInt(i As ArcadePlayerAnimationController.Integers, value As Integer)
		MyBase.animator.SetInteger(i.ToString(), value)
	End Sub

	' Token: 0x06003B67 RID: 15207 RVA: 0x00214AEF File Offset: 0x00212EEF
	Protected Sub SetTrigger(t As ArcadePlayerAnimationController.Triggers)
		MyBase.animator.SetTrigger(t.ToString())
	End Sub

	' Token: 0x06003B68 RID: 15208 RVA: 0x00214B09 File Offset: 0x00212F09
	Protected Sub ResetTrigger(t As ArcadePlayerAnimationController.Triggers)
		MyBase.animator.ResetTrigger(t.ToString())
	End Sub

	' Token: 0x06003B69 RID: 15209 RVA: 0x00214B24 File Offset: 0x00212F24
	Private Sub SetAlpha(a As Single)
		Dim color As Color = Me.spriteRenderer.color
		color.a = a
		Me.spriteRenderer.color = color
		Me.armRenderer.color = color
	End Sub

	' Token: 0x06003B6A RID: 15210 RVA: 0x00214B60 File Offset: 0x00212F60
	Public Sub SetColor(color As Color)
		Dim a As Single = Me.spriteRenderer.color.a
		color.a = a
		Me.spriteRenderer.color = color
	End Sub

	' Token: 0x06003B6B RID: 15211 RVA: 0x00214B98 File Offset: 0x00212F98
	Public Sub ResetColor()
		Dim a As Single = Me.spriteRenderer.color.a
		Me.spriteRenderer.color = New Color(1F, 1F, 1F, a)
	End Sub

	' Token: 0x06003B6C RID: 15212 RVA: 0x00214BD9 File Offset: 0x00212FD9
	Public Sub SetColorOverTime(color As Color, time As Single)
		Me.StopColorCoroutine()
		Me.colorCoroutine = Me.setColor_cr(color, time)
		MyBase.StartCoroutine(Me.colorCoroutine)
	End Sub

	' Token: 0x06003B6D RID: 15213 RVA: 0x00214BFC File Offset: 0x00212FFC
	Public Sub StopColorCoroutine()
		If Me.colorCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.colorCoroutine)
		End If
		Me.colorCoroutine = Nothing
	End Sub

	' Token: 0x06003B6E RID: 15214 RVA: 0x00214C1C File Offset: 0x0021301C
	Private Iterator Function setColor_cr(color As Color, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim startColor As Color = Me.spriteRenderer.color
		While t < time
			Dim val As Single = t / time
			Me.SetColor(Color.Lerp(startColor, color, val))
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.SetColor(color)
		Yield Nothing
		Return
	End Function

	' Token: 0x170004EA RID: 1258
	' (get) Token: 0x06003B6F RID: 15215 RVA: 0x00214C45 File Offset: 0x00213045
	Private ReadOnly Property Flashing As Boolean
		Get
			Return MyBase.player.damageReceiver.state = PlayerDamageReceiver.State.Invulnerable
		End Get
	End Property

	' Token: 0x06003B70 RID: 15216 RVA: 0x00214C5C File Offset: 0x0021305C
	Private Iterator Function flash_cr() As IEnumerator
		Dim t As Single = 0F
		While True
			While Not Me.Flashing
				Yield True
			End While
			Yield CupheadTime.WaitForSeconds(Me, 0.5F)
			While Me.Flashing
				Me.SetAlpha(0.3F)
				t = 0F
				While t < 0.05F
					If Not Me.Flashing Then
						Me.SetAlpha(1F)
						Exit While
					End If
					t += MyBase.LocalDeltaTime
					Yield Nothing
				End While
				If Not Me.Flashing Then
					Me.SetAlpha(1F)
					Exit While
				End If
				Me.SetAlpha(1F)
				t = 0F
				While t < 0.2F
					If Not Me.Flashing Then
						Me.SetAlpha(1F)
						Exit While
					End If
					t += MyBase.LocalDeltaTime
					Yield Nothing
				End While
				If Not Me.Flashing Then
					Me.SetAlpha(1F)
					Exit While
				End If
			End While
		End While
		Return
	End Function

	' Token: 0x040042D6 RID: 17110
	<SerializeField()>
	Private prong As GameObject

	' Token: 0x040042D7 RID: 17111
	<SerializeField()>
	Private cuphead As GameObject

	' Token: 0x040042D8 RID: 17112
	<SerializeField()>
	Private mugman As GameObject

	' Token: 0x040042D9 RID: 17113
	<SerializeField()>
	Private cupheadArm As GameObject

	' Token: 0x040042DA RID: 17114
	<SerializeField()>
	Private mugmanArm As GameObject

	' Token: 0x040042DB RID: 17115
	<Space(10F)>
	<SerializeField()>
	Private runDustRoot As Transform

	' Token: 0x040042DC RID: 17116
	<Space(10F)>
	<SerializeField()>
	Private dashEffect As Effect

	' Token: 0x040042DD RID: 17117
	<SerializeField()>
	Private groundedEffect As Effect

	' Token: 0x040042DE RID: 17118
	<SerializeField()>
	Private hitEffect As Effect

	' Token: 0x040042DF RID: 17119
	<SerializeField()>
	Private runEffect As Effect

	' Token: 0x040042E0 RID: 17120
	Private spriteRenderer As SpriteRenderer

	' Token: 0x040042E1 RID: 17121
	Private armRenderer As SpriteRenderer

	' Token: 0x040042E2 RID: 17122
	Private hitAnimation As Boolean

	' Token: 0x040042E3 RID: 17123
	Private super As Boolean

	' Token: 0x040042E4 RID: 17124
	Private shooting As Boolean

	' Token: 0x040042E5 RID: 17125
	Private fired As Boolean

	' Token: 0x040042E6 RID: 17126
	Private timeSinceStoppedShooting As Single = 100F

	' Token: 0x040042E7 RID: 17127
	Private Const STOP_SHOOTING_DELAY As Single = 0.0833F

	' Token: 0x040042E8 RID: 17128
	Private Const JUMP_END_ANIMATION_TIME As Single = 0.15F

	' Token: 0x040042E9 RID: 17129
	Private Const DASH_END_ANIMATION_TIME As Single = 0.15F

	' Token: 0x040042EA RID: 17130
	Private Const DASH_END_AIR_ANIMATION_TIME As Single = 0.108333334F

	' Token: 0x040042EB RID: 17131
	Private colorCoroutine As IEnumerator

	' Token: 0x020009D6 RID: 2518
	Public Enum Booleans
		' Token: 0x040042ED RID: 17133
		Dashing
		' Token: 0x040042EE RID: 17134
		Shooting
		' Token: 0x040042EF RID: 17135
		Grounded
		' Token: 0x040042F0 RID: 17136
		Turning
		' Token: 0x040042F1 RID: 17137
		Intro
		' Token: 0x040042F2 RID: 17138
		Dead
		' Token: 0x040042F3 RID: 17139
		NearLanding
		' Token: 0x040042F4 RID: 17140
		NearDashEnd
	End Enum

	' Token: 0x020009D7 RID: 2519
	Public Enum Integers
		' Token: 0x040042F6 RID: 17142
		MoveX
		' Token: 0x040042F7 RID: 17143
		MoveY
		' Token: 0x040042F8 RID: 17144
		LookX
		' Token: 0x040042F9 RID: 17145
		LookY
		' Token: 0x040042FA RID: 17146
		ArmVariant
	End Enum

	' Token: 0x020009D8 RID: 2520
	Public Enum Triggers
		' Token: 0x040042FC RID: 17148
		OnJump
		' Token: 0x040042FD RID: 17149
		OnGround
		' Token: 0x040042FE RID: 17150
		OnParry
		' Token: 0x040042FF RID: 17151
		OnWin
		' Token: 0x04004300 RID: 17152
		OnTurn
		' Token: 0x04004301 RID: 17153
		OnFire
	End Enum
End Class
