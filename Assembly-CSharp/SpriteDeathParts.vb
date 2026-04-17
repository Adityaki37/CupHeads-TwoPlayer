Imports System
Imports UnityEngine

' Token: 0x02000B1D RID: 2845
Public Class SpriteDeathParts
	Inherits AbstractCollidableObject

	' Token: 0x060044E1 RID: 17633 RVA: 0x0013DD3C File Offset: 0x0013C13C
	Public Function CreatePart(position As Vector3) As SpriteDeathParts
		Dim spriteDeathParts As SpriteDeathParts = Me.InstantiatePrefab(Of SpriteDeathParts)()
		spriteDeathParts.transform.position = position
		Return spriteDeathParts
	End Function

	' Token: 0x060044E2 RID: 17634 RVA: 0x0013DD60 File Offset: 0x0013C160
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.velocity = New Vector2(Global.UnityEngine.Random.Range(Me.VelocityXMin, Me.VelocityXMax), Global.UnityEngine.Random.Range(Me.VelocityYMin, Me.VelocityYMax))
		If Me.rotate Then
			Me.rotationSpeed = Global.UnityEngine.Random.Range(Me.rotationSpeedRange.minimum, Me.rotationSpeedRange.maximum) * CSng(Rand.PosOrNeg())
		End If
	End Sub

	' Token: 0x060044E3 RID: 17635 RVA: 0x0013DDD3 File Offset: 0x0013C1D3
	Protected Overridable Sub Update()
		Me.[Step](Time.fixedDeltaTime)
	End Sub

	' Token: 0x060044E4 RID: 17636 RVA: 0x0013DDE0 File Offset: 0x0013C1E0
	Protected Overridable Sub [Step](deltaTime As Single)
		If Me.clampFallVelocity Then
			Me.velocity.y = Mathf.Clamp(Me.velocity.y, -5000F, Single.MaxValue)
		End If
		MyBase.transform.position += (Me.velocity + New Vector2(0F, Me.accumulatedGravity)) * deltaTime
		Me.accumulatedGravity += Me.GRAVITY
		If Me.rotate Then
			Me.currentAngle += Me.rotationSpeed * deltaTime
			MyBase.transform.rotation = Quaternion.Euler(0F, 0F, Me.currentAngle)
		End If
		If MyBase.transform.position.y < -360F - Me.bottomOffset Then
			Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
		End If
	End Sub

	' Token: 0x060044E5 RID: 17637 RVA: 0x0013DEDB File Offset: 0x0013C2DB
	Public Sub SetVelocityX(min As Single, max As Single)
		Me.velocity.x = Global.UnityEngine.Random.Range(min, max)
	End Sub

	' Token: 0x060044E6 RID: 17638 RVA: 0x0013DEEF File Offset: 0x0013C2EF
	Public Sub SetVelocityY(min As Single, max As Single)
		Me.velocity.y = Global.UnityEngine.Random.Range(min, max)
	End Sub

	' Token: 0x04004AAE RID: 19118
	Public bottomOffset As Single = 100F

	' Token: 0x04004AAF RID: 19119
	Public VelocityXMin As Single = -500F

	' Token: 0x04004AB0 RID: 19120
	Public VelocityXMax As Single = 500F

	' Token: 0x04004AB1 RID: 19121
	Public VelocityYMin As Single = 500F

	' Token: 0x04004AB2 RID: 19122
	Public VelocityYMax As Single = 1000F

	' Token: 0x04004AB3 RID: 19123
	Public GRAVITY As Single = -100F

	' Token: 0x04004AB4 RID: 19124
	<SerializeField()>
	Protected clampFallVelocity As Boolean

	' Token: 0x04004AB5 RID: 19125
	<SerializeField()>
	Private rotate As Boolean

	' Token: 0x04004AB6 RID: 19126
	<SerializeField()>
	Private rotationSpeedRange As Rangef

	' Token: 0x04004AB7 RID: 19127
	Protected velocity As Vector2

	' Token: 0x04004AB8 RID: 19128
	Private accumulatedGravity As Single

	' Token: 0x04004AB9 RID: 19129
	Private rotationSpeed As Single

	' Token: 0x04004ABA RID: 19130
	Private currentAngle As Single
End Class
