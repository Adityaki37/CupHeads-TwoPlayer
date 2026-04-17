Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007CE RID: 1998
Public Class SaltbakerLevelLime
	Inherits SaltbakerLevelPhaseOneProjectile

	' Token: 0x06002D5C RID: 11612 RVA: 0x001AC0D4 File Offset: 0x001AA4D4
	Public Overridable Function Init(position As Vector3, onLeft As Boolean, isHigh As Boolean, properties As LevelProperties.Saltbaker.Limes, id As Integer, anim As Integer) As SaltbakerLevelLime
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = position
		Me.properties = properties
		Me.onLeft = onLeft
		Me.isHigh = isHigh
		Me.Move()
		MyBase.animator.Play(anim.ToString())
		Me.sfxID = id
		Me.SFX_SALTBAKER_P1_LimeProjectileLoop()
		Return Me
	End Function

	' Token: 0x06002D5D RID: 11613 RVA: 0x001AC13C File Offset: 0x001AA53C
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002D5E RID: 11614 RVA: 0x001AC15A File Offset: 0x001AA55A
	Private Sub Move()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06002D5F RID: 11615 RVA: 0x001AC169 File Offset: 0x001AA569
	Protected Overrides Sub OnDestroy()
		AudioManager.[Stop]("sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (Me.sfxID + 1))
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06002D60 RID: 11616 RVA: 0x001AC190 File Offset: 0x001AA590
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim startPosX As Single = CSng(If((Not Me.onLeft), Level.Current.Right, Level.Current.Left))
		Dim curveStartY As Single = If((Not Me.isHigh), Me.properties.lowStartY, Me.properties.highStartY)
		Dim curveEndY As Single = If((Not Me.isHigh), Me.properties.lowEndY, Me.properties.highEndY)
		Dim boomerangSpeed As Single = Me.properties.straightSpeed
		Dim distToTurn As Single = Me.properties.distToTurn
		Dim loopSizeX As Single = 100F
		Dim gravity As Single = Me.properties.straightGravity
		Dim speed As Single = boomerangSpeed
		Dim pivotY As Single = Mathf.Lerp(curveStartY, curveEndY, 0.5F)
		Dim pivotX As Single = If((Not Me.onLeft), (-distToTurn), distToTurn)
		Dim loopSizeY As Single = Mathf.Abs(pivotY - curveStartY)
		Me.pivot = New Vector3(pivotX, pivotY)
		Dim offset As Single = If((Not Me.isHigh), (-loopSizeY), loopSizeY)
		MyBase.transform.SetPosition(New Single?(startPosX), New Single?(Me.pivot.y + offset), Nothing)
		If Me.onLeft Then
			While MyBase.transform.position.x < distToTurn
				MyBase.transform.position += Vector3.right * speed * CupheadTime.FixedDelta
				MyBase.HandleShadow(0F, 40F)
				Yield wait
			End While
		Else
			While MyBase.transform.position.x > -distToTurn
				MyBase.transform.position += Vector3.left * speed * CupheadTime.FixedDelta
				MyBase.HandleShadow(0F, 40F)
				Yield wait
			End While
		End If
		Dim angleToStopAt As Single = 3.1415927F
		Dim angle As Single = 0F
		Dim angleStartSpeed As Single = Me.properties.angleSpeedToLerp.min
		Dim angleEndSpeed As Single = Me.properties.angleSpeedToLerp.max
		Dim timeTolerp As Single = Me.properties.angleLerpTime
		Dim t As Single = 0F
		angle *= 0.017453292F
		While angle < angleToStopAt
			t += CupheadTime.FixedDelta
			Dim s As Single = Mathf.Lerp(angleStartSpeed, angleEndSpeed, t / timeTolerp)
			angle += s * CupheadTime.FixedDelta
			Dim handleRotationX As Vector3
			If Me.onLeft Then
				handleRotationX = New Vector3(Mathf.Sin(angle) * loopSizeX, 0F, 0F)
			Else
				handleRotationX = New Vector3(-Mathf.Sin(angle) * loopSizeX, 0F, 0F)
			End If
			Dim handleRotationY As Vector3
			If Me.isHigh Then
				handleRotationY = New Vector3(0F, Mathf.Cos(angle) * loopSizeY, 0F)
			Else
				handleRotationY = New Vector3(0F, -Mathf.Cos(angle) * loopSizeY, 0F)
			End If
			MyBase.transform.position = Me.pivot
			MyBase.transform.position += handleRotationX + handleRotationY
			MyBase.HandleShadow(0F, 40F)
			Yield New WaitForFixedUpdate()
		End While
		speed = boomerangSpeed
		If Me.onLeft Then
			While MyBase.transform.position.x > CSng(Level.Current.Left) - 300F
				speed += gravity * CupheadTime.FixedDelta
				MyBase.transform.position += Vector3.left * speed * CupheadTime.FixedDelta
				MyBase.HandleShadow(0F, 40F)
				Yield wait
			End While
		Else
			While MyBase.transform.position.x < CSng(Level.Current.Right) + 300F
				speed += gravity * CupheadTime.FixedDelta
				MyBase.transform.position += Vector3.right * speed * CupheadTime.FixedDelta
				MyBase.HandleShadow(0F, 40F)
				Yield wait
			End While
		End If
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		AudioManager.[Stop]("sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (Me.sfxID + 1))
		Return
	End Function

	' Token: 0x06002D61 RID: 11617 RVA: 0x001AC1AB File Offset: 0x001AA5AB
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Gizmos.DrawWireSphere(Me.pivot, 10F)
	End Sub

	' Token: 0x06002D62 RID: 11618 RVA: 0x001AC1C4 File Offset: 0x001AA5C4
	Private Sub SFX_SALTBAKER_P1_LimeProjectileLoop()
		Dim text As String = "sfx_dlc_saltbaker_p1_lime_projectile_loop_" + (Me.sfxID + 1)
		AudioManager.PlayLoop(text)
		Me.emitAudioFromObject.Add(text)
	End Sub

	' Token: 0x040035E6 RID: 13798
	Private properties As LevelProperties.Saltbaker.Limes

	' Token: 0x040035E7 RID: 13799
	Private isDead As Boolean

	' Token: 0x040035E8 RID: 13800
	Private onLeft As Boolean

	' Token: 0x040035E9 RID: 13801
	Private isHigh As Boolean

	' Token: 0x040035EA RID: 13802
	Private sfxID As Integer

	' Token: 0x040035EB RID: 13803
	Private pivot As Vector3
End Class
