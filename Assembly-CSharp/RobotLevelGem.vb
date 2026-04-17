Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200077B RID: 1915
Public Class RobotLevelGem
	Inherits AbstractCollidableObject

	' Token: 0x060029F4 RID: 10740 RVA: 0x001888D0 File Offset: 0x00186CD0
	Public Sub InitFinalStage(parent As RobotLevelHelihead, properties As LevelProperties.Robot, isBlueGem As Boolean)
		Me.parent = parent
		Me.properties = properties
		Dim robotLevelHelihead As RobotLevelHelihead = Me.parent
		robotLevelHelihead.OnDeath = CType([Delegate].Combine(robotLevelHelihead.OnDeath, AddressOf Me.OnDeath), Action)
		Me.rotation = 0F
		If Me.isFirstWave Then
			Me.bulletPrefab.CreatePool(200)
		End If
		Me.isBlueGem = isBlueGem
		If isBlueGem Then
			Me.waveRotation = properties.CurrentState.blueGem.gemWaveRotation
		Else
			Me.waveRotation = properties.CurrentState.redGem.gemWaveRotation
		End If
		If Me.isBlueGem Then
			Me.pinkPattern = Me.properties.CurrentState.blueGem.pinkString.Split(New Char() { ","c })
		Else
			Me.pinkPattern = Me.properties.CurrentState.redGem.pinkString.Split(New Char() { ","c })
		End If
		MyBase.StartCoroutine(Me.rotate_cr())
		MyBase.StartCoroutine(Me.attack_cr())
	End Sub

	' Token: 0x060029F5 RID: 10741 RVA: 0x001889F8 File Offset: 0x00186DF8
	Private Iterator Function attack_cr() As IEnumerator
		While True
			If Me.isBlueGem Then
				Me.FireBullets(Me.properties.CurrentState.blueGem.numberOfSpawnPoints, 0F)
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.blueGem.bulletSpawnDelay)
			Else
				Me.FireBullets(Me.properties.CurrentState.redGem.numberOfSpawnPoints, 0F)
				Yield CupheadTime.WaitForSeconds(Me, Me.properties.CurrentState.redGem.bulletSpawnDelay)
			End If
		End While
		Return
	End Function

	' Token: 0x060029F6 RID: 10742 RVA: 0x00188A14 File Offset: 0x00186E14
	Private Iterator Function rotate_cr() As IEnumerator
		Dim rotationSpeed As Single
		Dim rotationRange As MinMax
		If Me.isBlueGem Then
			rotationSpeed = Me.properties.CurrentState.blueGem.robotRotationSpeed
			rotationRange = Me.properties.CurrentState.blueGem.gemRotationRange
		Else
			rotationSpeed = Me.properties.CurrentState.redGem.robotRotationSpeed
			rotationRange = Me.properties.CurrentState.redGem.gemRotationRange
		End If
		While True
			If Me.waveRotation AndAlso (Vector3.Angle(Vector3.up, MyBase.transform.right) > rotationRange.max OrElse Vector3.Angle(Vector3.up, MyBase.transform.right) < rotationRange.min) Then
				rotationSpeed *= -1F
			End If
			Me.rotation += rotationSpeed
			MyBase.transform.eulerAngles = Vector3.forward * Me.rotation
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060029F7 RID: 10743 RVA: 0x00188A2F File Offset: 0x00186E2F
	Public Sub OnAttackEnd()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x060029F8 RID: 10744 RVA: 0x00188A37 File Offset: 0x00186E37
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bulletPrefab = Nothing
	End Sub

	' Token: 0x060029F9 RID: 10745 RVA: 0x00188A46 File Offset: 0x00186E46
	Private Sub OnDeath()
		Dim robotLevelHelihead As RobotLevelHelihead = Me.parent
		robotLevelHelihead.OnDeath = CType([Delegate].Remove(robotLevelHelihead.OnDeath, AddressOf Me.OnDeath), Action)
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x060029FA RID: 10746 RVA: 0x00188A78 File Offset: 0x00186E78
	Private Sub FireBullets(count As Integer, Optional offset As Single = 0F)
		offset = Me.rotation
		For i As Integer = 0 To count - 1
			Dim num As Single = 360F * (CSng(i) / CSng(count))
			If Me.isBlueGem Then
				Me.bulletPrefab.Spawn(MyBase.transform.position, Quaternion.Euler(New Vector3(0F, 0F, offset + num - 180F))).Init(Me.properties.CurrentState.blueGem.bulletSpeed, CSng(Me.properties.CurrentState.blueGem.bulletSpeedAcceleration), CSng(Me.properties.CurrentState.blueGem.bulletSineWaveStrength), Me.properties.CurrentState.blueGem.bulletWaveSpeedMultiplier, Me.properties.CurrentState.blueGem.bulletLifeTime, Me.isBlueGem, Me.pinkPattern(Me.pinkIndex)(0) = "P"c)
			Else
				Me.bulletPrefab.Spawn(MyBase.transform.position, Quaternion.Euler(New Vector3(0F, 0F, offset + num - 180F))).Init(Me.properties.CurrentState.redGem.bulletSpeed, CSng(Me.properties.CurrentState.redGem.bulletSpeedAcceleration), CSng(Me.properties.CurrentState.redGem.bulletSineWaveStrength), Me.properties.CurrentState.redGem.bulletWaveSpeedMultiplier, Me.properties.CurrentState.redGem.bulletLifeTime, Me.isBlueGem, Me.pinkPattern(Me.pinkIndex)(0) = "P"c)
			End If
			Me.pinkIndex = (Me.pinkIndex + 1) Mod Me.pinkPattern.Length
		Next
	End Sub

	' Token: 0x040032D4 RID: 13012
	<SerializeField()>
	Private bulletPrefab As RobotLevelGemProjectile

	' Token: 0x040032D5 RID: 13013
	Private parent As RobotLevelHelihead

	' Token: 0x040032D6 RID: 13014
	Private properties As LevelProperties.Robot

	' Token: 0x040032D7 RID: 13015
	Private isFirstWave As Boolean = True

	' Token: 0x040032D8 RID: 13016
	Private waveRotation As Boolean

	' Token: 0x040032D9 RID: 13017
	Private nextBulletIndex As Integer

	' Token: 0x040032DA RID: 13018
	Private rotation As Single

	' Token: 0x040032DB RID: 13019
	Private isBlueGem As Boolean

	' Token: 0x040032DC RID: 13020
	Private pinkPattern As String()

	' Token: 0x040032DD RID: 13021
	Private pinkIndex As Integer
End Class
