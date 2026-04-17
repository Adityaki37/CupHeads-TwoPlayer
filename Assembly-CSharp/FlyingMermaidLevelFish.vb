Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000687 RID: 1671
Public Class FlyingMermaidLevelFish
	Inherits AbstractProjectile

	' Token: 0x0600233A RID: 9018 RVA: 0x0014B049 File Offset: 0x00149449
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x0600233B RID: 9019 RVA: 0x0014B051 File Offset: 0x00149451
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600233C RID: 9020 RVA: 0x0014B06F File Offset: 0x0014946F
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600233D RID: 9021 RVA: 0x0014B088 File Offset: 0x00149488
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600233E RID: 9022 RVA: 0x0014B0A8 File Offset: 0x001494A8
	Public Function Create(pos As Vector2, properties As LevelProperties.FlyingMermaid.Fish) As FlyingMermaidLevelFish
		Dim flyingMermaidLevelFish As FlyingMermaidLevelFish = TryCast(MyBase.Create(), FlyingMermaidLevelFish)
		flyingMermaidLevelFish.properties = properties
		flyingMermaidLevelFish.transform.position = pos
		flyingMermaidLevelFish.Init()
		Return flyingMermaidLevelFish
	End Function

	' Token: 0x0600233F RID: 9023 RVA: 0x0014B0E0 File Offset: 0x001494E0
	Private Sub Init()
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x06002340 RID: 9024 RVA: 0x0014B0F0 File Offset: 0x001494F0
	Private Iterator Function loop_cr() As IEnumerator
		Dim velocityY As Single = Me.properties.flyingUpSpeed
		While True
			MyBase.transform.AddPosition(-Me.properties.flyingSpeed * CupheadTime.Delta, velocityY * CupheadTime.Delta, 0F)
			velocityY -= Me.properties.flyingGravity * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04002BE7 RID: 11239
	Private properties As LevelProperties.FlyingMermaid.Fish
End Class
