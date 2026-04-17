Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000605 RID: 1541
Public Class FlowerLevelChomperSeed
	Inherits AbstractCollidableObject

	' Token: 0x06001EA2 RID: 7842 RVA: 0x0011A15C File Offset: 0x0011855C
	Public Sub OnChomperStart(parent As FlowerLevelFlower, properties As LevelProperties.Flower.EnemyPlants)
		AudioManager.Play("flower_plants_chomper")
		Me.currentHP = CSng(properties.chomperPlantHP)
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.StartDeath
		Me.explosion = MyBase.transform.GetChild(0)
		Dim [integer] As Integer = MyBase.animator.GetInteger("MaxVariants")
		MyBase.animator.SetInteger("Variant", Global.UnityEngine.Random.Range(0, [integer]))
	End Sub

	' Token: 0x06001EA3 RID: 7843 RVA: 0x0011A1D8 File Offset: 0x001185D8
	Private Iterator Function grow_cr() As IEnumerator
		Dim pct As Single = 0.3F
		While pct < 1F
			Me.chomperSprite.transform.localScale = Vector3.one * pct
			pct += CupheadTime.Delta * 6F
			If pct > 1F Then
				pct = 1F
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001EA4 RID: 7844 RVA: 0x0011A1F4 File Offset: 0x001185F4
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.currentHP -= info.damage
		If Me.currentHP <= 0F Then
			AudioManager.[Stop]("flower_plants_chomper")
			If Me.isDead Then
				Return
			End If
			Me.isDead = True
			Me.StopAllCoroutines()
			MyBase.GetComponent(Of BoxCollider2D)().enabled = False
			MyBase.animator.Play("Death")
			MyBase.StartCoroutine(Me.die_cr())
			MyBase.StartCoroutine(Me.spawnPetals_cr())
		End If
	End Sub

	' Token: 0x06001EA5 RID: 7845 RVA: 0x0011A280 File Offset: 0x00118680
	Private Iterator Function spawnPetals_cr() As IEnumerator
		Dim child As Animator = Me.explosion.GetComponent(Of Animator)()
		child.SetInteger("Variant", 0)
		Yield CupheadTime.WaitForSeconds(Me, MyBase.animator.GetCurrentAnimatorStateInfo(0).length / 4F)
		child.Play("Death")
		Yield New WaitForEndOfFrame()
		Dim delay As Single = child.GetCurrentAnimatorStateInfo(0).length
		Yield CupheadTime.WaitForSeconds(Me, delay / 4F)
		Me.SpawnPetals()
		Yield CupheadTime.WaitForSeconds(Me, delay)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06001EA6 RID: 7846 RVA: 0x0011A29C File Offset: 0x0011869C
	Public Sub SpawnPetals()
		Dim vector As Vector3 = MyBase.transform.position + Vector3.up * CSng((Global.UnityEngine.Random.Range(-10, 10) + 70))
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.petalA, vector, Quaternion.identity)
		gameObject.GetComponent(Of Animator)().Play("Plant_LeafA", Global.UnityEngine.Random.Range(0, 1))
		MyBase.StartCoroutine(Me.fade_cr(gameObject, 0.8F, 125F, False))
		gameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.petalB, vector + Vector3.down * 50F, Quaternion.identity)
		gameObject.GetComponent(Of Animator)().Play("Plant_LeafB")
		MyBase.StartCoroutine(Me.fade_cr(gameObject, 1F, 100F, False))
	End Sub

	' Token: 0x06001EA7 RID: 7847 RVA: 0x0011A364 File Offset: 0x00118764
	Private Iterator Function die_cr() As IEnumerator
		Yield New WaitForEndOfFrame()
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Death", 0, False, True)
		Me.explosion.GetComponent(Of Animator)().Play("Death")
		Return
	End Function

	' Token: 0x06001EA8 RID: 7848 RVA: 0x0011A380 File Offset: 0x00118780
	Private Iterator Function fade_cr(petal As GameObject, duration As Single, speed As Single, Optional lastPetal As Boolean = False) As IEnumerator
		Dim petalSprite As SpriteRenderer = petal.GetComponent(Of SpriteRenderer)()
		Dim currentTime As Single = duration
		Dim pct As Single = currentTime / duration
		While pct >= 0F
			Dim c As Color = petalSprite.material.color
			c.a = pct
			petalSprite.material.color = c
			petalSprite.transform.position += Vector3.down * speed * CupheadTime.Delta
			currentTime -= CupheadTime.Delta
			pct = currentTime / duration
			Yield Nothing
		End While
		Global.UnityEngine.[Object].Destroy(petal)
		If lastPetal Then
			Me.Die()
		End If
		Return
	End Function

	' Token: 0x06001EA9 RID: 7849 RVA: 0x0011A3B8 File Offset: 0x001187B8
	Private Sub StartDeath()
		AudioManager.Play("flower_minion_simple_deathpop_low")
		Me.emitAudioFromObject.Add("flower_minion_simple_deathpop_low")
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of BoxCollider2D)().enabled = False
		MyBase.animator.Play("Death")
		MyBase.StartCoroutine(Me.die_cr())
		MyBase.StartCoroutine(Me.spawnPetals_cr())
	End Sub

	' Token: 0x06001EAA RID: 7850 RVA: 0x0011A41B File Offset: 0x0011881B
	Private Sub Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001EAB RID: 7851 RVA: 0x0011A43C File Offset: 0x0011883C
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.transform.localScale = New Vector3(MyBase.transform.localScale.x * CSng(MathUtils.PlusOrMinus()), MyBase.transform.localScale.y, MyBase.transform.localScale.z)
		MyBase.Awake()
	End Sub

	' Token: 0x06001EAC RID: 7852 RVA: 0x0011A4CD File Offset: 0x001188CD
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001EAD RID: 7853 RVA: 0x0011A4E5 File Offset: 0x001188E5
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001EAE RID: 7854 RVA: 0x0011A504 File Offset: 0x00118904
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		If hit.GetComponent(Of FlowerLevelFlowerDamageRegion)() IsNot Nothing Then
			Dim damageInfo As DamageDealer.DamageInfo = New DamageDealer.DamageInfo(1F, DamageDealer.Direction.Neutral, hit.transform.position, DamageDealer.DamageSource.Enemy)
			Me.OnDamageTaken(damageInfo)
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06001EAF RID: 7855 RVA: 0x0011A54E File Offset: 0x0011894E
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.StartDeath
		MyBase.OnDestroy()
	End Sub

	' Token: 0x06001EB0 RID: 7856 RVA: 0x0011A56D File Offset: 0x0011896D
	Private Sub OnDeath()
	End Sub

	' Token: 0x06001EB1 RID: 7857 RVA: 0x0011A56F File Offset: 0x0011896F
	Private Sub SpawnChomper()
		MyBase.animator.Play("Trigger_Plant", 1)
		MyBase.StartCoroutine(Me.grow_cr())
	End Sub

	' Token: 0x06001EB2 RID: 7858 RVA: 0x0011A58F File Offset: 0x0011898F
	Private Sub GroundBurstStartAudio()
		AudioManager.Play("flower_ground_pop")
	End Sub

	' Token: 0x04002774 RID: 10100
	<SerializeField()>
	Private petalA As GameObject

	' Token: 0x04002775 RID: 10101
	<SerializeField()>
	Private petalB As GameObject

	' Token: 0x04002776 RID: 10102
	Private currentHP As Single

	' Token: 0x04002777 RID: 10103
	Private explosion As Transform

	' Token: 0x04002778 RID: 10104
	Private parent As FlowerLevelFlower

	' Token: 0x04002779 RID: 10105
	<SerializeField()>
	Private chomperSprite As SpriteRenderer

	' Token: 0x0400277A RID: 10106
	Private damageDealer As DamageDealer

	' Token: 0x0400277B RID: 10107
	Private damageReceiver As DamageReceiver

	' Token: 0x0400277C RID: 10108
	Private isDead As Boolean
End Class
