Imports System
Imports UnityEngine

' Token: 0x02000A71 RID: 2673
Public Class WeaponChargeProjectile
	Inherits BasicProjectile

	' Token: 0x06003FD8 RID: 16344 RVA: 0x0022D584 File Offset: 0x0022B984
	Protected Overrides Sub Die()
		If Me.fullyCharged Then
			Dim vector As Vector2 = MathUtils.AngleToDirection(MyBase.transform.eulerAngles.z) * 75F
			MyBase.transform.AddPosition(vector.x, vector.y, 0F)
		End If
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, CSng(Global.UnityEngine.Random.Range(0, 360)))
		MyBase.Die()
		If Me.fullyCharged Then
			AudioManager.Play("player_weapon_charge_full_impact")
			Me.emitAudioFromObject.Add("player_weapon_charge_full_impact")
		End If
	End Sub

	' Token: 0x040046B2 RID: 18098
	Public fullyCharged As Boolean
End Class
