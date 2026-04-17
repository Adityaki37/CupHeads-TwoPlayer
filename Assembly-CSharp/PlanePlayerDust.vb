Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A9B RID: 2715
Public Class PlanePlayerDust
	Inherits AbstractMonoBehaviour

	' Token: 0x06004112 RID: 16658 RVA: 0x002359B3 File Offset: 0x00233DB3
	Public Sub Initialize(playerController As AbstractPlayerController, smallY As Single, bigY As Single)
		Me.playerController = CType(playerController, PlanePlayerController)
		Me.smallY = smallY
		Me.bigY = bigY
		If playerController IsNot Nothing Then
			MyBase.StartCoroutine(Me.setupSorting_cr())
		End If
	End Sub

	' Token: 0x06004113 RID: 16659 RVA: 0x002359E8 File Offset: 0x00233DE8
	Private Iterator Function setupSorting_cr() As IEnumerator
		While Me.playerController.animationController.spriteRenderer Is Nothing
			Yield Nothing
		End While
		Dim playerOrder As Integer = Me.playerController.animationController.spriteRenderer.sortingOrder
		Me.shadowRenderer.sortingOrder += playerOrder
		Me.backRenderer.sortingOrder += playerOrder
		Me.frontRenderer.sortingOrder += playerOrder
		Return
	End Function

	' Token: 0x06004114 RID: 16660 RVA: 0x00235A04 File Offset: 0x00233E04
	Private Sub Update()
		If Me.playerController Is Nothing Then
			Return
		End If
		If Me.playerController.IsDead Then
			MyBase.animator.SetInteger(PlanePlayerDust.SizeParameter, 0)
			MyBase.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, False)
			Me.shadowRenderer.enabled = False
			Return
		End If
		Dim bottom As Single = Me.playerController.bottom
		If bottom < Me.bigY Then
			MyBase.animator.SetInteger(PlanePlayerDust.SizeParameter, 2)
			MyBase.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, True)
		ElseIf bottom < Me.smallY Then
			MyBase.animator.SetInteger(PlanePlayerDust.SizeParameter, 1)
			Me.setManualShadow(bottom)
		Else
			MyBase.animator.SetInteger(PlanePlayerDust.SizeParameter, 0)
			Me.setManualShadow(bottom)
		End If
		MyBase.transform.position = New Vector3(Me.playerController.center.x, Me.bigY) + PlanePlayerDust.PositionOffset
	End Sub

	' Token: 0x06004115 RID: 16661 RVA: 0x00235B18 File Offset: 0x00233F18
	Private Sub setManualShadow(bottom As Single)
		MyBase.animator.SetBool(PlanePlayerDust.ShadowLoopParameter, False)
		If bottom >= Me.smallY Then
			Me.shadowRenderer.enabled = False
		Else
			Me.shadowRenderer.enabled = True
			Dim num As Single = MathUtilities.LerpMapping(bottom, Me.smallY, Me.bigY, 0F, CSng(PlanePlayerDust.ManualShadowSpriteCount), True)
			MyBase.animator.Play(PlanePlayerDust.ManualState, 2, num / CSng(PlanePlayerDust.ManualShadowSpriteCount))
		End If
	End Sub

	' Token: 0x040047AC RID: 18348
	Private Shared SizeParameter As Integer = Animator.StringToHash("Size")

	' Token: 0x040047AD RID: 18349
	Private Shared ShadowLoopParameter As Integer = Animator.StringToHash("ShadowLoop")

	' Token: 0x040047AE RID: 18350
	Private Shared ManualState As Integer = Animator.StringToHash("Manual")

	' Token: 0x040047AF RID: 18351
	Private Shared PositionOffset As Vector3 = New Vector3(-70F, -20F)

	' Token: 0x040047B0 RID: 18352
	Private Shared ManualShadowSpriteCount As Integer = 7

	' Token: 0x040047B1 RID: 18353
	<SerializeField()>
	Private shadowRenderer As SpriteRenderer

	' Token: 0x040047B2 RID: 18354
	<SerializeField()>
	Private backRenderer As SpriteRenderer

	' Token: 0x040047B3 RID: 18355
	<SerializeField()>
	Private frontRenderer As SpriteRenderer

	' Token: 0x040047B4 RID: 18356
	Private smallY As Single

	' Token: 0x040047B5 RID: 18357
	Private bigY As Single

	' Token: 0x040047B6 RID: 18358
	Private playerController As PlanePlayerController
End Class
