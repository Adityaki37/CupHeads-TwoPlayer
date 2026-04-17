Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000590 RID: 1424
Public Class DevilLevelPitchforkOrbitingProjectile
	Inherits AbstractProjectile

	' Token: 0x17000354 RID: 852
	' (get) Token: 0x06001B39 RID: 6969 RVA: 0x000F9E35 File Offset: 0x000F8235
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return -1F
		End Get
	End Property

	' Token: 0x06001B3A RID: 6970 RVA: 0x000F9E3C File Offset: 0x000F823C
	Public Function Create(target As AbstractProjectile, angle As Single, rotationSpeed As Single, radius As Single, parent As DevilLevelSittingDevil, waitTime As Single) As DevilLevelPitchforkOrbitingProjectile
		Dim devilLevelPitchforkOrbitingProjectile As DevilLevelPitchforkOrbitingProjectile = Me.InstantiatePrefab(Of DevilLevelPitchforkOrbitingProjectile)()
		devilLevelPitchforkOrbitingProjectile.target = target
		devilLevelPitchforkOrbitingProjectile.angle = angle
		devilLevelPitchforkOrbitingProjectile.rotationSpeed = rotationSpeed
		devilLevelPitchforkOrbitingProjectile.radius = radius
		devilLevelPitchforkOrbitingProjectile.parent = parent
		devilLevelPitchforkOrbitingProjectile.waitTime = waitTime
		devilLevelPitchforkOrbitingProjectile.waitTimeUp = False
		Return devilLevelPitchforkOrbitingProjectile
	End Function

	' Token: 0x06001B3B RID: 6971 RVA: 0x000F9E88 File Offset: 0x000F8288
	Public Function Create(target As AbstractProjectile, angle As Single, rotationSpeed As Single, radius As Single, parent As DevilLevelSittingDevil) As DevilLevelPitchforkOrbitingProjectile
		Dim devilLevelPitchforkOrbitingProjectile As DevilLevelPitchforkOrbitingProjectile = Me.InstantiatePrefab(Of DevilLevelPitchforkOrbitingProjectile)()
		devilLevelPitchforkOrbitingProjectile.target = target
		devilLevelPitchforkOrbitingProjectile.angle = angle
		devilLevelPitchforkOrbitingProjectile.rotationSpeed = rotationSpeed
		devilLevelPitchforkOrbitingProjectile.radius = radius
		devilLevelPitchforkOrbitingProjectile.parent = parent
		devilLevelPitchforkOrbitingProjectile.waitTimeUp = True
		Return devilLevelPitchforkOrbitingProjectile
	End Function

	' Token: 0x06001B3C RID: 6972 RVA: 0x000F9EC9 File Offset: 0x000F82C9
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.parent Is Nothing Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001B3D RID: 6973 RVA: 0x000F9EE8 File Offset: 0x000F82E8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001B3E RID: 6974 RVA: 0x000F9F08 File Offset: 0x000F8308
	Private Iterator Function wait_time_cr() As IEnumerator
		Yield New WaitForSeconds(Me.waitTime)
		Me.waitTimeUp = True
		MyBase.GetComponent(Of Collider2D)().enabled = True
		MyBase.animator.SetTrigger("Continue")
		MyBase.animator.SetBool("StartAtHalf", Rand.Bool())
		Return
	End Function

	' Token: 0x06001B3F RID: 6975 RVA: 0x000F9F24 File Offset: 0x000F8324
	Protected Overrides Sub Start()
		MyBase.Start()
		Dim vector As Vector2 = Me.target.transform.position + MathUtils.AngleToDirection(Me.angle) * Me.radius
		MyBase.transform.SetPosition(New Single?(vector.x), New Single?(vector.y), Nothing)
		If Not Me.waitTimeUp Then
			MyBase.GetComponent(Of Collider2D)().enabled = False
			MyBase.StartCoroutine(Me.wait_time_cr())
		End If
	End Sub

	' Token: 0x06001B40 RID: 6976 RVA: 0x000F9FB8 File Offset: 0x000F83B8
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not Me.waitTimeUp Then
			Return
		End If
		If MyBase.dead Then
			Return
		End If
		If Me.target Is Nothing OrElse Me.target.dead Then
			Me.Die()
			Return
		End If
		Me.angle += Me.rotationSpeed * CupheadTime.FixedDelta
		Dim vector As Vector2 = Me.target.transform.position + MathUtils.AngleToDirection(Me.angle) * Me.radius
		MyBase.transform.SetPosition(New Single?(vector.x), New Single?(vector.y), Nothing)
	End Sub

	' Token: 0x06001B41 RID: 6977 RVA: 0x000FA081 File Offset: 0x000F8481
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Me.OrbitStopSFX()
	End Sub

	' Token: 0x06001B42 RID: 6978 RVA: 0x000FA09A File Offset: 0x000F849A
	Private Sub OrbitStartSFX()
		AudioManager.PlayLoop("devil_orbit_projectile")
		Me.emitAudioFromObject.Add("devil_orbit_projectile")
	End Sub

	' Token: 0x06001B43 RID: 6979 RVA: 0x000FA0B6 File Offset: 0x000F84B6
	Private Sub OrbitStopSFX()
		AudioManager.[Stop]("devil_orbit_projectile")
	End Sub

	' Token: 0x0400247A RID: 9338
	Private target As AbstractProjectile

	' Token: 0x0400247B RID: 9339
	Private rotationSpeed As Single

	' Token: 0x0400247C RID: 9340
	Private radius As Single

	' Token: 0x0400247D RID: 9341
	Private angle As Single

	' Token: 0x0400247E RID: 9342
	Private waitTime As Single

	' Token: 0x0400247F RID: 9343
	Private parent As DevilLevelSittingDevil

	' Token: 0x04002480 RID: 9344
	Private waitTimeUp As Boolean
End Class
