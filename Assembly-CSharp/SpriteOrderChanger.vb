Imports System
Imports UnityEngine

' Token: 0x02000B21 RID: 2849
Public Class SpriteOrderChanger
	Inherits AbstractMonoBehaviour

	' Token: 0x060044F3 RID: 17651 RVA: 0x0024763B File Offset: 0x00245A3B
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.spriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
	End Sub

	' Token: 0x060044F4 RID: 17652 RVA: 0x0024764F File Offset: 0x00245A4F
	Private Sub Update()
		If Me.t >= Me.frameDelay Then
			Me.t = 0
			Me.spriteRenderer.sortingOrder += Me.change
		End If
		Me.t += 1
	End Sub

	' Token: 0x04004ACA RID: 19146
	Public change As Integer = 1

	' Token: 0x04004ACB RID: 19147
	Public frameDelay As Integer = 2

	' Token: 0x04004ACC RID: 19148
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04004ACD RID: 19149
	Private t As Integer
End Class
