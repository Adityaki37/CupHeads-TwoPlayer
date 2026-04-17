Imports System
Imports UnityEngine

' Token: 0x02000A86 RID: 2694
Public Class PlayerLevelSpreadEx
	Inherits AbstractProjectile

	' Token: 0x06004065 RID: 16485 RVA: 0x0023156C File Offset: 0x0022F96C
	Public Sub Init(speed As Single, damage As Single, childCount As Integer, radius As Single)
		Dim num As Single = CSng((360 / childCount))
		Dim meterScoreTracker As MeterScoreTracker = New MeterScoreTracker(MeterScoreTracker.Type.Ex)
		For i As Integer = 0 To childCount - 1
			Dim basicProjectile As BasicProjectile = Me.childPrefab.Create(MyBase.transform.position, num * CSng(i), Vector2.one, speed)
			Me.childPrefab.Damage = damage
			Me.childPrefab.Speed = speed
			Me.childPrefab.PlayerId = Me.PlayerId
			Me.childPrefab.transform.AddPositionForward2D(radius)
			meterScoreTracker.Add(basicProjectile)
		Next
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x06004066 RID: 16486 RVA: 0x0023160E File Offset: 0x0022FA0E
	Protected Overrides Function GetVariants() As Integer
		Return 1
	End Function

	' Token: 0x06004067 RID: 16487 RVA: 0x00231611 File Offset: 0x0022FA11
	Protected Overrides Sub SetBool([boolean] As String, b As Boolean)
	End Sub

	' Token: 0x06004068 RID: 16488 RVA: 0x00231613 File Offset: 0x0022FA13
	Protected Overrides Sub SetInt([integer] As String, i As Integer)
	End Sub

	' Token: 0x06004069 RID: 16489 RVA: 0x00231615 File Offset: 0x0022FA15
	Protected Overrides Sub SetTrigger(trigger As String)
	End Sub

	' Token: 0x04004734 RID: 18228
	<SerializeField()>
	Private childPrefab As PlayerLevelSpreadExChild
End Class
