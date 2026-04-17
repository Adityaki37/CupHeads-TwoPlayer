Imports System
Imports UnityEngine

' Token: 0x020005F4 RID: 1524
Public Class DragonLevelLightning
	Inherits AbstractPausableComponent

	' Token: 0x06001E5E RID: 7774 RVA: 0x00118430 File Offset: 0x00116830
	Public Sub PlayLightning()
		Dim num As Integer = Global.UnityEngine.Random.Range(1, 11)
		MyBase.animator.SetInteger("LightningID", num)
		MyBase.animator.SetTrigger("Continue")
		num = Global.UnityEngine.Random.Range(0, Me.layerOrder.Length)
		Me.spriteRenderer.sortingOrder = Me.layerOrder(num)
		AudioManager.Play("level_dragon_amb_thunder")
	End Sub

	' Token: 0x0400273C RID: 10044
	Private layerOrder As Integer() = New Integer() { 91, 93, 95 }

	' Token: 0x0400273D RID: 10045
	<SerializeField()>
	Private spriteRenderer As SpriteRenderer
End Class
