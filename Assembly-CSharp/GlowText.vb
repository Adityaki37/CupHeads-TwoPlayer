Imports System
Imports System.Collections
Imports TMPro
Imports UnityEngine
Imports UnityEngine.PostProcessing
Imports UnityEngine.UI

' Token: 0x02000449 RID: 1097
Public Class GlowText
	Inherits MonoBehaviour

	' Token: 0x0600105A RID: 4186 RVA: 0x0009F2E0 File Offset: 0x0009D6E0
	Public Sub InitTMPText(ParamArray tmp_texts As MaskableGraphic())
		If tmp_texts.Length > Me.tmpTextsToGlow.Length Then
			Return
		End If
		For i As Integer = 0 To tmp_texts.Length - 1
			If TypeOf tmp_texts(i)Is Text Then
				Dim text As Text = TryCast(tmp_texts(i), Text)
				Me.tmpTextsToGlow(i).enabled = True
				Me.tmpTextsToGlow(i).text = text.text
				Me.tmpTextsToGlow(i).fontSize = CSng(text.fontSize)
			ElseIf TypeOf tmp_texts(i)Is TextMeshProUGUI Then
				Dim textMeshProUGUI As TextMeshProUGUI = TryCast(tmp_texts(i), TextMeshProUGUI)
				tmp_texts(i) = TryCast(tmp_texts(i), Text)
				Me.tmpTextsToGlow(i).enabled = True
				Me.tmpTextsToGlow(i).text = textMeshProUGUI.text
				Me.tmpTextsToGlow(i).fontSize = textMeshProUGUI.fontSize
				Me.tmpTextsToGlow(i).font = textMeshProUGUI.font
				Me.tmpTextsToGlow(i).outlineWidth = textMeshProUGUI.outlineWidth
			End If
		Next
	End Sub

	' Token: 0x0600105B RID: 4187 RVA: 0x0009F3E0 File Offset: 0x0009D7E0
	Public Sub DisableTMPText()
		For i As Integer = 0 To Me.tmpTextsToGlow.Length - 1
			Me.tmpTextsToGlow(i).enabled = False
		Next
	End Sub

	' Token: 0x0600105C RID: 4188 RVA: 0x0009F414 File Offset: 0x0009D814
	Public Sub InitImages(ParamArray images As Image())
		If images.Length > Me.imagesToGlow.Length Then
			Return
		End If
		For i As Integer = 0 To images.Length - 1
			Me.imagesToGlow(i).enabled = True
			Me.imagesToGlow(i).sprite = images(i).sprite
			Me.imagesToGlow(i).color = images(i).color
		Next
	End Sub

	' Token: 0x0600105D RID: 4189 RVA: 0x0009F480 File Offset: 0x0009D880
	Public Sub DisableImages()
		For i As Integer = 0 To Me.imagesToGlow.Length - 1
			Me.imagesToGlow(i).enabled = False
		Next
	End Sub

	' Token: 0x0600105E RID: 4190 RVA: 0x0009F4B4 File Offset: 0x0009D8B4
	Public Sub BeginGlow()
		Me.rawImageGlow.enabled = True
		MyBase.StartCoroutine(Me.Glow_cr())
	End Sub

	' Token: 0x0600105F RID: 4191 RVA: 0x0009F4CF File Offset: 0x0009D8CF
	Public Sub StopGlow()
		Me.rawImageGlow.enabled = False
	End Sub

	' Token: 0x06001060 RID: 4192 RVA: 0x0009F4E0 File Offset: 0x0009D8E0
	Private Iterator Function Glow_cr() As IEnumerator
		Dim rt As RenderTexture = RenderTexture.active
		RenderTexture.active = Me.renderTextureGlow
		GL.Clear(True, True, Color.clear)
		RenderTexture.active = rt
		Me.cameraGlow.GetComponent(Of PostProcessingBehaviour)().enabled = True
		Yield Nothing
		Me.cameraGlow.GetComponent(Of PostProcessingBehaviour)().enabled = False
		Return
	End Function

	' Token: 0x040019B1 RID: 6577
	<SerializeField()>
	Private renderTextureGlow As RenderTexture

	' Token: 0x040019B2 RID: 6578
	<SerializeField()>
	Private cameraGlow As GameObject

	' Token: 0x040019B3 RID: 6579
	<SerializeField()>
	Private rawImageGlow As RawImage

	' Token: 0x040019B4 RID: 6580
	<SerializeField()>
	Private tmpTextsToGlow As TextMeshProUGUI()

	' Token: 0x040019B5 RID: 6581
	<SerializeField()>
	Private imagesToGlow As Image()
End Class
