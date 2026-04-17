Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006F4 RID: 1780
Public Class MouseLevelSawBlade
	Inherits AbstractCollidableObject

	' Token: 0x170003C8 RID: 968
	' (get) Token: 0x06002621 RID: 9761 RVA: 0x0016458E File Offset: 0x0016298E
	' (set) Token: 0x06002622 RID: 9762 RVA: 0x00164596 File Offset: 0x00162996
	Public Property state As MouseLevelSawBlade.State

	' Token: 0x06002623 RID: 9763 RVA: 0x0016459F File Offset: 0x0016299F
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
		MyBase.GetComponent(Of Collider2D)().enabled = False
	End Sub

	' Token: 0x06002624 RID: 9764 RVA: 0x001645BE File Offset: 0x001629BE
	Private Sub Start()
		MyBase.animator.SetFloat("SawID", CSng(Me.sawId) / 5F)
		MyBase.animator.SetFloat("StickID", CSng(Me.stickId) / 7F)
	End Sub

	' Token: 0x06002625 RID: 9765 RVA: 0x001645FA File Offset: 0x001629FA
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06002626 RID: 9766 RVA: 0x00164612 File Offset: 0x00162A12
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If Me.damageDealer IsNot Nothing AndAlso phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06002627 RID: 9767 RVA: 0x0016463B File Offset: 0x00162A3B
	Public Sub Begin(properties As LevelProperties.Mouse)
		Me.properties = properties
		MyBase.StartCoroutine(Me.intro_cr())
		Me.attackX = Me.attackMinX
		Me.fullAttackX = Me.attackMinX
	End Sub

	' Token: 0x06002628 RID: 9768 RVA: 0x00164669 File Offset: 0x00162A69
	Public Sub Attack()
		AudioManager.Play("level_mouse_buzzsaw_small")
		If Me.state = MouseLevelSawBlade.State.Idle Then
			Me.state = MouseLevelSawBlade.State.Warning
			MyBase.StartCoroutine(Me.attack_cr(False))
		End If
	End Sub

	' Token: 0x06002629 RID: 9769 RVA: 0x00164698 File Offset: 0x00162A98
	Public Sub FullAttack()
		If Me.state = MouseLevelSawBlade.State.Warning Then
			Me.StopAllCoroutines()
		ElseIf Me.state = MouseLevelSawBlade.State.Idle Then
			Me.state = MouseLevelSawBlade.State.Warning
		End If
		Me.fullAttacking = True
		MyBase.StartCoroutine(Me.attack_cr(True))
	End Sub

	' Token: 0x0600262A RID: 9770 RVA: 0x001646E4 File Offset: 0x00162AE4
	Private Iterator Function intro_cr() As IEnumerator
		Dim t As Single = 0F
		Dim introTime As Single = Mathf.Abs(Me.idleX - Me.initX) / Me.properties.CurrentState.brokenCanSawBlades.entrySpeed
		While t < introTime
			If t > introTime * 0.75F Then
				MyBase.GetComponent(Of Collider2D)().enabled = True
			End If
			MyBase.transform.SetLocalPosition(New Single?(Mathf.Lerp(Me.initX, Me.idleX, t / introTime)), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetLocalPosition(New Single?(Me.idleX), Nothing, Nothing)
		Me.state = MouseLevelSawBlade.State.Idle
		Return
	End Function

	' Token: 0x0600262B RID: 9771 RVA: 0x00164700 File Offset: 0x00162B00
	Private Iterator Function attack_cr(fullAttack As Boolean) As IEnumerator
		Dim p As LevelProperties.Mouse.BrokenCanSawBlades = Me.properties.CurrentState.brokenCanSawBlades
		Dim t As Single = 0F
		While t < p.delayBeforeAttack
			Me.progress = t / p.delayBeforeAttack
			Dim x As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, Me.idleX, Me.attackMinX, Me.progress)
			Me.setX(x, fullAttack)
			t += CupheadTime.Delta
			Me.blade.transform.Rotate(Vector3.forward, Me.rotateSpeed / 2F * CupheadTime.Delta)
			Yield Nothing
		End While
		MyBase.animator.SetBool("Attacking", True)
		Me.state = MouseLevelSawBlade.State.Attack
		Dim attackTime As Single = 2F * Mathf.Abs(Me.attackMaxX - Me.attackMinX) / p.speed
		t = 0F
		While t < attackTime
			Dim start As Single = Me.attackMinX
			Me.progress = t / attackTime * 2F
			If Me.progress > 1F Then
				start = Me.idleX
				Me.progress = 2F - Me.progress
				Me.blade.transform.Rotate(Vector3.forward, EaseUtils.EaseInOutSine(0F, Me.rotateSpeed, Me.progress) * CupheadTime.Delta)
			Else
				Me.blade.transform.Rotate(Vector3.forward, Me.rotateSpeed * CupheadTime.Delta)
			End If
			Dim x2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, Me.attackMaxX, Me.progress)
			Me.setX(x2, fullAttack)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.setX(Me.idleX, fullAttack)
		If fullAttack Then
			Me.fullAttacking = False
		End If
		If Not Me.fullAttacking Then
			MyBase.animator.SetBool("Attacking", False)
			Me.attackX = Me.attackMinX
			Me.fullAttackX = Me.attackMinX
			Me.state = MouseLevelSawBlade.State.Idle
		End If
		Return
	End Function

	' Token: 0x0600262C RID: 9772 RVA: 0x00164722 File Offset: 0x00162B22
	Public Sub Leave()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.leave_cr())
	End Sub

	' Token: 0x0600262D RID: 9773 RVA: 0x00164738 File Offset: 0x00162B38
	Private Iterator Function leave_cr() As IEnumerator
		Dim leaveTime As Single = 0F
		If Me.state = MouseLevelSawBlade.State.Warning Then
			Me.state = MouseLevelSawBlade.State.Idle
			MyBase.animator.SetBool("Attacking", False)
		End If
		If Me.state = MouseLevelSawBlade.State.Attack Then
			leaveTime = Mathf.Abs(MyBase.transform.localPosition.x - Me.initX) / Me.properties.CurrentState.brokenCanSawBlades.speed
		Else
			leaveTime = 2F
		End If
		Dim t As Single = 0F
		Dim startingX As Single = MyBase.transform.localPosition.x
		While t < leaveTime
			If t > leaveTime * 0.25F Then
				MyBase.GetComponent(Of Collider2D)().enabled = False
			End If
			MyBase.transform.SetLocalPosition(New Single?(Mathf.Lerp(startingX, Me.initX, t / leaveTime)), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.transform.SetLocalPosition(New Single?(Me.initX), Nothing, Nothing)
		Return
	End Function

	' Token: 0x0600262E RID: 9774 RVA: 0x00164754 File Offset: 0x00162B54
	Private Sub setX(x As Single, fullAttack As Boolean)
		If fullAttack Then
			Me.fullAttackX = x
		Else
			Me.attackX = x
		End If
		If Me.idleX > 0F Then
			MyBase.transform.SetLocalPosition(New Single?(Mathf.Min(Me.attackX, Me.fullAttackX)), Nothing, Nothing)
		Else
			MyBase.transform.SetLocalPosition(New Single?(Mathf.Max(Me.attackX, Me.fullAttackX)), Nothing, Nothing)
		End If
	End Sub

	' Token: 0x04002EA0 RID: 11936
	Private Const SawParameterName As String = "SawID"

	' Token: 0x04002EA1 RID: 11937
	Private Const StickParameterName As String = "StickID"

	' Token: 0x04002EA2 RID: 11938
	Private Const AttackParameterName As String = "Attacking"

	' Token: 0x04002EA3 RID: 11939
	Private Const SawIdMax As Integer = 6

	' Token: 0x04002EA4 RID: 11940
	Private Const StickIdMax As Integer = 8

	' Token: 0x04002EA6 RID: 11942
	<SerializeField()>
	Private initX As Single

	' Token: 0x04002EA7 RID: 11943
	<SerializeField()>
	Private idleX As Single

	' Token: 0x04002EA8 RID: 11944
	<SerializeField()>
	Private attackMinX As Single

	' Token: 0x04002EA9 RID: 11945
	<SerializeField()>
	Private attackMaxX As Single

	' Token: 0x04002EAA RID: 11946
	<SerializeField()>
	Private blade As Transform

	' Token: 0x04002EAB RID: 11947
	<SerializeField()>
	Private rotateSpeed As Single

	' Token: 0x04002EAC RID: 11948
	<Range(0F, 5F)>
	<SerializeField()>
	Private sawId As Integer

	' Token: 0x04002EAD RID: 11949
	<Range(0F, 7F)>
	<SerializeField()>
	Private stickId As Integer

	' Token: 0x04002EAE RID: 11950
	Private properties As LevelProperties.Mouse

	' Token: 0x04002EAF RID: 11951
	Private fullAttacking As Boolean

	' Token: 0x04002EB0 RID: 11952
	Private goBackwards As Boolean

	' Token: 0x04002EB1 RID: 11953
	Private attackX As Single

	' Token: 0x04002EB2 RID: 11954
	Private fullAttackX As Single

	' Token: 0x04002EB3 RID: 11955
	Private progress As Single

	' Token: 0x04002EB4 RID: 11956
	Private damageDealer As DamageDealer

	' Token: 0x020006F5 RID: 1781
	Public Enum State
		' Token: 0x04002EB6 RID: 11958
		Init
		' Token: 0x04002EB7 RID: 11959
		Idle
		' Token: 0x04002EB8 RID: 11960
		Warning
		' Token: 0x04002EB9 RID: 11961
		Attack
	End Enum
End Class
