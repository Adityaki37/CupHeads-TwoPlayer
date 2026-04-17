Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000434 RID: 1076
Public Class GroundHomingMovement
	Inherits AbstractPausableComponent

	' Token: 0x17000287 RID: 647
	' (get) Token: 0x06000FC5 RID: 4037 RVA: 0x0009C8F2 File Offset: 0x0009ACF2
	' (set) Token: 0x06000FC6 RID: 4038 RVA: 0x0009C8FA File Offset: 0x0009ACFA
	Public Property TrackingPlayer As AbstractPlayerController

	' Token: 0x17000288 RID: 648
	' (get) Token: 0x06000FC7 RID: 4039 RVA: 0x0009C903 File Offset: 0x0009AD03
	' (set) Token: 0x06000FC8 RID: 4040 RVA: 0x0009C90B File Offset: 0x0009AD0B
	Public Property EnableHoming As Boolean

	' Token: 0x17000289 RID: 649
	' (get) Token: 0x06000FC9 RID: 4041 RVA: 0x0009C914 File Offset: 0x0009AD14
	' (set) Token: 0x06000FCA RID: 4042 RVA: 0x0009C91C File Offset: 0x0009AD1C
	Public Property MoveDirection As GroundHomingMovement.Direction

	' Token: 0x06000FCB RID: 4043 RVA: 0x0009C925 File Offset: 0x0009AD25
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.StartCoroutine(Me.loop_cr())
		If Me.startOnAwake Then
			Me.EnableHoming = True
		End If
	End Sub

	' Token: 0x06000FCC RID: 4044 RVA: 0x0009C94C File Offset: 0x0009AD4C
	Private Function hitPauseCoefficient() As Single
		Dim component As DamageReceiver = MyBase.GetComponent(Of DamageReceiver)()
		If component Is Nothing Then
			Return 1F
		End If
		Return If((Not component.IsHitPaused), 1F, 0F)
	End Function

	' Token: 0x06000FCD RID: 4045 RVA: 0x0009C98C File Offset: 0x0009AD8C
	Private Iterator Function loop_cr() As IEnumerator
		Dim radishRot As Quaternion = MyBase.transform.localRotation
		While Me.TrackingPlayer Is Nothing
			Yield Nothing
		End While
		While True
			If Not MyBase.enabled Then
				Yield Nothing
			Else
				If Me.TrackingPlayer Is Nothing OrElse Me.TrackingPlayer.IsDead Then
					Me.TrackingPlayer = PlayerManager.GetNext()
				End If
				If Me.EnableHoming Then
					If Me.TrackingPlayer.transform.position.x > MyBase.transform.position.x Then
						Me.MoveDirection = GroundHomingMovement.Direction.Right
						If radishRot.z < 0.05235988F Then
							radishRot.z += 0.01F
						End If
					Else
						Me.MoveDirection = GroundHomingMovement.Direction.Left
						If radishRot.z > -0.05235988F Then
							radishRot.z -= 0.01F
						End If
					End If
				End If
				If Me.MoveDirection = GroundHomingMovement.Direction.Right Then
					Me.velocityX += Me.acceleration * CupheadTime.Delta * Me.hitPauseCoefficient()
				Else
					Me.velocityX -= Me.acceleration * CupheadTime.Delta * Me.hitPauseCoefficient()
				End If
				Me.velocityX = Mathf.Clamp(Me.velocityX, -Me.maxSpeed, Me.maxSpeed)
				Dim position As Vector2 = MyBase.transform.localPosition
				position.x += Me.velocityX * CupheadTime.Delta * Me.hitPauseCoefficient()
				If Me.bounceEnabled Then
					If position.x < CSng(Level.Current.Left) + Me.leftPadding Then
						position.x = CSng(Level.Current.Left) + Me.leftPadding
						Me.velocityX *= -Me.bounceRatio
					End If
					If position.x > CSng(Level.Current.Right) - Me.rightPadding Then
						position.x = CSng(Level.Current.Right) - Me.rightPadding
						Me.velocityX *= -Me.bounceRatio
					End If
				End If
				If Me.destroyOffScreen Then
					Dim component As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
					If position.x < CSng(Level.Current.Left) - component.bounds.size.x / 2F OrElse position.x > CSng(Level.Current.Right) + component.bounds.size.x / 2F Then
						Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
					End If
				End If
				MyBase.transform.localPosition = position
				If Me.enableRadishRot Then
					MyBase.transform.localRotation = radishRot
				End If
				Yield Nothing
			End If
		End While
		Return
	End Function

	' Token: 0x04001957 RID: 6487
	Public startOnAwake As Boolean

	' Token: 0x04001958 RID: 6488
	Public maxSpeed As Single

	' Token: 0x04001959 RID: 6489
	Public acceleration As Single

	' Token: 0x0400195A RID: 6490
	Public bounceRatio As Single

	' Token: 0x0400195B RID: 6491
	Public bounceEnabled As Boolean

	' Token: 0x0400195C RID: 6492
	Public leftPadding As Single

	' Token: 0x0400195D RID: 6493
	Public rightPadding As Single

	' Token: 0x0400195E RID: 6494
	Public destroyOffScreen As Boolean

	' Token: 0x0400195F RID: 6495
	Public enableRadishRot As Boolean

	' Token: 0x04001960 RID: 6496
	Private velocityX As Single

	' Token: 0x02000435 RID: 1077
	Public Enum Direction
		' Token: 0x04001965 RID: 6501
		Left
		' Token: 0x04001966 RID: 6502
		Right
	End Enum
End Class
