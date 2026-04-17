Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004FE RID: 1278
Public Class BaronessLevelPeppermint
	Inherits ParrySwitch

	' Token: 0x0600167F RID: 5759 RVA: 0x000CA50B File Offset: 0x000C890B
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageDealer = DamageDealer.NewEnemy()
	End Sub

	' Token: 0x06001680 RID: 5760 RVA: 0x000CA51E File Offset: 0x000C891E
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x06001681 RID: 5761 RVA: 0x000CA53C File Offset: 0x000C893C
	Private Sub Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x06001682 RID: 5762 RVA: 0x000CA554 File Offset: 0x000C8954
	Public Sub Init(pos As Vector2, speed As Single)
		MyBase.transform.position = pos
		Me.speed = speed
		AudioManager.Play("level_baroness_candy_roll")
		MyBase.StartCoroutine(Me.fade_color_cr())
		MyBase.StartCoroutine(Me.move_cr())
	End Sub

	' Token: 0x06001683 RID: 5763 RVA: 0x000CA594 File Offset: 0x000C8994
	Protected Overridable Iterator Function fade_color_cr() As IEnumerator
		Dim fadeTime As Single = 0.7F
		Dim t As Single = 0F
		While t < fadeTime
			MyBase.GetComponent(Of SpriteRenderer)().color = New Color(t / fadeTime, t / fadeTime, t / fadeTime, 1F)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.GetComponent(Of SpriteRenderer)().color = New Color(1F, 1F, 1F, 1F)
		Yield Nothing
		Return
	End Function

	' Token: 0x06001684 RID: 5764 RVA: 0x000CA5B0 File Offset: 0x000C89B0
	Private Iterator Function move_cr() As IEnumerator
		Dim offsetX As Single = 220F
		Dim pos As Vector3 = MyBase.transform.position
		While True
			If MyBase.transform.position.x > -640F - offsetX Then
				pos.x = Mathf.MoveTowards(MyBase.transform.position.x, -640F - offsetX, Me.speed * CupheadTime.FixedDelta)
			Else
				Me.Die()
			End If
			MyBase.transform.position = pos
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06001685 RID: 5765 RVA: 0x000CA5CB File Offset: 0x000C89CB
	Private Sub Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06001686 RID: 5766 RVA: 0x000CA5D8 File Offset: 0x000C89D8
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		MyBase.OnParryPrePause(player)
		player.stats.ParryOneQuarter()
	End Sub

	' Token: 0x06001687 RID: 5767 RVA: 0x000CA5EC File Offset: 0x000C89EC
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		MyBase.IsParryable = False
		MyBase.StartCoroutine(Me.peppermintParryCooldown_cr())
	End Sub

	' Token: 0x06001688 RID: 5768 RVA: 0x000CA60C File Offset: 0x000C8A0C
	Private Iterator Function peppermintParryCooldown_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.coolDown
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.IsParryable = True
		Yield Nothing
		Return
	End Function

	' Token: 0x04001FDA RID: 8154
	Private damageDealer As DamageDealer

	' Token: 0x04001FDB RID: 8155
	Private speed As Single
End Class
