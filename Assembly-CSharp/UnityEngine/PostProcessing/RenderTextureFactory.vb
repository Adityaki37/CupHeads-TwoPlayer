Imports System
Imports System.Collections.Generic

Namespace UnityEngine.PostProcessing
	' Token: 0x02000C02 RID: 3074
	Public NotInheritable Class RenderTextureFactory
		Implements IDisposable

		' Token: 0x0600495F RID: 18783 RVA: 0x00265917 File Offset: 0x00263D17
		Public Sub New()
			Me.m_TemporaryRTs = New HashSet(Of RenderTexture)()
		End Sub

		' Token: 0x06004960 RID: 18784 RVA: 0x0026592C File Offset: 0x00263D2C
		Public Function [Get](baseRenderTexture As RenderTexture) As RenderTexture
			Return Me.[Get](baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, If((Not baseRenderTexture.sRGB), RenderTextureReadWrite.Linear, RenderTextureReadWrite.sRGB), baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture")
		End Function

		' Token: 0x06004961 RID: 18785 RVA: 0x0026597C File Offset: 0x00263D7C
		Public Function [Get](width As Integer, height As Integer, Optional depthBuffer As Integer = 0, Optional format As RenderTextureFormat = RenderTextureFormat.ARGBHalf, Optional rw As RenderTextureReadWrite = RenderTextureReadWrite.[Default], Optional filterMode As FilterMode = FilterMode.Bilinear, Optional wrapMode As TextureWrapMode = TextureWrapMode.Clamp, Optional name As String = "FactoryTempTexture") As RenderTexture
			Dim temporary As RenderTexture = RenderTexture.GetTemporary(width, height, depthBuffer, format, rw)
			temporary.filterMode = filterMode
			temporary.wrapMode = wrapMode
			temporary.name = name
			Me.m_TemporaryRTs.Add(temporary)
			Return temporary
		End Function

		' Token: 0x06004962 RID: 18786 RVA: 0x002659BC File Offset: 0x00263DBC
		Public Sub Release(rt As RenderTexture)
			If rt Is Nothing Then
				Return
			End If
			If Not Me.m_TemporaryRTs.Contains(rt) Then
				Throw New ArgumentException(String.Format("Attempting to remove a RenderTexture that was not allocated: {0}", rt))
			End If
			Me.m_TemporaryRTs.Remove(rt)
			RenderTexture.ReleaseTemporary(rt)
		End Sub

		' Token: 0x06004963 RID: 18787 RVA: 0x00265A0C File Offset: 0x00263E0C
		Public Sub ReleaseAll()
			For Each renderTexture As RenderTexture In Me.m_TemporaryRTs
				RenderTexture.ReleaseTemporary(renderTexture)
			Next
			Me.m_TemporaryRTs.Clear()
		End Sub

		' Token: 0x06004964 RID: 18788 RVA: 0x00265A4D File Offset: 0x00263E4D
		Public Sub Dispose() Implements System.IDisposable.Dispose
			Me.ReleaseAll()
		End Sub

		' Token: 0x04004F7C RID: 20348
		Private m_TemporaryRTs As HashSet(Of RenderTexture)
	End Class
End Namespace
