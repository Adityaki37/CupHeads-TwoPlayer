Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000887 RID: 2183
Public Class TreePlatformingLevelBeetleSpawner
	Inherits PlatformingLevelEnemySpawner

	' Token: 0x060032BB RID: 12987 RVA: 0x001D7714 File Offset: 0x001D5B14
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.beetles = New TreePlatformingLevelBeetle(MyBase.GetComponentsInChildren(Of TreePlatformingLevelBeetle)().Length - 1) {}
		Me.beetles = MyBase.GetComponentsInChildren(Of TreePlatformingLevelBeetle)()
	End Sub

	' Token: 0x060032BC RID: 12988 RVA: 0x001D773B File Offset: 0x001D5B3B
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.check_to_play_sfx())
	End Sub

	' Token: 0x060032BD RID: 12989 RVA: 0x001D7750 File Offset: 0x001D5B50
	Protected Overrides Sub Spawn()
		MyBase.Spawn()
		Me.Activate()
	End Sub

	' Token: 0x060032BE RID: 12990 RVA: 0x001D7760 File Offset: 0x001D5B60
	Private Sub Activate()
		Dim num As Integer = Global.UnityEngine.Random.Range(0, Me.beetles.Length)
		If Not Me.beetles(num).isActivated Then
			Me.beetles(num).Activate()
		End If
	End Sub

	' Token: 0x060032BF RID: 12991 RVA: 0x001D779C File Offset: 0x001D5B9C
	Private Iterator Function check_to_play_sfx() As IEnumerator
		While True
			For Each treePlatformingLevelBeetle As TreePlatformingLevelBeetle In Me.beetles
				If treePlatformingLevelBeetle.onCamera Then
					treePlatformingLevelBeetle.PlayIdleSFX()
				End If
			Next
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003AEC RID: 15084
	Private beetles As TreePlatformingLevelBeetle()
End Class
