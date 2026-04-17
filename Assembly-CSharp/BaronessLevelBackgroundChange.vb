Imports System
Imports UnityEngine

' Token: 0x020004FA RID: 1274
Public Class BaronessLevelBackgroundChange
	Inherits AbstractPausableComponent

	' Token: 0x06001672 RID: 5746 RVA: 0x000C98F8 File Offset: 0x000C7CF8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim component As SpriteRenderer = MyBase.transform.GetComponent(Of SpriteRenderer)()
		Me.size = If((Me.b_axis <> BaronessLevelBackgroundChange.B_Axis.X), CInt(component.sprite.bounds.size.y), CInt(component.sprite.bounds.size.x)) - Me.b_offset
		Me.getOffset.x = MyBase.transform.position.x
		For i As Integer = 0 To Me.b_count - 1
			Me.copy = New GameObject(MyBase.gameObject.name + " Copy")
			Me.copy.transform.parent = MyBase.transform
			Me.copy.transform.ResetLocalTransforms()
			Dim spriteRenderer As SpriteRenderer = Me.copy.AddComponent(Of SpriteRenderer)()
			spriteRenderer.sortingLayerID = component.sortingLayerID
			spriteRenderer.sortingOrder = component.sortingOrder
			spriteRenderer.sprite = component.sprite
			spriteRenderer.material = component.material
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.copy)
			gameObject.transform.parent = MyBase.transform
			Me.copy.transform.SetLocalPosition(New Single?(CSng((-CSng((Me.size + Me.size * i))))), New Single?(0F), New Single?(0F))
			gameObject.transform.SetLocalPosition(New Single?(CSng((-CSng((Me.size * 2 + Me.size * i))))), New Single?(0F), New Single?(0F))
		Next
	End Sub

	' Token: 0x06001673 RID: 5747 RVA: 0x000C9AC1 File Offset: 0x000C7EC1
	Private Sub OnEnable()
		If Not Me.isClouds AndAlso Me.sprite IsNot Nothing Then
			Me.sprite.GetComponent(Of OneTimeScrollingSprite)().OutCondition = Function() Me.baroness.state = BaronessLevelCastle.State.Dead
		End If
	End Sub

	' Token: 0x06001674 RID: 5748 RVA: 0x000C9AFB File Offset: 0x000C7EFB
	Private Sub OnDisable()
		If Me.sprite IsNot Nothing Then
			Me.sprite.GetComponent(Of OneTimeScrollingSprite)().OutCondition = Nothing
		End If
	End Sub

	' Token: 0x06001675 RID: 5749 RVA: 0x000C9B20 File Offset: 0x000C7F20
	Private Sub Update()
		If Not Me.isClouds AndAlso Me.baroness.state <> BaronessLevelCastle.State.Chase Then
			Return
		End If
		If Not Me.baroness.pauseScrolling Then
			If MyBase.GetComponent(Of ParallaxLayer)() IsNot Nothing Then
				MyBase.GetComponent(Of ParallaxLayer)().enabled = False
			End If
			If Me.sprite IsNot Nothing Then
				Me.sprite.speed = -Me.speed
			End If
			Dim localPosition As Vector3 = MyBase.transform.localPosition
			If localPosition.x >= -(CSng(Me.size) - Me.getOffset.x) Then
				localPosition.x -= CSng(Me.size)
			End If
			If localPosition.x <= CSng(Me.size) - Me.getOffset.x Then
				localPosition.x += CSng(Me.size)
			End If
			localPosition.x -= CSng(If((Not Me.b_negativeDirection), 1, (-1))) * Me.speed * CupheadTime.Delta * Me.b_playbackSpeed
			MyBase.transform.localPosition = localPosition
		ElseIf Me.sprite IsNot Nothing Then
			Me.sprite.GetComponent(Of OneTimeScrollingSprite)().speed = 0F
		End If
	End Sub

	' Token: 0x04001FBC RID: 8124
	Public b_axis As BaronessLevelBackgroundChange.B_Axis

	' Token: 0x04001FBD RID: 8125
	Private size As Integer

	' Token: 0x04001FBE RID: 8126
	Private Const X_OUT As Single = -1280F

	' Token: 0x04001FBF RID: 8127
	<Range(0F, 2000F)>
	Public speed As Single

	' Token: 0x04001FC0 RID: 8128
	<SerializeField()>
	Private isClouds As Boolean

	' Token: 0x04001FC1 RID: 8129
	<SerializeField()>
	Protected b_negativeDirection As Boolean

	' Token: 0x04001FC2 RID: 8130
	<SerializeField()>
	Protected b_offset As Integer

	' Token: 0x04001FC3 RID: 8131
	<SerializeField()>
	<Range(1F, 10F)>
	Protected b_count As Integer = 1

	' Token: 0x04001FC4 RID: 8132
	<SerializeField()>
	Private baroness As BaronessLevelCastle

	' Token: 0x04001FC5 RID: 8133
	<SerializeField()>
	Private sprite As OneTimeScrollingSprite

	' Token: 0x04001FC6 RID: 8134
	<NonSerialized()>
	Public b_playbackSpeed As Single = 1F

	' Token: 0x04001FC7 RID: 8135
	Private copy As GameObject

	' Token: 0x04001FC8 RID: 8136
	Private getOffset As Vector3

	' Token: 0x020004FB RID: 1275
	Public Enum B_Axis
		' Token: 0x04001FCA RID: 8138
		X
		' Token: 0x04001FCB RID: 8139
		Y
	End Enum
End Class
