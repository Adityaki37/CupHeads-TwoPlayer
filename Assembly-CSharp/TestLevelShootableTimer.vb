Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004AE RID: 1198
Public Class TestLevelShootableTimer
	Inherits AbstractCollidableObject

	' Token: 0x0600138B RID: 5003 RVA: 0x000AC0CC File Offset: 0x000AA4CC
	Private Sub Start()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
		AddHandler Me.child.OnDamageTaken, AddressOf Me.OnDamageTaken
		MyBase.StartCoroutine(Me.timer_cr())
	End Sub

	' Token: 0x0600138C RID: 5004 RVA: 0x000AC120 File Offset: 0x000AA520
	Private Sub Update()
		If Input.GetKeyDown(KeyCode.T) Then
			Me.timerStarted = True
		End If
	End Sub

	' Token: 0x0600138D RID: 5005 RVA: 0x000AC135 File Offset: 0x000AA535
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.timerStarted Then
			Me.damageTaken += info.damage
		End If
	End Sub

	' Token: 0x0600138E RID: 5006 RVA: 0x000AC158 File Offset: 0x000AA558
	Private Iterator Function timer_cr() As IEnumerator
		While True
			Dim t As Single = 0F
			If Me.timerStarted Then
				While t < Me.maxTime
					t += CupheadTime.Delta
					Yield Nothing
				End While
				Yield Nothing
				Me.damageTaken = 0F
				Me.timerStarted = False
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04001C9A RID: 7322
	<SerializeField()>
	Private maxTime As Single = 3F

	' Token: 0x04001C9B RID: 7323
	<SerializeField()>
	Private child As DamageReceiver

	' Token: 0x04001C9C RID: 7324
	Private damageReceiver As DamageReceiver

	' Token: 0x04001C9D RID: 7325
	Private damageTaken As Single

	' Token: 0x04001C9E RID: 7326
	Private timerStarted As Boolean
End Class
