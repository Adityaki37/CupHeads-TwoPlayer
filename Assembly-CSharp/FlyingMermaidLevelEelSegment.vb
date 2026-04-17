Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000686 RID: 1670
Public Class FlyingMermaidLevelEelSegment
	Inherits AbstractPausableComponent

	' Token: 0x06002337 RID: 9015 RVA: 0x0014ADE4 File Offset: 0x001491E4
	Public Function Create(position As Vector2, sortingLayer As String, sortingOrder As Integer) As FlyingMermaidLevelEelSegment
		Dim flyingMermaidLevelEelSegment As FlyingMermaidLevelEelSegment = Global.UnityEngine.[Object].Instantiate(Of FlyingMermaidLevelEelSegment)(Me)
		flyingMermaidLevelEelSegment.transform.position = position
		flyingMermaidLevelEelSegment.velocity = Me.launchSpeed * MathUtils.AngleToDirection(Me.angleRange.RandomFloat())
		flyingMermaidLevelEelSegment.transform.Rotate(0F, 0F, Global.UnityEngine.Random.Range(0F, 360F))
		If Global.UnityEngine.Random.Range(0F, 1F) > 0.5F Then
			flyingMermaidLevelEelSegment.transform.SetScale(New Single?(-1F), Nothing, Nothing)
		End If
		Dim component As SpriteRenderer = flyingMermaidLevelEelSegment.GetComponent(Of SpriteRenderer)()
		component.sortingLayerName = sortingLayer
		component.sortingOrder = sortingOrder
		flyingMermaidLevelEelSegment.animator.Play("Idle", 0, Global.UnityEngine.Random.Range(0F, 1F))
		flyingMermaidLevelEelSegment.StartCoroutine(flyingMermaidLevelEelSegment.move_cr())
		Return flyingMermaidLevelEelSegment
	End Function

	' Token: 0x06002338 RID: 9016 RVA: 0x0014AED4 File Offset: 0x001492D4
	Private Iterator Function move_cr() As IEnumerator
		While MyBase.transform.position.y > Me.despawnY OrElse Me.velocity.y > 0F
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.Delta
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.Delta, Me.velocity.y * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04002BE2 RID: 11234
	<SerializeField()>
	Private gravity As Single

	' Token: 0x04002BE3 RID: 11235
	<SerializeField()>
	Private angleRange As MinMax

	' Token: 0x04002BE4 RID: 11236
	<SerializeField()>
	Private launchSpeed As Single

	' Token: 0x04002BE5 RID: 11237
	<SerializeField()>
	Private despawnY As Single

	' Token: 0x04002BE6 RID: 11238
	Private velocity As Vector2
End Class
