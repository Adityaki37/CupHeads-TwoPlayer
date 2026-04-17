Imports System
Imports UnityEngine

' Token: 0x020007C0 RID: 1984
Public Class SaltbakerLevelBGTornado
	Inherits AbstractMonoBehaviour

	' Token: 0x06002CE5 RID: 11493 RVA: 0x001A6FB8 File Offset: 0x001A53B8
	Private Sub Start()
		Me.t = Global.UnityEngine.Random.Range(0F, 3.1415927F)
		Me.rend.material.SetFloat("_BlurAmount", Me.blurAmount * 5F)
		Me.rend.material.SetFloat("_BlurLerp", Me.blurAmount * 5F)
	End Sub

	' Token: 0x06002CE6 RID: 11494 RVA: 0x001A701C File Offset: 0x001A541C
	Private Sub Update()
		Me.t += CupheadTime.Delta
		MyBase.transform.GetChild(0).localPosition = New Vector3(Mathf.Sin(Me.t * Me.moveSpeed) * Me.moveRange, 0F)
	End Sub

	' Token: 0x0400355A RID: 13658
	<SerializeField()>
	Private moveRange As Single

	' Token: 0x0400355B RID: 13659
	<SerializeField()>
	Private moveSpeed As Single

	' Token: 0x0400355C RID: 13660
	Private t As Single

	' Token: 0x0400355D RID: 13661
	<SerializeField()>
	Private rend As SpriteRenderer

	' Token: 0x0400355E RID: 13662
	<SerializeField()>
	Private blurAmount As Single
End Class
