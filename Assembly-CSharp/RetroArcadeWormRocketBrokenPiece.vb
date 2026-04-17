Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200076D RID: 1901
Public Class RetroArcadeWormRocketBrokenPiece
	Inherits BasicProjectile

	' Token: 0x06002958 RID: 10584 RVA: 0x00181C18 File Offset: 0x00180018
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.Damage = PlayerManager.DamageMultiplier
		MyBase.StartCoroutine(Me.turnOnCollider_cr())
	End Sub

	' Token: 0x06002959 RID: 10585 RVA: 0x00181C38 File Offset: 0x00180038
	Private Iterator Function turnOnCollider_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 0.25F)
		MyBase.GetComponent(Of Collider2D)().enabled = True
		Return
	End Function

	' Token: 0x0600295A RID: 10586 RVA: 0x00181C53 File Offset: 0x00180053
	Protected Overrides Sub Die()
		MyBase.Die()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub
End Class
