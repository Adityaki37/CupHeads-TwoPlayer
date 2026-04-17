Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200064C RID: 1612
Public Class FlyingCowboyLevelBirdProjectile
	Inherits BasicProjectile

	' Token: 0x1700038A RID: 906
	' (get) Token: 0x06002129 RID: 8489 RVA: 0x00132B1C File Offset: 0x00130F1C
	Protected Overrides ReadOnly Property Direction As Vector3
		Get
			Return Me._direction
		End Get
	End Property

	' Token: 0x1700038B RID: 907
	' (get) Token: 0x0600212A RID: 8490 RVA: 0x00132B24 File Offset: 0x00130F24
	' (set) Token: 0x0600212B RID: 8491 RVA: 0x00132B2C File Offset: 0x00130F2C
	Public Property shrapnelCount As Integer

	' Token: 0x0600212C RID: 8492 RVA: 0x00132B35 File Offset: 0x00130F35
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.landingPosition_cr())
		MyBase.StartCoroutine(Me.shrapnel_cr())
		MyBase.StartCoroutine(Me.shadow_cr())
	End Sub

	' Token: 0x0600212D RID: 8493 RVA: 0x00132B64 File Offset: 0x00130F64
	Public Sub Initialize(initialVelocity As Vector2, gravity As Single, shrapnelDelay As Single, shrapnelSpeed As Single, shrapnelSpreadAngle As Single, cowgirl As FlyingCowboyLevelCowboy)
		Me.Speed = initialVelocity.magnitude
		Me._direction = initialVelocity.normalized
		Me.gravity = gravity
		Me.shrapnelDelay = shrapnelDelay
		Me.shrapnelSpeed = shrapnelSpeed
		Me.shrapnelSpreadAngle = shrapnelSpreadAngle
		Me.cowgirl = cowgirl
		Me.landingPosition = FlyingCowboyLevelBirdProjectile.HighLandingPosition
	End Sub

	' Token: 0x0600212E RID: 8494 RVA: 0x00132BC4 File Offset: 0x00130FC4
	Protected Overrides Sub FixedUpdate()
		Dim vector As Vector3 = Me.Direction * Me.Speed
		vector.y -= Me.gravity * CupheadTime.FixedDelta
		Me._direction = vector.normalized
		Me.Speed = vector.magnitude
		MyBase.FixedUpdate()
	End Sub

	' Token: 0x0600212F RID: 8495 RVA: 0x00132C20 File Offset: 0x00131020
	Private Iterator Function landingPosition_cr() As IEnumerator
		While MyBase.transform.position.y > 0F
			Yield Nothing
		End While
		If Me.cowgirl.onBottom AndAlso Me.cowgirl.state = FlyingCowboyLevelCowboy.State.BeamAttack Then
			Me.landingPosition = FlyingCowboyLevelBirdProjectile.LowLandingPosition
		End If
		Return
	End Function

	' Token: 0x06002130 RID: 8496 RVA: 0x00132C3C File Offset: 0x0013103C
	Private Iterator Function shadow_cr() As IEnumerator
		While MyBase.transform.position.y > Me.landingPosition + FlyingCowboyLevelBirdProjectile.ShadowTriggerDistance
			Yield Nothing
		End While
		MyBase.animator.Play("Land", FlyingCowboyLevelBirdProjectile.ShadowLayer)
		MyBase.animator.Update(0F)
		While Not MyBase.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelBirdProjectile.ShadowLayer).IsName("Off")
			Dim position As Vector3 = Me.shadowTransform.position
			position.y = Me.landingPosition
			Me.shadowTransform.position = position
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002131 RID: 8497 RVA: 0x00132C58 File Offset: 0x00131058
	Private Iterator Function shrapnel_cr() As IEnumerator
		While MyBase.transform.position.y > Me.landingPosition
			Yield Nothing
		End While
		Dim transform As Transform = MyBase.transform
		Dim num As Single? = New Single?(Me.landingPosition)
		transform.SetPosition(Nothing, num, Nothing)
		Me.move = False
		Dim initialAngle As Single = (180F - Me.shrapnelSpreadAngle) * 0.5F
		Dim angleInterval As Single = Me.shrapnelSpreadAngle / CSng((Me.shrapnelCount - 1))
		For i As Integer = 0 To Me.shrapnelCount - 1 Step 2
			Me.shrapnelPrefab.Create(Me.spawnPoint.position, initialAngle + angleInterval * CSng(i), Me.shrapnelSpeed)
		Next
		Me.SFX_COWGIRL_COWGIRL_P1_DynamiteExp()
		MyBase.animator.Play("Bounce")
		MyBase.animator.Play("A", FlyingCowboyLevelBirdProjectile.ExplosionLayer)
		MyBase.animator.Play("A", FlyingCowboyLevelBirdProjectile.SmokeLayer)
		MyBase.StartCoroutine(Me.moveSmoke_cr("A"))
		MyBase.animator.Play("Off", FlyingCowboyLevelBirdProjectile.ShadowLayer)
		Yield CupheadTime.WaitForSeconds(Me, Me.shrapnelDelay)
		For j As Integer = 1 To Me.shrapnelCount - 1 Step 2
			Me.shrapnelPrefab.Create(Me.spawnPoint.position, initialAngle + angleInterval * CSng(j), Me.shrapnelSpeed)
		Next
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.animator.Play("Off")
		MyBase.animator.Play("B", FlyingCowboyLevelBirdProjectile.ExplosionLayer)
		MyBase.animator.Play("B", FlyingCowboyLevelBirdProjectile.SmokeLayer)
		MyBase.StartCoroutine(Me.moveSmoke_cr("B"))
		Return
	End Function

	' Token: 0x06002132 RID: 8498 RVA: 0x00132C74 File Offset: 0x00131074
	Private Iterator Function moveSmoke_cr(animationName As String) As IEnumerator
		Dim initialPosition As Vector3 = Me.smokeTransform.position
		Yield MyBase.animator.WaitForAnimationToStart(Me, animationName, FlyingCowboyLevelBirdProjectile.SmokeLayer, False)
		Dim speed As Single = 0F
		While Not MyBase.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelBirdProjectile.SmokeLayer).IsName("Off")
			Yield Nothing
			speed += CupheadTime.Delta * 1500F
			Dim position As Vector3 = Me.smokeTransform.position
			position.x -= speed * CupheadTime.Delta
			Me.smokeTransform.position = position
		End While
		Me.smokeTransform.position = initialPosition
		Return
	End Function

	' Token: 0x06002133 RID: 8499 RVA: 0x00132C96 File Offset: 0x00131096
	Private Sub animationEvent_ExplosionsFinished()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002134 RID: 8500 RVA: 0x00132CA3 File Offset: 0x001310A3
	Private Sub SFX_COWGIRL_COWGIRL_P1_DynamiteExp()
		AudioManager.Play("sfx_DLC_Cowgirl_P1_DynamiteExp")
		Me.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_DynamiteExp")
	End Sub

	' Token: 0x040029BD RID: 10685
	Private Shared ExplosionLayer As Integer = 1

	' Token: 0x040029BE RID: 10686
	Private Shared SmokeLayer As Integer = 2

	' Token: 0x040029BF RID: 10687
	Private Shared ShadowLayer As Integer = 3

	' Token: 0x040029C0 RID: 10688
	Private Shared ShadowTriggerDistance As Single = 260F

	' Token: 0x040029C1 RID: 10689
	Public Shared HighLandingPosition As Single = -300F

	' Token: 0x040029C2 RID: 10690
	Public Shared LowLandingPosition As Single = -340F

	' Token: 0x040029C3 RID: 10691
	Private _direction As Vector3

	' Token: 0x040029C5 RID: 10693
	<SerializeField()>
	Private shadowTransform As Transform

	' Token: 0x040029C6 RID: 10694
	<SerializeField()>
	Private spawnPoint As Transform

	' Token: 0x040029C7 RID: 10695
	<SerializeField()>
	Private smokeTransform As Transform

	' Token: 0x040029C8 RID: 10696
	<SerializeField()>
	Private shrapnelPrefab As BasicProjectile

	' Token: 0x040029C9 RID: 10697
	Private landingPosition As Single

	' Token: 0x040029CA RID: 10698
	Private gravity As Single

	' Token: 0x040029CB RID: 10699
	Private shrapnelDelay As Single

	' Token: 0x040029CC RID: 10700
	Private shrapnelSpeed As Single

	' Token: 0x040029CD RID: 10701
	Private shrapnelSpreadAngle As Single

	' Token: 0x040029CE RID: 10702
	Private cowgirl As FlyingCowboyLevelCowboy
End Class
