Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020008A9 RID: 2217
Public Class CircusPlatformingLevelPoleHandler
	Inherits AbstractPausableComponent

	' Token: 0x060033AF RID: 13231 RVA: 0x001E052D File Offset: 0x001DE92D
	Private Sub Start()
		Me.SetupBots()
	End Sub

	' Token: 0x060033B0 RID: 13232 RVA: 0x001E0538 File Offset: 0x001DE938
	Private Sub SetupBots()
		Me.poleBots = New List(Of CircusPlatformingLevelPoleBot)()
		Dim y As Single = Me.poleBot.GetComponent(Of BoxCollider2D)().size.y
		For i As Integer = 0 To Me.poleBotCount - 1
			Dim vector As Vector2 = New Vector2(Me.poleRoot.transform.position.x, Me.poleRoot.transform.position.y + y * 1.38F * CSng(i))
			Me.poleBots.Add(Me.poleBot.Spawn(vector))
		Next
		MyBase.StartCoroutine(Me.check_to_slide_cr())
	End Sub

	' Token: 0x060033B1 RID: 13233 RVA: 0x001E05F0 File Offset: 0x001DE9F0
	Private Iterator Function check_to_slide_cr() As IEnumerator
		Dim indexToSlide As Integer = 1000
		While True
			For i As Integer = Me.poleBots.Count - 1 To 0 Step -1
				If Me.poleBots(i).isDying Then
					Me.poleBots.RemoveAt(i)
					indexToSlide = i
					Exit For
				End If
				If i >= indexToSlide Then
					While Me.poleBots(i).isSliding
						Yield Nothing
					End While
					Me.poleBots(i).SlideDown()
				End If
				If i = 0 Then
					indexToSlide = 1000
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003BFC RID: 15356
	<SerializeField()>
	Private poleBotCount As Integer

	' Token: 0x04003BFD RID: 15357
	<SerializeField()>
	Private poleRoot As Transform

	' Token: 0x04003BFE RID: 15358
	<SerializeField()>
	Private poleBot As CircusPlatformingLevelPoleBot

	' Token: 0x04003BFF RID: 15359
	Private poleBots As List(Of CircusPlatformingLevelPoleBot)
End Class
