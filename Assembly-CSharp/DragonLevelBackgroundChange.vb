Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005EA RID: 1514
Public Class DragonLevelBackgroundChange
	Inherits DragonLevelScrollingSprite

	' Token: 0x06001E07 RID: 7687 RVA: 0x001147AE File Offset: 0x00112BAE
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.FrameDelayedCallback(AddressOf Me.DisableSprites, 1)
	End Sub

	' Token: 0x06001E08 RID: 7688 RVA: 0x001147CC File Offset: 0x00112BCC
	Private Sub DisableSprites()
		Me.fadeTime = 6F
		Me.current = MyBase.GetComponent(Of SpriteRenderer)()
		Me.replacementClones = Me.replacementSprite.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		Me.currentClones = Me.current.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		Me.replacementSprite.transform.position = New Vector2(MyBase.transform.position.x, Me.replacementSprite.transform.position.y)
		Me.replacementSprite.gameObject.GetComponent(Of SpriteRenderer)().enabled = False
		For i As Integer = 0 To Me.replacementClones.Length - 1
			Me.replacementClones(i).enabled = False
		Next
	End Sub

	' Token: 0x06001E09 RID: 7689 RVA: 0x001148A8 File Offset: 0x00112CA8
	Public Sub StartChange()
		MyBase.StartCoroutine(Me.change_cr())
	End Sub

	' Token: 0x06001E0A RID: 7690 RVA: 0x001148B8 File Offset: 0x00112CB8
	Private Iterator Function change_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.fadeTime
			For i As Integer = 0 To Me.replacementClones.Length - 1
				If Me.replacementClones(i).transform IsNot Nothing Then
					Me.replacementClones(i).enabled = True
					Me.replacementClones(i).color = New Color(1F, 1F, 1F, t / Me.fadeTime)
				End If
			Next
			For j As Integer = 0 To Me.currentClones.Length - 1
				Me.currentClones(j).color = New Color(1F, 1F, 1F, 1F - t / Me.fadeTime)
			Next
			t += CupheadTime.Delta
			Yield Nothing
		End While
		For k As Integer = 0 To Me.replacementClones.Length - 1
			Me.replacementClones(k).color = New Color(1F, 1F, 1F, 1F)
		Next
		For l As Integer = 0 To Me.currentClones.Length - 1
			Me.currentClones(l).enabled = False
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x06001E0B RID: 7691 RVA: 0x001148D3 File Offset: 0x00112CD3
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.replacementClones = Nothing
		Me.currentClones = Nothing
	End Sub

	' Token: 0x040026D2 RID: 9938
	<SerializeField()>
	Private replacementSprite As Transform

	' Token: 0x040026D3 RID: 9939
	Private replacementClones As SpriteRenderer()

	' Token: 0x040026D4 RID: 9940
	Private current As SpriteRenderer

	' Token: 0x040026D5 RID: 9941
	Private currentClones As SpriteRenderer()

	' Token: 0x040026D6 RID: 9942
	Private changeStart As Boolean

	' Token: 0x040026D7 RID: 9943
	Private fadeTime As Single
End Class
