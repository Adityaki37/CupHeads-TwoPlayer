Imports System
Imports UnityEngine

' Token: 0x020004BF RID: 1215
Public Class AirplaneLevelLeaderAnimation
	Inherits MonoBehaviour

	' Token: 0x06001437 RID: 5175 RVA: 0x000B45E0 File Offset: 0x000B29E0
	Private Sub Start()
		Me.rootPosition = MyBase.transform.position
	End Sub

	' Token: 0x06001438 RID: 5176 RVA: 0x000B45F3 File Offset: 0x000B29F3
	Private Sub AniEvent_StartBulldog()
		Me.bulldogAnimation.SetTrigger("Continue")
	End Sub

	' Token: 0x06001439 RID: 5177 RVA: 0x000B4605 File Offset: 0x000B2A05
	Private Sub AniEvent_SFX_LeaderBark()
		AudioManager.Play("sfx_dlc_dogfight_leadervocal_introbark")
	End Sub

	' Token: 0x0600143A RID: 5178 RVA: 0x000B4614 File Offset: 0x000B2A14
	Private Sub Update()
		MyBase.transform.position = Me.rootPosition + Mathf.Sin(Me.wobbleTimer * 3F) * Me.wobbleX * Vector3.right + Mathf.Sin(Me.wobbleTimer * 2F) * Me.wobbleY * Vector3.up
		Me.wobbleTimer += CupheadTime.Delta * Me.wobbleSpeed
	End Sub

	' Token: 0x0600143B RID: 5179 RVA: 0x000B469E File Offset: 0x000B2A9E
	Private Sub AnimationEvent_SFX_DOGFIGHT_Intro_LeaderCopterFlyby()
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterflybyexit")
	End Sub

	' Token: 0x04001D61 RID: 7521
	<SerializeField()>
	Private bulldogAnimation As Animator

	' Token: 0x04001D62 RID: 7522
	Private rootPosition As Vector3

	' Token: 0x04001D63 RID: 7523
	Private wobbleTimer As Single

	' Token: 0x04001D64 RID: 7524
	<SerializeField()>
	Private wobbleX As Single = 10F

	' Token: 0x04001D65 RID: 7525
	<SerializeField()>
	Private wobbleY As Single = 10F

	' Token: 0x04001D66 RID: 7526
	<SerializeField()>
	Private wobbleSpeed As Single = 1F
End Class
