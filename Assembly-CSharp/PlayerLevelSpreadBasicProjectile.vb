Imports System
Imports UnityEngine

' Token: 0x02000A85 RID: 2693
Public Class PlayerLevelSpreadBasicProjectile
	Inherits BasicProjectile

	' Token: 0x06004061 RID: 16481 RVA: 0x002314C4 File Offset: 0x0022F8C4
	Protected Overrides Sub OnDieDistance()
		If MyBase.dead Then
			Return
		End If
		Me.Die()
		MyBase.animator.SetTrigger("OnDistanceDie")
	End Sub

	' Token: 0x06004062 RID: 16482 RVA: 0x002314E8 File Offset: 0x0022F8E8
	Protected Overrides Sub Die()
		MyBase.Die()
		MyBase.transform.SetEulerAngles(New Single?(0F), New Single?(0F), New Single?(CSng(Global.UnityEngine.Random.Range(0, 360))))
		MyBase.transform.SetScale(New Single?(CSng(MathUtils.PlusOrMinus())), New Single?(CSng(MathUtils.PlusOrMinus())), New Single?(1F))
	End Sub

	' Token: 0x06004063 RID: 16483 RVA: 0x00231556 File Offset: 0x0022F956
	Private Sub _OnDieAnimComplete()
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub
End Class
