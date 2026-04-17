Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020009A4 RID: 2468
Public Class MapUIVignetteDialogue
	Inherits AbstractMonoBehaviour

	' Token: 0x170004B4 RID: 1204
	' (get) Token: 0x060039E8 RID: 14824 RVA: 0x0020EFCB File Offset: 0x0020D3CB
	' (set) Token: 0x060039E9 RID: 14825 RVA: 0x0020EFD2 File Offset: 0x0020D3D2
	Public Shared Property Current As MapUIVignetteDialogue

	' Token: 0x060039EA RID: 14826 RVA: 0x0020EFDA File Offset: 0x0020D3DA
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MapUIVignetteDialogue.Current = Me
		Me.canvasGroup.alpha = 0F
	End Sub

	' Token: 0x060039EB RID: 14827 RVA: 0x0020EFF8 File Offset: 0x0020D3F8
	Private Sub LateUpdate()
		MyBase.transform.position = CupheadMapCamera.Current.transform.position
	End Sub

	' Token: 0x060039EC RID: 14828 RVA: 0x0020F014 File Offset: 0x0020D414
	Public Sub FadeIn()
		Me.Fade(1F)
	End Sub

	' Token: 0x060039ED RID: 14829 RVA: 0x0020F021 File Offset: 0x0020D421
	Public Sub FadeOut()
		Me.Fade(0F)
	End Sub

	' Token: 0x060039EE RID: 14830 RVA: 0x0020F02E File Offset: 0x0020D42E
	Public Sub Fade(target As Single)
		MyBase.StartCoroutine(Me.fade_cr(Me.canvasGroup.alpha, target))
	End Sub

	' Token: 0x060039EF RID: 14831 RVA: 0x0020F04C File Offset: 0x0020D44C
	Private Iterator Function fade_cr(startOpacity As Single, endOpacity As Single) As IEnumerator
		Dim t As Single = 0F
		While t < MapUIVignetteDialogue.fadeTime
			Yield Nothing
			t += CupheadTime.Delta
			Me.canvasGroup.alpha = Mathf.Lerp(startOpacity, endOpacity, t / MapUIVignetteDialogue.fadeTime)
		End While
		Me.canvasGroup.alpha = endOpacity
		Return
	End Function

	' Token: 0x040041D8 RID: 16856
	Public Shared fadeTime As Single = 0.5F

	' Token: 0x040041D9 RID: 16857
	<SerializeField()>
	Private canvasGroup As CanvasGroup
End Class
