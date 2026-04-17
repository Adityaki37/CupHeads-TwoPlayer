Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200067D RID: 1661
Public Class FlyingGenieLevelSword
	Inherits AbstractProjectile

	' Token: 0x0600230A RID: 8970 RVA: 0x00148D10 File Offset: 0x00147110
	Public Sub Init(startPos As Vector3, endPos As Vector3, properties As LevelProperties.FlyingGenie.Swords, player As AbstractPlayerController)
		Me.startPos = startPos
		MyBase.transform.position = startPos
		Me.properties = properties
		Me.endPos = endPos
		Me.player = player
		MyBase.StartCoroutine(Me.move_to_pos_cr())
		AudioManager.Play("genie_chest_sword_spawn")
		Me.emitAudioFromObject.Add("genie_chest_sword_spawn")
	End Sub

	' Token: 0x0600230B RID: 8971 RVA: 0x00148D6D File Offset: 0x0014716D
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x0600230C RID: 8972 RVA: 0x00148D96 File Offset: 0x00147196
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x0600230D RID: 8973 RVA: 0x00148DB4 File Offset: 0x001471B4
	Private Iterator Function move_to_pos_cr() As IEnumerator
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, 90F)
		While MyBase.transform.position.y < (Me.startPos + Vector3.up * Me.outOfChestY).y
			MyBase.transform.AddPosition(0F, Me.outOfChestY * Me.outOfChestSpeed * CupheadTime.Delta, 0F)
			Yield Nothing
		End While
		Me.swordRenderer.sortingLayerName = "Projectiles"
		Me.swordRenderer.sortingOrder = 2
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, MathUtils.DirectionToAngle(Me.endPos - MyBase.transform.position))
		While MyBase.transform.position <> Me.endPos
			MyBase.transform.position = Vector3.MoveTowards(MyBase.transform.position, Me.endPos, Me.properties.swordSpeed * CupheadTime.Delta)
			Yield Nothing
		End While
		Dim t As Single = 0F
		While t < Me.properties.attackDelay
			MyBase.transform.Rotate(Vector3.forward, Me.swordRotationSpeed * CupheadTime.Delta)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.StartCoroutine(Me.move_continue_cr())
		AudioManager.Play("genie_chest_sword_attack")
		Me.emitAudioFromObject.Add("genie_chest_sword_attack")
		Yield Nothing
		Return
	End Function

	' Token: 0x0600230E RID: 8974 RVA: 0x00148DD0 File Offset: 0x001471D0
	Private Iterator Function move_continue_cr() As IEnumerator
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, 35F)
		MyBase.animator.SetTrigger("Spin")
		Yield CupheadTime.WaitForSeconds(Me, Me.fastSpinTime)
		MyBase.animator.SetTrigger("Attack")
		If Me.player Is Nothing OrElse Me.player.IsDead Then
			Me.player = PlayerManager.GetNext()
		End If
		Dim direction As Vector3 = Me.player.transform.position - MyBase.transform.position
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(MathUtils.DirectionToAngle(direction)))
		While True
			MyBase.transform.position += MyBase.transform.right * Me.properties.swordSpeed * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x0600230F RID: 8975 RVA: 0x00148DEB File Offset: 0x001471EB
	Public Overrides Sub SetParryable(parryable As Boolean)
		MyBase.SetParryable(parryable)
		MyBase.animator.SetFloat("Pink", If((Not parryable), 0F, 1F))
	End Sub

	' Token: 0x04002BA3 RID: 11171
	Private Const PinkParameterName As String = "Pink"

	' Token: 0x04002BA4 RID: 11172
	Private Const SpinParameterName As String = "Spin"

	' Token: 0x04002BA5 RID: 11173
	Private Const AttackParameterName As String = "Attack"

	' Token: 0x04002BA6 RID: 11174
	Private Const ProjectilesLayer As String = "Projectiles"

	' Token: 0x04002BA7 RID: 11175
	Private Const spinRotationOffset As Single = 35F

	' Token: 0x04002BA8 RID: 11176
	<SerializeField()>
	Private outOfChestY As Single

	' Token: 0x04002BA9 RID: 11177
	<SerializeField()>
	Private outOfChestSpeed As Single

	' Token: 0x04002BAA RID: 11178
	<SerializeField()>
	Private swordRotationSpeed As Single

	' Token: 0x04002BAB RID: 11179
	<SerializeField()>
	Private fastSpinTime As Single

	' Token: 0x04002BAC RID: 11180
	<SerializeField()>
	Private swordRenderer As SpriteRenderer

	' Token: 0x04002BAD RID: 11181
	Private properties As LevelProperties.FlyingGenie.Swords

	' Token: 0x04002BAE RID: 11182
	Private player As AbstractPlayerController

	' Token: 0x04002BAF RID: 11183
	Private endPos As Vector3

	' Token: 0x04002BB0 RID: 11184
	Private startPos As Vector3
End Class
