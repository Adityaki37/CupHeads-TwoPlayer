Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000565 RID: 1381
Public Class ClownLevelDogBalloon
	Inherits AbstractProjectile

	' Token: 0x17000349 RID: 841
	' (get) Token: 0x060019FA RID: 6650 RVA: 0x000ED8CD File Offset: 0x000EBCCD
	' (set) Token: 0x060019FB RID: 6651 RVA: 0x000ED8D5 File Offset: 0x000EBCD5
	Public Property state As ClownLevelDogBalloon.State

	' Token: 0x060019FC RID: 6652 RVA: 0x000ED8E0 File Offset: 0x000EBCE0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AudioManager.Play("clown_dog_balloon_regular_intro")
		Me.emitAudioFromObject.Add("clown_dog_balloon_regular_intro")
	End Sub

	' Token: 0x060019FD RID: 6653 RVA: 0x000ED930 File Offset: 0x000EBD30
	Public Sub Init(HP As Single, pos As Vector2, velocity As Single, player As AbstractPlayerController, properties As LevelProperties.Clown.HeliumClown, flipped As Boolean)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.player = player
		Me.velocity = velocity
		Me.health = HP
		If flipped Then
			MyBase.transform.SetScale(New Single?(1F), New Single?(-MyBase.transform.localScale.y), New Single?(1F))
		End If
		Me.CalculateDirection()
		Me.CalculateSin()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060019FE RID: 6654 RVA: 0x000ED9C4 File Offset: 0x000EBDC4
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060019FF RID: 6655 RVA: 0x000ED9E4 File Offset: 0x000EBDE4
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		If Me.properties.dogDieOnGround AndAlso phase = CollisionPhase.Enter AndAlso Me.state <> ClownLevelDogBalloon.State.Unspawned Then
			Me.state = ClownLevelDogBalloon.State.Unspawned
			Me.StopAllCoroutines()
			MyBase.animator.SetTrigger("Death")
			AudioManager.Play("clown_dog_balloon_regular_death")
			Me.emitAudioFromObject.Add("clown_dog_balloon_regular_death")
		End If
	End Sub

	' Token: 0x06001A00 RID: 6656 RVA: 0x000EDA54 File Offset: 0x000EBE54
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F AndAlso Me.state <> ClownLevelDogBalloon.State.Unspawned Then
			Me.state = ClownLevelDogBalloon.State.Unspawned
			Me.StopAllCoroutines()
			MyBase.animator.SetTrigger("Death")
			AudioManager.Play("clown_dog_balloon_regular_death")
			Me.emitAudioFromObject.Add("clown_dog_balloon_regular_death")
		End If
	End Sub

	' Token: 0x06001A01 RID: 6657 RVA: 0x000EDAC7 File Offset: 0x000EBEC7
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A02 RID: 6658 RVA: 0x000EDAE8 File Offset: 0x000EBEE8
	Private Sub CalculateSin()
		Dim zero As Vector2 = Vector2.zero
		zero.x = (Me.player.transform.position.x + MyBase.transform.position.x) / 2F
		zero.y = (Me.player.transform.position.y + MyBase.transform.position.y) / 2F
		Dim num As Single = -((Me.player.transform.position.x - MyBase.transform.position.x) / (Me.player.transform.position.y - MyBase.transform.position.y))
		Dim num2 As Single = zero.y - num * zero.x
		Dim zero2 As Vector2 = Vector2.zero
		zero2.x = zero.x + 1F
		zero2.y = num * zero2.x + num2
		Me.normalized = Vector3.zero
		Me.normalized = zero2 - zero
		Me.normalized.Normalize()
	End Sub

	' Token: 0x06001A03 RID: 6659 RVA: 0x000EDC3C File Offset: 0x000EC03C
	Private Sub CalculateDirection()
		Dim num As Single = Me.player.transform.position.x - MyBase.transform.position.x
		Dim num2 As Single = Me.player.transform.position.y - MyBase.transform.position.y
		Dim num3 As Single = Mathf.Atan2(num2, num) * 57.29578F
		Me.pointAtPlayer = MathUtils.AngleToDirection(num3)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(num3))
	End Sub

	' Token: 0x06001A04 RID: 6660 RVA: 0x000EDCEC File Offset: 0x000EC0EC
	Private Iterator Function move_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		While MyBase.transform.position.y > -560F
			Me.angle += 10F * CupheadTime.Delta
			If CupheadTime.Delta IsNot 0F Then
				pos += Me.normalized * Mathf.Sin(Me.angle) * 2F
			End If
			pos += Me.pointAtPlayer * Me.velocity * CupheadTime.Delta
			MyBase.transform.position = pos
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A05 RID: 6661 RVA: 0x000EDD07 File Offset: 0x000EC107
	Private Sub ChompSound()
		AudioManager.Play("clown_dog_balloon_regular_chomp")
		Me.emitAudioFromObject.Add("clown_dog_balloon_regular_chomp")
	End Sub

	' Token: 0x06001A06 RID: 6662 RVA: 0x000EDD23 File Offset: 0x000EC123
	Protected Overrides Sub Die()
		AudioManager.Play("clown_dog_balloon_regular_death")
		Me.emitAudioFromObject.Add("clown_dog_balloon_regular_death")
		MyBase.Die()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub

	' Token: 0x04002320 RID: 8992
	Public properties As LevelProperties.Clown.HeliumClown

	' Token: 0x04002321 RID: 8993
	Private player As AbstractPlayerController

	' Token: 0x04002322 RID: 8994
	Private pointAtPlayer As Vector3

	' Token: 0x04002323 RID: 8995
	Private normalized As Vector3

	' Token: 0x04002324 RID: 8996
	Private health As Single

	' Token: 0x04002325 RID: 8997
	Private pointAt As Single

	' Token: 0x04002326 RID: 8998
	Private velocity As Single

	' Token: 0x04002327 RID: 8999
	Private angle As Single

	' Token: 0x04002328 RID: 9000
	Private damageReceiver As DamageReceiver

	' Token: 0x02000566 RID: 1382
	Public Enum State
		' Token: 0x0400232A RID: 9002
		Spawned
		' Token: 0x0400232B RID: 9003
		Unspawned
	End Enum
End Class
