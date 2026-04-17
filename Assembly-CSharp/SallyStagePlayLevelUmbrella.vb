Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020007B9 RID: 1977
Public Class SallyStagePlayLevelUmbrella
	Inherits GroundHomingMovement

	' Token: 0x06002CAC RID: 11436 RVA: 0x001A5795 File Offset: 0x001A3B95
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.drop_cr())
	End Sub

	' Token: 0x06002CAD RID: 11437 RVA: 0x001A57AA File Offset: 0x001A3BAA
	Public Sub GetProperties(properties As LevelProperties.SallyStagePlay)
		Me.properties = properties
	End Sub

	' Token: 0x06002CAE RID: 11438 RVA: 0x001A57B4 File Offset: 0x001A3BB4
	Private Sub FixedUpdate()
		Me.shadow.transform.SetLocalEulerAngles(Nothing, Nothing, New Single?(-MyBase.transform.localEulerAngles.z))
	End Sub

	' Token: 0x06002CAF RID: 11439 RVA: 0x001A57FC File Offset: 0x001A3BFC
	Private Iterator Function drop_cr() As IEnumerator
		AudioManager.PlayLoop("sally_umbrella_fall")
		Me.emitAudioFromObject.Add("sally_umbrella_fall")
		Dim wait As YieldInstruction = New WaitForFixedUpdate()
		Dim speed As Single = 200F
		Dim t As Single = 0F
		Dim rotateAmount As Single = 12F
		Dim rotateT As Single = 0F
		Dim time As Single = 0.2F
		While MyBase.transform.position.y > CSng(Level.Current.Ground) + 100F
			t += CupheadTime.FixedDelta
			MyBase.transform.AddPosition(0F, -speed * CupheadTime.FixedDelta, 0F)
			rotateT += CupheadTime.FixedDelta
			Dim phase As Single = Mathf.Sin(rotateT / time)
			MyBase.transform.localRotation = Quaternion.Euler(New Vector3(0F, 0F, phase * rotateAmount))
			Yield wait
		End While
		Me.maxSpeed = Me.properties.CurrentState.umbrella.homingMaxSpeed
		Me.acceleration = Me.properties.CurrentState.umbrella.homingAcceleration
		Me.bounceRatio = Me.properties.CurrentState.umbrella.homingBounceRatio
		Global.UnityEngine.[Object].Destroy(MyBase.GetComponent(Of LevelCharacterShadow)())
		AudioManager.[Stop]("sally_umbrella_fall")
		AudioManager.PlayLoop("sally_umbrella_spin_loop")
		Me.emitAudioFromObject.Add("sally_umbrella_spin_loop")
		MyBase.animator.SetTrigger("Land")
		MyBase.EnableHoming = True
		Me.enableRadishRot = True
		MyBase.StartCoroutine(Me.check_dir_change_cr())
		Yield Nothing
		Return
	End Function

	' Token: 0x06002CB0 RID: 11440 RVA: 0x001A5818 File Offset: 0x001A3C18
	Private Iterator Function check_dir_change_cr() As IEnumerator
		While True
			If MyBase.MoveDirection <> Me.moveDir Then
				AudioManager.Play("sally_umbrella_change_direction")
				Me.emitAudioFromObject.Add("sally_umbrella_change_direction")
				Me.moveDir = MyBase.MoveDirection
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x04003531 RID: 13617
	<SerializeField()>
	Private shadow As Transform

	' Token: 0x04003532 RID: 13618
	Private startPosX As Single

	' Token: 0x04003533 RID: 13619
	Private properties As LevelProperties.SallyStagePlay

	' Token: 0x04003534 RID: 13620
	Private moveDir As GroundHomingMovement.Direction
End Class
