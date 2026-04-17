Imports System
Imports UnityEngine

' Token: 0x020005F9 RID: 1529
Public Class DragonLevelPeashotFlash
	Inherits AbstractPausableComponent

	' Token: 0x06001E6F RID: 7791 RVA: 0x00118B34 File Offset: 0x00116F34
	Public Sub Flash()
		MyBase.transform.SetScale(New Single?(1F), New Single?(CSng(MathUtils.PlusOrMinus())), New Single?(1F))
		MyBase.animator.SetInteger("i", Global.UnityEngine.Random.Range(0, 4))
		MyBase.animator.SetTrigger("OnChange")
	End Sub
End Class
