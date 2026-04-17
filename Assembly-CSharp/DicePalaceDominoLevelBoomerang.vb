Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005B2 RID: 1458
Public Class DicePalaceDominoLevelBoomerang
	Inherits AbstractProjectile

	' Token: 0x06001C3D RID: 7229 RVA: 0x00102E1C File Offset: 0x0010121C
	Public Function Create(pos As Vector2, speed As Single, hp As Single) As DicePalaceDominoLevelBoomerang
		Dim dicePalaceDominoLevelBoomerang As DicePalaceDominoLevelBoomerang = TryCast(MyBase.Create(pos), DicePalaceDominoLevelBoomerang)
		dicePalaceDominoLevelBoomerang.speed = speed
		dicePalaceDominoLevelBoomerang.HP = hp
		Return dicePalaceDominoLevelBoomerang
	End Function

	' Token: 0x06001C3E RID: 7230 RVA: 0x00102E45 File Offset: 0x00101245
	Protected Overrides Sub Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.Start()
	End Sub

	' Token: 0x06001C3F RID: 7231 RVA: 0x00102E7D File Offset: 0x0010127D
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.HP -= info.damage
		If Me.HP <= 0F AndAlso Not Me.isDead Then
			Me.isDead = True
			Me.Killed()
		End If
	End Sub

	' Token: 0x06001C40 RID: 7232 RVA: 0x00102EBA File Offset: 0x001012BA
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001C41 RID: 7233 RVA: 0x00102ED8 File Offset: 0x001012D8
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001C42 RID: 7234 RVA: 0x00102EF8 File Offset: 0x001012F8
	Private Iterator Function move_cr() As IEnumerator
		Dim dropPoint As Single = CSng(Level.Current.Ground) + MyBase.GetComponent(Of Collider2D)().bounds.size.y
		Dim goToPos As Single = -440F
		While MyBase.transform.position.x > goToPos
			MyBase.transform.position += Vector3.left * Me.speed * CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("OnDrop")
		Yield MyBase.animator.WaitForAnimationToStart(Me, "Fly_Drop_Start", False)
		AudioManager.Play("dice_palace_domino_bird_dive")
		Me.emitAudioFromObject.Add("dice_palace_domino_bird_dive")
		While MyBase.transform.position.y > dropPoint
			MyBase.transform.position += Vector3.down * Me.speed * CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("OnStop")
		Yield Nothing
		Return
	End Function

	' Token: 0x06001C43 RID: 7235 RVA: 0x00102F13 File Offset: 0x00101313
	Private Sub ChangeDirection()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.fly_right_cr())
	End Sub

	' Token: 0x06001C44 RID: 7236 RVA: 0x00102F28 File Offset: 0x00101328
	Private Iterator Function fly_right_cr() As IEnumerator
		While MyBase.transform.position.x < 840F
			MyBase.transform.position += Vector3.right * Me.speed * CupheadTime.Delta
			Yield Nothing
		End While
		Me.Die()
		Yield Nothing
		Return
	End Function

	' Token: 0x06001C45 RID: 7237 RVA: 0x00102F43 File Offset: 0x00101343
	Private Sub WingFlapSFX()
		AudioManager.Play("bird_bird_flap")
		Me.emitAudioFromObject.Add("bird_bird_flap")
	End Sub

	' Token: 0x06001C46 RID: 7238 RVA: 0x00102F60 File Offset: 0x00101360
	Private Sub Killed()
		Me.StopAllCoroutines()
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Me.Die()
		AudioManager.Play("dice_bird_die")
		Me.emitAudioFromObject.Add("dice_bird_die")
		MyBase.animator.SetTrigger("OnDeath")
	End Sub

	' Token: 0x06001C47 RID: 7239 RVA: 0x00102FAF File Offset: 0x001013AF
	Private Sub SpawnEffect()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.deathPoof.Create(MyBase.transform.position)
	End Sub

	' Token: 0x06001C48 RID: 7240 RVA: 0x00102FD4 File Offset: 0x001013D4
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.deathPoof = Nothing
	End Sub

	' Token: 0x04002545 RID: 9541
	<SerializeField()>
	Private deathPoof As Effect

	' Token: 0x04002546 RID: 9542
	Private damageReceiver As DamageReceiver

	' Token: 0x04002547 RID: 9543
	Private speed As Single

	' Token: 0x04002548 RID: 9544
	Private HP As Single

	' Token: 0x04002549 RID: 9545
	Private isDead As Boolean
End Class
