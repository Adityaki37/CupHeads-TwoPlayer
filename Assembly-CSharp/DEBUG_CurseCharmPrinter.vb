Imports System
Imports UnityEngine

' Token: 0x02000B39 RID: 2873
Public Class DEBUG_CurseCharmPrinter
	Inherits MonoBehaviour

	' Token: 0x060045A0 RID: 17824 RVA: 0x0024BCF4 File Offset: 0x0024A0F4
	Private Sub Awake()
		Global.UnityEngine.[Object].DontDestroyOnLoad(MyBase.gameObject)
	End Sub

	' Token: 0x060045A1 RID: 17825 RVA: 0x0024BD04 File Offset: 0x0024A104
	Private Sub OnGUI()
		If Time.frameCount < 120 Then
			Return
		End If
		If Me.style Is Nothing Then
			Me.style = New GUIStyle(GUI.skin.GetStyle("Box"))
			Me.style.alignment = TextAnchor.UpperLeft
		End If
		If PlayerData.Data Is Nothing Then
			Return
		End If
		Dim text As String = String.Format("Curse: {0} / {1} / {2}", CharmCurse.CalculateLevel(PlayerId.PlayerOne), PlayerData.Data.CalculateCurseCharmAccumulatedValue(PlayerId.PlayerOne, CharmCurse.CountableLevels), CharmCurse.IsMaxLevel(PlayerId.PlayerOne))
		GUI.Box(New Rect(0F, 0F, 200F, 100F), text)
	End Sub

	' Token: 0x04004BCB RID: 19403
	Private style As GUIStyle
End Class
