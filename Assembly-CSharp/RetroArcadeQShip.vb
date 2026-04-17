Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000748 RID: 1864
Public Class RetroArcadeQShip
	Inherits RetroArcadeEnemy

	' Token: 0x170003DB RID: 987
	' (get) Token: 0x0600289E RID: 10398 RVA: 0x0017B232 File Offset: 0x00179632
	' (set) Token: 0x0600289F RID: 10399 RVA: 0x0017B23A File Offset: 0x0017963A
	Public Property TileRotationSpeed As Single

	' Token: 0x060028A0 RID: 10400 RVA: 0x0017B243 File Offset: 0x00179643
	Public Sub LevelInit(properties As LevelProperties.RetroArcade)
		Me.properties = properties
	End Sub

	' Token: 0x060028A1 RID: 10401 RVA: 0x0017B24C File Offset: 0x0017964C
	Public Sub StartQShip()
		MyBase.gameObject.SetActive(True)
		Me.p = Me.properties.CurrentState.qShip
		Me.hp = Me.p.hp
		Me.TileRotationSpeed = Me.p.tileRotationSpeed.min
		MyBase.PointsBonus = Me.p.pointsGained
		MyBase.PointsWorth = Me.p.pointsBonus
		Me.tiles = New List(Of RetroArcadeQShipOrbitingTile)()
		For i As Integer = 0 To Me.p.numSpinningTiles - 1
			Dim retroArcadeQShipOrbitingTile As RetroArcadeQShipOrbitingTile = Me.tilePrefab.Create(Me, 360F * CSng(i) / CSng(Me.p.numSpinningTiles), Me.p)
			Me.tiles.Add(retroArcadeQShipOrbitingTile)
		Next
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.tentacle_cr())
	End Sub

	' Token: 0x060028A2 RID: 10402 RVA: 0x0017B33C File Offset: 0x0017973C
	Protected Overrides Sub FixedUpdate()
		Me.TileRotationSpeed = Me.p.tileRotationSpeed.min * Mathf.Pow(Me.p.tileRotationSpeed.max / Me.p.tileRotationSpeed.min, 1F - Me.hp / Me.p.hp)
	End Sub

	' Token: 0x060028A3 RID: 10403 RVA: 0x0017B3A0 File Offset: 0x001797A0
	Private Iterator Function move_cr() As IEnumerator
		MyBase.transform.SetPosition(New Single?(0F), New Single?(350F + Me.p.tileRotationDistance), Nothing)
		MyBase.MoveY(Me.p.yPos - (350F + Me.p.tileRotationDistance), 500F)
		While Me.movingY
			Yield New WaitForFixedUpdate()
		End While
		Dim t As Single = 0F
		Dim moveTime As Single = Me.p.maxXPos * 2F / Me.p.moveSpeed
		While True
			t += CupheadTime.FixedDelta
			MyBase.transform.SetPosition(New Single?(Mathf.Sin(t * 3.1415927F / moveTime) * Me.p.maxXPos), Nothing, Nothing)
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x060028A4 RID: 10404 RVA: 0x0017B3BC File Offset: 0x001797BC
	Public Overrides Sub Dead()
		Me.StopAllCoroutines()
		For Each collider2D As Collider2D In MyBase.GetComponentsInChildren(Of Collider2D)()
			collider2D.enabled = False
		Next
		MyBase.IsDead = True
		For Each spriteRenderer As SpriteRenderer In MyBase.GetComponentsInChildren(Of SpriteRenderer)()
			spriteRenderer.color = New Color(0F, 0F, 0F, 0.25F)
		Next
		Me.properties.DealDamageToNextNamedState()
		MyBase.StartCoroutine(Me.moveOffscreen_cr())
	End Sub

	' Token: 0x060028A5 RID: 10405 RVA: 0x0017B45C File Offset: 0x0017985C
	Private Iterator Function moveOffscreen_cr() As IEnumerator
		MyBase.MoveY(350F + Me.p.tileRotationDistance - MyBase.transform.position.y, 500F)
		While Me.movingY
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x060028A6 RID: 10406 RVA: 0x0017B478 File Offset: 0x00179878
	Public Sub ShootProjectile()
		Me.projectilePrefab.Create(Me.projectileRoot.position, -90F - Me.p.shotSpreadAngle, Me.p.shotSpeed)
		Me.projectilePrefab.Create(Me.projectileRoot.position, -90F, Me.p.shotSpeed)
		Me.projectilePrefab.Create(Me.projectileRoot.position, -90F + Me.p.shotSpreadAngle, Me.p.shotSpeed)
	End Sub

	' Token: 0x060028A7 RID: 10407 RVA: 0x0017B524 File Offset: 0x00179924
	Private Iterator Function tentacle_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, Me.p.tentacleSpawnRange.RandomFloat())
			Dim left As Boolean = Rand.Bool()
			MyBase.animator.SetBool(If((Not left), "RightTentacle", "LeftTentacle"), True)
			Yield CupheadTime.WaitForSeconds(Me, Me.p.tentacleWarningDuration)
			MyBase.animator.SetBool(If((Not left), "RightTentacle", "LeftTentacle"), False)
			Me.tentaclePrefab.Create(If((Not left), RetroArcadeQShipTentacle.Direction.Left, RetroArcadeQShipTentacle.Direction.Right), Me.p)
		End While
		Return
	End Function

	' Token: 0x04003176 RID: 12662
	Private Const OFFSCREEN_Y As Single = 350F

	' Token: 0x04003177 RID: 12663
	Private Const MOVE_Y_SPEED As Single = 500F

	' Token: 0x04003178 RID: 12664
	Private properties As LevelProperties.RetroArcade

	' Token: 0x04003179 RID: 12665
	Private p As LevelProperties.RetroArcade.QShip

	' Token: 0x0400317A RID: 12666
	<SerializeField()>
	Private tilePrefab As RetroArcadeQShipOrbitingTile

	' Token: 0x0400317B RID: 12667
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x0400317C RID: 12668
	<SerializeField()>
	Private projectileRoot As Transform

	' Token: 0x0400317D RID: 12669
	<SerializeField()>
	Private tentaclePrefab As RetroArcadeQShipTentacle

	' Token: 0x0400317E RID: 12670
	Private tiles As List(Of RetroArcadeQShipOrbitingTile)
End Class
