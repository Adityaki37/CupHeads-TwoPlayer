Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A77 RID: 2679
Public Class WeaponExploderProjectile
	Inherits BasicProjectile

	' Token: 0x06004003 RID: 16387 RVA: 0x0022EC95 File Offset: 0x0022D095
	Protected Overrides Sub Awake()
		MyBase.Awake()
	End Sub

	' Token: 0x06004004 RID: 16388 RVA: 0x0022EC9D File Offset: 0x0022D09D
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Not Me.isEx Then
			Me.UpdateDamageState()
		End If
	End Sub

	' Token: 0x06004005 RID: 16389 RVA: 0x0022ECB8 File Offset: 0x0022D0B8
	Private Sub UpdateDamageState()
		If MyBase.lifetime < WeaponProperties.LevelWeaponExploder.Basic.timeStateTwo Then
			Me.Damage = WeaponProperties.LevelWeaponExploder.Basic.baseDamage
			MyBase.transform.SetScale(New Single?(1F), New Single?(1F), Nothing)
			Me.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.baseExplosionRadius
		ElseIf MyBase.lifetime < WeaponProperties.LevelWeaponExploder.Basic.timeStateThree Then
			Me.Damage = WeaponProperties.LevelWeaponExploder.Basic.damageStateTwo
			MyBase.transform.SetScale(New Single?(1.5F), New Single?(1.5F), Nothing)
			Me.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.explosionRadiusStateTwo
		Else
			Me.Damage = WeaponProperties.LevelWeaponExploder.Basic.damageStateThree
			MyBase.transform.SetScale(New Single?(2.5F), New Single?(2.5F), Nothing)
			Me.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.explosionRadiusStateThree
		End If
	End Sub

	' Token: 0x06004006 RID: 16390 RVA: 0x0022EDAC File Offset: 0x0022D1AC
	Protected Overrides Sub Die()
		MyBase.Die()
		Me.explosionPrefab.Create(MyBase.transform.position, Me.explodeRadius, Me.Damage, MyBase.DamageMultiplier, Me.weapon, Me.tracker)
		If Me.shrapnelPrefab IsNot Nothing Then
			Dim basicProjectile As BasicProjectile = Me.shrapnelPrefab.Create(MyBase.transform.position, MyBase.transform.eulerAngles.z + 180F, WeaponProperties.LevelWeaponExploder.Ex.shrapnelSpeed)
			If Not WeaponProperties.LevelWeaponExploder.Ex.damageOn Then
				basicProjectile.DamagesType.Player = False
			End If
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06004007 RID: 16391 RVA: 0x0022EE64 File Offset: 0x0022D264
	Public Overrides Sub AddToMeterScoreTracker(tracker As MeterScoreTracker)
		MyBase.AddToMeterScoreTracker(tracker)
		Me.tracker = tracker
	End Sub

	' Token: 0x06004008 RID: 16392 RVA: 0x0022EE74 File Offset: 0x0022D274
	Public Sub EaseSpeed()
		MyBase.StartCoroutine(Me.ease_speed_cr())
	End Sub

	' Token: 0x06004009 RID: 16393 RVA: 0x0022EE84 File Offset: 0x0022D284
	Private Iterator Function ease_speed_cr() As IEnumerator
		Dim t As Single = 0F
		Dim time As Single = Me.easeTime
		While t < time
			t += CupheadTime.Delta
			Me.Speed = Me.minMaxSpeed.GetFloatAt(t / time)
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x040046CE RID: 18126
	<SerializeField()>
	Private explosionPrefab As WeaponExploderProjectileExplosion

	' Token: 0x040046CF RID: 18127
	<SerializeField()>
	Private shrapnelPrefab As BasicProjectile

	' Token: 0x040046D0 RID: 18128
	<SerializeField()>
	Private isEx As Boolean

	' Token: 0x040046D1 RID: 18129
	Public explodeRadius As Single

	' Token: 0x040046D2 RID: 18130
	Public easeTime As Single

	' Token: 0x040046D3 RID: 18131
	Public minMaxSpeed As MinMax

	' Token: 0x040046D4 RID: 18132
	Public weapon As WeaponExploder

	' Token: 0x040046D5 RID: 18133
	Private tracker As MeterScoreTracker
End Class
