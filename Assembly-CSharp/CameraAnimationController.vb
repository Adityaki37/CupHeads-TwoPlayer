Imports System
Imports UnityEngine
Imports UnityStandardAssets.ImageEffects

' Token: 0x020003D5 RID: 981
Public Class CameraAnimationController
	Inherits MonoBehaviour

	' Token: 0x06000CEB RID: 3307 RVA: 0x0008A418 File Offset: 0x00088818
	Private Sub Start()
		Dim gameObject As GameObject = GameObject.FindWithTag("MainCamera")
		Me.Camera = gameObject.GetComponent(Of Camera)()
		Me.MapCamera = gameObject.GetComponent(Of CupheadMapCamera)()
		Me.BlurOptimized = gameObject.GetComponent(Of BlurOptimized)()
	End Sub

	' Token: 0x06000CEC RID: 3308 RVA: 0x0008A454 File Offset: 0x00088854
	Private Sub Update()
		Me.ApplyProperties()
	End Sub

	' Token: 0x06000CED RID: 3309 RVA: 0x0008A45C File Offset: 0x0008885C
	Private Sub OnEnable()
	End Sub

	' Token: 0x06000CEE RID: 3310 RVA: 0x0008A45E File Offset: 0x0008885E
	Private Sub OnDisable()
		Me.ApplyProperties()
	End Sub

	' Token: 0x06000CEF RID: 3311 RVA: 0x0008A468 File Offset: 0x00088868
	Private Sub ApplyProperties()
		If Me.Blur > 0F AndAlso Not Me.BlurOptimized.enabled Then
			Me.BlurOptimized.enabled = True
		End If
		If Me.Blur <= 0F AndAlso Me.BlurOptimized.enabled Then
			Me.BlurOptimized.enabled = False
		End If
		If Me.MapCamera IsNot Nothing Then
			Me.MapCamera.centerOnPlayer = Me.CenterOnPlayer
			Me.Camera.orthographicSize = Me.OrthoSize
		End If
		Me.BlurOptimized.blurSize = Me.Blur
	End Sub

	' Token: 0x04001657 RID: 5719
	Public CenterOnPlayer As Boolean = True

	' Token: 0x04001658 RID: 5720
	Public OrthoSize As Single

	' Token: 0x04001659 RID: 5721
	Public Blur As Single

	' Token: 0x0400165A RID: 5722
	Private Camera As Camera

	' Token: 0x0400165B RID: 5723
	Private MapCamera As CupheadMapCamera

	' Token: 0x0400165C RID: 5724
	Private BlurOptimized As BlurOptimized
End Class
