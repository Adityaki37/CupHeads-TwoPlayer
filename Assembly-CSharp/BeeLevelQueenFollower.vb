Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200051D RID: 1309
Public Class BeeLevelQueenFollower
	Inherits AbstractProjectile

	' Token: 0x17000328 RID: 808
	' (get) Token: 0x06001766 RID: 5990 RVA: 0x000D2BCD File Offset: 0x000D0FCD
	Public Overrides ReadOnly Property ParryMeterMultiplier As Single
		Get
			Return 0.25F
		End Get
	End Property

	' Token: 0x06001767 RID: 5991 RVA: 0x000D2BD4 File Offset: 0x000D0FD4
	Public Function Create(pos As Vector2, properties As BeeLevelQueenFollower.Properties) As BeeLevelQueenFollower
		Dim beeLevelQueenFollower As BeeLevelQueenFollower = TryCast(MyBase.Create(), BeeLevelQueenFollower)
		beeLevelQueenFollower.transform.position = pos
		beeLevelQueenFollower.properties = properties
		Return beeLevelQueenFollower
	End Function

	' Token: 0x17000329 RID: 809
	' (get) Token: 0x06001768 RID: 5992 RVA: 0x000D2C06 File Offset: 0x000D1006
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 300F
		End Get
	End Property

	' Token: 0x06001769 RID: 5993 RVA: 0x000D2C10 File Offset: 0x000D1010
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.circleCollider = MyBase.GetComponent(Of CircleCollider2D)()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AudioManager.PlayLoop("bee_queen_follower_loop")
		Me.emitAudioFromObject.Add("bee_queen_follower_loop")
		MyBase.StartCoroutine(Me.check_pos_cr())
	End Sub

	' Token: 0x0600176A RID: 5994 RVA: 0x000D2C7C File Offset: 0x000D107C
	Private Iterator Function check_pos_cr() As IEnumerator
		Dim offset As Single = 175F
		While MyBase.transform.position.y < CSng(Level.Current.Ceiling) + offset AndAlso MyBase.transform.position.y > CSng(Level.Current.Ground) - offset AndAlso MyBase.transform.position.x > CSng(Level.Current.Left) - offset AndAlso MyBase.transform.position.x < CSng(Level.Current.Right) + offset
			Yield Nothing
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x0600176B RID: 5995 RVA: 0x000D2C97 File Offset: 0x000D1097
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.childPrefab = Nothing
	End Sub

	' Token: 0x0600176C RID: 5996 RVA: 0x000D2CBD File Offset: 0x000D10BD
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.attacking AndAlso Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x0600176D RID: 5997 RVA: 0x000D2CF1 File Offset: 0x000D10F1
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.properties.health -= info.damage
		If Me.properties.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x0600176E RID: 5998 RVA: 0x000D2D26 File Offset: 0x000D1126
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.properties.parryable Then
			Me.SetParryable(True)
		End If
		MyBase.StartCoroutine(Me.go_cr())
	End Sub

	' Token: 0x0600176F RID: 5999 RVA: 0x000D2D54 File Offset: 0x000D1154
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.aim Is Nothing OrElse Me.properties.player Is Nothing OrElse MyBase.dead Then
			Return
		End If
		MyBase.transform.position += MyBase.transform.right * (Me.properties.speed * CupheadTime.Delta)
		Me.aim.LookAt2D(Me.properties.player.center)
		If Me.rotate Then
			MyBase.transform.rotation = Quaternion.Slerp(MyBase.transform.rotation, Me.aim.rotation, Me.properties.rotationSpeed * CupheadTime.Delta)
		End If
	End Sub

	' Token: 0x06001770 RID: 6000 RVA: 0x000D2E38 File Offset: 0x000D1238
	Protected Overrides Sub Die()
		MyBase.Die()
		AudioManager.[Stop]("bee_queen_follower_loop")
		Me.circleCollider.enabled = False
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x06001771 RID: 6001 RVA: 0x000D2E5C File Offset: 0x000D125C
	Private Iterator Function go_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.introTime)
		MyBase.animator.SetTrigger("Continue")
		Me.attacking = True
		Me.aim = New GameObject("Aim").transform
		Me.aim.SetParent(MyBase.transform)
		Me.aim.ResetLocalTransforms()
		Dim t As Single = 0F
		While t < 2F
			Dim val As Single = t / 2F
			Me.properties.speed = Mathf.Lerp(0F, Me.properties.speedMax, val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.properties.speed = Me.properties.speedMax
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.homingTime)
		Me.rotate = False
		Return
	End Function

	' Token: 0x06001772 RID: 6002 RVA: 0x000D2E78 File Offset: 0x000D1278
	Private Iterator Function children_cr() As IEnumerator
		While True
			Me.childPrefab.Create(MyBase.transform.position, CSng(Global.UnityEngine.Random.Range(0, 360)), 0F, Me.properties.childHealth)
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.childDelay)
		End While
		Return
	End Function

	' Token: 0x06001773 RID: 6003 RVA: 0x000D2E93 File Offset: 0x000D1293
	Public Overrides Sub OnParry(player As AbstractPlayerController)
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x06001774 RID: 6004 RVA: 0x000D2EA2 File Offset: 0x000D12A2
	Public Overrides Sub OnParryDie()
	End Sub

	' Token: 0x06001775 RID: 6005 RVA: 0x000D2EA4 File Offset: 0x000D12A4
	Private Iterator Function timer_cr() As IEnumerator
		Me.SetParryable(False)
		Dim t As Single = 0F
		While t < Me.coolDown
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.SetParryable(True)
		Return
	End Function

	' Token: 0x040020A0 RID: 8352
	Public coolDown As Single = 0.4F

	' Token: 0x040020A1 RID: 8353
	<SerializeField()>
	Private childPrefab As BasicDamagableProjectile

	' Token: 0x040020A2 RID: 8354
	Private properties As BeeLevelQueenFollower.Properties

	' Token: 0x040020A3 RID: 8355
	Private aim As Transform

	' Token: 0x040020A4 RID: 8356
	Private circleCollider As CircleCollider2D

	' Token: 0x040020A5 RID: 8357
	Private damageReceiver As DamageReceiver

	' Token: 0x040020A6 RID: 8358
	Private attacking As Boolean

	' Token: 0x040020A7 RID: 8359
	Private rotate As Boolean = True

	' Token: 0x0200051E RID: 1310
	Public Class Properties
		' Token: 0x06001776 RID: 6006 RVA: 0x000D2EC0 File Offset: 0x000D12C0
		Public Sub New(player As AbstractPlayerController, introTime As Single, speed As Single, rotationSpeed As Single, homingTime As Single, health As Single, childDelay As Single, childHealth As Single, parryable As Boolean)
			Me.player = player
			Me.introTime = introTime
			Me.speedMax = speed
			Me.rotationSpeed = rotationSpeed
			Me.homingTime = homingTime
			Me.healthMax = health
			Me.childDelay = childDelay
			Me.childHealth = childHealth
			Me.speed = 0F
			Me.health = health
			Me.parryable = parryable
		End Sub

		' Token: 0x040020A8 RID: 8360
		Public player As AbstractPlayerController

		' Token: 0x040020A9 RID: 8361
		Public introTime As Single

		' Token: 0x040020AA RID: 8362
		Public speedMax As Single

		' Token: 0x040020AB RID: 8363
		Public rotationSpeed As Single

		' Token: 0x040020AC RID: 8364
		Public homingTime As Single

		' Token: 0x040020AD RID: 8365
		Public healthMax As Single

		' Token: 0x040020AE RID: 8366
		Public childDelay As Single

		' Token: 0x040020AF RID: 8367
		Public childHealth As Single

		' Token: 0x040020B0 RID: 8368
		Public parryable As Boolean

		' Token: 0x040020B1 RID: 8369
		Public speed As Single

		' Token: 0x040020B2 RID: 8370
		Public health As Single
	End Class
End Class
