Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A7A RID: 2682
Public Class WeaponFirecrackerEXProjectile
	Inherits BasicProjectile

	' Token: 0x06004019 RID: 16409 RVA: 0x0022F36B File Offset: 0x0022D76B
	Protected Overrides Sub Start()
		MyBase.Start()
	End Sub

	' Token: 0x0600401A RID: 16410 RVA: 0x0022F373 File Offset: 0x0022D773
	Protected Overrides Sub OnCollisionEnemy(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionEnemy(hit, phase)
		If phase = CollisionPhase.Enter Then
			MyBase.StartCoroutine(Me.explosion_cr())
		End If
	End Sub

	' Token: 0x0600401B RID: 16411 RVA: 0x0022F390 File Offset: 0x0022D790
	Private Iterator Function explosion_cr() As IEnumerator
		Me.move = False
		MyBase.transform.SetScale(New Single?(Me.explosionSize), New Single?(Me.explosionSize), Nothing)
		Yield CupheadTime.WaitForSeconds(Me, Me.explosionDuration)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Return
	End Function

	' Token: 0x040046DD RID: 18141
	Public bulletLife As Single

	' Token: 0x040046DE RID: 18142
	Public explosionSize As Single

	' Token: 0x040046DF RID: 18143
	Public explosionDuration As Single
End Class
