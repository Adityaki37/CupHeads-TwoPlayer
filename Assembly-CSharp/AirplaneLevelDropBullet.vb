Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020004BA RID: 1210
Public Class AirplaneLevelDropBullet
	Inherits AbstractProjectile

	' Token: 0x1700030F RID: 783
	' (get) Token: 0x06001406 RID: 5126 RVA: 0x000B23D3 File Offset: 0x000B07D3
	' (set) Token: 0x06001407 RID: 5127 RVA: 0x000B23DB File Offset: 0x000B07DB
	Public Property isMoving As Boolean

	' Token: 0x06001408 RID: 5128 RVA: 0x000B23E4 File Offset: 0x000B07E4
	Public Overridable Function Init(targetPos As Vector3, startPos As Vector3, dropSpeed As Single, shootSpeed As Single, onLeft As Boolean, camHorizontal As Boolean) As AirplaneLevelDropBullet
		MyBase.ResetLifetime()
		MyBase.ResetDistance()
		MyBase.transform.position = startPos
		Me.YtoSwitch = targetPos.y
		Me.shootSpeed = shootSpeed
		Me.onLeft = onLeft
		MyBase.transform.SetScale(New Single?(CSng(If((Not onLeft), (-1), 1))), Nothing, Nothing)
		Me.rend.sortingOrder = If((Not onLeft), 501, 500)
		Me.targetPos = targetPos
		Me.bounds = If((Not camHorizontal), CupheadLevelCamera.Current.Bounds.xMax, CupheadLevelCamera.Current.Bounds.yMax)
		Me.dropSpeed = dropSpeed
		Me.moveDir = If((Not onLeft), Vector3.right, Vector3.left)
		Me.goingDown = True
		Me.isMoving = True
		Me.boxColl.enabled = False
		Me.circColl.enabled = True
		Me.t = 0.7853982F
		Me.startPos = startPos + Vector3.down * (Mathf.Sin(Me.t) * 600F)
		Return Me
	End Function

	' Token: 0x06001409 RID: 5129 RVA: 0x000B2531 File Offset: 0x000B0931
	Protected Overrides Sub Start()
		MyBase.Start()
		MyBase.StartCoroutine(Me.rotate_cr())
	End Sub

	' Token: 0x0600140A RID: 5130 RVA: 0x000B2548 File Offset: 0x000B0948
	Protected Overrides Sub FixedUpdate()
		MyBase.FixedUpdate()
		If Me.goingDown Then
			Me.t += CupheadTime.FixedDelta * Me.dropSpeed
			If Me.t < 3.1415927F Then
				MyBase.transform.position = New Vector3(EaseUtils.EaseOutSine(Me.startPos.x, Me.targetPos.x, Mathf.InverseLerp(0.7853982F, 3.1415927F, Me.t)), Me.startPos.y + Mathf.Sin(Me.t) * 600F)
			Else
				MyBase.transform.position += Vector3.up * (Mathf.Sin(3.1415927F) - Mathf.Sin(3.1415927F - CupheadTime.FixedDelta * Me.dropSpeed)) * 600F
			End If
			If MyBase.transform.position.y < Me.YtoSwitch Then
				MyBase.transform.position = New Vector3(MyBase.transform.position.x, Me.YtoSwitch)
				Me.moveDir = If((Not Me.onLeft), Vector3.left, Vector3.right)
				Me.dropSpeed = Me.shootSpeed
				Me.goingDown = False
				MyBase.animator.SetTrigger("ToShoot")
				Me.boxColl.enabled = True
				Me.circColl.enabled = False
				Me.shootFX.Create(MyBase.transform.position, MyBase.transform.localScale)
				Me.t = 0F
			End If
		Else
			Me.t += CupheadTime.FixedDelta
			If Me.t > 0.33333334F Then
				Me.speedLines.enabled = False
			End If
			MyBase.transform.position += Me.moveDir * Me.dropSpeed * CupheadTime.FixedDelta
			If MyBase.transform.position.x < -Me.bounds - 100F OrElse MyBase.transform.position.x > Me.bounds + 100F Then
				Me.isMoving = False
				Me.Recycle()
			End If
		End If
	End Sub

	' Token: 0x0600140B RID: 5131 RVA: 0x000B27C6 File Offset: 0x000B0BC6
	Protected Overrides Sub OnCollisionPlayer(hit As GameObject, phase As CollisionPhase)
		If phase <> CollisionPhase.[Exit] Then
			Me.damageDealer.DealDamage(hit)
		End If
		MyBase.OnCollisionPlayer(hit, phase)
	End Sub

	' Token: 0x0600140C RID: 5132 RVA: 0x000B27E4 File Offset: 0x000B0BE4
	Private Iterator Function rotate_cr() As IEnumerator
		Dim wait As WaitForFrameTimePersistent = New WaitForFrameTimePersistent(0.041666668F, False)
		Dim startRotation As Single = 360F
		Dim rotateSpeed As Single = 1F
		Dim rotateTime As Single = 0F
		While Me.goingDown
			MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(startRotation * rotateTime))
			rotateTime += CupheadTime.FixedDelta * rotateSpeed
			Yield wait
		End While
		MyBase.transform.SetEulerAngles(Nothing, Nothing, New Single?(startRotation))
		Yield Nothing
		Return
	End Function

	' Token: 0x04001D29 RID: 7465
	Private Const SPAWN_OFFSET As Single = 100F

	' Token: 0x04001D2A RID: 7466
	Private Const ARC_HEIGHT As Single = 600F

	' Token: 0x04001D2C RID: 7468
	Private moveDir As Vector3

	' Token: 0x04001D2D RID: 7469
	Private startPos As Vector3

	' Token: 0x04001D2E RID: 7470
	Private targetPos As Vector3

	' Token: 0x04001D2F RID: 7471
	Private shootSpeed As Single

	' Token: 0x04001D30 RID: 7472
	Private dropSpeed As Single

	' Token: 0x04001D31 RID: 7473
	Private YtoSwitch As Single

	' Token: 0x04001D32 RID: 7474
	Private bounds As Single

	' Token: 0x04001D33 RID: 7475
	Private goingDown As Boolean

	' Token: 0x04001D34 RID: 7476
	Private onLeft As Boolean

	' Token: 0x04001D35 RID: 7477
	<SerializeField()>
	Private circColl As CircleCollider2D

	' Token: 0x04001D36 RID: 7478
	<SerializeField()>
	Private boxColl As BoxCollider2D

	' Token: 0x04001D37 RID: 7479
	<SerializeField()>
	Private shootFX As Effect

	' Token: 0x04001D38 RID: 7480
	<SerializeField()>
	Private speedLines As SpriteRenderer

	' Token: 0x04001D39 RID: 7481
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x04001D3A RID: 7482
	Private t As Single
End Class
