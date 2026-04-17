Imports System
Imports Rewired.Utils
Imports UnityEngine
Imports UnityEngine.UI

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C0A RID: 3082
	<RequireComponent(GetType(CanvasScalerExt))>
	Public Class CanvasScalerFitter
		Inherits MonoBehaviour

		' Token: 0x06004994 RID: 18836 RVA: 0x0026749E File Offset: 0x0026589E
		Private Sub OnEnable()
			Me.canvasScaler = MyBase.GetComponent(Of CanvasScalerExt)()
			Me.Update()
			Me.canvasScaler.ForceRefresh()
		End Sub

		' Token: 0x06004995 RID: 18837 RVA: 0x002674BD File Offset: 0x002658BD
		Private Sub Update()
			If Screen.width <> Me.screenWidth OrElse Screen.height <> Me.screenHeight Then
				Me.screenWidth = Screen.width
				Me.screenHeight = Screen.height
				Me.UpdateSize()
			End If
		End Sub

		' Token: 0x06004996 RID: 18838 RVA: 0x002674FC File Offset: 0x002658FC
		Private Sub UpdateSize()
			If Me.canvasScaler.uiScaleMode <> CanvasScaler.ScaleMode.ScaleWithScreenSize Then
				Return
			End If
			If Me.breakPoints Is Nothing Then
				Return
			End If
			Dim num As Single = CSng(Screen.width) / CSng(Screen.height)
			Dim num2 As Single = Single.PositiveInfinity
			Dim num3 As Integer = 0
			For i As Integer = 0 To Me.breakPoints.Length - 1
				Dim num4 As Single = Mathf.Abs(num - Me.breakPoints(i).screenAspectRatio)
				If num4 <= Me.breakPoints(i).screenAspectRatio OrElse MathTools.IsNear(Me.breakPoints(i).screenAspectRatio, 0.01F) Then
					If num4 < num2 Then
						num2 = num4
						num3 = i
					End If
				End If
			Next
			Me.canvasScaler.referenceResolution = Me.breakPoints(num3).referenceResolution
		End Sub

		' Token: 0x04004FAC RID: 20396
		<SerializeField()>
		Private breakPoints As CanvasScalerFitter.BreakPoint()

		' Token: 0x04004FAD RID: 20397
		Private canvasScaler As CanvasScalerExt

		' Token: 0x04004FAE RID: 20398
		Private screenWidth As Integer

		' Token: 0x04004FAF RID: 20399
		Private screenHeight As Integer

		' Token: 0x04004FB0 RID: 20400
		Private ScreenSizeChanged As Action

		' Token: 0x02000C0B RID: 3083
		<Serializable()>
		Private Class BreakPoint
			' Token: 0x04004FB1 RID: 20401
			<SerializeField()>
			Public name As String

			' Token: 0x04004FB2 RID: 20402
			<SerializeField()>
			Public screenAspectRatio As Single

			' Token: 0x04004FB3 RID: 20403
			<SerializeField()>
			Public referenceResolution As Vector2
		End Class
	End Class
End Namespace
