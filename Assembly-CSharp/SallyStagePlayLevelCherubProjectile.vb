Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007AA RID: 1962
Public Class SallyStagePlayLevelCherubProjectile
	Inherits BasicProjectile

	' Token: 0x06002C18 RID: 11288 RVA: 0x0019EBD0 File Offset: 0x0019CFD0
	Public Function Create(position As Vector3, rotation As Single, speed As Single) As SallyStagePlayLevelCherubProjectile
		Return TryCast(MyBase.Create(position, rotation, speed), SallyStagePlayLevelCherubProjectile)
	End Function

	' Token: 0x06002C19 RID: 11289 RVA: 0x0019EBF2 File Offset: 0x0019CFF2
	Protected Overrides Sub Start()
		MyBase.Start()
		Me.selfAnimator = MyBase.GetComponent(Of Animator)()
		AudioManager.Play("sally_cherub_vocal_wheelin")
		Me.emitAudioFromObject.Add("sally_cherub_vocal_wheelin")
		MyBase.StartCoroutine(Me.wait_to_launch())
	End Sub

	' Token: 0x06002C1A RID: 11290 RVA: 0x0019EC30 File Offset: 0x0019D030
	Private Iterator Function wait_to_launch() As IEnumerator
		While MyBase.transform.position.x < -400F
			Yield Nothing
		End While
		Me.selfAnimator.SetTrigger("OnLaunch")
		Yield Me.selfAnimator.WaitForAnimationToEnd(Me, "Cherub_Push", False, True)
		MyBase.StartCoroutine(Me.cherub_leaves_cr())
		Return
	End Function

	' Token: 0x06002C1B RID: 11291 RVA: 0x0019EC4C File Offset: 0x0019D04C
	Private Iterator Function cherub_leaves_cr() As IEnumerator
		Dim time As Single = 1F
		Dim t As Single = 0F
		Dim start As Single = Me.cherub.transform.position.x
		Dim [end] As Single = -740F
		While t < time
			Dim val As Single = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0F, 1F, t / time)
			Me.cherub.transform.SetPosition(New Single?(Mathf.Lerp(start, [end], val)), Nothing, Nothing)
			t += CupheadTime.Delta
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x06002C1C RID: 11292 RVA: 0x0019EC67 File Offset: 0x0019D067
	Private Sub CherubPush()
		Me.move = False
		MyBase.StartCoroutine(Me.launch_firewheel_cr())
	End Sub

	' Token: 0x06002C1D RID: 11293 RVA: 0x0019EC80 File Offset: 0x0019D080
	Private Iterator Function launch_firewheel_cr() As IEnumerator
		Me.firewheel.PlaySound()
		While Me.firewheel.transform.position.x < 740F
			Me.firewheel.transform.position += Vector3.right * Me.Speed * CupheadTime.FixedDelta
			Yield New WaitForFixedUpdate()
		End While
		Return
	End Function

	' Token: 0x040034CD RID: 13517
	Private Const DIST_TO_LAUNCH As Single = -400F

	' Token: 0x040034CE RID: 13518
	Private Const OFFSET As Single = 100F

	' Token: 0x040034CF RID: 13519
	<SerializeField()>
	Private cherub As GameObject

	' Token: 0x040034D0 RID: 13520
	<SerializeField()>
	Private firewheel As SallyStagePlayLevelFirewheel

	' Token: 0x040034D1 RID: 13521
	Private selfAnimator As Animator
End Class
