Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200062E RID: 1582
Public Class FlyingBlimpLevelArrowProjectile
	Inherits HomingProjectile

	' Token: 0x06002034 RID: 8244 RVA: 0x00128158 File Offset: 0x00126558
	Public Function Create(pos As Vector2, startRotation As Single, startSpeed As Single, speed As Single, rotation As Single, timeBeforeDeath As Single, timeBeforeHoming As Single, player As AbstractPlayerController, hp As Single) As FlyingBlimpLevelArrowProjectile
		Dim flyingBlimpLevelArrowProjectile As FlyingBlimpLevelArrowProjectile = TryCast(MyBase.Create(pos, startRotation, startSpeed, speed, rotation, timeBeforeDeath, timeBeforeHoming, player), FlyingBlimpLevelArrowProjectile)
		flyingBlimpLevelArrowProjectile.CollisionDeath.OnlyPlayer()
		flyingBlimpLevelArrowProjectile.DamagesType.OnlyPlayer()
		flyingBlimpLevelArrowProjectile.health = hp
		flyingBlimpLevelArrowProjectile.timeToDeath = timeBeforeDeath
		flyingBlimpLevelArrowProjectile.speed = speed
		Return flyingBlimpLevelArrowProjectile
	End Function

	' Token: 0x06002035 RID: 8245 RVA: 0x001281AE File Offset: 0x001265AE
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.trail_cr())
	End Sub

	' Token: 0x06002036 RID: 8246 RVA: 0x001281E6 File Offset: 0x001265E6
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x06002037 RID: 8247 RVA: 0x001281FC File Offset: 0x001265FC
	Private Iterator Function trail_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.1F)
			Dim trail As Effect = Global.UnityEngine.[Object].Instantiate(Of Effect)(Me.trailPrefab)
			trail.transform.position = MyBase.transform.position
			trail.GetComponent(Of Animator)().SetInteger("PickAni", Global.UnityEngine.Random.Range(0, 3))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06002038 RID: 8248 RVA: 0x00128217 File Offset: 0x00126617
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06002039 RID: 8249 RVA: 0x00128244 File Offset: 0x00126644
	Protected Overrides Sub Die()
		MyBase.animator.SetTrigger("dead")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(-90F))
		MyBase.Die()
	End Sub

	' Token: 0x0600203A RID: 8250 RVA: 0x001282A2 File Offset: 0x001266A2
	Private Sub Destroy()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0600203B RID: 8251 RVA: 0x001282B0 File Offset: 0x001266B0
	Private Iterator Function timer_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.timeToDeath)
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		MyBase.HomingEnabled = False
		While True
			MyBase.transform.position += MyBase.transform.right * Me.speed * CupheadTime.FixedDelta
			Yield wait
		End While
		Return
	End Function

	' Token: 0x040028B4 RID: 10420
	<SerializeField()>
	Private trailPrefab As Effect

	' Token: 0x040028B5 RID: 10421
	Private speed As Single

	' Token: 0x040028B6 RID: 10422
	Private health As Single

	' Token: 0x040028B7 RID: 10423
	Private timeToDeath As Single

	' Token: 0x040028B8 RID: 10424
	Private damageReceiver As DamageReceiver
End Class
