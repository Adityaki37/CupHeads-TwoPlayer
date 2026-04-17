Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007EA RID: 2026
Public Class SnowCultLevelEyeProjectile
	Inherits AbstractProjectile

	' Token: 0x17000415 RID: 1045
	' (get) Token: 0x06002E5B RID: 11867 RVA: 0x001B511B File Offset: 0x001B351B
	' (set) Token: 0x06002E5C RID: 11868 RVA: 0x001B5123 File Offset: 0x001B3523
	Public Property IsGone As Boolean

	' Token: 0x06002E5D RID: 11869 RVA: 0x001B512C File Offset: 0x001B352C
	Public Overridable Function Init(startPos As Vector3, endPos As Vector3, onRight As Boolean, upsideDown As Boolean, properties As LevelProperties.SnowCult.EyeAttack) As SnowCultLevelEyeProjectile
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		Me.startPos = startPos
		MyBase.transform.position = startPos
		Me.properties = properties
		Me.endPos = endPos
		Me.onRight = onRight
		MyBase.transform.localScale = New Vector3(CSng(If((Not onRight), (-1), 1)), 1F)
		Me.upsideDown = upsideDown
		Me.readyToOpenMouth = False
		Me.angle = 0F
		Me.IsGone = False
		MyBase.StartCoroutine(Me.move_cr())
		Me.beamCR = MyBase.StartCoroutine(Me.beam_cr())
		Return Me
	End Function

	' Token: 0x06002E5E RID: 11870 RVA: 0x001B51D2 File Offset: 0x001B35D2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002E5F RID: 11871 RVA: 0x001B51F0 File Offset: 0x001B35F0
	Private Iterator Function beam_cr() As IEnumerator
		Me.beamAnimator.Play("AuraStart")
		Dim delay As Single = Me.properties.initialBeamDelay.RandomFloat()
		While Not Me.IsGone
			Yield CupheadTime.WaitForSeconds(Me, delay)
			delay = Me.properties.beamDelay
			Me.beamAnimator.SetBool("Attack", True)
			Me.SFX_SNOWCULT_JackFrostEyeballZap()
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.beamDuration)
			Me.beamAnimator.SetBool("Attack", False)
		End While
		Return
	End Function

	' Token: 0x06002E60 RID: 11872 RVA: 0x001B520C File Offset: 0x001B360C
	Private Iterator Function move_in_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		If Not Me.onRight Then
			While MyBase.transform.position.x < Me.properties.distanceToTurn
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position += Vector3.right * Me.properties.eyeStraightSpeed * CupheadTime.FixedDelta
				Yield wait
			End While
		Else
			While MyBase.transform.position.x > -Me.properties.distanceToTurn
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position += Vector3.left * Me.properties.eyeStraightSpeed * CupheadTime.FixedDelta
				Yield wait
			End While
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002E61 RID: 11873 RVA: 0x001B5228 File Offset: 0x001B3628
	Private Iterator Function move_out_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		If Not Me.onRight Then
			While MyBase.transform.position.x < Me.endPos.x - Me.openMouthDistance
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position += Vector3.right * Me.properties.eyeStraightSpeed * CupheadTime.FixedDelta
				Yield wait
			End While
		Else
			While MyBase.transform.position.x > Me.endPos.x + Me.openMouthDistance
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position += Vector3.left * Me.properties.eyeStraightSpeed * CupheadTime.FixedDelta
				Yield wait
			End While
		End If
		Me.readyToOpenMouth = True
		If Not Me.onRight Then
			While MyBase.transform.position.x < Me.endPos.x - Me.beamEndDistance
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position += Vector3.right * Me.properties.eyeStraightSpeed * CupheadTime.FixedDelta
				Yield wait
			End While
		Else
			While MyBase.transform.position.x > Me.endPos.x + Me.beamEndDistance
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position += Vector3.left * Me.properties.eyeStraightSpeed * CupheadTime.FixedDelta
				Yield wait
			End While
		End If
		MyBase.StopCoroutine(Me.beamCR)
		Me.beamAnimator.SetBool("Attack", False)
		Me.beamAnimator.SetTrigger("End")
		If Not Me.onRight Then
			While MyBase.transform.position.x < Me.endPos.x - Me.animatorTakeoverDistance
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position += Vector3.right * Me.properties.eyeStraightSpeed * CupheadTime.FixedDelta
				Yield wait
			End While
		Else
			While MyBase.transform.position.x > Me.endPos.x + Me.animatorTakeoverDistance
				Me.lastPos = MyBase.transform.position
				MyBase.transform.position += Vector3.left * Me.properties.eyeStraightSpeed * CupheadTime.FixedDelta
				Yield wait
			End While
		End If
		Me.readyToCloseMouth = True
		Me.controlledByParent = True
		Yield Nothing
		Me.shadow.SetActive(True)
		Return
	End Function

	' Token: 0x06002E62 RID: 11874 RVA: 0x001B5244 File Offset: 0x001B3644
	Private Iterator Function move_cr() As IEnumerator
		Me.SFX_SNOWCULT_JackFrostEyeballLoop()
		Dim loopSpeed As Single = Me.properties.eyeCurveSpeed
		Dim pivotY As Single = (Me.startPos.y + Me.endPos.y) / 2F
		Dim loopSizeY As Single = Mathf.Abs(Me.startPos.y - Me.endPos.y) / 2F
		Dim loopSizeX As Single = loopSizeY
		Dim pivotX As Single = If((Not Me.onRight), Me.properties.distanceToTurn, (-Me.properties.distanceToTurn))
		Dim pivot As Vector3 = New Vector3(pivotX, pivotY)
		Dim angleToStopAt As Single = 3.1415927F
		If Not Me.upsideDown Then
			MyBase.transform.SetPosition(Nothing, New Single?(pivot.y - loopSizeY), Nothing)
		Else
			MyBase.transform.SetPosition(Nothing, New Single?(pivot.y + loopSizeY), Nothing)
		End If
		Me.angle *= 0.017453292F
		Yield MyBase.StartCoroutine(Me.move_in_cr())
		While Me.angle < angleToStopAt
			Me.angle += loopSpeed * CupheadTime.FixedDelta
			Dim handleRotationX As Vector3
			If Not Me.onRight Then
				handleRotationX = New Vector3(Mathf.Sin(Me.angle) * loopSizeX, 0F, 0F)
			Else
				handleRotationX = New Vector3(-Mathf.Sin(Me.angle) * loopSizeX, 0F, 0F)
			End If
			Dim handleRotationY As Vector3
			If Not Me.upsideDown Then
				handleRotationY = New Vector3(0F, -Mathf.Cos(Me.angle) * loopSizeY, 0F)
			Else
				handleRotationY = New Vector3(0F, Mathf.Cos(Me.angle) * loopSizeY, 0F)
			End If
			Me.lastPos = MyBase.transform.position
			MyBase.transform.position = pivot
			MyBase.transform.position += handleRotationX + handleRotationY
			Yield New WaitForFixedUpdate()
		End While
		Me.onRight = Not Me.onRight
		MyBase.StartCoroutine(Me.move_out_cr())
		Return
	End Function

	' Token: 0x06002E63 RID: 11875 RVA: 0x001B525F File Offset: 0x001B365F
	Public Sub ReturnToSnowflake()
		Me.SFX_SNOWCULT_JackFrostEyeballLoopStop()
		Me.Recycle()
		Me.IsGone = True
	End Sub

	' Token: 0x06002E64 RID: 11876 RVA: 0x001B5274 File Offset: 0x001B3674
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.dead Then
			MyBase.transform.position += Me.vel * CupheadTime.FixedDelta
		End If
	End Sub

	' Token: 0x06002E65 RID: 11877 RVA: 0x001B52B0 File Offset: 0x001B36B0
	Private Sub LateUpdate()
		If Me.main.dead <> Me.dead Then
			Me.dead = True
			Me.vel /= CupheadTime.FixedDelta
			Me.SFX_SNOWCULT_JackFrostEyeballLoopStop()
			Me.StopAllCoroutines()
		ElseIf Not Me.dead Then
			Me.vel = MyBase.transform.position - Me.lastPos
		End If
		If Me.controlledByParent Then
			MyBase.transform.position = Me.main.eyeProjectileGuide.position
		End If
	End Sub

	' Token: 0x06002E66 RID: 11878 RVA: 0x001B534E File Offset: 0x001B374E
	Private Sub SFX_SNOWCULT_JackFrostEyeballLoop()
		AudioManager.PlayLoop("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop")
	End Sub

	' Token: 0x06002E67 RID: 11879 RVA: 0x001B536A File Offset: 0x001B376A
	Private Sub SFX_SNOWCULT_JackFrostEyeballLoopStop()
		AudioManager.[Stop]("sfx_dlc_snowcult_p3_snowflake_eyeball_attack_loop")
	End Sub

	' Token: 0x06002E68 RID: 11880 RVA: 0x001B5376 File Offset: 0x001B3776
	Private Sub SFX_SNOWCULT_JackFrostEyeballZap()
		AudioManager.Play("sfx_dlc_snowcult_p3_snowflake_eyeball_zap")
		Me.emitAudioFromObject.Add("sfx_dlc_snowcult_p3_snowflake_eyeball_zap")
	End Sub

	' Token: 0x040036EF RID: 14063
	Private properties As LevelProperties.SnowCult.EyeAttack

	' Token: 0x040036F0 RID: 14064
	Private endPos As Vector3

	' Token: 0x040036F1 RID: 14065
	Private startPos As Vector3

	' Token: 0x040036F2 RID: 14066
	Private angle As Single

	' Token: 0x040036F3 RID: 14067
	Private onRight As Boolean

	' Token: 0x040036F4 RID: 14068
	Private upsideDown As Boolean

	' Token: 0x040036F5 RID: 14069
	Public readyToOpenMouth As Boolean

	' Token: 0x040036F6 RID: 14070
	Public readyToCloseMouth As Boolean

	' Token: 0x040036F8 RID: 14072
	<SerializeField()>
	Private beamAnimator As Animator

	' Token: 0x040036F9 RID: 14073
	<SerializeField()>
	Private openMouthDistance As Single = 400F

	' Token: 0x040036FA RID: 14074
	<SerializeField()>
	Private beamEndDistance As Single = 200F

	' Token: 0x040036FB RID: 14075
	<SerializeField()>
	Private animatorTakeoverDistance As Single = 31F

	' Token: 0x040036FC RID: 14076
	Public main As SnowCultLevelJackFrost

	' Token: 0x040036FD RID: 14077
	Private beamCR As Coroutine

	' Token: 0x040036FE RID: 14078
	Private controlledByParent As Boolean

	' Token: 0x040036FF RID: 14079
	<SerializeField()>
	Private shadow As GameObject

	' Token: 0x04003700 RID: 14080
	Private vel As Vector3

	' Token: 0x04003701 RID: 14081
	Private lastPos As Vector3

	' Token: 0x04003702 RID: 14082
	Private dead As Boolean
End Class
