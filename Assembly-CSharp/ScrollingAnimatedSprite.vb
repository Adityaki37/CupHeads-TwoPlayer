Imports System
Imports UnityEngine

' Token: 0x02000B17 RID: 2839
Public Class ScrollingAnimatedSprite
	Inherits AbstractPausableComponent

	' Token: 0x060044CC RID: 17612 RVA: 0x00246ABC File Offset: 0x00244EBC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If ScrollingAnimatedSprite.copying Then
			Return
		End If
		ScrollingAnimatedSprite.copying = True
		Dim component As SpriteRenderer = MyBase.transform.GetComponent(Of SpriteRenderer)()
		Me.size = If((Me.axis <> ScrollingAnimatedSprite.Axis.X), CInt(component.sprite.bounds.size.y), CInt(component.sprite.bounds.size.x)) - Me.offset
		For i As Integer = 0 To Me.count - 1
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject)
			Dim gameObject2 As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(MyBase.gameObject)
			gameObject.GetComponent(Of ScrollingAnimatedSprite)().enabled = False
			gameObject2.GetComponent(Of ScrollingAnimatedSprite)().enabled = False
			gameObject.transform.SetParent(MyBase.transform)
			gameObject2.transform.SetParent(MyBase.transform)
			If Me.axis = ScrollingAnimatedSprite.Axis.X Then
				gameObject.transform.SetLocalPosition(New Single?(CSng((Me.size + Me.size * i))), New Single?(0F), New Single?(0F))
				gameObject2.transform.SetLocalPosition(New Single?(CSng((-CSng((Me.size + Me.size * i))))), New Single?(0F), New Single?(0F))
			Else
				gameObject.transform.SetLocalPosition(New Single?(0F), New Single?(CSng((Me.size + Me.size * i))), New Single?(0F))
				gameObject2.transform.SetLocalPosition(New Single?(0F), New Single?(CSng((-CSng((Me.size + Me.size * i))))), New Single?(0F))
			End If
		Next
		ScrollingAnimatedSprite.copying = False
	End Sub

	' Token: 0x060044CD RID: 17613 RVA: 0x00246CA8 File Offset: 0x002450A8
	Private Sub Update()
		Dim localPosition As Vector3 = MyBase.transform.localPosition
		If Me.axis = ScrollingAnimatedSprite.Axis.X Then
			If localPosition.x <= CSng((-CSng(Me.size))) Then
				localPosition.x += CSng(Me.size)
			End If
			If localPosition.x >= CSng(Me.size) Then
				localPosition.x -= CSng(Me.size)
			End If
			localPosition.x -= CSng(If((Not Me.negativeDirection), 1, (-1))) * Me.speed * CupheadTime.Delta * Me.playbackSpeed
		Else
			If localPosition.y <= CSng((-CSng(Me.size))) Then
				localPosition.y += CSng(Me.size)
			End If
			If localPosition.y >= CSng(Me.size) Then
				localPosition.y -= CSng(Me.size)
			End If
			localPosition.y -= CSng(If((Not Me.negativeDirection), 1, (-1))) * Me.speed * CupheadTime.Delta * Me.playbackSpeed
		End If
		MyBase.transform.localPosition = localPosition
	End Sub

	' Token: 0x04004A85 RID: 19077
	Public axis As ScrollingAnimatedSprite.Axis

	' Token: 0x04004A86 RID: 19078
	<SerializeField()>
	Private negativeDirection As Boolean

	' Token: 0x04004A87 RID: 19079
	<SerializeField()>
	<Range(0F, 2000F)>
	Public speed As Single

	' Token: 0x04004A88 RID: 19080
	<SerializeField()>
	Private offset As Integer

	' Token: 0x04004A89 RID: 19081
	<SerializeField()>
	<Range(1F, 10F)>
	Private count As Integer = 1

	' Token: 0x04004A8A RID: 19082
	<NonSerialized()>
	Public playbackSpeed As Single = 1F

	' Token: 0x04004A8B RID: 19083
	Private size As Integer

	' Token: 0x04004A8C RID: 19084
	Private Shared copying As Boolean

	' Token: 0x02000B18 RID: 2840
	Public Enum Axis
		' Token: 0x04004A8E RID: 19086
		X
		' Token: 0x04004A8F RID: 19087
		Y
	End Enum
End Class
