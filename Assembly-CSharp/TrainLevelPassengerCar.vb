Imports System
Imports UnityEngine

' Token: 0x02000823 RID: 2083
Public Class TrainLevelPassengerCar
	Inherits AbstractTrainLevelTrainCar

	' Token: 0x06003060 RID: 12384 RVA: 0x001C81B4 File Offset: 0x001C65B4
	Public Sub Explode(i As Integer)
		MyBase.animator.SetInteger("State", i)
		MyBase.animator.SetTrigger("OnDamaged")
		If i <> 0 Then
			If i = 1 Then
				Me.explosionEffects(1).Create(MyBase.transform.position)
			End If
		Else
			Me.explosionEffects(0).Create(MyBase.transform.position)
		End If
	End Sub

	' Token: 0x06003061 RID: 12385 RVA: 0x001C8235 File Offset: 0x001C6635
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
		Me.explosionEffects = Nothing
	End Sub

	' Token: 0x0400390C RID: 14604
	<SerializeField()>
	Private explosionEffects As Effect()
End Class
