Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x0200078C RID: 1932
Public Class RumRunnersLevelFullscreenDirtFX
	Inherits Effect

	' Token: 0x06002AB2 RID: 10930 RVA: 0x0018EE70 File Offset: 0x0018D270
	Public Overrides Sub Initialize(position As Vector3, scale As Vector3, randomR As Boolean)
		MyBase.Initialize(position, scale, randomR)
		Dim num As Integer = MyBase.animator.GetInteger("Effect")
		While num = RumRunnersLevelFullscreenDirtFX.PreviousEffectA OrElse num = RumRunnersLevelFullscreenDirtFX.PreviousEffectB
			num = Global.UnityEngine.Random.Range(0, MyBase.animator.GetInteger("Count"))
			MyBase.animator.SetInteger("Effect", num)
		End While
		RumRunnersLevelFullscreenDirtFX.PreviousEffectB = RumRunnersLevelFullscreenDirtFX.PreviousEffectA
		RumRunnersLevelFullscreenDirtFX.PreviousEffectA = num
		MyBase.animator.Update(0F)
		Dim y As Single = MyBase.GetComponent(Of SpriteRenderer)().sprite.bounds.size.y
		If num = 0 OrElse num = 1 Then
			MyBase.StartCoroutine(Me.fall_cr(Me.loopDirtSpeed, y))
		Else
			Dim currentClipLength As Single = MyBase.animator.GetCurrentClipLength(0)
			If currentClipLength = 0F Then
				Me.OnEffectComplete()
			End If
			MyBase.animator.Update(0F)
			Dim num2 As Single = (887.79285F + y) / currentClipLength
			MyBase.StartCoroutine(Me.fall_cr(num2, y))
		End If
	End Sub

	' Token: 0x06002AB3 RID: 10931 RVA: 0x0018EF90 File Offset: 0x0018D390
	Private Iterator Function fall_cr(speed As Single, spriteHeight As Single) As IEnumerator
		While MyBase.transform.position.y > -360F - spriteHeight - 100F
			Yield Nothing
			Dim position As Vector3 = MyBase.transform.position
			position.y -= speed * CupheadTime.Delta
			MyBase.transform.position = position
		End While
		Me.OnEffectComplete()
		Return
	End Function

	' Token: 0x04003370 RID: 13168
	Private Shared PreviousEffectA As Integer = -1

	' Token: 0x04003371 RID: 13169
	Private Shared PreviousEffectB As Integer = -1

	' Token: 0x04003372 RID: 13170
	<SerializeField()>
	Private loopDirtSpeed As Single
End Class
