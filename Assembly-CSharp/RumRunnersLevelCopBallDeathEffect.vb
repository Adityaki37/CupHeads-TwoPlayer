Imports System
Imports System.Collections.Generic
Imports UnityEngine

' Token: 0x0200078A RID: 1930
Public Class RumRunnersLevelCopBallDeathEffect
	Inherits Effect

	' Token: 0x06002AA6 RID: 10918 RVA: 0x0018E9FC File Offset: 0x0018CDFC
	Public Overrides Sub Initialize(position As Vector3, scale As Vector3, randomR As Boolean)
		Dim i As Integer = Global.UnityEngine.Random.Range(0, MyBase.animator.GetInteger("Count"))
		MyBase.animator.SetInteger("Effect", i)
		Dim transform As Transform = MyBase.transform
		transform.position = position
		transform.localScale = scale
		If randomR Then
			transform.eulerAngles = New Vector3(0F, 0F, CSng((Global.UnityEngine.Random.Range(0, 8) * 45)))
		End If
		Dim list As List(Of Integer) = New List(Of Integer)()
		i = 0
		While i < 5
			list.Add(i)
			i += 1
		End While
		list.RemoveAt(Global.UnityEngine.Random.Range(0, list.Count))
		list.RemoveAt(Global.UnityEngine.Random.Range(0, list.Count))
		i = 0
		While i < 3
			Me.shrapnel(i).Play(list(i).ToString())
			Me.shrapnel(i).transform.parent = Nothing
			i += 1
		End While
	End Sub

	' Token: 0x0400336B RID: 13163
	<SerializeField()>
	Private shrapnel As Animator()
End Class
