Imports System
Imports UnityEngine
Imports UnityEngine.UI

Namespace TMPro
	' Token: 0x02000C7E RID: 3198
	Public Interface ITextElement
		' Token: 0x1700085F RID: 2143
		' (get) Token: 0x06005062 RID: 20578
		ReadOnly Property sharedMaterial As Material

		' Token: 0x06005063 RID: 20579
		Sub Rebuild(update As CanvasUpdate)

		' Token: 0x06005064 RID: 20580
		Function GetInstanceID() As Integer
	End Interface
End Namespace
