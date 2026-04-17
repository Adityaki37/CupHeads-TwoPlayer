Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000475 RID: 1141
Public MustInherit Class AbstractLevelEntity
	Inherits AbstractCollidableObject

	' Token: 0x170002BB RID: 699
	' (get) Token: 0x0600117F RID: 4479 RVA: 0x0000880D File Offset: 0x00006C0D
	Public ReadOnly Property canParry As Boolean
		Get
			Return Me._canParry
		End Get
	End Property

	' Token: 0x06001180 RID: 4480 RVA: 0x00008815 File Offset: 0x00006C15
	Public Overridable Sub OnParry(player As AbstractPlayerController)
	End Sub

	' Token: 0x06001181 RID: 4481 RVA: 0x00008818 File Offset: 0x00006C18
	Protected Iterator Function flash_cr(start As Color, [end] As Color, time As Single, Optional onComplete As Action = Nothing) As IEnumerator
		Dim renderer As SpriteRenderer = MyBase.GetComponent(Of SpriteRenderer)()
		renderer.color = start
		Dim t As Single = 0F
		While t < time
			Dim val As Single = t / time
			renderer.color = Color.Lerp(start, [end], val)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		renderer.color = [end]
		If onComplete IsNot Nothing Then
			onComplete()
		End If
		Return
	End Function

	' Token: 0x06001182 RID: 4482 RVA: 0x00008850 File Offset: 0x00006C50
	Protected Iterator Function dieFlash_cr() As IEnumerator
		For i As Integer = 0 To 4 - 1
			Yield MyBase.StartCoroutine(Me.flash_cr(Color.red, Color.black, 0.3F, Nothing))
			Yield CupheadTime.WaitForSeconds(Me, 0.2F)
		Next
		Return
	End Function

	' Token: 0x04001B0F RID: 6927
	Protected Const DIE_FLASH_TIME As Single = 0.3F

	' Token: 0x04001B10 RID: 6928
	Protected Const DIE_FLASH_DELAY As Single = 0.2F

	' Token: 0x04001B11 RID: 6929
	Protected Const DIE_FLASH_LOOPS As Integer = 4

	' Token: 0x04001B12 RID: 6930
	<SerializeField()>
	Protected _canParry As Boolean
End Class
