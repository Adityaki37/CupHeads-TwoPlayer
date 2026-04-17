Imports System
Imports UnityEngine

' Token: 0x02000905 RID: 2309
Public Class PlatformingLevelIntroAnimation
	Inherits AbstractLevelHUDComponent

	' Token: 0x0600362A RID: 13866 RVA: 0x001F73F8 File Offset: 0x001F57F8
	Public Shared Function Create(callback As Action) As PlatformingLevelIntroAnimation
		Dim platformingLevelIntroAnimation As PlatformingLevelIntroAnimation = Global.UnityEngine.[Object].Instantiate(Of PlatformingLevelIntroAnimation)(Level.Current.LevelResources.platformingIntro)
		platformingLevelIntroAnimation.callback = callback
		Return platformingLevelIntroAnimation
	End Function

	' Token: 0x0600362B RID: 13867 RVA: 0x001F7422 File Offset: 0x001F5822
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me._parentToHudCanvas = True
		MyBase.transform.SetParent(Camera.main.transform, False)
		MyBase.transform.ResetLocalTransforms()
	End Sub

	' Token: 0x0600362C RID: 13868 RVA: 0x001F7452 File Offset: 0x001F5852
	Private Sub StartLevel()
		If Me.callback IsNot Nothing Then
			Me.callback()
		End If
	End Sub

	' Token: 0x0600362D RID: 13869 RVA: 0x001F746A File Offset: 0x001F586A
	Private Sub OnAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600362E RID: 13870 RVA: 0x001F7477 File Offset: 0x001F5877
	Public Sub Play()
		MyBase.GetComponent(Of Animator)().Play("Intro")
	End Sub

	' Token: 0x04003E2B RID: 15915
	Private callback As Action
End Class
