Imports System
Imports UnityEngine

' Token: 0x0200094F RID: 2383
Public Class MapNPCCompetition
	Inherits MonoBehaviour

	' Token: 0x060037B4 RID: 14260 RVA: 0x001FFAA0 File Offset: 0x001FDEA0
	Private Sub Start()
		Dim curseCharmPuzzleOrder As Integer() = PlayerData.Data.curseCharmPuzzleOrder
		For Each num As Integer In PlayerData.Data.curseCharmPuzzleOrder
		Next
		For j As Integer = 0 To curseCharmPuzzleOrder.Length - 1
			Dialoguer.SetGlobalFloat(Me.dialogueVarIndices(j), CSng(curseCharmPuzzleOrder(j)))
		Next
	End Sub

	' Token: 0x04003FB8 RID: 16312
	Private dialogueVarIndices As Integer() = New Integer() { 26, 27, 28 }
End Class
