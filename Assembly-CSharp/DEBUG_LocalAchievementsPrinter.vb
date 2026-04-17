Imports System
Imports System.Collections.Generic
Imports System.Text
Imports UnityEngine

' Token: 0x02000B3D RID: 2877
Public Class DEBUG_LocalAchievementsPrinter
	Inherits MonoBehaviour

	' Token: 0x060045AD RID: 17837 RVA: 0x0024C177 File Offset: 0x0024A577
	Private Sub Awake()
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x060045AE RID: 17838 RVA: 0x0024C184 File Offset: 0x0024A584
	Private Sub OnGUI()
		If Time.frameCount < 120 Then
			Return
		End If
		Dim unlockedAchievements As IList(Of LocalAchievementsManager.Achievement) = LocalAchievementsManager.GetUnlockedAchievements()
		For Each text As String In DEBUG_LocalAchievementsPrinter.AllAchievements
			Dim flag As Boolean = unlockedAchievements.Contains(CType([Enum].Parse(GetType(LocalAchievementsManager.Achievement), text), LocalAchievementsManager.Achievement))
			Me.builder.AppendFormat("{0}....{1}" & vbLf, If((Not flag), "L", "U"), text)
		Next
		Dim guistyle As GUIStyle = New GUIStyle(GUI.skin.GetStyle("Box"))
		guistyle.alignment = TextAnchor.UpperLeft
		GUI.Box(New Rect(0F, 0F, 200F, 500F), Me.builder.ToString(), guistyle)
		Me.builder.Length = 0
	End Sub

	' Token: 0x04004BDA RID: 19418
	Private Shared AllAchievements As String() = [Enum].GetNames(GetType(LocalAchievementsManager.Achievement))

	' Token: 0x04004BDB RID: 19419
	Private builder As StringBuilder = New StringBuilder()
End Class
