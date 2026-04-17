Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007EB RID: 2027
Public Class SnowCultLevelIcePillar
	Inherits AbstractProjectile

	' Token: 0x06002E6A RID: 11882 RVA: 0x001B609C File Offset: 0x001B449C
	Public Overridable Function Init(pos As Vector3, properties As LevelProperties.SnowCult.IcePillar, typeToPlay As Boolean, timeToDelay As Single) As SnowCultLevelIcePillar
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = pos
		Me.typeString = If((Not typeToPlay), "B", "A")
		MyBase.animator.Play("IceBlade_Start" + Me.typeString)
		Me.timeToDelay = timeToDelay
		Me.outTime = properties.outTime
		Me.Attack()
		Return Me
	End Function

	' Token: 0x06002E6B RID: 11883 RVA: 0x001B6112 File Offset: 0x001B4512
	Private Sub Attack()
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x06002E6C RID: 11884 RVA: 0x001B6124 File Offset: 0x001B4524
	Private Iterator Function attack_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.timeToDelay)
		MyBase.animator.SetTrigger("popUp")
		Me.SFX_SNOWCULT_BladeStabfromGround()
		Return
	End Function

	' Token: 0x06002E6D RID: 11885 RVA: 0x001B613F File Offset: 0x001B453F
	Private Sub WaitAndRetract()
		MyBase.StartCoroutine(Me.waitandretract_cr())
	End Sub

	' Token: 0x06002E6E RID: 11886 RVA: 0x001B6150 File Offset: 0x001B4550
	Private Iterator Function waitandretract_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.outTime)
		MyBase.animator.SetTrigger("popDown")
		Return
	End Function

	' Token: 0x06002E6F RID: 11887 RVA: 0x001B616B File Offset: 0x001B456B
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002E70 RID: 11888 RVA: 0x001B6189 File Offset: 0x001B4589
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x06002E71 RID: 11889 RVA: 0x001B6191 File Offset: 0x001B4591
	Private Sub WarningSmokeFX()
		Me.warningSmoke.Create(MyBase.transform.position)
	End Sub

	' Token: 0x06002E72 RID: 11890 RVA: 0x001B61AA File Offset: 0x001B45AA
	Private Sub SFX_SNOWCULT_BladeStabfromGround()
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_blade_stabfromground")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_blade_stabfromground")
	End Sub

	' Token: 0x04003703 RID: 14083
	Private Const Y_POS_START As Single = -430F

	' Token: 0x04003704 RID: 14084
	Private Const Y_POS_END As Single = -200F

	' Token: 0x04003705 RID: 14085
	Private typeString As String

	' Token: 0x04003706 RID: 14086
	Private timeToDelay As Single

	' Token: 0x04003707 RID: 14087
	Private outTime As Single

	' Token: 0x04003708 RID: 14088
	<SerializeField()>
	Private warningSmoke As Effect
End Class
