Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200074D RID: 1869
Public MustInherit Class RetroArcadeEnemy
	Inherits AbstractProjectile

	' Token: 0x170003DC RID: 988
	' (get) Token: 0x060028BB RID: 10427 RVA: 0x00176BD5 File Offset: 0x00174FD5
	' (set) Token: 0x060028BC RID: 10428 RVA: 0x00176BDD File Offset: 0x00174FDD
	Public Property IsDead As Boolean

	' Token: 0x170003DD RID: 989
	' (get) Token: 0x060028BD RID: 10429 RVA: 0x00176BE6 File Offset: 0x00174FE6
	' (set) Token: 0x060028BE RID: 10430 RVA: 0x00176BEE File Offset: 0x00174FEE
	Public Property PointsWorth As Single

	' Token: 0x170003DE RID: 990
	' (get) Token: 0x060028BF RID: 10431 RVA: 0x00176BF7 File Offset: 0x00174FF7
	' (set) Token: 0x060028C0 RID: 10432 RVA: 0x00176BFF File Offset: 0x00174FFF
	Public Property PointsBonus As Single

	' Token: 0x170003DF RID: 991
	' (get) Token: 0x060028C1 RID: 10433 RVA: 0x00176C08 File Offset: 0x00175008
	Protected Overrides ReadOnly Property DestroyLifetime As Single
		Get
			Return 0F
		End Get
	End Property

	' Token: 0x060028C2 RID: 10434 RVA: 0x00176C0F File Offset: 0x0017500F
	Protected Overrides Sub Awake()
		MyBase.Awake()
		If MyBase.GetComponent(Of DamageReceiver)() IsNot Nothing Then
			AddHandler MyBase.GetComponent(Of DamageReceiver)().OnDamageTaken, AddressOf Me.OnDamageTaken
		End If
		Me.IsDead = False
	End Sub

	' Token: 0x060028C3 RID: 10435 RVA: 0x00176C47 File Offset: 0x00175047
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060028C4 RID: 10436 RVA: 0x00176C65 File Offset: 0x00175065
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060028C5 RID: 10437 RVA: 0x00176C84 File Offset: 0x00175084
	Protected Overridable Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		RetroArcadeLevel.TOTAL_POINTS += Me.PointsWorth
		Me.hp -= info.damage
		If Me.hp < 0F AndAlso Not Me.IsDead Then
			Me.Dead()
		End If
	End Sub

	' Token: 0x060028C6 RID: 10438 RVA: 0x00176CD6 File Offset: 0x001750D6
	Public Sub MoveY(moveAmount As Single, moveSpeed As Single)
		If Me.moveCoroutine IsNot Nothing Then
			MyBase.StopCoroutine(Me.moveCoroutine)
		End If
		Me.movingY = True
		Me.moveCoroutine = MyBase.StartCoroutine(Me.moveY_cr(moveAmount, Mathf.Abs(moveAmount) / moveSpeed))
	End Sub

	' Token: 0x060028C7 RID: 10439 RVA: 0x00176D14 File Offset: 0x00175114
	Private Iterator Function moveY_cr(moveAmount As Single, time As Single) As IEnumerator
		Dim t As Single = 0F
		Dim startY As Single = MyBase.transform.position.y
		Dim endY As Single = startY + moveAmount
		While t < time
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, startY, endY, t / time)), Nothing)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(endY), Nothing)
		Me.movingY = False
		Return
	End Function

	' Token: 0x060028C8 RID: 10440 RVA: 0x00176D40 File Offset: 0x00175140
	Public Overridable Sub Dead()
		If Me.type <> RetroArcadeEnemy.Type.IsBoss Then
			Me.CheckForColorBonus()
		End If
		Dim component As Collider2D = MyBase.GetComponent(Of Collider2D)()
		If component IsNot Nothing Then
			component.enabled = False
		End If
		Me.IsDead = True
		MyBase.GetComponent(Of SpriteRenderer)().color = New Color(0F, 0F, 0F, 0.25F)
	End Sub

	' Token: 0x060028C9 RID: 10441 RVA: 0x00176DA4 File Offset: 0x001751A4
	Private Sub CheckForColorBonus()
		If Me.type = RetroArcadeEnemy.LAST_TYPE Then
			RetroArcadeEnemy.COUNTER += 1
			If RetroArcadeEnemy.COUNTER >= 3 Then
				Me.GiveBonus()
				RetroArcadeEnemy.COUNTER = 0
				RetroArcadeEnemy.LAST_TYPE = RetroArcadeEnemy.Type.None
			Else
				RetroArcadeEnemy.LAST_TYPE = Me.type
			End If
		Else
			RetroArcadeEnemy.COUNTER = 1
			RetroArcadeEnemy.LAST_TYPE = Me.type
		End If
	End Sub

	' Token: 0x060028CA RID: 10442 RVA: 0x00176E10 File Offset: 0x00175210
	Protected Overridable Sub GiveBonus()
		RetroArcadeLevel.TOTAL_POINTS += Me.PointsBonus
	End Sub

	' Token: 0x04003193 RID: 12691
	<SerializeField()>
	Public type As RetroArcadeEnemy.Type

	' Token: 0x04003194 RID: 12692
	Private Shared COUNTER As Integer

	' Token: 0x04003195 RID: 12693
	Private Shared LAST_TYPE As RetroArcadeEnemy.Type

	' Token: 0x04003196 RID: 12694
	Private moveCoroutine As Coroutine

	' Token: 0x0400319A RID: 12698
	Private pointsBonusAccuracy As Single

	' Token: 0x0400319B RID: 12699
	Private inComboChain As Boolean

	' Token: 0x0400319C RID: 12700
	Protected hp As Single

	' Token: 0x0400319D RID: 12701
	Protected movingY As Boolean

	' Token: 0x0200074E RID: 1870
	Public Enum Type
		' Token: 0x0400319F RID: 12703
		A
		' Token: 0x040031A0 RID: 12704
		B
		' Token: 0x040031A1 RID: 12705
		C
		' Token: 0x040031A2 RID: 12706
		None
		' Token: 0x040031A3 RID: 12707
		IsBoss
	End Enum
End Class
