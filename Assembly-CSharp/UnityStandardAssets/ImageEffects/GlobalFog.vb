Imports System
Imports UnityEngine

Namespace UnityStandardAssets.ImageEffects
	' Token: 0x02000CDA RID: 3290
	<ExecuteInEditMode()>
	<RequireComponent(GetType(Camera))>
	<AddComponentMenu("Image Effects/Rendering/Global Fog")>
	Friend Class GlobalFog
		Inherits PostEffectsBase

		' Token: 0x0600521C RID: 21020 RVA: 0x002A19E8 File Offset: 0x0029FDE8
		Public Overrides Function CheckResources() As Boolean
			MyBase.CheckSupport(True)
			Me.fogMaterial = MyBase.CheckShaderAndCreateMaterial(Me.fogShader, Me.fogMaterial)
			If Not Me.isSupported Then
				MyBase.ReportAutoDisable()
			End If
			Return Me.isSupported
		End Function

		' Token: 0x0600521D RID: 21021 RVA: 0x002A1A24 File Offset: 0x0029FE24
		<ImageEffectOpaque()>
		Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
			If Not Me.CheckResources() OrElse (Not Me.distanceFog AndAlso Not Me.heightFog) Then
				Graphics.Blit(source, destination)
				Return
			End If
			Dim component As Camera = MyBase.GetComponent(Of Camera)()
			Dim transform As Transform = component.transform
			Dim nearClipPlane As Single = component.nearClipPlane
			Dim farClipPlane As Single = component.farClipPlane
			Dim fieldOfView As Single = component.fieldOfView
			Dim aspect As Single = component.aspect
			Dim identity As Matrix4x4 = Matrix4x4.identity
			Dim num As Single = fieldOfView * 0.5F
			Dim vector As Vector3 = transform.right * nearClipPlane * Mathf.Tan(num * 0.017453292F) * aspect
			Dim vector2 As Vector3 = transform.up * nearClipPlane * Mathf.Tan(num * 0.017453292F)
			Dim vector3 As Vector3 = transform.forward * nearClipPlane - vector + vector2
			Dim num2 As Single = vector3.magnitude * farClipPlane / nearClipPlane
			vector3.Normalize()
			vector3 *= num2
			Dim vector4 As Vector3 = transform.forward * nearClipPlane + vector + vector2
			vector4.Normalize()
			vector4 *= num2
			Dim vector5 As Vector3 = transform.forward * nearClipPlane + vector - vector2
			vector5.Normalize()
			vector5 *= num2
			Dim vector6 As Vector3 = transform.forward * nearClipPlane - vector - vector2
			vector6.Normalize()
			vector6 *= num2
			identity.SetRow(0, vector3)
			identity.SetRow(1, vector4)
			identity.SetRow(2, vector5)
			identity.SetRow(3, vector6)
			Dim position As Vector3 = transform.position
			Dim num3 As Single = position.y - Me.height
			Dim num4 As Single = If((num3 > 0F), 0F, 1F)
			Me.fogMaterial.SetMatrix("_FrustumCornersWS", identity)
			Me.fogMaterial.SetVector("_CameraWS", position)
			Me.fogMaterial.SetVector("_HeightParams", New Vector4(Me.height, num3, num4, Me.heightDensity * 0.5F))
			Me.fogMaterial.SetVector("_DistanceParams", New Vector4(-Mathf.Max(Me.startDistance, 0F), 0F, 0F, 0F))
			Dim fogMode As FogMode = RenderSettings.fogMode
			Dim fogDensity As Single = RenderSettings.fogDensity
			Dim fogStartDistance As Single = RenderSettings.fogStartDistance
			Dim fogEndDistance As Single = RenderSettings.fogEndDistance
			Dim flag As Boolean = fogMode = FogMode.Linear
			Dim num5 As Single = If((Not flag), 0F, (fogEndDistance - fogStartDistance))
			Dim num6 As Single = If((Mathf.Abs(num5) <= 0.0001F), 0F, (1F / num5))
			Dim vector7 As Vector4
			vector7.x = fogDensity * 1.2011224F
			vector7.y = fogDensity * 1.442695F
			vector7.z = If((Not flag), 0F, (-num6))
			vector7.w = If((Not flag), 0F, (fogEndDistance * num6))
			Me.fogMaterial.SetVector("_SceneFogParams", vector7)
			Me.fogMaterial.SetVector("_SceneFogMode", New Vector4(CSng(fogMode), CSng(If((Not Me.useRadialDistance), 0, 1)), 0F, 0F))
			Dim num7 As Integer
			If Me.distanceFog AndAlso Me.heightFog Then
				num7 = 0
			ElseIf Me.distanceFog Then
				num7 = 1
			Else
				num7 = 2
			End If
			GlobalFog.CustomGraphicsBlit(source, destination, Me.fogMaterial, num7)
		End Sub

		' Token: 0x0600521E RID: 21022 RVA: 0x002A1DE0 File Offset: 0x002A01E0
		Private Shared Sub CustomGraphicsBlit(source As RenderTexture, dest As RenderTexture, fxMaterial As Material, passNr As Integer)
			RenderTexture.active = dest
			fxMaterial.SetTexture("_MainTex", source)
			GL.PushMatrix()
			GL.LoadOrtho()
			fxMaterial.SetPass(passNr)
			GL.Begin(7)
			GL.MultiTexCoord2(0, 0F, 0F)
			GL.Vertex3(0F, 0F, 3F)
			GL.MultiTexCoord2(0, 1F, 0F)
			GL.Vertex3(1F, 0F, 2F)
			GL.MultiTexCoord2(0, 1F, 1F)
			GL.Vertex3(1F, 1F, 1F)
			GL.MultiTexCoord2(0, 0F, 1F)
			GL.Vertex3(0F, 1F, 0F)
			GL.[End]()
			GL.PopMatrix()
		End Sub

		' Token: 0x0400567A RID: 22138
		<Tooltip("Apply distance-based fog?")>
		Public distanceFog As Boolean = True

		' Token: 0x0400567B RID: 22139
		<Tooltip("Distance fog is based on radial distance from camera when checked")>
		Public useRadialDistance As Boolean

		' Token: 0x0400567C RID: 22140
		<Tooltip("Apply height-based fog?")>
		Public heightFog As Boolean = True

		' Token: 0x0400567D RID: 22141
		<Tooltip("Fog top Y coordinate")>
		Public height As Single = 1F

		' Token: 0x0400567E RID: 22142
		<Range(0.001F, 10F)>
		Public heightDensity As Single = 2F

		' Token: 0x0400567F RID: 22143
		<Tooltip("Push fog away from the camera by this amount")>
		Public startDistance As Single

		' Token: 0x04005680 RID: 22144
		Public fogShader As Shader

		' Token: 0x04005681 RID: 22145
		Private fogMaterial As Material
	End Class
End Namespace
