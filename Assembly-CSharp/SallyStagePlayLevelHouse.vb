Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007AF RID: 1967
Public Class SallyStagePlayLevelHouse
	Inherits AbstractPausableComponent

	' Token: 0x06002C34 RID: 11316 RVA: 0x0019FD18 File Offset: 0x0019E118
	Public Sub StartPhase2(parent As SallyStagePlayLevel, properties As LevelProperties.SallyStagePlay)
		Me.SetUp(parent, properties)
	End Sub

	' Token: 0x06002C35 RID: 11317 RVA: 0x0019FD22 File Offset: 0x0019E122
	Public Sub StartAttacks()
		If Not SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE Then
			MyBase.StartCoroutine(Me.family_cr())
		Else
			MyBase.StartCoroutine(Me.nuns_cr())
		End If
	End Sub

	' Token: 0x06002C36 RID: 11318 RVA: 0x0019FD4D File Offset: 0x0019E14D
	Private Sub SetUp(parent As SallyStagePlayLevel, properties As LevelProperties.SallyStagePlay)
		Me.parent = parent
		Me.properties = properties
		AddHandler parent.OnPhase3, AddressOf Me.OnPhase3
		MyBase.StartCoroutine(Me.setup_windows_cr())
	End Sub

	' Token: 0x06002C37 RID: 11319 RVA: 0x0019FD7C File Offset: 0x0019E17C
	Private Iterator Function setup_windows_cr() As IEnumerator
		Dim pos As Vector3 = Vector3.zero
		Dim num As Integer = 1
		Me.windows = New SallyStagePlayLevelWindow(8) {}
		For i As Integer = 0 To 9 - 1
			Me.windows(i) = Global.UnityEngine.[Object].Instantiate(Of SallyStagePlayLevelWindow)(Me.windowPrefab)
			Me.windows(i).transform.position = Me.windowRoots(i).position
			Me.windows(i).Init(Me.windowRoots(i).position, Me.parent)
			Me.windows(i).transform.parent = MyBase.transform
			Me.windows(i).windowNum = num + i
		Next
		Yield Nothing
		Return
	End Function

	' Token: 0x06002C38 RID: 11320 RVA: 0x0019FD98 File Offset: 0x0019E198
	Private Iterator Function nuns_cr() As IEnumerator
		Dim p As LevelProperties.SallyStagePlay.Nun = Me.properties.CurrentState.nun
		Dim windowPattern As String() = p.appearPosition.GetRandom().Split(New Char() { ","c })
		Dim windowPos As Integer = 0
		Dim pinkStringMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.pinkString.Length)
		Dim pinkString As String() = p.pinkString(pinkStringMainIndex).Split(New Char() { ","c })
		Dim pinkStringIndex As Integer = Global.UnityEngine.Random.Range(0, pinkString.Length)
		While True
			For i As Integer = 0 To windowPattern.Length - 1
				Parser.IntTryParse(windowPattern(i), windowPos)
				For Each window As SallyStagePlayLevelWindow In Me.windows
					If window.windowNum = windowPos Then
						window.WindowOpenNun(Me.properties, pinkString(pinkStringIndex)(0) = "P"c)
						If pinkStringIndex < pinkString.Length - 1 Then
							pinkStringIndex += 1
						Else
							pinkStringMainIndex = (pinkStringMainIndex + 1) Mod p.pinkString.Length
							pinkStringIndex = 0
						End If
						Yield CupheadTime.WaitForSeconds(Me, p.reappearDelayRange.RandomFloat())
					End If
				Next
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002C39 RID: 11321 RVA: 0x0019FDB4 File Offset: 0x0019E1B4
	Private Iterator Function family_cr() As IEnumerator
		Dim p As LevelProperties.SallyStagePlay.Baby = Me.properties.CurrentState.baby
		Dim windowPattern As String() = p.appearPosition.GetRandom().Split(New Char() { ","c })
		Dim windowPos As Integer = 0
		While True
			For i As Integer = 0 To windowPattern.Length - 1
				Parser.IntTryParse(windowPattern(i), windowPos)
				For Each window As SallyStagePlayLevelWindow In Me.windows
					If window.windowNum = windowPos Then
						window.WindowOpenBaby(Me.properties)
						Yield CupheadTime.WaitForSeconds(Me, p.hesitate)
						Yield CupheadTime.WaitForSeconds(Me, p.reappearDelayRange.RandomFloat())
					End If
				Next
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002C3A RID: 11322 RVA: 0x0019FDCF File Offset: 0x0019E1CF
	Private Sub OnPhase3()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject, 1F)
		RemoveHandler Me.parent.OnPhase3, AddressOf Me.OnPhase3
	End Sub

	' Token: 0x06002C3B RID: 11323 RVA: 0x0019FDFE File Offset: 0x0019E1FE
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.windowPrefab = Nothing
	End Sub

	' Token: 0x040034DF RID: 13535
	<SerializeField()>
	Private windowRoots As Transform()

	' Token: 0x040034E0 RID: 13536
	<SerializeField()>
	Private windowPrefab As SallyStagePlayLevelWindow

	' Token: 0x040034E1 RID: 13537
	Private windows As SallyStagePlayLevelWindow()

	' Token: 0x040034E2 RID: 13538
	Private properties As LevelProperties.SallyStagePlay

	' Token: 0x040034E3 RID: 13539
	Private parent As SallyStagePlayLevel

	' Token: 0x040034E4 RID: 13540
	Private Const WINDOW_NUM As Integer = 9
End Class
