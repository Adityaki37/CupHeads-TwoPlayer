Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000B1A RID: 2842
Public Class ScrollingSprite
	Inherits AbstractPausableComponent

	' Token: 0x17000628 RID: 1576
	' (get) Token: 0x060044D6 RID: 17622 RVA: 0x00114278 File Offset: 0x00112678
	' (set) Token: 0x060044D7 RID: 17623 RVA: 0x00114280 File Offset: 0x00112680
	Public Property copyRenderers As List(Of SpriteRenderer)

	' Token: 0x17000629 RID: 1577
	' (get) Token: 0x060044D8 RID: 17624 RVA: 0x00114289 File Offset: 0x00112689
	' (set) Token: 0x060044D9 RID: 17625 RVA: 0x00114291 File Offset: 0x00112691
	Public Property looping As Boolean

	' Token: 0x060044DA RID: 17626 RVA: 0x0011429C File Offset: 0x0011269C
	Protected Overridable Sub Start()
		Me.looping = True
		Me.copyRenderers = New List(Of SpriteRenderer)()
		Me.direction = If((Not Me.negativeDirection), 1, (-1))
		Dim component As SpriteRenderer = MyBase.transform.GetComponent(Of SpriteRenderer)()
		Me.copyRenderers.Add(component)
		Me.size = If((Me.axis <> ScrollingSprite.Axis.X), component.sprite.bounds.size.y, component.sprite.bounds.size.x) - Me.offset
		For i As Integer = 0 To Me.count - 1
			Dim gameObject As GameObject = New GameObject(MyBase.gameObject.name + " Copy")
			gameObject.transform.parent = MyBase.transform
			gameObject.transform.ResetLocalTransforms()
			Dim spriteRenderer As SpriteRenderer = gameObject.AddComponent(Of SpriteRenderer)()
			spriteRenderer.sortingLayerID = component.sortingLayerID
			spriteRenderer.sortingOrder = component.sortingOrder
			spriteRenderer.sprite = component.sprite
			spriteRenderer.material = component.material
			Me.copyRenderers.Add(spriteRenderer)
			Dim gameObject2 As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(gameObject)
			gameObject2.transform.parent = MyBase.transform
			gameObject2.transform.ResetLocalTransforms()
			Me.copyRenderers.Add(gameObject2.GetComponent(Of SpriteRenderer)())
			If Me.axis = ScrollingSprite.Axis.X Then
				gameObject.transform.SetLocalPosition(New Single?(CSng(Me.direction) * (Me.size + Me.size * CSng(i))), New Single?(0F), New Single?(0F))
				gameObject2.transform.SetLocalPosition(New Single?(CSng(Me.direction) * -(Me.size + Me.size * CSng(i))), New Single?(0F), New Single?(0F))
			Else
				gameObject.transform.SetLocalPosition(New Single?(0F), New Single?(Me.size + Me.size * CSng(i)), New Single?(0F))
				gameObject2.transform.SetLocalPosition(New Single?(0F), New Single?(-(Me.size + Me.size * CSng(i))), New Single?(0F))
			End If
		Next
		Me.startY = MyBase.transform.localPosition.y
	End Sub

	' Token: 0x060044DB RID: 17627 RVA: 0x0011452C File Offset: 0x0011292C
	Protected Overridable Sub Update()
		Me.pos = MyBase.transform.localPosition
		If Me.axis = ScrollingSprite.Axis.X Then
			If Me.pos.x <= -Me.size AndAlso Me.looping Then
				Me.pos.x = Me.pos.x + Me.size
				If Me.isRotated Then
					Me.pos.y = Me.startY
				End If
				Me.onLoop()
			End If
			If Me.pos.x >= Me.size AndAlso Me.looping Then
				Me.pos.x = Me.pos.x - Me.size
				Me.onLoop()
			End If
			If Not Me.isRotated Then
				Me.pos.x = Me.pos.x - CSng(If((Not Me.negativeDirection), 1, (-1))) * Me.speed * CupheadTime.Delta * Me.playbackSpeed
			End If
		Else
			If Me.pos.y <= -Me.size AndAlso Me.looping Then
				Me.pos.y = Me.pos.y + Me.size
				Me.onLoop()
			End If
			If Me.pos.y >= Me.size AndAlso Me.looping Then
				Me.pos.y = Me.pos.y - Me.size
				Me.onLoop()
			End If
			If Not Me.isRotated Then
				Me.pos.y = Me.pos.y - CSng(If((Not Me.negativeDirection), 1, (-1))) * Me.speed * CupheadTime.Delta * Me.playbackSpeed
			End If
		End If
		If Me.isRotated Then
			Me.pos -= MyBase.transform.right * Me.speed * CupheadTime.Delta
		End If
		MyBase.transform.localPosition = Me.pos
	End Sub

	' Token: 0x060044DC RID: 17628 RVA: 0x00114758 File Offset: 0x00112B58
	Protected Overridable Sub onLoop()
	End Sub

	' Token: 0x060044DD RID: 17629 RVA: 0x0011475A File Offset: 0x00112B5A
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.copyRenderers = Nothing
	End Sub

	' Token: 0x04004A9A RID: 19098
	Public axis As ScrollingSprite.Axis

	' Token: 0x04004A9B RID: 19099
	<SerializeField()>
	Protected negativeDirection As Boolean

	' Token: 0x04004A9C RID: 19100
	<SerializeField()>
	Private onLeft As Boolean

	' Token: 0x04004A9D RID: 19101
	<SerializeField()>
	Private isRotated As Boolean

	' Token: 0x04004A9E RID: 19102
	<Range(0F, 4000F)>
	Public speed As Single

	' Token: 0x04004A9F RID: 19103
	<SerializeField()>
	Public offset As Single

	' Token: 0x04004AA0 RID: 19104
	<SerializeField()>
	<Range(1F, 10F)>
	Private count As Integer = 1

	' Token: 0x04004AA1 RID: 19105
	<NonSerialized()>
	Public playbackSpeed As Single = 1F

	' Token: 0x04004AA2 RID: 19106
	Protected size As Single

	' Token: 0x04004AA3 RID: 19107
	Protected pos As Vector3

	' Token: 0x04004AA4 RID: 19108
	Private startY As Single

	' Token: 0x04004AA5 RID: 19109
	Protected direction As Integer

	' Token: 0x02000B1B RID: 2843
	Public Enum Axis
		' Token: 0x04004AA9 RID: 19113
		X
		' Token: 0x04004AAA RID: 19114
		Y
	End Enum
End Class
