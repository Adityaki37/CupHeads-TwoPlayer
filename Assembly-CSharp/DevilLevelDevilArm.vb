Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200056F RID: 1391
Public Class DevilLevelDevilArm
	Inherits AbstractCollidableObject

	' Token: 0x06001A5E RID: 6750 RVA: 0x000F1560 File Offset: 0x000EF960
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.startX = MyBase.transform.position.x
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001A5F RID: 6751 RVA: 0x000F1597 File Offset: 0x000EF997
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001A60 RID: 6752 RVA: 0x000F15AF File Offset: 0x000EF9AF
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001A61 RID: 6753 RVA: 0x000F15D8 File Offset: 0x000EF9D8
	Public Sub Attack(speed As Single)
		Me.state = DevilLevelDevilArm.State.Attacking
		MyBase.animator.SetTrigger("ArmsIn")
		MyBase.StartCoroutine(Me.attack_cr(speed))
	End Sub

	' Token: 0x06001A62 RID: 6754 RVA: 0x000F1600 File Offset: 0x000EFA00
	Private Iterator Function attack_cr(speed As Single) As IEnumerator
		Me.speed = speed
		Dim isClapping As Boolean = False
		Dim t As Single = 0F
		MyBase.GetComponent(Of Collider2D)().enabled = True
		While t < speed
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(Me.startX, Me.endPos.position.x, t / speed)), Nothing, Nothing)
			Yield New WaitForFixedUpdate()
			t += CupheadTime.FixedDelta
			If t / speed > 0.85F AndAlso Not isClapping Then
				MyBase.animator.SetTrigger("OnAttack")
				isClapping = True
			End If
		End While
		MyBase.transform.SetPosition(New Single?(Me.endPos.position.x), Nothing, Nothing)
		CupheadLevelCamera.Current.Shake(20F, 0.7F, False)
		Yield New WaitForFixedUpdate()
		Return
	End Function

	' Token: 0x06001A63 RID: 6755 RVA: 0x000F1622 File Offset: 0x000EFA22
	Public Sub MoveAway()
		MyBase.StartCoroutine(Me.move_away_cr())
	End Sub

	' Token: 0x06001A64 RID: 6756 RVA: 0x000F1634 File Offset: 0x000EFA34
	Private Iterator Function move_away_cr() As IEnumerator
		Dim currentPos As Single = MyBase.transform.position.x
		Dim t As Single = 0F
		Dim moveTime As Single = Me.speed
		t = 0F
		While t < moveTime
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.linear, 0F, 1F, t / moveTime)
			MyBase.transform.SetPosition(New Single?(Mathf.Lerp(currentPos, Me.startX, val)), Nothing, Nothing)
			Yield New WaitForFixedUpdate()
			t += CupheadTime.FixedDelta
		End While
		MyBase.transform.SetPosition(New Single?(Me.startX), Nothing, Nothing)
		Me.RamSlapSFXActive = False
		MyBase.GetComponent(Of Collider2D)().enabled = False
		Me.state = DevilLevelDevilArm.State.Idle
		Yield Nothing
		Return
	End Function

	' Token: 0x06001A65 RID: 6757 RVA: 0x000F164F File Offset: 0x000EFA4F
	Private Sub HandclapSFX()
		If Me.isRight Then
			AudioManager.Play("devil_hand_clap")
		End If
	End Sub

	' Token: 0x06001A66 RID: 6758 RVA: 0x000F1666 File Offset: 0x000EFA66
	Private Sub RamSlapSFX()
		If Not Me.RamSlapSFXActive Then
			AudioManager.Play("devil_ram_slap")
			Me.RamSlapSFXActive = True
		End If
	End Sub

	' Token: 0x04002386 RID: 9094
	Public state As DevilLevelDevilArm.State

	' Token: 0x04002387 RID: 9095
	Private RamSlapSFXActive As Boolean

	' Token: 0x04002388 RID: 9096
	Private damageDealer As DamageDealer

	' Token: 0x04002389 RID: 9097
	<SerializeField()>
	Private endPos As Transform

	' Token: 0x0400238A RID: 9098
	Private speed As Single

	' Token: 0x0400238B RID: 9099
	Private startX As Single

	' Token: 0x0400238C RID: 9100
	Public isRight As Boolean

	' Token: 0x02000570 RID: 1392
	Public Enum State
		' Token: 0x0400238E RID: 9102
		Idle
		' Token: 0x0400238F RID: 9103
		Attacking
	End Enum
End Class
