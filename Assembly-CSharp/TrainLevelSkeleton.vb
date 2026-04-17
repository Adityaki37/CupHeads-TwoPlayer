Imports System
Imports System.Collections
Imports System.Diagnostics
Imports UnityEngine

' Token: 0x02000828 RID: 2088
Public Class TrainLevelSkeleton
	Inherits LevelProperties.Train.Entity

	' Token: 0x14000057 RID: 87
	' (add) Token: 0x06003081 RID: 12417 RVA: 0x001C9160 File Offset: 0x001C7560
	' (remove) Token: 0x06003082 RID: 12418 RVA: 0x001C9198 File Offset: 0x001C7598
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDamageTakenEvent As TrainLevelSkeleton.OnDamageTakenHandler

	' Token: 0x14000058 RID: 88
	' (add) Token: 0x06003083 RID: 12419 RVA: 0x001C91D0 File Offset: 0x001C75D0
	' (remove) Token: 0x06003084 RID: 12420 RVA: 0x001C9208 File Offset: 0x001C7608
	<DebuggerBrowsable(DebuggerBrowsableState.Never)>
	Public Event OnDeathEvent As Action

	' Token: 0x06003085 RID: 12421 RVA: 0x001C923E File Offset: 0x001C763E
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.damageReceiver = Me.head.GetComponent(Of DamageReceiver)()
		AddHandler Me.damageReceiver.OnDamageTaken, AddressOf Me.OnDamageTaken
	End Sub

	' Token: 0x06003086 RID: 12422 RVA: 0x001C9270 File Offset: 0x001C7670
	Private Sub OnDamageTaken(info As DamageDealer.DamageInfo)
		If Me.dead Then
			Return
		End If
		If Me.OnDamageTakenEvent IsNot Nothing Then
			Me.OnDamageTakenEvent(info.damage)
		End If
		Me.health -= info.damage
		If Me.health <= 0F Then
			Me.Die()
		End If
	End Sub

	' Token: 0x06003087 RID: 12423 RVA: 0x001C92CE File Offset: 0x001C76CE
	Private Sub Die()
		If Me.dead Then
			Return
		End If
		Me.dead = True
		Me.StopAllCoroutines()
		MyBase.StartCoroutine(Me.die_cr())
	End Sub

	' Token: 0x06003088 RID: 12424 RVA: 0x001C92F6 File Offset: 0x001C76F6
	Public Overrides Sub LevelInit(properties As LevelProperties.Train)
		MyBase.LevelInit(properties)
		Me.health = properties.CurrentState.skeleton.health
	End Sub

	' Token: 0x06003089 RID: 12425 RVA: 0x001C9315 File Offset: 0x001C7715
	Public Sub StartSkeleton()
		AudioManager.Play("train_passenger_car_explode")
		Me.emitAudioFromObject.Add("train_passenger_car_explode")
		MyBase.StartCoroutine(Me.loop_cr())
	End Sub

	' Token: 0x0600308A RID: 12426 RVA: 0x001C933E File Offset: 0x001C773E
	Private Sub [In]()
		AudioManager.Play("level_train_skeleton_up")
		Me.head.[In]()
		Me.leftHand.[In]()
		Me.rightHand.[In]()
	End Sub

	' Token: 0x0600308B RID: 12427 RVA: 0x001C936B File Offset: 0x001C776B
	Private Sub Out()
		AudioManager.Play("train_skeleton_hand_out")
		Me.emitAudioFromObject.Add("train_skeleton_hand_out")
		Me.head.Out()
		Me.leftHand.Out()
		Me.rightHand.Out()
	End Sub

	' Token: 0x0600308C RID: 12428 RVA: 0x001C93A8 File Offset: 0x001C77A8
	Private Sub RandomizeLocations()
		Dim position As TrainLevelSkeleton.Position
		position = Me.currentPosition
		While position = Me.currentPosition
			position = CType(Global.UnityEngine.Random.Range(0, 3), TrainLevelSkeleton.Position)
		End While
		Me.currentPosition = position
		Me.head.SetPosition(position)
		Select Case position
			Case TrainLevelSkeleton.Position.Right
				Me.leftHand.SetPosition(TrainLevelSkeleton.Position.Left)
				Me.rightHand.SetPosition(TrainLevelSkeleton.Position.Center)
			Case TrainLevelSkeleton.Position.Center
				Me.leftHand.SetPosition(TrainLevelSkeleton.Position.Left)
				Me.rightHand.SetPosition(TrainLevelSkeleton.Position.Right)
			Case Else
				Me.leftHand.SetPosition(TrainLevelSkeleton.Position.Center)
				Me.rightHand.SetPosition(TrainLevelSkeleton.Position.Right)
		End Select
	End Sub

	' Token: 0x0600308D RID: 12429 RVA: 0x001C9458 File Offset: 0x001C7858
	Private Iterator Function loop_cr() As IEnumerator
		Me.currentPosition = TrainLevelSkeleton.Position.Center
		Dim attackDelay As Single = 0F
		Dim handAnimator As Animator = Me.rightHand.GetComponent(Of Animator)()
		While True
			attackDelay = Mathf.Lerp(MyBase.properties.CurrentState.skeleton.attackDelay.max, MyBase.properties.CurrentState.skeleton.attackDelay.min, Me.health / MyBase.properties.CurrentState.skeleton.health)
			Me.[In]()
			Yield handAnimator.WaitForAnimationToEnd(Me, "In", False, True)
			Yield CupheadTime.WaitForSeconds(Me, attackDelay)
			AudioManager.Play("train_skeleton_hand_slap")
			Me.leftHand.Slap()
			Me.rightHand.Slap()
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.skeleton.slapHoldTime)
			Me.Out()
			Yield Me.head.animator.WaitForAnimationToEnd(Me, "Out", False, True)
			Yield CupheadTime.WaitForSeconds(Me, MyBase.properties.CurrentState.skeleton.appearDelay)
			Me.RandomizeLocations()
		End While
		Return
	End Function

	' Token: 0x0600308E RID: 12430 RVA: 0x001C9474 File Offset: 0x001C7874
	Private Iterator Function die_cr() As IEnumerator
		Dim handAnimator As Animator = Me.rightHand.GetComponent(Of Animator)()
		Me.head.Die()
		Me.rightHand.Die()
		Me.leftHand.Die()
		AudioManager.Play("train_skeleton_hand_death")
		Me.emitAudioFromObject.Add("train_skeleton_hand_death")
		Yield handAnimator.WaitForAnimationToEnd(Me, "Death", False, True)
		Me.head.EndDeath()
		Me.rightHand.EndDeath()
		Me.leftHand.EndDeath()
		Yield CupheadTime.WaitForSeconds(Me, 1F)
		If Me.OnDeathEvent IsNot Nothing Then
			Me.OnDeathEvent()
		End If
		Return
	End Function

	' Token: 0x04003928 RID: 14632
	<SerializeField()>
	Private head As TrainLevelSkeletonHead

	' Token: 0x04003929 RID: 14633
	<SerializeField()>
	Private leftHand As TrainLevelSkeletonHand

	' Token: 0x0400392A RID: 14634
	<SerializeField()>
	Private rightHand As TrainLevelSkeletonHand

	' Token: 0x0400392B RID: 14635
	Private damageReceiver As DamageReceiver

	' Token: 0x0400392C RID: 14636
	Private health As Single

	' Token: 0x0400392D RID: 14637
	Private dead As Boolean

	' Token: 0x0400392E RID: 14638
	Private currentPosition As TrainLevelSkeleton.Position

	' Token: 0x02000829 RID: 2089
	Public Enum Position
		' Token: 0x04003932 RID: 14642
		Right
		' Token: 0x04003933 RID: 14643
		Center
		' Token: 0x04003934 RID: 14644
		Left
	End Enum

	' Token: 0x0200082A RID: 2090
	' (Invoke) Token: 0x06003090 RID: 12432
	Public Delegate Sub OnDamageTakenHandler(damage As Single)
End Class
