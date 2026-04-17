Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005C7 RID: 1479
Public Class DicePalaceFlyingMemoryLevelCard
	Inherits ParrySwitch

	' Token: 0x17000364 RID: 868
	' (get) Token: 0x06001CED RID: 7405 RVA: 0x00109218 File Offset: 0x00107618
	' (set) Token: 0x06001CEE RID: 7406 RVA: 0x00109220 File Offset: 0x00107620
	Public Property flippedUp As Boolean

	' Token: 0x06001CEF RID: 7407 RVA: 0x00109229 File Offset: 0x00107629
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.flippedUp = False
		Me.flippedDownCard = MyBase.GetComponent(Of SpriteRenderer)().sprite
	End Sub

	' Token: 0x06001CF0 RID: 7408 RVA: 0x0010924C File Offset: 0x0010764C
	Public Sub FlipUp()
		MyBase.StartCoroutine(Me.rotate_cr(0F, 360F, 0.6F))
		Me.flippedUpCard = Me.flippedUpCards(CInt(Me.card))
		MyBase.GetComponent(Of SpriteRenderer)().sprite = Me.flippedUpCard
		Me.flippedUp = True
		Me.pinkDot.enabled = False
	End Sub

	' Token: 0x06001CF1 RID: 7409 RVA: 0x001092AC File Offset: 0x001076AC
	Public Sub EnableCards()
		If Not Me.permanentlyFlipped Then
			If Me.flippedUp Then
				MyBase.StartCoroutine(Me.rotate_cr(0F, 360F, 0.6F))
				MyBase.GetComponent(Of SpriteRenderer)().sprite = Me.flippedDownCard
				Me.flippedUp = False
			End If
			Me.pinkDot.enabled = True
			MyBase.StartCoroutine(Me.fade_pink_cr(False))
			MyBase.GetComponent(Of Collider2D)().enabled = True
		End If
	End Sub

	' Token: 0x06001CF2 RID: 7410 RVA: 0x00109329 File Offset: 0x00107729
	Public Sub DisableCard()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		If Not Me.flippedUp OrElse Not Me.permanentlyFlipped Then
			MyBase.StartCoroutine(Me.fade_pink_cr(True))
		End If
	End Sub

	' Token: 0x06001CF3 RID: 7411 RVA: 0x0010935C File Offset: 0x0010775C
	Private Iterator Function rotate_cr(start As Single, [end] As Single, time As Single) As IEnumerator
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(EaseUtils.Ease(Me.ROTATION_EASE, start, [end], val)), New Single?(0F))
			t += Time.deltaTime
			Yield Nothing
		End While
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(0F))
		Yield Nothing
		Return
	End Function

	' Token: 0x06001CF4 RID: 7412 RVA: 0x0010938C File Offset: 0x0010778C
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		Me.FlipUp()
	End Sub

	' Token: 0x06001CF5 RID: 7413 RVA: 0x0010939C File Offset: 0x0010779C
	Private Iterator Function fade_pink_cr(fadingOut As Boolean) As IEnumerator
		If fadingOut Then
			Dim t As Single = 0F
			While t < Me.fadeTime
				Me.pinkDot.color = New Color(1F, 1F, 1F, 1F - t / Me.fadeTime)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			Me.pinkDot.color = New Color(1F, 1F, 1F, 0F)
		Else
			Dim t2 As Single = 0F
			While t2 < Me.fadeTime
				Me.pinkDot.color = New Color(1F, 1F, 1F, t2 / Me.fadeTime)
				t2 += CupheadTime.Delta
				Yield Nothing
			End While
			Me.pinkDot.color = New Color(1F, 1F, 1F, 1F)
		End If
		Return
	End Function

	' Token: 0x06001CF6 RID: 7414 RVA: 0x001093BE File Offset: 0x001077BE
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.flippedUpCards = Nothing
		Me.flippedUpCard = Nothing
		Me.flippedDownCard = Nothing
	End Sub

	' Token: 0x040025D7 RID: 9687
	Public permanentlyFlipped As Boolean

	' Token: 0x040025D8 RID: 9688
	Private Const ROTATION_TIME As Single = 0.6F

	' Token: 0x040025D9 RID: 9689
	Private Const ROTATION_BACK As Single = 360F

	' Token: 0x040025DA RID: 9690
	Private ROTATION_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeOutBack

	' Token: 0x040025DB RID: 9691
	<SerializeField()>
	Private flippedUpCards As Sprite()

	' Token: 0x040025DC RID: 9692
	<SerializeField()>
	Private pinkDot As SpriteRenderer

	' Token: 0x040025DD RID: 9693
	Private flippedUpCard As Sprite

	' Token: 0x040025DE RID: 9694
	Private flippedDownCard As Sprite

	' Token: 0x040025DF RID: 9695
	Private rotationCoroutine As Coroutine

	' Token: 0x040025E0 RID: 9696
	Private fadeTime As Single = 0.7F

	' Token: 0x040025E1 RID: 9697
	Public card As DicePalaceFlyingMemoryLevelCard.Card

	' Token: 0x020005C8 RID: 1480
	Public Enum Card
		' Token: 0x040025E3 RID: 9699
		Cuphead
		' Token: 0x040025E4 RID: 9700
		Chips
		' Token: 0x040025E5 RID: 9701
		Flowers
		' Token: 0x040025E6 RID: 9702
		Shield
		' Token: 0x040025E7 RID: 9703
		Spindle
		' Token: 0x040025E8 RID: 9704
		Mugman
	End Enum
End Class
