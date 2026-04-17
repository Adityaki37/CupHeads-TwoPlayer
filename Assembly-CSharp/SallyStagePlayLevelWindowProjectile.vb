Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007BD RID: 1981
Public Class SallyStagePlayLevelWindowProjectile
	Inherits AbstractProjectile

	' Token: 0x06002CD5 RID: 11477 RVA: 0x001A6AA0 File Offset: 0x001A4EA0
	Public Function Create(pos As Vector2, rotation As Single, speed As Single, parent As SallyStagePlayLevel) As SallyStagePlayLevelWindowProjectile
		Dim sallyStagePlayLevelWindowProjectile As SallyStagePlayLevelWindowProjectile = TryCast(MyBase.Create(), SallyStagePlayLevelWindowProjectile)
		sallyStagePlayLevelWindowProjectile.transform.position = pos
		sallyStagePlayLevelWindowProjectile.rotation = rotation
		sallyStagePlayLevelWindowProjectile.speed = speed
		Return sallyStagePlayLevelWindowProjectile
	End Function

	' Token: 0x06002CD6 RID: 11478 RVA: 0x001A6AD9 File Offset: 0x001A4ED9
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002CD7 RID: 11479 RVA: 0x001A6AF7 File Offset: 0x001A4EF7
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002CD8 RID: 11480 RVA: 0x001A6B18 File Offset: 0x001A4F18
	Private Iterator Function move_cr() As IEnumerator
		Me.move = True
		Dim dir As Vector3 = MathUtils.AngleToDirection(Me.rotation)
		While Me.move
			MyBase.transform.position += dir * Me.speed * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06002CD9 RID: 11481 RVA: 0x001A6B34 File Offset: 0x001A4F34
	Protected Overrides Sub Start()
		MyBase.Start()
		If Me.child IsNot Nothing Then
			Me.child.transform.SetEulerAngles(Nothing, Nothing, New Single?(0F))
			AddHandler Me.child.OnPlayerCollision, AddressOf Me.OnCollisionPlayer
		End If
		MyBase.StartCoroutine(Me.move_cr())
		MyBase.StartCoroutine(Me.on_ground_hit_cr())
	End Sub

	' Token: 0x06002CDA RID: 11482 RVA: 0x001A6BB6 File Offset: 0x001A4FB6
	Private Sub OnPhase3()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002CDB RID: 11483 RVA: 0x001A6BC3 File Offset: 0x001A4FC3
	Protected Overrides Sub Die()
		MyBase.Die()
	End Sub

	' Token: 0x06002CDC RID: 11484 RVA: 0x001A6BCC File Offset: 0x001A4FCC
	Private Iterator Function on_ground_hit_cr() As IEnumerator
		While MyBase.transform.position.y > CSng(Level.Current.Ground)
			Yield Nothing
		End While
		Me.move = False
		If Me.isBaby Then
			MyBase.animator.SetTrigger("OnSmash")
			AudioManager.Play("sally_bottle_smash")
			Me.emitAudioFromObject.Add("sally_bottle_smash")
		Else
			MyBase.animator.Play("Death")
		End If
		Yield Nothing
		Return
	End Function

	' Token: 0x06002CDD RID: 11485 RVA: 0x001A6BE7 File Offset: 0x001A4FE7
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x0400354E RID: 13646
	<SerializeField()>
	Private isBaby As Boolean

	' Token: 0x0400354F RID: 13647
	<SerializeField()>
	Private child As CollisionChild

	' Token: 0x04003550 RID: 13648
	Private speed As Single

	' Token: 0x04003551 RID: 13649
	Private rotation As Single

	' Token: 0x04003552 RID: 13650
	Private move As Boolean
End Class
