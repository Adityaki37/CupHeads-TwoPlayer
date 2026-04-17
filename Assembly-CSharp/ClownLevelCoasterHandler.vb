Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000562 RID: 1378
Public Class ClownLevelCoasterHandler
	Inherits LevelProperties.Clown.Entity

	' Token: 0x14000040 RID: 64
	' (add) Token: 0x060019EF RID: 6639 RVA: 0x000ED1F0 File Offset: 0x000EB5F0
	' (remove) Token: 0x060019F0 RID: 6640 RVA: 0x000ED228 File Offset: 0x000EB628
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnCoasterLeave As Action

	' Token: 0x060019F1 RID: 6641 RVA: 0x000ED25E File Offset: 0x000EB65E
	Public Overrides Sub LevelInit(properties As LevelProperties.Clown)
		MyBase.LevelInit(properties)
		Me.finalRun = False
	End Sub

	' Token: 0x060019F2 RID: 6642 RVA: 0x000ED26E File Offset: 0x000EB66E
	Public Sub StartCoaster()
		Me.isRunning = True
		MyBase.StartCoroutine(Me.coaster_cr())
	End Sub

	' Token: 0x060019F3 RID: 6643 RVA: 0x000ED284 File Offset: 0x000EB684
	Private Iterator Function coaster_cr() As IEnumerator
		Dim p As LevelProperties.Clown.Coaster = MyBase.properties.CurrentState.coaster
		Dim coasterPattern As String() = p.coasterTypeString.GetRandom().Split(New Char() { ","c })
		Dim coasterSize As Single = Me.redCoaster.GetComponent(Of Renderer)().bounds.size.x
		If MyBase.properties.CurrentState.stateName = LevelProperties.Clown.States.Swing Then
			While Me.swing.state = ClownLevelClownSwing.State.Intro
				Yield Nothing
			End While
		End If
		Yield CupheadTime.WaitForSeconds(Me, p.initialDelay)
		While Me.isRunning
			Yield Nothing
			Dim coaster As ClownLevelCoaster = Global.UnityEngine.[Object].Instantiate(Of ClownLevelCoaster)(Me.coasterPrefab)
			coaster.Init(Me.backTrackStart.position, Me.frontTrackStart.position, p, CSng(coasterPattern.Length), coasterSize, Me.warningLight)
			Dim lastInstantiatedRoot As Transform = coaster.pieceRoot
			For i As Integer = 0 To coasterPattern.Length - 1
				If i Mod 2 = 1 Then
					Dim clownLevelCoasterPiece As ClownLevelCoasterPiece = Global.UnityEngine.[Object].Instantiate(Of ClownLevelCoasterPiece)(Me.blueCoaster)
					clownLevelCoasterPiece.Init(lastInstantiatedRoot.position)
					lastInstantiatedRoot = clownLevelCoasterPiece.newPieceRoot
					clownLevelCoasterPiece.transform.parent = coaster.transform
					If i = coasterPattern.Length Then
						lastInstantiatedRoot = clownLevelCoasterPiece.tailRoot
					End If
					If coasterPattern(i)(0) = "F"c Then
						Dim clownLevelRiders As ClownLevelRiders = Global.UnityEngine.[Object].Instantiate(Of ClownLevelRiders)(Me.ridersPrefab)
						Dim clownLevelRiders2 As ClownLevelRiders = Global.UnityEngine.[Object].Instantiate(Of ClownLevelRiders)(Me.ridersPrefab)
						clownLevelRiders.transform.position = clownLevelCoasterPiece.ridersFrontRoot.position
						clownLevelRiders.transform.parent = clownLevelCoasterPiece.ridersFrontRoot.transform
						clownLevelRiders.inFront = True
						clownLevelCoasterPiece.riders.Add(clownLevelRiders)
						clownLevelRiders2.transform.position = clownLevelCoasterPiece.ridersBackRoot.position
						clownLevelRiders2.transform.parent = clownLevelCoasterPiece.ridersBackRoot.transform
						clownLevelRiders2.inFront = False
						clownLevelCoasterPiece.riders.Add(clownLevelRiders2)
					End If
				Else
					Dim clownLevelCoasterPiece2 As ClownLevelCoasterPiece = Global.UnityEngine.[Object].Instantiate(Of ClownLevelCoasterPiece)(Me.redCoaster)
					clownLevelCoasterPiece2.Init(lastInstantiatedRoot.position)
					lastInstantiatedRoot = clownLevelCoasterPiece2.newPieceRoot
					clownLevelCoasterPiece2.transform.parent = coaster.transform
					If i = coasterPattern.Length Then
						lastInstantiatedRoot = clownLevelCoasterPiece2.tailRoot
					End If
					If coasterPattern(i)(0) = "F"c Then
						Dim clownLevelRiders3 As ClownLevelRiders = Global.UnityEngine.[Object].Instantiate(Of ClownLevelRiders)(Me.ridersPrefab)
						Dim clownLevelRiders4 As ClownLevelRiders = Global.UnityEngine.[Object].Instantiate(Of ClownLevelRiders)(Me.ridersPrefab)
						clownLevelRiders3.transform.position = clownLevelCoasterPiece2.ridersFrontRoot.position
						clownLevelRiders3.transform.parent = clownLevelCoasterPiece2.ridersFrontRoot.transform
						clownLevelRiders3.inFront = True
						clownLevelCoasterPiece2.riders.Add(clownLevelRiders3)
						clownLevelRiders4.transform.position = clownLevelCoasterPiece2.ridersBackRoot.position
						clownLevelRiders4.transform.parent = clownLevelCoasterPiece2.ridersBackRoot.transform
						clownLevelRiders4.inFront = False
						clownLevelCoasterPiece2.riders.Add(clownLevelRiders4)
					End If
				End If
			Next
			Dim tail As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.tailPrefab)
			tail.transform.position = lastInstantiatedRoot.position
			tail.transform.parent = coaster.transform
			coaster.BackCoasterSetup()
			While coaster IsNot Nothing
				Yield Nothing
			End While
			If Me.OnCoasterLeave IsNot Nothing Then
				Me.OnCoasterLeave()
			End If
			If Me.finalRun Then
				Me.isRunning = False
				Me.finalRun = False
				Return
			End If
			Yield CupheadTime.WaitForSeconds(Me, p.mainLoopDelay)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060019F4 RID: 6644 RVA: 0x000ED29F File Offset: 0x000EB69F
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.redCoaster = Nothing
		Me.blueCoaster = Nothing
		Me.ridersPrefab = Nothing
		Me.tailPrefab = Nothing
		Me.coasterPrefab = Nothing
	End Sub

	' Token: 0x0400230E RID: 8974
	Public finalRun As Boolean

	' Token: 0x0400230F RID: 8975
	Public isRunning As Boolean

	' Token: 0x04002310 RID: 8976
	<SerializeField()>
	Private swing As ClownLevelClownSwing

	' Token: 0x04002311 RID: 8977
	<SerializeField()>
	Private warningLight As ClownLevelLights

	' Token: 0x04002312 RID: 8978
	<SerializeField()>
	Private frontTrackStart As Transform

	' Token: 0x04002313 RID: 8979
	<SerializeField()>
	Private backTrackStart As Transform

	' Token: 0x04002314 RID: 8980
	<SerializeField()>
	Private redCoaster As ClownLevelCoasterPiece

	' Token: 0x04002315 RID: 8981
	<SerializeField()>
	Private blueCoaster As ClownLevelCoasterPiece

	' Token: 0x04002316 RID: 8982
	<SerializeField()>
	Private ridersPrefab As ClownLevelRiders

	' Token: 0x04002317 RID: 8983
	<SerializeField()>
	Private tailPrefab As GameObject

	' Token: 0x04002318 RID: 8984
	<SerializeField()>
	Private coasterPrefab As ClownLevelCoaster
End Class
