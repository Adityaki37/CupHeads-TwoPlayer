Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007CA RID: 1994
Public Class SaltbakerLevelHeart
	Inherits AbstractProjectile

	' Token: 0x1700040E RID: 1038
	' (get) Token: 0x06002D37 RID: 11575 RVA: 0x001A9ECA File Offset: 0x001A82CA
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06002D38 RID: 11576 RVA: 0x001A9ED4 File Offset: 0x001A82D4
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.ballSize = MyBase.GetComponent(Of Collider2D)().bounds.size.y / 2F
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.impactFX.transform.parent = Nothing
	End Sub

	' Token: 0x06002D39 RID: 11577 RVA: 0x001A9F42 File Offset: 0x001A8342
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002D3A RID: 11578 RVA: 0x001A9F60 File Offset: 0x001A8360
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.parent.TakeDamage(info)
	End Sub

	' Token: 0x06002D3B RID: 11579 RVA: 0x001A9F70 File Offset: 0x001A8370
	Public Sub Init(pos As Vector3, leftPillar As GameObject, rightPillar As GameObject, properties As LevelProperties.Saltbaker.DarkHeart, parent As SaltbakerLevelPillarHandler)
		MyBase.transform.position = pos
		Me.properties = properties
		Me.isMoving = False
		Me.speed = properties.heartSpeed
		Me.parent = parent
		Me.leftPillarColl = leftPillar.GetComponent(Of Collider2D)()
		Me.rightPillarColl = rightPillar.GetComponent(Of Collider2D)()
		Me.SetParryable(True)
		Me.coll.enabled = False
		Me.angleString = New PatternString(properties.angleOffsetString, True, True)
		Me.dir = MathUtils.AngleToDirection(properties.baseAngle)
		MyBase.transform.localScale = New Vector3(-Mathf.Sign(Me.dir.x), 1F)
		Me.lastDirNoOffset = Me.dir
		MyBase.StartCoroutine(Me.warning_cr())
	End Sub

	' Token: 0x06002D3C RID: 11580 RVA: 0x001AA044 File Offset: 0x001A8444
	Private Iterator Function warning_cr() As IEnumerator
		Me.SFX_SALTB_HeartWarning()
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Warning", False, True)
		Me.isMoving = True
		Me.coll.enabled = True
		Return
	End Function

	' Token: 0x06002D3D RID: 11581 RVA: 0x001AA060 File Offset: 0x001A8460
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Me.isMoving Then
			MyBase.transform.position += Me.dir * Me.speed * CupheadTime.Delta
		End If
		Me.CheckBounds()
		If Not Me.isDead Then
			Me.pinkSprite.enabled = MyBase.CanParry
		End If
		If MyBase.CanParry AndAlso Not Me.isDead Then
			Me.pinkSprite.color = New Color(Me.pinkSprite.color.r, Me.pinkSprite.color.g, Me.pinkSprite.color.b, Me.pinkSprite.color.a + Time.deltaTime * 2F)
			Me.regularSprite.color = New Color(Me.regularSprite.color.r, Me.regularSprite.color.g, Me.regularSprite.color.b, Me.regularSprite.color.a - Time.deltaTime * 0.5F)
		End If
	End Sub

	' Token: 0x06002D3E RID: 11582 RVA: 0x001AA1D8 File Offset: 0x001A85D8
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		Me.SetParryable(False)
		Me.pinkSprite.color = New Color(0F, 0F, 0F, 0F)
		Me.regularSprite.color = Color.black
		MyBase.StartCoroutine(Me.coolDown_cr())
		MyBase.StartCoroutine(Me.colliderCoolDown_cr())
	End Sub

	' Token: 0x06002D3F RID: 11583 RVA: 0x001AA23C File Offset: 0x001A863C
	Private Iterator Function coolDown_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.parryTimeOut)
		Me.SetParryable(True)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002D40 RID: 11584 RVA: 0x001AA258 File Offset: 0x001A8658
	Private Iterator Function colliderCoolDown_cr() As IEnumerator
		Me.damageDealer.SetDamageFlags(False, False, False)
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.collisionTimeOut)
		Me.damageDealer.SetDamageFlags(True, False, False)
		Yield Nothing
		Return
	End Function

	' Token: 0x06002D41 RID: 11585 RVA: 0x001AA274 File Offset: 0x001A8674
	Private Sub CheckBounds()
		If Me.lastHit <> SaltbakerLevelHeart.LastHit.Up AndAlso MyBase.transform.position.y > CupheadLevelCamera.Current.Bounds.yMax - Me.ballSize Then
			Me.SetNewDir(True, False)
			Me.lastHit = SaltbakerLevelHeart.LastHit.Up
		ElseIf Me.lastHit <> SaltbakerLevelHeart.LastHit.Down AndAlso MyBase.transform.position.y < CupheadLevelCamera.Current.Bounds.yMin + Me.ballSize Then
			Me.SetNewDir(False, False)
			Me.lastHit = SaltbakerLevelHeart.LastHit.Down
		ElseIf Me.lastHit <> SaltbakerLevelHeart.LastHit.Left AndAlso MyBase.transform.position.x < Me.leftPillarColl.bounds.max.x + Me.ballSize Then
			Me.SetNewDir(False, True)
			Me.lastHit = SaltbakerLevelHeart.LastHit.Left
		ElseIf Me.lastHit <> SaltbakerLevelHeart.LastHit.Right AndAlso MyBase.transform.position.x > Me.rightPillarColl.bounds.min.x - Me.ballSize Then
			Me.SetNewDir(True, True)
			Me.lastHit = SaltbakerLevelHeart.LastHit.Right
		End If
	End Sub

	' Token: 0x06002D42 RID: 11586 RVA: 0x001AA3D8 File Offset: 0x001A87D8
	Private Sub SetNewDir(getMin As Boolean, isX As Boolean)
		Me.angleOffset = Me.angleString.PopFloat()
		Dim vector As Vector3 = Me.lastDirNoOffset
		If getMin Then
			If isX Then
				vector.x = Mathf.Min(vector.x, -vector.x)
				MyBase.StartCoroutine(Me.turn_cr())
			Else
				vector.y = Mathf.Min(vector.y, -vector.y)
			End If
		ElseIf isX Then
			vector.x = Mathf.Max(vector.x, -vector.x)
			MyBase.StartCoroutine(Me.turn_cr())
		Else
			vector.y = Mathf.Max(vector.y, -vector.y)
		End If
		Me.lastDirNoOffset = vector
		Dim num As Single = MathUtils.DirectionToAngle(vector)
		num += Me.angleOffset
		vector = MathUtils.AngleToDirection(num)
		Me.dir = vector
	End Sub

	' Token: 0x06002D43 RID: 11587 RVA: 0x001AA4D4 File Offset: 0x001A88D4
	Private Iterator Function turn_cr() As IEnumerator
		Me.isMoving = False
		MyBase.animator.Play("Turn")
		Me.impactFX.transform.position = MyBase.transform.position
		Me.impactFX.transform.localScale = MyBase.transform.localScale
		Me.impactFX.Play(If((Not Rand.Bool()), "B", "A"), 0, 0F)
		Me.SFX_SALTB_HeartBounce()
		Yield Nothing
		While MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.18181819F
			Yield Nothing
		End While
		Me.isMoving = True
		MyBase.StartCoroutine(Me.turn_fx_cr())
		Return
	End Function

	' Token: 0x06002D44 RID: 11588 RVA: 0x001AA4F0 File Offset: 0x001A88F0
	Private Iterator Function turn_fx_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		Dim fxCount As Integer = Global.UnityEngine.Random.Range(2, 4)
		For i As Integer = 0 To fxCount - 1
			Me.turnFX.Create(pos)
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 0.1F))
		Next
		Return
	End Function

	' Token: 0x06002D45 RID: 11589 RVA: 0x001AA50C File Offset: 0x001A890C
	Private Sub AniEvent_Turn()
		MyBase.transform.localScale = New Vector3(-MyBase.transform.localScale.x, 1F)
	End Sub

	' Token: 0x06002D46 RID: 11590 RVA: 0x001AA544 File Offset: 0x001A8944
	Public Sub Die()
		Me.StopAllCoroutines()
		Me.coll.enabled = False
		Me.isMoving = False
		Me.regularSprite.enabled = False
		Me.pinkSprite.enabled = True
		Me.pinkSprite.color = New Color(0F, 0F, 0F, 1F)
		Me.isDead = True
		MyBase.animator.Play("Death")
		AudioManager.Play("level_explosion_boss_death")
	End Sub

	' Token: 0x06002D47 RID: 11591 RVA: 0x001AA5C7 File Offset: 0x001A89C7
	Private Sub SFX_SALTB_HeartBounce()
		AudioManager.Play("sfx_DLC_Saltbaker_P4_Heart_Bounce")
		Me.emitAudioFromObject.Add("sfx_DLC_Saltbaker_P4_Heart_Bounce")
	End Sub

	' Token: 0x06002D48 RID: 11592 RVA: 0x001AA5E3 File Offset: 0x001A89E3
	Private Sub SFX_SALTB_HeartWarning()
		AudioManager.Play("sfx_DLC_Saltbaker_P4_Heart_Warning")
		Me.emitAudioFromObject.Add("sfx_DLC_Saltbaker_P4_Heart_Warning")
	End Sub

	' Token: 0x040035AD RID: 13741
	<SerializeField()>
	Private pinkSprite As SpriteRenderer

	' Token: 0x040035AE RID: 13742
	<SerializeField()>
	Private regularSprite As SpriteRenderer

	' Token: 0x040035AF RID: 13743
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x040035B0 RID: 13744
	<SerializeField()>
	Private impactFX As Animator

	' Token: 0x040035B1 RID: 13745
	<SerializeField()>
	Private turnFX As Effect

	' Token: 0x040035B2 RID: 13746
	Private ballSize As Single

	' Token: 0x040035B3 RID: 13747
	Private speed As Single

	' Token: 0x040035B4 RID: 13748
	Private angleOffset As Single

	' Token: 0x040035B5 RID: 13749
	Private isMoving As Boolean

	' Token: 0x040035B6 RID: 13750
	Private isDead As Boolean

	' Token: 0x040035B7 RID: 13751
	Private properties As LevelProperties.Saltbaker.DarkHeart

	' Token: 0x040035B8 RID: 13752
	Private parent As SaltbakerLevelPillarHandler

	' Token: 0x040035B9 RID: 13753
	Private damageReceiver As DamageReceiver

	' Token: 0x040035BA RID: 13754
	Private leftPillarColl As Collider2D

	' Token: 0x040035BB RID: 13755
	Private rightPillarColl As Collider2D

	' Token: 0x040035BC RID: 13756
	Private dir As Vector3

	' Token: 0x040035BD RID: 13757
	Private lastDirNoOffset As Vector3

	' Token: 0x040035BE RID: 13758
	Private angleString As PatternString

	' Token: 0x040035BF RID: 13759
	Public lastHit As SaltbakerLevelHeart.LastHit

	' Token: 0x020007CB RID: 1995
	Public Enum LastHit
		' Token: 0x040035C1 RID: 13761
		None
		' Token: 0x040035C2 RID: 13762
		Left
		' Token: 0x040035C3 RID: 13763
		Right
		' Token: 0x040035C4 RID: 13764
		Up
		' Token: 0x040035C5 RID: 13765
		Down
	End Enum
End Class
