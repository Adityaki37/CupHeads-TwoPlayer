Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020008D8 RID: 2264
Public Class MountainPlatformingLevelCyclops
	Inherits PlatformingLevelAutoscrollObject

	' Token: 0x060034EF RID: 13551 RVA: 0x001EC852 File Offset: 0x001EAC52
	Protected Overrides Sub Start()
		Me.damageDealer = DamageDealer.NewEnemy()
		Me.checkToLock = True
		Me.lockDistance = -1300F
		Me.IsEnabled(False)
		MyBase.StartCoroutine(Me.start_scrolling_cr())
		MyBase.Start()
	End Sub

	' Token: 0x060034F0 RID: 13552 RVA: 0x001EC88B File Offset: 0x001EAC8B
	Protected Overrides Sub Update()
		MyBase.Update()
		If Me.damageDealer IsNot Nothing Then
			Me.damageDealer.Update()
		End If
	End Sub

	' Token: 0x060034F1 RID: 13553 RVA: 0x001EC8A9 File Offset: 0x001EACA9
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		MyBase.OnCollisionPlayer(hit, phase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
	End Sub

	' Token: 0x060034F2 RID: 13554 RVA: 0x001EC8C7 File Offset: 0x001EACC7
	Private Sub IsEnabled(isenabled As Boolean)
		MyBase.GetComponent(Of Collider2D)().enabled = isenabled
		MyBase.GetComponent(Of SpriteRenderer)().enabled = isenabled
	End Sub

	' Token: 0x060034F3 RID: 13555 RVA: 0x001EC8E1 File Offset: 0x001EACE1
	Protected Overrides Sub StartAutoscroll()
		MyBase.StartAutoscroll()
		CupheadLevelCamera.Current.OffsetCamera(True, False)
	End Sub

	' Token: 0x060034F4 RID: 13556 RVA: 0x001EC8F8 File Offset: 0x001EACF8
	Private Iterator Function start_scrolling_cr() As IEnumerator
		While Not Me.isLocked
			Yield Nothing
		End While
		Me.cyclopsMoving = True
		MyBase.StartCoroutine(Me.start_moving_cr())
		Me.IsEnabled(True)
		Dim level As PlatformingLevel = CType(Level.Current, PlatformingLevel)
		level.useAltQuote = True
		While MyBase.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 650F
			Yield Nothing
		End While
		Me.autoscrollMoving = True
		MyBase.StartCoroutine(Me.check_to_move_forward_cr())
		Me.StartAutoscroll()
		Return
	End Function

	' Token: 0x060034F5 RID: 13557 RVA: 0x001EC914 File Offset: 0x001EAD14
	Private Iterator Function start_moving_cr() As IEnumerator
		MyBase.animator.Play("Run")
		While Me.cyclopsMoving
			MyBase.transform.position += Vector3.right * (200F * Me.autoScrollMultiplier) * CupheadTime.Delta
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060034F6 RID: 13558 RVA: 0x001EC930 File Offset: 0x001EAD30
	Private Iterator Function check_to_move_forward_cr() As IEnumerator
		While Me.autoscrollMoving
			Dim dist As Single = PlayerManager.Center.x - MyBase.transform.position.x
			If MyBase.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 1600F Then
				MyBase.transform.position = New Vector3(CupheadLevelCamera.Current.transform.position.x - 1600F, MyBase.transform.position.y)
			End If
			If dist > 1300F Then
				If CupheadLevelCamera.Current.autoScrolling Then
					CupheadLevelCamera.Current.SetAutoScroll(False)
				End If
			ElseIf Not CupheadLevelCamera.Current.autoScrolling Then
				CupheadLevelCamera.Current.SetAutoScroll(True)
				CupheadLevelCamera.Current.SetAutoscrollSpeedMultiplier(Me.autoScrollMultiplier)
			End If
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x060034F7 RID: 13559 RVA: 0x001EC94B File Offset: 0x001EAD4B
	Protected Overrides Sub StartEndingAutoscroll()
		MyBase.StartEndingAutoscroll()
		Me.autoscrollMoving = False
	End Sub

	' Token: 0x060034F8 RID: 13560 RVA: 0x001EC95A File Offset: 0x001EAD5A
	Protected Overrides Sub EndAutoscroll()
		MyBase.EndAutoscroll()
		MyBase.StartCoroutine(Me.fall_cr())
	End Sub

	' Token: 0x060034F9 RID: 13561 RVA: 0x001EC970 File Offset: 0x001EAD70
	Private Iterator Function fall_cr() As IEnumerator
		While MyBase.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 1300F
			Yield Nothing
		End While
		CupheadLevelCamera.Current.LockCamera(True)
		Me.cyclopsMoving = False
		MyBase.GetComponent(Of SpriteRenderer)().sortingLayerName = SpriteLayer.Map.ToString()
		MyBase.animator.SetTrigger("OnFall")
		Yield MyBase.animator.WaitForAnimationToEnd(Me, "Fall", False, True)
		CupheadLevelCamera.Current.LockCamera(False)
		CupheadLevelCamera.Current.OffsetCamera(False, False)
		Yield CupheadTime.WaitForSeconds(Me, 0.5F)
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		Yield Nothing
		Return
	End Function

	' Token: 0x060034FA RID: 13562 RVA: 0x001EC98B File Offset: 0x001EAD8B
	Public Sub CameraShake()
		CupheadLevelCamera.Current.Shake(10F, 0.5F, False)
	End Sub

	' Token: 0x060034FB RID: 13563 RVA: 0x001EC9A2 File Offset: 0x001EADA2
	Private Sub SoundCyclopsFall()
		AudioManager.Play("castle_giant_rock_chase_death")
	End Sub

	' Token: 0x060034FC RID: 13564 RVA: 0x001EC9AE File Offset: 0x001EADAE
	Private Sub SoundCyclopsFootstep()
		AudioManager.Play("castle_giant_rock_chase_footstep")
		Me.emitAudioFromObject.Add("castle_giant_rock_chase_footstep")
	End Sub

	' Token: 0x04003D20 RID: 15648
	<SerializeField()>
	Private autoScrollMultiplier As Single

	' Token: 0x04003D21 RID: 15649
	Private Const MAX_AUTOSCROLL_DIST As Single = 1300F

	' Token: 0x04003D22 RID: 15650
	Private Const MAX_CYCLOPS_DIST As Single = 1600F

	' Token: 0x04003D23 RID: 15651
	Private autoscrollMoving As Boolean

	' Token: 0x04003D24 RID: 15652
	Private cyclopsMoving As Boolean

	' Token: 0x04003D25 RID: 15653
	Private damageDealer As DamageDealer
End Class
