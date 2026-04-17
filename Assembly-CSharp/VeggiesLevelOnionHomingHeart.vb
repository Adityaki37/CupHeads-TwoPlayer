Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200084F RID: 2127
Public Class VeggiesLevelOnionHomingHeart
	Inherits AbstractProjectile

	' Token: 0x17000429 RID: 1065
	' (get) Token: 0x0600314A RID: 12618 RVA: 0x001CDCBE File Offset: 0x001CC0BE
	' (set) Token: 0x0600314B RID: 12619 RVA: 0x001CDCC6 File Offset: 0x001CC0C6
	Public Property state As VeggiesLevelOnionHomingHeart.State

	' Token: 0x0600314C RID: 12620 RVA: 0x001CDCD0 File Offset: 0x001CC0D0
	Public Function CreateRadish(pos As Vector2, max As Single, acc As Single, hp As Integer, onLeft As Boolean) As VeggiesLevelOnionHomingHeart
		MyBase.transform.position = pos
		Dim veggiesLevelOnionHomingHeart As VeggiesLevelOnionHomingHeart = TryCast(MyBase.Create(), VeggiesLevelOnionHomingHeart)
		veggiesLevelOnionHomingHeart.maxSpeed = max
		veggiesLevelOnionHomingHeart.acceletration = acc
		veggiesLevelOnionHomingHeart.health = CSng(hp)
		veggiesLevelOnionHomingHeart.isOnLeft = onLeft
		Return veggiesLevelOnionHomingHeart
	End Function

	' Token: 0x1700042A RID: 1066
	' (get) Token: 0x0600314D RID: 12621 RVA: 0x001CDD1A File Offset: 0x001CC11A
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x0600314E RID: 12622 RVA: 0x001CDD24 File Offset: 0x001CC124
	Protected Overrides Sub Start()
		If Not Me.isOnLeft Then
			MyBase.transform.SetScale(New Single?(-MyBase.transform.localScale.x), New Single?(1F), New Single?(1F))
		End If
		MyBase.Start()
		Me.sprite = MyBase.GetComponent(Of SpriteRenderer)()
		Me.homingMovement = MyBase.GetComponent(Of GroundHomingMovement)()
		Me.homingMovement.maxSpeed = Me.maxSpeed
		Me.homingMovement.acceleration = Me.acceletration
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.start_cr())
	End Sub

	' Token: 0x0600314F RID: 12623 RVA: 0x001CDDE4 File Offset: 0x001CC1E4
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06003150 RID: 12624 RVA: 0x001CDE02 File Offset: 0x001CC202
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06003151 RID: 12625 RVA: 0x001CDE20 File Offset: 0x001CC220
	Private Iterator Function start_cr() As IEnumerator
		Me.state = VeggiesLevelOnionHomingHeart.State.Alive
		Me.sprite.sortingLayerName = SpriteLayer.Enemies.ToString()
		Me.sprite.sortingOrder = 0
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Radish_Intro", False, True)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		AudioManager.PlayLoop("level_veggies_raddish_loop")
		Me.emitAudioFromObject.Add("level_veggies_raddish_loop")
		Me.homingMovement.EnableHoming = True
		MyBase.StartCoroutine(Me.loop_cr())
		Return
	End Function

	' Token: 0x06003152 RID: 12626 RVA: 0x001CDE3B File Offset: 0x001CC23B
	Private Sub ChangeLayer()
		Me.sprite.sortingOrder = 3
	End Sub

	' Token: 0x06003153 RID: 12627 RVA: 0x001CDE4C File Offset: 0x001CC24C
	Private Iterator Function loop_cr() As IEnumerator
		While Me.state <> VeggiesLevelOnionHomingHeart.State.Dead
			Me.homingMovement.TrackingPlayer = PlayerManager.GetNext()
			Yield CupheadTime.WaitForSeconds(Me, 20F)
		End While
		Return
	End Function

	' Token: 0x06003154 RID: 12628 RVA: 0x001CDE68 File Offset: 0x001CC268
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F AndAlso Me.state <> VeggiesLevelOnionHomingHeart.State.Dead Then
			Me.state = VeggiesLevelOnionHomingHeart.State.Dead
			Me.homingMovement.enabled = False
			Me.StopAllCoroutines()
			AudioManager.[Stop]("level_veggies_raddish_loop")
			AudioManager.Play("level_veggies_raddish_End")
			Me.emitAudioFromObject.Add("level_veggies_raddish_End")
			MyBase.animator.SetTrigger("Dead")
		End If
	End Sub

	' Token: 0x06003155 RID: 12629 RVA: 0x001CDEF1 File Offset: 0x001CC2F1
	Private Sub CreateEffect()
		Me.deathPoof.Create(MyBase.transform.position)
	End Sub

	' Token: 0x06003156 RID: 12630 RVA: 0x001CDF0C File Offset: 0x001CC30C
	Private Sub CreatePieces()
		For Each spriteDeathParts As SpriteDeathParts In Me.deathPieces
			spriteDeathParts.CreatePart(MyBase.transform.position)
		Next
	End Sub

	' Token: 0x06003157 RID: 12631 RVA: 0x001CDF4A File Offset: 0x001CC34A
	Private Sub Destroy()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003158 RID: 12632 RVA: 0x001CDF57 File Offset: 0x001CC357
	Private Sub RaddishBonkSFX()
		AudioManager.Play("level_veggies_raddish_bonk")
		Me.emitAudioFromObject.Add("level_veggies_raddish_bonk")
	End Sub

	' Token: 0x06003159 RID: 12633 RVA: 0x001CDF73 File Offset: 0x001CC373
	Private Sub RaddishLoopStartSFX()
		AudioManager.Play("level_veggies_raddish_start")
		Me.emitAudioFromObject.Add("level_veggies_raddish_start")
	End Sub

	' Token: 0x0600315A RID: 12634 RVA: 0x001CDF8F File Offset: 0x001CC38F
	Private Sub RaddishDeathSFX()
		AudioManager.Play("level_veggies_raddish_death")
		Me.emitAudioFromObject.Add("level_veggies_raddish_death")
	End Sub

	' Token: 0x0600315B RID: 12635 RVA: 0x001CDFAB File Offset: 0x001CC3AB
	Private Sub RaddishVoiceDeathSFX()
		AudioManager.Play("veggies_Raddish_Voice_Death")
		Me.emitAudioFromObject.Add("veggies_Raddish_Voice_Death")
	End Sub

	' Token: 0x0600315C RID: 12636 RVA: 0x001CDFC7 File Offset: 0x001CC3C7
	Private Sub RaddishVoiceIntroSFX()
		AudioManager.Play("veggies_Raddish_Voice_Intro")
		Me.emitAudioFromObject.Add("veggies_Raddish_Voice_Intro")
	End Sub

	' Token: 0x040039DD RID: 14813
	<SerializeField()>
	Private deathPoof As Effect

	' Token: 0x040039DE RID: 14814
	<SerializeField()>
	Private deathPieces As SpriteDeathParts()

	' Token: 0x040039DF RID: 14815
	Private player As AbstractPlayerController

	' Token: 0x040039E0 RID: 14816
	Private isOnLeft As Boolean

	' Token: 0x040039E2 RID: 14818
	Private sprite As SpriteRenderer

	' Token: 0x040039E3 RID: 14819
	Private homingMovement As GroundHomingMovement

	' Token: 0x040039E4 RID: 14820
	Private damageReceiver As DamageReceiver

	' Token: 0x040039E5 RID: 14821
	Private maxSpeed As Single

	' Token: 0x040039E6 RID: 14822
	Private acceletration As Single

	' Token: 0x040039E7 RID: 14823
	Private health As Single

	' Token: 0x02000850 RID: 2128
	Public Enum State
		' Token: 0x040039E9 RID: 14825
		Alive
		' Token: 0x040039EA RID: 14826
		Dead
	End Enum
End Class
