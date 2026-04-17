Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000A34 RID: 2612
Public Class LevelPlayerParryAnimator
	Inherits AbstractMonoBehaviour

	' Token: 0x06003E1D RID: 15901 RVA: 0x00223480 File Offset: 0x00221880
	Protected Overrides Sub Awake()
		MyBase.Awake()
		Me.r = MyBase.GetComponent(Of SpriteRenderer)()
		For Each sprite As Sprite In Me.sprites
			sprite.name = sprite.name.Replace("_pink", String.Empty)
		Next
	End Sub

	' Token: 0x06003E1E RID: 15902 RVA: 0x002234DC File Offset: 0x002218DC
	Private Sub [Set]()
		For Each sprite As Sprite In Me.sprites
			If sprite.name.Contains(Me.r.sprite.name) Then
				Me.r.sprite = sprite
				Return
			End If
		Next
	End Sub

	' Token: 0x06003E1F RID: 15903 RVA: 0x00223535 File Offset: 0x00221935
	Public Sub StartSet()
		MyBase.StartCoroutine(Me.set_cr())
		MyBase.StartCoroutine(Me.setLate_cr())
	End Sub

	' Token: 0x06003E20 RID: 15904 RVA: 0x00223551 File Offset: 0x00221951
	Public Sub StopSet()
		Me.StopAllCoroutines()
	End Sub

	' Token: 0x06003E21 RID: 15905 RVA: 0x0022355C File Offset: 0x0022195C
	Private Iterator Function set_cr() As IEnumerator
		While True
			Me.[Set]()
			Yield Nothing
		End While
		Return
	End Function

	' Token: 0x06003E22 RID: 15906 RVA: 0x00223578 File Offset: 0x00221978
	Private Iterator Function setLate_cr() As IEnumerator
		While True
			Me.[Set]()
			Yield New WaitForEndOfFrame()
		End While
		Return
	End Function

	' Token: 0x04004557 RID: 17751
	<SerializeField()>
	Private sprites As Sprite()

	' Token: 0x04004558 RID: 17752
	Private r As SpriteRenderer
End Class
