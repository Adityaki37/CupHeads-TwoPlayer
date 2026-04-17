Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A87 RID: 2695
Public Class PlayerLevelSpreadExChild
	Inherits BasicProjectile

	' Token: 0x0600406B RID: 16491 RVA: 0x0023161F File Offset: 0x0022FA1F
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.damageDealer.SetDamageSource(DamageDealer.DamageSource.Ex)
		MyBase.StartCoroutine(Me.trail_cr())
	End Sub

	' Token: 0x0600406C RID: 16492 RVA: 0x00231640 File Offset: 0x0022FA40
	Private Iterator Function trail_cr() As IEnumerator
		While True
			Yield CupheadTime.WaitForSeconds(Me, 0.15F)
			Dim t As Transform = Me.trailEffectPrefab.Create(MyBase.transform.position).transform
			t.SetParent(MyBase.transform)
			t.ResetLocalTransforms()
			t.AddPositionForward2D(100F)
			t.SetParent(Nothing)
		End While
		Return
	End Function

	' Token: 0x0600406D RID: 16493 RVA: 0x0023165B File Offset: 0x0022FA5B
	Private Sub _OnDieAnimComplete()
		Me.StopAllCoroutines()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x04004735 RID: 18229
	Private Const TRAIL_TIME As Single = 0.15F

	' Token: 0x04004736 RID: 18230
	<SerializeField()>
	Private trailEffectPrefab As Effect
End Class
