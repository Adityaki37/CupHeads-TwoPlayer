Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000760 RID: 1888
Public Class RetroArcadeTrafficManager
	Inherits LevelProperties.RetroArcade.Entity

	' Token: 0x06002921 RID: 10529 RVA: 0x0017FAF0 File Offset: 0x0017DEF0
	Public Sub StartTraffic()
		Me.SpawnTrafficLights()
		Me.StartUFO()
		MyBase.StartCoroutine(Me.move_ufo_cr())
	End Sub

	' Token: 0x06002922 RID: 10530 RVA: 0x0017FB0C File Offset: 0x0017DF0C
	Public Sub StartUFO()
		Me.trafficUFO.gameObject.SetActive(True)
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		If [next].transform.position.x > 0F Then
			Me.trafficUFO.transform.position = Me.trafficGrid(0, 3).transform.position
		Else
			Me.trafficUFO.transform.position = Me.trafficGrid(3, 3).transform.position
		End If
	End Sub

	' Token: 0x06002923 RID: 10531 RVA: 0x0017FBA0 File Offset: 0x0017DFA0
	Private Sub SpawnTrafficLights()
		Me.trafficGrid = New GameObject(3, 3) {}
		Dim num As Single = CSng((Level.Current.Width / 4))
		Dim num2 As Single = CSng((Level.Current.Height / 4))
		Dim vector As Vector3 = New Vector3(CSng(Level.Current.Left) + num / 2F, CSng(Level.Current.Ground) + num2 / 2F)
		For i As Integer = 0 To 4 - 1
			For j As Integer = 0 To 4 - 1
				Dim vector2 As Vector3 = New Vector3(CSng(i) * num, CSng(j) * num2)
				Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.trafficLightPrefab)
				Me.trafficGrid(i, j) = gameObject
				Me.trafficGrid(i, j).transform.position = vector + vector2
				Me.trafficGrid(i, j).transform.parent = MyBase.transform
			Next
		Next
	End Sub

	' Token: 0x06002924 RID: 10532 RVA: 0x0017FC98 File Offset: 0x0017E098
	Private Iterator Function move_ufo_cr() As IEnumerator
		Dim p As LevelProperties.RetroArcade.Traffic = MyBase.properties.CurrentState.traffic
		Dim lightMainIndex As Integer = Global.UnityEngine.Random.Range(0, p.lightOrderString.Length)
		Dim lightString As String() = p.lightOrderString(lightMainIndex).Split(New Char() { ","c })
		Dim lightX As Integer = 0
		Dim lightY As Integer = 0
		Me.positionsToTravel = New List(Of Vector3)()
		While Not Me.trafficUFO.IsDead
			lightString = p.lightOrderString(lightMainIndex).Split(New Char() { ","c })
			For i As Integer = 0 To lightString.Length - 1
				Yield CupheadTime.WaitForSeconds(Me, p.lightDelay)
				Dim getLightCoordX As String = lightString(i).Substring(1)
				Dim getLightCoordY As String = lightString(i).Substring(0, 1)
				Parser.IntTryParse(getLightCoordX, lightX)
				If getLightCoordY IsNot Nothing Then
					If Not(getLightCoordY = "A") Then
						If Not(getLightCoordY = "B") Then
							If Not(getLightCoordY = "C") Then
								If getLightCoordY = "D" Then
									lightY = 3
								End If
							Else
								lightY = 2
							End If
						Else
							lightY = 1
						End If
					Else
						lightY = 0
					End If
				End If
				Me.trafficGrid(lightX, lightY).GetComponent(Of Animator)().SetBool("IsGreen", True)
				Me.positionsToTravel.Add(Me.trafficGrid(lightX, lightY).transform.position)
			Next
			Me.trafficUFO.StartMoving(Me.positionsToTravel, p.moveSpeed, p.moveDelay)
			While Me.trafficUFO.IsMoving AndAlso Not Me.trafficUFO.IsDead
				Yield Nothing
			End While
			Me.ResetLights()
			Me.positionsToTravel.Clear()
			lightMainIndex = (lightMainIndex + 1) Mod p.lightOrderString.Length
			Yield Nothing
		End While
		Me.EndPhase()
		Return
	End Function

	' Token: 0x06002925 RID: 10533 RVA: 0x0017FCB3 File Offset: 0x0017E0B3
	Private Sub EndPhase()
		Me.StopAllCoroutines()
		Me.DestroyLights()
		Global.UnityEngine.[Object].Destroy(Me.trafficUFO.gameObject)
		MyBase.properties.DealDamageToNextNamedState()
	End Sub

	' Token: 0x06002926 RID: 10534 RVA: 0x0017FCDC File Offset: 0x0017E0DC
	Private Sub ResetLights()
		For i As Integer = 0 To 4 - 1
			For j As Integer = 0 To 4 - 1
				Me.trafficGrid(i, j).GetComponent(Of Animator)().SetBool("IsGreen", False)
			Next
		Next
	End Sub

	' Token: 0x06002927 RID: 10535 RVA: 0x0017FD2C File Offset: 0x0017E12C
	Private Sub DestroyLights()
		For i As Integer = 0 To 4 - 1
			For j As Integer = 0 To 4 - 1
				Global.UnityEngine.[Object].Destroy(Me.trafficGrid(i, j))
			Next
		Next
	End Sub

	' Token: 0x04003217 RID: 12823
	<SerializeField()>
	Private trafficUFO As RetroArcadeTrafficUFO

	' Token: 0x04003218 RID: 12824
	<SerializeField()>
	Private trafficLightPrefab As GameObject

	' Token: 0x04003219 RID: 12825
	Private Const GRIDX As Integer = 4

	' Token: 0x0400321A RID: 12826
	Private Const GRIDY As Integer = 4

	' Token: 0x0400321B RID: 12827
	Private trafficGrid As GameObject(,)

	' Token: 0x0400321C RID: 12828
	Private positionsToTravel As List(Of Vector3)
End Class
