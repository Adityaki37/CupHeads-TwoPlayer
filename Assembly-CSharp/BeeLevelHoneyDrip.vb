Imports System
Imports UnityEngine

' Token: 0x02000514 RID: 1300
Public Class BeeLevelHoneyDrip
	Inherits AbstractMonoBehaviour

	' Token: 0x0600172B RID: 5931 RVA: 0x000D0134 File Offset: 0x000CE534
	Public Function Create() As BeeLevelHoneyDrip
		Return Global.UnityEngine.[Object].Instantiate(Of BeeLevelHoneyDrip)(Me)
	End Function

	' Token: 0x0600172C RID: 5932 RVA: 0x000D014C File Offset: 0x000CE54C
	Private Function Create(number As Integer) As BeeLevelHoneyDrip
		Dim beeLevelHoneyDrip As BeeLevelHoneyDrip = Me.Create()
		beeLevelHoneyDrip.i = number
		Return beeLevelHoneyDrip
	End Function

	' Token: 0x0600172D RID: 5933 RVA: 0x000D0168 File Offset: 0x000CE568
	Protected Overrides Sub Awake()
		MyBase.Awake()
		MyBase.GetComponent(Of Animator)().SetInteger("I", Global.UnityEngine.Random.Range(0, 6))
		MyBase.transform.SetParent(Camera.main.transform)
		MyBase.transform.SetLocalPosition(New Single?(CSng(Global.UnityEngine.Random.Range(-540, 540))), New Single?(415F), New Single?(100F))
		MyBase.transform.SetParent(Nothing)
		AudioManager.Play("bee_honey_glug_sweet")
	End Sub

	' Token: 0x0600172E RID: 5934 RVA: 0x000D01F1 File Offset: 0x000CE5F1
	Private Sub OnAnimationEnd()
		If Me.i < 4 AndAlso Global.UnityEngine.Random.value < 0.5F Then
			Me.Create(Me.i + 1)
		End If
		Global.UnityEngine.[Object].Destroy(MyBase.gameObject)
	End Sub

	' Token: 0x0400206A RID: 8298
	Private Const START_Y As Single = 415F

	' Token: 0x0400206B RID: 8299
	Private Const MAX As Integer = 5

	' Token: 0x0400206C RID: 8300
	Private i As Integer
End Class
