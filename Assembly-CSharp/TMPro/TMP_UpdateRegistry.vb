Imports System
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

Namespace TMPro
	' Token: 0x02000C93 RID: 3219
	Public Class TMP_UpdateRegistry
		' Token: 0x0600514E RID: 20814 RVA: 0x00298C6C File Offset: 0x0029706C
		Protected Sub New()
			AddHandler Canvas.willRenderCanvases, AddressOf Me.PerformUpdateForCanvasRendererObjects
		End Sub

		' Token: 0x170008A1 RID: 2209
		' (get) Token: 0x0600514F RID: 20815 RVA: 0x00298CBC File Offset: 0x002970BC
		Public Shared ReadOnly Property instance As TMP_UpdateRegistry
			Get
				If TMP_UpdateRegistry.s_Instance Is Nothing Then
					TMP_UpdateRegistry.s_Instance = New TMP_UpdateRegistry()
				End If
				Return TMP_UpdateRegistry.s_Instance
			End Get
		End Property

		' Token: 0x06005150 RID: 20816 RVA: 0x00298CD7 File Offset: 0x002970D7
		Public Shared Sub RegisterCanvasElementForLayoutRebuild(element As ICanvasElement)
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForLayoutRebuild(element)
		End Sub

		' Token: 0x06005151 RID: 20817 RVA: 0x00298CE8 File Offset: 0x002970E8
		Private Function InternalRegisterCanvasElementForLayoutRebuild(element As ICanvasElement) As Boolean
			Dim instanceID As Integer = TryCast(element, Global.UnityEngine.[Object]).GetInstanceID()
			If Me.m_LayoutQueueLookup.ContainsKey(instanceID) Then
				Return False
			End If
			Me.m_LayoutQueueLookup(instanceID) = instanceID
			Me.m_LayoutRebuildQueue.Add(element)
			Return True
		End Function

		' Token: 0x06005152 RID: 20818 RVA: 0x00298D2E File Offset: 0x0029712E
		Public Shared Sub RegisterCanvasElementForGraphicRebuild(element As ICanvasElement)
			TMP_UpdateRegistry.instance.InternalRegisterCanvasElementForGraphicRebuild(element)
		End Sub

		' Token: 0x06005153 RID: 20819 RVA: 0x00298D3C File Offset: 0x0029713C
		Private Function InternalRegisterCanvasElementForGraphicRebuild(element As ICanvasElement) As Boolean
			Dim instanceID As Integer = TryCast(element, Global.UnityEngine.[Object]).GetInstanceID()
			If Me.m_GraphicQueueLookup.ContainsKey(instanceID) Then
				Return False
			End If
			Me.m_GraphicQueueLookup(instanceID) = instanceID
			Me.m_GraphicRebuildQueue.Add(element)
			Return True
		End Function

		' Token: 0x06005154 RID: 20820 RVA: 0x00298D84 File Offset: 0x00297184
		Private Sub PerformUpdateForCanvasRendererObjects()
			For i As Integer = 0 To Me.m_LayoutRebuildQueue.Count - 1
				Dim canvasElement As ICanvasElement = TMP_UpdateRegistry.instance.m_LayoutRebuildQueue(i)
				canvasElement.Rebuild(CanvasUpdate.Prelayout)
			Next
			If Me.m_LayoutRebuildQueue.Count > 0 Then
				Me.m_LayoutRebuildQueue.Clear()
				Me.m_LayoutQueueLookup.Clear()
			End If
			For j As Integer = 0 To Me.m_GraphicRebuildQueue.Count - 1
				Dim canvasElement2 As ICanvasElement = TMP_UpdateRegistry.instance.m_GraphicRebuildQueue(j)
				canvasElement2.Rebuild(CanvasUpdate.PreRender)
			Next
			If Me.m_GraphicRebuildQueue.Count > 0 Then
				Me.m_GraphicRebuildQueue.Clear()
				Me.m_GraphicQueueLookup.Clear()
			End If
		End Sub

		' Token: 0x06005155 RID: 20821 RVA: 0x00298E47 File Offset: 0x00297247
		Private Sub PerformUpdateForMeshRendererObjects()
		End Sub

		' Token: 0x06005156 RID: 20822 RVA: 0x00298E49 File Offset: 0x00297249
		Public Shared Sub UnRegisterCanvasElementForRebuild(element As ICanvasElement)
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForLayoutRebuild(element)
			TMP_UpdateRegistry.instance.InternalUnRegisterCanvasElementForGraphicRebuild(element)
		End Sub

		' Token: 0x06005157 RID: 20823 RVA: 0x00298E64 File Offset: 0x00297264
		Private Sub InternalUnRegisterCanvasElementForLayoutRebuild(element As ICanvasElement)
			Dim instanceID As Integer = TryCast(element, Global.UnityEngine.[Object]).GetInstanceID()
			TMP_UpdateRegistry.instance.m_LayoutRebuildQueue.Remove(element)
			Me.m_GraphicQueueLookup.Remove(instanceID)
		End Sub

		' Token: 0x06005158 RID: 20824 RVA: 0x00298E9C File Offset: 0x0029729C
		Private Sub InternalUnRegisterCanvasElementForGraphicRebuild(element As ICanvasElement)
			Dim instanceID As Integer = TryCast(element, Global.UnityEngine.[Object]).GetInstanceID()
			TMP_UpdateRegistry.instance.m_GraphicRebuildQueue.Remove(element)
			Me.m_LayoutQueueLookup.Remove(instanceID)
		End Sub

		' Token: 0x040053F3 RID: 21491
		Private Shared s_Instance As TMP_UpdateRegistry

		' Token: 0x040053F4 RID: 21492
		Private m_LayoutRebuildQueue As List(Of ICanvasElement) = New List(Of ICanvasElement)()

		' Token: 0x040053F5 RID: 21493
		Private m_LayoutQueueLookup As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)()

		' Token: 0x040053F6 RID: 21494
		Private m_GraphicRebuildQueue As List(Of ICanvasElement) = New List(Of ICanvasElement)()

		' Token: 0x040053F7 RID: 21495
		Private m_GraphicQueueLookup As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)()
	End Class
End Namespace
