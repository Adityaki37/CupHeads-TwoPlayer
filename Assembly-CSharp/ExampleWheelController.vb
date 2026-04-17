Imports System
Imports UnityEngine

' Token: 0x02000C03 RID: 3075
Public Class ExampleWheelController
	Inherits MonoBehaviour

	' Token: 0x06004966 RID: 18790 RVA: 0x00265A5D File Offset: 0x00263E5D
	Private Sub Start()
		Me.m_Rigidbody = MyBase.GetComponent(Of Rigidbody)()
		Me.m_Rigidbody.maxAngularVelocity = 100F
	End Sub

	' Token: 0x06004967 RID: 18791 RVA: 0x00265A7C File Offset: 0x00263E7C
	Private Sub Update()
		If Input.GetKey(KeyCode.UpArrow) Then
			Me.m_Rigidbody.AddRelativeTorque(New Vector3(-1F * Me.acceleration, 0F, 0F), ForceMode.Acceleration)
		ElseIf Input.GetKey(KeyCode.DownArrow) Then
			Me.m_Rigidbody.AddRelativeTorque(New Vector3(1F * Me.acceleration, 0F, 0F), ForceMode.Acceleration)
		End If
		Dim num As Single = -Me.m_Rigidbody.angularVelocity.x / 100F
		If Me.motionVectorRenderer Then
			Me.motionVectorRenderer.material.SetFloat(ExampleWheelController.Uniforms._MotionAmount, Mathf.Clamp(num, -0.25F, 0.25F))
		End If
	End Sub

	' Token: 0x04004F7D RID: 20349
	Public acceleration As Single

	' Token: 0x04004F7E RID: 20350
	Public motionVectorRenderer As Renderer

	' Token: 0x04004F7F RID: 20351
	Private m_Rigidbody As Rigidbody

	' Token: 0x02000C04 RID: 3076
	Private NotInheritable Class Uniforms
		' Token: 0x04004F80 RID: 20352
		Friend Shared _MotionAmount As Integer = Shader.PropertyToID("_MotionAmount")
	End Class
End Class
