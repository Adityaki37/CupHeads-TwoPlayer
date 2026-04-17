Imports System
Imports System.Collections
Imports UnityEngine
Imports UnityStandardAssets.ImageEffects

' Token: 0x020003E2 RID: 994
<ExecuteInEditMode()>
<RequireComponent(GetType(Camera))>
<AddComponentMenu("Image Effects/Other/Chromatic Aberration Film Grain")>
Public Class ChromaticAberrationFilmGrain
	Inherits PostEffectsBase

	' Token: 0x06000D54 RID: 3412 RVA: 0x0008D254 File Offset: 0x0008B654
	Public Sub Initialize(filmGrain As Texture2D())
		MyBase.enabled = True
		Me.rStart = Me.r
		Me.gStart = Me.g
		Me.bStart = Me.b
		If Not SystemInfo.supportsImageEffects Then
			MyBase.enabled = False
			Return
		End If
		Me.textures = filmGrain
		MyBase.StartCoroutine(Me.animate_cr())
	End Sub

	' Token: 0x06000D55 RID: 3413 RVA: 0x0008D2B4 File Offset: 0x0008B6B4
	Private Iterator Function animate_cr() As IEnumerator
		Dim t As Single = 0F
		Dim loopsUntilFullLoop As Integer = Global.UnityEngine.Random.Range(7, 15)
		While True
			t += Time.deltaTime
			While t > 0.025F
				t -= 0.025F
				If Me.animated Then
					Me.currentTexture += 1
					If loopsUntilFullLoop > 0 Then
						If Me.currentTexture >= Me.earlyLoopPoint Then
							Me.currentTexture = 0
							loopsUntilFullLoop -= 1
							Me.UV_Transform = New Vector4(CSng(MathUtils.PlusOrMinus()), 0F, 0F, CSng(MathUtils.PlusOrMinus()))
						End If
					ElseIf Me.currentTexture >= Me.textures.Length Then
						Me.currentTexture = 0
						loopsUntilFullLoop = Global.UnityEngine.Random.Range(7, 15)
						Me.UV_Transform = New Vector4(CSng(MathUtils.PlusOrMinus()), 0F, 0F, CSng(MathUtils.PlusOrMinus()))
					End If
				End If
			End While
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06000D56 RID: 3414 RVA: 0x0008D2CF File Offset: 0x0008B6CF
	Public Overrides Function CheckResources() As Boolean
		MyBase.CheckSupport(False)
		Me.material = MyBase.CheckShaderAndCreateMaterial(Me.shader, Me.material)
		If Not Me.isSupported Then
			MyBase.ReportAutoDisable()
		End If
		Return Me.isSupported
	End Function

	' Token: 0x06000D57 RID: 3415 RVA: 0x0008D308 File Offset: 0x0008B708
	Private Sub OnRenderImage(source As RenderTexture, destination As RenderTexture)
		If Not Me.CheckResources() Then
			Graphics.Blit(source, destination)
			Return
		End If
		Me.material.SetVector("_UV_Transform", Me.UV_Transform)
		Me.material.SetFloat("_Intensity", Me.intensity)
		If Me.textures IsNot Nothing AndAlso Me.textures.Length > Me.currentTexture AndAlso Me.textures(Me.currentTexture) IsNot Nothing Then
			Me.material.SetTexture("_Overlay", Me.textures(Me.currentTexture))
		End If
		Dim num As Single = CSng(source.width) / CSng(source.height)
		Dim num2 As Single = If((num >= 1.7777778F), 1F, (num / 1.7777778F))
		num2 *= 1F - 0.1F * SettingsData.Data.overscan
		Dim num3 As Single = SettingsData.Data.chromaticAberration * num2 * CSng(source.height) / 1080F
		Dim vector As Vector2 = Me.r * num3
		Dim vector2 As Vector2 = Me.g * num3
		Dim vector3 As Vector2 = Me.b * num3
		If SettingsData.Data.filter = BlurGamma.Filter.TwoStrip Then
			Dim vector4 As Vector2 = vector3 * 0.4F + vector2 * 0.6F
			vector2 = vector4
		End If
		Me.material.SetVector("_Screen", New Vector2(CSng(source.width), CSng(source.height)))
		Me.material.SetVector("_Red", vector)
		Me.material.SetVector("_Green", vector2)
		Me.material.SetVector("_Blue", vector3)
		Dim num4 As Integer = 0
		Dim filter As BlurGamma.Filter = SettingsData.Data.filter
		If filter <> BlurGamma.Filter.TwoStrip Then
			If filter = BlurGamma.Filter.BW Then
				num4 += 2
			End If
		Else
			num4 += 1
		End If
		Graphics.Blit(source, destination, Me.material, num4)
	End Sub

	' Token: 0x06000D58 RID: 3416 RVA: 0x0008D519 File Offset: 0x0008B919
	Protected Overridable Sub OnDisable()
		If Me.material Then
			Global.UnityEngine.[Object].DestroyImmediate(Me.material)
		End If
	End Sub

	' Token: 0x06000D59 RID: 3417 RVA: 0x0008D536 File Offset: 0x0008B936
	Public Sub PsychedelicEffect(amount As Single, speed As Single, time As Single)
		MyBase.StartCoroutine(Me.psychedelic_effect(amount, speed, time))
	End Sub

	' Token: 0x06000D5A RID: 3418 RVA: 0x0008D548 File Offset: 0x0008B948
	Private Iterator Function psychedelic_effect(amount As Single, speed As Single, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim slowdownTime As Single = 0.5F
		While amount > 0F
			t += Time.deltaTime
			Dim angle As Single = speed * t
			Dim phase As Single = Mathf.Sin(angle) * amount
			Me.r = Vector2.up * phase
			Me.g = Vector2.up * phase / 2F
			Me.b = Vector2.down * phase
			If t >= time Then
				amount -= slowdownTime
			End If
			Yield Nothing
		End While
		Me.r = Me.rStart
		Me.g = Me.gStart
		Me.b = Me.bStart
		Yield Nothing
		Return
	End Function

	' Token: 0x040016CC RID: 5836
	Public shader As Shader

	' Token: 0x040016CD RID: 5837
	Private material As Material

	' Token: 0x040016CE RID: 5838
	Private Const FRAME_TIME As Single = 0.025F

	' Token: 0x040016CF RID: 5839
	Private UV_Transform As Vector4 = New Vector4(1F, 0F, 0F, 1F)

	' Token: 0x040016D0 RID: 5840
	Public intensity As Single = 1F

	' Token: 0x040016D1 RID: 5841
	Public animated As Boolean = True

	' Token: 0x040016D2 RID: 5842
	Public earlyLoopPoint As Integer = 102

	' Token: 0x040016D3 RID: 5843
	Private currentTexture As Integer

	' Token: 0x040016D4 RID: 5844
	Public r As Vector2

	' Token: 0x040016D5 RID: 5845
	Public g As Vector2

	' Token: 0x040016D6 RID: 5846
	Public b As Vector2

	' Token: 0x040016D7 RID: 5847
	Private textures As Texture2D()

	' Token: 0x040016D8 RID: 5848
	Private rStart As Vector2

	' Token: 0x040016D9 RID: 5849
	Private gStart As Vector2

	' Token: 0x040016DA RID: 5850
	Private bStart As Vector2
End Class
