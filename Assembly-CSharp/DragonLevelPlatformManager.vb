Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x020005FA RID: 1530
Public Class DragonLevelPlatformManager
	Inherits AbstractPausableComponent

	' Token: 0x06001E71 RID: 7793 RVA: 0x00118BA4 File Offset: 0x00116FA4
	Public Sub Init(properties As LevelProperties.Dragon.Clouds)
		Me.properties = properties
		Me.toggleDelay = True
		Me.platforms = New List(Of DragonLevelCloudPlatform)()
		For i As Integer = 0 To Me.maxPlatforms - 1
			Dim dragonLevelCloudPlatform As DragonLevelCloudPlatform = Global.UnityEngine.[Object].Instantiate(Of DragonLevelCloudPlatform)(Me.platformPrefab)
			dragonLevelCloudPlatform.gameObject.SetActive(False)
			dragonLevelCloudPlatform.transform.parent = MyBase.transform
			Me.platforms.Add(dragonLevelCloudPlatform)
		Next
		MyBase.StartCoroutine(Me.spawn_platforms())
		MyBase.StartCoroutine(Me.run_delay_cr())
	End Sub

	' Token: 0x06001E72 RID: 7794 RVA: 0x00118C30 File Offset: 0x00117030
	Public Sub UpdateProperties(properties As LevelProperties.Dragon.Clouds)
		Me.toggleDelay = True
		Me.properties = properties
		For Each dragonLevelCloudPlatform As DragonLevelCloudPlatform In Me.platforms
			dragonLevelCloudPlatform.GetProperties(properties, False)
		Next
		MyBase.StartCoroutine(Me.run_delay_cr())
	End Sub

	' Token: 0x06001E73 RID: 7795 RVA: 0x00118CA8 File Offset: 0x001170A8
	Public Sub DestroyObjectPool(obj As DragonLevelCloudPlatform)
		Me.platforms.Add(obj)
		obj.gameObject.SetActive(False)
	End Sub

	' Token: 0x06001E74 RID: 7796 RVA: 0x00118CC4 File Offset: 0x001170C4
	Private Iterator Function run_delay_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.cloudSpeed / 200F)
		Me.toggleDelay = False
		Return
	End Function

	' Token: 0x06001E75 RID: 7797 RVA: 0x00118CE0 File Offset: 0x001170E0
	Private Iterator Function spawn_platforms() As IEnumerator
		Dim positions As List(Of String) = New List(Of String)(Me.properties.cloudPositions)
		Dim mainIndex As Integer = Global.UnityEngine.Random.Range(0, positions.Count)
		Dim positionString As String() = positions(mainIndex).Split(New Char() { ","c })
		Dim positionIndex As Integer = Global.UnityEngine.Random.Range(0, positionString.Length)
		Dim platformIndex As Integer = 0
		Dim platformWidth As Single = Me.platformPrefab.GetComponent(Of Renderer)().bounds.size.x / 2F
		Dim waitTime As Single = 0F
		Dim position As Single = 0F
		While True
			While Me.toggleDelay
				Yield Nothing
			End While
			positionString = positions(mainIndex).Split(New Char() { ","c })
			Me.startPosition = If((Not Me.properties.movingRight), (640F + platformWidth), (-640F - platformWidth))
			If positionString(positionIndex)(0) = "D"c Then
				Parser.FloatTryParse(positionString(positionIndex).Substring(1), waitTime)
			Else
				Dim array As String() = positionString(positionIndex).Split(New Char() { "-"c })
				For Each text As String In array
					Parser.FloatTryParse(text, position)
					Me.platforms(platformIndex).transform.position = New Vector3(Me.startPosition, 360F - position, 0F)
					Me.platforms(platformIndex).gameObject.SetActive(True)
					Me.platforms(platformIndex).GetProperties(Me, Me.properties)
					platformIndex = (platformIndex + 1) Mod Me.platforms.Count
				Next
				waitTime = Me.properties.cloudDelay
			End If
			Yield CupheadTime.WaitForSeconds(Me, waitTime)
			If positionIndex < positionString.Length - 1 Then
				positionIndex += 1
			ElseIf positions.Count > 1 Then
				positions.Remove(positions(mainIndex))
				positionIndex = 0
				mainIndex = Global.UnityEngine.Random.Range(0, positions.Count)
			Else
				positionIndex = 0
				mainIndex = 0
				positions = New List(Of String)(Me.properties.cloudPositions)
			End If
		End While
		Return
	End Function

	' Token: 0x06001E76 RID: 7798 RVA: 0x00118CFB File Offset: 0x001170FB
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.platformPrefab = Nothing
	End Sub

	' Token: 0x04002749 RID: 10057
	Private Const LEFT_X As Single = -1280F

	' Token: 0x0400274A RID: 10058
	Private Const RIGHT_X As Single = 1280F

	' Token: 0x0400274B RID: 10059
	<SerializeField()>
	Public platforms As List(Of DragonLevelCloudPlatform)

	' Token: 0x0400274C RID: 10060
	<SerializeField()>
	Private platformPrefab As DragonLevelCloudPlatform

	' Token: 0x0400274D RID: 10061
	Private properties As LevelProperties.Dragon.Clouds

	' Token: 0x0400274E RID: 10062
	Private maxPlatforms As Integer = 20

	' Token: 0x0400274F RID: 10063
	Private startPosition As Single

	' Token: 0x04002750 RID: 10064
	Private toggleDelay As Boolean
End Class
