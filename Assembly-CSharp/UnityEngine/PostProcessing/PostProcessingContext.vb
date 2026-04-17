Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BFC RID: 3068
	Public Class PostProcessingContext
		' Token: 0x1700068C RID: 1676
		' (get) Token: 0x06004942 RID: 18754 RVA: 0x002651FA File Offset: 0x002635FA
		' (set) Token: 0x06004943 RID: 18755 RVA: 0x00265202 File Offset: 0x00263602
		Public Property interrupted As Boolean

		' Token: 0x06004944 RID: 18756 RVA: 0x0026520B File Offset: 0x0026360B
		Public Sub Interrupt()
			Me.interrupted = True
		End Sub

		' Token: 0x06004945 RID: 18757 RVA: 0x00265214 File Offset: 0x00263614
		Public Function Reset() As PostProcessingContext
			Me.profile = Nothing
			Me.camera = Nothing
			Me.materialFactory = Nothing
			Me.renderTextureFactory = Nothing
			Me.interrupted = False
			Return Me
		End Function

		' Token: 0x1700068D RID: 1677
		' (get) Token: 0x06004946 RID: 18758 RVA: 0x0026523A File Offset: 0x0026363A
		Public ReadOnly Property isGBufferAvailable As Boolean
			Get
				Return Me.camera.actualRenderingPath = RenderingPath.DeferredShading
			End Get
		End Property

		' Token: 0x1700068E RID: 1678
		' (get) Token: 0x06004947 RID: 18759 RVA: 0x0026524A File Offset: 0x0026364A
		Public ReadOnly Property isHdr As Boolean
			Get
				Return Me.camera.allowHDR
			End Get
		End Property

		' Token: 0x1700068F RID: 1679
		' (get) Token: 0x06004948 RID: 18760 RVA: 0x00265257 File Offset: 0x00263657
		Public ReadOnly Property width As Integer
			Get
				Return Me.camera.pixelWidth
			End Get
		End Property

		' Token: 0x17000690 RID: 1680
		' (get) Token: 0x06004949 RID: 18761 RVA: 0x00265264 File Offset: 0x00263664
		Public ReadOnly Property height As Integer
			Get
				Return Me.camera.pixelHeight
			End Get
		End Property

		' Token: 0x17000691 RID: 1681
		' (get) Token: 0x0600494A RID: 18762 RVA: 0x00265271 File Offset: 0x00263671
		Public ReadOnly Property viewport As Rect
			Get
				Return Me.camera.rect
			End Get
		End Property

		' Token: 0x04004F5F RID: 20319
		Public profile As PostProcessingProfile

		' Token: 0x04004F60 RID: 20320
		Public camera As Camera

		' Token: 0x04004F61 RID: 20321
		Public materialFactory As MaterialFactory

		' Token: 0x04004F62 RID: 20322
		Public renderTextureFactory As RenderTextureFactory
	End Class
End Namespace
