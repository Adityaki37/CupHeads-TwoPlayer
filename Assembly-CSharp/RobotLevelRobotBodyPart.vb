Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000774 RID: 1908
Public Class RobotLevelRobotBodyPart
	Inherits AbstractCollidableObject

	' Token: 0x060029A4 RID: 10660 RVA: 0x0018485C File Offset: 0x00182C5C
	Public Overridable Sub InitBodyPart(parent As RobotLevelRobot, properties As LevelProperties.Robot, Optional primaryHP As Integer = 0, Optional secondaryHP As Integer = 1, Optional attackDelayMinus As Single = 0F)
		Me.parent = parent
		AddHandler Me.parent.OnDeathEvent, AddressOf Me.Die
		AddHandler Me.parent.OnPrimaryDeathEvent, AddressOf Me.OnPrimaryDeath
		AddHandler Me.parent.OnSecondaryDeathEvent, AddressOf Me.OnSecondaryDeath
		Me.properties = properties
		Me.current = RobotLevelRobotBodyPart.state.primary
		Me.currentHealth(0) = CSng(primaryHP)
		Me.currentHealth(1) = CSng(secondaryHP)
		Me.attackDelayMinus = attackDelayMinus
		MyBase.StartCoroutine(Me.checkCurrentState_cr())
	End Sub

	' Token: 0x060029A5 RID: 10661 RVA: 0x001848F0 File Offset: 0x00182CF0
	Protected Overridable Sub StartPrimary()
		MyBase.StartCoroutine(Me.primaryAttack_cr())
	End Sub

	' Token: 0x060029A6 RID: 10662 RVA: 0x00184900 File Offset: 0x00182D00
	Protected Overridable Iterator Function primaryAttack_cr() As IEnumerator
		While Me.current = RobotLevelRobotBodyPart.state.primary
			Yield CupheadTime.WaitForSeconds(Me, Me.primaryAttackDelay)
			Me.isAttacking = True
			Me.OnPrimaryAttack()
			While Me.isAttacking
				Yield Nothing
			End While
		End While
		Yield Nothing
		Return
	End Function

	' Token: 0x060029A7 RID: 10663 RVA: 0x0018491B File Offset: 0x00182D1B
	Protected Overridable Sub StartSecondary()
		MyBase.StartCoroutine(Me.secondaryAttack_cr())
	End Sub

	' Token: 0x060029A8 RID: 10664 RVA: 0x0018492C File Offset: 0x00182D2C
	Protected Overridable Iterator Function secondaryAttack_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 1.5F)
		While Me.current = RobotLevelRobotBodyPart.state.secondary
			Me.OnSecondaryAttack()
			Yield Nothing
			If Me.secondaryAttackDelay > 0F Then
				Yield CupheadTime.WaitForSeconds(Me, Me.secondaryAttackDelay)
			Else
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x060029A9 RID: 10665 RVA: 0x00184948 File Offset: 0x00182D48
	Protected Overridable Sub AttackDestroyed(isPrimary As Boolean)
		If isPrimary Then
			AudioManager.Play("robot_vocals_angry")
			Me.emitAudioFromObject.Add("robot_vocals_angry")
			Me.parent.PrimaryDied()
			Me.current = RobotLevelRobotBodyPart.state.secondary
		Else
			Me.current = RobotLevelRobotBodyPart.state.none
			MyBase.GetComponent(Of BoxCollider2D)().enabled = False
		End If
	End Sub

	' Token: 0x060029AA RID: 10666 RVA: 0x0018499F File Offset: 0x00182D9F
	Protected Overrides Sub Awake()
		Me.currentHealth = New Single(1) {}
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.Awake()
	End Sub

	' Token: 0x060029AB RID: 10667 RVA: 0x001849D8 File Offset: 0x00182DD8
	Protected Overridable Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Dim num As Single = Me.currentHealth(CInt(Me.current))
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			Me.currentHealth(CInt(Me.current)) -= info.damage
		End If
		If num > 0F Then
			Level.Current.timeline.DealDamage(Mathf.Clamp(num - Me.currentHealth(CInt(Me.current)), 0F, num))
		End If
	End Sub

	' Token: 0x060029AC RID: 10668 RVA: 0x00184A50 File Offset: 0x00182E50
	Protected Iterator Function checkCurrentState_cr() As IEnumerator
		While Me.current <> RobotLevelRobotBodyPart.state.none
			Dim state As RobotLevelRobotBodyPart.state = Me.current
			If state <> RobotLevelRobotBodyPart.state.primary Then
				If state = RobotLevelRobotBodyPart.state.secondary Then
					If Me.currentHealth(1) <= 0F Then
						Me.AttackDestroyed(False)
					End If
				End If
			ElseIf Me.currentHealth(0) <= 0F Then
				Me.AttackDestroyed(True)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060029AD RID: 10669 RVA: 0x00184A6B File Offset: 0x00182E6B
	Protected Overridable Sub OnPrimaryAttack()
	End Sub

	' Token: 0x060029AE RID: 10670 RVA: 0x00184A6D File Offset: 0x00182E6D
	Protected Overridable Sub OnPrimaryDeath()
		If Me.current = RobotLevelRobotBodyPart.state.primary Then
			Me.primaryAttackDelay -= Me.attackDelayMinus
		End If
	End Sub

	' Token: 0x060029AF RID: 10671 RVA: 0x00184A8D File Offset: 0x00182E8D
	Protected Overridable Sub OnSecondaryAttack()
	End Sub

	' Token: 0x060029B0 RID: 10672 RVA: 0x00184A8F File Offset: 0x00182E8F
	Protected Overridable Sub OnSecondaryDeath()
	End Sub

	' Token: 0x060029B1 RID: 10673 RVA: 0x00184A91 File Offset: 0x00182E91
	Protected Overridable Sub ExitCurrentAttacks()
	End Sub

	' Token: 0x060029B2 RID: 10674 RVA: 0x00184A93 File Offset: 0x00182E93
	Protected Overridable Sub DeathEffect()
		If Me.deathEffect IsNot Nothing Then
			MyBase.StartCoroutine(Me.death_effects_cr())
		End If
	End Sub

	' Token: 0x060029B3 RID: 10675 RVA: 0x00184AB4 File Offset: 0x00182EB4
	Private Iterator Function death_effects_cr() As IEnumerator
		While True
			Yield Nothing
			Me.deathEffect.Create(MyBase.transform.position).GetComponent(Of Animator)().SetBool("IsA", Rand.Bool())
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(2F, 5F))
		End While
		Return
	End Function

	' Token: 0x060029B4 RID: 10676 RVA: 0x00184AD0 File Offset: 0x00182ED0
	Protected Overridable Sub Die()
		Me.StopAllCoroutines()
		Me.ExitCurrentAttacks()
		Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		If component IsNot Nothing Then
			component.enabled = False
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject, 15F)
	End Sub

	' Token: 0x060029B5 RID: 10677 RVA: 0x00184B14 File Offset: 0x00182F14
	Protected Overrides Sub OnDestroy()
		If Me.parent IsNot Nothing Then
			RemoveHandler Me.parent.OnDeathEvent, AddressOf Me.Die
			RemoveHandler Me.parent.OnPrimaryDeathEvent, AddressOf Me.OnPrimaryDeath
			RemoveHandler Me.parent.OnSecondaryDeathEvent, AddressOf Me.OnSecondaryDeath
		End If
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04003292 RID: 12946
	Protected current As RobotLevelRobotBodyPart.state

	' Token: 0x04003293 RID: 12947
	Protected properties As LevelProperties.Robot

	' Token: 0x04003294 RID: 12948
	Protected parent As RobotLevelRobot

	' Token: 0x04003295 RID: 12949
	Protected decreaseAttackDelayAmount As Single

	' Token: 0x04003296 RID: 12950
	Protected currentHealth As Single()

	' Token: 0x04003297 RID: 12951
	Protected primaryAttackDelay As Single

	' Token: 0x04003298 RID: 12952
	Protected secondaryAttackDelay As Single

	' Token: 0x04003299 RID: 12953
	Protected attackDelayMinus As Single

	' Token: 0x0400329A RID: 12954
	Protected isAttacking As Boolean

	' Token: 0x0400329B RID: 12955
	Protected damageReceiver As DamageReceiver

	' Token: 0x0400329C RID: 12956
	<SerializeField()>
	Protected deathEffect As Effect

	' Token: 0x0400329D RID: 12957
	<SerializeField()>
	Protected primary As GameObject

	' Token: 0x0400329E RID: 12958
	<SerializeField()>
	Protected secondary As GameObject

	' Token: 0x02000775 RID: 1909
	Protected Enum state
		' Token: 0x040032A0 RID: 12960
		primary
		' Token: 0x040032A1 RID: 12961
		secondary
		' Token: 0x040032A2 RID: 12962
		none
	End Enum
End Class
