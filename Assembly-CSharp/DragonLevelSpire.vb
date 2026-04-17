Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x02000600 RID: 1536
Public Class DragonLevelSpire
	Inherits AbstractPausableComponent

	' Token: 0x06001E8A RID: 7818 RVA: 0x001197BE File Offset: 0x00117BBE
	Private Sub Start()
		Me.helper = MyBase.GetComponent(Of AnimationHelper)()
		Me.helper.Speed = 0F
		Me.fadeTime = 3F
		Me.replacementSprite.enabled = False
	End Sub

	' Token: 0x06001E8B RID: 7819 RVA: 0x001197F3 File Offset: 0x00117BF3
	Private Sub Update()
		Me.helper.Speed = DragonLevel.SPEED
	End Sub

	' Token: 0x06001E8C RID: 7820 RVA: 0x00119805 File Offset: 0x00117C05
	Public Sub StartChange()
		MyBase.StartCoroutine(Me.change_cr())
	End Sub

	' Token: 0x06001E8D RID: 7821 RVA: 0x00119814 File Offset: 0x00117C14
	Private Iterator Function change_cr() As IEnumerator
		Dim t As Single = 0F
		While t < Me.fadeTime
			Me.replacementSprite.enabled = True
			Me.replacementSprite.color = New Color(1F, 1F, 1F, t / Me.fadeTime)
			t += CupheadTime.Delta
			Yield Nothing
		End While
		Me.replacementSprite.color = New Color(1F, 1F, 1F, 1F)
		Yield Nothing
		Return
	End Function

	' Token: 0x04002763 RID: 10083
	Private helper As AnimationHelper

	' Token: 0x04002764 RID: 10084
	<SerializeField()>
	Private replacementSprite As SpriteRenderer

	' Token: 0x04002765 RID: 10085
	Private fadeTime As Single
End Class
