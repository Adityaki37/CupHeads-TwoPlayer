Imports System
Imports UnityEngine

' Token: 0x02000B1F RID: 2847
Public Class SpriteErrorManager
	Inherits AbstractMonoBehaviour

	' Token: 0x060044EE RID: 17646 RVA: 0x00247518 File Offset: 0x00245918
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		SpriteErrorManager.Pair.InitializePairs(Me.errors)
	End Sub

	' Token: 0x060044EF RID: 17647 RVA: 0x00247538 File Offset: 0x00245938
	Private Sub OnWillRenderObject()
		For Each pair As SpriteErrorManager.Pair In Me.errors
			Dim name As String = Me.spriteRenderer.sprite.name
			If name = pair.name Then
				If Me.lastFrame = name OrElse pair.chance > Global.UnityEngine.Random.Range(1, 101) Then
					Me.spriteRenderer.sprite = pair.sprite
					Me.lastFrame = name
				End If
				Return
			End If
			Me.lastFrame = String.Empty
		Next
	End Sub

	' Token: 0x04004AC3 RID: 19139
	Public Const ERROR_STRING As String = "_error"

	' Token: 0x04004AC4 RID: 19140
	<SerializeField()>
	Private errors As SpriteErrorManager.Pair()

	' Token: 0x04004AC5 RID: 19141
	Private lastFrame As String

	' Token: 0x04004AC6 RID: 19142
	Private spriteRenderer As SpriteRenderer

	' Token: 0x02000B20 RID: 2848
	<Serializable()>
	Public Class Pair
		' Token: 0x060044F1 RID: 17649 RVA: 0x002475E0 File Offset: 0x002459E0
		Public Shared Sub InitializePairs(p As SpriteErrorManager.Pair())
			For i As Integer = 0 To p.Length - 1
				p(i).name = p(i).sprite.name.Replace("_error", String.Empty)
			Next
		End Sub

		' Token: 0x04004AC7 RID: 19143
		Public sprite As Sprite

		' Token: 0x04004AC8 RID: 19144
		<Range(1F, 100F)>
		Public chance As Integer = 10

		' Token: 0x04004AC9 RID: 19145
		<HideInInspector()>
		Public name As String
	End Class
End Class
