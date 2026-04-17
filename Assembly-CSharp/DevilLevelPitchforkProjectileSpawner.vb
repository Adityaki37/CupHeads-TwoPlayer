Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200057B RID: 1403
Public Class DevilLevelPitchforkProjectileSpawner
	' Token: 0x06001AB4 RID: 6836 RVA: 0x000F4DFA File Offset: 0x000F31FA
	Public Sub New(numProjectiles As Integer, angleOffsets As String)
		Me.numProjectiles = numProjectiles
		Me.angleOffsets = angleOffsets.Split(New Char() { ","c })
		Me.angleOffsetIndex = Global.UnityEngine.Random.Range(0, angleOffsets.Length)
	End Sub

	' Token: 0x06001AB5 RID: 6837 RVA: 0x000F4E34 File Offset: 0x000F3234
	Public Function getSpawnAngles() As List(Of Single)
		Dim list As List(Of Single) = New List(Of Single)()
		Me.angleOffsetIndex = (Me.angleOffsetIndex + 1) Mod Me.angleOffsets.Length
		Dim num As Single = 0F
		Parser.FloatTryParse(Me.angleOffsets(Me.angleOffsetIndex), num)
		For i As Integer = 0 To Me.numProjectiles - 1
			list.Add(CSng(i) * 360F / CSng(Me.numProjectiles) + num + 90F)
		Next
		Return list
	End Function

	' Token: 0x040023E1 RID: 9185
	Private numProjectiles As Integer

	' Token: 0x040023E2 RID: 9186
	Private angleOffsets As String()

	' Token: 0x040023E3 RID: 9187
	Private angleOffsetIndex As Integer
End Class
