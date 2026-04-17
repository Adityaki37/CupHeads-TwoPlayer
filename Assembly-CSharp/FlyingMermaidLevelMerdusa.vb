Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200068D RID: 1677
Public Class FlyingMermaidLevelMerdusa
	Inherits LevelProperties.FlyingMermaid.Entity

	' Token: 0x170003A0 RID: 928
	' (get) Token: 0x06002359 RID: 9049 RVA: 0x0014BE4C File Offset: 0x0014A24C
	' (set) Token: 0x0600235A RID: 9050 RVA: 0x0014BE54 File Offset: 0x0014A254
	Public Property state As FlyingMermaidLevelMerdusa.State

	' Token: 0x0600235B RID: 9051 RVA: 0x0014BE60 File Offset: 0x0014A260
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Dim collisionChild As CollisionChild = Me.blockingColliders.gameObject.AddComponent(Of CollisionChild)()
		AddHandler collisionChild.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
	End Sub

	' Token: 0x0600235C RID: 9052 RVA: 0x0014BEC5 File Offset: 0x0014A2C5
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x0600235D RID: 9053 RVA: 0x0014BED8 File Offset: 0x0014A2D8
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600235E RID: 9054 RVA: 0x0014BEF0 File Offset: 0x0014A2F0
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600235F RID: 9055 RVA: 0x0014BF10 File Offset: 0x0014A310
	Public Overrides Sub LevelInit(properties As LevelProperties.FlyingMermaid)
		MyBase.LevelInit(properties)
		For Each flyingMermaidLevelEel As FlyingMermaidLevelEel In Me.eels
			flyingMermaidLevelEel.Init(properties.CurrentState.eel)
		Next
		AddHandler properties.OnBossDeath, AddressOf Me.OnBossDeath
	End Sub

	' Token: 0x06002360 RID: 9056 RVA: 0x0014BF68 File Offset: 0x0014A368
	Public Sub StartIntro(pos As Vector2)
		AudioManager.Play("level_mermaid_merdusa_cackle")
		MyBase.transform.position = pos
		MyBase.animator.SetTrigger("Continue")
		MyBase.StartCoroutine(Me.intro_cr())
		MyBase.StartCoroutine(Me.moveBack_cr())
	End Sub

	' Token: 0x06002361 RID: 9057 RVA: 0x0014BFBC File Offset: 0x0014A3BC
	Private Iterator Function intro_cr() As IEnumerator
		Me.StartEels()
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.state = FlyingMermaidLevelMerdusa.State.Idle
		Return
	End Function

	' Token: 0x06002362 RID: 9058 RVA: 0x0014BFD8 File Offset: 0x0014A3D8
	Private Iterator Function moveBack_cr() As IEnumerator
		Dim startX As Single = MyBase.transform.position.x
		Dim t As Single = 0F
		While t < Me.introMoveTime
			t += CupheadTime.Delta
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(startX, startX + Me.transformMoveX, t / Me.introMoveTime)), Nothing, Nothing)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002363 RID: 9059 RVA: 0x0014BFF4 File Offset: 0x0014A3F4
	Private Sub BlinkMaybe()
		Me.blinks += 1
		If Me.blinks >= Me.maxBlinks Then
			Me.blinks = 0
			Me.maxBlinks = Global.UnityEngine.Random.Range(2, 4)
			Me.blinkOverlaySprite.enabled = True
		Else
			Me.blinkOverlaySprite.enabled = False
		End If
	End Sub

	' Token: 0x06002364 RID: 9060 RVA: 0x0014C054 File Offset: 0x0014A454
	Public Sub StartEels()
		For Each flyingMermaidLevelEel As FlyingMermaidLevelEel In Me.eels
			flyingMermaidLevelEel.StartPattern()
		Next
	End Sub

	' Token: 0x06002365 RID: 9061 RVA: 0x0014C088 File Offset: 0x0014A488
	Private Iterator Function eels_cr() As IEnumerator
		For Each flyingMermaidLevelEel As FlyingMermaidLevelEel In Me.eels
			flyingMermaidLevelEel.Spawn()
		Next
		Dim allEelsGone As Boolean = False
		While Not allEelsGone
			allEelsGone = True
			For Each flyingMermaidLevelEel2 As FlyingMermaidLevelEel In Me.eels
				If flyingMermaidLevelEel2.state = FlyingMermaidLevelEel.State.Spawned Then
					allEelsGone = False
				End If
			Next
			Yield Nothing
		End While
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.eel.hesitateAfterAttack)
		Me.state = FlyingMermaidLevelMerdusa.State.Idle
		Return
	End Function

	' Token: 0x06002366 RID: 9062 RVA: 0x0014C0A3 File Offset: 0x0014A4A3
	Public Sub StartZap()
		Me.state = FlyingMermaidLevelMerdusa.State.Zap
		MyBase.StartCoroutine(Me.zap_cr())
	End Sub

	' Token: 0x06002367 RID: 9063 RVA: 0x0014C0BC File Offset: 0x0014A4BC
	Private Iterator Function zap_cr() As IEnumerator
		AudioManager.Play("level_mermaid_merdusa_zap_loop_start")
		MyBase.animator.SetTrigger("Zap")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Zap_Start", False, True)
		Me.laser.SetStoneTime(MyBase.properties.CurrentState.zap.stoneTime)
		Me.laser.animator.SetTrigger("Start")
		Me.laser.transform.SetParent(Nothing)
		AudioManager.PlayLoop("level_mermaid_merdusa_zap_loop")
		Me.laser.StartLaser()
		Yield Me.laser.animator.WaitForAnimationToEnd(Me, "Lightning_Start", False, True)
		Me.laser.animator.SetTrigger("End")
		AudioManager.[Stop]("level_mermaid_merdusa_zap_loop")
		AudioManager.Play("level_mermaid_merdusa_zap_loop_end")
		Me.laser.StopLaser()
		MyBase.animator.SetTrigger("Continue")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Zap_End", False, True)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.zap.hesitateAfterAttack.RandomFloat())
		Me.state = FlyingMermaidLevelMerdusa.State.Idle
		Return
	End Function

	' Token: 0x06002368 RID: 9064 RVA: 0x0014C0D8 File Offset: 0x0014A4D8
	Public Sub StartTransform()
		If Me.state = FlyingMermaidLevelMerdusa.State.Zap Then
			AudioManager.Play("level_mermaid_merdusa_zap_loop_end")
			Me.laser.StopLaser()
		End If
		AudioManager.[Stop]("level_mermaid_merdusa_zap_loop")
		Me.head.StartIntro(Me.headRoot.position)
		RemoveHandler MyBase.properties.OnBossDeath, AddressOf Me.OnBossDeath
		Me.Die()
	End Sub

	' Token: 0x06002369 RID: 9065 RVA: 0x0014C148 File Offset: 0x0014A548
	Private Sub Die()
		Dim list As List(Of FlyingMermaidLevelMerdusaBodyPart) = New List(Of FlyingMermaidLevelMerdusaBodyPart)()
		Me.StopAllCoroutines()
		AudioManager.Play("level_mermaid_merdusa_fallapart_turnstone")
		list.Add(Me.bodyPrefab.Create(Me.bodyRoot.position))
		list.Add(Me.leftArmPrefab.Create(Me.leftArmRoot.position))
		list.Add(Me.rightArmPrefab.Create(Me.rightArmRoot.position))
		Me.head.CheckParts(list.ToArray())
		Me.StopAllCoroutines()
		CupheadLevelCamera.Current.Shake(20F, 0.7F, False)
		For Each flyingMermaidLevelEel As FlyingMermaidLevelEel In Me.eels
			flyingMermaidLevelEel.Die(True, True)
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600236A RID: 9066 RVA: 0x0014C22C File Offset: 0x0014A62C
	Private Sub DieEasyMode()
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("Die")
		For Each flyingMermaidLevelEel As FlyingMermaidLevelEel In Me.eels
			flyingMermaidLevelEel.Die(True, True)
		Next
	End Sub

	' Token: 0x0600236B RID: 9067 RVA: 0x0014C276 File Offset: 0x0014A676
	Private Sub OnBossDeath()
		If Level.CurrentMode = Level.Mode.Easy Then
			Me.DieEasyMode()
		Else
			Me.Die()
		End If
	End Sub

	' Token: 0x0600236C RID: 9068 RVA: 0x0014C293 File Offset: 0x0014A693
	Private Sub RightSplash()
		Me.splashRight.Create(Me.splashRoot.transform.position)
	End Sub

	' Token: 0x0600236D RID: 9069 RVA: 0x0014C2B1 File Offset: 0x0014A6B1
	Private Sub LeftSplash()
		Me.splashLeft.Create(Me.splashRoot.transform.position)
	End Sub

	' Token: 0x04002BF7 RID: 11255
	<SerializeField()>
	Private introMoveTime As Single

	' Token: 0x04002BF8 RID: 11256
	<SerializeField()>
	Private transformMoveX As Single

	' Token: 0x04002BF9 RID: 11257
	<SerializeField()>
	Private blinkOverlaySprite As SpriteRenderer

	' Token: 0x04002BFA RID: 11258
	<SerializeField()>
	Private blockingColliders As Transform

	' Token: 0x04002BFB RID: 11259
	<SerializeField()>
	Private laser As FlyingMermaidLevelLaser

	' Token: 0x04002BFC RID: 11260
	<SerializeField()>
	Private eels As FlyingMermaidLevelEel()

	' Token: 0x04002BFD RID: 11261
	<SerializeField()>
	Private head As FlyingMermaidLevelMerdusaHead

	' Token: 0x04002BFE RID: 11262
	<SerializeField()>
	Private bodyPrefab As FlyingMermaidLevelMerdusaBodyPart

	' Token: 0x04002BFF RID: 11263
	<SerializeField()>
	Private leftArmPrefab As FlyingMermaidLevelMerdusaBodyPart

	' Token: 0x04002C00 RID: 11264
	<SerializeField()>
	Private rightArmPrefab As FlyingMermaidLevelMerdusaBodyPart

	' Token: 0x04002C01 RID: 11265
	<SerializeField()>
	Private headRoot As Transform

	' Token: 0x04002C02 RID: 11266
	<SerializeField()>
	Private bodyRoot As Transform

	' Token: 0x04002C03 RID: 11267
	<SerializeField()>
	Private leftArmRoot As Transform

	' Token: 0x04002C04 RID: 11268
	<SerializeField()>
	Private rightArmRoot As Transform

	' Token: 0x04002C05 RID: 11269
	<SerializeField()>
	Private splashLeft As Effect

	' Token: 0x04002C06 RID: 11270
	<SerializeField()>
	Private splashRight As Effect

	' Token: 0x04002C07 RID: 11271
	<SerializeField()>
	Private splashRoot As Transform

	' Token: 0x04002C08 RID: 11272
	Private damageDealer As DamageDealer

	' Token: 0x04002C09 RID: 11273
	Private damageReceiver As DamageReceiver

	' Token: 0x04002C0A RID: 11274
	Private startPos As Vector2

	' Token: 0x04002C0B RID: 11275
	Private blinks As Integer

	' Token: 0x04002C0C RID: 11276
	Private maxBlinks As Integer = 3

	' Token: 0x0200068E RID: 1678
	Public Enum State
		' Token: 0x04002C0E RID: 11278
		Intro
		' Token: 0x04002C0F RID: 11279
		Idle
		' Token: 0x04002C10 RID: 11280
		Zap
	End Enum
End Class
