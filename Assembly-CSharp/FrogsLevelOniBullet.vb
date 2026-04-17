Imports System
Imports UnityEngine

' Token: 0x020006B4 RID: 1716
Public Class FrogsLevelOniBullet
	Inherits AbstractFrogsLevelSlotBullet

	' Token: 0x0600246D RID: 9325 RVA: 0x0015593C File Offset: 0x00153D3C
	Public Function Create(pos As Vector2, speed As Single, properties As LevelProperties.Frogs.Demon) As FrogsLevelOniBullet
		Dim frogsLevelOniBullet As FrogsLevelOniBullet = TryCast(MyBase.Create(pos, speed), FrogsLevelOniBullet)
		frogsLevelOniBullet.properties = properties
		Return frogsLevelOniBullet
	End Function

	' Token: 0x0600246E RID: 9326 RVA: 0x0015595F File Offset: 0x00153D5F
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.SetSize()
	End Sub

	' Token: 0x0600246F RID: 9327 RVA: 0x00155970 File Offset: 0x00153D70
	Private Sub SetSize()
		Me.parryBox.SetScale(Nothing, New Single?(Me.properties.demonParryHeight), Nothing)
		Me.hurtBox.SetScale(Nothing, New Single?(Me.properties.demonFlameHeight), Nothing)
	End Sub

	' Token: 0x04002D20 RID: 11552
	<SerializeField()>
	Private parryBox As Transform

	' Token: 0x04002D21 RID: 11553
	<SerializeField()>
	Private hurtBox As Transform

	' Token: 0x04002D22 RID: 11554
	Private properties As LevelProperties.Frogs.Demon
End Class
