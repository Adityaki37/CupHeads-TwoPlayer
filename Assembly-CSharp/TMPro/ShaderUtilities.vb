Imports System
Imports System.Linq
Imports UnityEngine

Namespace TMPro
	' Token: 0x02000CB1 RID: 3249
	Public Module ShaderUtilities
		' Token: 0x0600519B RID: 20891 RVA: 0x0029A188 File Offset: 0x00298588
		Public Sub GetShaderPropertyIDs()
			If Not ShaderUtilities.isInitialized Then
				ShaderUtilities.isInitialized = True
				ShaderUtilities.ID_MainTex = Shader.PropertyToID("_MainTex")
				ShaderUtilities.ID_FaceTex = Shader.PropertyToID("_FaceTex")
				ShaderUtilities.ID_FaceColor = Shader.PropertyToID("_FaceColor")
				ShaderUtilities.ID_FaceDilate = Shader.PropertyToID("_FaceDilate")
				ShaderUtilities.ID_Shininess = Shader.PropertyToID("_FaceShininess")
				ShaderUtilities.ID_UnderlayColor = Shader.PropertyToID("_UnderlayColor")
				ShaderUtilities.ID_UnderlayOffsetX = Shader.PropertyToID("_UnderlayOffsetX")
				ShaderUtilities.ID_UnderlayOffsetY = Shader.PropertyToID("_UnderlayOffsetY")
				ShaderUtilities.ID_UnderlayDilate = Shader.PropertyToID("_UnderlayDilate")
				ShaderUtilities.ID_UnderlaySoftness = Shader.PropertyToID("_UnderlaySoftness")
				ShaderUtilities.ID_WeightNormal = Shader.PropertyToID("_WeightNormal")
				ShaderUtilities.ID_WeightBold = Shader.PropertyToID("_WeightBold")
				ShaderUtilities.ID_OutlineTex = Shader.PropertyToID("_OutlineTex")
				ShaderUtilities.ID_OutlineWidth = Shader.PropertyToID("_OutlineWidth")
				ShaderUtilities.ID_OutlineSoftness = Shader.PropertyToID("_OutlineSoftness")
				ShaderUtilities.ID_OutlineColor = Shader.PropertyToID("_OutlineColor")
				ShaderUtilities.ID_GradientScale = Shader.PropertyToID("_GradientScale")
				ShaderUtilities.ID_ScaleX = Shader.PropertyToID("_ScaleX")
				ShaderUtilities.ID_ScaleY = Shader.PropertyToID("_ScaleY")
				ShaderUtilities.ID_PerspectiveFilter = Shader.PropertyToID("_PerspectiveFilter")
				ShaderUtilities.ID_TextureWidth = Shader.PropertyToID("_TextureWidth")
				ShaderUtilities.ID_TextureHeight = Shader.PropertyToID("_TextureHeight")
				ShaderUtilities.ID_BevelAmount = Shader.PropertyToID("_Bevel")
				ShaderUtilities.ID_LightAngle = Shader.PropertyToID("_LightAngle")
				ShaderUtilities.ID_EnvMap = Shader.PropertyToID("_Cube")
				ShaderUtilities.ID_EnvMatrix = Shader.PropertyToID("_EnvMatrix")
				ShaderUtilities.ID_EnvMatrixRotation = Shader.PropertyToID("_EnvMatrixRotation")
				ShaderUtilities.ID_GlowColor = Shader.PropertyToID("_GlowColor")
				ShaderUtilities.ID_GlowOffset = Shader.PropertyToID("_GlowOffset")
				ShaderUtilities.ID_GlowPower = Shader.PropertyToID("_GlowPower")
				ShaderUtilities.ID_GlowOuter = Shader.PropertyToID("_GlowOuter")
				ShaderUtilities.ID_MaskCoord = Shader.PropertyToID("_MaskCoord")
				ShaderUtilities.ID_ClipRect = Shader.PropertyToID("_ClipRect")
				ShaderUtilities.ID_UseClipRect = Shader.PropertyToID("_UseClipRect")
				ShaderUtilities.ID_MaskSoftnessX = Shader.PropertyToID("_MaskSoftnessX")
				ShaderUtilities.ID_MaskSoftnessY = Shader.PropertyToID("_MaskSoftnessY")
				ShaderUtilities.ID_VertexOffsetX = Shader.PropertyToID("_VertexOffsetX")
				ShaderUtilities.ID_VertexOffsetY = Shader.PropertyToID("_VertexOffsetY")
				ShaderUtilities.ID_StencilID = Shader.PropertyToID("_Stencil")
				ShaderUtilities.ID_StencilOp = Shader.PropertyToID("_StencilOp")
				ShaderUtilities.ID_StencilComp = Shader.PropertyToID("_StencilComp")
				ShaderUtilities.ID_StencilReadMask = Shader.PropertyToID("_StencilReadMask")
				ShaderUtilities.ID_StencilWriteMask = Shader.PropertyToID("_StencilWriteMask")
				ShaderUtilities.ID_ShaderFlags = Shader.PropertyToID("_ShaderFlags")
				ShaderUtilities.ID_ScaleRatio_A = Shader.PropertyToID("_ScaleRatioA")
				ShaderUtilities.ID_ScaleRatio_B = Shader.PropertyToID("_ScaleRatioB")
				ShaderUtilities.ID_ScaleRatio_C = Shader.PropertyToID("_ScaleRatioC")
			End If
		End Sub

		' Token: 0x0600519C RID: 20892 RVA: 0x0029A468 File Offset: 0x00298868
		Public Sub UpdateShaderRatios(mat As Material, isBold As Boolean)
			Dim flag As Boolean = Not mat.shaderKeywords.Contains(ShaderUtilities.Keyword_Ratios)
			Dim float As Single = mat.GetFloat(ShaderUtilities.ID_GradientScale)
			Dim float2 As Single = mat.GetFloat(ShaderUtilities.ID_FaceDilate)
			Dim float3 As Single = mat.GetFloat(ShaderUtilities.ID_OutlineWidth)
			Dim float4 As Single = mat.GetFloat(ShaderUtilities.ID_OutlineSoftness)
			Dim num As Single = If(isBold, (mat.GetFloat(ShaderUtilities.ID_WeightBold) * 2F / float), (mat.GetFloat(ShaderUtilities.ID_WeightNormal) * 2F / float))
			Dim num2 As Single = Mathf.Max(1F, num + float2 + float3 + float4)
			Dim num3 As Single = If((Not flag), 1F, ((float - ShaderUtilities.m_clamp) / (float * num2)))
			mat.SetFloat(ShaderUtilities.ID_ScaleRatio_A, num3)
			If mat.HasProperty(ShaderUtilities.ID_GlowOffset) Then
				Dim float5 As Single = mat.GetFloat(ShaderUtilities.ID_GlowOffset)
				Dim float6 As Single = mat.GetFloat(ShaderUtilities.ID_GlowOuter)
				Dim num4 As Single = (num + float2) * (float - ShaderUtilities.m_clamp)
				num2 = Mathf.Max(1F, float5 + float6)
				Dim num5 As Single = If((Not flag), 1F, (Mathf.Max(0F, float - ShaderUtilities.m_clamp - num4) / (float * num2)))
				mat.SetFloat(ShaderUtilities.ID_ScaleRatio_B, num5)
			End If
			If mat.HasProperty(ShaderUtilities.ID_UnderlayOffsetX) Then
				Dim float7 As Single = mat.GetFloat(ShaderUtilities.ID_UnderlayOffsetX)
				Dim float8 As Single = mat.GetFloat(ShaderUtilities.ID_UnderlayOffsetY)
				Dim float9 As Single = mat.GetFloat(ShaderUtilities.ID_UnderlayDilate)
				Dim float10 As Single = mat.GetFloat(ShaderUtilities.ID_UnderlaySoftness)
				Dim num6 As Single = (num + float2) * (float - ShaderUtilities.m_clamp)
				num2 = Mathf.Max(1F, Mathf.Max(Mathf.Abs(float7), Mathf.Abs(float8)) + float9 + float10)
				Dim num7 As Single = If((Not flag), 1F, (Mathf.Max(0F, float - ShaderUtilities.m_clamp - num6) / (float * num2)))
				mat.SetFloat(ShaderUtilities.ID_ScaleRatio_C, num7)
			End If
		End Sub

		' Token: 0x0600519D RID: 20893 RVA: 0x0029A67A File Offset: 0x00298A7A
		Public Function GetFontExtent(material As Material) As Vector4
			Return Vector4.zero
		End Function

		' Token: 0x0600519E RID: 20894 RVA: 0x0029A684 File Offset: 0x00298A84
		Public Function IsMaskingEnabled(material As Material) As Boolean
			Return Not(material Is Nothing) AndAlso material.HasProperty(ShaderUtilities.ID_ClipRect) AndAlso (material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_SOFT) OrElse material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_HARD) OrElse material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_TEX))
		End Function

		' Token: 0x0600519F RID: 20895 RVA: 0x0029A6F4 File Offset: 0x00298AF4
		Public Function GetPadding(material As Material, enableExtraPadding As Boolean, isBold As Boolean) As Single
			If Not ShaderUtilities.isInitialized Then
				ShaderUtilities.GetShaderPropertyIDs()
			End If
			If material Is Nothing Then
				Return 0F
			End If
			Dim num As Integer = If((Not enableExtraPadding), 0, 4)
			If Not material.HasProperty(ShaderUtilities.ID_GradientScale) Then
				Return CSng(num)
			End If
			Dim vector As Vector4 = Vector4.zero
			Dim zero As Vector4 = Vector4.zero
			Dim num2 As Single = 0F
			Dim num3 As Single = 0F
			Dim num4 As Single = 0F
			Dim num5 As Single = 0F
			Dim num6 As Single = 0F
			Dim num7 As Single = 0F
			Dim num8 As Single = 0F
			Dim num9 As Single = 0F
			ShaderUtilities.UpdateShaderRatios(material, isBold)
			Dim shaderKeywords As String() = material.shaderKeywords
			If material.HasProperty(ShaderUtilities.ID_ScaleRatio_A) Then
				num5 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_A)
			End If
			If material.HasProperty(ShaderUtilities.ID_FaceDilate) Then
				num2 = material.GetFloat(ShaderUtilities.ID_FaceDilate) * num5
			End If
			If material.HasProperty(ShaderUtilities.ID_OutlineSoftness) Then
				num3 = material.GetFloat(ShaderUtilities.ID_OutlineSoftness) * num5
			End If
			If material.HasProperty(ShaderUtilities.ID_OutlineWidth) Then
				num4 = material.GetFloat(ShaderUtilities.ID_OutlineWidth) * num5
			End If
			Dim num10 As Single = num4 + num3 + num2
			If material.HasProperty(ShaderUtilities.ID_GlowOffset) AndAlso shaderKeywords.Contains(ShaderUtilities.Keyword_Glow) Then
				If material.HasProperty(ShaderUtilities.ID_ScaleRatio_B) Then
					num6 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_B)
				End If
				num8 = material.GetFloat(ShaderUtilities.ID_GlowOffset) * num6
				num9 = material.GetFloat(ShaderUtilities.ID_GlowOuter) * num6
			End If
			num10 = Mathf.Max(num10, num2 + num8 + num9)
			If material.HasProperty(ShaderUtilities.ID_UnderlaySoftness) AndAlso shaderKeywords.Contains(ShaderUtilities.Keyword_Underlay) Then
				If material.HasProperty(ShaderUtilities.ID_ScaleRatio_C) Then
					num7 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_C)
				End If
				Dim num11 As Single = material.GetFloat(ShaderUtilities.ID_UnderlayOffsetX) * num7
				Dim num12 As Single = material.GetFloat(ShaderUtilities.ID_UnderlayOffsetY) * num7
				Dim num13 As Single = material.GetFloat(ShaderUtilities.ID_UnderlayDilate) * num7
				Dim num14 As Single = material.GetFloat(ShaderUtilities.ID_UnderlaySoftness) * num7
				vector.x = Mathf.Max(vector.x, num2 + num13 + num14 - num11)
				vector.y = Mathf.Max(vector.y, num2 + num13 + num14 - num12)
				vector.z = Mathf.Max(vector.z, num2 + num13 + num14 + num11)
				vector.w = Mathf.Max(vector.w, num2 + num13 + num14 + num12)
			End If
			vector.x = Mathf.Max(vector.x, num10)
			vector.y = Mathf.Max(vector.y, num10)
			vector.z = Mathf.Max(vector.z, num10)
			vector.w = Mathf.Max(vector.w, num10)
			vector.x += CSng(num)
			vector.y += CSng(num)
			vector.z += CSng(num)
			vector.w += CSng(num)
			vector.x = Mathf.Min(vector.x, 1F)
			vector.y = Mathf.Min(vector.y, 1F)
			vector.z = Mathf.Min(vector.z, 1F)
			vector.w = Mathf.Min(vector.w, 1F)
			zero.x = If((zero.x >= vector.x), zero.x, vector.x)
			zero.y = If((zero.y >= vector.y), zero.y, vector.y)
			zero.z = If((zero.z >= vector.z), zero.z, vector.z)
			zero.w = If((zero.w >= vector.w), zero.w, vector.w)
			Dim float As Single = material.GetFloat(ShaderUtilities.ID_GradientScale)
			vector *= float
			num10 = Mathf.Max(vector.x, vector.y)
			num10 = Mathf.Max(vector.z, num10)
			num10 = Mathf.Max(vector.w, num10)
			Return num10 + 0.5F
		End Function

		' Token: 0x060051A0 RID: 20896 RVA: 0x0029AB7C File Offset: 0x00298F7C
		Public Function GetPadding(materials As Material(), enableExtraPadding As Boolean, isBold As Boolean) As Single
			If Not ShaderUtilities.isInitialized Then
				ShaderUtilities.GetShaderPropertyIDs()
			End If
			If materials Is Nothing Then
				Return 0F
			End If
			Dim num As Integer = If((Not enableExtraPadding), 0, 4)
			If Not materials(0).HasProperty(ShaderUtilities.ID_GradientScale) Then
				Return CSng(num)
			End If
			Dim vector As Vector4 = Vector4.zero
			Dim zero As Vector4 = Vector4.zero
			Dim num2 As Single = 0F
			Dim num3 As Single = 0F
			Dim num4 As Single = 0F
			Dim num5 As Single = 0F
			Dim num6 As Single = 0F
			Dim num7 As Single = 0F
			Dim num8 As Single = 0F
			Dim num9 As Single = 0F
			Dim num10 As Single
			For i As Integer = 0 To materials.Length - 1
				ShaderUtilities.UpdateShaderRatios(materials(i), isBold)
				Dim shaderKeywords As String() = materials(i).shaderKeywords
				If materials(i).HasProperty(ShaderUtilities.ID_ScaleRatio_A) Then
					num5 = materials(i).GetFloat(ShaderUtilities.ID_ScaleRatio_A)
				End If
				If materials(i).HasProperty(ShaderUtilities.ID_FaceDilate) Then
					num2 = materials(i).GetFloat(ShaderUtilities.ID_FaceDilate) * num5
				End If
				If materials(i).HasProperty(ShaderUtilities.ID_OutlineSoftness) Then
					num3 = materials(i).GetFloat(ShaderUtilities.ID_OutlineSoftness) * num5
				End If
				If materials(i).HasProperty(ShaderUtilities.ID_OutlineWidth) Then
					num4 = materials(i).GetFloat(ShaderUtilities.ID_OutlineWidth) * num5
				End If
				num10 = num4 + num3 + num2
				If materials(i).HasProperty(ShaderUtilities.ID_GlowOffset) AndAlso shaderKeywords.Contains(ShaderUtilities.Keyword_Glow) Then
					If materials(i).HasProperty(ShaderUtilities.ID_ScaleRatio_B) Then
						num6 = materials(i).GetFloat(ShaderUtilities.ID_ScaleRatio_B)
					End If
					num8 = materials(i).GetFloat(ShaderUtilities.ID_GlowOffset) * num6
					num9 = materials(i).GetFloat(ShaderUtilities.ID_GlowOuter) * num6
				End If
				num10 = Mathf.Max(num10, num2 + num8 + num9)
				If materials(i).HasProperty(ShaderUtilities.ID_UnderlaySoftness) AndAlso shaderKeywords.Contains(ShaderUtilities.Keyword_Underlay) Then
					If materials(i).HasProperty(ShaderUtilities.ID_ScaleRatio_C) Then
						num7 = materials(i).GetFloat(ShaderUtilities.ID_ScaleRatio_C)
					End If
					Dim num11 As Single = materials(i).GetFloat(ShaderUtilities.ID_UnderlayOffsetX) * num7
					Dim num12 As Single = materials(i).GetFloat(ShaderUtilities.ID_UnderlayOffsetY) * num7
					Dim num13 As Single = materials(i).GetFloat(ShaderUtilities.ID_UnderlayDilate) * num7
					Dim num14 As Single = materials(i).GetFloat(ShaderUtilities.ID_UnderlaySoftness) * num7
					vector.x = Mathf.Max(vector.x, num2 + num13 + num14 - num11)
					vector.y = Mathf.Max(vector.y, num2 + num13 + num14 - num12)
					vector.z = Mathf.Max(vector.z, num2 + num13 + num14 + num11)
					vector.w = Mathf.Max(vector.w, num2 + num13 + num14 + num12)
				End If
				vector.x = Mathf.Max(vector.x, num10)
				vector.y = Mathf.Max(vector.y, num10)
				vector.z = Mathf.Max(vector.z, num10)
				vector.w = Mathf.Max(vector.w, num10)
				vector.x += CSng(num)
				vector.y += CSng(num)
				vector.z += CSng(num)
				vector.w += CSng(num)
				vector.x = Mathf.Min(vector.x, 1F)
				vector.y = Mathf.Min(vector.y, 1F)
				vector.z = Mathf.Min(vector.z, 1F)
				vector.w = Mathf.Min(vector.w, 1F)
				zero.x = If((zero.x >= vector.x), zero.x, vector.x)
				zero.y = If((zero.y >= vector.y), zero.y, vector.y)
				zero.z = If((zero.z >= vector.z), zero.z, vector.z)
				zero.w = If((zero.w >= vector.w), zero.w, vector.w)
			Next
			Dim float As Single = materials(0).GetFloat(ShaderUtilities.ID_GradientScale)
			vector *= float
			num10 = Mathf.Max(vector.x, vector.y)
			num10 = Mathf.Max(vector.z, num10)
			num10 = Mathf.Max(vector.w, num10)
			Return num10 + 0.25F
		End Function

		' Token: 0x040054D6 RID: 21718
		Public ID_MainTex As Integer

		' Token: 0x040054D7 RID: 21719
		Public ID_FaceTex As Integer

		' Token: 0x040054D8 RID: 21720
		Public ID_FaceColor As Integer

		' Token: 0x040054D9 RID: 21721
		Public ID_FaceDilate As Integer

		' Token: 0x040054DA RID: 21722
		Public ID_Shininess As Integer

		' Token: 0x040054DB RID: 21723
		Public ID_UnderlayColor As Integer

		' Token: 0x040054DC RID: 21724
		Public ID_UnderlayOffsetX As Integer

		' Token: 0x040054DD RID: 21725
		Public ID_UnderlayOffsetY As Integer

		' Token: 0x040054DE RID: 21726
		Public ID_UnderlayDilate As Integer

		' Token: 0x040054DF RID: 21727
		Public ID_UnderlaySoftness As Integer

		' Token: 0x040054E0 RID: 21728
		Public ID_WeightNormal As Integer

		' Token: 0x040054E1 RID: 21729
		Public ID_WeightBold As Integer

		' Token: 0x040054E2 RID: 21730
		Public ID_OutlineTex As Integer

		' Token: 0x040054E3 RID: 21731
		Public ID_OutlineWidth As Integer

		' Token: 0x040054E4 RID: 21732
		Public ID_OutlineSoftness As Integer

		' Token: 0x040054E5 RID: 21733
		Public ID_OutlineColor As Integer

		' Token: 0x040054E6 RID: 21734
		Public ID_GradientScale As Integer

		' Token: 0x040054E7 RID: 21735
		Public ID_ScaleX As Integer

		' Token: 0x040054E8 RID: 21736
		Public ID_ScaleY As Integer

		' Token: 0x040054E9 RID: 21737
		Public ID_PerspectiveFilter As Integer

		' Token: 0x040054EA RID: 21738
		Public ID_TextureWidth As Integer

		' Token: 0x040054EB RID: 21739
		Public ID_TextureHeight As Integer

		' Token: 0x040054EC RID: 21740
		Public ID_BevelAmount As Integer

		' Token: 0x040054ED RID: 21741
		Public ID_GlowColor As Integer

		' Token: 0x040054EE RID: 21742
		Public ID_GlowOffset As Integer

		' Token: 0x040054EF RID: 21743
		Public ID_GlowPower As Integer

		' Token: 0x040054F0 RID: 21744
		Public ID_GlowOuter As Integer

		' Token: 0x040054F1 RID: 21745
		Public ID_LightAngle As Integer

		' Token: 0x040054F2 RID: 21746
		Public ID_EnvMap As Integer

		' Token: 0x040054F3 RID: 21747
		Public ID_EnvMatrix As Integer

		' Token: 0x040054F4 RID: 21748
		Public ID_EnvMatrixRotation As Integer

		' Token: 0x040054F5 RID: 21749
		Public ID_MaskCoord As Integer

		' Token: 0x040054F6 RID: 21750
		Public ID_ClipRect As Integer

		' Token: 0x040054F7 RID: 21751
		Public ID_MaskSoftnessX As Integer

		' Token: 0x040054F8 RID: 21752
		Public ID_MaskSoftnessY As Integer

		' Token: 0x040054F9 RID: 21753
		Public ID_VertexOffsetX As Integer

		' Token: 0x040054FA RID: 21754
		Public ID_VertexOffsetY As Integer

		' Token: 0x040054FB RID: 21755
		Public ID_UseClipRect As Integer

		' Token: 0x040054FC RID: 21756
		Public ID_StencilID As Integer

		' Token: 0x040054FD RID: 21757
		Public ID_StencilOp As Integer

		' Token: 0x040054FE RID: 21758
		Public ID_StencilComp As Integer

		' Token: 0x040054FF RID: 21759
		Public ID_StencilReadMask As Integer

		' Token: 0x04005500 RID: 21760
		Public ID_StencilWriteMask As Integer

		' Token: 0x04005501 RID: 21761
		Public ID_ShaderFlags As Integer

		' Token: 0x04005502 RID: 21762
		Public ID_ScaleRatio_A As Integer

		' Token: 0x04005503 RID: 21763
		Public ID_ScaleRatio_B As Integer

		' Token: 0x04005504 RID: 21764
		Public ID_ScaleRatio_C As Integer

		' Token: 0x04005505 RID: 21765
		Public Keyword_Bevel As String = "BEVEL_ON"

		' Token: 0x04005506 RID: 21766
		Public Keyword_Glow As String = "GLOW_ON"

		' Token: 0x04005507 RID: 21767
		Public Keyword_Underlay As String = "UNDERLAY_ON"

		' Token: 0x04005508 RID: 21768
		Public Keyword_Ratios As String = "RATIOS_OFF"

		' Token: 0x04005509 RID: 21769
		Public Keyword_MASK_SOFT As String = "MASK_SOFT"

		' Token: 0x0400550A RID: 21770
		Public Keyword_MASK_HARD As String = "MASK_HARD"

		' Token: 0x0400550B RID: 21771
		Public Keyword_MASK_TEX As String = "MASK_TEX"

		' Token: 0x0400550C RID: 21772
		Public ShaderTag_ZTestMode As String = "unity_GUIZTestMode"

		' Token: 0x0400550D RID: 21773
		Public ShaderTag_CullMode As String = "_CullMode"

		' Token: 0x0400550E RID: 21774
		Private m_clamp As Single = 1F

		' Token: 0x0400550F RID: 21775
		Public isInitialized As Boolean
	End Module
End Namespace
