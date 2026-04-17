Imports System
Imports UnityEngine

' Token: 0x020004A5 RID: 1189
Public Class LevelIntroAnimation
	Inherits AbstractLevelHUDComponent

	' Token: 0x06001363 RID: 4963 RVA: 0x000AB468 File Offset: 0x000A9868
	Public Shared Function Create(callback As Action) As LevelIntroAnimation
		Dim levelIntroAnimation As LevelIntroAnimation = Global.UnityEngine.[Object].Instantiate(Of LevelIntroAnimation)(Level.Current.LevelResources.levelIntro)
		levelIntroAnimation.callback = callback
		Return levelIntroAnimation
	End Function

	' Token: 0x06001364 RID: 4964 RVA: 0x000AB494 File Offset: 0x000A9894
	Public Shared Function CreateCustom(prefab As LevelIntroAnimation, callback As Action) As LevelIntroAnimation
		Dim levelIntroAnimation As LevelIntroAnimation = Global.UnityEngine.[Object].Instantiate(Of LevelIntroAnimation)(prefab)
		levelIntroAnimation.callback = callback
		Return levelIntroAnimation
	End Function

	' Token: 0x06001365 RID: 4965 RVA: 0x000AB4B0 File Offset: 0x000A98B0
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me._parentToHudCanvas = True
		MyBase.transform.SetParent(Camera.main.transform, False)
		MyBase.transform.ResetLocalTransforms()
	End Sub

	' Token: 0x06001366 RID: 4966 RVA: 0x000AB4E0 File Offset: 0x000A98E0
	Private Sub StartLevel()
		If Me.callback IsNot Nothing Then
			Me.callback()
		End If
	End Sub

	' Token: 0x06001367 RID: 4967 RVA: 0x000AB4F8 File Offset: 0x000A98F8
	Private Sub OnAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001368 RID: 4968 RVA: 0x000AB505 File Offset: 0x000A9905
	Public Sub Play()
		MyBase.GetComponent(Of Animator)().Play("Intro")
	End Sub

	' Token: 0x04001C7C RID: 7292
	Private callback As Action
End Class
