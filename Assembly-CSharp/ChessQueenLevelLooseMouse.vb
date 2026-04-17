Imports System
Imports UnityEngine

' Token: 0x0200054D RID: 1357
Public Class ChessQueenLevelLooseMouse
	Inherits MonoBehaviour

	' Token: 0x06001916 RID: 6422 RVA: 0x000E36B7 File Offset: 0x000E1AB7
	Private Sub Start()
		Me.jumpSwitchTime = Global.UnityEngine.Random.Range(2F, 3F)
	End Sub

	' Token: 0x06001917 RID: 6423 RVA: 0x000E36D0 File Offset: 0x000E1AD0
	Private Sub Update()
		Me.anim.SetBool("Right", Me.queen.transform.position.x > -200F)
		If Not Me.won AndAlso Me.activeCannonball Is Nothing Then
			Me.jumpSwitchTime -= CupheadTime.Delta
			If Me.jumpSwitchTime < 0F Then
				Me.anim.SetBool("Jump", Not Me.anim.GetBool("Jump"))
				Me.jumpSwitchTime = If((Not Me.anim.GetBool("Jump")), Global.UnityEngine.Random.Range(3F, 4F), Global.UnityEngine.Random.Range(0.5F, 2F))
			End If
		End If
	End Sub

	' Token: 0x06001918 RID: 6424 RVA: 0x000E37B0 File Offset: 0x000E1BB0
	Public Sub HitQueen()
		Me.anim.SetTrigger("HitQueen")
		Me.anim.SetBool("Jump", True)
		Me.jumpSwitchTime = Global.UnityEngine.Random.Range(2F, 3F)
	End Sub

	' Token: 0x06001919 RID: 6425 RVA: 0x000E37E8 File Offset: 0x000E1BE8
	Public Sub CannonFired(cannonBall As GameObject)
		Me.anim.SetBool("Jump", False)
		Me.activeCannonball = cannonBall
		Me.jumpSwitchTime = Global.UnityEngine.Random.Range(3F, 4F)
	End Sub

	' Token: 0x0600191A RID: 6426 RVA: 0x000E3817 File Offset: 0x000E1C17
	Public Sub Win()
		Me.anim.SetBool("Jump", True)
		Me.won = True
	End Sub

	' Token: 0x04002230 RID: 8752
	<SerializeField()>
	Private anim As Animator

	' Token: 0x04002231 RID: 8753
	<SerializeField()>
	Private queen As ChessQueenLevelQueen

	' Token: 0x04002232 RID: 8754
	Private won As Boolean

	' Token: 0x04002233 RID: 8755
	Private jumpSwitchTime As Single

	' Token: 0x04002234 RID: 8756
	Private activeCannonball As GameObject
End Class
