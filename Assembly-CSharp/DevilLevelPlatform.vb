Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200057C RID: 1404
Public Class DevilLevelPlatform
	Inherits AbstractPausableComponent

	' Token: 0x06001AB7 RID: 6839 RVA: 0x000F4EB8 File Offset: 0x000F32B8
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.animator.Play("Platform_" + Me.type.ToString())
		Me.baseY = MyBase.transform.position.y
	End Sub

	' Token: 0x06001AB8 RID: 6840 RVA: 0x000F4F0A File Offset: 0x000F330A
	Public Sub Raise(speed As Single, height As Single, delay As Single)
		Me.state = DevilLevelPlatform.State.Raising
		MyBase.StartCoroutine(Me.raise_cr(speed, height, delay))
	End Sub

	' Token: 0x06001AB9 RID: 6841 RVA: 0x000F4F24 File Offset: 0x000F3324
	Private Iterator Function raise_cr(speed As Single, height As Single, delay As Single) As IEnumerator
		Dim t As Single = 0F
		Dim moveTime As Single = height / speed
		While t < moveTime
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, Me.baseY, Me.baseY + height, t / moveTime)), Nothing)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(Me.baseY + height), Nothing)
		Yield CupheadTime.WaitForSeconds(Me, delay)
		t = 0F
		While t < moveTime
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, Me.baseY + height, Me.baseY, t / moveTime)), Nothing)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		MyBase.transform.SetPosition(Nothing, New Single?(Me.baseY), Nothing)
		Me.state = DevilLevelPlatform.State.Idle
		Return
	End Function

	' Token: 0x06001ABA RID: 6842 RVA: 0x000F4F54 File Offset: 0x000F3354
	Public Sub Lower(speed As Single)
		Me.state = DevilLevelPlatform.State.Lowering
		MyBase.StartCoroutine(Me.lower_cr(speed))
	End Sub

	' Token: 0x06001ABB RID: 6843 RVA: 0x000F4F6C File Offset: 0x000F336C
	Private Iterator Function lower_cr(speed As Single) As IEnumerator
		Dim t As Single = 0F
		Dim moveTime As Single = 300F / speed
		While t < moveTime
			MyBase.transform.SetPosition(Nothing, New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, Me.baseY, Me.baseY - 300F, t / moveTime)), Nothing)
			t += CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Me.Die()
		Return
	End Function

	' Token: 0x06001ABC RID: 6844 RVA: 0x000F4F90 File Offset: 0x000F3390
	Public Sub Die()
		For Each abstractPlayerController As AbstractPlayerController In MyBase.GetComponentsInChildren(Of AbstractPlayerController)()
			If Not(abstractPlayerController Is Nothing) Then
				abstractPlayerController.transform.parent = Nothing
			End If
		Next
		MyBase.gameObject.SetActive(False)
		Me.state = DevilLevelPlatform.State.Dead
	End Sub

	' Token: 0x040023E4 RID: 9188
	Private Const LOWER_DISTANCE As Single = 300F

	' Token: 0x040023E5 RID: 9189
	Public type As DevilLevelPlatform.PlatformType

	' Token: 0x040023E6 RID: 9190
	Public state As DevilLevelPlatform.State

	' Token: 0x040023E7 RID: 9191
	Private baseY As Single

	' Token: 0x0200057D RID: 1405
	Public Enum PlatformType
		' Token: 0x040023E9 RID: 9193
		A
		' Token: 0x040023EA RID: 9194
		B
		' Token: 0x040023EB RID: 9195
		C
		' Token: 0x040023EC RID: 9196
		D
		' Token: 0x040023ED RID: 9197
		E
	End Enum

	' Token: 0x0200057E RID: 1406
	Public Enum State
		' Token: 0x040023EF RID: 9199
		Idle
		' Token: 0x040023F0 RID: 9200
		Raising
		' Token: 0x040023F1 RID: 9201
		Lowering
		' Token: 0x040023F2 RID: 9202
		Dead
	End Enum
End Class
