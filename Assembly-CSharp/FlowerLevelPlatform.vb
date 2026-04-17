Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200060E RID: 1550
Public Class FlowerLevelPlatform
	Inherits LevelPlatform

	' Token: 0x06001F43 RID: 8003 RVA: 0x0011F294 File Offset: 0x0011D694
	Private Sub Start()
		Me.YPositionDown = Me.YPositionUp - 30F
		Me.YFall = Me.YPositionUp - 35F
		If Me.shadow IsNot Nothing Then
			Me.shadow.parent = Nothing
			Dim position As Vector3 = Me.shadow.position
			position.y = CSng(Level.Current.Ground)
			Me.shadow.position = position
		End If
		Me.startPos = MyBase.transform.position
		Me.startPos.y = Me.YPositionUp
		Me.endPos = MyBase.transform.position
		Me.endPos.y = Me.YPositionDown
		If Me.state = FlowerLevelPlatform.State.Down Then
			MyBase.transform.SetPosition(Nothing, New Single?(Me.YPositionUp), Nothing)
			Me.StartDown()
		Else
			MyBase.transform.SetPosition(Nothing, New Single?(Me.YPositionDown), Nothing)
			Me.StartUp()
		End If
	End Sub

	' Token: 0x06001F44 RID: 8004 RVA: 0x0011F3BD File Offset: 0x0011D7BD
	Public Sub StartDown()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.down_cr())
	End Sub

	' Token: 0x06001F45 RID: 8005 RVA: 0x0011F3D2 File Offset: 0x0011D7D2
	Public Sub StartUp()
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.up_cr())
	End Sub

	' Token: 0x06001F46 RID: 8006 RVA: 0x0011F3E7 File Offset: 0x0011D7E7
	Public Overrides Sub AddChild(player As Transform)
		MyBase.AddChild(player)
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.fall_cr())
	End Sub

	' Token: 0x06001F47 RID: 8007 RVA: 0x0011F403 File Offset: 0x0011D803
	Public Overrides Sub OnPlayerExit(player As Transform)
		MyBase.OnPlayerExit(player)
		Me.StartUp()
	End Sub

	' Token: 0x06001F48 RID: 8008 RVA: 0x0011F414 File Offset: 0x0011D814
	Private Iterator Function down_cr() As IEnumerator
		Yield New WaitForSeconds(0F)
		Yield MyBase.StartCoroutine(Me.goTo_cr(Me.YPositionUp, Me.YPositionDown, 3F, EaseUtils.EaseType.easeInOutSine))
		Me.StartUp()
		Return
	End Function

	' Token: 0x06001F49 RID: 8009 RVA: 0x0011F430 File Offset: 0x0011D830
	Private Iterator Function up_cr() As IEnumerator
		Yield New WaitForSeconds(0F)
		Yield MyBase.StartCoroutine(Me.goTo_cr(Me.YPositionDown, Me.YPositionUp, 3F, EaseUtils.EaseType.easeInOutSine))
		Me.StartDown()
		Return
	End Function

	' Token: 0x06001F4A RID: 8010 RVA: 0x0011F44C File Offset: 0x0011D84C
	Private Iterator Function fall_cr() As IEnumerator
		Dim time As Single = (1F - MyBase.transform.position.y / Me.YPositionDown) * 0.13F
		Yield MyBase.StartCoroutine(Me.goTo_cr(MyBase.transform.position.y, Me.YFall, time, EaseUtils.EaseType.easeOutSine))
		Yield MyBase.StartCoroutine(Me.goTo_cr(Me.YFall, Me.YPositionDown, 0.12F, EaseUtils.EaseType.easeInOutSine))
		Return
	End Function

	' Token: 0x06001F4B RID: 8011 RVA: 0x0011F468 File Offset: 0x0011D868
	Private Iterator Function goTo_cr(start As Single, [end] As Single, time As Single, ease As EaseUtils.EaseType) As IEnumerator
		Dim t As Single = 0F
		MyBase.transform.SetPosition(Nothing, New Single?(start), Nothing)
		While t < time
			Dim val As Single = t / time
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(ease, start, [end], val)), Nothing)
			t += Time.deltaTime
			Yield MyBase.StartCoroutine(MyBase.WaitForPause_CR())
		End While
		MyBase.transform.SetPosition(Nothing, New Single?([end]), Nothing)
		Return
	End Function

	' Token: 0x040027D9 RID: 10201
	Public YPositionUp As Single

	' Token: 0x040027DA RID: 10202
	Public Const TIME As Single = 3F

	' Token: 0x040027DB RID: 10203
	Public Const FALL_TIME As Single = 0.13F

	' Token: 0x040027DC RID: 10204
	Public Const FALL_BOUNCE_TIME As Single = 0.12F

	' Token: 0x040027DD RID: 10205
	Public Const DELAY As Single = 0F

	' Token: 0x040027DE RID: 10206
	Public Const FLOAT_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeInOutSine

	' Token: 0x040027DF RID: 10207
	Public Const FALL_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeOutSine

	' Token: 0x040027E0 RID: 10208
	Public Const FALL_BOUNCE_EASE As EaseUtils.EaseType = EaseUtils.EaseType.easeInOutSine

	' Token: 0x040027E1 RID: 10209
	<SerializeField()>
	Private state As FlowerLevelPlatform.State

	' Token: 0x040027E2 RID: 10210
	<SerializeField()>
	Private shadow As Transform

	' Token: 0x040027E3 RID: 10211
	Private startPos As Vector3

	' Token: 0x040027E4 RID: 10212
	Private endPos As Vector3

	' Token: 0x040027E5 RID: 10213
	Private YPositionDown As Single

	' Token: 0x040027E6 RID: 10214
	Private YFall As Single

	' Token: 0x0200060F RID: 1551
	Public Enum State
		' Token: 0x040027E8 RID: 10216
		Up
		' Token: 0x040027E9 RID: 10217
		Down
		' Token: 0x040027EA RID: 10218
		PlayerOn
	End Enum
End Class
