Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000830 RID: 2096
Public Class ShmupTutorialExitSign
	Inherits AbstractLevelInteractiveEntity

	' Token: 0x060030A6 RID: 12454 RVA: 0x001C9FBE File Offset: 0x001C83BE
	Protected Overrides Sub Activate()
		If Me.activated Then
			Return
		End If
		MyBase.Activate()
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x060030A7 RID: 12455 RVA: 0x001C9FDF File Offset: 0x001C83DF
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 1F)
	End Sub

	' Token: 0x060030A8 RID: 12456 RVA: 0x001C9FF4 File Offset: 0x001C83F4
	Private Iterator Function go_cr() As IEnumerator
		Me.activated = True
		PlayerData.SaveCurrentFile()
		CupheadTime.SetLayerSpeed(CupheadTime.Layer.Player, 0F)
		For Each planePlayerController As PlanePlayerController In Global.UnityEngine.[Object].FindObjectsOfType(Of PlanePlayerController)()
			planePlayerController.PauseAll()
		Next
		For Each planeSuperBomb As PlaneSuperBomb In Global.UnityEngine.[Object].FindObjectsOfType(Of PlaneSuperBomb)()
			planeSuperBomb.Pause()
		Next
		For Each planeSuperChalice As PlaneSuperChalice In Global.UnityEngine.[Object].FindObjectsOfType(Of PlaneSuperChalice)()
			planeSuperChalice.Pause()
		Next
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		SceneLoader.LoadScene(Scenes.scene_map_world_1, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
		Return
	End Function

	' Token: 0x0400394C RID: 14668
	Private activated As Boolean
End Class
