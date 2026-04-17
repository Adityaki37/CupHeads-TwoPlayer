Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200043D RID: 1085
Public Class ScrollingGameObject
	Inherits AbstractMonoBehaviour

	' Token: 0x06000FF6 RID: 4086 RVA: 0x0009DE60 File Offset: 0x0009C260
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Dim gameObject As GameObject = New GameObject("Container")
		gameObject.transform.SetParent(MyBase.transform)
		If Me.resetTransforms Then
			MyBase.transform.ResetLocalTransforms()
		End If
		Dim enumerator As IEnumerator = MyBase.transform.GetEnumerator()
		Try
			While enumerator.MoveNext()
				Dim obj As Object = enumerator.Current
				Dim transform As Transform = CType(obj, Transform)
				transform.SetParent(gameObject.transform)
			End While
		Finally
			Dim disposable As IDisposable = TryCast(enumerator, IDisposable)
			Dim disposable2 As IDisposable = disposable
			If disposable IsNot Nothing Then
				disposable2.Dispose()
			End If
		End Try
		Dim gameObject2 As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(gameObject.gameObject)
		gameObject2.transform.SetParent(MyBase.transform)
		gameObject2.transform.ResetLocalTransforms()
		gameObject2.transform.SetLocalPosition(New Single?(CSng(Me.size)), New Single?(0F), New Single?(0F))
		Dim gameObject3 As GameObject = Global.UnityEngine.[Object].Instantiate(Of GameObject)(gameObject2)
		gameObject3.transform.SetParent(MyBase.transform)
		gameObject3.transform.SetLocalPosition(New Single?(CSng((-CSng(Me.size)))), New Single?(0F), New Single?(0F))
	End Sub

	' Token: 0x06000FF7 RID: 4087 RVA: 0x0009DFA4 File Offset: 0x0009C3A4
	Private Sub Update()
		Dim localPosition As Vector3 = MyBase.transform.localPosition
		If localPosition.x <= CSng((-CSng(Me.size))) Then
			localPosition.x += CSng(Me.size)
		End If
		If localPosition.x >= 1280F Then
			localPosition.x -= CSng(Me.size)
		End If
		localPosition.x -= CSng(If((Not Me.negativeDirection), 1, (-1))) * Me.speed * CupheadTime.Delta
		MyBase.transform.localPosition = localPosition
	End Sub

	' Token: 0x04001987 RID: 6535
	Public axis As ScrollingGameObject.Axis

	' Token: 0x04001988 RID: 6536
	<SerializeField()>
	Private negativeDirection As Boolean

	' Token: 0x04001989 RID: 6537
	<Range(0F, 500F)>
	<SerializeField()>
	Private speed As Single

	' Token: 0x0400198A RID: 6538
	<SerializeField()>
	Private size As Integer = 1280

	' Token: 0x0400198B RID: 6539
	<SerializeField()>
	Private resetTransforms As Boolean = True

	' Token: 0x0200043E RID: 1086
	Public Enum Axis
		' Token: 0x0400198D RID: 6541
		X
		' Token: 0x0400198E RID: 6542
		Y
	End Enum
End Class
