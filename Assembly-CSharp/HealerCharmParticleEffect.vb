Imports System
Imports UnityEngine

' Token: 0x02000A0C RID: 2572
Public Class HealerCharmParticleEffect
	Inherits AbstractPausableComponent

	' Token: 0x06003CD0 RID: 15568 RVA: 0x0021A43C File Offset: 0x0021883C
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.transform.localScale = New Vector3(CSng(MathUtils.PlusOrMinus()), 1F)
		Me.acceleration = Me.accelerationRange.RandomFloat()
		Me.timeBeforeSeek = Me.timeBeforeSeekRange.RandomFloat()
		Me.maxSpeed = Me.maxSpeedRange.RandomFloat()
		MyBase.animator.Play("Loop", 0, Global.UnityEngine.Random.Range(0F, 1F))
		MyBase.animator.Update(0F)
	End Sub

	' Token: 0x06003CD1 RID: 15569 RVA: 0x0021A4D0 File Offset: 0x002188D0
	Public Sub SetVars(newVel As Vector2, newTarget As AbstractPlayerController, newMain As HealerCharmSparkEffect)
		Me.target = newTarget
		Me.vel = newVel * Me.initialEmissionSpeed
		Me.main = newMain
		MyBase.transform.position += Me.vel * 0.04F
	End Sub

	' Token: 0x06003CD2 RID: 15570 RVA: 0x0021A528 File Offset: 0x00218928
	Private Sub FixedUpdate()
		If CupheadTime.FixedDelta = 0F Then
			Return
		End If
		If Me.target Is Nothing Then
			Return
		End If
		Me.frameTimer += CupheadTime.FixedDelta
		While Me.frameTimer > Me.frameTime
			Me.frameTimer -= Me.frameTime
			Me.FrameUpdate()
		End While
	End Sub

	' Token: 0x06003CD3 RID: 15571 RVA: 0x0021A598 File Offset: 0x00218998
	Private Sub FrameUpdate()
		MyBase.transform.position += Me.vel * Me.frameTime
		MyBase.transform.eulerAngles = New Vector3(0F, 0F, Mathf.Lerp(-25F, 25F, Mathf.InverseLerp(-Me.maxSpeed, Me.maxSpeed, Me.vel.x)) * -Mathf.Sign(MyBase.transform.localScale.x))
		Me.timer += Me.frameTime
		If Me.timer > Me.timeBeforeSeek Then
			Dim vector As Vector3 = Me.target.center - MyBase.transform.position
			Dim magnitude As Single = vector.magnitude
			If magnitude < Me.contactDistance AndAlso Me.timer > Me.timeBeforeCanCollect Then
				If Me.main Then
					Me.main.StartPlayerFlash()
				End If
				Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
			Else
				Me.vel += vector * (Me.timer - Me.timeBeforeSeek) * (Me.timer - Me.timeBeforeSeek) * Me.acceleration * Me.frameTime
				If Me.vel.magnitude > Me.maxSpeed Then
					Me.vel = Me.vel.normalized * Me.maxSpeed
				End If
				If Me.timer > Me.timeBeforeLerp Then
					Dim num As Single = Mathf.InverseLerp(Me.timeBeforeLerp, Me.maxTime, Me.timer)
					num *= num
					MyBase.transform.position = Vector3.Lerp(MyBase.transform.position, Me.target.center, num)
				End If
			End If
		End If
	End Sub

	' Token: 0x04004417 RID: 17431
	<SerializeField()>
	Private initialEmissionSpeed As Single = 400F

	' Token: 0x04004418 RID: 17432
	<SerializeField()>
	Private timeBeforeSeekRange As MinMax = New MinMax(0.025F, 0.075F)

	' Token: 0x04004419 RID: 17433
	Private timeBeforeSeek As Single

	' Token: 0x0400441A RID: 17434
	<SerializeField()>
	Private timeBeforeCanCollect As Single = 0.5F

	' Token: 0x0400441B RID: 17435
	<SerializeField()>
	Private timeBeforeLerp As Single = 1F

	' Token: 0x0400441C RID: 17436
	<SerializeField()>
	Private maxTime As Single = 0.75F

	' Token: 0x0400441D RID: 17437
	<SerializeField()>
	Private accelerationRange As MinMax = New MinMax(150F, 250F)

	' Token: 0x0400441E RID: 17438
	Private acceleration As Single

	' Token: 0x0400441F RID: 17439
	<SerializeField()>
	Private maxSpeedRange As MinMax = New MinMax(1500F, 2500F)

	' Token: 0x04004420 RID: 17440
	Private maxSpeed As Single

	' Token: 0x04004421 RID: 17441
	<SerializeField()>
	Private contactDistance As Single = 25F

	' Token: 0x04004422 RID: 17442
	Private target As AbstractPlayerController

	' Token: 0x04004423 RID: 17443
	Private timer As Single

	' Token: 0x04004424 RID: 17444
	Private vel As Vector3

	' Token: 0x04004425 RID: 17445
	<SerializeField()>
	Private frameTime As Single = 0.041666668F

	' Token: 0x04004426 RID: 17446
	Private frameTimer As Single

	' Token: 0x04004427 RID: 17447
	Private main As HealerCharmSparkEffect
End Class
