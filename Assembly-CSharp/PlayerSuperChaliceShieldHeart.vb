Imports System
Imports UnityEngine

' Token: 0x02000A57 RID: 2647
Public Class PlayerSuperChaliceShieldHeart
	Inherits MonoBehaviour

	' Token: 0x06003F16 RID: 16150 RVA: 0x00228CE5 File Offset: 0x002270E5
	Public Sub Destroy()
		Me.popped = True
		Me.animator.Play("HeartDie")
		AudioManager.Play("player_super_chalice_shield_end")
	End Sub

	' Token: 0x06003F17 RID: 16151 RVA: 0x00228D08 File Offset: 0x00227108
	Private Sub HeartDie()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06003F18 RID: 16152 RVA: 0x00228D18 File Offset: 0x00227118
	Private Sub FixedUpdate()
		If Not Me.popped AndAlso Me.player IsNot Nothing Then
			Me.offset = New Vector3(Me.hoverWidth * Mathf.Cos(Me.hoverTime) / (1F + Mathf.Sin(Me.hoverTime) * Mathf.Sin(Me.hoverTime)), Me.hoverWidth * Mathf.Sin(Me.hoverTime) * Mathf.Cos(Me.hoverTime) / (1F + Mathf.Sin(Me.hoverTime) * Mathf.Sin(Me.hoverTime)))
			Me.hoverTime += CupheadTime.FixedDelta * 2F
			Me.lerpSpeed = Mathf.Min(Me.lerpSpeed + CupheadTime.FixedDelta, 3F)
			MyBase.transform.position = Vector3.Lerp(MyBase.transform.position, Me.player.transform.position + Me.offset, CupheadTime.FixedDelta * Me.lerpSpeed)
			MyBase.transform.localScale = New Vector3(Me.player.transform.localScale.x, 1F)
		End If
	End Sub

	' Token: 0x0400462B RID: 17963
	<SerializeField()>
	Private animator As Animator

	' Token: 0x0400462C RID: 17964
	Public player As Transform

	' Token: 0x0400462D RID: 17965
	Private offset As Vector3

	' Token: 0x0400462E RID: 17966
	Private hoverTime As Single = 1.5707964F

	' Token: 0x0400462F RID: 17967
	Private hoverWidth As Single = 100F

	' Token: 0x04004630 RID: 17968
	Private popped As Boolean

	' Token: 0x04004631 RID: 17969
	Private lerpSpeed As Single
End Class
