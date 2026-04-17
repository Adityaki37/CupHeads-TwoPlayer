Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200086F RID: 2159
Public Class PlatformingLevelShootingEnemy
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x06003234 RID: 12852 RVA: 0x001CF48D File Offset: 0x001CD88D
	Protected Overrides Sub Start()
		MyBase.Start()
		Me._aim = New GameObject("Aim").transform
		Me._aim.SetParent(Me._projectileRoot)
		Me._aim.ResetLocalTransforms()
	End Sub

	' Token: 0x06003235 RID: 12853 RVA: 0x001CF4C8 File Offset: 0x001CD8C8
	Protected Overrides Sub OnStart()
		Me._projectileDelay = MyBase.Properties.ProjectileDelay.RandomFloat()
		Select Case Me._triggerType
			Case PlatformingLevelShootingEnemy.TriggerType.Range
				MyBase.StartCoroutine(Me.ranged_cr())
			Case PlatformingLevelShootingEnemy.TriggerType.TriggerVolumes
				MyBase.StartCoroutine(Me.triggerVolumes_cr())
			Case PlatformingLevelShootingEnemy.TriggerType.OnScreen
				MyBase.StartCoroutine(Me.onscreen_cr())
			Case PlatformingLevelShootingEnemy.TriggerType.Indefinite
				MyBase.StartCoroutine(Me.indefinite_cr())
		End Select
	End Sub

	' Token: 0x06003236 RID: 12854 RVA: 0x001CF55A File Offset: 0x001CD95A
	Protected Overridable Sub StartShoot()
		MyBase.animator.SetTrigger("Shoot")
	End Sub

	' Token: 0x06003237 RID: 12855 RVA: 0x001CF56C File Offset: 0x001CD96C
	Protected Overridable Sub Shoot()
		Dim num As Single = MyBase.Properties.ProjectileAngle
		Dim num2 As Single = MyBase.Properties.ProjectileSpeed
		If Me._target Is Nothing OrElse Me._target.IsDead Then
			Me._target = PlayerManager.GetNext()
		End If
		Select Case MyBase.Properties.ProjectileAimMode
			Case EnemyProperties.AimMode.AimedAtPlayer
				Me._aim.LookAt2D(Me._target.center)
				num = Me._aim.transform.eulerAngles.z
			Case EnemyProperties.AimMode.ArcAimedAtPlayer
				Dim num3 As Single = Single.MaxValue
				Dim vector As Vector2 = Me._target.center - Me._projectileRoot.position
				vector.x = Mathf.Abs(vector.x)
				Dim minMax As MinMax = New MinMax(MyBase.Properties.ArcProjectileMinAngle, MyBase.Properties.ProjectileAngle)
				Dim minMax2 As MinMax = New MinMax(MyBase.Properties.ArcProjectileMinSpeed, MyBase.Properties.ProjectileSpeed)
				If vector.y > 0F AndAlso Me._ArcExtraSpeedUnderPlayerMultiplier > 0F Then
					Dim num4 As Single = minMax2.max / MyBase.Properties.ProjectileGravity
					Dim num5 As Single = minMax2.max * num4 - 0.5F * MyBase.Properties.ProjectileGravity * num4 * num4
					Dim num6 As Single = num5 + vector.y * Me._ArcExtraSpeedUnderPlayerMultiplier
					Dim num7 As Single = Mathf.Sqrt(2F * num6 / MyBase.Properties.ProjectileGravity)
					minMax2.max = num7 * MyBase.Properties.ProjectileGravity
					minMax2.min *= minMax2.max / MyBase.Properties.ProjectileSpeed
				End If
				Dim num8 As Single = 0F
				While num8 < 1F
					Dim floatAt As Single = minMax.GetFloatAt(num8)
					Dim floatAt2 As Single = minMax2.GetFloatAt(num8)
					Dim vector2 As Vector2 = MathUtils.AngleToDirection(floatAt) * floatAt2
					Dim num9 As Single = vector.x / vector2.x
					Dim num10 As Single = vector2.y * num9 - 0.5F * MyBase.Properties.ProjectileGravity * num9 * num9
					Dim num11 As Single = Mathf.Abs(vector.y - num10)
					If MyBase.Properties.ProjectileGravity <= 0.01F Then
						GoTo IL_0292
					End If
					Dim num12 As Single = vector2.y - MyBase.Properties.ProjectileGravity * num9
					If num12 <= 0F Then
						GoTo IL_0292
					End If
					IL_02A5:
					num8 += 0.01F
					Continue While
					IL_0292:
					If num11 < num3 Then
						num3 = num11
						num = floatAt
						num2 = floatAt2
						GoTo IL_02A5
					End If
					GoTo IL_02A5
				End While
				If(Not Me._hasFacingDirection AndAlso Me._target.center.x < MyBase.transform.position.x) OrElse (Me._hasFacingDirection AndAlso Me._direction = PlatformingLevelShootingEnemy.Direction.Left) Then
					num = 180F - num
				End If
			Case EnemyProperties.AimMode.Spread
				Dim vector3 As Vector3 = MathUtils.AngleToDirection(MyBase.Properties.ProjectileAngle)
				Dim num3 As Single = Single.MaxValue
				Dim vector As Vector2 = vector3 - Me._projectileRoot.position
				vector.x = Mathf.Abs(vector.x)
				Dim minMax2 As MinMax = New MinMax(MyBase.Properties.ArcProjectileMinSpeed, MyBase.Properties.ProjectileSpeed)
				If vector.y > 0F Then
					Dim num13 As Single = minMax2.max / MyBase.Properties.ProjectileGravity
					Dim num14 As Single = minMax2.max * num13 - 0.5F * MyBase.Properties.ProjectileGravity * num13 * num13
					Dim num15 As Single = num14 + vector.y * Me._ArcExtraSpeedUnderPlayerMultiplier
					Dim num16 As Single = Mathf.Sqrt(2F * num15 / MyBase.Properties.ProjectileGravity)
					minMax2.max = num16 * MyBase.Properties.ProjectileGravity
					minMax2.min *= minMax2.max / MyBase.Properties.ProjectileSpeed
				End If
				Dim num17 As Single = minMax2.RandomFloat()
				Dim vector4 As Vector2 = MathUtils.AngleToDirection(MyBase.Properties.ProjectileAngle) * num17
				Dim num18 As Single = vector.x / vector4.x
				Dim num19 As Single = vector4.y * num18 - 0.5F * MyBase.Properties.ProjectileGravity * num18 * num18
				Dim num20 As Single = Mathf.Abs(vector.y - num19)
				If num20 < num3 Then
					num = MyBase.Properties.ProjectileAngle
					num2 = num17
				End If
				For i As Integer = 0 To 2 - 1
					Dim num21 As Single = If((i <> 1), 90F, (180F - num))
					Dim basicProjectile As BasicProjectile = Me.projectilePrefab.Create(Me._projectileRoot.position, num21, num2)
					basicProjectile.SetParryable(MyBase.Properties.ProjectileParryable)
					basicProjectile.Gravity = MyBase.Properties.ProjectileGravity
				Next
			Case EnemyProperties.AimMode.Arc
				Dim vector3 As Vector3 = MathUtils.AngleToDirection(MyBase.Properties.ProjectileAngle)
				Dim num3 As Single = Single.MaxValue
				Dim vector As Vector2 = vector3 - Me._projectileRoot.position
				vector.x = Mathf.Abs(vector.x)
				Dim minMax2 As MinMax = New MinMax(MyBase.Properties.ArcProjectileMinSpeed, MyBase.Properties.ProjectileSpeed)
				If vector.y > 0F Then
					Dim num22 As Single = minMax2.max / MyBase.Properties.ProjectileGravity
					Dim num23 As Single = minMax2.max * num22 - 0.5F * MyBase.Properties.ProjectileGravity * num22 * num22
					Dim num24 As Single = num23 + vector.y * Me._ArcExtraSpeedUnderPlayerMultiplier
					Dim num25 As Single = Mathf.Sqrt(2F * num24 / MyBase.Properties.ProjectileGravity)
					minMax2.max = num25 * MyBase.Properties.ProjectileGravity
					minMax2.min *= minMax2.max / MyBase.Properties.ProjectileSpeed
				End If
				Dim num26 As Single = minMax2.RandomFloat()
				Dim vector5 As Vector2 = MathUtils.AngleToDirection(MyBase.Properties.ProjectileAngle) * num26
				Dim num27 As Single = vector.x / vector5.x
				Dim num28 As Single = vector5.y * num27 - 0.5F * MyBase.Properties.ProjectileGravity * num27 * num27
				Dim num29 As Single = Mathf.Abs(vector.y - num28)
				If num29 < num3 Then
					num = MyBase.Properties.ProjectileAngle
					num2 = num26
				End If
		End Select
		Dim basicProjectile2 As BasicProjectile = Me.projectilePrefab.Create(Me._projectileRoot.position, num, num2)
		basicProjectile2.SetParryable(MyBase.Properties.ProjectileParryable)
		basicProjectile2.SetStoneTime(MyBase.Properties.ProjectileStoneTime)
		basicProjectile2.Gravity = MyBase.Properties.ProjectileGravity
		Me.SpawnShootEffect()
	End Sub

	' Token: 0x06003238 RID: 12856 RVA: 0x001CFCAF File Offset: 0x001CE0AF
	Protected Overridable Sub SpawnShootEffect()
		If Me._shootEffect IsNot Nothing Then
			Me._shootEffect.Create(Me._effectRoot.position)
		End If
	End Sub

	' Token: 0x06003239 RID: 12857 RVA: 0x001CFCDC File Offset: 0x001CE0DC
	Protected Sub setDirection(direction As PlatformingLevelShootingEnemy.Direction)
		Me._direction = direction
		MyBase.transform.SetScale(New Single?(CSng(If((Me._direction <> PlatformingLevelShootingEnemy.Direction.Right), 1, (-1)))), Nothing, Nothing)
	End Sub

	' Token: 0x0600323A RID: 12858 RVA: 0x001CFD26 File Offset: 0x001CE126
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.projectilePrefab = Nothing
	End Sub

	' Token: 0x0600323B RID: 12859 RVA: 0x001CFD38 File Offset: 0x001CE138
	Private Iterator Function indefinite_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me._initialShotDelay.RandomFloat())
		While True
			If Me._hasShootingAnimation Then
				Me.StartShoot()
				Yield MyBase.animator.WaitForAnimationToStart(Me, "Shoot", False)
				Me._target = PlayerManager.GetNext()
			Else
				Me._target = PlayerManager.GetNext()
				Me.Shoot()
			End If
			Yield CupheadTime.WaitForSeconds(Me, Me._projectileDelay)
		End While
		Return
	End Function

	' Token: 0x0600323C RID: 12860 RVA: 0x001CFD54 File Offset: 0x001CE154
	Private Iterator Function onscreen_cr() As IEnumerator
		While True
			While Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(-Me.onScreenTriggerPadding, 0F))
				Yield Nothing
			End While
			If Not Me._hasFired Then
				Yield CupheadTime.WaitForSeconds(Me, Me._initialShotDelay.RandomFloat())
				Me._hasFired = True
			Else
				If Me._hasShootingAnimation Then
					Me.StartShoot()
					Yield MyBase.animator.WaitForAnimationToStart(Me, "Shoot", False)
					Me._target = PlayerManager.GetNext()
				Else
					Me._target = PlayerManager.GetNext()
					Me.Shoot()
				End If
				Yield CupheadTime.WaitForSeconds(Me, Me._projectileDelay)
			End If
		End While
		Return
	End Function

	' Token: 0x0600323D RID: 12861 RVA: 0x001CFD70 File Offset: 0x001CE170
	Private Iterator Function ranged_cr() As IEnumerator
		Dim lastPlayer As PlayerId = PlayerId.None
		While True
			Dim currentPlayer As PlayerId = PlayerId.PlayerOne
			Dim inRange As Boolean = False
			While Not inRange
				Dim cuphead As Boolean = Me.IsPlayerInRange(PlayerId.PlayerOne)
				Dim mugman As Boolean = PlayerManager.Multiplayer AndAlso Me.IsPlayerInRange(PlayerId.PlayerTwo)
				If cuphead AndAlso mugman Then
					currentPlayer = If((lastPlayer <> PlayerId.PlayerOne), PlayerId.PlayerOne, PlayerId.PlayerTwo)
					inRange = True
				ElseIf cuphead AndAlso Not mugman Then
					currentPlayer = PlayerId.PlayerOne
					inRange = True
				ElseIf Not cuphead AndAlso mugman Then
					currentPlayer = PlayerId.PlayerTwo
					inRange = True
				End If
				lastPlayer = currentPlayer
				Me._target = PlayerManager.GetPlayer(currentPlayer)
				Yield Nothing
			End While
			If Not Me._hasFired Then
				Yield CupheadTime.WaitForSeconds(Me, Me._initialShotDelay.RandomFloat())
				Me._hasFired = True
			Else
				If Me._hasShootingAnimation Then
					Me.StartShoot()
					Yield MyBase.animator.WaitForAnimationToStart(Me, "Shoot", False)
					Me._target = PlayerManager.GetPlayer(currentPlayer)
				Else
					Me._target = PlayerManager.GetPlayer(currentPlayer)
					Me.Shoot()
				End If
				Yield CupheadTime.WaitForSeconds(Me, Me._projectileDelay)
			End If
		End While
		Return
	End Function

	' Token: 0x0600323E RID: 12862 RVA: 0x001CFD8B File Offset: 0x001CE18B
	Private Function IsPlayerInRange(player As PlayerId) As Boolean
		Return Vector2.Distance(MyBase.transform.position, PlayerManager.GetPlayer(player).center) <= Me.triggerRange
	End Function

	' Token: 0x0600323F RID: 12863 RVA: 0x001CFDC0 File Offset: 0x001CE1C0
	Private Iterator Function triggerVolumes_cr() As IEnumerator
		Dim lastPlayer As PlayerId = PlayerId.None
		While True
			Dim currentPlayer As PlayerId = PlayerId.PlayerOne
			Dim within As Boolean = False
			While Not within
				Dim cuphead As Boolean = Me.IsPlayerInVolumes(PlayerId.PlayerOne)
				Dim mugman As Boolean = PlayerManager.Multiplayer AndAlso Me.IsPlayerInVolumes(PlayerId.PlayerTwo)
				If cuphead AndAlso mugman Then
					currentPlayer = If((lastPlayer <> PlayerId.PlayerOne), PlayerId.PlayerOne, PlayerId.PlayerTwo)
					within = True
				ElseIf cuphead AndAlso Not mugman Then
					currentPlayer = PlayerId.PlayerOne
					within = True
				ElseIf Not cuphead AndAlso mugman Then
					currentPlayer = PlayerId.PlayerTwo
					within = True
				End If
				lastPlayer = currentPlayer
				Yield Nothing
			End While
			If Not Me._hasFired Then
				Yield CupheadTime.WaitForSeconds(Me, Me._initialShotDelay.RandomFloat())
				Me._hasFired = True
			Else
				If Me._hasShootingAnimation Then
					Me.StartShoot()
					Yield MyBase.animator.WaitForAnimationToStart(Me, "Shoot", False)
					Me._target = PlayerManager.GetPlayer(currentPlayer)
				Else
					Me._target = PlayerManager.GetPlayer(currentPlayer)
					Me.Shoot()
				End If
				Yield CupheadTime.WaitForSeconds(Me, Me._projectileDelay)
			End If
		End While
		Return
	End Function

	' Token: 0x06003240 RID: 12864 RVA: 0x001CFDDC File Offset: 0x001CE1DC
	Protected Overridable Function IsPlayerInVolumes(player As PlayerId) As Boolean
		Dim vector As Vector2 = PlayerManager.GetPlayer(player).center
		For Each triggerVolumeProperties As PlatformingLevelShootingEnemy.TriggerVolumeProperties In Me._triggerVolumes
			Dim shape As PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape = triggerVolumeProperties.shape
			If shape <> PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.BoxCollider Then
				If shape = PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.CircleCollider Then
					Dim position As Vector2 = triggerVolumeProperties.position
					If triggerVolumeProperties.space = PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace Then
						position.x += MyBase.transform.position.x
						position.y += MyBase.transform.position.y
					End If
					If MathUtils.CircleContains(position, triggerVolumeProperties.circleRadius, vector) Then
						Return True
					End If
				End If
			Else
				Dim rect As Rect = RectUtils.NewFromCenter(triggerVolumeProperties.position.x, triggerVolumeProperties.position.y, triggerVolumeProperties.boxSize.x, triggerVolumeProperties.boxSize.y)
				If triggerVolumeProperties.space = PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace Then
					rect.x += MyBase.transform.position.x
					rect.y += MyBase.transform.position.y
				End If
				If rect.Contains(vector) Then
					Return True
				End If
			End If
		Next
		Return False
	End Function

	' Token: 0x06003241 RID: 12865 RVA: 0x001CFF84 File Offset: 0x001CE384
	Protected Overrides Sub OnDrawGizmos()
		MyBase.OnDrawGizmos()
		Me.DrawGizmos(0.2F)
	End Sub

	' Token: 0x06003242 RID: 12866 RVA: 0x001CFF97 File Offset: 0x001CE397
	Protected Overrides Sub OnDrawGizmosSelected()
		MyBase.OnDrawGizmosSelected()
		Me.DrawGizmos(1F)
	End Sub

	' Token: 0x06003243 RID: 12867 RVA: 0x001CFFAC File Offset: 0x001CE3AC
	Private Sub DrawGizmos(alpha As Single)
		If MyBase.Properties Is Nothing Then
			Return
		End If
		Dim triggerType As PlatformingLevelShootingEnemy.TriggerType = Me._triggerType
		If triggerType <> PlatformingLevelShootingEnemy.TriggerType.Range Then
			If triggerType <> PlatformingLevelShootingEnemy.TriggerType.TriggerVolumes Then
				If triggerType = PlatformingLevelShootingEnemy.TriggerType.Indefinite Then
					Me.DrawIndefiniteTriggerGizmos(alpha)
				End If
			Else
				Me.DrawTriggerVolumesTriggerGizmos(alpha)
			End If
		Else
			Me.DrawRangeTriggerGizmos(alpha)
		End If
		Dim projectileAimMode As EnemyProperties.AimMode = MyBase.Properties.ProjectileAimMode
		If projectileAimMode <> EnemyProperties.AimMode.AimedAtPlayer Then
			If projectileAimMode = EnemyProperties.AimMode.Straight Then
				Me.DrawStraightAimGizmos(alpha)
			End If
		Else
			Me.DrawAimedAtPlayerAimGizmos(alpha)
		End If
	End Sub

	' Token: 0x06003244 RID: 12868 RVA: 0x001D0044 File Offset: 0x001CE444
	Private Sub DrawStraightAimGizmos(alpha As Single)
		Dim red As Color = Color.red
		red.a = alpha
		Gizmos.color = red
		Dim position As Vector3 = MyBase.transform.position
		Dim vector As Vector3 = position + Quaternion.Euler(0F, 0F, MyBase.Properties.ProjectileAngle) * Vector3.right * Me.triggerRange
		Dim vector2 As Vector3 = position + Quaternion.Euler(0F, 0F, MyBase.Properties.ProjectileAngle) * Vector3.right * 10000F
		Gizmos.DrawLine(position, vector)
		red.a *= 0.25F
		Gizmos.color = red
		Gizmos.DrawLine(position, vector2)
	End Sub

	' Token: 0x06003245 RID: 12869 RVA: 0x001D0104 File Offset: 0x001CE504
	Private Sub DrawAimedAtPlayerAimGizmos(alpha As Single)
		Dim red As Color = Color.red
		red.a = alpha
		Gizmos.color = red
		Dim vector As Vector3 = MyBase.transform.position + New Vector3(-100F, 100F, 0F)
		Dim vector2 As Vector3 = Vector3.one * 40F / 2F
		vector2.z = 0.001F
		Dim vector3 As Vector3 = vector + New Vector3(-vector2.x / 2F, vector2.y / 2F, 0F)
		Dim vector4 As Vector3 = vector3
		vector4.y -= vector2.y * 2F
		Gizmos.DrawWireCube(vector, vector2)
		Gizmos.DrawLine(vector3, vector4)
	End Sub

	' Token: 0x06003246 RID: 12870 RVA: 0x001D01CC File Offset: 0x001CE5CC
	Private Sub DrawRangeTriggerGizmos(alpha As Single)
		Dim yellow As Color = Color.yellow
		yellow.a = alpha
		Gizmos.color = yellow
		Gizmos.DrawWireSphere(MyBase.transform.position, Me.triggerRange)
	End Sub

	' Token: 0x06003247 RID: 12871 RVA: 0x001D0204 File Offset: 0x001CE604
	Private Sub DrawTriggerVolumesTriggerGizmos(alpha As Single)
		Dim yellow As Color = Color.yellow
		yellow.a = alpha
		Gizmos.color = yellow
		For Each triggerVolumeProperties As PlatformingLevelShootingEnemy.TriggerVolumeProperties In Me._triggerVolumes
			Dim vector As Vector2 = triggerVolumeProperties.position
			If triggerVolumeProperties.space = PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace Then
				vector += MyBase.transform.position
			End If
			Dim shape As PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape = triggerVolumeProperties.shape
			If shape <> PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.CircleCollider Then
				If shape = PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.BoxCollider Then
					Gizmos.DrawWireCube(vector, triggerVolumeProperties.boxSize)
				End If
			Else
				Gizmos.DrawWireSphere(vector, triggerVolumeProperties.circleRadius)
			End If
		Next
	End Sub

	' Token: 0x06003248 RID: 12872 RVA: 0x001D02E4 File Offset: 0x001CE6E4
	Private Sub DrawIndefiniteTriggerGizmos(alpha As Single)
		Dim yellow As Color = Color.yellow
		yellow.a = alpha
		Gizmos.color = yellow
		Dim vector As Vector3 = MyBase.transform.position + New Vector3(100F, 100F, 0F)
		Dim vector2 As Vector3 = New Vector3(vector.x, vector.y + 10F, 0F)
		Dim vector3 As Vector3 = vector2
		vector3.y -= 40F
		Dim vector4 As Vector3 = vector2
		vector4.x -= 10F
		Dim vector5 As Vector3 = vector2
		vector5.x += 10F
		Dim vector6 As Vector3 = vector3
		vector6.x -= 10F
		Dim vector7 As Vector3 = vector3
		vector7.x += 10F
		Gizmos.DrawLine(vector2, vector3)
		Gizmos.DrawLine(vector4, vector5)
		Gizmos.DrawLine(vector6, vector7)
	End Sub

	' Token: 0x04003A8A RID: 14986
	<Header("Trigger Properties")>
	<SerializeField()>
	Private _triggerType As PlatformingLevelShootingEnemy.TriggerType

	' Token: 0x04003A8B RID: 14987
	<SerializeField()>
	Private _triggerVolumes As List(Of PlatformingLevelShootingEnemy.TriggerVolumeProperties)

	' Token: 0x04003A8C RID: 14988
	<SerializeField()>
	Protected _shootEffect As Effect

	' Token: 0x04003A8D RID: 14989
	<SerializeField()>
	Protected _effectRoot As Transform

	' Token: 0x04003A8E RID: 14990
	<SerializeField()>
	Private _projectileRoot As Transform

	' Token: 0x04003A8F RID: 14991
	<SerializeField()>
	Private _hasShootingAnimation As Boolean

	' Token: 0x04003A90 RID: 14992
	<SerializeField()>
	Private _initialShotDelay As MinMax

	' Token: 0x04003A91 RID: 14993
	<SerializeField()>
	Private _hasFacingDirection As Boolean

	' Token: 0x04003A92 RID: 14994
	<SerializeField()>
	Private _ArcExtraSpeedUnderPlayerMultiplier As Single

	' Token: 0x04003A93 RID: 14995
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x04003A94 RID: 14996
	Public triggerRange As Single = 1000F

	' Token: 0x04003A95 RID: 14997
	Public onScreenTriggerPadding As Single

	' Token: 0x04003A96 RID: 14998
	Protected _target As AbstractPlayerController

	' Token: 0x04003A97 RID: 14999
	Private _aim As Transform

	' Token: 0x04003A98 RID: 15000
	Private _hasFired As Boolean

	' Token: 0x04003A99 RID: 15001
	Private _projectileDelay As Single

	' Token: 0x04003A9A RID: 15002
	Private _direction As PlatformingLevelShootingEnemy.Direction

	' Token: 0x04003A9B RID: 15003
	Private Const GIZMO_LETTER_LENGTH As Single = 40F

	' Token: 0x02000870 RID: 2160
	Public Enum TriggerType
		' Token: 0x04003A9D RID: 15005
		Range
		' Token: 0x04003A9E RID: 15006
		TriggerVolumes
		' Token: 0x04003A9F RID: 15007
		OnScreen
		' Token: 0x04003AA0 RID: 15008
		Indefinite
	End Enum

	' Token: 0x02000871 RID: 2161
	Public Enum Direction
		' Token: 0x04003AA2 RID: 15010
		Left
		' Token: 0x04003AA3 RID: 15011
		Right
	End Enum

	' Token: 0x02000872 RID: 2162
	<Serializable()>
	Public Class TriggerVolumeProperties
		' Token: 0x0600324A RID: 12874 RVA: 0x001D0404 File Offset: 0x001CE804
		Public Function ToRect() As Rect
			Dim rect As Rect = New Rect(Me.position, Me.boxSize)
			rect.x -= rect.width / 2F
			rect.y -= rect.height / 2F
			Return rect
		End Function

		' Token: 0x04003AA4 RID: 15012
		Public shape As PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape

		' Token: 0x04003AA5 RID: 15013
		Public space As PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space

		' Token: 0x04003AA6 RID: 15014
		Public position As Vector2 = Vector2.zero

		' Token: 0x04003AA7 RID: 15015
		Public boxSize As Vector2 = New Vector2(100F, 100F)

		' Token: 0x04003AA8 RID: 15016
		Public circleRadius As Single = 100F

		' Token: 0x02000873 RID: 2163
		Public Enum Shape
			' Token: 0x04003AAA RID: 15018
			BoxCollider
			' Token: 0x04003AAB RID: 15019
			CircleCollider
		End Enum

		' Token: 0x02000874 RID: 2164
		Public Enum Space
			' Token: 0x04003AAD RID: 15021
			RelativeSpace
			' Token: 0x04003AAE RID: 15022
			WorldSpace
		End Enum
	End Class
End Class
