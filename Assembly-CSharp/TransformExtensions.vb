Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x02000370 RID: 880
Public Module TransformExtensions
	' Token: 0x06000A11 RID: 2577 RVA: 0x0007DE72 File Offset: 0x0007C272
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub ResetScale(transform As Transform)
		transform.localScale = Vector3.one
	End Sub

	' Token: 0x06000A12 RID: 2578 RVA: 0x0007DE7F File Offset: 0x0007C27F
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub ResetPosition(transform As Transform)
		transform.position = Vector3.zero
	End Sub

	' Token: 0x06000A13 RID: 2579 RVA: 0x0007DE8C File Offset: 0x0007C28C
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub ResetLocalPosition(transform As Transform)
		transform.localPosition = Vector3.zero
	End Sub

	' Token: 0x06000A14 RID: 2580 RVA: 0x0007DE99 File Offset: 0x0007C299
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub ResetRotation(transform As Transform)
		transform.eulerAngles = Vector3.zero
	End Sub

	' Token: 0x06000A15 RID: 2581 RVA: 0x0007DEA6 File Offset: 0x0007C2A6
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub ResetLocalRotation(transform As Transform)
		transform.localEulerAngles = Vector3.zero
	End Sub

	' Token: 0x06000A16 RID: 2582 RVA: 0x0007DEB3 File Offset: 0x0007C2B3
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub ResetTransforms(transform As Transform)
		transform.ResetPosition()
		transform.ResetRotation()
		transform.ResetScale()
	End Sub

	' Token: 0x06000A17 RID: 2583 RVA: 0x0007DEC7 File Offset: 0x0007C2C7
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub ResetLocalTransforms(transform As Transform)
		transform.ResetLocalPosition()
		transform.ResetLocalRotation()
		transform.ResetScale()
	End Sub

	' Token: 0x06000A18 RID: 2584 RVA: 0x0007DEDC File Offset: 0x0007C2DC
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub SetPosition(transform As Transform, Optional x As Single? = Nothing, Optional y As Single? = Nothing, Optional z As Single? = Nothing)
		Dim position As Vector3 = transform.position
		If x IsNot Nothing Then
			position.x = x.Value
		End If
		If y IsNot Nothing Then
			position.y = y.Value
		End If
		If z IsNot Nothing Then
			position.z = z.Value
		End If
		transform.position = position
	End Sub

	' Token: 0x06000A19 RID: 2585 RVA: 0x0007DF48 File Offset: 0x0007C348
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub SetLocalPosition(transform As Transform, Optional x As Single? = Nothing, Optional y As Single? = Nothing, Optional z As Single? = Nothing)
		Dim localPosition As Vector3 = transform.localPosition
		If x IsNot Nothing Then
			localPosition.x = x.Value
		End If
		If y IsNot Nothing Then
			localPosition.y = y.Value
		End If
		If z IsNot Nothing Then
			localPosition.z = z.Value
		End If
		transform.localPosition = localPosition
	End Sub

	' Token: 0x06000A1A RID: 2586 RVA: 0x0007DFB4 File Offset: 0x0007C3B4
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub SetEulerAngles(transform As Transform, Optional x As Single? = Nothing, Optional y As Single? = Nothing, Optional z As Single? = Nothing)
		Dim eulerAngles As Vector3 = transform.eulerAngles
		If x IsNot Nothing Then
			eulerAngles.x = x.Value
		End If
		If y IsNot Nothing Then
			eulerAngles.y = y.Value
		End If
		If z IsNot Nothing Then
			eulerAngles.z = z.Value
		End If
		transform.eulerAngles = eulerAngles
	End Sub

	' Token: 0x06000A1B RID: 2587 RVA: 0x0007E020 File Offset: 0x0007C420
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub SetLocalEulerAngles(transform As Transform, Optional x As Single? = Nothing, Optional y As Single? = Nothing, Optional z As Single? = Nothing)
		Dim localEulerAngles As Vector3 = transform.localEulerAngles
		If x IsNot Nothing Then
			localEulerAngles.x = x.Value
		End If
		If y IsNot Nothing Then
			localEulerAngles.y = y.Value
		End If
		If z IsNot Nothing Then
			localEulerAngles.z = z.Value
		End If
		transform.localEulerAngles = localEulerAngles
	End Sub

	' Token: 0x06000A1C RID: 2588 RVA: 0x0007E08C File Offset: 0x0007C48C
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub SetScale(transform As Transform, Optional x As Single? = Nothing, Optional y As Single? = Nothing, Optional z As Single? = Nothing)
		Dim localScale As Vector3 = transform.localScale
		If x IsNot Nothing Then
			localScale.x = x.Value
		End If
		If y IsNot Nothing Then
			localScale.y = y.Value
		End If
		If z IsNot Nothing Then
			localScale.z = z.Value
		End If
		transform.localScale = localScale
	End Sub

	' Token: 0x06000A1D RID: 2589 RVA: 0x0007E0F8 File Offset: 0x0007C4F8
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub AddPosition(transform As Transform, Optional x As Single = 0F, Optional y As Single = 0F, Optional z As Single = 0F)
		Dim position As Vector3 = transform.position
		position.x += x
		position.y += y
		position.z += z
		transform.position = position
	End Sub

	' Token: 0x06000A1E RID: 2590 RVA: 0x0007E140 File Offset: 0x0007C540
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub AddLocalPosition(transform As Transform, Optional x As Single = 0F, Optional y As Single = 0F, Optional z As Single = 0F)
		Dim localPosition As Vector3 = transform.localPosition
		localPosition.x += x
		localPosition.y += y
		localPosition.z += z
		transform.localPosition = localPosition
	End Sub

	' Token: 0x06000A1F RID: 2591 RVA: 0x0007E188 File Offset: 0x0007C588
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub AddPositionForward2D(transform As Transform, forward As Single)
		transform.position += transform.right * forward
	End Sub

	' Token: 0x06000A20 RID: 2592 RVA: 0x0007E1A8 File Offset: 0x0007C5A8
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub AddEulerAngles(transform As Transform, Optional x As Single = 0F, Optional y As Single = 0F, Optional z As Single = 0F)
		Dim eulerAngles As Vector3 = transform.eulerAngles
		eulerAngles.x += x
		eulerAngles.y += y
		eulerAngles.z += z
		transform.eulerAngles = eulerAngles
	End Sub

	' Token: 0x06000A21 RID: 2593 RVA: 0x0007E1F0 File Offset: 0x0007C5F0
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub AddLocalEulerAngles(transform As Transform, Optional x As Single = 0F, Optional y As Single = 0F, Optional z As Single = 0F)
		Dim localEulerAngles As Vector3 = transform.localEulerAngles
		localEulerAngles.x += x
		localEulerAngles.y += y
		localEulerAngles.z += z
		transform.localEulerAngles = localEulerAngles
	End Sub

	' Token: 0x06000A22 RID: 2594 RVA: 0x0007E238 File Offset: 0x0007C638
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub AddScale(transform As Transform, Optional x As Single = 0F, Optional y As Single = 0F, Optional z As Single = 0F)
		Dim localScale As Vector3 = transform.localScale
		localScale.x += x
		localScale.y += y
		localScale.z += z
		transform.localScale = localScale
	End Sub

	' Token: 0x06000A23 RID: 2595 RVA: 0x0007E280 File Offset: 0x0007C680
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub MoveForward(transform As Transform, amount As Single)
		transform.position += transform.forward * amount
	End Sub

	' Token: 0x06000A24 RID: 2596 RVA: 0x0007E29F File Offset: 0x0007C69F
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub MoveForward2D(transform As Transform, amount As Single)
		transform.position += transform.right * amount
	End Sub

	' Token: 0x06000A25 RID: 2597 RVA: 0x0007E2BE File Offset: 0x0007C6BE
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub LookAt2D(transform As Transform, target As Transform)
		transform.LookAt2D(target.position)
	End Sub

	' Token: 0x06000A26 RID: 2598 RVA: 0x0007E2CC File Offset: 0x0007C6CC
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Sub LookAt2D(transform As Transform, target As Vector3)
		Dim vector As Vector3 = target - transform.position
		vector.Normalize()
		transform.rotation = Quaternion.Euler(0F, 0F, Mathf.Atan2(vector.y, vector.x) * 57.29578F)
	End Sub

	' Token: 0x06000A27 RID: 2599 RVA: 0x0007E31C File Offset: 0x0007C71C
	<System.Runtime.CompilerServices.ExtensionAttribute()>
	Public Function GetChildTransforms(transform As Transform) As Transform()
		Dim list As List(Of Transform) = New List(Of Transform)()
		Dim enumerator As IEnumerator = transform.GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim obj As Object = enumerator.Current
				Dim transform2 As Transform = CType(obj, Transform)
				list.Add(transform2)
			End While
		Finally
			Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable2 As IDisposable = disposable
			If disposable IsNot Nothing Then
				disposable2.Dispose()
			End If
		End Try
		list.Remove(transform)
		Return list.ToArray()
	End Function
End Module
