Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200069D RID: 1693
Public Class FlyingMermaidLevelSkullBubble
	Inherits BasicSineProjectile

	' Token: 0x060023DF RID: 9183 RVA: 0x00151224 File Offset: 0x0014F624
	Public Function CreateBubble(pos As Vector2, velocity As Single, sinVelocity As Single, sinSize As Single, rotation As Single) As FlyingMermaidLevelSkullBubble
		Return TryCast(MyBase.Create(pos, rotation, velocity, sinVelocity, sinSize), FlyingMermaidLevelSkullBubble)
	End Function

	' Token: 0x060023E0 RID: 9184 RVA: 0x00151245 File Offset: 0x0014F645
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.smallBubbles = New List(Of Effect)()
	End Sub

	' Token: 0x060023E1 RID: 9185 RVA: 0x00151258 File Offset: 0x0014F658
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.spawn_small_bubbles_cr())
	End Sub

	' Token: 0x060023E2 RID: 9186 RVA: 0x00151270 File Offset: 0x0014F670
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Not Me.isDead Then
			If phase <> CollisionPhase.[Exit] Then
				Me.damageDealer.DealDamage(hit)
			End If
			Me.isDead = True
			Me.StopAllCoroutines()
			MyBase.StartCoroutine(Me.dying_cr())
		End If
	End Sub

	' Token: 0x060023E3 RID: 9187 RVA: 0x001512C0 File Offset: 0x0014F6C0
	Private Iterator Function spawn_small_bubbles_cr() As IEnumerator
		While Not Me.isDead
			Dim offset As Vector3 = New Vector3(Global.UnityEngine.Random.Range(-15F, 15F), Global.UnityEngine.Random.Range(-15F, 15F), 0F)
			Me.smallBubbles.Add(Me.smallBubblesPrefab.Create(MyBase.transform.position + offset, New Vector3(Me.smallBubblesSize, Me.smallBubblesSize, Me.smallBubblesSize)))
			Yield CupheadTime.WaitForSeconds(Me, 0.2F)
			Yield Nothing
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060023E4 RID: 9188 RVA: 0x001512DB File Offset: 0x0014F6DB
	Protected Overrides Sub Die()
		MyBase.Die()
	End Sub

	' Token: 0x060023E5 RID: 9189 RVA: 0x001512E4 File Offset: 0x0014F6E4
	Private Iterator Function dying_cr() As IEnumerator
		MyBase.animator.Play("Pop")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Pop", False, True)
		While MyBase.transform.position.y > -660F
			MyBase.transform.AddPosition(0F, (-Me.velocity + Me.accumulatedGravity) * CupheadTime.Delta, 0F)
			Me.accumulatedGravity += -25F
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x04002CAC RID: 11436
	<SerializeField()>
	Private smallBubblesPrefab As Effect

	' Token: 0x04002CAD RID: 11437
	<SerializeField()>
	Private smallBubblesSize As Single

	' Token: 0x04002CAE RID: 11438
	Private Const GRAVITY As Single = -25F

	' Token: 0x04002CAF RID: 11439
	Private Const bubblesOffsetX As Single = 15F

	' Token: 0x04002CB0 RID: 11440
	Private Const bubblesOffsetY As Single = 15F

	' Token: 0x04002CB1 RID: 11441
	Private smallBubbles As List(Of Effect)

	' Token: 0x04002CB2 RID: 11442
	Private accumulatedGravity As Single

	' Token: 0x04002CB3 RID: 11443
	Private isDead As Boolean
End Class
