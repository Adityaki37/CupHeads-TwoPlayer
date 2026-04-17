Imports System
Imports TMPro
Imports UnityEngine

' Token: 0x02000999 RID: 2457
Public Class MapBasicStartUI
	Inherits AbstractMapSceneStartUI

	' Token: 0x170004A9 RID: 1193
	' (get) Token: 0x06003974 RID: 14708 RVA: 0x00209EBB File Offset: 0x002082BB
	' (set) Token: 0x06003975 RID: 14709 RVA: 0x00209EC2 File Offset: 0x002082C2
	Public Shared Property Current As MapBasicStartUI

	' Token: 0x06003976 RID: 14710 RVA: 0x00209ECA File Offset: 0x002082CA
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MapBasicStartUI.Current = Me
	End Sub

	' Token: 0x06003977 RID: 14711 RVA: 0x00209ED8 File Offset: 0x002082D8
	Private Sub OnDestroy()
		If MapBasicStartUI.Current Is Me Then
			MapBasicStartUI.Current = Nothing
		End If
	End Sub

	' Token: 0x06003978 RID: 14712 RVA: 0x00209EF0 File Offset: 0x002082F0
	Private Sub UpdateCursor()
		Me.cursor.transform.position = Me.enter.transform.position
		Me.cursor.sizeDelta = New Vector2(Me.enter.sizeDelta.x + 30F, Me.enter.sizeDelta.y + 20F)
	End Sub

	' Token: 0x06003979 RID: 14713 RVA: 0x00209F5F File Offset: 0x0020835F
	Private Sub Update()
		Me.UpdateCursor()
		If MyBase.CurrentState = AbstractMapSceneStartUI.State.Active Then
			Me.CheckInput()
		End If
	End Sub

	' Token: 0x0600397A RID: 14714 RVA: 0x00209F79 File Offset: 0x00208379
	Private Sub CheckInput()
		If Not MyBase.Able Then
			Return
		End If
		If MyBase.GetButtonDown(CupheadButton.Cancel) Then
			MyBase.Out()
		End If
		If MyBase.GetButtonDown(CupheadButton.Accept) Then
			MyBase.LoadLevel()
		End If
	End Sub

	' Token: 0x0600397B RID: 14715 RVA: 0x00209FAD File Offset: 0x002083AD
	Public Sub [In](playerController As MapPlayerController)
		MyBase.[In](playerController)
		If Me.Animator IsNot Nothing Then
			Me.Animator.SetTrigger("ZoomIn")
			AudioManager.Play("world_map_level_menu_open")
		End If
		Me.InitUI(Me.level)
	End Sub

	' Token: 0x0600397C RID: 14716 RVA: 0x00209FF0 File Offset: 0x002083F0
	Public Sub InitUI(level As String)
		Dim translationElement As TranslationElement = Localization.Find(level)
		If translationElement IsNot Nothing Then
			Me.Title.GetComponent(Of LocalizationHelper)().ApplyTranslation(translationElement, Nothing)
			If Localization.language = Localization.Languages.Japanese Then
				Me.Title.lineSpacing = 0F
			Else
				Me.Title.lineSpacing = 17.46F
			End If
		End If
	End Sub

	' Token: 0x0400411A RID: 16666
	Public Animator As Animator

	' Token: 0x0400411B RID: 16667
	Public Title As TMP_Text

	' Token: 0x0400411C RID: 16668
	<SerializeField()>
	Private cursor As RectTransform

	' Token: 0x0400411D RID: 16669
	<Header("Options")>
	<SerializeField()>
	Private enter As RectTransform
End Class
