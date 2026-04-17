Imports System
Imports UnityEngine

' Token: 0x020006C8 RID: 1736
Public Class GraveyardLevelSplitDevilBeam
	Inherits AbstractProjectile

	' Token: 0x170003B8 RID: 952
	' (get) Token: 0x060024F1 RID: 9457 RVA: 0x0015A7E6 File Offset: 0x00158BE6
	' (set) Token: 0x060024F2 RID: 9458 RVA: 0x0015A7EE File Offset: 0x00158BEE
	Public Property devil As GraveyardLevelSplitDevil

	' Token: 0x060024F3 RID: 9459 RVA: 0x0015A7F7 File Offset: 0x00158BF7
	Protected Overrides Sub RandomizeVariant()
	End Sub

	' Token: 0x060024F4 RID: 9460 RVA: 0x0015A7FC File Offset: 0x00158BFC
	Public Function Create(pos As Vector3, xVelocity As Single, warningTime As Single, devil As GraveyardLevelSplitDevil) As GraveyardLevelSplitDevilBeam
		Dim graveyardLevelSplitDevilBeam As GraveyardLevelSplitDevilBeam = TryCast(MyBase.Create(pos), GraveyardLevelSplitDevilBeam)
		graveyardLevelSplitDevilBeam.xVelocity = xVelocity
		graveyardLevelSplitDevilBeam.DestroyDistance = CSng((Level.Current.Width + 200))
		graveyardLevelSplitDevilBeam.devil = devil
		graveyardLevelSplitDevilBeam.warningTime = warningTime
		graveyardLevelSplitDevilBeam.fireOn = Not graveyardLevelSplitDevilBeam.devil.isAngel
		graveyardLevelSplitDevilBeam.coll.enabled = Not graveyardLevelSplitDevilBeam.devil.isAngel
		graveyardLevelSplitDevilBeam.UpdateFade(1F)
		If graveyardLevelSplitDevilBeam.fireOn Then
			graveyardLevelSplitDevilBeam.fireAnim.Play("Form", 1, 0F)
			graveyardLevelSplitDevilBeam.fireAnim.Update(0F)
			Dim effect As Effect = Me.igniteFX.Create(graveyardLevelSplitDevilBeam.transform.position, Me.fireAnim)
			effect.transform.parent = graveyardLevelSplitDevilBeam.transform
			AudioManager.Play("sfx_dlc_graveyard_beamchange_fireon")
			graveyardLevelSplitDevilBeam.emitAudioFromObject.Add("sfx_dlc_graveyard_beamchange_fireon")
		Else
			For Each spriteRenderer As SpriteRenderer In graveyardLevelSplitDevilBeam.lightRend
				spriteRenderer.color = New Color(1F, 1F, 1F, 0F)
			Next
		End If
		CupheadLevelCamera.Current.StartShake(4F)
		Return graveyardLevelSplitDevilBeam
	End Function

	' Token: 0x060024F5 RID: 9461 RVA: 0x0015A954 File Offset: 0x00158D54
	Protected Overrides Sub Update()
		MyBase.Update()
		If MyBase.dead Then
			Return
		End If
		If Me.devil.dead Then
			Me.coll.enabled = False
			Me.forceFade = True
		End If
		If Me.warningTime <= 0F Then
			MyBase.transform.AddPosition(Me.xVelocity * CupheadTime.Delta, 0F, 0F)
			If Me.fireAnim.GetBool("Smoke") Then
				Me.flameTrailDistanceTracker += Mathf.Abs(Me.xVelocity) * CupheadTime.Delta
			End If
		Else
			Me.warningTime -= CupheadTime.Delta
		End If
		While Me.flameTrailDistanceTracker > Me.flameTrailSpacing AndAlso Not Me.forceFade
			Me.flameTrailDistanceTracker -= Me.flameTrailSpacing
			Me.SpawnTrailFX()
		End While
		If Mathf.Abs(MyBase.transform.position.x) < CSng(If((Mathf.Sign(MyBase.transform.position.x) <> Mathf.Sign(Me.xVelocity)), 600, 400)) AndAlso Not Me.fireAnim.GetBool("Smoke") Then
			Me.SpawnTrailFX()
		End If
		If Mathf.Abs(MyBase.transform.position.x) < 550F AndAlso Not Me.onGround Then
			Me.onGround = True
			Me.lightAnim.Play(If((Not Rand.Bool()), "B", "A"), 1, 0F)
		End If
		Me.fireAnim.SetBool("Smoke", Mathf.Abs(MyBase.transform.position.x) < CSng(If((Mathf.Sign(MyBase.transform.position.x) <> Mathf.Sign(Me.xVelocity)), 600, 400)))
		Me.coll.enabled = Not Me.devil.isAngel
		If Me.fireOn <> Not Me.devil.isAngel Then
			If Me.fireOn AndAlso Not Me.fireAnim.GetCurrentAnimatorStateInfo(1).IsName("Form") AndAlso Me.fireAnim.GetBool("Smoke") Then
				Dim effect As Effect = Me.bottomSmokeFX.Create(MyBase.transform.position)
				If Me.bottomSmokeFXTypeA Then
					effect.Play()
				End If
				Me.bottomSmokeFXTypeA = Not Me.bottomSmokeFXTypeA
				effect.transform.parent = MyBase.transform
				Dim effect2 As Effect = Me.midSmokeFX.Create(Me.midSmokePos.position)
				effect2.transform.localScale = Me.midSmokePos.localScale
				effect2.transform.parent = MyBase.transform
			End If
			If Not Me.fireOn Then
				AudioManager.Play("sfx_dlc_graveyard_beamchange_fireon")
				Me.emitAudioFromObject.Add("sfx_dlc_graveyard_beamchange_fireon")
				Dim effect3 As Effect = Me.igniteFX.Create(MyBase.transform.position, Me.fireAnim)
				effect3.transform.parent = MyBase.transform
			End If
			Me.fireAnim.Play(If((Not Me.fireOn), "Form", "Dissipate"), 1, 0F)
			Me.fireAnim.Update(0F)
			Me.fireOn = Not Me.devil.isAngel
		End If
		Me.frameTimer += CupheadTime.Delta
		While Me.frameTimer > 0.041666668F
			Me.frameTimer -= 0.041666668F
			Me.UpdateFade(0.25F)
		End While
	End Sub

	' Token: 0x060024F6 RID: 9462 RVA: 0x0015AD78 File Offset: 0x00159178
	Private Sub UpdateFade(amount As Single)
		For Each spriteRenderer As SpriteRenderer In Me.fireRend
			spriteRenderer.color = New Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + If((Not(Me.coll.enabled And Not Me.forceFade)), (-amount), amount), 0F, 1F))
		Next
		For Each spriteRenderer2 As SpriteRenderer In Me.lightRend
			spriteRenderer2.color = New Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + If((Not Me.coll.enabled AndAlso Not Me.forceFade), amount, (-amount)), 0F, 1F))
		Next
		Me.groundSpotlight.color = New Color(1F, 1F, 1F, Mathf.Clamp(Me.groundSpotlight.color.a + If((Me.coll.enabled OrElse Not(Me.onGround And Not Me.forceFade)), (-amount), amount), 0F, 1F))
	End Sub

	' Token: 0x060024F7 RID: 9463 RVA: 0x0015AF34 File Offset: 0x00159334
	Private Sub SpawnTrailFX()
		Me.trailFX.Create(New Vector3(Mathf.Clamp(MyBase.transform.position.x + Me.flameTrailSpacing * Mathf.Sign(Me.xVelocity), -550F, 550F), MyBase.transform.position.y), New Vector3(-Mathf.Sign(Me.xVelocity), 1F), Me, Me.flameTrailAnim)
		Me.flameTrailAnim = (Me.flameTrailAnim + 1) Mod 3
	End Sub

	' Token: 0x060024F8 RID: 9464 RVA: 0x0015AFC8 File Offset: 0x001593C8
	Private Sub LateUpdate()
		Me.fireRend(0).enabled = Me.fireFormDissipate.sprite Is Nothing
		Me.sparkleBeam.transform.localPosition = New Vector3(0F, CSng((-1280 + CInt(Me.lightAnim.GetCurrentAnimatorStateInfo(0).normalizedTime) Mod 8 * 280)))
	End Sub

	' Token: 0x060024F9 RID: 9465 RVA: 0x0015B031 File Offset: 0x00159431
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060024FA RID: 9466 RVA: 0x0015B04F File Offset: 0x0015944F
	Protected Overrides Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002D93 RID: 11667
	Private xVelocity As Single

	' Token: 0x04002D95 RID: 11669
	<SerializeField()>
	Private fireAnim As Animator

	' Token: 0x04002D96 RID: 11670
	<SerializeField()>
	Private lightAnim As Animator

	' Token: 0x04002D97 RID: 11671
	<SerializeField()>
	Private fireRend As SpriteRenderer()

	' Token: 0x04002D98 RID: 11672
	<SerializeField()>
	Private lightRend As SpriteRenderer()

	' Token: 0x04002D99 RID: 11673
	<SerializeField()>
	Private fireFormDissipate As SpriteRenderer

	' Token: 0x04002D9A RID: 11674
	<SerializeField()>
	Private bottomSmokeFX As Effect

	' Token: 0x04002D9B RID: 11675
	Private bottomSmokeFXTypeA As Boolean = True

	' Token: 0x04002D9C RID: 11676
	<SerializeField()>
	Private midSmokeFX As Effect

	' Token: 0x04002D9D RID: 11677
	<SerializeField()>
	Private midSmokePos As Transform

	' Token: 0x04002D9E RID: 11678
	<SerializeField()>
	Private igniteFX As GraveyardLevelSplitDevilBeamIgniteFX

	' Token: 0x04002D9F RID: 11679
	<SerializeField()>
	Private trailFX As GraveyardLevelSplitDevilBeamTrailFX

	' Token: 0x04002DA0 RID: 11680
	<SerializeField()>
	Private flameTrailSpacing As Single = 128F

	' Token: 0x04002DA1 RID: 11681
	<SerializeField()>
	Private sparkleBeam As GameObject

	' Token: 0x04002DA2 RID: 11682
	<SerializeField()>
	Private groundSpotlight As SpriteRenderer

	' Token: 0x04002DA3 RID: 11683
	Private onGround As Boolean

	' Token: 0x04002DA4 RID: 11684
	Private fireOn As Boolean

	' Token: 0x04002DA5 RID: 11685
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x04002DA6 RID: 11686
	Private warningTime As Single

	' Token: 0x04002DA7 RID: 11687
	Private frameTimer As Single

	' Token: 0x04002DA8 RID: 11688
	Private flameTrailDistanceTracker As Single

	' Token: 0x04002DA9 RID: 11689
	Private flameTrailAnim As Integer

	' Token: 0x04002DAA RID: 11690
	Private forceFade As Boolean
End Class
