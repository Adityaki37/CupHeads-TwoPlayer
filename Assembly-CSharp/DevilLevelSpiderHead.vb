Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000581 RID: 1409
Public Class DevilLevelSpiderHead
	Inherits AbstractCollidableObject

	' Token: 0x06001AEC RID: 6892 RVA: 0x000F792D File Offset: 0x000F5D2D
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.gameObject.SetActive(False)
	End Sub

	' Token: 0x06001AED RID: 6893 RVA: 0x000F794C File Offset: 0x000F5D4C
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001AEE RID: 6894 RVA: 0x000F7964 File Offset: 0x000F5D64
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001AEF RID: 6895 RVA: 0x000F7990 File Offset: 0x000F5D90
	Public Sub Attack(xPos As Single, downSpeed As Single, upSpeed As Single)
		MyBase.gameObject.SetActive(True)
		MyBase.animator.SetBool("IsFalling", True)
		Me.state = DevilLevelSpiderHead.State.Attacking
		MyBase.transform.SetPosition(New Single?(xPos), Nothing, Nothing)
		MyBase.StartCoroutine(Me.attack_cr(downSpeed, upSpeed))
	End Sub

	' Token: 0x06001AF0 RID: 6896 RVA: 0x000F79F4 File Offset: 0x000F5DF4
	Private Iterator Function attack_cr(downSpeed As Single, upSpeed As Single) As IEnumerator
		Dim moveTime As Single = Mathf.Abs(Me.moveDistanceY) / downSpeed
		Dim startY As Single = MyBase.transform.position.y
		Dim t As Single = 0F
		MyBase.GetComponent(Of Collider2D)().enabled = True
		AudioManager.Play("devil_spider_fall")
		Me.emitAudioFromObject.Add("devil_spider_fall")
		While t < moveTime
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, startY, startY + Me.moveDistanceY, t / moveTime)), Nothing)
			Yield New WaitForFixedUpdate()
			t += CupheadTime.FixedDelta
		End While
		AudioManager.Play("devil_spider_head_hit_floor")
		Me.emitAudioFromObject.Add("devil_spider_head_hit_floor")
		MyBase.animator.SetBool("IsFalling", False)
		MyBase.transform.SetPosition(Nothing, New Single?(startY + Me.moveDistanceY), Nothing)
		t = 0F
		moveTime = Mathf.Abs(Me.moveDistanceY) / upSpeed
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fall_Splat", False, True)
		While t < moveTime
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, startY + Me.moveDistanceY, startY, t / moveTime)), Nothing)
			Yield New WaitForFixedUpdate()
			t += CupheadTime.FixedDelta
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(startY), Nothing)
		Me.state = DevilLevelSpiderHead.State.Idle
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x04002426 RID: 9254
	Public state As DevilLevelSpiderHead.State

	' Token: 0x04002427 RID: 9255
	Private damageDealer As DamageDealer

	' Token: 0x04002428 RID: 9256
	<SerializeField()>
	Private moveDistanceY As Single

	' Token: 0x02000582 RID: 1410
	Public Enum State
		' Token: 0x0400242A RID: 9258
		Idle
		' Token: 0x0400242B RID: 9259
		Attacking
	End Enum
End Class
