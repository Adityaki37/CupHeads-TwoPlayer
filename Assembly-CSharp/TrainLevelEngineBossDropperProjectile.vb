Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000813 RID: 2067
Public Class TrainLevelEngineBossDropperProjectile
	Inherits AbstractProjectile

	' Token: 0x06002FEC RID: 12268 RVA: 0x001C59C8 File Offset: 0x001C3DC8
	Public Function Create(pos As Vector2, upSpeed As Single, xSpeed As Single, gravity As Single) As TrainLevelEngineBossDropperProjectile
		Dim trainLevelEngineBossDropperProjectile As TrainLevelEngineBossDropperProjectile = Me.InstantiatePrefab(Of TrainLevelEngineBossDropperProjectile)()
		trainLevelEngineBossDropperProjectile.Init(pos, upSpeed, xSpeed, gravity)
		Return trainLevelEngineBossDropperProjectile
	End Function

	' Token: 0x06002FED RID: 12269 RVA: 0x001C59E8 File Offset: 0x001C3DE8
	Private Sub Init(pos As Vector2, upSpeed As Single, xSpeed As Single, gravity As Single)
		MyBase.transform.position = pos
		Me.velocity.y = upSpeed
		Me.velocity.x = xSpeed
		Me.gravity = gravity
		MyBase.transform.localScale = Vector3.one * 0.5F
		MyBase.StartCoroutine(Me.go_cr())
		MyBase.StartCoroutine(Me.scale_cr())
	End Sub

	' Token: 0x06002FEE RID: 12270 RVA: 0x001C5A5C File Offset: 0x001C3E5C
	Private Iterator Function scale_cr() As IEnumerator
		Dim t As Single = 0F
		While t < 0.4F
			MyBase.transform.localScale = Vector3.Lerp(Vector3.one * 0.5F, Vector3.one, t / 0.4F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002FEF RID: 12271 RVA: 0x001C5A78 File Offset: 0x001C3E78
	Private Iterator Function go_cr() As IEnumerator
		Dim target As AbstractPlayerController = PlayerManager.GetNext()
		While MyBase.transform.position.y > target.center.y
			Dim vel As Vector3 = Vector3.zero
			MyBase.transform.AddPosition(0F, Me.velocity.y * CupheadTime.Delta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.Delta
			Yield Nothing
			If target Is Nothing OrElse target.IsDead Then
				target = PlayerManager.GetNext()
			End If
		End While
		Dim direction As Integer = If((target.center.x <= MyBase.transform.position.x), (-1), 1)
		MyBase.transform.localScale = New Vector3(CSng((-CSng(direction))), 1F, 1F)
		MyBase.animator.SetTrigger("Horizontal")
		Me.dustFX.Create(MyBase.transform.position, New Vector3(CSng(direction), 1F, 1F)).Play()
		Me.verticalCollider.enabled = False
		Me.horizontalCollider.enabled = True
		While True
			MyBase.transform.AddPosition(CSng(direction) * Me.velocity.x * CupheadTime.Delta, 0F, 0F)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002FF0 RID: 12272 RVA: 0x001C5A93 File Offset: 0x001C3E93
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
			Me.Die()
		End If
	End Sub

	' Token: 0x06002FF1 RID: 12273 RVA: 0x001C5AB7 File Offset: 0x001C3EB7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.dustFX = Nothing
	End Sub

	' Token: 0x040038C3 RID: 14531
	Private Const HorizontalParameterName As String = "Horizontal"

	' Token: 0x040038C4 RID: 14532
	Private Const ScaleTime As Single = 0.4F

	' Token: 0x040038C5 RID: 14533
	Private Const StartScale As Single = 0.5F

	' Token: 0x040038C6 RID: 14534
	<SerializeField()>
	Private dustFX As Effect

	' Token: 0x040038C7 RID: 14535
	<SerializeField()>
	Private verticalCollider As CircleCollider2D

	' Token: 0x040038C8 RID: 14536
	<SerializeField()>
	Private horizontalCollider As BoxCollider2D

	' Token: 0x040038C9 RID: 14537
	Private velocity As Vector2

	' Token: 0x040038CA RID: 14538
	Private gravity As Single
End Class
