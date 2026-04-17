Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C47 RID: 3143
	<AddComponentMenu("")>
	Public Class UIControlSet
		Inherits MonoBehaviour

		' Token: 0x170007C3 RID: 1987
		' (get) Token: 0x06004D49 RID: 19785 RVA: 0x00275990 File Offset: 0x00273D90
		Private ReadOnly Property controls As Dictionary(Of Integer, UIControl)
			Get
				Dim controls As Dictionary(Of Integer, UIControl) = Me._controls
				Dim dictionary As Dictionary(Of Integer, UIControl) = controls
				If controls Is Nothing Then
					Dim dictionary2 As Dictionary(Of Integer, UIControl) = New Dictionary(Of Integer, UIControl)()
					Dim dictionary3 As Dictionary(Of Integer, UIControl) = dictionary2
					Me._controls = dictionary2
					dictionary = dictionary3
				End If
				Return dictionary
			End Get
		End Property

		' Token: 0x06004D4A RID: 19786 RVA: 0x002759B8 File Offset: 0x00273DB8
		Public Sub SetTitle(text As String)
			If Me.title Is Nothing Then
				Return
			End If
			Me.title.text = text
		End Sub

		' Token: 0x06004D4B RID: 19787 RVA: 0x002759D8 File Offset: 0x00273DD8
		Public Function GetControl(Of T As UIControl)(uniqueId As Integer) As T
			Dim uicontrol As UIControl
			Me.controls.TryGetValue(uniqueId, uicontrol)
			Return TryCast(uicontrol, T)
		End Function

		' Token: 0x06004D4C RID: 19788 RVA: 0x00275A00 File Offset: 0x00273E00
		Public Function CreateSlider(prefab As GameObject, icon As Sprite, minValue As Single, maxValue As Single, valueChangedCallback As Action(Of Integer, Single), cancelCallback As Action(Of Integer)) As UISliderControl
			Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(prefab)
			Dim control As UISliderControl = gameObject.GetComponent(Of UISliderControl)()
			If control Is Nothing Then
				Global.UnityEngine.[Object].Destroy(gameObject)
				Global.UnityEngine.Debug.LogError("Prefab missing UISliderControl component!")
				Return Nothing
			End If
			gameObject.transform.SetParent(MyBase.transform, False)
			If control.iconImage IsNot Nothing Then
				control.iconImage.sprite = icon
			End If
			If control.slider IsNot Nothing Then
				control.slider.minValue = minValue
				control.slider.maxValue = maxValue
				If valueChangedCallback IsNot Nothing Then
					control.slider.onValueChanged.AddListener(Sub(value As Single)
						valueChangedCallback(control.id, value)
					End Sub)
				End If
				If cancelCallback IsNot Nothing Then
					control.SetCancelCallback(Sub()
						cancelCallback(control.id)
					End Sub)
				End If
			End If
			Me.controls.Add(control.id, control)
			Return control
		End Function

		' Token: 0x04005189 RID: 20873
		<SerializeField()>
		Private title As Text

		' Token: 0x0400518A RID: 20874
		Private _controls As Dictionary(Of Integer, UIControl)
	End Class
End Namespace
