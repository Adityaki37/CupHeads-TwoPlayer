Imports System
Imports UnityEngine

' Token: 0x020007E7 RID: 2023
Public Class SnowCultLevelBatDrip
	Inherits SnowCultLevelBatEffect

	' Token: 0x06002E55 RID: 11861 RVA: 0x001B4FD0 File Offset: 0x001B33D0
	Private Sub FixedUpdate()
		MyBase.transform.position += Me.vel * CupheadTime.FixedDelta
		Me.vel.y = Me.vel.y - Me.gravity * CupheadTime.FixedDelta
		If MyBase.transform.position.y <= CSng(Level.Current.Ground) + -20F Then
			Me.vel = Vector3.zero
			Me.gravity = 0F
			MyBase.animator.Play("Splat" + Me.colorString)
		End If
	End Sub

	' Token: 0x040036E7 RID: 14055
	Private Const GROUND_OFFSET As Single = -20F

	' Token: 0x040036E8 RID: 14056
	<SerializeField()>
	Private gravity As Single = 10F

	' Token: 0x040036E9 RID: 14057
	Public vel As Vector3
End Class
