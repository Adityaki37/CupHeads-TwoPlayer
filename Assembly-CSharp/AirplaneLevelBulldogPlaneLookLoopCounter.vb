Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004B7 RID: 1207
Public Class AirplaneLevelBulldogPlaneLookLoopCounter
	Inherits MonoBehaviour

	' Token: 0x060013E6 RID: 5094 RVA: 0x000B13E6 File Offset: 0x000AF7E6
	Private Sub OnDestroy()
		Me.WORKAROUND_NullifyFields()
	End Sub

	' Token: 0x060013E7 RID: 5095 RVA: 0x000B13EE File Offset: 0x000AF7EE
	Private Sub aniEvent_IncreaseIdleLookLoopCount()
		Me.bullDogPlane.SetInteger("IdleLoopCount", Me.bullDogPlane.GetInteger("IdleLoopCount") + 1)
	End Sub

	' Token: 0x060013E8 RID: 5096 RVA: 0x000B1412 File Offset: 0x000AF812
	Private Sub AniEvent_RecedeIntoDistance()
		MyBase.StartCoroutine(Me.recede_cr())
	End Sub

	' Token: 0x060013E9 RID: 5097 RVA: 0x000B1424 File Offset: 0x000AF824
	Private Iterator Function recede_cr() As IEnumerator
		Dim startTime As Single = Me.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime
		Dim startPos As Vector3 = MyBase.transform.position
		Dim endPos As Vector3 = New Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 100F, MyBase.transform.position.z)
		endPos = Vector3.Lerp(startPos, endPos, 0.8F)
		While Me.bullDogPlane.GetCurrentAnimatorStateInfo(0).IsName("Death")
			Dim t As Single = Mathf.InverseLerp(startTime, 1F, Me.bullDogPlane.GetCurrentAnimatorStateInfo(0).normalizedTime)
			MyBase.transform.position = Vector3.Lerp(startPos, endPos, EaseUtils.EaseInSine(0F, 1F, t))
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060013EA RID: 5098 RVA: 0x000B143F File Offset: 0x000AF83F
	Private Sub AnimationEvent_SFX_DOGFIGHT_Intro_BulldogPlaneFlyby()
		AudioManager.Play("sfx_dlc_dogfight_bulldogplane_introflyby")
	End Sub

	' Token: 0x060013EB RID: 5099 RVA: 0x000B144B File Offset: 0x000AF84B
	Private Sub AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectDown()
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectdown")
	End Sub

	' Token: 0x060013EC RID: 5100 RVA: 0x000B1457 File Offset: 0x000AF857
	Private Sub AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectUp()
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectUp")
	End Sub

	' Token: 0x060013ED RID: 5101 RVA: 0x000B1463 File Offset: 0x000AF863
	Private Sub AnimationEvent_SFX_DOGFIGHT_Bulldog_EjectLeverPull()
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_ejectleverpull")
	End Sub

	' Token: 0x060013EE RID: 5102 RVA: 0x000B146F File Offset: 0x000AF86F
	Private Sub AnimationEvent_SFX_DOGFIGHT_Bulldog_LandsCockpit()
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_landscockpit")
	End Sub

	' Token: 0x060013EF RID: 5103 RVA: 0x000B147B File Offset: 0x000AF87B
	Private Sub SFX_DOGFIGHT_Bulldog_WingExtend_WhimperOut()
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_whimperout")
	End Sub

	' Token: 0x060013F0 RID: 5104 RVA: 0x000B1487 File Offset: 0x000AF887
	Private Sub SFX_DOGFIGHT_Bulldog_WingExtend_WhistleOut()
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_Whistle_Out")
	End Sub

	' Token: 0x060013F1 RID: 5105 RVA: 0x000B1493 File Offset: 0x000AF893
	Private Sub AnimationEvent_SFX_DOGFIGHT_BulldogPlane_WingStretchOut()
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_WingStretch_Out")
	End Sub

	' Token: 0x060013F2 RID: 5106 RVA: 0x000B149F File Offset: 0x000AF89F
	Private Sub AnimationEvent_SFX_DOGFIGHT_BulldogPlane_WingStretchIn()
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_WingStretch_In")
	End Sub

	' Token: 0x060013F3 RID: 5107 RVA: 0x000B14AB File Offset: 0x000AF8AB
	Private Sub AnimationEvent_SFX_DOGFIGHT_BulldogPlane_DiePlaneExplodes()
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_planeexplodes")
	End Sub

	' Token: 0x060013F4 RID: 5108 RVA: 0x000B14B7 File Offset: 0x000AF8B7
	Private Sub AnimationEvent_SFX_DOGFIGHT_BulldogPlane_DiePlaneExplodes_VO()
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_PlaneExplodes_VO")
		CupheadLevelCamera.Current.Shake(30F, 0.29166666F, False)
	End Sub

	' Token: 0x060013F5 RID: 5109 RVA: 0x000B14D8 File Offset: 0x000AF8D8
	Private Sub WORKAROUND_NullifyFields()
		Me.bullDogPlane = Nothing
	End Sub

	' Token: 0x04001D15 RID: 7445
	<SerializeField()>
	Private bullDogPlane As Animator
End Class
