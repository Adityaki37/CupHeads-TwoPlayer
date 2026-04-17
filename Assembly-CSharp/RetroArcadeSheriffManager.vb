Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000757 RID: 1879
Public Class RetroArcadeSheriffManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x060028EF RID: 10479 RVA: 0x0017D4BE File Offset: 0x0017B8BE
	Public Sub StartSheriff()
		Me.SetupSpawnPositions()
		MyBase.StartCoroutine(Me.sheriff_cr())
	End Sub

	' Token: 0x060028F0 RID: 10480 RVA: 0x0017D4D4 File Offset: 0x0017B8D4
	Private Sub SetupSpawnPositions()
		Dim num As Integer = 6
		Dim num2 As Integer = 4
		Dim num3 As Single = Me.bottom.position.y - CSng(Level.Current.Ground)
		Dim num4 As Single = (CSng(Level.Current.Right) - 20F) / CSng((num - 1)) * 2F
		Dim num5 As Single = (CSng(Level.Current.Ceiling) - Mathf.Abs(num3) - 20F) / CSng((num2 - 1)) * 2F
		Dim num6 As Integer = 0
		Me.spawnPositions = New List(Of Vector3)()
		For i As Integer = 0 To num * 2 - 1
			Dim num7 As Single = CSng(Level.Current.Left) + 20F + num4 * CSng(num6)
			Dim num8 As Single = If((i >= num), (CSng(Level.Current.Ground) + 20F), (CSng(Level.Current.Ceiling) - 20F))
			Me.spawnPositions.Add(New Vector3(num7, num8))
			num6 = If((i <> num - 1), (num6 + 1), 0)
		Next
		num6 = 1
		For j As Integer = 1 To num2 * 2 - 1 - 1
			Dim num9 As Single = If((j >= num2), (CSng(Level.Current.Left) + 20F), (CSng(Level.Current.Right) - 20F))
			Dim num10 As Single = CSng(Level.Current.Ground) + 20F + num5 * CSng(num6)
			Me.spawnPositions.Add(New Vector3(num9, num10))
			If j = num2 - 2 Then
				num6 = 1
				j = num2 - 1 + 1
			Else
				num6 += 1
			End If
		Next
	End Sub

	' Token: 0x060028F1 RID: 10481 RVA: 0x0017D684 File Offset: 0x0017BA84
	Private Iterator Function sheriff_cr() As IEnumerator
		Dim p As LevelProperties.RetroArcade.Sheriff = MyBase.properties.CurrentState.sheriff
		Dim delayMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.delayString.Length)
		Dim delayString As String() = p.delayString(delayMainIndex).Split(New Char() { ","c })
		Dim delayIndex As Integer = Global.UnityEngine.Random.Range(0, delayString.Length)
		Dim colorMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.colorString.Length)
		Dim colorString As String() = p.colorString(colorMainIndex).Split(New Char() { ","c })
		Dim colorIndex As Integer = Global.UnityEngine.Random.Range(0, colorString.Length)
		Dim sheriffChosen As RetroArcadeSheriff = Nothing
		Dim direction As Boolean = False
		Me.sheriffs = New List(Of RetroArcadeSheriff)()
		For i As Integer = 0 To Me.spawnPositions.Count - 1
			colorString = p.colorString(colorMainIndex).Split(New Char() { ","c })
			If colorString(colorIndex)(0) = "G"c Then
				sheriffChosen = Me.sheriffGreenPrefab
			ElseIf colorString(colorIndex)(0) = "Y"c Then
				sheriffChosen = Me.sheriffYellowPrefab
			ElseIf colorString(colorIndex)(0) = "O"c Then
				sheriffChosen = Me.sheriffOrangePrefab
			End If
			Dim retroArcadeSheriff As RetroArcadeSheriff = sheriffChosen.Create(Me.spawnPositions(i), p.moveSpeed, direction, 20F, p)
			Me.sheriffs.Add(retroArcadeSheriff)
			If colorIndex < colorString.Length - 1 Then
				colorIndex += 1
			Else
				colorMainIndex = (colorMainIndex + 1) Mod p.colorString.Length
				colorIndex = 0
			End If
		Next
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		For Each retroArcadeSheriff2 As RetroArcadeSheriff In Me.sheriffs
			retroArcadeSheriff2.StartMoving()
		Next
		MyBase.StartCoroutine(Me.check_if_dead_cr())
		Dim delay As Single = 0F
		Parser.FloatTryParse(delayString(delayIndex), delay)
		Yield CupheadTime.WaitForSeconds(Me, delay - Me.delaySubstract)
		While True
			Dim chosen As Integer = Global.UnityEngine.Random.Range(0, Me.sheriffs.Count)
			Dim countDeadOnes As Integer = 0
			For j As Integer = 0 To Me.sheriffs.Count - 1
				If Me.sheriffs(j).IsDead Then
					countDeadOnes += 1
				End If
			Next
			If countDeadOnes >= Me.sheriffs.Count Then
				Exit For
			End If
			While Me.sheriffs(chosen).IsDead
				chosen = Global.UnityEngine.Random.Range(0, Me.sheriffs.Count)
				Yield Nothing
			End While
			Dim player As AbstractPlayerController = PlayerManager.GetNext()
			Me.sheriffs(chosen).Shoot(player)
			If delayIndex < delayString.Length - 1 Then
				delayIndex += 1
			Else
				delayMainIndex = (delayMainIndex + 1) Mod p.delayString.Length
				delayIndex = 0
			End If
			Yield Nothing
			Parser.FloatTryParse(delayString(delayIndex), delay)
			Yield CupheadTime.WaitForSeconds(Me, delay - Me.delaySubstract)
		End While
		Me.EndPhase()
		Yield Nothing
		Return
	End Function

	' Token: 0x060028F2 RID: 10482 RVA: 0x0017D6A0 File Offset: 0x0017BAA0
	Private Iterator Function check_if_dead_cr() As IEnumerator
		Dim killedOff As Boolean() = New Boolean(Me.sheriffs.Count - 1) {}
		While True
			For i As Integer = 0 To Me.sheriffs.Count - 1
				If Me.sheriffs(i).IsDead AndAlso Not killedOff(i) Then
					killedOff(i) = True
					Me.HandleDeathChange()
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060028F3 RID: 10483 RVA: 0x0017D6BC File Offset: 0x0017BABC
	Private Sub HandleDeathChange()
		For i As Integer = 0 To Me.sheriffs.Count - 1
			Me.sheriffs(i).speed += MyBase.properties.CurrentState.sheriff.moveSpeedAddition
		Next
		Me.delaySubstract += MyBase.properties.CurrentState.sheriff.delayMinus
	End Sub

	' Token: 0x060028F4 RID: 10484 RVA: 0x0017D734 File Offset: 0x0017BB34
	Private Sub EndPhase()
		Me.StopAllCoroutines()
		For Each retroArcadeSheriff As RetroArcadeSheriff In Me.sheriffs
			Global.UnityEngine.[Object].Destroy(retroArcadeSheriff.gameObject)
		Next
		MyBase.properties.DealDamageToNextNamedState()
	End Sub

	' Token: 0x040031D5 RID: 12757
	<SerializeField()>
	Private bottom As Transform

	' Token: 0x040031D6 RID: 12758
	<SerializeField()>
	Private sheriffGreenPrefab As RetroArcadeSheriff

	' Token: 0x040031D7 RID: 12759
	<SerializeField()>
	Private sheriffYellowPrefab As RetroArcadeSheriff

	' Token: 0x040031D8 RID: 12760
	<SerializeField()>
	Private sheriffOrangePrefab As RetroArcadeSheriff

	' Token: 0x040031D9 RID: 12761
	Private sheriffs As List(Of RetroArcadeSheriff)

	' Token: 0x040031DA RID: 12762
	Private spawnPositions As List(Of Vector3)

	' Token: 0x040031DB RID: 12763
	Private Const offset As Single = 20F

	' Token: 0x040031DC RID: 12764
	Private delaySubstract As Single
End Class
