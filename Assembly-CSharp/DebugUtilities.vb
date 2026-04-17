Imports System
Imports UnityEngine

' Token: 0x0200038D RID: 909
Public Module DebugUtilities
	' Token: 0x06000AC8 RID: 2760 RVA: 0x00080A9C File Offset: 0x0007EE9C
	Public Sub DrawLine(start As Vector3, [end] As Vector3)
		DebugUtilities.DrawLine(start, [end], DebugUtilities.DefaultColor, 0F)
	End Sub

	' Token: 0x06000AC9 RID: 2761 RVA: 0x00080AAF File Offset: 0x0007EEAF
	Public Sub DrawLine(start As Vector3, [end] As Vector3, color As Color, Optional duration As Single = 0F)
		DebugUtilities.DebugDrawer.DrawLine(start, [end], color, duration)
	End Sub

	' Token: 0x06000ACA RID: 2762 RVA: 0x00080ABA File Offset: 0x0007EEBA
	Public Sub DrawRay(origin As Vector3, direction As Vector3)
		DebugUtilities.DrawRay(origin, direction, DebugUtilities.DefaultColor, 0F)
	End Sub

	' Token: 0x06000ACB RID: 2763 RVA: 0x00080ACD File Offset: 0x0007EECD
	Public Sub DrawRay(origin As Vector3, direction As Vector3, color As Color, Optional duration As Single = 0F)
		DebugUtilities.DebugDrawer.DrawLine(origin, origin + direction, color, duration)
	End Sub

	' Token: 0x06000ACC RID: 2764 RVA: 0x00080ADE File Offset: 0x0007EEDE
	Public Sub DrawBox2D(origin As Vector2, size As Vector2, angle As Single)
		DebugUtilities.DrawBox2D(origin, size, angle, DebugUtilities.DefaultColor, 0F)
	End Sub

	' Token: 0x06000ACD RID: 2765 RVA: 0x00080AF4 File Offset: 0x0007EEF4
	Public Sub DrawBox2D(origin As Vector2, size As Vector2, angle As Single, color As Color, Optional duration As Single = 0F)
		Dim vector As Vector2 = size * 0.5F
		Dim vector2 As Vector2 = origin + New Vector2(-vector.x, vector.y)
		Dim vector3 As Vector2 = origin + New Vector2(-vector.x, -vector.y)
		Dim vector4 As Vector2 = origin + New Vector2(vector.x, vector.y)
		Dim vector5 As Vector2 = origin + New Vector2(vector.x, -vector.y)
		If Not Mathf.Approximately(angle, 0F) Then
			Throw New Exception("Not supported in this library")
		End If
		DebugUtilities.DebugDrawer.DrawLine(vector2, vector4, color, duration)
		DebugUtilities.DebugDrawer.DrawLine(vector4, vector5, color, duration)
		DebugUtilities.DebugDrawer.DrawLine(vector5, vector3, color, duration)
		DebugUtilities.DebugDrawer.DrawLine(vector3, vector2, color, duration)
	End Sub

	' Token: 0x06000ACE RID: 2766 RVA: 0x00080BE7 File Offset: 0x0007EFE7
	Public Sub DrawVerticalPole(center As Vector3, height As Single)
		DebugUtilities.DrawVerticalPole(center, height, DebugUtilities.DefaultColor, 0F)
	End Sub

	' Token: 0x06000ACF RID: 2767 RVA: 0x00080BFA File Offset: 0x0007EFFA
	Public Sub DrawVerticalPole(center As Vector3, height As Single, color As Color, Optional duration As Single = 0F)
		DebugUtilities.DrawLine(center + Vector3.up * height, center - Vector3.up * height, color, duration)
	End Sub

	' Token: 0x06000AD0 RID: 2768 RVA: 0x00080C25 File Offset: 0x0007F025
	Public Sub DrawHorizontalPole(center As Vector3, width As Single)
		DebugUtilities.DrawHorizontalPole(center, width, DebugUtilities.DefaultColor, 0F)
	End Sub

	' Token: 0x06000AD1 RID: 2769 RVA: 0x00080C38 File Offset: 0x0007F038
	Public Sub DrawHorizontalPole(center As Vector3, width As Single, color As Color, Optional duration As Single = 0F)
		DebugUtilities.DrawLine(center + Vector3.right * width, center - Vector3.right * width, color, duration)
	End Sub

	' Token: 0x06000AD2 RID: 2770 RVA: 0x00080C63 File Offset: 0x0007F063
	Public Sub DrawCircle2D(position As Vector3, radius As Single)
		DebugUtilities.DrawCircle2D(position, radius, DebugUtilities.DefaultColor, 0F, True)
	End Sub

	' Token: 0x06000AD3 RID: 2771 RVA: 0x00080C77 File Offset: 0x0007F077
	Public Sub DrawCircle2D(position As Vector3, radius As Single, color As Color, Optional duration As Single = 0F, Optional depthTest As Boolean = True)
		DebugUtilities.DrawCircle(position, Vector3.forward, Vector3.up, radius, 20, color, duration, depthTest)
	End Sub

	' Token: 0x06000AD4 RID: 2772 RVA: 0x00080C90 File Offset: 0x0007F090
	Public Sub DrawCircle(position As Vector3, forward As Vector3, up As Vector3, radius As Single, Optional segments As Integer = 20)
		DebugUtilities.DrawCircle(position, forward, up, radius, segments, DebugUtilities.DefaultColor, 0F, True)
	End Sub

	' Token: 0x06000AD5 RID: 2773 RVA: 0x00080CA8 File Offset: 0x0007F0A8
	Public Sub DrawCircle(position As Vector3, forward As Vector3, up As Vector3, radius As Single, segments As Integer, color As Color, Optional duration As Single = 0F, Optional depthTest As Boolean = True)
		DebugUtilities.DrawEllipse(position, forward, up, radius, radius, segments, color, duration, depthTest)
	End Sub

	' Token: 0x06000AD6 RID: 2774 RVA: 0x00080CC8 File Offset: 0x0007F0C8
	Public Sub DrawEllipse(pos As Vector3, forward As Vector3, up As Vector3, radiusX As Single, radiusY As Single, segments As Integer, color As Color, Optional duration As Single = 0F, Optional depthTest As Boolean = True)
		Dim num As Single = 0F
		Dim quaternion As Quaternion = Quaternion.LookRotation(forward, up)
		Dim vector As Vector3 = Vector3.zero
		Dim zero As Vector3 = Vector3.zero
		For i As Integer = 0 To segments + 1 - 1
			zero.x = Mathf.Sin(0.017453292F * num) * radiusX
			zero.y = Mathf.Cos(0.017453292F * num) * radiusY
			If i > 0 Then
				DebugUtilities.DebugDrawer.DrawLine(quaternion * vector + pos, quaternion * zero + pos, color, duration)
			End If
			vector = zero
			num += 360F / CSng(segments)
		Next
	End Sub

	' Token: 0x04001492 RID: 5266
	Private Const DefaultEllipseSegments As Integer = 20

	' Token: 0x04001493 RID: 5267
	Private DefaultColor As Color = Color.white

	' Token: 0x0200038E RID: 910
	Private NotInheritable Class DebugDrawer
		' Token: 0x06000AD8 RID: 2776 RVA: 0x00080D78 File Offset: 0x0007F178
		Public Shared Sub DrawLine(start As Vector3, [end] As Vector3, color As Color, duration As Single)
			Global.Debug.DrawLine(start, [end], color, duration, False)
		End Sub
	End Class
End Module
