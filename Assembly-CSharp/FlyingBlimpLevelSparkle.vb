Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000640 RID: 1600
Public Class FlyingBlimpLevelSparkle
	Inherits ScrollingSprite

	' Token: 0x060020DA RID: 8410 RVA: 0x0012F722 File Offset: 0x0012DB22
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.FrameDelayedCallback(AddressOf Me.DisableStars, 1)
	End Sub

	' Token: 0x060020DB RID: 8411 RVA: 0x0012F740 File Offset: 0x0012DB40
	Private Sub DisableStars()
		Me.twinkleSprite = MyBase.GetComponent(Of SpriteRenderer)()
		Me.starClones = Me.starSprite.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		Me.twinkleClones = MyBase.gameObject.transform.GetComponentsInChildren(Of SpriteRenderer)()
		Me.starSprite.enabled = False
		Me.twinkleSprite.enabled = False
		Me.change = True
		Me.fadeTime = 0.8F
		For i As Integer = 0 To Me.starClones.Length - 1
			Me.starClones(i).enabled = False
		Next
		For j As Integer = 0 To Me.twinkleClones.Length - 1
			Me.twinkleClones(j).enabled = False
		Next
	End Sub

	' Token: 0x060020DC RID: 8412 RVA: 0x0012F802 File Offset: 0x0012DC02
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.moonLady.state = FlyingBlimpLevelMoonLady.State.Morph AndAlso Me.change Then
			MyBase.StartCoroutine(Me.fadein_cr())
			Me.change = False
		End If
	End Sub

	' Token: 0x060020DD RID: 8413 RVA: 0x0012F83C File Offset: 0x0012DC3C
	Private Iterator Function fadein_cr() As IEnumerator
		Dim t As Single = 0F
		Me.starSprite.enabled = True
		While t < Me.fadeTime
			Me.starSprite.color = New Color(1F, 1F, 1F, t / Me.fadeTime)
			For i As Integer = 0 To Me.starClones.Length - 1
				Me.starClones(i).enabled = True
				Me.starClones(i).color = New Color(1F, 1F, 1F, t / Me.fadeTime)
			Next
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.starSprite.color = New Color(1F, 1F, 1F, 1F)
		For j As Integer = 0 To Me.starClones.Length - 1
			Me.starClones(j).color = New Color(1F, 1F, 1F, 1F)
		Next
		For k As Integer = 0 To Me.twinkleClones.Length - 1
			Me.twinkleClones(k).color = New Color(1F, 1F, 1F, 1F)
		Next
		MyBase.StartCoroutine(Me.twinkle_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060020DE RID: 8414 RVA: 0x0012F858 File Offset: 0x0012DC58
	Private Iterator Function twinkle_cr() As IEnumerator
		Me.twinkleSprite.enabled = True
		For i As Integer = 0 To Me.twinkleClones.Length - 1
			Me.twinkleClones(i).enabled = True
		Next
		While True
			Me.getSecond = Global.UnityEngine.Random.Range(Me.minSecond, Me.maxSecond)
			If Me.fadeIn Then
				Dim t As Single = 0F
				While t < Me.fadeTime
					Me.twinkleSprite.color = New Color(1F, 1F, 1F, t / Me.fadeTime)
					For j As Integer = 0 To Me.twinkleClones.Length - 1
						Me.twinkleClones(j).color = New Color(1F, 1F, 1F, t / Me.fadeTime)
					Next
					t += CupheadTime.Delta
					Yield Nothing
				End While
				Me.twinkleSprite.color = New Color(1F, 1F, 1F, 1F)
				For k As Integer = 0 To Me.twinkleClones.Length - 1
					Me.twinkleClones(k).color = New Color(1F, 1F, 1F, 1F)
				Next
			Else
				Dim t2 As Single = 0F
				While t2 < Me.fadeTime
					Me.twinkleSprite.color = New Color(1F, 1F, 1F, 1F - t2 / Me.fadeTime)
					For l As Integer = 0 To Me.twinkleClones.Length - 1
						Me.twinkleClones(l).color = New Color(1F, 1F, 1F, 1F - t2 / Me.fadeTime)
					Next
					t2 += CupheadTime.Delta
					Yield Nothing
				End While
				Me.twinkleSprite.color = New Color(1F, 1F, 1F, 0F)
				For m As Integer = 0 To Me.twinkleClones.Length - 1
					Me.twinkleClones(m).color = New Color(1F, 1F, 1F, 0F)
				Next
				Yield CupheadTime.WaitForSeconds(Me, Me.getSecond)
			End If
			Me.fadeIn = Not Me.fadeIn
		End While
		Return
	End Function

	' Token: 0x0400296E RID: 10606
	<SerializeField()>
	Private minSecond As Single

	' Token: 0x0400296F RID: 10607
	<SerializeField()>
	Private maxSecond As Single

	' Token: 0x04002970 RID: 10608
	Private getSecond As Single

	' Token: 0x04002971 RID: 10609
	Private fadeTime As Single

	' Token: 0x04002972 RID: 10610
	Private setSpeed As Single

	' Token: 0x04002973 RID: 10611
	Private fadeIn As Boolean

	' Token: 0x04002974 RID: 10612
	Private change As Boolean

	' Token: 0x04002975 RID: 10613
	<SerializeField()>
	Private moonLady As FlyingBlimpLevelMoonLady

	' Token: 0x04002976 RID: 10614
	Private twinkleSprite As SpriteRenderer

	' Token: 0x04002977 RID: 10615
	<SerializeField()>
	Private starSprite As SpriteRenderer

	' Token: 0x04002978 RID: 10616
	Private twinkleClones As SpriteRenderer()

	' Token: 0x04002979 RID: 10617
	Private starClones As SpriteRenderer()
End Class
