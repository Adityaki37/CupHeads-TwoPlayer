Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020006B7 RID: 1719
Public Class FrogsLevelTigerBullet
	Inherits AbstractFrogsLevelSlotBullet

	' Token: 0x06002474 RID: 9332 RVA: 0x00155A03 File Offset: 0x00153E03
	Protected Overrides Sub Start()
		MyBase.Start()
		AddHandler Me.bullet.OnPlayerCollision, AddressOf MyBase.DealDamage
		MyBase.StartCoroutine(Me.bullet_cr())
	End Sub

	' Token: 0x06002475 RID: 9333 RVA: 0x00155A30 File Offset: 0x00153E30
	Private Iterator Function bullet_cr() As IEnumerator
		Dim t As Single = 0F
		Dim trans As Transform = Me.bullet.transform
		Dim start As Single = trans.localPosition.y
		Dim [end] As Single = start + 500F
		While True
			t = 0F
			AudioManager.Play("level_frogs_ball_platform_ball_launch")
			While t < 0.5F
				Dim val As Single = t / 0.5F
				Dim y As Single = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, start, [end], val)
				trans.SetLocalPosition(Nothing, New Single?(y), Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
			t = 0F
			While t < 0.5F
				Dim val2 As Single = t / 0.5F
				Dim y2 As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, [end], start, val2)
				trans.SetLocalPosition(Nothing, New Single?(y2), Nothing)
				t += CupheadTime.Delta
				Yield Nothing
			End While
		End While
		Return
	End Function

	' Token: 0x04002D23 RID: 11555
	Private Const BULLET_TIME As Single = 0.5F

	' Token: 0x04002D24 RID: 11556
	Private Const BULLET_HEIGHT As Single = 500F

	' Token: 0x04002D25 RID: 11557
	Public bullet As CollisionChild
End Class
