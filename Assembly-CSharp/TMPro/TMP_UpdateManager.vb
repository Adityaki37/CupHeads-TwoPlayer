Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

Namespace TMPro
	' Token: 0x02000C92 RID: 3218
	Public Class TMP_UpdateManager
		' Token: 0x06005144 RID: 20804 RVA: 0x00298A18 File Offset: 0x00296E18
		Protected Sub New()
			Camera.onPreRender = CType([Delegate].Combine(Camera.onPreRender, AddressOf Me.OnCameraPreRender), Camera.CameraCallback)
		End Sub

		' Token: 0x170008A0 RID: 2208
		' (get) Token: 0x06005145 RID: 20805 RVA: 0x00298A77 File Offset: 0x00296E77
		Public Shared ReadOnly Property instance As TMP_UpdateManager
			Get
				If TMP_UpdateManager.s_Instance Is Nothing Then
					TMP_UpdateManager.s_Instance = New TMP_UpdateManager()
				End If
				Return TMP_UpdateManager.s_Instance
			End Get
		End Property

		' Token: 0x06005146 RID: 20806 RVA: 0x00298A92 File Offset: 0x00296E92
		Public Shared Sub RegisterTextElementForLayoutRebuild(element As TMP_Text)
			TMP_UpdateManager.instance.InternalRegisterTextElementForLayoutRebuild(element)
		End Sub

		' Token: 0x06005147 RID: 20807 RVA: 0x00298AA0 File Offset: 0x00296EA0
		Private Function InternalRegisterTextElementForLayoutRebuild(element As TMP_Text) As Boolean
			Dim instanceID As Integer = element.GetInstanceID()
			If Me.m_LayoutQueueLookup.ContainsKey(instanceID) Then
				Return False
			End If
			Me.m_LayoutQueueLookup(instanceID) = instanceID
			Me.m_LayoutRebuildQueue.Add(element)
			Return True
		End Function

		' Token: 0x06005148 RID: 20808 RVA: 0x00298AE1 File Offset: 0x00296EE1
		Public Shared Sub RegisterTextElementForGraphicRebuild(element As TMP_Text)
			TMP_UpdateManager.instance.InternalRegisterTextElementForGraphicRebuild(element)
		End Sub

		' Token: 0x06005149 RID: 20809 RVA: 0x00298AF0 File Offset: 0x00296EF0
		Private Function InternalRegisterTextElementForGraphicRebuild(element As TMP_Text) As Boolean
			Dim instanceID As Integer = element.GetInstanceID()
			If Me.m_GraphicQueueLookup.ContainsKey(instanceID) Then
				Return False
			End If
			Me.m_GraphicQueueLookup(instanceID) = instanceID
			Me.m_GraphicRebuildQueue.Add(element)
			Return True
		End Function

		' Token: 0x0600514A RID: 20810 RVA: 0x00298B34 File Offset: 0x00296F34
		Private Sub OnCameraPreRender(cam As Camera)
			For i As Integer = 0 To Me.m_LayoutRebuildQueue.Count - 1
				Me.m_LayoutRebuildQueue(i).Rebuild(CanvasUpdate.Prelayout)
			Next
			If Me.m_LayoutRebuildQueue.Count > 0 Then
				Me.m_LayoutRebuildQueue.Clear()
				Me.m_LayoutQueueLookup.Clear()
			End If
			For j As Integer = 0 To Me.m_GraphicRebuildQueue.Count - 1
				Me.m_GraphicRebuildQueue(j).Rebuild(CanvasUpdate.PreRender)
			Next
			If Me.m_GraphicRebuildQueue.Count > 0 Then
				Me.m_GraphicRebuildQueue.Clear()
				Me.m_GraphicQueueLookup.Clear()
			End If
		End Sub

		' Token: 0x0600514B RID: 20811 RVA: 0x00298BEB File Offset: 0x00296FEB
		Public Shared Sub UnRegisterTextElementForRebuild(element As TMP_Text)
			TMP_UpdateManager.instance.InternalUnRegisterTextElementForGraphicRebuild(element)
			TMP_UpdateManager.instance.InternalUnRegisterTextElementForLayoutRebuild(element)
		End Sub

		' Token: 0x0600514C RID: 20812 RVA: 0x00298C04 File Offset: 0x00297004
		Private Sub InternalUnRegisterTextElementForGraphicRebuild(element As TMP_Text)
			Dim instanceID As Integer = element.GetInstanceID()
			TMP_UpdateManager.instance.m_GraphicRebuildQueue.Remove(element)
			Me.m_GraphicQueueLookup.Remove(instanceID)
		End Sub

		' Token: 0x0600514D RID: 20813 RVA: 0x00298C38 File Offset: 0x00297038
		Private Sub InternalUnRegisterTextElementForLayoutRebuild(element As TMP_Text)
			Dim instanceID As Integer = element.GetInstanceID()
			TMP_UpdateManager.instance.m_LayoutRebuildQueue.Remove(element)
			Me.m_LayoutQueueLookup.Remove(instanceID)
		End Sub

		' Token: 0x040053EE RID: 21486
		Private Shared s_Instance As TMP_UpdateManager

		' Token: 0x040053EF RID: 21487
		Private m_LayoutRebuildQueue As List(Of TMP_Text) = New List(Of TMP_Text)()

		' Token: 0x040053F0 RID: 21488
		Private m_LayoutQueueLookup As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)()

		' Token: 0x040053F1 RID: 21489
		Private m_GraphicRebuildQueue As List(Of TMP_Text) = New List(Of TMP_Text)()

		' Token: 0x040053F2 RID: 21490
		Private m_GraphicQueueLookup As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)()
	End Class
End Namespace
