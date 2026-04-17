Imports System

' Token: 0x02000AC7 RID: 2759
Public MustInherit Class PlayerEvent(Of T As{PlayerEvent(Of T), New})
	Inherits GameEvent

	' Token: 0x0600423D RID: 16957 RVA: 0x0023C31D File Offset: 0x0023A71D
	Public Sub New()
	End Sub

	' Token: 0x170005CE RID: 1486
	' (get) Token: 0x0600423E RID: 16958 RVA: 0x0023C325 File Offset: 0x0023A725
	' (set) Token: 0x0600423F RID: 16959 RVA: 0x0023C32D File Offset: 0x0023A72D
	Public Property playerId As PlayerId

	' Token: 0x06004240 RID: 16960 RVA: 0x0023C336 File Offset: 0x0023A736
	Public Shared Function [Shared](playerId As PlayerId) As T
		If PlayerEvent(Of T)._instance Is Nothing Then
			PlayerEvent(Of T)._instance = New T()
		End If
		PlayerEvent(Of T)._instance.playerId = playerId
		Return PlayerEvent(Of T)._instance
	End Function

	' Token: 0x040048B1 RID: 18609
	Public Shared _instance As T
End Class
