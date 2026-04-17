Imports System
Imports UnityEngine

' Token: 0x020004BB RID: 1211
Public Class AirplaneLevelHydrantAttack
	Inherits MonoBehaviour

	' Token: 0x0600140E RID: 5134 RVA: 0x000B297A File Offset: 0x000B0D7A
	Private Sub AniEvent_SpawnHydrant(ev As AnimationEvent)
		CType(ev.objectReferenceParameter, GameObject).GetComponent(Of BasicProjectile)().Create(Me.spawnPos.position, ev.floatParameter, CSng(ev.intParameter) * 1.5F)
	End Sub

	' Token: 0x0600140F RID: 5135 RVA: 0x000B29B5 File Offset: 0x000B0DB5
	Private Sub SFX_DOGFIGHT_Leader_CopterBG()
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterbackground")
	End Sub

	' Token: 0x06001410 RID: 5136 RVA: 0x000B29C1 File Offset: 0x000B0DC1
	Private Sub SFX_DOGFIGHT_Leader_CopterBGCannonFire()
		AudioManager.Play("sfx_dlc_dogfight_p1_leader_copterbackground_canon")
	End Sub

	' Token: 0x04001D3B RID: 7483
	Private Const SPEED_MODIFIER As Single = 1.5F

	' Token: 0x04001D3C RID: 7484
	<SerializeField()>
	Private spawnPos As Transform

	' Token: 0x04001D3D RID: 7485
	Private speed As Single = 800F
End Class
