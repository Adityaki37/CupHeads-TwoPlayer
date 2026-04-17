Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020007E4 RID: 2020
Public Class SnowCultHandleBackground
	Inherits AbstractPausableComponent

	' Token: 0x06002E3F RID: 11839 RVA: 0x001B44B0 File Offset: 0x001B28B0
	Private Sub Update()
		Me.fadeTimer += CupheadTime.Delta
		For i As Integer = 0 To Me.fadeRenderers.Length - 1
			Me.fadeRenderers(i).color = New Color(1F, 1F, 1F, Mathf.Lerp(Me.fadeMin(i), Me.fadeMax(i), Mathf.Abs((Me.fadeTimer + Me.fadeOffset(i) * Me.fadePeriod(i)) Mod Me.fadePeriod(i) - Me.fadePeriod(i) / 2F)) / (Me.fadePeriod(i) / 2F))
		Next
		Me.glimmerTimer -= CupheadTime.Delta
		If Me.glimmerTimer <= 0F Then
			Me.glimmer.Play("Glimmer", 0, 0F)
			Me.glimmerTimer += Global.UnityEngine.Random.Range(3.5F, 6.5F)
		End If
		Me.sparkleTimer -= CupheadTime.Delta
		If Me.sparkleTimer <= 0F Then
			If Me.sparkleList.Count = 0 Then
				For j As Integer = 0 To Me.sparkles.Length - 1
					Me.sparkleList.Add(j)
				Next
			End If
			Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.sparkleList.Count)
			Me.sparkles(Me.sparkleList(num)).Play("Sparkle", 0, 0F)
			Me.sparkleList.RemoveAt(num)
			Me.sparkleTimer = Global.UnityEngine.Random.Range(0.25F, 0.75F)
		End If
	End Sub

	' Token: 0x06002E40 RID: 11840 RVA: 0x001B4674 File Offset: 0x001B2A74
	Public Sub CandleGust()
		For Each animator As Animator In Me.candles
			animator.SetTrigger("OnGust")
		Next
	End Sub

	' Token: 0x040036B9 RID: 14009
	Private fadeTimer As Single

	' Token: 0x040036BA RID: 14010
	<SerializeField()>
	Private fadeRenderers As SpriteRenderer()

	' Token: 0x040036BB RID: 14011
	<SerializeField()>
	Private fadePeriod As Single()

	' Token: 0x040036BC RID: 14012
	<SerializeField()>
	Private fadeOffset As Single()

	' Token: 0x040036BD RID: 14013
	<SerializeField()>
	Private fadeMin As Single()

	' Token: 0x040036BE RID: 14014
	<SerializeField()>
	Private fadeMax As Single()

	' Token: 0x040036BF RID: 14015
	<SerializeField()>
	Private candles As Animator()

	' Token: 0x040036C0 RID: 14016
	<SerializeField()>
	Private glimmer As Animator

	' Token: 0x040036C1 RID: 14017
	Private glimmerTimer As Single = 2F

	' Token: 0x040036C2 RID: 14018
	<SerializeField()>
	Private sparkles As Animator()

	' Token: 0x040036C3 RID: 14019
	Private sparkleTimer As Single = 1F

	' Token: 0x040036C4 RID: 14020
	Private sparkleList As List(Of Integer) = New List(Of Integer)()
End Class
