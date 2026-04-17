Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020004CF RID: 1231
Public Class AirshipCrabLevelCrab
	Inherits LevelProperties.AirshipCrab.Entity

	' Token: 0x17000316 RID: 790
	' (get) Token: 0x060014F3 RID: 5363 RVA: 0x000BBD09 File Offset: 0x000BA109
	' (set) Token: 0x060014F4 RID: 5364 RVA: 0x000BBD11 File Offset: 0x000BA111
	Public Property state As AirshipCrabLevelCrab.State

	' Token: 0x060014F5 RID: 5365 RVA: 0x000BBD1C File Offset: 0x000BA11C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.gems = New List(Of AirshipCrabLevelGems)()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = Me.crabHitBox.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.crabHitBox.enabled = False
	End Sub

	' Token: 0x060014F6 RID: 5366 RVA: 0x000BBD7C File Offset: 0x000BA17C
	Public Overrides Sub LevelInit(properties As LevelProperties.AirshipCrab)
		MyBase.LevelInit(properties)
		MyBase.StartCoroutine(Me.intro_cr())
		Me.closedPos = MyBase.transform.position
		Me.openPos = MyBase.transform.position
		Me.openPos.y = MyBase.transform.position.y + properties.CurrentState.main.openCrabOffsetY
	End Sub

	' Token: 0x060014F7 RID: 5367 RVA: 0x000BBDEE File Offset: 0x000BA1EE
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.properties.DealDamage(info.damage)
	End Sub

	' Token: 0x060014F8 RID: 5368 RVA: 0x000BBE01 File Offset: 0x000BA201
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060014F9 RID: 5369 RVA: 0x000BBE19 File Offset: 0x000BA219
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060014FA RID: 5370 RVA: 0x000BBE30 File Offset: 0x000BA230
	Private Iterator Function intro_cr() As IEnumerator
		Me.state = AirshipCrabLevelCrab.State.Closed
		Yield CupheadTime.WaitForSeconds(Me, 3F)
		MyBase.StartCoroutine(Me.barnacle_cr())
		MyBase.StartCoroutine(Me.spawn_gems_cr())
		Return
	End Function

	' Token: 0x060014FB RID: 5371 RVA: 0x000BBE4C File Offset: 0x000BA24C
	Private Sub StateHandler()
		If Me.state = AirshipCrabLevelCrab.State.Open Then
			MyBase.transform.position = Me.openPos
			Me.crabHitBox.enabled = True
			If Me.stateCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.stateCoroutine)
			End If
			Me.stateCoroutine = MyBase.StartCoroutine(Me.bubbles_cr())
		ElseIf Me.state = AirshipCrabLevelCrab.State.Closed Then
			MyBase.transform.position = Me.closedPos
			Me.crabHitBox.enabled = False
			If Me.stateCoroutine IsNot Nothing Then
				MyBase.StopCoroutine(Me.stateCoroutine)
			End If
			Me.stateCoroutine = MyBase.StartCoroutine(Me.gems_cr())
		End If
	End Sub

	' Token: 0x060014FC RID: 5372 RVA: 0x000BBF04 File Offset: 0x000BA304
	Private Iterator Function barnacle_cr() As IEnumerator
		Dim p As LevelProperties.AirshipCrab.Barnicles = MyBase.properties.CurrentState.barnicles
		Dim offsetY As Single = p.barnicleOffsetY
		Dim offsetX As Single = p.barnicleOffsetX
		Dim rotation As Single = Mathf.Atan2(0F, CSng(Level.Current.Left)) * 57.29578F
		While True
			Dim i As Integer = 0
			While CSng(i) < p.barnicleAmount
				Dim pos As Vector2 = Me.barncileRoot.position
				pos.y = Me.barncileRoot.position.y + offsetY * CSng(i)
				pos.x = Me.barncileRoot.position.x + offsetX
				Me.barnicleProjectile.Create(pos, rotation, p.bulletSpeed)
				Yield CupheadTime.WaitForSeconds(Me, p.shotDelay)
				i += 1
			End While
			Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
		End While
		Return
	End Function

	' Token: 0x060014FD RID: 5373 RVA: 0x000BBF20 File Offset: 0x000BA320
	Private Iterator Function spawn_gems_cr() As IEnumerator
		Me.state = AirshipCrabLevelCrab.State.Closed
		Dim p As LevelProperties.AirshipCrab.Gems = MyBase.properties.CurrentState.gems
		Dim anglePattern As String() = p.angleString.GetRandom().Split(New Char() { ","c })
		Dim angleIndex As Integer = 0
		Dim angle As Single = 0F
		Dim offsetX As Single = p.gemOffsetX
		Dim offsetY As Single = p.gemOffsetY
		Dim i As Integer = 0
		While CSng(i) < p.gemAmount
			Parser.FloatTryParse(anglePattern(angleIndex), angle)
			Dim pos As Vector2 = Me.barncileRoot.position
			pos.y = Me.barncileRoot.position.y + offsetY * CSng(i)
			pos.x = Me.barncileRoot.position.x + offsetX
			Dim gem As AirshipCrabLevelGems = Global.UnityEngine.[Object].Instantiate(Of AirshipCrabLevelGems)(Me.gemProjectile)
			gem.Init(p, pos, angle)
			Me.gems.Add(gem)
			angleIndex = (angleIndex + 1) Mod anglePattern.Length
			Yield Nothing
			i += 1
		End While
		Me.releaseAllAtOnce = False
		MyBase.StartCoroutine(Me.gems_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060014FE RID: 5374 RVA: 0x000BBF3C File Offset: 0x000BA33C
	Private Iterator Function gems_cr() As IEnumerator
		Dim p As LevelProperties.AirshipCrab.Gems = MyBase.properties.CurrentState.gems
		Dim delayPattern As String() = p.gemReleaseDelay.GetRandom().Split(New Char() { ","c })
		Dim waitTime As Single = 0F
		Dim t As Single = 0F
		Dim delayIndex As Integer = 0
		Dim counter As Integer = 0
		Dim checking As Boolean = True
		Dim startTimer As Boolean = False
		Dim resetTimer As Boolean = False
		Dim i As Integer = 0
		While CSng(i) < p.gemATKAmount
			If Not Me.gems(i).moving Then
				If Not Me.releaseAllAtOnce Then
					Parser.FloatTryParse(delayPattern(delayIndex), waitTime)
				End If
				Me.gems(i).parried = False
				Me.gems(i).lastSideHit = AirshipCrabLevelGems.SideHit.None
				Me.gems(i).PickMovement()
				If Not Me.releaseAllAtOnce Then
					Yield CupheadTime.WaitForSeconds(Me, waitTime)
					delayIndex = delayIndex Mod delayPattern.Length
				End If
			End If
			i += 1
		End While
		While checking
			Dim num As Integer = 0
			While CSng(num) < p.gemATKAmount
				If Me.gems(num).parried Then
					counter += 1
				End If
				If Me.gems(num).startTimer Then
					Me.gems(num).startTimer = False
					startTimer = True
					resetTimer = True
				End If
				If CSng(counter) = p.gemATKAmount Then
					checking = False
					Exit While
				End If
				num += 1
			End While
			If startTimer Then
				If resetTimer Then
					t = 0F
					resetTimer = False
				End If
				If t < p.gemHoldDuration Then
					t += CupheadTime.Delta
				Else
					Me.releaseAllAtOnce = True
					Me.state = AirshipCrabLevelCrab.State.Closed
					Me.StateHandler()
					startTimer = False
				End If
			End If
			counter = 0
			Yield Nothing
		End While
		Me.releaseAllAtOnce = False
		Me.state = AirshipCrabLevelCrab.State.Open
		Me.StateHandler()
		Return
	End Function

	' Token: 0x060014FF RID: 5375 RVA: 0x000BBF58 File Offset: 0x000BA358
	Private Iterator Function bubbles_cr() As IEnumerator
		Me.state = AirshipCrabLevelCrab.State.Open
		Dim p As LevelProperties.AirshipCrab.Bubbles = MyBase.properties.CurrentState.bubbles
		Dim bubblePattern As String() = p.bubbleCount.GetRandom().Split(New Char() { ","c })
		Dim index As Integer = 0
		MyBase.StartCoroutine(Me.bubble_timer_cr())
		While Me.state = AirshipCrabLevelCrab.State.Open
			Dim count As Single
			Parser.FloatTryParse(bubblePattern(index), count)
			Dim i As Integer = 0
			While CSng(i) < count
				Dim bubbles As AirshipCrabLevelBubbles = Global.UnityEngine.[Object].Instantiate(Of AirshipCrabLevelBubbles)(Me.bubbleProjectile)
				bubbles.Init(Me.bubbleRoot.transform.position, p, p.bubbleSpeed)
				Yield CupheadTime.WaitForSeconds(Me, p.bubbleRepeatDelay)
				i += 1
			End While
			index = (index + 1) Mod bubblePattern.Length
			Yield CupheadTime.WaitForSeconds(Me, p.bubbleMainDelay)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001500 RID: 5376 RVA: 0x000BBF74 File Offset: 0x000BA374
	Private Iterator Function bubble_timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.bubbles.openTimer)
		Me.state = AirshipCrabLevelCrab.State.Closed
		MyBase.StopCoroutine(Me.bubbles_cr())
		Me.StateHandler()
		Return
	End Function

	' Token: 0x04001E4F RID: 7759
	<SerializeField()>
	Private barncileRoot As Transform

	' Token: 0x04001E50 RID: 7760
	<SerializeField()>
	Private bubbleRoot As Transform

	' Token: 0x04001E51 RID: 7761
	<SerializeField()>
	Private crabHitBox As Collider2D

	' Token: 0x04001E52 RID: 7762
	<SerializeField()>
	Private barnicleProjectile As BasicProjectile

	' Token: 0x04001E53 RID: 7763
	<SerializeField()>
	Private bubbleProjectile As AirshipCrabLevelBubbles

	' Token: 0x04001E54 RID: 7764
	<SerializeField()>
	Private gemProjectile As AirshipCrabLevelGems

	' Token: 0x04001E56 RID: 7766
	Private damageDealer As DamageDealer

	' Token: 0x04001E57 RID: 7767
	Private damageReceiver As DamageReceiver

	' Token: 0x04001E58 RID: 7768
	Private closedPos As Vector3

	' Token: 0x04001E59 RID: 7769
	Private openPos As Vector3

	' Token: 0x04001E5A RID: 7770
	Private gems As List(Of AirshipCrabLevelGems)

	' Token: 0x04001E5B RID: 7771
	Private stateCoroutine As Coroutine

	' Token: 0x04001E5C RID: 7772
	Private releaseAllAtOnce As Boolean

	' Token: 0x020004D0 RID: 1232
	Public Enum State
		' Token: 0x04001E5E RID: 7774
		Closed
		' Token: 0x04001E5F RID: 7775
		Open
		' Token: 0x04001E60 RID: 7776
		Dead
	End Enum
End Class
