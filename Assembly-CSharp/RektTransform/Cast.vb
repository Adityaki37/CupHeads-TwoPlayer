Imports System
Imports UnityEngine
Imports UnityEngine.EventSystems

Namespace RektTransform
	' Token: 0x0200036D RID: 877
	Public Module Cast
		' Token: 0x060009C8 RID: 2504 RVA: 0x0007D0A4 File Offset: 0x0007B4A4
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function RT(go As GameObject) As RectTransform
			If go Is Nothing OrElse go.transform Is Nothing Then
				Return Nothing
			End If
			Return go.GetComponent(Of RectTransform)()
		End Function

		' Token: 0x060009C9 RID: 2505 RVA: 0x0007D0CB File Offset: 0x0007B4CB
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function RT(t As Transform) As RectTransform
			If Not(TypeOf t Is RectTransform) Then
				Return Nothing
			End If
			Return TryCast(t, RectTransform)
		End Function

		' Token: 0x060009CA RID: 2506 RVA: 0x0007D0E0 File Offset: 0x0007B4E0
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function RT(c As Component) As RectTransform
			Return c.transform.RT()
		End Function

		' Token: 0x060009CB RID: 2507 RVA: 0x0007D0ED File Offset: 0x0007B4ED
		<System.Runtime.CompilerServices.ExtensionAttribute()>
		Public Function RT(ui As UIBehaviour) As RectTransform
			If ui Is Nothing Then
				Return Nothing
			End If
			Return TryCast(ui.transform, RectTransform)
		End Function
	End Module
End Namespace
