Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000824 RID: 2084
Public Class TrainLevelPlatform
	Inherits LevelPlatform

	' Token: 0x06003063 RID: 12387 RVA: 0x001C824C File Offset: 0x001C664C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.animHelper = MyBase.GetComponent(Of AnimationHelper)()
		Me.middlePos = MyBase.transform.position.x + 390F
		Me.leftPos = MyBase.transform.position.x
		Me.rightPos = MyBase.transform.position.x + 780F
		Me.position = TrainLevelPlatform.CartPosition.Left
		AddHandler Me.rightSwitch.OnActivate, AddressOf Me.OnRight
		AddHandler Me.leftSwitch.OnActivate, AddressOf Me.OnLeft
		MyBase.StartCoroutine(Me.spark_cr())
	End Sub

	' Token: 0x06003064 RID: 12388 RVA: 0x001C8304 File Offset: 0x001C6704
	Private Sub OnLeft()
		If Me.isMoving Then
			Return
		End If
		AudioManager.Play("train_hand_car_valves_spin")
		Me.emitAudioFromObject.Add("train_hand_car_valves_spin")
		Me.position = If((Me.position <> TrainLevelPlatform.CartPosition.Right), TrainLevelPlatform.CartPosition.Left, TrainLevelPlatform.CartPosition.Middle)
		Me.Move(Me.SelectPosition())
	End Sub

	' Token: 0x06003065 RID: 12389 RVA: 0x001C835C File Offset: 0x001C675C
	Private Sub OnRight()
		If Me.isMoving Then
			Return
		End If
		AudioManager.Play("train_hand_car_valves_spin")
		Me.emitAudioFromObject.Add("train_hand_car_valves_spin")
		Me.position = If((Me.position <> TrainLevelPlatform.CartPosition.Left), TrainLevelPlatform.CartPosition.Right, TrainLevelPlatform.CartPosition.Middle)
		Me.Move(Me.SelectPosition())
	End Sub

	' Token: 0x06003066 RID: 12390 RVA: 0x001C83B4 File Offset: 0x001C67B4
	Private Function SelectPosition() As Single
		Dim num As Single = 0F
		Dim cartPosition As TrainLevelPlatform.CartPosition = Me.position
		If cartPosition <> TrainLevelPlatform.CartPosition.Left Then
			If cartPosition <> TrainLevelPlatform.CartPosition.Right Then
				If cartPosition = TrainLevelPlatform.CartPosition.Middle Then
					num = Me.middlePos
				End If
			Else
				num = Me.rightPos
			End If
		Else
			num = Me.leftPos
		End If
		Return num
	End Function

	' Token: 0x06003067 RID: 12391 RVA: 0x001C840C File Offset: 0x001C680C
	Private Sub Move(x As Single)
		MyBase.StartCoroutine(Me.move_cr(x))
	End Sub

	' Token: 0x06003068 RID: 12392 RVA: 0x001C841C File Offset: 0x001C681C
	Private Iterator Function move_cr(x As Single) As IEnumerator
		Me.isMoving = True
		Me.rightSwitch.gameObject.SetActive(False)
		Me.leftSwitch.gameObject.SetActive(False)
		MyBase.animator.SetTrigger("OnSlap")
		MyBase.animator.SetBool("Spinning", True)
		MyBase.animator.SetBool("Effect", False)
		Me.animHelper.Speed = 1F
		Dim t As Single = 0F
		Dim time As Single = 1.5F
		Dim startX As Single = MyBase.transform.position.x
		MyBase.transform.SetPosition(New Single?(startX), Nothing, Nothing)
		Yield Nothing
		While t < time
			t += CupheadTime.Delta
			Dim val As Single = t / time
			MyBase.transform.SetPosition(New Single?(EaseUtils.Ease(EaseUtils.EaseType.easeOutCubic, startX, x, val)), Nothing, Nothing)
			If val > 0.5F Then
				Me.animHelper.Speed = 0.5F
			End If
			Yield Nothing
		End While
		MyBase.transform.SetPosition(New Single?(x), Nothing, Nothing)
		Me.isMoving = False
		Return
	End Function

	' Token: 0x06003069 RID: 12393 RVA: 0x001C8440 File Offset: 0x001C6840
	Private Sub FadeIn()
		Me.rightSwitch.gameObject.SetActive(True)
		Me.leftSwitch.gameObject.SetActive(True)
		Me.animHelper.Speed = 1F
		MyBase.animator.SetTrigger("OnContinue")
		MyBase.animator.SetBool("Effect", True)
	End Sub

	' Token: 0x0600306A RID: 12394 RVA: 0x001C84A0 File Offset: 0x001C68A0
	Private Iterator Function spark_cr() As IEnumerator
		While True
			While Not Me.leftSwitch.isActiveAndEnabled
				Yield Nothing
			End While
			Yield CupheadTime.WaitForSeconds(Me, Global.UnityEngine.Random.Range(0.5F, 1F))
			Me.sparkEffectPrefab.Create(Me.sparkRoots.RandomChoice().position)
			Yield CupheadTime.WaitForSeconds(Me, 0.33333334F)
		End While
		Return
	End Function

	' Token: 0x0400390D RID: 14605
	Private Const DISTANCE As Single = 390F

	' Token: 0x0400390E RID: 14606
	<SerializeField()>
	Private rightSwitch As ParrySwitch

	' Token: 0x0400390F RID: 14607
	<SerializeField()>
	Private leftSwitch As ParrySwitch

	' Token: 0x04003910 RID: 14608
	<SerializeField()>
	Private sparkRoots As Transform()

	' Token: 0x04003911 RID: 14609
	<SerializeField()>
	Private sparkEffectPrefab As Effect

	' Token: 0x04003912 RID: 14610
	Private position As TrainLevelPlatform.CartPosition

	' Token: 0x04003913 RID: 14611
	Private animHelper As AnimationHelper

	' Token: 0x04003914 RID: 14612
	Private spriteRenderer As SpriteRenderer

	' Token: 0x04003915 RID: 14613
	Private middlePos As Single

	' Token: 0x04003916 RID: 14614
	Private leftPos As Single

	' Token: 0x04003917 RID: 14615
	Private rightPos As Single

	' Token: 0x04003918 RID: 14616
	Private isMoving As Boolean

	' Token: 0x02000825 RID: 2085
	Public Enum CartPosition
		' Token: 0x0400391A RID: 14618
		Left
		' Token: 0x0400391B RID: 14619
		Middle
		' Token: 0x0400391C RID: 14620
		Right
	End Enum
End Class
