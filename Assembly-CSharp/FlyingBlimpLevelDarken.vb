Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000632 RID: 1586
Public Class FlyingBlimpLevelDarken
	Inherits AbstractPausableComponent

	' Token: 0x0600207A RID: 8314 RVA: 0x0012B65B File Offset: 0x00129A5B
	Private Sub Update()
		Me.children = MyBase.transform.GetComponentsInChildren(Of SpriteRenderer)()
		If Me.blimpLady.fading AndAlso Not Me.startedFade Then
			Me.startedFade = True
			Me.StartFade()
		End If
	End Sub

	' Token: 0x0600207B RID: 8315 RVA: 0x0012B696 File Offset: 0x00129A96
	Private Sub StartFade()
		MyBase.StartCoroutine(Me.fade_cr())
	End Sub

	' Token: 0x0600207C RID: 8316 RVA: 0x0012B6A8 File Offset: 0x00129AA8
	Private Iterator Function fade_cr() As IEnumerator
		Dim t As Single = 0F
		Dim fadeTime As Single = 0.005F
		Dim fadeVal As Single = 1F
		While t < fadeTime AndAlso fadeVal > Me.fadeMax
			For i As Integer = 0 To Me.children.Length - 1
				If Me.children(i).transform IsNot Nothing Then
					Me.children(i).color = New Color(fadeVal - t / fadeTime, fadeVal - t / fadeTime, fadeVal - t / fadeTime, 1F)
				End If
			Next
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.dark_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x0600207D RID: 8317 RVA: 0x0012B6C4 File Offset: 0x00129AC4
	Private Iterator Function dark_cr() As IEnumerator
		While True
			For i As Integer = 0 To Me.children.Length - 1
				If Me.children(i).transform IsNot Nothing Then
					Me.children(i).color = New Color(Me.fadeMax, Me.fadeMax, Me.fadeMax, 1F)
				End If
			Next
			If Not Me.blimpLady.fading Then
				Exit For
			End If
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.light_cr())
		Return
		Return
	End Function

	' Token: 0x0600207E RID: 8318 RVA: 0x0012B6E0 File Offset: 0x00129AE0
	Private Iterator Function light_cr() As IEnumerator
		Dim fadeMid As Single = 0.87F
		While Me.startedFade
			For i As Integer = 0 To Me.children.Length - 1
				If Me.children(i) IsNot Nothing AndAlso Me.children(i).transform IsNot Nothing Then
					Me.children(i).color = New Color(fadeMid, fadeMid, fadeMid, 1F)
				End If
			Next
			If Me.blimpLady.state = FlyingBlimpLevelBlimpLady.State.Idle Then
				For j As Integer = 0 To Me.children.Length - 1
					If Me.children(j).transform IsNot Nothing Then
						Me.children(j).color = New Color(1F, 1F, 1F, 1F)
					End If
				Next
				Me.startedFade = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040028FB RID: 10491
	<SerializeField()>
	Private blimpLady As FlyingBlimpLevelBlimpLady

	' Token: 0x040028FC RID: 10492
	Private children As SpriteRenderer()

	' Token: 0x040028FD RID: 10493
	Private fadeMax As Single = 0.75F

	' Token: 0x040028FE RID: 10494
	Private startedFade As Boolean
End Class
