Imports System
Imports UnityEngine

' Token: 0x02000A84 RID: 2692
Public Class WeaponSplitterProjectile
	Inherits BasicProjectile

	' Token: 0x0600405A RID: 16474 RVA: 0x00231212 File Offset: 0x0022F612
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.splitDamage > -1F Then
			Me.damageDealer.SetDamage(Me.splitDamage)
		End If
	End Sub

	' Token: 0x0600405B RID: 16475 RVA: 0x0023123B File Offset: 0x0022F63B
	Protected Overrides Sub OnDieDistance()
		If MyBase.dead Then
			Return
		End If
		Me.Die()
		MyBase.animator.SetTrigger("OnDistanceDie")
	End Sub

	' Token: 0x0600405C RID: 16476 RVA: 0x0023125F File Offset: 0x0022F65F
	Protected Overrides Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600405D RID: 16477 RVA: 0x0023126C File Offset: 0x0022F66C
	Private Sub _OnDieAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600405E RID: 16478 RVA: 0x0023127C File Offset: 0x0022F67C
	Private Sub Split()
		Me.baseAngle = MyBase.transform.eulerAngles.z
		If Me.isMain Then
			Me.damageDealer.SetDamage(If((Me.nextDistance <> WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB), WeaponProperties.LevelWeaponSplitter.Basic.bulletDamageA, WeaponProperties.LevelWeaponSplitter.Basic.bulletDamageB))
			Dim weaponSplitterProjectile As WeaponSplitterProjectile = Global.UnityEngine.[Object].Instantiate(Of WeaponSplitterProjectile)(Me, MyBase.transform.position, Quaternion.identity)
			weaponSplitterProjectile.isMain = False
			weaponSplitterProjectile.splitAngle = -WeaponProperties.LevelWeaponSplitter.Basic.splitAngle
			weaponSplitterProjectile.transform.eulerAngles = New Vector3(0F, 0F, Me.baseAngle + Me.splitAngle)
			weaponSplitterProjectile.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance
			weaponSplitterProjectile.dist = Me.dist
			weaponSplitterProjectile.splitDamage = Me.Damage
			weaponSplitterProjectile = Global.UnityEngine.[Object].Instantiate(Of WeaponSplitterProjectile)(Me, MyBase.transform.position, Quaternion.identity)
			weaponSplitterProjectile.isMain = False
			weaponSplitterProjectile.splitAngle = WeaponProperties.LevelWeaponSplitter.Basic.splitAngle
			weaponSplitterProjectile.transform.eulerAngles = New Vector3(0F, 0F, Me.baseAngle + Me.splitAngle)
			weaponSplitterProjectile.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance
			weaponSplitterProjectile.dist = Me.dist
			weaponSplitterProjectile.splitDamage = Me.Damage
		Else
			MyBase.transform.eulerAngles = New Vector3(0F, 0F, Me.baseAngle + Me.splitAngle)
			Me.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance
		End If
		If Me.nextDistance = WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB Then
			Me.nextDistance = Single.MaxValue
		Else
			Me.nextDistance = WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB
		End If
	End Sub

	' Token: 0x0600405F RID: 16479 RVA: 0x00231420 File Offset: 0x0022F820
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		Me.dist += Me.Speed * CupheadTime.FixedDelta
		If Me.dist > Me.nextDistance Then
			Me.Split()
		End If
		If Me.distancePastSplit > 0F Then
			Me.distancePastSplit -= Me.Speed * CupheadTime.FixedDelta
			If Me.distancePastSplit <= 0F Then
				MyBase.transform.eulerAngles = New Vector3(0F, 0F, Me.baseAngle)
			End If
		End If
	End Sub

	' Token: 0x0400472D RID: 18221
	Public isMain As Boolean

	' Token: 0x0400472E RID: 18222
	Public nextDistance As Single

	' Token: 0x0400472F RID: 18223
	Public baseAngle As Single

	' Token: 0x04004730 RID: 18224
	Private distancePastSplit As Single

	' Token: 0x04004731 RID: 18225
	Private splitAngle As Single

	' Token: 0x04004732 RID: 18226
	Private dist As Single

	' Token: 0x04004733 RID: 18227
	Private splitDamage As Single = -1F
End Class
