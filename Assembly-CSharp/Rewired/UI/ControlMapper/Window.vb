Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.Events
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C4F RID: 3151
	<AddComponentMenu("")>
	<RequireComponent(GetType(CanvasGroup))>
	Public Class Window
		Inherits MonoBehaviour

		' Token: 0x170007C8 RID: 1992
		' (get) Token: 0x06004D6B RID: 19819 RVA: 0x00265D73 File Offset: 0x00264173
		Public ReadOnly Property hasFocus As Boolean
			Get
				Return Me._isFocusedCallback IsNot Nothing AndAlso Me._isFocusedCallback(Me._id)
			End Get
		End Property

		' Token: 0x170007C9 RID: 1993
		' (get) Token: 0x06004D6C RID: 19820 RVA: 0x00265D97 File Offset: 0x00264197
		Public ReadOnly Property id As Integer
			Get
				Return Me._id
			End Get
		End Property

		' Token: 0x170007CA RID: 1994
		' (get) Token: 0x06004D6D RID: 19821 RVA: 0x00265D9F File Offset: 0x0026419F
		Public ReadOnly Property rectTransform As RectTransform
			Get
				If Me._rectTransform Is Nothing Then
					Me._rectTransform = MyBase.gameObject.GetComponent(Of RectTransform)()
				End If
				Return Me._rectTransform
			End Get
		End Property

		' Token: 0x170007CB RID: 1995
		' (get) Token: 0x06004D6E RID: 19822 RVA: 0x00265DC9 File Offset: 0x002641C9
		Public ReadOnly Property titleText As Text
			Get
				Return Me._titleText
			End Get
		End Property

		' Token: 0x170007CC RID: 1996
		' (get) Token: 0x06004D6F RID: 19823 RVA: 0x00265DD1 File Offset: 0x002641D1
		Public ReadOnly Property contentText As List(Of Text)
			Get
				Return Me._contentText
			End Get
		End Property

		' Token: 0x170007CD RID: 1997
		' (get) Token: 0x06004D70 RID: 19824 RVA: 0x00265DD9 File Offset: 0x002641D9
		' (set) Token: 0x06004D71 RID: 19825 RVA: 0x00265DE1 File Offset: 0x002641E1
		Public Property defaultUIElement As GameObject
			Get
				Return Me._defaultUIElement
			End Get
			Set(value As GameObject)
				Me._defaultUIElement = value
			End Set
		End Property

		' Token: 0x170007CE RID: 1998
		' (get) Token: 0x06004D72 RID: 19826 RVA: 0x00265DEA File Offset: 0x002641EA
		' (set) Token: 0x06004D73 RID: 19827 RVA: 0x00265DF2 File Offset: 0x002641F2
		Public Property updateCallback As Action(Of Integer)
			Get
				Return Me._updateCallback
			End Get
			Set(value As Action(Of Integer))
				Me._updateCallback = value
			End Set
		End Property

		' Token: 0x170007CF RID: 1999
		' (get) Token: 0x06004D74 RID: 19828 RVA: 0x00265DFB File Offset: 0x002641FB
		Public ReadOnly Property timer As Window.Timer
			Get
				Return Me._timer
			End Get
		End Property

		' Token: 0x170007D0 RID: 2000
		' (get) Token: 0x06004D75 RID: 19829 RVA: 0x00265E04 File Offset: 0x00264204
		' (set) Token: 0x06004D76 RID: 19830 RVA: 0x00265E28 File Offset: 0x00264228
		Public Property width As Integer
			Get
				Return CInt(Me.rectTransform.sizeDelta.x)
			End Get
			Set(value As Integer)
				Dim sizeDelta As Vector2 = Me.rectTransform.sizeDelta
				sizeDelta.x = CSng(value)
				Me.rectTransform.sizeDelta = sizeDelta
			End Set
		End Property

		' Token: 0x170007D1 RID: 2001
		' (get) Token: 0x06004D77 RID: 19831 RVA: 0x00265E58 File Offset: 0x00264258
		' (set) Token: 0x06004D78 RID: 19832 RVA: 0x00265E7C File Offset: 0x0026427C
		Public Property height As Integer
			Get
				Return CInt(Me.rectTransform.sizeDelta.y)
			End Get
			Set(value As Integer)
				Dim sizeDelta As Vector2 = Me.rectTransform.sizeDelta
				sizeDelta.y = CSng(value)
				Me.rectTransform.sizeDelta = sizeDelta
			End Set
		End Property

		' Token: 0x170007D2 RID: 2002
		' (get) Token: 0x06004D79 RID: 19833 RVA: 0x00265EAA File Offset: 0x002642AA
		Protected ReadOnly Property initialized As Boolean
			Get
				Return Me._initialized
			End Get
		End Property

		' Token: 0x06004D7A RID: 19834 RVA: 0x00265EB2 File Offset: 0x002642B2
		Private Sub OnEnable()
			MyBase.StartCoroutine("OnEnableAsync")
		End Sub

		' Token: 0x06004D7B RID: 19835 RVA: 0x00265EC0 File Offset: 0x002642C0
		Protected Overridable Sub Update()
			If Not Me._initialized Then
				Return
			End If
			If Not Me.hasFocus Then
				Return
			End If
			Me.CheckUISelection()
			If Me._updateCallback IsNot Nothing Then
				Me._updateCallback(Me._id)
			End If
		End Sub

		' Token: 0x06004D7C RID: 19836 RVA: 0x00265EFC File Offset: 0x002642FC
		Public Overridable Sub Initialize(id As Integer, isFocusedCallback As Func(Of Integer, Boolean))
			If Me._initialized Then
				Global.UnityEngine.Debug.LogError("Window is already initialized!")
				Return
			End If
			Me._id = id
			Me._isFocusedCallback = isFocusedCallback
			Me._timer = New Window.Timer()
			Me._contentText = New List(Of Text)()
			Me._canvasGroup = MyBase.GetComponent(Of CanvasGroup)()
			Me._initialized = True
		End Sub

		' Token: 0x06004D7D RID: 19837 RVA: 0x00265F56 File Offset: 0x00264356
		Public Sub SetSize(width As Integer, height As Integer)
			Me.rectTransform.sizeDelta = New Vector2(CSng(width), CSng(height))
		End Sub

		' Token: 0x06004D7E RID: 19838 RVA: 0x00265F6C File Offset: 0x0026436C
		Public Sub CreateTitleText(prefab As GameObject, offset As Vector2)
			Me.CreateText(prefab, Me._titleText, "Title Text", UIPivot.TopCenter, UIAnchor.TopHStretch, offset)
		End Sub

		' Token: 0x06004D7F RID: 19839 RVA: 0x00265F8B File Offset: 0x0026438B
		Public Sub CreateTitleText(prefab As GameObject, offset As Vector2, text As String)
			Me.CreateTitleText(prefab, offset)
			Me.SetTitleText(text)
		End Sub

		' Token: 0x06004D80 RID: 19840 RVA: 0x00265F9C File Offset: 0x0026439C
		Public Sub AddContentText(prefab As GameObject, pivot As UIPivot, anchor As UIAnchor, offset As Vector2)
			Dim text As Text = Nothing
			Me.CreateText(prefab, text, "Content Text", pivot, anchor, offset)
			Me._contentText.Add(text)
		End Sub

		' Token: 0x06004D81 RID: 19841 RVA: 0x00265FC9 File Offset: 0x002643C9
		Public Sub AddContentText(prefab As GameObject, pivot As UIPivot, anchor As UIAnchor, offset As Vector2, text As String)
			Me.AddContentText(prefab, pivot, anchor, offset, text, 0)
		End Sub

		' Token: 0x06004D82 RID: 19842 RVA: 0x00265FD9 File Offset: 0x002643D9
		Public Sub AddContentText(prefab As GameObject, pivot As UIPivot, anchor As UIAnchor, offset As Vector2, text As String, fontSize As Integer)
			Me.AddContentText(prefab, pivot, anchor, offset)
			Me.SetContentText(text, fontSize, Me._contentText.Count - 1)
		End Sub

		' Token: 0x06004D83 RID: 19843 RVA: 0x00265FFD File Offset: 0x002643FD
		Public Sub AddContentImage(prefab As GameObject, pivot As UIPivot, anchor As UIAnchor, offset As Vector2)
			Me.CreateImage(prefab, "Image", pivot, anchor, offset)
		End Sub

		' Token: 0x06004D84 RID: 19844 RVA: 0x0026600F File Offset: 0x0026440F
		Public Sub AddContentImage(prefab As GameObject, pivot As UIPivot, anchor As UIAnchor, offset As Vector2, text As String)
			Me.AddContentImage(prefab, pivot, anchor, offset)
		End Sub

		' Token: 0x06004D85 RID: 19845 RVA: 0x0026601C File Offset: 0x0026441C
		Public Sub CreateButton(prefab As GameObject, pivot As UIPivot, anchor As UIAnchor, offset As Vector2, buttonText As String, confirmCallback As UnityAction, cancelCallback As UnityAction, setDefault As Boolean)
			If prefab Is Nothing Then
				Return
			End If
			Dim buttonInfo As ButtonInfo
			Dim gameObject As GameObject = Me.CreateButton(prefab, "Button", anchor, pivot, offset, buttonInfo)
			If gameObject Is Nothing Then
				Return
			End If
			Dim component As Button = gameObject.GetComponent(Of Button)()
			If confirmCallback IsNot Nothing Then
				component.onClick.AddListener(confirmCallback)
			End If
			Dim customButton As CustomButton = TryCast(component, CustomButton)
			If cancelCallback IsNot Nothing AndAlso customButton IsNot Nothing Then
				AddHandler customButton.CancelEvent, cancelCallback
			End If
			If buttonInfo.text IsNot Nothing Then
				buttonInfo.text.text = buttonText
			End If
			If setDefault Then
				Me._defaultUIElement = gameObject
			End If
		End Sub

		' Token: 0x06004D86 RID: 19846 RVA: 0x002660BF File Offset: 0x002644BF
		Public Function GetTitleText(text As String) As String
			If Me._titleText Is Nothing Then
				Return String.Empty
			End If
			Return Me._titleText.text
		End Function

		' Token: 0x06004D87 RID: 19847 RVA: 0x002660E3 File Offset: 0x002644E3
		Public Sub SetTitleText(text As String)
			If Me._titleText Is Nothing Then
				Return
			End If
			Me._titleText.text = text
		End Sub

		' Token: 0x06004D88 RID: 19848 RVA: 0x00266104 File Offset: 0x00264504
		Public Function GetContentText(index As Integer) As String
			If Me._contentText Is Nothing OrElse Me._contentText.Count <= index OrElse Me._contentText(index) Is Nothing Then
				Return String.Empty
			End If
			Return Me._contentText(index).text
		End Function

		' Token: 0x06004D89 RID: 19849 RVA: 0x0026615C File Offset: 0x0026455C
		Public Function GetContentTextHeight(index As Integer) As Single
			If Me._contentText Is Nothing OrElse Me._contentText.Count <= index OrElse Me._contentText(index) Is Nothing Then
				Return 0F
			End If
			Return Me._contentText(index).rectTransform.sizeDelta.y
		End Function

		' Token: 0x06004D8A RID: 19850 RVA: 0x002661C0 File Offset: 0x002645C0
		Public Sub SetContentText(text As String, fontsize As Integer, index As Integer)
			Me.SetContentText(text, index)
			If fontsize > 0 Then
				Me._contentText(index).fontSize = fontsize
			End If
		End Sub

		' Token: 0x06004D8B RID: 19851 RVA: 0x002661E4 File Offset: 0x002645E4
		Public Sub SetContentText(text As String, index As Integer)
			If Me._contentText Is Nothing OrElse Me._contentText.Count <= index OrElse Me._contentText(index) Is Nothing Then
				Return
			End If
			Me._contentText(index).text = text
		End Sub

		' Token: 0x06004D8C RID: 19852 RVA: 0x00266237 File Offset: 0x00264637
		Public Sub SetUpdateCallback(callback As Action(Of Integer))
			Me.updateCallback = callback
		End Sub

		' Token: 0x06004D8D RID: 19853 RVA: 0x00266240 File Offset: 0x00264640
		Public Overridable Sub TakeInputFocus()
			If EventSystem.current Is Nothing Then
				Return
			End If
			EventSystem.current.SetSelectedGameObject(Me._defaultUIElement)
			Me.Enable()
		End Sub

		' Token: 0x06004D8E RID: 19854 RVA: 0x00266269 File Offset: 0x00264669
		Public Overridable Sub Enable()
			Me._canvasGroup.interactable = True
		End Sub

		' Token: 0x06004D8F RID: 19855 RVA: 0x00266277 File Offset: 0x00264677
		Public Overridable Sub Disable()
			Me._canvasGroup.interactable = False
		End Sub

		' Token: 0x06004D90 RID: 19856 RVA: 0x00266285 File Offset: 0x00264685
		Public Overridable Sub Cancel()
			If Not Me.initialized Then
				Return
			End If
			If Me.cancelCallback IsNot Nothing Then
				Me.cancelCallback()
			End If
		End Sub

		' Token: 0x06004D91 RID: 19857 RVA: 0x002662AC File Offset: 0x002646AC
		Private Sub CreateText(prefab As GameObject, ByRef textComponent As Text, name As String, pivot As UIPivot, anchor As UIAnchor, offset As Vector2)
			If prefab Is Nothing OrElse Me.content Is Nothing Then
				Return
			End If
			If textComponent IsNot Nothing Then
				Global.UnityEngine.Debug.LogError("Window already has " + name + "!")
				Return
			End If
			Dim gameObject As GameObject = UITools.InstantiateGUIObject(Of Text)(prefab, Me.content.transform, name, pivot, anchor.min, anchor.max, offset)
			If gameObject Is Nothing Then
				Return
			End If
			textComponent = gameObject.GetComponent(Of Text)()
		End Sub

		' Token: 0x06004D92 RID: 19858 RVA: 0x0026633C File Offset: 0x0026473C
		Private Sub CreateImage(prefab As GameObject, name As String, pivot As UIPivot, anchor As UIAnchor, offset As Vector2)
			If prefab Is Nothing OrElse Me.content Is Nothing Then
				Return
			End If
			UITools.InstantiateGUIObject(Of Image)(prefab, Me.content.transform, name, pivot, anchor.min, anchor.max, offset)
		End Sub

		' Token: 0x06004D93 RID: 19859 RVA: 0x00266390 File Offset: 0x00264790
		Private Function CreateButton(prefab As GameObject, name As String, anchor As UIAnchor, pivot As UIPivot, offset As Vector2, <System.Runtime.InteropServices.OutAttribute()> ByRef buttonInfo As ButtonInfo) As GameObject
			buttonInfo = Nothing
			If prefab Is Nothing Then
				Return Nothing
			End If
			Dim gameObject As GameObject = UITools.InstantiateGUIObject(Of ButtonInfo)(prefab, Me.content.transform, name, pivot, anchor.min, anchor.max, offset)
			If gameObject Is Nothing Then
				Return Nothing
			End If
			buttonInfo = gameObject.GetComponent(Of ButtonInfo)()
			Dim component As Button = gameObject.GetComponent(Of Button)()
			If component Is Nothing Then
				Global.UnityEngine.Debug.Log("Button prefab is missing Button component!")
				Return Nothing
			End If
			If buttonInfo Is Nothing Then
				Global.UnityEngine.Debug.Log("Button prefab is missing ButtonInfo component!")
				Return Nothing
			End If
			Return gameObject
		End Function

		' Token: 0x06004D94 RID: 19860 RVA: 0x0026642C File Offset: 0x0026482C
		Private Iterator Function OnEnableAsync() As IEnumerator
			Yield 1
			If EventSystem.current Is Nothing Then
				Return
			End If
			If Me.defaultUIElement IsNot Nothing Then
				EventSystem.current.SetSelectedGameObject(Me.defaultUIElement)
			Else
				EventSystem.current.SetSelectedGameObject(Nothing)
			End If
			Return
		End Function

		' Token: 0x06004D95 RID: 19861 RVA: 0x00266448 File Offset: 0x00264848
		Private Sub CheckUISelection()
			If Not Me.hasFocus Then
				Return
			End If
			If EventSystem.current Is Nothing Then
				Return
			End If
			If EventSystem.current.currentSelectedGameObject Is Nothing Then
				Me.RestoreDefaultOrLastUISelection()
			End If
			Me.lastUISelection = EventSystem.current.currentSelectedGameObject
		End Sub

		' Token: 0x06004D96 RID: 19862 RVA: 0x002664A0 File Offset: 0x002648A0
		Private Sub RestoreDefaultOrLastUISelection()
			If Not Me.hasFocus Then
				Return
			End If
			If Me.lastUISelection Is Nothing OrElse Not Me.lastUISelection.activeInHierarchy Then
				Me.SetUISelection(Me._defaultUIElement)
				Return
			End If
			Me.SetUISelection(Me.lastUISelection)
		End Sub

		' Token: 0x06004D97 RID: 19863 RVA: 0x002664F3 File Offset: 0x002648F3
		Private Sub SetUISelection(selection As GameObject)
			If EventSystem.current Is Nothing Then
				Return
			End If
			EventSystem.current.SetSelectedGameObject(selection)
		End Sub

		' Token: 0x04005199 RID: 20889
		Public backgroundImage As Image

		' Token: 0x0400519A RID: 20890
		Public content As GameObject

		' Token: 0x0400519B RID: 20891
		Private _initialized As Boolean

		' Token: 0x0400519C RID: 20892
		Private _id As Integer = -1

		' Token: 0x0400519D RID: 20893
		Private _rectTransform As RectTransform

		' Token: 0x0400519E RID: 20894
		Private _titleText As Text

		' Token: 0x0400519F RID: 20895
		Private _contentText As List(Of Text)

		' Token: 0x040051A0 RID: 20896
		Private _defaultUIElement As GameObject

		' Token: 0x040051A1 RID: 20897
		Private _updateCallback As Action(Of Integer)

		' Token: 0x040051A2 RID: 20898
		Private _isFocusedCallback As Func(Of Integer, Boolean)

		' Token: 0x040051A3 RID: 20899
		Private _timer As Window.Timer

		' Token: 0x040051A4 RID: 20900
		Private _canvasGroup As CanvasGroup

		' Token: 0x040051A5 RID: 20901
		Public cancelCallback As UnityAction

		' Token: 0x040051A6 RID: 20902
		Private lastUISelection As GameObject

		' Token: 0x02000C50 RID: 3152
		Public Class Timer
			' Token: 0x170007D3 RID: 2003
			' (get) Token: 0x06004D99 RID: 19865 RVA: 0x00266519 File Offset: 0x00264919
			Public ReadOnly Property started As Boolean
				Get
					Return Me._started
				End Get
			End Property

			' Token: 0x170007D4 RID: 2004
			' (get) Token: 0x06004D9A RID: 19866 RVA: 0x00266521 File Offset: 0x00264921
			Public ReadOnly Property finished As Boolean
				Get
					If Not Me.started Then
						Return False
					End If
					If Time.realtimeSinceStartup < Me.[end] Then
						Return False
					End If
					Me._started = False
					Return True
				End Get
			End Property

			' Token: 0x170007D5 RID: 2005
			' (get) Token: 0x06004D9B RID: 19867 RVA: 0x0026654A File Offset: 0x0026494A
			Public ReadOnly Property remaining As Single
				Get
					If Not Me._started Then
						Return 0F
					End If
					Return Me.[end] - Time.realtimeSinceStartup
				End Get
			End Property

			' Token: 0x06004D9C RID: 19868 RVA: 0x00266569 File Offset: 0x00264969
			Public Sub Start(length As Single)
				Me.[end] = Time.realtimeSinceStartup + length
				Me._started = True
			End Sub

			' Token: 0x06004D9D RID: 19869 RVA: 0x0026657F File Offset: 0x0026497F
			Public Sub [Stop]()
				Me._started = False
			End Sub

			' Token: 0x040051A7 RID: 20903
			Private _started As Boolean

			' Token: 0x040051A8 RID: 20904
			Private [end] As Single
		End Class
	End Class
End Namespace
