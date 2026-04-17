Imports System
Imports UnityEngine

' Token: 0x02000509 RID: 1289
Public Class BatLevelMiniBat
	Inherits BasicSineProjectile

	' Token: 0x060016DD RID: 5853 RVA: 0x000CDA54 File Offset: 0x000CBE54
	Public Function Create(pos As Vector2, rotation As Single, velocity As Single, sinVelocity As Single, sinSize As Single, health As Single) As BatLevelMiniBat
		Dim batLevelMiniBat As BatLevelMiniBat = TryCast(MyBase.Create(pos, rotation, velocity, sinVelocity, sinSize), BatLevelMiniBat)
		batLevelMiniBat.health = health
		Return batLevelMiniBat
	End Function

	' Token: 0x060016DE RID: 5854 RVA: 0x000CDA7D File Offset: 0x000CBE7D
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x060016DF RID: 5855 RVA: 0x000CDAA8 File Offset: 0x000CBEA8
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		Me.health -= info.damage
		If Me.health < 0F Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x04002028 RID: 8232
	Private damageReceiver As DamageReceiver

	' Token: 0x04002029 RID: 8233
	Private health As Single
End Class
