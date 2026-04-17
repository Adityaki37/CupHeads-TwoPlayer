Imports System

Namespace UnityEngine.PostProcessing
	' Token: 0x02000BA3 RID: 2979
	Public NotInheritable Class DitheringComponent
		Inherits PostProcessingComponentRenderTexture(Of DitheringModel)

		' Token: 0x1700064E RID: 1614
		' (get) Token: 0x06004864 RID: 18532 RVA: 0x00260502 File Offset: 0x0025E902
		Public Overrides ReadOnly Property active As Boolean
			Get
				Return MyBase.model.enabled AndAlso Not Me.context.interrupted
			End Get
		End Property

		' Token: 0x06004865 RID: 18533 RVA: 0x00260525 File Offset: 0x0025E925
		Public Overrides Sub OnDisable()
			Me.noiseTextures = Nothing
		End Sub

		' Token: 0x06004866 RID: 18534 RVA: 0x00260530 File Offset: 0x0025E930
		Private Sub LoadNoiseTextures()
			Me.noiseTextures = New Texture2D(63) {}
			For i As Integer = 0 To 64 - 1
				Me.noiseTextures(i) = Resources.Load(Of Texture2D)("Bluenoise64/LDR_LLL1_" + i)
			Next
		End Sub

		' Token: 0x06004867 RID: 18535 RVA: 0x0026057C File Offset: 0x0025E97C
		Public Overrides Sub Prepare(uberMaterial As Material)
			Dim num As Integer = Me.textureIndex + 1
			Dim num2 As Integer = num
			Me.textureIndex = num
			If num2 >= 64 Then
				Me.textureIndex = 0
			End If
			Dim value As Single = Random.value
			Dim value2 As Single = Random.value
			If Me.noiseTextures Is Nothing Then
				Me.LoadNoiseTextures()
			End If
			Dim texture2D As Texture2D = Me.noiseTextures(Me.textureIndex)
			uberMaterial.EnableKeyword("DITHERING")
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D)
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, New Vector4(CSng(Me.context.width) / CSng(texture2D.width), CSng(Me.context.height) / CSng(texture2D.height), value, value2))
		End Sub

		' Token: 0x04004DD9 RID: 19929
		Private noiseTextures As Texture2D()

		' Token: 0x04004DDA RID: 19930
		Private textureIndex As Integer

		' Token: 0x04004DDB RID: 19931
		Private Const k_TextureCount As Integer = 64

		' Token: 0x02000BA4 RID: 2980
		Private NotInheritable Class Uniforms
			' Token: 0x04004DDC RID: 19932
			Friend Shared _DitheringTex As Integer = Shader.PropertyToID("_DitheringTex")

			' Token: 0x04004DDD RID: 19933
			Friend Shared _DitheringCoords As Integer = Shader.PropertyToID("_DitheringCoords")
		End Class
	End Class
End Namespace
