Imports System
Imports UnityEngine

' Token: 0x0200038B RID: 907
Public Module BoundsUtilities
	' Token: 0x06000AC2 RID: 2754 RVA: 0x00080748 File Offset: 0x0007EB48
	Public Function CalculateBounds(size As Vector2, offset As Vector2, transform As Transform) As Bounds
		Dim vector As Vector2 = size * 0.5F
		Dim vector2 As Vector3 = New Vector2(-vector.x, vector.y) + offset
		Dim vector3 As Vector3 = New Vector2(vector.x, vector.y) + offset
		Dim vector4 As Vector3 = New Vector2(-vector.x, -vector.y) + offset
		Dim vector5 As Vector3 = New Vector2(vector.x, -vector.y) + offset
		vector2 = transform.TransformPoint(vector2)
		vector3 = transform.TransformPoint(vector3)
		vector4 = transform.TransformPoint(vector4)
		vector5 = transform.TransformPoint(vector5)
		Dim num As Single = Mathf.Min(Mathf.Min(Mathf.Min(vector2.x, vector3.x), vector4.x), vector5.x)
		Dim num2 As Single = Mathf.Min(Mathf.Min(Mathf.Min(vector2.y, vector3.y), vector4.y), vector5.y)
		Dim num3 As Single = Mathf.Max(Mathf.Max(Mathf.Max(vector2.x, vector3.x), vector4.x), vector5.x)
		Dim num4 As Single = Mathf.Max(Mathf.Max(Mathf.Max(vector2.y, vector3.y), vector4.y), vector5.y)
		Dim num5 As Single = Mathf.Min(Mathf.Min(Mathf.Min(vector2.z, vector3.z), vector4.z), vector5.z)
		Dim num6 As Single = Mathf.Max(Mathf.Max(Mathf.Max(vector2.z, vector3.z), vector4.z), vector5.z)
		Dim bounds As Bounds = Nothing
		bounds.SetMinMax(New Vector3(num, num2), New Vector3(num3, num4))
		Return bounds
	End Function
End Module
