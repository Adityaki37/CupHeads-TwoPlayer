Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000876 RID: 2166
Public Class ForestPlatformingLevelAcorn
	Inherits AbstractPlatformingLevelEnemy

	' Token: 0x0600324D RID: 12877 RVA: 0x001D4D9C File Offset: 0x001D319C
	Public Function Spawn(parent As ForestPlatformingLevelAcornMaker, position As Vector2, direction As ForestPlatformingLevelAcorn.Direction, moveUpFirst As Boolean) As ForestPlatformingLevelAcorn
		Dim forestPlatformingLevelAcorn As ForestPlatformingLevelAcorn = Me.InstantiatePrefab(Of ForestPlatformingLevelAcorn)()
		forestPlatformingLevelAcorn.transform.position = position
		forestPlatformingLevelAcorn._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant
		forestPlatformingLevelAcorn._direction = direction
		forestPlatformingLevelAcorn._player = PlayerManager.GetNext()
		forestPlatformingLevelAcorn.parent = parent
		If moveUpFirst Then
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.move_up_cr())
		Else
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.main_cr())
		End If
		Return forestPlatformingLevelAcorn
	End Function

	' Token: 0x0600324E RID: 12878 RVA: 0x001D4E08 File Offset: 0x001D3208
	Public Function Spawn(position As Vector2, direction As ForestPlatformingLevelAcorn.Direction, moveUpFirst As Boolean) As ForestPlatformingLevelAcorn
		Dim forestPlatformingLevelAcorn As ForestPlatformingLevelAcorn = Me.InstantiatePrefab(Of ForestPlatformingLevelAcorn)()
		forestPlatformingLevelAcorn.transform.position = position
		forestPlatformingLevelAcorn._startCondition = AbstractPlatformingLevelEnemy.StartCondition.Instant
		forestPlatformingLevelAcorn._direction = direction
		forestPlatformingLevelAcorn._player = PlayerManager.GetNext()
		If moveUpFirst Then
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.move_up_cr())
		Else
			forestPlatformingLevelAcorn.StartCoroutine(forestPlatformingLevelAcorn.main_cr())
		End If
		Return forestPlatformingLevelAcorn
	End Function

	' Token: 0x0600324F RID: 12879 RVA: 0x001D4E6C File Offset: 0x001D326C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		AudioManager.PlayLoop("level_acorn_fly")
		Me.emitAudioFromObject.Add("level_acorn_fly")
	End Sub

	' Token: 0x06003250 RID: 12880 RVA: 0x001D4E90 File Offset: 0x001D3290
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.parent IsNot Nothing Then
			Dim forestPlatformingLevelAcornMaker As ForestPlatformingLevelAcornMaker = Me.parent
			forestPlatformingLevelAcornMaker.killAcorns = CType([Delegate].Combine(forestPlatformingLevelAcornMaker.killAcorns, AddressOf Me.Kill), Action)
			MyBase.StartCoroutine(Me.acorn_death_timer_cr())
		End If
	End Sub

	' Token: 0x06003251 RID: 12881 RVA: 0x001D4EE8 File Offset: 0x001D32E8
	Protected Overrides Sub OnStart()
	End Sub

	' Token: 0x06003252 RID: 12882 RVA: 0x001D4EEC File Offset: 0x001D32EC
	Protected Overrides Sub Update()
		MyBase.Update()
		If CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position) AndAlso Not Me._enteredScreen Then
			Me._enteredScreen = True
		End If
		If Me._enteredScreen AndAlso Not CupheadLevelCamera.Current.ContainsPoint(MyBase.transform.position, New Vector2(100F, 100F)) Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		If MyBase.transform.position.x < CSng(PlatformingLevel.Current.Left) - 100F OrElse MyBase.transform.position.x > CSng(PlatformingLevel.Current.Right) + 100F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		MyBase.transform.SetScale(New Single?(CSng(If((Me._direction <> ForestPlatformingLevelAcorn.Direction.Left), (-1), 1))), Nothing, Nothing)
	End Sub

	' Token: 0x06003253 RID: 12883 RVA: 0x001D5008 File Offset: 0x001D3408
	Private Iterator Function move_up_cr() As IEnumerator
		Dim yOffset As Single = 100F
		While MyBase.transform.position.y < CupheadLevelCamera.Current.Bounds.yMax - yOffset
			MyBase.transform.AddPosition(0F, MyBase.Properties.AcornFlySpeed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.main_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06003254 RID: 12884 RVA: 0x001D5024 File Offset: 0x001D3424
	Private Iterator Function main_cr() As IEnumerator
		While (Me._direction = ForestPlatformingLevelAcorn.Direction.Left AndAlso MyBase.transform.position.x > Me._player.center.x) OrElse (Me._direction = ForestPlatformingLevelAcorn.Direction.Right AndAlso MyBase.transform.position.x < Me._player.center.x)
			MyBase.transform.AddPosition(If((Me._direction <> ForestPlatformingLevelAcorn.Direction.Right), (-MyBase.Properties.AcornFlySpeed * CupheadTime.FixedDelta), (MyBase.Properties.AcornFlySpeed * CupheadTime.FixedDelta)), 0F, 0F)
			Yield New WaitForFixedUpdate()
			If Me._player Is Nothing OrElse Me._player.IsDead Then
				Me._player = PlayerManager.GetNext()
			End If
		End While
		MyBase.animator.SetTrigger("Drop")
		AudioManager.[Stop]("level_acorn_fly")
		AudioManager.Play("level_acorn_drop")
		Me.emitAudioFromObject.Add("level_acorn_drop")
		Dim t As Single = 0F
		Me._hasDropped = True
		Me.LaunchPropeller()
		While t < 0.5F
			MyBase.transform.AddPosition(0F, -MyBase.Properties.AcornDropSpeed * CupheadTime.FixedDelta * t / 0.5F, 0F)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		While True
			MyBase.transform.AddPosition(0F, -MyBase.Properties.AcornDropSpeed * CupheadTime.FixedDelta, 0F)
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06003255 RID: 12885 RVA: 0x001D5040 File Offset: 0x001D3440
	Private Iterator Function acorn_death_timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Dim forestPlatformingLevelAcornMaker As ForestPlatformingLevelAcornMaker = Me.parent
		forestPlatformingLevelAcornMaker.killAcorns = CType([Delegate].Remove(forestPlatformingLevelAcornMaker.killAcorns, AddressOf Me.Kill), Action)
		Yield Nothing
		Return
	End Function

	' Token: 0x06003256 RID: 12886 RVA: 0x001D505B File Offset: 0x001D345B
	Private Sub LaunchPropeller()
		Me.propellerPrefab.Create(MyBase.transform.position, MyBase.Properties.AcornPropellerSpeed)
	End Sub

	' Token: 0x06003257 RID: 12887 RVA: 0x001D5084 File Offset: 0x001D3484
	Protected Overrides Sub Die()
		If Not Me._hasDropped Then
			Me.LaunchPropeller()
			AudioManager.[Stop]("level_acorn_fly")
		Else
			AudioManager.[Stop]("level_acorn_drop")
		End If
		AudioManager.Play("level_flowergrunt_death")
		Me.emitAudioFromObject.Add("level_flowergrunt_death")
		MyBase.Die()
	End Sub

	' Token: 0x06003258 RID: 12888 RVA: 0x001D50DB File Offset: 0x001D34DB
	Private Sub Kill()
		MyBase.Die()
	End Sub

	' Token: 0x06003259 RID: 12889 RVA: 0x001D50E3 File Offset: 0x001D34E3
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.propellerPrefab = Nothing
	End Sub

	' Token: 0x04003AAF RID: 15023
	<SerializeField()>
	Private propellerPrefab As ForestPlatformingLevelAcornPropeller

	' Token: 0x04003AB0 RID: 15024
	Private Const SCREEN_PADDING As Single = 100F

	' Token: 0x04003AB1 RID: 15025
	Private Const DROP_EASE_TIME As Single = 0.5F

	' Token: 0x04003AB2 RID: 15026
	Private _direction As ForestPlatformingLevelAcorn.Direction

	' Token: 0x04003AB3 RID: 15027
	Private _player As AbstractPlayerController

	' Token: 0x04003AB4 RID: 15028
	Private _hasDropped As Boolean

	' Token: 0x04003AB5 RID: 15029
	Private _enteredScreen As Boolean

	' Token: 0x04003AB6 RID: 15030
	Private parent As ForestPlatformingLevelAcornMaker

	' Token: 0x02000877 RID: 2167
	Public Enum Direction
		' Token: 0x04003AB8 RID: 15032
		Left
		' Token: 0x04003AB9 RID: 15033
		Right
	End Enum
End Class
