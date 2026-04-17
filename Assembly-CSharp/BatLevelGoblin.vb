Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000506 RID: 1286
Public Class BatLevelGoblin
	Inherits AbstractCollidableObject

	' Token: 0x060016C7 RID: 5831 RVA: 0x000CCDB3 File Offset: 0x000CB1B3
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060016C8 RID: 5832 RVA: 0x000CCDE9 File Offset: 0x000CB1E9
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x060016C9 RID: 5833 RVA: 0x000CCE14 File Offset: 0x000CB214
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060016CA RID: 5834 RVA: 0x000CCE2C File Offset: 0x000CB22C
	Public Sub Init(properties As LevelProperties.Bat.Goblins, pos As Vector2, onLeft As Boolean, isShooter As Boolean, health As Single)
		MyBase.transform.position = pos
		Me.onLeft = onLeft
		Me.isShooter = isShooter
		Me.properties = properties
		Me.health = health
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x060016CB RID: 5835 RVA: 0x000CCE6A File Offset: 0x000CB26A
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060016CC RID: 5836 RVA: 0x000CCE84 File Offset: 0x000CB284
	Private Iterator Function move_cr() As IEnumerator
		Dim endpos As Single = CSng(If((Not Me.onLeft), (-640), 640))
		Dim t As Single = 0F
		While MyBase.transform.position.x <> endpos
			Dim pos As Vector3 = MyBase.transform.position
			pos.x = Mathf.MoveTowards(MyBase.transform.position.x, endpos, Me.properties.runSpeed * CupheadTime.Delta)
			MyBase.transform.position = pos
			If Me.isShooter Then
				t += CupheadTime.Delta
				If t >= Me.properties.timeBeforeShoot Then
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.initalShotDelay)
					Me.ShootBullet()
					Yield CupheadTime.WaitForSeconds(Me, Me.properties.shooterHold)
					Me.isShooter = False
				End If
			End If
			Yield Nothing
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x060016CD RID: 5837 RVA: 0x000CCEA0 File Offset: 0x000CB2A0
	Private Sub ShootBullet()
		Dim num As Single
		If Me.onLeft Then
			num = 15469.86F
		Else
			num = -5156.62F
		End If
		Dim num2 As Single = Mathf.Atan2(MyBase.transform.position.y, num) * 57.29578F
		Me.projectile.Create(MyBase.transform.position, num2, Me.properties.bulletSpeed)
	End Sub

	' Token: 0x060016CE RID: 5838 RVA: 0x000CCF17 File Offset: 0x000CB317
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04002014 RID: 8212
	<SerializeField()>
	Private projectile As BasicProjectile

	' Token: 0x04002015 RID: 8213
	Private properties As LevelProperties.Bat.Goblins

	' Token: 0x04002016 RID: 8214
	Private onLeft As Boolean

	' Token: 0x04002017 RID: 8215
	Private isShooter As Boolean

	' Token: 0x04002018 RID: 8216
	Private health As Single

	' Token: 0x04002019 RID: 8217
	Private damageDealer As DamageDealer

	' Token: 0x0400201A RID: 8218
	Private damageReceiver As DamageReceiver
End Class
