Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.EventSystems
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C4D RID: 3149
	<AddComponentMenu("")>
	Public Class UISliderControl
		Inherits UIControl

		' Token: 0x170007C6 RID: 1990
		' (get) Token: 0x06004D5F RID: 19807 RVA: 0x00275F17 File Offset: 0x00274317
		' (set) Token: 0x06004D60 RID: 19808 RVA: 0x00275F1F File Offset: 0x0027431F
		Public Property showIcon As Boolean
			Get
				Return Me._showIcon
			End Get
			Set(value As Boolean)
				If Me.iconImage Is Nothing Then
					Return
				End If
				Me.iconImage.gameObject.SetActive(value)
				Me._showIcon = value
			End Set
		End Property

		' Token: 0x170007C7 RID: 1991
		' (get) Token: 0x06004D61 RID: 19809 RVA: 0x00275F4B File Offset: 0x0027434B
		' (set) Token: 0x06004D62 RID: 19810 RVA: 0x00275F53 File Offset: 0x00274353
		Public Property showSlider As Boolean
			Get
				Return Me._showSlider
			End Get
			Set(value As Boolean)
				If Me.slider Is Nothing Then
					Return
				End If
				Me.slider.gameObject.SetActive(value)
				Me._showSlider = value
			End Set
		End Property

		' Token: 0x06004D63 RID: 19811 RVA: 0x00275F80 File Offset: 0x00274380
		Public Overrides Sub SetCancelCallback(cancelCallback As Action)
			MyBase.SetCancelCallback(cancelCallback)
			If cancelCallback Is Nothing OrElse Me.slider Is Nothing Then
				Return
			End If
			If TypeOf Me.slider Is ICustomSelectable Then
				AddHandler TryCast(Me.slider, ICustomSelectable).CancelEvent, Sub()
					cancelCallback()
				End Sub
			Else
				Dim eventTrigger As EventTrigger = Me.slider.GetComponent(Of EventTrigger)()
				If eventTrigger Is Nothing Then
					eventTrigger = Me.slider.gameObject.AddComponent(Of EventTrigger)()
				End If
				Dim entry As EventTrigger.Entry = New EventTrigger.Entry()
				entry.callback = New EventTrigger.TriggerEvent()
				entry.eventID = EventTriggerType.Cancel
				entry.callback.AddListener(Sub(data As BaseEventData)
					cancelCallback()
				End Sub)
				If eventTrigger.triggers Is Nothing Then
					eventTrigger.triggers = New List(Of EventTrigger.Entry)()
				End If
				eventTrigger.triggers.Add(entry)
			End If
		End Sub

		' Token: 0x04005195 RID: 20885
		Public iconImage As Image

		' Token: 0x04005196 RID: 20886
		Public slider As Slider

		' Token: 0x04005197 RID: 20887
		Private _showIcon As Boolean

		' Token: 0x04005198 RID: 20888
		Private _showSlider As Boolean
	End Class
End Namespace
