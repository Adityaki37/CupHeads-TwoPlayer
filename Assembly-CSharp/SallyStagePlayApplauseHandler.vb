Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007A2 RID: 1954
Public Class SallyStagePlayApplauseHandler
	Inherits AbstractPausableComponent

	' Token: 0x06002BC3 RID: 11203 RVA: 0x00198BE8 File Offset: 0x00196FE8
	Private Sub Start()
		Me.handsStartPos = New Vector3(Me.hands.Length - 1) {}
		For i As Integer = 0 To Me.hands.Length - 1
			Me.hands(i).GetComponent(Of Animator)().Play(If((Not Rand.Bool()), "B", "A"))
			Me.handsStartPos(i) = Me.hands(i).transform.position
		Next
		Me.pinkPattern = Me.pinkString.Split(New Char() { ","c })
		Me.pinkIndex = Global.UnityEngine.Random.Range(0, Me.pinkPattern.Length)
	End Sub

	' Token: 0x06002BC4 RID: 11204 RVA: 0x00198C9F File Offset: 0x0019709F
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawLine(MyBase.transform.position, Me.endPos.transform.position)
	End Sub

	' Token: 0x06002BC5 RID: 11205 RVA: 0x00198CC8 File Offset: 0x001970C8
	Public Sub SlideApplause(slideIn As Boolean)
		For i As Integer = 0 To Me.hands.Length - 1
			MyBase.StartCoroutine(Me.slide_cr(Me.hands(i), Me.handsStartPos(i), slideIn, Global.UnityEngine.Random.Range(0.3F, 0.8F)))
			AudioManager.Play("sally_audience_applause")
		Next
	End Sub

	' Token: 0x06002BC6 RID: 11206 RVA: 0x00198D30 File Offset: 0x00197130
	Private Iterator Function slide_cr(hand As Transform, handStart As Vector3, slideIn As Boolean, delay As Single) As IEnumerator
		Dim start As Vector3 = If((Not slideIn), New Vector3(hand.transform.position.x, Me.endPos.position.y), handStart)
		Dim [end] As Vector3 = If((Not slideIn), handStart, New Vector3(hand.transform.position.x, Me.endPos.position.y))
		Dim t As Single = 0F
		Dim frameTime As Single = 0F
		Dim time As Single = 0.6F
		Yield CupheadTime.WaitForSeconds(Me, delay)
		While t < time
			frameTime += CupheadTime.Delta
			t += CupheadTime.Delta
			If frameTime > 0.041666668F Then
				frameTime -= 0.041666668F
				hand.transform.position = Vector3.Lerp(start, [end], t / time)
			End If
			Yield Nothing
		End While
		hand.transform.position = [end]
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BC7 RID: 11207 RVA: 0x00198D68 File Offset: 0x00197168
	Public Sub ThrowRose(pos As Vector3, properties As LevelProperties.SallyStagePlay.Roses)
		MyBase.StartCoroutine(Me.throw_rose_cr(pos, properties, Me.roseHands(Global.UnityEngine.Random.Range(0, Me.roseHands.Length))))
	End Sub

	' Token: 0x06002BC8 RID: 11208 RVA: 0x00198D90 File Offset: 0x00197190
	Private Iterator Function throw_rose_cr(pos As Vector3, properties As LevelProperties.SallyStagePlay.Roses, arm As Transform) As IEnumerator
		Dim component As Animator = arm.GetComponent(Of Animator)()
		Dim text As String = If((Not Rand.Bool()), "Rose_2_A", "Rose_1_A")
		Dim text2 As String = text
		Dim animationName As String = text
		component.Play(text2)
		arm.transform.SetPosition(New Single?(pos.x), Nothing, Nothing)
		Dim speed As Single = 900F
		Yield arm.GetComponent(Of Animator)().WaitForAnimationToEnd(Me, animationName, False, True)
		Me.roseStill.transform.position = New Vector3(arm.transform.position.x, arm.transform.position.y + 50F)
		While Me.roseStill.transform.position.y < CSng(Level.Current.Ceiling) + 100F
			Me.roseStill.transform.position += Vector3.up * speed * CupheadTime.Delta
			Yield Nothing
		End While
		Dim r As SallyStagePlayLevelRose = Me.rose.Create(pos, properties)
		r.SetParryable(Me.pinkPattern(Me.pinkIndex)(0) = "P"c)
		Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkPattern.Length
		Yield Nothing
		Return
	End Function

	' Token: 0x06002BC9 RID: 11209 RVA: 0x00198DC0 File Offset: 0x001971C0
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.rose = Nothing
	End Sub

	' Token: 0x0400346E RID: 13422
	Private Const FRAME_TIME As Single = 0.041666668F

	' Token: 0x0400346F RID: 13423
	<SerializeField()>
	Private rose As SallyStagePlayLevelRose

	' Token: 0x04003470 RID: 13424
	<SerializeField()>
	Private hands As Transform()

	' Token: 0x04003471 RID: 13425
	Private handsStartPos As Vector3()

	' Token: 0x04003472 RID: 13426
	<SerializeField()>
	Private roseHands As Transform()

	' Token: 0x04003473 RID: 13427
	<SerializeField()>
	Private roseStill As Transform

	' Token: 0x04003474 RID: 13428
	<SerializeField()>
	Private endPos As Transform

	' Token: 0x04003475 RID: 13429
	<SerializeField()>
	Private pinkString As String

	' Token: 0x04003476 RID: 13430
	Private pinkPattern As String()

	' Token: 0x04003477 RID: 13431
	Private pinkIndex As Integer
End Class
