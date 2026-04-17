Imports System
Imports UnityEngine

' Token: 0x0200036A RID: 874
Public Module RectTransformExtensions
	' Token: 0x060009C4 RID: 2500 RVA: 0x0007CD60 File Offset: 0x0007B160
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function Copy(transform As RectTransform, target As RectTransform) As RectTransform
		transform.SetParent(target.parent)
		transform.position = target.position
		transform.localScale = target.localScale
		transform.rotation = target.rotation
		transform.anchoredPosition3D = target.anchoredPosition3D
		transform.anchorMax = target.anchorMax
		transform.anchorMin = target.anchorMin
		transform.offsetMax = target.offsetMax
		transform.offsetMin = target.offsetMin
		transform.pivot = target.pivot
		transform.sizeDelta = target.sizeDelta
		Return transform
	End Function
End Module
