Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000507 RID: 1287
Public Class BatLevelHomingSoul
	Inherits AbstractCollidableObject

	' Token: 0x060016D0 RID: 5840 RVA: 0x000CD160 File Offset: 0x000CB560
	Public Sub Init(pos As Vector2, player As AbstractPlayerController, properties As LevelProperties.Bat.WolfSoul)
		Me.aim = New GameObject("Aim").transform
		Me.aim.SetParent(MyBase.transform)
		Me.aim.ResetLocalTransforms()
		Me.properties = properties
		Me.player = player
		Me.durationString = properties.floatUpDuration.GetRandom().Split(New Char() { ","c })
	End Sub

	' Token: 0x060016D1 RID: 5841 RVA: 0x000CD1CD File Offset: 0x000CB5CD
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.isHoming = True
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x060016D2 RID: 5842 RVA: 0x000CD1F4 File Offset: 0x000CB5F4
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
		If Me.aim Is Nothing OrElse Me.player Is Nothing Then
			Return
		End If
		If Me.isHoming Then
			Dim num As Single = Vector3.Distance(MyBase.transform.position, Me.player.transform.position)
			MyBase.transform.position -= MyBase.transform.right * Me.properties.homingSpeed * CupheadTime.Delta
			Me.aim.LookAt2D(2F * MyBase.transform.position - Me.player.center)
			MyBase.transform.rotation = Quaternion.Slerp(MyBase.transform.rotation, Me.aim.rotation, Me.properties.homingRotation * CupheadTime.Delta)
			If Mathf.Abs(num) < Me.maxDist AndAlso Me.isHoming Then
				MyBase.StartCoroutine(Me.attack_cr())
				Me.isHoming = False
			End If
		End If
	End Sub

	' Token: 0x060016D3 RID: 5843 RVA: 0x000CD342 File Offset: 0x000CB742
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		Me.damageDealer.DealDamage(hit)
	End Sub

	' Token: 0x060016D4 RID: 5844 RVA: 0x000CD35C File Offset: 0x000CB75C
	Private Iterator Function attack_cr() As IEnumerator
		MyBase.animator.SetTrigger("Warning")
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.floatWarningDuration)
		MyBase.animator.SetTrigger("Attack")
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Yield CupheadTime.WaitForSeconds(Me, Me.properties.attackDuration)
		MyBase.animator.SetTrigger("End")
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.StartCoroutine(Me.float_up_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x060016D5 RID: 5845 RVA: 0x000CD378 File Offset: 0x000CB778
	Private Iterator Function float_up_cr() As IEnumerator
		Dim t As Single = 0F
		Dim duration As Single = 0F
		Parser.FloatTryParse(Me.durationString(Me.durationIndex), duration)
		Me.player = PlayerManager.GetNext()
		While t < duration
			MyBase.transform.AddPosition(0F, Me.properties.floatSpeed * CupheadTime.Delta, 0F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.isHoming = True
		Me.durationIndex = Me.durationIndex Mod Me.durationString.Length
		Yield Nothing
		Return
	End Function

	' Token: 0x0400201B RID: 8219
	Private properties As LevelProperties.Bat.WolfSoul

	' Token: 0x0400201C RID: 8220
	Private player As AbstractPlayerController

	' Token: 0x0400201D RID: 8221
	Private damageDealer As DamageDealer

	' Token: 0x0400201E RID: 8222
	Private durationIndex As Integer

	' Token: 0x0400201F RID: 8223
	Private aim As Transform

	' Token: 0x04002020 RID: 8224
	Private targetPos As Transform

	' Token: 0x04002021 RID: 8225
	Private maxDist As Single = 100F

	' Token: 0x04002022 RID: 8226
	Private isHoming As Boolean

	' Token: 0x04002023 RID: 8227
	Private durationString As String()
End Class
