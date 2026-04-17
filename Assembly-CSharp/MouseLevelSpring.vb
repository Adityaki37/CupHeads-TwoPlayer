Imports System
Imports UnityEngine

' Token: 0x020006FA RID: 1786
Public Class MouseLevelSpring
	Inherits ParrySwitch

	' Token: 0x0600263F RID: 9791 RVA: 0x00165850 File Offset: 0x00163C50
	Protected Overrides Sub OnCollision(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollision(hit, phase)
		If phase = CollisionPhase.Enter AndAlso hit.GetComponent(Of MouseLevelCanMouse)() IsNot Nothing AndAlso Not Me.isLaunched Then
			Me.smallExplosion.Create(MyBase.transform.position)
			MyBase.animator.SetTrigger("OnDeath")
		End If
	End Sub

	' Token: 0x06002640 RID: 9792 RVA: 0x001658AE File Offset: 0x00163CAE
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		player.GetComponent(Of LevelPlayerMotor)().OnTrampolineKnockUp(Me.knockUpHeight)
		If Not Me.isLaunched Then
			MyBase.animator.SetTrigger("OnLaunch")
		End If
	End Sub

	' Token: 0x06002641 RID: 9793 RVA: 0x001658E3 File Offset: 0x00163CE3
	Private Sub GotRunOver()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06002642 RID: 9794 RVA: 0x00165900 File Offset: 0x00163D00
	Public Sub LaunchSpring(position As Vector2, velocity As Vector2, gravity As Single)
		If MyBase.gameObject.activeSelf Then
			MyBase.animator.Play("Flip")
		End If
		MyBase.transform.position = position
		Me.velocity = velocity
		Me.gravity = gravity
		Me.isLaunched = True
		MyBase.gameObject.SetActive(True)
		MyBase.GetComponent(Of Collider2D)().enabled = True
	End Sub

	' Token: 0x06002643 RID: 9795 RVA: 0x0016596C File Offset: 0x00163D6C
	Private Sub Update()
		If Me.isLaunched Then
			MyBase.transform.AddPosition(Me.velocity.x * CupheadTime.Delta, Me.velocity.y * CupheadTime.Delta, 0F)
			Me.velocity.y = Me.velocity.y - Me.gravity * CupheadTime.Delta
			If MyBase.transform.position.y < CSng(Level.Current.Ground) + Me.offset Then
				MyBase.transform.SetPosition(Nothing, New Single?(CSng(Level.Current.Ground) + Me.offset), Nothing)
				If Me.isLaunched Then
					Me.Landed()
				End If
				Me.isLaunched = False
			End If
		End If
	End Sub

	' Token: 0x06002644 RID: 9796 RVA: 0x00165A59 File Offset: 0x00163E59
	Private Sub Landed()
		MyBase.animator.SetTrigger("OnLand")
		AudioManager.Play("level_mouse_can_springboard_land")
		Me.emitAudioFromObject.Add("level_mouse_can_springboard_land")
	End Sub

	' Token: 0x06002645 RID: 9797 RVA: 0x00165A85 File Offset: 0x00163E85
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.smallExplosion = Nothing
	End Sub

	' Token: 0x04002ECB RID: 11979
	<SerializeField()>
	Private smallExplosion As Effect

	' Token: 0x04002ECC RID: 11980
	Public knockUpHeight As Single = -1.5F

	' Token: 0x04002ECD RID: 11981
	Private isLaunched As Boolean

	' Token: 0x04002ECE RID: 11982
	Private velocity As Vector2

	' Token: 0x04002ECF RID: 11983
	Private gravity As Single

	' Token: 0x04002ED0 RID: 11984
	Private offset As Single = 120F
End Class
