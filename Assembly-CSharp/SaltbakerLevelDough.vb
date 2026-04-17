Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007C6 RID: 1990
Public Class SaltbakerLevelDough
	Inherits SaltbakerLevelPhaseOneProjectile

	' Token: 0x06002D09 RID: 11529 RVA: 0x001A8920 File Offset: 0x001A6D20
	Public Overridable Function Init(startPos As Vector3, speedX As Single, speedY As Single, gravity As Single, hp As Single, sortingOrder As Integer, animalType As Integer) As SaltbakerLevelDough
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = startPos
		Me.speedX = speedX
		Me.speedY = speedY
		Me.gravity = gravity
		Me.fromLeft = speedX > 0F
		Me.hp = hp
		Me.Jump()
		MyBase.GetComponent(Of SpriteRenderer)().sortingOrder = sortingOrder
		Me.animalType = animalType
		MyBase.animator.Play(Me.clipNames(animalType) + "Up")
		Return Me
	End Function

	' Token: 0x06002D0A RID: 11530 RVA: 0x001A89AB File Offset: 0x001A6DAB
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002D0B RID: 11531 RVA: 0x001A89D6 File Offset: 0x001A6DD6
	Protected Overrides Function SparksFollow() As Boolean
		Return Rand.Bool()
	End Function

	' Token: 0x06002D0C RID: 11532 RVA: 0x001A89DD File Offset: 0x001A6DDD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002D0D RID: 11533 RVA: 0x001A89FB File Offset: 0x001A6DFB
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.hp -= info.damage
		If Me.hp <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002D0E RID: 11534 RVA: 0x001A8A26 File Offset: 0x001A6E26
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002D0F RID: 11535 RVA: 0x001A8A44 File Offset: 0x001A6E44
	Private Sub Jump()
		MyBase.StartCoroutine(Me.jump_cr())
	End Sub

	' Token: 0x06002D10 RID: 11536 RVA: 0x001A8A54 File Offset: 0x001A6E54
	Private Iterator Function jump_cr() As IEnumerator
		MyBase.animator.Play(Global.UnityEngine.Random.Range(0, 4).ToString(), 1, 0F)
		MyBase.transform.localScale = New Vector3(Mathf.Sign(Me.speedX), 1F)
		Dim velocityX As Single = Me.speedX
		Dim velocityY As Single = Me.speedY
		Dim sizeX As Single = MyBase.GetComponent(Of Collider2D)().bounds.size.x
		Dim sizeY As Single = MyBase.GetComponent(Of Collider2D)().bounds.size.y
		Dim ground As Single = CSng(Level.Current.Ground) + sizeY / 2F + 50F
		Dim jumping As Boolean = False
		Dim goingUp As Boolean = False
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			velocityY = Me.speedY
			velocityX = Me.speedX
			jumping = True
			goingUp = True
			Dim arcTriggered As Boolean = False
			MyBase.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0F)
			While jumping
				velocityY -= Me.gravity * CupheadTime.FixedDelta
				MyBase.transform.AddPosition(velocityX * CupheadTime.FixedDelta, velocityY * CupheadTime.FixedDelta, 0F)
				MyBase.HandleShadow(0F, 0F)
				If goingUp AndAlso Not arcTriggered AndAlso velocityY <= Me.gravity * 4F * CupheadTime.FixedDelta Then
					MyBase.animator.SetTrigger("Arc")
					arcTriggered = True
				End If
				If velocityY < 0F AndAlso goingUp Then
					goingUp = False
					arcTriggered = False
				End If
				If velocityY < 0F AndAlso jumping AndAlso MyBase.transform.position.y - velocityY * CupheadTime.FixedDelta <= ground Then
					jumping = False
					MyBase.transform.position = New Vector3(MyBase.transform.position.x, ground)
					MyBase.HandleShadow(0F, 0F)
					MyBase.animator.SetTrigger("Bounce")
				End If
				If(MyBase.transform.position.x < CSng(Level.Current.Left) - sizeX AndAlso Not Me.fromLeft) OrElse (MyBase.transform.position.x > CSng(Level.Current.Right) + sizeX AndAlso Me.fromLeft) Then
					Me.Die()
				End If
				Yield wait
			End While
			Yield MyBase.animator.WaitForAnimationToStart(Me, Me.clipNames(Me.animalType) + "Bounce", False)
			While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75F
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x06002D11 RID: 11537 RVA: 0x001A8A70 File Offset: 0x001A6E70
	Private Sub AniEvent_SpawnDustCloud()
		Me.dustEffect.Create(New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground)))
	End Sub

	' Token: 0x06002D12 RID: 11538 RVA: 0x001A8AAC File Offset: 0x001A6EAC
	Protected Overrides Sub Die()
		Me.StopAllCoroutines()
		Me.coll.enabled = False
		Me.shadow.enabled = False
		Dim num As Integer = Global.UnityEngine.Random.Range(1, 4)
		For i As Integer = 0 To num - 1
			Me.debris.Create(MyBase.transform.position + MathUtils.RandomPointInUnitCircle() * 20F)
		Next
		Dim num2 As Integer = Global.UnityEngine.Random.Range(1, 3)
		MyBase.animator.Play("Death_" + num2)
		MyBase.transform.localScale = New Vector3(CSng(MathUtils.PlusOrMinus()), CSng(If((num2 >= 2), 1, MathUtils.PlusOrMinus())))
		If num2 < 2 Then
			MyBase.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		End If
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06002D13 RID: 11539 RVA: 0x001A8BAA File Offset: 0x001A6FAA
	Private Sub AnimationEvent_SFX_SALTBAKER_CookieBounce()
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookiebounce")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookiebounce")
	End Sub

	' Token: 0x06002D14 RID: 11540 RVA: 0x001A8BC6 File Offset: 0x001A6FC6
	Private Sub AnimationEvent_SFX_SALTBAKER_Cookie_AnimalCamel()
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalcamel")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalcamel")
	End Sub

	' Token: 0x06002D15 RID: 11541 RVA: 0x001A8BE2 File Offset: 0x001A6FE2
	Private Sub AnimationEvent_SFX_SALTBAKER_Cookie_AnimalLion()
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalLion")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalLion")
	End Sub

	' Token: 0x06002D16 RID: 11542 RVA: 0x001A8BFE File Offset: 0x001A6FFE
	Private Sub AnimationEvent_SFX_SALTBAKER_Cookie_AnimalElephant()
		AudioManager.Play("sfx_dlc_saltbaker_p1_cookie_animalElephant")
		Me.emitAudioFromObject.Add("sfx_dlc_saltbaker_p1_cookie_animalElephant")
	End Sub

	' Token: 0x04003588 RID: 13704
	Private Const GROUND_OFFSET As Single = 50F

	' Token: 0x04003589 RID: 13705
	Private speedX As Single

	' Token: 0x0400358A RID: 13706
	Private speedY As Single

	' Token: 0x0400358B RID: 13707
	Private gravity As Single

	' Token: 0x0400358C RID: 13708
	Private hp As Single

	' Token: 0x0400358D RID: 13709
	Private fromLeft As Boolean

	' Token: 0x0400358E RID: 13710
	Private damageReceiver As DamageReceiver

	' Token: 0x0400358F RID: 13711
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x04003590 RID: 13712
	<SerializeField()>
	Private dustEffect As Effect

	' Token: 0x04003591 RID: 13713
	<SerializeField()>
	Private debris As Effect

	' Token: 0x04003592 RID: 13714
	Private clipNames As String() = New String() { "Elephant", "Lion", "Camel" }

	' Token: 0x04003593 RID: 13715
	Private animalType As Integer
End Class
