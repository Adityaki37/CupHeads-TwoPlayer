Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004E4 RID: 1252
Public Class BaronessLevelJellybeans
	Inherits AbstractProjectile

	' Token: 0x1700031D RID: 797
	' (get) Token: 0x060015A9 RID: 5545 RVA: 0x000C214C File Offset: 0x000C054C
	' (set) Token: 0x060015AA RID: 5546 RVA: 0x000C2154 File Offset: 0x000C0554
	Public Property state As BaronessLevelJellybeans.State

	' Token: 0x060015AB RID: 5547 RVA: 0x000C2160 File Offset: 0x000C0560
	Public Function Create(properties As LevelProperties.Baroness.Jellybeans, pos As Vector3, speed As Single, health As Single) As BaronessLevelJellybeans
		Dim baronessLevelJellybeans As BaronessLevelJellybeans = TryCast(MyBase.Create(), BaronessLevelJellybeans)
		baronessLevelJellybeans.properties = properties
		baronessLevelJellybeans.speed = speed
		baronessLevelJellybeans.health = health
		baronessLevelJellybeans.transform.position = pos
		Return baronessLevelJellybeans
	End Function

	' Token: 0x060015AC RID: 5548 RVA: 0x000C219C File Offset: 0x000C059C
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.GetComponent(Of SpriteRenderer)().enabled = True
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.state = BaronessLevelJellybeans.State.Run
		AudioManager.Play("level_baroness_jellybean_spawn")
		Me.emitAudioFromObject.Add("level_baroness_jellybean_spawn")
		MyBase.StartCoroutine(Me.fade_color_cr())
		MyBase.StartCoroutine(Me.beginning_offset_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060015AD RID: 5549 RVA: 0x000C2232 File Offset: 0x000C0632
	Private Sub KillJelly()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.Die()
	End Sub

	' Token: 0x060015AE RID: 5550 RVA: 0x000C2246 File Offset: 0x000C0646
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060015AF RID: 5551 RVA: 0x000C2264 File Offset: 0x000C0664
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060015B0 RID: 5552 RVA: 0x000C2284 File Offset: 0x000C0684
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F AndAlso Me.state <> BaronessLevelJellybeans.State.Dead Then
			Me.state = BaronessLevelJellybeans.State.Dead
			MyBase.GetComponent(Of Collider2D)().enabled = False
			MyBase.animator.Play(If((Not Rand.Bool()), "Jellybean_Death_B", "Jellybean_Death_A"))
		End If
	End Sub

	' Token: 0x060015B1 RID: 5553 RVA: 0x000C22F6 File Offset: 0x000C06F6
	Protected Overridable Function hitPauseCoefficient() As Single
		Return If((Not MyBase.GetComponent(Of DamageReceiver)().IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x060015B2 RID: 5554 RVA: 0x000C2318 File Offset: 0x000C0718
	Private Iterator Function fade_color_cr() As IEnumerator
		Dim endColor As Color = MyBase.GetComponent(Of SpriteRenderer)().color
		Dim fadeTime As Single = 0.2F
		Dim t As Single = 0F
		Dim start As Color = New Color(0F, 0F, 0F, 1F)
		While t < fadeTime
			MyBase.GetComponent(Of SpriteRenderer)().color = Color.Lerp(start, endColor, t / fadeTime)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.GetComponent(Of SpriteRenderer)().color = endColor
		Yield Nothing
		Return
	End Function

	' Token: 0x060015B3 RID: 5555 RVA: 0x000C2333 File Offset: 0x000C0733
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.explosion = Nothing
	End Sub

	' Token: 0x060015B4 RID: 5556 RVA: 0x000C2344 File Offset: 0x000C0744
	Private Iterator Function beginning_offset_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim pos As Vector3 = MyBase.transform.position
		Dim startPos As Vector3 = MyBase.transform.position
		Me.velocity = Me.properties.jumpSpeed
		pos.y = MyBase.transform.position.y
		startPos.y = MyBase.transform.position.y + 40F
		Me.originalPos = pos
		MyBase.transform.position = startPos
		While MyBase.transform.position.y >= pos.y
			If Me.state = BaronessLevelJellybeans.State.Run Then
				MyBase.transform.AddPosition(0F, -100F * CupheadTime.FixedDelta * Me.hitPauseCoefficient(), 0F)
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x060015B5 RID: 5557 RVA: 0x000C2360 File Offset: 0x000C0760
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.state = BaronessLevelJellybeans.State.Run
		Dim offset As Single = 200F
		While MyBase.transform.position.x > -640F - offset
			If Me.state <> BaronessLevelJellybeans.State.Jump Then
				Dim pos As Vector3 = MyBase.transform.position
				pos.x += -Me.speed * CupheadTime.FixedDelta * Me.hitPauseCoefficient()
				MyBase.transform.position = pos
			End If
			Yield wait
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x060015B6 RID: 5558 RVA: 0x000C237B File Offset: 0x000C077B
	Private Sub StartJump()
		Me.state = BaronessLevelJellybeans.State.Jump
		MyBase.StartCoroutine(Me.jump_cr())
	End Sub

	' Token: 0x060015B7 RID: 5559 RVA: 0x000C2394 File Offset: 0x000C0794
	Private Iterator Function jump_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Me.velocity = Me.properties.jumpSpeed
		Dim decrement As Single = 1F
		Dim pos As Vector3 = MyBase.transform.position
		Dim jumping As Boolean = True
		Dim landing As Boolean = False
		MyBase.animator.Play("Jellybean_Jump_Antic")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jellybean_Jump_Antic", False, True)
		While jumping
			MyBase.transform.AddPosition(0F, Me.velocity * CupheadTime.FixedDelta * Me.hitPauseCoefficient(), 0F)
			If MyBase.transform.position.y >= Me.properties.heightDefault + Me.properties.jumpHeight.RandomFloat() Then
				Me.velocity -= decrement
				If Not landing Then
					Me.velocity = -Me.velocity
					MyBase.animator.SetTrigger("Land")
					landing = True
				End If
			End If
			If MyBase.transform.position.y <= Me.originalPos.y Then
				If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsName("Jellybean_Jump_Land") Then
					Yield MyBase.animator.WaitForAnimationToEnd(Me, "Jellybean_Jump_Land", False, True)
				End If
				jumping = False
			End If
			Yield wait
		End While
		MyBase.StartCoroutine(Me.timer_cr())
		pos.y = Me.originalPos.y
		MyBase.transform.position = pos
		Me.state = BaronessLevelJellybeans.State.Run
		Yield Nothing
		Return
	End Function

	' Token: 0x060015B8 RID: 5560 RVA: 0x000C23B0 File Offset: 0x000C07B0
	Private Iterator Function timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.afterJumpDuration)
		Return
	End Function

	' Token: 0x060015B9 RID: 5561 RVA: 0x000C23CB File Offset: 0x000C07CB
	Private Sub DeathComplete()
		Me.explosion.Create(MyBase.transform.position)
		AudioManager.Play("level_baroness_jellybean_death")
		Me.emitAudioFromObject.Add("level_baroness_jellybean_death")
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x060015BA RID: 5562 RVA: 0x000C2409 File Offset: 0x000C0809
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
	End Sub

	' Token: 0x04001F04 RID: 7940
	<SerializeField()>
	Private explosion As Effect

	' Token: 0x04001F06 RID: 7942
	Private health As Single

	' Token: 0x04001F07 RID: 7943
	Private speed As Single

	' Token: 0x04001F08 RID: 7944
	Private velocity As Single

	' Token: 0x04001F09 RID: 7945
	Private originalPos As Vector3

	' Token: 0x04001F0A RID: 7946
	Private properties As LevelProperties.Baroness.Jellybeans

	' Token: 0x04001F0B RID: 7947
	Private damageReceiver As DamageReceiver

	' Token: 0x020004E5 RID: 1253
	Public Enum State
		' Token: 0x04001F0D RID: 7949
		Dead
		' Token: 0x04001F0E RID: 7950
		Run
		' Token: 0x04001F0F RID: 7951
		Jump
	End Enum
End Class
