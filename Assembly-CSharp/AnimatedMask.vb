Imports System
Imports UnityEngine

' Token: 0x02000456 RID: 1110
<RequireComponent(GetType(SpriteMask))>
Public Class AnimatedMask
	Inherits MonoBehaviour

	' Token: 0x060010C7 RID: 4295 RVA: 0x000A0BC4 File Offset: 0x0009EFC4
	Private Sub Start()
		Me.currentMask = Me.maskRequest
		Me.mask = MyBase.GetComponent(Of SpriteMask)()
		Me.mask.sprite = Me.currentMask
	End Sub

	' Token: 0x060010C8 RID: 4296 RVA: 0x000A0BEF File Offset: 0x0009EFEF
	Private Sub LateUpdate()
		If Me.currentMask IsNot Me.maskRequest Then
			Me.currentMask = Me.maskRequest
			Me.mask.sprite = Me.currentMask
		End If
	End Sub

	' Token: 0x04001A0F RID: 6671
	Public maskRequest As Sprite

	' Token: 0x04001A10 RID: 6672
	Private currentMask As Sprite

	' Token: 0x04001A11 RID: 6673
	Private mask As SpriteMask
End Class
