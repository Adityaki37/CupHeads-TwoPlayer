Imports System
Imports UnityEngine

' Token: 0x0200071E RID: 1822
Public Class PirateLevelBoatContainer
	Inherits AbstractPausableComponent

	' Token: 0x060027AE RID: 10158 RVA: 0x00173FF6 File Offset: 0x001723F6
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.startPos = MyBase.transform.position
		Me.endPos = Me.startPos + New Vector3(0F, Me.targetY, 0F)
	End Sub

	' Token: 0x060027AF RID: 10159 RVA: 0x00174038 File Offset: 0x00172438
	Private Sub Update()
		If PauseManager.state = PauseManager.State.Paused OrElse Me.state = PirateLevelBoatContainer.State.[Static] Then
			Return
		End If
		Dim num As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0F, 1F, Mathf.PingPong(Me.time, 1F))
		MyBase.transform.position = Vector3.Lerp(Me.startPos, Me.endPos, num)
		Me.time += Time.deltaTime / 2F
	End Sub

	' Token: 0x060027B0 RID: 10160 RVA: 0x001740B4 File Offset: 0x001724B4
	Public Sub EndBobbing()
		MyBase.transform.position = Me.startPos
		Me.state = PirateLevelBoatContainer.State.[Static]
	End Sub

	' Token: 0x0400306B RID: 12395
	<SerializeField()>
	Private targetY As Single

	' Token: 0x0400306C RID: 12396
	Private state As PirateLevelBoatContainer.State

	' Token: 0x0400306D RID: 12397
	Private startPos As Vector3

	' Token: 0x0400306E RID: 12398
	Private endPos As Vector3

	' Token: 0x0400306F RID: 12399
	Private time As Single

	' Token: 0x0200071F RID: 1823
	Public Enum State
		' Token: 0x04003071 RID: 12401
		Bobbing
		' Token: 0x04003072 RID: 12402
		ToStatic
		' Token: 0x04003073 RID: 12403
		[Static]
	End Enum
End Class
