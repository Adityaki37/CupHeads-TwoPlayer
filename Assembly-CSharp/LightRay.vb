Imports System
Imports UnityEngine

' Token: 0x02000B12 RID: 2834
Public Class LightRay
	Inherits AbstractMonoBehaviour

	' Token: 0x060044BF RID: 17599 RVA: 0x002465F8 File Offset: 0x002449F8
	Private Sub Start()
		Dim num As Single = If((Not Me.randomOffset), Me.customOffset, Global.UnityEngine.Random.Range(0F, 1F))
		Me.t = 4F * num / Me.speed
	End Sub

	' Token: 0x060044C0 RID: 17600 RVA: 0x00246640 File Offset: 0x00244A40
	Private Sub Update()
		If Not Me.widthCached Then
			Dim texture As Texture2D = MyBase.GetComponent(Of SpriteRenderer)().sprite.texture
			If texture Is Nothing Then
				Return
			End If
			Me.textureWidth = 2.3232484F / (CSng(texture.width) / CSng(texture.height))
			Me.widthCached = True
		End If
		Me.accumulator += CupheadTime.Delta
		While Me.accumulator > 0.041666668F
			Me.accumulator -= 0.041666668F
			Me.t += 0.041666668F
		End While
		Dim material As Material = MyBase.GetComponent(Of SpriteRenderer)().material
		material.SetFloat("t", Me.t)
		material.SetFloat("textureWidth", Me.textureWidth)
		material.SetFloat("textureSpeed", Me.speed)
	End Sub

	' Token: 0x04004A76 RID: 19062
	Private t As Single

	' Token: 0x04004A77 RID: 19063
	Private accumulator As Single

	' Token: 0x04004A78 RID: 19064
	Private textureWidth As Single

	' Token: 0x04004A79 RID: 19065
	<SerializeField()>
	Private speed As Single = 0.03F

	' Token: 0x04004A7A RID: 19066
	<SerializeField()>
	Private randomOffset As Boolean = True

	' Token: 0x04004A7B RID: 19067
	<SerializeField()>
	<Range(0F, 1F)>
	Private customOffset As Single = 0.5F

	' Token: 0x04004A7C RID: 19068
	Private widthCached As Boolean
End Class
