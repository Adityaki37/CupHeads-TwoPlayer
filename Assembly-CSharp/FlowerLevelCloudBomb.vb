Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000606 RID: 1542
Public Class FlowerLevelCloudBomb
	Inherits AbstractCollidableObject

	' Token: 0x06001EB4 RID: 7860 RVA: 0x0011AAA9 File Offset: 0x00118EA9
	Public Sub OnCloudBombStart(target As Vector3, s As Single, delay As Single)
		Me.playerPos = target
		MyBase.transform.LookAt2D(Me.playerPos)
		Me.speed = s
		Me.detonationDelay = delay
		Me.hasDetonated = False
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001EB5 RID: 7861 RVA: 0x0011AAE3 File Offset: 0x00118EE3
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001EB6 RID: 7862 RVA: 0x0011AAFC File Offset: 0x00118EFC
	Private Sub FixedUpdate()
		If Not Me.hasDetonated Then
			If(Me.playerPos - MyBase.transform.position).magnitude > (MyBase.transform.right * (Me.speed * CupheadTime.FixedDelta)).magnitude Then
				MyBase.transform.position += MyBase.transform.right * (Me.speed * CupheadTime.FixedDelta)
			Else
				Me.hasDetonated = True
				MyBase.StartCoroutine(Me.explode_cr())
				MyBase.transform.position += Me.playerPos - MyBase.transform.position
			End If
		End If
	End Sub

	' Token: 0x06001EB7 RID: 7863 RVA: 0x0011ABD2 File Offset: 0x00118FD2
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x06001EB8 RID: 7864 RVA: 0x0011ABFC File Offset: 0x00118FFC
	Private Iterator Function explode_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, Me.detonationDelay)
		MyBase.animator.SetTrigger("Explode")
		Dim collider As BoxCollider2D = MyBase.GetComponent(Of BoxCollider2D)()
		collider.size = MyBase.GetComponent(Of SpriteRenderer)().bounds.size
		Return
	End Function

	' Token: 0x06001EB9 RID: 7865 RVA: 0x0011AC17 File Offset: 0x00119017
	Protected Sub Die()
		AudioManager.Play("flower_minion_simple_deathpop_high")
		Me.emitAudioFromObject.Add("flower_minion_simple_deathpop_high")
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400277D RID: 10109
	Private hasDetonated As Boolean

	' Token: 0x0400277E RID: 10110
	Private speed As Single

	' Token: 0x0400277F RID: 10111
	Private detonationDelay As Single

	' Token: 0x04002780 RID: 10112
	Private playerPos As Vector3

	' Token: 0x04002781 RID: 10113
	Private damageDealer As DamageDealer
End Class
