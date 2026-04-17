Imports System
Imports UnityEngine

Namespace Rewired.UI.ControlMapper
	' Token: 0x02000C0C RID: 3084
	<RequireComponent(GetType(CanvasScalerExt))>
	Public Class CanvasScalerFitterControlMapperOverride
		Inherits MonoBehaviour

		' Token: 0x06004999 RID: 18841 RVA: 0x002675EB File Offset: 0x002659EB
		Private Sub OnEnable()
			Me.canvasScaler = MyBase.GetComponent(Of CanvasScalerExt)()
		End Sub

		' Token: 0x0600499A RID: 18842 RVA: 0x002675F9 File Offset: 0x002659F9
		Private Sub LateUpdate()
			Me.canvasScaler.referenceResolution = Me.targetResolution
		End Sub

		' Token: 0x04004FB4 RID: 20404
		<SerializeField()>
		Private targetResolution As Vector2 = New Vector2(1885F, 600F)

		' Token: 0x04004FB5 RID: 20405
		Private canvasScaler As CanvasScalerExt
	End Class
End Namespace
