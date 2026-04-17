Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000568 RID: 1384
Public Class ClownLevelEnemy
	Inherits AbstractProjectile

	' Token: 0x1700034A RID: 842
	' (get) Token: 0x06001A18 RID: 6680 RVA: 0x000EE911 File Offset: 0x000ECD11
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x06001A19 RID: 6681 RVA: 0x000EE918 File Offset: 0x000ECD18
	Public Function Create(pos As Vector3, targetPosition As Single, HP As Single, properties As LevelProperties.Clown.Swing, parent As ClownLevelClownSwing) As ClownLevelEnemy
		Dim clownLevelEnemy As ClownLevelEnemy = TryCast(MyBase.Create(pos), ClownLevelEnemy)
		clownLevelEnemy.transform.position = pos
		clownLevelEnemy.properties = properties
		clownLevelEnemy.targetPosition = targetPosition
		clownLevelEnemy.HP = HP
		clownLevelEnemy.parent = parent
		Return clownLevelEnemy
	End Function

	' Token: 0x06001A1A RID: 6682 RVA: 0x000EE964 File Offset: 0x000ECD64
	Protected Overrides Sub Start()
		MyBase.Start()
		Dim clownLevelClownSwing As ClownLevelClownSwing = Me.parent
		clownLevelClownSwing.OnDeath = CType([Delegate].Combine(clownLevelClownSwing.OnDeath, AddressOf Me.Die), Action)
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.damageReceiver.enabled = False
		AudioManager.Play("clown_penguin_roll_start")
		Me.emitAudioFromObject.Add("clown_penguin_roll_start")
	End Sub

	' Token: 0x06001A1B RID: 6683 RVA: 0x000EE9E8 File Offset: 0x000ECDE8
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A1C RID: 6684 RVA: 0x000EEA06 File Offset: 0x000ECE06
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001A1D RID: 6685 RVA: 0x000EEA24 File Offset: 0x000ECE24
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.HP -= info.damage
		If Me.HP <= 0F AndAlso Not Me.isDead Then
			Me.isDead = True
			Me.Die()
		End If
	End Sub

	' Token: 0x06001A1E RID: 6686 RVA: 0x000EEA61 File Offset: 0x000ECE61
	Protected Overrides Sub OnCollisionOther(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionOther(hit, phase)
		If hit.GetComponent(Of ClownLevelCoaster)() Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06001A1F RID: 6687 RVA: 0x000EEA81 File Offset: 0x000ECE81
	Private Sub StartMoving()
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001A20 RID: 6688 RVA: 0x000EEA90 File Offset: 0x000ECE90
	Private Iterator Function move_cr() As IEnumerator
		Dim pos As Vector3 = MyBase.transform.position
		Dim target As Single = -640F + Me.targetPosition
		While MyBase.transform.position.x <> target
			pos.x = Mathf.MoveTowards(MyBase.transform.position.x, target, Me.properties.movementSpeed * CupheadTime.Delta)
			MyBase.transform.position = pos
			Yield Nothing
		End While
		MyBase.animator.SetTrigger("Continue")
		AudioManager.Play("clown_penguin_roll_end")
		Me.emitAudioFromObject.Add("clown_penguin_roll_end")
		Me.damageReceiver.enabled = True
		MyBase.StartCoroutine(Me.shoot_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A21 RID: 6689 RVA: 0x000EEAAC File Offset: 0x000ECEAC
	Private Iterator Function shoot_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.initialAttackDelay)
		While True
			MyBase.animator.SetTrigger("OnAttack")
			Yield CupheadTime.WaitForSeconds(Me, Me.properties.attackDelayRange.RandomFloat())
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06001A22 RID: 6690 RVA: 0x000EEAC8 File Offset: 0x000ECEC8
	Private Sub ShootBullet()
		AudioManager.Play("clown_penguin_clap")
		Me.emitAudioFromObject.Add("clown_penguin_clap")
		Dim [next] As AbstractPlayerController = PlayerManager.GetNext()
		Dim vector As Vector3 = [next].transform.position - MyBase.transform.position
		Me.bullet.Create(Me.root.transform.position, MathUtils.DirectionToAngle(vector), Me.properties.bulletSpeed)
	End Sub

	' Token: 0x06001A23 RID: 6691 RVA: 0x000EEB48 File Offset: 0x000ECF48
	Protected Overrides Sub Die()
		AudioManager.Play("clown_penguin_death")
		Me.emitAudioFromObject.Add("clown_penguin_death")
		MyBase.animator.SetTrigger("OnDeath")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.StopAllCoroutines()
		Dim clownLevelClownSwing As ClownLevelClownSwing = Me.parent
		clownLevelClownSwing.OnDeath = CType([Delegate].Remove(clownLevelClownSwing.OnDeath, AddressOf Me.Die), Action)
		MyBase.Die()
	End Sub

	' Token: 0x06001A24 RID: 6692 RVA: 0x000EEBC0 File Offset: 0x000ECFC0
	Private Sub SwitchLayer()
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Projectiles.ToString()
	End Sub

	' Token: 0x06001A25 RID: 6693 RVA: 0x000EEBE7 File Offset: 0x000ECFE7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.bullet = Nothing
	End Sub

	' Token: 0x0400233C RID: 9020
	<SerializeField()>
	Private bullet As BasicProjectile

	' Token: 0x0400233D RID: 9021
	<SerializeField()>
	Private root As Transform

	' Token: 0x0400233E RID: 9022
	Private properties As LevelProperties.Clown.Swing

	' Token: 0x0400233F RID: 9023
	Private parent As ClownLevelClownSwing

	' Token: 0x04002340 RID: 9024
	Private handler As ClownLevelCoasterHandler

	' Token: 0x04002341 RID: 9025
	Private damageReceiver As DamageReceiver

	' Token: 0x04002342 RID: 9026
	Private targetPosition As Single

	' Token: 0x04002343 RID: 9027
	Private HP As Single

	' Token: 0x04002344 RID: 9028
	Private isDead As Boolean
End Class
