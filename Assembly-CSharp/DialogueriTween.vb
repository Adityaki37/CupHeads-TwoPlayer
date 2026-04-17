Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000B83 RID: 2947
Public Class DialogueriTween
	Inherits MonoBehaviour

	' Token: 0x060046DA RID: 18138 RVA: 0x0024FF0C File Offset: 0x0024E30C
	Public Shared Sub Init(target As GameObject)
		DialogueriTween.MoveBy(target, Vector3.zero, 0F)
	End Sub

	' Token: 0x060046DB RID: 18139 RVA: 0x0024FF20 File Offset: 0x0024E320
	Public Shared Sub CameraFadeFrom(amount As Single, time As Single)
		If DialogueriTween.cameraFade Then
			DialogueriTween.CameraFadeFrom(DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
		Else
			Global.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", Nothing)
		End If
	End Sub

	' Token: 0x060046DC RID: 18140 RVA: 0x0024FF7E File Offset: 0x0024E37E
	Public Shared Sub CameraFadeFrom(args As Hashtable)
		If DialogueriTween.cameraFade Then
			DialogueriTween.ColorFrom(DialogueriTween.cameraFade, args)
		Else
			Global.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", Nothing)
		End If
	End Sub

	' Token: 0x060046DD RID: 18141 RVA: 0x0024FFAC File Offset: 0x0024E3AC
	Public Shared Sub CameraFadeTo(amount As Single, time As Single)
		If DialogueriTween.cameraFade Then
			DialogueriTween.CameraFadeTo(DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
		Else
			Global.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", Nothing)
		End If
	End Sub

	' Token: 0x060046DE RID: 18142 RVA: 0x0025000A File Offset: 0x0024E40A
	Public Shared Sub CameraFadeTo(args As Hashtable)
		If DialogueriTween.cameraFade Then
			DialogueriTween.ColorTo(DialogueriTween.cameraFade, args)
		Else
			Global.Debug.LogError("iTween Error: You must first add a camera fade object with CameraFadeAdd() before atttempting to use camera fading.", Nothing)
		End If
	End Sub

	' Token: 0x060046DF RID: 18143 RVA: 0x00250038 File Offset: 0x0024E438
	Public Shared Sub ValueTo(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		If Not args.Contains("onupdate") OrElse Not args.Contains("from") OrElse Not args.Contains("to") Then
			Global.Debug.LogError("iTween Error: ValueTo() requires an 'onupdate' callback function and a 'from' and 'to' property.  The supplied 'onupdate' callback must accept a single argument that is the same type as the supplied 'from' and 'to' properties!", Nothing)
			Return
		End If
		args("type") = "value"
		If args("from").[GetType]() Is GetType(Vector2) Then
			args("method") = "vector2"
		ElseIf args("from").[GetType]() Is GetType(Vector3) Then
			args("method") = "vector3"
		ElseIf args("from").[GetType]() Is GetType(Rect) Then
			args("method") = "rect"
		ElseIf args("from").[GetType]() Is GetType(Single) Then
			args("method") = "float"
		Else
			If args("from").[GetType]() IsNot GetType(Color) Then
				Global.Debug.LogError("iTween Error: ValueTo() only works with interpolating Vector3s, Vector2s, floats, ints, Rects and Colors!", Nothing)
				Return
			End If
			args("method") = "color"
		End If
		If Not args.Contains("easetype") Then
			args.Add("easetype", DialogueriTween.EaseType.linear)
		End If
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046E0 RID: 18144 RVA: 0x002501D2 File Offset: 0x0024E5D2
	Public Shared Sub FadeFrom(target As GameObject, alpha As Single, time As Single)
		DialogueriTween.FadeFrom(target, DialogueriTween.Hash(New Object() { "alpha", alpha, "time", time }))
	End Sub

	' Token: 0x060046E1 RID: 18145 RVA: 0x00250207 File Offset: 0x0024E607
	Public Shared Sub FadeFrom(target As GameObject, args As Hashtable)
		DialogueriTween.ColorFrom(target, args)
	End Sub

	' Token: 0x060046E2 RID: 18146 RVA: 0x00250210 File Offset: 0x0024E610
	Public Shared Sub FadeTo(target As GameObject, alpha As Single, time As Single)
		DialogueriTween.FadeTo(target, DialogueriTween.Hash(New Object() { "alpha", alpha, "time", time }))
	End Sub

	' Token: 0x060046E3 RID: 18147 RVA: 0x00250245 File Offset: 0x0024E645
	Public Shared Sub FadeTo(target As GameObject, args As Hashtable)
		DialogueriTween.ColorTo(target, args)
	End Sub

	' Token: 0x060046E4 RID: 18148 RVA: 0x0025024E File Offset: 0x0024E64E
	Public Shared Sub ColorFrom(target As GameObject, color As Color, time As Single)
		DialogueriTween.ColorFrom(target, DialogueriTween.Hash(New Object() { "color", color, "time", time }))
	End Sub

	' Token: 0x060046E5 RID: 18149 RVA: 0x00250284 File Offset: 0x0024E684
	Public Shared Sub ColorFrom(target As GameObject, args As Hashtable)
		Dim color As Color = Nothing
		Dim color2 As Color = Nothing
		args = DialogueriTween.CleanArgs(args)
		If Not args.Contains("includechildren") OrElse CBool(args("includechildren")) Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					Dim hashtable As Hashtable = CType(args.Clone(), Hashtable)
					hashtable("ischild") = True
					DialogueriTween.ColorFrom(transform.gameObject, hashtable)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
		If Not args.Contains("easetype") Then
			args.Add("easetype", DialogueriTween.EaseType.linear)
		End If
		If target.GetComponent(GetType(GUITexture)) Then
			Dim color3 As Color = target.GetComponent(Of GUITexture)().color
			color = color3
			color2 = color3
		ElseIf target.GetComponent(GetType(GUIText)) Then
			Dim color4 As Color = target.GetComponent(Of GUIText)().material.color
			color = color4
			color2 = color4
		ElseIf target.GetComponent(Of Renderer)() Then
			Dim color5 As Color = target.GetComponent(Of Renderer)().material.color
			color = color5
			color2 = color5
		ElseIf target.GetComponent(Of Light)() Then
			Dim color6 As Color = target.GetComponent(Of Light)().color
			color = color6
			color2 = color6
		End If
		If args.Contains("color") Then
			color = CType(args("color"), Color)
		Else
			If args.Contains("r") Then
				color.r = CSng(args("r"))
			End If
			If args.Contains("g") Then
				color.g = CSng(args("g"))
			End If
			If args.Contains("b") Then
				color.b = CSng(args("b"))
			End If
			If args.Contains("a") Then
				color.a = CSng(args("a"))
			End If
		End If
		If args.Contains("amount") Then
			color.a = CSng(args("amount"))
			args.Remove("amount")
		ElseIf args.Contains("alpha") Then
			color.a = CSng(args("alpha"))
			args.Remove("alpha")
		End If
		If target.GetComponent(GetType(GUITexture)) Then
			target.GetComponent(Of GUITexture)().color = color
		ElseIf target.GetComponent(GetType(GUIText)) Then
			target.GetComponent(Of GUIText)().material.color = color
		ElseIf target.GetComponent(Of Renderer)() Then
			target.GetComponent(Of Renderer)().material.color = color
		ElseIf target.GetComponent(Of Light)() Then
			target.GetComponent(Of Light)().color = color
		End If
		args("color") = color2
		args("type") = "color"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046E6 RID: 18150 RVA: 0x00250614 File Offset: 0x0024EA14
	Public Shared Sub ColorTo(target As GameObject, color As Color, time As Single)
		DialogueriTween.ColorTo(target, DialogueriTween.Hash(New Object() { "color", color, "time", time }))
	End Sub

	' Token: 0x060046E7 RID: 18151 RVA: 0x0025064C File Offset: 0x0024EA4C
	Public Shared Sub ColorTo(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		If Not args.Contains("includechildren") OrElse CBool(args("includechildren")) Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					Dim hashtable As Hashtable = CType(args.Clone(), Hashtable)
					hashtable("ischild") = True
					DialogueriTween.ColorTo(transform.gameObject, hashtable)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
		If Not args.Contains("easetype") Then
			args.Add("easetype", DialogueriTween.EaseType.linear)
		End If
		args("type") = "color"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046E8 RID: 18152 RVA: 0x0025074C File Offset: 0x0024EB4C
	Public Shared Sub AudioFrom(target As GameObject, volume As Single, pitch As Single, time As Single)
		DialogueriTween.AudioFrom(target, DialogueriTween.Hash(New Object() { "volume", volume, "pitch", pitch, "time", time }))
	End Sub

	' Token: 0x060046E9 RID: 18153 RVA: 0x002507A0 File Offset: 0x0024EBA0
	Public Shared Sub AudioFrom(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		Dim audioSource As AudioSource
		If args.Contains("audiosource") Then
			audioSource = CType(args("audiosource"), AudioSource)
		Else
			If Not target.GetComponent(GetType(AudioSource)) Then
				Global.Debug.LogError("iTween Error: AudioFrom requires an AudioSource.", Nothing)
				Return
			End If
			audioSource = target.GetComponent(Of AudioSource)()
		End If
		Dim volume As Single = audioSource.volume
		Dim num As Single = volume
		Dim vector As Vector2
		vector.x = volume
		Dim vector2 As Vector2
		vector2.x = num
		Dim pitch As Single = audioSource.pitch
		num = pitch
		vector.y = pitch
		vector2.y = num
		If args.Contains("volume") Then
			vector.x = CSng(args("volume"))
		End If
		If args.Contains("pitch") Then
			vector.y = CSng(args("pitch"))
		End If
		audioSource.volume = vector.x
		audioSource.pitch = vector.y
		args("volume") = vector2.x
		args("pitch") = vector2.y
		If Not args.Contains("easetype") Then
			args.Add("easetype", DialogueriTween.EaseType.linear)
		End If
		args("type") = "audio"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046EA RID: 18154 RVA: 0x0025091C File Offset: 0x0024ED1C
	Public Shared Sub AudioTo(target As GameObject, volume As Single, pitch As Single, time As Single)
		DialogueriTween.AudioTo(target, DialogueriTween.Hash(New Object() { "volume", volume, "pitch", pitch, "time", time }))
	End Sub

	' Token: 0x060046EB RID: 18155 RVA: 0x00250970 File Offset: 0x0024ED70
	Public Shared Sub AudioTo(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		If Not args.Contains("easetype") Then
			args.Add("easetype", DialogueriTween.EaseType.linear)
		End If
		args("type") = "audio"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046EC RID: 18156 RVA: 0x002509CE File Offset: 0x0024EDCE
	Public Shared Sub Stab(target As GameObject, audioclip As AudioClip, delay As Single)
		DialogueriTween.Stab(target, DialogueriTween.Hash(New Object() { "audioclip", audioclip, "delay", delay }))
	End Sub

	' Token: 0x060046ED RID: 18157 RVA: 0x002509FE File Offset: 0x0024EDFE
	Public Shared Sub Stab(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "stab"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046EE RID: 18158 RVA: 0x00250A1F File Offset: 0x0024EE1F
	Public Shared Sub LookFrom(target As GameObject, looktarget As Vector3, time As Single)
		DialogueriTween.LookFrom(target, DialogueriTween.Hash(New Object() { "looktarget", looktarget, "time", time }))
	End Sub

	' Token: 0x060046EF RID: 18159 RVA: 0x00250A54 File Offset: 0x0024EE54
	Public Shared Sub LookFrom(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		Dim eulerAngles As Vector3 = target.transform.eulerAngles
		If args("looktarget").[GetType]() Is GetType(Transform) Then
			Dim transform As Transform = target.transform
			Dim transform2 As Transform = CType(args("looktarget"), Transform)
			Dim vector As Vector3? = CType(args("up"), Vector3?)
			transform.LookAt(transform2, If((vector Is Nothing), DialogueriTween.Defaults.up, vector.Value))
		ElseIf args("looktarget").[GetType]() Is GetType(Vector3) Then
			Dim transform3 As Transform = target.transform
			Dim vector2 As Vector3 = CType(args("looktarget"), Vector3)
			Dim vector3 As Vector3? = CType(args("up"), Vector3?)
			transform3.LookAt(vector2, If((vector3 Is Nothing), DialogueriTween.Defaults.up, vector3.Value))
		End If
		If args.Contains("axis") Then
			Dim eulerAngles2 As Vector3 = target.transform.eulerAngles
			Dim text As String = CStr(args("axis"))
			If text IsNot Nothing Then
				If Not(text = "x") Then
					If Not(text = "y") Then
						If text = "z" Then
							eulerAngles2.x = eulerAngles.x
							eulerAngles2.y = eulerAngles.y
						End If
					Else
						eulerAngles2.x = eulerAngles.x
						eulerAngles2.z = eulerAngles.z
					End If
				Else
					eulerAngles2.y = eulerAngles.y
					eulerAngles2.z = eulerAngles.z
				End If
			End If
			target.transform.eulerAngles = eulerAngles2
		End If
		args("rotation") = eulerAngles
		args("type") = "rotate"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046F0 RID: 18160 RVA: 0x00250C5E File Offset: 0x0024F05E
	Public Shared Sub LookTo(target As GameObject, looktarget As Vector3, time As Single)
		DialogueriTween.LookTo(target, DialogueriTween.Hash(New Object() { "looktarget", looktarget, "time", time }))
	End Sub

	' Token: 0x060046F1 RID: 18161 RVA: 0x00250C94 File Offset: 0x0024F094
	Public Shared Sub LookTo(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		If args.Contains("looktarget") AndAlso args("looktarget").[GetType]() Is GetType(Transform) Then
			Dim transform As Transform = CType(args("looktarget"), Transform)
			args("position") = New Vector3(transform.position.x, transform.position.y, transform.position.z)
			args("rotation") = New Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)
		End If
		args("type") = "look"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046F2 RID: 18162 RVA: 0x00250D91 File Offset: 0x0024F191
	Public Shared Sub MoveTo(target As GameObject, position As Vector3, time As Single)
		DialogueriTween.MoveTo(target, DialogueriTween.Hash(New Object() { "position", position, "time", time }))
	End Sub

	' Token: 0x060046F3 RID: 18163 RVA: 0x00250DC8 File Offset: 0x0024F1C8
	Public Shared Sub MoveTo(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		If args.Contains("position") AndAlso args("position").[GetType]() Is GetType(Transform) Then
			Dim transform As Transform = CType(args("position"), Transform)
			args("position") = New Vector3(transform.position.x, transform.position.y, transform.position.z)
			args("rotation") = New Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)
			args("scale") = New Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z)
		End If
		args("type") = "move"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046F4 RID: 18164 RVA: 0x00250F07 File Offset: 0x0024F307
	Public Shared Sub MoveFrom(target As GameObject, position As Vector3, time As Single)
		DialogueriTween.MoveFrom(target, DialogueriTween.Hash(New Object() { "position", position, "time", time }))
	End Sub

	' Token: 0x060046F5 RID: 18165 RVA: 0x00250F3C File Offset: 0x0024F33C
	Public Shared Sub MoveFrom(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		Dim flag As Boolean
		If args.Contains("islocal") Then
			flag = CBool(args("islocal"))
		Else
			flag = DialogueriTween.Defaults.isLocal
		End If
		If args.Contains("path") Then
			Dim array2 As Vector3()
			If args("path").[GetType]() Is GetType(Vector3()) Then
				Dim array As Vector3() = CType(args("path"), Vector3())
				array2 = New Vector3(array.Length - 1) {}
				Array.Copy(array, array2, array.Length)
			Else
				Dim array3 As Transform() = CType(args("path"), Transform())
				array2 = New Vector3(array3.Length - 1) {}
				For i As Integer = 0 To array3.Length - 1
					array2(i) = array3(i).position
				Next
			End If
			If array2(array2.Length - 1) <> target.transform.position Then
				Dim array4 As Vector3() = New Vector3(array2.Length + 1 - 1) {}
				Array.Copy(array2, array4, array2.Length)
				If flag Then
					array4(array4.Length - 1) = target.transform.localPosition
					target.transform.localPosition = array4(0)
				Else
					array4(array4.Length - 1) = target.transform.position
					target.transform.position = array4(0)
				End If
				args("path") = array4
			Else
				If flag Then
					target.transform.localPosition = array2(0)
				Else
					target.transform.position = array2(0)
				End If
				args("path") = array2
			End If
		Else
			Dim vector As Vector3
			Dim vector2 As Vector3
			If flag Then
				Dim localPosition As Vector3 = target.transform.localPosition
				vector = localPosition
				vector2 = localPosition
			Else
				Dim position As Vector3 = target.transform.position
				vector = position
				vector2 = position
			End If
			If args.Contains("position") Then
				If args("position").[GetType]() Is GetType(Transform) Then
					Dim transform As Transform = CType(args("position"), Transform)
					vector = transform.position
				ElseIf args("position").[GetType]() Is GetType(Vector3) Then
					vector = CType(args("position"), Vector3)
				End If
			Else
				If args.Contains("x") Then
					vector.x = CSng(args("x"))
				End If
				If args.Contains("y") Then
					vector.y = CSng(args("y"))
				End If
				If args.Contains("z") Then
					vector.z = CSng(args("z"))
				End If
			End If
			If flag Then
				target.transform.localPosition = vector
			Else
				target.transform.position = vector
			End If
			args("position") = vector2
		End If
		args("type") = "move"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046F6 RID: 18166 RVA: 0x002512A8 File Offset: 0x0024F6A8
	Public Shared Sub MoveAdd(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.MoveAdd(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x060046F7 RID: 18167 RVA: 0x002512DD File Offset: 0x0024F6DD
	Public Shared Sub MoveAdd(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "move"
		args("method") = "add"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046F8 RID: 18168 RVA: 0x0025130E File Offset: 0x0024F70E
	Public Shared Sub MoveBy(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.MoveBy(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x060046F9 RID: 18169 RVA: 0x00251343 File Offset: 0x0024F743
	Public Shared Sub MoveBy(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "move"
		args("method") = "by"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046FA RID: 18170 RVA: 0x00251374 File Offset: 0x0024F774
	Public Shared Sub ScaleTo(target As GameObject, scale As Vector3, time As Single)
		DialogueriTween.ScaleTo(target, DialogueriTween.Hash(New Object() { "scale", scale, "time", time }))
	End Sub

	' Token: 0x060046FB RID: 18171 RVA: 0x002513AC File Offset: 0x0024F7AC
	Public Shared Sub ScaleTo(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		If args.Contains("scale") AndAlso args("scale").[GetType]() Is GetType(Transform) Then
			Dim transform As Transform = CType(args("scale"), Transform)
			args("position") = New Vector3(transform.position.x, transform.position.y, transform.position.z)
			args("rotation") = New Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)
			args("scale") = New Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z)
		End If
		args("type") = "scale"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046FC RID: 18172 RVA: 0x002514EB File Offset: 0x0024F8EB
	Public Shared Sub ScaleFrom(target As GameObject, scale As Vector3, time As Single)
		DialogueriTween.ScaleFrom(target, DialogueriTween.Hash(New Object() { "scale", scale, "time", time }))
	End Sub

	' Token: 0x060046FD RID: 18173 RVA: 0x00251520 File Offset: 0x0024F920
	Public Shared Sub ScaleFrom(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		Dim localScale As Vector3 = target.transform.localScale
		Dim vector As Vector3 = localScale
		Dim vector2 As Vector3 = localScale
		If args.Contains("scale") Then
			If args("scale").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = CType(args("scale"), Transform)
				vector = transform.localScale
			ElseIf args("scale").[GetType]() Is GetType(Vector3) Then
				vector = CType(args("scale"), Vector3)
			End If
		Else
			If args.Contains("x") Then
				vector.x = CSng(args("x"))
			End If
			If args.Contains("y") Then
				vector.y = CSng(args("y"))
			End If
			If args.Contains("z") Then
				vector.z = CSng(args("z"))
			End If
		End If
		target.transform.localScale = vector
		args("scale") = vector2
		args("type") = "scale"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x060046FE RID: 18174 RVA: 0x0025167D File Offset: 0x0024FA7D
	Public Shared Sub ScaleAdd(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.ScaleAdd(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x060046FF RID: 18175 RVA: 0x002516B2 File Offset: 0x0024FAB2
	Public Shared Sub ScaleAdd(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "scale"
		args("method") = "add"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004700 RID: 18176 RVA: 0x002516E3 File Offset: 0x0024FAE3
	Public Shared Sub ScaleBy(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.ScaleBy(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x06004701 RID: 18177 RVA: 0x00251718 File Offset: 0x0024FB18
	Public Shared Sub ScaleBy(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "scale"
		args("method") = "by"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004702 RID: 18178 RVA: 0x00251749 File Offset: 0x0024FB49
	Public Shared Sub RotateTo(target As GameObject, rotation As Vector3, time As Single)
		DialogueriTween.RotateTo(target, DialogueriTween.Hash(New Object() { "rotation", rotation, "time", time }))
	End Sub

	' Token: 0x06004703 RID: 18179 RVA: 0x00251780 File Offset: 0x0024FB80
	Public Shared Sub RotateTo(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		If args.Contains("rotation") AndAlso args("rotation").[GetType]() Is GetType(Transform) Then
			Dim transform As Transform = CType(args("rotation"), Transform)
			args("position") = New Vector3(transform.position.x, transform.position.y, transform.position.z)
			args("rotation") = New Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z)
			args("scale") = New Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z)
		End If
		args("type") = "rotate"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004704 RID: 18180 RVA: 0x002518BF File Offset: 0x0024FCBF
	Public Shared Sub RotateFrom(target As GameObject, rotation As Vector3, time As Single)
		DialogueriTween.RotateFrom(target, DialogueriTween.Hash(New Object() { "rotation", rotation, "time", time }))
	End Sub

	' Token: 0x06004705 RID: 18181 RVA: 0x002518F4 File Offset: 0x0024FCF4
	Public Shared Sub RotateFrom(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		Dim flag As Boolean
		If args.Contains("islocal") Then
			flag = CBool(args("islocal"))
		Else
			flag = DialogueriTween.Defaults.isLocal
		End If
		Dim vector As Vector3
		Dim vector2 As Vector3
		If flag Then
			Dim localEulerAngles As Vector3 = target.transform.localEulerAngles
			vector = localEulerAngles
			vector2 = localEulerAngles
		Else
			Dim eulerAngles As Vector3 = target.transform.eulerAngles
			vector = eulerAngles
			vector2 = eulerAngles
		End If
		If args.Contains("rotation") Then
			If args("rotation").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = CType(args("rotation"), Transform)
				vector = transform.eulerAngles
			ElseIf args("rotation").[GetType]() Is GetType(Vector3) Then
				vector = CType(args("rotation"), Vector3)
			End If
		Else
			If args.Contains("x") Then
				vector.x = CSng(args("x"))
			End If
			If args.Contains("y") Then
				vector.y = CSng(args("y"))
			End If
			If args.Contains("z") Then
				vector.z = CSng(args("z"))
			End If
		End If
		If flag Then
			target.transform.localEulerAngles = vector
		Else
			target.transform.eulerAngles = vector
		End If
		args("rotation") = vector2
		args("type") = "rotate"
		args("method") = "to"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004706 RID: 18182 RVA: 0x00251AAD File Offset: 0x0024FEAD
	Public Shared Sub RotateAdd(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.RotateAdd(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x06004707 RID: 18183 RVA: 0x00251AE2 File Offset: 0x0024FEE2
	Public Shared Sub RotateAdd(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "rotate"
		args("method") = "add"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004708 RID: 18184 RVA: 0x00251B13 File Offset: 0x0024FF13
	Public Shared Sub RotateBy(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.RotateBy(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x06004709 RID: 18185 RVA: 0x00251B48 File Offset: 0x0024FF48
	Public Shared Sub RotateBy(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "rotate"
		args("method") = "by"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x0600470A RID: 18186 RVA: 0x00251B79 File Offset: 0x0024FF79
	Public Shared Sub ShakePosition(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.ShakePosition(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x0600470B RID: 18187 RVA: 0x00251BAE File Offset: 0x0024FFAE
	Public Shared Sub ShakePosition(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "shake"
		args("method") = "position"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x0600470C RID: 18188 RVA: 0x00251BDF File Offset: 0x0024FFDF
	Public Shared Sub ShakeScale(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.ShakeScale(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x0600470D RID: 18189 RVA: 0x00251C14 File Offset: 0x00250014
	Public Shared Sub ShakeScale(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "shake"
		args("method") = "scale"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x0600470E RID: 18190 RVA: 0x00251C45 File Offset: 0x00250045
	Public Shared Sub ShakeRotation(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.ShakeRotation(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x0600470F RID: 18191 RVA: 0x00251C7A File Offset: 0x0025007A
	Public Shared Sub ShakeRotation(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "shake"
		args("method") = "rotation"
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004710 RID: 18192 RVA: 0x00251CAB File Offset: 0x002500AB
	Public Shared Sub PunchPosition(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.PunchPosition(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x06004711 RID: 18193 RVA: 0x00251CE0 File Offset: 0x002500E0
	Public Shared Sub PunchPosition(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "punch"
		args("method") = "position"
		args("easetype") = DialogueriTween.EaseType.punch
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004712 RID: 18194 RVA: 0x00251D2E File Offset: 0x0025012E
	Public Shared Sub PunchRotation(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.PunchRotation(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x06004713 RID: 18195 RVA: 0x00251D64 File Offset: 0x00250164
	Public Shared Sub PunchRotation(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "punch"
		args("method") = "rotation"
		args("easetype") = DialogueriTween.EaseType.punch
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004714 RID: 18196 RVA: 0x00251DB2 File Offset: 0x002501B2
	Public Shared Sub PunchScale(target As GameObject, amount As Vector3, time As Single)
		DialogueriTween.PunchScale(target, DialogueriTween.Hash(New Object() { "amount", amount, "time", time }))
	End Sub

	' Token: 0x06004715 RID: 18197 RVA: 0x00251DE8 File Offset: 0x002501E8
	Public Shared Sub PunchScale(target As GameObject, args As Hashtable)
		args = DialogueriTween.CleanArgs(args)
		args("type") = "punch"
		args("method") = "scale"
		args("easetype") = DialogueriTween.EaseType.punch
		DialogueriTween.Launch(target, args)
	End Sub

	' Token: 0x06004716 RID: 18198 RVA: 0x00251E38 File Offset: 0x00250238
	Private Sub GenerateTargets()
		Dim text As String = Me.type
		Select Case text
			Case "value"
				Dim text2 As String = Me.method
				If text2 IsNot Nothing Then
					If Not(text2 = "float") Then
						If Not(text2 = "vector2") Then
							If Not(text2 = "vector3") Then
								If Not(text2 = "color") Then
									If text2 = "rect" Then
										Me.GenerateRectTargets()
										Me.apply = AddressOf Me.ApplyRectTargets
									End If
								Else
									Me.GenerateColorTargets()
									Me.apply = AddressOf Me.ApplyColorTargets
								End If
							Else
								Me.GenerateVector3Targets()
								Me.apply = AddressOf Me.ApplyVector3Targets
							End If
						Else
							Me.GenerateVector2Targets()
							Me.apply = AddressOf Me.ApplyVector2Targets
						End If
					Else
						Me.GenerateFloatTargets()
						Me.apply = AddressOf Me.ApplyFloatTargets
					End If
				End If
			Case "color"
				Dim text3 As String = Me.method
				If text3 IsNot Nothing Then
					If text3 = "to" Then
						Me.GenerateColorToTargets()
						Me.apply = AddressOf Me.ApplyColorToTargets
					End If
				End If
			Case "audio"
				Dim text4 As String = Me.method
				If text4 IsNot Nothing Then
					If text4 = "to" Then
						Me.GenerateAudioToTargets()
						Me.apply = AddressOf Me.ApplyAudioToTargets
					End If
				End If
			Case "move"
				Dim text5 As String = Me.method
				If text5 IsNot Nothing Then
					If Not(text5 = "to") Then
						If text5 = "by" OrElse text5 = "add" Then
							Me.GenerateMoveByTargets()
							Me.apply = AddressOf Me.ApplyMoveByTargets
						End If
					ElseIf Me.tweenArguments.Contains("path") Then
						Me.GenerateMoveToPathTargets()
						Me.apply = AddressOf Me.ApplyMoveToPathTargets
					Else
						Me.GenerateMoveToTargets()
						Me.apply = AddressOf Me.ApplyMoveToTargets
					End If
				End If
			Case "scale"
				Dim text6 As String = Me.method
				If text6 IsNot Nothing Then
					If Not(text6 = "to") Then
						If Not(text6 = "by") Then
							If text6 = "add" Then
								Me.GenerateScaleAddTargets()
								Me.apply = AddressOf Me.ApplyScaleToTargets
							End If
						Else
							Me.GenerateScaleByTargets()
							Me.apply = AddressOf Me.ApplyScaleToTargets
						End If
					Else
						Me.GenerateScaleToTargets()
						Me.apply = AddressOf Me.ApplyScaleToTargets
					End If
				End If
			Case "rotate"
				Dim text7 As String = Me.method
				If text7 IsNot Nothing Then
					If Not(text7 = "to") Then
						If Not(text7 = "add") Then
							If text7 = "by" Then
								Me.GenerateRotateByTargets()
								Me.apply = AddressOf Me.ApplyRotateAddTargets
							End If
						Else
							Me.GenerateRotateAddTargets()
							Me.apply = AddressOf Me.ApplyRotateAddTargets
						End If
					Else
						Me.GenerateRotateToTargets()
						Me.apply = AddressOf Me.ApplyRotateToTargets
					End If
				End If
			Case "shake"
				Dim text8 As String = Me.method
				If text8 IsNot Nothing Then
					If Not(text8 = "position") Then
						If Not(text8 = "scale") Then
							If text8 = "rotation" Then
								Me.GenerateShakeRotationTargets()
								Me.apply = AddressOf Me.ApplyShakeRotationTargets
							End If
						Else
							Me.GenerateShakeScaleTargets()
							Me.apply = AddressOf Me.ApplyShakeScaleTargets
						End If
					Else
						Me.GenerateShakePositionTargets()
						Me.apply = AddressOf Me.ApplyShakePositionTargets
					End If
				End If
			Case "punch"
				Dim text9 As String = Me.method
				If text9 IsNot Nothing Then
					If Not(text9 = "position") Then
						If Not(text9 = "rotation") Then
							If text9 = "scale" Then
								Me.GeneratePunchScaleTargets()
								Me.apply = AddressOf Me.ApplyPunchScaleTargets
							End If
						Else
							Me.GeneratePunchRotationTargets()
							Me.apply = AddressOf Me.ApplyPunchRotationTargets
						End If
					Else
						Me.GeneratePunchPositionTargets()
						Me.apply = AddressOf Me.ApplyPunchPositionTargets
					End If
				End If
			Case "look"
				Dim text10 As String = Me.method
				If text10 IsNot Nothing Then
					If text10 = "to" Then
						Me.GenerateLookToTargets()
						Me.apply = AddressOf Me.ApplyLookToTargets
					End If
				End If
			Case "stab"
				Me.GenerateStabTargets()
				Me.apply = AddressOf Me.ApplyStabTargets
		End Select
	End Sub

	' Token: 0x06004717 RID: 18199 RVA: 0x00252458 File Offset: 0x00250858
	Private Sub GenerateRectTargets()
		Me.rects = New Rect(2) {}
		Me.rects(0) = CType(Me.tweenArguments("from"), Rect)
		Me.rects(1) = CType(Me.tweenArguments("to"), Rect)
	End Sub

	' Token: 0x06004718 RID: 18200 RVA: 0x002524C0 File Offset: 0x002508C0
	Private Sub GenerateColorTargets()
		Me.colors = New Color(0, 2) {}
		Me.colors(0, 0) = CType(Me.tweenArguments("from"), Color)
		Me.colors(0, 1) = CType(Me.tweenArguments("to"), Color)
	End Sub

	' Token: 0x06004719 RID: 18201 RVA: 0x00252528 File Offset: 0x00250928
	Private Sub GenerateVector3Targets()
		Me.vector3s = New Vector3(2) {}
		Me.vector3s(0) = CType(Me.tweenArguments("from"), Vector3)
		Me.vector3s(1) = CType(Me.tweenArguments("to"), Vector3)
		If Me.tweenArguments.Contains("speed") Then
			Dim num As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x0600471A RID: 18202 RVA: 0x002525EC File Offset: 0x002509EC
	Private Sub GenerateVector2Targets()
		Me.vector2s = New Vector2(2) {}
		Me.vector2s(0) = CType(Me.tweenArguments("from"), Vector2)
		Me.vector2s(1) = CType(Me.tweenArguments("to"), Vector2)
		If Me.tweenArguments.Contains("speed") Then
			Dim vector As Vector3 = New Vector3(Me.vector2s(0).x, Me.vector2s(0).y, 0F)
			Dim vector2 As Vector3 = New Vector3(Me.vector2s(1).x, Me.vector2s(1).y, 0F)
			Dim num As Single = Math.Abs(Vector3.Distance(vector, vector2))
			Me.time = num / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x0600471B RID: 18203 RVA: 0x002526EC File Offset: 0x00250AEC
	Private Sub GenerateFloatTargets()
		Me.floats = New Single(2) {}
		Me.floats(0) = CSng(Me.tweenArguments("from"))
		Me.floats(1) = CSng(Me.tweenArguments("to"))
		If Me.tweenArguments.Contains("speed") Then
			Dim num As Single = Math.Abs(Me.floats(0) - Me.floats(1))
			Me.time = num / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x0600471C RID: 18204 RVA: 0x00252788 File Offset: 0x00250B88
	Private Sub GenerateColorToTargets()
		If MyBase.GetComponent(GetType(GUITexture)) Then
			Me.colors = New Color(0, 2) {}
			Dim ptr As __ByRef(Of Color) = Me.colors(0, 0)
			Dim ptr2 As __ByRef(Of Color) = Me.colors(0, 1)
			Dim color As Color = MyBase.GetComponent(Of GUITexture)().color
			Dim color2 As Color = color
			ptr2 = color
			ptr = color2
		ElseIf MyBase.GetComponent(GetType(GUIText)) Then
			Me.colors = New Color(0, 2) {}
			Dim ptr3 As __ByRef(Of Color) = Me.colors(0, 0)
			Dim ptr4 As __ByRef(Of Color) = Me.colors(0, 1)
			Dim color3 As Color = MyBase.GetComponent(Of GUIText)().material.color
			Dim color2 As Color = color3
			ptr4 = color3
			ptr3 = color2
		ElseIf MyBase.GetComponent(Of Renderer)() Then
			Me.colors = New Color(MyBase.GetComponent(Of Renderer)().materials.Length - 1, 2) {}
			For i As Integer = 0 To MyBase.GetComponent(Of Renderer)().materials.Length - 1
				Me.colors(i, 0) = MyBase.GetComponent(Of Renderer)().materials(i).GetColor(Me.namedcolorvalue.ToString())
				Me.colors(i, 1) = MyBase.GetComponent(Of Renderer)().materials(i).GetColor(Me.namedcolorvalue.ToString())
			Next
		ElseIf MyBase.GetComponent(Of Light)() Then
			Me.colors = New Color(0, 2) {}
			Dim ptr5 As __ByRef(Of Color) = Me.colors(0, 0)
			Dim ptr6 As __ByRef(Of Color) = Me.colors(0, 1)
			Dim color4 As Color = MyBase.GetComponent(Of Light)().color
			Dim color2 As Color = color4
			ptr6 = color4
			ptr5 = color2
		Else
			Me.colors = New Color(0, 2) {}
		End If
		If Me.tweenArguments.Contains("color") Then
			For j As Integer = 0 To Me.colors.GetLength(0) - 1
				Me.colors(j, 1) = CType(Me.tweenArguments("color"), Color)
			Next
		Else
			If Me.tweenArguments.Contains("r") Then
				For k As Integer = 0 To Me.colors.GetLength(0) - 1
					Me.colors(k, 1).r = CSng(Me.tweenArguments("r"))
				Next
			End If
			If Me.tweenArguments.Contains("g") Then
				For l As Integer = 0 To Me.colors.GetLength(0) - 1
					Me.colors(l, 1).g = CSng(Me.tweenArguments("g"))
				Next
			End If
			If Me.tweenArguments.Contains("b") Then
				For m As Integer = 0 To Me.colors.GetLength(0) - 1
					Me.colors(m, 1).b = CSng(Me.tweenArguments("b"))
				Next
			End If
			If Me.tweenArguments.Contains("a") Then
				For n As Integer = 0 To Me.colors.GetLength(0) - 1
					Me.colors(n, 1).a = CSng(Me.tweenArguments("a"))
				Next
			End If
		End If
		If Me.tweenArguments.Contains("amount") Then
			For num As Integer = 0 To Me.colors.GetLength(0) - 1
				Me.colors(num, 1).a = CSng(Me.tweenArguments("amount"))
			Next
		ElseIf Me.tweenArguments.Contains("alpha") Then
			For num2 As Integer = 0 To Me.colors.GetLength(0) - 1
				Me.colors(num2, 1).a = CSng(Me.tweenArguments("alpha"))
			Next
		End If
	End Sub

	' Token: 0x0600471D RID: 18205 RVA: 0x00252C00 File Offset: 0x00251000
	Private Sub GenerateAudioToTargets()
		Me.vector2s = New Vector2(2) {}
		If Me.tweenArguments.Contains("audiosource") Then
			Me.audioSource = CType(Me.tweenArguments("audiosource"), AudioSource)
		ElseIf MyBase.GetComponent(GetType(AudioSource)) Then
			Me.audioSource = MyBase.GetComponent(Of AudioSource)()
		Else
			Global.Debug.LogError("iTween Error: AudioTo requires an AudioSource.", Nothing)
			Me.Dispose()
		End If
		Dim array As Vector2() = Me.vector2s
		Dim num As Integer = 0
		Dim array2 As Vector2() = Me.vector2s
		Dim num2 As Integer = 1
		Dim vector As Vector2 = New Vector2(Me.audioSource.volume, Me.audioSource.pitch)
		Dim vector2 As Vector2 = vector
		array2(num2) = vector
		array(num) = vector2
		If Me.tweenArguments.Contains("volume") Then
			Me.vector2s(1).x = CSng(Me.tweenArguments("volume"))
		End If
		If Me.tweenArguments.Contains("pitch") Then
			Me.vector2s(1).y = CSng(Me.tweenArguments("pitch"))
		End If
	End Sub

	' Token: 0x0600471E RID: 18206 RVA: 0x00252D40 File Offset: 0x00251140
	Private Sub GenerateStabTargets()
		If Me.tweenArguments.Contains("audiosource") Then
			Me.audioSource = CType(Me.tweenArguments("audiosource"), AudioSource)
		ElseIf MyBase.GetComponent(GetType(AudioSource)) Then
			Me.audioSource = MyBase.GetComponent(Of AudioSource)()
		Else
			MyBase.gameObject.AddComponent(GetType(AudioSource))
			Me.audioSource = MyBase.GetComponent(Of AudioSource)()
			Me.audioSource.playOnAwake = False
		End If
		Me.audioSource.clip = CType(Me.tweenArguments("audioclip"), AudioClip)
		If Me.tweenArguments.Contains("pitch") Then
			Me.audioSource.pitch = CSng(Me.tweenArguments("pitch"))
		End If
		If Me.tweenArguments.Contains("volume") Then
			Me.audioSource.volume = CSng(Me.tweenArguments("volume"))
		End If
		Me.time = Me.audioSource.clip.length / Me.audioSource.pitch
	End Sub

	' Token: 0x0600471F RID: 18207 RVA: 0x00252E88 File Offset: 0x00251288
	Private Sub GenerateLookToTargets()
		Me.vector3s = New Vector3(2) {}
		Me.vector3s(0) = MyBase.transform.eulerAngles
		If Me.tweenArguments.Contains("looktarget") Then
			If Me.tweenArguments("looktarget").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = MyBase.transform
				Dim transform2 As Transform = CType(Me.tweenArguments("looktarget"), Transform)
				Dim vector As Vector3? = CType(Me.tweenArguments("up"), Vector3?)
				transform.LookAt(transform2, If((vector Is Nothing), DialogueriTween.Defaults.up, vector.Value))
			ElseIf Me.tweenArguments("looktarget").[GetType]() Is GetType(Vector3) Then
				Dim transform3 As Transform = MyBase.transform
				Dim vector2 As Vector3 = CType(Me.tweenArguments("looktarget"), Vector3)
				Dim vector3 As Vector3? = CType(Me.tweenArguments("up"), Vector3?)
				transform3.LookAt(vector2, If((vector3 Is Nothing), DialogueriTween.Defaults.up, vector3.Value))
			End If
		Else
			Global.Debug.LogError("iTween Error: LookTo needs a 'looktarget' property!", Nothing)
			Me.Dispose()
		End If
		Me.vector3s(1) = MyBase.transform.eulerAngles
		MyBase.transform.eulerAngles = Me.vector3s(0)
		If Me.tweenArguments.Contains("axis") Then
			Dim text As String = CStr(Me.tweenArguments("axis"))
			If text IsNot Nothing Then
				If Not(text = "x") Then
					If Not(text = "y") Then
						If text = "z" Then
							Me.vector3s(1).x = Me.vector3s(0).x
							Me.vector3s(1).y = Me.vector3s(0).y
						End If
					Else
						Me.vector3s(1).x = Me.vector3s(0).x
						Me.vector3s(1).z = Me.vector3s(0).z
					End If
				Else
					Me.vector3s(1).y = Me.vector3s(0).y
					Me.vector3s(1).z = Me.vector3s(0).z
				End If
			End If
		End If
		Me.vector3s(1) = New Vector3(Me.clerp(Me.vector3s(0).x, Me.vector3s(1).x, 1F), Me.clerp(Me.vector3s(0).y, Me.vector3s(1).y, 1F), Me.clerp(Me.vector3s(0).z, Me.vector3s(1).z, 1F))
		If Me.tweenArguments.Contains("speed") Then
			Dim num As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004720 RID: 18208 RVA: 0x00253250 File Offset: 0x00251650
	Private Sub GenerateMoveToPathTargets()
		Dim array2 As Vector3()
		If Me.tweenArguments("path").[GetType]() Is GetType(Vector3()) Then
			Dim array As Vector3() = CType(Me.tweenArguments("path"), Vector3())
			If array.Length = 1 Then
				Global.Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!", Nothing)
				Me.Dispose()
			End If
			array2 = New Vector3(array.Length - 1) {}
			Array.Copy(array, array2, array.Length)
		Else
			Dim array3 As Transform() = CType(Me.tweenArguments("path"), Transform())
			If array3.Length = 1 Then
				Global.Debug.LogError("iTween Error: Attempting a path movement with MoveTo requires an array of more than 1 entry!", Nothing)
				Me.Dispose()
			End If
			array2 = New Vector3(array3.Length - 1) {}
			For i As Integer = 0 To array3.Length - 1
				array2(i) = array3(i).position
			Next
		End If
		Dim flag As Boolean
		Dim num As Integer
		If MyBase.transform.position <> array2(0) Then
			If Not Me.tweenArguments.Contains("movetopath") OrElse CBool(Me.tweenArguments("movetopath")) Then
				flag = True
				num = 3
			Else
				flag = False
				num = 2
			End If
		Else
			flag = False
			num = 2
		End If
		Me.vector3s = New Vector3(array2.Length + num - 1) {}
		If flag Then
			Me.vector3s(1) = MyBase.transform.position
			num = 2
		Else
			num = 1
		End If
		Array.Copy(array2, 0, Me.vector3s, num, array2.Length)
		Me.vector3s(0) = Me.vector3s(1) + (Me.vector3s(1) - Me.vector3s(2))
		Me.vector3s(Me.vector3s.Length - 1) = Me.vector3s(Me.vector3s.Length - 2) + (Me.vector3s(Me.vector3s.Length - 2) - Me.vector3s(Me.vector3s.Length - 3))
		If Me.vector3s(1) = Me.vector3s(Me.vector3s.Length - 2) Then
			Dim array4 As Vector3() = New Vector3(Me.vector3s.Length - 1) {}
			Array.Copy(Me.vector3s, array4, Me.vector3s.Length)
			array4(0) = array4(array4.Length - 3)
			array4(array4.Length - 1) = array4(2)
			Me.vector3s = New Vector3(array4.Length - 1) {}
			Array.Copy(array4, Me.vector3s, array4.Length)
		End If
		Me.path = New DialogueriTween.CRSpline(Me.vector3s)
		If Me.tweenArguments.Contains("speed") Then
			Dim num2 As Single = DialogueriTween.PathLength(Me.vector3s)
			Me.time = num2 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004721 RID: 18209 RVA: 0x002535B0 File Offset: 0x002519B0
	Private Sub GenerateMoveToTargets()
		Me.vector3s = New Vector3(2) {}
		If Me.isLocal Then
			Dim array As Vector3() = Me.vector3s
			Dim num As Integer = 0
			Dim array2 As Vector3() = Me.vector3s
			Dim num2 As Integer = 1
			Dim localPosition As Vector3 = MyBase.transform.localPosition
			Dim vector As Vector3 = localPosition
			array2(num2) = localPosition
			array(num) = vector
		Else
			Dim array3 As Vector3() = Me.vector3s
			Dim num3 As Integer = 0
			Dim array4 As Vector3() = Me.vector3s
			Dim num4 As Integer = 1
			Dim position As Vector3 = MyBase.transform.position
			Dim vector As Vector3 = position
			array4(num4) = position
			array3(num3) = vector
		End If
		If Me.tweenArguments.Contains("position") Then
			If Me.tweenArguments("position").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = CType(Me.tweenArguments("position"), Transform)
				Me.vector3s(1) = transform.position
			ElseIf Me.tweenArguments("position").[GetType]() Is GetType(Vector3) Then
				Me.vector3s(1) = CType(Me.tweenArguments("position"), Vector3)
			End If
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
		If Me.tweenArguments.Contains("orienttopath") AndAlso CBool(Me.tweenArguments("orienttopath")) Then
			Me.tweenArguments("looktarget") = Me.vector3s(1)
		End If
		If Me.tweenArguments.Contains("speed") Then
			Dim num5 As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num5 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004722 RID: 18210 RVA: 0x00253858 File Offset: 0x00251C58
	Private Sub GenerateMoveByTargets()
		Me.vector3s = New Vector3(5) {}
		Me.vector3s(4) = MyBase.transform.eulerAngles
		Dim array As Vector3() = Me.vector3s
		Dim num As Integer = 0
		Dim array2 As Vector3() = Me.vector3s
		Dim num2 As Integer = 1
		Dim array3 As Vector3() = Me.vector3s
		Dim num3 As Integer = 3
		Dim position As Vector3 = MyBase.transform.position
		Dim vector As Vector3 = position
		array3(num3) = position
		Dim vector2 As Vector3 = vector
		vector = vector2
		array2(num2) = vector2
		array(num) = vector
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) = Me.vector3s(0) + CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = Me.vector3s(0).x + CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = Me.vector3s(0).y + CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = Me.vector3s(0).z + CSng(Me.tweenArguments("z"))
			End If
		End If
		MyBase.transform.Translate(Me.vector3s(1), Me.space)
		Me.vector3s(5) = MyBase.transform.position
		MyBase.transform.position = Me.vector3s(0)
		If Me.tweenArguments.Contains("orienttopath") AndAlso CBool(Me.tweenArguments("orienttopath")) Then
			Me.tweenArguments("looktarget") = Me.vector3s(1)
		End If
		If Me.tweenArguments.Contains("speed") Then
			Dim num4 As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num4 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004723 RID: 18211 RVA: 0x00253B1C File Offset: 0x00251F1C
	Private Sub GenerateScaleToTargets()
		Me.vector3s = New Vector3(2) {}
		Dim array As Vector3() = Me.vector3s
		Dim num As Integer = 0
		Dim array2 As Vector3() = Me.vector3s
		Dim num2 As Integer = 1
		Dim localScale As Vector3 = MyBase.transform.localScale
		Dim vector As Vector3 = localScale
		array2(num2) = localScale
		array(num) = vector
		If Me.tweenArguments.Contains("scale") Then
			If Me.tweenArguments("scale").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = CType(Me.tweenArguments("scale"), Transform)
				Me.vector3s(1) = transform.localScale
			ElseIf Me.tweenArguments("scale").[GetType]() Is GetType(Vector3) Then
				Me.vector3s(1) = CType(Me.tweenArguments("scale"), Vector3)
			End If
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
		If Me.tweenArguments.Contains("speed") Then
			Dim num3 As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num3 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004724 RID: 18212 RVA: 0x00253D30 File Offset: 0x00252130
	Private Sub GenerateScaleByTargets()
		Me.vector3s = New Vector3(2) {}
		Dim array As Vector3() = Me.vector3s
		Dim num As Integer = 0
		Dim array2 As Vector3() = Me.vector3s
		Dim num2 As Integer = 1
		Dim localScale As Vector3 = MyBase.transform.localScale
		Dim vector As Vector3 = localScale
		array2(num2) = localScale
		array(num) = vector
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) = Vector3.Scale(Me.vector3s(1), CType(Me.tweenArguments("amount"), Vector3))
		Else
			If Me.tweenArguments.Contains("x") Then
				Dim array3 As Vector3() = Me.vector3s
				Dim num3 As Integer = 1
				array3(num3).x = array3(num3).x * CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Dim array4 As Vector3() = Me.vector3s
				Dim num4 As Integer = 1
				array4(num4).y = array4(num4).y * CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Dim array5 As Vector3() = Me.vector3s
				Dim num5 As Integer = 1
				array5(num5).z = array5(num5).z * CSng(Me.tweenArguments("z"))
			End If
		End If
		If Me.tweenArguments.Contains("speed") Then
			Dim num6 As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num6 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004725 RID: 18213 RVA: 0x00253EF4 File Offset: 0x002522F4
	Private Sub GenerateScaleAddTargets()
		Me.vector3s = New Vector3(2) {}
		Dim array As Vector3() = Me.vector3s
		Dim num As Integer = 0
		Dim array2 As Vector3() = Me.vector3s
		Dim num2 As Integer = 1
		Dim localScale As Vector3 = MyBase.transform.localScale
		Dim vector As Vector3 = localScale
		array2(num2) = localScale
		array(num) = vector
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) += CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Dim array3 As Vector3() = Me.vector3s
				Dim num3 As Integer = 1
				array3(num3).x = array3(num3).x + CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Dim array4 As Vector3() = Me.vector3s
				Dim num4 As Integer = 1
				array4(num4).y = array4(num4).y + CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Dim array5 As Vector3() = Me.vector3s
				Dim num5 As Integer = 1
				array5(num5).z = array5(num5).z + CSng(Me.tweenArguments("z"))
			End If
		End If
		If Me.tweenArguments.Contains("speed") Then
			Dim num6 As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num6 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004726 RID: 18214 RVA: 0x002540B0 File Offset: 0x002524B0
	Private Sub GenerateRotateToTargets()
		Me.vector3s = New Vector3(2) {}
		If Me.isLocal Then
			Dim array As Vector3() = Me.vector3s
			Dim num As Integer = 0
			Dim array2 As Vector3() = Me.vector3s
			Dim num2 As Integer = 1
			Dim localEulerAngles As Vector3 = MyBase.transform.localEulerAngles
			Dim vector As Vector3 = localEulerAngles
			array2(num2) = localEulerAngles
			array(num) = vector
		Else
			Dim array3 As Vector3() = Me.vector3s
			Dim num3 As Integer = 0
			Dim array4 As Vector3() = Me.vector3s
			Dim num4 As Integer = 1
			Dim eulerAngles As Vector3 = MyBase.transform.eulerAngles
			Dim vector As Vector3 = eulerAngles
			array4(num4) = eulerAngles
			array3(num3) = vector
		End If
		If Me.tweenArguments.Contains("rotation") Then
			If Me.tweenArguments("rotation").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = CType(Me.tweenArguments("rotation"), Transform)
				Me.vector3s(1) = transform.eulerAngles
			ElseIf Me.tweenArguments("rotation").[GetType]() Is GetType(Vector3) Then
				Me.vector3s(1) = CType(Me.tweenArguments("rotation"), Vector3)
			End If
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
		Me.vector3s(1) = New Vector3(Me.clerp(Me.vector3s(0).x, Me.vector3s(1).x, 1F), Me.clerp(Me.vector3s(0).y, Me.vector3s(1).y, 1F), Me.clerp(Me.vector3s(0).z, Me.vector3s(1).z, 1F))
		If Me.tweenArguments.Contains("speed") Then
			Dim num5 As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num5 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004727 RID: 18215 RVA: 0x002543A0 File Offset: 0x002527A0
	Private Sub GenerateRotateAddTargets()
		Me.vector3s = New Vector3(4) {}
		Dim array As Vector3() = Me.vector3s
		Dim num As Integer = 0
		Dim array2 As Vector3() = Me.vector3s
		Dim num2 As Integer = 1
		Dim array3 As Vector3() = Me.vector3s
		Dim num3 As Integer = 3
		Dim eulerAngles As Vector3 = MyBase.transform.eulerAngles
		Dim vector As Vector3 = eulerAngles
		array3(num3) = eulerAngles
		Dim vector2 As Vector3 = vector
		vector = vector2
		array2(num2) = vector2
		array(num) = vector
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) += CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Dim array4 As Vector3() = Me.vector3s
				Dim num4 As Integer = 1
				array4(num4).x = array4(num4).x + CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Dim array5 As Vector3() = Me.vector3s
				Dim num5 As Integer = 1
				array5(num5).y = array5(num5).y + CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Dim array6 As Vector3() = Me.vector3s
				Dim num6 As Integer = 1
				array6(num6).z = array6(num6).z + CSng(Me.tweenArguments("z"))
			End If
		End If
		If Me.tweenArguments.Contains("speed") Then
			Dim num7 As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num7 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004728 RID: 18216 RVA: 0x00254570 File Offset: 0x00252970
	Private Sub GenerateRotateByTargets()
		Me.vector3s = New Vector3(3) {}
		Dim array As Vector3() = Me.vector3s
		Dim num As Integer = 0
		Dim array2 As Vector3() = Me.vector3s
		Dim num2 As Integer = 1
		Dim array3 As Vector3() = Me.vector3s
		Dim num3 As Integer = 3
		Dim eulerAngles As Vector3 = MyBase.transform.eulerAngles
		Dim vector As Vector3 = eulerAngles
		array3(num3) = eulerAngles
		Dim vector2 As Vector3 = vector
		vector = vector2
		array2(num2) = vector2
		array(num) = vector
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) += Vector3.Scale(CType(Me.tweenArguments("amount"), Vector3), New Vector3(360F, 360F, 360F))
		Else
			If Me.tweenArguments.Contains("x") Then
				Dim array4 As Vector3() = Me.vector3s
				Dim num4 As Integer = 1
				array4(num4).x = array4(num4).x + 360F * CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Dim array5 As Vector3() = Me.vector3s
				Dim num5 As Integer = 1
				array5(num5).y = array5(num5).y + 360F * CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Dim array6 As Vector3() = Me.vector3s
				Dim num6 As Integer = 1
				array6(num6).z = array6(num6).z + 360F * CSng(Me.tweenArguments("z"))
			End If
		End If
		If Me.tweenArguments.Contains("speed") Then
			Dim num7 As Single = Math.Abs(Vector3.Distance(Me.vector3s(0), Me.vector3s(1)))
			Me.time = num7 / CSng(Me.tweenArguments("speed"))
		End If
	End Sub

	' Token: 0x06004729 RID: 18217 RVA: 0x00254768 File Offset: 0x00252B68
	Private Sub GenerateShakePositionTargets()
		Me.vector3s = New Vector3(3) {}
		Me.vector3s(3) = MyBase.transform.eulerAngles
		Me.vector3s(0) = MyBase.transform.position
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) = CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
	End Sub

	' Token: 0x0600472A RID: 18218 RVA: 0x002548AC File Offset: 0x00252CAC
	Private Sub GenerateShakeScaleTargets()
		Me.vector3s = New Vector3(2) {}
		Me.vector3s(0) = MyBase.transform.localScale
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) = CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
	End Sub

	' Token: 0x0600472B RID: 18219 RVA: 0x002549D4 File Offset: 0x00252DD4
	Private Sub GenerateShakeRotationTargets()
		Me.vector3s = New Vector3(2) {}
		Me.vector3s(0) = MyBase.transform.eulerAngles
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) = CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
	End Sub

	' Token: 0x0600472C RID: 18220 RVA: 0x00254AFC File Offset: 0x00252EFC
	Private Sub GeneratePunchPositionTargets()
		Me.vector3s = New Vector3(4) {}
		Me.vector3s(4) = MyBase.transform.eulerAngles
		Me.vector3s(0) = MyBase.transform.position
		Dim array As Vector3() = Me.vector3s
		Dim num As Integer = 1
		Dim array2 As Vector3() = Me.vector3s
		Dim num2 As Integer = 3
		Dim zero As Vector3 = Vector3.zero
		Dim vector As Vector3 = zero
		array2(num2) = zero
		array(num) = vector
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) = CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
	End Sub

	' Token: 0x0600472D RID: 18221 RVA: 0x00254C68 File Offset: 0x00253068
	Private Sub GeneratePunchRotationTargets()
		Me.vector3s = New Vector3(3) {}
		Me.vector3s(0) = MyBase.transform.eulerAngles
		Dim array As Vector3() = Me.vector3s
		Dim num As Integer = 1
		Dim array2 As Vector3() = Me.vector3s
		Dim num2 As Integer = 3
		Dim zero As Vector3 = Vector3.zero
		Dim vector As Vector3 = zero
		array2(num2) = zero
		array(num) = vector
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) = CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
	End Sub

	' Token: 0x0600472E RID: 18222 RVA: 0x00254DB8 File Offset: 0x002531B8
	Private Sub GeneratePunchScaleTargets()
		Me.vector3s = New Vector3(2) {}
		Me.vector3s(0) = MyBase.transform.localScale
		Me.vector3s(1) = Vector3.zero
		If Me.tweenArguments.Contains("amount") Then
			Me.vector3s(1) = CType(Me.tweenArguments("amount"), Vector3)
		Else
			If Me.tweenArguments.Contains("x") Then
				Me.vector3s(1).x = CSng(Me.tweenArguments("x"))
			End If
			If Me.tweenArguments.Contains("y") Then
				Me.vector3s(1).y = CSng(Me.tweenArguments("y"))
			End If
			If Me.tweenArguments.Contains("z") Then
				Me.vector3s(1).z = CSng(Me.tweenArguments("z"))
			End If
		End If
	End Sub

	' Token: 0x0600472F RID: 18223 RVA: 0x00254EF4 File Offset: 0x002532F4
	Private Sub ApplyRectTargets()
		Me.rects(2).x = Me.ease(Me.rects(0).x, Me.rects(1).x, Me.percentage)
		Me.rects(2).y = Me.ease(Me.rects(0).y, Me.rects(1).y, Me.percentage)
		Me.rects(2).width = Me.ease(Me.rects(0).width, Me.rects(1).width, Me.percentage)
		Me.rects(2).height = Me.ease(Me.rects(0).height, Me.rects(1).height, Me.percentage)
		Me.tweenArguments("onupdateparams") = Me.rects(2)
		If Me.percentage = 1F Then
			Me.tweenArguments("onupdateparams") = Me.rects(1)
		End If
	End Sub

	' Token: 0x06004730 RID: 18224 RVA: 0x00255070 File Offset: 0x00253470
	Private Sub ApplyColorTargets()
		Me.colors(0, 2).r = Me.ease(Me.colors(0, 0).r, Me.colors(0, 1).r, Me.percentage)
		Me.colors(0, 2).g = Me.ease(Me.colors(0, 0).g, Me.colors(0, 1).g, Me.percentage)
		Me.colors(0, 2).b = Me.ease(Me.colors(0, 0).b, Me.colors(0, 1).b, Me.percentage)
		Me.colors(0, 2).a = Me.ease(Me.colors(0, 0).a, Me.colors(0, 1).a, Me.percentage)
		Me.tweenArguments("onupdateparams") = Me.colors(0, 2)
		If Me.percentage = 1F Then
			Me.tweenArguments("onupdateparams") = Me.colors(0, 1)
		End If
	End Sub

	' Token: 0x06004731 RID: 18225 RVA: 0x002551F0 File Offset: 0x002535F0
	Private Sub ApplyVector3Targets()
		Me.vector3s(2).x = Me.ease(Me.vector3s(0).x, Me.vector3s(1).x, Me.percentage)
		Me.vector3s(2).y = Me.ease(Me.vector3s(0).y, Me.vector3s(1).y, Me.percentage)
		Me.vector3s(2).z = Me.ease(Me.vector3s(0).z, Me.vector3s(1).z, Me.percentage)
		Me.tweenArguments("onupdateparams") = Me.vector3s(2)
		If Me.percentage = 1F Then
			Me.tweenArguments("onupdateparams") = Me.vector3s(1)
		End If
	End Sub

	' Token: 0x06004732 RID: 18226 RVA: 0x00255328 File Offset: 0x00253728
	Private Sub ApplyVector2Targets()
		Me.vector2s(2).x = Me.ease(Me.vector2s(0).x, Me.vector2s(1).x, Me.percentage)
		Me.vector2s(2).y = Me.ease(Me.vector2s(0).y, Me.vector2s(1).y, Me.percentage)
		Me.tweenArguments("onupdateparams") = Me.vector2s(2)
		If Me.percentage = 1F Then
			Me.tweenArguments("onupdateparams") = Me.vector2s(1)
		End If
	End Sub

	' Token: 0x06004733 RID: 18227 RVA: 0x0025541C File Offset: 0x0025381C
	Private Sub ApplyFloatTargets()
		Me.floats(2) = Me.ease(Me.floats(0), Me.floats(1), Me.percentage)
		Me.tweenArguments("onupdateparams") = Me.floats(2)
		If Me.percentage = 1F Then
			Me.tweenArguments("onupdateparams") = Me.floats(1)
		End If
	End Sub

	' Token: 0x06004734 RID: 18228 RVA: 0x0025549C File Offset: 0x0025389C
	Private Sub ApplyColorToTargets()
		For i As Integer = 0 To Me.colors.GetLength(0) - 1
			Me.colors(i, 2).r = Me.ease(Me.colors(i, 0).r, Me.colors(i, 1).r, Me.percentage)
			Me.colors(i, 2).g = Me.ease(Me.colors(i, 0).g, Me.colors(i, 1).g, Me.percentage)
			Me.colors(i, 2).b = Me.ease(Me.colors(i, 0).b, Me.colors(i, 1).b, Me.percentage)
			Me.colors(i, 2).a = Me.ease(Me.colors(i, 0).a, Me.colors(i, 1).a, Me.percentage)
		Next
		If MyBase.GetComponent(GetType(GUITexture)) Then
			MyBase.GetComponent(Of GUITexture)().color = Me.colors(0, 2)
		ElseIf MyBase.GetComponent(GetType(GUIText)) Then
			MyBase.GetComponent(Of GUIText)().material.color = Me.colors(0, 2)
		ElseIf MyBase.GetComponent(Of Renderer)() Then
			For j As Integer = 0 To Me.colors.GetLength(0) - 1
				MyBase.GetComponent(Of Renderer)().materials(j).SetColor(Me.namedcolorvalue.ToString(), Me.colors(j, 2))
			Next
		ElseIf MyBase.GetComponent(Of Light)() Then
			MyBase.GetComponent(Of Light)().color = Me.colors(0, 2)
		End If
		If Me.percentage = 1F Then
			If MyBase.GetComponent(GetType(GUITexture)) Then
				MyBase.GetComponent(Of GUITexture)().color = Me.colors(0, 1)
			ElseIf MyBase.GetComponent(GetType(GUIText)) Then
				MyBase.GetComponent(Of GUIText)().material.color = Me.colors(0, 1)
			ElseIf MyBase.GetComponent(Of Renderer)() Then
				For k As Integer = 0 To Me.colors.GetLength(0) - 1
					MyBase.GetComponent(Of Renderer)().materials(k).SetColor(Me.namedcolorvalue.ToString(), Me.colors(k, 1))
				Next
			ElseIf MyBase.GetComponent(Of Light)() Then
				MyBase.GetComponent(Of Light)().color = Me.colors(0, 1)
			End If
		End If
	End Sub

	' Token: 0x06004735 RID: 18229 RVA: 0x002557EC File Offset: 0x00253BEC
	Private Sub ApplyAudioToTargets()
		Me.vector2s(2).x = Me.ease(Me.vector2s(0).x, Me.vector2s(1).x, Me.percentage)
		Me.vector2s(2).y = Me.ease(Me.vector2s(0).y, Me.vector2s(1).y, Me.percentage)
		Me.audioSource.volume = Me.vector2s(2).x
		Me.audioSource.pitch = Me.vector2s(2).y
		If Me.percentage = 1F Then
			Me.audioSource.volume = Me.vector2s(1).x
			Me.audioSource.pitch = Me.vector2s(1).y
		End If
	End Sub

	' Token: 0x06004736 RID: 18230 RVA: 0x00255901 File Offset: 0x00253D01
	Private Sub ApplyStabTargets()
	End Sub

	' Token: 0x06004737 RID: 18231 RVA: 0x00255904 File Offset: 0x00253D04
	Private Sub ApplyMoveToPathTargets()
		Me.preUpdate = MyBase.transform.position
		Dim num As Single = Me.ease(0F, 1F, Me.percentage)
		If Me.isLocal Then
			MyBase.transform.localPosition = Me.path.Interp(Mathf.Clamp(num, 0F, 1F))
		Else
			MyBase.transform.position = Me.path.Interp(Mathf.Clamp(num, 0F, 1F))
		End If
		If Me.tweenArguments.Contains("orienttopath") AndAlso CBool(Me.tweenArguments("orienttopath")) Then
			Dim num2 As Single
			If Me.tweenArguments.Contains("lookahead") Then
				num2 = CSng(Me.tweenArguments("lookahead"))
			Else
				num2 = DialogueriTween.Defaults.lookAhead
			End If
			Dim num3 As Single = Me.ease(0F, 1F, Mathf.Min(1F, Me.percentage + num2))
			Me.tweenArguments("looktarget") = Me.path.Interp(Mathf.Clamp(num3, 0F, 1F))
		End If
		Me.postUpdate = MyBase.transform.position
		If Me.physics Then
			MyBase.transform.position = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MovePosition(Me.postUpdate)
		End If
	End Sub

	' Token: 0x06004738 RID: 18232 RVA: 0x00255A98 File Offset: 0x00253E98
	Private Sub ApplyMoveToTargets()
		Me.preUpdate = MyBase.transform.position
		Me.vector3s(2).x = Me.ease(Me.vector3s(0).x, Me.vector3s(1).x, Me.percentage)
		Me.vector3s(2).y = Me.ease(Me.vector3s(0).y, Me.vector3s(1).y, Me.percentage)
		Me.vector3s(2).z = Me.ease(Me.vector3s(0).z, Me.vector3s(1).z, Me.percentage)
		If Me.isLocal Then
			MyBase.transform.localPosition = Me.vector3s(2)
		Else
			MyBase.transform.position = Me.vector3s(2)
		End If
		If Me.percentage = 1F Then
			If Me.isLocal Then
				MyBase.transform.localPosition = Me.vector3s(1)
			Else
				MyBase.transform.position = Me.vector3s(1)
			End If
		End If
		Me.postUpdate = MyBase.transform.position
		If Me.physics Then
			MyBase.transform.position = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MovePosition(Me.postUpdate)
		End If
	End Sub

	' Token: 0x06004739 RID: 18233 RVA: 0x00255C60 File Offset: 0x00254060
	Private Sub ApplyMoveByTargets()
		Me.preUpdate = MyBase.transform.position
		Dim vector As Vector3 = Nothing
		If Me.tweenArguments.Contains("looktarget") Then
			vector = MyBase.transform.eulerAngles
			MyBase.transform.eulerAngles = Me.vector3s(4)
		End If
		Me.vector3s(2).x = Me.ease(Me.vector3s(0).x, Me.vector3s(1).x, Me.percentage)
		Me.vector3s(2).y = Me.ease(Me.vector3s(0).y, Me.vector3s(1).y, Me.percentage)
		Me.vector3s(2).z = Me.ease(Me.vector3s(0).z, Me.vector3s(1).z, Me.percentage)
		MyBase.transform.Translate(Me.vector3s(2) - Me.vector3s(3), Me.space)
		Me.vector3s(3) = Me.vector3s(2)
		If Me.tweenArguments.Contains("looktarget") Then
			MyBase.transform.eulerAngles = vector
		End If
		Me.postUpdate = MyBase.transform.position
		If Me.physics Then
			MyBase.transform.position = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MovePosition(Me.postUpdate)
		End If
	End Sub

	' Token: 0x0600473A RID: 18234 RVA: 0x00255E48 File Offset: 0x00254248
	Private Sub ApplyScaleToTargets()
		Me.vector3s(2).x = Me.ease(Me.vector3s(0).x, Me.vector3s(1).x, Me.percentage)
		Me.vector3s(2).y = Me.ease(Me.vector3s(0).y, Me.vector3s(1).y, Me.percentage)
		Me.vector3s(2).z = Me.ease(Me.vector3s(0).z, Me.vector3s(1).z, Me.percentage)
		MyBase.transform.localScale = Me.vector3s(2)
		If Me.percentage = 1F Then
			MyBase.transform.localScale = Me.vector3s(1)
		End If
	End Sub

	' Token: 0x0600473B RID: 18235 RVA: 0x00255F6C File Offset: 0x0025436C
	Private Sub ApplyLookToTargets()
		Me.vector3s(2).x = Me.ease(Me.vector3s(0).x, Me.vector3s(1).x, Me.percentage)
		Me.vector3s(2).y = Me.ease(Me.vector3s(0).y, Me.vector3s(1).y, Me.percentage)
		Me.vector3s(2).z = Me.ease(Me.vector3s(0).z, Me.vector3s(1).z, Me.percentage)
		If Me.isLocal Then
			MyBase.transform.localRotation = Quaternion.Euler(Me.vector3s(2))
		Else
			MyBase.transform.rotation = Quaternion.Euler(Me.vector3s(2))
		End If
	End Sub

	' Token: 0x0600473C RID: 18236 RVA: 0x00256098 File Offset: 0x00254498
	Private Sub ApplyRotateToTargets()
		Me.preUpdate = MyBase.transform.eulerAngles
		Me.vector3s(2).x = Me.ease(Me.vector3s(0).x, Me.vector3s(1).x, Me.percentage)
		Me.vector3s(2).y = Me.ease(Me.vector3s(0).y, Me.vector3s(1).y, Me.percentage)
		Me.vector3s(2).z = Me.ease(Me.vector3s(0).z, Me.vector3s(1).z, Me.percentage)
		If Me.isLocal Then
			MyBase.transform.localRotation = Quaternion.Euler(Me.vector3s(2))
		Else
			MyBase.transform.rotation = Quaternion.Euler(Me.vector3s(2))
		End If
		If Me.percentage = 1F Then
			If Me.isLocal Then
				MyBase.transform.localRotation = Quaternion.Euler(Me.vector3s(1))
			Else
				MyBase.transform.rotation = Quaternion.Euler(Me.vector3s(1))
			End If
		End If
		Me.postUpdate = MyBase.transform.eulerAngles
		If Me.physics Then
			MyBase.transform.eulerAngles = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MoveRotation(Quaternion.Euler(Me.postUpdate))
		End If
	End Sub

	' Token: 0x0600473D RID: 18237 RVA: 0x0025627C File Offset: 0x0025467C
	Private Sub ApplyRotateAddTargets()
		Me.preUpdate = MyBase.transform.eulerAngles
		Me.vector3s(2).x = Me.ease(Me.vector3s(0).x, Me.vector3s(1).x, Me.percentage)
		Me.vector3s(2).y = Me.ease(Me.vector3s(0).y, Me.vector3s(1).y, Me.percentage)
		Me.vector3s(2).z = Me.ease(Me.vector3s(0).z, Me.vector3s(1).z, Me.percentage)
		MyBase.transform.Rotate(Me.vector3s(2) - Me.vector3s(3), Me.space)
		Me.vector3s(3) = Me.vector3s(2)
		Me.postUpdate = MyBase.transform.eulerAngles
		If Me.physics Then
			MyBase.transform.eulerAngles = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MoveRotation(Quaternion.Euler(Me.postUpdate))
		End If
	End Sub

	' Token: 0x0600473E RID: 18238 RVA: 0x00256404 File Offset: 0x00254804
	Private Sub ApplyShakePositionTargets()
		If Me.isLocal Then
			Me.preUpdate = MyBase.transform.localPosition
		Else
			Me.preUpdate = MyBase.transform.position
		End If
		Dim vector As Vector3 = Nothing
		If Me.tweenArguments.Contains("looktarget") Then
			vector = MyBase.transform.eulerAngles
			MyBase.transform.eulerAngles = Me.vector3s(3)
		End If
		If Me.percentage = 0F Then
			MyBase.transform.Translate(Me.vector3s(1), Me.space)
		End If
		If Me.isLocal Then
			MyBase.transform.localPosition = Me.vector3s(0)
		Else
			MyBase.transform.position = Me.vector3s(0)
		End If
		Dim num As Single = 1F - Me.percentage
		Me.vector3s(2).x = Global.UnityEngine.Random.Range(-Me.vector3s(1).x * num, Me.vector3s(1).x * num)
		Me.vector3s(2).y = Global.UnityEngine.Random.Range(-Me.vector3s(1).y * num, Me.vector3s(1).y * num)
		Me.vector3s(2).z = Global.UnityEngine.Random.Range(-Me.vector3s(1).z * num, Me.vector3s(1).z * num)
		If Me.isLocal Then
			MyBase.transform.localPosition += Me.vector3s(2)
		Else
			MyBase.transform.position += Me.vector3s(2)
		End If
		If Me.tweenArguments.Contains("looktarget") Then
			MyBase.transform.eulerAngles = vector
		End If
		Me.postUpdate = MyBase.transform.position
		If Me.physics Then
			MyBase.transform.position = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MovePosition(Me.postUpdate)
		End If
	End Sub

	' Token: 0x0600473F RID: 18239 RVA: 0x00256684 File Offset: 0x00254A84
	Private Sub ApplyShakeScaleTargets()
		If Me.percentage = 0F Then
			MyBase.transform.localScale = Me.vector3s(1)
		End If
		MyBase.transform.localScale = Me.vector3s(0)
		Dim num As Single = 1F - Me.percentage
		Me.vector3s(2).x = Global.UnityEngine.Random.Range(-Me.vector3s(1).x * num, Me.vector3s(1).x * num)
		Me.vector3s(2).y = Global.UnityEngine.Random.Range(-Me.vector3s(1).y * num, Me.vector3s(1).y * num)
		Me.vector3s(2).z = Global.UnityEngine.Random.Range(-Me.vector3s(1).z * num, Me.vector3s(1).z * num)
		MyBase.transform.localScale += Me.vector3s(2)
	End Sub

	' Token: 0x06004740 RID: 18240 RVA: 0x002567C4 File Offset: 0x00254BC4
	Private Sub ApplyShakeRotationTargets()
		Me.preUpdate = MyBase.transform.eulerAngles
		If Me.percentage = 0F Then
			MyBase.transform.Rotate(Me.vector3s(1), Me.space)
		End If
		MyBase.transform.eulerAngles = Me.vector3s(0)
		Dim num As Single = 1F - Me.percentage
		Me.vector3s(2).x = Global.UnityEngine.Random.Range(-Me.vector3s(1).x * num, Me.vector3s(1).x * num)
		Me.vector3s(2).y = Global.UnityEngine.Random.Range(-Me.vector3s(1).y * num, Me.vector3s(1).y * num)
		Me.vector3s(2).z = Global.UnityEngine.Random.Range(-Me.vector3s(1).z * num, Me.vector3s(1).z * num)
		MyBase.transform.Rotate(Me.vector3s(2), Me.space)
		Me.postUpdate = MyBase.transform.eulerAngles
		If Me.physics Then
			MyBase.transform.eulerAngles = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MoveRotation(Quaternion.Euler(Me.postUpdate))
		End If
	End Sub

	' Token: 0x06004741 RID: 18241 RVA: 0x0025695C File Offset: 0x00254D5C
	Private Sub ApplyPunchPositionTargets()
		Me.preUpdate = MyBase.transform.position
		Dim vector As Vector3 = Nothing
		If Me.tweenArguments.Contains("looktarget") Then
			vector = MyBase.transform.eulerAngles
			MyBase.transform.eulerAngles = Me.vector3s(4)
		End If
		If Me.vector3s(1).x > 0F Then
			Me.vector3s(2).x = Me.punch(Me.vector3s(1).x, Me.percentage)
		ElseIf Me.vector3s(1).x < 0F Then
			Me.vector3s(2).x = -Me.punch(Mathf.Abs(Me.vector3s(1).x), Me.percentage)
		End If
		If Me.vector3s(1).y > 0F Then
			Me.vector3s(2).y = Me.punch(Me.vector3s(1).y, Me.percentage)
		ElseIf Me.vector3s(1).y < 0F Then
			Me.vector3s(2).y = -Me.punch(Mathf.Abs(Me.vector3s(1).y), Me.percentage)
		End If
		If Me.vector3s(1).z > 0F Then
			Me.vector3s(2).z = Me.punch(Me.vector3s(1).z, Me.percentage)
		ElseIf Me.vector3s(1).z < 0F Then
			Me.vector3s(2).z = -Me.punch(Mathf.Abs(Me.vector3s(1).z), Me.percentage)
		End If
		MyBase.transform.Translate(Me.vector3s(2) - Me.vector3s(3), Me.space)
		Me.vector3s(3) = Me.vector3s(2)
		If Me.tweenArguments.Contains("looktarget") Then
			MyBase.transform.eulerAngles = vector
		End If
		Me.postUpdate = MyBase.transform.position
		If Me.physics Then
			MyBase.transform.position = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MovePosition(Me.postUpdate)
		End If
	End Sub

	' Token: 0x06004742 RID: 18242 RVA: 0x00256C50 File Offset: 0x00255050
	Private Sub ApplyPunchRotationTargets()
		Me.preUpdate = MyBase.transform.eulerAngles
		If Me.vector3s(1).x > 0F Then
			Me.vector3s(2).x = Me.punch(Me.vector3s(1).x, Me.percentage)
		ElseIf Me.vector3s(1).x < 0F Then
			Me.vector3s(2).x = -Me.punch(Mathf.Abs(Me.vector3s(1).x), Me.percentage)
		End If
		If Me.vector3s(1).y > 0F Then
			Me.vector3s(2).y = Me.punch(Me.vector3s(1).y, Me.percentage)
		ElseIf Me.vector3s(1).y < 0F Then
			Me.vector3s(2).y = -Me.punch(Mathf.Abs(Me.vector3s(1).y), Me.percentage)
		End If
		If Me.vector3s(1).z > 0F Then
			Me.vector3s(2).z = Me.punch(Me.vector3s(1).z, Me.percentage)
		ElseIf Me.vector3s(1).z < 0F Then
			Me.vector3s(2).z = -Me.punch(Mathf.Abs(Me.vector3s(1).z), Me.percentage)
		End If
		MyBase.transform.Rotate(Me.vector3s(2) - Me.vector3s(3), Me.space)
		Me.vector3s(3) = Me.vector3s(2)
		Me.postUpdate = MyBase.transform.eulerAngles
		If Me.physics Then
			MyBase.transform.eulerAngles = Me.preUpdate
			MyBase.GetComponent(Of Rigidbody)().MoveRotation(Quaternion.Euler(Me.postUpdate))
		End If
	End Sub

	' Token: 0x06004743 RID: 18243 RVA: 0x00256EE4 File Offset: 0x002552E4
	Private Sub ApplyPunchScaleTargets()
		If Me.vector3s(1).x > 0F Then
			Me.vector3s(2).x = Me.punch(Me.vector3s(1).x, Me.percentage)
		ElseIf Me.vector3s(1).x < 0F Then
			Me.vector3s(2).x = -Me.punch(Mathf.Abs(Me.vector3s(1).x), Me.percentage)
		End If
		If Me.vector3s(1).y > 0F Then
			Me.vector3s(2).y = Me.punch(Me.vector3s(1).y, Me.percentage)
		ElseIf Me.vector3s(1).y < 0F Then
			Me.vector3s(2).y = -Me.punch(Mathf.Abs(Me.vector3s(1).y), Me.percentage)
		End If
		If Me.vector3s(1).z > 0F Then
			Me.vector3s(2).z = Me.punch(Me.vector3s(1).z, Me.percentage)
		ElseIf Me.vector3s(1).z < 0F Then
			Me.vector3s(2).z = -Me.punch(Mathf.Abs(Me.vector3s(1).z), Me.percentage)
		End If
		MyBase.transform.localScale = Me.vector3s(0) + Me.vector3s(2)
	End Sub

	' Token: 0x06004744 RID: 18244 RVA: 0x002570FC File Offset: 0x002554FC
	Private Iterator Function TweenDelay() As IEnumerator
		Me.delayStarted = Time.time
		Yield New WaitForSeconds(Me.delay)
		If Me.wasPaused Then
			Me.wasPaused = False
			Me.TweenStart()
		End If
		Return
	End Function

	' Token: 0x06004745 RID: 18245 RVA: 0x00257118 File Offset: 0x00255518
	Private Sub TweenStart()
		Me.CallBack("onstart")
		If Not Me.[loop] Then
			Me.ConflictCheck()
			Me.GenerateTargets()
		End If
		If Me.type = "stab" Then
			Me.audioSource.PlayOneShot(Me.audioSource.clip)
		End If
		If Me.type = "move" OrElse Me.type = "scale" OrElse Me.type = "rotate" OrElse Me.type = "punch" OrElse Me.type = "shake" OrElse Me.type = "curve" OrElse Me.type = "look" Then
			Me.EnableKinematic()
		End If
		Me.isRunning = True
	End Sub

	' Token: 0x06004746 RID: 18246 RVA: 0x00257214 File Offset: 0x00255614
	Private Iterator Function TweenRestart() As IEnumerator
		If Me.delay > 0F Then
			Me.delayStarted = Time.time
			Yield New WaitForSeconds(Me.delay)
		End If
		Me.[loop] = True
		Me.TweenStart()
		Return
	End Function

	' Token: 0x06004747 RID: 18247 RVA: 0x0025722F File Offset: 0x0025562F
	Private Sub TweenUpdate()
		Me.apply()
		Me.CallBack("onupdate")
		Me.UpdatePercentage()
	End Sub

	' Token: 0x06004748 RID: 18248 RVA: 0x00257250 File Offset: 0x00255650
	Private Sub TweenComplete()
		Me.isRunning = False
		If Me.percentage > 0.5F Then
			Me.percentage = 1F
		Else
			Me.percentage = 0F
		End If
		Me.apply()
		If Me.type = "value" Then
			Me.CallBack("onupdate")
		End If
		If Me.loopType = DialogueriTween.LoopType.none Then
			Me.Dispose()
		Else
			Me.TweenLoop()
		End If
		Me.CallBack("oncomplete")
	End Sub

	' Token: 0x06004749 RID: 18249 RVA: 0x002572E4 File Offset: 0x002556E4
	Private Sub TweenLoop()
		Me.DisableKinematic()
		Dim loopType As DialogueriTween.LoopType = Me.loopType
		If loopType <> DialogueriTween.LoopType.[loop] Then
			If loopType = DialogueriTween.LoopType.pingPong Then
				Me.reverse = Not Me.reverse
				Me.runningTime = 0F
				MyBase.StartCoroutine("TweenRestart")
			End If
		Else
			Me.percentage = 0F
			Me.runningTime = 0F
			Me.apply()
			MyBase.StartCoroutine("TweenRestart")
		End If
	End Sub

	' Token: 0x0600474A RID: 18250 RVA: 0x00257370 File Offset: 0x00255770
	Public Shared Function RectUpdate(currentValue As Rect, targetValue As Rect, speed As Single) As Rect
		Dim rect As Rect = New Rect(DialogueriTween.FloatUpdate(currentValue.x, targetValue.x, speed), DialogueriTween.FloatUpdate(currentValue.y, targetValue.y, speed), DialogueriTween.FloatUpdate(currentValue.width, targetValue.width, speed), DialogueriTween.FloatUpdate(currentValue.height, targetValue.height, speed))
		Return rect
	End Function

	' Token: 0x0600474B RID: 18251 RVA: 0x002573D8 File Offset: 0x002557D8
	Public Shared Function Vector3Update(currentValue As Vector3, targetValue As Vector3, speed As Single) As Vector3
		Dim vector As Vector3 = targetValue - currentValue
		currentValue += vector * speed * Time.deltaTime
		Return currentValue
	End Function

	' Token: 0x0600474C RID: 18252 RVA: 0x00257408 File Offset: 0x00255808
	Public Shared Function Vector2Update(currentValue As Vector2, targetValue As Vector2, speed As Single) As Vector2
		Dim vector As Vector2 = targetValue - currentValue
		currentValue += vector * speed * Time.deltaTime
		Return currentValue
	End Function

	' Token: 0x0600474D RID: 18253 RVA: 0x00257438 File Offset: 0x00255838
	Public Shared Function FloatUpdate(currentValue As Single, targetValue As Single, speed As Single) As Single
		Dim num As Single = targetValue - currentValue
		currentValue += num * speed * Time.deltaTime
		Return currentValue
	End Function

	' Token: 0x0600474E RID: 18254 RVA: 0x00257457 File Offset: 0x00255857
	Public Shared Sub FadeUpdate(target As GameObject, args As Hashtable)
		args("a") = args("alpha")
		DialogueriTween.ColorUpdate(target, args)
	End Sub

	' Token: 0x0600474F RID: 18255 RVA: 0x00257476 File Offset: 0x00255876
	Public Shared Sub FadeUpdate(target As GameObject, alpha As Single, time As Single)
		DialogueriTween.FadeUpdate(target, DialogueriTween.Hash(New Object() { "alpha", alpha, "time", time }))
	End Sub

	' Token: 0x06004750 RID: 18256 RVA: 0x002574AC File Offset: 0x002558AC
	Public Shared Sub ColorUpdate(target As GameObject, args As Hashtable)
		DialogueriTween.CleanArgs(args)
		Dim array As Color() = New Color(3) {}
		If Not args.Contains("includechildren") OrElse CBool(args("includechildren")) Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					DialogueriTween.ColorUpdate(transform.gameObject, args)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
		Dim num As Single
		If args.Contains("time") Then
			num = CSng(args("time"))
			num *= DialogueriTween.Defaults.updateTimePercentage
		Else
			num = DialogueriTween.Defaults.updateTime
		End If
		If target.GetComponent(GetType(GUITexture)) Then
			Dim array2 As Color() = array
			Dim num2 As Integer = 0
			Dim array3 As Color() = array
			Dim num3 As Integer = 1
			Dim color As Color = target.GetComponent(Of GUITexture)().color
			Dim color2 As Color = color
			array3(num3) = color
			array2(num2) = color2
		ElseIf target.GetComponent(GetType(GUIText)) Then
			Dim array4 As Color() = array
			Dim num4 As Integer = 0
			Dim array5 As Color() = array
			Dim num5 As Integer = 1
			Dim color3 As Color = target.GetComponent(Of GUIText)().material.color
			Dim color2 As Color = color3
			array5(num5) = color3
			array4(num4) = color2
		ElseIf target.GetComponent(Of Renderer)() Then
			Dim array6 As Color() = array
			Dim num6 As Integer = 0
			Dim array7 As Color() = array
			Dim num7 As Integer = 1
			Dim color4 As Color = target.GetComponent(Of Renderer)().material.color
			Dim color2 As Color = color4
			array7(num7) = color4
			array6(num6) = color2
		ElseIf target.GetComponent(Of Light)() Then
			Dim array8 As Color() = array
			Dim num8 As Integer = 0
			Dim array9 As Color() = array
			Dim num9 As Integer = 1
			Dim color5 As Color = target.GetComponent(Of Light)().color
			Dim color2 As Color = color5
			array9(num9) = color5
			array8(num8) = color2
		End If
		If args.Contains("color") Then
			array(1) = CType(args("color"), Color)
		Else
			If args.Contains("r") Then
				array(1).r = CSng(args("r"))
			End If
			If args.Contains("g") Then
				array(1).g = CSng(args("g"))
			End If
			If args.Contains("b") Then
				array(1).b = CSng(args("b"))
			End If
			If args.Contains("a") Then
				array(1).a = CSng(args("a"))
			End If
		End If
		array(3).r = Mathf.SmoothDamp(array(0).r, array(1).r, array(2).r, num)
		array(3).g = Mathf.SmoothDamp(array(0).g, array(1).g, array(2).g, num)
		array(3).b = Mathf.SmoothDamp(array(0).b, array(1).b, array(2).b, num)
		array(3).a = Mathf.SmoothDamp(array(0).a, array(1).a, array(2).a, num)
		If target.GetComponent(GetType(GUITexture)) Then
			target.GetComponent(Of GUITexture)().color = array(3)
		ElseIf target.GetComponent(GetType(GUIText)) Then
			target.GetComponent(Of GUIText)().material.color = array(3)
		ElseIf target.GetComponent(Of Renderer)() Then
			target.GetComponent(Of Renderer)().material.color = array(3)
		ElseIf target.GetComponent(Of Light)() Then
			target.GetComponent(Of Light)().color = array(3)
		End If
	End Sub

	' Token: 0x06004751 RID: 18257 RVA: 0x00257910 File Offset: 0x00255D10
	Public Shared Sub ColorUpdate(target As GameObject, color As Color, time As Single)
		DialogueriTween.ColorUpdate(target, DialogueriTween.Hash(New Object() { "color", color, "time", time }))
	End Sub

	' Token: 0x06004752 RID: 18258 RVA: 0x00257948 File Offset: 0x00255D48
	Public Shared Sub AudioUpdate(target As GameObject, args As Hashtable)
		DialogueriTween.CleanArgs(args)
		Dim array As Vector2() = New Vector2(3) {}
		Dim num As Single
		If args.Contains("time") Then
			num = CSng(args("time"))
			num *= DialogueriTween.Defaults.updateTimePercentage
		Else
			num = DialogueriTween.Defaults.updateTime
		End If
		Dim audioSource As AudioSource
		If args.Contains("audiosource") Then
			audioSource = CType(args("audiosource"), AudioSource)
		Else
			If Not target.GetComponent(GetType(AudioSource)) Then
				Global.Debug.LogError("iTween Error: AudioUpdate requires an AudioSource.", Nothing)
				Return
			End If
			audioSource = target.GetComponent(Of AudioSource)()
		End If
		Dim array2 As Vector2() = array
		Dim num2 As Integer = 0
		Dim array3 As Vector2() = array
		Dim num3 As Integer = 1
		Dim vector As Vector2 = New Vector2(audioSource.volume, audioSource.pitch)
		Dim vector2 As Vector2 = vector
		array3(num3) = vector
		array2(num2) = vector2
		If args.Contains("volume") Then
			array(1).x = CSng(args("volume"))
		End If
		If args.Contains("pitch") Then
			array(1).y = CSng(args("pitch"))
		End If
		array(3).x = Mathf.SmoothDampAngle(array(0).x, array(1).x, array(2).x, num)
		array(3).y = Mathf.SmoothDampAngle(array(0).y, array(1).y, array(2).y, num)
		audioSource.volume = array(3).x
		audioSource.pitch = array(3).y
	End Sub

	' Token: 0x06004753 RID: 18259 RVA: 0x00257B04 File Offset: 0x00255F04
	Public Shared Sub AudioUpdate(target As GameObject, volume As Single, pitch As Single, time As Single)
		DialogueriTween.AudioUpdate(target, DialogueriTween.Hash(New Object() { "volume", volume, "pitch", pitch, "time", time }))
	End Sub

	' Token: 0x06004754 RID: 18260 RVA: 0x00257B58 File Offset: 0x00255F58
	Public Shared Sub RotateUpdate(target As GameObject, args As Hashtable)
		DialogueriTween.CleanArgs(args)
		Dim array As Vector3() = New Vector3(3) {}
		Dim eulerAngles As Vector3 = target.transform.eulerAngles
		Dim num As Single
		If args.Contains("time") Then
			num = CSng(args("time"))
			num *= DialogueriTween.Defaults.updateTimePercentage
		Else
			num = DialogueriTween.Defaults.updateTime
		End If
		Dim flag As Boolean
		If args.Contains("islocal") Then
			flag = CBool(args("islocal"))
		Else
			flag = DialogueriTween.Defaults.isLocal
		End If
		If flag Then
			array(0) = target.transform.localEulerAngles
		Else
			array(0) = target.transform.eulerAngles
		End If
		If args.Contains("rotation") Then
			If args("rotation").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = CType(args("rotation"), Transform)
				array(1) = transform.eulerAngles
			ElseIf args("rotation").[GetType]() Is GetType(Vector3) Then
				array(1) = CType(args("rotation"), Vector3)
			End If
		End If
		array(3).x = Mathf.SmoothDampAngle(array(0).x, array(1).x, array(2).x, num)
		array(3).y = Mathf.SmoothDampAngle(array(0).y, array(1).y, array(2).y, num)
		array(3).z = Mathf.SmoothDampAngle(array(0).z, array(1).z, array(2).z, num)
		If flag Then
			target.transform.localEulerAngles = array(3)
		Else
			target.transform.eulerAngles = array(3)
		End If
		If target.GetComponent(Of Rigidbody)() IsNot Nothing Then
			Dim eulerAngles2 As Vector3 = target.transform.eulerAngles
			target.transform.eulerAngles = eulerAngles
			target.GetComponent(Of Rigidbody)().MoveRotation(Quaternion.Euler(eulerAngles2))
		End If
	End Sub

	' Token: 0x06004755 RID: 18261 RVA: 0x00257DC3 File Offset: 0x002561C3
	Public Shared Sub RotateUpdate(target As GameObject, rotation As Vector3, time As Single)
		DialogueriTween.RotateUpdate(target, DialogueriTween.Hash(New Object() { "rotation", rotation, "time", time }))
	End Sub

	' Token: 0x06004756 RID: 18262 RVA: 0x00257DF8 File Offset: 0x002561F8
	Public Shared Sub ScaleUpdate(target As GameObject, args As Hashtable)
		DialogueriTween.CleanArgs(args)
		Dim array As Vector3() = New Vector3(3) {}
		Dim num As Single
		If args.Contains("time") Then
			num = CSng(args("time"))
			num *= DialogueriTween.Defaults.updateTimePercentage
		Else
			num = DialogueriTween.Defaults.updateTime
		End If
		Dim array2 As Vector3() = array
		Dim num2 As Integer = 0
		Dim array3 As Vector3() = array
		Dim num3 As Integer = 1
		Dim localScale As Vector3 = target.transform.localScale
		Dim vector As Vector3 = localScale
		array3(num3) = localScale
		array2(num2) = vector
		If args.Contains("scale") Then
			If args("scale").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = CType(args("scale"), Transform)
				array(1) = transform.localScale
			ElseIf args("scale").[GetType]() Is GetType(Vector3) Then
				array(1) = CType(args("scale"), Vector3)
			End If
		Else
			If args.Contains("x") Then
				array(1).x = CSng(args("x"))
			End If
			If args.Contains("y") Then
				array(1).y = CSng(args("y"))
			End If
			If args.Contains("z") Then
				array(1).z = CSng(args("z"))
			End If
		End If
		array(3).x = Mathf.SmoothDamp(array(0).x, array(1).x, array(2).x, num)
		array(3).y = Mathf.SmoothDamp(array(0).y, array(1).y, array(2).y, num)
		array(3).z = Mathf.SmoothDamp(array(0).z, array(1).z, array(2).z, num)
		target.transform.localScale = array(3)
	End Sub

	' Token: 0x06004757 RID: 18263 RVA: 0x00258041 File Offset: 0x00256441
	Public Shared Sub ScaleUpdate(target As GameObject, scale As Vector3, time As Single)
		DialogueriTween.ScaleUpdate(target, DialogueriTween.Hash(New Object() { "scale", scale, "time", time }))
	End Sub

	' Token: 0x06004758 RID: 18264 RVA: 0x00258078 File Offset: 0x00256478
	Public Shared Sub MoveUpdate(target As GameObject, args As Hashtable)
		DialogueriTween.CleanArgs(args)
		Dim array As Vector3() = New Vector3(3) {}
		Dim position As Vector3 = target.transform.position
		Dim num As Single
		If args.Contains("time") Then
			num = CSng(args("time"))
			num *= DialogueriTween.Defaults.updateTimePercentage
		Else
			num = DialogueriTween.Defaults.updateTime
		End If
		Dim flag As Boolean
		If args.Contains("islocal") Then
			flag = CBool(args("islocal"))
		Else
			flag = DialogueriTween.Defaults.isLocal
		End If
		If flag Then
			Dim array2 As Vector3() = array
			Dim num2 As Integer = 0
			Dim array3 As Vector3() = array
			Dim num3 As Integer = 1
			Dim localPosition As Vector3 = target.transform.localPosition
			Dim vector As Vector3 = localPosition
			array3(num3) = localPosition
			array2(num2) = vector
		Else
			Dim array4 As Vector3() = array
			Dim num4 As Integer = 0
			Dim array5 As Vector3() = array
			Dim num5 As Integer = 1
			Dim position2 As Vector3 = target.transform.position
			Dim vector As Vector3 = position2
			array5(num5) = position2
			array4(num4) = vector
		End If
		If args.Contains("position") Then
			If args("position").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = CType(args("position"), Transform)
				array(1) = transform.position
			ElseIf args("position").[GetType]() Is GetType(Vector3) Then
				array(1) = CType(args("position"), Vector3)
			End If
		Else
			If args.Contains("x") Then
				array(1).x = CSng(args("x"))
			End If
			If args.Contains("y") Then
				array(1).y = CSng(args("y"))
			End If
			If args.Contains("z") Then
				array(1).z = CSng(args("z"))
			End If
		End If
		array(3).x = Mathf.SmoothDamp(array(0).x, array(1).x, array(2).x, num)
		array(3).y = Mathf.SmoothDamp(array(0).y, array(1).y, array(2).y, num)
		array(3).z = Mathf.SmoothDamp(array(0).z, array(1).z, array(2).z, num)
		If args.Contains("orienttopath") AndAlso CBool(args("orienttopath")) Then
			args("looktarget") = array(3)
		End If
		If args.Contains("looktarget") Then
			DialogueriTween.LookUpdate(target, args)
		End If
		If flag Then
			target.transform.localPosition = array(3)
		Else
			target.transform.position = array(3)
		End If
		If target.GetComponent(Of Rigidbody)() IsNot Nothing Then
			Dim position3 As Vector3 = target.transform.position
			target.transform.position = position
			target.GetComponent(Of Rigidbody)().MovePosition(position3)
		End If
	End Sub

	' Token: 0x06004759 RID: 18265 RVA: 0x002583E1 File Offset: 0x002567E1
	Public Shared Sub MoveUpdate(target As GameObject, position As Vector3, time As Single)
		DialogueriTween.MoveUpdate(target, DialogueriTween.Hash(New Object() { "position", position, "time", time }))
	End Sub

	' Token: 0x0600475A RID: 18266 RVA: 0x00258418 File Offset: 0x00256818
	Public Shared Sub LookUpdate(target As GameObject, args As Hashtable)
		DialogueriTween.CleanArgs(args)
		Dim array As Vector3() = New Vector3(4) {}
		Dim num As Single
		If args.Contains("looktime") Then
			num = CSng(args("looktime"))
			num *= DialogueriTween.Defaults.updateTimePercentage
		ElseIf args.Contains("time") Then
			num = CSng(args("time")) * 0.15F
			num *= DialogueriTween.Defaults.updateTimePercentage
		Else
			num = DialogueriTween.Defaults.updateTime
		End If
		array(0) = target.transform.eulerAngles
		If args.Contains("looktarget") Then
			If args("looktarget").[GetType]() Is GetType(Transform) Then
				Dim transform As Transform = target.transform
				Dim transform2 As Transform = CType(args("looktarget"), Transform)
				Dim vector As Vector3? = CType(args("up"), Vector3?)
				transform.LookAt(transform2, If((vector Is Nothing), DialogueriTween.Defaults.up, vector.Value))
			ElseIf args("looktarget").[GetType]() Is GetType(Vector3) Then
				Dim transform3 As Transform = target.transform
				Dim vector2 As Vector3 = CType(args("looktarget"), Vector3)
				Dim vector3 As Vector3? = CType(args("up"), Vector3?)
				transform3.LookAt(vector2, If((vector3 Is Nothing), DialogueriTween.Defaults.up, vector3.Value))
			End If
			array(1) = target.transform.eulerAngles
			target.transform.eulerAngles = array(0)
			array(3).x = Mathf.SmoothDampAngle(array(0).x, array(1).x, array(2).x, num)
			array(3).y = Mathf.SmoothDampAngle(array(0).y, array(1).y, array(2).y, num)
			array(3).z = Mathf.SmoothDampAngle(array(0).z, array(1).z, array(2).z, num)
			target.transform.eulerAngles = array(3)
			If args.Contains("axis") Then
				array(4) = target.transform.eulerAngles
				Dim text As String = CStr(args("axis"))
				If text IsNot Nothing Then
					If Not(text = "x") Then
						If Not(text = "y") Then
							If text = "z" Then
								array(4).x = array(0).x
								array(4).y = array(0).y
							End If
						Else
							array(4).x = array(0).x
							array(4).z = array(0).z
						End If
					Else
						array(4).y = array(0).y
						array(4).z = array(0).z
					End If
				End If
				target.transform.eulerAngles = array(4)
			End If
			Return
		End If
		Global.Debug.LogError("iTween Error: LookUpdate needs a 'looktarget' property!", Nothing)
	End Sub

	' Token: 0x0600475B RID: 18267 RVA: 0x002587BD File Offset: 0x00256BBD
	Public Shared Sub LookUpdate(target As GameObject, looktarget As Vector3, time As Single)
		DialogueriTween.LookUpdate(target, DialogueriTween.Hash(New Object() { "looktarget", looktarget, "time", time }))
	End Sub

	' Token: 0x0600475C RID: 18268 RVA: 0x002587F4 File Offset: 0x00256BF4
	Public Shared Function PathLength(path As Transform()) As Single
		Dim array As Vector3() = New Vector3(path.Length - 1) {}
		Dim num As Single = 0F
		For i As Integer = 0 To path.Length - 1
			array(i) = path(i).position
		Next
		Dim array2 As Vector3() = DialogueriTween.PathControlPointGenerator(array)
		Dim vector As Vector3 = DialogueriTween.Interp(array2, 0F)
		Dim num2 As Integer = path.Length * 20
		For j As Integer = 1 To num2
			Dim num3 As Single = CSng(j) / CSng(num2)
			Dim vector2 As Vector3 = DialogueriTween.Interp(array2, num3)
			num += Vector3.Distance(vector, vector2)
			vector = vector2
		Next
		Return num
	End Function

	' Token: 0x0600475D RID: 18269 RVA: 0x00258890 File Offset: 0x00256C90
	Public Shared Function PathLength(path As Vector3()) As Single
		Dim num As Single = 0F
		Dim array As Vector3() = DialogueriTween.PathControlPointGenerator(path)
		Dim vector As Vector3 = DialogueriTween.Interp(array, 0F)
		Dim num2 As Integer = path.Length * 20
		For i As Integer = 1 To num2
			Dim num3 As Single = CSng(i) / CSng(num2)
			Dim vector2 As Vector3 = DialogueriTween.Interp(array, num3)
			num += Vector3.Distance(vector, vector2)
			vector = vector2
		Next
		Return num
	End Function

	' Token: 0x0600475E RID: 18270 RVA: 0x002588F4 File Offset: 0x00256CF4
	Public Shared Function CameraTexture(color As Color) As Texture2D
		Dim texture2D As Texture2D = New Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, False)
		Dim array As Color() = New Color(Screen.width * Screen.height - 1) {}
		For i As Integer = 0 To array.Length - 1
			array(i) = color
		Next
		texture2D.SetPixels(array)
		texture2D.Apply()
		Return texture2D
	End Function

	' Token: 0x0600475F RID: 18271 RVA: 0x00258953 File Offset: 0x00256D53
	Public Shared Sub PutOnPath(target As GameObject, path As Vector3(), percent As Single)
		target.transform.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent)
	End Sub

	' Token: 0x06004760 RID: 18272 RVA: 0x0025896C File Offset: 0x00256D6C
	Public Shared Sub PutOnPath(target As Transform, path As Vector3(), percent As Single)
		target.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent)
	End Sub

	' Token: 0x06004761 RID: 18273 RVA: 0x00258980 File Offset: 0x00256D80
	Public Shared Sub PutOnPath(target As GameObject, path As Transform(), percent As Single)
		Dim array As Vector3() = New Vector3(path.Length - 1) {}
		For i As Integer = 0 To path.Length - 1
			array(i) = path(i).position
		Next
		target.transform.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent)
	End Sub

	' Token: 0x06004762 RID: 18274 RVA: 0x002589D8 File Offset: 0x00256DD8
	Public Shared Sub PutOnPath(target As Transform, path As Transform(), percent As Single)
		Dim array As Vector3() = New Vector3(path.Length - 1) {}
		For i As Integer = 0 To path.Length - 1
			array(i) = path(i).position
		Next
		target.position = DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent)
	End Sub

	' Token: 0x06004763 RID: 18275 RVA: 0x00258A28 File Offset: 0x00256E28
	Public Shared Function PointOnPath(path As Transform(), percent As Single) As Vector3
		Dim array As Vector3() = New Vector3(path.Length - 1) {}
		For i As Integer = 0 To path.Length - 1
			array(i) = path(i).position
		Next
		Return DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(array), percent)
	End Function

	' Token: 0x06004764 RID: 18276 RVA: 0x00258A72 File Offset: 0x00256E72
	Public Shared Sub DrawLine(line As Vector3())
		If line.Length > 0 Then
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "gizmos")
		End If
	End Sub

	' Token: 0x06004765 RID: 18277 RVA: 0x00258A8D File Offset: 0x00256E8D
	Public Shared Sub DrawLine(line As Vector3(), color As Color)
		If line.Length > 0 Then
			DialogueriTween.DrawLineHelper(line, color, "gizmos")
		End If
	End Sub

	' Token: 0x06004766 RID: 18278 RVA: 0x00258AA4 File Offset: 0x00256EA4
	Public Shared Sub DrawLine(line As Transform())
		If line.Length > 0 Then
			Dim array As Vector3() = New Vector3(line.Length - 1) {}
			For i As Integer = 0 To line.Length - 1
				array(i) = line(i).position
			Next
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "gizmos")
		End If
	End Sub

	' Token: 0x06004767 RID: 18279 RVA: 0x00258AFC File Offset: 0x00256EFC
	Public Shared Sub DrawLine(line As Transform(), color As Color)
		If line.Length > 0 Then
			Dim array As Vector3() = New Vector3(line.Length - 1) {}
			For i As Integer = 0 To line.Length - 1
				array(i) = line(i).position
			Next
			DialogueriTween.DrawLineHelper(array, color, "gizmos")
		End If
	End Sub

	' Token: 0x06004768 RID: 18280 RVA: 0x00258B4F File Offset: 0x00256F4F
	Public Shared Sub DrawLineGizmos(line As Vector3())
		If line.Length > 0 Then
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "gizmos")
		End If
	End Sub

	' Token: 0x06004769 RID: 18281 RVA: 0x00258B6A File Offset: 0x00256F6A
	Public Shared Sub DrawLineGizmos(line As Vector3(), color As Color)
		If line.Length > 0 Then
			DialogueriTween.DrawLineHelper(line, color, "gizmos")
		End If
	End Sub

	' Token: 0x0600476A RID: 18282 RVA: 0x00258B84 File Offset: 0x00256F84
	Public Shared Sub DrawLineGizmos(line As Transform())
		If line.Length > 0 Then
			Dim array As Vector3() = New Vector3(line.Length - 1) {}
			For i As Integer = 0 To line.Length - 1
				array(i) = line(i).position
			Next
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "gizmos")
		End If
	End Sub

	' Token: 0x0600476B RID: 18283 RVA: 0x00258BDC File Offset: 0x00256FDC
	Public Shared Sub DrawLineGizmos(line As Transform(), color As Color)
		If line.Length > 0 Then
			Dim array As Vector3() = New Vector3(line.Length - 1) {}
			For i As Integer = 0 To line.Length - 1
				array(i) = line(i).position
			Next
			DialogueriTween.DrawLineHelper(array, color, "gizmos")
		End If
	End Sub

	' Token: 0x0600476C RID: 18284 RVA: 0x00258C2F File Offset: 0x0025702F
	Public Shared Sub DrawLineHandles(line As Vector3())
		If line.Length > 0 Then
			DialogueriTween.DrawLineHelper(line, DialogueriTween.Defaults.color, "handles")
		End If
	End Sub

	' Token: 0x0600476D RID: 18285 RVA: 0x00258C4A File Offset: 0x0025704A
	Public Shared Sub DrawLineHandles(line As Vector3(), color As Color)
		If line.Length > 0 Then
			DialogueriTween.DrawLineHelper(line, color, "handles")
		End If
	End Sub

	' Token: 0x0600476E RID: 18286 RVA: 0x00258C64 File Offset: 0x00257064
	Public Shared Sub DrawLineHandles(line As Transform())
		If line.Length > 0 Then
			Dim array As Vector3() = New Vector3(line.Length - 1) {}
			For i As Integer = 0 To line.Length - 1
				array(i) = line(i).position
			Next
			DialogueriTween.DrawLineHelper(array, DialogueriTween.Defaults.color, "handles")
		End If
	End Sub

	' Token: 0x0600476F RID: 18287 RVA: 0x00258CBC File Offset: 0x002570BC
	Public Shared Sub DrawLineHandles(line As Transform(), color As Color)
		If line.Length > 0 Then
			Dim array As Vector3() = New Vector3(line.Length - 1) {}
			For i As Integer = 0 To line.Length - 1
				array(i) = line(i).position
			Next
			DialogueriTween.DrawLineHelper(array, color, "handles")
		End If
	End Sub

	' Token: 0x06004770 RID: 18288 RVA: 0x00258D0F File Offset: 0x0025710F
	Public Shared Function PointOnPath(path As Vector3(), percent As Single) As Vector3
		Return DialogueriTween.Interp(DialogueriTween.PathControlPointGenerator(path), percent)
	End Function

	' Token: 0x06004771 RID: 18289 RVA: 0x00258D1D File Offset: 0x0025711D
	Public Shared Sub DrawPath(path As Vector3())
		If path.Length > 0 Then
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "gizmos")
		End If
	End Sub

	' Token: 0x06004772 RID: 18290 RVA: 0x00258D38 File Offset: 0x00257138
	Public Shared Sub DrawPath(path As Vector3(), color As Color)
		If path.Length > 0 Then
			DialogueriTween.DrawPathHelper(path, color, "gizmos")
		End If
	End Sub

	' Token: 0x06004773 RID: 18291 RVA: 0x00258D50 File Offset: 0x00257150
	Public Shared Sub DrawPath(path As Transform())
		If path.Length > 0 Then
			Dim array As Vector3() = New Vector3(path.Length - 1) {}
			For i As Integer = 0 To path.Length - 1
				array(i) = path(i).position
			Next
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "gizmos")
		End If
	End Sub

	' Token: 0x06004774 RID: 18292 RVA: 0x00258DA8 File Offset: 0x002571A8
	Public Shared Sub DrawPath(path As Transform(), color As Color)
		If path.Length > 0 Then
			Dim array As Vector3() = New Vector3(path.Length - 1) {}
			For i As Integer = 0 To path.Length - 1
				array(i) = path(i).position
			Next
			DialogueriTween.DrawPathHelper(array, color, "gizmos")
		End If
	End Sub

	' Token: 0x06004775 RID: 18293 RVA: 0x00258DFB File Offset: 0x002571FB
	Public Shared Sub DrawPathGizmos(path As Vector3())
		If path.Length > 0 Then
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "gizmos")
		End If
	End Sub

	' Token: 0x06004776 RID: 18294 RVA: 0x00258E16 File Offset: 0x00257216
	Public Shared Sub DrawPathGizmos(path As Vector3(), color As Color)
		If path.Length > 0 Then
			DialogueriTween.DrawPathHelper(path, color, "gizmos")
		End If
	End Sub

	' Token: 0x06004777 RID: 18295 RVA: 0x00258E30 File Offset: 0x00257230
	Public Shared Sub DrawPathGizmos(path As Transform())
		If path.Length > 0 Then
			Dim array As Vector3() = New Vector3(path.Length - 1) {}
			For i As Integer = 0 To path.Length - 1
				array(i) = path(i).position
			Next
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "gizmos")
		End If
	End Sub

	' Token: 0x06004778 RID: 18296 RVA: 0x00258E88 File Offset: 0x00257288
	Public Shared Sub DrawPathGizmos(path As Transform(), color As Color)
		If path.Length > 0 Then
			Dim array As Vector3() = New Vector3(path.Length - 1) {}
			For i As Integer = 0 To path.Length - 1
				array(i) = path(i).position
			Next
			DialogueriTween.DrawPathHelper(array, color, "gizmos")
		End If
	End Sub

	' Token: 0x06004779 RID: 18297 RVA: 0x00258EDB File Offset: 0x002572DB
	Public Shared Sub DrawPathHandles(path As Vector3())
		If path.Length > 0 Then
			DialogueriTween.DrawPathHelper(path, DialogueriTween.Defaults.color, "handles")
		End If
	End Sub

	' Token: 0x0600477A RID: 18298 RVA: 0x00258EF6 File Offset: 0x002572F6
	Public Shared Sub DrawPathHandles(path As Vector3(), color As Color)
		If path.Length > 0 Then
			DialogueriTween.DrawPathHelper(path, color, "handles")
		End If
	End Sub

	' Token: 0x0600477B RID: 18299 RVA: 0x00258F10 File Offset: 0x00257310
	Public Shared Sub DrawPathHandles(path As Transform())
		If path.Length > 0 Then
			Dim array As Vector3() = New Vector3(path.Length - 1) {}
			For i As Integer = 0 To path.Length - 1
				array(i) = path(i).position
			Next
			DialogueriTween.DrawPathHelper(array, DialogueriTween.Defaults.color, "handles")
		End If
	End Sub

	' Token: 0x0600477C RID: 18300 RVA: 0x00258F68 File Offset: 0x00257368
	Public Shared Sub DrawPathHandles(path As Transform(), color As Color)
		If path.Length > 0 Then
			Dim array As Vector3() = New Vector3(path.Length - 1) {}
			For i As Integer = 0 To path.Length - 1
				array(i) = path(i).position
			Next
			DialogueriTween.DrawPathHelper(array, color, "handles")
		End If
	End Sub

	' Token: 0x0600477D RID: 18301 RVA: 0x00258FBC File Offset: 0x002573BC
	Public Shared Sub CameraFadeDepth(depth As Integer)
		If DialogueriTween.cameraFade Then
			DialogueriTween.cameraFade.transform.position = New Vector3(DialogueriTween.cameraFade.transform.position.x, DialogueriTween.cameraFade.transform.position.y, CSng(depth))
		End If
	End Sub

	' Token: 0x0600477E RID: 18302 RVA: 0x0025901C File Offset: 0x0025741C
	Public Shared Sub CameraFadeDestroy()
		If DialogueriTween.cameraFade Then
			Global.UnityEngine.[Object].Destroy(DialogueriTween.cameraFade)
		End If
	End Sub

	' Token: 0x0600477F RID: 18303 RVA: 0x00259037 File Offset: 0x00257437
	Public Shared Sub CameraFadeSwap(texture As Texture2D)
		If DialogueriTween.cameraFade Then
			DialogueriTween.cameraFade.GetComponent(Of GUITexture)().texture = texture
		End If
	End Sub

	' Token: 0x06004780 RID: 18304 RVA: 0x00259058 File Offset: 0x00257458
	Public Shared Function CameraFadeAdd(texture As Texture2D, depth As Integer) As GameObject
		If DialogueriTween.cameraFade Then
			Return Nothing
		End If
		DialogueriTween.cameraFade = New GameObject("iTween Camera Fade")
		DialogueriTween.cameraFade.transform.position = New Vector3(0.5F, 0.5F, CSng(depth))
		DialogueriTween.cameraFade.AddComponent(Of GUITexture)()
		DialogueriTween.cameraFade.GetComponent(Of GUITexture)().texture = texture
		DialogueriTween.cameraFade.GetComponent(Of GUITexture)().color = New Color(0.5F, 0.5F, 0.5F, 0F)
		Return DialogueriTween.cameraFade
	End Function

	' Token: 0x06004781 RID: 18305 RVA: 0x002590F0 File Offset: 0x002574F0
	Public Shared Function CameraFadeAdd(texture As Texture2D) As GameObject
		If DialogueriTween.cameraFade Then
			Return Nothing
		End If
		DialogueriTween.cameraFade = New GameObject("iTween Camera Fade")
		DialogueriTween.cameraFade.transform.position = New Vector3(0.5F, 0.5F, CSng(DialogueriTween.Defaults.cameraFadeDepth))
		DialogueriTween.cameraFade.AddComponent(Of GUITexture)()
		DialogueriTween.cameraFade.GetComponent(Of GUITexture)().texture = texture
		DialogueriTween.cameraFade.GetComponent(Of GUITexture)().color = New Color(0.5F, 0.5F, 0.5F, 0F)
		Return DialogueriTween.cameraFade
	End Function

	' Token: 0x06004782 RID: 18306 RVA: 0x0025918C File Offset: 0x0025758C
	Public Shared Function CameraFadeAdd() As GameObject
		If DialogueriTween.cameraFade Then
			Return Nothing
		End If
		DialogueriTween.cameraFade = New GameObject("iTween Camera Fade")
		DialogueriTween.cameraFade.transform.position = New Vector3(0.5F, 0.5F, CSng(DialogueriTween.Defaults.cameraFadeDepth))
		DialogueriTween.cameraFade.AddComponent(Of GUITexture)()
		DialogueriTween.cameraFade.GetComponent(Of GUITexture)().texture = DialogueriTween.CameraTexture(Color.black)
		DialogueriTween.cameraFade.GetComponent(Of GUITexture)().color = New Color(0.5F, 0.5F, 0.5F, 0F)
		Return DialogueriTween.cameraFade
	End Function

	' Token: 0x06004783 RID: 18307 RVA: 0x00259230 File Offset: 0x00257630
	Public Shared Sub [Resume](target As GameObject)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			dialogueriTween.enabled = True
		Next
	End Sub

	' Token: 0x06004784 RID: 18308 RVA: 0x00259274 File Offset: 0x00257674
	Public Shared Sub [Resume](target As GameObject, includechildren As Boolean)
		DialogueriTween.[Resume](target)
		If includechildren Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					DialogueriTween.[Resume](transform.gameObject, True)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
	End Sub

	' Token: 0x06004785 RID: 18309 RVA: 0x002592EC File Offset: 0x002576EC
	Public Shared Sub [Resume](target As GameObject, type As String)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			Dim text As String = dialogueriTween.type + dialogueriTween.method
			text = text.Substring(0, type.Length)
			If text.ToLower() = type.ToLower() Then
				dialogueriTween.enabled = True
			End If
		Next
	End Sub

	' Token: 0x06004786 RID: 18310 RVA: 0x0025936C File Offset: 0x0025776C
	Public Shared Sub [Resume](target As GameObject, type As String, includechildren As Boolean)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			Dim text As String = dialogueriTween.type + dialogueriTween.method
			text = text.Substring(0, type.Length)
			If text.ToLower() = type.ToLower() Then
				dialogueriTween.enabled = True
			End If
		Next
		If includechildren Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					DialogueriTween.[Resume](transform.gameObject, type, True)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
	End Sub

	' Token: 0x06004787 RID: 18311 RVA: 0x00259458 File Offset: 0x00257858
	Public Shared Sub [Resume]()
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			Dim gameObject As GameObject = CType(hashtable("target"), GameObject)
			DialogueriTween.[Resume](gameObject)
		Next
	End Sub

	' Token: 0x06004788 RID: 18312 RVA: 0x002594A8 File Offset: 0x002578A8
	Public Shared Sub [Resume](type As String)
		Dim arrayList As ArrayList = New ArrayList()
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			Dim gameObject As GameObject = CType(hashtable("target"), GameObject)
			arrayList.Insert(arrayList.Count, gameObject)
		Next
		For j As Integer = 0 To arrayList.Count - 1
			DialogueriTween.[Resume](CType(arrayList(j), GameObject), type)
		Next
	End Sub

	' Token: 0x06004789 RID: 18313 RVA: 0x00259534 File Offset: 0x00257934
	Public Shared Sub Pause(target As GameObject)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			If dialogueriTween.delay > 0F Then
				dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted
				dialogueriTween.StopCoroutine("TweenDelay")
			End If
			dialogueriTween.isPaused = True
			dialogueriTween.enabled = False
		Next
	End Sub

	' Token: 0x0600478A RID: 18314 RVA: 0x002595B4 File Offset: 0x002579B4
	Public Shared Sub Pause(target As GameObject, includechildren As Boolean)
		DialogueriTween.Pause(target)
		If includechildren Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					DialogueriTween.Pause(transform.gameObject, True)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
	End Sub

	' Token: 0x0600478B RID: 18315 RVA: 0x0025962C File Offset: 0x00257A2C
	Public Shared Sub Pause(target As GameObject, type As String)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			Dim text As String = dialogueriTween.type + dialogueriTween.method
			text = text.Substring(0, type.Length)
			If text.ToLower() = type.ToLower() Then
				If dialogueriTween.delay > 0F Then
					dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted
					dialogueriTween.StopCoroutine("TweenDelay")
				End If
				dialogueriTween.isPaused = True
				dialogueriTween.enabled = False
			End If
		Next
	End Sub

	' Token: 0x0600478C RID: 18316 RVA: 0x002596E8 File Offset: 0x00257AE8
	Public Shared Sub Pause(target As GameObject, type As String, includechildren As Boolean)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			Dim text As String = dialogueriTween.type + dialogueriTween.method
			text = text.Substring(0, type.Length)
			If text.ToLower() = type.ToLower() Then
				If dialogueriTween.delay > 0F Then
					dialogueriTween.delay -= Time.time - dialogueriTween.delayStarted
					dialogueriTween.StopCoroutine("TweenDelay")
				End If
				dialogueriTween.isPaused = True
				dialogueriTween.enabled = False
			End If
		Next
		If includechildren Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					DialogueriTween.Pause(transform.gameObject, type, True)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
	End Sub

	' Token: 0x0600478D RID: 18317 RVA: 0x00259810 File Offset: 0x00257C10
	Public Shared Sub Pause()
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			Dim gameObject As GameObject = CType(hashtable("target"), GameObject)
			DialogueriTween.Pause(gameObject)
		Next
	End Sub

	' Token: 0x0600478E RID: 18318 RVA: 0x00259860 File Offset: 0x00257C60
	Public Shared Sub Pause(type As String)
		Dim arrayList As ArrayList = New ArrayList()
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			Dim gameObject As GameObject = CType(hashtable("target"), GameObject)
			arrayList.Insert(arrayList.Count, gameObject)
		Next
		For j As Integer = 0 To arrayList.Count - 1
			DialogueriTween.Pause(CType(arrayList(j), GameObject), type)
		Next
	End Sub

	' Token: 0x0600478F RID: 18319 RVA: 0x002598EB File Offset: 0x00257CEB
	Public Shared Function Count() As Integer
		Return DialogueriTween.tweens.Count
	End Function

	' Token: 0x06004790 RID: 18320 RVA: 0x002598F8 File Offset: 0x00257CF8
	Public Shared Function Count(type As String) As Integer
		Dim num As Integer = 0
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			Dim text As String = CStr(hashtable("type")) + CStr(hashtable("method"))
			text = text.Substring(0, type.Length)
			If text.ToLower() = type.ToLower() Then
				num += 1
			End If
		Next
		Return num
	End Function

	' Token: 0x06004791 RID: 18321 RVA: 0x00259984 File Offset: 0x00257D84
	Public Shared Function Count(target As GameObject) As Integer
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		Return components.Length
	End Function

	' Token: 0x06004792 RID: 18322 RVA: 0x002599A8 File Offset: 0x00257DA8
	Public Shared Function Count(target As GameObject, type As String) As Integer
		Dim num As Integer = 0
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			Dim text As String = dialogueriTween.type + dialogueriTween.method
			text = text.Substring(0, type.Length)
			If text.ToLower() = type.ToLower() Then
				num += 1
			End If
		Next
		Return num
	End Function

	' Token: 0x06004793 RID: 18323 RVA: 0x00259A2C File Offset: 0x00257E2C
	Public Shared Sub [Stop]()
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			Dim gameObject As GameObject = CType(hashtable("target"), GameObject)
			DialogueriTween.[Stop](gameObject)
		Next
		DialogueriTween.tweens.Clear()
	End Sub

	' Token: 0x06004794 RID: 18324 RVA: 0x00259A88 File Offset: 0x00257E88
	Public Shared Sub [Stop](type As String)
		Dim arrayList As ArrayList = New ArrayList()
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			Dim gameObject As GameObject = CType(hashtable("target"), GameObject)
			arrayList.Insert(arrayList.Count, gameObject)
		Next
		For j As Integer = 0 To arrayList.Count - 1
			DialogueriTween.[Stop](CType(arrayList(j), GameObject), type)
		Next
	End Sub

	' Token: 0x06004795 RID: 18325 RVA: 0x00259B14 File Offset: 0x00257F14
	Public Shared Sub StopByName(name As String)
		Dim arrayList As ArrayList = New ArrayList()
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			Dim gameObject As GameObject = CType(hashtable("target"), GameObject)
			arrayList.Insert(arrayList.Count, gameObject)
		Next
		For j As Integer = 0 To arrayList.Count - 1
			DialogueriTween.StopByName(CType(arrayList(j), GameObject), name)
		Next
	End Sub

	' Token: 0x06004796 RID: 18326 RVA: 0x00259BA0 File Offset: 0x00257FA0
	Public Shared Sub [Stop](target As GameObject)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			dialogueriTween.Dispose()
		Next
	End Sub

	' Token: 0x06004797 RID: 18327 RVA: 0x00259BE4 File Offset: 0x00257FE4
	Public Shared Sub [Stop](target As GameObject, includechildren As Boolean)
		DialogueriTween.[Stop](target)
		If includechildren Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					DialogueriTween.[Stop](transform.gameObject, True)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
	End Sub

	' Token: 0x06004798 RID: 18328 RVA: 0x00259C5C File Offset: 0x0025805C
	Public Shared Sub [Stop](target As GameObject, type As String)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			Dim text As String = dialogueriTween.type + dialogueriTween.method
			text = text.Substring(0, type.Length)
			If text.ToLower() = type.ToLower() Then
				dialogueriTween.Dispose()
			End If
		Next
	End Sub

	' Token: 0x06004799 RID: 18329 RVA: 0x00259CDC File Offset: 0x002580DC
	Public Shared Sub StopByName(target As GameObject, name As String)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			If dialogueriTween._name = name Then
				dialogueriTween.Dispose()
			End If
		Next
	End Sub

	' Token: 0x0600479A RID: 18330 RVA: 0x00259D30 File Offset: 0x00258130
	Public Shared Sub [Stop](target As GameObject, type As String, includechildren As Boolean)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			Dim text As String = dialogueriTween.type + dialogueriTween.method
			text = text.Substring(0, type.Length)
			If text.ToLower() = type.ToLower() Then
				dialogueriTween.Dispose()
			End If
		Next
		If includechildren Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					DialogueriTween.[Stop](transform.gameObject, type, True)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
	End Sub

	' Token: 0x0600479B RID: 18331 RVA: 0x00259E1C File Offset: 0x0025821C
	Public Shared Sub StopByName(target As GameObject, name As String, includechildren As Boolean)
		Dim components As Component() = target.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			If dialogueriTween._name = name Then
				dialogueriTween.Dispose()
			End If
		Next
		If includechildren Then
			Dim enumerator As IEnumerator = target.transform.GetEnumerator()
			Try
				While enumerator.MoveNext()
					Dim obj As Object = enumerator.Current
					Dim transform As Transform = CType(obj, Transform)
					DialogueriTween.StopByName(transform.gameObject, name, True)
				End While
			Finally
				Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
				Dim disposable2 As IDisposable = disposable
				If disposable IsNot Nothing Then
					disposable2.Dispose()
				End If
			End Try
		End If
	End Sub

	' Token: 0x0600479C RID: 18332 RVA: 0x00259EDC File Offset: 0x002582DC
	Public Shared Function Hash(ParamArray args As Object()) As Hashtable
		Dim hashtable As Hashtable = New Hashtable(args.Length / 2)
		If args.Length Mod 2 <> 0 Then
			Global.Debug.LogError("Tween Error: Hash requires an even number of arguments!", Nothing)
			Return Nothing
		End If
		For i As Integer = 0 To args.Length - 1 - 1 Step 2
			hashtable.Add(args(i), args(i + 1))
		Next
		Return hashtable
	End Function

	' Token: 0x0600479D RID: 18333 RVA: 0x00259F30 File Offset: 0x00258330
	Private Sub Awake()
		Me.RetrieveArgs()
		Me.lastRealTime = Time.realtimeSinceStartup
	End Sub

	' Token: 0x0600479E RID: 18334 RVA: 0x00259F44 File Offset: 0x00258344
	Private Iterator Function Start() As IEnumerator
		If Me.delay > 0F Then
			Yield MyBase.StartCoroutine("TweenDelay")
		End If
		Me.TweenStart()
		Return
	End Function

	' Token: 0x0600479F RID: 18335 RVA: 0x00259F60 File Offset: 0x00258360
	Private Sub Update()
		If Me.isRunning AndAlso Not Me.physics Then
			If Not Me.reverse Then
				If Me.percentage < 1F Then
					Me.TweenUpdate()
				Else
					Me.TweenComplete()
				End If
			ElseIf Me.percentage > 0F Then
				Me.TweenUpdate()
			Else
				Me.TweenComplete()
			End If
		End If
	End Sub

	' Token: 0x060047A0 RID: 18336 RVA: 0x00259FD8 File Offset: 0x002583D8
	Private Sub FixedUpdate()
		If Me.isRunning AndAlso Me.physics Then
			If Not Me.reverse Then
				If Me.percentage < 1F Then
					Me.TweenUpdate()
				Else
					Me.TweenComplete()
				End If
			ElseIf Me.percentage > 0F Then
				Me.TweenUpdate()
			Else
				Me.TweenComplete()
			End If
		End If
	End Sub

	' Token: 0x060047A1 RID: 18337 RVA: 0x0025A050 File Offset: 0x00258450
	Private Sub LateUpdate()
		If Me.tweenArguments.Contains("looktarget") AndAlso Me.isRunning AndAlso (Me.type = "move" OrElse Me.type = "shake" OrElse Me.type = "punch") Then
			DialogueriTween.LookUpdate(MyBase.gameObject, Me.tweenArguments)
		End If
	End Sub

	' Token: 0x060047A2 RID: 18338 RVA: 0x0025A0D0 File Offset: 0x002584D0
	Private Sub OnEnable()
		If Me.isRunning Then
			Me.EnableKinematic()
		End If
		If Me.isPaused Then
			Me.isPaused = False
			If Me.delay > 0F Then
				Me.wasPaused = True
				Me.ResumeDelay()
			End If
		End If
	End Sub

	' Token: 0x060047A3 RID: 18339 RVA: 0x0025A11D File Offset: 0x0025851D
	Private Sub OnDisable()
		Me.DisableKinematic()
	End Sub

	' Token: 0x060047A4 RID: 18340 RVA: 0x0025A128 File Offset: 0x00258528
	Private Shared Sub DrawLineHelper(line As Vector3(), color As Color, method As String)
		Gizmos.color = color
		For i As Integer = 0 To line.Length - 1 - 1
			If method = "gizmos" Then
				Gizmos.DrawLine(line(i), line(i + 1))
			ElseIf method = "handles" Then
				Global.Debug.LogError("iTween Error: Drawing a line with Handles is temporarily disabled because of compatability issues with Unity 2.6!", Nothing)
			End If
		Next
	End Sub

	' Token: 0x060047A5 RID: 18341 RVA: 0x0025A1A0 File Offset: 0x002585A0
	Private Shared Sub DrawPathHelper(path As Vector3(), color As Color, method As String)
		Dim array As Vector3() = DialogueriTween.PathControlPointGenerator(path)
		Dim vector As Vector3 = DialogueriTween.Interp(array, 0F)
		Gizmos.color = color
		Dim num As Integer = path.Length * 20
		For i As Integer = 1 To num
			Dim num2 As Single = CSng(i) / CSng(num)
			Dim vector2 As Vector3 = DialogueriTween.Interp(array, num2)
			If method = "gizmos" Then
				Gizmos.DrawLine(vector2, vector)
			ElseIf method = "handles" Then
				Global.Debug.LogError("iTween Error: Drawing a path with Handles is temporarily disabled because of compatability issues with Unity 2.6!", Nothing)
			End If
			vector = vector2
		Next
	End Sub

	' Token: 0x060047A6 RID: 18342 RVA: 0x0025A22C File Offset: 0x0025862C
	Private Shared Function PathControlPointGenerator(path As Vector3()) As Vector3()
		Dim num As Integer = 2
		Dim array As Vector3() = New Vector3(path.Length + num - 1) {}
		Array.Copy(path, 0, array, 1, path.Length)
		array(0) = array(1) + (array(1) - array(2))
		array(array.Length - 1) = array(array.Length - 2) + (array(array.Length - 2) - array(array.Length - 3))
		If array(1) = array(array.Length - 2) Then
			Dim array2 As Vector3() = New Vector3(array.Length - 1) {}
			Array.Copy(array, array2, array.Length)
			array2(0) = array2(array2.Length - 3)
			array2(array2.Length - 1) = array2(2)
			array = New Vector3(array2.Length - 1) {}
			Array.Copy(array2, array, array2.Length)
		End If
		Return array
	End Function

	' Token: 0x060047A7 RID: 18343 RVA: 0x0025A360 File Offset: 0x00258760
	Private Shared Function Interp(pts As Vector3(), t As Single) As Vector3
		Dim num As Integer = pts.Length - 3
		Dim num2 As Integer = Mathf.Min(Mathf.FloorToInt(t * CSng(num)), num - 1)
		Dim num3 As Single = t * CSng(num) - CSng(num2)
		Dim vector As Vector3 = pts(num2)
		Dim vector2 As Vector3 = pts(num2 + 1)
		Dim vector3 As Vector3 = pts(num2 + 2)
		Dim vector4 As Vector3 = pts(num2 + 3)
		Return 0.5F * ((-vector + 3F * vector2 - 3F * vector3 + vector4) * (num3 * num3 * num3) + (2F * vector - 5F * vector2 + 4F * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2F * vector2)
	End Function

	' Token: 0x060047A8 RID: 18344 RVA: 0x0025A478 File Offset: 0x00258878
	Private Shared Sub Launch(target As GameObject, args As Hashtable)
		If Not args.Contains("id") Then
			args("id") = DialogueriTween.GenerateID()
		End If
		If Not args.Contains("target") Then
			args("target") = target
		End If
		DialogueriTween.tweens.Insert(0, args)
		target.AddComponent(Of DialogueriTween)()
	End Sub

	' Token: 0x060047A9 RID: 18345 RVA: 0x0025A4D4 File Offset: 0x002588D4
	Private Shared Function CleanArgs(args As Hashtable) As Hashtable
		Dim hashtable As Hashtable = New Hashtable(args.Count)
		Dim hashtable2 As Hashtable = New Hashtable(args.Count)
		Dim enumerator As IDictionaryEnumerator = args.GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim obj As Object = enumerator.Current
				Dim dictionaryEntry As DictionaryEntry = CType(obj, DictionaryEntry)
				hashtable.Add(dictionaryEntry.Key, dictionaryEntry.Value)
			End While
		Finally
			Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable2 As IDisposable = disposable
			If disposable IsNot Nothing Then
				disposable2.Dispose()
			End If
		End Try
		Dim enumerator2 As IDictionaryEnumerator = hashtable.GetEnumerator()
		Try
			While enumerator2.MoveNext()
				Dim obj2 As Object = enumerator2.Current
				Dim dictionaryEntry2 As DictionaryEntry = CType(obj2, DictionaryEntry)
				If dictionaryEntry2.Value.[GetType]() Is GetType(Integer) Then
					Dim num As Integer = CInt(dictionaryEntry2.Value)
					Dim num2 As Single = CSng(num)
					args(dictionaryEntry2.Key) = num2
				End If
				If dictionaryEntry2.Value.[GetType]() Is GetType(Double) Then
					Dim num3 As Double = CDbl(dictionaryEntry2.Value)
					Dim num4 As Single = CSng(num3)
					args(dictionaryEntry2.Key) = num4
				End If
			End While
		Finally
			Dim disposable3 As IDisposable = TryCast(enumerator2, IDisposable)
			Dim disposable4 As IDisposable = disposable3
			If disposable3 IsNot Nothing Then
				disposable4.Dispose()
			End If
		End Try
		Dim enumerator3 As IDictionaryEnumerator = args.GetEnumerator()
		Try
			While enumerator3.MoveNext()
				Dim obj3 As Object = enumerator3.Current
				Dim dictionaryEntry3 As DictionaryEntry = CType(obj3, DictionaryEntry)
				hashtable2.Add(dictionaryEntry3.Key.ToString().ToLower(), dictionaryEntry3.Value)
			End While
		Finally
			Dim disposable5 As IDisposable = TryCast(enumerator3, IDisposable)
			Dim disposable6 As IDisposable = disposable5
			If disposable5 IsNot Nothing Then
				disposable6.Dispose()
			End If
		End Try
		args = hashtable2
		Return args
	End Function

	' Token: 0x060047AA RID: 18346 RVA: 0x0025A6A0 File Offset: 0x00258AA0
	Private Shared Function GenerateID() As String
		Dim num As Integer = 15
		Dim array As Char() = New Char() { "a"c, "b"c, "c"c, "d"c, "e"c, "f"c, "g"c, "h"c, "i"c, "j"c, "k"c, "l"c, "m"c, "n"c, "o"c, "p"c, "q"c, "r"c, "s"c, "t"c, "u"c, "v"c, "w"c, "x"c, "y"c, "z"c, "A"c, "B"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "I"c, "J"c, "K"c, "L"c, "M"c, "N"c, "O"c, "P"c, "Q"c, "R"c, "S"c, "T"c, "U"c, "V"c, "W"c, "X"c, "Y"c, "Z"c, "0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c }
		Dim num2 As Integer = array.Length - 1
		Dim text As String = String.Empty
		For i As Integer = 0 To num - 1
			text += array(CInt(Mathf.Floor(CSng(Global.UnityEngine.Random.Range(0, num2)))))
		Next
		Return text
	End Function

	' Token: 0x060047AB RID: 18347 RVA: 0x0025A704 File Offset: 0x00258B04
	Private Sub RetrieveArgs()
		Dim enumerator As IEnumerator = DialogueriTween.tweens.GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim obj As Object = enumerator.Current
				Dim hashtable As Hashtable = CType(obj, Hashtable)
				If CType(hashtable("target"), GameObject) Is MyBase.gameObject Then
					Me.tweenArguments = hashtable
					Exit While
				End If
			End While
		Finally
			Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable2 As IDisposable = disposable
			If disposable IsNot Nothing Then
				disposable2.Dispose()
			End If
		End Try
		Me.id = CStr(Me.tweenArguments("id"))
		Me.type = CStr(Me.tweenArguments("type"))
		Me._name = CStr(Me.tweenArguments("name"))
		Me.method = CStr(Me.tweenArguments("method"))
		If Me.tweenArguments.Contains("time") Then
			Me.time = CSng(Me.tweenArguments("time"))
		Else
			Me.time = DialogueriTween.Defaults.time
		End If
		If MyBase.GetComponent(Of Rigidbody)() IsNot Nothing Then
			Me.physics = True
		End If
		If Me.tweenArguments.Contains("delay") Then
			Me.delay = CSng(Me.tweenArguments("delay"))
		Else
			Me.delay = DialogueriTween.Defaults.delay
		End If
		If Me.tweenArguments.Contains("namedcolorvalue") Then
			If Me.tweenArguments("namedcolorvalue").[GetType]() Is GetType(DialogueriTween.NamedValueColor) Then
				Me.namedcolorvalue = CType(Me.tweenArguments("namedcolorvalue"), DialogueriTween.NamedValueColor)
			Else
				Try
					Me.namedcolorvalue = CType([Enum].Parse(GetType(DialogueriTween.NamedValueColor), CStr(Me.tweenArguments("namedcolorvalue")), True), DialogueriTween.NamedValueColor)
				Catch
					Me.namedcolorvalue = DialogueriTween.NamedValueColor._Color
				End Try
			End If
		Else
			Me.namedcolorvalue = DialogueriTween.Defaults.namedColorValue
		End If
		If Me.tweenArguments.Contains("looptype") Then
			If Me.tweenArguments("looptype").[GetType]() Is GetType(DialogueriTween.LoopType) Then
				Me.loopType = CType(Me.tweenArguments("looptype"), DialogueriTween.LoopType)
			Else
				Try
					Me.loopType = CType([Enum].Parse(GetType(DialogueriTween.LoopType), CStr(Me.tweenArguments("looptype")), True), DialogueriTween.LoopType)
				Catch
					Me.loopType = DialogueriTween.LoopType.none
				End Try
			End If
		Else
			Me.loopType = DialogueriTween.LoopType.none
		End If
		If Me.tweenArguments.Contains("easetype") Then
			If Me.tweenArguments("easetype").[GetType]() Is GetType(DialogueriTween.EaseType) Then
				Me.easeType = CType(Me.tweenArguments("easetype"), DialogueriTween.EaseType)
			Else
				Try
					Me.easeType = CType([Enum].Parse(GetType(DialogueriTween.EaseType), CStr(Me.tweenArguments("easetype")), True), DialogueriTween.EaseType)
				Catch
					Me.easeType = DialogueriTween.Defaults.easeType
				End Try
			End If
		Else
			Me.easeType = DialogueriTween.Defaults.easeType
		End If
		If Me.tweenArguments.Contains("space") Then
			If Me.tweenArguments("space").[GetType]() Is GetType(Space) Then
				Me.space = CType(Me.tweenArguments("space"), Space)
			Else
				Try
					Me.space = CType([Enum].Parse(GetType(Space), CStr(Me.tweenArguments("space")), True), Space)
				Catch
					Me.space = DialogueriTween.Defaults.space
				End Try
			End If
		Else
			Me.space = DialogueriTween.Defaults.space
		End If
		If Me.tweenArguments.Contains("islocal") Then
			Me.isLocal = CBool(Me.tweenArguments("islocal"))
		Else
			Me.isLocal = DialogueriTween.Defaults.isLocal
		End If
		If Me.tweenArguments.Contains("ignoretimescale") Then
			Me.useRealTime = CBool(Me.tweenArguments("ignoretimescale"))
		Else
			Me.useRealTime = DialogueriTween.Defaults.useRealTime
		End If
		Me.GetEasingFunction()
	End Sub

	' Token: 0x060047AC RID: 18348 RVA: 0x0025ABF8 File Offset: 0x00258FF8
	Private Sub GetEasingFunction()
		Select Case Me.easeType
			Case DialogueriTween.EaseType.easeInQuad
				Me.ease = AddressOf Me.easeInQuad
			Case DialogueriTween.EaseType.easeOutQuad
				Me.ease = AddressOf Me.easeOutQuad
			Case DialogueriTween.EaseType.easeInOutQuad
				Me.ease = AddressOf Me.easeInOutQuad
			Case DialogueriTween.EaseType.easeInCubic
				Me.ease = AddressOf Me.easeInCubic
			Case DialogueriTween.EaseType.easeOutCubic
				Me.ease = AddressOf Me.easeOutCubic
			Case DialogueriTween.EaseType.easeInOutCubic
				Me.ease = AddressOf Me.easeInOutCubic
			Case DialogueriTween.EaseType.easeInQuart
				Me.ease = AddressOf Me.easeInQuart
			Case DialogueriTween.EaseType.easeOutQuart
				Me.ease = AddressOf Me.easeOutQuart
			Case DialogueriTween.EaseType.easeInOutQuart
				Me.ease = AddressOf Me.easeInOutQuart
			Case DialogueriTween.EaseType.easeInQuint
				Me.ease = AddressOf Me.easeInQuint
			Case DialogueriTween.EaseType.easeOutQuint
				Me.ease = AddressOf Me.easeOutQuint
			Case DialogueriTween.EaseType.easeInOutQuint
				Me.ease = AddressOf Me.easeInOutQuint
			Case DialogueriTween.EaseType.easeInSine
				Me.ease = AddressOf Me.easeInSine
			Case DialogueriTween.EaseType.easeOutSine
				Me.ease = AddressOf Me.easeOutSine
			Case DialogueriTween.EaseType.easeInOutSine
				Me.ease = AddressOf Me.easeInOutSine
			Case DialogueriTween.EaseType.easeInExpo
				Me.ease = AddressOf Me.easeInExpo
			Case DialogueriTween.EaseType.easeOutExpo
				Me.ease = AddressOf Me.easeOutExpo
			Case DialogueriTween.EaseType.easeInOutExpo
				Me.ease = AddressOf Me.easeInOutExpo
			Case DialogueriTween.EaseType.easeInCirc
				Me.ease = AddressOf Me.easeInCirc
			Case DialogueriTween.EaseType.easeOutCirc
				Me.ease = AddressOf Me.easeOutCirc
			Case DialogueriTween.EaseType.easeInOutCirc
				Me.ease = AddressOf Me.easeInOutCirc
			Case DialogueriTween.EaseType.linear
				Me.ease = AddressOf Me.linear
			Case DialogueriTween.EaseType.spring
				Me.ease = AddressOf Me.spring
			Case DialogueriTween.EaseType.easeInBounce
				Me.ease = AddressOf Me.easeInBounce
			Case DialogueriTween.EaseType.easeOutBounce
				Me.ease = AddressOf Me.easeOutBounce
			Case DialogueriTween.EaseType.easeInOutBounce
				Me.ease = AddressOf Me.easeInOutBounce
			Case DialogueriTween.EaseType.easeInBack
				Me.ease = AddressOf Me.easeInBack
			Case DialogueriTween.EaseType.easeOutBack
				Me.ease = AddressOf Me.easeOutBack
			Case DialogueriTween.EaseType.easeInOutBack
				Me.ease = AddressOf Me.easeInOutBack
			Case DialogueriTween.EaseType.easeInElastic
				Me.ease = AddressOf Me.easeInElastic
			Case DialogueriTween.EaseType.easeOutElastic
				Me.ease = AddressOf Me.easeOutElastic
			Case DialogueriTween.EaseType.easeInOutElastic
				Me.ease = AddressOf Me.easeInOutElastic
		End Select
	End Sub

	' Token: 0x060047AD RID: 18349 RVA: 0x0025AF78 File Offset: 0x00259378
	Private Sub UpdatePercentage()
		If Me.useRealTime Then
			Me.runningTime += Time.realtimeSinceStartup - Me.lastRealTime
		Else
			Me.runningTime += Time.deltaTime
		End If
		If Me.reverse Then
			Me.percentage = 1F - Me.runningTime / Me.time
		Else
			Me.percentage = Me.runningTime / Me.time
		End If
		Me.lastRealTime = Time.realtimeSinceStartup
	End Sub

	' Token: 0x060047AE RID: 18350 RVA: 0x0025B008 File Offset: 0x00259408
	Private Sub CallBack(callbackType As String)
		If Me.tweenArguments.Contains(callbackType) AndAlso Not Me.tweenArguments.Contains("ischild") Then
			Dim gameObject As GameObject
			If Me.tweenArguments.Contains(callbackType + "target") Then
				gameObject = CType(Me.tweenArguments(callbackType + "target"), GameObject)
			Else
				gameObject = MyBase.gameObject
			End If
			If Me.tweenArguments(callbackType).[GetType]() Is GetType(String) Then
				gameObject.SendMessage(CStr(Me.tweenArguments(callbackType)), Me.tweenArguments(callbackType + "params"), SendMessageOptions.DontRequireReceiver)
			Else
				Global.Debug.LogError("iTween Error: Callback method references must be passed as a String!", Nothing)
				Global.UnityEngine.[Object].Destroy(Me)
			End If
		End If
	End Sub

	' Token: 0x060047AF RID: 18351 RVA: 0x0025B0E4 File Offset: 0x002594E4
	Private Sub Dispose()
		For i As Integer = 0 To DialogueriTween.tweens.Count - 1
			Dim hashtable As Hashtable = CType(DialogueriTween.tweens(i), Hashtable)
			If CStr(hashtable("id")) = Me.id Then
				DialogueriTween.tweens.RemoveAt(i)
				Exit For
			End If
		Next
		Global.UnityEngine.[Object].Destroy(Me)
	End Sub

	' Token: 0x060047B0 RID: 18352 RVA: 0x0025B154 File Offset: 0x00259554
	Private Sub ConflictCheck()
		Dim components As Component() = MyBase.GetComponents(GetType(DialogueriTween))
		For Each dialogueriTween As DialogueriTween In components
			If dialogueriTween.type = "value" Then
				Return
			End If
			If dialogueriTween.isRunning AndAlso dialogueriTween.type = Me.type Then
				If dialogueriTween.method <> Me.method Then
					Return
				End If
				If dialogueriTween.tweenArguments.Count <> Me.tweenArguments.Count Then
					dialogueriTween.Dispose()
					Return
				End If
				Dim enumerator As IDictionaryEnumerator = Me.tweenArguments.GetEnumerator()
				Try
					While enumerator.MoveNext()
						Dim obj As Object = enumerator.Current
						Dim dictionaryEntry As DictionaryEntry = CType(obj, DictionaryEntry)
						If Not dialogueriTween.tweenArguments.Contains(dictionaryEntry.Key) Then
							dialogueriTween.Dispose()
							Return
						End If
						If Not dialogueriTween.tweenArguments(dictionaryEntry.Key).Equals(Me.tweenArguments(dictionaryEntry.Key)) AndAlso CStr(dictionaryEntry.Key) <> "id" Then
							dialogueriTween.Dispose()
							Return
						End If
					End While
				Finally
					Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
					Dim disposable2 As IDisposable = disposable
					If disposable IsNot Nothing Then
						disposable2.Dispose()
					End If
				End Try
				Me.Dispose()
			End If
		Next
	End Sub

	' Token: 0x060047B1 RID: 18353 RVA: 0x0025B2D8 File Offset: 0x002596D8
	Private Sub EnableKinematic()
	End Sub

	' Token: 0x060047B2 RID: 18354 RVA: 0x0025B2DA File Offset: 0x002596DA
	Private Sub DisableKinematic()
	End Sub

	' Token: 0x060047B3 RID: 18355 RVA: 0x0025B2DC File Offset: 0x002596DC
	Private Sub ResumeDelay()
		MyBase.StartCoroutine("TweenDelay")
	End Sub

	' Token: 0x060047B4 RID: 18356 RVA: 0x0025B2EA File Offset: 0x002596EA
	Private Function linear(start As Single, [end] As Single, value As Single) As Single
		Return Mathf.Lerp(start, [end], value)
	End Function

	' Token: 0x060047B5 RID: 18357 RVA: 0x0025B2F4 File Offset: 0x002596F4
	Private Function clerp(start As Single, [end] As Single, value As Single) As Single
		Dim num As Single = 0F
		Dim num2 As Single = 360F
		Dim num3 As Single = Mathf.Abs((num2 - num) / 2F)
		Dim num5 As Single
		If [end] - start < -num3 Then
			Dim num4 As Single = (num2 - start + [end]) * value
			num5 = start + num4
		ElseIf [end] - start > num3 Then
			Dim num4 As Single = -(num2 - [end] + start) * value
			num5 = start + num4
		Else
			num5 = start + ([end] - start) * value
		End If
		Return num5
	End Function

	' Token: 0x060047B6 RID: 18358 RVA: 0x0025B36C File Offset: 0x0025976C
	Private Function spring(start As Single, [end] As Single, value As Single) As Single
		value = Mathf.Clamp01(value)
		value = (Mathf.Sin(value * 3.1415927F * (0.2F + 2.5F * value * value * value)) * Mathf.Pow(1F - value, 2.2F) + value) * (1F + 1.2F * (1F - value))
		Return start + ([end] - start) * value
	End Function

	' Token: 0x060047B7 RID: 18359 RVA: 0x0025B3D0 File Offset: 0x002597D0
	Private Function easeInQuad(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * value * value + start
	End Function

	' Token: 0x060047B8 RID: 18360 RVA: 0x0025B3DE File Offset: 0x002597DE
	Private Function easeOutQuad(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return-[end] * value * (value - 2F) + start
	End Function

	' Token: 0x060047B9 RID: 18361 RVA: 0x0025B3F4 File Offset: 0x002597F4
	Private Function easeInOutQuad(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * value * value + start
		End If
		value -= 1F
		Return-[end] / 2F * (value * (value - 2F) - 1F) + start
	End Function

	' Token: 0x060047BA RID: 18362 RVA: 0x0025B44B File Offset: 0x0025984B
	Private Function easeInCubic(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * value * value * value + start
	End Function

	' Token: 0x060047BB RID: 18363 RVA: 0x0025B45B File Offset: 0x0025985B
	Private Function easeOutCubic(start As Single, [end] As Single, value As Single) As Single
		value -= 1F
		[end] -= start
		Return [end] * (value * value * value + 1F) + start
	End Function

	' Token: 0x060047BC RID: 18364 RVA: 0x0025B47C File Offset: 0x0025987C
	Private Function easeInOutCubic(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * value * value * value + start
		End If
		value -= 2F
		Return [end] / 2F * (value * value * value + 2F) + start
	End Function

	' Token: 0x060047BD RID: 18365 RVA: 0x0025B4D0 File Offset: 0x002598D0
	Private Function easeInQuart(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * value * value * value * value + start
	End Function

	' Token: 0x060047BE RID: 18366 RVA: 0x0025B4E2 File Offset: 0x002598E2
	Private Function easeOutQuart(start As Single, [end] As Single, value As Single) As Single
		value -= 1F
		[end] -= start
		Return-[end] * (value * value * value * value - 1F) + start
	End Function

	' Token: 0x060047BF RID: 18367 RVA: 0x0025B504 File Offset: 0x00259904
	Private Function easeInOutQuart(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * value * value * value * value + start
		End If
		value -= 2F
		Return-[end] / 2F * (value * value * value * value - 2F) + start
	End Function

	' Token: 0x060047C0 RID: 18368 RVA: 0x0025B55D File Offset: 0x0025995D
	Private Function easeInQuint(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * value * value * value * value * value + start
	End Function

	' Token: 0x060047C1 RID: 18369 RVA: 0x0025B571 File Offset: 0x00259971
	Private Function easeOutQuint(start As Single, [end] As Single, value As Single) As Single
		value -= 1F
		[end] -= start
		Return [end] * (value * value * value * value * value + 1F) + start
	End Function

	' Token: 0x060047C2 RID: 18370 RVA: 0x0025B594 File Offset: 0x00259994
	Private Function easeInOutQuint(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * value * value * value * value * value + start
		End If
		value -= 2F
		Return [end] / 2F * (value * value * value * value * value + 2F) + start
	End Function

	' Token: 0x060047C3 RID: 18371 RVA: 0x0025B5F0 File Offset: 0x002599F0
	Private Function easeInSine(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return-[end] * Mathf.Cos(value / 1F * 1.5707964F) + [end] + start
	End Function

	' Token: 0x060047C4 RID: 18372 RVA: 0x0025B610 File Offset: 0x00259A10
	Private Function easeOutSine(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * Mathf.Sin(value / 1F * 1.5707964F) + start
	End Function

	' Token: 0x060047C5 RID: 18373 RVA: 0x0025B62D File Offset: 0x00259A2D
	Private Function easeInOutSine(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return-[end] / 2F * (Mathf.Cos(3.1415927F * value / 1F) - 1F) + start
	End Function

	' Token: 0x060047C6 RID: 18374 RVA: 0x0025B657 File Offset: 0x00259A57
	Private Function easeInExpo(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * Mathf.Pow(2F, 10F * (value / 1F - 1F)) + start
	End Function

	' Token: 0x060047C7 RID: 18375 RVA: 0x0025B67F File Offset: 0x00259A7F
	Private Function easeOutExpo(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return [end] * (-Mathf.Pow(2F, -10F * value / 1F) + 1F) + start
	End Function

	' Token: 0x060047C8 RID: 18376 RVA: 0x0025B6A8 File Offset: 0x00259AA8
	Private Function easeInOutExpo(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return [end] / 2F * Mathf.Pow(2F, 10F * (value - 1F)) + start
		End If
		value -= 1F
		Return [end] / 2F * (-Mathf.Pow(2F, -10F * value) + 2F) + start
	End Function

	' Token: 0x060047C9 RID: 18377 RVA: 0x0025B71B File Offset: 0x00259B1B
	Private Function easeInCirc(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Return-[end] * (Mathf.Sqrt(1F - value * value) - 1F) + start
	End Function

	' Token: 0x060047CA RID: 18378 RVA: 0x0025B73B File Offset: 0x00259B3B
	Private Function easeOutCirc(start As Single, [end] As Single, value As Single) As Single
		value -= 1F
		[end] -= start
		Return [end] * Mathf.Sqrt(1F - value * value) + start
	End Function

	' Token: 0x060047CB RID: 18379 RVA: 0x0025B760 File Offset: 0x00259B60
	Private Function easeInOutCirc(start As Single, [end] As Single, value As Single) As Single
		value /= 0.5F
		[end] -= start
		If value < 1F Then
			Return-[end] / 2F * (Mathf.Sqrt(1F - value * value) - 1F) + start
		End If
		value -= 2F
		Return [end] / 2F * (Mathf.Sqrt(1F - value * value) + 1F) + start
	End Function

	' Token: 0x060047CC RID: 18380 RVA: 0x0025B7D0 File Offset: 0x00259BD0
	Private Function easeInBounce(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		Return [end] - Me.easeOutBounce(0F, [end], num - value) + start
	End Function

	' Token: 0x060047CD RID: 18381 RVA: 0x0025B7FC File Offset: 0x00259BFC
	Private Function easeOutBounce(start As Single, [end] As Single, value As Single) As Single
		value /= 1F
		[end] -= start
		If value < 0.36363637F Then
			Return [end] * (7.5625F * value * value) + start
		End If
		If value < 0.72727275F Then
			value -= 0.54545456F
			Return [end] * (7.5625F * value * value + 0.75F) + start
		End If
		If CDbl(value) < 0.9090909090909091 Then
			value -= 0.8181818F
			Return [end] * (7.5625F * value * value + 0.9375F) + start
		End If
		value -= 0.95454544F
		Return [end] * (7.5625F * value * value + 0.984375F) + start
	End Function

	' Token: 0x060047CE RID: 18382 RVA: 0x0025B8A4 File Offset: 0x00259CA4
	Private Function easeInOutBounce(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		If value < num / 2F Then
			Return Me.easeInBounce(0F, [end], value * 2F) * 0.5F + start
		End If
		Return Me.easeOutBounce(0F, [end], value * 2F - num) * 0.5F + [end] * 0.5F + start
	End Function

	' Token: 0x060047CF RID: 18383 RVA: 0x0025B90C File Offset: 0x00259D0C
	Private Function easeInBack(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		value /= 1F
		Dim num As Single = 1.70158F
		Return [end] * value * value * ((num + 1F) * value - num) + start
	End Function

	' Token: 0x060047D0 RID: 18384 RVA: 0x0025B940 File Offset: 0x00259D40
	Private Function easeOutBack(start As Single, [end] As Single, value As Single) As Single
		Dim num As Single = 1.70158F
		[end] -= start
		value = value / 1F - 1F
		Return [end] * (value * value * ((num + 1F) * value + num) + 1F) + start
	End Function

	' Token: 0x060047D1 RID: 18385 RVA: 0x0025B980 File Offset: 0x00259D80
	Private Function easeInOutBack(start As Single, [end] As Single, value As Single) As Single
		Dim num As Single = 1.70158F
		[end] -= start
		value /= 0.5F
		If value < 1F Then
			num *= 1.525F
			Return [end] / 2F * (value * value * ((num + 1F) * value - num)) + start
		End If
		value -= 2F
		num *= 1.525F
		Return [end] / 2F * (value * value * ((num + 1F) * value + num) + 2F) + start
	End Function

	' Token: 0x060047D2 RID: 18386 RVA: 0x0025BA00 File Offset: 0x00259E00
	Private Function punch(amplitude As Single, value As Single) As Single
		If value = 0F Then
			Return 0F
		End If
		If value = 1F Then
			Return 0F
		End If
		Dim num As Single = 0.3F
		Dim num2 As Single = num / 6.2831855F * Mathf.Asin(0F)
		Return amplitude * Mathf.Pow(2F, -10F * value) * Mathf.Sin((value * 1F - num2) * 6.2831855F / num)
	End Function

	' Token: 0x060047D3 RID: 18387 RVA: 0x0025BA78 File Offset: 0x00259E78
	Private Function easeInElastic(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		Dim num2 As Single = num * 0.3F
		Dim num3 As Single = 0F
		If value = 0F Then
			Return start
		End If
		Dim num4 As Single = value / num
		value = num4
		If num4 = 1F Then
			Return start + [end]
		End If
		Dim num5 As Single
		If num3 = 0F OrElse num3 < Mathf.Abs([end]) Then
			num3 = [end]
			num5 = num2 / 4F
		Else
			num5 = num2 / 6.2831855F * Mathf.Asin([end] / num3)
		End If
		Dim num6 As Single = num3
		Dim num7 As Single = 2F
		Dim num8 As Single = 10F
		Dim num9 As Single = value - 1F
		value = num9
		Return-(num6 * Mathf.Pow(num7, num8 * num9) * Mathf.Sin((value * num - num5) * 6.2831855F / num2)) + start
	End Function

	' Token: 0x060047D4 RID: 18388 RVA: 0x0025BB30 File Offset: 0x00259F30
	Private Function easeOutElastic(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		Dim num2 As Single = num * 0.3F
		Dim num3 As Single = 0F
		If value = 0F Then
			Return start
		End If
		Dim num4 As Single = value / num
		value = num4
		If num4 = 1F Then
			Return start + [end]
		End If
		Dim num5 As Single
		If num3 = 0F OrElse num3 < Mathf.Abs([end]) Then
			num3 = [end]
			num5 = num2 / 4F
		Else
			num5 = num2 / 6.2831855F * Mathf.Asin([end] / num3)
		End If
		Return num3 * Mathf.Pow(2F, -10F * value) * Mathf.Sin((value * num - num5) * 6.2831855F / num2) + [end] + start
	End Function

	' Token: 0x060047D5 RID: 18389 RVA: 0x0025BBE0 File Offset: 0x00259FE0
	Private Function easeInOutElastic(start As Single, [end] As Single, value As Single) As Single
		[end] -= start
		Dim num As Single = 1F
		Dim num2 As Single = num * 0.3F
		Dim num3 As Single = 0F
		If value = 0F Then
			Return start
		End If
		Dim num4 As Single = value / (num / 2F)
		value = num4
		If num4 = 2F Then
			Return start + [end]
		End If
		Dim num5 As Single
		If num3 = 0F OrElse num3 < Mathf.Abs([end]) Then
			num3 = [end]
			num5 = num2 / 4F
		Else
			num5 = num2 / 6.2831855F * Mathf.Asin([end] / num3)
		End If
		If value < 1F Then
			Dim num6 As Single = -0.5F
			Dim num7 As Single = num3
			Dim num8 As Single = 2F
			Dim num9 As Single = 10F
			Dim num10 As Single = value - 1F
			value = num10
			Return num6 * (num7 * Mathf.Pow(num8, num9 * num10) * Mathf.Sin((value * num - num5) * 6.2831855F / num2)) + start
		End If
		Dim num11 As Single = num3
		Dim num12 As Single = 2F
		Dim num13 As Single = -10F
		Dim num14 As Single = value - 1F
		value = num14
		Return num11 * Mathf.Pow(num12, num13 * num14) * Mathf.Sin((value * num - num5) * 6.2831855F / num2) * 0.5F + [end] + start
	End Function

	' Token: 0x04004CBA RID: 19642
	Public Shared tweens As ArrayList = New ArrayList()

	' Token: 0x04004CBB RID: 19643
	Private Shared cameraFade As GameObject

	' Token: 0x04004CBC RID: 19644
	Public id As String

	' Token: 0x04004CBD RID: 19645
	Public type As String

	' Token: 0x04004CBE RID: 19646
	Public method As String

	' Token: 0x04004CBF RID: 19647
	Public easeType As DialogueriTween.EaseType

	' Token: 0x04004CC0 RID: 19648
	Public time As Single

	' Token: 0x04004CC1 RID: 19649
	Public delay As Single

	' Token: 0x04004CC2 RID: 19650
	Public loopType As DialogueriTween.LoopType

	' Token: 0x04004CC3 RID: 19651
	Public isRunning As Boolean

	' Token: 0x04004CC4 RID: 19652
	Public isPaused As Boolean

	' Token: 0x04004CC5 RID: 19653
	Public _name As String

	' Token: 0x04004CC6 RID: 19654
	Private runningTime As Single

	' Token: 0x04004CC7 RID: 19655
	Private percentage As Single

	' Token: 0x04004CC8 RID: 19656
	Private delayStarted As Single

	' Token: 0x04004CC9 RID: 19657
	Private kinematic As Boolean

	' Token: 0x04004CCA RID: 19658
	Private isLocal As Boolean

	' Token: 0x04004CCB RID: 19659
	Private [loop] As Boolean

	' Token: 0x04004CCC RID: 19660
	Private reverse As Boolean

	' Token: 0x04004CCD RID: 19661
	Private wasPaused As Boolean

	' Token: 0x04004CCE RID: 19662
	Private physics As Boolean

	' Token: 0x04004CCF RID: 19663
	Private tweenArguments As Hashtable

	' Token: 0x04004CD0 RID: 19664
	Private space As Space

	' Token: 0x04004CD1 RID: 19665
	Private ease As DialogueriTween.EasingFunction

	' Token: 0x04004CD2 RID: 19666
	Private apply As DialogueriTween.ApplyTween

	' Token: 0x04004CD3 RID: 19667
	Private audioSource As AudioSource

	' Token: 0x04004CD4 RID: 19668
	Private vector3s As Vector3()

	' Token: 0x04004CD5 RID: 19669
	Private vector2s As Vector2()

	' Token: 0x04004CD6 RID: 19670
	Private colors As Color(,)

	' Token: 0x04004CD7 RID: 19671
	Private floats As Single()

	' Token: 0x04004CD8 RID: 19672
	Private rects As Rect()

	' Token: 0x04004CD9 RID: 19673
	Private path As DialogueriTween.CRSpline

	' Token: 0x04004CDA RID: 19674
	Private preUpdate As Vector3

	' Token: 0x04004CDB RID: 19675
	Private postUpdate As Vector3

	' Token: 0x04004CDC RID: 19676
	Private namedcolorvalue As DialogueriTween.NamedValueColor

	' Token: 0x04004CDD RID: 19677
	Private lastRealTime As Single

	' Token: 0x04004CDE RID: 19678
	Private useRealTime As Boolean

	' Token: 0x02000B84 RID: 2948
	' (Invoke) Token: 0x060047D8 RID: 18392
	Private Delegate Function EasingFunction(start As Single, [end] As Single, value As Single) As Single

	' Token: 0x02000B85 RID: 2949
	' (Invoke) Token: 0x060047DC RID: 18396
	Private Delegate Sub ApplyTween()

	' Token: 0x02000B86 RID: 2950
	Public Enum EaseType
		' Token: 0x04004CE1 RID: 19681
		easeInQuad
		' Token: 0x04004CE2 RID: 19682
		easeOutQuad
		' Token: 0x04004CE3 RID: 19683
		easeInOutQuad
		' Token: 0x04004CE4 RID: 19684
		easeInCubic
		' Token: 0x04004CE5 RID: 19685
		easeOutCubic
		' Token: 0x04004CE6 RID: 19686
		easeInOutCubic
		' Token: 0x04004CE7 RID: 19687
		easeInQuart
		' Token: 0x04004CE8 RID: 19688
		easeOutQuart
		' Token: 0x04004CE9 RID: 19689
		easeInOutQuart
		' Token: 0x04004CEA RID: 19690
		easeInQuint
		' Token: 0x04004CEB RID: 19691
		easeOutQuint
		' Token: 0x04004CEC RID: 19692
		easeInOutQuint
		' Token: 0x04004CED RID: 19693
		easeInSine
		' Token: 0x04004CEE RID: 19694
		easeOutSine
		' Token: 0x04004CEF RID: 19695
		easeInOutSine
		' Token: 0x04004CF0 RID: 19696
		easeInExpo
		' Token: 0x04004CF1 RID: 19697
		easeOutExpo
		' Token: 0x04004CF2 RID: 19698
		easeInOutExpo
		' Token: 0x04004CF3 RID: 19699
		easeInCirc
		' Token: 0x04004CF4 RID: 19700
		easeOutCirc
		' Token: 0x04004CF5 RID: 19701
		easeInOutCirc
		' Token: 0x04004CF6 RID: 19702
		linear
		' Token: 0x04004CF7 RID: 19703
		spring
		' Token: 0x04004CF8 RID: 19704
		easeInBounce
		' Token: 0x04004CF9 RID: 19705
		easeOutBounce
		' Token: 0x04004CFA RID: 19706
		easeInOutBounce
		' Token: 0x04004CFB RID: 19707
		easeInBack
		' Token: 0x04004CFC RID: 19708
		easeOutBack
		' Token: 0x04004CFD RID: 19709
		easeInOutBack
		' Token: 0x04004CFE RID: 19710
		easeInElastic
		' Token: 0x04004CFF RID: 19711
		easeOutElastic
		' Token: 0x04004D00 RID: 19712
		easeInOutElastic
		' Token: 0x04004D01 RID: 19713
		punch
	End Enum

	' Token: 0x02000B87 RID: 2951
	Public Enum LoopType
		' Token: 0x04004D03 RID: 19715
		none
		' Token: 0x04004D04 RID: 19716
		[loop]
		' Token: 0x04004D05 RID: 19717
		pingPong
	End Enum

	' Token: 0x02000B88 RID: 2952
	Public Enum NamedValueColor
		' Token: 0x04004D07 RID: 19719
		_Color
		' Token: 0x04004D08 RID: 19720
		_SpecColor
		' Token: 0x04004D09 RID: 19721
		_Emission
		' Token: 0x04004D0A RID: 19722
		_ReflectColor
	End Enum

	' Token: 0x02000B89 RID: 2953
	Public NotInheritable Class Defaults
		' Token: 0x04004D0B RID: 19723
		Public Shared time As Single = 1F

		' Token: 0x04004D0C RID: 19724
		Public Shared delay As Single = 0F

		' Token: 0x04004D0D RID: 19725
		Public Shared namedColorValue As DialogueriTween.NamedValueColor = DialogueriTween.NamedValueColor._Color

		' Token: 0x04004D0E RID: 19726
		Public Shared loopType As DialogueriTween.LoopType = DialogueriTween.LoopType.none

		' Token: 0x04004D0F RID: 19727
		Public Shared easeType As DialogueriTween.EaseType = DialogueriTween.EaseType.easeOutExpo

		' Token: 0x04004D10 RID: 19728
		Public Shared lookSpeed As Single = 3F

		' Token: 0x04004D11 RID: 19729
		Public Shared isLocal As Boolean = False

		' Token: 0x04004D12 RID: 19730
		Public Shared space As Space = Space.Self

		' Token: 0x04004D13 RID: 19731
		Public Shared orientToPath As Boolean = False

		' Token: 0x04004D14 RID: 19732
		Public Shared color As Color = Color.white

		' Token: 0x04004D15 RID: 19733
		Public Shared updateTimePercentage As Single = 0.05F

		' Token: 0x04004D16 RID: 19734
		Public Shared updateTime As Single = 1F * DialogueriTween.Defaults.updateTimePercentage

		' Token: 0x04004D17 RID: 19735
		Public Shared cameraFadeDepth As Integer = 999999

		' Token: 0x04004D18 RID: 19736
		Public Shared lookAhead As Single = 0.05F

		' Token: 0x04004D19 RID: 19737
		Public Shared useRealTime As Boolean = False

		' Token: 0x04004D1A RID: 19738
		Public Shared up As Vector3 = Vector3.up
	End Class

	' Token: 0x02000B8A RID: 2954
	Private Class CRSpline
		' Token: 0x060047E0 RID: 18400 RVA: 0x0025BD8C File Offset: 0x0025A18C
		Public Sub New(ParamArray pts As Vector3())
			Me.pts = New Vector3(pts.Length - 1) {}
			Array.Copy(pts, Me.pts, pts.Length)
		End Sub

		' Token: 0x060047E1 RID: 18401 RVA: 0x0025BDB4 File Offset: 0x0025A1B4
		Public Function Interp(t As Single) As Vector3
			Dim num As Integer = Me.pts.Length - 3
			Dim num2 As Integer = Mathf.Min(Mathf.FloorToInt(t * CSng(num)), num - 1)
			Dim num3 As Single = t * CSng(num) - CSng(num2)
			Dim vector As Vector3 = Me.pts(num2)
			Dim vector2 As Vector3 = Me.pts(num2 + 1)
			Dim vector3 As Vector3 = Me.pts(num2 + 2)
			Dim vector4 As Vector3 = Me.pts(num2 + 3)
			Return 0.5F * ((-vector + 3F * vector2 - 3F * vector3 + vector4) * (num3 * num3 * num3) + (2F * vector - 5F * vector2 + 4F * vector3 - vector4) * (num3 * num3) + (-vector + vector3) * num3 + 2F * vector2)
		End Function

		' Token: 0x04004D1B RID: 19739
		Public pts As Vector3()
	End Class
End Class
