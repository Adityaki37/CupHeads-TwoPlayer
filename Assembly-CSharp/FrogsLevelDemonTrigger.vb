Imports System
Imports UnityEngine

' Token: 0x020006AA RID: 1706
Public Class FrogsLevelDemonTrigger
	Inherits AbstractCollidableObject

	' Token: 0x06002428 RID: 9256 RVA: 0x00153721 File Offset: 0x00151B21
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06002429 RID: 9257 RVA: 0x0015374C File Offset: 0x00151B4C
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		MyBase.GetComponent(Of Collider2D)().enabled = False
		MyBase.GetComponent(Of SpriteRenderer)().enabled = False
		Me.isTriggered = True
	End Sub

	' Token: 0x0600242A RID: 9258 RVA: 0x0015376D File Offset: 0x00151B6D
	Public Function getTrigger() As Boolean
		Return Me.isTriggered
	End Function

	' Token: 0x04002CEB RID: 11499
	Private damageReceiver As DamageReceiver

	' Token: 0x04002CEC RID: 11500
	Private isTriggered As Boolean
End Class
