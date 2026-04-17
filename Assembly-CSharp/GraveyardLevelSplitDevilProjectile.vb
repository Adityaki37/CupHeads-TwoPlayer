Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006CB RID: 1739
Public Class GraveyardLevelSplitDevilProjectile
	Inherits BasicProjectileContinuesOnLevelEnd

	' Token: 0x06002504 RID: 9476 RVA: 0x0015B3D4 File Offset: 0x001597D4
	Public Function Create(position As Vector2, rotation As Single, speed As Single, devil As GraveyardLevelSplitDevil) As GraveyardLevelSplitDevilProjectile
		Dim graveyardLevelSplitDevilProjectile As GraveyardLevelSplitDevilProjectile = TryCast(MyBase.Create(position, rotation, speed), GraveyardLevelSplitDevilProjectile)
		graveyardLevelSplitDevilProjectile.devil = devil
		graveyardLevelSplitDevilProjectile.animator.SetInteger("FireVariant", Global.UnityEngine.Random.Range(0, 3))
		graveyardLevelSplitDevilProjectile.animator.SetInteger("LightVariant", Global.UnityEngine.Random.Range(0, 2))
		graveyardLevelSplitDevilProjectile.SetBool("IsFire", Not devil.isAngel)
		graveyardLevelSplitDevilProjectile.coll.enabled = Not devil.isAngel
		graveyardLevelSplitDevilProjectile.UpdateFade(2F)
		graveyardLevelSplitDevilProjectile.StartCoroutine(graveyardLevelSplitDevilProjectile.spawn_fx_cr())
		Return graveyardLevelSplitDevilProjectile
	End Function

	' Token: 0x06002505 RID: 9477 RVA: 0x0015B46C File Offset: 0x0015986C
	Private Iterator Function spawn_fx_cr() As IEnumerator
		Me.fxAngle = CSng(Global.UnityEngine.Random.Range(0, 360))
		While Not Me.dead AndAlso Not Me.impacted
			Yield CupheadTime.WaitForSeconds(Me, Me.fxSpawnDelay)
			Dim count As Integer = 1
			If Me.fxSpawnDelay < CupheadTime.Delta Then
				count = CInt((CupheadTime.Delta / Me.fxSpawnDelay))
			End If
			For i As Integer = 0 To count - 1
				Dim effect As Effect = If((Not Me.devil.isAngel), Me.fireFX.Create(MyBase.transform.position + MathUtils.AngleToDirection(Me.fxAngle) * Me.fxDistanceRange.RandomFloat()), Me.lightFX.Create(MyBase.transform.position + MathUtils.AngleToDirection(Me.fxAngle) * Me.fxDistanceRange.RandomFloat()))
				If Not Me.devil.isAngel Then
					effect.transform.eulerAngles = New Vector3(0F, 0F, MyBase.transform.eulerAngles.z + 50F)
				End If
				Me.fxAngle = (Me.fxAngle + Me.fxAngleShiftRange.RandomFloat()) Mod 360F
			Next
		End While
		Return
	End Function

	' Token: 0x170003B9 RID: 953
	' (get) Token: 0x06002506 RID: 9478 RVA: 0x0015B487 File Offset: 0x00159887
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return True
		End Get
	End Property

	' Token: 0x06002507 RID: 9479 RVA: 0x0015B48C File Offset: 0x0015988C
	Protected Overrides Sub Update()
		If Not Me.impacted Then
			MyBase.Update()
		End If
		If Not Me.dead Then
			Me.coll.enabled = Not Me.devil.isAngel
		End If
		If Not Me.impacted Then
			If MyBase.animator.GetBool("IsFire") = Me.devil.isAngel Then
				MyBase.animator.Play("LightTransition" + Global.UnityEngine.Random.Range(0, 3).ToString(), 2, 0F)
			End If
			MyBase.animator.SetBool("IsFire", Not Me.devil.isAngel)
			Me.frameTimer += CupheadTime.Delta
			While Me.frameTimer > 0.041666668F
				Me.frameTimer -= 0.041666668F
				Me.UpdateFade(0.25F)
			End While
		End If
		If Not Me.impacted AndAlso Mathf.Abs(MyBase.transform.position.x) < 550F AndAlso MyBase.transform.position.y < -297F Then
			Me.impacted = True
			Me.Speed = 0F
			MyBase.transform.eulerAngles = Vector3.zero
			Me.fireRend(0).transform.eulerAngles = Vector3.zero
			Me.lightRend(0).transform.eulerAngles = Vector3.zero
			MyBase.animator.Play(If((Not Rand.Bool()), "ImpactB", "ImpactA"), If(Me.devil.isAngel, 1, 0))
			Me.UpdateFade(2F)
			Me.Die()
		End If
	End Sub

	' Token: 0x06002508 RID: 9480 RVA: 0x0015B678 File Offset: 0x00159A78
	Private Sub UpdateFade(amount As Single)
		For Each spriteRenderer As SpriteRenderer In Me.fireRend
			spriteRenderer.color = New Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Clamp(spriteRenderer.color.a + If((Not Me.coll.enabled), (-amount), amount), 0F, 1F))
		Next
		For Each spriteRenderer2 As SpriteRenderer In Me.lightRend
			spriteRenderer2.color = New Color(spriteRenderer2.color.r, spriteRenderer2.color.g, spriteRenderer2.color.b, Mathf.Clamp(spriteRenderer2.color.a + If((Not Me.coll.enabled), amount, If((Not(spriteRenderer2.gameObject.name = "Ring")), (-amount), (-amount * 0.7F))), 0F, 1F))
		Next
	End Sub

	' Token: 0x06002509 RID: 9481 RVA: 0x0015B7D6 File Offset: 0x00159BD6
	Protected Overrides Sub Die()
		Me.dead = True
		Me.coll.enabled = False
	End Sub

	' Token: 0x04002DB2 RID: 11698
	Private devil As GraveyardLevelSplitDevil

	' Token: 0x04002DB3 RID: 11699
	<SerializeField()>
	Private fireRend As SpriteRenderer()

	' Token: 0x04002DB4 RID: 11700
	<SerializeField()>
	Private lightRend As SpriteRenderer()

	' Token: 0x04002DB5 RID: 11701
	<SerializeField()>
	Private fireFX As Effect

	' Token: 0x04002DB6 RID: 11702
	<SerializeField()>
	Private lightFX As Effect

	' Token: 0x04002DB7 RID: 11703
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x04002DB8 RID: 11704
	Private frameTimer As Single

	' Token: 0x04002DB9 RID: 11705
	Private dead As Boolean

	' Token: 0x04002DBA RID: 11706
	Private impacted As Boolean

	' Token: 0x04002DBB RID: 11707
	<SerializeField()>
	Private fxSpawnDelay As Single = 0.15F

	' Token: 0x04002DBC RID: 11708
	<SerializeField()>
	Private fxAngleShiftRange As MinMax = New MinMax(60F, 300F)

	' Token: 0x04002DBD RID: 11709
	<SerializeField()>
	Private fxDistanceRange As MinMax = New MinMax(0F, 20F)

	' Token: 0x04002DBE RID: 11710
	Private fxAngle As Single
End Class
