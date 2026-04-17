Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000613 RID: 1555
Public Class FlowerLevelVenusSpawn
	Inherits AbstractCollidableObject

	' Token: 0x06001F67 RID: 8039 RVA: 0x001206BC File Offset: 0x0011EABC
	Public Sub OnVenusSpawn(parent As FlowerLevelFlower, hp As Integer, rotSpeed As Single, moveSpeed As Integer, rotDelay As Single)
		AudioManager.Play("flower_venus_a_chomp")
		Me.rotationDelay = rotDelay
		Me.rotationSpeed = rotSpeed
		Me.movementSpeed = moveSpeed
		Me.lockRotation = False
		Me.currentHP = CSng(hp)
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.Die
		MyBase.animator.SetInteger("Variant", Global.UnityEngine.Random.Range(0, 2))
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001F68 RID: 8040 RVA: 0x0012073B File Offset: 0x0011EB3B
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.currentHP -= info.damage
		If Me.currentHP <= 0F Then
			Me.Die()
			Me.damageReceiver.enabled = False
		End If
	End Sub

	' Token: 0x06001F69 RID: 8041 RVA: 0x00120774 File Offset: 0x0011EB74
	Private Iterator Function spawnPetals_cr() As IEnumerator
		Yield New WaitForEndOfFrame()
		Yield CupheadTime.WaitForSeconds(Me, MyBase.animator.GetCurrentAnimatorStateInfo(0).length / 4F)
		Me.SpawnPetals()
		Return
	End Function

	' Token: 0x06001F6A RID: 8042 RVA: 0x00120790 File Offset: 0x0011EB90
	Public Sub SpawnPetals()
		Dim vector As Vector3 = MyBase.transform.position + Vector3.up * CSng((Global.UnityEngine.Random.Range(-10, 10) + 70))
		Dim gameObject As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.petalA, vector, Quaternion.identity)
		gameObject.GetComponent(Of Animator)().Play("Plant_LeafA", Global.UnityEngine.Random.Range(0, 1))
		MyBase.StartCoroutine(Me.fade_cr(gameObject, 0.8F, 125F, False))
		gameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.petalB, vector + Vector3.down * 50F, Quaternion.identity)
		gameObject.GetComponent(Of Animator)().Play("Plant_LeafB")
		MyBase.StartCoroutine(Me.fade_cr(gameObject, 1F, 100F, True))
	End Sub

	' Token: 0x06001F6B RID: 8043 RVA: 0x00120858 File Offset: 0x0011EC58
	Private Iterator Function die_cr() As IEnumerator
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Death", 0, False, True)
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Return
	End Function

	' Token: 0x06001F6C RID: 8044 RVA: 0x00120874 File Offset: 0x0011EC74
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
			Me.StopAllCoroutines()
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
		Return
	End Function

	' Token: 0x06001F6D RID: 8045 RVA: 0x001208AC File Offset: 0x0011ECAC
	Private Iterator Function move_cr() As IEnumerator
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Yield MyBase.animator.WaitForAnimationToEnd(Me, True)
		While True
			If Not Me.lockRotation AndAlso Me.rotationDelay <= 0F Then
				Dim vector As Vector3 = PlayerManager.GetNext().center - MyBase.transform.position
				MyBase.transform.right = Vector3.RotateTowards(MyBase.transform.right, -vector.normalized * MyBase.transform.localScale.x, Me.rotationSpeed * CupheadTime.FixedDelta, 0F)
				If MyBase.transform.localScale.x = 1F Then
					If Vector3.Angle(MyBase.transform.right, -vector.normalized) < 5F Then
						Me.lockRotation = True
					End If
				ElseIf Vector3.Angle(MyBase.transform.right, -vector.normalized) > 175F Then
					Me.lockRotation = True
				End If
			End If
			MyBase.transform.position -= MyBase.transform.right * CSng(Me.movementSpeed) * CupheadTime.FixedDelta * MyBase.transform.localScale.x
			Yield wait
		End While
		Return
	End Function

	' Token: 0x06001F6E RID: 8046 RVA: 0x001208C8 File Offset: 0x0011ECC8
	Private Sub Die()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		MyBase.animator.SetTrigger("OnDeath")
		AudioManager.Play("flower_minion_simple_deathpop")
		Me.emitAudioFromObject.Add("flower_minion_simple_deathpop")
		MyBase.StartCoroutine(Me.die_cr())
		MyBase.StartCoroutine(Me.spawnPetals_cr())
	End Sub

	' Token: 0x06001F6F RID: 8047 RVA: 0x0012092B File Offset: 0x0011ED2B
	Protected Overrides Sub Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x06001F70 RID: 8048 RVA: 0x00120961 File Offset: 0x0011ED61
	Private Sub Update()
		If Me.rotationDelay > 0F Then
			Me.rotationDelay -= CupheadTime.Delta
		End If
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001F71 RID: 8049 RVA: 0x001209A0 File Offset: 0x0011EDA0
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionEnemy(hit, phase)
	End Sub

	' Token: 0x06001F72 RID: 8050 RVA: 0x001209BE File Offset: 0x0011EDBE
	Protected Overrides Sub OnCollisionGround(hit As GameObject, phase As CollisionPhase)
		Me.Die()
		MyBase.OnCollisionGround(hit, phase)
	End Sub

	' Token: 0x06001F73 RID: 8051 RVA: 0x001209CE File Offset: 0x0011EDCE
	Protected Overrides Sub OnCollisionCeiling(hit As GameObject, phase As CollisionPhase)
		MyBase.StartCoroutine(Me.offScreenDeath_cr())
		MyBase.OnCollisionCeiling(hit, phase)
	End Sub

	' Token: 0x06001F74 RID: 8052 RVA: 0x001209E5 File Offset: 0x0011EDE5
	Protected Overrides Sub OnCollisionWalls(hit As GameObject, phase As CollisionPhase)
		MyBase.StartCoroutine(Me.offScreenDeath_cr())
		MyBase.OnCollisionWalls(hit, phase)
	End Sub

	' Token: 0x06001F75 RID: 8053 RVA: 0x001209FC File Offset: 0x0011EDFC
	Protected Overrides Sub OnDestroy()
		RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Die
		Me.StopAllCoroutines()
		MyBase.OnDestroy()
		Me.petalA = Nothing
		Me.petalB = Nothing
	End Sub

	' Token: 0x06001F76 RID: 8054 RVA: 0x00120A30 File Offset: 0x0011EE30
	Private Iterator Function offScreenDeath_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x06001F77 RID: 8055 RVA: 0x00120A4B File Offset: 0x0011EE4B
	Private Sub RandomiseVariant()
		MyBase.animator.SetInteger("Variant", Global.UnityEngine.Random.Range(0, 3))
	End Sub

	' Token: 0x06001F78 RID: 8056 RVA: 0x00120A64 File Offset: 0x0011EE64
	Private Sub VenusGrowEndAudio()
	End Sub

	' Token: 0x04002801 RID: 10241
	<SerializeField()>
	Private petalA As GameObject

	' Token: 0x04002802 RID: 10242
	<SerializeField()>
	Private petalB As GameObject

	' Token: 0x04002803 RID: 10243
	Private lockRotation As Boolean

	' Token: 0x04002804 RID: 10244
	Private rotationSpeed As Single

	' Token: 0x04002805 RID: 10245
	Private movementSpeed As Integer

	' Token: 0x04002806 RID: 10246
	Private rotationDelay As Single

	' Token: 0x04002807 RID: 10247
	Private currentHP As Single

	' Token: 0x04002808 RID: 10248
	Private damageDealer As DamageDealer

	' Token: 0x04002809 RID: 10249
	Private damageReceiver As DamageReceiver

	' Token: 0x0400280A RID: 10250
	Private parent As FlowerLevelFlower
End Class
