Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000768 RID: 1896
Public Class RetroArcadeWorm
	Inherits RetroArcadeEnemy

	' Token: 0x0600294A RID: 10570 RVA: 0x00181309 File Offset: 0x0017F709
	Public Sub LevelInit(properties As LevelProperties.RetroArcade)
		Me.properties = properties
	End Sub

	' Token: 0x0600294B RID: 10571 RVA: 0x00181314 File Offset: 0x0017F714
	Public Sub StartWorm()
		MyBase.gameObject.SetActive(True)
		Me.p = Me.properties.CurrentState.worm
		Me.hp = Me.p.hp
		MyBase.PointsWorth = Me.p.pointsGained
		Me.platform.Rise()
		MyBase.transform.SetPosition(New Single?(0F), New Single?(-650F), Nothing)
		Me.direction = If((Not Rand.Bool()), RetroArcadeWorm.Direction.Right, RetroArcadeWorm.Direction.Left)
		Me.tongue.transform.parent = Nothing
		Me.tongue.Init(Me.p)
		Me.tongue.Extend()
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.rocket_cr())
	End Sub

	' Token: 0x0600294C RID: 10572 RVA: 0x001813F8 File Offset: 0x0017F7F8
	Private Iterator Function move_cr() As IEnumerator
		MyBase.MoveY(430F, 100F)
		While Me.movingY
			Yield New WaitForFixedUpdate()
		End While
		While True
			Dim normalizedHpRemaining As Single = Me.hp / Me.p.hp
			Dim speed As Single = Me.p.moveSpeed.min * Mathf.Pow(Me.p.moveSpeed.max / Me.p.moveSpeed.min, 1F - normalizedHpRemaining)
			MyBase.transform.AddPosition(CSng(If((Me.direction <> RetroArcadeWorm.Direction.Left), 1, (-1))) * speed * CupheadTime.FixedDelta, 0F, 0F)
			If(Me.direction = RetroArcadeWorm.Direction.Left AndAlso MyBase.transform.position.x < -160F) OrElse (Me.direction = RetroArcadeWorm.Direction.Right AndAlso MyBase.transform.position.x > 160F) Then
				Me.direction = If((Me.direction <> RetroArcadeWorm.Direction.Left), RetroArcadeWorm.Direction.Left, RetroArcadeWorm.Direction.Right)
			End If
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x0600294D RID: 10573 RVA: 0x00181414 File Offset: 0x0017F814
	Public Overrides Sub Dead()
		Me.StopAllCoroutines()
		For Each collider2D As Collider2D In MyBase.GetComponentsInChildren(Of Collider2D)()
			collider2D.enabled = False
		Next
		Me.properties.DealDamageToNextNamedState()
		Me.tongue.Retract()
		MyBase.StartCoroutine(Me.moveOffscreen_cr())
	End Sub

	' Token: 0x0600294E RID: 10574 RVA: 0x00181470 File Offset: 0x0017F870
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(-430F, 100F)
		While Me.movingY
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x0600294F RID: 10575 RVA: 0x0018148C File Offset: 0x0017F88C
	Private Iterator Function rocket_cr() As IEnumerator
		Dim rocketDirection As RetroArcadeWormRocket.Direction = If((Not Rand.Bool()), RetroArcadeWormRocket.Direction.Right, RetroArcadeWormRocket.Direction.Left)
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.p.rocketSpawnDelay)
			Me.rocketPrefab.Create(rocketDirection, Me.p)
		End While
		Return
	End Function

	' Token: 0x04003246 RID: 12870
	Private Const OFFSCREEN_Y As Single = -650F

	' Token: 0x04003247 RID: 12871
	Private Const ONSCREEN_Y As Single = -220F

	' Token: 0x04003248 RID: 12872
	Private Const MOVE_Y_SPEED As Single = 100F

	' Token: 0x04003249 RID: 12873
	Private Const TURNAROUND_X As Single = 160F

	' Token: 0x0400324A RID: 12874
	Private properties As LevelProperties.RetroArcade

	' Token: 0x0400324B RID: 12875
	Private p As LevelProperties.RetroArcade.Worm

	' Token: 0x0400324C RID: 12876
	<SerializeField()>
	Private platform As RetroArcadeWormPlatform

	' Token: 0x0400324D RID: 12877
	<SerializeField()>
	Private tongue As RetroArcadeWormTongue

	' Token: 0x0400324E RID: 12878
	<SerializeField()>
	Private rocketPrefab As RetroArcadeWormRocket

	' Token: 0x0400324F RID: 12879
	Private direction As RetroArcadeWorm.Direction

	' Token: 0x02000769 RID: 1897
	Private Enum Direction
		' Token: 0x04003251 RID: 12881
		Left
		' Token: 0x04003252 RID: 12882
		Right
	End Enum
End Class
