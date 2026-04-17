Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x0200079E RID: 1950
Public Class RumRunnersLevelSnoutTongue
	Inherits ParrySwitch

	' Token: 0x1400004A RID: 74
	' (add) Token: 0x06002B65 RID: 11109 RVA: 0x001940A0 File Offset: 0x001924A0
	' (remove) Token: 0x06002B66 RID: 11110 RVA: 0x001940D8 File Offset: 0x001924D8
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnPlayerCollision As CollisionChild.OnCollisionHandler

	' Token: 0x06002B67 RID: 11111 RVA: 0x0019410E File Offset: 0x0019250E
	Private Sub OnEnable()
		AddHandler MyBase.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.onPlayerCollision
	End Sub

	' Token: 0x06002B68 RID: 11112 RVA: 0x00194127 File Offset: 0x00192527
	Private Sub OnDisable()
		RemoveHandler MyBase.GetComponent(Of CollisionChild)().OnPlayerCollision, AddressOf Me.onPlayerCollision
	End Sub

	' Token: 0x06002B69 RID: 11113 RVA: 0x00194140 File Offset: 0x00192540
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.gameObject.tag = "Enemy"
		Me.collisionChild = MyBase.GetComponent(Of CollisionChild)()
	End Sub

	' Token: 0x06002B6A RID: 11114 RVA: 0x00194164 File Offset: 0x00192564
	Public Overrides Sub OnParryPrePause(player As AbstractPlayerController)
		If Me.parrySpark Then
			Me.parrySpark.Create(player.transform.position)
		End If
		MyBase.FirePrePauseEvent()
		player.stats.ParryOneQuarter()
	End Sub

	' Token: 0x06002B6B RID: 11115 RVA: 0x0019419E File Offset: 0x0019259E
	Public Overrides Sub OnParryPostPause(player As AbstractPlayerController)
		MyBase.OnParryPostPause(player)
		MyBase.IsParryable = False
		Me.collisionChild.enabled = False
		MyBase.StartCoroutine(Me.parryCooldown_cr())
	End Sub

	' Token: 0x06002B6C RID: 11116 RVA: 0x001941C8 File Offset: 0x001925C8
	Private Iterator Function parryCooldown_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.coolDown
			t += CupheadTime.Delta
			Yield Nothing
		End While
		MyBase.IsParryable = True
		Me.collisionChild.enabled = True
		Return
	End Function

	' Token: 0x06002B6D RID: 11117 RVA: 0x001941E3 File Offset: 0x001925E3
	Private Sub onPlayerCollision(hit As GameObject, phase As CollisionPhase)
		If MyBase.IsParryable AndAlso Me.OnPlayerCollision IsNot Nothing Then
			Me.OnPlayerCollision(hit, phase)
		End If
	End Sub

	' Token: 0x0400341A RID: 13338
	Private collisionChild As CollisionChild
End Class
