Imports System
Imports UnityEngine

' Token: 0x02000A1E RID: 2590
Public Class LevelPlayerDeathEffect
	Inherits Effect

	' Token: 0x06003D70 RID: 15728 RVA: 0x0021EF88 File Offset: 0x0021D388
	Public Sub Init(pos As Vector2, id As PlayerId, playerGrounded As Boolean)
		MyBase.transform.position = pos
		If id = PlayerId.PlayerOne OrElse id <> PlayerId.PlayerTwo Then
			If PlayerManager.GetPlayer(id).stats.isChalice Then
				Me.chalice.enabled = True
			ElseIf PlayerManager.player1IsMugman Then
				Me.mugman.enabled = True
			Else
				Me.cuphead.enabled = True
			End If
		ElseIf PlayerManager.GetPlayer(id).stats.isChalice Then
			Me.chalice.enabled = True
		ElseIf PlayerManager.player1IsMugman Then
			Me.cuphead.enabled = True
		Else
			Me.mugman.enabled = True
		End If
		If playerGrounded Then
			Me.shadow.enabled = True
		End If
	End Sub

	' Token: 0x06003D71 RID: 15729 RVA: 0x0021F06E File Offset: 0x0021D46E
	Public Sub Init(pos As Vector2)
		MyBase.transform.position = pos
	End Sub

	' Token: 0x040044B1 RID: 17585
	<SerializeField()>
	Private cuphead As SpriteRenderer

	' Token: 0x040044B2 RID: 17586
	<SerializeField()>
	Private mugman As SpriteRenderer

	' Token: 0x040044B3 RID: 17587
	<SerializeField()>
	Private chalice As SpriteRenderer

	' Token: 0x040044B4 RID: 17588
	<SerializeField()>
	Private shadow As SpriteRenderer
End Class
