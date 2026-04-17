Imports System
Imports UnityEngine
Imports UnityEngine.UI

' Token: 0x0200048F RID: 1167
Public Class LevelHUDPlayerSuperCard
	Inherits AbstractMonoBehaviour

	' Token: 0x06001256 RID: 4694 RVA: 0x000A9D71 File Offset: 0x000A8171
	Private Sub Start()
		Me.[end] = MyBase.transform.localPosition
		Me.start = Me.[end] + New Vector3(0F, -30F, 0F)
	End Sub

	' Token: 0x06001257 RID: 4695 RVA: 0x000A9DA9 File Offset: 0x000A81A9
	Private Sub Update()
		Me.UpdatePosition()
	End Sub

	' Token: 0x06001258 RID: 4696 RVA: 0x000A9DB4 File Offset: 0x000A81B4
	Private Sub UpdatePosition()
		If Not Me.initialized Then
			Return
		End If
		Me.target = Vector3.Lerp(Me.start, Me.[end], Me.current / Me.max)
		MyBase.transform.localPosition = Vector3.Lerp(MyBase.transform.localPosition, Me.target, CupheadTime.Delta * 10F)
		Me.image.fillAmount = Mathf.Lerp(Me.image.fillAmount, Me.current / Me.max, CupheadTime.Delta * 10F)
	End Sub

	' Token: 0x06001259 RID: 4697 RVA: 0x000A9E5C File Offset: 0x000A825C
	Public Sub Init(playerId As PlayerId, exCost As Single)
		Me.max = exCost
		If playerId <> PlayerId.PlayerOne Then
			If playerId <> PlayerId.PlayerTwo Then
				If playerId <> PlayerId.Any AndAlso playerId <> PlayerId.None Then
				End If
			Else
				MyBase.animator.SetInteger("Player", If((Not PlayerManager.player1IsMugman), 1, 0))
			End If
		Else
			MyBase.animator.SetInteger("Player", If((Not PlayerManager.player1IsMugman), 0, 1))
		End If
		Me.initialized = True
	End Sub

	' Token: 0x0600125A RID: 4698 RVA: 0x000A9EF0 File Offset: 0x000A82F0
	Public Sub SetAmount(amount As Single)
		Me.current = Mathf.Clamp(amount, 0F, Me.max)
	End Sub

	' Token: 0x0600125B RID: 4699 RVA: 0x000A9F09 File Offset: 0x000A8309
	Public Sub SetSuper(super As Boolean)
		MyBase.animator.SetBool("Super", super)
	End Sub

	' Token: 0x0600125C RID: 4700 RVA: 0x000A9F1C File Offset: 0x000A831C
	Public Sub SetEx(ex As Boolean)
		MyBase.animator.SetBool("Ex", ex)
	End Sub

	' Token: 0x04001BC3 RID: 7107
	Private Const SPEED As Single = 10F

	' Token: 0x04001BC4 RID: 7108
	Private Const Y_DIFF As Single = -30F

	' Token: 0x04001BC5 RID: 7109
	<SerializeField()>
	Private image As Image

	' Token: 0x04001BC6 RID: 7110
	Private initialized As Boolean

	' Token: 0x04001BC7 RID: 7111
	Private current As Single

	' Token: 0x04001BC8 RID: 7112
	Private max As Single

	' Token: 0x04001BC9 RID: 7113
	Private start As Vector3

	' Token: 0x04001BCA RID: 7114
	Private [end] As Vector3

	' Token: 0x04001BCB RID: 7115
	Private target As Vector3
End Class
