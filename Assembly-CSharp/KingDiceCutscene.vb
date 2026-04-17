Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000407 RID: 1031
Public Class KingDiceCutscene
	Inherits Cutscene

	' Token: 0x06000E5C RID: 3676 RVA: 0x00092E24 File Offset: 0x00091224
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.input = New CupheadInput.AnyPlayerInput(False)
		CutsceneGUI.Current.pause.pauseAllowed = False
		If PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Normal) AndAlso PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Normal) AndAlso PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Normal) Then
			MyBase.StartCoroutine(Me.have_all_contracts_cr())
		Else
			MyBase.StartCoroutine(Me.missing_contracts_cr())
		End If
	End Sub

	' Token: 0x06000E5D RID: 3677 RVA: 0x00092EB4 File Offset: 0x000912B4
	Private Iterator Function have_all_contracts_cr() As IEnumerator
		MyBase.animator.Play("All_Contracts")
		Dim numScreens As Integer = 2
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		For i As Integer = 0 To numScreens - 1
			Yield CupheadTime.WaitForSeconds(Me, 1.25F)
			Me.arrowVisible = True
			While Not Me.input.GetAnyButtonDown()
				Yield Nothing
			End While
			Me.arrowVisible = False
			MyBase.animator.SetTrigger("Continue")
		Next
		SceneLoader.LoadScene(Scenes.scene_level_dice_palace_main, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Return
	End Function

	' Token: 0x06000E5E RID: 3678 RVA: 0x00092ED0 File Offset: 0x000912D0
	Private Iterator Function missing_contracts_cr() As IEnumerator
		MyBase.animator.Play("Missing_Contracts")
		Dim numScreens As Integer = 3
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		For i As Integer = 0 To numScreens - 1
			Yield CupheadTime.WaitForSeconds(Me, 1.25F)
			Me.arrowVisible = True
			While Not Me.input.GetAnyButtonDown()
				Yield Nothing
			End While
			Me.arrowVisible = False
			MyBase.animator.SetTrigger("Continue")
		Next
		SceneLoader.LoadLastMap()
		Yield Nothing
		Return
	End Function

	' Token: 0x06000E5F RID: 3679 RVA: 0x00092EEC File Offset: 0x000912EC
	Private Sub Update()
		If Me.arrowVisible Then
			Me.arrowTransparency = Mathf.Clamp01(Me.arrowTransparency + Time.deltaTime / 0.25F)
		Else
			Me.arrowTransparency = 0F
		End If
		Me.arrow.color = New Color(1F, 1F, 1F, Me.arrowTransparency)
	End Sub

	' Token: 0x06000E60 RID: 3680 RVA: 0x00092F56 File Offset: 0x00091356
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.arrow = Nothing
	End Sub

	' Token: 0x040017A2 RID: 6050
	Private input As CupheadInput.AnyPlayerInput

	' Token: 0x040017A3 RID: 6051
	<SerializeField()>
	Private arrow As Image

	' Token: 0x040017A4 RID: 6052
	Private arrowTransparency As Single

	' Token: 0x040017A5 RID: 6053
	Private arrowVisible As Boolean
End Class
