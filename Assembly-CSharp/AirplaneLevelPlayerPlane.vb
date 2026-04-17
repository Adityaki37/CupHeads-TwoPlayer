Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004C0 RID: 1216
Public Class AirplaneLevelPlayerPlane
	Inherits LevelProperties.Airplane.Entity

	' Token: 0x0600143D RID: 5181 RVA: 0x000B475C File Offset: 0x000B2B5C
	Public Overrides Sub LevelInit(properties As LevelProperties.Airplane)
		MyBase.LevelInit(properties)
		Me.tiltableBasePos = Me.tiltable.localPosition
		Me.maxParallaxX = CupheadLevelCamera.Current.Bounds.xMax - properties.CurrentState.plane.endScreenOffset
		Me.rotationDist = Vector3.Distance(Me.edgeLeft.position, Me.edgeRight.position)
		Me.rotationVal = Me.rotationDist / 2F
		MyBase.StartCoroutine(Me.handle_player_move_cr())
		MyBase.StartCoroutine(Me.handle_tilt_cr())
		Me.puffTimer(0) = 1F
		Me.puffTimer(1) = 0.8F
		Me.SFX_DOGFIGHT_PlayerPlane_Loop()
		Me.SFX_DOGFIGHT_PlayerPlane_HighSpeed_Loop()
	End Sub

	' Token: 0x0600143E RID: 5182 RVA: 0x000B4820 File Offset: 0x000B2C20
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		If Me.player1 IsNot Nothing Then
			Dim component As LevelPlayerWeaponManager = Me.player1.gameObject.GetComponent(Of LevelPlayerWeaponManager)()
			RemoveHandler component.OnSuperStart, AddressOf Me.StartP1Super
			RemoveHandler component.OnSuperEnd, AddressOf Me.EndP1Super
			RemoveHandler component.OnExStart, AddressOf Me.StartP1Super
			RemoveHandler component.OnExEnd, AddressOf Me.EndP1Super
		End If
		If Me.player2 IsNot Nothing Then
			Dim component2 As LevelPlayerWeaponManager = Me.player2.gameObject.GetComponent(Of LevelPlayerWeaponManager)()
			RemoveHandler component2.OnSuperStart, AddressOf Me.StartP2Super
			RemoveHandler component2.OnSuperEnd, AddressOf Me.EndP2Super
			RemoveHandler component2.OnExStart, AddressOf Me.StartP2Super
			RemoveHandler component2.OnExEnd, AddressOf Me.EndP2Super
		End If
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x0600143F RID: 5183 RVA: 0x000B4910 File Offset: 0x000B2D10
	Private Sub Update()
		If CType(Level.Current, AirplaneLevel).Rotating Then
			If Me.playerInSuper(0) Then
				Me.restorePlayerPos(0) = True
			End If
			If Me.playerInSuper(1) Then
				Me.restorePlayerPos(1) = True
			End If
		End If
		If Me.player1 Is Nothing Then
			Me.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne)
			If Me.player1 IsNot Nothing Then
				Dim component As LevelPlayerWeaponManager = Me.player1.gameObject.GetComponent(Of LevelPlayerWeaponManager)()
				AddHandler component.OnSuperStart, AddressOf Me.StartP1Super
				AddHandler component.OnSuperEnd, AddressOf Me.EndP1Super
				AddHandler component.OnExStart, AddressOf Me.StartP1Super
				AddHandler component.OnExEnd, AddressOf Me.EndP1Super
			End If
		End If
		If Me.player2 Is Nothing Then
			Me.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo)
			If Me.player2 IsNot Nothing Then
				Dim component2 As LevelPlayerWeaponManager = Me.player2.gameObject.GetComponent(Of LevelPlayerWeaponManager)()
				AddHandler component2.OnSuperStart, AddressOf Me.StartP2Super
				AddHandler component2.OnSuperEnd, AddressOf Me.EndP2Super
				AddHandler component2.OnExStart, AddressOf Me.StartP2Super
				AddHandler component2.OnExEnd, AddressOf Me.EndP2Super
			End If
		End If
		If Me.player1 IsNot Nothing Then
			Me.p1IsColliding = Me.player1.transform.parent Is Me.airplane1.transform
			Me.player1.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		Else
			Me.p1IsColliding = False
		End If
		If Me.player2 IsNot Nothing Then
			Me.p2IsColliding = Me.player2.transform.parent Is Me.airplane1.transform
			Me.player2.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
		Else
			Me.p2IsColliding = False
		End If
		Me.autoTiltTime = Mathf.Clamp(Me.autoTiltTime + CupheadTime.Delta * If((Not Me.autoX OrElse Not Me.autoTilt), (-1F), 3F), 0F, 1F)
		For i As Integer = 0 To Me.puffTimer.Length - 1
			Me.puffTimer(i) -= CupheadTime.Delta
			If Me.puffTimer(i) <= 0F Then
				Me.puffTimer(i) += If((i <> 0), 0.8F, 1F)
				Dim effect As Effect = Me.planePuffFX.Create(Me.planePuffPos(i).position)
				effect.transform.SetEulerAngles(Nothing, Nothing, New Single?(CSng(If((i <> 0), 30, (-30)))))
			End If
		Next
	End Sub

	' Token: 0x06001440 RID: 5184 RVA: 0x000B4C4C File Offset: 0x000B304C
	Private Sub StartP1Super()
		Me.playerInSuper(0) = True
		Me.playerRelativePosAtSuperStart(0) = Me.player1.transform.position.y - MyBase.transform.position.y
	End Sub

	' Token: 0x06001441 RID: 5185 RVA: 0x000B4C98 File Offset: 0x000B3098
	Private Sub EndP1Super()
		Me.playerInSuper(0) = False
		If Me.restorePlayerPos(0) Then
			Me.player1.transform.position = New Vector3(Me.player1.transform.position.x, MyBase.transform.position.y + Me.playerRelativePosAtSuperStart(0))
		End If
		Me.restorePlayerPos(0) = False
	End Sub

	' Token: 0x06001442 RID: 5186 RVA: 0x000B4D10 File Offset: 0x000B3110
	Private Sub StartP2Super()
		Me.playerInSuper(1) = True
		Me.playerRelativePosAtSuperStart(1) = Me.player2.transform.position.y - MyBase.transform.position.y
	End Sub

	' Token: 0x06001443 RID: 5187 RVA: 0x000B4D5C File Offset: 0x000B315C
	Private Sub EndP2Super()
		Me.playerInSuper(1) = False
		If Me.restorePlayerPos(1) Then
			Me.player2.transform.position = New Vector3(Me.player1.transform.position.x, MyBase.transform.position.y + Me.playerRelativePosAtSuperStart(1))
		End If
		Me.restorePlayerPos(1) = False
	End Sub

	' Token: 0x06001444 RID: 5188 RVA: 0x000B4DD4 File Offset: 0x000B31D4
	Public Sub AutoMoveToPos(pos As Vector3, Optional controlTilt As Boolean = True, Optional holdForYToReleaseX As Boolean = True)
		If Me.autoMoveCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.autoMoveCoroutine)
		End If
		Me.autoTilt = controlTilt
		Me.autoDest = pos
		Me.autoX = True
		Me.autoY = True
		Me.autoMoveCoroutine = MyBase.StartCoroutine(Me.auto_move_to_pos_cr(holdForYToReleaseX))
	End Sub

	' Token: 0x06001445 RID: 5189 RVA: 0x000B4E28 File Offset: 0x000B3228
	Private Iterator Function auto_move_to_pos_cr(holdForYToReleaseX As Boolean) As IEnumerator
		If Mathf.Abs(Me.autoDest.y - MyBase.transform.position.y) > 50F Then
			Me.moveSpeed.y = Mathf.Sign(MyBase.transform.position.y - Me.autoDest.y) * 100F
		End If
		Dim maxYSpeed As Single = 100F
		Dim yDir As Single = Mathf.Sign(Me.autoDest.y - MyBase.transform.position.y)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While Me.autoX OrElse Me.autoY
			If Not CupheadTime.IsPaused() Then
				If Me.autoX Then
					Dim num As Single = If((Mathf.Abs(Me.autoDest.x - MyBase.transform.position.x) >= 100F), 1F, 0.5F)
					Me.moveSpeed.x = Mathf.Clamp(Me.moveSpeed.x + Mathf.Sign(Me.autoDest.x - MyBase.transform.position.x) * 5F * num, -400F, 400F)
					Me.MoveAirplane()
					If Mathf.Abs(MyBase.transform.position.x - Me.autoDest.x) < 50F AndAlso Mathf.Abs(MyBase.transform.position.y - Me.autoDest.y) < CSng(If((Not holdForYToReleaseX), 1000, 20)) Then
						Me.autoX = False
						Me.moveSpeed.x = Mathf.Clamp(Me.moveSpeed.x, MyBase.properties.CurrentState.plane.speedAtMaxTilt.min, MyBase.properties.CurrentState.plane.speedAtMaxTilt.max)
					End If
				End If
				If Me.autoY Then
					Dim num2 As Single = If((Mathf.Abs(Me.autoDest.y - MyBase.transform.position.y) >= 50F), 1F, 0.5F)
					Me.moveSpeed.y = Mathf.Clamp(Me.moveSpeed.y + Mathf.Sign(Me.autoDest.y - MyBase.transform.position.y) * 3F * num2, -maxYSpeed, maxYSpeed)
					If yDir <> Mathf.Sign(Me.autoDest.y - MyBase.transform.position.y) Then
						maxYSpeed *= 0.99F
					End If
					If Mathf.Abs(MyBase.transform.position.y - Me.autoDest.y) < 5F AndAlso Me.moveSpeed.y < 2F Then
						Me.autoY = False
						Me.moveSpeed.y = 0F
						MyBase.transform.position = New Vector3(MyBase.transform.position.x, Me.autoDest.y)
					End If
				End If
			End If
			Yield wait
		End While
		Me.autoX = False
		Return
	End Function

	' Token: 0x06001446 RID: 5190 RVA: 0x000B4E4C File Offset: 0x000B324C
	Private Sub HandleDip()
		Me.p1contactTime = If((Not Me.p1IsColliding), 0F, Mathf.Clamp(Me.p1contactTime + CupheadTime.Delta, 0F, 1.2F))
		Me.p2contactTime = If((Not Me.p2IsColliding), 0F, Mathf.Clamp(Me.p2contactTime + CupheadTime.Delta, 0F, 1.2F))
		Dim num As Single = Mathf.Clamp(Mathf.Sin(Me.p1contactTime / 1.2F * 3.1415927F) * 12F + Mathf.Sin(Me.p2contactTime / 1.2F * 3.1415927F) * 12F, 0F, 12F)
		If Not Me.p1IsColliding AndAlso Not Me.p2IsColliding Then
			Me.tiltable.localPosition = Vector3.Lerp(Me.tiltable.transform.localPosition, Me.tiltableBasePos + Vector3.up * 9F, 0.1F)
		Else
			Me.tiltable.localPosition = Vector3.Lerp(Me.tiltable.transform.localPosition, Me.tiltableBasePos + Vector3.down * num, 0.5F)
		End If
	End Sub

	' Token: 0x06001447 RID: 5191 RVA: 0x000B4FB0 File Offset: 0x000B33B0
	Private Function GetDestRotationVal() As Single
		Dim plane As LevelProperties.Airplane.Plane = MyBase.properties.CurrentState.plane
		Dim num As Single = Vector3.Distance(Me.edgeRight.position, Vector3.Lerp(Me.edgeLeft.position, Me.edgeRight.position, Mathf.InverseLerp(plane.speedAtMaxTilt.min, plane.speedAtMaxTilt.max, Me.moveSpeed.x)))
		Dim num2 As Single
		If Me.p1IsColliding AndAlso Me.p2IsColliding AndAlso Me.player1 IsNot Nothing AndAlso Me.player2 IsNot Nothing Then
			num2 = Vector3.Distance(Vector3.Lerp(Me.player1.transform.position, Me.player2.transform.position, 0.5F), Me.edgeRight.position)
		Else
			Dim abstractPlayerController As AbstractPlayerController = Nothing
			If Me.p1IsColliding AndAlso Me.player1 IsNot Nothing Then
				abstractPlayerController = Me.player1
			ElseIf Me.p2IsColliding AndAlso Me.player2 IsNot Nothing Then
				abstractPlayerController = Me.player2
			End If
			If abstractPlayerController IsNot Nothing Then
				num2 = Vector3.Distance(Me.edgeRight.position, abstractPlayerController.transform.position)
			Else
				num2 = num
			End If
		End If
		Return Mathf.Lerp(num2, num, Me.autoTiltTime)
	End Function

	' Token: 0x06001448 RID: 5192 RVA: 0x000B5128 File Offset: 0x000B3528
	Private Iterator Function handle_tilt_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Plane = MyBase.properties.CurrentState.plane
		Dim destRotationVal As Single = Me.rotationVal
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If Not CupheadTime.IsPaused() Then
				If Not Me.p1IsColliding AndAlso Not Me.p2IsColliding AndAlso Me.moveSpeed.x = 0F Then
					Mathf.Lerp(destRotationVal, Vector3.Distance(Me.edgeRight.position, MyBase.transform.position), 0.05F)
				Else
					destRotationVal = Me.GetDestRotationVal()
				End If
				If Mathf.Abs(destRotationVal - Me.rotationVal) > 0.1F Then
					Me.rotationVal = Mathf.Lerp(Me.rotationVal, destRotationVal, 0.15F)
				Else
					Me.rotationVal = destRotationVal
				End If
				Me.tiltable.transform.SetEulerAngles(Nothing, Nothing, New Single?(p.tiltAngle.GetFloatAt(Me.rotationVal / Me.rotationDist)))
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001449 RID: 5193 RVA: 0x000B5144 File Offset: 0x000B3544
	Private Iterator Function handle_player_move_cr() As IEnumerator
		Dim p As LevelProperties.Airplane.Plane = MyBase.properties.CurrentState.plane
		Dim destMoveSpeed As Single = 0F
		Dim goingLeft As Boolean = False
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		While True
			If Not CupheadTime.IsPaused() Then
				While Me.autoX
					Yield Nothing
				End While
				goingLeft = Me.moveSpeed.x < 0F
				If Not Me.p1IsColliding AndAlso Not Me.p2IsColliding Then
					If Me.moveSpeed.x < 0F AndAlso goingLeft Then
						Me.moveSpeed.x = Me.moveSpeed.x + p.decelerationAmount
					ElseIf Me.moveSpeed.x > 0F AndAlso Not goingLeft Then
						Me.moveSpeed.x = Me.moveSpeed.x - p.decelerationAmount
					End If
					destMoveSpeed = Me.moveSpeed.x
				Else
					destMoveSpeed = -p.speedAtMaxTilt.GetFloatAt(Me.rotationVal / Me.rotationDist)
					If Mathf.Abs(destMoveSpeed - Me.moveSpeed.x) > 4.1F Then
						Me.moveSpeed.x = Me.moveSpeed.x + Mathf.Sign(destMoveSpeed - Me.moveSpeed.x) * 4.1F
					Else
						Me.moveSpeed.x = destMoveSpeed
					End If
				End If
				Me.MoveAirplane()
			End If
			Yield wait
		End While
		Return
	End Function

	' Token: 0x0600144A RID: 5194 RVA: 0x000B515F File Offset: 0x000B355F
	Public Sub SetXRange(min As Single, max As Single)
		Me.minX = min
		Me.maxX = max
	End Sub

	' Token: 0x0600144B RID: 5195 RVA: 0x000B5170 File Offset: 0x000B3570
	Private Sub SetPartAngles()
		Dim num As Single = MyBase.transform.position.x / Me.maxParallaxX
		For i As Integer = 0 To Me.planeParts.Length - 1
			Me.planeParts(i).SetLocalEulerAngles(Nothing, Nothing, New Single?(Mathf.LerpUnclamped(0F, Me.planePartAngleRanges(i), num)))
			Me.planeParts(i).SetLocalPosition(New Single?(Mathf.LerpUnclamped(0F, Me.planePartPosOffsets(i).x, num)), New Single?(Mathf.Lerp(0F, Me.planePartPosOffsets(i).y, Mathf.Abs(num))), Nothing)
		Next
	End Sub

	' Token: 0x0600144C RID: 5196 RVA: 0x000B5248 File Offset: 0x000B3648
	Private Sub MoveAirplane()
		If CupheadTime.IsPaused() Then
			Return
		End If
		Me.HandleDip()
		Me.SetPartAngles()
		MyBase.transform.position += Vector3.up * Me.moveSpeed.y * CupheadTime.FixedDelta
		MyBase.transform.position += Vector3.right * Me.moveSpeed.x * CupheadTime.FixedDelta
		If Not Me.autoX Then
			If MyBase.transform.position.x < Me.minX AndAlso Me.moveSpeed.x < 0F Then
				Me.moveSpeed.x = Me.moveSpeed.x * (12.5F * CupheadTime.FixedDelta)
				Me.AutoMoveToPos(New Vector3(Me.minX + 50F, MyBase.transform.position.y), False, False)
			End If
			If MyBase.transform.position.x > Me.maxX AndAlso Me.moveSpeed.x > 0F Then
				Me.moveSpeed.x = Me.moveSpeed.x * (12.5F * CupheadTime.FixedDelta)
				Me.AutoMoveToPos(New Vector3(Me.maxX - 50F, MyBase.transform.position.y), False, False)
			End If
		End If
		Me.updateCount += 1
		If Me.updateCount Mod 5 = 0 Then
			Me.UpdateSound()
		End If
	End Sub

	' Token: 0x0600144D RID: 5197 RVA: 0x000B53FC File Offset: 0x000B37FC
	Private Sub UpdateSound()
		Dim num As Single = Mathf.Abs(Me.moveSpeed.x) / MyBase.properties.CurrentState.plane.speedAtMaxTilt.max
		If Mathf.Abs(num - Me.lastNormalizedSpeed) < 0.01F Then
			Return
		End If
		Me.lastNormalizedSpeed = num
		AudioManager.ChangeSFXPitch("sfx_dlc_dogfight_playerplane_loop", 1F + num * Me.pitchIncreaseFactor, 0F)
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_playerplane_loop", Me.volume.GetFloatAt(num), 0F)
		Dim floatAt As Single = Me.volumeHighSpeed.GetFloatAt(Mathf.InverseLerp(Me.volumeHighSpeedSpeedFloor, 1F, num))
		If floatAt > Me.cachedHighSpeedVolume Then
			Me.cachedHighSpeedVolume += Me.highSpeedVolumeIncreaseRate * CupheadTime.FixedDelta
			If Me.cachedHighSpeedVolume > floatAt Then
				Me.cachedHighSpeedVolume = floatAt
			End If
		End If
		If floatAt < Me.cachedHighSpeedVolume Then
			Me.cachedHighSpeedVolume -= Me.highSpeedVolumeDecreaseRate * CupheadTime.FixedDelta
			If Me.cachedHighSpeedVolume < floatAt Then
				Me.cachedHighSpeedVolume = floatAt
			End If
		End If
		AudioManager.ChangeSFXPitch("sfx_dlc_dogfight_playerplane_highspeed_loop", 1F + num * Me.pitchIncreaseFactorHighSpeed, 0F)
		AudioManager.FadeSFXVolume("sfx_dlc_dogfight_playerplane_highspeed_loop", Me.cachedHighSpeedVolume, 0F)
	End Sub

	' Token: 0x0600144E RID: 5198 RVA: 0x000B554B File Offset: 0x000B394B
	Private Sub SFX_DOGFIGHT_PlayerPlane_Loop()
		AudioManager.PlayLoop("sfx_dlc_dogfight_playerplane_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_playerplane_loop")
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_playerplane_loop", 0.25F, 3F)
	End Sub

	' Token: 0x0600144F RID: 5199 RVA: 0x000B557B File Offset: 0x000B397B
	Private Sub SFX_DOGFIGHT_PlayerPlane_HighSpeed_Loop()
		AudioManager.PlayLoop("sfx_dlc_dogfight_playerplane_highspeed_loop")
		Me.emitAudioFromObject.Add("sfx_dlc_dogfight_playerplane_highspeed_loop")
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_dogfight_playerplane_highspeed_loop", 0.6F, 3F)
	End Sub

	' Token: 0x06001450 RID: 5200 RVA: 0x000B55AB File Offset: 0x000B39AB
	Private Sub SFX_DOGFIGHT_PlayerPlane_StopLoop()
		AudioManager.[Stop]("sfx_dlc_dogfight_playerplane_loop")
	End Sub

	' Token: 0x06001451 RID: 5201 RVA: 0x000B55B7 File Offset: 0x000B39B7
	Private Sub AnimationEvent_SFX_DOGFIGHT_PlayerPlane_CanteenCheer()
		AudioManager.Play("sfx_dlc_dogfight_p2_pilotclap")
	End Sub

	' Token: 0x06001452 RID: 5202 RVA: 0x000B55C4 File Offset: 0x000B39C4
	Private Sub WORKAROUND_NullifyFields()
		Me.volume = Nothing
		Me.volumeHighSpeed = Nothing
		Me.edgeLeft = Nothing
		Me.edgeRight = Nothing
		Me.airplane1 = Nothing
		Me.tiltable = Nothing
		Me.planeParts = Nothing
		Me.planePartAngleRanges = Nothing
		Me.planePartPosOffsets = Nothing
		Me.planePuffFX = Nothing
		Me.planePuffPos = Nothing
		Me.player1 = Nothing
		Me.player2 = Nothing
		Me.playerInSuper = Nothing
		Me.restorePlayerPos = Nothing
		Me.playerRelativePosAtSuperStart = Nothing
		Me.puffTimer = Nothing
		Me.autoMoveCoroutine = Nothing
	End Sub

	' Token: 0x04001D67 RID: 7527
	Private Const PUFF_DELAY_L As Single = 1F

	' Token: 0x04001D68 RID: 7528
	Private Const PUFF_DELAY_R As Single = 0.8F

	' Token: 0x04001D69 RID: 7529
	Private Const AUTO_MOVE_MAX_X_DIST As Single = 50F

	' Token: 0x04001D6A RID: 7530
	Private Const AUTO_MOVE_MAX_Y_DIST As Single = 5F

	' Token: 0x04001D6B RID: 7531
	Private Const AUTO_MOVE_MAX_Y_END_SPEED As Single = 2F

	' Token: 0x04001D6C RID: 7532
	Private Const AUTO_MOVE_MAX_X_SPEED As Single = 400F

	' Token: 0x04001D6D RID: 7533
	Private Const AUTO_MOVE_MAX_Y_SPEED As Single = 100F

	' Token: 0x04001D6E RID: 7534
	Private Const AUTO_ACCEL_X As Single = 5F

	' Token: 0x04001D6F RID: 7535
	Private Const AUTO_ACCEL_Y As Single = 3F

	' Token: 0x04001D70 RID: 7536
	Private Const MIN_TILT_DIFFERENCE As Single = 0.1F

	' Token: 0x04001D71 RID: 7537
	Private Const TILT_ATTENUATION As Single = 0.15F

	' Token: 0x04001D72 RID: 7538
	Private Const ACCEL_SPEED As Single = 4.1F

	' Token: 0x04001D73 RID: 7539
	Private Const BOUNCE_TIME As Single = 1.2F

	' Token: 0x04001D74 RID: 7540
	Private Const BOUNCE_DIST As Single = 12F

	' Token: 0x04001D75 RID: 7541
	Private Const RISE_DIST As Single = 9F

	' Token: 0x04001D76 RID: 7542
	Private Const RISE_RATE As Single = 0.1F

	' Token: 0x04001D77 RID: 7543
	Private Const BOUNCE_RATE As Single = 0.5F

	' Token: 0x04001D78 RID: 7544
	Private Const DAMP_ON_BOUNDARY_COLLIDE As Single = 12.5F

	' Token: 0x04001D79 RID: 7545
	<SerializeField()>
	Private pitchIncreaseFactor As Single = 0.5F

	' Token: 0x04001D7A RID: 7546
	<SerializeField()>
	Private pitchIncreaseFactorHighSpeed As Single = 0.5F

	' Token: 0x04001D7B RID: 7547
	<SerializeField()>
	Private volume As MinMax = New MinMax(0.25F, 0.5F)

	' Token: 0x04001D7C RID: 7548
	<SerializeField()>
	Private volumeHighSpeed As MinMax = New MinMax(0.25F, 0.5F)

	' Token: 0x04001D7D RID: 7549
	Private cachedHighSpeedVolume As Single = 1E-06F

	' Token: 0x04001D7E RID: 7550
	<SerializeField()>
	Private highSpeedVolumeIncreaseRate As Single = 1F

	' Token: 0x04001D7F RID: 7551
	<SerializeField()>
	Private highSpeedVolumeDecreaseRate As Single = 0.25F

	' Token: 0x04001D80 RID: 7552
	<SerializeField()>
	Private volumeHighSpeedSpeedFloor As Single = 0.5F

	' Token: 0x04001D81 RID: 7553
	<SerializeField()>
	Private edgeLeft As Transform

	' Token: 0x04001D82 RID: 7554
	<SerializeField()>
	Private edgeRight As Transform

	' Token: 0x04001D83 RID: 7555
	<SerializeField()>
	Private airplane1 As Transform

	' Token: 0x04001D84 RID: 7556
	<SerializeField()>
	Private tiltable As Transform

	' Token: 0x04001D85 RID: 7557
	<SerializeField()>
	Private planeParts As Transform()

	' Token: 0x04001D86 RID: 7558
	<SerializeField()>
	Private planePartAngleRanges As Single()

	' Token: 0x04001D87 RID: 7559
	<SerializeField()>
	Private planePartPosOffsets As Vector2()

	' Token: 0x04001D88 RID: 7560
	<SerializeField()>
	Private planePuffFX As Effect

	' Token: 0x04001D89 RID: 7561
	<SerializeField()>
	Private planePuffPos As Transform()

	' Token: 0x04001D8A RID: 7562
	Private player1 As AbstractPlayerController

	' Token: 0x04001D8B RID: 7563
	Private player2 As AbstractPlayerController

	' Token: 0x04001D8C RID: 7564
	Private p1IsColliding As Boolean

	' Token: 0x04001D8D RID: 7565
	Private p2IsColliding As Boolean

	' Token: 0x04001D8E RID: 7566
	Private tiltableBasePos As Vector3

	' Token: 0x04001D8F RID: 7567
	Private maxParallaxX As Single

	' Token: 0x04001D90 RID: 7568
	Public autoX As Boolean

	' Token: 0x04001D91 RID: 7569
	Public autoY As Boolean

	' Token: 0x04001D92 RID: 7570
	Public autoDest As Vector3

	' Token: 0x04001D93 RID: 7571
	Private autoTiltTime As Single

	' Token: 0x04001D94 RID: 7572
	Private autoTilt As Boolean

	' Token: 0x04001D95 RID: 7573
	Private minX As Single

	' Token: 0x04001D96 RID: 7574
	Private maxX As Single

	' Token: 0x04001D97 RID: 7575
	Private rotationDist As Single

	' Token: 0x04001D98 RID: 7576
	Private rotationVal As Single

	' Token: 0x04001D99 RID: 7577
	Private p1contactTime As Single

	' Token: 0x04001D9A RID: 7578
	Private p2contactTime As Single

	' Token: 0x04001D9B RID: 7579
	Private playerInSuper As Boolean() = New Boolean(1) {}

	' Token: 0x04001D9C RID: 7580
	Private restorePlayerPos As Boolean() = New Boolean(1) {}

	' Token: 0x04001D9D RID: 7581
	Private playerRelativePosAtSuperStart As Single() = New Single(1) {}

	' Token: 0x04001D9E RID: 7582
	Private puffTimer As Single() = New Single(1) {}

	' Token: 0x04001D9F RID: 7583
	Private moveSpeed As Vector3

	' Token: 0x04001DA0 RID: 7584
	Private autoMoveCoroutine As Coroutine

	' Token: 0x04001DA1 RID: 7585
	Private lastNormalizedSpeed As Single

	' Token: 0x04001DA2 RID: 7586
	Private updateCount As Integer
End Class
