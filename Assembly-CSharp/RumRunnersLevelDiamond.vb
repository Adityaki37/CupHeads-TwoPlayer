Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200078B RID: 1931
Public Class RumRunnersLevelDiamond
	Inherits AbstractCollidableObject

	' Token: 0x06002AA8 RID: 10920 RVA: 0x0018EAFE File Offset: 0x0018CEFE
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06002AA9 RID: 10921 RVA: 0x0018EB11 File Offset: 0x0018CF11
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002AAA RID: 10922 RVA: 0x0018EB29 File Offset: 0x0018CF29
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x06002AAB RID: 10923 RVA: 0x0018EB40 File Offset: 0x0018CF40
	Public Sub Die()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06002AAC RID: 10924 RVA: 0x0018EB53 File Offset: 0x0018CF53
	Public Sub SetAttack(attack As Boolean)
		If attack Then
			Me.horn.enabled = False
			Me.hornAttack.enabled = True
		Else
			Me.horn.enabled = True
			Me.hornAttack.enabled = False
		End If
	End Sub

	' Token: 0x06002AAD RID: 10925 RVA: 0x0018EB90 File Offset: 0x0018CF90
	Public Sub StartSparkle()
		MyBase.StartCoroutine(Me.startSparkle_cr())
	End Sub

	' Token: 0x06002AAE RID: 10926 RVA: 0x0018EBA0 File Offset: 0x0018CFA0
	Private Iterator Function startSparkle_cr() As IEnumerator
		Dim color As Color = Me.sparkle.color
		color.a = 0F
		Me.sparkle.color = color
		MyBase.animator.Play("On", 1)
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.2F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			color.a = Mathf.Lerp(0F, 1F, elapsedTime / 0.2F)
			Me.sparkle.color = color
		End While
		Return
	End Function

	' Token: 0x06002AAF RID: 10927 RVA: 0x0018EBBB File Offset: 0x0018CFBB
	Public Sub EndSparkle()
		MyBase.StartCoroutine(Me.endSparkle_cr())
	End Sub

	' Token: 0x06002AB0 RID: 10928 RVA: 0x0018EBCC File Offset: 0x0018CFCC
	Private Iterator Function endSparkle_cr() As IEnumerator
		Dim color As Color = Me.sparkle.color
		Me.sparkle.color = color
		Dim elapsedTime As Single = 0F
		While elapsedTime < 0.45F
			Yield Nothing
			elapsedTime += CupheadTime.Delta
			color.a = Mathf.Lerp(1F, 0F, elapsedTime / 0.45F)
			Me.sparkle.color = color
		End While
		MyBase.animator.Play("Off", 1)
		Return
	End Function

	' Token: 0x0400336C RID: 13164
	<SerializeField()>
	Private horn As SpriteRenderer

	' Token: 0x0400336D RID: 13165
	<SerializeField()>
	Private hornAttack As SpriteRenderer

	' Token: 0x0400336E RID: 13166
	<SerializeField()>
	Private sparkle As SpriteRenderer

	' Token: 0x0400336F RID: 13167
	Private damageDealer As DamageDealer
End Class
