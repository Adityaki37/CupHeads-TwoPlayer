Imports System
Imports UnityEngine

' Token: 0x02000881 RID: 2177
Public Class ForestPlatformingLevelLobberProjectile
	Inherits BasicProjectile

	' Token: 0x1700043B RID: 1083
	' (get) Token: 0x0600328A RID: 12938 RVA: 0x001D66D9 File Offset: 0x001D4AD9
	Protected Overrides ReadOnly Property DestroyedAfterLeavingScreen As Boolean
		Get
			Return False
		End Get
	End Property

	' Token: 0x0600328B RID: 12939 RVA: 0x001D66DC File Offset: 0x001D4ADC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.SetScale(New Single?(CSng(If((Not MathUtils.RandomBool()), (-1), 1))), Nothing, Nothing)
		MyBase.animator.Play("A", 0, Global.UnityEngine.Random.Range(0F, 1F))
	End Sub

	' Token: 0x0600328C RID: 12940 RVA: 0x001D6744 File Offset: 0x001D4B44
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Dim component As LevelPlatform = hit.GetComponent(Of LevelPlatform)()
		If component Is Nothing OrElse (Not component.canFallThrough AndAlso Mathf.Abs(Me._accumulativeGravity) > MyBase.transform.right.y * Me.Speed) Then
			Me.explode()
		End If
	End Sub

	' Token: 0x0600328D RID: 12941 RVA: 0x001D67A8 File Offset: 0x001D4BA8
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
		Dim component As LevelPlatform = hit.GetComponent(Of LevelPlatform)()
		If component IsNot Nothing AndAlso Not component.canFallThrough AndAlso Mathf.Abs(Me._accumulativeGravity) > MyBase.transform.right.y * Me.Speed Then
			Me.explode()
		End If
	End Sub

	' Token: 0x0600328E RID: 12942 RVA: 0x001D680B File Offset: 0x001D4C0B
	Private Sub explode()
		If Not MyBase.dead Then
			Me.explosionPrefab.Create(MyBase.transform.position)
			Me.Die()
		End If
	End Sub

	' Token: 0x0600328F RID: 12943 RVA: 0x001D6835 File Offset: 0x001D4C35
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003290 RID: 12944 RVA: 0x001D6848 File Offset: 0x001D4C48
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.explosionPrefab = Nothing
	End Sub

	' Token: 0x04003ADE RID: 15070
	<SerializeField()>
	Private explosionPrefab As ForestPlatformingLevelLobberProjectileExplosion
End Class
