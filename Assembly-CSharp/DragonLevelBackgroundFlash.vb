Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020005EB RID: 1515
Public Class DragonLevelBackgroundFlash
	Inherits MonoBehaviour

	' Token: 0x06001E0D RID: 7693 RVA: 0x00114B34 File Offset: 0x00112F34
	Public Sub SetFlash1()
		Dim copyRenderers As List(Of SpriteRenderer) = Me.scrollSprite.copyRenderers
		For i As Integer = 0 To copyRenderers.Count - 1
			copyRenderers(i).sprite = Me.flashSprite1
		Next
	End Sub

	' Token: 0x06001E0E RID: 7694 RVA: 0x00114B78 File Offset: 0x00112F78
	Public Sub SetFlash2()
		Dim copyRenderers As List(Of SpriteRenderer) = Me.scrollSprite.copyRenderers
		For i As Integer = 0 To copyRenderers.Count - 1
			copyRenderers(i).sprite = Me.flashSprite2
		Next
	End Sub

	' Token: 0x06001E0F RID: 7695 RVA: 0x00114BBC File Offset: 0x00112FBC
	Public Sub SetNormal()
		Dim copyRenderers As List(Of SpriteRenderer) = Me.scrollSprite.copyRenderers
		For i As Integer = 0 To copyRenderers.Count - 1
			copyRenderers(i).sprite = Me.normalSprite
		Next
	End Sub

	' Token: 0x06001E10 RID: 7696 RVA: 0x00114BFE File Offset: 0x00112FFE
	Private Sub OnDestroy()
		Me.normalSprite = Nothing
		Me.flashSprite1 = Nothing
		Me.flashSprite2 = Nothing
	End Sub

	' Token: 0x040026D8 RID: 9944
	<SerializeField()>
	Private normalSprite As Sprite

	' Token: 0x040026D9 RID: 9945
	<SerializeField()>
	Private flashSprite1 As Sprite

	' Token: 0x040026DA RID: 9946
	<SerializeField()>
	Private flashSprite2 As Sprite

	' Token: 0x040026DB RID: 9947
	<SerializeField()>
	Private scrollSprite As ScrollingSprite
End Class
