Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000553 RID: 1363
Public Class ChessRookLevelRegularCannonball
	Inherits AbstractProjectile

	' Token: 0x0600195C RID: 6492 RVA: 0x000E5E78 File Offset: 0x000E4278
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x0600195D RID: 6493 RVA: 0x000E5E80 File Offset: 0x000E4280
	Public Function Create(position As Vector3, apexHeight As Single, targetDistance As Single, properties As LevelProperties.ChessRook.RegularCannonBall) As ChessRookLevelRegularCannonball
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = position
		Me.apexHeight = apexHeight
		Me.targetDistance = targetDistance
		Me.gravity = properties.cannonGravity
		Me.Move()
		Return Me
	End Function

	' Token: 0x0600195E RID: 6494 RVA: 0x000E5EBC File Offset: 0x000E42BC
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600195F RID: 6495 RVA: 0x000E5EDA File Offset: 0x000E42DA
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001960 RID: 6496 RVA: 0x000E5EE9 File Offset: 0x000E42E9
	Protected Overrides Sub Die()
		Me.Recycle()
	End Sub

	' Token: 0x06001961 RID: 6497 RVA: 0x000E5EF4 File Offset: 0x000E42F4
	Private Iterator Function move_cr() As IEnumerator
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_launch")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_launch")
		Dim endPosY As Single = CSng(Level.Current.Ground)
		Dim x As Single = New Vector3(MyBase.transform.position.x - Me.targetDistance, endPosY).x - MyBase.transform.position.x
		Dim apexDist As Single = Me.apexHeight
		Dim toSqrRootForviY As Single = Mathf.Abs(2F * Me.gravity * apexDist)
		Dim viY As Single = Mathf.Sqrt(toSqrRootForviY)
		Dim timeToApex As Single = Mathf.Abs(viY / Me.gravity)
		Dim toSqrtForTimeToG As Single = Mathf.Abs(2F * (MyBase.transform.position.y + apexDist) / Me.gravity)
		Dim timeToGround As Single = Mathf.Sqrt(toSqrtForTimeToG)
		Dim viX As Single = x / (timeToApex + timeToGround)
		Dim speed As Vector3 = New Vector3(viX, viY)
		Dim stillMoving As Boolean = True
		While stillMoving
			speed += New Vector3(0F, Me.gravity * CupheadTime.FixedDelta)
			MyBase.transform.Translate(speed * CupheadTime.FixedDelta)
			Yield New WaitForFixedUpdate()
			If MyBase.transform.position.y < CSng((Level.Current.Ground + 40)) Then
				stillMoving = False
				Exit While
			End If
		End While
		MyBase.animator.Play("Explode", 1, 0F)
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitground_explode")
		Me.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitground_explode")
		Me.rend.flipX = Rand.Bool()
		Me.coll.enabled = False
		Return
	End Function

	' Token: 0x06001962 RID: 6498 RVA: 0x000E5F10 File Offset: 0x000E4310
	Private Sub LateUpdate()
		Me.shadow.transform.position = New Vector3(MyBase.transform.position.x, CSng(Level.Current.Ground))
		Dim num As Integer = CInt((Mathf.Abs(MyBase.transform.position.y - CSng(Level.Current.Ground)) / Me.maxShadowDistance * CSng(Me.shadowSprites.Length)))
		Me.shadow.enabled = Me.coll.enabled AndAlso num >= 0 AndAlso num < Me.shadowSprites.Length
		If Me.shadow.enabled Then
			Me.shadow.sprite = Me.shadowSprites(num)
		End If
	End Sub

	' Token: 0x04002276 RID: 8822
	Private apexTime As Single

	' Token: 0x04002277 RID: 8823
	Private apexHeight As Single

	' Token: 0x04002278 RID: 8824
	Private targetDistance As Single

	' Token: 0x04002279 RID: 8825
	Private gravity As Single

	' Token: 0x0400227A RID: 8826
	<SerializeField()>
	Private coll As Collider2D

	' Token: 0x0400227B RID: 8827
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x0400227C RID: 8828
	<SerializeField()>
	Private shadow As SpriteRenderer

	' Token: 0x0400227D RID: 8829
	<SerializeField()>
	Private shadowSprites As Sprite()

	' Token: 0x0400227E RID: 8830
	<SerializeField()>
	Private maxShadowDistance As Single = 750F
End Class
