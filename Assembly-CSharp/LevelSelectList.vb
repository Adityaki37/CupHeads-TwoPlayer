Imports System
Imports System.Collections.Generic
Imports RektTransform
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x020009A5 RID: 2469
Public Class LevelSelectList
	Inherits AbstractMonoBehaviour

	' Token: 0x060039F2 RID: 14834 RVA: 0x0020F188 File Offset: 0x0020D588
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.SetupList()
	End Sub

	' Token: 0x060039F3 RID: 14835 RVA: 0x0020F198 File Offset: 0x0020D598
	Private Sub SetupList()
		Dim list As List(Of Scenes) = New List(Of Scenes)()
		For Each scenes As Scenes In EnumUtils.GetValues(Of Scenes)()
			If Me.GetSceneGroup(scenes).included Then
				list.Add(scenes)
			End If
		Next
		Dim num As Integer = 0
		For Each scenes2 As Scenes In list
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.button.gameObject)
			Dim b As Button = gameObject.GetComponent(Of Button)()
			Dim text As String = scenes2.ToString().Replace("scene_", String.Empty).Replace("level_", String.Empty).Replace("dice_palace_", String.Empty).Replace("platforming_", String.Empty)
			b.name = scenes2.ToString()
			gameObject.GetComponentInChildren(Of Text)().text = text
			b.onClick.AddListener(Sub()
				SceneLoader.LoadScene(b.name, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, Nothing)
			End Sub)
			b.transform.SetParent(Me.button.transform.parent)
			b.transform.ResetLocalTransforms()
			num += 1
		Next
		Me.button.gameObject.SetActive(False)
		Me.contentPanel.SetHeight(30F * CSng(num))
	End Sub

	' Token: 0x060039F4 RID: 14836 RVA: 0x0020F34C File Offset: 0x0020D74C
	Public Function GetSceneGroup(s As Scenes) As LevelSelectList.SceneGroup
		For Each sceneGroup As LevelSelectList.SceneGroup In Me.scenes
			If sceneGroup.scene = s Then
				Return sceneGroup
			End If
		Next
		Return New LevelSelectList.SceneGroup()
	End Function

	' Token: 0x060039F5 RID: 14837 RVA: 0x0020F38C File Offset: 0x0020D78C
	Public Function ContainsScene(s As Scenes) As Boolean
		For Each sceneGroup As LevelSelectList.SceneGroup In Me.scenes
			If sceneGroup.scene = s Then
				Return True
			End If
		Next
		Return False
	End Function

	' Token: 0x040041DA RID: 16858
	<HideInInspector()>
	Public scenes As LevelSelectList.SceneGroup()

	' Token: 0x040041DB RID: 16859
	Public button As Button

	' Token: 0x040041DC RID: 16860
	Public contentPanel As RectTransform

	' Token: 0x020009A6 RID: 2470
	<Serializable()>
	Public Class SceneGroup
		' Token: 0x040041DD RID: 16861
		Public included As Boolean

		' Token: 0x040041DE RID: 16862
		Public scene As Scenes
	End Class
End Class
