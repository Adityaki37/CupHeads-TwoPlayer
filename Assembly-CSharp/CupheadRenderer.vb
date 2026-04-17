Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x02000AF9 RID: 2809
Public Class CupheadRenderer
	Inherits AbstractMonoBehaviour

	' Token: 0x06004411 RID: 17425 RVA: 0x00240BC1 File Offset: 0x0023EFC1
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If CupheadRenderer.Instance Is Nothing Then
			CupheadRenderer.Instance = Me
			Me.Setup()
			Return
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06004412 RID: 17426 RVA: 0x00240BF6 File Offset: 0x0023EFF6
	Private Sub OnDestroy()
		If CupheadRenderer.Instance Is Me Then
			CupheadRenderer.Instance = Nothing
		End If
	End Sub

	' Token: 0x06004413 RID: 17427 RVA: 0x00240C0E File Offset: 0x0023F00E
	Private Sub Setup()
		Me.rendererCamera = Global.UnityEngine.[Object].Instantiate(Of CupheadRendererCamera)(Me.cameraPrefab)
		Me.rendererCamera.transform.SetParent(MyBase.transform)
		Me.rendererCamera.transform.ResetLocalTransforms()
	End Sub

	' Token: 0x06004414 RID: 17428 RVA: 0x00240C47 File Offset: 0x0023F047
	Public Sub TouchFuzzy(amount As Single, speed As Single, time As Single)
		Me.rendererCamera.GetComponent(Of ChromaticAberrationFilmGrain)().PsychedelicEffect(amount, speed, time)
		MyBase.StartCoroutine(Me.change_blur_cr(time))
	End Sub

	' Token: 0x06004415 RID: 17429 RVA: 0x00240C6C File Offset: 0x0023F06C
	Private Iterator Function change_blur_cr(time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim incrementTime As Single = 1F
		Dim blurStart As Single = Me.rendererCamera.GetComponent(Of BlurGamma)().blurSize
		Me.rendererCamera.GetComponent(Of BlurGamma)().blurSize += incrementTime
		While Me.rendererCamera.GetComponent(Of BlurGamma)().blurSize > blurStart
			t += Time.deltaTime
			If t >= time / 2F Then
				Me.rendererCamera.GetComponent(Of BlurGamma)().blurSize -= incrementTime * Time.deltaTime
			Else
				Me.rendererCamera.GetComponent(Of BlurGamma)().blurSize += incrementTime * Time.deltaTime
			End If
			Yield Nothing
		End While
		Me.rendererCamera.GetComponent(Of BlurGamma)().blurSize = blurStart
		Return
	End Function

	' Token: 0x040049B8 RID: 18872
	Public Shared Instance As CupheadRenderer

	' Token: 0x040049B9 RID: 18873
	<SerializeField()>
	Private cameraPrefab As CupheadRendererCamera

	' Token: 0x040049BA RID: 18874
	Private rendererCamera As CupheadRendererCamera

	' Token: 0x040049BB RID: 18875
	Private bgCamera As Camera

	' Token: 0x040049BC RID: 18876
	Private canvas As Canvas

	' Token: 0x040049BD RID: 18877
	Private rendererParents As Dictionary(Of CupheadRenderer.RenderLayer, RectTransform)

	' Token: 0x040049BE RID: 18878
	Private background As Image

	' Token: 0x040049BF RID: 18879
	Private fader As Image

	' Token: 0x040049C0 RID: 18880
	Public fuzzyEffectPlaying As Boolean

	' Token: 0x02000AFA RID: 2810
	Public Enum RenderLayer
		' Token: 0x040049C2 RID: 18882
		None
		' Token: 0x040049C3 RID: 18883
		Game
		' Token: 0x040049C4 RID: 18884
		UI
		' Token: 0x040049C5 RID: 18885
		Loader
	End Enum
End Class
