Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200054B RID: 1355
Public Class ChessQueenLevelLightning
	Inherits AbstractProjectile

	' Token: 0x1700033E RID: 830
	' (get) Token: 0x06001904 RID: 6404 RVA: 0x000E2C38 File Offset: 0x000E1038
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x1700033F RID: 831
	' (get) Token: 0x06001905 RID: 6405 RVA: 0x000E2C3F File Offset: 0x000E103F
	' (set) Token: 0x06001906 RID: 6406 RVA: 0x000E2C47 File Offset: 0x000E1047
	Public Property isGone As Boolean

	' Token: 0x06001907 RID: 6407 RVA: 0x000E2C50 File Offset: 0x000E1050
	Public Function Create(posX As Single, properties As LevelProperties.ChessQueen.Lightning) As ChessQueenLevelLightning
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = New Vector3(posX, -385F)
		Me.properties = properties
		Me.lionsLandDustFX.Create(Me.dropDustPos.transform.position)
		MyBase.StartCoroutine(Me.move_cr())
		Return Me
	End Function

	' Token: 0x06001908 RID: 6408 RVA: 0x000E2CB0 File Offset: 0x000E10B0
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001909 RID: 6409 RVA: 0x000E2CCE File Offset: 0x000E10CE
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.StopAllCoroutines()
		Me.Die()
		Me.isGone = True
		MyBase.StartCoroutine(Me.death_cr())
	End Sub

	' Token: 0x0600190A RID: 6410 RVA: 0x000E2CF0 File Offset: 0x000E10F0
	Private Sub LateUpdate()
		Dim num As Integer = Mathf.Clamp(CInt((MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * CSng(Me.deathSparkSprites.Length))), 0, Me.deathSparkSprites.Length - 1)
		If num < 0 Then
			Return
		End If
		Me.bottomRenderer.sortingOrder = If((Not ChessQueenLevelLightning.BottomInFront(num)), (-1), 1)
		Me.topRenderer.sortingOrder = If((Not ChessQueenLevelLightning.MiddleInFront(num)), 1, (-1))
	End Sub

	' Token: 0x0600190B RID: 6411 RVA: 0x000E2D74 File Offset: 0x000E1174
	Private Iterator Function move_cr() As IEnumerator
		Me.SFX_KOG_QUEEN_ChessPiecesFall()
		MyBase.transform.localScale = New Vector3(Mathf.Sign(MyBase.transform.position.x), 1F)
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Intro", False, True)
		Dim delayTime As Single = Me.properties.lightningDelayTime - 0.5416667F
		Yield CupheadTime.WaitForSeconds(Me, delayTime)
		Me.SFX_KOG_QUEEN_ChessPieceRoar()
		Me.speed = Mathf.Sign(MyBase.transform.position.x) * -Me.properties.lightningSweepSpeed
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While MyBase.transform.position.x > CSng(Level.Current.Left) - 400F AndAlso MyBase.transform.position.x < CSng(Level.Current.Right) + 400F
			MyBase.transform.AddPosition(Me.speed * CupheadTime.FixedDelta, 0F, 0F)
			Yield wait
		End While
		Me.isGone = True
		Me.Recycle()
		Return
	End Function

	' Token: 0x0600190C RID: 6412 RVA: 0x000E2D90 File Offset: 0x000E1190
	Private Iterator Function death_cr() As IEnumerator
		Dim animationHelper As AnimationHelper = MyBase.GetComponent(Of AnimationHelper)()
		animationHelper.Speed = 0F
		Dim index As Integer = Mathf.Clamp(CInt((MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * CSng(Me.deathSparkSprites.Length))), 0, Me.deathSparkSprites.Length - 1)
		If index < 0 Then
			index = 0
		End If
		Me.bottomRenderer.enabled = False
		Me.deathSparkRenderer.sprite = Me.deathSparkSprites(index)
		Yield CupheadTime.WaitForSeconds(Me, 0.041666668F)
		Me.deathSparkRenderer.sprite = Nothing
		Me.bottomRenderer.enabled = True
		animationHelper.Speed = 1F
		MyBase.animator.Play("Death")
		Me.SFX_KOG_QUEEN_ChessPiecesParried()
		MyBase.StartCoroutine(Me.SFX_KOG_QUEEN_ChessPieceMeow_cr())
		MyBase.animator.SetTrigger("DustEnd")
		Dim minSpeed As Single = Me.speed * 0.2F
		Dim maxSpeed As Single = Me.speed * 0.8F
		Dim part As SpriteDeathParts = Me.deathParts.CreatePart(Me.bottomRenderer.transform.position)
		part.SetVelocityX(minSpeed, maxSpeed)
		part.GetComponent(Of SpriteRenderer)().sortingOrder = 13
		part.transform.localScale = MyBase.transform.localScale
		part = Me.deathParts.CreatePart(Me.middleRenderer.transform.position)
		part.SetVelocityX(minSpeed, maxSpeed)
		part.GetComponent(Of SpriteRenderer)().sortingOrder = 14
		part.transform.localScale = New Vector3(-MyBase.transform.localScale.x, 1F)
		part = Me.deathParts.CreatePart(Me.topRenderer.transform.position)
		part.SetVelocityX(minSpeed, maxSpeed)
		part.transform.localScale = MyBase.transform.localScale
		part = Me.deathDust.CreatePart(Me.dustRenderer.transform.position)
		part.SetVelocityX(Me.speed * 0.5F, Me.speed * 0.5F)
		part.transform.localScale = MyBase.transform.localScale
		Return
	End Function

	' Token: 0x0600190D RID: 6413 RVA: 0x000E2DAB File Offset: 0x000E11AB
	Private Sub SFX_KOG_QUEEN_ChessPieceRoar()
		AudioManager.Play("sfx_dlc_kog_queen_chesspieceroar")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspieceroar")
	End Sub

	' Token: 0x0600190E RID: 6414 RVA: 0x000E2DC7 File Offset: 0x000E11C7
	Private Sub SFX_KOG_QUEEN_ChessPiecesFall()
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecesfall")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecesfall")
	End Sub

	' Token: 0x0600190F RID: 6415 RVA: 0x000E2DE3 File Offset: 0x000E11E3
	Private Sub SFX_KOG_QUEEN_ChessPiecesParried()
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecesparried")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecesparried")
	End Sub

	' Token: 0x06001910 RID: 6416 RVA: 0x000E2E00 File Offset: 0x000E1200
	Private Iterator Function SFX_KOG_QUEEN_ChessPieceMeow_cr() As IEnumerator
		AudioManager.[Stop]("sfx_dlc_kog_queen_chesspieceroar")
		Yield CupheadTime.WaitForSeconds(Me, 0.17F)
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecemeow")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecemeow")
		Return
	End Function

	' Token: 0x0400221E RID: 8734
	Private Const YPosition As Single = -385F

	' Token: 0x0400221F RID: 8735
	Private Shared BottomInFront As Boolean() = New Boolean() { True, True, True, False, False, False, False, False, True, True, True, False, False, False, False, False }

	' Token: 0x04002220 RID: 8736
	Private Shared MiddleInFront As Boolean() = New Boolean() { False, False, False, False, True, True, True, False, False, False, False, False, True, True, True, False }

	' Token: 0x04002221 RID: 8737
	<SerializeField()>
	Private bottomRenderer As SpriteRenderer

	' Token: 0x04002222 RID: 8738
	<SerializeField()>
	Private middleRenderer As SpriteRenderer

	' Token: 0x04002223 RID: 8739
	<SerializeField()>
	Private topRenderer As SpriteRenderer

	' Token: 0x04002224 RID: 8740
	<SerializeField()>
	Private dustRenderer As SpriteRenderer

	' Token: 0x04002225 RID: 8741
	<SerializeField()>
	Private deathSparkRenderer As SpriteRenderer

	' Token: 0x04002226 RID: 8742
	<SerializeField()>
	Private rotatingSprites As Sprite()

	' Token: 0x04002227 RID: 8743
	<SerializeField()>
	Private deathSparkSprites As Sprite()

	' Token: 0x04002228 RID: 8744
	<SerializeField()>
	Private lionsLandDustFX As Effect

	' Token: 0x04002229 RID: 8745
	<SerializeField()>
	Private dropDustPos As Transform

	' Token: 0x0400222A RID: 8746
	<SerializeField()>
	Private deathParts As SpriteDeathParts

	' Token: 0x0400222B RID: 8747
	<SerializeField()>
	Private deathDust As SpriteDeathPartsDLC

	' Token: 0x0400222D RID: 8749
	Private properties As LevelProperties.ChessQueen.Lightning

	' Token: 0x0400222E RID: 8750
	Private speed As Single
End Class
