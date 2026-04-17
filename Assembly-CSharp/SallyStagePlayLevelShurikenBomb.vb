Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007B8 RID: 1976
Public Class SallyStagePlayLevelShurikenBomb
	Inherits AbstractProjectile

	' Token: 0x06002CA5 RID: 11429 RVA: 0x001A52E0 File Offset: 0x001A36E0
	Public Sub InitShuriken(properties As LevelProperties.SallyStagePlay, direction As Integer, target As AbstractPlayerController)
		Me.boxCollider = MyBase.GetComponent(Of BoxCollider2D)()
		Me.properties = properties
		Me.speed = properties.CurrentState.shuriken.InitialMovementSpeed
		Me.target = target
		Me.isActive = True
		Me.childSpawnCount = 0
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002CA6 RID: 11430 RVA: 0x001A5338 File Offset: 0x001A3738
	Public Sub InitChildShuriken(direction As Integer, childSpawnCount As Integer, target As AbstractPlayerController, properties As LevelProperties.SallyStagePlay)
		Me.properties = properties
		MyBase.GetComponent(Of SpriteRenderer)().sprite = Me.shuriken
		If childSpawnCount > 1 Then
			Me.currentYVelocity = properties.CurrentState.shuriken.ArcTwoVerticalVelocity
			Me.horizontalVelocity = properties.CurrentState.shuriken.ArcTwoHorizontalVelocity * Mathf.Sign(CSng(direction))
			Me.gravity = properties.CurrentState.shuriken.ArcTwoGravity
		Else
			Me.currentYVelocity = properties.CurrentState.shuriken.ArcOneVerticalVelocity
			Me.horizontalVelocity = properties.CurrentState.shuriken.ArcOneHorizontalVelocity * Mathf.Sign(CSng(direction))
			Me.gravity = properties.CurrentState.shuriken.ArcOneGravity
		End If
		Me.target = target
		Me.isActive = True
		Me.childSpawnCount = childSpawnCount
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002CA7 RID: 11431 RVA: 0x001A5428 File Offset: 0x001A3828
	Private Iterator Function move_cr() As IEnumerator
		If Me.target Is Nothing OrElse Me.target.IsDead Then
			Me.target = PlayerManager.GetNext()
		End If
		Dim direction As Vector3 = (New Vector3(Me.target.center.x, CSng(Level.Current.Ground), 0F) - MyBase.transform.position).normalized
		While True
			If Me.boxCollider IsNot Nothing Then
				Me.boxCollider.enabled = True
			End If
			If Me.childSpawnCount > 0 Then
				MyBase.transform.position += (Vector3.right * Me.horizontalVelocity + Vector3.up * Me.currentYVelocity) * CupheadTime.Delta
				Me.currentYVelocity -= Me.gravity
			Else
				MyBase.transform.position += direction * Me.speed * CupheadTime.Delta
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002CA8 RID: 11432 RVA: 0x001A5444 File Offset: 0x001A3844
	Private Sub Explode()
		MyBase.GetComponent(Of SpriteRenderer)().sprite = Me.explosion
		If Me.childSpawnCount < Me.properties.CurrentState.shuriken.NumberOfChildSpawns Then
			Me.childSpawnCount += 1
			Dim x As Single = MyBase.GetComponent(Of SpriteRenderer)().bounds.size.x
			For i As Integer = -1 To 1 - 1
				Dim abstractProjectile As AbstractProjectile = Me.Create(MyBase.transform.position + Vector3.right * x / 2F * Mathf.Sign(CSng(i)) + Vector3.up * 50F)
				abstractProjectile.GetComponent(Of SallyStagePlayLevelShurikenBomb)().InitChildShuriken(i, Me.childSpawnCount, Me.target, Me.properties)
			Next
		End If
	End Sub

	' Token: 0x06002CA9 RID: 11433 RVA: 0x001A552F File Offset: 0x001A392F
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		If Me.isActive Then
			Me.isActive = False
			Me.Explode()
			Me.StopAllCoroutines()
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject, 0.1F)
		End If
		MyBase.OnCollisionGround(hit, phase)
	End Sub

	' Token: 0x06002CAA RID: 11434 RVA: 0x001A5567 File Offset: 0x001A3967
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x04003526 RID: 13606
	<SerializeField()>
	Private shuriken As Sprite

	' Token: 0x04003527 RID: 13607
	<SerializeField()>
	Private explosion As Sprite

	' Token: 0x04003528 RID: 13608
	Private currentYVelocity As Single

	' Token: 0x04003529 RID: 13609
	Private horizontalVelocity As Single

	' Token: 0x0400352A RID: 13610
	Private gravity As Single

	' Token: 0x0400352B RID: 13611
	Private target As AbstractPlayerController

	' Token: 0x0400352C RID: 13612
	Private speed As Single

	' Token: 0x0400352D RID: 13613
	Private childSpawnCount As Integer

	' Token: 0x0400352E RID: 13614
	Private isActive As Boolean

	' Token: 0x0400352F RID: 13615
	Private properties As LevelProperties.SallyStagePlay

	' Token: 0x04003530 RID: 13616
	Private boxCollider As BoxCollider2D
End Class
