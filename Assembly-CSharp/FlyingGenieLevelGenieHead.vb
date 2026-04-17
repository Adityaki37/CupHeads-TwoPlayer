Imports System
Imports UnityEngine

' Token: 0x0200066A RID: 1642
Public Class FlyingGenieLevelGenieHead
	Inherits AbstractProjectile

	' Token: 0x06002263 RID: 8803 RVA: 0x00142137 File Offset: 0x00140537
	Public Sub Init(pos As Vector3, health As Single, parent As FlyingGenieLevelGenie)
		MyBase.transform.position = pos
		Me.parent = parent
		Me.health = health
	End Sub

	' Token: 0x06002264 RID: 8804 RVA: 0x00142154 File Offset: 0x00140554
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		Me.darkSprite.sortingOrder = MyBase.GetComponent(Of SpriteRenderer)().sortingOrder + 1
	End Sub

	' Token: 0x06002265 RID: 8805 RVA: 0x001421A2 File Offset: 0x001405A2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002266 RID: 8806 RVA: 0x001421C0 File Offset: 0x001405C0
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002267 RID: 8807 RVA: 0x001421DE File Offset: 0x001405DE
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F Then
			Me.Die()
		End If
		Me.parent.DoDamage(info.damage)
	End Sub

	' Token: 0x06002268 RID: 8808 RVA: 0x0014221C File Offset: 0x0014061C
	Protected Overrides Sub Die()
		AudioManager.Play("genie_pillar_destruction")
		Me.emitAudioFromObject.Add("genie_pillar_destruction")
		Me.headExplode.Create(New Vector3(MyBase.transform.position.x - 75F, MyBase.transform.position.y))
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Me.darkSprite.GetComponent(Of SpriteRenderer)().enabled = False
		MyBase.Die()
	End Sub

	' Token: 0x04002B0F RID: 11023
	<SerializeField()>
	Private headExplode As Effect

	' Token: 0x04002B10 RID: 11024
	<SerializeField()>
	Private darkSprite As SpriteRenderer

	' Token: 0x04002B11 RID: 11025
	Private damageReceiver As DamageReceiver

	' Token: 0x04002B12 RID: 11026
	Private parent As FlyingGenieLevelGenie

	' Token: 0x04002B13 RID: 11027
	Private health As Single
End Class
