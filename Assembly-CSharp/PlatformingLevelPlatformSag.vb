Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000909 RID: 2313
Public Class PlatformingLevelPlatformSag
	Inherits LevelPlatform

	' Token: 0x06003646 RID: 13894 RVA: 0x000C2C54 File Offset: 0x000C1054
	Private Sub Start()
		Me.localPosY = MyBase.transform.localPosition.y
	End Sub

	' Token: 0x06003647 RID: 13895 RVA: 0x000C2C7A File Offset: 0x000C107A
	Public Overrides Sub AddChild(player As Transform)
		MyBase.AddChild(player)
		If MyBase.gameObject.activeInHierarchy Then
			MyBase.StartCoroutine(Me.fall_cr())
		End If
	End Sub

	' Token: 0x06003648 RID: 13896 RVA: 0x000C2CA0 File Offset: 0x000C10A0
	Public Overrides Sub OnPlayerExit(player As Transform)
		MyBase.OnPlayerExit(player)
		If MyBase.gameObject.activeInHierarchy Then
			MyBase.StartCoroutine(Me.go_up_cr())
		End If
	End Sub

	' Token: 0x06003649 RID: 13897 RVA: 0x000C2CC8 File Offset: 0x000C10C8
	Private Iterator Function goTo_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		Dim t As Single = 0F
		MyBase.transform.SetLocalPosition(Nothing, New Single?(start), Nothing)
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetLocalPosition(Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing)
			t += Time.deltaTime
			Yield MyBase.StartCoroutine(MyBase.WaitForPause_CR())
		End While
		MyBase.transform.SetLocalPosition(Nothing, New Single?([end]), Nothing)
		Return
	End Function

	' Token: 0x0600364A RID: 13898 RVA: 0x000C2D00 File Offset: 0x000C1100
	Private Iterator Function fall_cr() As IEnumerator
		Yield MyBase.StartCoroutine(Me.goTo_cr(MyBase.transform.localPosition.y, Me.localPosY - Me.sagAmount, 0.4F, EaseUtils.EaseType.easeOutBounce))
		Return
	End Function

	' Token: 0x0600364B RID: 13899 RVA: 0x000C2D1C File Offset: 0x000C111C
	Private Iterator Function go_up_cr() As IEnumerator
		Yield MyBase.StartCoroutine(Me.goTo_cr(MyBase.transform.localPosition.y, Me.localPosY, 0.6F, EaseUtils.EaseType.easeOutBounce))
		Return
	End Function

	' Token: 0x04003E3C RID: 15932
	<SerializeField()>
	Private sagAmount As Single = 30F

	' Token: 0x04003E3D RID: 15933
	Private Const FALL_BOUNCE_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeOutBounce

	' Token: 0x04003E3E RID: 15934
	Public Const FALL_TIME As Single = 0.4F

	' Token: 0x04003E3F RID: 15935
	Public Const RISE_TIME As Single = 0.6F

	' Token: 0x04003E40 RID: 15936
	Private localPosY As Single
End Class
