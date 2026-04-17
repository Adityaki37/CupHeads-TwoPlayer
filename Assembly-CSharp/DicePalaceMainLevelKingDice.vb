Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005D7 RID: 1495
Public Class DicePalaceMainLevelKingDice
	Inherits LevelProperties.DicePalaceMain.Entity

	' Token: 0x06001D70 RID: 7536 RVA: 0x0010E3F8 File Offset: 0x0010C7F8
	Private Sub Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageReceiver.enabled = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
		AddHandler Level.Current.OnWinEvent, AddressOf Me.OnDeath
		AudioManager.Play("king_dice_intro")
		Me.emitAudioFromObject.Add("king_dice_intro")
	End Sub

	' Token: 0x06001D71 RID: 7537 RVA: 0x0010E470 File Offset: 0x0010C870
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x06001D72 RID: 7538 RVA: 0x0010E484 File Offset: 0x0010C884
	Public Sub StartKingDiceBattle()
		AudioManager.FadeBGMVolume(0F, 0.5F, True)
		AudioManager.Play("king_dice_trans")
		AudioManager.PlayBGMPlaylistManually(False)
		MyBase.animator.SetBool("IsAttacking", True)
		MyBase.animator.SetBool("IsBattling", True)
		Dim levelIntroAnimation As LevelIntroAnimation = LevelIntroAnimation.Create(Nothing)
		levelIntroAnimation.Play()
		MyBase.StartCoroutine(Me.cards_cr())
	End Sub

	' Token: 0x06001D73 RID: 7539 RVA: 0x0010E4ED File Offset: 0x0010C8ED
	Private Sub RevealSFX()
		AudioManager.Play("king_dice_reveal")
		Me.emitAudioFromObject.Add("king_dice_reveal")
	End Sub

	' Token: 0x06001D74 RID: 7540 RVA: 0x0010E50C File Offset: 0x0010C90C
	Public Sub RevealDice()
		Dim dicePalaceMainLevel As DicePalaceMainLevel = TryCast(Level.Current, DicePalaceMainLevel)
		dicePalaceMainLevel.GameManager.RevealDice()
	End Sub

	' Token: 0x06001D75 RID: 7541 RVA: 0x0010E530 File Offset: 0x0010C930
	Private Iterator Function cards_cr() As IEnumerator
		Dim p As LevelProperties.DicePalaceMain.Cards = MyBase.properties.CurrentState.cards
		Dim cardIndex As Integer = Global.UnityEngine.Random.Range(0, p.cardString.Length)
		Dim sideString As String() = p.cardSideOrder.GetRandom().Split(New Char() { ","c })
		Dim suitIndex As Integer = Global.UnityEngine.Random.Range(0, 3)
		Dim sideIndex As Integer = Global.UnityEngine.Random.Range(0, sideString.Length)
		Dim onLeft As Boolean = False
		Dim rootPos As Vector3 = Vector3.zero
		Me.damageReceiver.enabled = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		While True
			Dim cardString As String() = p.cardString(cardIndex).Split(New Char() { ","c })
			If sideString(sideIndex)(0) = "L"c Then
				onLeft = True
				rootPos = Me.leftRoot.transform.position
			ElseIf sideString(sideIndex)(0) = "R"c Then
				onLeft = False
				rootPos = Me.rightRoot.transform.position
			Else
				Global.Debug.LogError("Invalid pattern string", Nothing)
			End If
			MyBase.animator.SetBool("OnLeftAttack", onLeft)
			Yield MyBase.animator.WaitForAnimationToEnd(Me, If((Not onLeft), "Attack_Right", "Attack_Left"), False, True)
			AudioManager.PlayLoop("king_dice_march_loop")
			Me.emitAudioFromObject.Add("king_dice_march_loop")
			MyBase.StartCoroutine(Me.kd_laugh_cr())
			For i As Integer = 0 To cardString.Length - 1
				If cardString(i)(0) = "R"c Then
					Dim dicePalaceMainLevelCard As DicePalaceMainLevelCard = Me.cardRegular.Create(rootPos, p, onLeft)
					dicePalaceMainLevelCard.transform.SetScale(New Single?(CSng(If((Not onLeft), (-1), 1))), Nothing, Nothing)
					dicePalaceMainLevelCard.GetComponent(Of SpriteRenderer)().sortingOrder = i
					suitIndex = (suitIndex + 1) Mod 3
				ElseIf cardString(i)(0) = "P"c Then
					Dim dicePalaceMainLevelCard2 As DicePalaceMainLevelCard = Me.cardPink.Create(rootPos, p, onLeft)
					dicePalaceMainLevelCard2.transform.SetScale(New Single?(CSng(If((Not onLeft), (-1), 1))), Nothing, Nothing)
					dicePalaceMainLevelCard2.GetComponent(Of SpriteRenderer)().sortingOrder = i
				Else
					Global.Debug.LogError("Invalid pattern string", Nothing)
				End If
				Yield CupheadTime.WaitForSeconds(Me, p.cardDelay)
			Next
			AudioManager.[Stop]("king_dice_march_loop")
			MyBase.animator.SetBool("IsAttacking", False)
			Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
			MyBase.animator.SetBool("IsAttacking", True)
			sideIndex = (sideIndex + 1) Mod sideString.Length
			cardIndex = (cardIndex + 1) Mod p.cardString.Length
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001D76 RID: 7542 RVA: 0x0010E54B File Offset: 0x0010C94B
	Private Sub AttackSFX()
		AudioManager.PlayLoop("king_dice_attack")
		Me.emitAudioFromObject.Add("king_dice_attack")
	End Sub

	' Token: 0x06001D77 RID: 7543 RVA: 0x0010E567 File Offset: 0x0010C967
	Private Sub IntroSFX()
		AudioManager.Play("king_dice_intro")
		Me.emitAudioFromObject.Add("king_dice_intro")
	End Sub

	' Token: 0x06001D78 RID: 7544 RVA: 0x0010E583 File Offset: 0x0010C983
	Private Sub VoxCurious()
		AudioManager.Play("vox_curious")
		Me.emitAudioFromObject.Add("vox_curious")
	End Sub

	' Token: 0x06001D79 RID: 7545 RVA: 0x0010E59F File Offset: 0x0010C99F
	Private Sub AttackSFXStop()
		AudioManager.[Stop]("king_dice_attack")
	End Sub

	' Token: 0x06001D7A RID: 7546 RVA: 0x0010E5AC File Offset: 0x0010C9AC
	Private Sub OnDeath()
		AudioManager.PlayLoop("king_dice_death")
		AudioManager.Play("vox_death")
		Me.emitAudioFromObject.Add("vox_death")
		MyBase.animator.SetTrigger("OnDeath")
		Me.StopAllCoroutines()
		Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		component.sortingLayerName = "Background"
		component.sortingOrder = 100
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06001D7B RID: 7547 RVA: 0x0010E61C File Offset: 0x0010CA1C
	Private Iterator Function kd_laugh_cr() As IEnumerator
		Dim delay As MinMax = New MinMax(1F, 3.5F)
		While MyBase.animator.GetBool("IsAttacking")
			AudioManager.Play("king_dice_attack_vox")
			Me.emitAudioFromObject.Add("king_dice_attack_vox")
			While AudioManager.CheckIfPlaying("king_dice_attack_vox")
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, delay.RandomFloat())
		End While
		Return
	End Function

	' Token: 0x06001D7C RID: 7548 RVA: 0x0010E637 File Offset: 0x0010CA37
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.cardPink = Nothing
		Me.cardRegular = Nothing
	End Sub

	' Token: 0x04002663 RID: 9827
	<SerializeField()>
	Private rightRoot As Transform

	' Token: 0x04002664 RID: 9828
	<SerializeField()>
	Private leftRoot As Transform

	' Token: 0x04002665 RID: 9829
	<SerializeField()>
	Private cardRegular As DicePalaceMainLevelCard

	' Token: 0x04002666 RID: 9830
	<SerializeField()>
	Private cardPink As DicePalaceMainLevelCard

	' Token: 0x04002667 RID: 9831
	Private damageReceiver As DamageReceiver
End Class
