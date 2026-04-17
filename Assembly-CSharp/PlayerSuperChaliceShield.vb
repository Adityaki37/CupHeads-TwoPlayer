Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A56 RID: 2646
Public Class PlayerSuperChaliceShield
	Inherits AbstractPlayerSuper

	' Token: 0x06003F10 RID: 16144 RVA: 0x002289D9 File Offset: 0x00226DD9
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.tag = "Untagged"
	End Sub

	' Token: 0x06003F11 RID: 16145 RVA: 0x002289EC File Offset: 0x00226DEC
	Protected Overrides Sub StartSuper()
		RemoveHandler Me.player.weaponManager.OnSuperStart, AddressOf Me.player.motor.StartSuper
		If Me.player.motor.Grounded Then
			RemoveHandler Me.player.weaponManager.OnSuperEnd, AddressOf Me.player.motor.OnSuperEnd
		End If
		MyBase.StartSuper()
		AudioManager.Play("player_super_chalice_shield")
		MyBase.StartCoroutine(Me.super_cr())
		Level.ScoringData.superMeterUsed += 5
	End Sub

	' Token: 0x06003F12 RID: 16146 RVA: 0x00228A8C File Offset: 0x00226E8C
	Private Sub CreateHeart()
		Me.shieldHeart = Global.UnityEngine.[Object].Instantiate(Of GameObject)(Me.shieldHeartPrefab)
		Me.shieldHeart.transform.position = Me.shieldHeartSpawnPos.position
		Me.shieldHeartScript = Me.shieldHeart.GetComponent(Of PlayerSuperChaliceShieldHeart)()
		Me.shieldHeartScript.player = Me.player.transform
		Me.player.stats.SetChaliceShield(True)
		Me.player.damageReceiver.Invulnerable(0.1F)
	End Sub

	' Token: 0x06003F13 RID: 16147 RVA: 0x00228B12 File Offset: 0x00226F12
	Private Sub LetPlayerMove()
		Me.Fire()
		Me.EndSuper(True)
	End Sub

	' Token: 0x06003F14 RID: 16148 RVA: 0x00228B24 File Offset: 0x00226F24
	Private Iterator Function super_cr() As IEnumerator
		If Not Me.player.motor.Grounded Then
			MyBase.animator.Play("SuperAir")
		End If
		While Me.player AndAlso Not Me.player.stats.ChaliceShieldOn
			Yield Nothing
		End While
		While Me.player AndAlso Me.player.stats.ChaliceShieldOn
			Yield Nothing
		End While
		Me.shieldHeartScript.Destroy()
		If Me.player Then
			Me.player.damageReceiver.OnRevive(Vector3.zero)
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x04004626 RID: 17958
	<SerializeField()>
	Private shadowOffset As Vector3

	' Token: 0x04004627 RID: 17959
	<SerializeField()>
	Private shieldHeartPrefab As GameObject

	' Token: 0x04004628 RID: 17960
	Private shieldHeart As GameObject

	' Token: 0x04004629 RID: 17961
	<SerializeField()>
	Private shieldHeartSpawnPos As Transform

	' Token: 0x0400462A RID: 17962
	Private shieldHeartScript As PlayerSuperChaliceShieldHeart
End Class
