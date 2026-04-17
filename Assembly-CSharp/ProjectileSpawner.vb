Imports System
Imports UnityEngine

' Token: 0x02000AF7 RID: 2807
Public Class ProjectileSpawner
	Inherits AbstractPausableComponent

	' Token: 0x0600440C RID: 17420 RVA: 0x00240A38 File Offset: 0x0023EE38
	Private Sub Start()
		AddHandler Level.Current.OnLevelStartEvent, AddressOf Me.OnStart
		Me.aim = New GameObject("Aim").transform
		Me.aim.SetParent(MyBase.transform)
		Me.aim.ResetLocalTransforms()
	End Sub

	' Token: 0x0600440D RID: 17421 RVA: 0x00240A8C File Offset: 0x0023EE8C
	Public Sub OnStart()
		Me.started = True
	End Sub

	' Token: 0x0600440E RID: 17422 RVA: 0x00240A95 File Offset: 0x0023EE95
	Public Sub OnStop()
		Me.started = False
	End Sub

	' Token: 0x0600440F RID: 17423 RVA: 0x00240AA0 File Offset: 0x0023EEA0
	Private Sub Update()
		If Not Me.started Then
			Return
		End If
		If Level.Current Is Nothing OrElse PlayerManager.Count < 1 Then
			Return
		End If
		If Me.projectilePrefab Is Nothing Then
			Return
		End If
		If Me.timer >= Me.delay Then
			If Me.type = ProjectileSpawner.Type.Aimed Then
				Me.aim.LookAt2D(PlayerManager.GetNext().transform)
				Me.angle = Me.aim.transform.eulerAngles.z
			End If
			Dim basicProjectile As BasicProjectile = Me.projectilePrefab.Create(MyBase.transform.position, Me.angle, Me.speed)
			If Me.parryable Then
				basicProjectile.SetParryable(Me.parryable)
			End If
			If Me.stoneTime > 0F Then
				basicProjectile.SetStoneTime(Me.stoneTime)
			End If
			Me.timer = 0F
		Else
			Me.timer += CupheadTime.Delta
		End If
	End Sub

	' Token: 0x040049AB RID: 18859
	Public type As ProjectileSpawner.Type

	' Token: 0x040049AC RID: 18860
	Public delay As Single = 1F

	' Token: 0x040049AD RID: 18861
	Public speed As Single = 500F

	' Token: 0x040049AE RID: 18862
	Public parryable As Boolean

	' Token: 0x040049AF RID: 18863
	<Space(10F)>
	Public stoneTime As Single

	' Token: 0x040049B0 RID: 18864
	<Space(10F)>
	<SerializeField()>
	Private projectilePrefab As BasicProjectile

	' Token: 0x040049B1 RID: 18865
	Public angle As Single

	' Token: 0x040049B2 RID: 18866
	Private timer As Single

	' Token: 0x040049B3 RID: 18867
	Private started As Boolean

	' Token: 0x040049B4 RID: 18868
	Private aim As Transform

	' Token: 0x02000AF8 RID: 2808
	Public Enum Type
		' Token: 0x040049B6 RID: 18870
		Straight
		' Token: 0x040049B7 RID: 18871
		Aimed
	End Enum
End Class
