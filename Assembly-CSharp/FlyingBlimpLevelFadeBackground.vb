Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000637 RID: 1591
Public Class FlyingBlimpLevelFadeBackground
	Inherits ScrollingSprite

	' Token: 0x0600209F RID: 8351 RVA: 0x0012CA56 File Offset: 0x0012AE56
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.FrameDelayedCallback(AddressOf Me.DisableSprites, 1)
	End Sub

	' Token: 0x060020A0 RID: 8352 RVA: 0x0012CA74 File Offset: 0x0012AE74
	Private Sub DisableSprites()
		Me.fadeTime = 10F
		Me.current = MyBase.GetComponent(Of SpriteRenderer)()
		Me.replacementClones = Me.replacementSprite.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		Me.currentClones = Me.current.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		Me.replacementSprite.transform.position = New Vector2(MyBase.transform.position.x, Me.replacementSprite.transform.position.y)
		Me.replacementSprite.gameObject.GetComponent(Of SpriteRenderer)().enabled = False
		For i As Integer = 0 To Me.replacementClones.Length - 1
			Me.replacementClones(i).enabled = False
		Next
	End Sub

	' Token: 0x060020A1 RID: 8353 RVA: 0x0012CB50 File Offset: 0x0012AF50
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.moonLady.state = FlyingBlimpLevelMoonLady.State.Morph AndAlso Not Me.startedChange Then
			Me.startedChange = True
			Me.StartChange()
		End If
	End Sub

	' Token: 0x060020A2 RID: 8354 RVA: 0x0012CB81 File Offset: 0x0012AF81
	Private Sub StartChange()
		MyBase.StartCoroutine(Me.change_cr())
	End Sub

	' Token: 0x060020A3 RID: 8355 RVA: 0x0012CB90 File Offset: 0x0012AF90
	Private Iterator Function change_cr() As IEnumerator
		Dim t As Single = 0F
		Dim alphaValue As Single = 1F
		Dim startSpeed As Single = Me.speed
		Dim endSpeed As Single = Me.speed + Me.speed * 0.3F
		While t < Me.fadeTime
			For j As Integer = 0 To Me.replacementClones.Length - 1
				If Me.replacementClones(j).transform IsNot Nothing Then
					Me.replacementClones(j).enabled = True
					Me.replacementClones(j).color = New Color(1F, 1F, 1F, t / Me.fadeTime)
				End If
			Next
			If Me.fadeOriginal Then
				For i As Integer = 0 To Me.currentClones.Length - 1
					If Me.currentClones(i).transform IsNot Nothing Then
						Me.currentClones(i).color = New Color(1F, 1F, 1F, alphaValue - t / Me.fadeTime)
						If alphaValue <= 0F Then
							Me.currentClones(i).color = New Color(1F, 1F, 1F, 0F)
							Yield Nothing
						End If
					End If
				Next
			End If
			Me.speed = Mathf.Lerp(startSpeed, endSpeed, t / Me.fadeTime)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		For k As Integer = 0 To Me.replacementClones.Length - 1
			Me.replacementClones(k).color = New Color(1F, 1F, 1F, 1F)
		Next
		If Me.fadeOriginal Then
			For l As Integer = 0 To Me.currentClones.Length - 1
				Me.currentClones(l).enabled = False
			Next
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x04002921 RID: 10529
	Public fadeOriginal As Boolean

	' Token: 0x04002922 RID: 10530
	<SerializeField()>
	Private moonLady As FlyingBlimpLevelMoonLady

	' Token: 0x04002923 RID: 10531
	<SerializeField()>
	Private replacementSprite As Transform

	' Token: 0x04002924 RID: 10532
	Private replacementClones As SpriteRenderer()

	' Token: 0x04002925 RID: 10533
	Private current As SpriteRenderer

	' Token: 0x04002926 RID: 10534
	Private currentClones As SpriteRenderer()

	' Token: 0x04002927 RID: 10535
	Private fadeTime As Single

	' Token: 0x04002928 RID: 10536
	Private startedChange As Boolean
End Class
