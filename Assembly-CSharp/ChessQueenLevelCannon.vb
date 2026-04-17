Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000547 RID: 1351
Public Class ChessQueenLevelCannon
	Inherits AbstractCollidableObject

	' Token: 0x1700033C RID: 828
	' (get) Token: 0x060018E7 RID: 6375 RVA: 0x000E1C08 File Offset: 0x000E0008
	' (set) Token: 0x060018E8 RID: 6376 RVA: 0x000E1C10 File Offset: 0x000E0010
	Public Property IsActive As Boolean

	' Token: 0x060018E9 RID: 6377 RVA: 0x000E1C1C File Offset: 0x000E001C
	Private Sub Start()
		Me.parryColliders = New Collider2D(Me.parry.Length - 1) {}
		For i As Integer = 0 To Me.parry.Length - 1
			AddHandler Me.parry(i).OnActivate, AddressOf Me.shootCannonball
			Me.parryColliders(i) = Me.parry(i).GetComponent(Of Collider2D)()
		Next
		Me.SetActive(False)
		Me.mouseAnimator.Play("Idle")
		Me.mouseLookTime = Global.UnityEngine.Random.Range(1F, 3F)
		Me.setupWickParabola()
	End Sub

	' Token: 0x060018EA RID: 6378 RVA: 0x000E1CB5 File Offset: 0x000E00B5
	Public Sub SetProperties(minAngle As Single, maxAngle As Single, rotationTime As Single, cannonPosition As ChessQueenLevelCannon.CannonPosition, properties As LevelProperties.ChessQueen.Turret, queen As ChessQueenLevelQueen)
		Me.minAngle = minAngle
		Me.maxAngle = maxAngle
		Me.rotationTime = rotationTime
		Me.cannonPosition = cannonPosition
		Me.properties = properties
		Me.queen = queen
		Me.move()
	End Sub

	' Token: 0x060018EB RID: 6379 RVA: 0x000E1CEC File Offset: 0x000E00EC
	Public Sub SetActive(setActive As Boolean)
		Me.mouseAnimator.SetBool("Idle", Not setActive)
		If setActive Then
			Me.mouseAnimator.SetBool("LookRight", False)
		End If
		Me.IsActive = setActive
		For Each collider2D As Collider2D In Me.parryColliders
			collider2D.enabled = setActive
		Next
	End Sub

	' Token: 0x060018EC RID: 6380 RVA: 0x000E1D54 File Offset: 0x000E0154
	Private Sub LateUpdate()
		If Not Me.IsActive Then
			Me.mouseLookTime -= CupheadTime.Delta
			If Me.mouseLookTime < 0F Then
				Me.mouseAnimator.SetBool("LookRight", Not Me.mouseAnimator.GetBool("LookRight"))
				Me.mouseLookTime += Global.UnityEngine.Random.Range(1F, 3F)
			End If
		End If
		Dim num As Integer = Array.IndexOf(Of Sprite)(Me.baseSprites, Me.baseRenderer.sprite)
		If num < 0 Then
			If MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("01") Then
				num = 0
			ElseIf MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("05") Then
				num = 4
			ElseIf MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("10") Then
				num = 9
			Else
				If Not MyBase.animator.GetCurrentAnimatorStateInfo(0).IsTag("15") Then
					Return
				End If
				num = 14
			End If
		End If
		Dim num2 As Single = CSng(num) / CSng((Me.baseSprites.Length - 1))
		If Me.cannonPosition = ChessQueenLevelCannon.CannonPosition.Side Then
			num2 = EaseUtils.EaseInOutCubic(0F, 1F, num2)
		End If
		Dim num3 As Single = Mathf.Lerp(Me.minAngle, Me.maxAngle, num2)
		If Me.cannonPosition = ChessQueenLevelCannon.CannonPosition.Center Then
			num3 *= CSng(If((Not Me.baseRenderer.flipX), 1, (-1)))
		End If
		Me.barrelTransform.rotation = Quaternion.Euler(0F, 0F, num3)
		Me.barrelHighlightRenderer.sprite = Me.barrelHighlightSprites(Mathf.RoundToInt((1F - num2) * CSng((Me.barrelHighlightSprites.Length - 1))))
		Me.barrelHighlightRenderer.flipX = Me.cannonPosition = ChessQueenLevelCannon.CannonPosition.Center AndAlso Me.baseRenderer.flipX
		Me.barrelHighlightRenderer.enabled = MyBase.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle")
		If Me.wickFollowsParabola Then
			Dim num4 As Single = num2 * Me.wickParametricDuration
			Dim vector As Vector3 = Me.wickStartPosition
			vector.x -= 2F * Me.wickParabolaParameter * num4 * CSng(If((Not Me.baseRenderer.flipX), 1, (-1)))
			vector.y += Me.wickParabolaParameter * num4 * num4
			Me.wickTransform.position = vector
		End If
		If Level.Current.Ending Then
			For Each collider2D As Collider2D In Me.parryColliders
				collider2D.enabled = False
			Next
		End If
	End Sub

	' Token: 0x060018ED RID: 6381 RVA: 0x000E203F File Offset: 0x000E043F
	Private Sub move()
		If Me.cannonPosition = ChessQueenLevelCannon.CannonPosition.Side Then
			MyBase.animator.Play(0, ChessQueenLevelCannon.BaseAnimatorLayer, 0.5F)
		End If
		MyBase.StartCoroutine(Me.cannonActive_cr())
	End Sub

	' Token: 0x060018EE RID: 6382 RVA: 0x000E2070 File Offset: 0x000E0470
	Private Iterator Function cannonActive_cr() As IEnumerator
		While True
			While Not Me.IsActive
				Yield Nothing
			End While
			MyBase.animator.SetBool("Moving", True)
			MyBase.animator.SetFloat("BaseSpeed", ChessQueenLevelCannon.BaseRotationDuration / Me.rotationTime)
			MyBase.animator.SetTrigger("WickIgnite")
			Me.SFX_KOG_QUEEN_CannonFuseLoop()
			While Me.IsActive
				For Each collider2D As Collider2D In Me.parryColliders
					If Me.queen.activeLightning Is Nothing OrElse Me.queen.activeLightning.isGone Then
						collider2D.enabled = True
					Else
						collider2D.enabled = Mathf.Abs(collider2D.transform.position.x + collider2D.offset.x - Me.queen.activeLightning.transform.position.x) > Me.queen.lightningDisableRange
					End If
				Next
				Dim animTime As Single = MyBase.animator.GetCurrentAnimatorStateInfo(ChessQueenLevelCannon.BaseAnimatorLayer).normalizedTime Mod 1F
				If Me.mouseReverses Then
					If Me.cannonPosition = ChessQueenLevelCannon.CannonPosition.Side Then
						Me.mouseAnimator.SetBool("Reverse", animTime > 0.4F AndAlso animTime <= 0.9F)
					Else
						Me.mouseAnimator.SetBool("Reverse", animTime > 0.5F <> Me.baseRenderer.flipX)
					End If
				End If
				Yield Nothing
			End While
			MyBase.animator.SetBool("Moving", False)
		End While
		Return
	End Function

	' Token: 0x060018EF RID: 6383 RVA: 0x000E208C File Offset: 0x000E048C
	Private Sub shootCannonball()
		If Me.IsActive Then
			MyBase.animator.SetTrigger("CannonBlast")
			MyBase.animator.SetTrigger("WickBlast")
			Me.SFX_KOG_QUEEN_CannonShoot()
			Me.SFX_KOG_QUEEN_CannonFuseLoopStop()
			Me.SetActive(False)
			MyBase.StartCoroutine(Me.shoot_cr())
		End If
	End Sub

	' Token: 0x060018F0 RID: 6384 RVA: 0x000E20E4 File Offset: 0x000E04E4
	Private Iterator Function shoot_cr() As IEnumerator
		Me.wickFollowsParabola = False
		Me.wickTransform.parent = Me.wickBlastPositionerTransform
		Me.blastFXTransform.position = Me.blastFXSpawnPoint.position
		Me.blastFXTransform.eulerAngles = Me.barrelTransform.eulerAngles
		MyBase.animator.Play(If((Not CType(Level.Current, ChessQueenLevel).cannonBlastFXVariant), "BlastFXB", "BlastFXA"), ChessQueenLevelCannon.BlastFXAnimatorLayer, 0F)
		CType(Level.Current, ChessQueenLevel).cannonBlastFXVariant = Not CType(Level.Current, ChessQueenLevel).cannonBlastFXVariant
		MyBase.animator.Update(0F)
		While MyBase.animator.GetCurrentAnimatorStateInfo(ChessQueenLevelCannon.CannonAnimatorLayer).normalizedTime < 0.7F
			Yield Nothing
		End While
		MyBase.animator.SetFloat("BaseSpeed", ChessQueenLevelCannon.BaseRotationDuration / Me.rotationTime)
		MyBase.animator.SetBool("Moving", False)
		Me.wickTransform.parent = MyBase.transform
		Me.wickFollowsParabola = True
		Me.wickTransform.localEulerAngles = Vector3.zero
		Return
	End Function

	' Token: 0x060018F1 RID: 6385 RVA: 0x000E20FF File Offset: 0x000E04FF
	Private Sub animationEvent_CannonFinishedCycle()
		If Me.cannonPosition = ChessQueenLevelCannon.CannonPosition.Center Then
			Me.baseRenderer.flipX = Not Me.baseRenderer.flipX
			Me.baseTopperRenderer.flipX = Me.baseRenderer.flipX
		End If
	End Sub

	' Token: 0x060018F2 RID: 6386 RVA: 0x000E213C File Offset: 0x000E053C
	Private Sub animationEvent_FireBullet()
		If Level.Current.Ending Then
			Return
		End If
		Me.looseMouse.CannonFired(Me.cannonBall.Create(Me.bulletSpawnPoint.transform.position, MathUtils.DirectionToAngle(Me.barrelTransform.up), Me.properties.turretCannonballSpeed).gameObject)
	End Sub

	' Token: 0x060018F3 RID: 6387 RVA: 0x000E21AC File Offset: 0x000E05AC
	Private Sub setupWickParabola()
		Me.wickStartPosition = Me.wickTransform.position
		Dim vector As Vector3 = Me.wickParabolaEndTransform.position - Me.wickStartPosition
		Me.wickParabolaParameter = vector.x * vector.x / (4F * vector.y)
		Me.wickParametricDuration = vector.x / (2F * Me.wickParabolaParameter)
	End Sub

	' Token: 0x060018F4 RID: 6388 RVA: 0x000E2220 File Offset: 0x000E0620
	Private Sub SFX_KOG_QUEEN_CannonShoot()
		AudioManager.[Stop]("sfx_DLC_KOG_Queen_CannonFuse_Loop")
		AudioManager.Play("sfx_DLC_KOG_Queen_CannonShoot")
		AudioManager.Pan("sfx_DLC_KOG_Queen_CannonShoot", If((Mathf.Abs(MyBase.transform.position.x) <= 100F), 0F, Mathf.Sign(MyBase.transform.position.x)))
	End Sub

	' Token: 0x060018F5 RID: 6389 RVA: 0x000E2290 File Offset: 0x000E0690
	Private Sub SFX_KOG_QUEEN_CannonFuseLoop()
		AudioManager.PlayLoop("sfx_DLC_KOG_Queen_CannonFuse_Loop")
		AudioManager.Pan("sfx_DLC_KOG_Queen_CannonFuse_Loop", If((Mathf.Abs(MyBase.transform.position.x) <= 100F), 0F, Mathf.Sign(MyBase.transform.position.x)))
	End Sub

	' Token: 0x060018F6 RID: 6390 RVA: 0x000E22F5 File Offset: 0x000E06F5
	Private Sub SFX_KOG_QUEEN_CannonFuseLoopStop()
		AudioManager.[Stop]("sfx_DLC_KOG_Queen_CannonFuse_Loop")
	End Sub

	' Token: 0x040021EA RID: 8682
	Private Shared BaseRotationDuration As Single = 0.625F

	' Token: 0x040021EB RID: 8683
	Private Shared BaseAnimatorLayer As Integer

	' Token: 0x040021EC RID: 8684
	Private Shared CannonAnimatorLayer As Integer = 1

	' Token: 0x040021ED RID: 8685
	Private Shared BlastFXAnimatorLayer As Integer = 3

	' Token: 0x040021EE RID: 8686
	<SerializeField()>
	Private cannonBall As ChessQueenLevelCannonball

	' Token: 0x040021EF RID: 8687
	<SerializeField()>
	Private parry As ParrySwitch()

	' Token: 0x040021F0 RID: 8688
	<SerializeField()>
	Private baseRenderer As SpriteRenderer

	' Token: 0x040021F1 RID: 8689
	<SerializeField()>
	Private barrelHighlightRenderer As SpriteRenderer

	' Token: 0x040021F2 RID: 8690
	<SerializeField()>
	Private baseTopperRenderer As SpriteRenderer

	' Token: 0x040021F3 RID: 8691
	<SerializeField()>
	Private barrelTransform As Transform

	' Token: 0x040021F4 RID: 8692
	<SerializeField()>
	Private bulletSpawnPoint As Transform

	' Token: 0x040021F5 RID: 8693
	<SerializeField()>
	Private blastFXSpawnPoint As Transform

	' Token: 0x040021F6 RID: 8694
	<SerializeField()>
	Private blastFXTransform As Transform

	' Token: 0x040021F7 RID: 8695
	<SerializeField()>
	Private baseSprites As Sprite()

	' Token: 0x040021F8 RID: 8696
	<SerializeField()>
	Private barrelHighlightSprites As Sprite()

	' Token: 0x040021F9 RID: 8697
	<SerializeField()>
	Private wickTransform As Transform

	' Token: 0x040021FA RID: 8698
	<SerializeField()>
	Private wickParabolaEndTransform As Transform

	' Token: 0x040021FB RID: 8699
	<SerializeField()>
	Private wickBlastPositionerTransform As Transform

	' Token: 0x040021FC RID: 8700
	<SerializeField()>
	Private mouseAnimator As Animator

	' Token: 0x040021FD RID: 8701
	<SerializeField()>
	Private looseMouse As ChessQueenLevelLooseMouse

	' Token: 0x040021FE RID: 8702
	<SerializeField()>
	Private mouseReverses As Boolean

	' Token: 0x04002200 RID: 8704
	Private parryColliders As Collider2D()

	' Token: 0x04002201 RID: 8705
	Private properties As LevelProperties.ChessQueen.Turret

	' Token: 0x04002202 RID: 8706
	Private rotationTime As Single

	' Token: 0x04002203 RID: 8707
	Private minAngle As Single

	' Token: 0x04002204 RID: 8708
	Private maxAngle As Single

	' Token: 0x04002205 RID: 8709
	Private cannonPosition As ChessQueenLevelCannon.CannonPosition

	' Token: 0x04002206 RID: 8710
	Private queen As ChessQueenLevelQueen

	' Token: 0x04002207 RID: 8711
	Private mouseLookTime As Single

	' Token: 0x04002208 RID: 8712
	Private wickFollowsParabola As Boolean = True

	' Token: 0x04002209 RID: 8713
	Private wickStartPosition As Vector3

	' Token: 0x0400220A RID: 8714
	Private wickParabolaParameter As Single

	' Token: 0x0400220B RID: 8715
	Private wickParametricDuration As Single

	' Token: 0x02000548 RID: 1352
	Public Enum CannonPosition
		' Token: 0x0400220D RID: 8717
		Side
		' Token: 0x0400220E RID: 8718
		Center
	End Enum
End Class
