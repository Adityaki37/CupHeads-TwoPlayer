Imports System

' Token: 0x02000467 RID: 1127
Public Class PlayerJoinPrompt
	Inherits FlashingPrompt

	' Token: 0x170002B2 RID: 690
	' (get) Token: 0x06001136 RID: 4406 RVA: 0x000A4238 File Offset: 0x000A2638
	Protected Overrides ReadOnly Property ShouldShow As Boolean
		Get
			Return PlayerManager.ShouldShowJoinPrompt
		End Get
	End Property
End Class
