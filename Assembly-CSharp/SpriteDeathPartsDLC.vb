Imports System
Imports UnityEngine

' Token: 0x02000B1E RID: 2846
Public Class SpriteDeathPartsDLC
	Inherits SpriteDeathParts

	' Token: 0x060044E8 RID: 17640 RVA: 0x002473B0 File Offset: 0x002457B0
	Private Sub Start()
		If Me.progressiveBlur Then
			Me.rend.material.SetFloat("_BlurAmount", 0F)
			Me.rend.material.SetFloat("_BlurLerp", 0F)
		End If
		If Me.progressiveDim Then
			Me.startColor = Me.rend.color
		End If
	End Sub

	' Token: 0x060044E9 RID: 17641 RVA: 0x00247418 File Offset: 0x00245818
	Public Sub SetVelocity(vel As Vector3)
		Me.velocity = vel
	End Sub

	' Token: 0x060044EA RID: 17642 RVA: 0x00247426 File Offset: 0x00245826
	Private Sub FixedUpdate()
		If CupheadTime.FixedDelta > 0F Then
			Me.[Step](CupheadTime.FixedDelta * 1.2F)
		End If
	End Sub

	' Token: 0x060044EB RID: 17643 RVA: 0x00247448 File Offset: 0x00245848
	Protected Overrides Sub [Step](deltaTime As Single)
		MyBase.[Step](deltaTime)
		If Me.progressiveBlur Then
			Me.rend.material.SetFloat("_BlurAmount", Me.rend.material.GetFloat("_BlurAmount") + deltaTime * Me.blurIncreaseSpeed)
			Me.rend.material.SetFloat("_BlurLerp", Me.rend.material.GetFloat("_BlurLerp") + deltaTime * Me.blurIncreaseSpeed)
		End If
		If Me.progressiveDim Then
			Me.dimTimer += deltaTime * Me.dimIncreaseSpeed
			Me.rend.color = Color.Lerp(Me.startColor, Color.black, Me.dimTimer)
		End If
	End Sub

	' Token: 0x060044EC RID: 17644 RVA: 0x0024750E File Offset: 0x0024590E
	Protected Overrides Sub Update()
	End Sub

	' Token: 0x04004ABB RID: 19131
	Private Const UPDATE_TIMING_ADJUST As Single = 1.2F

	' Token: 0x04004ABC RID: 19132
	<SerializeField()>
	Private progressiveBlur As Boolean

	' Token: 0x04004ABD RID: 19133
	<SerializeField()>
	Private blurIncreaseSpeed As Single = 3F

	' Token: 0x04004ABE RID: 19134
	<SerializeField()>
	Private progressiveDim As Boolean

	' Token: 0x04004ABF RID: 19135
	<SerializeField()>
	Private dimIncreaseSpeed As Single = 3F

	' Token: 0x04004AC0 RID: 19136
	Private startColor As Color

	' Token: 0x04004AC1 RID: 19137
	Private dimTimer As Single

	' Token: 0x04004AC2 RID: 19138
	<SerializeField()>
	Private rend As SpriteRenderer
End Class
