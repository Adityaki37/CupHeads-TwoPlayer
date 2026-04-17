Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000763 RID: 1891
Public Class RetroArcadeUFO
	Inherits RetroArcadeEnemy

	' Token: 0x06002935 RID: 10549 RVA: 0x001804E6 File Offset: 0x0017E8E6
	Public Sub LevelInit(properties As LevelProperties.RetroArcade)
		Me.properties = properties
	End Sub

	' Token: 0x06002936 RID: 10550 RVA: 0x001804F0 File Offset: 0x0017E8F0
	Public Sub StartUFO()
		MyBase.gameObject.SetActive(True)
		Me.p = Me.properties.CurrentState.uFO
		MyBase.transform.SetPosition(New Single?(0F), New Single?(500F), Nothing)
		MyBase.MoveY(-200F, 500F)
		Me.alien = Me.alienPrefab.Create(Me, Me.p)
		Me.mole = Me.molePrefab.Create(Me.p)
		Me.turrets = New List(Of RetroArcadeUFOTurret)()
		For i As Integer = 0 To Me.p.turretCount - 1
			Dim retroArcadeUFOTurret As RetroArcadeUFOTurret = Me.turretPrefab.Create(Me, Me.p, CSng(i) / CSng(Me.p.turretCount))
			Me.turrets.Add(retroArcadeUFOTurret)
		Next
		MyBase.StartCoroutine(Me.shoot_cr())
	End Sub

	' Token: 0x06002937 RID: 10551 RVA: 0x001805E8 File Offset: 0x0017E9E8
	Private Iterator Function shoot_cr() As IEnumerator
		While True
			Dim waitTime As Single = Me.p.shotRate.min * Mathf.Pow(Me.p.shotRate.max / Me.p.shotRate.min, 1F - Me.alien.NormalizedHpRemaining)
			Yield CupheadTime.WaitForSeconds(Me, waitTime)
			For Each retroArcadeUFOTurret As RetroArcadeUFOTurret In Me.turrets
				retroArcadeUFOTurret.Shoot()
			Next
		End While
		Return
	End Function

	' Token: 0x06002938 RID: 10552 RVA: 0x00180604 File Offset: 0x0017EA04
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(200F, 500F)
		While Me.movingY
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06002939 RID: 10553 RVA: 0x0018061F File Offset: 0x0017EA1F
	Public Sub OnAlienDie()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.moveOffscreen_cr())
		Me.properties.DealDamageToNextNamedState()
		Me.mole.OnWaveEnd()
	End Sub

	' Token: 0x04003221 RID: 12833
	Private Const OFFSCREEN_Y As Single = 500F

	' Token: 0x04003222 RID: 12834
	Private Const ONSCREEN_Y As Single = 300F

	' Token: 0x04003223 RID: 12835
	Private Const MOVE_Y_SPEED As Single = 500F

	' Token: 0x04003224 RID: 12836
	Public Const WIDTH As Single = 600F

	' Token: 0x04003225 RID: 12837
	Public Const HEIGHT As Single = 300F

	' Token: 0x04003226 RID: 12838
	Public Const INNER_WIDTH As Single = 500F

	' Token: 0x04003227 RID: 12839
	Public Const INNER_HEIGHT As Single = 150F

	' Token: 0x04003228 RID: 12840
	Public Const INNER_TURNAROUND_X As Single = 220F

	' Token: 0x04003229 RID: 12841
	Private properties As LevelProperties.RetroArcade

	' Token: 0x0400322A RID: 12842
	Private p As LevelProperties.RetroArcade.UFO

	' Token: 0x0400322B RID: 12843
	<SerializeField()>
	Private turretPrefab As RetroArcadeUFOTurret

	' Token: 0x0400322C RID: 12844
	<SerializeField()>
	Private alienPrefab As RetroArcadeUFOAlien

	' Token: 0x0400322D RID: 12845
	<SerializeField()>
	Private molePrefab As RetroArcadeUFOMole

	' Token: 0x0400322E RID: 12846
	Private alien As RetroArcadeUFOAlien

	' Token: 0x0400322F RID: 12847
	Private turrets As List(Of RetroArcadeUFOTurret)

	' Token: 0x04003230 RID: 12848
	Private mole As RetroArcadeUFOMole
End Class
