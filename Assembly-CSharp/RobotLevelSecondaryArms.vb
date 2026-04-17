Imports System
Imports UnityEngine

' Token: 0x02000781 RID: 1921
Public Class RobotLevelSecondaryArms
	Inherits AbstractCollidableObject

	' Token: 0x170003E9 RID: 1001
	' (get) Token: 0x06002A37 RID: 10807 RVA: 0x0018B387 File Offset: 0x00189787
	' (set) Token: 0x06002A38 RID: 10808 RVA: 0x0018B38F File Offset: 0x0018978F
	Public Property BossAlive As Boolean

	' Token: 0x06002A39 RID: 10809 RVA: 0x0018B398 File Offset: 0x00189798
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.BossAlive = True
		MyBase.Awake()
	End Sub

	' Token: 0x06002A3A RID: 10810 RVA: 0x0018B3B2 File Offset: 0x001897B2
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002A3B RID: 10811 RVA: 0x0018B3CA File Offset: 0x001897CA
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06002A3C RID: 10812 RVA: 0x0018B3E8 File Offset: 0x001897E8
	Public Sub InitHelper(properties As LevelProperties.Robot)
		Me.spawnPoint = MyBase.transform.GetChild(2).transform
		Me.ShootTwicePerCycle = properties.CurrentState.twistyArms.shootTwicePerCycle
		Me.bulletSpeed = properties.CurrentState.twistyArms.bulletSpeed
	End Sub

	' Token: 0x06002A3D RID: 10813 RVA: 0x0018B438 File Offset: 0x00189838
	Private Sub OnTwistyArmsShoot()
		If Me.twistyArmsProjectile IsNot Nothing Then
			AudioManager.Play("robot_arms_hand_shoot")
			Me.emitAudioFromObject.Add("robot_arms_hand_shoot")
			Me.twistyArmsProjectile.Create(Me.spawnPoint.position + Vector3.up * 100F, 90F, Me.bulletSpeed)
			Me.twistyArmsProjectile.Create(Me.spawnPoint.position + Vector3.down * 100F, -90F, Me.bulletSpeed)
		End If
	End Sub

	' Token: 0x06002A3E RID: 10814 RVA: 0x0018B4E8 File Offset: 0x001898E8
	Private Sub SwapAnimations()
		If Me.BossAlive Then
			Dim num As Single = MyBase.animator.GetCurrentAnimatorStateInfo(0).normalizedTime Mod 1F
			If Me.ShootTwicePerCycle AndAlso num < 0.5F Then
				MyBase.animator.Play("Shoot B")
			ElseIf num > 0.5F Then
				MyBase.animator.Play("Shoot A")
			End If
		End If
	End Sub

	' Token: 0x04003311 RID: 13073
	Private ShootTwicePerCycle As Boolean

	' Token: 0x04003312 RID: 13074
	Private bulletSpeed As Single

	' Token: 0x04003313 RID: 13075
	Private spawnPoint As Transform

	' Token: 0x04003314 RID: 13076
	Private damageDealer As DamageDealer

	' Token: 0x04003316 RID: 13078
	<SerializeField()>
	Private twistyArmsProjectile As BasicProjectile
End Class
