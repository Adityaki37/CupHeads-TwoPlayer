Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000436 RID: 1078
Public Class HitFlash
	Inherits AbstractMonoBehaviour

	' Token: 0x1700028A RID: 650
	' (get) Token: 0x06000FCF RID: 4047 RVA: 0x0009CE99 File Offset: 0x0009B299
	' (set) Token: 0x06000FD0 RID: 4048 RVA: 0x0009CEA1 File Offset: 0x0009B2A1
	Public Property flashing As Boolean

	' Token: 0x1700028B RID: 651
	' (get) Token: 0x06000FD1 RID: 4049 RVA: 0x0009CEAA File Offset: 0x0009B2AA
	' (set) Token: 0x06000FD2 RID: 4050 RVA: 0x0009CEB2 File Offset: 0x0009B2B2
	Public Property disabled As Boolean

	' Token: 0x06000FD3 RID: 4051 RVA: 0x0009CEBC File Offset: 0x0009B2BC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If Me.includeSelf Then
			Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
			If component IsNot Nothing Then
				Me.self = New HitFlash.RendererProperties(component)
			End If
		End If
		Me.renderers = New List(Of HitFlash.RendererProperties)()
		For i As Integer = 0 To Me.otherRenderers.Length - 1
			Me.renderers.Add(New HitFlash.RendererProperties(Me.otherRenderers(i)))
		Next
		If Me.damageReceiver Is Nothing Then
			Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		End If
		If Me.damageReceiver Then
			AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		End If
	End Sub

	' Token: 0x06000FD4 RID: 4052 RVA: 0x0009CF79 File Offset: 0x0009B379
	Private Sub Update()
		Me.time -= CupheadTime.Delta
	End Sub

	' Token: 0x06000FD5 RID: 4053 RVA: 0x0009CF92 File Offset: 0x0009B392
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.Flash(0.1F)
	End Sub

	' Token: 0x06000FD6 RID: 4054 RVA: 0x0009CF9F File Offset: 0x0009B39F
	Public Overrides Sub StopAllCoroutines()
		MyBase.StopAllCoroutines()
		Me.SetColor(1F)
		Me.SetScale(Vector3.one, 1F)
		Me.time = 0F
		Me.flashing = False
	End Sub

	' Token: 0x06000FD7 RID: 4055 RVA: 0x0009CFD4 File Offset: 0x0009B3D4
	Public Sub StopAllCoroutinesWithoutSettingScale()
		MyBase.StopAllCoroutines()
		Me.SetColor(1F)
		Me.time = 0F
		Me.flashing = False
	End Sub

	' Token: 0x06000FD8 RID: 4056 RVA: 0x0009CFFC File Offset: 0x0009B3FC
	Public Sub Flash(Optional t As Single = 0.1F)
		If Me.disabled Then
			Return
		End If
		Me.time = t
		If Me.flashing Then
			Return
		End If
		If MyBase.gameObject.activeSelf AndAlso MyBase.gameObject.activeInHierarchy Then
			MyBase.StartCoroutine(Me.flash_cr())
		End If
	End Sub

	' Token: 0x06000FD9 RID: 4057 RVA: 0x0009D058 File Offset: 0x0009B458
	Public Sub SetColor(t As Single)
		If Me.self IsNot Nothing Then
			Dim color As Color = Color.Lerp(Me.self.normalColor, Me.damageColor, t)
			Me.self.renderer.color = color
		End If
		For Each rendererProperties As HitFlash.RendererProperties In Me.renderers
			Dim color2 As Color = Color.Lerp(rendererProperties.normalColor, Me.damageColor, t)
			rendererProperties.renderer.color = color2
		Next
	End Sub

	' Token: 0x06000FDA RID: 4058 RVA: 0x0009D100 File Offset: 0x0009B500
	Private Sub SetScale(original As Vector3, s As Single)
		If Me.self IsNot Nothing Then
			Me.self.transform.localScale = original * s
		End If
		For Each rendererProperties As HitFlash.RendererProperties In Me.renderers
			rendererProperties.transform.localScale = rendererProperties.scale * s
		Next
	End Sub

	' Token: 0x06000FDB RID: 4059 RVA: 0x0009D190 File Offset: 0x0009B590
	Private Iterator Function flash_cr() As IEnumerator
		Me.flashing = True
		While Me.time > 0F
			Me.SetColor(1F)
			Yield CupheadTime.WaitForSeconds(Me, 0.0416F)
			Me.SetColor(0F)
			Yield CupheadTime.WaitForSeconds(Me, 0.0832F)
		End While
		Me.flashing = False
		Return
	End Function

	' Token: 0x04001967 RID: 6503
	<SerializeField()>
	Private damageColor As Color = New Color(1F, 0F, 0F, 1F)

	' Token: 0x04001968 RID: 6504
	<SerializeField()>
	Private damageReceiver As DamageReceiver

	' Token: 0x04001969 RID: 6505
	<SerializeField()>
	Private includeSelf As Boolean = True

	' Token: 0x0400196A RID: 6506
	Public otherRenderers As SpriteRenderer()

	' Token: 0x0400196B RID: 6507
	Private time As Single

	' Token: 0x0400196E RID: 6510
	Private coroutine As Coroutine

	' Token: 0x0400196F RID: 6511
	Private self As HitFlash.RendererProperties

	' Token: 0x04001970 RID: 6512
	Private renderers As List(Of HitFlash.RendererProperties)

	' Token: 0x02000437 RID: 1079
	Public Class RendererProperties
		' Token: 0x06000FDC RID: 4060 RVA: 0x0009D1AB File Offset: 0x0009B5AB
		Public Sub New(r As SpriteRenderer)
			Me.renderer = r
			Me.normalColor = r.color
			Me.transform = r.transform
			Me.scale = r.transform.localScale
		End Sub

		' Token: 0x04001971 RID: 6513
		Public renderer As SpriteRenderer

		' Token: 0x04001972 RID: 6514
		Public normalColor As Color

		' Token: 0x04001973 RID: 6515
		Public transform As Transform

		' Token: 0x04001974 RID: 6516
		Public scale As Vector3
	End Class
End Class
