Imports System
Imports UnityEngine

' Token: 0x0200071D RID: 1821
Public Class PirateLevelBoatBeam
	Inherits ParrySwitch

	' Token: 0x060027A3 RID: 10147 RVA: 0x00173EDC File Offset: 0x001722DC
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = New DamageDealer(1F, 0.1F, DamageDealer.DamageSource.Enemy, True, False, False)
		Me.damageDealer.SetDirection(DamageDealer.Direction.Left, MyBase.transform)
	End Sub

	' Token: 0x060027A4 RID: 10148 RVA: 0x00173F0F File Offset: 0x0017230F
	Private Sub Update()
		Me.damageDealer.Update()
	End Sub

	' Token: 0x060027A5 RID: 10149 RVA: 0x00173F1C File Offset: 0x0017231C
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase = CollisionPhase.[Exit] Then
			Return
		End If
		Dim component As LevelPlayerController = hit.GetComponent(Of LevelPlayerController)()
		If component Is Nothing OrElse component.Ducking Then
			Return
		End If
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060027A6 RID: 10150 RVA: 0x00173F60 File Offset: 0x00172360
	Public Function Create(parent As Transform) As PirateLevelBoatBeam
		Dim pirateLevelBoatBeam As PirateLevelBoatBeam = Me.InstantiatePrefab(Of PirateLevelBoatBeam)()
		pirateLevelBoatBeam.Init(parent)
		Return pirateLevelBoatBeam
	End Function

	' Token: 0x060027A7 RID: 10151 RVA: 0x00173F7C File Offset: 0x0017237C
	Private Sub Init(parent As Transform)
		AudioManager.Play("level_pirate_ship_beam_fire")
		MyBase.transform.SetParent(parent)
		MyBase.transform.ResetLocalPosition()
		MyBase.transform.ResetLocalRotation()
	End Sub

	' Token: 0x060027A8 RID: 10152 RVA: 0x00173FAA File Offset: 0x001723AA
	Public Sub StartBeam()
	End Sub

	' Token: 0x060027A9 RID: 10153 RVA: 0x00173FAC File Offset: 0x001723AC
	Public Sub EndBeam()
		MyBase.animator.SetTrigger("OnEnd")
	End Sub

	' Token: 0x060027AA RID: 10154 RVA: 0x00173FBE File Offset: 0x001723BE
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		MyBase.OnParryPrePause(player)
		player.stats.ParryOneQuarter()
	End Sub

	' Token: 0x060027AB RID: 10155 RVA: 0x00173FD2 File Offset: 0x001723D2
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		MyBase.StartParryCooldown()
	End Sub

	' Token: 0x060027AC RID: 10156 RVA: 0x00173FE1 File Offset: 0x001723E1
	Private Sub OnEndAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400306A RID: 12394
	Private damageDealer As DamageDealer
End Class
