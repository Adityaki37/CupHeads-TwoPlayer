Imports System

' Token: 0x020004A0 RID: 1184
<Serializable()>
Public Structure CoinPositionAndID
	' Token: 0x06001349 RID: 4937 RVA: 0x000AAA53 File Offset: 0x000A8E53
	Public Sub New(id As String, pos As Single)
		Me.CoinID = id
		Me.xPos = pos
	End Sub

	' Token: 0x04001C70 RID: 7280
	Public CoinID As String

	' Token: 0x04001C71 RID: 7281
	Public xPos As Single
End Structure
