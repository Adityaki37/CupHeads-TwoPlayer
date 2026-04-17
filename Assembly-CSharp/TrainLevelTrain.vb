Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200082E RID: 2094
Public Class TrainLevelTrain
	Inherits LevelProperties.Train.Entity

	' Token: 0x17000421 RID: 1057
	' (get) Token: 0x0600309C RID: 12444 RVA: 0x001C9AA3 File Offset: 0x001C7EA3
	' (set) Token: 0x0600309D RID: 12445 RVA: 0x001C9AAB File Offset: 0x001C7EAB
	Public Property state As TrainLevelTrain.State

	' Token: 0x0600309E RID: 12446 RVA: 0x001C9AB4 File Offset: 0x001C7EB4
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.SetPosition(New Single?(455F), Nothing, Nothing)
	End Sub

	' Token: 0x0600309F RID: 12447 RVA: 0x001C9AEE File Offset: 0x001C7EEE
	Public Sub OnBlindSpectreDeath()
		MyBase.StartCoroutine(Me.blindSpectreDeath_cr())
	End Sub

	' Token: 0x060030A0 RID: 12448 RVA: 0x001C9AFD File Offset: 0x001C7EFD
	Public Sub OnSkeletonDeath()
		MyBase.StartCoroutine(Me.skeletonDeath_cr())
	End Sub

	' Token: 0x060030A1 RID: 12449 RVA: 0x001C9B0C File Offset: 0x001C7F0C
	Public Sub OnLollipopsDeath()
		MyBase.StartCoroutine(Me.lollipopsDeath_cr())
	End Sub

	' Token: 0x060030A2 RID: 12450 RVA: 0x001C9B1C File Offset: 0x001C7F1C
	Private Iterator Function blindSpectreDeath_cr() As IEnumerator
		Me.state = TrainLevelTrain.State.Skeleton
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Yield MyBase.TweenPositionX(MyBase.transform.position.x, -960F, 2.5F, EaseUtils.EaseType.easeInOutSine)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		For i As Integer = 0 To Me.skeletonCars.Length - 1
			Dim num As Integer = If((i <> 1), 0, 1)
			Me.skeletonCars(i).Explode(num)
		Next
		AudioManager.Play("level_train_top_explode")
		Me.skeleton.StartSkeleton()
		Return
	End Function

	' Token: 0x060030A3 RID: 12451 RVA: 0x001C9B38 File Offset: 0x001C7F38
	Private Iterator Function skeletonDeath_cr() As IEnumerator
		Me.state = TrainLevelTrain.State.LollipopGhouls
		Me.ghouls.Setup()
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Yield MyBase.TweenPositionX(MyBase.transform.position.x, -2358F, 2.5F, EaseUtils.EaseType.easeInOutSine)
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		Me.ghouls.StartGhouls()
		Return
	End Function

	' Token: 0x060030A4 RID: 12452 RVA: 0x001C9B54 File Offset: 0x001C7F54
	Private Iterator Function lollipopsDeath_cr() As IEnumerator
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Yield MyBase.TweenPositionX(MyBase.transform.position.x, -4816F, 2.5F, EaseUtils.EaseType.easeInSine)
		Me.engineCar.PlayRage()
		Yield MyBase.TweenPositionX(MyBase.transform.position.x, -6016F, 2.5F, EaseUtils.EaseType.linear)
		Me.engineCar.[End]()
		Yield CupheadTime.WaitForSeconds(Me, 2F)
		Me.engineBoss.StartBoss()
		MyBase.gameObject.SetActive(False)
		Return
	End Function

	' Token: 0x0400393B RID: 14651
	Public Const TRAIN_MOVE_TIME As Single = 2.5F

	' Token: 0x0400393C RID: 14652
	Public Const START_X As Single = 455F

	' Token: 0x0400393D RID: 14653
	Public Const SKELETON_X As Single = -960F

	' Token: 0x0400393E RID: 14654
	Public Const GHOUL_X As Single = -2358F

	' Token: 0x0400393F RID: 14655
	Public Const ENGINE_MID_X As Single = -4816F

	' Token: 0x04003940 RID: 14656
	Public Const ENGINE_X As Single = -6016F

	' Token: 0x04003942 RID: 14658
	<SerializeField()>
	Private skeleton As TrainLevelSkeleton

	' Token: 0x04003943 RID: 14659
	<SerializeField()>
	Private skeletonCars As TrainLevelPassengerCar()

	' Token: 0x04003944 RID: 14660
	<Space(10F)>
	<SerializeField()>
	Private ghouls As TrainLevelLollipopGhoulsManager

	' Token: 0x04003945 RID: 14661
	<Space(10F)>
	<SerializeField()>
	Private engineCar As TrainLevelEngineCar

	' Token: 0x04003946 RID: 14662
	<SerializeField()>
	Private engineBoss As TrainLevelEngineBoss

	' Token: 0x0200082F RID: 2095
	Public Enum State
		' Token: 0x04003948 RID: 14664
		BlindSpecter
		' Token: 0x04003949 RID: 14665
		Skeleton
		' Token: 0x0400394A RID: 14666
		LollipopGhouls
		' Token: 0x0400394B RID: 14667
		Engine
	End Enum
End Class
