Imports System
Imports UnityEngine

' Token: 0x020007F3 RID: 2035
Public Class SnowCultLevelShard
	Inherits BasicProjectileContinuesOnLevelEnd

	' Token: 0x06002EBE RID: 11966 RVA: 0x001B965C File Offset: 0x001B7A5C
	Public Overridable Function Init(pivotPos As Vector3, angle As Single, loopSizeX As Single, loopSizeY As Single, properties As LevelProperties.SnowCult.ShardAttack) As SnowCultLevelShard
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Me.pivotPos = pivotPos
		Me.speed = properties.shardSpeed
		Me.Health = properties.shardHealth
		MyBase.GetComponent(Of Collider2D)().enabled = False
		angle *= 0.017453292F
		MyBase.transform.position = pivotPos + New Vector3(-Mathf.Sin(angle) * loopSizeX, Mathf.Cos(angle) * loopSizeY)
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(90F + MathUtils.DirectionToAngle(pivotPos - MyBase.transform.position)))
		Me.basePos = MyBase.transform.position
		Me.SFX_SNOWCULT_JackFrostIceCreamProjSplatLoop()
		Return Me
	End Function

	' Token: 0x06002EBF RID: 11967 RVA: 0x001B9733 File Offset: 0x001B7B33
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.Health -= info.damage
		If Me.Health < 0F Then
			Me.Recycle()
		End If
	End Sub

	' Token: 0x06002EC0 RID: 11968 RVA: 0x001B975E File Offset: 0x001B7B5E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002EC1 RID: 11969 RVA: 0x001B977C File Offset: 0x001B7B7C
	Public Sub Appear()
		MyBase.animator.SetTrigger("Appear")
		Me.SFX_SNOWCULT_JackFrostIcecreamAppear()
		Me.smoke.Create(MyBase.transform.position)
		MyBase.GetComponent(Of Collider2D)().enabled = True
	End Sub

	' Token: 0x06002EC2 RID: 11970 RVA: 0x001B97B7 File Offset: 0x001B7BB7
	Public Sub LaunchProjectile()
		MyBase.animator.SetTrigger("StartMove")
		Me.moving = True
	End Sub

	' Token: 0x06002EC3 RID: 11971 RVA: 0x001B97D0 File Offset: 0x001B7BD0
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not Me.moving Then
			MyBase.transform.position = Me.basePos + Vector3.right * Mathf.Sin(Me.wobbleTimer * 2F) * Me.wobbleX + Vector3.up * Mathf.Cos(Me.wobbleTimer * 3F) * Me.wobbleY
			Me.wobbleTimer += CupheadTime.FixedDelta * Me.wobbleSpeed
		Else
			MyBase.transform.position += MyBase.transform.up * -Me.speed * CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x06002EC4 RID: 11972 RVA: 0x001B98AA File Offset: 0x001B7CAA
	Private Sub SFX_SNOWCULT_JackFrostIceCreamProjSplatLoop()
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop")
	End Sub

	' Token: 0x06002EC5 RID: 11973 RVA: 0x001B98C6 File Offset: 0x001B7CC6
	Private Sub SFX_SNOWCULT_JackFrostIcecreamAppear()
		AudioManager.[Stop]("sfx_dlc_snowcult_p3_snowflake_icecreamcone_splat_pre_loop")
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_icecreamcone_appear")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_icecreamcone_appear")
	End Sub

	' Token: 0x04003766 RID: 14182
	Private basePos As Vector3

	' Token: 0x04003767 RID: 14183
	<SerializeField()>
	Private wobbleX As Single = 10F

	' Token: 0x04003768 RID: 14184
	<SerializeField()>
	Private wobbleY As Single = 10F

	' Token: 0x04003769 RID: 14185
	<SerializeField()>
	Private wobbleSpeed As Single = 2F

	' Token: 0x0400376A RID: 14186
	Private wobbleTimer As Single

	' Token: 0x0400376B RID: 14187
	Private speed As Single

	' Token: 0x0400376C RID: 14188
	Private Health As Single

	' Token: 0x0400376D RID: 14189
	Private moving As Boolean

	' Token: 0x0400376E RID: 14190
	Private pivotPos As Vector2

	' Token: 0x0400376F RID: 14191
	Private damageReceiver As DamageReceiver

	' Token: 0x04003770 RID: 14192
	<SerializeField()>
	Private smoke As Effect
End Class
