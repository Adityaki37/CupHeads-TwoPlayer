Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000848 RID: 2120
Public Class VeggiesLevelCarrotHomingProjectile
	Inherits HomingProjectile

	' Token: 0x17000424 RID: 1060
	' (get) Token: 0x0600310F RID: 12559 RVA: 0x001CCAF2 File Offset: 0x001CAEF2
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 1000F
		End Get
	End Property

	' Token: 0x17000425 RID: 1061
	' (get) Token: 0x06003110 RID: 12560 RVA: 0x001CCAF9 File Offset: 0x001CAEF9
	' (set) Token: 0x06003111 RID: 12561 RVA: 0x001CCB01 File Offset: 0x001CAF01
	Public Property state As VeggiesLevelCarrotHomingProjectile.State

	' Token: 0x06003112 RID: 12562 RVA: 0x001CCB0C File Offset: 0x001CAF0C
	Public Function Create(player As AbstractPlayerController, parent As VeggiesLevelCarrot, pos As Vector2, speed As Single, rotationSpeed As Single, health As Single) As VeggiesLevelCarrotHomingProjectile
		Dim veggiesLevelCarrotHomingProjectile As VeggiesLevelCarrotHomingProjectile = TryCast(MyBase.Create(pos, -90F, speed, speed, rotationSpeed, Me.DestroyLifetime, 0F, player), VeggiesLevelCarrotHomingProjectile)
		veggiesLevelCarrotHomingProjectile.CollisionDeath.OnlyPlayer()
		veggiesLevelCarrotHomingProjectile.DamagesType.OnlyPlayer()
		veggiesLevelCarrotHomingProjectile.Init(parent, health)
		Return veggiesLevelCarrotHomingProjectile
	End Function

	' Token: 0x06003113 RID: 12563 RVA: 0x001CCB5E File Offset: 0x001CAF5E
	Private Sub LateUpdate()
		Me.UpdateHitBox()
	End Sub

	' Token: 0x06003114 RID: 12564 RVA: 0x001CCB66 File Offset: 0x001CAF66
	Private Sub UpdateHitBox()
		Me.hitBox.position = MyBase.transform.position
	End Sub

	' Token: 0x06003115 RID: 12565 RVA: 0x001CCB7E File Offset: 0x001CAF7E
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.OnDeath
	End Sub

	' Token: 0x06003116 RID: 12566 RVA: 0x001CCB9D File Offset: 0x001CAF9D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06003117 RID: 12567 RVA: 0x001CCBA7 File Offset: 0x001CAFA7
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionGround(hit, phase)
		Me.Die()
	End Sub

	' Token: 0x06003118 RID: 12568 RVA: 0x001CCBB8 File Offset: 0x001CAFB8
	Protected Overrides Sub Die()
		If Not MyBase.GetComponent(Of Collider2D)().enabled Then
			Return
		End If
		MyBase.Die()
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.hitBox.gameObject.SetActive(False)
		Me.StopAllCoroutines()
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(-90F))
	End Sub

	' Token: 0x06003119 RID: 12569 RVA: 0x001CCC38 File Offset: 0x001CB038
	Private Sub Init(parent As VeggiesLevelCarrot, health As Single)
		AddHandler Me.hitBox.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.parent = parent
		Me.health = health
		AddHandler parent.OnDeathEvent, AddressOf Me.OnDeath
		MyBase.transform.localScale = Vector3.one * (1F + Global.UnityEngine.Random.Range(0.1F, -0.1F))
	End Sub

	' Token: 0x0600311A RID: 12570 RVA: 0x001CCCAC File Offset: 0x001CB0AC
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health <= 0F AndAlso Me.state <> VeggiesLevelCarrotHomingProjectile.State.Dead Then
			Me.state = VeggiesLevelCarrotHomingProjectile.State.Dead
			AudioManager.Play("level_veggies_carrot_projectile_death")
			Me.emitAudioFromObject.Add("level_veggies_carrot_projectile_death")
			MyBase.StartCoroutine(Me.dying_cr())
		End If
	End Sub

	' Token: 0x0600311B RID: 12571 RVA: 0x001CCD18 File Offset: 0x001CB118
	Private Iterator Function dying_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.1F)
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x0600311C RID: 12572 RVA: 0x001CCD33 File Offset: 0x001CB133
	Private Sub OnDeath()
		Me.hitBox.gameObject.SetActive(False)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.parentDied_cr())
	End Sub

	' Token: 0x0600311D RID: 12573 RVA: 0x001CCD5F File Offset: 0x001CB15F
	Private Sub [End]()
		Me.Die()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x0600311E RID: 12574 RVA: 0x001CCD70 File Offset: 0x001CB170
	Private Iterator Function parentDied_cr() As IEnumerator
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0F, 0.5F))
		Me.[End]()
		Return
	End Function

	' Token: 0x040039B2 RID: 14770
	Public Const SCALE_RAND As Single = 0.1F

	' Token: 0x040039B4 RID: 14772
	<SerializeField()>
	Private hitBox As Transform

	' Token: 0x040039B5 RID: 14773
	Private parent As VeggiesLevelCarrot

	' Token: 0x040039B6 RID: 14774
	Private health As Single

	' Token: 0x02000849 RID: 2121
	Public Enum State
		' Token: 0x040039B8 RID: 14776
		[In]
		' Token: 0x040039B9 RID: 14777
		InComplete
		' Token: 0x040039BA RID: 14778
		Firing
		' Token: 0x040039BB RID: 14779
		Dead
	End Enum
End Class
