Imports System
Imports UnityEngine

' Token: 0x020004C5 RID: 1221
Public Class AirplaneLevelSecretTerrierBulletShrapnel
	Inherits BasicProjectile

	' Token: 0x17000312 RID: 786
	' (get) Token: 0x06001489 RID: 5257 RVA: 0x000B839F File Offset: 0x000B679F
	Protected Overrides ReadOnly Property Direction As Vector3
		Get
			Return-MyBase.transform.up
		End Get
	End Property

	' Token: 0x0600148A RID: 5258 RVA: 0x000B83B4 File Offset: 0x000B67B4
	Public Overrides Function Create() As AbstractProjectile
		Dim airplaneLevelSecretTerrierBulletShrapnel As AirplaneLevelSecretTerrierBulletShrapnel = CType(MyBase.Create(), AirplaneLevelSecretTerrierBulletShrapnel)
		airplaneLevelSecretTerrierBulletShrapnel.anim.Play("Move", 0, CSng(Global.UnityEngine.Random.Range(0, 1)))
		Return airplaneLevelSecretTerrierBulletShrapnel
	End Function

	' Token: 0x04001DDF RID: 7647
	<SerializeField()>
	Private anim As Animator
End Class
