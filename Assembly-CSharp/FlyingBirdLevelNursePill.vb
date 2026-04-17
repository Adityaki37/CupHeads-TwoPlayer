Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000622 RID: 1570
Public Class FlyingBirdLevelNursePill
	Inherits AbstractProjectile

	' Token: 0x06001FF0 RID: 8176 RVA: 0x00125600 File Offset: 0x00123A00
	Protected Overrides Sub FixedUpdate()
		If Me.gravity Then
			If Me.velocity.magnitude < Me.properties.pillSpeed Then
			End If
			Me.velocity.y = Me.velocity.y - 10F
		End If
		MyBase.FixedUpdate()
	End Sub

	' Token: 0x06001FF1 RID: 8177 RVA: 0x00125650 File Offset: 0x00123A50
	Public Sub InitPill(properties As LevelProperties.FlyingBird.Nurses, target As PlayerId, parryable As Boolean)
		Me.SetParryable(parryable)
		Me.target = target
		Me.properties = properties
		Me.velocity = MyBase.transform.up.normalized * properties.pillSpeed
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001FF2 RID: 8178 RVA: 0x001256A3 File Offset: 0x00123AA3
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		If parryable Then
			Me.parryPill.SetActive(True)
		Else
			Me.normalPill.SetActive(True)
		End If
	End Sub

	' Token: 0x06001FF3 RID: 8179 RVA: 0x001256CF File Offset: 0x00123ACF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001FF4 RID: 8180 RVA: 0x001256F0 File Offset: 0x00123AF0
	Private Iterator Function move_cr() As IEnumerator
		While True
			MyBase.transform.position += Me.velocity * CupheadTime.Delta
			If MyBase.transform.position.y >= Me.properties.pillMaxHeight AndAlso Not Me.gravity Then
				Me.gravity = True
				MyBase.StartCoroutine(Me.detonate_cr())
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001FF5 RID: 8181 RVA: 0x0012570C File Offset: 0x00123B0C
	Private Iterator Function detonate_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.pillExplodeDelay)
		Dim player As AbstractPlayerController = PlayerManager.GetPlayer(Me.target)
		If player Is Nothing OrElse player.IsDead Then
			player = PlayerManager.GetNext()
		End If
		MyBase.transform.right = (player.center - MyBase.transform.position).normalized
		Dim top As FlyingBirdLevelNursePillProjectile = TryCast(Me.topHalf.Create(MyBase.transform.position, MyBase.transform.eulerAngles.z, Me.properties.bulletSpeed), FlyingBirdLevelNursePillProjectile)
		Dim bottom As FlyingBirdLevelNursePillProjectile = TryCast(Me.bottomHalf.Create(MyBase.transform.position, MyBase.transform.eulerAngles.z + 180F, Me.properties.bulletSpeed), FlyingBirdLevelNursePillProjectile)
		If MyBase.CanParry Then
			top.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.LightPink)
			top.SetParryable(True)
			bottom.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.DarkPink)
			bottom.SetParryable(True)
		Else
			top.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.Yellow)
			bottom.SetPillColor(FlyingBirdLevelNursePillProjectile.PillColor.Blue)
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x0400286B RID: 10347
	<SerializeField()>
	Private topHalf As FlyingBirdLevelNursePillProjectile

	' Token: 0x0400286C RID: 10348
	<SerializeField()>
	Private bottomHalf As FlyingBirdLevelNursePillProjectile

	' Token: 0x0400286D RID: 10349
	<SerializeField()>
	Private normalPill As GameObject

	' Token: 0x0400286E RID: 10350
	<SerializeField()>
	Private parryPill As GameObject

	' Token: 0x0400286F RID: 10351
	Private gravity As Boolean

	' Token: 0x04002870 RID: 10352
	Private velocity As Vector3

	' Token: 0x04002871 RID: 10353
	Private target As PlayerId

	' Token: 0x04002872 RID: 10354
	Private properties As LevelProperties.FlyingBird.Nurses
End Class
