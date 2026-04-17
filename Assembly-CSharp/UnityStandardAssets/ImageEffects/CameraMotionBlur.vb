Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CC6 RID: 3270
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Camera/Camera Motion Blur")>
	Public Class CameraMotionBlur
		Inherits PostEffectsBase

		' Token: 0x060051CC RID: 20940 RVA: 0x0029D6E4 File Offset: 0x0029BAE4
		Private Sub CalculateViewProjection()
			Dim worldToCameraMatrix As Matrix4x4 = Me._camera.worldToCameraMatrix
			Dim gpuprojectionMatrix As Matrix4x4 = GL.GetGPUProjectionMatrix(Me._camera.projectionMatrix, True)
			Me.currentViewProjMat = gpuprojectionMatrix * worldToCameraMatrix
		End Sub

		' Token: 0x060051CD RID: 20941 RVA: 0x0029D71C File Offset: 0x0029BB1C
		Private Sub Start()
			Me.CheckResources()
			If Me._camera Is Nothing Then
				Me._camera = MyBase.GetComponent(Of Camera)()
			End If
			Me.wasActive = MyBase.gameObject.activeInHierarchy
			Me.CalculateViewProjection()
			Me.Remember()
			Me.wasActive = False
		End Sub

		' Token: 0x060051CE RID: 20942 RVA: 0x0029D771 File Offset: 0x0029BB71
		Private Sub OnEnable()
			If Me._camera Is Nothing Then
				Me._camera = MyBase.GetComponent(Of Camera)()
			End If
			Me._camera.depthTextureMode = Me._camera.depthTextureMode Or DepthTextureMode.Depth
		End Sub

		' Token: 0x060051CF RID: 20943 RVA: 0x0029D7A4 File Offset: 0x0029BBA4
		Private Sub OnDisable()
			If Nothing IsNot Me.motionBlurMaterial Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.motionBlurMaterial)
				Me.motionBlurMaterial = Nothing
			End If
			If Nothing IsNot Me.dx11MotionBlurMaterial Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.dx11MotionBlurMaterial)
				Me.dx11MotionBlurMaterial = Nothing
			End If
			If Nothing IsNot Me.tmpCam Then
				Global.UnityEngine.[Object].DestroyImmediate(Me.tmpCam)
				Me.tmpCam = Nothing
			End If
		End Sub

		' Token: 0x060051D0 RID: 20944 RVA: 0x0029D81C File Offset: 0x0029BC1C
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(True, True)
			Me.motionBlurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.shader, Me.motionBlurMaterial)
			If Me.supportDX11 AndAlso Me.filterType = CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 Then
				Me.dx11MotionBlurMaterial = MyBase.CheckShaderAndCreateMaterial(Me.dx11MotionBlurShader, Me.dx11MotionBlurMaterial)
			End If
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x060051D1 RID: 20945 RVA: 0x0029D890 File Offset: 0x0029BC90
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() Then
				Graphics.Blit(source, destination)
				Return
			End If
			If Me.filterType = CameraMotionBlur.MotionBlurFilter.CameraMotion Then
				Me.StartFrame()
			End If
			Dim renderTextureFormat As RenderTextureFormat = If((Not SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.RGHalf)), RenderTextureFormat.ARGBHalf, RenderTextureFormat.RGHalf)
			Dim temporary As RenderTexture = RenderTexture.GetTemporary(CameraMotionBlur.divRoundUp(source.width, Me.velocityDownsample), CameraMotionBlur.divRoundUp(source.height, Me.velocityDownsample), 0, renderTextureFormat)
			Me.maxVelocity = Mathf.Max(2F, Me.maxVelocity)
			Dim num As Single = Me.maxVelocity
			Dim flag As Boolean = Me.filterType = CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 AndAlso Me.dx11MotionBlurMaterial Is Nothing
			Dim num2 As Integer
			Dim num3 As Integer
			If Me.filterType = CameraMotionBlur.MotionBlurFilter.Reconstruction OrElse flag OrElse Me.filterType = CameraMotionBlur.MotionBlurFilter.ReconstructionDisc Then
				Me.maxVelocity = Mathf.Min(Me.maxVelocity, CameraMotionBlur.MAX_RADIUS)
				num2 = CameraMotionBlur.divRoundUp(temporary.width, CInt(Me.maxVelocity))
				num3 = CameraMotionBlur.divRoundUp(temporary.height, CInt(Me.maxVelocity))
				num = CSng((temporary.width / num2))
			Else
				num2 = CameraMotionBlur.divRoundUp(temporary.width, CInt(Me.maxVelocity))
				num3 = CameraMotionBlur.divRoundUp(temporary.height, CInt(Me.maxVelocity))
				num = CSng((temporary.width / num2))
			End If
			Dim temporary2 As RenderTexture = RenderTexture.GetTemporary(num2, num3, 0, renderTextureFormat)
			Dim temporary3 As RenderTexture = RenderTexture.GetTemporary(num2, num3, 0, renderTextureFormat)
			temporary.filterMode = FilterMode.Point
			temporary2.filterMode = FilterMode.Point
			temporary3.filterMode = FilterMode.Point
			If Me.noiseTexture Then
				Me.noiseTexture.filterMode = FilterMode.Point
			End If
			source.wrapMode = TextureWrapMode.Clamp
			temporary.wrapMode = TextureWrapMode.Clamp
			temporary3.wrapMode = TextureWrapMode.Clamp
			temporary2.wrapMode = TextureWrapMode.Clamp
			Me.CalculateViewProjection()
			If MyBase.gameObject.activeInHierarchy AndAlso Not Me.wasActive Then
				Me.Remember()
			End If
			Me.wasActive = MyBase.gameObject.activeInHierarchy
			Dim matrix4x As Matrix4x4 = Matrix4x4.Inverse(Me.currentViewProjMat)
			Me.motionBlurMaterial.SetMatrix("_InvViewProj", matrix4x)
			Me.motionBlurMaterial.SetMatrix("_PrevViewProj", Me.prevViewProjMat)
			Me.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", Me.prevViewProjMat * matrix4x)
			Me.motionBlurMaterial.SetFloat("_MaxVelocity", num)
			Me.motionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num)
			Me.motionBlurMaterial.SetFloat("_MinVelocity", Me.minVelocity)
			Me.motionBlurMaterial.SetFloat("_VelocityScale", Me.velocityScale)
			Me.motionBlurMaterial.SetFloat("_Jitter", Me.jitter)
			Me.motionBlurMaterial.SetTexture("_NoiseTex", Me.noiseTexture)
			Me.motionBlurMaterial.SetTexture("_VelTex", temporary)
			Me.motionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3)
			Me.motionBlurMaterial.SetTexture("_TileTexDebug", temporary2)
			If Me.preview Then
				Dim worldToCameraMatrix As Matrix4x4 = Me._camera.worldToCameraMatrix
				Dim identity As Matrix4x4 = Matrix4x4.identity
				identity.SetTRS(Me.previewScale * 0.3333F, Quaternion.identity, Vector3.one)
				Dim gpuprojectionMatrix As Matrix4x4 = GL.GetGPUProjectionMatrix(Me._camera.projectionMatrix, True)
				Me.prevViewProjMat = gpuprojectionMatrix * identity * worldToCameraMatrix
				Me.motionBlurMaterial.SetMatrix("_PrevViewProj", Me.prevViewProjMat)
				Me.motionBlurMaterial.SetMatrix("_ToPrevViewProjCombined", Me.prevViewProjMat * matrix4x)
			End If
			If Me.filterType = CameraMotionBlur.MotionBlurFilter.CameraMotion Then
				Dim zero As Vector4 = Vector4.zero
				Dim num4 As Single = Vector3.Dot(MyBase.transform.up, Vector3.up)
				Dim vector As Vector3 = Me.prevFramePos - MyBase.transform.position
				Dim magnitude As Single = vector.magnitude
				Dim num5 As Single = Vector3.Angle(MyBase.transform.up, Me.prevFrameUp) / Me._camera.fieldOfView * (CSng(source.width) * 0.75F)
				zero.x = Me.rotationScale * num5
				num5 = Vector3.Angle(MyBase.transform.forward, Me.prevFrameForward) / Me._camera.fieldOfView * (CSng(source.width) * 0.75F)
				zero.y = Me.rotationScale * num4 * num5
				num5 = Vector3.Angle(MyBase.transform.forward, Me.prevFrameForward) / Me._camera.fieldOfView * (CSng(source.width) * 0.75F)
				zero.z = Me.rotationScale * (1F - num4) * num5
				If magnitude > Mathf.Epsilon AndAlso Me.movementScale > Mathf.Epsilon Then
					zero.w = Me.movementScale * Vector3.Dot(MyBase.transform.forward, vector) * (CSng(source.width) * 0.5F)
					zero.x += Me.movementScale * Vector3.Dot(MyBase.transform.up, vector) * (CSng(source.width) * 0.5F)
					zero.y += Me.movementScale * Vector3.Dot(MyBase.transform.right, vector) * (CSng(source.width) * 0.5F)
				End If
				If Me.preview Then
					Me.motionBlurMaterial.SetVector("_BlurDirectionPacked", New Vector4(Me.previewScale.y, Me.previewScale.x, 0F, Me.previewScale.z) * 0.5F * Me._camera.fieldOfView)
				Else
					Me.motionBlurMaterial.SetVector("_BlurDirectionPacked", zero)
				End If
			Else
				Graphics.Blit(source, temporary, Me.motionBlurMaterial, 0)
				Dim camera As Camera = Nothing
				If Me.excludeLayers.value <> 0 Then
					camera = Me.GetTmpCam()
				End If
				If camera AndAlso Me.excludeLayers.value <> 0 AndAlso Me.replacementClear AndAlso Me.replacementClear.isSupported Then
					camera.targetTexture = temporary
					camera.cullingMask = Me.excludeLayers
					camera.RenderWithShader(Me.replacementClear, String.Empty)
				End If
			End If
			If Not Me.preview AndAlso Time.frameCount <> Me.prevFrameCount Then
				Me.prevFrameCount = Time.frameCount
				Me.Remember()
			End If
			source.filterMode = FilterMode.Bilinear
			If Me.showVelocity Then
				Me.motionBlurMaterial.SetFloat("_DisplayVelocityScale", Me.showVelocityScale)
				Graphics.Blit(temporary, destination, Me.motionBlurMaterial, 1)
			ElseIf Me.filterType = CameraMotionBlur.MotionBlurFilter.ReconstructionDX11 AndAlso Not flag Then
				Me.dx11MotionBlurMaterial.SetFloat("_MinVelocity", Me.minVelocity)
				Me.dx11MotionBlurMaterial.SetFloat("_VelocityScale", Me.velocityScale)
				Me.dx11MotionBlurMaterial.SetFloat("_Jitter", Me.jitter)
				Me.dx11MotionBlurMaterial.SetTexture("_NoiseTex", Me.noiseTexture)
				Me.dx11MotionBlurMaterial.SetTexture("_VelTex", temporary)
				Me.dx11MotionBlurMaterial.SetTexture("_NeighbourMaxTex", temporary3)
				Me.dx11MotionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025F, Me.softZDistance))
				Me.dx11MotionBlurMaterial.SetFloat("_MaxRadiusOrKInPaper", num)
				Graphics.Blit(temporary, temporary2, Me.dx11MotionBlurMaterial, 0)
				Graphics.Blit(temporary2, temporary3, Me.dx11MotionBlurMaterial, 1)
				Graphics.Blit(source, destination, Me.dx11MotionBlurMaterial, 2)
			ElseIf Me.filterType = CameraMotionBlur.MotionBlurFilter.Reconstruction OrElse flag Then
				Me.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025F, Me.softZDistance))
				Graphics.Blit(temporary, temporary2, Me.motionBlurMaterial, 2)
				Graphics.Blit(temporary2, temporary3, Me.motionBlurMaterial, 3)
				Graphics.Blit(source, destination, Me.motionBlurMaterial, 4)
			ElseIf Me.filterType = CameraMotionBlur.MotionBlurFilter.CameraMotion Then
				Graphics.Blit(source, destination, Me.motionBlurMaterial, 6)
			ElseIf Me.filterType = CameraMotionBlur.MotionBlurFilter.ReconstructionDisc Then
				Me.motionBlurMaterial.SetFloat("_SoftZDistance", Mathf.Max(0.00025F, Me.softZDistance))
				Graphics.Blit(temporary, temporary2, Me.motionBlurMaterial, 2)
				Graphics.Blit(temporary2, temporary3, Me.motionBlurMaterial, 3)
				Graphics.Blit(source, destination, Me.motionBlurMaterial, 7)
			Else
				Graphics.Blit(source, destination, Me.motionBlurMaterial, 5)
			End If
			RenderTexture.ReleaseTemporary(temporary)
			RenderTexture.ReleaseTemporary(temporary2)
			RenderTexture.ReleaseTemporary(temporary3)
		End Sub

		' Token: 0x060051D2 RID: 20946 RVA: 0x0029E15C File Offset: 0x0029C55C
		Private Sub Remember()
			Me.prevViewProjMat = Me.currentViewProjMat
			Me.prevFrameForward = MyBase.transform.forward
			Me.prevFrameUp = MyBase.transform.up
			Me.prevFramePos = MyBase.transform.position
		End Sub

		' Token: 0x060051D3 RID: 20947 RVA: 0x0029E1A8 File Offset: 0x0029C5A8
		Private Function GetTmpCam() As Camera
			If Me.tmpCam Is Nothing Then
				Dim text As String = "_" + Me._camera.name + "_MotionBlurTmpCam"
				Dim gameObject As GameObject = GameObject.Find(text)
				If Nothing Is gameObject Then
					Me.tmpCam = New GameObject(text, New Type() { GetType(Camera) })
				Else
					Me.tmpCam = gameObject
				End If
			End If
			Me.tmpCam.hideFlags = HideFlags.DontSave
			Me.tmpCam.transform.position = Me._camera.transform.position
			Me.tmpCam.transform.rotation = Me._camera.transform.rotation
			Me.tmpCam.transform.localScale = Me._camera.transform.localScale
			Me.tmpCam.GetComponent(Of Camera)().CopyFrom(Me._camera)
			Me.tmpCam.GetComponent(Of Camera)().enabled = False
			Me.tmpCam.GetComponent(Of Camera)().depthTextureMode = DepthTextureMode.None
			Me.tmpCam.GetComponent(Of Camera)().clearFlags = CameraClearFlags.[Nothing]
			Return Me.tmpCam.GetComponent(Of Camera)()
		End Function

		' Token: 0x060051D4 RID: 20948 RVA: 0x0029E2E0 File Offset: 0x0029C6E0
		Private Sub StartFrame()
			Me.prevFramePos = Vector3.Slerp(Me.prevFramePos, MyBase.transform.position, 0.75F)
		End Sub

		' Token: 0x060051D5 RID: 20949 RVA: 0x0029E303 File Offset: 0x0029C703
		Private Shared Function divRoundUp(x As Integer, d As Integer) As Integer
			Return(x + d - 1) / d
		End Function

		' Token: 0x040055AB RID: 21931
		Private Shared MAX_RADIUS As Single = 10F

		' Token: 0x040055AC RID: 21932
		Public filterType As CameraMotionBlur.MotionBlurFilter = CameraMotionBlur.MotionBlurFilter.Reconstruction

		' Token: 0x040055AD RID: 21933
		Public preview As Boolean

		' Token: 0x040055AE RID: 21934
		Public previewScale As Vector3 = Vector3.one

		' Token: 0x040055AF RID: 21935
		Public movementScale As Single

		' Token: 0x040055B0 RID: 21936
		Public rotationScale As Single = 1F

		' Token: 0x040055B1 RID: 21937
		Public maxVelocity As Single = 8F

		' Token: 0x040055B2 RID: 21938
		Public minVelocity As Single = 0.1F

		' Token: 0x040055B3 RID: 21939
		Public velocityScale As Single = 0.375F

		' Token: 0x040055B4 RID: 21940
		Public softZDistance As Single = 0.005F

		' Token: 0x040055B5 RID: 21941
		Public velocityDownsample As Integer = 1

		' Token: 0x040055B6 RID: 21942
		Public excludeLayers As LayerMask = 0

		' Token: 0x040055B7 RID: 21943
		Private tmpCam As GameObject

		' Token: 0x040055B8 RID: 21944
		Public shader As Shader

		' Token: 0x040055B9 RID: 21945
		Public dx11MotionBlurShader As Shader

		' Token: 0x040055BA RID: 21946
		Public replacementClear As Shader

		' Token: 0x040055BB RID: 21947
		Private motionBlurMaterial As Material

		' Token: 0x040055BC RID: 21948
		Private dx11MotionBlurMaterial As Material

		' Token: 0x040055BD RID: 21949
		Public noiseTexture As Texture2D

		' Token: 0x040055BE RID: 21950
		Public jitter As Single = 0.05F

		' Token: 0x040055BF RID: 21951
		Public showVelocity As Boolean

		' Token: 0x040055C0 RID: 21952
		Public showVelocityScale As Single = 1F

		' Token: 0x040055C1 RID: 21953
		Private currentViewProjMat As Matrix4x4

		' Token: 0x040055C2 RID: 21954
		Private prevViewProjMat As Matrix4x4

		' Token: 0x040055C3 RID: 21955
		Private prevFrameCount As Integer

		' Token: 0x040055C4 RID: 21956
		Private wasActive As Boolean

		' Token: 0x040055C5 RID: 21957
		Private prevFrameForward As Vector3 = Vector3.forward

		' Token: 0x040055C6 RID: 21958
		Private prevFrameUp As Vector3 = Vector3.up

		' Token: 0x040055C7 RID: 21959
		Private prevFramePos As Vector3 = Vector3.zero

		' Token: 0x040055C8 RID: 21960
		Private _camera As Camera

		' Token: 0x02000CC7 RID: 3271
		Public Enum MotionBlurFilter
			' Token: 0x040055CA RID: 21962
			CameraMotion
			' Token: 0x040055CB RID: 21963
			LocalBlur
			' Token: 0x040055CC RID: 21964
			Reconstruction
			' Token: 0x040055CD RID: 21965
			ReconstructionDX11
			' Token: 0x040055CE RID: 21966
			ReconstructionDisc
		End Enum
	End Class
End Namespace
